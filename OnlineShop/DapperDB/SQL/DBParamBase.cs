using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DapperDB.SQL
{
    public abstract class DBParamBase<T> : IDBParam where T:class
    {
        /// <summary>
        /// テーブル名
        /// </summary>
        public string TableName { get; set; }

        private string _NullStr = "Null";

        protected bool _isExecutable = true;
        protected bool IsExecutable
        {
            get { return _isExecutable; }
        }

        protected bool IsForcedWriting = false;

        protected string NotExecutableReason { get; set; }

        protected List<SqlParam> _whereParams;
        protected List<SqlParam> _valueParams;
        protected List<SqlParam> _orderbyParams;

        protected bool AutoSetWhere = false;
        protected bool AutoSetValue = false;

        public DBParamBase(bool isForcedWriting = false)
        {
            TableName = DBUtility.GetTableName(typeof(T));
            _whereParams = new List<SqlParam>();
            _valueParams = new List<SqlParam>();
            _orderbyParams = new List<SqlParam>();

            IsForcedWriting = isForcedWriting;
        }

        #region WHERE句

        protected void SetAutoKeyWhere(T data)
        {
            //初期化
            _whereParams = new List<SqlParam>();

            PropertyInfo[] infoArray = data.GetType().GetProperties();

            foreach (PropertyInfo info in infoArray)
            {
                var colAtt = info.GetCustomAttribute<ColumnAttribute>();
                if (colAtt == null) continue;

                var keyAtt = info.GetCustomAttribute<KeyAttribute>();
                if (keyAtt == null) continue;

                var value = info.GetValue(data);
                if (value == null)
                {
                    _isExecutable = false;
                    NotExecutableReason = "where句にNULL指定あり";
                }

                AddWhere(info.Name, value);
            }
        }

        protected void SetKeyWhere(T data)
        {
            foreach (var para in _whereParams)
            {
                var property = data.GetType().GetProperty(para.ColumnName);
                if (property.GetValue(data) != null)
                {
                    para.Value = property.GetValue(data).ToString();
                }
                else
                {
                    para.Value = null;
                }
                
            }
        }

        protected void AddWhere<U>(string columnName, U value , OperatorCode operatorCode)
        {
            Type columnType = DBUtility.GetColumType(typeof(T), columnName);
            string valueStr = null;
            if (value != null) valueStr = value.ToString();
            if (valueStr == new object().ToString()) valueStr = null;
            _whereParams.Add(new SqlParam(columnName, columnType, valueStr, operatorCode, LogicalOperatorCode.AND));
        }

        protected void AddWhere<U>(string columnName, U value)
        {
            AddWhere(columnName, value , OperatorCode.EQ);
        }

        protected string GetWhereString()
        {
            var result = new StringBuilder();
            if (_whereParams.Count > 0)
            {
                result.Append(" WHERE ");
                bool first = true;

                foreach (var para in _whereParams)
                {

                    if (!first)
                    {
                        result.Append(SqlParam.GetLogicalOperator(para.LogicalOperator) + " ");
                    }

                    first = false;
                    string operatorStr = SqlParam.GetOperator(para.Operator);

                    Type paraType = GetTypeSqlParam(para);
                    if (para.Value == _NullStr)
                    {
                        result.AppendFormat("{0}" + operatorStr + "{1} ", para.ColumnName, para.Value);
                    }
                    else
                    {

                        switch (Type.GetTypeCode(paraType))
                        {
                            case TypeCode.String:
                            case TypeCode.DateTime:
                                result.AppendFormat("{0}" + operatorStr + "'{1}' ", para.ColumnName, para.Value);
                                break;
                            default:
                                if (paraType == typeof(System.Guid))
                                {
                                    result.AppendFormat("{0}" + operatorStr + "'{1}' ", para.ColumnName, para.Value);
                                    break;
                                }
                                else
                                {
                                    result.AppendFormat("{0}" + operatorStr + "{1} ", para.ColumnName, para.Value);
                                    break;
                                }
                        }
                    }
                }
            }
            return result.ToString();
        }

        protected void AddOrderBy(string columnName, OrderByCode? operatorCode)
        {
            Type columnType = DBUtility.GetColumType(typeof(T), columnName);
            _orderbyParams.Add(new SqlParam(columnName, columnType, operatorCode));
        }

        protected void AddOrderBy(string columnName)
        {
            AddOrderBy(columnName, null);
        }

        protected string GetOrderByString()
        {
            var result = new StringBuilder();
            if (_orderbyParams.Count > 0)
            {
                result.Append(" ORDER BY ");
                bool first = true;

                foreach (var para in _orderbyParams)
                {

                    if (!first)
                    {
                        result.Append(", ");
                    }

                    first = false;
                    string operatorStr = SqlParam.GetOrderByCode(para.Order);

                    result.AppendFormat("{0}" + " {1} ", para.ColumnName, operatorStr);
                }
            }
            return result.ToString();
        }

        #endregion

        #region VALUE句
        protected void SetAutoValue(T data , bool key = false)
        {
            //初期化
            _valueParams = new List<SqlParam>();

            PropertyInfo[] infoArray = data.GetType().GetProperties();

            foreach (PropertyInfo info in infoArray)
            {
                var colAtt = info.GetCustomAttribute<ColumnAttribute>();
                if (colAtt == null) continue;

                var keyAtt = info.GetCustomAttribute<KeyAttribute>();
                if (keyAtt != null && key == false) continue;

                if(SkipColumnName.IndexOf(info.Name) >= 0) continue;

                var value = info.GetValue(data);
                if (value == null)
                {
                    if(IsForcedWriting) AddValue(info.Name, new object());

                    continue;
                }

                AddValue(info.Name, info.GetValue(data));
            }
        }

        protected List<string> SkipColumnName = new List<string>() { nameof(DbDataBase.full_count) };

        public void AddSkipColumn(string columnName)
        {
            SkipColumnName.Add(columnName);
        }

        protected void SetValue(T data)
        {
            foreach (var para in _valueParams)
            {
                var property = data.GetType().GetProperty(para.ColumnName);
                var value = property.GetValue(data);
                if(value != null)
                {
                    para.Value = property.GetValue(data).ToString();
                }
                else
                {
                    para.Value = null;
                }
                
            }
        }

        protected void AddValue<U>(string columnName, U value)
        {
            Type columnType = DBUtility.GetColumType(typeof(T), columnName);
            string valueStr = null;
            if (value != null) valueStr = value.ToString();
            if (valueStr == new object().ToString()) valueStr = null;
            _valueParams.Add(new SqlParam(columnName, columnType, valueStr, OperatorCode.EQ, null));
        }

        protected string GetSetValueString()
        {
            var result = new StringBuilder();

            if (_valueParams.Count > 0)
            {
                result.Append("SET ");
                bool first = true;

                foreach (var para in _valueParams)
                {

                    if (!first)
                    {
                        result.Append(", ");
                    }

                    first = false;
                    string operatorStr = SqlParam.GetOperator(para.Operator);

                    Type paraType = GetTypeSqlParam(para);

                    if(para.Value == _NullStr)
                    {
                        result.AppendFormat("{0}" + operatorStr + "{1} ", para.ColumnName, para.Value);
                    }
                    else
                    {
                        switch (Type.GetTypeCode(paraType))
                        {
                            case TypeCode.String:
                            case TypeCode.DateTime:
                                result.AppendFormat("{0}" + operatorStr + "'{1}' ", para.ColumnName, para.Value);
                                break;
                            default:
                                result.AppendFormat("{0}" + operatorStr + "{1} ", para.ColumnName, para.Value);
                                break;
                        }
                    }
                }
            }

            return result.ToString();
        }

        protected string GetColumnNameString()
        {
            var result = new StringBuilder();

            if (_valueParams.Count > 0)
            {
                bool first = true;

                foreach (var para in _valueParams)
                {

                    if (!first)
                    {
                        result.Append(", ");
                    }

                    first = false;
                    result.AppendFormat("{0} ", para.ColumnName);
                }
            }

            return result.ToString();
        }

        protected string GetValuesString()
        {
            var result = new StringBuilder();

            if (_valueParams.Count > 0)
            {
                bool first = true;

                foreach (var para in _valueParams)
                {

                    if (!first)
                    {
                        result.Append(", ");
                    }

                    first = false;

                    Type paraType = GetTypeSqlParam(para);
                    if (para.Value == _NullStr)
                    {
                        result.AppendFormat("{0}", para.Value);
                    }
                    else
                    {
                        switch (Type.GetTypeCode(paraType))
                        {
                            case TypeCode.String:
                            case TypeCode.DateTime:
                                result.AppendFormat("'{0}'", para.Value);
                                break;
                            default:
                                result.AppendFormat("{0}", para.Value);
                                break;
                        }
                    }
                }
            }

            return result.ToString();
        }

        #endregion

        private Type GetTypeSqlParam(SqlParam param)
        {
            Type paraType = param.ColumnType;
            try
            {
                if (true == paraType.IsGenericType && paraType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    paraType = Nullable.GetUnderlyingType(paraType);
                }
            }
            catch { }

            if (null == param.Value)
            {
                param.Value = _NullStr;
            }

            return paraType;
        }

        protected bool IsExecutableCheck(string sql)
        {
            if (!IsExecutable)
            {
                throw new Exception(NotExecutableReason + " => sql: " + sql);
            }

            return true;
        }
    }

}
