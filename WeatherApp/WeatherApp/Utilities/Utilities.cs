using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherApp.Utilities
{
    public static class Utilities
    {
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
           
            var time=TimeZoneInfo.Local;
            var convert= TimeZoneInfo.ConvertTime(dtDateTime, TimeZoneInfo.FindSystemTimeZoneById(time.Id));
            
            return dtDateTime;
        }
    }
}
