using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Reflection;
using TaskTracker.Application.DTOs.Common.Grid;
using TaskTracker.Application.Interfaces.Repositories.Common;
using TaskTracker.Application.Interfaces.Services.Common;

namespace TaskTracker.Infrastructure.Services.Common
{
    public class GridService<T> : IGridService<T> where T : class
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public GridService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<GridEntity<T>> GetGridDataAsync(GridOptions gridOption, string procName, string callType, string orderby, params string[] parameters)
        {
            try
            {
                gridOption.take = gridOption.skip + gridOption.take;

                string filterby = gridOption.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
                if (!string.IsNullOrEmpty(filterby) && filterby.Contains("YYYY-MM-DD"))
                    filterby = filterby.Replace("YYYY-MM-DD", "yyyy-MM-dd");

                if (gridOption.sort != null && gridOption.sort.Count > 0)
                    orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
                else
                    orderby += " DESC";

                var p = new DynamicParameters();
                p.Add("@CallType", callType);
                p.Add("@skip", gridOption.skip);
                p.Add("@take", gridOption.take);
                p.Add("@filter", filterby);
                p.Add("@orderby", orderby);

                for (int i = 0; i < Math.Min(parameters.Length, 10); i++)
                    p.Add("@param" + (i + 1), parameters[i]);

                using var connection = _connectionFactory.CreateDefaultConnection();
                using var multi = await connection.QueryMultipleAsync(procName, p, commandType: CommandType.StoredProcedure);

                var count = await multi.ReadFirstAsync<int>();
                var data = (await multi.ReadAsync<T>()).ToList();

                return new GridResult<T>().Data(data, count);
            }
            catch (Exception ex)
            {
                throw new Exception($"GridData Error: {ex.Message}", ex);
            }
        }

        public async Task<GridEntity<T>> GetGridDataAsync(GridOptions gridOption, string procName, string callType, string orderby,
            string param1 = "", string param2 = "", string param3 = "", string param4 = "", string param5 = "",
            string param6 = "", string param7 = "", string param8 = "", string param9 = "", string param10 = "")
        {
            string filterby = gridOption?.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";

            if (gridOption?.sort != null && gridOption.sort.Count > 0)
                orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
            else
                orderby += " DESC";

            if (!string.IsNullOrEmpty(filterby) && filterby.Contains("YYYY-MM-DD"))
                filterby = filterby.Replace("YYYY-MM-DD", "yyyy-MM-dd");

            gridOption.take = gridOption.skip + gridOption.take;

            var parameters = new DynamicParameters();
            parameters.Add("@CallType", callType);
            parameters.Add("@skip", gridOption.skip);
            parameters.Add("@take", gridOption.take);
            parameters.Add("@filter", filterby);
            parameters.Add("@orderby", orderby);
            parameters.Add("@param1", param1);
            parameters.Add("@param2", param2);
            parameters.Add("@param3", param3);
            parameters.Add("@param4", param4);
            parameters.Add("@param5", param5);
            parameters.Add("@param6", param6);
            parameters.Add("@param7", param7);
            parameters.Add("@param8", param8);
            parameters.Add("@param9", param9);
            parameters.Add("@param10", param10);

            using var connection = _connectionFactory.CreateDefaultConnection();
            using var multi = await connection.QueryMultipleAsync(procName, parameters, commandType: CommandType.StoredProcedure);

            var totalCountResult = await multi.ReadFirstOrDefaultAsync();
            var totalCount = totalCountResult?.TotalCount ?? 0;

            var data = (await multi.ReadAsync<T>()).ToList();
            return new GridResult<T>().Data(data, totalCount);
        }

        public async Task<GridEntity<T>> GetGridDataCmdAsync(GridOptions gridOption, string sqlQuery, string orderby)
        {
            string filterby = gridOption?.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";

            if (gridOption?.sort != null && gridOption.sort.Count > 0)
                orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
            else
                orderby += " DESC";

            gridOption.take = gridOption.skip + gridOption.take;

            var parameters = new DynamicParameters();
            parameters.Add("@skip", gridOption.skip);
            parameters.Add("@take", gridOption.take);
            parameters.Add("@filter", filterby);
            parameters.Add("@orderby", orderby);

            using var connection = _connectionFactory.CreateDefaultConnection();
            using var multi = await connection.QueryMultipleAsync(sqlQuery, parameters);

            var totalCountResult = await multi.ReadFirstOrDefaultAsync();
            var totalCount = totalCountResult?.totalcount ?? 0;

            var data = (await multi.ReadAsync<T>()).ToList();
            return new GridResult<T>().Data(data, totalCount);
        }

        public async Task<GridEntity<T>> GetTransactionalGridDataCmdAsync(GridOptions gridOption, string sqlQuery, string orderby, string[] conditionalFields, string[] conditionalValues)
        {
            conditionalFields ??= Array.Empty<string>();
            conditionalValues ??= Array.Empty<string>();
            for (int i = 0; i < conditionalFields.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(conditionalFields[i]) || string.IsNullOrWhiteSpace(conditionalValues[i]))
                    continue;

                var field = conditionalFields[i].Split(".")[1];
                var placeholder = conditionalFields[i].Replace(".", "").Replace(" between", "");

                sqlQuery = sqlQuery.Replace("@" + placeholder, "'" + conditionalValues[i] + "'");

                if (conditionalFields[i].ToLower().Contains("date") && i + 1 < conditionalValues.Length)
                    sqlQuery = sqlQuery.Replace("@" + field, "'" + conditionalValues[i + 1] + "'");
            }

            return await GetGridDataCmdAsync(gridOption, sqlQuery, orderby);
        }

        public async Task<GridEntity<T>> GetAuthGridDataCmdAsync(GridOptions gridOption, string sqlQuery, string orderby)
        {
            string filterby = gridOption?.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";

            if (gridOption?.sort != null && gridOption.sort.Count > 0)
                orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
            else
                orderby += " DESC";

            gridOption.take = gridOption.skip + gridOption.take;

            var parameters = new DynamicParameters();
            parameters.Add("@skip", gridOption.skip);
            parameters.Add("@take", gridOption.take);
            parameters.Add("@filter", filterby);
            parameters.Add("@orderby", orderby);

            using var connection = _connectionFactory.CreateAuthConnection();
            using var multi = await connection.QueryMultipleAsync(sqlQuery, parameters);

            var totalCountResult = await multi.ReadFirstOrDefaultAsync();
            var totalCount = totalCountResult?.totalcount ?? 0;

            var data = (await multi.ReadAsync<T>()).ToList();
            return new GridResult<T>().Data(data, totalCount);
        }

        // Apply Filters for EF IQueryable
        public IQueryable<T> ApplyFilters(IQueryable<T> query, AzFilter.GridFilter filter)
        {
            if (filter == null || string.IsNullOrEmpty(filter.Field) || filter.Value == null)
                return query;

            // Handle operator dynamically
            switch (filter.Operator?.ToLower())
            {
                case "contains":
                    query = query.Where($"{filter.Field}.Contains(@0)", filter.Value);
                    break;
                case "eq":
                    query = query.Where($"{filter.Field} == @0", filter.Value);
                    break;
                case "neq":
                    query = query.Where($"{filter.Field} != @0", filter.Value);
                    break;
                case "gte":
                    query = query.Where($"{filter.Field} >= @0", filter.Value);
                    break;
                case "lte":
                    query = query.Where($"{filter.Field} <= @0", filter.Value);
                    break;
                default:
                    // Unknown operator, skip filtering
                    break;
            }

            return query;
        }

        public IQueryable<T> ApplySorting(IQueryable<T> query, List<AzFilter.GridSort> sort)
        {
            if (sort == null || !sort.Any())
                return query;

            var orderByString = string.Join(", ", sort.Select(s => $"{s.field} {s.dir}"));
            return query.OrderBy(orderByString);
        }



        // Apply multiple filters
        public IQueryable<T> ApplyFilters(IQueryable<T> query, List<AzFilter.GridFilter> filters)
        {
            if (filters == null || !filters.Any())
                return query;

            foreach (var filter in filters)
                query = ApplyFilters(query, filter);

            return query;
        }



        public IQueryable<T> ApplyGlobalSearch(IQueryable<T> query, List<AzFilter.GridFilter> filters)
        {
            if (filters == null || !filters.Any())
                return query;

            var conditions = new List<string>();
            var parameters = new List<object>();

            foreach (var filter in filters)
            {
                var property = typeof(T).GetProperty(
                    filter.Field,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
                );

                if (property == null || string.IsNullOrEmpty(filter.Value))
                    continue;

                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var value = filter.Value;

                if (propertyType == typeof(string))
                {
                    conditions.Add($"{property.Name}.Contains(@{parameters.Count})");
                    parameters.Add(value);
                }
                else if (propertyType == typeof(int))
                {
                    if (int.TryParse(value, out int intVal))
                    {
                        conditions.Add($"{property.Name} == @{parameters.Count}");
                        parameters.Add(intVal);
                    }
                }
                else if (propertyType == typeof(bool))
                {
                    if (value.ToLower() == "true" || value.ToLower() == "yes" || value == "1")
                    {
                        conditions.Add($"{property.Name} == @{parameters.Count}");
                        parameters.Add(true);
                    }
                    else if (value.ToLower() == "false" || value.ToLower() == "no" || value == "0")
                    {
                        conditions.Add($"{property.Name} == @{parameters.Count}");
                        parameters.Add(false);
                    }
                }
                else if (propertyType == typeof(DateOnly))
                {
                    if (DateOnly.TryParse(value, out DateOnly dateOnlyVal))
                    {
                        conditions.Add($"{property.Name} == @{parameters.Count}");
                        parameters.Add(dateOnlyVal);
                    }
                }
                else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTimeOffset))
                {
                    if (DateTime.TryParse(value, out DateTime dateVal))
                    {
                        conditions.Add($"{property.Name}.Date == @{parameters.Count}");
                        parameters.Add(dateVal.Date);
                    }
                }
            }

            if (conditions.Any())
            {
                var whereClause = string.Join(" OR ", conditions);
                query = query.Where(whereClause, parameters.ToArray());
            }

            return query;
        }




        public async Task<GridEntity<T>> GetEfGridDataAsync(
     IQueryable<T> query,
     GridOptions options,
     CancellationToken cancellationToken,
     string? defaultOrderBy = "Id DESC")
        {
            //if (options?.filter != null && options.filter.Filters.Count > 0)
            //{
            //    query = ApplyFilters(query, options.filter.Filters);
            //}

            // GLOBAL SEARCH
            if (options?.filter != null && options.filter.Filters.Count > 0)
            {
                query = ApplyGlobalSearch(query, options.filter.Filters);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            if (options?.sort != null && options.sort.Count > 0)
            {
                query = ApplySorting(query, options.sort);
            }
            else if (!string.IsNullOrWhiteSpace(defaultOrderBy))
            {
                query = query.OrderBy(defaultOrderBy);
            }

            var data = await query
                .Skip(options.skip)
                .Take(options.take)
                .ToListAsync(cancellationToken);

            return new GridResult<T>().Data(data, totalCount);
        }

        //// Apply single filter (used internally)
        //public IQueryable<T> ApplyFilters(IQueryable<T> query, AzFilter.GridFilter filter)
        //{
        //    if (filter == null || string.IsNullOrEmpty(filter.Field) || filter.Value == null)
        //        return query;

        //    switch (filter.Operator?.ToLower())
        //    {
        //        case "contains":
        //            query = query.Where($"{filter.Field}.Contains(@0)", filter.Value);
        //            break;
        //        case "eq":
        //            query = query.Where($"{filter.Field} == @0", filter.Value);
        //            break;
        //        case "neq":
        //            query = query.Where($"{filter.Field} != @0", filter.Value);
        //            break;
        //        case "gte":
        //            query = query.Where($"{filter.Field} >= @0", filter.Value);
        //            break;
        //        case "lte":
        //            query = query.Where($"{filter.Field} <= @0", filter.Value);
        //            break;
        //    }

        //    return query;
        //}

        //// Apply sorting
        //public IQueryable<T> ApplySorting(IQueryable<T> query, List<AzFilter.GridSort> sort)
        //{
        //    if (sort == null || !sort.Any())
        //        return query;

        //    var orderByString = string.Join(", ", sort.Select(s => $"{s.field} {s.dir}"));
        //    return query.OrderBy(orderByString);
        //}


    }



}
