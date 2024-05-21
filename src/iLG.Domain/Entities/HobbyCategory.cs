using iLG.Domain.Abstractions;

namespace iLG.Domain.Entities
{
    public class HobbyCategory : Entity<int>
    {
        public string Title { get; set; }

        public virtual List<Hobby> Hobbies { get; set; } = [];
    }
}