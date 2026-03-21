using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MISA.Fresher.Core.Exceptions;
using MISA.Fresher.Core.Interfaces.Repository;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.MISAAttributes;

namespace MISA.Fresher.Core.Services
{
    /// <summary>
    /// Service cơ sở cung cấp các chức năng CRUD chung cho hầu hết các Entity.
    /// Bao gồm các nghiệp vụ kiểm tra chung như Validate Required và Duplicate.
    /// </summary>
    /// <typeparam name="T">Kiểu entity (phải là reference type: class)</typeparam>
    /// Created by: HoanTD (10/12/2025)
    public class BaseService<T>(IBaseRepository<T> baseRepository) : IBaseService<T> where T : class
    {
        /// <summary>
        /// Tạo mới một bản ghi sau khi thực hiện validation các nghiệp vụ chung.
        /// </summary>
        /// <param name="entity">Đối tượng cần tạo (Entity T)</param>
        /// <returns>Đối tượng đã được tạo (Entity T)</returns>
        /// <exception cref="MISAValidateException">Ném ra khi dữ liệu không hợp lệ</exception>
        /// Created by: HoanTD (05/12/2025)
        public async Task<T> CreateAsync(T entity)
        {
            await ValidateEntityAsync(entity);
            return await baseRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Xóa bản ghi theo ID (Thường là Xóa Cứng/Hard Delete).
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa (GUID/UUID)</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Created by: HoanTD (05/12/2025)
        public Task<int> DeleteAsync(string id)
        {
            return baseRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Xóa nhiều bản ghi theo danh sách ID (Thường là Xóa Cứng/Hard Delete).
        /// </summary>
        /// <param name="ids">Danh sách ID (GUID/UUID) cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// <exception cref="MISAValidateException">Ném ra khi danh sách ID trống</exception>
        /// Created by: HoanTD (10/12/2025)
        public async Task<int> DeleteMultipleAsync(List<string> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                throw new MISAValidateException("Danh sách ID không được để trống");
            }

            return await baseRepository.DeleteMultipleAsync(ids);
        }

        /// <summary>
        /// Lấy tất cả bản ghi với khả năng tìm kiếm theo từ khóa.
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm (có thể null)</param>
        /// <returns>Danh sách các bản ghi</returns>
        /// Created by: HoanTD (05/12/2025)
        public Task<IEnumerable<T>> GetAllAsync(string? keyword)
        {
            return baseRepository.GetAllAsync(keyword);
        }

        /// <summary>
        /// Lấy bản ghi theo ID.
        /// </summary>
        /// <param name="id">ID của bản ghi (GUID/UUID)</param>
        /// <returns>Bản ghi tìm được hoặc null nếu không tồn tại</returns>
        /// Created by: HoanTD (05/12/2025)
        public Task<T?> GetByIdAsync(string id)
        {
            return baseRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Cập nhật thông tin bản ghi sau khi thực hiện validation các nghiệp vụ chung.
        /// </summary>
        /// <param name="id">ID của bản ghi cần cập nhật</param>
        /// <param name="entity">Đối tượng chứa thông tin mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// <exception cref="MISAValidateException">Ném ra khi dữ liệu không hợp lệ</exception>
        /// Created by: HoanTD (10/12/2025)
        public async Task<int> UpdateAsync(string id, T entity)
        {
            await ValidateEntityAsync(entity, id);
            return await baseRepository.UpdateAsync(id, entity);
        }

        /// <summary>
        /// Validate thông tin entity (kiểm tra các Attribute: MISARequired và MISACheckDuplicate)
        /// Sử dụng Reflection để đọc các thuộc tính custom trên Entity T.
        /// </summary>
        /// <param name="entity">Đối tượng cần validate</param>
        /// <param name="excludeId">ID cần loại trừ khi kiểm tra trùng lặp (dùng khi update)</param>
        /// <returns>True nếu hợp lệ, nếu không sẽ ném exception</returns>
        /// <exception cref="MISAValidateException">Ném ra khi có trường required bị bỏ trống</exception>
        /// <exception cref="MISADuplicateException">Ném ra khi có trường bị trùng lặp</exception>
        /// Created by: HoanTD (05/12/2025)
        public async Task<bool> ValidateEntityAsync(T entity, string? excludeId = null)
        {
            var properties = typeof(T).GetProperties();

            // 1. Kiểm tra MISARequired
            foreach (var prop in properties)
            {
                var requiredAttr = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (requiredAttr.Length > 0)
                {
                    var value = prop.GetValue(entity);
                    if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                    {
                        var requiredMessage = ((MISARequired)requiredAttr.First()).ErrorMessage;
                        throw new MISAValidateException(requiredMessage ?? $"{prop.Name} không được để trống.");
                    }
                }
            }

            // 2. Kiểm tra MISACheckDuplicate
            foreach (var prop in properties)
            {
                var duplicateAttr = prop.GetCustomAttribute<MISACheckDuplicate>();
                if (duplicateAttr != null)
                {
                    var value = prop.GetValue(entity);
                    if (value != null)
                    {
                        var columnAttr = prop.GetCustomAttribute<MISAColumnName>();
                        var columnName = columnAttr != null ? columnAttr.ColumnName : prop.Name.ToLower();

                        var isDuplicate = await baseRepository.CheckDuplicateAsync(columnName, value, excludeId);
                        if (isDuplicate)
                        {
                            var duplicateMessage = duplicateAttr.ErrorMessage;
                            throw new MISADuplicateException(duplicateMessage ?? "Không được phép trùng lặp giá trị.", prop.Name, value.ToString() ?? "");
                        }
                    }
                }
            }

            return true;
        }
    }
}
