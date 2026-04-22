using AutoMapper;
using MediatR;
using Scholify.Application.Features.Teachers.TeacherOperation.ViewModels;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.UnitOfWorkContracts;
using TaskTracker.Core.Entities;

namespace TaskTracker.Application.Features.TaskOperation.Commands
{
    public class UpsertTaskCommand : TaskItemModel, IRequest<ResultVM>
    {
        public class Handler : IRequestHandler<UpsertTaskCommand, ResultVM>
        {
            private readonly ITaskTrackerUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(ITaskTrackerUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ResultVM> Handle(UpsertTaskCommand request, CancellationToken cancellationToken)
            {
                TaskItem entity = _mapper.Map<TaskItem>(request);

                if (request.Id == 0)
                {
                    entity = await _unitOfWork.TaskRepository.InsertAsync(entity, cancellationToken);
                }
                else
                {
                    entity = await _unitOfWork.TaskRepository.UpdateAsync(entity, cancellationToken);
                }

                await _unitOfWork.SaveAsync(cancellationToken);

                var resultDto = _mapper.Map<TaskItemModel>(entity);

                return new ResultVM
                {
                    Status = "Success",
                    Message = request.Id == 0
                        ? "Task created successfully."
                        : "Task updated successfully.",
                    Id = resultDto.Id.ToString(),
                    Data = resultDto
                };
            }
        }
    }
}