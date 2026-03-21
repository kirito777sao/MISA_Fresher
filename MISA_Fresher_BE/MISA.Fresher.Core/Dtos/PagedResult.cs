using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO chứa kết quả trả về khi thực hiện phân trang (Paging)
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của các bản ghi</typeparam>
    /// Created by: HoanTD (15/12/2025)
    public class PagedResult<T>
    {
        /// <summary>
        /// Danh sách các bản ghi được trả về trong trang hiện tại
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// Số trang hiện tại đang được trả về (Bắt đầu từ 1)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Kích thước trang (Số lượng bản ghi trên một trang)
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Tổng số lượng bản ghi (trên tất cả các trang)
        /// </summary>
        public int TotalCount { get; set; }
    }
}
