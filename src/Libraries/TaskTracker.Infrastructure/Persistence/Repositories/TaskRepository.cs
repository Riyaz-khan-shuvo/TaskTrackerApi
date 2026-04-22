using Microsoft.EntityFrameworkCore;
using Scholify.Application.Features.Teachers.TeacherOperation.ViewModels;
using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Interfaces.Repositories;
using TaskTracker.Application.Interfaces.Services.Common;
using TaskTracker.Core.Entities;
using TaskTracker.Infrastructure.Persistence.DataContext;
using TaskTracker.Infrastructure.Persistence.Repositories.Common;

namespace TaskTracker.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepository<TaskItem, int>, ITaskRepository
    {
        private readonly IGridService<TaskItemModel> _gridService;
        private readonly ApplicationDbContext _context;

        public TaskRepository(
            ApplicationDbContext context,
            IGridService<TaskItemModel> gridService)
            : base(context)
        {
            _context = context;
            _gridService = gridService;
        }

        public async Task<GridEntity<TaskItemModel>> GridDataAsync(
            GridOptions options,
            CancellationToken cancellationToken)
        {
            var query = _context.Tasks
                .AsNoTracking()
                .Select(x => new TaskItemModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsCompleted = x.IsCompleted,
                    DueDate = x.DueDate,
                    CreatedAt = x.CreatedAt
                });

            return await _gridService.GetEfGridDataAsync(query, options, cancellationToken);
        }

        public async Task<TaskItemModel?> GetAsync(
            int id,
            CancellationToken cancellationToken)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new TaskItemModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    IsCompleted = x.IsCompleted,
                    DueDate = x.DueDate,
                    CreatedAt = x.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}