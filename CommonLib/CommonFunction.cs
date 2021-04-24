using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace CommonLib
{
    public class CommonFunction
    {

        public String ESC_XML(String strOld)//Escaped Chars转义字符
        {
            return strOld.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("\"", "&quot;");
            //String strReturn = "";
            //if(strOld != null )
            //    strReturn=strOld;
            //strReturn = strReturn.Replace("<", "&lt;");
            //strReturn = strReturn.Replace(">", "&gt;");
            ////strReturn = strReturn.Replace("&", "&amp;");
            //strReturn = strReturn.Replace("'", "&apos;");
            ////strReturn = strReturn.Replace("\"", "&quot;");
            //return strReturn;
            //转义字符 xml特殊字符 符号名 
            //&lt;      < 小于号 
            //&gt;      > 大于号 
            //&amp;     & 和 
            //&apos;    ' 单引号 
            //&quot;    " 双引号 
        }
        public String UnESC_XML(String strOld)//Escaped Chars转义字符
        {
            return strOld.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&apos;", "'").Replace("&quot;", "\"");

            //转义字符 xml特殊字符 符号名 
            //&lt;      < 小于号 
            //&gt;      > 大于号 
            //&amp;     & 和 
            //&apos;    ' 单引号 
            //&quot;    " 双引号 
        }
        public static DateTime StampToDateTime(string time)
        {
            time = time.Substring(0, 10);
            double timestamp = Convert.ToInt64(time);
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }
        //DateTime类型转换为时间戳(毫秒值)
        public static long DateTimeToTicks(DateTime? time)
        {
            return ((time.HasValue ? time.Value.Ticks : DateTime.Parse("1990-01-01").Ticks) - 621355968000000000) / 10000;
        }

        public static byte[] GetImage(string Path)
        {
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] imgBytesIn = br.ReadBytes((int)fs.Length);
            return imgBytesIn;
        }
    }
}
