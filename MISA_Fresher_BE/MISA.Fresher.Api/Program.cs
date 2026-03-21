using MISA.Fresher.Core.Interfaces.Repository;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.Middlewares;
using MISA.Fresher.Core.Services;
using MISA.Fresher.Infrastructure.Repositories;
// Khởi tạo WebApplication Builder
var builder = WebApplication.CreateBuilder(args);

// Cấu hình Dapper
// <summary>
// Cấu hình Dapper tự động ánh xạ tên thuộc tính PascalCase trong C# sang tên cột snake_case trong DB.
// Ví dụ: EmployeeCode -> employee_code.
// </summary>
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Đăng ký Service và Repository (Dependency Injection)
// <summary>
// Đăng ký Generic Base Repository và Base Service (Scoped Lifetime)
// Đảm bảo mỗi request HTTP sẽ nhận được một instance mới của các đối tượng này.
// </summary>
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

// <summary>
// Đăng ký WorkShift-specific Service và Repository (Scoped Lifetime)
// </summary>
builder.Services.AddScoped<IWorkShiftService, WorkShiftService>();
builder.Services.AddScoped<IWorkShiftRepository, WorkShiftRepository>();

// Config CORS (Cross-Origin Resource Sharing)
// <summary>
// Cấu hình chính sách CORS để cho phép Frontend (VueJS) truy cập API từ một origin khác.
// </summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        // <summary>
        // Chỉ cho phép request từ origin http://localhost:5173 (môi trường phát triển Vue).
        // </summary>
        policy.WithOrigins("http://localhost:5173")
             // <summary>
             // Cho phép tất cả các header được gửi trong request.
             // </summary>
             .AllowAnyHeader()
             // <summary>
             // Cho phép tất cả các method HTTP (GET, POST, PUT, DELETE, etc.).
             // </summary>
             .AllowAnyMethod();
    });
});

// Add services to the container.
// <summary>
// Đăng ký các dịch vụ MVC/Controller.
// </summary>
builder.Services.AddControllers();

// <summary>
// Cấu hình cho Swagger/OpenAPI.
// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Xây dựng ứng dụng
var app = builder.Build();

// Configure the HTTP request pipeline.
// <summary>
// Cấu hình sử dụng Swagger UI chỉ trong môi trường phát triển (Development).
// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Cấu hình Middleware
// <summary>
// Middleware xử lý lỗi tập trung. Nó sẽ bắt các Exception (ví dụ: MISAValidateException) 
// và chuyển chúng thành phản hồi HTTP chuẩn (thường là 400 hoặc 500).
// </summary>
app.UseMiddleware<ErrorExceptionMiddleware>();

// <summary>
// Áp dụng chính sách CORS đã định nghĩa ("AllowFrontend"). Phải đặt trước UseAuthorization và MapControllers.
// </summary>
app.UseCors("AllowFrontend");

// <summary>
// Kích hoạt Middleware Xác thực (Authorization).
// </summary>
app.UseAuthorization();

// <summary>
// Ánh xạ các Controller endpoints.
// </summary>
app.MapControllers();

// Khởi chạy ứng dụng
app.Run();