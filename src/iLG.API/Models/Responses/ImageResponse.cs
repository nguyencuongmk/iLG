﻿using Newtonsoft.Json;

namespace iLG.API.Models.Responses
{
    public class ImageResponse
    {
        [JsonProperty("Path")]
        public string? Path { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}