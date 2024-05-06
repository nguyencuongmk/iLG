﻿namespace iLG.API.Models.Responses
{
    public class HobbyResponse
    {
        public string Title { get; set; }

        public List<HobbyDetailResponse> HobbyDetails { get; set; }

        public class HobbyDetailResponse
        {
            public string? Name { get; set; }

            public int HobbyId { get; set; }
        }
    }
}