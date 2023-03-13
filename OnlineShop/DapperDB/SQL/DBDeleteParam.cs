using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;

namespace DapperDB.SQL
{
    public class DBDeleteParam<T> : DBParamBase<T>, IDBSqlGet<T> where T : class
    {
        public DBDeleteParam(bool autoSetWhere = true)
        {
            AutoSetWhere = autoSetWhere;
        }

        public string GetSql(T data)
        {
            if (data == null || _whereParams.Count > 0) GetSql();

            if (AutoSetWhere)
            {
                SetAutoKeyWhere(data);
            }
            else
            {
                SetKeyWhere(data);
            }

            return GetSql();
        }

        public string GetSql()
        {
            if (_whereParams.Count == 0)
            {
                _isExecutable = false;
                NotExecutableReason = "Where句の定義なし";
            }

            var result = new StringBuilder();
            result.AppendFormat("DELETE FROM {0} ", TableName);
            result.Append(GetWhereString());
            result.Append(";");

            IsExecutableCheck(result.ToString());

            return result.ToString();
        }

        /// <summary>
        /// Where句の追加
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="columnName">カラム名</param>
        /// <param name="value">値</param>
        public new void AddWhere<U>(string columnName, U value)
        {
            base.AddWhere(columnName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        /// <param name="operatorCode"></param>
        public new void AddWhere<U>(string columnName, U value, OperatorCode operatorCode)
        {
            base.AddWhere(columnName, value, operatorCode);
        }
    }
}
