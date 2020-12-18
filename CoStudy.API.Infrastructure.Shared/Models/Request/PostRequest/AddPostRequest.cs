using CoStudy.API.Domain.Entities.Application;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoStudy.API.Infrastructure.Shared.Models.Request.PostRequest
{
    public class AddPostRequest
    {

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [FromForm(Name = "string_contents")]
        [JsonPropertyName("string_contents")]
        public List<PostContent> StringContents { get; set; }
    }
}
