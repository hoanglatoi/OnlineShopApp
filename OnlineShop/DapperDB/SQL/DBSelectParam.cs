using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;

namespace DapperDB.SQL
{
    public class DBSelectParam<T> : DBParamBase<T> where T : class
    {

        public DBSelectParam()
        {
            
        }

        /// <summary>
        /// Where句の追加
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="columnName">カラム名</param>
        /// <param name="value">値</param>
        public new void AddWhere<U>(string columnName, U value , OperatorCode operatorCode)
        {
            base.AddWhere(columnName, value , operatorCode);
        }

        /// <summary>
        /// OrderBy句の追加
        /// </summary>
        /// <param name="columnName">カラム名</param>
        public new void AddOrderBy(string columnName)
        {
            base.AddOrderBy(columnName);
        }

        /// <summary>
        /// OrderBy句の追加
        /// </summary>
        /// <param name="columnName">カラム名</param>
        /// <param name="code">ソート方向</param>
        public new void AddOrderBy(string columnName, OrderByCode? code)
        {
            base.AddOrderBy(columnName, code);
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
        public virtual string GetSql()
        {
            var result = new StringBuilder();
            result.AppendFormat("SELECT * , count(*) over() as full_count FROM {0} ",TableName);
            result.Append(GetWhereString());
            result.Append(GetOrderByString());
            result.Append(";");
            return result.ToString();
        }

        public void CopyWhere(DBSelectParam<T> param)
        {
            base._whereParams.AddRange(param.GetWhereParams());
        }

        public List<SqlParam> GetWhereParams()
        {
            return base._whereParams;
        }

    }
}
