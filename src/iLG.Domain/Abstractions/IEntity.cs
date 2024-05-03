namespace iLG.Domain.Abstractions
{
    public interface IEntity<T> : IEntity
    {
        public T Id { get; set; }
    }

    public interface IEntity
    {
        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}