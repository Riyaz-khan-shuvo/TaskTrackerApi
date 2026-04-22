namespace TaskTracker.Core.Entities
{
    public class BusinessCategory 
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int BusinessCategoryTypeId { get; set; }
        public bool IsActive { get; set; }
        public BusinessCategoryType BusinessCategoryType { get; set; }
    
    }
}
