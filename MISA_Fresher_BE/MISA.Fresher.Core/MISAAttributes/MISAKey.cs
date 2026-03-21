using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Core.MISAAttributes
{
    /// <summary>
    /// Attribute đánh dấu property là khóa chính (Primary Key)
    /// </summary>
    /// Created by: HoanTD (10/12/2025)
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAKey : Attribute
    {
    }
}
