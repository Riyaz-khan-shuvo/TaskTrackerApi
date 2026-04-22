using AutoMapper;
using MediatR;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.UnitOfWorkContracts;
using TaskTracker.Core.Entities;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Command
{
    public class UpsertRoleMenuCommand : RoleMenuVM, IRequest<ResultVM>
    {

        public class Handler : IRequestHandler<UpsertRoleMenuCommand, ResultVM>
        {
            private readonly ITaskTrackerUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(ITaskTrackerUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<ResultVM> Handle(UpsertRoleMenuCommand request, CancellationToken cancellationToken)
            {
                await _unitOfWork.MenuAuthorizationRepository.DeleteRoleMenusByRoleId(request.RoleId ?? 0, cancellationToken);

                var checkedMenus = request.RoleMenuList.Where(m => m.IsChecked).ToList();

                foreach (var menu in checkedMenus)
                {
                    var entity = _mapper.Map<RoleMenu>(menu);

                    entity.RoleId = request.RoleId ?? 0;
                    entity.MenuId = menu.MenuId;
                    await _unitOfWork.MenuAuthorizationRepository.InsertRoleMenu(entity, cancellationToken);
                }


                await _unitOfWork.SaveAsync(cancellationToken);

                return new ResultVM
                {
                    Status = "Success",
                    Message = "Role menu updated successfully.",
                    Count = checkedMenus.Count,
                    Data = checkedMenus
                };
            }
        }
    }
}
