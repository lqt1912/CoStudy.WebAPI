using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class UserViewModel
    {
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }
        [JsonProperty("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("first_name")]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("full_name")]
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonProperty("date_of_birth")]
        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }


        [JsonProperty("address")]
        [JsonPropertyName("address")]
        public Address Address { get; set; }

        [JsonProperty("full_address")]
        [JsonPropertyName("full_address")]
        public string FullAddress { get; set; }

        [JsonProperty("avatar")]
        [JsonPropertyName("avatar")]
        public Image Avatar { get; set; }

        [JsonProperty("avatar_hash")]
        [JsonPropertyName("avatar_hash")]
        public string AvatarHash { get; set; }

        private ItemStatus _status;

        public ItemStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                switch (_status)
                {
                    case ItemStatus.Active:
                        StatusName = "Active";
                        break;
                    case ItemStatus.Offline:
                        StatusName = "Offline";
                        break;
                    case ItemStatus.Blocked:
                        StatusName = "Blocked";
                        break;
                    case ItemStatus.Await:
                        StatusName = "Await";
                        break;
                    case ItemStatus.Deleted:
                        StatusName = "Deleted";
                        break;
                    default:
                        break;
                }
            }
        }


        [JsonProperty("refresh_token")]
        [JsonPropertyName("refresh_token")]
        public string LatestRefreshToken { get; set; }

        [JsonProperty("status_name")]
        [JsonPropertyName("status_name")]
        public string StatusName { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("modified_date")]
        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("post_count")]
        [JsonPropertyName("post_count")]
        public int PostCount { get; set; }

        [JsonProperty("followers")]
        [JsonPropertyName("followers")]
        public int Followers { get; set; }

        [JsonProperty("followings")]
        [JsonPropertyName("followings")]
        public int Following { get; set; }

        [JsonProperty("additional_infos")]
        [JsonPropertyName("additional_infos")]
        public List<AdditionalInfomation> AdditionalInfos { get; set; }

        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public List<object> Fields { get; set; } = new List<object>();

        [JsonProperty("post_saved")]
        [JsonPropertyName("post_saved")]
        public List<string> PostSaved { get; set; }

        [JsonProperty("call_id")]
        [JsonPropertyName("call_id")]
        public string CallId { get; set; }
    }
}
