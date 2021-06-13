namespace CoStudy.API.WebAPI.Models
{
    public class LoggingViewModel
    {

        public string oid { get; set; }
              public string RequestMethod { get; set; }
              public string Location { get; set; }
              public string RequestPath { get; set; }
              public int StatusCode { get; set; }
              public double TimeElapsed { get; set; }
              public string Message { get; set; }
              public string Ip { get; set; }
              public string CreatedDate { get; set; }

    }
}
