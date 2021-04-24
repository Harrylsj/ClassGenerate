using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ClassGenerate.Proc;
using System.Linq;
namespace ClassGenerate.Proc
{
    /// <summary>
    /// 列集合
    /// </summary>
    public enum ColumnsType { AllColumns, NonIdentityColumns, KeyColumns };
    public class TableInfo
    {
        List<ColumnInfo> columnList=new List<ColumnInfo>();
        String keyType;

        public String KeyType
        {
            get { return keyType; }
            set { keyType = value; }
        }
        public List<ColumnInfo> ColumnsList
        {
            get { return columnList; }
            set { columnList = value; }
        }
        public TableInfo()
        {
        }
        public TableInfo(string strPrefix, string strTableName)
        {
            this.dsTableInfo = pt.GetTableInfo(strTableName);
            this.strPrefix = strPrefix;
            className = ps.GetClassNameByTableName(strTableName, strPrefix);
            this.strTableName = strTableName;// dsTableInfo.Tables[0].Rows[0][0].ToString();
            dsTableInfo.Tables[1].TableName = strTableName;
            tableDescription = pt.GetTableDesciption(strTableName);
            //20101112--lsj--
            if (dsTableInfo.Tables[1].Rows.Count > 0)
            {
                for (int i = 0; i < dsTableInfo.Tables[1].Rows.Count; i++)
                {
                    ColumnInfo col = new ColumnInfo();
                    col.ColumnName = dsTableInfo.Tables[1].Rows[i]["Column_name"].ToString();
                    col.TypeName = dsTableInfo.Tables[1].Rows[i]["Type"].ToString();
                    col.Length = dsTableInfo.Tables[1].Rows[i]["Length"].ToString();
                    columnList.Add(col);
                }
                keyType = ColumnsList[0].TypeName;//20110920
            }
            int rows;
            #region 标识列 Field1,Field2..
            //if (dsTableInfo.Tables[2].Rows[0]["Type"].ToString() == "NUMBER")
            //lsj2021-2-2-sql与oracle的区别
            if (dsTableInfo.Tables[1].Rows[0]["Type"].ToString() == "int")
            {
                rows = dsTableInfo.Tables[1].Rows.Count;
                if (dsTableInfo.Tables[1].Rows.Count > 0)
                {
                    indentityColumns = new string[rows];
                    for (int i = 0; i < rows; i++)
                    {
                        string strColumn = dsTableInfo.Tables[1].Rows[i][0].ToString();
                        //string t=dsTableInfo.Tables[2].Rows[i][3].ToString();
                        indentityColumns[i] = strColumn.Trim();
                    }
                }
            }
            #endregion

            #region 主键列 Field1,Field2..
            
            if (dsTableInfo.Tables.Count > 5 && dsTableInfo.Tables[5].Rows.Count > 0)
            {
                //strPKName = dsTableInfo.Tables[5].Rows[0]["constraint_name"].ToString().Trim();
                //lsj2021-2-2-sql与oracle的区别
                strPKName = dsTableInfo.Tables[6].Rows[0]["constraint_name"].ToString().Trim();
                
                strKeyColumns = "";
                for (int i = 0; i < dsTableInfo.Tables[6].Rows.Count; i++)
                {
                    //strKeyColumns += dsTableInfo.Tables[5].Rows[i][0].ToString().Trim() + ",";
                    //lsj2021-2-2-sql与oracle的区别
                    strKeyColumns += dsTableInfo.Tables[6].Rows[i]["constraint_keys"].ToString().Trim() + ",";
                }
                strKeyColumns = strKeyColumns.Substring(0, strKeyColumns.Length - 1);
            }
            else
            {
                strPKName = "";
                //throw new Exception(dsTableInfo.Tables[1].TableName+":表没缺少主键");
            }
            #endregion

            #region 列名数组
            DataTable dtColumnsType = dsTableInfo.Tables[1];
            rows = dtColumnsType.Rows.Count;

            if (rows <= 0)
            {
                throw new Exception("没有表列类型相关信息");
            }
            columns = new string[rows];
            for (int i = 0; i < rows; i++)
            {
                columns[i] = dtColumnsType.Rows[i][0].ToString().Trim();
            }
            #endregion
        }
        public TableInfo(string strNameSpace, string strPrefix,string strTableName)
            : this(strPrefix, strTableName)
        {
            this.strNamespace = strNameSpace;
            ps.SetTabs(strNameSpace);
        }
        public bool CheckColumnName(string strColumnName)
        {
            foreach (string field in columns)
            {
                if (strColumnName == field.ToUpper())
                    return true;
            }
            return false;
        }
        ProcTable pt = new ProcTable();
        ProcString ps = new ProcString();
        /// <summary>
        /// 空间空间
        /// </summary>
        string strNamespace = "";
        /// <summary>
        /// 表的前缀
        /// </summary>
        string strPrefix;
        public DataTable ColumnType { get { return dsTableInfo.Tables[1]; } }
        /// <summary>
        /// 表的相关信息 Tables[0]表名, Tables[1]列类型信息, Tables[2] 标识列,Tables[5]主键列信息信息
        /// </summary>
        public DataSet dsTableInfo;
        /// <summary>
        /// 表描述
        /// </summary>
        public string tableDescription;
        /// <summary>
        /// 表名
        /// </summary>
        string strTableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return strTableName; }
            set { strTableName = value; }
        }
        /// <summary>
        /// 表对应的类名
        /// </summary>
        string className;
        /// <summary>
        /// 获取表对应的类名
        /// </summary>
        public string ClassName
        {
            get
            {
                return className;
            }
            set { className = value; }
        }
        /// <summary>
        /// 获取表对应的类的对象
        /// </summary>
        public string ClassObject
        {
            get
            {
                return ps.GetObjectByTableName(strTableName, strPrefix);
            }
        }
        /// <summary>
        /// 主键列  field1,field2
        /// </summary>
        public string strKeyColumns = "";
        public string strPKName = "";
        /// <summary>
        /// 获取主键列
        /// </summary>
        public string[] KeyColumns
        {
            get
            {
                if (string.IsNullOrEmpty(strKeyColumns))
                {
                    //throw new Exception("缺少主键");
                    return null;
                }

                return strKeyColumns.Split(',');
            }
        }

        /// <summary>
        /// 主键列  field1,field2
        /// </summary>
        public string strForeignKeyColumns = "";
        /// <summary>
        /// 获取主键列
        /// </summary>
        public string[] ForeignKeyColumns
        {
            get
            {
                return strForeignKeyColumns.Split(',');
            }
        }
        /// <summary>
        /// 是否为外键
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool IsForeignKey( String columnName)
        {
            bool bFkey = false;
            foreach (string FKey in ForeignKeyColumns)
            {
                if (FKey == columnName)
                {
                    bFkey = true;
                    break;
                }
            }
            return bFkey;
        }
        /// <summary>
        /// 是否为主键
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool IsKey(String columnName)
        {
            bool bFkey = false;
            foreach (string FKey in KeyColumns)
            {
                if (FKey == columnName)
                {
                    bFkey = true;
                    break;
                }
            }
            return bFkey;
        }
        /// <summary>
        /// 表所有列
        /// </summary>
        string[] columns;
        /// <summary>
        /// 表所有列
        /// </summary>
        public string[] AllColumns
        {
            get { return columns; }
        }
        /// <summary>
        /// 获取所有字段名  field1,field2
        /// </summary>
        public string AllColumnsString
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                foreach (string field in columns)
                {
                    sb.Append(field + ",");
                }

                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 标识列 Field1,Field2..
        /// </summary>
        string[] indentityColumns;
        /// <summary>
        /// 获取非标识列
        /// </summary>
        public string[] NonIdentityColumns
        {
            get
            {
                List<string> t = new List<string>();
                foreach (string column in columns)
                {
                    //if (!ps.IsContains(column, indentityColumns))
                    if (!ps.IsContains(column, KeyColumns))
                    {
                        t.Add(column);
                    }
                }
                string[] list = new string[t.Count];
                t.CopyTo(list);
                return list;
            }
             
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="isContainIndentity">true包含标识列,false不包含标识列</param>
        /// <returns></returns>
        public List<NameValue> GetProperties(ColumnsType type)
        {
            return GetProperties(type,"");
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="isContainIndentity">true包含标识列,false不包含标识列</param>
        /// <returns></returns>
        public List<NameValue> GetProperties(ColumnsType type, string classObject)
        {
            List<NameValue> list = new List<NameValue>();
            if (columns == null)
            {
                throw new Exception("Param.columns 为空");
            }
            else
            {
                string[] cols = null;
                switch (type)
                {
                    case ColumnsType.AllColumns:
                        cols = columns;
                        break;
                    case ColumnsType.NonIdentityColumns:
                        cols = NonIdentityColumns;
                        break;
                    case ColumnsType.KeyColumns:
                        cols = KeyColumns;
                        break;
                }
                ProcString ps = new ProcString();
                //对象首字符小写
                string strObject = "";
                if (!string.IsNullOrEmpty(classObject))
                {
                    strObject = ps.GetObjectByTableName(strTableName.Trim(), strPrefix) + ".";

                    foreach (string field in cols)
                    {
                        string columnName = field.Trim();//字段
                        string propertyName = ps.ConvertStringToUpperOrLower(columnName, false);//属性
                        NameValue nv = new NameValue();
                        nv.Name = "@" + columnName;
                        nv.FieldName = ps.ConvertToSpecial(columnName,strTableName) ;
                        nv.FieldName = ps.ConvertStringToUpperOrLower(nv.FieldName, true);//2021-2-2-lsj-第一个字母大写
                        nv.MemberName = propertyName;
                        nv.Value = strObject + propertyName;
                        nv.FieldType = this.columnList.Where(c => c.ColumnName == columnName).Single().TypeName;

                        nv.ValueIsNotEmpty = ValueIsNotEmpty(nv);

                        list.Add(nv);
                    }
                }
                else
                {
                    strObject = classObject.Trim();

                    foreach (string field in cols)
                    {
                        string columnName = field.Trim();//字段
                        string propertyName = ps.ConvertStringToUpperOrLower(columnName, false);//属性
                        NameValue nv = new NameValue();
                        nv.Name = "@" + columnName;
                        nv.FieldName = ps.ConvertToSpecial(columnName, strTableName);
                        nv.MemberName = propertyName;
                        nv.Value = strObject + propertyName;
                        nv.FieldType = this.columnList.Where(c => c.ColumnName == columnName).Single().TypeName;
                        nv.ValueIsNotEmpty = ValueIsNotEmpty(nv);
                        list.Add(nv);
                    }

                }
            }
            return list;
        }
        string ValueIsNotEmpty(NameValue nv)
        {
            //if (nv.FieldName.ToLower() == "sex")
            //{
            //    return "this." + nv.FieldName + "!=Sex.UnKnown";
            //}
            //switch (nv.FieldType)
            //{

            //    case "DATE":
            //        return "this." + nv.FieldName + ".ToShortDateString()!=\"1900-1-1\"";
            //    case "NUMBER":
            //        return "this." + nv.FieldName + "!=0";
            //    case "VARCHAR2":

                //default:
            return "this." + nv.MemberName + "!=\"\"";
            //}
        }
        /// <summary>
        /// 获取INSERT字段  Field1,Field2... or  @Field1,@Field2... 
        /// </summary>
        /// <param name="strChar">字段前要加的符号</param>
        /// <returns></returns>
        public string GetInsertFields(string strChar)
        {
            StringBuilder sbFields = new StringBuilder();
            if (columns == null)
            {
                throw new Exception("Param.columns 为空");
            }
            else
            {
                string[] list = AllColumns;
                foreach (string field in list)
                {
                    string columnName = field.Trim();//列名
                    sbFields.Append(strChar + columnName);
                    sbFields.Append(",");
                }
                if (sbFields.Length > 0)
                {//去掉最后的逗号
                    sbFields.Remove(sbFields.Length - 1, 1);
                }
            }

            return sbFields.ToString();
        }
        public string GetInsertValues_sp()
        {
            StringBuilder sbFields = new StringBuilder();
            if (columns == null)
            {
                throw new Exception("Param.columns 为空");
            }
            else
            {
                string[] list = AllColumns;
                foreach (string field in list)
                {
                    string columnName = field.Trim();//列名
                    if (columnName == "ChangeTime")
                        sbFields.Append("getdate()");
                    else
                        sbFields.Append("@" + columnName);
                    sbFields.Append(",");
                }
                if (sbFields.Length > 0)
                {//去掉最后的逗号
                    sbFields.Remove(sbFields.Length - 1, 1);
                }
            }

            return sbFields.ToString();
        }
        public string GetInsertValues(string classObject)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.AllColumns, classObject);
            sbAddParams.AppendLine("\" +");
            foreach (NameValue nv in list)
            {
                //if (nv.FieldName.ToLower() == "sex")//oracle
                //    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"'\"+(int)" + nv.Value.ToString() + "+\"',\"+");
                //else
                //{
                    switch (nv.FieldType)
                    {
                        case "DATE":
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + ",false,false))+\",\"+");
                            break;
                        case "BLOB":
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\":fs,\"+");
                            break;
                        default:
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":" + "\"'\"+ESC(" + nv.Value.ToString() + ")+\"'\")+\",\"+");
                            //(data_source_ == "" ? "NULL" : "'"+ESC(data_source_)+"'") + "," +
                            break;
                    }
                    
                //}
            }
            if (sbAddParams.Length > 0)
            {//去掉最后的逗号 ,\+
                sbAddParams.Remove(sbAddParams.Length - ",\"+\r\n".Length, ",\"+\r\n".Length);
            }
            
            return sbAddParams.ToString();
        }
        /// <summary>
        /// 获取UPDATE字段  Field1=@Field1,Field1=@Field1...
        /// </summary>
        /// <returns></returns>
        public string GetUpdateFields()
        {
            StringBuilder sbFields = new StringBuilder();
            if (columns == null)
            {
                throw new Exception("Param.columns 为空");
            }
            else
            {
                string[] cols = NonIdentityColumns;
                foreach (string columnName in cols)
                {
                    string c = columnName.Trim();
                    sbFields.Append(c);
                    sbFields.Append("=@");
                    sbFields.Append(c);
                    sbFields.Append(",");
                }
                if (sbFields.Length > 0)
                {//去掉最后的逗号
                    sbFields.Remove(sbFields.Length - 1, 1);
                }
            }

            return sbFields.ToString();
        }
        /// <summary>
        /// 20210317--lsj--作废，使用Parameter
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns></returns>
        public string GetUpdateValues(string classObject)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, classObject);
            sbAddParams.AppendLine("\"+");
            foreach (NameValue nv in list)
            {
                //if (nv.FieldName.ToLower() == "sex")//oracle
                //    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "='\"+" + "(int)" + nv.Value.ToString() + "+\"',\"+");
                //else
                //{
                    switch (nv.FieldType)
                    {
                        case "DATE":
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+" + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + " ,false,false))+\",\"+");
                            break;
                        case "BLOB":
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=:fs\"+\",\"+");
                            break;
                        default:
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":" + "\"'\"+ESC(" + nv.Value.ToString() + ")+\"'\")+\",\"+");
                            //(syn_data_count_.ToString()==""?"NULL": "'"+ESC(syn_data_count_)+"'")
                            break;
                    }
                //}
            }
            if (sbAddParams.Length > 4)
            {//去掉最后的逗号 ,\+
                sbAddParams.Remove(sbAddParams.Length - "+\",\"+\r\n".Length, "+\",\"+\r\n".Length);
            }
            list = GetProperties(ColumnsType.KeyColumns, classObject);
            sbAddParams.AppendLine("+\" WHERE \"+");
            foreach (NameValue nv in list)
            {
                switch (nv.FieldType)
                {
                    case "DATE":
                        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+"
                            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + ",false,false)+\" and \"+");
                        break;
                    default:
                        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "='\"+ESC(" + nv.Value.ToString() + ")+\"'and \"+");
                        break;
                }

            }
            sbAddParams.Remove(sbAddParams.Length - "+\"and \"+".Length, "+\"and \"+".Length);
            if (list[list.Count - 1].FieldType == "DATE")
            {
                sbAddParams.Remove(sbAddParams.Length - "+".Length, "+".Length);
            }
            else
            {
                sbAddParams.Append(" \"'\"");
            }
            sbAddParams.Append(" ;");
            return sbAddParams.ToString();
        }
        public string GetUpdateValues_Parameter()
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, "");
            sbAddParams.AppendLine("\"+");
            foreach (NameValue nv in list)
            {
                switch (nv.FieldType)
                {
                    case "DATE":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+" + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                        + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + " ,false,false))+\",\"+");
                        break;
                    case "BLOB":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=:fs\"+\",\"+");
                        break;
                    default:
                        if (nv.FieldName == "ChangeTime")
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "= getdate(),\"+");
                        else
                            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "= @" + nv.FieldName + ",\"+");
                        break;
                }
                //}
            }
            if (sbAddParams.Length > 4)
            {//去掉最后的逗号 ,\+
                sbAddParams.Remove(sbAddParams.Length - ",\"+\r\n".Length, ",\"+\r\n".Length);
            }
            list = GetProperties(ColumnsType.KeyColumns, "");
            sbAddParams.AppendLine("  WHERE \"+");
            foreach (NameValue nv in list)
            {
                switch (nv.FieldType)
                {
                    case "DATE":
                        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+"
                            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + ",false,false)+\" and \"+");
                        break;
                    default:
                        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=@" + nv.FieldName + " \"; ");
                        break;
                }

            }
            return sbAddParams.ToString();
        }
        public string GetUpdateValues_ParameterForPartial()
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, "");
            for (int i = 0; i < list.Count;i++ )
            {
                if (list[i].FieldName == "ChangeTime")
                {
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "ChangeTime = DateTime.Now.ToString();");
                }
                switch (list[i].FieldType)
                {
                    case "image":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "if (" + list[i].FieldName + "!=null)");
                        break;
                    default:
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "if (!String.IsNullOrEmpty(" + list[i].FieldName + "))");
                        break;
                } 
                if (i < list.Count - 1)
                {              
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "{");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNext + "strSQL +=\"" + list[i].FieldName + "= @" + list[i].FieldName + ",\";");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNext + "boolPartial = true;");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "}");
                }
                else if (i == list.Count - 1)
                {
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "{");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNext + " strSQL +=\"" + list[i].FieldName + "= @" + list[i].FieldName + "\";");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "}");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "else");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "{");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNext + "if(boolPartial)");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNext + "{");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNextMore + "strSQL = strSQL.Remove(strSQL.Length - 1, 1);//去掉后面的逗号。");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTopNext + "}");
                    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "}");
                }
            }
            list = GetProperties(ColumnsType.KeyColumns, "");
            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + " strSQL +=\" WHERE \"; ");
            foreach (NameValue nv in list)
            {
                sbAddParams.Append(ps.tabIfLocalVarTop1 + "strSQL +=\"" + nv.FieldName + "=@" + nv.FieldName + " \"; ");
            }
            return sbAddParams.ToString();
        }
       
        public string GetSqlParameter(ColumnsType type)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(type, "");
            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "SqlParameter[] sp = new SqlParameter[" + list.Count + "];");
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].FieldType)
                {
                    //case "DATE":
                    //    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+" + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                    //    + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + " ,false,false))+\",\"+");
                    //    break;
                    //case "BLOB":
                    //    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=:fs\"+\",\"+");
                    //    break;
                    case "image":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "sp[" + i + "] = new SqlParameter(\"@" + list[i].FieldName + "\", SqlDbType.Image);");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "if(" + list[i].FieldName + "==null)");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop2 + "sp[" + i + "].Value = DBNull.Value;");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "else");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop2 + "sp[" + i + "].Value = " + list[i].FieldName + ";");
                        break;
                    default:
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "sp[" + i + "] = new SqlParameter(\"@" + list[i].FieldName + "\", " + list[i].FieldName + ");");
                        break;
                }
            }
            return sbAddParams.ToString();
        }
        public string GetSqlParameterForPartial(ColumnsType type)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(type, "");
            sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "SqlParameter[] sp = new SqlParameter[" + list.Count + "];");
            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].FieldType)
                {
                    //case "DATE":
                    //    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+" + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                    //    + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + " ,false,false))+\",\"+");
                    //    break;
                    //case "BLOB":
                    //    sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=:fs\"+\",\"+");
                    //    break;
                    case "image":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "sp[" + i + "] = new SqlParameter(\"@" + list[i].FieldName + "\", SqlDbType.Image);");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "if( " + list[i].FieldName + "==null)");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop2 + "sp[" + i + "].Value = DBNull.Value;");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "else");
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop2 + "sp[" + i + "].Value = " + list[i].FieldName + ";");
                        break;
                    default:
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "sp[" + i + "] = new SqlParameter(\"@" + list[i].FieldName + "\", " + list[i].FieldName + ");");
                        break;
                }
                
            }
            return sbAddParams.ToString();
        }
        public string GetUpdateEntity(string classObject)
        {
            string strClassName = ps.GetClassName(strTableName);//2021-2-2-lsj-类名首字母大写
            string strClassObject = strClassName.ToLower();//2021-2-2-lsj-类对象全部字母小写
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, classObject);
            foreach (NameValue nv in list)
            {
                switch (nv.FieldType)
                {
                    case "DATE":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+" + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                        + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + " ,false,false))+\",\"+");
                        break;
                    case "BLOB":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=:fs\"+\",\"+");
                        break;
                    default:
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + " " + strClassObject + "." + nv.FieldName + " = dr[\"" + nv.FieldName + "\"] == DBNull.Value ? \"\" : dr[\"" + nv.FieldName + "\"].ToString();");
                        
                        break;
                }
                //}
            }
            
            return sbAddParams.ToString();
        }
        public string GetEntity(string classObject)
        {
            string strClassName = ps.GetClassName(strTableName);//2021-2-2-lsj-类名首字母大写
            string strClassObject = strClassName.ToLower();//2021-2-2-lsj-类对象全部字母小写
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.AllColumns, classObject);
            foreach (NameValue nv in list)
            {
                switch (nv.FieldType)
                {
                    case "DATE":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+" + "(String.IsNullOrEmpty(" + nv.Value.ToString() + ")?\"NULL\":"
                        + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + " ,false,false))+\",\"+");
                        break;
                    case "BLOB":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=:fs\"+\",\"+");
                        break;
                    case "image":
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + " " + strClassObject + "." + nv.FieldName + " = dr[\"" + nv.FieldName + "\"] == DBNull.Value ? null :(byte [])dr[\"" + nv.FieldName + "\"];");
                        break;
                    default:
                        sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + " " + strClassObject + "." + nv.FieldName + " = dr[\"" + nv.FieldName + "\"] == DBNull.Value ? \"\" : dr[\"" + nv.FieldName + "\"].ToString();");

                        break;
                }
                //}
            }
            return sbAddParams.ToString();
        }
        public string GetEntity_ashx(string classObject)
        {
            string strClassName = ps.GetClassName(strTableName);//2021-2-2-lsj-类名首字母大写
            string strClassObject = strClassName.ToLower();//2021-2-2-lsj-类对象全部字母小写
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.AllColumns, classObject);
            foreach (NameValue nv in list)
            {
                sbAddParams.AppendLine(ps.tabLocalVar + strClassObject + "." + nv.FieldName + " = context.Request.Form[\"" + nv.FieldName + "\"];");
            }
            return sbAddParams.ToString();
        }
        public string GetKeyValue(string classObject)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, classObject);
            //sbAddParams.AppendLine("\"+");
            
            list = GetProperties(ColumnsType.KeyColumns, classObject);
            
            foreach (NameValue nv in list)
            {
                sbAddParams.Append("ESC(" + nv.Value.ToString() + ") +");
                //switch (nv.FieldType)
                //{
                //    case "DATE":
                //        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + ""
                //            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + ",false,false)+\" and \"+");
                //        break;
                //    default:
                //        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "='\"+ESC(" + nv.Value.ToString() + ")+\"'and \"+");
                //        break;
                //}

            }
            sbAddParams.Remove(sbAddParams.Length - "+".Length, "+".Length);
            return sbAddParams.ToString();
        }
        public string GetKeyValue()
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.KeyColumns, "");
            foreach (NameValue nv in list)
            {
                sbAddParams.Append( nv.FieldName.ToString() );
                //sbAddParams.Append("ESC(" + nv.Value.ToString() + ") +");
                //switch (nv.FieldType)
                //{
                //    case "DATE":
                //        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + ""
                //            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + ",false,false)+\" and \"+");
                //        break;
                //    default:
                //        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "='\"+ESC(" + nv.Value.ToString() + ")+\"'and \"+");
                //        break;
                //}

            }
           // sbAddParams.Remove(sbAddParams.Length - "+".Length, "+".Length);
            return sbAddParams.ToString();
        }
        public string GetUpdateValuesForPID()
        {
            string classObject = "";
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, classObject);
            sbAddParams.AppendLine("\"+");
            List<NameValue> listPID=  list.Where(n => n.FieldName == "PIX_PATIENT_ID").ToList();
            if (listPID.Count == 1)
            {
                sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + "\"PIX_PATIENT_ID='\"+" + listPID[0].Value.ToString() + "+\"'\"+");
            }
            
            list = GetProperties(ColumnsType.KeyColumns, classObject);
            sbAddParams.AppendLine(" \" WHERE \"+");
            foreach (NameValue nv in list)
            {
                switch (nv.FieldType)
                {
                    case "DATE":
                        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "=\"+"
                            + "JHDBCustomFunctionUse.ToDate(" + nv.Value.ToString() + ",false,false)+\" and \"+");
                        break;
                    default:
                        sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "='\"+ESC(" + nv.Value.ToString() + ")+\"'and \"+");
                        break;
                }

            }
            //sbAddParams.Remove(sbAddParams.Length - "+\"and \"+".Length, "+\"and \"+".Length);
            //sbAddParams.Append("  \"'\";");
            sbAddParams.Remove(sbAddParams.Length - "+\"and \"+".Length, "+\"and \"+".Length);
            if (list[list.Count - 1].FieldType == "DATE")
            {
                sbAddParams.Remove(sbAddParams.Length - "+".Length, "+".Length);
            }
            else
            {
                sbAddParams.Append(" \"'\"");
            }
            sbAddParams.Append(" ;");
            return sbAddParams.ToString();
        }

        /// <summary>
        /// 获取DELETE字段  AND Field1=@Field1 AND Field1=@Field1...
        /// </summary>
        /// <returns></returns>
        public string GetDeleteFields()
        {
            StringBuilder sbFields = new StringBuilder();
            if (columns == null)
            {
                throw new Exception("Param.columns 为空");
            }
            else
            {
                string[] cols = KeyColumns;
                foreach (string columnName in cols)
                {
                    string c = columnName.Trim();
                    sbFields.Append(" AND ");
                    sbFields.Append(c);
                    sbFields.Append("=@");
                    sbFields.Append(c);
                }
            }

            return sbFields.ToString();
        }
        public string GetDeleteValues(string classObject)
        {
            StringBuilder sb = new StringBuilder();
            ProcString ps = new ProcString();
            DataTable dtType = dsTableInfo.Tables[1].Copy();
            List<NameValue> list = GetProperties(ColumnsType.KeyColumns, classObject);
            string[] keys = KeyColumns;
            sb.Append("\"+");
            foreach (string field in keys)
            {
                string f = field.Trim();
                DataRow[] drs = dtType.Select("Column_name='" + f + "'");
                if (drs.Length > 0)
                {
                    string type = ps.ConvertType(drs[0]["Type"].ToString(), f) + " ";
                    string var = ps.ConvertStringToUpperOrLower(f, false);
                    //sb.Append(" \"" + f + "='\"+" + var + "+\"'and \"+");
                    //----
                    //switch (type.Trim())
                    //{
                    //    case "DateTime":
                    //        sb.Append(" \"" + f + "=\"+"
                    //            + "JHDBCustomFunctionUse.ToDate(" + var + ".ToString(),false,false)+\" and \"+");
                    //        break;
                    //    case "String":
                    //    default:
                    //        sb.Append(" \"" + f + "='\"+" + var + "+\"'and \"+");
                    //        break;
                    //}
                    foreach (NameValue nv in list)
                    {
                        f = f.ToUpper();
                        if (f == nv.FieldName.ToUpper())
                        {
                            //sbAddParams.Append(ps.tabIfLocalVarTop1 + "\"" + nv.FieldName + "='\"+" + nv.Value.ToString() + "+\"'and \"+");
                            switch (nv.FieldType)
                            {
                                case "DATE":
                                    sb.Append(" \"" + f + "=\"+"
                                + "JHDBCustomFunctionUse.ToDate(" + var + ".ToString(),false,false)+\" and \"+");
                                    break;
                                default:
                                    sb.Append(" \"" + f + "='\"+ESC(" + var + ")+\"'and \"+");
                                    //"\"+(" + nv.Value.ToString() + "==\"\"?\" IS NULL\":" + "\"='\"+ESC(" + nv.Value.ToString() + ")+\"'and \"+");
                                    //"=\"+(" + nv.Value.ToString() + "==\"\"?\"NULL\":" + "\"'\"+ESC(" + nv.Value.ToString() + ")+\"'\"),\"+");
                                    break;
                            }
                        }
                    }

                }
            }
            //if (sb.Length > 0)
            //{
            //    sb.Remove(sb.Length - "+\"and \"+".Length, "+\"and \"+".Length);
            //}
            //sb.Append(" \"'\";");
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - "+\"and \"+".Length, "+\"and \"+".Length);
                string Type= this.columnList.Where(c => c.ColumnName == keys[keys.Length - 1]).Single().TypeName;
                if (Type == "DATE")
                {
                    sb.Remove(sb.Length - "+".Length, "+".Length);
                }
                else
                {
                    sb.Append(" \"'\"");
                }
            }
            sb.Append(" ;");
            return sb.ToString();
        
        }
        public string GetDeleteValues_sp()
        {
            StringBuilder sb = new StringBuilder();
            ProcString ps = new ProcString();
            DataTable dtType = dsTableInfo.Tables[1].Copy();
            List<NameValue> list = GetProperties(ColumnsType.KeyColumns, "");
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append( list[i].FieldName + "=@" + list[i].FieldName + "  \";");
            }
            return sb.ToString();
        }
        public bool ExistIsDeleted()
        {
            bool boolIsExist = false;
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, "");
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].FieldName == "IsDeleted")
                {
                    boolIsExist = true;
                    break;
                }
            }
            return boolIsExist;
        }
        public bool ExistChangeTime()
        {
            bool boolIsExist = false;
            List<NameValue> list = GetProperties(ColumnsType.NonIdentityColumns, "");
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].FieldName == "ChangeTime")
                {
                    boolIsExist = true;
                    break;
                }
            }
            return boolIsExist;
        }
        public string GetIndexSQL(string classObject)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                ProcString ps = new ProcString();
                DataTable dtType = dsTableInfo.Tables[1].Copy();
                string[] keys = KeyColumns;
                if (keys != null)
                {
                    foreach (string field in keys)
                    {
                        string f = field.Trim();
                        sb.Append(f + ",");
                        //    sb.Append(" \"" + f + "=\"+"
                        //+ "JHDBCustomFunctionUse.ToDate(" + var + ".ToString(),false,false)+\" and \"+");

                    }
                    if (keys.Length > 0 && !string.IsNullOrEmpty(keys[0]))
                    {
                        sb.Remove(sb.Length - ",".Length, ",".Length);
                    }
                    return sb.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 获取WHERE字段  Field1=@Field1 strCondition Field1=@Field1...
        /// </summary>
        /// <returns></returns>
        public string GetWhereFields(ColumnsType type,string strCondition)
        {
            StringBuilder sbFields = new StringBuilder();
            if (columns == null)
            {
                throw new Exception("Param.columns 为空");
            }
            else
            {
                sbFields.Append(strCondition);
                string[] cols = null;
                switch (type)
                {
                    case ColumnsType.AllColumns:
                        cols = columns;
                        break;
                    case ColumnsType.NonIdentityColumns:
                        cols = NonIdentityColumns;
                        break;
                    case ColumnsType.KeyColumns:
                        cols = KeyColumns;
                        break;
                }
                foreach (string columnName in cols)
                {
                    sbFields.Append(columnName);// + "=@" + columnName);
                    sbFields.Append("=@");
                    sbFields.Append(columnName);
                    sbFields.Append(strCondition);
                }
                if (sbFields.Length > 0)
                {
                    sbFields.Remove(sbFields.Length - strCondition.Length, strCondition.Length);
                }
            }

            return sbFields.ToString();
        }
        /// <summary>
        /// 获取参数字符串  "idb.AddParameter("@colName",value);"
        /// </summary>
        /// <param name="type">列集合类型</param>
        /// <param name="daoObject">数据访问对象名</param>
        /// <returns></returns>
        public string GetParamaters(ColumnsType type, string daoObject)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(type);
            foreach (NameValue nv in list)
            {
                string strValue = ps.ConvertStringToUpperOrLower(nv.Value.ToString(), false);
                sbAddParams.AppendLine(ps.tabLocalVar + daoObject + ".AddParameter(\"" + nv.Name + "\", " + strValue + ");");
            }
            return sbAddParams.ToString();
        }
        /// <summary>
        /// 获取参数字符串  "idb.AddParameter("@colName",value);"
        /// </summary>
        /// <param name="type">列集合类型</param>
        /// <param name="daoObject">数据访问对象名</param>
        /// <returns></returns>
        public string GetParamaters(ColumnsType type, string daoObject, string classObject)
        {
            StringBuilder sbAddParams = new StringBuilder();
            List<NameValue> list = GetProperties(type, classObject);
            foreach (NameValue nv in list)
            {
                //sbAddParams.AppendLine(ps.tabLocalVar + "if (" + nv.Value.ToString() + " == null)");
                //sbAddParams.AppendLine(ps.tabLocalVar + "{");
                //sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + daoObject + ".AddParameter(\"" + nv.Name + "\", DBNull.Value);");
                //sbAddParams.AppendLine(ps.tabLocalVar + "}");
                //sbAddParams.AppendLine(ps.tabLocalVar + "else");
                //sbAddParams.AppendLine(ps.tabLocalVar + "{");
                //sbAddParams.AppendLine(ps.tabIfLocalVarTop1 + daoObject + ".AddParameter(\"" + nv.Name + "\", " + nv.Value.ToString()+");");
                //sbAddParams.AppendLine(ps.tabLocalVar + "}");
                //2021-2-2-lsj
                sbAddParams.AppendLine(ps.tabMember + "if (" + nv.Value.ToString() + " == null)");
                sbAddParams.AppendLine(ps.tabMember + "{");
                sbAddParams.AppendLine(ps.tabMember + daoObject + ".AddParameter(\"" + nv.Name + "\", DBNull.Value);");
                sbAddParams.AppendLine(ps.tabMember + "}");
                sbAddParams.AppendLine(ps.tabMember + "else");
                sbAddParams.AppendLine(ps.tabMember + "{");
                sbAddParams.AppendLine(ps.tabMember + daoObject + ".AddParameter(\"" + nv.Name + "\", " + nv.Value.ToString() + ");");
                sbAddParams.AppendLine(ps.tabMember + "}");
            }
            return sbAddParams.ToString();
        }
        /// <summary>
        /// 获取 对对象的属性的设置的字符串  user.ID = Convert.ToInt32(dr["columnName"]);
        /// </summary>
        /// <param name="columnName">列名也即对象的属性</param>
        /// <returns></returns>
        public string GetPropertyString(string columnName)
        {
            string ret = "";
            string obj = ClassObject;
            ProcString ps = new ProcString();
            string propertyName = ps.ConvertStringToUpperOrLower(columnName, true);
            DataTable dtType = dsTableInfo.Tables[1];
            DataRow[] drs = dtType.Select("Column_name='" + columnName+"'");
            if (drs.Length > 0)
            {                               
                //ret = "if (dr[\"" + columnName + "\"] != DBNull.Value) " + obj + "." + propertyName + " = (" + ps.ConvertType(drs[0]["Type"].ToString()) + ")dr[\"" + columnName + "\"];";
                ret = "if (dr[\"" + columnName + "\"] != DBNull.Value) " + obj + "." + propertyName + " = " + ps.ConvertField(drs[0]["Type"].ToString(), "dr[\"" + columnName + "\"]")+";";
            }
            return ret;
        }
        /// <summary>
        /// 获取方法参数字符串  string columnName, ......
        /// </summary>
        /// <returns></returns>
        public string GetFunctionParams()
        {
            return GetFunctionParams("");
        }
        /// <summary>
        /// 获取方法参数字符串  string varColumnName, ......
        /// </summary>
        /// <returns></returns>
        public string GetFunctionParams(string varPrefix)
        {
            StringBuilder sb = new StringBuilder();
            ProcString ps = new ProcString();
            DataTable dtType = dsTableInfo.Tables[1].Copy();
            string[] keys = KeyColumns;
            foreach (string field in keys)
            {
                string f = field.Trim();
                DataRow[] drs = dtType.Select("Column_name='" + f + "'");
                if (drs.Length > 0)
                {
                    string type = ps.ConvertType(drs[0]["Type"].ToString(), f,this) + " ";
                    string var = varPrefix + ps.ConvertStringToUpperOrLower(f,false) + ",";
                    sb.Append(type + var);
                }
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

    }
}
