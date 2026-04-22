using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Features.TaskOperation.ViewModels;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Core.Entities;

namespace TaskTracker.Application.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskItem, int>
    {
        Task<GridEntity<TaskItemModel>> GridDataAsync(GridOptions options, CancellationToken cancellationToken);
        Task<TaskItemModel?> GetAsync(int id, CancellationToken cancellationToken);
    }
}
