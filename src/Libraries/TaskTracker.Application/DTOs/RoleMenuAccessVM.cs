namespace TaskTracker.Application.DTOs
{
    public class RoleMenuAccessVM
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        public List<RoleMenuItemVM> Menus { get; set; } = new();
    }

    public class RoleMenuItemVM
    {
        public Guid MenuId { get; set; }
        public string MenuName { get; set; }

        public bool CanView { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
    }
}
