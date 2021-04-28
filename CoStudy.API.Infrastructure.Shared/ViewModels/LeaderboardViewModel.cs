using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    /// <summary>
    /// Class LeaderBoardViewModel
    /// </summary>
    public class LeaderBoardViewModel
    {
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        [JsonPropertyName("current_user")]
        [JsonProperty("current_user")]
        public CurrentUserLeaderBoardViewModel CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the leader boards.
        /// </summary>
        /// <value>
        /// The leader boards.
        /// </value>
        [JsonProperty("leaderboard")]
        [JsonPropertyName("leaderboard")]
        public  List<UserLeaderBoardViewModel> LeaderBoards { get; set; }

        public LeaderBoardViewModel()
        {
            LeaderBoards = new List<UserLeaderBoardViewModel>();
        }
    }

    /// <summary>
    /// Class UserLeaderBoardViewModel
    /// </summary>
    public class UserLeaderBoardViewModel
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [JsonProperty("user_name")]
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user avatar.
        /// </summary>
        /// <value>
        /// The user avatar.
        /// </value>
        [JsonProperty("user_avatar")]
        [JsonPropertyName("user_avatar")]
        public string  UserAvatar { get; set; }

        /// <summary>
        /// Gets or sets the total point.
        /// </summary>
        /// <value>
        /// The total point.
        /// </value>
        [JsonProperty("total_point")]
        [JsonPropertyName("total_point")]
        public int? TotalPoint { get; set; }

    }

    /// <summary>
    /// Class CurrentUserLeaderBoardViewModel
    /// </summary>
    public class CurrentUserLeaderBoardViewModel
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        [JsonProperty("index")]
        [JsonPropertyName("index")]
        public int? Index { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [JsonProperty("user_id")]
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [JsonProperty("user_name")]
        [JsonPropertyName("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user avatar.
        /// </summary>
        /// <value>
        /// The user avatar.
        /// </value>
        [JsonProperty("user_avatar")]
        [JsonPropertyName("user_avatar")]
        public string UserAvatar { get; set; }

        /// <summary>
        /// Gets or sets the total point.
        /// </summary>
        /// <value>
        /// The total point.
        /// </value>
        [JsonProperty("total_point")]
        [JsonPropertyName("total_point")]
        public int? TotalPoint { get; set; }
    }
}
