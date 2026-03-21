using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Fresher.Core.Dtos;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Interfaces.Repository;
using MISA.Fresher.Infrastructure.Mappings;
using MySqlConnector;

namespace MISA.Fresher.Infrastructure.Repositories
{
    /// <summary>
    /// Repository xử lý truy vấn dữ liệu WorkShift.
    /// </summary>
    /// Created by: HoanTD (10/12/2025)
    public class WorkShiftRepository(IConfiguration configuration) : BaseRepository<WorkShift>(configuration), IWorkShiftRepository
    {
        // Ghi đè connectionString từ BaseRepository
        // protected readonly string connectionString; 

        /// <summary>
        /// Lấy danh sách ca làm việc có hỗ trợ phân trang, tìm kiếm, lọc cột, sắp xếp
        /// </summary>
        /// <param name="request">Đối tượng payload WorkShiftFilterDto chứa các tham số phân trang, tìm kiếm, lọc, sắp xếp</param>
        /// <returns>Đối tượng PagedResult<WorkShift> chứa danh sách dữ liệu và tổng số bản ghi</returns>
        /// Created by: HoanTD (04/12/2025)
        public async Task<PagedResult<WorkShift>> GetPagingAsync(WorkShiftFilterDto request)
        {
            // quy định đầu vào phân trang
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;
            var offset = (page - 1) * pageSize;

            // khởi tạo câu lệnh where và param
            var whereClauses = new List<string>();
            var sortClauses = new List<string>();
            var dynamicParameters = new DynamicParameters();

            // tìm kiếm theo keyword
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                var searchColumns = new List<string> {
                    "work_shift_code",
                    "work_shift_name",
                    "created_by",
                    "modified_by"
                };
                var searchByConditions = searchColumns
                    .Select(col => $"{col} LIKE @keyword")
                    .ToArray();
                whereClauses.Add($"({string.Join(" OR ", searchByConditions)}) ");
                dynamicParameters.Add("@keyword", $"%{request.Keyword}%");
            }

            // lấy mapping cột
            var columnMapping = DbColumnMapping.WorkShiftMapping;

            // Tìm kiếm theo cột (FilterColumn)
            if (request.FilterColumn.Count > 0)
            {
                var paramIndex = 0;
                foreach (var filter in request.FilterColumn)
                {
                    if (filter == null || string.IsNullOrWhiteSpace(filter.ColumnName)) continue;
                    var paramName = $"@f{paramIndex}";

                    // Lấy tên cột DB từ mapping
                    if (columnMapping.TryGetValue(filter.ColumnName, out var dbColumnName))
                    {
                        // Xử lý các toán tử lọc
                        switch (filter.FilterOperator?.ToLower())
                        {
                            case "isnull":
                                whereClauses.Add($"{dbColumnName} IS NULL");
                                break;
                            case "notnull":
                                whereClauses.Add($"{dbColumnName} IS NOT NULL");
                                break;
                            case "=":
                                whereClauses.Add($"{dbColumnName} = {paramName}");
                                dynamicParameters.Add(paramName, filter.FilterValue);
                                break;
                            case "<>":
                                whereClauses.Add($"{dbColumnName} <> {paramName}");
                                dynamicParameters.Add(paramName, filter.FilterValue);
                                break;
                            case "contains":
                                whereClauses.Add($"{dbColumnName} LIKE {paramName}");
                                dynamicParameters.Add(paramName, $"%{filter.FilterValue}%");
                                break;
                            case "notcontains":
                                whereClauses.Add($"{dbColumnName} NOT LIKE {paramName}");
                                dynamicParameters.Add(paramName, $"%{filter.FilterValue}%");
                                break;
                            case "startswith":
                                whereClauses.Add($"{dbColumnName} LIKE {paramName}");
                                dynamicParameters.Add(paramName, $"{filter.FilterValue}%");
                                break;
                            case "endswith":
                                whereClauses.Add($"{dbColumnName} LIKE {paramName}");
                                dynamicParameters.Add(paramName, $"%{filter.FilterValue}");
                                break;
                            case "active":
                                whereClauses.Add($"{dbColumnName} = {paramName}");
                                dynamicParameters.Add(paramName, true);
                                break;
                            case "inactive":
                                whereClauses.Add($"{dbColumnName} = {paramName}");
                                dynamicParameters.Add(paramName, false);
                                break;
                            case "<":
                            case "lessthan":
                                whereClauses.Add($"{dbColumnName} < {paramName}");
                                dynamicParameters.Add(paramName, filter.FilterValue);
                                break;
                            case "<=":
                            case "lessthanequal":
                                whereClauses.Add($"{dbColumnName} <= {paramName}");
                                dynamicParameters.Add(paramName, filter.FilterValue);
                                break;
                            case ">":
                            case "greaterthan":
                                whereClauses.Add($"{dbColumnName} > {paramName}");
                                dynamicParameters.Add(paramName, filter.FilterValue);
                                break;
                            case ">=":
                            case "greaterthanequal":
                                whereClauses.Add($"{dbColumnName} >= {paramName}");
                                dynamicParameters.Add(paramName, filter.FilterValue);
                                break;
                            case null:
                                break;
                            default:
                                break;
                        }
                    }
                    paramIndex++;
                }
            }

            // Sắp xếp theo cột (SortColumn)
            if (request.SortColumn.Count > 0)
            {
                foreach (var sort in request.SortColumn)
                {
                    if (string.IsNullOrWhiteSpace(sort?.Selector)) continue;

                    // Lấy tên cột DB từ mapping
                    if (columnMapping.TryGetValue(sort.Selector, out var dbColumnName))
                    {
                        var direction = sort.Desc ? "DESC" : "ASC";
                        sortClauses.Add($"{dbColumnName} {direction}");
                    }
                }
            }

            // Mặc định sắp xếp theo mã nếu không có sắp xếp nào
            if (!sortClauses.Any())
            {
                sortClauses.Add("work_shift_code ASC");
            }

            var whereClause = whereClauses.Any() ? " WHERE " + string.Join(" AND ", whereClauses) : string.Empty;
            var sortClause = sortClauses.Any() ? " ORDER BY " + string.Join(", ", sortClauses) : string.Empty;

            // dataSql
            var dataSql = $@"
                SELECT
                  work_shift_id,
                  work_shift_code,
                  work_shift_name,
                  start_time,
                  end_time,
                  break_start,
                  break_end,
                  working_hours,
                  break_hours,
                  work_shift_status,
                  created_by,
                  created_date,
                  modified_by,
                  modified_date,
                  description
                FROM work_shift
                {whereClause}
                {sortClause}
                LIMIT @limit OFFSET @offset;";
            dynamicParameters.Add("@limit", pageSize);
            dynamicParameters.Add("@offset", offset);

            // Thực thi truy vấn và trả về kết quả phân trang
            using (var db = new MySqlConnection(connectionString))
            {
                var items = (await db.QueryAsync<WorkShift>(dataSql, dynamicParameters)).ToList();

                // Truy vấn tổng số bản ghi
                var countSql = $"SELECT COUNT(*) FROM work_shift {whereClause};";
                var total = await db.ExecuteScalarAsync<int>(countSql, dynamicParameters);

                var pagedResult = new PagedResult<WorkShift>
                {
                    Items = items,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = total,
                };

                return pagedResult;
            }
        }

        /// <summary>
        /// Cập nhật thông tin chi tiết ca làm việc bằng cách truyền DTO.
        /// </summary>
        /// <param name="id">ID của ca làm việc cần cập nhật</param>
        /// <param name="request">Đối tượng payload WorkShiftUpdateDto</param>
        /// <returns>Đối tượng WorkShift đã được cập nhật từ DB</returns>
        /// Created by: HoanTD (05/12/2025)
        public async Task<WorkShift> UpdateWorkShiftAsync(string id, WorkShiftUpdateDto request)
        {
            var sql = @"
            UPDATE work_shift SET
                work_shift_code = @WorkShiftCode,
                work_shift_name = @WorkShiftName,
                start_time = @StartTime,
                end_time = @EndTime,
                break_start = @BreakStart,
                break_end = @BreakEnd,
                working_hours = @WorkingHours,
                break_hours = @BreakHours,
                work_shift_status = @WorkShiftStatus,
                modified_by = @ModifiedBy,
                modified_date = NOW(),
                description = @Description
            WHERE work_shift_id = @WorkShiftId;";

            // Tạo anonymous object cho tham số Dapper
            var parameters = new
            {
                WorkShiftId = id,
                WorkShiftCode = request.WorkShiftCode,
                WorkShiftName = request.WorkShiftName,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                BreakStart = request.BreakStart,
                BreakEnd = request.BreakEnd,
                WorkingHours = request.WorkingHours,
                BreakHours = request.BreakHours,
                WorkShiftStatus = request.WorkShiftStatus,
                ModifiedBy = "HoanTD", // Hardcoded modified_by
                Description = request.Description,
            };

            using (var db = new MySqlConnection(connectionString))
            {
                var affected = await db.ExecuteAsync(sql, parameters);
                // Sau khi cập nhật, lấy lại Entity từ DB
                var updatedEntity = await GetByIdAsync(id);
                return updatedEntity!; // Dấu ! để khẳng định updatedEntity không null nếu affected > 0, hoặc có thể kiểm tra null rõ ràng hơn.
            }
        }

        /// <summary>
        /// Cập nhật trạng thái (work_shift_status) của nhiều ca làm việc cùng lúc.
        /// </summary>
        /// <param name="request">Đối tượng payload StatusUpdateDto chứa danh sách ID (ids) và trạng thái mới (newStatus)</param>
        /// <returns>Số bản ghi được cập nhật trạng thái</returns>
        /// Created by: HoanTD (10/12/2025)
        public async Task<int> UpdateMultipleStatusAsync(StatusUpdateDto request)
        {
            string sql = @"
            UPDATE work_shift 
            SET work_shift_status = @NewStatus
            WHERE work_shift_id IN @Ids;";

            // Dapper tự động xử lý mảng/List cho toán tử IN
            var parameters = new
            {
                NewStatus = request.newStatus,
                Ids = request.ids
            };

            using (var db = new MySqlConnection(connectionString))
            {
                int affectedRows = await db.ExecuteAsync(sql, parameters);
                return affectedRows;
            }
        }
    }
}
