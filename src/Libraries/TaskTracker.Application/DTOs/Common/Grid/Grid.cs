using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TaskTracker.Application.DTOs.Common.Grid;

public class GridService<T> where T : class
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbConnection _authDbConnection;

    public GridService(IConfiguration configuration)
    {
        _dbConnection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        _authDbConnection = new SqlConnection(configuration.GetConnectionString("AuthConnection"));
    }

    public async Task<GridEntity<T>> GetGridDataAsync(GridOptions gridOption, string procName, string callType, string orderby, params string[] parameters)
    {
        try
        {
            gridOption.take = gridOption.skip + gridOption.take;
            string filterby = gridOption.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
            if (!string.IsNullOrEmpty(filterby) && filterby.Contains("YYYY-MM-DD"))
            {
                filterby = filterby.Replace("YYYY-MM-DD", "yyyy-MM-dd");
            }

            if (gridOption.sort != null && gridOption.sort.Count > 0)
            {
                orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
            }
            else
            {
                orderby += " DESC";
            }

            var p = new DynamicParameters();
            p.Add("@CallType", callType);
            p.Add("@skip", gridOption.skip);
            p.Add("@take", gridOption.take);
            p.Add("@filter", filterby);
            p.Add("@orderby", orderby);

            for (int i = 0; i < Math.Min(parameters.Length, 10); i++)
            {
                p.Add("@param" + (i + 1), parameters[i]);
            }

            using var multi = await _dbConnection.QueryMultipleAsync(procName, p, commandType: CommandType.StoredProcedure);
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
        var filterby = gridOption?.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
        if (gridOption?.sort != null)
        {
            orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
        }
        else
        {
            orderby += " DESC";
        }

        if (!string.IsNullOrEmpty(filterby) && filterby.Contains("YYYY-MM-DD"))
        {
            filterby = filterby.Replace("YYYY-MM-DD", "yyyy-MM-dd");
        }

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

        using var multi = await _dbConnection.QueryMultipleAsync(procName, parameters, commandType: CommandType.StoredProcedure);

        var totalCountResult = await multi.ReadFirstOrDefaultAsync();
        var totalCount = totalCountResult?.TotalCount ?? 0;

        var data = (await multi.ReadAsync<T>()).ToList();
        return new GridResult<T>().Data(data, totalCount);
    }

    public async Task<GridEntity<T>> GetGridDataCmdAsync(GridOptions gridOption, string sqlQuery, string orderby)
    {
        var filterby = gridOption?.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
        if (gridOption?.sort != null)
        {
            orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
        }
        else
        {
            orderby += " DESC";
        }

        gridOption.take = gridOption.skip + gridOption.take;

        var parameters = new DynamicParameters();
        parameters.Add("@skip", gridOption.skip);
        parameters.Add("@take", gridOption.take);
        parameters.Add("@filter", filterby);
        parameters.Add("@orderby", orderby);

        using var multi = await _dbConnection.QueryMultipleAsync(sqlQuery, parameters);

        var totalCountResult = await multi.ReadFirstOrDefaultAsync();
        var totalCount = totalCountResult?.totalcount ?? 0;
        var data = (await multi.ReadAsync<T>()).ToList();

        return new GridResult<T>().Data(data, totalCount);
    }

    public async Task<GridEntity<T>> GetTransactionalGridDataCmdAsync(GridOptions gridOption, string sqlQuery, string orderby, string[] conditionalFields, string[] conditionalValues)
    {
        for (int i = 0; i < conditionalFields.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(conditionalFields[i]) || string.IsNullOrWhiteSpace(conditionalValues[i]))
                continue;

            var field = conditionalFields[i].Split(".")[1];
            var placeholder = conditionalFields[i].Replace(".", "").Replace(" between", "");

            sqlQuery = sqlQuery.Replace("@" + placeholder, "'" + conditionalValues[i] + "'");

            if (conditionalFields[i].ToLower().Contains("date") && i + 1 < conditionalValues.Length)
            {
                sqlQuery = sqlQuery.Replace("@" + field, "'" + conditionalValues[i + 1] + "'");
            }
        }

        return await GetGridDataCmdAsync(gridOption, sqlQuery, orderby);
    }

    public async Task<GridEntity<T>> GetAuthGridDataCmdAsync(GridOptions gridOption, string sqlQuery, string orderby)
    {
        var filterby = gridOption?.filter != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
        if (gridOption?.sort != null)
        {
            orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
        }
        else
        {
            orderby += " DESC";
        }

        gridOption.take = gridOption.skip + gridOption.take;

        var parameters = new DynamicParameters();
        parameters.Add("@skip", gridOption.skip);
        parameters.Add("@take", gridOption.take);
        parameters.Add("@filter", filterby);
        parameters.Add("@orderby", orderby);

        using var multi = await _dbConnection.QueryMultipleAsync(sqlQuery, parameters);

        var totalCountResult = await multi.ReadFirstOrDefaultAsync();
        var totalCount = totalCountResult?.totalcount ?? 0;
        var data = (await multi.ReadAsync<T>()).ToList();

        return new GridResult<T>().Data(data, totalCount);
    }

}






















//using TaskTracker.Core.Entities;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TaskTracker.Core.Common.Grid
//{
//    public class Grid<T>
//    {
//        private static SqlDataAdapter da;
//        private static SqlConnection dbConn;
//        private static SqlCommand cmd;
//        private static DataSet ds;
//        private static DataTable dt;
//        private static int totalCount = 0;

//        public static GridEntity<T> GetGridData(GridOptions gridOption, string ProcName, string CallType, string orderby, string param1 = "", string param2 = "", string param3 = "", string param4 = "", string param5 = "", string param6 = "", string param7 = "", string param8 = "", string param9 = "", string param10 = "")
//        {
//            try
//            {
//                dt = new DataTable();
//                gridOption.take = gridOption.skip + gridOption.take;
//                var filterby = "";

//                if (gridOption.filter != null)
//                {
//                    filterby = gridOption != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
//                }
//                if (gridOption.sort != null)
//                {
//                    orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
//                }
//                else if (gridOption.sort == null)
//                {
//                    orderby = orderby + " " + " DESC ";
//                }
//                if (!string.IsNullOrEmpty(filterby) && filterby.Contains("YYYY-MM-DD"))
//                {
//                    filterby = filterby.Replace("YYYY-MM-DD", "yyyy-MM-dd");
//                }


//                dbConn = new SqlConnection(DatabaseHelper.GetConnectionString());
//                dbConn.Open();
//                cmd = new SqlCommand(ProcName, dbConn);
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.Add(new SqlParameter("@CallType", CallType));
//                cmd.Parameters.Add(new SqlParameter("@skip", gridOption.skip));
//                cmd.Parameters.Add(new SqlParameter("@take ", gridOption.take));
//                cmd.Parameters.Add(new SqlParameter("@filter", filterby));
//                cmd.Parameters.Add(new SqlParameter("@orderby", orderby.Trim()));
//                cmd.Parameters.Add(new SqlParameter("@param1", param1));
//                cmd.Parameters.Add(new SqlParameter("@param2", param2));
//                cmd.Parameters.Add(new SqlParameter("@param3", param3));
//                cmd.Parameters.Add(new SqlParameter("@param4", param4));
//                cmd.Parameters.Add(new SqlParameter("@param5", param5));
//                cmd.Parameters.Add(new SqlParameter("@param6", param6));
//                cmd.Parameters.Add(new SqlParameter("@param7", param7));
//                cmd.Parameters.Add(new SqlParameter("@param8", param8));
//                cmd.Parameters.Add(new SqlParameter("@param9", param9));
//                cmd.Parameters.Add(new SqlParameter("@param10", param10));

//                da = new SqlDataAdapter(cmd);
//                ds = new DataSet();
//                da.Fill(ds);
//                dbConn.Close();
//                dbConn.Dispose();
//                cmd.Dispose();

//                dt = ds.Tables[1];
//                totalCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]);
//                var dataList = (List<T>)ListConversion.ConvertTo<T>(dt).ToList();
//                var result = new GridResult<T>().Data(dataList, totalCount);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public static GridEntity<T> GetGridData_CMD(GridOptions gridOption, string sqlQuery, string orderby, string param1 = "", string param2 = "", string param3 = "", string param4 = "", string param5 = "", string param6 = "", string param7 = "", string param8 = "", string param9 = "", string param10 = "")
//        {
//            try
//            {
//                gridOption.take = gridOption.skip + gridOption.take;
//                var filterby = "";
//                if (gridOption.filter.Filters.Count > 0)
//                {
//                    filterby = gridOption != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
//                }

//                if (gridOption.sort.Count > 0)
//                {
//                    orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
//                }
//                else
//                {
//                    orderby = orderby + " DESC";
//                }

//                // Open SQL connection
//                dbConn = new SqlConnection(DatabaseHelper.GetConnectionString());
//                dbConn.Open();
//                cmd = new SqlCommand(sqlQuery, dbConn);
//                cmd.CommandType = CommandType.Text;

//                // Add parameters dynamically
//                cmd.Parameters.Add(new SqlParameter("@skip", gridOption.skip));
//                cmd.Parameters.Add(new SqlParameter("@take", gridOption.take));
//                cmd.Parameters.Add(new SqlParameter("@filter", filterby));
//                cmd.Parameters.Add(new SqlParameter("@orderby", orderby.Trim()));

//                da = new SqlDataAdapter(cmd);
//                ds = new DataSet();
//                da.Fill(ds);
//                dbConn.Close();
//                dbConn.Dispose();
//                cmd.Dispose();

//                DataTable countTable = ds.Tables[0]; // Total count should be in the first result set
//                int totalCount = 0;

//                if (countTable.Rows.Count > 0)
//                {
//                    totalCount = Convert.ToInt32(countTable.Rows[0]["totalcount"]);
//                }

//                // Access the second table for actual data (should be in Tables[1])
//                DataTable dataTable = ds.Tables[1]; // The actual data should be in the second result set
//                var dataList = (List<T>)ListConversion.ConvertTo<T>(dataTable).ToList();

//                // Create result object
//                var result = new GridResult<T>().Data(dataList, totalCount);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public static GridEntity<T> GetTransactionalGridData_CMD(GridOptions gridOption, string sqlQuery, string orderby, string[] conditionalFields, string[] conditionalValues, string param1 = "", string param2 = "", string param3 = "", string param4 = "", string param5 = "", string param6 = "", string param7 = "", string param8 = "", string param9 = "", string param10 = "")
//        {
//            try
//            {
//                int count = 1;
//                gridOption.take = gridOption.skip + gridOption.take;
//                var filterby = "";
//                if (gridOption.filter.Filters.Count > 0)
//                {
//                    filterby = gridOption != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
//                }

//                if (gridOption.sort.Count > 0)
//                {
//                    orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
//                }
//                else
//                {
//                    orderby = orderby + " DESC";
//                }

//                for (int i = 0; i < conditionalFields.Length; i++)
//                {
//                    string cField = "";
//                    string field = "";
//                    if (string.IsNullOrWhiteSpace(conditionalFields[i]) || string.IsNullOrWhiteSpace(conditionalValues[i]))
//                    {
//                        continue;
//                    }
//                    field = conditionalFields[i].ToString().Split(".")[1];
//                    cField = conditionalFields[i].ToString();
//                    cField = cField.Replace(".", "").Replace(" between", "");
//                    field = field.Replace(".", "").Replace(" between", "");
//                    sqlQuery = sqlQuery.Replace("@" + cField, "'" + conditionalValues[i] + "'");

//                    if (conditionalFields[i].ToLower().Contains("date"))
//                    {
//                        if (count == 1)
//                        {
//                            sqlQuery = sqlQuery.Replace("@" + field, "'" + conditionalValues[i + 1] + "'");
//                        }
//                        count++;
//                    }
//                }

//                // Open SQL connection
//                dbConn = new SqlConnection(DatabaseHelper.GetConnectionString());
//                dbConn.Open();
//                cmd = new SqlCommand(sqlQuery, dbConn);
//                cmd.CommandType = CommandType.Text;

//                // Add parameters dynamically
//                cmd.Parameters.Add(new SqlParameter("@skip", gridOption.skip));
//                cmd.Parameters.Add(new SqlParameter("@take", gridOption.take));
//                cmd.Parameters.Add(new SqlParameter("@filter", filterby));
//                cmd.Parameters.Add(new SqlParameter("@orderby", orderby.Trim()));

//                da = new SqlDataAdapter(cmd);
//                ds = new DataSet();
//                da.Fill(ds);
//                dbConn.Close();
//                dbConn.Dispose();
//                cmd.Dispose();

//                DataTable countTable = ds.Tables[0]; // Total count should be in the first result set
//                int totalCount = 0;

//                if (countTable.Rows.Count > 0)
//                {
//                    totalCount = Convert.ToInt32(countTable.Rows[0]["totalcount"]);
//                }

//                // Access the second table for actual data (should be in Tables[1])
//                DataTable dataTable = ds.Tables[1]; // The actual data should be in the second result set
//                var dataList = (List<T>)ListConversion.ConvertTo<T>(dataTable).ToList();

//                // Create result object
//                var result = new GridResult<T>().Data(dataList, totalCount);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public static GridEntity<T> GetAuthGridData_CMD(GridOptions gridOption, string sqlQuery, string orderby, string param1 = "", string param2 = "", string param3 = "", string param4 = "", string param5 = "", string param6 = "", string param7 = "", string param8 = "", string param9 = "", string param10 = "")
//        {
//            try
//            {
//                gridOption.take = gridOption.skip + gridOption.take;
//                var filterby = "";
//                if (gridOption.filter.Filters.Count > 0)
//                {
//                    filterby = gridOption != null ? GridQueryBuilder<T>.FilterCondition(gridOption.filter) : "";
//                }

//                if (gridOption.sort.Count > 0)
//                {
//                    orderby = gridOption.sort[0].field + " " + gridOption.sort[0].dir;
//                }
//                else
//                {
//                    orderby = orderby + " DESC";
//                }

//                // Open SQL connection
//                dbConn = new SqlConnection(DatabaseHelper.GetAuthConnectionString());
//                dbConn.Open();
//                cmd = new SqlCommand(sqlQuery, dbConn);
//                cmd.CommandType = CommandType.Text;

//                // Add parameters dynamically
//                cmd.Parameters.Add(new SqlParameter("@skip", gridOption.skip));
//                cmd.Parameters.Add(new SqlParameter("@take", gridOption.take));
//                cmd.Parameters.Add(new SqlParameter("@filter", filterby));
//                cmd.Parameters.Add(new SqlParameter("@orderby", orderby.Trim()));

//                da = new SqlDataAdapter(cmd);
//                ds = new DataSet();
//                da.Fill(ds);
//                dbConn.Close();
//                dbConn.Dispose();
//                cmd.Dispose();

//                DataTable countTable = ds.Tables[0]; // Total count should be in the first result set
//                int totalCount = 0;

//                if (countTable.Rows.Count > 0)
//                {
//                    totalCount = Convert.ToInt32(countTable.Rows[0]["totalcount"]);
//                }

//                // Access the second table for actual data (should be in Tables[1])
//                DataTable dataTable = ds.Tables[1]; // The actual data should be in the second result set
//                var dataList = (List<T>)ListConversion.ConvertTo<T>(dataTable).ToList();

//                // Create result object
//                var result = new GridResult<T>().Data(dataList, totalCount);

//                return result;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public List<T> Select_Data_List<T>(string procedureName, string callName, string parm1 = "", string parm2 = "", string parm3 = "", string parm4 = "", string parm5 = "", string parm6 = "", string parm7 = "", string parm8 = "", string parm9 = "", string parm10 = "")
//        {
//            try
//            {
//                dt = new DataTable();
//                dbConn = new SqlConnection(DatabaseHelper.GetConnectionString());
//                dbConn.Open();

//                cmd = new SqlCommand(procedureName, dbConn);
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.Parameters.Add(new SqlParameter("@CallType", callName));
//                cmd.Parameters.Add(new SqlParameter("@Desc1", parm1));
//                cmd.Parameters.Add(new SqlParameter("@Desc2", parm2));
//                cmd.Parameters.Add(new SqlParameter("@Desc3", parm3));
//                cmd.Parameters.Add(new SqlParameter("@Desc4", parm4));
//                cmd.Parameters.Add(new SqlParameter("@Desc5", parm5));
//                cmd.Parameters.Add(new SqlParameter("@Desc6", parm6));
//                cmd.Parameters.Add(new SqlParameter("@Desc7", parm7));
//                cmd.Parameters.Add(new SqlParameter("@Desc8", parm8));
//                cmd.Parameters.Add(new SqlParameter("@Desc9", parm9));
//                cmd.Parameters.Add(new SqlParameter("@Desc10", parm10));

//                da = new SqlDataAdapter(cmd);
//                ds = new DataSet();
//                da.Fill(ds);
//                dbConn.Close();

//                dt = ds.Tables[0];
//                var dataList = new List<T>();

//                if (dt.Rows.Count > 0)
//                {
//                    dataList = (List<T>)ListConversion.ConvertTo<T>(dt);
//                }

//                return dataList;
//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }
//        }

//        public static string[] RoleCreateEdit(string ProcName, UserRoleVM model)
//        {
//            string[] results = new string[3];
//            results[0] = "Fail";
//            results[1] = "Fail";
//            results[2] = "";

//            try
//            {
//                using (var dbConn = new SqlConnection(DatabaseHelper.GetConnectionString()))
//                {
//                    dbConn.Open();
//                    using (var cmd = new SqlCommand(ProcName, dbConn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;

//                        cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.NVarChar, 50) { Value = model.Id });
//                        cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 50) { Value = model.Name.Trim() });
//                        cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = model.CreatedBy });
//                        cmd.Parameters.Add(new SqlParameter("@CreatedFrom", SqlDbType.NVarChar, 50) { Value = model.CreatedFrom });
//                        cmd.Parameters.Add(new SqlParameter("@Operation", SqlDbType.NVarChar, 50) { Value = model.Operation });

//                        var result = cmd.ExecuteScalar();

//                        dbConn.Close();
//                        dbConn.Dispose();
//                        cmd.Dispose();

//                        if (result.ToString() == "-1")
//                        {
//                            throw new Exception(model.Operation.ToLower() == "add" ? MessageModel.InsertSuccess : MessageModel.UpdateSuccess);
//                        }
//                        else
//                        {
//                            results[0] = "Success";
//                            results[1] = model.Operation.ToLower() == "add" ? MessageModel.InsertSuccess : MessageModel.UpdateSuccess;
//                            results[2] = result.ToString();

//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                results[0] = "Fail";
//                results[1] = ex.Message;
//                throw;
//            }

//            return results;
//        }

//        public static DataTable GetAll(string procName, int id, string userId)
//        {
//            var dt = new DataTable();
//            try
//            {
//                using (var dbConn = new SqlConnection(DatabaseHelper.GetConnectionString()))
//                {
//                    dbConn.Open();
//                    using (var cmd = new SqlCommand(procName, dbConn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.Parameters.Add(new SqlParameter("@Id", id));

//                        if (!string.IsNullOrEmpty(userId))
//                        {
//                            cmd.Parameters.Add(new SqlParameter("@UserId", userId));
//                        }

//                        using (var da = new SqlDataAdapter(cmd))
//                        {
//                            da.Fill(dt);
//                        }
//                    }
//                }
//                return dt;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error in GetAll: " + ex.Message);
//            }
//        }

//        public static string[] RoleMenuCreateEdit(string ProcName, RoleMenuVM model)
//        {
//            string[] results = new string[2];
//            results[0] = "Fail";
//            results[1] = "Fail";

//            try
//            {
//                using (var dbConn = new SqlConnection(DatabaseHelper.GetConnectionString()))
//                {
//                    dbConn.Open();
//                    using (var cmd = new SqlCommand(ProcName, dbConn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;

//                        cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.NVarChar, 50) { Value = model.RoleId });
//                        cmd.Parameters.Add(new SqlParameter("@MenuId", SqlDbType.NVarChar, 50) { Value = model.MenuId });
//                        cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = model.CreatedBy });
//                        cmd.Parameters.Add(new SqlParameter("@CreatedFrom", SqlDbType.NVarChar, 50) { Value = model.CreatedFrom });

//                        var result = cmd.ExecuteNonQuery();

//                        dbConn.Close();
//                        dbConn.Dispose();
//                        cmd.Dispose();

//                        if (result == -1)
//                        {
//                            throw new Exception(MessageModel.SubmissionFail);
//                        }
//                        else
//                        {
//                            results[0] = "Success";
//                            results[1] = MessageModel.SubmissionSuccess;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                results[0] = "Fail";
//                results[1] = ex.Message;
//                throw;
//            }

//            return results;
//        }

//        public static string[] UserMenuCreateEdit(string ProcName, UserMenuVM model)
//        {
//            string[] results = new string[2];
//            results[0] = "Fail";
//            results[1] = "Fail";

//            try
//            {
//                using (var dbConn = new SqlConnection(DatabaseHelper.GetConnectionString()))
//                {
//                    dbConn.Open();
//                    using (var cmd = new SqlCommand(ProcName, dbConn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;

//                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = model.UserId });
//                        cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.NVarChar, 50) { Value = model.RoleId });
//                        cmd.Parameters.Add(new SqlParameter("@MenuId", SqlDbType.NVarChar, 50) { Value = model.MenuId });
//                        cmd.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = model.CreatedBy });
//                        cmd.Parameters.Add(new SqlParameter("@CreatedFrom", SqlDbType.NVarChar, 50) { Value = model.CreatedFrom });

//                        var result = cmd.ExecuteNonQuery();

//                        dbConn.Close();
//                        dbConn.Dispose();
//                        cmd.Dispose();

//                        if (result == -1)
//                        {
//                            throw new Exception(MessageModel.SubmissionFail);
//                        }
//                        else
//                        {
//                            results[0] = "Success";
//                            results[1] = MessageModel.SubmissionSuccess;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                results[0] = "Fail";
//                results[1] = ex.Message;
//                throw;
//            }

//            return results;
//        }

//        public static string[] Delete(string ProcName, string RoleId, string UserId)
//        {
//            string[] results = new string[2];
//            results[0] = "Fail";
//            results[1] = "Fail";

//            try
//            {
//                using (var dbConn = new SqlConnection(DatabaseHelper.GetConnectionString()))
//                {
//                    dbConn.Open();
//                    using (var cmd = new SqlCommand(ProcName, dbConn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;

//                        cmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.NVarChar, 50) { Value = RoleId });
//                        cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.NVarChar, 50) { Value = UserId });

//                        var result = cmd.ExecuteNonQuery();

//                        dbConn.Close();
//                        dbConn.Dispose();
//                        cmd.Dispose();

//                        if (result == -1)
//                        {
//                            throw new Exception(MessageModel.DeleteFail);
//                        }
//                        else
//                        {
//                            results[0] = "Success";
//                            results[1] = MessageModel.DeleteSuccess;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                results[0] = "Fail";
//                results[1] = ex.Message;
//                throw;
//            }

//            return results;
//        }

//        public static DataSet GetDashBoardData(string procName, string branchId, string sslConn = "")
//        {
//            try
//            {
//                using (var dbConn = new SqlConnection(DatabaseHelper.GetConnectionString()))
//                {
//                    dbConn.Open();
//                    using (var cmd = new SqlCommand(procName, dbConn))
//                    {
//                        cmd.CommandType = CommandType.StoredProcedure;
//                        cmd.CommandTimeout = 2000; // Set timeout to 2 minutes
//                        cmd.Parameters.Add(new SqlParameter("@BranchId", SqlDbType.NVarChar) { Value = branchId });

//                        using (var da = new SqlDataAdapter(cmd))
//                        {
//                            var ds = new DataSet();
//                            da.Fill(ds);
//                            dbConn.Close();
//                            return ds;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error executing stored procedure: {ex.Message}", ex);
//            }
//        }





//    }

//}
