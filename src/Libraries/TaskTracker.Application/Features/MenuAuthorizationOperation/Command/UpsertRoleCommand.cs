using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskTracker.Application.DTOs.Common;
using TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels;
using TaskTracker.Application.UnitOfWorkContracts;
using TaskTracker.Core.Entities;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.Command
{
    public class UpsertRoleCommand : UserRoleVM, IRequest<ResultVM>
    {
        public class Handler : IRequestHandler<UpsertRoleCommand, ResultVM>
        {
            private readonly ITaskTrackerUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly RoleManager<ApplicationRole> _roleManager;


            public Handler(ITaskTrackerUnitOfWork unitOfWork, IMapper mapper, RoleManager<ApplicationRole> roleManager)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _roleManager = roleManager;
            }
            public async Task<ResultVM> Handle(UpsertRoleCommand request, CancellationToken cancellationToken)
            {
                Role entity;

                if (request.Id == 0)
                {
                    var identityRole = new ApplicationRole
                    {
                        Name = request.Name,
                        NormalizedName = request.Name.ToUpper()
                    };

                    var createRes = await _roleManager.CreateAsync(identityRole);
                    if (!createRes.Succeeded)
                    {
                        return new ResultVM
                        {
                            Status = "Fail",
                            Message = string.Join(", ", createRes.Errors.Select(e => e.Description))
                        };
                    }

                    entity = _mapper.Map<Role>(request);
                    entity.IdentityRoleId = identityRole.Id;

                    await _unitOfWork.MenuAuthorizationRepository.InsertAsync(entity, cancellationToken);
                }
                else
                {
                    entity = await _unitOfWork.MenuAuthorizationRepository.GetByIdAsync(request.Id);

                    if (entity == null)
                    {
                        return new ResultVM
                        {
                            Status = "Fail",
                            Message = "Role not found"
                        };
                    }
                    var identityRole = await _roleManager.FindByIdAsync(entity.IdentityRoleId.ToString());

                    if (identityRole != null)
                    {
                        identityRole.Name = request.Name;
                        identityRole.NormalizedName = request.Name.ToUpper();

                        var updateRes = await _roleManager.UpdateAsync(identityRole);
                        if (!updateRes.Succeeded)
                        {
                            return new ResultVM
                            {
                                Status = "Fail",
                                Message = string.Join(", ", updateRes.Errors.Select(e => e.Description))
                            };
                        }
                    }
                    entity.Name = request.Name;

                    await _unitOfWork.MenuAuthorizationRepository.UpdateAsync(entity, cancellationToken);
                }

                await _unitOfWork.SaveAsync(cancellationToken);

                return new ResultVM
                {
                    Status = "Success",
                    Id = entity.Id.ToString(),
                    Message = request.Id == 0 ? "Role created successfully." : "Role updated successfully."
                };
            }


        }

    }
}
