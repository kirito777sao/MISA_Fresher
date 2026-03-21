<template>
  <!--
    Component: ShiftPopup
    Description: Popup dùng chung cho Thêm mới, Sửa, Nhân bản Ca làm việc.
    Created by: HoanTD (06/12/2025)
  -->
  <div v-if="isVisible" class="bg-overlay fixed inset-0 flex justify-center items-center z-1000">
    <!-- Loading Indicator: Hiển thị khi đang gọi API -->
    <div v-if="isLoading" class="bg-overlay absolute inset-0 flex justify-center items-center z-20">
      <svg
        class="animate-spin h-8 w-8 text-teal-600"
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
      >
        <circle
          class="opacity-25"
          cx="12"
          cy="12"
          r="10"
          stroke="currentColor"
          stroke-width="4"
        ></circle>
        <path
          class="opacity-75"
          fill="currentColor"
          d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
        ></path>
      </svg>
    </div>

    <!-- Main Popup Container -->
    <div
      ref="popup"
      class="w-[680px] bg-white rounded-lg shadow-xl min-w-2xl max-w-3xl transform transition-all overflow-hidden"
      @keydown.esc="handleClose"
      role="dialog"
      aria-modal="true"
      tabindex="0"
    >
      <!-- Popup Header -->
      <div class="h-[68px] flex justify-between items-center px-5 py-4">
        <h2 class="text-2xl font-bold">{{ isEdit ? "Sửa" : "Thêm" }} Ca làm việc</h2>
        <div class="flex items-center gap-2">
          <!-- Nút Help: Mở hướng dẫn sử dụng -->
          <ms-button
            type="text"
            icon="icon_help"
            iconSize="20"
            no-margin
            no-padding
            toolTip="Trợ giúp"
            @click="handleHelp"
          ></ms-button>
          <!-- Nút Close: Đóng popup -->
          <ms-button
            type="text"
            icon="icon_close"
            iconSize="20"
            no-margin
            no-padding          
            toolTip="(Đóng) ESC"
            @click="handleClose"
          ></ms-button>
        </div>
      </div>
      <!-- Popup Body: Form nhập liệu -->
      <div class="p-5">
        <!-- Mã ca -->
        <div class="form-group">
          <ms-input
            ref="firstInput"
            v-model="formData.workShiftCode"
            label="Mã ca"
            tabIndex="1"
            required
            :isError="!formData.workShiftCode"
            maxLength="20"
          ></ms-input>
        </div>

        <!-- Tên ca -->
        <div class="form-group">
          <ms-input
            v-model="formData.workShiftName"
            label="Tên ca"
            tabIndex="2"
            required
            :isError="!formData.workShiftName"
            maxLength="50"
          ></ms-input>
        </div>

        <!-- Giờ vào ca, Giờ hết ca -->
        <div class="flex gap-9">
          <div class="form-group w-1/2">
            <ms-time-picker
              v-model="formData.startTime"
              label="Giờ vào ca"
              required
              tabIndex="3"
              width="w-[122px]"
              :isError="!formData.startTime"
            ></ms-time-picker>
          </div>
          <div class="form-group w-1/2">
            <ms-time-picker
              v-model="formData.endTime"
              label="Giờ hết ca"
              required
              tabIndex="4"
              width="w-[122px]"
              :isError="!formData.endTime"
            ></ms-time-picker>
          </div>
        </div>

        <!-- Bắt đầu nghỉ giữa ca, Kết thúc nghỉ giữa ca -->
        <div class="flex gap-9">
          <div class="form-group w-1/2">
            <ms-time-picker
              v-model="formData.breakStart"
              label="Bắt đầu nghỉ giữa ca"
              tabIndex="5"
              width="w-[122px]"
            ></ms-time-picker>
          </div>
          <div class="form-group w-1/2">
            <ms-time-picker
              v-model="formData.breakEnd"
              label="Kết thúc nghỉ giữa ca"
              tabIndex="6"
              width="w-[122px]"
            ></ms-time-picker>
          </div>
        </div>

        <!-- Thời gian làm việc, Thời gian nghỉ giữa ca -->
        <div class="flex gap-9">
          <div class="form-group w-1/2">
            <ms-input
              v-model="formData.workingHours"
              label="Thời gian làm việc (giờ)"
              width="w-[122px]"
              justify="between"
              tabIndex="-1"
              isDisabled
              placeholderAlign="right"
              :isWarning="isWarning"
            ></ms-input>
          </div>
          <div class="form-group w-1/2">
            <ms-input
              v-model="formData.breakHours"
              label="Thời gian nghỉ giữa ca (giờ)"
              width="w-[122px]"
              justify="between"
              tabIndex="-1"
              isDisabled
              placeholderAlign="right"
            ></ms-input>
          </div>
        </div>

        <!-- Mô tả -->
        <div class="form-group">
          <ms-textarea v-model="formData.description" label="Mô tả" tabIndex="7"></ms-textarea>
        </div>

        <!-- Trạng thái (chỉ hiển thị ở chế độ sửa) -->
        <div v-if="isEdit" class="flex">
          <label class="ms-input-label">Trạng thái</label>
          <div class="flex items-center space-x-4">
            <label class="inline-flex items-center">
              <input
                type="radio"
                class="form-radio h-4 w-4 text-teal-600 border-gray-300"
                name="status"
                value="true"
                v-model="formData.workShiftStatus"
                tabindex="8"
              />
              <span class="ml-2 text-sm text-gray-700">Đang sử dụng</span>
            </label>
            <label class="inline-flex items-center">
              <input
                type="radio"
                class="form-radio h-4 w-4 text-teal-600 border-gray-300"
                name="status"
                value="false"
                v-model="formData.workShiftStatus"
                tabindex="9"
              />
              <span class="ml-2 text-sm text-gray-700">Ngừng sử dụng</span>
            </label>
          </div>
        </div>
      </div>

      <!-- Popup Footer: Các nút thao tác -->
      <div class="action-container flex justify-end gap-2 px-5 py-3">
        <!-- Nút Hủy: Đóng popup, không lưu -->
        <ms-button 
          type="outlined" 
          @click="handleCancel"           
          tabIndex="12"
          >Hủy
        </ms-button>
        <!-- Nút Lưu và Thêm: Lưu và mở lại form trắng -->
        <ms-button
          type="outlined"
          toolTip="Ctrl + Shift + S"
          @click="handleSaveAndAdd"
          tabIndex="11"
        >
          Lưu và Thêm
        </ms-button>
        <!-- Nút Lưu: Lưu và đóng popup -->
        <ms-button toolTip="Ctrl + S" @click="handleSave" tabIndex="10"> Lưu </ms-button>
      </div>
    </div>
  </div>
</template>

<script setup>
/**
 * Script Setup: Logic nghiệp vụ cho Popup Thêm/Sửa Ca làm việc.
 * Sử dụng Vue 3 Composition API.
 *
 * Created by: HoanTD (06/12/2025)
 */
import {
  ref,
  onMounted,
  defineProps,
  defineEmits,
  watch,
  computed,
  nextTick,
  onUnmounted,
} from "vue";
import MsButton from "@/components/ms-button/MsButton.vue";
import MsInput from "@/components/ms-input/MsInput.vue";
import MsTimePicker from "@/components/ms-timepicker/MsTimePicker.vue";
import MsTextarea from "@/components/ms-textarea/MsTextarea.vue";
import { convertTimeToISO } from "@/utils/DateTimeFns.js";
import WorkShiftsAPI from "@/apis/components/WorkShiftAPI.js";
import { STATUS_CODE } from "@/enums/Enum";
import { message, Modal } from "ant-design-vue";

// --- Props & Emits ---
// Khai báo các Props mà component này nhận từ bên ngoài
const props = defineProps({
  isVisible: {
    type: Boolean,
    default: false,
  },
  initialData: {
    type: Object,
    default: () => null,
  },
});

// Khai báo các Events mà component này emit ra bên ngoài
const emit = defineEmits(["close", "save", "saveAndAdd"]);

// --- State ---
// Form Data: Dữ liệu binding với các input trên form
const formData = ref({
  workShiftCode: null,
  workShiftName: null,
  startTime: null,
  endTime: null,
  breakStart: null,
  breakEnd: null,
  workingHours: null,
  breakHours: null,
  workShiftStatus: true,
  description: null,
});

// Các Refs để thao tác với các phần tử DOM
const firstInput = ref(null); // Input đầu tiên để focus vào khi mở popup
const popup = ref(null); // Toàn bộ popup để focus vào khi mở popup

// Các trạng thái UI
const isLoading = ref(false); // Trạng thái loading khi gọi API
const isClose = ref(false); // Cờ để biết có đóng popup sau khi save hay không
const isWarning = ref(false); // Cờ để hiển thị cảnh báo nếu giờ làm việc < 0

// --- Computed Properties ---

/**
 * <summary>
 * Xác định xem popup đang ở chế độ Thêm mới hay Sửa.
 * </summary>
 * <returns>True nếu là Sửa, False nếu là Thêm mới.</returns>
 * Created by: HoanTD (08/12/2025)
 */
const isEdit = computed(() => !!props.initialData?.workShiftId);

// --- Methods ---

/**
 * <summary>
 * Tính toán khoảng thời gian giữa 2 mốc thời gian (HH:mm).
 * </summary>
 * <param name="start">Thời gian bắt đầu (HH:mm)</param>
 * <param name="end">Thời gian kết thúc (HH:mm)</param>
 * <returns>Số giờ (định dạng string), hoặc chuỗi rỗng nếu tính toán thất bại.</returns>
 */
const calculateDuration = (start, end) => {
  if (!start || !end) return ""; // Không tính nếu thiếu start hoặc end
  try {
    // Tách giờ và phút từ chuỗi
    const [startH, startM] = start.split(":").map(Number);
    const [endH, endM] = end.split(":").map(Number);

    // Tạo đối tượng Date để tính toán
    const startDate = new Date(2000, 0, 1, startH, startM);
    let endDate = new Date(2000, 0, 1, endH, endM);

    // Nếu giờ kết thúc < giờ bắt đầu, cộng thêm 1 ngày (ví dụ ca đêm)
    if (endDate < startDate) {
      endDate = new Date(endDate.setDate(endDate.getDate() + 1));
    }

    // Tính số mili giây giữa 2 thời điểm
    const diffMs = endDate.getTime() - startDate.getTime();
    // Đổi sang giờ
    const diffHours = diffMs / (1000 * 60 * 60);
    // Trả về string đã format
    return diffHours.toFixed(2).replace(/\.?0+$/, ""); // Loại bỏ các số 0 vô nghĩa
  } catch {
    return ""; // Lỗi: trả về chuỗi rỗng
  }
};

/**
 * <summary>
 * Thiết lập trạng thái Loading cho popup.
 * </summary>
 * <param name="value">True để hiển thị loading, False để ẩn.</param>
 * Created by: HoanTD (06/12/2025)
 */
const setLoading = (value) => {
  isLoading.value = value;
};

/**
 * <summary>
 * Gọi API để thêm mới ca làm việc.
 * </summary>
 * <param name="formData">Dữ liệu form đã được binding.</param>
 * Created by: HoanTD (08/12/2025)
 */
const addNewWorkShift = async (formData) => {
  setLoading(true);
  try {
    // Chuẩn bị payload để gọi API
    const payload = {
      workShiftCode: formData.value.workShiftCode,
      workShiftName: formData.value.workShiftName,
      description: formData.value.description,
      startTime: convertTimeToISO(formData.value.startTime) || null,
      endTime: convertTimeToISO(formData.value.endTime) || null,
      breakStart: convertTimeToISO(formData.value.breakStart) || null,
      breakEnd: convertTimeToISO(formData.value.breakEnd) || null,
      workingHours: parseFloat(formData.value.workingHours) || 0,
      breakHours: parseFloat(formData.value.breakHours) || 0,
      workShiftStatus: formData.value.workShiftStatus,
    };

    const response = await WorkShiftsAPI.add(payload);
    if (response.data.code === STATUS_CODE.OK) {
      // API trả về thành công
      message.success(response.data.userMessage || ""); // Hiển thị message
      emit("save", response.data.data); // Emit event 'save' để component cha xử lý
      if (isClose.value) {
        handleClose(); // Nếu nhấn nút 'Lưu', đóng popup
      } else {
        resetForm(); // Nếu nhấn nút 'Lưu và thêm', reset form
      }
    }
  } catch (error) {
    // API trả về lỗi
    console.error("Lỗi khi gọi API thêm ca làm việc:", error);
    const errorData = error.response.data;
    if (errorData.Code === STATUS_CODE.VALIDATION || errorData.Code === STATUS_CODE.CONFLICT) {
      // Lỗi validation hoặc trùng lặp
      const errorMess = Object.values(errorData.ValidateInfo);
      Modal.warning({
        title: "Cảnh báo",
        content: errorMess[0],
        okText: "Đóng",
        centered: true,
      });
    }
  } finally {
    setLoading(false); // Luôn ẩn loading dù thành công hay thất bại
  }
};

/**
 * <summary>
 * Gọi API để cập nhật ca làm việc.
 * </summary>
 * <param name="id">ID của ca làm việc cần cập nhật.</param>
 * <param name="formData">Dữ liệu form đã binding.</param>
 * Created by: HoanTD (08/12/2025)
 */
const updateWorkShift = async (id, formData) => {
  setLoading(true);
  try {
    // Chuẩn bị payload
    const payload = {
      workShiftCode: formData.value.workShiftCode,
      workShiftName: formData.value.workShiftName,
      description: formData.value.description,
      startTime: convertTimeToISO(formData.value.startTime),
      endTime: convertTimeToISO(formData.value.endTime),
      breakStart: convertTimeToISO(formData.value.breakStart),
      breakEnd: convertTimeToISO(formData.value.breakEnd),
      workingHours: parseFloat(formData.value.workingHours) || 0,
      breakHours: parseFloat(formData.value.breakHours) || 0,
      workShiftStatus: JSON.parse(formData.value.workShiftStatus), // Convert string to boolean
    };

    const response = await WorkShiftsAPI.update(id, payload);
    if (response.data.code === STATUS_CODE.OK) {
      // API thành công
      message.success(response.data.userMessage || "");
      emit("save", response.data.data);
      if (isClose.value) {
        handleClose(); // Đóng popup
      } else {
        resetForm(); // Reset form
      }
    }
  } catch (error) {
    // API lỗi
    console.error("Lỗi khi gọi API sửa ca làm việc:", error);
    const errorData = error.response.data;
    if (errorData.Code === STATUS_CODE.VALIDATION || errorData.Code === STATUS_CODE.CONFLICT) {
      // Lỗi validation
      const errorMess = Object.values(errorData.ValidateInfo);
      Modal.warning({
        title: "Cảnh báo",
        content: errorMess[0],
        okText: "Đóng",
        centered: true,
      });
    }
  } finally {
    setLoading(false);
  }
};

/**
 * <summary>
 * Reset form về trạng thái ban đầu (trống).
 * </summary>
 * Created by: HoanTD (09/12/2025)
 */
const resetForm = () => {
  formData.value = {
    workShiftCode: null,
    workShiftName: null,
    startTime: null,
    endTime: null,
    breakStart: null,
    breakEnd: null,
    workingHours: null,
    breakHours: null,
    workShiftStatus: true,
    description: null,
  };
  // Focus vào input đầu tiên
  nextTick(() => {
    const inputElement = firstInput.value?.$el?.querySelector("input");
    if (inputElement) {
      inputElement.focus();
    }
  });
};

/**
 * <summary>
 * Kiểm tra xem form đã điền đầy đủ các trường bắt buộc chưa.
 * </summary>
 * <returns>True nếu hợp lệ, False nếu không.</returns>
 * Created by: HoanTD (08/12/2025)
 */
const validateForm = () => {
  return (
    formData.value.workShiftCode &&
    formData.value.workShiftName &&
    formData.value.startTime &&
    formData.value.endTime
  );
};

/**
 * <summary>
 * Xử lý sự kiện click nút 'Lưu'.
 * </summary>
 * Created by: HoanTD (08/12/2025)
 */
const handleSave = () => {
  isClose.value = true; // Đánh dấu là cần đóng popup sau khi save
  if (isEdit.value) {
    // Chế độ Sửa
    const id = props.initialData?.workShiftId || "";
    updateWorkShift(id, formData);
  } else {
    // Chế độ Thêm mới
    addNewWorkShift(formData);
  }
};

/**
 * <summary>
 * Hàm xử lý phím tắt Ctrl + S (Lưu) và Ctrl + Shift + S (Lưu và thêm).
 * </summary>
 * <param name="event">Sự kiện keydown</param>
 * Created by: HoanTD (09/12/2025)
 */
const handleSaveShortcut = (event) => {
  const isSaveKey = event.key === "s" || event.key === "S"; // Kiểm tra phím 's'
  const isModifierPressed = event.ctrlKey || event.metaKey; // Kiểm tra Ctrl hoặc Cmd

  if (isSaveKey && isModifierPressed) {
    // Nếu là phím tắt 'Lưu'
    event.preventDefault(); // Ngăn trình duyệt thực hiện hành động mặc định

    if (event.shiftKey) {
      // Nếu có Shift, gọi 'Lưu và thêm'
      handleSaveAndAdd();
    } else {
      // Nếu không có Shift, gọi 'Lưu'
      handleSave();
    }
  }
};

/**
 * <summary>
 * Xử lý sự kiện click nút 'Lưu và thêm'.
 * </summary>
 * Created by: HoanTD (08/12/2025)
 */
const handleSaveAndAdd = () => {
  isClose.value = false; // Đánh dấu là KHÔNG đóng popup
  if (isEdit.value) {
    // Chế độ Sửa
    const id = props.initialData?.workShiftId || "";
    updateWorkShift(id, formData);
  } else {
    // Chế độ Thêm mới
    addNewWorkShift(formData);
  }
};

/**
 * <summary>
 * Xử lý sự kiện click nút 'Hủy'.
 * </summary>
 * Created by: HoanTD (08/12/2025)
 */
const handleCancel = () => {
  emit("close"); // Emit event 'close' để component cha đóng popup
};

/**
 * <summary>
 * Xử lý sự kiện click icon 'Đóng' (góc trên bên phải).
 * </summary>
 * Created by: HoanTD (08/12/2025)
 */
const handleClose = () => {
  emit("close");
};

/**
 * <summary>
 * Xử lý sự kiện click icon 'Trợ giúp'.
 * </summary>
 */
const handleHelp = () => {
  alert("Chức năng trợ giúp đang trong quá trình phát triển");
};

// --- Lifecycle Hooks ---

/**
 * <summary>
 * onMounted: Gọi khi component được mount vào DOM.
 * Đăng ký event listener cho phím tắt Ctrl + S.
 * </summary>
 */
onMounted(() => {
  window.addEventListener("keydown", handleSaveShortcut);
});

/**
 * <summary>
 * onUnmounted: Gọi khi component bị unmount khỏi DOM.
 * Gỡ bỏ event listener để tránh memory leak.
 * </summary>
 */
onUnmounted(() => {
  window.removeEventListener("keydown", handleSaveShortcut);
});

// --- Watchers ---

/**
 * <summary>
 * Watcher: Theo dõi sự thay đổi của các trường thời gian
 * (startTime, endTime, breakStart, breakEnd) để tính toán lại giờ làm việc/nghỉ.
 * </summary>
 */
watch(
  () => [
    formData.value.startTime,
    formData.value.endTime,
    formData.value.breakStart,
    formData.value.breakEnd,
  ],
  () => {
    // Tính tổng thời gian ca làm việc
    const totalShiftDuration = calculateDuration(formData.value.startTime, formData.value.endTime);

    // Tính tổng thời gian nghỉ giữa ca
    const totalBreakDuration = calculateDuration(
      formData.value.breakStart,
      formData.value.breakEnd
    );
    formData.value.breakHours = totalBreakDuration || "0";

    // Tính thời gian làm việc thực tế: Tổng ca - Tổng nghỉ
    if (totalShiftDuration && totalBreakDuration) {
      const workHours = parseFloat(totalShiftDuration) - parseFloat(totalBreakDuration);
      if (workHours < 0) {
        // Nếu giờ làm việc âm, hiển thị cảnh báo
        formData.value.workingHours =
          "(" +
          Math.abs(workHours)
            .toFixed(2)
            .replace(/\.?0+$/, "") +
          ")";
        isWarning.value = true;
      } else {
        // Nếu giờ làm việc dương, hiển thị bình thường
        formData.value.workingHours = workHours.toFixed(2).replace(/\.?0+$/, "");
        isWarning.value = false;
      }
    } else {
      formData.value.workingHours = totalShiftDuration;
    }
  },
  { immediate: true } // Chạy ngay khi component mount
);

/**
 * <summary>
 * Watcher: Theo dõi sự thay đổi của prop 'isVisible' để thực hiện các hành động
 * khi popup được mở (focus vào input đầu tiên, reset form...).
 * </summary>
 */
watch(
  () => props.isVisible,
  (newVal) => {
    if (newVal) {
      // Khi popup được mở
      if (props.initialData) {
        // Nếu là chế độ Sửa, clone dữ liệu từ prop vào form
        formData.value = { ...props.initialData };
      } else {
        // Nếu là chế độ Thêm mới, reset form
        formData.value = {
          workShiftCode: null,
          workShiftName: null,
          startTime: null,
          endTime: null,
          breakStart: null,
          breakEnd: null,
          workingHours: null,
          breakHours: null,
          workShiftStatus: true,
          description: null,
        };
      }

      // Focus vào popup
      nextTick(() => {
        if (popup.value) {
          popup.value.focus();
        }
        // Focus vào input đầu tiên
        nextTick(() => {
          const inputElement = firstInput.value?.$el?.querySelector("input");
          if (inputElement) {
            inputElement.focus();
          }
        });
      });
    }
  },
  { immediate: true } // Chạy ngay khi component mount
);
</script>

<style scoped>
.bg-overlay {
  background-color: #0000004d;
}
.custom-border-style {
  border: 1px solid #d1d5db;
  transition: border-color 0.15s ease-in-out;
}
.custom-border-style:hover {
  border-color: #9ca3af !important;
}
.custom-border-style:focus-within,
.custom-border-style:focus {
  border-color: #009b71 !important;
  outline: none;
  box-shadow: none;
}
.form-group {
  margin-bottom: 16px;
}
.form-group:last-child {
  margin-bottom: 0;
}
.ms-input-label {
  width: 180px;
  color: #262626;
  font-weight: 500;
  font-size: 13px;
}
.action-container {
  height: 52px;
  border-top: 1px solid #d5dfe2;
}
</style>