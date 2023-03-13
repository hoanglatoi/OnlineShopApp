using DapperDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DapperDB
{
    public partial class DapperDBConnection : PostgrSQLConnection
    {
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

        public static Type[] GetSubclass<T>() where T : DbDataBase
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsSubclassOf(typeof(T))).ToArray();
        }

        public static Type[] GetSubclass<T>(Assembly targetassembly) where T : DbDataBase
        {
            return targetassembly.GetTypes().Where(c => c.IsSubclassOf(typeof(T))).ToArray();
        }


        static void MakeTableNames()
        {
            MakeTableNames(Assembly.GetExecutingAssembly());
        }

        public static void MakeTableNames(Assembly target)
        {
            var tables = GetSubclass<DbDataBase>(target);

            foreach (var table in tables)
            {
                var method = table.GetMethod("SetTableName");
                method.Invoke(null, new object[] { GetTableName(table) });
            }
        }

        public static void Prepare()
        {
            MakeTableNames();
        }

        public DapperDBConnection(string connectionString) : base(connectionString)
        {
        }
    }
}
