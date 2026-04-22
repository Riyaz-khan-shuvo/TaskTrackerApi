namespace TaskTracker.Core.Common
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedOn { get; set; }
        string? CreatedBy { get; set; }
        DateTimeOffset? LastModifiedOn { get; set; }
        string? LastModifiedBy { get; set; }
    }
}
