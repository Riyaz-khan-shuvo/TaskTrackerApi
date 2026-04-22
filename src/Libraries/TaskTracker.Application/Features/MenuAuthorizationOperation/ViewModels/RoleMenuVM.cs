using AutoMapper;
using TaskTracker.Core.Entities;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels
{
    public class RoleMenuVM : IMapFrom<RoleMenu>
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int MenuId { get; set; }
        public bool List { get; set; }
        public bool Insert { get; set; }
        public bool Delete { get; set; }
        public bool Post { get; set; }
        public bool IsChecked { get; set; }
        public int? ParentId { get; set; }
        public int? SubParentId { get; set; }
        public string? RoleName { get; set; }
        public string? MenuName { get; set; }

        public List<RoleMenuVM> RoleMenuList { get; set; }
        public RoleMenuVM()
        {
            RoleMenuList = new List<RoleMenuVM>();

        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoleMenuVM, RoleMenu>()
        .ForMember(dest => dest.Id, opt => opt.Ignore()) // ignore PK
        .ForMember(dest => dest.MenuId, opt => opt.Ignore()); // let us set manually

        }
    }
}
