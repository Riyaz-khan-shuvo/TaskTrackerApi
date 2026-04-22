using Microsoft.AspNetCore.Http;

namespace TaskTracker.Application.Interfaces.Services.Common
{
    public interface ICommonService
    {
        Task<List<T>> SelectDataListAsync<T>(string storedProcedure, string action, object? parameters = null);
        Task<List<T>> SelectAuthDataListAsync<T>(string storedProcedure, string action, object? parameters = null);
        Task<string> UploadImageAsync(IFormFile imageFile, string subFolder, int maxSizeKB);

    }
}
