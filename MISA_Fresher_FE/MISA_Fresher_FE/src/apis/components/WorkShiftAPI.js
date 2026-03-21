import api from '@/apis/config/APIConfig.js';
import BaseAPI from '@/apis/base/BaseAPI.js';

/**
 * Lớp WorkShiftsAPI quản lý các giao tiếp API liên quan đến Ca làm việc.
 * Kế thừa các phương thức CRUD cơ bản từ BaseAPI.
 */
class WorkShiftsAPI extends BaseAPI {
    /**
     * Khởi tạo và thiết lập controller cho endpoint "WorkShift".
     */
    constructor() {
        super(); // Gọi constructor của BaseAPI để khởi tạo this.controller
        this.controller = "WorkShift"; // Thiết lập tên controller API
    }

    // ------------------------------------------------------------------
    // PHƯƠNG THỨC NGHIỆP VỤ ĐẶC THÙ
    // ------------------------------------------------------------------

    /**
     * <summary>
     * Hàm kiểm tra trùng lặp theo nghiệp vụ riêng (POST: /WorkShift/check-duplicate).
     * Thường dùng để kiểm tra mã ca làm việc đã tồn tại hay chưa trước khi lưu.
     * </summary>
     * @param {Object} payload - Dữ liệu cần kiểm tra (ví dụ: { workShiftCode: 'Code101' }).
     * @returns {Promise} Promise object của Axios.
     */
    checkDuplicate(payload) {
        return api.post(`${this.controller}/check-duplicate`, payload);
    }

    /**
     * <summary>
     * Hàm cập nhật trạng thái nhiều bản ghi (Batch Update Status) (POST: /WorkShift/update_status).
     * </summary>
     * @param {Object} payload - Dữ liệu chứa danh sách IDs và trạng thái mới (ví dụ: { ids: [...], newStatus: true/false }).
     * @returns {Promise} Promise object của Axios. Trả về số lượng bản ghi được cập nhật.
     * Created by: HoanTD (10/12/2025)
     */
    updateMultipleStatus(payload) {
        return api.post(`${this.controller}/update_status`, payload);
    }

    /**
     * <summary>
     * Hàm xóa nhiều bản ghi (Batch Delete) (POST: /WorkShift/delete).
     * </summary>
     * @param {Object} payload - Dữ liệu chứa danh sách IDs cần xóa (ví dụ: { ids: [...] }).
     * @returns {Promise} Promise object của Axios. Trả về số lượng bản ghi vừa xóa.
     * Created by: HoanTD (10/12/2025)
     */
    deleteMultiple(payload) {
        return api.post(`${this.controller}/delete`, payload);
    }
}

// Export một thể hiện (instance) duy nhất của lớp WorkShiftsAPI
export default new WorkShiftsAPI();