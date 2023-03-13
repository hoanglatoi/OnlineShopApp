using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;


namespace DapperDB.SQL
{
    public class DBInsertParam<T> : DBParamBase<T> , IDBSqlGet<T> where T : class
    {
        public DBInsertParam(bool isForcedWriting = false , bool autoSetWhere = false, bool autoSetValue = true) : base(isForcedWriting)
        {
            AutoSetWhere = autoSetWhere;
            AutoSetValue = autoSetValue;
        }

        public DBInsertParam(DBUpsertParam<T> param , T data, bool isForcedWriting = false) : base(isForcedWriting)
        {
            AutoSetWhere = false;
            _valueParams = param.GetValueParams(false, data);
            if (_valueParams.Count == 0)
            {
                AutoSetValue = true;
            }
            else
            {
                AutoSetValue = false;
            }
        }

        public string GetSql(T data)
        {
            if (AutoSetValue)
            {
                SetAutoValue(data , true);
            }
            else
            {
                SetValue(data);
            }

            var result = new StringBuilder();
            result.AppendFormat("INSERT INTO {0} ( ", TableName);
            result.Append(GetColumnNameString());
            result.Append(") VALUES ( ");
            result.Append(GetValuesString());
            result.Append(");");

            IsExecutableCheck(result.ToString());

            return result.ToString();
        }

        public void AddValue(string columnName)
        {
            base.AddValue(columnName, new object());
            AutoSetValue = false;
        }
    }
}
