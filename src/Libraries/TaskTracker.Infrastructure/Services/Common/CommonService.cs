using Dapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.Interfaces.Services.Common;

namespace TaskTracker.Infrastructure.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public CommonService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<T>> SelectDataListAsync<T>(
            string storedProcedure,
            string action,
            object? parameters = null)
        {
            try
            {
                using var connection = _connectionFactory.CreateDefaultConnection();

                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("@calltype", action);

                if (parameters != null)
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        dynamicParams.Add($"@{prop.Name}", prop.GetValue(parameters));
                    }
                }

                var result = await connection.QueryAsync<T>(
                    storedProcedure,
                    dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"SelectDataListAsync Error: {ex.Message}", ex);
            }
        }

        public async Task<List<T>> SelectAuthDataListAsync<T>(
            string storedProcedure,
            string action,
            object? parameters = null)
        {
            try
            {
                using var connection = _connectionFactory.CreateAuthConnection();

                var dynamicParams = new DynamicParameters();
                dynamicParams.Add("@Action", action);

                if (parameters != null)
                {
                    foreach (var prop in parameters.GetType().GetProperties())
                    {
                        dynamicParams.Add($"@{prop.Name}", prop.GetValue(parameters));
                    }
                }

                var result = await connection.QueryAsync<T>(
                    storedProcedure,
                    dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"SelectAuthDataListAsync Error: {ex.Message}", ex);
            }
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string subFolder, int maxSizeKB)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new Exception("No image file provided.");

            if (imageFile.Length > maxSizeKB * 1024)
                throw new Exception($"Image size must be less than {maxSizeKB} KB.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", subFolder);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/{subFolder}/{fileName}";
        }
    }
}
