﻿using System.Text;

namespace Nintendo.Aamp.IO
{
    internal static class StringExtensions
    {
        internal static string Indent(this string value, int size)
        {
            var strArray = value.Split('\n');
            var sb = new StringBuilder();
            foreach (var s in strArray)
                sb.Append(new string(' ', size)).Append(s);
            return sb.ToString();
        }
    }
}
