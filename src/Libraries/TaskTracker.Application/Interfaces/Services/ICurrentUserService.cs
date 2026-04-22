namespace TaskTracker.Application.Interfaces.Services
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Username { get; }
        string[] Roles { get; }
        bool IsAuthenticated { get; }
        string AuthenticationToken { get; }
    }
}
