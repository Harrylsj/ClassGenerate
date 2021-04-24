using System;
using System.Collections.Generic;
using System.Text;

namespace ClassGenerate.Proc
{
    public class NameValue
    {
        public NameValue() { }
        public NameValue(string name, string fieldName, object value) 
        {
            Name = name;
            FieldName = fieldName;
            Value = value;
        }
        public string Name;
        public string FieldName;//NAME
        public string MemberName;
        public object Value;//name_
        public string FieldType;
        public string ValueIsNotEmpty;
    }
}
