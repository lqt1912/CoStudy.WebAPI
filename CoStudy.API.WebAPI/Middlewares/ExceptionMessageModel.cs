using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.WebAPI.Middlewares
{
    public class ExceptionMessageModel
    {
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
