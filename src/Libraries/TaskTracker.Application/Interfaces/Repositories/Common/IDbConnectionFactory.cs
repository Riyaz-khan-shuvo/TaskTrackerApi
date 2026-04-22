using System.Data;

namespace TaskTracker.Application.Interfaces.Repositories.Common
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDefaultConnection();
        IDbConnection CreateAuthConnection();
    }
}
