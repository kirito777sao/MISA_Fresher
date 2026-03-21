<template>
  <a-tooltip>
    <template #title v-if="toolTip">{{ toolTip }}</template>
    
    <button
      :class="['btn', `btn--${type}`, noPadding ? 'no-padding' : '']"
      :disabled="props.disabled"
      :tabindex="tabIndex"
    >
      <div
        v-if="icon && positionIcon === 'left'"
        :class="[icon, `icon-${iconSize}`, noMargin ? 'no-margin' : '']"
        class="btn__icon btn__icon--left"
      ></div>

      <span class="btn__content">
        <slot></slot>
      </span>

      <div
        v-if="icon && positionIcon === 'right'"
        :class="[icon, `icon-${iconSize}`, noMargin ? 'no-margin' : '']"
        class="btn__icon btn__icon--right"
      ></div>
    </button>
  </a-tooltip>
</template>

<script setup>
/**
 * Component Button dùng chung cho toàn hệ thống
 * Hỗ trợ nhiều loại hiển thị (type), icon, tooltip và các trạng thái disabled.
 * * @props {String} type - Loại button (misa, outlined, filled-neutral, text, warning, danger,...)
 * @props {String} icon - Class của icon (ví dụ: 'mi mi-add')
 * @props {String} positionIcon - Vị trí icon ('left' hoặc 'right')
 * @props {String} iconSize - Kích thước icon ('16', '20', '24')
 * @props {Boolean} disabled - Trạng thái vô hiệu hóa button
 * @props {Boolean} noMargin - Loại bỏ margin của icon
 * @props {Boolean} noPadding - Loại bỏ padding của button
 * @props {String|Number} tabIndex - Thứ tự focus khi nhấn phím Tab
 * @props {String} toolTip - Nội dung hiển thị khi hover
 * * Created by: HoanTD (16/12/2025)
 */
const props = defineProps({
  // Định dạng hiển thị của Button (Mặc định là phong cách MISA)
  type: {
    type: String,
    default: "misa",
  },
  // Class icon đi kèm
  icon: {
    type: String,
    default: null,
  },
  // Vị trí hiển thị của icon so với chữ
  positionIcon: {
    type: String,
    default: "left",
    validator: (value) => ["left", "right"].includes(value),
  },
  // Kích thước icon (đơn vị pixel)
  iconSize: {
    type: String,
    default: "16",
    validator: (value) => ["16", "20", "24"].includes(value),
  },
  // Trạng thái khóa button
  disabled: {
    type: Boolean,
    default: false,
  },
  // Tùy chọn xóa lề của icon
  noMargin: {
    type: Boolean,
    default: false,
  },
  // Tùy chọn xóa khoảng đệm của button
  noPadding: {
    type: Boolean,
    default: false,
  },
  // Chỉ số tab index để điều hướng bàn phím
  tabIndex: {
    type: [String, Number],
    default: null,
  },
  // Văn bản hiển thị tooltip
  toolTip: {
    type: [String],
    default: "",
  },
});
</script>

<style scoped>
.btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  height: 28px;
  padding: 6px 12px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 13px;
  font-weight: 500;
  transition: all 0.3s ease;
}
/* misa */
.btn--misa {
  background-color: #009b71;
  color: #fff;
}
.btn--misa:hover {
  background-color: #007b5d;
}
/* outlined */
.btn--outlined {
  border: 1px solid #d1d5db;
  color: #111827;
  background-color: #fff;
}
.btn--outlined:hover {
  background-color: #f3f4f6;
}
.btn--outline-success {
  border: 1px solid #009b71;
  color: #009b71;
  background-color: #fff;
}
.btn--outline-success:hover {
  background-color: #dcfce7;
}
.btn--outline-danger {
  border: 1px solid #dc2626;
  color: #dc2626;
  background-color: #fff;
}
.btn--outline-danger:hover {
  background-color: #fee2e2;
}
/* filled */
.btn--filled-neutral {
  background-color: #f3f4f6;
  color: #111827;
  border: none;
}
.btn--filled-neutral:hover {
  background-color: #e5e7eb;
}
.btn--text {
  background: none;
  border: none;
  color: #007b5d;
  font-weight: 400;
}
.btn--text:hover {
  text-decoration: underline;
}
.btn--warning {
  background-color: #ff9800;
  color: white;
  border: 1px solid #ff9800;
}
.btn--danger {
  background-color: #f44336;
  color: white;
  border: 1px solid #f44336;
}
.btn:disabled {
  cursor: not-allowed;
  opacity: 0.6;
}
.btn__content {
  white-space: nowrap;
}
.btn__icon {
  display: inline-flex;
  align-items: center;
}
.btn__icon--left {
  margin-right: 4px;
}
.btn__icon--right {
  margin-left: 4px;
}
.no-margin {
  margin: 0;
}
.no-padding {
  padding: 0;
}
.text-delete-all-filter {
  color: #f06666;
  white-space: nowrap;
}
</style>