<template>
  <div class="ms-table-container flex-1 bg-white">
    <div class="ms-table-wrapper" ref="tableWrapper">
      <table class="ms-table">
        <thead class="ms-thead">
          <tr class="ms-tr header-row">
            <th class="ms-th th-first-cell fixed-left">
              <ms-checkbox
                class="min-w-10"
                v-model:checked="isAllSelected"
                style="border-right: 1px solid #d1d5db"
              />
            </th>

            <th
              v-for="(col, index) in columns"
              :key="col.field"
              class="ms-th"
              :class="{
                'fixed-left': col.fixed === 'left',
                'fixed-right': col.fixed === 'right',
                'has-filter': col.filterable,
                'has-sort': col.sortable,
              }"
              :style="getThStyle(col, index)"
              @click.stop="handleHeaderClick(col)"
            >
              <div
                class="flex-container"
                :class="col.align == 'right' ? 'justify-end' : 'justify-between'"
              >
                <div class="flex items-center">
                  <span class="truncate">{{ col.title }}</span>
                  <div v-if="col.sortable && getColumnSortDirection(col) !== 'NONE'" class="ml-2">
                    <div
                      :class="[
                        getColumnSortDirection(col) === 'ASC' ? 'icon_arrow-up' : 'icon_arrow-down',
                        'icon-16',
                      ]"
                    ></div>
                  </div>
                </div>
                <div
                  v-if="col.filterable"
                  class="filter-icon-container"
                  @click.stop="toggleFilterMenu(col)"
                >
                  <div v-if="isColumnFiltered(col)" class="icon_filter--active icon-16"></div>
                  <div v-else class="icon_filter--default icon-16"></div>
                </div>
              </div>

              <div v-if="activeHeaderMenu === col.field && col.sortable" class="header-menu">
                <ul class="menu-list">
                  <li class="menu-item" @click.stop="handleSort(col, 'NONE')">
                    <div class="icon_empty icon-16"></div>
                    <span class="menu-text">Không sắp xếp</span>
                  </li>
                  <li class="menu-item" @click.stop="handleSort(col, 'ASC')">
                    <div class="icon_arrow-up icon-16"></div>
                    <span class="menu-text">Tăng dần</span>
                  </li>
                  <li class="menu-item" @click.stop="handleSort(col, 'DESC')">
                    <div class="icon_arrow-down icon-16"></div>
                    <span class="menu-text">Giảm dần</span>
                  </li>
                  <hr class="menu-divider" />
                  <li class="menu-item" @click.stop="handlePinning(col, 'left')">
                    <div class="icon_pin icon-16"></div>
                    <span class="menu-text">Ghim cột</span>
                  </li>
                  <li class="menu-item" @click.stop="handlePinning(col, null)">
                    <div class="icon_unpin icon-16"></div>
                    <span class="menu-text">Bỏ ghim cột</span>
                  </li>
                </ul>
              </div>

              <div v-if="activeFilterMenu === col.field" class="filter-popup" @click.stop>
                <div class="filter-content flex flex-col gap-4">
                  <div class="filter-title flex items-center justify-between">
                    <span>Lọc {{ col.title }}</span>
                    <div
                      class="icon_close icon-16 icon-hover"
                      @click.stop="toggleFilterMenu(col)"
                    ></div>
                  </div>
                  <div class="flex flex-col gap-2">
                    <ms-combobox
                      v-model="filterOperator"
                      :options="getMenuOptions(col)"
                    ></ms-combobox>
                    <ms-input
                      v-if="col.filterType !== 'enum'"
                      v-model="filterValue"
                      placeholder="Nhập giá trị lọc"
                      :readOnly="filterOperator === 'isnull' || filterOperator === 'notnull'"
                    ></ms-input>
                  </div>
                  <div class="filter-actions">
                    <ms-button type="filled-neutral" @click.stop="handleCancelFilter(col.field)"
                      >Bỏ lọc</ms-button
                    >
                    <div class="flex gap-2">
                      <ms-button type="outlined" @click.stop="toggleFilterMenu(col)">Hủy</ms-button>
                      <ms-button type="misa" @click.stop="handleFilterColumn(col)"
                        >Áp dụng</ms-button
                      >
                    </div>
                  </div>
                </div>
              </div>
            </th>

            <th
              class="ms-th fixed-right action-col"
              :style="{ right: '0px', width: '80px', minWidth: '80px' }"
            ></th>
          </tr>
        </thead>

        <tbody class="ms-tbody">
          <template v-if="props.loading">
            <tr v-for="i in 10" :key="'skeleton-' + i" class="ms-tr skeleton-row">
              <td
                class="ms-td fixed-left"
                :style="{ left: '0px', width: '30px', minWidth: '30px' }"
              ></td>

              <td
                v-for="(col, colIndex) in columns"
                :key="col.field"
                class="ms-td"
                :class="{
                  'fixed-left': col.fixed === 'left',
                  'fixed-right': col.fixed === 'right',
                }"
                :style="getTdStyle(col, colIndex)"
              >
                <div class="skeleton-line"></div>
              </td>

              <td
                class="ms-td fixed-right action-col"
                :style="{ right: '0px', width: '80px', minWidth: '80px' }"
              ></td>
            </tr>
          </template>
          <template v-else-if="tableData.length > 0">
            <tr
              v-for="(row, rowIndex) in tableData"
              :key="rowIndex"
              class="ms-tr"
              :class="[{ 'row-selected': row.selected }, { 'row-clicked': activeRow === rowIndex }]"
              @click.stop="clickRow(rowIndex)"
              @dblclick="handleRowDoubleClick(row)"
            >
              <td
                class="ms-td fixed-left"
                :style="{ left: '0px', width: '30px', minWidth: '30px' }"
              >
                <ms-checkbox v-model:checked="row.selected"></ms-checkbox>
              </td>

              <td
                v-for="(col, colIndex) in columns"
                :key="col.field"
                class="ms-td"
                :class="{
                  'fixed-left': col.fixed === 'left',
                  'fixed-right': col.fixed === 'right',
                }"
                :style="getTdStyle(col, colIndex)"
              >
                <template v-if="$slots[col.field]">
                  <slot :name="col.field" :rowData="row"></slot>
                </template>
                <template v-else>
                  <div class="truncate">{{ row[col.field] || "-" }}</div>
                </template>
              </td>

              <td
                class="ms-td fixed-right action-col"
                :style="{ right: '0px', width: '80px', minWidth: '80px' }"
              >
                <div class="action-icons-container">
                  <div class="action-icon-wrapper" @click="handleOpenEditPopup(row)">
                    <div class="icon_pencil icon-16 action-icon"></div>
                  </div>
                  <div class="action-icon-wrapper" @click.stop="toggleActionMenu(rowIndex)">
                    <div class="icon_feature-more icon-16 action-icon"></div>
                  </div>
                  <div v-if="activeRowActionMenu === rowIndex" class="action-menu" @click.stop>
                    <ul class="menu-list">
                      <li class="menu-item" @click.stop="handleAction('duplicate', row)">
                        <div class="icon_duplicate icon-16"></div>
                        <span class="menu-text">Nhân bản</span>
                      </li>
                      <li
                        v-if="row.workShiftStatus === STATUS.INACTIVE"
                        class="menu-item"
                        @click.stop="handleAction('updateStatus', row)"
                      >
                        <div class="icon_active icon-16"></div>
                        <span class="menu-text">Sử dụng</span>
                      </li>
                      <li v-else class="menu-item" @click.stop="handleAction('updateStatus', row)">
                        <div class="icon_empty icon-16"></div>
                        <span class="menu-text">Ngừng sử dụng</span>
                      </li>
                      <li class="menu-item" @click.stop="handleAction('delete', row)">
                        <div class="icon_trash icon-16"></div>
                        <span class="menu-text">Xóa</span>
                      </li>
                    </ul>
                  </div>
                </div>
              </td>
            </tr>
          </template>
          <template v-else>
            <tr class="w-[150px] absolute table-no-data">
              Không có dữ liệu
            </tr>
          </template>
        </tbody>
      </table>
    </div>
    <div v-if="props.loading" class="loading-overlay"></div>
  </div>
</template>

<script setup>
import MsCheckbox from "../ms-checkbox/MsCheckbox.vue";
import MsCombobox from "../ms-combobox/MsCombobox.vue";
import MsInput from "../ms-input/MsInput.vue";
import MsButton from "../ms-button/MsButton.vue";
import { ref, computed, onMounted, watch } from "vue";
import { STATUS } from "@/enums/Enum";
import {
  TEXT_CONDITION_OPTIONS,
  NUMBER_CONDITION_OPTIONS,
  STATUS_CONDITION_OPTIONS,
} from "@/constants/filterConditions";
import { getLabelByValue } from "@/utils/commonFns";

// props
const props = defineProps({
  columns: { type: Array, required: true },
  data: { type: Array, required: true },
  loading: { type: Boolean, default: false },
  sortColumn: { type: Array, default: [] },
  filterColumn: { type: Array, default: [] },
});

// emit
const emit = defineEmits([
  "openEdit",
  "doubleClickRow",
  "action",
  "update:selectedRows",
  "sort",
  "filterColumn",
  "cancelFilterColumn",
]);

// state
const tableData = ref([]);
const activeHeaderMenu = ref(null);
const activeFilterMenu = ref(null);
const activeRowActionMenu = ref(null);
const activeRow = ref(null);
const filterOperator = ref(null);
const filterValue = ref(null);

/// <summary>
/// Theo dõi giá trị của filterOperator
/// clear filterValue khi filterOperator check null
/// </summary>
/// <param name="newVal">Giá trị operator mới</param>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
watch(
  () => filterOperator.value,
  (newVal) => {
    if (newVal === "isnull" || newVal === "notnull") {
      filterValue.value = null;
    }
  }
);

/// <summary>
/// Lấy hướng sắp xếp của một cột
/// </summary>
/// <param name="col">Đối tượng cột</param>
/// <returns>Chuỗi DESC, ASC hoặc NONE</returns>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const getColumnSortDirection = (col) => {
  const sortItem = props.sortColumn.find((item) => item.selector === col.field);
  if (sortItem) {
    return sortItem.desc ? "DESC" : "ASC";
  }
  return "NONE";
};

/// <summary>
/// Kiểm tra xem cột có đang được áp dụng lọc hay không
/// </summary>
/// <param name="col">Cột hiện tại</param>
/// <returns>Boolean</returns>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const isColumnFiltered = (col) => {
  const filterItem = props.filterColumn.find((item) => item.columnName === col.field);
  if (filterItem) {
    return true;
  }
  return false;
};

/// <summary>
/// Emit danh sách các hàng đã chọn lên component cha
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const emitSelectedRows = () => {
  const selectedRows = tableData.value.filter((row) => row.selected);
  emit("update:selectedRows", selectedRows);
};

/// <summary>
/// Hàm cập nhật data cho table khi props data thay đổi
/// </summary>
/// <param name="newVal">Dữ liệu mới từ props</param>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
watch(
  () => props.data,
  (newVal) => {
    const newLocalData = newVal.map((item) => {
      const existingRow = tableData.value.find((r) => r.workShiftId === item.workShiftId);

      return {
        ...item,
        selected: existingRow ? existingRow.selected : false,
      };
    });
    tableData.value = newLocalData;
    emitSelectedRows();
  },
  { immediate: true, deep: true }
);

/// <summary>
/// Hàm selected tất cả
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const isAllSelected = computed({
  get() {
    if (tableData.value.length === 0) return false;

    return tableData.value.every((row) => row.selected);
  },
  set(newValue) {
    tableData.value.forEach((row) => {
      row.selected = newValue;
    });
    emitSelectedRows();
  },
});

/// <summary>
/// Theo dõi thay đổi trạng thái selected của từng row
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
watch(
  () => tableData.value.map((row) => row.selected),
  () => {
    emitSelectedRows();
  },
  { deep: true }
);

/// <summary>
/// Hàm tính toán style cho header cell - Thuật toán Sticky Position
/// </summary>
/// <param name="col">Cột cần tính</param>
/// <param name="index">Vị trí của cột</param>
/// <returns>CSS Style Object</returns>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const getThStyle = (col, index) => {
  const style = {};
  if (col.width) style.width = col.width;
  if (col.minWidth) style.minWidth = col.minWidth;
  if (col.maxWidth) style.maxWidth = col.maxWidth;

  // Tính toán vị trí bên trái cho cột ghim bên trái
  if (col.fixed === "left") {
    let leftOffset = 30; // 30px là độ rộng cột checkbox ghim đầu tiên
    for (let i = 0; i < index; i++) {
      if (props.columns[i].fixed === "left") {
        leftOffset += parseInt(props.columns[i].width || props.columns[i].minWidth || "150px");
      }
    }
    style.left = `${leftOffset}px`;
  } 
  // Tính toán vị trí bên phải cho cột ghim bên phải
  else if (col.fixed === "right") {
    let rightOffset = 80; // 80px là độ rộng cột action ghim cuối cùng
    for (let i = props.columns.length - 1; i > index; i--) {
      if (props.columns[i].fixed === "right") {
        rightOffset += parseInt(props.columns[i].width || props.columns[i].minWidth || "150px");
      }
    }
    style.right = `${rightOffset}px`;
  }

  return style;
};

/// <summary>
/// Hàm style cho body cell
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const getTdStyle = (col, index) => {
  const style = {};
  if (col.width) style.width = col.width;
  if (col.minWidth) style.minWidth = col.minWidth;
  if (col.maxWidth) style.maxWidth = col.maxWidth;

  if (col.align) {
    style.textAlign = col.align;
  }
  return style;
};

/// <summary>
/// Đóng/mở action menu
/// </summary>
/// <param name="index">Index của row</param>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const toggleActionMenu = (index) => {
  if (activeRowActionMenu.value === index) {
    activeRowActionMenu.value = null;
  } else {
    activeRowActionMenu.value = index;
    activeHeaderMenu.value = null;
    activeFilterMenu.value = null;
  }
};

/// <summary>
/// Click vào table header cell
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleHeaderClick = (col) => {
  if (activeHeaderMenu.value === col.field) {
    activeHeaderMenu.value = null;
  } else {
    activeHeaderMenu.value = col.field;
    activeFilterMenu.value = null;
  }
};

/// <summary>
/// Đóng/mở filter menu
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const toggleFilterMenu = (col) => {
  if (!col.filterable) return;
  if (activeFilterMenu.value === col.field) {
    activeFilterMenu.value = null;
  } else {
    activeFilterMenu.value = col.field;
    activeHeaderMenu.value = null;

    const existedFilter = props.filterColumn.find((item) => item.columnName === col.field);
    if (existedFilter) {
      filterOperator.value = existedFilter.filterOperator;
      filterValue.value = existedFilter.filterValue;
    } else {
      filterValue.value = null;
    }
  }
};

/// <summary>
/// Lấy danh sách tùy chọn cho menu lọc dựa trên kiểu dữ liệu của cột
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const getMenuOptions = (col) => {
  if (col.filterType === "number") {
    return NUMBER_CONDITION_OPTIONS;
  } else if (col.filterType === "enum") {
    return STATUS_CONDITION_OPTIONS;
  }
  return TEXT_CONDITION_OPTIONS;
};

/// <summary>
/// Sắp xếp cột
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleSort = (col, direction) => {
  emit("sort", col.field, direction);
  activeHeaderMenu.value = null;
};

/// <summary>
/// Lọc cột
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleFilterColumn = (col) => {
  const label = getLabelByValue(getMenuOptions(col), filterOperator.value);
  emit("filterColumn", col, filterOperator.value, filterValue.value, label);
  activeFilterMenu.value = null;
};

/// <summary>
/// Xử lý ghim/bỏ ghim cột
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handlePinning = (col, fixedSide) => {
  console.log(`Pinning column ${col.field} to ${fixedSide}`);
  activeHeaderMenu.value = null;
};

/// <summary>
/// Style khi click vào row
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const clickRow = (index) => {
  activeRow.value = index;
};

/// <summary>
/// Xử lý sự kiện mở popup sửa
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleOpenEditPopup = (row) => {
  emit("openEdit", row);
};

/// <summary>
/// Xử lý sự kiện Double-click vào row
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleRowDoubleClick = (row) => {
  emit("doubleClickRow", row);
};

/// <summary>
/// Xử lý các hành động (Nhân bản, Sử dụng, Xóa)
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleAction = (action, row) => {
  emit("action", action, row);
  activeRowActionMenu.value = null;
};

/// <summary>
/// Bỏ chọn tất cả các hàng
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const unselectAll = () => {
  tableData.value.forEach((row) => {
    row.selected = false;
  });
  emitSelectedRows();
};

/// <summary>
/// Xử lý sự kiện bỏ lọc cột
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleCancelFilter = (field) => {
  emit("cancelFilterColumn", field);
};

/// <summary>
/// Gán sự kiện khi click ra ngoài để đóng menu
/// </summary>
/// <remarks>Created by: HoanTD (16/12/2025)</remarks>
const handleClickOutside = () => {
  if (activeHeaderMenu.value || activeFilterMenu.value) {
    activeHeaderMenu.value = null;
    activeFilterMenu.value = null;
  }
  if (activeRowActionMenu.value !== null) {
    activeRowActionMenu.value = null;
  }
};

defineExpose({
  unselectAll,
  filterValue,
});

onMounted(() => {
  document.addEventListener("click", handleClickOutside);
});
</script>

<style scoped>
/* container */
.ms-table-container {
  height: 100%;
  border: none;
  font-size: 13px;
  overflow: auto;
}
.ms-table {
  table-layout: auto;
  width: 100%;
  border-collapse: collapse;
}
/* header */
.ms-thead {
  position: sticky;
  top: 0;
  z-index: 20;
}
.header-row {
  height: 30px;
  color: #1f2937;
}
.ms-th {
  text-align: left;

  font-size: 13px;
  font-weight: 600;
  background-color: #f3f4f6;
  border-bottom: 1px solid #d1d5db;
  cursor: pointer;
  height: 30px;
  color: #262626;
  position: sticky;
  top: 0;
  box-sizing: border-box;
}
.th-first-cell {
  left: 0px;
  width: 30px;
  height: 16px;
}
.flex-container {
  display: flex;
  align-items: center;
  height: 16px;
  border-right: 1px solid #d1d5db;
  padding-inline: 16px;
}

/* --- Fixed Columns --- */
.ms-th.fixed-left,
.ms-td.fixed-left {
  position: sticky;
  left: 0;
  z-index: 10;
  background-color: #f3f4f6;
}

.ms-td.fixed-left {
  background-color: white;
}
.ms-tr.row-selected .ms-td.fixed-left {
  background-color: #a4f6d3;
}

.ms-th.fixed-right,
.ms-td.fixed-right {
  position: sticky;
  right: 0;
  z-index: 10;
  background-color: #f3f4f6;
}
.ms-td.fixed-right {
  background-color: white;
}
.ms-tr.row-selected .ms-td.fixed-right {
  background-color: #a4f6d3;
}

/* --- Filter Icon --- */
.filter-icon-container .icon_filter--default {
  opacity: 0;
  transition: opacity 150ms ease-in-out;
}
.ms-th.has-filter:hover .filter-icon-container .icon_filter--default {
  opacity: 1;
}

/* --- Header Menu (Sắp xếp, Ghim) --- */
.header-menu {
  position: absolute;
  z-index: 999;
  top: 30px;
  background-color: #ffffff;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05); /* shadow-lg */
  border: 1px solid #e5e7eb;
  border-radius: 0.375rem;
  width: 10rem;
  font-size: 13px;
  animation: slide-down 0.2s ease;
}
.menu-list {
  padding-top: 0.25rem;
  padding-bottom: 0.25rem;
}
.menu-item {
  display: flex;
  align-items: center;
  padding-left: 0.75rem;
  padding-right: 0.75rem;
  padding-top: 0.25rem;
  padding-bottom: 0.25rem;
  cursor: pointer;
}
.menu-item:hover {
  background-color: #f3f4f6;
}
.menu-text {
  margin-left: 0.5rem;
  color: #111827;
  font-weight: 400;
}
.menu-divider {
  margin-top: 0.25rem;
  margin-bottom: 0.25rem;
  border-color: #e5e7eb;
}

/* --- Filter Popup (Lọc) --- */
.filter-popup {
  width: 350px;
  position: absolute;
  z-index: 40;
  top: 32px;
  left: 0;
  background-color: #ffffff;
  box-shadow: 0 0 8px #0000001a, 0 8px 16px #0000001a;
  border: 1px solid #e5e7eb;
  border-radius: 4px;
  font-size: 13px;
}
.filter-content {
  padding: 16px;
}
.filter-title {
  font-size: 16px;
  font-weight: 600;
}
.filter-input {
  width: 100%;
  border: 1px solid #d1d5db;
  padding: 0.25rem;
  border-radius: 0.25rem;
  font-size: 0.875rem;
}
.filter-actions {
  display: flex;
  justify-content: space-between;
}
/* ---------------------------------------------------- */
/* 3. BODY STYLES (ms-tbody, ms-tr, ms-td)              */
/* ---------------------------------------------------- */

.ms-tr {
  position: relative;
  height: 32px;
  border-bottom: 1px solid #e5e7eb;
  font-size: 13px;
  color: #111827;
}
.ms-td {
  padding-left: 16px;
  padding-right: 16px;
  white-space: nowrap;
  background-color: #fff;
}

/* Hover State */
.ms-tr:hover .ms-td {
  background-color: #e5e7eb;
}
.ms-tr.row-clicked .ms-td {
  background-color: #a4f6d3;
}
/* Selected Row State */
.ms-tr.row-selected .ms-td {
  background-color: #a4f6d3;
}
.ms-tr.row-clicked .ms-td {
  background-color: #a4f6d3;
}
.action-icons-container {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
}
.action-icon-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 24px;
  width: 24px;
  background-color: #fff;
  border-radius: 50%;
  cursor: pointer;
  transition: color 150ms ease-in-out;
}
.action-icon-wrapper .action-icon:hover {
  background-color: #009b71;
}
.table-no-data {
  text-align: center;
  font-style: normal;
  color: #111827;
  top: 30%;
  left: 50%;
  transform: translate(-50%, 100%);
}
/* --- Loading Overlay --- */
.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(255, 255, 255, 0.6);
  z-index: 50;
  pointer-events: none;
}
@keyframes loading-pulse {
  0% {
    background-color: #f3f4f6; /* light gray */
  }
  50% {
    background-color: #e5e7eb; /* slightly darker gray */
  }
  100% {
    background-color: #f3f4f6;
  }
}

.skeleton-row .ms-td {
  background-color: #fff !important;
}

.skeleton-line {
  height: 12px;
  width: 80%;
  border-radius: 4px;
  animation: loading-pulse 1.5s infinite ease-in-out;
}

/* --- Action Menu (Menu cho từng hàng) --- */
.action-menu {
  position: absolute;
  z-index: 999;
  top: 0;
  right: 88px;
  background-color: #ffffff;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
  border: 1px solid #e5e7eb;
  border-radius: 0.375rem;
  font-size: 13px;
  animation: slide-down 0.2s ease;
  margin-top: 4px;
}

.action-menu .menu-list {
  padding-top: 0.25rem;
  padding-bottom: 0.25rem;
}

.action-menu .menu-item {
  display: flex;
  align-items: center;
  padding-left: 0.75rem;
  padding-right: 0.75rem;
  padding-top: 0.5rem;
  padding-bottom: 0.5rem;
  cursor: pointer;
}
.action-menu .menu-item:hover {
  background-color: #f3f4f6;
}
.action-menu .menu-text {
  margin-left: 0.5rem;
  color: #111827;
  font-weight: 400;
}
.action-col .relative {
  position: relative;
  height: 100%;
  display: flex;
  align-items: center;
}
</style>