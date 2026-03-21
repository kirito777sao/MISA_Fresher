<template>
  <div class="horizontal-menu-container flex gap-1">
    <div v-for="group in menuData" :key="group.group" class="menu-column flex flex-col gap-2">
      <div class="menu-group-title px-8">{{ group.group }}</div>
      
      <RouterLink
        v-for="item in group.items"
        :key="item.endpoint"
        :to="item.endpoint"
        v-slot="{ isExactActive }"
      >
        <div class="menu-item-link pl-1 pr-8" :class="{ active: isExactActive }">
          <div class="menu-item-content flex gap-2">
            <div class="icon_arrow-down-right icon-20 icon-hover"></div>
            <span>{{ item.title }}</span>
          </div>
        </div>
      </RouterLink>
    </div>
  </div>
</template>

<script setup>
import { RouterLink } from "vue-router";

/**
 * Định nghĩa Props cho Horizontal Menu
 * @property {Array} menuData - Mảng chứa các nhóm menu và các item con của chúng
 * Cấu trúc dự kiến: [{ group: 'Tên nhóm', items: [{ title: 'Tiêu đề', endpoint: '/duong-dan' }] }]
 */
defineProps({
  menuData: {
    type: Array,
    required: true,
  },
});
</script>

<style scoped>
/* Định dạng tiêu đề của từng nhóm menu */
.menu-group-title {
  color: #4b5563;
  align-content: center;
}

/* Định dạng chung cho chiều cao và font chữ của Group và Link */
.menu-group-title,
.menu-item-link {
  height: 32px;
  font-weight: 500;
  align-content: center;
  border-radius: 4px;
}

/* Màu sắc mặc định của nội dung bên trong item (text và icon) */
.menu-item-content {
  color: #9ca3af;
}

/* --- TRẠNG THÁI HOVER --- */
/* Hiệu ứng nền khi di chuột qua item */
.horizontal-menu-container .menu-item-link:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

/* Đổi màu chữ sang trắng khi hover */
.horizontal-menu-container .menu-item-link:hover .menu-item-content {
  color: #ffffff;
}

/* Đổi màu icon sang trắng (mask-image) khi hover */
.horizontal-menu-container .menu-item-link:hover .icon-hover {
  background-color: #fff;
}

/* --- TRẠNG THÁI ACTIVE (Đúng đường dẫn) --- */
/* Hiệu ứng nền đậm hơn khi router đang ở trang này */
.horizontal-menu-container .menu-item-link.active {
  background-color: rgba(255, 255, 255, 0.25);
}

/* Giữ màu chữ trắng khi đang active */
.horizontal-menu-container .menu-item-link.active .menu-item-content {
  color: #ffffff;
}

/* Giữ màu icon trắng khi đang active */
.horizontal-menu-container .menu-item-link.active .icon-hover {
  background-color: #fff;
}
</style>