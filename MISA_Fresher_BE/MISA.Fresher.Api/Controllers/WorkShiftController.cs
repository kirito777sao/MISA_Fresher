using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Dtos;
using MISA.Fresher.Core.Entities;
using MISA.Fresher.Core.Enums;
using MISA.Fresher.Core.Interfaces.Service;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MISA.Fresher.Api.Controllers
{
    /// <summary>
    /// Controller quản lý các thao tác liên quan đến Ca làm việc (WorkShift).
    /// </summary>
    /// Created by: HoanTD (10/12/2025)
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WorkShiftController(IWorkShiftService workShiftService) : ControllerBase
    {
        private readonly IWorkShiftService _workShiftService = workShiftService;

        /// <summary>
        /// Lấy danh sách ca làm việc có hỗ trợ phân trang, tìm kiếm, lọc cột, sắp xếp (POST api/v1/workshift/paging).
        /// </summary>
        /// <param name="request">Đối tượng payload WorkShiftFilterDto chứa các tham số phân trang, tìm kiếm, lọc, sắp xếp</param>
        /// <returns>HTTP 200 OK cùng đối tượng ApiResponse chứa PagedResult<WorkShift></returns>
        /// Created by: HoanTD (04/12/2025)
        [HttpPost("paging")]
        public async Task<IActionResult> GetPaging([FromBody] WorkShiftFilterDto request)
        {
            var pagedResult = await _workShiftService.GetPagingAsync(request);

            var response = ApiResponse<PagedResult<WorkShift>>.SuccessResponse(
                userMessage: "Lấy danh sách ca làm việc thành công",
                code: HttpStatusCode.OK,
                data: pagedResult
            );
            return Ok(response);
        }

        /// <summary>
        /// Tạo mới ca làm việc (POST api/v1/workshift).
        /// </summary>
        /// <param name="request">Đối tượng payload WorkShiftCreateDto chứa thông tin ca làm việc cần tạo</param>
        /// <returns>HTTP 200 OK cùng đối tượng ApiResponse chứa WorkShift vừa tạo</returns>
        /// Created by: HoanTD (05/12/2025)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkShiftCreateDto request)
        {
            var createdWorkShift = await _workShiftService.CreateWorkShiftAsync(request);

            var response = ApiResponse<WorkShift>.SuccessResponse(
                userMessage: "Thêm mới ca làm việc thành công",
                code: HttpStatusCode.OK,
                data: createdWorkShift
            );
            return Ok(response);
        }

        /// <summary>
        /// Lấy ca làm việc theo Id (GET api/v1/workshift/{id}).
        /// </summary>
        /// <param name="id">Id ca làm việc</param>
        /// <returns>HTTP 200 OK cùng đối tượng ApiResponse chứa WorkShift hoặc thông báo không tìm thấy</returns>
        /// Created by: HoanTD (05/12/2025)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var entity = await _workShiftService.GetByIdAsync(id);
            var response = new ApiResponse<WorkShift>
            {
                Code = HttpStatusCode.OK,
                Success = entity != null,
                Data = entity,
                UserMessage = entity != null
                ? "Lấy thông tin ca thành công"
                : "Không tìm thấy ca"
            };
            return Ok(response);
        }

        /// <summary>
        /// Cập nhật thông tin ca làm việc (PUT api/v1/workshift/{id}).
        /// </summary>
        /// <param name="id">ID của ca làm việc cần cập nhật (truyền qua Route)</param>
        /// <param name="request">Đối tượng payload WorkShiftUpdateDto chứa thông tin mới</param>
        /// <returns>HTTP 200 OK cùng đối tượng ApiResponse chứa WorkShift đã được cập nhật</returns>
        /// Created by: HoanTD (05/12/2025)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] WorkShiftUpdateDto request)
        {
            var updatedEntity = await _workShiftService.UpdateWorkShiftAsync(id, request);

            var response = ApiResponse<WorkShift>.SuccessResponse(
                userMessage: "Cập nhật ca làm việc thành công",
                code: HttpStatusCode.OK,
                data: updatedEntity
            );

            return Ok(response);
        }

        /// <summary>
        /// Xóa nhiều ca làm việc theo danh sách ID (POST api/v1/workshift/delete).
        /// </summary>
        /// <param name="ids">Danh sách ID của các ca làm việc cần xóa (truyền qua Body)</param>
        /// <returns>HTTP 200 OK cùng đối tượng ApiResponse chứa số lượng bản ghi bị xóa</returns>
        /// Created by: HoanTD (05/12/2025)
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<string> ids)
        {
            int affectedRows = await _workShiftService.DeleteMultipleAsync(ids);

            var response = ApiResponse<int>.SuccessResponse(
                userMessage: "Xóa ca làm việc thành công",
                code: HttpStatusCode.OK,
                data: affectedRows
            );

            return Ok(response);
        }

        /// <summary>
        /// Cập nhật trạng thái (status) của nhiều ca làm việc (POST api/v1/workshift/update_status).
        /// </summary>
        /// <param name="request">Đối tượng StatusUpdateDto chứa danh sách ID và trạng thái mới</param>
        /// <returns>HTTP 200 OK cùng đối tượng ApiResponse chứa số lượng bản ghi được cập nhật trạng thái</returns>
        /// Created by: HoanTD (10/12/2025)
        [HttpPost("update_status")]
        public async Task<IActionResult> UpdateMultipleStatus([FromBody] StatusUpdateDto request)
        {
            int affectedRows = await _workShiftService.UpdateMultipleStatusAsync(request);

            var response = ApiResponse<int>.SuccessResponse(
                userMessage: "Cập nhật trạng thái ca làm việc thành công",
                code: HttpStatusCode.OK,
                data: affectedRows
            );

            return Ok(response);
        }
    }
}
