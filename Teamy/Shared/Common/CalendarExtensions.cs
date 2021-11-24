using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teamy.Shared.ViewModels;

namespace Teamy.Shared.Common
{
    public static class CalendarExtensions
    {
        public static string CalendarFile(this EventVM eventVM, string baseUri)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:teamy.one");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:PUBLISH");

            //create a time zone if needed, TZID to be used in the event itself
            //sb.AppendLine("BEGIN:VTIMEZONE");
            //sb.AppendLine("TZID:Europe/Amsterdam");
            //sb.AppendLine("BEGIN:STANDARD");
            //sb.AppendLine("TZOFFSETTO:+0100");
            //sb.AppendLine("TZOFFSETFROM:+0100");
            //sb.AppendLine("END:STANDARD");
            //sb.AppendLine("END:VTIMEZONE");

            sb.AppendLine("BEGIN:VEVENT");

            //with time zone specified
            //sb.AppendLine("DTSTART;TZID=Europe/Amsterdam:" + DateStart.ToString("yyyyMMddTHHmm00"));
            //sb.AppendLine("DTEND;TZID=Europe/Amsterdam:" + DateEnd.ToString("yyyyMMddTHHmm00"));
            //or without
            sb.AppendLine("DTSTART:" + eventVM.EventDate.Value.ToString("yyyyMMddTHHmm00"));
            sb.AppendLine("DTEND:" + eventVM.EventDateTo.Value.ToString("yyyyMMddTHHmm00"));

            sb.AppendLine("SUMMARY:" + eventVM.Title);
            sb.AppendLine("LOCATION:" + eventVM.Where);
            sb.AppendLine("DESCRIPTION:" + $"{eventVM.Description} {baseUri}Event/{eventVM.Id}");
            sb.AppendLine("PRIORITY:3");
            sb.AppendLine("END:VEVENT");

            sb.AppendLine("END:VCALENDAR");

            return sb.ToString();
        }
    }
}
