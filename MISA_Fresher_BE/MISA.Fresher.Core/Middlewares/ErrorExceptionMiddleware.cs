using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MISA.Fresher.Core.Dtos;
using MISA.Fresher.Core.Enums;
using MISA.Fresher.Core.Exceptions;

namespace MISA.Fresher.Core.Middlewares
{
    /// <summary>
    /// Middleware xử lý các exception (ngoại lệ) xảy ra trong pipeline request/response
    /// và chuyển đổi chúng thành response chuẩn (ApiResponse) với mã lỗi HTTP phù hợp.
    /// </summary>
    /// Created by: HoanTD (03/12/2025)
    public class ErrorExceptionMiddleware
    {
        private readonly RequestDelegate _delegate;

        /// <summary>
        /// Khởi tạo middleware
        /// </summary>
        /// <param name="delegate">Request delegate tiếp theo trong pipeline</param>
        /// Created by: HoanTD (03/12/2025)
        public ErrorExceptionMiddleware(RequestDelegate @delegate)
        {
            _delegate = @delegate;
        }

        /// <summary>
        /// Phương thức chính xử lý request và bắt các exception.
        /// Sử dụng khối try-catch để chụp lấy các exception custom và exception chung.
        /// </summary>
        /// <param name="context">HTTP context hiện tại của request</param>
        /// Created by: HoanTD (03/12/2025)
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Gọi delegate tiếp theo trong pipeline
                await _delegate(context);
            }
            catch (MISAValidateException ex)
            {
                // Bắt lỗi Validation (HTTP 400 Bad Request)
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message,
                    "Validation Error",
                    new List<string> { ex.Message });
            }
            catch (MISANotFoundException ex)
            {
                // Bắt lỗi Không tìm thấy (HTTP 404 Not Found)
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message,
                    "Not Found Error");
            }
            catch (MISADuplicateException ex)
            {
                // Bắt lỗi Trùng lặp dữ liệu (HTTP 409 Conflict)
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.Conflict,
                    ex.Message,
                    "Duplicate Error",
                    new List<string>
                    {
                        $"Mã ca {ex.ExistingName} đã tồn tại", //$"{ex.DuplicateField} {ex.ExistingName} đã tồn tại", để xuất ra trường bị trùng ở đây chỉ cần mã ca
                    });
            }
            catch (Exception ex)
            {
                // Bắt lỗi chung (HTTP 500 Internal Server Error)
                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Có lỗi xảy ra, vui lòng liên hệ MISA để được hỗ trợ.",
                    ex.Message); // Đưa ex.Message vào systemMessage để debug
            }
        }

        /// <summary>
        /// Xử lý ngoại lệ, thiết lập HTTP Status Code và ghi response lỗi chuẩn ApiResponse<object>
        /// </summary>
        /// <param name="context">HTTP context hiện tại</param>
        /// <param name="statusCode">Mã lỗi HTTP (ví dụ: 400, 404, 409, 500)</param>
        /// <param name="userMessage">Thông báo lỗi hiển thị cho người dùng (ngôn ngữ thân thiện)</param>
        /// <param name="systemMessage">Thông báo lỗi chi tiết cho lập trình viên (System/Debug message)</param>
        /// <param name="validateInfo">Danh sách các lỗi validation chi tiết (nếu có)</param>
        /// <returns>Task</returns>
        /// Created by: HoanTD (03/12/2025)
        private async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string userMessage,
            string? systemMessage = null,
            List<string>? validateInfo = null)
        {
            // Thiết lập Response Content-Type và Status Code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Tạo đối tượng response lỗi chuẩn theo format ApiResponse
            var response = ApiResponse<object>.ErrorResponse
            (
                userMessage: userMessage,
                code: statusCode,
                systemMessage: systemMessage,
                validateInfo: validateInfo
            );

            // Serialize đối tượng response thành chuỗi JSON và ghi vào HTTP Response Body
            var jsonResponse = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
