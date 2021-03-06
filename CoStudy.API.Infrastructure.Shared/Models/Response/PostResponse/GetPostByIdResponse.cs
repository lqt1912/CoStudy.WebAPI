﻿using CoStudy.API.Domain.Entities.Application;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Response.PostResponse
{
    public class GetPostByIdResponse
    {


        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }

        [JsonPropertyName("upvote")]
        public int Upvote { get; set; }

        [JsonPropertyName("downvote")]
        public int Downvote { get; set; }

        [JsonPropertyName("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("modified_date")]
        public DateTime ModifiedDate { get; set; }

           [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }


           [JsonPropertyName("image_contents")]
        public List<Image> MediaContents { get; set; }

            [JsonPropertyName("comments")]
        public List<Comment> Comments { get; set; }

            [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }
    }
}
