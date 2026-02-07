using CoStudy.API.Domain.Entities.BaseEntity;
using System;

namespace CoStudy.API.Domain.Entities.Identity.MongoAuthen
{
    public class RefreshToken : Entity
    {
        public Account Account { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

    }

    public enum Role
    {
        Admin,
        User
    }

}
