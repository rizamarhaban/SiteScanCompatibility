using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteScanCompatibility
{
    public static class CellWriter
    {
        /// <summary>
        /// Puts a stamp in the target cell of the requested spreadsheet.
        /// NOTE: Starting cell positions are NOT zero based, start from 1,1
        /// </summary>
        public static void StampWhoRequestedIt(ExcelWorksheet ws, int targetRow, int targetColumn)
        {
            StampWhoRequestedIt(ws, targetRow, targetColumn, String.Empty);
        }

        public static void StampWhoRequestedIt(ExcelWorksheet ws, int targetRow, int targetColumn, string additionalText)
        {
            ws.Cells[targetRow, targetColumn].Value = String.Format("generated {0:f} by {1} {2}", DateTime.Now, Environment.UserName, additionalText);
        }

        /// <summary>
        /// Stamps information to Excel document properties.
        /// </summary>
        /// <param name="wb">Workbook handler</param>
        /// <param name="title">The title of this document.</param>
        /// <param name="subject">The subject for this document.</param>
        /// <param name="comments">Comments.</param>
        /// <param name="tags">Tags or keywords (use comma delimited string).</param>
        public static void StampToDocumentProperties(ExcelWorkbook wb, string title, string subject, string comments, string tags)
        {
            wb.Properties.Author = "Riza Marhaban";

            if (String.IsNullOrEmpty(comments))
                wb.Properties.Comments = String.Format("Automatic generated reporting from Edge Site Scan. ({0})", DateTime.Now);
            else
                wb.Properties.Comments = comments;

            if (String.IsNullOrEmpty(title))
                wb.Properties.Title = String.Format("Edge Site Scan Report. ({0})", title, DateTime.Now);
            else
                wb.Properties.Title = String.Format("{0}. ({1})", title, DateTime.Now);

            wb.Properties.Subject = subject;
            wb.Properties.Keywords = tags;
            wb.Properties.Company = "Microsoft";
            wb.Properties.Category = "Report Document";
        }
    }
}
