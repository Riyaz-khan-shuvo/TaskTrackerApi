namespace TaskTracker.Application.Features.UserMenuOperation.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Email { get; set; }


    }
}
