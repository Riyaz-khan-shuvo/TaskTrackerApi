using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;

namespace TaskTracker.Application.Interfaces.Repositories.Common
{
    public interface ICommonRepository
    {
        Task<List<UserMenuVM>> GetAssignedMenuListAsync(Guid userId);
    }
}
