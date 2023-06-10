using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Moduler.Helpers
{
    public static class ArrayExt
    {
        public static int FindIndex<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindIndex(array, match);
        }
        public static T[] columnDistinct<T>(T[][] jaggedArray, int wanted_column)
        {
            T[] columnArray = new T[jaggedArray.Length];
            T[] rowArray;
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                rowArray = jaggedArray[i];
                if (wanted_column < rowArray.Length)
                    columnArray[i] = rowArray[wanted_column];
            }

            columnArray = columnArray.ToList().GroupBy(x => x).Select(x => x.First()).ToArray();
            return columnArray;
        }
        public static T[][] tableDistinct<T>(T[][] jaggedArray, int[] excepted_columns)
        {
            try
            {
                List<T[]> columnArray = new List<T[]>();
                columnArray.Add(jaggedArray[0]);
                for (int i = 1; i < jaggedArray.Length; i++)
                {
                    T[] rowArray = jaggedArray[i];
                    var previousArray = jaggedArray[i - 1];
                    var duplicate = true;
                    for (int j = 1; j < rowArray.Length; j++)
                    {
                        if (!excepted_columns.Contains(j))
                        {
                            if (rowArray[j] as String != previousArray[j] as String)
                            {
                                duplicate = false;
                            }
                        }
                    }
                    if (!duplicate)
                    {
                        columnArray.Add(rowArray);
                    }
                }
                return columnArray.ToArray();
            }
            catch (Exception)
            {
                return jaggedArray;
            }
        }
        public static T[] column<T>(T[][] jaggedArray, int wanted_column)
        {
            T[] columnArray = new T[jaggedArray.Length];
            T[] rowArray;
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                rowArray = jaggedArray[i];
                if (wanted_column < rowArray.Length)
                    columnArray[i] = rowArray[wanted_column];
            }
            return columnArray;
        }
        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException("array");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

            return result;
        }

        static void FindMatchesInStringArray(string markup, string[] strArray)
        {
            for (int i = 0; i < strArray.Length; i++)
            {
                Match Match = Regex.Match(markup, strArray[i], RegexOptions.IgnoreCase);
                if (Match.Success)
                {
                    Console.WriteLine(Match.Groups[0]);
                }
            }
        }
        public static Match FindMatches<T>(this string[] strArray, string markup)
        {
            for (int i = 0; i < strArray.Length; i++)
            {
                Match Match = Regex.Match(markup, strArray[i], RegexOptions.IgnoreCase);
                if (Match.Success)
                {
                    return Match;
                }
            }
            return null;
        }

        public static string[,] RemoveEmptyRows(string[,] strs)
        {
            int length1 = strs.GetLength(0);
            int length2 = strs.GetLength(1);

            // First we count the non-emtpy rows
            int nonEmpty = 0;

            for (int i = 0; i < length1; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    if (strs[i, j] != null)
                    {
                        nonEmpty++;
                        break;
                    }
                }
            }

            // Then we create an array of the right size
            string[,] strs2 = new string[nonEmpty, length2];

            for (int i1 = 0, i2 = 0; i2 < nonEmpty; i1++)
            {
                for (int j = 0; j < length2; j++)
                {
                    if (strs[i1, j] != null)
                    {
                        // If the i1 row is not empty, we copy it
                        for (int k = 0; k < length2; k++)
                        {
                            strs2[i2, k] = strs[i1, k];
                        }

                        i2++;
                        break;
                    }
                }
            }

            return strs2;
        }
        public static string[][] RemoveEmptyColumns(string[][] strs)
        {
            int length1 = strs.Length;
            int length2 = strs[0].Length;

            // Then we create an array of the right size
            string[][] strs2 = new string[length1][];

            for (int i = 0; i < length1; i++)
            {
                var newList = new List<string>();
                for (int j = 0; j < length2; j++)
                {
                    if (strs[0][j] != null)
                    {
                        newList.Add(strs[i][j]);
                    }
                }
                strs2[i] = newList.ToArray();
            }

            return strs2;
        }
        public static string ToString<T>(this T[] array, string delimiter)
        {
            if (array != null)
            {
                // determine if the length of the array is greater than the performance threshold for using a stringbuilder
                // 10 is just an arbitrary threshold value I've chosen
                if (array.Length < 10)
                {
                    // assumption is that for arrays of less than 10 elements
                    // this code would be more efficient than a StringBuilder.
                    string[] values = new string[array.Length];

                    for (int i = 0; i < values.Length; i++)
                        values[i] = array[i].ToString();

                    return string.Join(delimiter, values);
                }
                else
                {
                    // for arrays of length 10 or longer, use a StringBuilder
                    StringBuilder sb = new StringBuilder();

                    sb.Append(array[0]);
                    for (int i = 1; i < array.Length; i++)
                    {
                        sb.Append(delimiter);
                        sb.Append(array[i]);
                    }

                    return sb.ToString();
                }
            }
            else
            {
                return null;
            }
        }
        //public static IEnumerable<TSource> DistinctBy<TSource, TKey>
        //    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //    var known = new HashSet<TKey>();
        //    return source.Where(element => known.Add(keySelector(element)));
        //}
    }
}
