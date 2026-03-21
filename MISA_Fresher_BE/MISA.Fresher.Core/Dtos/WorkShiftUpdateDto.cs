using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.MISAAttributes;

namespace MISA.Fresher.Core.Dtos
{
    /// <summary>
    /// DTO cập nhật thông tin ca làm việc.
    /// Không bao gồm các trường CreatedDate/CreatedBy.
    /// </summary>
    /// Created by: HoanTD (04/12/2025)
    public class WorkShiftUpdateDto
    {
        [MISARequired("Mã ca không được để trống")]
        [MISAColumnName("work_shift_code")]
        [MISACheckDuplicate("Mã ca không được trùng")]
        /// <summary>
        /// Mã ca làm việc (Bắt buộc, không trùng)
        /// </summary>
        public string? WorkShiftCode { get; set; }

        [MISARequired("Tên ca không được để trống")]
        [MISAColumnName("work_shift_name")]
        /// <summary>
        /// Tên ca làm việc (Bắt buộc)
        /// </summary>
        public string? WorkShiftName { get; set; }

        [MISARequired("Thời gian bắt đầu ca không được để trống")]
        [MISAColumnName("start_time")]
        /// <summary>
        /// Thời gian bắt đầu ca (Bắt buộc)
        /// </summary>
        public DateTime? StartTime { get; set; }

        [MISARequired("Thời gian kết thúc ca không được để trống")]
        [MISAColumnName("end_time")]
        /// <summary>
        /// Thời gian kết thúc ca (Bắt buộc)
        /// </summary>
        public DateTime? EndTime { get; set; }

        [MISAColumnName("break_start")]
        /// <summary>
        /// Thời gian bắt đầu nghỉ giữa ca (Không bắt buộc)
        /// </summary>
        public DateTime? BreakStart { get; set; }

        [MISAColumnName("break_end")]
        /// <summary>
        /// Thời gian kết thúc nghỉ giữa ca (Không bắt buộc)
        /// </summary>
        public DateTime? BreakEnd { get; set; }

        [MISAColumnName("working_hours")]
        /// <summary>
        /// Tổng số giờ làm việc (Được tính toán ở Backend)
        /// </summary>
        public decimal WorkingHours { get; set; }

        [MISAColumnName("break_hours")]
        /// <summary>
        /// Tổng số giờ nghỉ (Được tính toán ở Backend)
        /// </summary>
        public decimal BreakHours { get; set; }

        [MISAColumnName("work_shift_status")]
        /// <summary>
        /// Trạng thái ca làm việc (1: Hoạt động, 0: Ngừng hoạt động)
        /// </summary>
        public bool? WorkShiftStatus { get; set; }

        [MISAColumnName("modified_date")]
        /// <summary>
        /// Ngày sửa đổi cuối cùng (Tự động cập nhật)
        /// </summary>
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [MISAColumnName("modified_by")]
        /// <summary>
        /// Người sửa đổi cuối cùng
        /// </summary>
        public string? ModifiedBy { get; set; } = "HoanTD";

        [MISAColumnName("description")]
        /// <summary>
        /// Mô tả chi tiết về ca làm việc
        /// </summary>
        public string? Description { get; set; }
    }
}
