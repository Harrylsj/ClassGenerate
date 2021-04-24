using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Entity
{
    public abstract class ATable
    {
        public ATable()
        {
        }
        public virtual T CloneEntity<T>(T oldT)
        {
            return new JHEntity().CloneEntity<T>(oldT);
        }
        protected String ESC(string strSQL)
        {
            if (!String.IsNullOrEmpty(strSQL))
            {
                strSQL = strSQL.Trim();
                return strSQL.Replace("'", "''");
            }
            else
                return "";
        }


        //public abstract String GetInsertSQL();
        //public abstract String GetUpdateSQL();

        public abstract String GetKeyValue();
    }
}
