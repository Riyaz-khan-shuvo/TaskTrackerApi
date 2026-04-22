using TaskTracker.Application.DTOs.Common.Grid;
using static TaskTracker.Application.DTOs.Common.Grid.AzFilter;

namespace TaskTracker.Application.Interfaces.Services.Common
{

    public interface IGridService<T> where T : class
    {
        Task<GridEntity<T>> GetGridDataAsync(
            GridOptions gridOption,
            string procName,
            string callType,
            string orderby,
            params string[] parameters);

        Task<GridEntity<T>> GetGridDataAsync(
            GridOptions gridOption,
            string procName,
            string callType,
            string orderby,
            string param1 = "",
            string param2 = "",
            string param3 = "",
            string param4 = "",
            string param5 = "",
            string param6 = "",
            string param7 = "",
            string param8 = "",
            string param9 = "",
            string param10 = "");

        Task<GridEntity<T>> GetGridDataCmdAsync(
            GridOptions gridOption,
            string sqlQuery,
            string orderby);

        Task<GridEntity<T>> GetTransactionalGridDataCmdAsync(
            GridOptions gridOption,
            string sqlQuery,
            string orderby,
            string[] conditionalFields,
            string[] conditionalValues);

        Task<GridEntity<T>> GetAuthGridDataCmdAsync(
            GridOptions gridOption,
            string sqlQuery,
            string orderby);

        /// <summary>
        /// Apply dynamic filters based on GridFilter
        /// </summary>
        IQueryable<T> ApplyFilters(IQueryable<T> query, GridFilter filter);
        IQueryable<T> ApplyFilters(IQueryable<T> query, List<AzFilter.GridFilter> filters);

        /// <summary>
        /// Apply dynamic sorting based on GridSort
        /// </summary>
        IQueryable<T> ApplySorting(IQueryable<T> query, List<GridSort> sort);

        Task<GridEntity<T>> GetEfGridDataAsync(
            IQueryable<T> query,
            GridOptions options,
            CancellationToken cancellationToken,
            string? defaultOrderBy = "Id DESC");

    }
}
