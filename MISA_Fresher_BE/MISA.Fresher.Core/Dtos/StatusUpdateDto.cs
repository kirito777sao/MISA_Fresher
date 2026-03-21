using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO dùng để cập nhật trạng thái (ví dụ: work_shift_status) cho nhiều bản ghi
    /// </summary>
    /// Created by: HoanTD (15/12/2025)
    public class StatusUpdateDto
    {
        /// <summary>
        /// Danh sách ID (GUID/UUID) của các bản ghi cần cập nhật trạng thái
        /// </summary>
        public HashSet<Guid> ids { get; set; } = new HashSet<Guid>();

        /// <summary>
        /// Trạng thái mới (True/False hoặc 1/0) áp dụng cho các bản ghi
        /// </summary>
        public bool newStatus { get; set; }
    }
}
