using Foundatio.Queues;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DemoQueue
{
    public static class Program
    {
        public static readonly string[] vnChars = new string[]
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

        public static bool IsMatchSearch(string query, string source)
        {
            query = RemoveExtraWhiteSpace(query).ToLower();
            var _queries = query.Split(' ');
            var queries = _queries.ToList();
            queries.ForEach(x=>{
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
        static void Main(string[] args)
        {
            var query = " C#     bác";
            var source = "BAC    nào biết    C#    chỉ em với";
            var result = IsMatchSearch(query, source); //return true
            Console.WriteLine(result);
            Console.Read();
        }
    }
}
