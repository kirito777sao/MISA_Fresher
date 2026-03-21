<template>
  <div class="ms-textarea-wrapper flex items-start" :class="[containerWidth]">
    <label v-if="label" class="ms-input-label flex-shrink-0 pt-[5px]">
      {{ label }}
      <span v-if="required" class="text-red-500 ml-0.5">*</span>
    </label>

    <div
      class="ms-textarea-container relative flex items-center rounded-sm bg-white overflow-hidden flex-1"
    >
      <textarea
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        :rows="rows"
        :readonly="readonly"
        class="ms-textarea-field w-full h-full text-sm bg-transparent border-none outline-none resize-none"
        :style="placeholderStyle"
        :tabindex="tabIndex"
      ></textarea>
    </div>
  </div>
</template>

<script setup>
import { computed } from "vue";

/**
 * Định nghĩa các Props cho component MsTextarea
 * @property {String|Number} modelValue - Giá trị của textarea dùng cho v-model
 * @property {String} placeholder - Văn bản hiển thị gợi ý khi chưa có dữ liệu
 * @property {String} label - Nhãn hiển thị bên trái của textarea
 * @property {Boolean} required - Trạng thái bắt buộc (hiển thị dấu *)
 * @property {Boolean} readonly - Chế độ chỉ đọc, không cho phép chỉnh sửa nội dung
 * @property {String} width - Class Tailwind quy định độ rộng của container (mặc định w-full)
 * @property {String|Number} rows - Số dòng hiển thị tối thiểu của textarea
 * @property {String} placeholderAlign - Căn lề cho placeholder (left hoặc right)
 * @property {String|Number} tabIndex - Thứ tự tabindex khi nhấn phím Tab
 */
const props = defineProps({
  modelValue: {
    type: [String, Number],
    default: "",
  },
  placeholder: {
    type: String,
    default: "",
  },
  label: {
    type: String,
    default: "",
  },
  required: {
    type: Boolean,
    default: false,
  },
  readonly: {
    type: Boolean,
    default: false,
  },
  width: {
    type: String,
    default: "w-full",
  },
  rows: {
    type: [String, Number],
    default: 3,
  },
  placeholderAlign: {
    type: String,
    default: "left",
    validator: (value) => ["left", "right"].includes(value),
  },
  tabIndex: {
    type: [String, Number],
    default: null,
  },
});

/**
 * Các sự kiện (emits) mà component phát ra
 */
const emit = defineEmits(["update:modelValue"]);

/// <summary>
/// Tính toán class Tailwind cho chiều rộng của container
/// </summary>
/// <returns>Chuỗi class tailwind (VD: w-full, w-1/2...)</returns>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const containerWidth = computed(() => props.width);

/// <summary>
/// Tính toán style cho CSS Variable để điều khiển căn lề placeholder động
/// </summary>
/// <returns>Object chứa biến CSS --placeholder-align</returns>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const placeholderStyle = computed(() => ({
  "--placeholder-align": props.placeholderAlign,
}));
</script>

<style scoped>
/* Khung bao ngoài cùng của textarea */
.ms-textarea-wrapper {
  min-height: 40px;
}

/* Nhãn hiển thị bên trái textarea */
.ms-input-label {
  width: 180px;
  color: #262626;
  font-weight: 500;
  font-size: 13px;
}

/* Container bao bọc thẻ textarea thực tế */
.ms-textarea-container {
  border: 1px solid #d1d5db;
  font-size: 13px;
  min-width: 0;
  padding: 0;
}

/* Trạng thái khi người dùng click vào vùng textarea */
.ms-textarea-container:focus-within {
  border-color: #009b71;
}

/* Định dạng nội dung bên trong vùng nhập liệu */
.ms-textarea-field {
  padding: 5px 12px;
  font-size: 13px;
  font-weight: 400;
  line-height: 1.5;
}

/* Định dạng cho văn bản gợi ý (placeholder) */
.ms-textarea-field::placeholder {
  font-size: 13px;
  font-weight: 400;
  color: #9ca3af;
  /* Sử dụng biến CSS đã được tính toán ở script để căn lề */
  text-align: var(--placeholder-align, left);
}
</style>