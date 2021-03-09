using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace CoStudy.API.Application.Features
{
    /// <summary>
    /// class JwtExtension
    /// </summary>
    public static class JwtExtension
    {
        /// <summary>
        /// Gets the identifier from JWT.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unauthorized</exception>
        public static string GetIdFromJwt(IHttpContextAccessor context, IConfiguration configuration)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(configuration["AppSettings:Secret"].ToString());
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                string accountId = jwtToken.Claims.First(x => x.Type == "_id").Value;
                return accountId;
            }
            else throw new Exception("Unauthorized");
        }
    }
}
