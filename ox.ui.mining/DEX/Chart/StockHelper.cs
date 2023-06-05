using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace OX.UI.Mining.DEX.Chart
{
    public static class StockHelper
    {
        public static string ToTimeString(this long Date, bool isHour)
        {
            if (isHour)
            {
                var y = Date / 1000000;
                var m = Date % 1000000;
                var ms = m / 10000;
                var ds = m % 10000;
                var dd = ds / 100;
                var h = ds % 100;
                var month = ms < 10 ? "0" + ms.ToString() : ms.ToString();
                var day = dd < 10 ? "0" + dd.ToString() : dd.ToString();
                var hour = h < 10 ? "0" + h.ToString() : h.ToString();
                return $"{y}-{month}-{day} : {hour}H";
            }
            else
            {
                var y = Date / 10000;
                var m = Date % 10000;
                var ms = m / 100;
                var ds = m % 100;
                var month = ms < 10 ? "0" + ms.ToString() : ms.ToString();
                var day = ds < 10 ? "0" + ds.ToString() : ds.ToString();
                return $"{y}-{month}-{day}";
            }
        }
        public static long ToDayLong(this uint timestamp)
        {
            var dt = timestamp.ToDateTime();
            return (long)dt.Year * 10000 + (long)dt.Month * 100 + dt.Day;
        }
        public static string ToDayString(this long dayLong)
        {
            var l = dayLong;
            var day = l % 100;
            var m = dayLong % 10000;
            var month = m / 100;
            var year = dayLong / 10000;
            return $"{year}-{month}-{day}";
        }

        public static long ToDateValue(this DateTime dt)
        {
            return (long)dt.Year * 10000 + (long)dt.Month * 100 + dt.Day;
        }
        public static long ToHourLong(this uint timestamp)
        {
            var dt = timestamp.ToDateTime();
            return (long)dt.Year * 1000000 + (long)dt.Month * 10000 + (long)dt.Day * 100 + dt.Hour;
        }

        public static string ToHourString(this long hourLong)
        {
            var hour = hourLong % 100;
            var m = hourLong % 10000;
            var day = m /100;
            var month = (hourLong % 1000000) / 10000;
            var year = hourLong / 1000000;
            return $"{year}-{month}-{day} {hour}:00:00";
        }
    }
}
