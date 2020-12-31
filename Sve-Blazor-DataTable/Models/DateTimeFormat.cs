using System;
using System.Linq.Expressions;

namespace Sve.Blazor.DataTable.Models
{
    public abstract class DateTimeFormat : Enumeration
    {
        public static readonly DateTimeFormat Date = new DateFormat(1, nameof (Date));
        public static readonly DateTimeFormat DateHourMinute = new DateHourMinuteFormat(2, nameof (DateHourMinute));
        public static readonly DateTimeFormat DateHourMinuteSecond = new DateHourMinuteSecondFormat(3, nameof (DateHourMinuteSecond));

        protected DateTimeFormat(int id, string name)
            : base(id, name)
        {
        }

        public abstract string Format { get; }

        public abstract Expression Expression { get; }

        private class DateFormat : DateTimeFormat
        {
            public override string Format => "yyyy-MM-dd";

            public override Expression Expression => throw new NotImplementedException();

            internal DateFormat(int id, string name)
                : base(id, name)
            {
            }
        }

        private class DateHourMinuteFormat : DateTimeFormat
        {
            public override string Format => "yyyy-MM-ddTHH:mm";

            public override Expression Expression => throw new NotImplementedException();

            internal DateHourMinuteFormat(int id, string name)
                : base(id, name)
            {
            }
        }

        private class DateHourMinuteSecondFormat : DateTimeFormat
        {
            public override string Format => "yyyy-MM-ddTHH:mm:ss";

            public override Expression Expression => throw new NotImplementedException();

            internal DateHourMinuteSecondFormat(int id, string name)
                : base(id, name)
            {
            }
        }
    }
}
