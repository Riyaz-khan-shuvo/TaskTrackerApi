namespace TaskTracker.Core.Common
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
        DateTimeOffset? CreatedOn { get; set; }
        DateTimeOffset? LastModifiedOn { get; set; }
    }

}
