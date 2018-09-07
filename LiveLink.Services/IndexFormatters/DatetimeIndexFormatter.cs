using System;
using Umbraco.Core;

namespace LiveLink.Services.IndexFormatters
{
    public class DatetimeIndexFormatter : IIndexFormatter<DateTime>
    {
        public string Format(DateTime value)
        {
            return value.ToIsoString();
        }
    }
}