<template>
  <aside class="sidebar">
    <div class="sidebar_item-list flex flex-col justify-between">
      <div class="sidebar_container">
        <menu-item
          v-for="item in lstNavItem"
          :key="item.iconName"
          :iconName="item.iconName"
          :title="item.title"
          :endpoint="item.endpoint"
          :children="item.children"
          :horizontalMenu="item.horizontalMenu"
        >
        </menu-item>
      </div>

      <div class="bottom-area w-full">
        <div
          class="collapse-btn flex items-center justify-center rounded-sm cursor-pointer"
          @click="handleCollapse"
        >
          <div class="icon-wrapper">
            <div class="icon_collapse icon-20"></div>
          </div>
          <div class="sidebar_item-title truncate ml-3">Thu gọn</div>
        </div>
      </div>
    </div>
  </aside>
</template>

<script setup>
import MenuItem from "@/layouts/menu-item/MenuItem.vue";
import { ref } from "vue";

/**
 * Danh sách các mục điều hướng trên Sidebar
 * Cấu trúc mỗi item bao gồm:
 * - endpoint: Đường dẫn router
 * - iconName: Class icon hiển thị
 * - title: Tên hiển thị trên menu
 * - children (tùy chọn): Danh sách menu con hiển thị dạng Accordion (dọc)
 * - horizontalMenu (tùy chọn): Danh sách menu con hiển thị dạng Popover (ngang)
 */
const lstNavItem = ref([
  {
    endpoint: "/dashboard",
    iconName: "icon_dashboard",
    title: "Tổng quan",
  },
  {
    endpoint: "/sale-order",
    iconName: "icon_order",
    title: "Đơn đặt hàng",
  },
  {
    endpoint: "/production-plan",
    iconName: "icon_plan",
    title: "Kế hoạch sản xuất",
    children: [
      {
        endpoint: "/production-plan/master",
        iconName: "icon_arrow-down-right",
        title: "Kế hoạch tổng thể",
      },
      {
        endpoint: "/production-plan/detail",
        iconName: "icon_arrow-down-right",
        title: "Kế hoạch chi tiết",
      },
      {
        endpoint: "/production-plan/material",
        iconName: "icon_arrow-down-right",
        title: "Kế hoạch nguyên vật liệu",
      },
      {
        endpoint: "/production-plan/material-request",
        iconName: "icon_arrow-down-right",
        title: "Yêu cầu mua NVL",
      },
    ],
  },
  {
    endpoint: "/production-execution",
    iconName: "icon_coordinate",
    title: "Điều phối và thực thi",
    children: [
      {
        endpoint: "/production-execution/order",
        iconName: "icon_arrow-down-right",
        title: "Lệnh sản xuất",
      },
      {
        endpoint: "/production-execution/schedule",
        iconName: "icon_arrow-down-right",
        title: "Lịch sản xuất",
      },
      {
        endpoint: "/production-execution/material-request",
        iconName: "icon_arrow-down-right",
        title: "Yêu cầu vật tư",
      },
      {
        endpoint: "/production-execution/statistics",
        iconName: "icon_arrow-down-right",
        title: "Thống kê sản xuất",
      },
      {
        endpoint: "/production-execution/product-request",
        iconName: "icon_arrow-down-right",
        title: "Yêu cầu nhập thành phẩm, nguyên vật liệu thừa",
      },
    ],
  },
  {
    endpoint: "/product-quality",
    iconName: "icon_quality-control",
    title: "Kiểm tra chất lượng",
    children: [
      {
        endpoint: "/product-quality/quality-check-request",
        iconName: "icon_arrow-down-right",
        title: "Yêu cầu kiểm tra",
      },
      {
        endpoint: "/product-quality/inspection-voucher",
        iconName: "icon_arrow-down-right",
        title: "Phiếu kiếm tra",
      },
      {
        endpoint: "/product-quality/standard",
        iconName: "icon_arrow-down-right",
        title: "Tiêu chuẩn",
      },
    ],
  },
  {
    endpoint: "/production-material",
    iconName: "icon_material-factory",
    title: "Kho vật tư",
    children: [
      {
        endpoint: "/production-material/request-material",
        iconName: "icon_arrow-down-right",
        title: "Đề nghị kho cấp vật tư",
      },
      {
        endpoint: "/production-material/factory-inward",
        iconName: "icon_arrow-down-right",
        title: "Nhập kho",
      },
      {
        endpoint: "/production-material/export-material",
        iconName: "icon_arrow-down-right",
        title: "Xuất kho",
      },
      {
        endpoint: "/production-material/transfer-material",
        iconName: "icon_arrow-down-right",
        title: "Điều chuyển",
      },
      {
        endpoint: "/production-material/opn-inventory-stock",
        iconName: "icon_arrow-down-right",
        title: "Tồn kho đầu kỳ",
      },
    ],
  },
  {
    endpoint: "/reportlist",
    iconName: "icon_production-report",
    title: "Báo cáo",
  },
  {
    endpoint: "/dictionary",
    iconName: "icon_product-material",
    title: "Sản phẩm, NVL",
    children: [
      {
        endpoint: "/dictionary/inventory-item",
        iconName: "icon_arrow-down-right",
        title: "Vật tư hàng hóa",
      },
      {
        endpoint: "/dictionary/inventory-item-group",
        iconName: "icon_arrow-down-right",
        title: "Nhóm vật tư hàng hóa",
      },
      {
        endpoint: "/dictionary/bill-of-materials",
        iconName: "icon_arrow-down-right",
        title: "Định mức nguyên vật liệu",
      },
      {
        endpoint: "/dictionary/alternative-material-list",
        iconName: "icon_arrow-down-right",
        title: "Nguyên vật liệu thay thế",
      },
    ],
  },
  {
    endpoint: "/production-process",
    iconName: "icon_process",
    title: "Quy trình sản xuất",
    children: [
      {
        endpoint: "/production-process/stage",
        iconName: "icon_arrow-down-right",
        title: "Công đoạn",
      },
      {
        endpoint: "/production-process/production-process",
        iconName: "icon_arrow-down-right",
        title: "Quy trình sản xuất",
      },
    ],
  },
  {
    endpoint: "/production-capacity",
    iconName: "icon_production-capacity",
    title: "Năng lực sản xuất",
    children: [
      {
        endpoint: "/production-capacity/production-team",
        iconName: "icon_arrow-down-right",
        title: "Tổ sản xuất",
      },
      {
        endpoint: "/production-capacity/production-equipment",
        iconName: "icon_arrow-down-right",
        title: "Máy móc",
      },
      {
        endpoint: "/production-capacity/production-capacity-group",
        iconName: "icon_arrow-down-right",
        title: "Nhóm năng lực",
      },
    ],
  },
  {
    endpoint: "/production-category",
    iconName: "icon_production-category",
    title: "Danh mục khác",
    horizontalMenu: [
      {
        group: "Đối tượng",
        items: [
          { title: "Khách hàng", endpoint: "/production-category/customer" },
          { title: "Nhân viên", endpoint: "/production-category/employee" },
          { title: "Đối tượng tập hợp chi phí", endpoint: "/production-category/job" },
        ],
      },
      {
        group: "Lịch làm việc",
        items: [
          { title: "Ca làm việc", endpoint: "/production-category/shift" },
          { title: "Ngày nghỉ", endpoint: "/production-category/holiday-schedule" },
          { title: "Lịch làm việc", endpoint: "/production-category/work-schedule" },
        ],
      },
      {
        group: "Khác",
        items: [
          { title: "Cơ cấu tổ chức", endpoint: "/production-category/organization" },
          { title: "Kho", endpoint: "/production-category/stock" },
          { title: "Đơn vị tính", endpoint: "/production-category/unit" },
          { title: "Lý do dừng công việc", endpoint: "/production-category/reason" },
        ],
      },
    ],
  },
  {
    endpoint: "/system",
    iconName: "icon_production-system",
    title: "Thiết lập",
    horizontalMenu: [
      {
        group: "Thiết lập",
        items: [
          { title: "Thông tin công ty", endpoint: "/system/options-general" },
          { title: "Người dùng, vai trò", endpoint: "/system/user" },
          { title: "Tùy chọn", endpoint: "/system/options" },
          { title: "Kết nối ứng dụng", endpoint: "/system/connect" },
        ],
      },
      {
        group: "Tiện ích",
        items: [{ title: "Nhật ký truy cập", endpoint: "/system/auditing-log" }],
      },
    ],
  },
]);

/**
 * Khai báo các sự kiện phát ra từ component
 */
const emits = defineEmits(["addCollapseClass"]);

/// <summary>
/// Xử lý sự kiện nhấn nút thu gọn Sidebar
/// Gửi tín hiệu ra component cha (Layout chính) để thay đổi class bao ngoài
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
function handleCollapse() {
  emits("addCollapseClass");
}
</script>

<style scoped>
/* --- Khung Sidebar chính --- */
.sidebar {
  width: 234px;
  height: 100%;
  background-color: #111827;
  transition: all 0.3s ease;
}

/* --- Danh sách các item bên trong --- */
.sidebar_item-list {
  height: 100%;
  padding: 12px;
  padding-bottom: unset;
  overflow: auto; /* Cho phép cuộn khi danh sách menu quá dài */
}

.sidebar_container {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

/* --- Khu vực chân Sidebar (Bottom Area) --- */
.sidebar .bottom-area {
  height: 56px;
  padding: 10px;
  border-top: 1px solid rgba(209, 213, 219, 0.3);
}

.sidebar .bottom-area .collapse-btn {
  height: 36px;
  color: #9ca3af;
  transition: background-color 0.2s;
}

.icon-wrapper {
  margin-right: -4px;
}

/* --- Xử lý trạng thái Sidebar khi bị thu gọn (Collapse) --- */
/* Class này thường được component cha bọc ngoài kiểm soát */
.sidebar-collapse .sidebar {
  width: 60px;
  overflow: hidden;
}

/* Đảo chiều icon và căn chỉnh lại vị trí khi thu gọn */
.sidebar-collapse .collapse-btn .icon_collapse {
  transform: rotate(180deg);
  margin-left: 8px;
}
</style>