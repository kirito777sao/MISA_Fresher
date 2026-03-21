using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO dùng để truyền danh sách ID (GUID/UUID) phục vụ cho thao tác xóa hàng loạt
    /// </summary>
    /// Created by: HoanTD (15/12/2025)
    public class WorkShiftDeleteDto
    {
        /// <summary>
        /// Danh sách ID của các bản ghi ca làm việc cần xóa
        /// </summary>
        public HashSet<Guid> workShiftIds { get; set; } = new HashSet<Guid>();
    }
}
