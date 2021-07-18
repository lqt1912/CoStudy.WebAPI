using CoStudy.API.Application.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CoStudy.API.Application.Utitlities
{
    public static class StringUtils
    {
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

        public static string RemoveVnChars(this string str)
        {
            if (str == null)
                return string.Empty;
            //Thay thế và lọc dấu từng char      
            for (int i = 1; i < vnChars.Length; i++)
            {
                for (int j = 0; j < vnChars[i].Length; j++)
                    str = str.Replace(vnChars[i][j], vnChars[0][i - 1]);
            }
            return str;
        }

        public static string RemoveExtraWhiteSpace(this string str)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            str = regex.Replace(str, " ");
            return str;
        }

        public static string TrimExToLower(this string s)
        {
            return Regex.Replace(s, " ", "").ToLower();
        }

        public static string NormalizeSearch(this string s)
        {
            return TrimExToLower((s));
        }

        public static bool ValidateAllowString(IViolenceWordRepository violenceWordRepository, string inputString)
        {
            var unAllowStrings = violenceWordRepository.GetAll().Select(x => x.Value);
            if (unAllowStrings == null)
                return true;
            foreach (var unallowString in unAllowStrings.ToList())
            {
                if (NormalizeSearch(inputString).Contains(NormalizeSearch(unallowString)))
                    return false;
            }
            return true;
        }

        public static bool IsMatchSearch(string query, string source)
        {
            query = RemoveExtraWhiteSpace(query).ToLower();
            var _queries = query.Split(' ');
            var queries = _queries.ToList();
            queries.ForEach(x => {
                x = RemoveVnChars(x);
            });

            var sources = source.ToLower().Split(' ').ToList();
            foreach (var item in sources)
            {
                var _x = RemoveVnChars(item);
                if (queries.Contains(_x))
                    return true;
            }
            return false;
        }
    }
}
