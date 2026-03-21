using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO chứa thông tin điều kiện lọc cho một cột
    /// </summary>
    /// Created by: HoanTD (15/12/2025)
    public class ColumnFilter
    {
        /// <summary>
        /// Tên cột cần lọc (Mapping với tên property trong Entity)
        /// </summary>
        public string? ColumnName { get; set; }

        /// <summary>
        /// Toán tử lọc (ví dụ: =, LIKE, >, <, CONTAIN, ...)
        /// </summary>
        public string? FilterOperator { get; set; }

        /// <summary>
        /// Giá trị dùng để lọc
        /// </summary>
        public object? FilterValue { get; set; }
    }
}
