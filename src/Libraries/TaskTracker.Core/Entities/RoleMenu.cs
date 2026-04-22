using TaskTracker.Core.Common;

namespace TaskTracker.Core.Entities
{
    public class RoleMenu : BaseEntity<int>
    {
        public int RoleId { get; set; }     
        public int MenuId { get; set; }     

        public bool List { get; set; }
        public bool Insert { get; set; }
        public bool Delete { get; set; }
        public bool Post { get; set; }

        public Menu Menu { get; set; }
    }
}
