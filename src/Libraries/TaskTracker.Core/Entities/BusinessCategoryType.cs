namespace TaskTracker.Core.Entities
{
    public class BusinessCategoryType 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<BusinessCategory> BusinessCategories { get; set; }
    }
}
