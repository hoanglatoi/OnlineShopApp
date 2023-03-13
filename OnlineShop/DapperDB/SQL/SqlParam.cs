using System;
using System.Collections.Generic;
using System.Text;
using DapperDB;
using System.Reflection;
using DapperDB.Models;
using System.Linq;

namespace DapperDB.SQL
{
    public static class OperatorSymbol
    {
        public const string LikeSymbol = "%";
    }

    public enum OrderByCode
    {
        ASC,
        DESC
    }

    public enum OperatorCode
    {
        /// <summary>
        /// =
        /// </summary>
        EQ,
        /// <summary>
        /// <>
        /// </summary>
        NE,
        /// <summary>
        /// >=
        /// </summary>
        GE,
        /// <summary>
        /// >
        /// </summary>
        GT,
        /// <summary>
        /// <=
        /// </summary>
        LE,
        /// <summary>
        /// <
        /// </summary>
        LT,
        /// <summary>
        /// LIKE
        /// </summary>
        LIKE
    }

    public enum LogicalOperatorCode
    {
        /// <summary>
        /// AND
        /// </summary>
        AND,
        /// <summary>
        /// OR
        /// </summary>
        OR
    }

    public class SqlParam
    {
        public string ColumnName { get; set; }
        public Type ColumnType { get; set; }
        public string Value { get; set; }
        public OperatorCode? Operator { get; set; }
        public LogicalOperatorCode? LogicalOperator { get; set; }
        public OrderByCode? Order { get; set; }

        protected void InitSqlParam(string columnName, Type columnType, string value, OperatorCode? operatorCode, LogicalOperatorCode? logicalOperatorCode, OrderByCode? order)
        {
            ColumnName = columnName;
            ColumnType = columnType;
            //エスケープ文字対策
            if (value != null) Value = value.Replace("'", "''");
            Operator = operatorCode;
            LogicalOperator = logicalOperatorCode;
            Order = order;
        }

        public SqlParam(string columnName, Type columnType, OrderByCode? order)
        {
            InitSqlParam(columnName, columnType, null, null, null, order);
        }

        public SqlParam(string columnName, Type columnType, string value, OperatorCode? operatorCode, LogicalOperatorCode? logicalOperatorCode)
        {
            InitSqlParam(columnName, columnType, value, operatorCode, logicalOperatorCode, null);
        }

        public static String GetOrderByCode(OrderByCode? code)
        {
            if (code == null)
            {
                return "";
            }

            switch (code)
            {
                case OrderByCode.ASC:
                    return "ASC";
                case OrderByCode.DESC:
                    return "DESC";
            }

            return "";
        }

        public static String GetOperator(OperatorCode? code)
        {
            if(code == null)
            {
                return "xxx";
            }

            switch (code)
            {
                case OperatorCode.EQ:
                    return "=";
                case OperatorCode.NE:
                    return "<>";
                case OperatorCode.GE:
                    return ">=";
                case OperatorCode.GT:
                    return ">";
                case OperatorCode.LE:
                    return "<=";
                case OperatorCode.LT:
                    return "<";
                case OperatorCode.LIKE:
                    return " LIKE ";
            }

            return "xxx";
        }

        public static String GetLogicalOperator(LogicalOperatorCode? code)
        {
            if (code == null)
            {
                return "xxx";
            }

            switch (code)
            {
                case LogicalOperatorCode.AND:
                    return "AND";
                case LogicalOperatorCode.OR:
                    return "OR";
            }

            return "xxx";
        }
    }
}
