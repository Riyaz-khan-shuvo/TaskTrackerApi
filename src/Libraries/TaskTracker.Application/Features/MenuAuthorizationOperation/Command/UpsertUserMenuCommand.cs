using AutoMapper;
using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.UnitOfWorkContracts;
using TaskTracker.Core.Entities;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Command
{
    public class UpsertUserMenuCommand : IRequest<ResultVM>
    {
        public string UserId { get; set; }
        public List<UserMenuVM> MenuList { get; set; }

        public class Handler : IRequestHandler<UpsertUserMenuCommand, ResultVM>
        {
            private readonly ITaskTrackerUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(ITaskTrackerUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }
            public async Task<ResultVM> Handle(UpsertUserMenuCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.MenuAuthorizationRepository.DeleteUserMenusByUserId(Guid.Parse(request.UserId), cancellationToken);

                var checkedMenus = request.MenuList?.ToList() ?? new List<UserMenuVM>();
                foreach (var menu in checkedMenus)
                {
                    var entity = _mapper.Map<UserMenu>(menu);
                    entity.UserId = Guid.Parse(request.UserId);
                    await _unitOfWork.MenuAuthorizationRepository.InsertUserMenu(entity, cancellationToken);
                }
                await _unitOfWork.SaveAsync(cancellationToken);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "User menu updated successfully.",
                    Count = checkedMenus.Count,
                    Data = checkedMenus
                };
            }
        }
    }
}
