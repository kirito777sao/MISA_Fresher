/**
 * File: filterConstants.js (hoặc tương tự)
 * Mục đích: Định nghĩa các hằng số cho các tùy chọn điều kiện lọc trong giao diện người dùng (ví dụ: trong lưới dữ liệu hoặc bộ lọc tìm kiếm).
 */

// -------------------------------------------------------------------------
// 1. Điều kiện lọc cho dữ liệu kiểu Text (Chuỗi)
// -------------------------------------------------------------------------
/**
 * Các điều kiện lọc text.
 * Created by: HoanTD (11/12/2025)
 */
export const TEXT_CONDITION_OPTIONS = [
  { label: "(Trống)", value: "isnull" },
  { label: "(Không trống)", value: "notnull" },
  { label: "Bằng", value: "=" },
  { label: "Khác", value: "<>" },
  { label: "Chứa", value: "contains", selected: true }, // Mặc định được chọn
  { label: "Không chứa", value: "notcontains" },
  { label: "Bắt đầu với", value: "startswith" },
  { label: "Kết thúc với", value: "endswith" },
];

// -------------------------------------------------------------------------
// 2. Điều kiện lọc cho dữ liệu kiểu Number (Số)
// -------------------------------------------------------------------------
/**
 * Các điều kiện lọc number.
 * Created by: HoanTD (11/12/2025)
 */
export const NUMBER_CONDITION_OPTIONS = [
  { label: "Bằng", value: "=", selected: true }, // Mặc định được chọn
  { label: "Khác", value: "<>" },
  { label: "Nhỏ hơn", value: "<" },
  { label: "Nhỏ hơn hoặc bằng", value: "<=" },
  { label: "Lớn hơn", value: ">" },
  { label: "Lớn hơn hoặc bằng", value: ">=" },
  { label: "(Trống)", value: "isnull" },
  { label: "(Không trống)", value: "notnull" },
];

// -------------------------------------------------------------------------
// 3. Điều kiện lọc cho dữ liệu kiểu Status (Trạng thái)
// -------------------------------------------------------------------------
/**
 * Các điều kiện lọc trạng thái (Thường dùng cho trường boolean hoặc enum).
 * Created by: HoanTD (11/12/2025)
 */
export const STATUS_CONDITION_OPTIONS = [
  { label: "Đang sử dụng", value: "ACTIVE" },
  { label: "Ngừng sử dụng", value: "INACTIVE" },
];