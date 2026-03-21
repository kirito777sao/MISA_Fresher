/**
 * File: Enum.js (hoặc tương tự)
 * Mục đích: Định nghĩa các hằng số enumeration cho các mã trạng thái API và trạng thái nghiệp vụ.
 */

// -------------------------------------------------------------------------
// 1. Mã Trạng thái HTTP (STATUS_CODE)
// -------------------------------------------------------------------------

/**
 * Các mã trạng thái HTTP chính được sử dụng trong ứng dụng (thường là mã lỗi từ API).
 */
export const STATUS_CODE = {
    // Thành công (OK)
    OK: 200,
    // Tạo mới thành công (Created)
    CREATED: 201,
    // Thành công, nhưng không trả về nội dung (No Content - thường dùng cho DELETE)
    NO_CONTENT: 204,
    // Lỗi từ client, dữ liệu không hợp lệ (Validation Error)
    VALIDATION: 400,
    // Xung đột dữ liệu (ví dụ: Mã đã tồn tại - Conflict)
    CONFLICT: 409,
    // Lỗi máy chủ (Bad Request - dùng thay thế cho Internal Server Error 500 nếu cần phân loại)
    BAD_REQUEST: 500, // Thường 5xx là Internal Server Error. 500 là lỗi chung.
};

// -------------------------------------------------------------------------
// 2. Trạng thái Nghiệp vụ (STATUS)
// -------------------------------------------------------------------------

/**
 * Các trạng thái nghiệp vụ chuẩn hóa, được ánh xạ tới chuỗi hiển thị Tiếng Việt.
 */
export const STATUS = {
    // Trạng thái đang sử dụng (ACTIVE)
    ACTIVE: "Đang sử dụng",
    // Trạng thái ngừng sử dụng (INACTIVE)
    INACTIVE: "Ngừng sử dụng",
};