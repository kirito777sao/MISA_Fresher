using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Infrastructure.Mappings
{
    /// <summary>
    /// Chứa các ánh xạ tên cột từ Entity (PascalCase) sang tên cột trong cơ sở dữ liệu (snake_case)
    /// </summary>
    /// Created by: HoanTD (10/12/2025)
    public class DbColumnMapping
    {
        /// <summary>
        /// Ánh xạ cột cho Entity WorkShift.
        /// Key: Tên thuộc tính trong C# Entity (PascalCase).
        /// Value: Tên cột tương ứng trong bảng work_shift của Database (snake_case).
        /// </summary>
        /// Created by: HoanTD (10/12/2025)
        public static readonly IDictionary<string, string> WorkShiftMapping =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "workShiftId", "work_shift_id" },
            { "workShiftCode", "work_shift_code" },
            { "workShiftName", "work_shift_name" },
            { "startTime", "start_time" },
            { "endTime", "end_time" },
            { "breakStart", "break_start" },
            { "breakEnd", "break_end" },
            { "workingHours", "working_hours" },
            { "breakHours", "break_hours" },
            { "workShiftStatus", "work_shift_status" },
            { "description", "description" },
            { "createdDate", "created_date" },
            { "createdBy", "created_by" },
            { "modifiedDate", "modified_date" },
            { "modifiedBy", "modified_by" },
        };
    }
}
