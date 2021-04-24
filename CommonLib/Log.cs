using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace CommonLib
{
    public class Log
    {
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="content">要写的内容</param>
        /// <returns></returns>
        public bool Write(string content)
        {
            //string fullpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "/log.txt";
            string fullpath = System.AppDomain.CurrentDomain.BaseDirectory + "/log.txt";
            using (StreamWriter sw = new StreamWriter(fullpath, true))
            {
                sw.WriteLine(DateTime.Now.ToString() + "===============" + content);
                sw.Close();
            }
            return true;
        }
    }
}
