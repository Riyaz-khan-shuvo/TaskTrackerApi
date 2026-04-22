using Microsoft.EntityFrameworkCore;
using TaskTracker.Core.Entities;

namespace TaskTracker.Infrastructure.Persistence.Seeder
{
    public static class SeederExtensions
    {
        public static void SeedMenuData(this ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 11, 18, 10, 0, 45, DateTimeKind.Utc);

            modelBuilder.Entity<Menu>().HasData(

                new Menu
                {
                    Id = 1,
                    Name = "Menu Authorization",
                    Url = null,
                    Module = "SetUp",
                    Controller = null,
                    ParentId = 0,
                    SubParentId = 0,
                    SubChildId = 0,
                    DisplayOrder = 8,
                    CreatedBy = "SYSTEM",
                    CreatedOn = now,
                    LastModifiedBy = "SYSTEM",
                    LastModifiedOn = now,
                    IconClass = "nav-icon fa-solid fa-user-shield",
                    IsActive=true

                },

                new Menu
                {
                    Id = 2,
                    Name = "Role",
                    Url = "/SetUp/MenuAuthorization/Role",
                    Module = "SetUp",
                    Controller = "MenuAuthorization",
                    ParentId = 1,
                    SubParentId = 0,
                    SubChildId = 0,
                    DisplayOrder = 9,
                    CreatedBy = "SYSTEM",
                    CreatedOn = now,
                    LastModifiedBy = "SYSTEM",
                    LastModifiedOn = now,
                    IconClass = "nav-icon fas fa-user-circle",
                    IsActive = true

                },

                new Menu
                {
                    Id = 3,
                    Name = "Role Menu Assign",
                    Url = "/SetUp/MenuAuthorization/RoleMenu",
                    Module = "SetUp",
                    Controller = "MenuAuthorization",
                    ParentId = 1,
                    SubParentId = 0,
                    SubChildId = 0,
                    DisplayOrder = 11,
                    CreatedBy = "SYSTEM",
                    CreatedOn = now,
                    LastModifiedBy = "SYSTEM",
                    LastModifiedOn = now,
                    IconClass = "nav-icon fas fa-user-shield",
                    IsActive = true

                }
            );
        }

        public static void SeedRoleData(this ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 11, 18, 10, 0, 45, DateTimeKind.Utc);

            var superAdminIdentityId = Guid.Parse("3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0");
            var adminIdentityId = Guid.Parse("37cc67e1-41ca-461c-bf34-2b5e62dbae32");
            var userIdentityId = Guid.Parse("a0cab2c3-6558-4a1c-be81-dfb39180da3d");

            modelBuilder.Entity<Role>().HasData(

                new Role
                {
                    Id = 1,
                    Name = "SuperAdmin",
                    IdentityRoleId = superAdminIdentityId,
                    CreatedBy = "SYSTEM",
                    CreatedOn = now,
                    LastModifiedBy = "SYSTEM",
                    LastModifiedOn = now,

                },

                new Role
                {
                    Id = 2,
                    Name = "Admin",
                    IdentityRoleId = adminIdentityId,
                    CreatedBy = "SYSTEM",
                    CreatedOn = now,
                    LastModifiedBy = "SYSTEM",
                    LastModifiedOn = now,

                },

                new Role
                {
                    Id = 3,
                    Name = "User",
                    IdentityRoleId = userIdentityId,
                    CreatedBy = "SYSTEM",
                    CreatedOn = now,
                    LastModifiedBy = "SYSTEM",
                    LastModifiedOn = now,

                }
            );
        }

        public static void SeedRoleMenuData(this ModelBuilder modelBuilder)
        {
            var now = new DateTime(2024, 11, 18, 10, 0, 45, DateTimeKind.Utc);

            const int superAdminRoleId = 1;
            const int adminRoleId = 2;
            const int userRoleId = 3;

            modelBuilder.Entity<RoleMenu>().HasData(

                new RoleMenu { Id = 1, RoleId = superAdminRoleId, MenuId = 1, List = true, Insert = true, Delete = true, Post = true, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },
                new RoleMenu { Id = 2, RoleId = superAdminRoleId, MenuId = 2, List = true, Insert = true, Delete = true, Post = true, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },
                new RoleMenu { Id = 3, RoleId = superAdminRoleId, MenuId = 3, List = true, Insert = true, Delete = true, Post = true, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },

                new RoleMenu { Id = 4, RoleId = adminRoleId, MenuId = 1, List = true, Insert = true, Delete = true, Post = true, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },
                new RoleMenu { Id = 5, RoleId = adminRoleId, MenuId = 2, List = true, Insert = true, Delete = true, Post = true, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },
                new RoleMenu { Id = 6, RoleId = adminRoleId, MenuId = 3, List = true, Insert = true, Delete = true, Post = true, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },

                new RoleMenu { Id = 7, RoleId = userRoleId, MenuId = 1, List = true, Insert = false, Delete = false, Post = false, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, },
                new RoleMenu { Id = 8, RoleId = userRoleId, MenuId = 2, List = true, Insert = false, Delete = false, Post = false, CreatedBy = "SYSTEM", CreatedOn = now, LastModifiedBy = "SYSTEM", LastModifiedOn = now, }
            );
        }
    }
}