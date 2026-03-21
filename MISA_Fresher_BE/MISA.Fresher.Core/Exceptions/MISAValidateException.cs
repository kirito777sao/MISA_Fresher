using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.Exceptions
{
    /// <summary>
    /// Exception được throw khi dữ liệu không hợp lệ (validation failed)
    /// </summary>
    /// Created by: HoanTD (03/12/2025)
    public class MISAValidateException : Exception
    {
        /// <summary>
        /// Khởi tạo exception với thông báo lỗi
        /// </summary>
        /// <param name="message">Thông báo lỗi validation</param>
        /// Created by: HoanTD (03/12/2025)
        public MISAValidateException(string message) : base(message)
        {
        }

        /// <summary>
        /// Khởi tạo exception với thông báo lỗi và exception gốc
        /// </summary>
        /// <param name="message">Thông báo lỗi validation</param>
        /// <param name="innerException">Exception gốc</param>
        /// Created by: HoanTD (03/12/2025)
        public MISAValidateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
