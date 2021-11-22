using System;

namespace Utils.Datetime
{
    public static class DatetimeExtentions
    {
        public static DateTime ToDateTime(this object obj) => Convert.ToDateTime(obj);

    }
}
