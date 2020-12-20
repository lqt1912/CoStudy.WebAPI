using CoStudy.API.Application.Features;
using CoStudy.API.Application.Repositories;
using CoStudy.API.Domain.Entities.Application;
using CoStudy.API.Infrastructure.Identity.Repositories.AccountRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoStudy.API.WebAPI.SignalR
{
    public class SignalRHub<T> : Hub<IHubClient<T>> where T : class
    {
        public SignalRHub(IClientConnectionsRepository clientConnectionsRepository, IClientGroupRepository clientGroupRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IAccountRepository accountRepository)
        {
            this.clientConnectionsRepository = clientConnectionsRepository;
            this.clientGroupRepository = clientGroupRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.accountRepository = accountRepository;
        }

        public IClientConnectionsRepository clientConnectionsRepository { get; }

        public IClientGroupRepository clientGroupRepository { get; }
        public IHttpContextAccessor httpContextAccessor { get; }
        public IUserRepository userRepository { get; }
        public IAccountRepository accountRepository { get; }

        public override Task OnConnectedAsync()
        {
            var currentUser = CurrentUser(httpContextAccessor, userRepository, accountRepository);
            if (currentUser != null)
            {
                var clientConnection = clientConnectionsRepository.GetById(ObjectId.Parse(currentUser.ClientConnectionsId));
                clientConnection.ClientConnection.Add(Context.ConnectionId);
                clientConnectionsRepository.Update(clientConnection, clientConnection.Id);
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var currentUser = CurrentUser(httpContextAccessor, userRepository, accountRepository);
            if (currentUser != null)
            {
                var clientConnection = clientConnectionsRepository.GetById(ObjectId.Parse(currentUser.ClientConnectionsId));
                clientConnection.ClientConnection.Remove(Context.ConnectionId);
                clientConnectionsRepository.Update(clientConnection, clientConnection.Id);
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNofti(T msg)
        {
            await Clients.All.SendNofti(msg);
        }
        public static User CurrentUser(IHttpContextAccessor _httpContextAccessor, IUserRepository userRepository, IAccountRepository accountRepository)
        {
            try
            {
                var token = _httpContextAccessor.HttpContext.Request.Query["access_token"].ToString();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("SuperSecretPassword@@@");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type == "_id").Value;
                var currentAccount = accountRepository.GetById(ObjectId.Parse(accountId));
                var user = userRepository.GetAll().SingleOrDefault(x => x.Email == currentAccount.Email);
                return user;
            }
            catch (Exception)
            {
                throw new Exception("No authen user");
            }

        }

    }
}
