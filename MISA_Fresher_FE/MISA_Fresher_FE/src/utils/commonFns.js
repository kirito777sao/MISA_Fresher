/**
 * File: utility.js (hoặc tương tự)
 * Hàm: getLabelByValue
 * Mô tả: Tìm kiếm label (nhãn hiển thị) từ một mảng cấu trúc (options) dựa trên giá trị (value).
 */

/**
 * Lấy label tương ứng với value từ một mảng options.
 * * @param {Array<Object>} options - Mảng các đối tượng options (ví dụ: [{ id: 1, name: 'Active' }, ...]).
 * @param {string|number} value - Giá trị cần tìm label.
 * @param {string} [keyField='value'] - Tên trường chứa giá trị (value) để so sánh.
 * @param {string} [labelField='label'] - Tên trường chứa nhãn (label) cần trả về.
 * @returns {string} Label tương ứng, hoặc chuỗi trống nếu không tìm thấy hoặc input không hợp lệ.
 * Created by: HoanTD (11/12/2025)
 */
export function getLabelByValue(options, value, keyField = 'value', labelField = 'label') {

    // 1. Kiểm tra đầu vào hợp lệ (options phải là một mảng)
    if (!options || !Array.isArray(options)) {
        return '';
    }

    // 2. Sử dụng Array.prototype.find() để tìm đối tượng đầu tiên khớp điều kiện
    const option = options.find(item => item[keyField] === value);

    // 3. Trả về kết quả
    // Nếu tìm thấy đối tượng (option), trả về giá trị của trường labelField
    // Ngược lại, trả về chuỗi trống
    return option ? option[labelField] : '';
}