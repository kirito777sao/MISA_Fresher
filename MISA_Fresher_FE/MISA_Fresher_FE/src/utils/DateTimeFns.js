/**
 * File: dateTimeUtils.js
 * Mục đích: Tập hợp các hàm tiện ích để xử lý định dạng Ngày/Giờ theo yêu cầu giao diện và API.
 */

// -------------------------------------------------------------------------
// 1. Hàm extractTime (Trích xuất HH:mm từ ISO)
// -------------------------------------------------------------------------

/**
 * <summary>
 * Hàm trợ giúp để trích xuất Giờ:Phút (HH:mm) từ chuỗi datetime chuẩn ISO 8601.
 * </summary>
 * @param {string} datetimeString - Chuỗi ISO 8601 (VD: "YYYY-MM-DDTHH:mm:ss").
 * @returns {string} - Chuỗi thời gian (VD: "HH:mm"), hoặc chuỗi trống nếu không hợp lệ.
 * Created by: HoanTD (06/12/2025)
 */
export const extractTime = (datetimeString) => {
  try {
    if (!datetimeString) return "";
    // Phân tách chuỗi tại ký tự 'T' để lấy phần thời gian
    const timePart = datetimeString.split("T")[1];

    if (timePart) {
      return timePart.substring(0, 5); // Lấy HH:mm (5 ký tự đầu)
    }
    return "";
  } catch (error) {
    console.error("Lỗi format thời gian:", error);
    return "";
  }
};

// -------------------------------------------------------------------------
// 2. Hàm convertTimeToISO (Chuyển HH:mm thành ISO)
// -------------------------------------------------------------------------

/**
 * <summary>
 * Hàm chuyển đổi Giờ:Phút (HH:MM) thành chuỗi ISO 8601 hoàn chỉnh.
 * Nó sử dụng ngày hiện tại và ghép thời gian đã cho vào.
 * </summary>
 * @param {string} timeString - Chuỗi thời gian (HH:MM).
 * @returns {string|null} Chuỗi ISO 8601 (VD: "YYYY-MM-DDT HH:MM:00.000Z"), hoặc null nếu đầu vào trống.
 */
export const convertTimeToISO = (timeString) => {
  if (!timeString) return null;

  const today = new Date();
  // Lấy phần ngày (YYYY-MM-DD) từ ISO String của ngày hiện tại
  const dateString = today.toISOString().split("T")[0];

  // Ghép lại thành chuỗi ISO 8601 chuẩn (bao gồm giây và múi giờ Z (UTC))
  return `${dateString}T${timeString}:00.000Z`;
};

// -------------------------------------------------------------------------
// 3. Hàm formatDateDDMMYYYY (Chuyển ISO thành DD/MM/YYYY)
// -------------------------------------------------------------------------

/**
 * <summary>
 * Chuyển đổi chuỗi ngày tháng định dạng ISO (YYYY-MM-DDTHH:mm:ss) sang định dạng DD/MM/YYYY.
 * </summary>
 * @param {string} isoDateString - Chuỗi ngày tháng ISO.
 * @returns {string} Ngày tháng đã định dạng (DD/MM/YYYY), hoặc chuỗi trống.
 */
export const formatDateDDMMYYYY = (isoDateString) => {
  if (!isoDateString) return "";

  try {
    // Chỉ lấy phần ngày (YYYY-MM-DD)
    const datePart = isoDateString.split('T')[0];
    const [year, month, day] = datePart.split('-');

    if (year && month && day) {
      // Sắp xếp lại thành DD/MM/YYYY
      return `${day}/${month}/${year}`;
    }

    return "";
  } catch (error) {
    console.error("Lỗi khi định dạng ngày:", error);
    return "";
  }
};