using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class HobbyDetail : Entity<int>
    {
        public string? Name { get; set; }

        public int HobbyId { get; set; }

        public Hobby Hobby { get; set; }
    }
}