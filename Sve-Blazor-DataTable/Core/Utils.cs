using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Sve.Blazor.DataTable.Models;

namespace Sve.Blazor.DataTable.Core
{
    public static class Utils
    {
        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            if (!(property.Body is MemberExpression memberExpression))
            {
                try
                {
                    memberExpression = ((UnaryExpression)property.Body).Operand as MemberExpression;
                }
                catch (InvalidCastException ex)
                {
                    throw new Exception("Expression is not a MemeberExpression");
                }
            }

            PropertyInfo member = (PropertyInfo)memberExpression.Member;
            Type propertyType = member.PropertyType;
            string name = member.Name;
            string[] strArray = memberExpression.ToString().Split('.');
            return strArray.Length > 2
                ? string.Join(".", ((IEnumerable<string>)strArray).Skip<string>(1))
                : memberExpression.Member.Name;
        }

        public static Type GetPropertyType<T>(Expression<Func<T, object>> property)
        {
            if (property.Body is MemberExpression memberExpression)
                return ((PropertyInfo)memberExpression.Member).PropertyType;
            try
            {
                memberExpression = ((UnaryExpression)property.Body).Operand as MemberExpression;
            }
            catch (InvalidCastException ex)
            {
                throw new Exception("Expression is not a MemeberExpression");
            }

            return ((PropertyInfo)memberExpression.Member).PropertyType;
        }

        public static Models.PagedResult<T> ApplyPaging<T>(this IQueryable<T> source, Pager pager)
        {
            if (!string.IsNullOrEmpty(pager.SortColumn))
                source = source.OrderBy(pager.SortColumn + " " +
                                           pager.SortDirection.ToString().ToLower());
            
            var count = (pager.PageNr - 1) * pager.PageSize;
            
            return new Models.PagedResult<T>(
                source.Skip(pager.PageNr).Take(count).ToList(),
                pager.PageNr,
                pager.PageSize,
                source.Count(),
                pager.SortColumn,
                pager.SortDirection);
        }

        public static async Task<Models.PagedResult<T>> ApplyPagingAsync<T>(
            this IQueryable<T> source,
            Pager pager)
        {
            if (!string.IsNullOrEmpty(pager.SortColumn))
                source = source.OrderBy<T>(pager.SortColumn + " " + pager.SortDirection.ToString().ToLower(),
                    Array.Empty<object>());
            var skip = (pager.PageNr - 1) * pager.PageSize;
            List<T> result = await source.Skip<T>(skip).Take<T>(pager.PageSize).ToListAsync<T>();
            return new Models.PagedResult<T>(result, pager.PageNr, pager.PageSize, source.Count<T>(), pager.SortColumn,
                pager.SortDirection);
        }

        public static Tuple<T, T> GetMinMaxValue<T>()
        {
            object obj1 = (object)default(T);
            object obj2 = (object)default(T);
            if (obj1 == null || obj2 == null)
                return (Tuple<T, T>)null;
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Char:
                    obj1 = (object)char.MinValue;
                    obj2 = (object)char.MaxValue;
                    break;
                case TypeCode.SByte:
                    obj1 = (object)sbyte.MinValue;
                    obj2 = (object)sbyte.MaxValue;
                    break;
                case TypeCode.Byte:
                    obj1 = (object)(byte)0;
                    obj2 = (object)byte.MaxValue;
                    break;
                case TypeCode.Int16:
                    obj1 = (object)short.MinValue;
                    obj2 = (object)short.MaxValue;
                    break;
                case TypeCode.UInt16:
                    obj1 = (object)(ushort)0;
                    obj2 = (object)ushort.MaxValue;
                    break;
                case TypeCode.Int32:
                    obj1 = (object)int.MinValue;
                    obj2 = (object)int.MaxValue;
                    break;
                case TypeCode.UInt32:
                    obj1 = (object)0U;
                    obj2 = (object)uint.MaxValue;
                    break;
                case TypeCode.Int64:
                    obj1 = (object)long.MinValue;
                    obj2 = (object)long.MaxValue;
                    break;
                case TypeCode.UInt64:
                    obj1 = (object)0UL;
                    obj2 = (object)ulong.MaxValue;
                    break;
                case TypeCode.Single:
                    obj1 = (object)float.MinValue;
                    obj2 = (object)float.MaxValue;
                    break;
                case TypeCode.Double:
                    obj1 = (object)double.MinValue;
                    obj2 = (object)double.MaxValue;
                    break;
                case TypeCode.Decimal:
                    obj1 = (object)Decimal.MinValue;
                    obj2 = (object)Decimal.MaxValue;
                    break;
                case TypeCode.DateTime:
                    obj1 = (object)DateTime.MinValue;
                    obj2 = (object)DateTime.MaxValue;
                    break;
            }

            return Tuple.Create<T, T>((T)obj1, (T)obj2);
        }

        private static bool IsNumber(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsNumber(object obj) => Utils.IsNumber(obj.GetType());
    }
}
