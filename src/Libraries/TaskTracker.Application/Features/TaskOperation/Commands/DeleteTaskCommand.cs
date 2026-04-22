using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.UnitOfWorkContracts;

namespace TaskTracker.Application.Features.TaskOperation.Commands
{
    public class DeleteTaskCommand : IRequest<ResultVM>
    {
        public string[] IDs { get; set; } = Array.Empty<string>();

        public class Handler : IRequestHandler<DeleteTaskCommand, ResultVM>
        {
            private readonly ITaskTrackerUnitOfWork _unitOfWork;

            public Handler(ITaskTrackerUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<ResultVM> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
            {
                var intIds = request.IDs
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToList();

                var result = await _unitOfWork.TaskRepository.DeleteAsync(intIds, cancellationToken);

                if (result.Status == "Success")
                {
                    result.IDs = intIds.Select(x => x.ToString()).ToArray();
                    await _unitOfWork.SaveAsync(cancellationToken);
                }

                return result;
            }
        }
    }
}