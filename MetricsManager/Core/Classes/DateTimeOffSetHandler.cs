using Dapper;
using System.Data;
using System;


namespace Core
{
    public class DateTimeOffSetHandler : SqlMapper.TypeHandler<DateTimeOffset>
    {
        public override DateTimeOffset Parse(object value)
            => DateTimeOffset.FromUnixTimeSeconds((Int64)value);

        public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
            => parameter.Value = value;
    }

}
