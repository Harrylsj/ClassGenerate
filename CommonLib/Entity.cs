using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Xml;

namespace CommonLib.Entity
{
    public class JHEntity
    {
        PropertyInfo prop;
        String PropName;
        String PropValue;
        Type type;
        object value;
        void PropSetValue(object tabObj)
        {
            try
            {
                //DataRow支持和此表无关的列
                if (type.GetProperties().ToList().Where(p => p.Name == PropName).ToList().Count > 0)
                {
                    prop = type.GetProperty(PropName);
                    //枚举类型无法直接转换
                    if (prop.PropertyType.BaseType == typeof(Enum))
                        value = Enum.Parse(prop.PropertyType, PropValue);
                    else
                    {
                        if (prop.PropertyType == typeof(DateTime) && PropValue == "")
                        {
                            value = Convert.ToDateTime("1900-1-1");
                        }
                        else if (prop.PropertyType == typeof(Decimal) && PropValue == "")
                        {
                            value = Convert.ChangeType(0, prop.PropertyType);
                        }
                        else
                        {
                            value = Convert.ChangeType(PropValue, prop.PropertyType);
                        }
                    }
                    prop.SetValue(tabObj, value, null);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 创建泛型类型的实例并将属性赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public T CreateModel<T>(DataRow drReader)
        {
            try
            {
                T tabObj = Activator.CreateInstance<T>();
                type = tabObj.GetType();
                for (int j = 0; j < drReader.Table.Columns.Count; j++)//DataRow支持此表部分属性，不需要提供全部属性列
                {
                    PropName = drReader.Table.Columns[j].ToString();
                    PropValue = drReader[PropName].ToString();
                    value = null;
                    PropSetValue(tabObj);
                }
                return tabObj;
                #region 
                #endregion
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public T CreateModel<T>(DataSet dsReader)
        {
            if (dsReader.Tables.Count > 0)
            {
                return CreateModel<T>(dsReader.Tables[0].Rows[0]);
            }
            else
            {
                return Activator.CreateInstance<T>();
            }
        }
        /// <summary>
        /// 返回泛型类型的所有实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public List<T> CreateModels<T>(DataSet dsReader)
        {
            List<T> objs = new List<T>();
            if (dsReader.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsReader.Tables[0].Rows.Count; i++)
                {
                    T obj = CreateModel<T>(dsReader.Tables[0].Rows[i]);
                    if (!objs.Contains(obj))
                        objs.Add(obj);
                }
            }
            return objs;
        }


        ///// <summary>
        ///// 返回表名类型的所有实体@SXX 2013-02-01
        ///// </summary>
        ///// <param name="reader"></param>
        ///// <returns></returns>
        //public List<Object> CreateModels(DataSet dsReader)
        //{
        //    List<Object> objs = new List<Object>();
        //    //if (dsReader.Tables[0].Rows.Count > 0)
        //    //{
        //    //    for (int i = 0; i < dsReader.Tables[0].Rows.Count; i++)
        //    //    {
        //    //        Object obj = CreateModel<Object>(dsReader.Tables[0].Rows[i]);
        //    //        if (!objs.Contains(obj))
        //    //            objs.Add(obj);
        //    //    }
        //    //}
        //    return objs;
        //}


        /// <summary>
        /// 根据部分属性（DataRow）初始化对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="drReader">
        /// 1、DataRow支持此表部分属性，不需要提供全部属性列；
        /// 2、DataRow支持和此表无关的列。</param>
        /// <returns></returns>
        public T CreateModel_Partial<T>(DataRow drReader)
        {
            try
            {
                T tabObj = Activator.CreateInstance<T>();
                type = tabObj.GetType();
                for (int j = 0; j < drReader.Table.Columns.Count; j++)//DataRow支持此表部分属性，不需要提供全部属性列
                {
                    PropName = drReader.Table.Columns[j].ToString();
                    PropValue = drReader[PropName].ToString();
                    //object obj = Convert.ChangeType(xnlTabInfo.Item(j).InnerText, type.GetProperty(xnlTabInfo.Item(j).Name).PropertyType);
                    value = null;
                    PropSetValue(tabObj);
                }
                return tabObj;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public T CreateModel<T>(String strXML)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(strXML);
            T tabObj = Activator.CreateInstance<T>();
            //XmlElement xmlProp = (XmlElement)xmlDoc.ChildNodes.Item(1);
            //XmlNodeList xnlTabInfo = xmlProp.ChildNodes;
            XmlNodeList xnlTabInfo = xmlDoc.ChildNodes[0].ChildNodes;
            type = tabObj.GetType();
            for (int j = 0; j < xnlTabInfo.Count; j++)
            {
                value = null;
                PropName = xnlTabInfo.Item(j).Name;
                PropValue = xnlTabInfo.Item(j).InnerText;
                PropSetValue(tabObj);
            }
            return tabObj;
        }
        public T CreateModel_WithoutTableName<T>(String strXML)
        {
            T tabObj = Activator.CreateInstance<T>();
            type = tabObj.GetType();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<" + type.Name + ">" + strXML + "</" + type.Name + ">");
            XmlNodeList xnlTabInfo = xmlDoc.ChildNodes[0].ChildNodes;
            for (int j = 0; j < xnlTabInfo.Count; j++)
            {
                value = null;
                PropName = xnlTabInfo.Item(j).Name;
                PropValue = new CommonFunction().UnESC_XML(xnlTabInfo.Item(j).InnerText);
                PropSetValue(tabObj);
            }
            //foreach (PropertyInfo prop in tabObj.GetType().GetProperties())
            //{
            //    //默认字段名为属性名
            //     PropName = prop.Name;
            //    XmlNode Node = xmlDoc.SelectSingleNode("/" + type.Name + "/" + PropName);
            //     PropValue = Node.InnerXml;
            //    if (!String.IsNullOrEmpty( PropValue ))
            //    {
            //        value = null;
            //        PropSetValue(tabObj);
            //    }
            //}
            return tabObj;
        }
        /// <summary>
        /// 克隆实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldT"></param>
        /// <returns></returns>
        public T CloneEntity<T>(T oldT)
        {
            try
            {
                T obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    //默认字段名为属性名
                    String prop_name = prop.Name;
                    object s = prop.GetValue(oldT, null);
                    if (oldT != null)
                    {
                        object value = null;
                        //枚举类型无法直接转换
                        if (s != null)
                        {
                            if (prop.PropertyType.BaseType == typeof(Enum))
                                value = Enum.Parse(prop.PropertyType, prop.GetValue(oldT, null).ToString());
                            else
                                value = Convert.ChangeType(prop.GetValue(oldT, null), prop.PropertyType);
                            prop.SetValue(obj, value, null);
                        }
                        else
                        {
                            prop.SetValue(obj, "", null);
                        }
                    }
                }
                return obj;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
