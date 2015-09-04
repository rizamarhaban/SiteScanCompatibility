using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteScanCompatibility
{
    public class ExportToExcel
    {

        /// <summary>
        /// Exports the scan result.
        /// </summary>
        /// <param name="tempFilename">The temporary filename.</param>
        /// <param name="dt">The data in DataTable.</param>
        public static void ExportScanResult(String tempFilename, DataTable dt)
        {
            MemoryStream ms = new MemoryStream(Properties.Resources.XportScanResults);
            ms.Seek(0, SeekOrigin.Begin);
            ExcelPackage pck = new ExcelPackage(ms);

            GenerateScanReport(pck, dt);

            MemoryStream msOut = new MemoryStream();
            pck.SaveAs(msOut);

            ms.Close();
            ms.Dispose();

            msOut.Seek(0, SeekOrigin.Begin);
            byte[] resutls = msOut.ToArray();

            msOut.Close();
            msOut.Dispose();

            using (BinaryWriter b = new BinaryWriter(File.Open(tempFilename, FileMode.Create)))
            {
                b.Write(resutls);
            }

        }

        /// <summary>
        /// Generates the scan report.
        /// </summary>
        /// <param name="pck">The Excel Package File.</param>
        /// <param name="dt">The data in DataTable.</param>
        private static void GenerateScanReport(ExcelPackage pck, DataTable dt)
        {
            ExcelWorksheet ws = pck.Workbook.Worksheets["SG Scan Sites"];
            CellWriter.StampWhoRequestedIt(ws, 1, 1);
            CellWriter.StampToDocumentProperties(pck.Workbook,
                "SG Scan Sites Report",
                "Microsoft Edge Compatibility in SG Scan Sites (live status).",
                null,
                "report,edge,compatibility,scan site");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rowDisplayValue = dt.Rows[i];

                for (int j = 0; j < dt.Columns.Count - 1; j++)
                {
                    bool checkForError = (bool)dt.Rows[i][8];

                    object data = dt.Rows[i][j];
                    if (data == null) continue;

                    ExcelRange cell = ws.Cells[4 + i, 2 + j];

                    if (dt.Columns[j].ColumnName.Equals("#"))
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    if (dt.Columns[j].ColumnName.Equals("Domain"))
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.General;

                    if (dt.Columns[j].DataType.Equals(typeof(bool)))
                    {
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        cell.Style.Font.SetFromFont(new Font("Segoe MDL2 Assets", 14f));
                        if (data.GetType().Equals(typeof(bool)))
                        {
                            bool b = (bool)data;
                            if (b) data = "";
                            else data = String.Empty;
                        }
                    }

                    if (checkForError)
                    {
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 200, 200));
                    }

                    cell.Value = data;
                }
            }

        }

    }
}
