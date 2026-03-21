<template>
  <div class="ms-combobox" ref="comboBoxRef">
    <div class="ms-combobox__trigger" :class="{ 'ms-combobox__trigger--active': isDropdownOpen }">
      <input
        type="text"
        v-model="searchText"
        @input="filterOptions"
        @keydown.enter="handleEnter"
        @keydown.up.prevent="moveHighlight(-1)"
        @keydown.down.prevent="moveHighlight(1)"
        @focus="openDropdown"
        class="ms-combobox__input"
        :placeholder="placeholder"
      />
      <div
        class="icon_angle-down icon-16"
        :class="isDropdownOpen ? 'rotate-180' : ''"
        @click.stop="toggleDropdown"
      ></div>
    </div>

    <transition name="dropdown-slide">
      <div v-if="isDropdownOpen" class="ms-combobox__dropdown">
        <ul class="ms-combobox__list">
          <li
            v-for="(option, index) in filteredOptions"
            :key="option.value"
            class="ms-combobox__item"
            :class="{
              'ms-combobox__item--highlight': index === highlightedIndex,
              'ms-combobox__item--selected': option.value === model,
            }"
            @click="selectOption(option)"
            @mouseenter="highlightedIndex = index"
          >
            <span>{{ option.label }}</span>
            <div v-if="option.value === model" class="icon_select-checked"></div>
          </li>
          <li
            v-if="filteredOptions.length === 0"
            class="ms-combobox__item ms-combobox__item--disabled"
          >
            Không tìm thấy kết quả
          </li>
        </ul>
      </div>
    </transition>
  </div>
</template>

<script setup>
/**
 * Service xử lý logic Combobox (AutoComplete)
 * Hỗ trợ: Phân trang tìm kiếm, Điều hướng bàn phím, Tự động chọn giá trị mặc định.
 * Created by: HoanTD (06/12/2025)
 */
import { ref, computed, watch, nextTick } from "vue";
import { onClickOutside } from "@vueuse/core";

// Khởi tạo model binding cho giá trị được chọn
const model = defineModel();

// Khai báo các props đầu vào
const props = defineProps({
  // Danh sách các tùy chọn (Array of Objects)
  options: {
    type: Array,
    required: true,
    validator: (arr) => arr.every((o) => o.label && o.value !== undefined),
  },
  // Văn bản gợi ý trong ô input
  placeholder: {
    type: String,
    default: "",
  },
});

// Khai báo trạng thái nội bộ của Component
const isDropdownOpen = ref(false); // Trạng thái đóng/mở dropdown
const searchText = ref("");        // Nội dung text tìm kiếm trên input
const highlightedIndex = ref(-1);   // Index của item đang được highlight (bằng chuột hoặc phím)
const comboBoxRef = ref(null);      // Tham chiếu tới element cha để xử lý click ngoài

/**
 * Lọc danh sách options dựa trên nội dung searchText (không phân biệt hoa thường)
 * @returns {Array} Danh sách đã lọc
 * Created by: HoanTD (09/12/2025)
 */
const filteredOptions = computed(() => {
  if (!searchText.value) {
    return props.options;
  }
  const searchLower = searchText.value.toLowerCase();
  return props.options.filter((option) => option.label.toLowerCase().includes(searchLower));
});

/**
 * Watcher: Xử lý gán giá trị mặc định khi danh sách options thay đổi (dựa vào thuộc tính 'selected')
 * Created by: HoanTD (06/12/2025)
 */
watch(
  () => props.options,
  (newOptions) => {
    const defaultOption = newOptions.find((o) => o.selected === true);
    if (defaultOption) {
      model.value = defaultOption.value;
      searchText.value = defaultOption.label;
    } else {
      model.value = null;
      searchText.value = "";
    }
  },
  { immediate: true }
);

/**
 * Watcher: Cập nhật text hiển thị trên Input khi giá trị model thay đổi từ bên ngoài
 * Created by: HoanTD (06/12/2025)
 */
watch(model, (newValue) => {
  if (newValue !== null && newValue !== undefined) {
    const selectedOption = props.options.find((o) => o.value === newValue);
    if (selectedOption) {
      searchText.value = selectedOption.label;
    }
  } else {
    searchText.value = "";
  }
});

/**
 * Đảo ngược trạng thái đóng/mở của dropdown
 * Created by: HoanTD (06/12/2025)
 */
const toggleDropdown = () => {
  isDropdownOpen.value = !isDropdownOpen.value;
  if (isDropdownOpen.value) {
    resetHighlight();
    searchText.value = ""; // Xóa text để người dùng thấy toàn bộ list khi click nút drop
  }
};

/**
 * Thực hiện mở dropdown và đồng bộ trạng thái highlight
 * Created by: HoanTD (06/12/2025)
 */
const openDropdown = () => {
  if (!isDropdownOpen.value) {
    isDropdownOpen.value = true;
    resetHighlight();
    searchText.value = "";
  }
};

/**
 * Đóng dropdown và khôi phục text hiển thị theo giá trị đã chọn
 * Created by: HoanTD (06/12/2025)
 */
const closeDropdown = () => {
  isDropdownOpen.value = false;
  if (model.value !== null && model.value !== undefined) {
    const selectedOption = props.options.find((o) => o.value === model.value);
    if (selectedOption) {
      searchText.value = selectedOption.label;
    }
  } else {
    searchText.value = "";
  }
};

/**
 * Xử lý khi người dùng nhập text: Mở dropdown và reset highlight
 * Created by: HoanTD (06/12/2025)
 */
const filterOptions = () => {
  isDropdownOpen.value = true;
  highlightedIndex.value = -1;
  // Nếu text nhập vào không khớp với item đã chọn, reset model về null
  if (
    !filteredOptions.value.some(
      (o) => o.value === model.value && o.label.toLowerCase() === searchText.value.toLowerCase()
    )
  ) {
    model.value = null;
  }
};

/**
 * Xử lý chọn một tùy chọn từ danh sách
 * @param {Object} option - Tùy chọn người dùng click vào
 * Created by: HoanTD (06/12/2025)
 */
const selectOption = (option) => {
  searchText.value = option.label;
  model.value = option.value;
  closeDropdown();
};

/**
 * Đặt vị trí highlight vào item đang được chọn trong danh sách
 * Created by: HoanTD (06/12/2025)
 */
const resetHighlight = () => {
  const selectedIndex = filteredOptions.value.findIndex((o) => o.value === model.value);
  highlightedIndex.value = selectedIndex !== -1 ? selectedIndex : -1;
};

/**
 * Điều khiển highlight bằng phím mũi tên lên/xuống và tự động scroll danh sách
 * @param {number} direction - Hướng di chuyển (1: xuống, -1: lên)
 * Created by: HoanTD (06/12/2025)
 */
const moveHighlight = (direction) => {
  if (!isDropdownOpen.value) {
    openDropdown();
    return;
  }

  const len = filteredOptions.value.length;
  if (len === 0) return;

  let newIndex = highlightedIndex.value + direction;

  // Xử lý vòng lặp index
  if (newIndex >= len) {
    newIndex = 0;
  } else if (newIndex < 0) {
    newIndex = len - 1;
  }

  highlightedIndex.value = newIndex;

  // Tự động scroll để item được highlight luôn hiển thị trong khung nhìn
  nextTick(() => {
    const listEl = comboBoxRef.value.querySelector(".ms-combobox__list");
    const highlightedEl = listEl?.children[newIndex];
    if (highlightedEl && listEl) {
      const listRect = listEl.getBoundingClientRect();
      const itemRect = highlightedEl.getBoundingClientRect();

      if (itemRect.top < listRect.top) {
        listEl.scrollTop -= listRect.top - itemRect.top;
      } else if (itemRect.bottom > listRect.bottom) {
        listEl.scrollTop += itemRect.bottom - listRect.bottom;
      }
    }
  });
};

/**
 * Xử lý sự kiện nhấn phím Enter để chọn nhanh item đang highlight
 * Created by: HoanTD (06/12/2025)
 */
const handleEnter = () => {
  if (isDropdownOpen.value && highlightedIndex.value !== -1) {
    selectOption(filteredOptions.value[highlightedIndex.value]);
  } else if (filteredOptions.value.length === 1) {
    selectOption(filteredOptions.value[0]);
  } else if (!isDropdownOpen.value) {
    openDropdown();
  }
};

/**
 * Tự động đóng dropdown khi click chuột ra ngoài vùng Component
 * Created by: HoanTD (06/12/2025)
 */
onClickOutside(comboBoxRef, (event) => {
  if (isDropdownOpen.value) {
    closeDropdown();
  }
});
</script>

<style scoped>
.ms-combobox {
  position: relative;
  display: inline-block;
  width: 100%;
  height: 28px;
  font-size: 13px;
  color: #111827;
}

.ms-combobox__trigger {
  padding: 5px 8px 5px 12px;
  height: 100%;
  display: flex;
  align-items: center;
  border: 1px solid #d1d5db;
  border-radius: 4px;
  background-color: #fff;
  transition: all 0.3s ease;
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
}
.ms-combobox__trigger:hover {
  border-color: #9ca3af;
}
.ms-combobox__trigger--active {
  border-color: #009b71;
}
.ms-combobox__input {
  width: 100%;
  font-weight: 400;
  line-height: 1.25rem;
  background-color: transparent;
  border: none;
  outline: none;
}
/* ---------------------------------------------------- */
/* 2. Dropdown List */
/* ---------------------------------------------------- */
.ms-combobox__dropdown {
  position: absolute;
  z-index: 10;
  width: 100%;
  margin-top: 0.25rem;
  background-color: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 4px;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -4px rgba(0, 0, 0, 0.1); /* shadow-lg */
  overflow: hidden;
}
.ms-combobox__list {
  padding-top: 0.25rem;
  padding-bottom: 0.25rem;
  max-height: 15rem; /* max-h-60 */
  overflow-y: auto;
  list-style: none; /* remove default list style */
  margin: 0;
  font-weight: 400;
}

.ms-combobox__item {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.5rem;
  font-size: 13px;
  cursor: pointer;
  transition: background-color 0.15s ease, border-left-width 0.15s ease;
}

.ms-combobox__item:hover {
  background-color: #f3f4f6;
}

.ms-combobox__item--highlight {
  background-color: rgba(220, 252, 231, 0.7);
}

.ms-combobox__item--selected {
  color: #059669;
  font-weight: 500;
  background-color: #f0fdf4;
}

.ms-combobox__item--disabled {
  color: #9ca3af;
  cursor: default;
  background-color: #fff;
}
.dropdown-slide-enter-active,
.dropdown-slide-leave-active {
  transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
  transform-origin: top;
}
.dropdown-slide-enter-from,
.dropdown-slide-leave-to {
  opacity: 0;
  transform: scaleY(0.8);
}
</style>