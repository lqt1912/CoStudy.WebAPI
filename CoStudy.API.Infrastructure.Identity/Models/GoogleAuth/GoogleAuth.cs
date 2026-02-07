using System;
using System.Collections.Generic;
using System.Text;

namespace CoStudy.API.Infrastructure.Identity.Models.GoogleAuth
{
    public class GoogleAuth
    {
        public string  Provider { get; set; }

        public string  IdToken { get; set; }
    }
}
