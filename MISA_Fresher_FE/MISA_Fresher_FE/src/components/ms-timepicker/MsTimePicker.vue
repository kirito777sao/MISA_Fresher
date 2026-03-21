<template>
  <div
    class="ms-timepicker-wrapper flex items-center w-full justify-between"
    :class="{ 'justify-end': isAlignEnd }"
  >
    <label v-if="label" class="ms-input-label flex-shrink-0">
      {{ label }}
      <span v-if="required" class="text-red-500 ml-0.5">*</span>
    </label>

    <div
      class="ms-timepicker-container relative"
      :class="props.width"
      v-click-outside="closeDropdown"
    >
      <div
        class="ms-input-box ms-input-container flex items-center rounded-sm bg-white overflow-hidden"
        :class="[
          containerHeight,
          { 'is-focus': isDropdownOpen },
          { 'border-error': shouldShowErrorBorder },
        ]"
      >
        <input
          type="text"
          :placeholder="placeholder"
          :value="modelValue"
          @input="handleInput"
          @blur="handleBlur"
          class="ms-input-field w-full h-full text-sm bg-transparent border-none outline-none"
          :style="placeholderStyle"
          maxlength="5"
          :tabindex="tabIndex"
        />

        <div
          class="ms-input-icon icon_time icon-16 right-3 ml-1 cursor-pointer"
          @mousedown.prevent="toggleDropdown"
        ></div>
      </div>

      <div
        v-if="isDropdownOpen"
        class="ms-timepicker-dropdown absolute z-10 w-full rounded-sm shadow-md mt-1 bg-white max-h-[200px] overflow-y-auto"
      >
        <div
          v-for="time in timeOptions"
          :key="time"
          class="time-option text-sm px-3 py-1.5 cursor-pointer"
          :class="{ 'is-selected': time === modelValue }"
          @click="selectTime(time)"
        >
          {{ time }}
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, onMounted } from "vue";

/**
 * Các props định nghĩa cấu hình cho Timepicker
 * @property {String} modelValue - Giá trị thời gian hiện tại (HH:mm)
 * @property {String} label - Nhãn hiển thị bên trái component
 * @property {String} placeholder - Văn bản hiển thị mặc định khi rỗng
 * @property {Boolean} required - Hiển thị dấu * đỏ nếu bắt buộc
 * @property {String} maxHeight - Giới hạn chiều cao của ô input
 * @property {String} placeholderAlign - Căn lề cho placeholder (left/right)
 * @property {String} width - Class quy định độ rộng (mặc định flex-1)
 * @property {Boolean} isAlignEnd - Căn phải cho toàn bộ wrapper
 * @property {Boolean} isError - Trạng thái lỗi truyền từ ngoài vào
 * @property {String|Number} tabIndex - Thứ tự tabindex
 */
const props = defineProps({
  modelValue: { type: String, default: "" },
  label: { type: String, default: "" },
  placeholder: { type: String, default: "HH:MM" },
  required: { type: Boolean, default: false },
  maxHeight: { type: String, default: "max-h-[28px]" },
  placeholderAlign: {
    type: String,
    default: "left",
    validator: (value) => ["left", "right"].includes(value),
  },
  width: {
    type: String,
    default: "flex-1",
  },
  isAlignEnd: {
    type: Boolean,
    default: false,
  },
  isError: {
    type: Boolean,
    default: false,
  },
  tabIndex: {
    type: [String, Number],
    default: null,
  },
});

const emit = defineEmits(["update:modelValue"]);

// Quản lý trạng thái tương tác và danh sách gợi ý
const isTouched = ref(false);
const isDropdownOpen = ref(false);
const timeOptions = ref([]);

/// <summary>
/// Tính toán các thuộc tính CSS dựa trên props truyền vào
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const containerHeight = computed(() => props.maxHeight);
const placeholderStyle = computed(() => ({
  "--placeholder-align": props.placeholderAlign,
}));

/// <summary>
/// Xác định trạng thái hiển thị viền lỗi (chỉ hiện khi đã touched và props isError = true)
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const shouldShowErrorBorder = computed(() => {
  return props.isError && isTouched.value;
});

/// <summary>
/// Directive xử lý sự kiện click bên ngoài component để tự đóng dropdown
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const vClickOutside = {
  mounted: (el, binding) => {
    el.__ClickOutsideHandler__ = (event) => {
      if (!(el === event.target || el.contains(event.target))) {
        binding.value(event);
      }
    };
    document.body.addEventListener("click", el.__ClickOutsideHandler__, true);
  },
  unmounted: (el) => {
    document.body.removeEventListener("click", el.__ClickOutsideHandler__, true);
  },
};

/// <summary>
/// Khởi tạo danh sách các mốc thời gian cách nhau mỗi 30 phút
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const generateTimeOptions = () => {
  const options = [];
  for (let h = 0; h < 24; h++) {
    for (let m = 0; m < 60; m += 30) {
      const hour = String(h).padStart(2, "0");
      const minute = String(m).padStart(2, "0");
      options.push(`${hour}:${minute}`);
    }
  }
  timeOptions.value = options;
};

/// <summary>
/// Xử lý định dạng thời gian trực tiếp khi người dùng nhập từ bàn phím
/// Đảm bảo format HH:MM và giới hạn giá trị hợp lệ (Max 23:59)
/// </summary>
/// <param name="event">Input event</param>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleInput = (event) => {
  let rawValue = event.target.value;
  let numbers = rawValue.replace(/[^0-9]/g, "");
  let formattedValue = "";

  if (numbers.length >= 1) {
    let hour = numbers.slice(0, 2);
    if (hour.length === 2 && parseInt(hour, 10) > 23) {
      hour = "23";
    }
    formattedValue += hour;
  }

  if (numbers.length >= 3) {
    formattedValue += ":";
    let minute = numbers.slice(2, 4);
    if (minute.length === 2 && parseInt(minute, 10) > 59) {
      minute = "59";
    }
    formattedValue += minute;
  }

  if (formattedValue.length > 5) {
    formattedValue = formattedValue.slice(0, 5);
  }

  emit("update:modelValue", formattedValue);
};

/// <summary>
/// Kiểm tra và chuẩn hóa dữ liệu khi người dùng rời khỏi ô nhập liệu
/// Tự động thêm số 0 hoặc xóa nếu dữ liệu không đủ điều kiện thời gian
/// </summary>
/// <param name="event">Blur event</param>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleBlur = (event) => {
  isTouched.value = true;
  let currentValue = event.target.value;
  const cleanValue = currentValue.replace(/[^0-9]/g, "");

  if (cleanValue.length === 4) {
    const hour = parseInt(cleanValue.substring(0, 2), 10);
    const minute = parseInt(cleanValue.substring(2, 4), 10);

    if (hour >= 0 && hour <= 23 && minute >= 0 && minute <= 59) {
      const formattedHour = String(hour).padStart(2, "0");
      const formattedMinute = String(minute).padStart(2, "0");
      const finalValue = `${formattedHour}:${formattedMinute}`;

      if (props.modelValue !== finalValue) {
        emit("update:modelValue", finalValue);
      }
      return;
    }
  }

  if (currentValue !== "") {
    emit("update:modelValue", "");
  }
};

/// <summary>
/// Điều khiển đóng/mở và lựa chọn dữ liệu từ dropdown
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const openDropdown = () => {
  isDropdownOpen.value = true;
};

const closeDropdown = () => {
  isDropdownOpen.value = false;
};

const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
};

const selectTime = (time) => {
  emit("update:modelValue", time);
  closeDropdown();
};

onMounted(() => {
  generateTimeOptions();
});
</script>

<style scoped>
/* Nhãn tiêu đề */
.ms-input-label {
  width: 150px;
  color: #262626;
  font-weight: 500;
  font-size: 13px;
}

/* Khung viền bao bọc input */
.ms-input-container {
  border: 1px solid #d1d5db;
  font-size: 13px;
  transition: border-color 0.2s ease;
}

/* Hiển thị viền đỏ khi có lỗi */
.ms-input-container.border-error {
  border-color: #ff0000;
}

.ms-input-container:hover {
  border-color: #9ca3af;
}

/* Highlight viền khi focus hoặc đang mở dropdown */
.ms-input-container:focus-within,
.ms-input-container.is-focus {
  border-color: #009b71;
}

/* Ô input văn bản */
.ms-input-field {
  padding: 5px 12px;
  font-size: 13px;
  font-weight: 400;
  padding-right: 32px;
}

.ms-input-field::placeholder {
  font-size: 13px;
  font-weight: 400;
  color: #9ca3af;
  text-align: var(--placeholder-align, left);
}

/* Icon đồng hồ bên phải */
.ms-input-icon {
  position: absolute;
  top: 50%;
  right: 12px;
  transform: translateY(-50%);
  pointer-events: none;
}

.ms-input-icon.cursor-pointer {
  pointer-events: all;
}

/* Khung dropdown chứa danh sách giờ */
.ms-timepicker-dropdown {
  border: 1px solid #d1d5db;
  position: absolute;
  top: 100%;
  left: 0;
  background-color: white;
  z-index: 10;
}

/* Từng dòng option trong danh sách */
.time-option {
  font-size: 13px;
  color: #374151;
}

.time-option:hover {
  background-color: #f3f4f6;
  color: #009b71;
}

/* Style cho option đang được chọn */
.time-option.is-selected {
  background-color: #e0f2f1;
  color: #009b71;
  font-weight: 600;
}
</style>