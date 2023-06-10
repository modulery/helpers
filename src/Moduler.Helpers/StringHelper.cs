using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Moduler.Helpers
{
    public static class StringHelper
    {
        private static Regex digitsOnly = new Regex(@"[^\d]+\,");
        private static Regex currencyRegex = new Regex(@"((?<CurrencySymbol>[^\d\-\ +\.,]{1,3}) *(?<CurrencyValue>[0-9\.\,]+))|((?<CurrencyValue>[0-9\.\,]+) *(?<CurrencyCode>[A-Z]{1,3}))");
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string CleanWhitespace(this string input)
        {
            return sWhitespace.Replace(input, "");
        }

        public static int ConvertToInt(this string value)
        {
            value = value?.Trim().Replace(".", "");
            int result = 0;
            int.TryParse(value, out result);
            return result;
        }
        public static double GetDouble(string value)
        {
            value = value.Trim().Replace(".", "");
            //var aaa = digitsOnly.Replace(value, "");
            double result = 0.00;
            double.TryParse(value, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "," }, out result);
            return result;
        }
        public static string ClearStore(this string str)
        {
            if (str.Contains("//")) str = str.Split(".")[1];
            return str;
        }
        public static string ClearTitle(this string text)
        {
            text = Regex.Replace(text, @"[\u00AD]", "").Replace("&shy;", "");
            //if (text.StartsWith("#"))
            //{

            //}
            text = Regex.Replace(text, @"^(#[0-9]{5,10} :?)", "");
            text = text.Trim();
            return text;
        }
        public static int? NullableTryParseInt32(string text)
        {
            int value;
            return int.TryParse(text, out value) ? (int?)value : null;
        }
        //public static double DoubleParseAdvanced(this string strToParse, char decimalSymbol = ',')
        //{
        //    string tmp = Regex.Match(strToParse, @"([-]?[0-9]+)([\s])?([0-9]+)?[." + decimalSymbol + "]?([0-9 ]+)?([0-9]+)?").Value;

        //    if (tmp.Length > 0 && strToParse.Contains(tmp))
        //    {
        //        var currDecSeparator = ".";

        //        tmp = tmp.Replace(".", currDecSeparator).Replace(decimalSymbol.ToString(), currDecSeparator);

        //        return double.Parse(tmp);
        //    }

        //    return 0;
        //}
        public static string ToUrlSlug(this string value)
        {
            //First to lower case
            value = value.ToLower();
            value = value.Replace("ğ", "g").Replace("ş", "s").Replace("ı", "i")
                          .Replace("ü", "u").Replace("ö", "o").Replace("ç", "c");

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s_]", " ", RegexOptions.Compiled);
            //Replace spaces
            value = Regex.Replace(value, @"\s", "_", RegexOptions.Compiled);


            //Trim dashes from end
            value = value.Trim(' ', '-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
        public static List<MyCurrency> PriceToAmountAndCurrency(string value)
        {
            var result = new List<MyCurrency>();
            value = value.Replace("<sup>", "").Replace("</sup>", "");
            var matches = currencyRegex.Matches(value);

            foreach (Match match in matches)
            {
                var item = new MyCurrency { Value = match.Value };
                if (match.Groups["CurrencyValue"].Success)
                    item.Amount = GetDouble(match.Groups["CurrencyValue"].Value);
                if (match.Groups["CurrencyCode"].Success)
                    item.Code = match.Groups["CurrencyCode"].Value;
                if (match.Groups["CurrencySymbol"].Success)
                {
                    item.Symbol = match.Groups["CurrencySymbol"].Value;
                    if (string.IsNullOrEmpty(item.Code)) item.Code = item.Symbol;
                }

                result.Add(item);
            }
            return result;
        }
        public static int GetDeterministicHashCode(this string str)
        {
            try
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public static string RemoveSpecialCharacters(string str)
        {
            if (string.IsNullOrEmpty(str)) { return string.Empty; }
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }
        public class MyCurrency
        {
            public string Value { get; set; }
            public string Symbol { get; set; }
            public string Code { get; set; }
            public double Amount { get; set; }
        }
    }
}
