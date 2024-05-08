
namespace iLG.Domain.Abstractions
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; } = "system";

        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

    public abstract class Entity : IEntity
    {
        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; } = "system";

        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}