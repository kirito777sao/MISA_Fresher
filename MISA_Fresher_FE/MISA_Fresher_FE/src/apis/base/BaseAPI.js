import api from '@/apis/config/APIConfig.js';

/**
 * Lớp API cơ sở (BaseAPI) cung cấp các phương thức CRUD chuẩn.
 * Các lớp API cụ thể (WorkShiftsAPI, EmployeesAPI,...) sẽ kế thừa lớp này.
 */
export default class BaseAPI {
    /**
     * Khởi tạo lớp BaseAPI.
     * Thuộc tính 'controller' phải được thiết lập trong lớp con
     * để xác định endpoint API (ví dụ: 'employees', 'workshifts').
     */
    constructor() {
        this.controller = null; // Cần được gán giá trị trong lớp con
    }

    // ------------------------------------------------------------------
    // I. READ OPERATIONS (Thao tác Đọc)
    // ------------------------------------------------------------------

    /**
     * <summary>
     * Phương thức lấy tất cả dữ liệu (GET: /controller).
     * </summary>
     * @returns {Promise} Promise object của Axios.
     */
    getAll() {
        return api.get(`${this.controller}`);
    }

    /**
     * <summary>
     * Hàm lấy dữ liệu phân trang (POST: /controller/paging).
     * </summary>
     * @param {Object} payload - Object chứa các tham số phân trang/lọc (pageSize, pageNumber, filters, v.v.).
     * @returns {Promise} Promise object của Axios.
     */
    paging(payload) {
        return api.post(`${this.controller}/paging`, payload);
    }

    /**
     * <summary>
     * Hàm lấy dữ liệu theo ID (GET: /controller/{id}).
     * </summary>
     * @param {string|number} id - ID của bản ghi cần lấy.
     * @returns {Promise} Promise object của Axios.
     */
    get(id) {
        return api.get(`${this.controller}/${id}`);
    }

    // ------------------------------------------------------------------
    // II. WRITE OPERATIONS (Thao tác Ghi/Chỉnh sửa)
    // ------------------------------------------------------------------

    /**
     * <summary>
     * Hàm thêm dữ liệu (POST: /controller).
     * </summary>
     * @param {Object} payload - Dữ liệu bản ghi mới.
     * @returns {Promise} Promise object của Axios.
     */
    add(payload) {
        return api.post(`${this.controller}`, payload);
    }

    /**
     * <summary>
     * Hàm cập nhật dữ liệu (PUT: /controller/{id}).
     * </summary>
     * @param {string|number} id - ID của bản ghi cần cập nhật.
     * @param {Object} body - Dữ liệu cập nhật.
     * @returns {Promise} Promise object của Axios.
     */
    update(id, body) {
        return api.put(`${this.controller}/${id}`, body);
    }

    /**
     * <summary>
     * Hàm xóa bản ghi (DELETE: /controller/{id}).
     * </summary>
     * @param {string|number} id - ID của bản ghi cần xóa.
     * @returns {Promise} Promise object của Axios.
     */
    delete(id) {
        return api.delete(`${this.controller}/${id}`);
    }
}