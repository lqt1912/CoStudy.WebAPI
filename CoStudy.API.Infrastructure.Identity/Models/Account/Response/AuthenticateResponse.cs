﻿using System;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Identity.Models.Account.Response
{
    public class AuthenticateResponse
    {

        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsVerified { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
