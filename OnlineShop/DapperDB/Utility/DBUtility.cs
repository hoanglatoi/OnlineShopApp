using Dapper;
using Npgsql;
using DapperDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DapperDB
{

    public enum ValueConversionCode { NONE, DB_DATE , AutomaticWarehouse}
    public static class DBUtility
    {
        #region DB Attribute

        public static string GetTableName(Type t)
        {
            TableAttribute table =
                (TableAttribute)Attribute.GetCustomAttribute(t, typeof(TableAttribute));
            if (null != table && table.Name != null && 0 != table.Name.Length)
            {
                return table.Name;
            }
            else
            {
                return t.Name;
            }
        }

        public static string GetColumnName(Type t, string columnName)
        {
            var column = GetColumnDetails(t, columnName);
            return column != null ? column.Name : string.Empty;
        }

        public static string GetColumnTypeName(Type t, string columnName)
        {
            var column = GetColumnDetails(t, columnName);
            return column != null ? column.TypeName : string.Empty;
        }

        public static ColumnAttribute GetColumnDetails(Type t, string columnName)
        {
            foreach (var info in t.GetProperties())
            {
                if (info.Name == columnName)
                {
                    ColumnAttribute column =
                    (ColumnAttribute)Attribute.GetCustomAttribute(info, typeof(ColumnAttribute));

                    return column;
                }
            }
            return null;
        }

        public static PropertyInfo GetPropertyInfo<T>(string columnName)
        {
            foreach (var info in typeof(T).GetProperties())
            {
                if (info.Name == columnName)
                {
                    return info;
                }
            }
            return null;
        }

        public static Type GetColumType(Type t, string columnName)
        {
            PropertyInfo[] infoArray = t.GetProperties();

            // プロパティ情報出力をループで回す
            foreach (PropertyInfo info in infoArray)
            {
                if (info.Name == columnName)
                {
                    return info.PropertyType;
                }
            }

            return null;
        }

        #endregion

        #region データ変換

        /// <summary>
        /// 日付変換（DateTime=>DB日付文字列）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetDateTimeConversion(DateTime? value, ValueConversionCode code = ValueConversionCode.DB_DATE)
        {
            if (value == null) return null;

            string result = "";

            DateTime date = (DateTime)value;

            switch (code)
            {
                case ValueConversionCode.DB_DATE:
                    result = date.ToString(DBParam.SqlTimeFormat);
                    break;
            }

            return result;
        }

        public static DateTime GetDateTimeFromDBNotNull(string dateTimeStr, ValueConversionCode code = ValueConversionCode.DB_DATE)
        {
            DateTime? result = GetDateTimeFromDB(dateTimeStr, code);
            return result == null ? default(DateTime) : (DateTime)result;
        }

        /// <summary>
        /// 日付変換（DB日付文字列=>DateTime）
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <returns></returns>
        public static DateTime? GetDateTimeFromDB(string dateTimeStr ,ValueConversionCode code = ValueConversionCode.DB_DATE)
        {
            if (dateTimeStr == null) return null;

            DateTime? result = null;
            try
            {
                switch (code)
                {
                    case ValueConversionCode.DB_DATE:
                        result = DateTime.ParseExact(dateTimeStr, DBParam.SqlTimeFormat, System.Globalization.DateTimeFormatInfo.InvariantInfo);
                        break;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region Search

        public static string MakeDatePeriodSql(string tableName , string dateColumn , string start, string end, int limit = DBParam.SelectLimit) 
        {
            //string query = string.Format(
            //    "SELECT * , count(*) over() as full_count FROM {0}  WHERE {1}>='{2}' and {3}<='{4}' limit " + limit + "; ",
            //    tableName,
            //    dateColumn,
            //    start,
            //    dateColumn,
            //    end
            //    );
            string query = string.Format(
               "SELECT * , count(*) over() as full_count FROM {0}  WHERE {1}>='{2}' and {3}<='{4}'; ",
               tableName,
               dateColumn,
               start,
               dateColumn,
               end
               );
            return query;
        }

        #endregion

    }
}
