using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class FieldGroupViewModel
    {
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }

        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonProperty("group_name")]
        [JsonPropertyName("group_name")]
        public string GroupName { get; set; }
        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }

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


        [JsonProperty("status_name")]
        [JsonPropertyName("status_name")]
        public string StatusName { get; set; }

    }
}
