using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FluidDbClient;

namespace DemoDb
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> seq, Action<T> doThis)
        {
            foreach (var item in seq)
            {
                doThis(item);
            }
        }

        public static string ToDiagnosticString(this IDataRecord rec)
        {
            var map = rec.ToDictionary();

            return string.Join(", ", map.Select(kvp => $"{kvp.Key} = {kvp.Value}"));
        }
    }
}
