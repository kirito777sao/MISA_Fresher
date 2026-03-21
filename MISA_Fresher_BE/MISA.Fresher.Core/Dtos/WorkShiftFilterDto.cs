using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO chứa tất cả các tham số dùng để lọc, tìm kiếm, sắp xếp và phân trang danh sách ca làm việc.
    /// </summary>
    /// Created by: HoanTD (15/12/2025)
    public class WorkShiftFilterDto
    {
        /// <summary>
        /// Số trang cần lấy (Mặc định là trang 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Kích thước trang (Số lượng bản ghi trên 1 trang, mặc định là 20)
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Từ khóa tìm kiếm chung (thường áp dụng cho Mã hoặc Tên)
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// Danh sách các cột cần sắp xếp và hướng sắp xếp (ASC/DESC)
        /// </summary>
        public List<ColumnSort> SortColumn { get; set; } = new List<ColumnSort>();

        /// <summary>
        /// Danh sách các điều kiện lọc chi tiết theo từng cột
        /// </summary>
        public List<ColumnFilter> FilterColumn { get; set; } = new List<ColumnFilter>();
    }
}
