namespace TaskTracker.Core.Common
{
    public interface IEntityBase<T>
    {
        T Id { get; set; }
    }

}
