using AutoMapper;
using TaskTracker.Core.Entities;
using TaskTracker.Shared.Contracts;

namespace TaskTracker.Application.Features.MenuAuthorizationOperation.ViewModels
{
    public class UserMenuVM : IMapFrom<UserMenu>
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string Url { get; set; }
        public string MenuName { get; set; }
        public string IconClass { get; set; }
        public string Controller { get; set; }
        public int ParentId { get; set; }
        public int SubParentId { get; set; }
        public int SubChildId { get; set; }
        public int DisplayOrder { get; set; }
        public int TotalChild { get; set; }
        public Guid UserId { get; set; }
        // Permission flags for middleware
        public bool List { get; set; }
        public bool Insert { get; set; }
        public bool Post { get; set; }
        public bool Delete { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserMenu, UserMenuVM>()
                   .ReverseMap();
        }
    }
}
