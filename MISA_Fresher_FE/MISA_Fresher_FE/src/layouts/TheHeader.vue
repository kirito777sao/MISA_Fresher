<template>
  <div class="navbar flex justify-between">
    
    <div class="navbar_left flex items-center pl-4">
      <div class="icon_menu icon-24 cursor-pointer"></div>
      
      <div class="logo-item flex">
        <div class="icon_logo icon-30 ml-4"></div>
        <div class="logo-text">Sản xuất</div>
      </div>
      
      <div class="cursor-pointer flex items-center ml-5">
        <div class="font-bold">(SMEClould) VĂN PHÒNG ĐẠI DIỆN CÔNG TY CỔ PHẦN MISA TẠI HÀ NỘI</div>
        <div class="icon_dropdown icon-16"></div>
      </div>
    </div>

    <div class="navbar_right flex items-center">
      <div class="icon-nav-hover mr-3 cursor-pointer" @click="goToExportExcel">
        <div class="icon_download-status icon-24" title="Xuất Excel"></div>
      </div>
      
      <div class="nav-separator"></div>
      
      <div class="icon-function-nav">
        <div class="icon_package icon-24" title="Tra cứu tồn kho" @click="goToInventoryLookup"></div>
      </div>
      <div class="icon-function-nav icon-nav-hover">
        <div class="icon_invite-box icon-24"></div>
      </div>
      <div class="icon-function-nav">
        <div class="icon_bell icon-24" title="Thông báo"></div>
      </div>
      <div class="icon-function-nav">
        <div class="icon_question-circle icon-24" title="Trợ giúp" @click="goToHelp"></div>
      </div>
      <div class="icon-function-nav">
        <div class="icon_dots-circle-horizontal icon-24" title="Tính năng khác" @click="goToOtherFeatures"></div>
      </div>
      <div class="icon-function-nav icon-nav-hover">
        <div class="icon_knowledge-trigger icon-24" title="Kiến thức hữu ích"></div>
      </div>

      <div class="avatar-account-container">
        <img class="avatar-account" :src="avatarImg" alt="misa-avatar" loading="lazy" />
      </div>
    </div>
  </div>
</template>

<script setup>
/**
 * Import ảnh đại diện mặc định từ thư mục assets
 * Sử dụng cho phần thông tin tài khoản trên Navbar
 */
import avatarImg from "@/assets/images/avatar-don-gian.jpg";
import { useRouter } from "vue-router"; 
const router = useRouter(); 
function goToExportExcel() {
    router.push("/export-excel");
}

function goToInventoryLookup() {
    router.push("/inventory-lookup");
}

function goToHelp() {
    router.push("/help");
}

function goToOtherFeatures() {
    router.push("/other-features");
}
</script>

<style scoped>
/* --- Tổng thể Navbar --- */
.navbar {
  height: 48px;
  font-family: Inter;
  border-bottom: 1px solid #d1d5db;
  background: #fff;
}

/* --- Khu vực Logo --- */
.logo-item {
  width: 140px;
  cursor: pointer;
}

.navbar_left .logo-item .logo-text {
  font-size: 18px;
  margin-left: 12px;
  font-weight: 700;
  color: #262626;
  white-space: nowrap;
}

/* --- Khu vực chức năng bên phải --- */
.navbar_right {
  padding-right: 16px;
}

/* Thanh phân cách dọc giữa các nhóm icon */
.nav-separator {
  width: 1px;
  height: 16px;
  background: #d1d5db;
  align-self: center;
  border-radius: 1px;
}

/* Khoảng cách giữa các icon chức năng */
.icon-function-nav {
  margin-left: 12px;
  cursor: pointer;
}

/* Hiệu ứng hover cho các icon trên Navbar */
.icon-nav-hover {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 32px;
  height: 32px;
}

.icon-nav-hover:hover {
  background-color: rgba(0, 0, 0, 0.1);
  border-radius: 50%;
}

/* --- Thông tin tài khoản (Avatar) --- */
.avatar-account-container {
  border-radius: 50%;
  overflow: hidden;
  width: 32px;
  height: 32px;
  margin-left: 16px;
  cursor: pointer;
}

.avatar-account {
  width: 100%;
  height: 100%;
  max-width: 32px;
  max-height: 32px;
  object-fit: cover;
  /* Giữ tỷ lệ ảnh 1:1 để không bị biến dạng */
}

.tooltip {
  position: relative;
  display: inline-block;
  cursor: pointer;
}

.tooltiptext {
  visibility: hidden;
  width: 100px;
  background-color: #333;
  color: #fff;
  text-align: center;
  border-radius: 4px;
  padding: 5px;
  position: absolute;
  z-index: 1;
  bottom: 125%; /* vị trí phía trên */
  left: 50%;
  transform: translateX(-50%);
  opacity: 0;
  transition: opacity 0.3s;
}

.tooltip:hover .tooltiptext {
  visibility: visible;
  opacity: 1;
}

</style>