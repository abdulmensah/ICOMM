using System;


namespace FullCalendar_MVC.Utilities
{

    public static class StringExtensions
    {
        public static string CutString(this string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength))+"...";
        }

    }
}