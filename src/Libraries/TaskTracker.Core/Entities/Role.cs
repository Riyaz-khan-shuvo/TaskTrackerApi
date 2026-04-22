using TaskTracker.Core.Common;

namespace TaskTracker.Core.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }
        public Guid? IdentityRoleId { get; set; }
    }
}
