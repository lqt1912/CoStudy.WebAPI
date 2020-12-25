using CoStudy.API.Domain.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Domain.Entities.Application
{
    public class Logging : Entity
    {
        public string RequestMethod { get; set; }
        public string Location { get; set; }
        public string RequestPath { get; set; }
        public int StatusCode { get; set; }
        public double TimeElapsed { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
