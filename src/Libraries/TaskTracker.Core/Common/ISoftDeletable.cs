namespace TaskTracker.Core.Common
{
    public interface ISoftDeletable
    {
        bool IsActive { get; set; }
        bool IsArchive { get; set; }
    }
}
