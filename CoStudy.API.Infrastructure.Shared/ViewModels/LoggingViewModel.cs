using System;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class LoggingViewModel
    {
              public string OId { get; set; }

              public int Index { get; set; }
              public string RequestMethod { get; set; }
              public string Location { get; set; }
              public string RequestPath { get; set; }
              public int StatusCode { get; set; }
              public double TimeElapsed { get; set; }
              public string Message { get; set; }
              public string Ip { get; set; }
              public String CreatedDate { get; set; }
    }
}
