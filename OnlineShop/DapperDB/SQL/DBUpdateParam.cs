using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;


namespace DapperDB.SQL
{

    public class DBUpdateParam<T> : DBParamBase<T> , IDBSqlGet<T> where T : class
    {
        bool updateDetail = false;

        public DBUpdateParam(bool isForcedWriting = false,  bool autoSetWhere = true  , bool autoSetValue = true) : base(isForcedWriting)
        {
            AutoSetWhere = autoSetWhere;
            AutoSetValue = autoSetValue;
        }

        public DBUpdateParam(DBUpsertParam<T> param , T data, bool isForcedWriting = false) : base(isForcedWriting)
        {
            _whereParams = param.GetWhereParams(data);
            if(_whereParams.Count == 0)
            {
                AutoSetWhere = true;
            }
            else
            {
                AutoSetWhere = false;
            }

            _valueParams = param.GetValueParams(true , data);
            if(_valueParams.Count == 0)
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
            if (AutoSetWhere)
            {
                SetAutoKeyWhere(data);
            }
            else
            {
                SetKeyWhere(data);
            }

            if (AutoSetValue)
            {
                SetAutoValue(data);
            }
            else
            {
                SetValue(data);
            }

            updateDetail = true;

            return GetSql();
        }

        public string GetSql()
        {
            if (!updateDetail)
            {
                _isExecutable = false;
                NotExecutableReason = "UPDATEの設定不備";
            }

            var result = new StringBuilder();
            result.AppendFormat("UPDATE {0} ", TableName);
            result.Append(GetSetValueString());
            result.Append(GetWhereString());
            result.Append(";");

            IsExecutableCheck(result.ToString());

            return result.ToString();
        }

        public void AddWhere(string columnName)
        {
            base.AddWhere(columnName, new object());
            AutoSetWhere = false;
        }

        public new void AddWhere<U>(string columnName, U value)
        {
            base.AddWhere(columnName, value);
            AutoSetWhere = false;
            updateDetail = true;
        }

        public new void AddWhere<U>(string columnName, U value, OperatorCode operatorCode)
        {
            base.AddWhere(columnName, value, operatorCode);
            AutoSetWhere = false;
            updateDetail = true;
        }

        public void AddValue(string columnName)
        {
            base.AddValue(columnName , new object());
            AutoSetValue = false;
        }

        public new void AddValue<U>(string columnName, U value)
        {
            base.AddValue(columnName, value);
            AutoSetValue = false;
            updateDetail = true;
        }
    }
}
