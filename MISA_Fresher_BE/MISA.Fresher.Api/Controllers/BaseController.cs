using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Core.Interfaces.Service;

namespace MISA.Fresher.Api.Controllers
{
    /// <summary>
    /// Controller cơ sở cung cấp các API CRUD chung (Generic CRUD API Endpoints).
    /// </summary>
    /// <typeparam name="T">Kiểu entity mà Controller này quản lý</typeparam>
    /// Created by: HoanTD (04/12/2025)
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T>(IBaseService<T> baseService) : ControllerBase where T : class
    {
        /// <summary>
        /// Service cơ sở để thực hiện các nghiệp vụ CRUD.
        /// </summary>
        protected readonly IBaseService<T> _baseService = baseService;

        /// <summary>
        /// Lấy tất cả bản ghi với khả năng tìm kiếm (GET api/[controller]).
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm (truyền qua Query String)</param>
        /// <returns>HTTP 200 OK cùng danh sách các bản ghi</returns>
        /// Created by: HoanTD (04/12/2025)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword = null)
        {
            var entities = await _baseService.GetAllAsync(keyword);
            return Ok(entities);
        }

        /// <summary>
        /// Lấy bản ghi theo ID (GET api/[controller]/{id}).
        /// </summary>
        /// <param name="id">ID của bản ghi</param>
        /// <returns>HTTP 200 OK cùng thông tin bản ghi, hoặc HTTP 404 Not Found nếu không tìm thấy</returns>
        /// Created by: HoanTD (04/12/2025)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var entity = await _baseService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        /// <summary>
        /// Tạo mới một bản ghi (POST api/[controller]).
        /// </summary>
        /// <param name="entity">Thông tin bản ghi cần tạo (truyền qua Body)</param>
        /// <returns>HTTP 200 OK cùng bản ghi đã được tạo</returns>
        /// Created by: HoanTD (04/12/2025)
        [HttpPost]
        public async Task<IActionResult> Create(T entity)
        {
            var createdEntity = await _baseService.CreateAsync(entity);
            return Ok(createdEntity);
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi theo ID (PUT api/[controller]/{id}).
        /// </summary>
        /// <param name="id">ID của bản ghi cần cập nhật (truyền qua Route)</param>
        /// <param name="entity">Thông tin bản ghi mới (truyền qua Body)</param>
        /// <returns>HTTP 200 OK cùng số bản ghi bị ảnh hưởng hoặc bản ghi đã được cập nhật</returns>
        /// Created by: HoanTD (04/12/2025)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, T entity)
        {
            var updatedEntity = await _baseService.UpdateAsync(id, entity);
            return Ok(updatedEntity);
        }

        /// <summary>
        /// Xóa bản ghi theo ID (DELETE api/[controller]/{id}).
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        /// <returns>HTTP 204 No Content nếu xóa thành công</returns>
        /// Created by: HoanTD (04/12/2025)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _baseService.DeleteAsync(id);
            return NoContent();
        }
    }
}
