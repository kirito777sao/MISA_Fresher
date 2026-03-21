using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.Dtos;
using MISA.Fresher.Core.Entities;

namespace MISA.Fresher.Core.Interfaces.Repository
{
    public interface IWorkShiftRepository : IBaseRepository<WorkShift>
    {
        /// <summary>
        /// Lấy danh sách ca làm việc có hỗ trợ phân trang, tìm kiếm, lọc cột, sắp xếp
        /// </summary>
        /// <param name="request">Đối tượng payload</param>
        /// <returns>Danh sách ca làm việc có hỗ trợ phân trang, tìm kiếm, lọc cột, sắp xếp</returns>
        /// Created by: HoanTD (04/12/2025)
        Task<PagedResult<WorkShift>> GetPagingAsync(WorkShiftFilterDto request);

        /// <summary>
        /// Cập nhật thông tin ca làm việc 
        /// </summary>
        /// <param name="id">ID của ca làm việc cần cập nhật</param>
        /// <param name="request">Đối tượng payload</param>
        /// <returns></returns>
        /// Created by: HoanTD (05/12/2025)
        Task<WorkShift> UpdateWorkShiftAsync(string id, WorkShiftUpdateDto request);

        /// <summary>
        /// Cập nhật trạng thái ca làm việc
        /// </summary>
        /// <param name="request">Đối tượng payload</param>
        /// <returns>Số bản ghi được cập nhật trạng thái</returns>
        /// <exception cref="MISAValidateException"></exception>
        /// Created by: HoanTD (10/12/2025)
        Task<int> UpdateMultipleStatusAsync(StatusUpdateDto request);
    }
}
