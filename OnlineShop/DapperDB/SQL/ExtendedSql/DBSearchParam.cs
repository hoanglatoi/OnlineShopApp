using DapperDB.SQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace DapperDB.SQL.ExtendedSql
{
    public class DBSearchParam<T> : DBSelectParam<T> where T : class
    {
        public DBSearchParam()
        {

        }
    }
}
