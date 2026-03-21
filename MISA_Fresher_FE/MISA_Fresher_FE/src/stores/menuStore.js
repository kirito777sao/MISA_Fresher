import { defineStore } from 'pinia';

/**
 * Store Pinia để quản lý trạng thái mở/đóng của các Sub-menu.
 */
export const useMenuStore = defineStore('menu', {
  /**
   * State: Định nghĩa dữ liệu trạng thái của Store.
   */
  state: () => ({
    /**
     * openEndpoint: Lưu trữ endpoint (đường dẫn) của Sub-menu đang được mở.
     * Chỉ một Sub-menu có thể mở tại một thời điểm (Accordion behavior).
     * Giá trị rỗng ('') có nghĩa là không có Sub-menu nào đang mở.
     */
    openEndpoint: '',
  }),
  
  /**
   * Actions: Định nghĩa các hàm thay đổi trạng thái.
   */
  actions: {
    /**
     * <summary>
     * Chuyển đổi trạng thái mở/đóng của sub-menu dựa trên endpoint.
     * Nếu endpoint hiện tại đang mở, nó sẽ đóng lại. Ngược lại, nó sẽ mở 
     * endpoint mới (và đóng endpoint cũ nếu có).
     * </summary>
     * @param {string} endpoint - Endpoint (đường dẫn hoặc ID duy nhất) của menu item.
     * Created by HoanTD - 2/12/2025
     */
    toggleOpenEndpoint(endpoint) {
      // Nếu endpoint truyền vào trùng với endpoint đang mở, thì đóng nó lại.
      if (this.openEndpoint === endpoint) {
        this.openEndpoint = '';
      } 
      // Ngược lại, mở endpoint mới (và tự động đóng endpoint cũ).
      else {
        this.openEndpoint = endpoint;
      }
    },
  },
});