using System;
using System.Linq.Expressions;
using Sve.Blazor.DataTable.Models;

namespace Sve.Blazor.DataTable.Components
{
    public class FilterRule<TModel>
    {
        public Guid Guid { get; private set; }

        public DataTableColumn<TModel> Column { get; private set; }

        public string PropertyName { get; private set; }

        public ObjectFilter FilterType { get; set; }

        public Type ExpectedValueType { get; private set; }

        public dynamic? FilterValue { get; private set; } = null;

        public bool IsApplied { get; set; } = false;

        public bool IsNullable { get; private set; } = false;

        public FilterRule(DataTableColumn<TModel> column, Type propertyType, string propertyName, ObjectFilter objectFilter)
        {
            Guid = Guid.NewGuid();
            Column = column;
            FilterType = objectFilter;
            PropertyName = propertyName;

            UpdatePropertyType(propertyType);
        }

        public void UpdateFilterProperty(DataTableColumn<TModel> column, Type propertyType, string propertyName)
        {
            Column = column;
            PropertyName = propertyName;
            UpdatePropertyType(propertyType);
        }

        public void UpdateFilterValue(ValueChangedEventArgs valueChangedEventArgs)
        {
            FilterValue = valueChangedEventArgs.Value;
        }

        public Expression<Func<TModel, bool>> GenerateExpression()
        {
            if (Type.GetTypeCode(ExpectedValueType) != TypeCode.DateTime)
                return FilterType.GenerateExpression<TModel>(Column.GetColumnPropertyName(), FilterValue);
            
            if (Column.DateTimeFormat.Equals(DateTimeFormat.Date)) return FilterType.GenerateExpression<TModel>($"{Column.GetColumnPropertyName()}.Date", FilterValue?.Date);

            if (!Column.DateTimeFormat.Equals(DateTimeFormat.DateHourMinute))
                return Column.DateTimeFormat.Equals(DateTimeFormat.DateHourMinuteSecond)
                    ? FilterType.GenerateExpression<TModel>(Column.GetColumnPropertyName(), FilterValue)
                    : FilterType.GenerateExpression<TModel>(Column.GetColumnPropertyName(), FilterValue);
            
            var dateExpression = FilterType.GenerateExpression<TModel>($"{Column.GetColumnPropertyName()}.Date", FilterValue?.Date);
            var hourExpression = FilterType.GenerateExpression<TModel>($"{Column.GetColumnPropertyName()}.Hour", FilterValue?.Hour);
            var minuteExpression = FilterType.GenerateExpression<TModel>($"{Column.GetColumnPropertyName()}.Minute", FilterValue?.Minute);
            var p1 = PredicateBuilder.And(dateExpression, hourExpression);
            
            return PredicateBuilder.And(p1, minuteExpression);
        }

        private void UpdatePropertyType(Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType)!;
                IsNullable = true;
            }

            ExpectedValueType = propertyType;

            if (propertyType.IsEnum) FilterValue = Enum.GetNames(propertyType)[0];
            else
            {
                switch (Type.GetTypeCode(propertyType))
                {
                    case TypeCode.Int16:
                        FilterValue = default(short);
                        break;
                    case TypeCode.Int32:
                        FilterValue = default(int);
                        break;
                    case TypeCode.Int64:
                        FilterValue = default(long);
                        break;
                    case TypeCode.UInt16:
                        FilterValue = default(ushort);
                        break;
                    case TypeCode.UInt32:
                        FilterValue = default(uint);
                        break;
                    case TypeCode.UInt64:
                        FilterValue = default(ulong);
                        break;
                    case TypeCode.Double:
                        FilterValue = default(double);
                        break;
                    case TypeCode.Decimal:
                        FilterValue = default(decimal);
                        break;
                    case TypeCode.Byte:
                        FilterValue = default(byte);
                        break;
                    case TypeCode.Boolean:
                        FilterValue = false;
                        break;
                    case TypeCode.String:
                        FilterValue = "";
                        break;
                    case TypeCode.DateTime:
                        FilterValue = DateTime.UtcNow;
                        break;

                    // TODO: Some types might be possible
                    case TypeCode.Object:
                    case TypeCode.Char:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                        throw new Exception("Unsupported property type for filtering");
                    case TypeCode.DBNull:
                        break;
                    case TypeCode.Empty:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string GetAppliedFilterRuleText()
        {
            if (FilterType.ValueRequired)
            {
                return Type.GetTypeCode(ExpectedValueType) == TypeCode.DateTime
                    ? $"{Column.GetColumnVisualPropertyName()}\t{FilterType}\t{FilterValue!.ToString(Column.DateTimeFormat.Format)}"
                    : $"{Column.GetColumnVisualPropertyName()}\t{FilterType}\t{FilterValue}";
            }

            return $"{Column.GetColumnVisualPropertyName()}\t{FilterType}";
        }
    }
}
