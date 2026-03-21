<template>
  <!-- 
    Component: WorkShiftList
    Description: Màn hình danh sách Ca làm việc.
    Chức năng bao gồm: 
    1. Hiển thị danh sách ca làm việc dạng bảng (Grid).
    2. Các chức năng trên Toolbar: Tìm kiếm, Lọc, Tải lại.
    3. Các chức năng thao tác dữ liệu: Thêm mới, Sửa, Xóa, Nhân bản.
    4. Các chức năng xử lý hàng loạt: Xóa nhiều, Cập nhật trạng thái nhiều bản ghi.
    
    Created by: HoanTD (04/12/2025)
  -->
  <div class="flex flex-col h-full w-full">
    <!-- Khu vực tiêu đề trang và nút tác vụ chính -->
    <div class="title-box flex justify-between items-center mb-4">
      <div class="font-bold text-2xl">Ca làm việc</div>
      <div class="main-button">
        <!-- Nút Thêm mới: Mở popup để nhập liệu ca làm việc mới -->
        <ms-button icon="icon_add-white" @click="handleOpenAddPopup">Thêm</ms-button>
      </div>
    </div>

    <!-- Layout phần thân: Chứa Toolbar và Grid dữ liệu -->
    <div class="body-layout flex-1 min-h-0 flex flex-col">
      
      <!-- Toolbar: Khu vực công cụ thao tác với danh sách -->
      <div class="tool-box flex justify-between">
        <div class="flex gap-2">
          <!-- Ô tìm kiếm: Tìm kiếm theo từ khóa (Mã hoặc Tên ca) -->
          <ms-input v-model="keyword" placeholder="Tìm kiếm" width="w-[200px]">
            <template #icon-left>
              <div class="icon_search icon-16"></div>
            </template>
          </ms-input>

          <!-- Khu vực hiển thị thao tác hàng loạt (Bulk Actions) -->
          <!-- Chỉ hiển thị khi người dùng có chọn ít nhất 1 dòng trong bảng -->
          <div v-if="isAnySelected" class="flex items-center gap-2">
            <div class="min-w-[60px]">
              Đã chọn <span class="font-bold">{{ selectedCount }}</span>
            </div>
            <!-- Bỏ chọn tất cả -->
            <ms-button type="text" @click="handleUnselectAll">Bỏ chọn</ms-button>
            
            <!-- Nút cập nhật trạng thái "Sử dụng" hàng loạt -->
            <!-- Chỉ hiện khi trong danh sách chọn có bản ghi đang "Ngừng sử dụng" -->
            <ms-button
              v-if="canActivate"
              type="outline-success"
              icon="icon_active--green"
              @click="handleUpdateStatusBatch(selectedWorkShifts, true)"
            >
              Sử dụng
            </ms-button>

            <!-- Nút cập nhật trạng thái "Ngừng sử dụng" hàng loạt -->
            <!-- Chỉ hiện khi trong danh sách chọn có bản ghi đang "Sử dụng" -->
            <ms-button
              v-if="canInactivate"
              type="outline-danger"
              icon="icon_inactive"
              @click="handleUpdateStatusBatch(selectedWorkShifts, false)"
            >
              Ngừng sử dụng
            </ms-button>

            <!-- Nút xóa hàng loạt các bản ghi đã chọn -->
            <ms-button
              type="outline-danger"
              icon="icon_trash"
              @click="handleDeleteBatch(selectedWorkShifts)"
            >
              Xóa
            </ms-button>
          </div>

          <!-- Khu vực hiển thị các Tag lọc (Filter Tags) -->
          <!-- Hiển thị khi không chọn dòng nào VÀ đang có điều kiện lọc được áp dụng -->
          <div
            v-else-if="workShiftFilterRequest.filterColumn.length > 0"
            class="filter-conditions h-full"
          >
            <!-- Render từng điều kiện lọc ra màn hình -->
            <div
              v-for="(filter, index) in workShiftFilterRequest.filterColumn"
              :key="filter.columnName"
            >
              <div class="filter-item">
                <div class="flex items-center gap-2">
                  <span>{{ getColumTitleByField(filter.columnName) }}</span>
                  <span style="color: #009b71">{{ labels[index] }}</span>
                  <span>{{ filter.filterValue }}</span>
                </div>
                <!-- Icon xóa một điều kiện lọc cụ thể -->
                <div
                  class="icon_close icon-16 cursor-pointer"
                  @click.stop="handleCancelFilter(filter.columnName)"
                ></div>
              </div>
            </div>
            <!-- Nút xóa tất cả bộ lọc -->
            <ms-button
              type="text"
              class="text-delete-all-filter"
              @click.stop="handleCancelAllFilter"
            >
              Bỏ lọc
            </ms-button>
          </div>
        </div>

        <!-- Nút Tải lại (Refresh): Lấy lại dữ liệu mới nhất từ Server -->
        <ms-button
          v-if="!isAnySelected"
          type="outlined"
          icon="icon_reload"
          noMargin="true"
          toolTip="Lấy lại dữ liệu"
          @click="handleReload"
        ></ms-button>
      </div>

      <!-- Grid Area: Khu vực hiển thị bảng dữ liệu và phân trang -->
      <div class="body-grid relative flex-1 min-h-0 flex flex-col">
        <!-- Component Bảng (MsTable) -->
        <ms-table
          ref="msTableRef"
          :columns="workShiftColumns"
          :data="workShiftStore.workShifts"
          :loading="isLoading"
          :sortColumn="workShiftFilterRequest.sortColumn"
          :filterColumn="workShiftFilterRequest.filterColumn"
          @openEdit="handleOpenEditPopup"
          @doubleClickRow="handleDoubleClickRow"
          @action="handleAction"
          @update:selectedRows="handleSelectedRows"
          @sort="handleSort"
          @filterColumn="handleFilterColumn"
          @cancelFilterColumn="handleCancelFilter"
        >
          <!-- Custom Slot: Hiển thị trạng thái có màu sắc (Badge) -->
          <template #workShiftStatus="{ rowData }">
            <div class="status-badge" :class="getStatusClass(rowData.workShiftStatus)">
              {{ rowData.workShiftStatus }}
            </div>
          </template>
          <!-- Custom Slot: Hiển thị giờ làm việc -->
          <template #workingHours="{ rowData }">
            {{ rowData.workingHours }}
          </template>
          <!-- Custom Slot: Hiển thị giờ nghỉ -->
          <template #breakHours="{ rowData }">
            {{ rowData.breakHours }}
          </template>
        </ms-table>

        <!-- Component Phân trang (Pagination) -->
        <ms-pagination
          :totalRecords="workShiftStore.totalRecords"
          :currentPage="workShiftFilterRequest.page"
          :pageSize="workShiftFilterRequest.pageSize"
          @update:currentPage="handlePageChange"
          @update:pageSize="handlePageSizeChange"
        ></ms-pagination>
      </div>
    </div>
  </div>

  <!-- Popup Form: Dùng chung cho Thêm mới, Sửa và Nhân bản -->
  <shift-popup
    :isVisible="isShowPopup"
    :initialData="initialData"
    @close="setPopupVisible(false)"
    @save="handleShiftSave"
  ></shift-popup>
</template>

<script setup>
/**
 * Script Setup: Logic nghiệp vụ cho trang Danh sách Ca làm việc.
 * Sử dụng Vue 3 Composition API.
 * 
 * Created by: HoanTD (04/12/2025)
 */

// --- Import Resources ---
import MsButton from "@/components/ms-button/MsButton.vue";
import MsPagination from "@/components/ms-pagination/MsPagination.vue";
import MsInput from "@/components/ms-input/MsInput.vue";
import MsTable from "@/components/ms-table/MsTable.vue";
import ShiftPopup from "./ShiftPopup.vue";
import { computed, nextTick, onMounted, ref, watch } from "vue";
import WorkShiftsAPI from "@/apis/components/WorkShiftAPI.js";
import { STATUS, STATUS_CODE } from "@/enums/Enum";
import { extractTime, formatDateDDMMYYYY } from "@/utils/DateTimeFns.js";
import { useWorkShiftStore } from "@/stores/workShiftStore";
import { Modal } from "ant-design-vue";

// --- State Management ---
// Khởi tạo Store (Pinia) để quản lý state của WorkShift
const workShiftStore = useWorkShiftStore();

// Reference đến component MsTable để gọi các hàm nội bộ của bảng
const msTableRef = ref(null);

// Cấu hình các cột hiển thị trên bảng (Grid)
const workShiftColumns = ref([
  {
    title: "Mã ca",
    field: "workShiftCode",
    minWidth: "120px",
    align: "left",
    filterable: true,
    sortable: true,
  },
  {
    title: "Tên ca",
    field: "workShiftName",
    minWidth: "250px",
    align: "left",
    filterable: true,
    sortable: true,
  },
  { title: "Giờ vào ca", field: "startTime", minWidth: "130px", align: "left", sortable: true },
  { title: "Giờ hết ca", field: "endTime", minWidth: "130px", align: "left", sortable: true },
  {
    title: "Bắt đầu nghỉ giữa ca",
    field: "breakStart",
    minWidth: "200px",
    align: "left",
    sortable: true,
  },
  {
    title: "Kết thúc nghỉ giữa ca",
    field: "breakEnd",
    minWidth: "200px",
    align: "left",
    sortable: true,
  },
  {
    title: "Thời gian làm việc (giờ)",
    field: "workingHours",
    minWidth: "210px",
    align: "right",
    filterable: true,
    filterType: "number",
    sortable: true,
  },
  {
    title: "Thời gian nghỉ giữa ca (giờ)",
    field: "breakHours",
    minWidth: "230px",
    align: "right",
    filterable: true,
    filterType: "number",
    sortable: true,
  },
  {
    title: "Trạng thái",
    field: "workShiftStatus",
    minWidth: "200px",
    align: "left",
    filterable: true,
    filterType: "enum",
    sortable: true,
  },
  {
    title: "Người tạo",
    field: "createdBy",
    minWidth: "160px",
    align: "left",
    filterable: true,
    sortable: true,
  },
  {
    title: "Ngày tạo",
    field: "createdDate",
    minWidth: "160px",
    align: "left",
    filterable: true,
    sortable: true,
  },
  {
    title: "Người sửa",
    field: "modifiedBy",
    minWidth: "160px",
    align: "left",
    filterable: true,
    sortable: true,
  },
  {
    title: "Ngày sửa",
    field: "modifiedDate",
    minWidth: "160px",
    align: "left",
    filterable: true,
    sortable: true,
  },
]);

// Trạng thái Loading của bảng dữ liệu
const isLoading = ref(true);

// Từ khóa tìm kiếm (binding 2 chiều với ô input)
const keyword = ref("");

// Biến lưu timeoutId cho kỹ thuật Debounce khi tìm kiếm
let timeoutId = null;

// Trạng thái hiển thị Popup (Thêm/Sửa)
const isShowPopup = ref(false);

// Dữ liệu khởi tạo truyền vào Popup (dùng cho Sửa/Nhân bản)
const initialData = ref(null);

// Danh sách các dòng đang được chọn (Checkbox)
const selectedWorkShifts = ref([]);

// Danh sách nhãn hiển thị cho các bộ lọc đang áp dụng
const labels = ref([]);

// Payload chuẩn gửi lên API để lấy danh sách phân trang
const workShiftFilterRequest = {
  page: 1, // Trang hiện tại
  pageSize: 20, // Số bản ghi trên trang
  keyword: "", // Từ khóa tìm kiếm
  sortColumn: [], // Danh sách cột cần sắp xếp
  filterColumn: [], // Danh sách cột cần lọc
};

// --- Computed Properties ---

/**
 * <summary>
 * Tính toán số lượng bản ghi đang được chọn.
 * </summary>
 * <returns>Số lượng (Number)</returns>
 * Created by: HoanTD (10/12/2025)
 */
const selectedCount = computed(() => selectedWorkShifts.value.length);

/**
 * <summary>
 * Kiểm tra xem có bản ghi nào được chọn hay không.
 * </summary>
 * <returns>True nếu có > 0 bản ghi được chọn</returns>
 * Created by: HoanTD (10/12/2025)
 */
const isAnySelected = computed(() => selectedCount.value > 0);

/**
 * <summary>
 * Kiểm tra xem có thể hiển thị nút "Sử dụng" hay không.
 * Logic: Hiển thị khi trong các dòng đã chọn có ít nhất 1 dòng đang "Ngừng sử dụng".
 * </summary>
 * <returns>True/False</returns>
 * Created by: HoanTD (10/12/2025)
 */
const canActivate = computed(() => {
  return selectedWorkShifts.value.some((row) => row.workShiftStatus === STATUS.INACTIVE);
});

/**
 * <summary>
 * Kiểm tra xem có thể hiển thị nút "Ngừng sử dụng" hay không.
 * Logic: Hiển thị khi trong các dòng đã chọn có ít nhất 1 dòng đang "Sử dụng".
 * </summary>
 * <returns>True/False</returns>
 * Created by: HoanTD (10/12/2025)
 */
const canInactivate = computed(() => {
  return selectedWorkShifts.value.some((row) => row.workShiftStatus === STATUS.ACTIVE);
});

// --- Methods / Functions ---

/**
 * <summary>
 * Thiết lập trạng thái hiển thị của Popup.
 * </summary>
 * <param name="value">True để mở, False để đóng</param>
 * Created by: HoanTD (09/12/2025)
 */
const setPopupVisible = (value) => {
  isShowPopup.value = value;
};

/**
 * <summary>
 * Gọi API lấy danh sách ca làm việc theo điều kiện lọc/phân trang.
 * Xử lý format dữ liệu (Ngày tháng, Giờ, Trạng thái) trước khi lưu vào Store.
 * </summary>
 * <param name="payload">Đối tượng chứa thông tin phân trang, lọc, tìm kiếm</param>
 * Created by: HoanTD (06/12/2025)
 */
const getPaging = async (payload) => {
  setLoading(true);
  await nextTick(); // Đảm bảo UI update trạng thái loading trước khi gọi API

  try {
    const response = await WorkShiftsAPI.paging(payload);
    if (response.data.code === STATUS_CODE.OK) {
      // Map lại dữ liệu trả về để phù hợp hiển thị trên Grid
      const formattedData = response.data.data.items.map((shift) => {
        return {
          ...shift,
          // Cắt chuỗi thời gian để hiển thị dạng HH:mm
          startTime: extractTime(shift.startTime),
          endTime: extractTime(shift.endTime),
          breakStart: extractTime(shift.breakStart),
          breakEnd: extractTime(shift.breakEnd),
          // Convert trạng thái boolean sang Enum text
          workShiftStatus: shift.workShiftStatus ? STATUS.ACTIVE : STATUS.INACTIVE,
          // Format ngày tháng
          createdDate: formatDateDDMMYYYY(shift.createdDate),
          modifiedDate: formatDateDDMMYYYY(shift.modifiedDate),
        };
      });

      // Lưu dữ liệu vào Pinia Store
      workShiftStore.setWorkShifts(formattedData, response.data.data.totalCount);
    }
  } catch (error) {
    console.error("Lỗi khi gọi API phân trang ca làm việc:", error);
  } finally {
    setLoading(false);
  }
};

/**
 * <summary>
 * Lấy chi tiết ca làm việc theo ID để hiển thị lên Form Sửa.
 * </summary>
 * <param name="id">Guid của ca làm việc</param>
 * Created by: HoanTD (09/12/2025)
 */
const getById = async (id) => {
  await nextTick();
  try {
    const response = await WorkShiftsAPI.get(id);
    if (response.data.code === STATUS_CODE.OK) {
      const responseData = response.data.data;
      // Format lại dữ liệu thời gian trước khi binding vào Form
      const formattedData = {
        ...responseData,
        startTime: extractTime(responseData.startTime),
        endTime: extractTime(responseData.endTime),
        breakStart: extractTime(responseData.breakStart),
        breakEnd: extractTime(responseData.breakEnd),
      };
      initialData.value = { ...formattedData };
    }
  } catch (error)
  {
    console.error("Lấy dữ liệu by Id lỗi: ", error);
  }
};

/**
 * <summary>
 * Cập nhật trạng thái (Active/Inactive) cho danh sách các ID.
 * </summary>
 * <param name="payload">Object chứa danh sách IDs và trạng thái mới</param>
 * Created by: HoanTD (10/12/2025)
 */
const updateStatus = async (payload) => {
  await nextTick();
  try {
    const response = await WorkShiftsAPI.updateMultipleStatus(payload);
    if (response.data.code === STATUS_CODE.OK) {
      // Nếu API thành công, cập nhật luôn trong Store để UI phản hồi ngay lập tức
      if (response.data.data === payload.ids?.length) {
        workShiftStore.updateStatusBatch(payload);
      }
    }
  } catch (error) {
    console.error("Cập nhật trạng thái lỗi: ", error);
  }
};

/**
 * <summary>
 * Xóa nhiều ca làm việc theo danh sách ID.
 * </summary>
 * <param name="payload">Mảng các ID cần xóa</param>
 * Created by: HoanTD (10/12/2025)
 */
const deleteShift = async (payload) => {
  await nextTick();
  try {
    const response = await WorkShiftsAPI.deleteMultiple(payload);
    if (response.data.code === STATUS_CODE.OK) {
      // Nếu xóa thành công, loại bỏ bản ghi khỏi Store
      if (response.data.data === payload.length) {
        workShiftStore.deleteBatch(payload);
      }
    }
  } catch (error) {
    console.error("Xóa ca làm việc lỗi: ", error);
  }
};

/**
 * <summary>
 * Xử lý sự kiện sau khi Lưu thành công từ Popup (Thêm mới hoặc Sửa).
 * </summary>
 * <param name="newShiftData">Dữ liệu object trả về từ Popup</param>
 * Created by: HoanTD (09/12/2025)
 */
const handleShiftSave = (newShiftData) => {
  console.log(newShiftData);

  // Format lại dữ liệu để khớp với cấu trúc hiển thị trên bảng
  const formattedShift = {
    ...newShiftData,
    startTime: extractTime(newShiftData.startTime),
    endTime: extractTime(newShiftData.endTime),
    breakStart: extractTime(newShiftData.breakStart),
    breakEnd: extractTime(newShiftData.breakEnd),
    workShiftStatus: newShiftData.workShiftStatus ? STATUS.ACTIVE : STATUS.INACTIVE,
    createdDate: formatDateDDMMYYYY(newShiftData.createdDate),
    modifiedDate: formatDateDDMMYYYY(newShiftData.modifiedDate),
  };

  // Kiểm tra xem là update hay insert dựa vào việc ID đã tồn tại trong store chưa
  const isUpdating = workShiftStore.workShifts.some(
    (shift) => shift.workShiftId === formattedShift.workShiftId
  );

  if (isUpdating) {
    // Trường hợp Sửa: Update dữ liệu trong Store
    workShiftStore.updateWorkShift(formattedShift);
  } else {
    // Trường hợp Thêm mới: Đẩy vào đầu danh sách trong Store
    workShiftStore.addWorkShift(formattedShift);

    // Xử lý UI: Nếu danh sách hiện tại vượt quá PageSize, xóa phần tử cuối cùng
    if (workShiftStore.workShifts.length > workShiftFilterRequest.pageSize) {
      workShiftStore.workShifts.pop();
    }
  }
};

/**
 * <summary>
 * Set trạng thái Loading cho Grid.
 * </summary>
 * <param name="value">True/False</param>
 * Created by: HoanTD (06/12/2025)
 */
const setLoading = (value) => {
  isLoading.value = value;
};

/**
 * <summary>
 * Lấy class CSS tương ứng cho trạng thái (Active/Inactive).
 * </summary>
 * <param name="status">Giá trị trạng thái (text)</param>
 * <returns>Tên class CSS</returns>
 * Created by: HoanTD (06/12/2025)
 */
const getStatusClass = (status) => {
  if (status === STATUS.ACTIVE) {
    return "active";
  } else if (status === STATUS.INACTIVE) {
    return "inactive";
  }
  return "";
};

/**
 * <summary>
 * Xử lý sự kiện chuyển trang.
 * </summary>
 * <param name="newPage">Số trang đích</param>
 * Created by: HoanTD (06/12/2025)
 */
const handlePageChange = (newPage) => {
  workShiftFilterRequest.page = newPage;
  getPaging(workShiftFilterRequest);
};

/**
 * <summary>
 * Xử lý sự kiện thay đổi số bản ghi trên trang (PageSize).
 * </summary>
 * <param name="pageSize">Kích thước trang mới</param>
 * Created by: HoanTD (06/12/2025)
 */
const handlePageSizeChange = (pageSize) => {
  workShiftFilterRequest.pageSize = pageSize;
  workShiftFilterRequest.page = 1; // Reset về trang 1
  getPaging(workShiftFilterRequest);
};

/**
 * <summary>
 * Watcher theo dõi biến keyword để tìm kiếm.
 * Áp dụng kỹ thuật Debounce: Chỉ tìm kiếm sau khi người dùng ngừng gõ 500ms.
 * </summary>
 * Created by: HoanTD (06/12/2025)
 */
watch(keyword, (newKeyword) => {
  // Clear timeout cũ nếu người dùng gõ tiếp
  if (timeoutId) {
    clearTimeout(timeoutId);
  }

  setLoading(true);
  // Set timeout mới
  timeoutId = setTimeout(() => {
    workShiftFilterRequest.keyword = newKeyword;
    workShiftFilterRequest.page = 1; // Reset về trang 1 khi tìm kiếm
    getPaging(workShiftFilterRequest);
  }, 500); 
});

/**
 * <summary>
 * Xử lý sự kiện click nút Tải lại (Refresh).
 * </summary>
 * Created by: HoanTD (06/12/2025)
 */
const handleReload = () => {
  getPaging(workShiftFilterRequest);
};

/**
 * <summary>
 * Xử lý mở Popup Thêm mới.
 * Reset dữ liệu initialData về null.
 * </summary>
 * Created by: HoanTD (09/12/2025)
 */
const handleOpenAddPopup = () => {
  initialData.value = null;
  setPopupVisible(true);
};

/**
 * <summary>
 * Xử lý mở Popup Sửa khi click icon Sửa trên dòng.
 * </summary>
 * <param name="row">Dữ liệu dòng được chọn</param>
 * Created by: HoanTD (09/12/2025)
 */
const handleOpenEditPopup = async (row) => {
  await getById(row.workShiftId); // Gọi API lấy chi tiết mới nhất
  setPopupVisible(true);
};

/**
 * <summary>
 * Xử lý sự kiện Double Click vào dòng (tương đương với chức năng Sửa).
 * </summary>
 * <param name="row">Dữ liệu dòng</param>
 * Created by: HoanTD (09/12/2025)
 */
const handleDoubleClickRow = async (row) => {
  await getById(row.workShiftId);
  setPopupVisible(true);
};

/**
 * <summary>
 * Hàm điều phối các Action (Nhân bản, Đổi trạng thái, Xóa) từ Context Menu của bảng.
 * </summary>
 * <param name="action">Tên hành động</param>
 * <param name="row">Dữ liệu dòng</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleAction = (action, row) => {
  switch (action) {
    case "duplicate":
      handleDuplicate(row);
      break;
    case "updateStatus":
      handleUpdateStatus(row);
      break;
    case "delete":
      handleDelete(row);
      break;
    default:
      break;
  }
};

/**
 * <summary>
 * Xử lý chức năng Nhân bản.
 * Lấy dữ liệu dòng hiện tại, gán vào initialData nhưng loại bỏ ID để form hiểu là thêm mới.
 * </summary>
 * <param name="row">Dữ liệu gốc</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleDuplicate = async (row) => {
  initialData.value = null;
  initialData.value = {
    workShiftName: row.workShiftName,
    startTime: row.startTime,
    endTime: row.endTime,
    breakStart: row.breakStart,
    breakEnd: row.breakEnd,
    workShiftStatus: true, // Mặc định Active cho bản ghi nhân bản
    description: row.description,
  };
  setPopupVisible(true);
};

/**
 * <summary>
 * Xử lý đổi trạng thái nhanh 1 dòng.
 * </summary>
 * <param name="row">Dữ liệu dòng</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleUpdateStatus = async (row) => {
  const payload = {
    ids: [row.workShiftId],
    // Đảo ngược trạng thái hiện tại
    newStatus: row.workShiftStatus === STATUS.ACTIVE ? false : true,
  };
  await updateStatus(payload);
};

/**
 * <summary>
 * Xử lý xóa 1 dòng. Hiển thị Confirm Dialog trước khi xóa.
 * </summary>
 * <param name="row">Dữ liệu dòng</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleDelete = async (row) => {
  const payload = [row.workShiftId];

  Modal.confirm({
    title: "Xóa Ca làm việc",
    content: `Ca làm việc ${row.workShiftCode} sau khi bị xóa sẽ không thể khôi phục. Bạn có muốn tiếp tục xóa không?`,
    async onOk() {
      await deleteShift(payload);
    },
    onCancel() {},
    okText: "Xóa",
    okType: "danger",
    cancelText: "Hủy",
    closable: true,
    centered: true,
  });
};

/**
 * <summary>
 * Callback cập nhật danh sách các dòng được chọn từ MsTable.
 * </summary>
 * <param name="rows">Mảng các dòng được chọn</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleSelectedRows = (rows) => {
  selectedWorkShifts.value = rows;
};

/**
 * <summary>
 * Xử lý bỏ chọn tất cả các dòng. Gọi vào method của component con.
 * </summary>
 * Created by: HoanTD (10/12/2025)
 */
const handleUnselectAll = () => {
  if (msTableRef.value && msTableRef.value.unselectAll) {
    msTableRef.value.unselectAll();
  }
};

/**
 * <summary>
 * Xử lý cập nhật trạng thái hàng loạt cho các dòng được chọn.
 * </summary>
 * <param name="rows">Danh sách dòng chọn</param>
 * <param name="status">Trạng thái đích (true/false)</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleUpdateStatusBatch = async (rows, status) => {
  const payload = {
    ids: rows.map((r) => r.workShiftId),
    newStatus: status,
  };
  await updateStatus(payload);
};

/**
 * <summary>
 * Xử lý xóa hàng loạt các dòng được chọn. Có Confirm Dialog.
 * </summary>
 * <param name="rows">Danh sách dòng chọn</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleDeleteBatch = async (rows) => {
  const payload = rows.map((r) => r.workShiftId);

  Modal.confirm({
    title: "Xóa Ca làm việc",
    content: `Các ca làm việc sau khi bị xóa sẽ không thể khôi phục. Bạn có muốn tiếp tục xóa không?`,
    async onOk() {
      await deleteShift(payload);
    },
    onCancel() {},
    okText: "Xóa",
    okType: "danger",
    cancelText: "Hủy",
    closable: true,
    centered: true,
  });
};

/**
 * <summary>
 * Xử lý sự kiện Sắp xếp cột (Server-side sort).
 * Hỗ trợ sắp xếp Toggle: ASC -> DESC -> NONE.
 * </summary>
 * <param name="field">Tên cột (db field)</param>
 * <param name="direction">Hướng sắp xếp (ASC/DESC/NONE)</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleSort = async (field, direction) => {
  const index = workShiftFilterRequest.sortColumn.findIndex((item) => item.selector === field);

  if (index > -1) {
    if (direction === "NONE") {
      // Nếu hướng là NONE thì xóa khỏi danh sách sort
      workShiftFilterRequest.sortColumn.splice(index, 1);
    } else {
      // Cập nhật hướng sort
      workShiftFilterRequest.sortColumn[index].desc = direction === "DESC";
    }
  } else {
    // Thêm cột mới vào danh sách sort
    workShiftFilterRequest.sortColumn.push({
      selector: field,
      desc: direction === "DESC",
    });
  }

  await getPaging(workShiftFilterRequest);
};

/**
 * <summary>
 * Xử lý sự kiện Lọc cột (Server-side filter).
 * </summary>
 * <param name="col">Cấu hình cột</param>
 * <param name="filterOperator">Toán tử lọc (=, contains, ...)</param>
 * <param name="filterValue">Giá trị lọc</param>
 * <param name="label">Nhãn hiển thị UI (VD: Chứa 'ABC')</param>
 * Created by: HoanTD (10/12/2025)
 */
const handleFilterColumn = async (col, filterOperator, filterValue, label) => {
  // Validate đầu vào
  if (col.filterType === "enum") {
    if (!filterOperator) return;
  } else {
    if (!filterValue) return;
  }

  // Parse số nếu cột là kiểu số
  if (col.filterType === "number" && filterValue !== null) {
    filterValue = parseFloat(filterValue);
  }

  const index = workShiftFilterRequest.filterColumn.findIndex(
    (item) => item.columnName === col.field
  );
  
  // Cập nhật hoặc thêm mới điều kiện lọc
  if (index > -1) {
    workShiftFilterRequest.filterColumn[index].filterOperator = filterOperator;
    workShiftFilterRequest.filterColumn[index].filterValue = filterValue;
    labels.value[index] = label;
  } else {
    workShiftFilterRequest.filterColumn.push({
      columnName: col.field,
      filterOperator: filterOperator,
      filterValue: filterValue,
    });
    labels.value.push(label);
  }

  workShiftFilterRequest.page = 1; // Reset về trang 1 khi lọc
  await getPaging(workShiftFilterRequest);
};

/**
 * <summary>
 * Helper: Lấy tiêu đề cột dựa vào tên field.
 * Dùng để hiển thị trên Tag lọc.
 * </summary>
 * <param name="field">Tên field</param>
 * <returns>Title của cột</returns>
 * Created by: HoanTD (11/12/2025)
 */
const getColumTitleByField = (field) => {
  const col = workShiftColumns.value.find((col) => col.field === field);
  return col ? col.title : "";
};

/**
 * <summary>
 * Hủy bỏ 1 điều kiện lọc cụ thể khi click icon X trên Tag lọc.
 * </summary>
 * <param name="columnName">Tên cột cần hủy lọc</param>
 * Created by: HoanTD (11/12/2025)
 */
const handleCancelFilter = async (columnName) => {
  const index = workShiftFilterRequest.filterColumn.findIndex(
    (item) => item.columnName === columnName
  );
  if (index > -1) {
    workShiftFilterRequest.filterColumn.splice(index, 1);
    labels.value.splice(index, 1);
    await getPaging(workShiftFilterRequest);
    
    // Reset giá trị trên UI của component con
    if (msTableRef.value) {
      msTableRef.value.filterValue = null;
    }
  }
};

/**
 * <summary>
 * Hủy bỏ tất cả điều kiện lọc.
 * </summary>
 * Created by: HoanTD (11/12/2025)
 */
const handleCancelAllFilter = async () => {
  workShiftFilterRequest.filterColumn = [];
  labels.value = [];
  await getPaging(workShiftFilterRequest);
};

/**
 * Lifecycle Hook: Gọi khi component được mount vào DOM.
 * Tải dữ liệu trang đầu tiên.
 * Created by: HoanTD (09/12/2025)
 */
onMounted(() => {
  getPaging(workShiftFilterRequest);
});
</script>

<style scoped>
.title-box {
  height: 30px;
}
.tool-box {
  /* height: 44px; */
  padding: 8px 16px;
  background: #fff;
  border-top-left-radius: 4px;
  border-top-right-radius: 4px;
}
.status-badge {
  height: 26px;
  display: flex;
  align-items: center;
  width: fit-content;
}
.status-badge.active {
  background-color: #ebfef6;
  color: #009b71;
  padding: 5px 8px;
  border-radius: 999px;
}
.status-badge.inactive {
  background-color: #fee2e2;
  color: #dc2626;
  padding: 5px 8px;
  border-radius: 999px;
}
.filter-conditions {
  display: flex;
  align-items: center;
  gap: 4px;
  flex-wrap: wrap;
  max-height: 56px;
  overflow-y: auto;
  margin-right: 8px;
}
.filter-item {
  display: flex;
  gap: 8px;
  background-color: #f3f4f6;
  padding: 2px 8px;
  border-radius: 4px;
}
</style>