using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using SiteScanCompatibility.Model;
using GalaSoft.MvvmLight.Threading;
using System.Windows.Media;
using System.Data;
using System.Windows;
using System.Collections;
using System.Collections.Generic;

namespace SiteScanCompatibility.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Fields
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;

        private ObservableCollection<ScanResultItemViewModel> _items = new ObservableCollection<ScanResultItemViewModel>();
        private RelayCommand<string> _clickCommand;
        private RelayCommand<IList> _extendedClickCommand;

        private string _fileName = "[No File]";
        private string _itemBeingScan;
        private int    _percentatge;
        private bool   _notScanning = true;
        private bool   _inProgress = false;
        private bool   _isUsingWWW = true;
        private IList  _selectedItems;

        // Change this default static_code_scan to your host server
        private string _scanSiteServer = "localhost:1337";  

        private Visibility _showExport = Visibility.Collapsed;
        private Visibility _showStop = Visibility.Collapsed;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {

            }
        }

        #region Public Members
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set { Set<string>(nameof(FileName), ref _fileName, value); }
        }

        /// <summary>
        /// Gets or sets the item being scan.
        /// </summary>
        public string ItemBeingScan
        {
            get { return _itemBeingScan; }
            set { Set<string>(nameof(ItemBeingScan), ref _itemBeingScan, value); }
        }

        /// <summary>
        /// Gets or sets the scan site server.
        /// </summary>
        public string ScanSiteServer
        {
            get { return _scanSiteServer; }
            set { Set<string>(nameof(ScanSiteServer), ref _scanSiteServer, value); }
        }

        public bool NotScanning
        {
            get { return _notScanning; }
            set { Set<bool>(nameof(NotScanning), ref _notScanning, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the site list will use www.
        /// </summary>
        public bool IsUsingWWW
        {
            get { return _isUsingWWW; }
            set { Set<bool>(nameof(IsUsingWWW), ref _isUsingWWW, value); }
        }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        public int Percentage
        {
            get { return _percentatge; }
            set { Set<int>(nameof(Percentage), ref _percentatge, value); }
        }

        /// <summary>
        /// Gets or sets the show export button visibility.
        /// </summary>
        public Visibility ShowExport
        {
            get { return _showExport; }
            set { Set<Visibility>(nameof(ShowExport), ref _showExport, value); }
        }

        /// <summary>
        /// Gets or sets the show stop.
        /// </summary>
        public Visibility ShowStop
        {
            get { return _showStop; }
            set { Set<Visibility>(nameof(ShowStop), ref _showStop, value); }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<ScanResultItemViewModel> Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the extended click command.
        /// </summary>
        public RelayCommand<IList> ExtendedClickCommand
        {
            get
            {
                return _extendedClickCommand ??
                    (_extendedClickCommand = new RelayCommand<IList>(item =>
                    {
                        if (item.Count > 0) _selectedItems = item;
                    }
                  ));
            }
        }

        /// <summary>
        /// Gets the click command.
        /// </summary>
        public RelayCommand<string> ClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new RelayCommand<string>((prameter) =>
                {
                    switch (prameter)
                    {
                        case "LoadFile":
                            LoadFile();
                            break;
                        case "ClearList":
                            ClearList();
                            break;
                        case "RunScan":
                            ShowExport = Visibility.Collapsed;
                            NotScanning = false;
                            RunScan();
                            break;
                        case "CancelScan":
                            NotScanning = true;
                            _inProgress = false;
                            ItemBeingScan = "[Process Canceled!]";
                            ShowExport = Visibility.Visible;
                            ShowStop = Visibility.Collapsed;
                            break;
                        case "Export":
                            ShowStop = Visibility.Collapsed;
                            NotScanning = false;
                            Export();
                            break;
                    }

                }));
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Clears the list.
        /// </summary>
        private async void ClearList()
        {
            if (_selectedItems != null)
            {
                _selectedItems.Clear();
                _selectedItems = null;
            }

            await DispatcherHelper.RunAsync(() =>
            {
                Items.Clear();
                FileName = "[No File]";
                ItemBeingScan = "[List Empty]";
                Percentage = 0;
                NotScanning = true;
                ShowExport = Visibility.Collapsed;
                ShowStop = Visibility.Collapsed;
                _inProgress = false;
            });
        }

        /// <summary>
        /// Exports this instance.
        /// </summary>
        private async void Export()
        {
            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.AddExtension = true;
            _saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx";

            bool? dialogResult = _saveFileDialog.ShowDialog();
            if (dialogResult.HasValue && !dialogResult.Value) return;

            string filename = _saveFileDialog.FileName;

            try
            {
                ExportProcess(filename);
            }
            catch (Exception)
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    ItemBeingScan = "[Error exporting file]";
                    NotScanning = true;
                    ShowStop = Visibility.Collapsed;
                    _inProgress = false;
                });

                return;
            }


            await DispatcherHelper.RunAsync(() =>
            {
                ItemBeingScan = "[Export Success]";
                Percentage = 0;
                NotScanning = true;
                ShowExport = Visibility.Collapsed;
                ShowStop = Visibility.Collapsed;
                _inProgress = false;
            });
        }

        /// <summary>
        /// Exports the process.
        /// </summary>
        /// <param name="filename">The filename.</param>
        private void ExportProcess(string filename)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("#", typeof(int));
            dt.Columns.Add("Domain", typeof(string));
            dt.Columns.Add("Browser Detection", typeof(bool));
            dt.Columns.Add("CSS Prefix", typeof(bool));
            dt.Columns.Add("Edge", typeof(bool));
            dt.Columns.Add("JS Library", typeof(bool));
            dt.Columns.Add("Plugin Free", typeof(bool));
            dt.Columns.Add("Process Time", typeof(double));
            dt.Columns.Add("Has Error", typeof(bool));

            foreach (var item in _items)
            {
                DataRow dr = dt.NewRow();

                dr[0] = item.Id;
                dr[1] = item.SiteHost;
                dr[2] = item.BrowserDetection;
                dr[3] = item.CSSPrefix;
                dr[4] = item.Edge;
                dr[5] = item.JSLib;
                dr[6] = item.PluginFree;
                dr[7] = item.ProcessTime;
                dr[8] = item.HasError;

                dt.Rows.Add(dr);
            }

            dt.AcceptChanges();

            ExportToExcel.ExportScanResult(filename, dt);

        }

        /// <summary>
        /// Runs the scan.
        /// </summary>
        private void RunScan()
        {
            if (_items.Count == 0) return;

            if (_inProgress) return;
            _inProgress = true;

            float totalItems = _items.Count;

            Percentage = 0;
            ShowStop = Visibility.Visible;

            if (_selectedItems != null)
            {
                if (_selectedItems.Count > 0)
                {
                    // scan selected sites
                    totalItems = _selectedItems.Count;
                    Scan(_selectedItems, totalItems);
                    return;
                }
            }

            // scan all sites
            Scan(_items, totalItems);

        }

        /// <summary>
        /// Scans the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="totalItems">The total items.</param>
        private async void Scan(IList list, float totalItems)
        {
            float count = 0;
            string www = "www.";

            if (!IsUsingWWW) www = "";

            foreach (ScanResultItemViewModel item in list)
            {
                if (_notScanning) return;
                string siteURL = $"http://{www}{item.SiteHost}";

                await DispatcherHelper.RunAsync(() =>
                {
                    ItemBeingScan = $"{siteURL} ({count + 1} from {list.Count})";
                    Percentage = Convert.ToInt32(count / totalItems * 100);
                });

                string url = $"http://{ScanSiteServer}/?url={HttpUtility.UrlEncode(siteURL)}";

                string result = String.Empty;

                try
                {
                    HttpClient client = new HttpClient();
                    result = await client.GetStringAsync(url);
                    client.Dispose();
                }
                catch
                {
                    await DispatcherHelper.RunAsync(() =>
                    {
                        item.BgColor = Brushes.Red;
                        item.HasError = true;
                    });
                    count++;
                    continue;
                }

                if (String.IsNullOrEmpty(result))
                {
                    count++;
                    continue;
                }

                RootObject testResult = null;

                try
                {
                    testResult = JsonConvert.DeserializeObject<RootObject>(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    await DispatcherHelper.RunAsync(() =>
                    {
                        item.BgColor = Brushes.Red;
                        item.HasError = true;
                    });

                    count++;
                    continue;
                }

                if (testResult != null)
                {
                    await DispatcherHelper.RunAsync(() =>
                    {
                        item.ProcessTime = Convert.ToDouble(testResult.processTime);
                        item.CSSPrefix = testResult.results.cssprefixes.passed;
                        item.Edge = testResult.results.rendermode.passed;
                        item.JSLib = testResult.results.jslibs.passed;
                        item.BrowserDetection = testResult.results.browserDetection.passed;
                        item.PluginFree = testResult.results.pluginfree.passed;
                    });
                }

                count++;
            }

            await DispatcherHelper.RunAsync(() =>
            {
                ItemBeingScan = "[Scanning Done!]";
                Percentage = 100;
                NotScanning = true;
                ShowExport = Visibility.Visible;
                ShowStop = Visibility.Collapsed;
                _inProgress = false;
            });
        }

        private void LoadFile()
        {
            ShowExport = Visibility.Collapsed;

            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Multiselect = false;
            _openFileDialog.Filter = "Sites List (*.txt)|*.txt";

            bool? dialogResult = _openFileDialog.ShowDialog();
            if (dialogResult.HasValue && !dialogResult.Value) return;

            FileName = _openFileDialog.FileName;

            string data = File.ReadAllText(_fileName);
            string[] splitter = { "\r\n" };

            string[] lists = data.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            // Do some site URL cleanup a bit, we need the host to be added only
            List<string> sites = new List<string>();
            foreach (var item in lists)
            {
                string host = GetHostFromURL(item);
                if (String.IsNullOrEmpty(host)) continue;
                sites.Add(host);
            }

            // Add the sorted sites to the data grid
            int i = 0;
            foreach (var site in sites.OrderBy(p => p))
            {
                _items.Add(new ScanResultItemViewModel { Id = (i + 1), SiteHost = site });
                i++;
            }

        }

        /// <summary>
        /// Check if there is valid Host.
        /// </summary>
        /// <param name="url">The URL.</param>
        private string GetHostFromURL(string url)
        {
            Uri uriResult;

            if (!url.StartsWith("http://")) url = "http://" + url;
            bool isGoodDomain = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!isGoodDomain) return String.Empty;

            return uriResult.Host;
        } 
        #endregion
    }
}