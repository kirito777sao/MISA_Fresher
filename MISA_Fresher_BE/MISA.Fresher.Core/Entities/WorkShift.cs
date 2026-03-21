using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.MISAAttributes;

namespace MISA.Fresher.Core.Entities
{
    [MISATableName("work_shift")]
    public class WorkShift
    {
        [MISAKey]
        [MISAColumnName("work_shift_id")]
        public Guid? WorkShiftId { get; set; } = Guid.NewGuid();

        [MISARequired("Không được để trống")]
        [MISAColumnName("work_shift_code")]
        [MISACheckDuplicate("Mã ca không được trùng")]
        public string WorkShiftCode { get; set; } = string.Empty;

        [MISARequired("Không được để trống")]
        [MISAColumnName("work_shift_name")]
        public string WorkShiftName { get; set; } = string.Empty;

        [MISAColumnName("start_time")]
        public DateTime StartTime { get; set; }

        [MISAColumnName("end_time")]
        public DateTime EndTime { get; set; }

        [MISAColumnName("break_start")]
        public DateTime? BreakStart { get; set; }

        [MISAColumnName("break_end")]
        public DateTime? BreakEnd { get; set; }

        [MISAColumnName("working_hours")]
        public decimal WorkingHours { get; set; }

        [MISAColumnName("break_hours")]
        public decimal BreakHours { get; set; }

        [MISAColumnName("work_shift_status")]
        public bool WorkShiftStatus { get; set; }

        [MISAColumnName("created_date")]
        public DateTime CreatedDate { get; set; }

        [MISAColumnName("created_by")]
        public string? CreatedBy { get; set; }

        [MISAColumnName("modified_date")]
        public DateTime ModifiedDate { get; set; }

        [MISAColumnName("modified_by")]
        public string? ModifiedBy { get; set; }

        [MISAColumnName("description")]
        public string? Description { get; set; }
    }
}
