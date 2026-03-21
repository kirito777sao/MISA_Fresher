import axios from 'axios';

// URL cơ sở cho tất cả các request API
const baseURL = "http://localhost:5237/api/v1/";

// Tạo một instance Axios tùy chỉnh
let api = axios.create({
    // Thiết lập URL cơ sở
    baseURL: baseURL,
    // Thiết lập các headers mặc định
    headers: {
        'Content-Type': 'application/json' // Yêu cầu và phản hồi dưới dạng JSON
    }
})

// -------------------------------------------------------------------------
// Bộ Chặn Phản hồi (Response Interceptor)
// -------------------------------------------------------------------------

/**
 * Interceptor: Xử lý các phản hồi từ API.
 * Nó kiểm tra lỗi 401 (Unauthorized) và 403 (Forbidden) 
 * để thực hiện các hành động như làm mới token hoặc chuyển hướng người dùng đến trang đăng nhập.
 */
api.interceptors.response.use(
    // Case 1: Phản hồi thành công (2xx)
    (response) => response,

    // Case 2: Phản hồi lỗi (>= 3xx)
    (error) => {
        // Kiểm tra xem lỗi có phải là lỗi phản hồi (error.response) và mã trạng thái
        if (error && error.response && [401, 403].includes(error.response.status)) {
            // Logic xử lý lỗi 401 hoặc 403:
            // 1. Nếu là 401: Thử làm mới token (refresh token)
            // 2. Nếu làm mới thất bại hoặc là 403: Đăng xuất người dùng và chuyển hướng đến trang đăng nhập
            console.error(`Lỗi xác thực/ủy quyền: ${error.response.status}`);
            // TODO: Implement logic to handle 401/403 (e.g., redirect to login)
            // Example: router.push('/login'); 
        }

        // Trả về Promise.reject để component gọi API có thể bắt và xử lý lỗi
        return Promise.reject(error);
    }
);

// Export instance Axios đã cấu hình
export default api;