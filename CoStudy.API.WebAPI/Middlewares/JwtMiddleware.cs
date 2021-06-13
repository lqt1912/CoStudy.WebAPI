using CoStudy.API.Infrastructure.Identity.Helpers;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using MongoDB.Driver;

namespace CoStudy.API.WebAPI.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly IUserRepository userRepository;



        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IUserRepository userRepository)
        {
            _next = next;
            this.userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IAccountRepository accountRepository)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await attachAccountToContext(context, token, accountRepository);
            }

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, string token, IAccountRepository accountRepository)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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
                string accountEmail = jwtToken.Claims.First(x => x.Type == "_email").Value;

                var user = await userRepository.FindAsync(Builders<User>.Filter.Eq("email", accountEmail));
                var isExist = user.JwtTokens.Any(x=>x==token);
                if (!isExist)
                    throw new ArgumentNullException();
                // attach account to context on successful jwt validation
                context.Items["Account"] = await accountRepository.GetByIdAsync(ObjectId.Parse(accountId));
            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}
