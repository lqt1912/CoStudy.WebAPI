using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CoStudy.API.Application.Utitlities
{
    /// <summary>
    /// String Utils 
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// The vn chars
        /// </summary>
        private static readonly string[] vnChars = new string[]
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        /// <summary>
        /// Removes the vn chars.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string RemoveVnChars(this string str)
        {
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < vnChars.Length; i++)
            {
                for (int j = 0; j < vnChars[i].Length; j++)
                    str = str.Replace(vnChars[i][j], vnChars[0][i - 1]);
            }
            return str;
        }

        /// <summary>
        /// Trims the ex to lower.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string TrimExToLower(this string s)
        {
            return Regex.Replace(s, " ", "").ToLower();
        }

        /// <summary>
        /// Normalizes the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string NormalizeSearch(this string s)
        {
            return TrimExToLower(RemoveVnChars(s));
        }

    }
}
