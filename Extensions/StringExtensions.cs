﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    private static Dictionary<string, string> _htmlReplacementCharacters;

    public static T ToEnum<T>(this string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return (T)Enum.Parse(typeof(T), "Undefined", true);
        }

        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static string ToStringOrEmpty(this object value)
    {
        if (value == null)
        {
            return String.Empty;
        }
        else
        {
            return value.ToString();
        }
    }

    public static List<T> ToList<T>(this string value, string[] delimiters = null)
    {
        // Check value.
        if (String.IsNullOrEmpty(value))
        {
            // Return null.
            return null;
        }

        // Check delimiters.
        if (delimiters == null || delimiters.Length == 0)
        {
            delimiters = new string[] { "," };
        }

        // Split string.
        string[] splitArray = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        // Check array.
        if (splitArray == null || splitArray.Length == 0)
        {
            return null;
        }

        List<T> results = new List<T>();

        foreach (string text in splitArray)
        {
            results.Add((T)Convert.ChangeType(text, typeof(T)));
        }

        return results;
    }

    public static List<int> ToSplitIntegerList(this string value, string[] delimiters = null)
    {
        // Check value.
        if (String.IsNullOrEmpty(value))
        {
            // Return null.
            return null;
        }

        // Check delimiters.
        if (delimiters == null || delimiters.Length == 0)
        {
            delimiters = new string[] { "," };
        }

        // Split string.
        string[] splitArray = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        // Check array.
        if (splitArray == null || splitArray.Length == 0)
        {
            return null;
        }

        // Initialize a result list to return.
        List<int> resultList = new List<int>();

        // Loop through array.
        for (int i = 0; i < splitArray.Length; i++)
        {
            // Initialize a temporary integer.
            int temp = 0;

            // Try to parse the value.
            if (Int32.TryParse(splitArray[i], out temp))
            {
                // Add parsed value to result list.
                resultList.Add(temp);
            }
        }

        // Return result list.
        return resultList;
    }

    public static List<string> ToSplitStringList(this string value, string[] delimiters = null)
    {
        // Check value.
        if (String.IsNullOrEmpty(value))
        {
            // Return null.
            return null;
        }

        // Check delimiters.
        if (delimiters == null || delimiters.Length == 0)
        {
            delimiters = new string[] { "," };
        }

        // Split string.
        string[] splitArray = value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

        // Check array.
        if (splitArray == null || splitArray.Length == 0)
        {
            return null;
        }

        // Return split list.
        return splitArray.ToList();
    }

    public static bool ToBool(this string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return false;
        }

        if (value.ToLower() == "true")
        {
            return true;
        }

        if (value.ToLower() == "a")
        {
            return true;
        }

        if (value.ToLower() == "y")
        {
            return true;
        }

        if (value.ToLower() == "1")
        {
            return true;
        }

        return false;
    }

    public static long ToLong(this string value, long defaultValue = 0)
    {
        long result = 0;

        if (Int64.TryParse(value, out result))
        {
            return result;
        }

        return defaultValue;
    }

    public static int ToInt(this string value, int defaultValue = 0)
    {
        int result = 0;

        if (Int32.TryParse(value, out result))
        {
            return result;
        }

        return defaultValue;
    }

    public static string Truncate(this string value, int length = 25)
    {
        if (String.IsNullOrEmpty(value))
        {
            return String.Empty;
        }

        if (value.Length <= length)
        {
            return value;
        }

        return String.Join("", value.Substring(0, length - 3), "...");
    }

    /// <summary>
    /// Get seo title
    /// </summary>
    /// <param name="title"> The title. </param>
    /// <returns> seo title </returns>
    public static string ToSlug(this string title, List<string> ignoredCharacters = null)
    {
        // Check input title.
        if (String.IsNullOrEmpty(title))
        {
            return "tanimsiz";
        }

        // Check if developer defined any ignored chars for this invocation.
        if (ignoredCharacters == null)
        {
            // Create a new list if it's empty.
            ignoredCharacters = new List<string>();
        }

        // Check if we generated html spesific characters before.
        if (_htmlReplacementCharacters == null)
        {
            // Generate if it's empty.
            _htmlReplacementCharacters = GenerateHtmlReplacementCharacters();
        }

        // Change to lowercase and trim.
        title = title.ToLower().Trim();

        // Remove HTML tags by regex.
        title = Regex.Replace(title, "<.*?>", string.Empty);

        // Loop throught replacement list.
        foreach (KeyValuePair<string, string> pair in _htmlReplacementCharacters)
        {
            // Check if ignore list contains our character.
            if (!ignoredCharacters.Contains(pair.Key))
            {
                // Replace title.
                title = title.Replace(pair.Key, pair.Value);
            }
        }

        // Return title.
        return title;
    }

    #region Helpers

    /// <summary>
    /// This method generated default characters to be replaced in order to make input string SEO friendly.
    /// </summary>
    /// <returns>Returns a dictonary of strings with replaced values of keys.</returns>
    private static Dictionary<string, string> GenerateHtmlReplacementCharacters()
    {
        Dictionary<string, string> results = new Dictionary<string, string>();

        results.Add("ı", "i");
        results.Add("ş", "s");
        results.Add("ğ", "g");
        results.Add("ü", "u");
        results.Add("ç", "c");
        results.Add("ö", "o");
        results.Add(" ", "-");
        results.Add("?", String.Empty);
        results.Add(":", String.Empty);
        results.Add("/", String.Empty);
        results.Add("%", String.Empty);
        results.Add(".", "-");
        results.Add("+", "-");
        results.Add("\\", "-");
        results.Add("&", String.Empty);
        results.Add("'", String.Empty);
        results.Add(Char.ConvertFromUtf32(34), String.Empty);
        results.Add("#", String.Empty);
        results.Add("--", "-");
        results.Add("*", String.Empty);
        results.Add("<", String.Empty);
        results.Add(">", String.Empty);

        return results;
    }

    #endregion
}