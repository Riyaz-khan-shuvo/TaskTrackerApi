using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces.Services;
using TaskTracker.Core.Entities;
using TaskTracker.Infrastructure.Persistence.Seeder;

namespace TaskTracker.Infrastructure.Persistence.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        private readonly ICurrentUserService _currentUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleMenu> RoleMenus { get; set; }
        public DbSet<UserMenu> UserMenus { get; set; }
        public DbSet<BusinessCategory> BusinessCategories { get; set; }
        public DbSet<BusinessCategoryType> BusinessCategoryTypes { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            ConfigureIndexes(modelBuilder);
            modelBuilder.SeedMenuData();
            modelBuilder.SeedRoleData();
            modelBuilder.SeedRoleMenuData();
        }

        private static void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>()
                .HasIndex(x => x.Name);
        }
    }
}