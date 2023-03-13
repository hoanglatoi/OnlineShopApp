using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDB.SQL
{
    public interface IDBSqlGet<T>
    {
        string GetSql(T data);
    }
}
