using System;
using System.Collections.Generic;

namespace Ametrin.Utils
{
    public static class SpanExtensions
    {
        public static List<Range> Split(this ReadOnlySpan<char> span, char delimiter)
        {
            var start = 0;
            var result = new List<Range>();
            for (int i = 0; i < span.Length; i++)
            {
                if (span[i] == delimiter)
                {
                    if (i > start) result.Add(new Range(start, i));

                    start = i + 1;
                }
            }

            if (span.Length > start) result.Add(new Range(start, span.Length));
            return result;
        }

        public static bool All<T>(this ReadOnlySpan<T> span, Func<T, bool> condition)
        {
            foreach (var element in span)
            {
                if (!condition(element)) return false;
            }
            return true;
        }
    }
}