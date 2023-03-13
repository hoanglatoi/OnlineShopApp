using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DapperDB.SQL
{
    public class DBUpsertParam<T> : DBParamBase<T> where T : class
    {
        public List<SqlParam> _AddParams = new List<SqlParam>();

        public DBUpsertParam(bool isForcedWriting = false , bool autoSetWhere = true, bool autoSetValue = true ) : base (isForcedWriting)
        {
            base.AutoSetWhere = autoSetWhere;
            base.AutoSetValue = autoSetValue;
        }

        public new bool AutoSetWhere
        {
            get { return base.AutoSetWhere; }
        }

        public new bool AutoSetValue
        {
            get { return base.AutoSetValue; }
        }

        private List<string> InsertSkipColumnNames = new List<string>();

        public void AddInsertSkipColumn(string columnName)
        {
            InsertSkipColumnNames.Add(columnName);
        }

        public List<SqlParam> GetWhereParams(T data)
        {
            _whereParams = new List<SqlParam>();

            foreach(var param in _AddParams)
            {
                var property = typeof(T).GetProperty(param.ColumnName);

                var colAtt = property.GetCustomAttribute<ColumnAttribute>();
                if (colAtt == null) continue;

                var keyAtt = property.GetCustomAttribute<KeyAttribute>();
                if (keyAtt == null) continue;

                AddWhere(property.Name, new object());
            }

            return _whereParams;
        }

        public List<SqlParam> GetValueParams(bool update , T data)
        {
            _valueParams = new List<SqlParam>();

            foreach (var param in _AddParams)
            {
                var property = typeof(T).GetProperty(param.ColumnName);

                var colAtt = property.GetCustomAttribute<ColumnAttribute>();
                if (colAtt == null) continue;

                var keyAtt = property.GetCustomAttribute<KeyAttribute>();
                if (keyAtt != null && update == true) continue;

                if (SkipColumnName.IndexOf(property.Name) >= 0) continue;

                AddValue(property.Name, new object());
            }

            return _valueParams;
        }

        public void Add(string columnName)
        {
            Type columnType = DBUtility.GetColumType(typeof(T), columnName);
            _AddParams.Add(new SqlParam( columnName , columnType , null, OperatorCode.EQ, LogicalOperatorCode.AND));
        }

        public DBUpdateParam<T> GetUpdateSql(T data)
        {
            return new DBUpdateParam<T>(this, data , IsForcedWriting);
        }

        public DBInsertParam<T> GetInsertSql(T data)
        {
            var param = new DBInsertParam<T>(this, data);

            foreach (var InsertSkip in InsertSkipColumnNames)
            {
                param.AddSkipColumn(InsertSkip);
            }

            return param;
        }
    }
}
