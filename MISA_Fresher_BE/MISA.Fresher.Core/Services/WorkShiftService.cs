using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.Dtos;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Repository;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.MISAAttributes;

namespace MISA.Fresher.Core.Services
{
    /// <summary>
    /// Service xử lý nghiệp vụ liên quan đến ca làm việc
    /// </summary>
    /// Created by: HoanTD (06/12/2025)
    public class WorkShiftService(IWorkShiftRepository workShiftRepository) : BaseService<WorkShift>(workShiftRepository), IWorkShiftService
    {
        private readonly IWorkShiftRepository _workShiftRepository = workShiftRepository;

        /// <summary>
        /// Lấy danh sách ca làm việc có hỗ trợ phân trang, tìm kiếm, lọc cột, sắp xếp.
        /// Sử dụng DTO WorkShiftFilterDto làm tham số đầu vào.
        /// </summary>
        /// <param name="request">Đối tượng payload chứa thông tin phân trang, lọc, tìm kiếm, sắp xếp</param>
        /// <returns>Đối tượng PagedResult<WorkShift> chứa danh sách và thông tin phân trang</returns>
        /// Created by: HoanTD (04/12/2025)
        public async Task<PagedResult<WorkShift>> GetPagingAsync(WorkShiftFilterDto request)
        {
            var pagedResult = await _workShiftRepository.GetPagingAsync(request);
            return pagedResult;
        }

        /// <summary>
        /// Tạo mới ca làm việc.
        /// Thực hiện validation dữ liệu DTO và kiểm tra logic nghiệp vụ giờ nghỉ.
        /// </summary>
        /// <param name="request">Đối tượng payload WorkShiftCreateDto</param>
        /// <returns>Ca làm việc vừa tạo (WorkShift entity)</returns>
        /// <exception cref="MISAValidateException">Ném ra khi có lỗi validation hoặc logic nghiệp vụ (ví dụ: giờ nghỉ nằm ngoài giờ làm)</exception>
        /// Created by: HoanTD (05/12/2025)
        public async Task<WorkShift> CreateWorkShiftAsync(WorkShiftCreateDto request)
        {
            // 1. Validate các trường bắt buộc, trùng lặp
            await ValidateShiftCreateDtoAsync(request);

            // 2. Validate logic nghiệp vụ: Giờ nghỉ phải nằm trong khoảng giờ làm
            if (request.BreakStart.HasValue && request.BreakEnd.HasValue)
            {
                if (request.BreakStart < request.StartTime || request.BreakStart > request.EndTime)
                {
                    throw new MISAValidateException($"Thời gian bắt đầu nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca");
                }
                if (request.BreakEnd > request.EndTime || request.BreakEnd < request.StartTime)
                {
                    throw new MISAValidateException($"Thời gian kết thúc nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca");
                }
            }


            // 3. Map DTO sang Entity và thêm vào DB
            var workShift = MapToWorkShift(request);

            var createdWorkShift = await _workShiftRepository.CreateAsync(workShift);

            return createdWorkShift;
        }

        /// <summary>
        /// Cập nhật thông tin ca làm việc.
        /// </summary>
        /// <param name="id">ID của ca làm việc cần cập nhật (GUID/UUID)</param>
        /// <param name="request">Đối tượng payload WorkShiftUpdateDto</param>
        /// <returns>Ca làm việc đã được cập nhật (WorkShift entity)</returns>
        /// <exception cref="MISANotFoundException">Ném ra khi không tìm thấy ca làm việc với ID đã cho</exception>
        /// <exception cref="MISAValidateException">Ném ra khi có lỗi validation hoặc logic nghiệp vụ</exception>
        /// Created by: HoanTD (05/12/2025)
        public async Task<WorkShift> UpdateWorkShiftAsync(string id, WorkShiftUpdateDto request)
        {
            // 1. Kiểm tra sự tồn tại
            var workShiftInDb = await _workShiftRepository.GetByIdAsync(id);
            if (workShiftInDb == null)
            {
                throw new MISANotFoundException("Ca làm việc không tồn tại trong hệ thống.");
            }

            // 2. Validate các trường bắt buộc, trùng lặp (loại trừ ID của chính nó)
            await ValidateShiftUpdateDtoAsync(request, workShiftInDb.WorkShiftCode);

            // 3. Validate logic nghiệp vụ: Giờ nghỉ phải nằm trong khoảng giờ làm
            if (request.BreakStart.HasValue && request.BreakEnd.HasValue)
            {
                if (request.BreakStart < request.StartTime || request.BreakStart > request.EndTime)
                {
                    throw new MISAValidateException($"Thời gian bắt đầu nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca");
                }
                if (request.BreakEnd > request.EndTime || request.BreakEnd < request.StartTime)
                {
                    throw new MISAValidateException($"Thời gian kết thúc nghỉ giữa ca phải nằm trong khoảng thời gian tính từ giờ vào ca đến giờ hết ca");
                }
            }


            // 4. Cập nhật vào DB
            var updatedEntity = await _workShiftRepository.UpdateWorkShiftAsync(id, request);

            return updatedEntity;
        }

        /// <summary>
        /// Validate thông tin ca làm việc từ DTO (Create/Insert)
        /// Áp dụng kiểm tra các Attribute: MISARequired và MISACheckDuplicate
        /// </summary>
        /// <param name="request">DTO chứa thông tin ca làm việc cần validate</param>
        /// <param name="excludeId">ID cần loại trừ khi kiểm tra trùng lặp (không dùng trong Create)</param>
        /// <returns>True nếu hợp lệ, nếu không sẽ ném exception</returns>
        /// <exception cref="MISAValidateException">Ném ra khi có trường required bị bỏ trống</exception>
        /// <exception cref="MISADuplicateException">Ném ra khi có trường bị trùng lặp</exception>
        /// Created by: HoanTD (05/12/2025)
        private async Task<bool> ValidateShiftCreateDtoAsync(WorkShiftCreateDto request, string? excludeId = null)
        {
            var properties = typeof(WorkShiftCreateDto).GetProperties();
            foreach (var prop in properties)
            {
                // Kiểm tra MISARequired
                var requiredAttr = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (requiredAttr.Length > 0)
                {
                    var value = prop.GetValue(request);
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        // Lấy thông báo lỗi từ Attribute (nếu có)
                        var requiredMessage = ((MISARequired)requiredAttr.First()).ErrorMessage;
                        throw new MISAValidateException(requiredMessage ?? $"{prop.Name} không được để trống.");
                    }
                }
            }

            foreach (var prop in properties)
            {
                // Kiểm tra MISACheckDuplicate
                var duplicateAttr = prop.GetCustomAttribute<MISACheckDuplicate>();
                if (duplicateAttr != null)
                {
                    var value = prop.GetValue(request);
                    if (value != null)
                    {
                        var columnAttr = prop.GetCustomAttribute<MISAColumnName>();
                        var columnName = columnAttr != null ? columnAttr.ColumnName : prop.Name.ToLower();

                        var isDuplicate = await _workShiftRepository.CheckDuplicateAsync(columnName, value, excludeId);
                        if (isDuplicate)
                        {
                            // Lấy thông báo lỗi từ Attribute (nếu có)
                            var duplicateMessage = duplicateAttr.ErrorMessage;
                            throw new MISADuplicateException(duplicateMessage ?? "Không được phép trùng lặp giá trị.", prop.Name, value.ToString() ?? "");
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Chuyển đổi từ WorkShiftCreateDto sang WorkShift entity
        /// </summary>
        /// <param name="request">DTO chứa thông tin ca làm việc</param>
        /// <returns>WorkShift entity đã được khởi tạo</returns>
        /// Created by: HoanTD (05/12/2025)
        private static WorkShift MapToWorkShift(WorkShiftCreateDto request)
        {
            var workShift = new WorkShift
            {
                WorkShiftCode = request.WorkShiftCode,
                WorkShiftName = request.WorkShiftName,
                StartTime = request.StartTime ?? DateTime.MinValue,
                EndTime = request.EndTime ?? DateTime.MinValue,
                BreakStart = request.BreakStart,
                BreakEnd = request.BreakEnd,
                WorkingHours = request.WorkingHours,
                BreakHours = request.BreakHours,
                WorkShiftStatus = request.WorkShiftStatus ?? true,
                CreatedDate = request.CreatedDate,
                CreatedBy = request.CreatedBy,
                ModifiedDate = request.ModifiedDate,
                ModifiedBy = request.ModifiedBy,
                Description = request.Description
            };
            return workShift;
        }

        /// <summary>
        /// Validate thông tin ca làm việc từ DTO (Update)
        /// Áp dụng kiểm tra các Attribute: MISARequired và MISACheckDuplicate
        /// </summary>
        /// <param name="request">DTO chứa thông tin ca làm việc cần validate</param>
        /// <param name="excludeId">Giá trị mã cần loại trừ khi kiểm tra trùng lặp (dùng khi update)</param>
        /// <returns>True nếu hợp lệ, nếu không sẽ ném exception</returns>
        /// <exception cref="MISAValidateException">Ném ra khi có trường required bị bỏ trống</exception>
        /// <exception cref="MISADuplicateException">Ném ra khi có trường bị trùng lặp</exception>
        /// Created by: HoanTD (05/12/2025)
        private async Task<bool> ValidateShiftUpdateDtoAsync(WorkShiftUpdateDto request, string? excludeId = null)
        {
            var properties = typeof(WorkShiftUpdateDto).GetProperties();
            foreach (var prop in properties)
            {
                // Kiểm tra MISARequired
                var requiredAttr = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (requiredAttr.Length > 0)
                {
                    var value = prop.GetValue(request);
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        var requiredMessage = ((MISARequired)requiredAttr.First()).ErrorMessage;
                        throw new MISAValidateException(requiredMessage ?? $"{prop.Name} không được để trống.");
                    }
                }
            }

            foreach (var prop in properties)
            {
                // Kiểm tra MISACheckDuplicate
                var duplicateAttr = prop.GetCustomAttribute<MISACheckDuplicate>();
                if (duplicateAttr != null)
                {
                    var value = prop.GetValue(request);
                    if (value != null)
                    {
                        var columnAttr = prop.GetCustomAttribute<MISAColumnName>();
                        var columnName = columnAttr != null ? columnAttr.ColumnName : prop.Name.ToLower();

                        // Truyền excludeId vào CheckDuplicateAsync
                        var isDuplicate = await _workShiftRepository.CheckDuplicateAsync(columnName, value, excludeId);
                        if (isDuplicate)
                        {
                            var duplicateMessage = duplicateAttr.ErrorMessage;
                            throw new MISADuplicateException(duplicateMessage ?? "Không được phép trùng lặp giá trị.", prop.Name, value.ToString() ?? "");
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Cập nhật trạng thái ca làm việc hàng loạt.
        /// Thường dùng để bật/tắt (active/inactive) ca làm việc.
        /// </summary>
        /// <param name="request">Đối tượng payload StatusUpdateDto chứa danh sách ID và trạng thái mới</param>
        /// <returns>Số bản ghi được cập nhật trạng thái</returns>
        /// <exception cref="MISAValidateException">Ném ra khi danh sách ID trống</exception>
        /// Created by: HoanTD (10/12/2025)
        public async Task<int> UpdateMultipleStatusAsync(StatusUpdateDto request)
        {
            if (request.ids.Count == 0)
            {
                throw new MISAValidateException("Danh sách ID không được để trống");
            }
            int affectedRows = await _workShiftRepository.UpdateMultipleStatusAsync(request);
            return affectedRows;
        }
    }
}
