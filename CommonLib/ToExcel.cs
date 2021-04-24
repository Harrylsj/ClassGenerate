using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Web;
namespace CommonLib
{
    public class ToExcel
    {
        public void CreateExcel(System.Web.HttpContext context, DataTable dt, string FileName)//HttpResponse Page.Response
        {
            string FileType = "application/ms-excel";
            context.Response.Clear();
            context.Response.Charset = "UTF-8";
            context.Response.Buffer = true;
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            context.Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + FileName + ".xls\"");
            context.Response.ContentType = FileType;
            string colHeaders = string.Empty;
            string ls_item = string.Empty;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i == (dt.Columns.Count - 1))
                {
                    ls_item += dt.Columns[i].ColumnName + "\n";
                }
                else
                {
                    ls_item += dt.Columns[i].ColumnName + "\t";
                }
            }
            context.Response.Output.Write(ls_item);
            ls_item = string.Empty;
            colHeaders = string.Empty;

            DataRow[] myRow = dt.Select();
            int cl = dt.Columns.Count;
            foreach (DataRow row in myRow)
            {
                int count = 0;
                for (int i = 0; i < cl; i++)
                {
                    if (i == (cl - 1))
                    {
                        ls_item += row[i].ToString() + "\n";
                    }
                    else
                    {
                        ls_item += row[i].ToString() + "\t";
                    }
                    count++;
                }
                context.Response.Output.Write(ls_item);
                ls_item = string.Empty;
            }
            context.Response.Output.Flush();
            context.Response.End();
        }
    }
}
