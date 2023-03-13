using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Npgsql;
using Dapper;

namespace DapperDB
{
    public abstract class PostgrSQLConnection : IDisposable
    {
        private static readonly log4net.ILog _logger
           = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region property

        ///<summary>
        ///コネクション文字列
        ///</summary>
        public string ConnectionString { get; set; }

        ///<summary>
        ///データベース接続
        ///</summary>
        private IDbConnection Connection { get; set; }

        /// <summary>
        /// トランザクション
        /// </summary>
        private IDbTransaction Transaction { get; set; }

        /// <summary>
        /// 接続状態
        /// </summary>
        public Boolean IsConnected
        {
            get
            {
                return (Connection.State == ConnectionState.Open);
            }
        }

        #endregion

        #region constructor
        ///<summary>
        /// コンストラクタ
        ///</summary>
        public PostgrSQLConnection(string connectionString)
        {
            if (null != connectionString)
                ConnectionString = connectionString;
            Connection = new NpgsqlConnection(ConnectionString);
        }
        #endregion

        #region method
        /// <summary>
        /// コネクションオープン
        /// </summary>
        public void ConnectionOpen()
        {
            if (!IsConnected)
            {
                Connection.Open();
            }
        }

        /// <summary>
        /// コネクションクローズ
        /// </summary>
        public void ConnectionClose()
        {
            if (IsConnected)
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            ConnectionOpen();
            Transaction = Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void CommitTransaction()
        {
            if (Transaction != null && Transaction.Connection != null)
            {
                Transaction.Commit();
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        public void RollBackTransaction()
        {
            if (Transaction != null && Transaction.Connection != null)
            {
                Transaction.Rollback();
            }
        }

        /// <summary>
        /// データ抽出
        /// </summary>
        public List<t> Select<t>(string query, object parameters) where t : class
        {
            return Connection.Query<t>(query, parameters).ToList();

        }

        /// <summary>
        /// データ抽出
        /// </summary>
        public List<t> Select<t>(string query) where t : class
        {
            return Connection.Query<t>(query).ToList();
        }

        public SqlMapper.GridReader SelectMultiple(string query)
        {
            return Connection.QueryMultiple(query);
        }

        /// <summary>
        /// データ抽出
        /// </summary>
        public DataTable Select(string query)
        {
            var result = new DataTable();
            result.Load(Connection.ExecuteReader(query));
            return result;
        }

        /// <summary>
        /// SQL実行
        /// </summary>
        public int Execute(string query)
        {
            _logger.Debug("SQL => " + query);
            return Connection.Execute(query);
        }

        /// <summary>
        /// SQL実行
        /// </summary>
        public int Execute(string query, object parameters)
        {
            return Connection.Execute(query, parameters);
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                    if(Connection != null) Connection.Dispose();
                }

                // TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージ リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~PostgrSQLConnection() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
