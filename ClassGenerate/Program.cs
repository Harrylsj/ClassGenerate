﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JHEMR.JHCommonLib.Entity.TableObject;

namespace ClassGenerate
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Generator());
            //
            Application.Run(new LoadServer());
        }
    }
}