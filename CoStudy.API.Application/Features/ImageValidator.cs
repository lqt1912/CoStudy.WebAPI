using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CoStudy.API.Application.Features
{
    public static class ImageValidator
    {
        static List<Tuple<string, string>> _allowImageType;

        static List<Tuple<string, List<string>>> _bitComparer;

        public static bool IsImage(Stream stream, List<string> comparer)
        {
            stream.Seek(0, SeekOrigin.Begin);
            foreach (string c in comparer)
            {
                string bit = stream.ReadByte().ToString("X2");
                if (0 != string.Compare(bit, c))
                    return false;
            }
            return true;
        }

        public static bool IsImage(Stream stream, out string type, IConfiguration configuration)
        {
            _allowImageType = new List<Tuple<string, string>>();
            string[] listImageTypeString = configuration.GetSection("FileSettings:AllowedTypes").Get<string[]>();
            string[] listAllowedTypeBit = configuration.GetSection("FileSettings:AllowedTypeBits").Get<string[]>();
            int i = 0;
            try
            {
                //Duyệt hết list image type hỗ trợ
                foreach (string imageType in listImageTypeString)
                {
                    if (!String.IsNullOrEmpty(imageType))
                    {
                        //Add giá trị first bit tương ứng vào 
                        Tuple<string, string> tuple = new Tuple<string, string>(imageType, listAllowedTypeBit[i]);
                        _allowImageType.Add(tuple);
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                // Nếu có vấn đề phát sinh 
                throw new Exception("Lỗi bất định");
            }

            _bitComparer = new List<Tuple<string, List<string>>>();
            try
            {
                //Duyệt hết định dạng mà appsetting có 
                foreach (string t in listImageTypeString)
                {
                    string[] bitValue = configuration.GetSection($"FileSettings:TypeBitComparers:{t}").Get<string[]>();

                    //Lấy danh sách bit để so sánh tương tướng
                    List<string> listBitComparer = bitValue.Where(x => x != String.Empty).ToList();

                    //Add vô dưới dạng tuple
                    _bitComparer.Add(new Tuple<string, List<string>>(t, listBitComparer));
                }

            }
            catch (Exception)
            {
                throw new Exception("Lỗi");
            }


            _allowImageType.Add(new Tuple<string, string>("NONE", "N/A"));

            type = "none";
            stream.Seek(0, SeekOrigin.Begin);
            //hexa bit đầu tiên
            string bit = stream.ReadByte().ToString("X2");
            foreach (Tuple<string, string> item in _allowImageType)
            {
                //Khớp bit đầu tính tiếp 
                if (item.Item2 == bit)
                {
                    foreach (Tuple<string, List<string>> item2 in _bitComparer)
                    {
                        //Khớp loại file => Lấy danh sách bit ra so sánh 
                        if (item2.Item1 == item.Item1)
                        {
                            if (IsImage(stream, item2.Item2))
                            {
                                type = item.Item1;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static string IsImageAndType(Stream stream, out string type, IConfiguration configuration)
        {
            _allowImageType = new List<Tuple<string, string>>();
            string[] listImageTypeString = configuration.GetSection("FileSettings:AllowedTypes").Get<string[]>();
            string[] listAllowedTypeBit = configuration.GetSection("FileSettings:AllowedTypeBits").Get<string[]>();
            int i = 0;
            try
            {
                //Duyệt hết list image type hỗ trợ
                foreach (string imageType in listImageTypeString)
                {
                    if (!String.IsNullOrEmpty(imageType))
                    {
                        //Add giá trị first bit tương ứng vào 
                        Tuple<string, string> tuple = new Tuple<string, string>(imageType, listAllowedTypeBit[i]);
                        _allowImageType.Add(tuple);
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                // Nếu có vấn đề phát sinh 
                throw new Exception("Lỗi");
            }

            _bitComparer = new List<Tuple<string, List<string>>>();
            try
            {
                //Duyệt hết định dạng mà appsetting có 
                foreach (string t in listImageTypeString)
                {
                    string[] bitValue = configuration.GetSection($"FileSettings:TypeBitComparers:{t}").Get<string[]>();

                    //Lấy danh sách bit để so sánh tương tướng
                    List<string> listBitComparer = bitValue.Where(x => x != String.Empty).ToList();

                    //Add vô dưới dạng tuple
                    _bitComparer.Add(new Tuple<string, List<string>>(t, listBitComparer));
                }

            }
            catch (Exception)
            {
                throw new Exception("Lỗi");
            }
            _allowImageType.Add(new Tuple<string, string>("NONE", "N/A"));
            type = "none";
            stream.Seek(0, SeekOrigin.Begin);
            //hexa bit đầu tiên
            string bit = stream.ReadByte().ToString("X2");
            foreach (Tuple<string, string> item in _allowImageType)
            {
                //Khớp bit đầu tính tiếp 
                if (item.Item2 == bit)
                {
                    foreach (Tuple<string, List<string>> item2 in _bitComparer)
                    {
                        //Khớp loại file => Lấy danh sách bit ra so sánh 
                        if (item2.Item1 == item.Item1)
                        {
                            if (IsImage(stream, item2.Item2))
                            {
                                type = item.Item1;
                                //return type; ;
                            }
                        }
                    }
                }
            }
            return type;
        }
    }
}
