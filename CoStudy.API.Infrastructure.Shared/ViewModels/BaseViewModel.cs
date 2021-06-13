using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class BaseViewModel
    {

        public int? Index { get; set; }

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
