using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO chứa thông tin sắp xếp (Sort) cho một cột trong truy vấn dữ liệu.
    /// </summary>
    /// Created by: HoanTD (15/12/2025)
    public class ColumnSort
    {
        /// <summary>
        /// Tên trường (cột) dùng để sắp xếp (ví dụ: "work_shift_name", "created_date").
        /// </summary>
        public string? Selector { get; set; }

        /// <summary>
        /// Xác định hướng sắp xếp:
        /// <para>True: Giảm dần (Descending - DESC)</para>
        /// <para>False: Tăng dần (Ascending - ASC)</para>
        /// </summary>
        public bool Desc { get; set; }
    }
}
