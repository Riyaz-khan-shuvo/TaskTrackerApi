using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.TaskOperation.Queries
{
    public class GetTaskGridQuery : GridOptions, IRequest<ResultVM>
    {
        public class Handler : IRequestHandler<GetTaskGridQuery, ResultVM>
        {
            private readonly ITaskRepository _taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<ResultVM> Handle(GetTaskGridQuery request, CancellationToken cancellationToken)
            {
                var gridData = await _taskRepository.GridDataAsync(request, cancellationToken);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Data retrieved successfully.",
                    Data = gridData,
                    Count = gridData.TotalCount
                };
            }
        }
    }
}