using CoStudy.API.Domain.Entities.Application;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.ViewModels
{
    public class PostViewModel : BaseViewModel
    {
        [JsonPropertyName("oid")]
        [JsonProperty("oid")]
        public string OId { get; set; }



        [JsonProperty("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }



        [JsonProperty("author_id")]
        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonProperty("author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }


        [JsonProperty("author_avatar")]
        [JsonPropertyName("author_avatar")]
        public string AuthorAvatar { get; set; }

        [JsonProperty("author_email")]
        [JsonPropertyName("author_email")]
        public string AuthorEmail { get; set; }

        [JsonProperty("upvote")]
        [JsonPropertyName("upvote")]
        public int Upvote { get; set; }

        [JsonProperty("downvote")]
        [JsonPropertyName("downvote")]
        public int Downvote { get; set; }

        [JsonProperty("created_date")]
        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("string_contents")]
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }

        [JsonProperty("image_contents")]
        [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

        [JsonPropertyName("media_type")]
        [JsonProperty("media_type")]
        public MediaType MediaType { get; set; }

        [JsonProperty("comment_count")]
        [JsonPropertyName("comments_count")]
        public int CommentCount { get; set; }


        [JsonProperty("field")]
        [JsonPropertyName("field")]
        public IEnumerable<object> Field { get; set; }

        [JsonProperty("is_vote_by_current")]
        [JsonPropertyName("is_vote_by_current")]
        public bool? IsVoteByCurrent { get; set; }

        [JsonProperty("is_downvote_by_current")]
        [JsonPropertyName("is_downvote_by_current")]
        public bool? IsDownVoteByCurrent { get; set; }

    }
}

