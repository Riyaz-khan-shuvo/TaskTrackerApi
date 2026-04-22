using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Interfaces.Repositories;

namespace TaskTracker.Application.Features.TaskOperation.Queries
{
    public class GetTaskDetailQuery : IRequest<ResultVM>
    {
        public int Id { get; set; }

        public class Handler : IRequestHandler<GetTaskDetailQuery, ResultVM>
        {
            private readonly ITaskRepository _taskRepository;

            public Handler(ITaskRepository taskRepository)
            {
                _taskRepository = taskRepository;
            }

            public async Task<ResultVM> Handle(GetTaskDetailQuery request, CancellationToken cancellationToken)
            {
                var data = await _taskRepository.GetAsync(request.Id, cancellationToken);

                if (data == null)
                {
                    return new ResultVM
                    {
                        Status = "Error",
                        Message = "Task not found.",
                        Data = null
                    };
                }

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Data retrieved successfully.",
                    Data = data
                };
            }
        }
    }
}