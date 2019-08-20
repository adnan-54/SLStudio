using System.Data;
using System.Text;

namespace SLStudio.Util
{
    public static class Converters
    {
        public static string DataTableToHTML(DataTable dt)
        {
            StringBuilder htmlTable = new StringBuilder();
            htmlTable.AppendLine("<table>");
            htmlTable.AppendLine("<tr>");
            foreach(DataColumn column in dt.Columns)
            {
                htmlTable.AppendLine($"<th>{column.ColumnName}</th>");
            }
            htmlTable.AppendLine("</tr>");
            foreach(DataRow row in dt.Rows)
            {
                htmlTable.AppendLine("<tr>");
                foreach(DataColumn column in dt.Columns)
                {
                    htmlTable.AppendLine($"<td>{row[column.ColumnName].ToString()}</td>");
                }
                htmlTable.AppendLine("</tr>");
            }
            htmlTable.AppendLine("</table>");

            return htmlTable.ToString(); 
        }
    }
}
