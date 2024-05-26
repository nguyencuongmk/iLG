using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace iLG.API.Models.Requests
{
    public class UserSuitableRequest
    {
        public int MinAge { get; set; }

        public int MaxAge { get; set; }

        public string Gender { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}
