using TaskTracker.Core.Common;

namespace TaskTracker.Core.Entities
{
    public class Menu : BaseEntity<int>
    {
        public string? Name { get; set; } = default!;
        public string? Url { get; set; } = default!;
        public string? Module { get; set; }
        public string? Controller { get; set; }

        public int? ParentId { get; set; }
        public int? SubParentId { get; set; }
        public int? SubChildId { get; set; }

        public int? DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; }
        public string? IconClass { get; set; }
        public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();



    }
}
