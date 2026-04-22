using TaskTracker.Application.DTOs.Common;

namespace TaskTracker.Application.Interfaces.Services.Common
{
    public interface IDropdownService
    {
        Task<ResultVM> GetCategoryDropdownAsync();
        Task<ResultVM> GetSubCategoryDropdownByCategory(Guid categoryId);

        Task<List<SelectModel>> GetBusinessCategoriesAsync(int businessCategoryTypeId);
        Task<List<SelectModel>> GetBusinessCategoryTypesAsync();
        Task<Dictionary<string, List<SelectModel>>> GetAllDropdownsAsync();
    }
}
