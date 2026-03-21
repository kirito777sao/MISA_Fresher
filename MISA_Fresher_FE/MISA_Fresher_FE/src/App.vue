<template>
  <div class="app-container">
    <the-header />

    <div class="content flex-1 flex" :class="{ 'sidebar-collapse': isSidebarCollapse }">
      <the-sidebar @addCollapseClass="handleAddCollapseClass" />

      <div class="flex-1 page-padding min-w-0 min-w-186">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script setup>
import TheHeader from '@/layouts/TheHeader.vue'
import TheSidebar from './layouts/TheSidebar.vue'
import { ref } from 'vue'

// State để quản lý trạng thái thu gọn/mở rộng của Sidebar
const isSidebarCollapse = ref(false)

/**
 * <summary>
 * Xử lý sự kiện từ TheSidebar để chuyển đổi trạng thái thu gọn của Sidebar.
 * </summary>
 * Created by: [Tên tác giả]
 */
function handleAddCollapseClass() {
  isSidebarCollapse.value = !isSidebarCollapse.value
}
</script>

<style scoped>
/* --- Container --- */
.app-container {
  height: 100vh;
  width: 100%;
  position: fixed; /* Giữ cố định trên màn hình */
  display: flex;
  flex-direction: column; /* Sắp xếp Header và Content theo chiều dọc */
}

/* --- Content (Khu vực chính dưới Header) --- */
.content {
  flex: 1; /* Chiếm hết không gian còn lại theo chiều dọc */
  /* 48px là chiều cao của TheHeader (cần kiểm tra chính xác) */
  height: calc(100% - 48px);
  background: #e5e7eb; /* Màu nền xám nhạt */
}

/* --- Padding cho khu vực Main Content --- */
.page-padding {
  padding: 16px 20px 20px;
}

/* * Chú ý: Class '.sidebar-collapse' sẽ được thêm vào '.content' khi Sidebar thu gọn, 
 * nhưng hiện tại không có CSS nào được định nghĩa cho class này để ảnh hưởng đến bố cục chung.
 * Thường các style cho Sidebar thu gọn sẽ được định nghĩa trong component TheSidebar
 * hoặc sử dụng các biến CSS/Tailwind để Main Content tự điều chỉnh kích thước/margin.
 */
</style>
