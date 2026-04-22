using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using TaskTracker.Application.Interfaces.Repositories.Common;

namespace TaskTracker.Infrastructure.Persistence.Repositories.Common
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _defaultConnectionString;
        private readonly string _authConnectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _defaultConnectionString = configuration.GetConnectionString("TaskTrackerConnection");
            _authConnectionString = configuration.GetConnectionString("TaskTrackerAuthConnection");
        }

        public IDbConnection CreateDefaultConnection() => new SqlConnection(_defaultConnectionString);

        public IDbConnection CreateAuthConnection() => new SqlConnection(_authConnectionString);
    }
}
