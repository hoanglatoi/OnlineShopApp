using DapperDB.Models;
using DapperDB.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DapperDB
{
    public partial class DapperDBConnection : PostgrSQLConnection
    {

        #region Get

        #region 汎用

        public List<T> GetData<T>(DBSelectParam<T> param) where T : class
        {
            try
            {
                string sql = param.GetSql();
                List<T> results = Select<T>(sql);
                return results;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        public List<T> GetData<T>(string sql) where T : class
        {
            try
            {
                List<T> results = Select<T>(sql);
                return results;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        public DataTable GetData(string sql)
        {
            try
            {
                DataTable results = Select(sql);
                return results;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        #endregion

        #region 特殊

        /// <summary>
        /// 期間指定検索
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="dateColumn"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<T> GetSearchFromPeriod<T>(string tableName, string dateColumn, string start, string end) where T : class
        {
            try
            {
                string sql = DBUtility.MakeDatePeriodSql(tableName, dateColumn, start, end);
                List<T> results = Select<T>(sql);
                return results;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Set

        public bool SetData<T>(IDBSqlGet<T> param, T data) where T : class
        {
            try
            {
                string sql = param.GetSql(data);
                return Execute(sql) == 0 ? false : true;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        public List<bool> SetData<T>(IDBSqlGet<T> param, List<T> datas) where T : class
        {
            List<bool> result = new List<bool>();

            try
            {
                foreach (var data in datas)
                {
                    result.Add(SetData(param, data));
                }
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }

            return result;
        }

        public List<bool> SetData<T>(DBUpsertParam<T> param, List<T> datas) where T : class
        {
            var result = new List<bool>();
            foreach (var data in datas)
            {
                result.Add(SetData(param, data));
            }
            return result;
        }

        public bool SetData<T>(DBUpsertParam<T> param, T data) where T : class
        {
            bool updated;
            return SetData<T>(param, data, out updated);
        }

        public bool SetData<T>(DBUpsertParam<T> param, T data, out bool updated) where T : class
        {
            var update = param.GetUpdateSql(data);
            if (!SetData<T>(update, data))
            {
                updated = false;
                var insert = param.GetInsertSql(data);
                return SetData<T>(insert, data);
            }
            else
            {
                updated = true;
                return true;
            }
        }

        public bool UpdateData<T>(DBUpdateParam<T> param) where T : class
        {
            try
            {
                string sql = param.GetSql();
                return Execute(sql) == 0 ? false : true;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        public bool DeleteData<T>(DBDeleteParam<T> param) where T : class
        {
            try
            {
                string sql = param.GetSql();
                return Execute(sql) == 0 ? false : true;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        #endregion

        #region sql

        public bool SetData(string sql)
        {
            try
            {
                return Execute(sql) == 0 ? false : true;
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
        }

        public List<bool> SetData(List<string> sqls)
        {
            List<bool> result = new List<bool>();
            try
            {
                foreach (var sql in sqls)
                {
                    result.Add(SetData(sql));
                }
            }
            catch (Exception ex)
            {
                // エラー処理は未実装　例外をcatchせず上位で処理？
                throw ex;
            }
            return result;
        }

        #endregion
    }
}
