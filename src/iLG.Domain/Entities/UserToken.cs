using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class UserToken : Entity<int>
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public string Platform { get; set; }

        public string MachineName { get; set; }

        public DateTime ExpiredTime { get; set; }

        public virtual User User { get; set; }
    }
}