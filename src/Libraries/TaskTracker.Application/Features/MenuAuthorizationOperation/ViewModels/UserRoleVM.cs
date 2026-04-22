using AutoMapper;
using System.ComponentModel.DataAnnotations;
using TaskTracker.Core.Entities;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels
{
    public class UserRoleVM : IMapFrom<Role>
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public Guid? IdentityRoleId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, UserRoleVM>()
                   .ReverseMap();
        }
    }
}
