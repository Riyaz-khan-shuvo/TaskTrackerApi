namespace TaskTracker.Core.Common
{
    public abstract class BaseEntity<T> : IEntityBase<T>, IAuditable
    where T : IComparable
    {
        public T Id { get; set; }

        public DateTimeOffset? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedFrom { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public string? LastUpdateFrom { get; set; }
    }


}
