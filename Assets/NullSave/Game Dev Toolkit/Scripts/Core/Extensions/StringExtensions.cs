//
// Game Developers Toolkit © 2023 by Thomas W Holtquist is licensed under CC BY-SA 4.0 
// https://www.nullsave.com
//

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NullSave.GDTK
{
    [AutoDocLocation("extensions")]
    [AutoDoc("This class contains methods that extends string.")]
    public static class StringExtensions
    {

        #region Public Methods

        [AutoDoc("Check is string is a valid id")]
        [AutoDocParameter("Value to check")]
        public static bool IsAllowedId(this string value)
        {
            return Regex.IsMatch(value, "^[A-Za-z0-9_]*$");
        }

        [AutoDoc("Check if is a string is a numeric value")]
        [AutoDocParameter("Value to check")]
        public static bool IsNumeric(this string value)
        {
            return float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
        }

        [AutoDoc("Log a message")]
        [AutoDocParameter("Message source")]
        [AutoDocParameter("Method source")]
        [AutoDocParameter("Message")]
        public static void Log(string source, string method, string message)
        {
            Debug.Log(source + "." + method + ": " + message);
        }

        [AutoDoc("Log a message")]
        [AutoDocParameter("Message source")]
        [AutoDocParameter("Method source")]
        [AutoDocParameter("Message")]
        public static void Log(UnityEngine.Object source, string method, string message)
        {
            if (source != null)
            {
                Debug.Log(source.name + "." + method + ": " + message, source);
            }
            else
            {
                Debug.Log(method + ": " + message);
            }
        }

        [AutoDoc("Log an error")]
        [AutoDocParameter("Message source")]
        [AutoDocParameter("Method source")]
        [AutoDocParameter("Message")]
        public static void LogError(string source, string method, string message)
        {
            Debug.LogError(source + "." + method + ": " + message);
        }

        [AutoDoc("Log an error")]
        [AutoDocParameter("Message source")]
        [AutoDocParameter("Method source")]
        [AutoDocParameter("Message")]
        public static void LogError(UnityEngine.Object source, string method, string message)
        {
            if (source != null)
            {
                Debug.LogError(source.name + "." + method + ": " + message, source);
            }
            else
            {
                Debug.LogError(source + "." + method + ": " + message);
            }
        }

        [AutoDoc("Log a warning")]
        [AutoDocParameter("Message source")]
        [AutoDocParameter("Method source")]
        [AutoDocParameter("Message")]
        public static void LogWarning(string source, string method, string message)
        {
            Debug.LogWarning(source + "." + method + ": " + message);
        }

        [AutoDoc("Log a warning")]
        [AutoDocParameter("Message source")]
        [AutoDocParameter("Method source")]
        [AutoDocParameter("Message")]
        public static void LogWarning(UnityEngine.Object source, string method, string message)
        {
            if (source != null)
            {
                Debug.LogWarning(source.name + "." + method + ": " + message, source);
            }
            else
            {
                Debug.LogWarning(source + "." + method + ": " + message);
            }
        }

        /// <summary>
        /// Convert string to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [AutoDoc("Convert string to boolean")]
        [AutoDocParameter("Value to convert")]
        public static bool ToBool(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (value.ToLower() == "true") return true;
            if (value.ToLower() == "yes") return true;
            if (value.ToLower() == "on") return true;
            if (value.ToLower() == "checked") return true;
            if (value.ToLower() == "check") return true;

            if (int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int iVal))
            {
                return iVal != 0;
            }

            return false;
        }

        [AutoDoc("Convert string to camel case")]
        [AutoDocParameter("Value to convert")]
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            var words = str.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            var leadWord = Regex.Replace(words[0], @"([A-Z])([A-Z]+|[a-z0-9]+)($|[A-Z]\w*)",
                m =>
                {
                    return m.Groups[1].Value.ToLower() + m.Groups[2].Value.ToLower() + m.Groups[3].Value;
                });
            var tailWords = words.Skip(1)
                .Select(word => char.ToUpper(word[0]) + word.Substring(1))
                .ToArray();
            return $"{leadWord}{string.Join(string.Empty, tailWords)}";
        }

        [AutoDoc("Convert string to allowed id")]
        [AutoDocParameter("Value to convert")]
        public static string ToAllowedId(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return Regex.Replace(str, "[^A-Za-z0-9_]", " ", RegexOptions.Compiled).ToCamelCase();
        }

        #endregion

    }
}