<template>
  <div class="ms-input-wrapper flex items-center" :class="getJustifyClass">
    <label v-if="label" class="ms-input-label flex-shrink-0">
      {{ label }}
      <span v-if="required" class="text-required">&nbsp;*</span>
    </label>

    <div
      class="ms-input-container relative flex items-center rounded-sm bg-white overflow-hidden"
      :class="[containerHeight, containerWidth, { 'border-error': shouldShowErrorBorder }]"
    >
      <div v-if="$slots['icon-left']" class="ms-input-icon left-3 mr-1">
        <slot name="icon-left"></slot>
      </div>

      <input
        :type="type"
        :placeholder="placeholder"
        :value="modelValue"
        @input="$emit('update:modelValue', $event.target.value)"
        @blur="handleBlur"
        class="ms-input-field w-full h-full text-sm bg-transparent border-none outline-none"
        :class="{
          'has-icon-left': $slots['icon-left'],
          'has-icon-right': $slots['icon-right'],
          'ms-disabled': isDisabled,
          'text-red': isWarning,
        }"
        :tabindex="tabIndex"
        :readonly="readOnly"
        :disabled="isDisabled"
        :style="[placeholderStyle]"
        :maxlength="maxLength"
      />

      <div v-if="$slots['icon-right']" class="ms-input-icon right-3 ml-1">
        <slot name="icon-right"></slot>
      </div>
    </div>
  </div>
</template>

<script setup>
/**
 * Component Input dùng chung cho toàn bộ ứng dụng.
 * Hỗ trợ Validation (Error/Warning), Label, Required, Icon 2 đầu, và đa dạng cấu hình hiển thị.
 */
import { computed, ref } from "vue";

// Định nghĩa các thuộc tính đầu vào (Props) cho Component
const props = defineProps({
  // Giá trị ràng buộc dữ liệu (v-model)
  modelValue: {
    type: [String, Number],
    default: "",
  },
  // Loại input (text, password, number,...)
  type: {
    type: String,
    default: "text",
  },
  // Gợi ý khi ô trống
  placeholder: {
    type: String,
    default: "",
  },
  // Độ rộng của container (Tailwind class)
  width: {
    type: String,
    default: "flex-1",
  },
  // Chiều cao tối đa (Tailwind class)
  maxHeight: {
    type: String,
    default: "max-h-[28px]",
  },
  // Căn lề văn bản trong input (left/right)
  placeholderAlign: {
    type: String,
    default: "left",
    validator: (value) => ["left", "right"].includes(value),
  },
  // Nhãn hiển thị bên trái input
  label: {
    type: String,
    default: "",
  },
  // Trạng thái bắt buộc nhập (hiển thị dấu *)
  required: {
    type: Boolean,
    default: false,
  },
  // Căn lề cho wrapper (start/between/end)
  justify: {
    type: String,
    default: "start",
    validator: (value) => ["start", "between", "end"].includes(value),
  },
  // Thứ tự focus bàn phím
  tabIndex: {
    type: [String, Number],
    default: null,
  },
  // Trạng thái vô hiệu hóa
  isDisabled: {
    type: Boolean,
    default: false,
  },
  // Trạng thái chỉ đọc
  readOnly: {
    type: Boolean,
    default: false,
  },
  // Trạng thái lỗi (hiển thị viền đỏ)
  isError: {
    type: Boolean,
    default: false,
  },
  // Trạng thái cảnh báo (đổi màu chữ)
  isWarning: {
    type: Boolean,
    default: false,
  },
  // Giới hạn số lượng ký tự tối đa
  maxLength: {
    type: [String, Number],
    default: null,
  },
});

// Định nghĩa các sự kiện trả ra ngoài
const emit = defineEmits(["update:modelValue"]);

// Trạng thái nội bộ: Đã từng tương tác (focus/blur) hay chưa
const isTouched = ref(false);

/**
 * Tính toán độ rộng của container dựa trên prop width truyền vào
 * @returns {string} Tailwind class width
 * Created by: HoanTD (06/12/2025)
 */
const containerWidth = computed(() => props.width);

/**
 * Tính toán chiều cao tối đa của container dựa trên prop maxHeight
 * @returns {string} Tailwind class height
 * Created by: HoanTD (06/12/2025)
 */
const containerHeight = computed(() => props.maxHeight);

/**
 * Tạo CSS Variable để truyền vào Style, xử lý căn lề linh hoạt cho input field
 * @returns {Object} Đối tượng chứa CSS variable
 * Created by: HoanTD (06/12/2025)
 */
const placeholderStyle = computed(() => ({
  "--placeholder-align": props.placeholderAlign,
}));

/**
 * Tính toán class căn lề nội dung cho toàn bộ hàng (wrapper)
 * @returns {string} Tailwind class justify
 * Created by: HoanTD (06/12/2025)
 */
const getJustifyClass = computed(() => {
  return "justify-" + props.justify;
});

/**
 * Xử lý sự kiện khi người dùng rời khỏi ô input (Blur).
 * Đánh dấu là đã tương tác để bắt đầu hiển thị lỗi validation nếu có.
 * Created by: HoanTD (08/12/2025)
 */
const handleBlur = () => {
  isTouched.value = true;
};

/**
 * Logic quyết định việc hiển thị viền lỗi.
 * Chỉ hiển thị lỗi khi có trạng thái lỗi (isError) VÀ người dùng đã từng tương tác (isTouched).
 * @returns {boolean} True nếu cần hiển thị viền đỏ
 * Created by: HoanTD (08/12/2025)
 */
const shouldShowErrorBorder = computed(() => {
  return props.isError && isTouched.value;
});
</script>

<style scoped>
.ms-input-label {
  width: 180px;
  color: #262626;
  font-weight: 500;
  font-size: 13px;
}
.text-required {
  color: #e54848;
}
.text-red {
  color: #ff0000 !important;
}
.ms-input-container {
  border: 1px solid #d1d5db;
  font-size: 13px;
  min-width: 0;
}
.ms-input-container:focus-within {
  border-color: #009b71;
}
.ms-input-container.border-error {
  border-color: #ff0000;
}
.ms-input-field {
  padding: 5px 12px;
  font-size: 13px;
  font-weight: 400;
  text-align: var(--placeholder-align, left);
}
.ms-input-icon {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  display: flex;
  align-items: center;
  justify-content: center;
  pointer-events: none;
}
.ms-input-field.has-icon-left {
  padding-left: 32px;
}
.ms-input-field.has-icon-right {
  padding-right: 32px;
}
.ms-input-field::placeholder {
  font-size: 13px;
  font-weight: 400;
  color: #9ca3af;
  text-align: var(--placeholder-align, left);
}
.ms-disabled {
  background-color: #f3f4f6;
  color: #4b5563;
}
</style>