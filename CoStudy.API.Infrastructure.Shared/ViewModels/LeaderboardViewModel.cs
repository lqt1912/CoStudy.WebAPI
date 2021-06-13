using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class LeaderBoardViewModel
    {
        [JsonPropertyName("current_user")]
        [JsonProperty("current_user")]
        public CurrentUserLeaderBoardViewModel CurrentUser { get; set; }

        [JsonProperty("leaderboard")]
        [JsonPropertyName("leaderboard")]
        public List<UserLeaderBoardViewModel> LeaderBoards { get; set; }

        public LeaderBoardViewModel()
        {
            LeaderBoards = new List<UserLeaderBoardViewModel>();
        }
    }

    public class UserLeaderBoardViewModel
    {
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_avatar")]
        [JsonPropertyName("user_avatar")]
        public string UserAvatar { get; set; }

        [JsonProperty("total_point")]
        [JsonPropertyName("total_point")]
        public int? TotalPoint { get; set; }

    }

    public class CurrentUserLeaderBoardViewModel
    {
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        [JsonProperty("user_avatar")]
        [JsonPropertyName("user_avatar")]
        public string UserAvatar { get; set; }

        [JsonProperty("total_point")]
        [JsonPropertyName("total_point")]
        public int? TotalPoint { get; set; }
    }
}
