using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core.Entities.Auth;

namespace TaskTracker.Infrastructure.Persistence.DataContext
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, Guid,
        ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim,
        ApplicationUserToken>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureIdentityTables(builder);
            ConfigureIndexes(builder);
            SeedIdentityData(builder);
        }

        private static void ConfigureIdentityTables(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<ApplicationRole>().ToTable("Roles");
            builder.Entity<ApplicationUserClaim>().ToTable("UserClaims");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles");
            builder.Entity<ApplicationUserLogin>().ToTable("UserLogins");
            builder.Entity<ApplicationRoleClaim>().ToTable("RoleClaims");
            builder.Entity<ApplicationUserToken>().ToTable("UserTokens");
        }

        private static void ConfigureIndexes(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        private static void SeedIdentityData(ModelBuilder builder)
        {
            var superAdminRoleId = Guid.Parse("3cfd9eee-08cb-4da3-9e6f-c3166b50d3b0");
            var adminRoleId = Guid.Parse("37cc67e1-41ca-461c-bf34-2b5e62dbae32");
            var userRoleId = Guid.Parse("a0cab2c3-6558-4a1c-be81-dfb39180da3d");

            var superAdminUserId = Guid.Parse("472ba632-6133-44a1-b158-6c10bd7d850d");

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = superAdminRoleId,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    ConcurrencyStamp = superAdminRoleId.ToString()
                },
                new ApplicationRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId.ToString()
                },
                new ApplicationRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = userRoleId.ToString()
                }
            );

            var superAdminUser = new ApplicationUser
            {
                Id = superAdminUserId,
                FirstName="Super",
                LastName="Admin",
                UserName = "superadmin@TaskTracker.com",
                Email = "superadmin@TaskTracker.com",
                NormalizedUserName = "SUPERADMIN@TaskTracker.COM",
                NormalizedEmail = "SUPERADMIN@TaskTracker.COM",
                EmailConfirmed = true,
                SecurityStamp = "STATIC_SECURITY_STAMP" 
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            superAdminUser.PasswordHash = hasher.HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<ApplicationUser>().HasData(superAdminUser);

            builder.Entity<ApplicationUserRole>().HasData(
                new ApplicationUserRole
                {
                    UserId = superAdminUserId,
                    RoleId = superAdminRoleId
                },
                new ApplicationUserRole
                {
                    UserId = superAdminUserId,
                    RoleId = adminRoleId
                },
                new ApplicationUserRole
                {
                    UserId = superAdminUserId,
                    RoleId = userRoleId
                }
            );
        }
    }
}