<template>
  <div class="sidebar_item">
    <div class="sidebar_item-link">
      <RouterLink v-slot="{ isExactActive }" :to="props.endpoint">
        <div
          class="sidebar_item-content flex justify-between relative"
          :class="{ active: isExactActive }"
          @click="toggleSubMenu"
          @mouseenter="handleOpenHorizontalMenu"
        >
          <div class="flex items-center justify-start gap-2">
            <div class="sidebar_item-icon ml-2">
              <div v-if="!props.isChild" :class="[props.iconName, 'icon-20', 'icon-hover']"></div>
              <div v-else :class="[props.iconName, 'icon-20', 'icon-hover']"></div>
            </div>
            <div class="sidebar_item-title truncate">{{ props.title }}</div>
          </div>

          <div
            v-if="props.children.length > 0"
            class="icon_dropdown-down icon-20 mr-2 icon-hover"
            :class="{ 'rotate-180': isCurrentlyOpen }"
          ></div>

          <div
            v-if="props.horizontalMenu.length > 0"
            class="icon_arrow-right icon-20 mr-2 icon-hover"
          ></div>

          <div
            v-if="props.horizontalMenu.length > 0 && isHorizontalMenuOpen"
            class="horizontal-sub-menu"
          >
            <HorizontalSubMenu
              :menuData="props.horizontalMenu"
              @mouseleave="handleCloseHorizontalMenu"
            />
          </div>
        </div>
      </RouterLink>
    </div>

    <div v-if="props.children.length > 0 && isCurrentlyOpen" class="sub-menu-list">
      <menu-item
        v-for="child in children"
        :key="child.endpoint"
        :iconName="child.iconName"
        :endpoint="child.endpoint"
        :title="child.title"
        :isChild="true"
      ></menu-item>
    </div>
  </div>
</template>

<script setup>
import { useMenuStore } from "@/stores/menuStore";
import { computed, ref } from "vue";
import { RouterLink } from "vue-router";
import HorizontalSubMenu from "./HorizontalSubMenu.vue";

/**
 * Định nghĩa Props cho Sidebar Item
 * @property {String} endpoint - Đường dẫn điều hướng của router
 * @property {String} iconName - Class name của icon (từ icon font hoặc svg sprite)
 * @property {String} title - Văn bản hiển thị của menu
 * @property {Array} children - Danh sách các menu con hiển thị dạng dọc (accordion)
 * @property {Boolean} isChild - Đánh dấu nếu item hiện tại là cấp con
 * @property {Array} horizontalMenu - Dữ liệu cho menu hiển thị theo chiều ngang khi hover
 */
const props = defineProps({
  endpoint: {
    type: String,
    default: "",
  },
  iconName: {
    type: String,
    default: "",
  },
  title: {
    type: String,
    default: "Menu Item",
  },
  children: {
    type: Array,
    default: () => [],
  },
  isChild: {
    type: Boolean,
    default: false,
  },
  horizontalMenu: {
    type: Array,
    default: () => [],
  },
});

// Khởi tạo store quản lý trạng thái menu tập trung
const menuStore = useMenuStore();

/// <summary>
/// Trạng thái đóng/mở của menu ngang (điều khiển bởi sự kiện hover)
/// </summary>
const isHorizontalMenuOpen = ref(false);

/// <summary>
/// Kiểm tra xem item hiện tại có đang được mở (dành cho menu có con) hay không
/// Dựa vào state openEndpoint trong Pinia store
/// </summary>
/// <returns>Boolean</returns>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const isCurrentlyOpen = computed(() => {
  if (props.isChild) return false;
  return menuStore.openEndpoint === props.endpoint;
});

/// <summary>
/// Xử lý việc bật/tắt menu con cấp dưới (Accordion)
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const toggleSubMenu = () => {
  if (props.children.length > 0) {
    menuStore.toggleOpenEndpoint(props.endpoint);
  }
};

/// <summary>
/// Xử lý hiển thị Horizontal Menu khi di chuột vào vùng item
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleOpenHorizontalMenu = () => {
  isHorizontalMenuOpen.value = true;
};

/// <summary>
/// Xử lý ẩn Horizontal Menu khi di chuột ra khỏi vùng item
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleCloseHorizontalMenu = () => {
  isHorizontalMenuOpen.value = false;
};
</script>

<style scoped>
/* --- Định dạng chung cho Sidebar Item --- */
.sidebar .sidebar_item {
  font-weight: 500;
}

/* Nội dung bên trong item (Flexbox container) */
.sidebar .sidebar_item .sidebar_item-content {
  color: #9ca3af;
  border-radius: 4px;
  padding: 9px 0;
  transition: all 0.2s ease;
}

/* Trạng thái Hover */
.sidebar .sidebar_item .sidebar_item-content:hover {
  background-color: rgba(255, 255, 255, 0.1);
  color: #ffffff;
}

.sidebar .sidebar_item .sidebar_item-content:hover .icon-hover {
  background-color: #ffffff;
}

/* Trạng thái Active khi Router khớp Endpoint */
.sidebar .sidebar_item .sidebar_item-content.active {
  background-color: #009b71;
  color: #ffffff;
}

.sidebar .sidebar_item .sidebar_item-content.active .icon-hover {
  background-color: #ffffff;
}

/* Giới hạn độ dài tiêu đề để tránh tràn layout */
.sidebar_item-title {
  max-width: 150px;
}

/* --- Xử lý giao diện khi Sidebar bị thu nhỏ (Collapse) --- */
.sidebar-collapse .sidebar .sidebar_item-content {
  height: 36px;
}

.sidebar-collapse .sidebar .sidebar_item-content .sidebar_item-icon {
  margin-right: 12px;
}

/* --- Danh sách menu con cấp dưới --- */
.sidebar .sub-menu-list {
  transition: all 0.3s ease-in-out;
}

/* --- Định dạng vị trí cho Horizontal Sub Menu (Fixed Position) --- */
.horizontal-sub-menu {
  position: fixed;
  left: 234px; /* Vị trí khớp với mép ngoài Sidebar mở rộng */
  background-color: #111827;
  margin-left: 4px;
  padding: 8px;
  border-radius: 4px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  z-index: 1000;
}

/* Điều chỉnh vị trí menu ngang khi sidebar thu nhỏ */
.sidebar-collapse .sidebar .horizontal-sub-menu {
  left: 60px;
}
</style>