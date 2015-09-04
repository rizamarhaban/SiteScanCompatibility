using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SiteScanCompatibility.ViewModel
{

    public class ScanResultItemViewModel : ViewModelBase
    {
        private string _siteHost;
        private double _processTime;
        private bool   _browserDetection;
        private bool   _cssPrefix;
        private bool   _edge;
        private bool   _jsLib;
        private bool   _pluginFree;

        private SolidColorBrush _bgColor = Brushes.Transparent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanResultItemViewModel"/> class.
        /// </summary>
        public ScanResultItemViewModel() { }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has error; otherwise, <c>false</c>.
        /// </value>
        public bool HasError { get; set; }

        /// <summary>
        /// Gets or sets the process time.
        /// </summary>
        public double ProcessTime
        {
            get { return _processTime; }
            set { Set<double>(nameof(ProcessTime), ref _processTime, value); }
        }

        /// <summary>
        /// Gets or sets the site Host.
        /// </summary>
        public string SiteHost
        {
            get { return _siteHost; }
            set { Set<string>(nameof(SiteHost), ref _siteHost, value); }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        public SolidColorBrush BgColor
        {
            get { return _bgColor; }
            set { Set<SolidColorBrush>(nameof(BgColor), ref _bgColor, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the site NOT using browser detection.
        /// </summary>
        public bool BrowserDetection
        {
            get { return _browserDetection; }
            set { Set<bool>(nameof(BrowserDetection), ref _browserDetection, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [CSS prefix].
        /// </summary>
        public bool CSSPrefix
        {
            get { return _cssPrefix; }
            set { Set<bool>(nameof(CSSPrefix), ref _cssPrefix, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the site is edge comaptible (using HTML5)
        /// </summary>
        public bool Edge
        {
            get { return _edge; }
            set { Set<bool>(nameof(Edge), ref _edge, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the site js library is uptodate.
        /// </summary>
        public bool JSLib
        {
            get { return _jsLib; }
            set { Set<bool>(nameof(JSLib), ref _jsLib, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the site is plugin free.
        /// </summary>
        public bool PluginFree
        {
            get { return _pluginFree; }
            set { Set<bool>(nameof(PluginFree), ref _pluginFree, value); }
        }
    }
}
