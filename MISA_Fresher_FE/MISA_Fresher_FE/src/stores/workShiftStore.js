import { STATUS } from '@/enums/Enum';
import { defineStore } from 'pinia';

/**
 * Store Pinia để quản lý danh sách Ca làm việc (Work Shift).
 */
export const useWorkShiftStore = defineStore('workShift', {
  
  /**
   * State: Định nghĩa dữ liệu trạng thái.
   */
  state: () => ({
    // Danh sách các đối tượng ca làm việc
    workShifts: [],
    // Tổng số bản ghi (dùng cho phân trang/thống kê)
    totalRecords: 0,
  }),
  
  /**
   * Actions: Định nghĩa các hàm thay đổi trạng thái (logic nghiệp vụ).
   */
  actions: {
    /**
     * <summary>
     * Thiết lập/ghi đè danh sách ca làm việc và tổng số bản ghi.
     * Thường được gọi sau khi fetch dữ liệu từ API.
     * </summary>
     * @param {Array} data - Dữ liệu danh sách ca.
     * @param {number} total - Tổng số bản ghi.
     */
    setWorkShifts(data, total) {
      this.workShifts = data;
      this.totalRecords = total;
    },

    /**
     * <summary>
     * Thêm một ca làm việc mới vào đầu danh sách (dùng cho thao tác thêm thành công).
     * </summary>
     * @param {Object} newShift - Dữ liệu ca làm việc mới.
     */
    addWorkShift(newShift) {
      // Thêm vào đầu danh sách
      this.workShifts = [newShift, ...this.workShifts];
      // Tăng tổng số bản ghi
      this.totalRecords++;
    },
    
    /**
     * <summary>
     * Cập nhật một ca làm việc trong danh sách (dùng cho thao tác sửa thành công).
     * </summary>
     * @param {Object} updatedShift - Dữ liệu ca làm việc đã cập nhật.
     */
    updateWorkShift(updatedShift) {
      // Tìm index của ca làm việc cần cập nhật
      const index = this.workShifts.findIndex(
        (shift) => shift.workShiftId === updatedShift.workShiftId
      );
      
      // Nếu tìm thấy, cập nhật đối tượng ca làm việc
      if (index !== -1) {
        this.workShifts[index] = { ...updatedShift };
      }
    },
    
    /**
     * <summary>
     * Cập nhật trạng thái (Sử dụng/Ngừng sử dụng) cho nhiều ca làm việc cùng lúc (Bulk update).
     * </summary>
     * @param {Object} payload - Chứa { ids: Array<string>, newStatus: boolean (true=ACTIVE, false=INACTIVE) }
     */
    updateStatusBatch(payload) {
      const idSet = new Set(payload.ids);
      const statusToSet = payload.newStatus ? STATUS.ACTIVE : STATUS.INACTIVE;

      // Duyệt qua danh sách hiện tại và cập nhật trạng thái nếu ID nằm trong tập hợp
      this.workShifts.forEach(shift => {
        if (idSet.has(shift.workShiftId)) {
          shift.workShiftStatus = statusToSet;
        }
      });
    },
    
    /**
     * <summary>
     * Xóa nhiều ca làm việc khỏi danh sách (Bulk delete).
     * </summary>
     * @param {Array<string>} ids - Danh sách ID của các ca làm việc cần xóa.
     */
    deleteBatch(ids) {
      const idSet = new Set(ids);
      // Lọc ra các ca làm việc không nằm trong danh sách cần xóa
      this.workShifts = this.workShifts.filter(shift => !idSet.has(shift.workShiftId));
      // Giảm tổng số bản ghi
      this.totalRecords -= ids.length;
    },
  },
});