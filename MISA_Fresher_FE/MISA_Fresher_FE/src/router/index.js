import Shift from '@/views/dictionary/shift/Shift.vue'
import Temp from '@/views/temps/Temp.vue'
import { createRouter, createWebHistory } from 'vue-router'

/**
 * Khởi tạo Vue Router.
 */
const router = createRouter({
  // Sử dụng history mode (URL sạch không có dấu #)
  history: createWebHistory(import.meta.env.BASE_URL),

  // Mảng định nghĩa các tuyến đường (routes)
  routes: [
    /**
     * Tuyến đường cho trang Quản lý Ca làm việc.
     */
    {
      path: '/production-category/shift',
      name: 'Shift',
      component: Shift, // Component tương ứng: Shift.vue
    },

    /**
     * Tuyến đường Catch-all (404 Not Found).
     * Tuyến đường này phải luôn nằm cuối cùng.
     */
    {
      // Biểu thức chính quy match tất cả các đường dẫn không khớp
      path: '/:catchAll(.*)',
      name: 'NotFound',
      component: Temp, // Component tương ứng: Temp.vue (Dùng làm trang thông báo tính năng đang phát triển/404)
    },
  ],
})

// Export router để sử dụng trong main.js hoặc tương đương
export default router