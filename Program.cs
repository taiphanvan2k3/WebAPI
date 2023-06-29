using System.Text;
using LearnApiWeb.Data;
using LearnApiWeb.Extensions;
using LearnApiWeb.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BookStoreconnectionString");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình để các ứng dụng khác có thể xài
builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
));

builder.Services.AddDbContext<BookStoreContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
    }
);

// Cần đưng kí AutoMapper mới có thể sử dụng
builder.Services.AddAutoMapper(typeof(Program));

// Extension methods
builder.Services.AddDependencyService();

// Để tự động map AppSettings có SecretKey vào đối tượng AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Dựa vào chuỗi secretkey này để mã hóa và sinh ra JWT;
// Lấy ra secretKey lưu ở appsetting.json
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

// Configure để add JWT và chú ý là phải đặt trước khi builder được build
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        // Tự cấp token, nếu không muốn thì set là true và phải chỉ đường dẫn đến Auth0 chẳng hạn
        ValidateIssuer = false,
        ValidateAudience = false,

        // Ký vào token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

        ClockSkew = TimeSpan.Zero
    };
});

// Chú ý phải add dependency trước builder.Build()
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Chú ý là đặt Authentication trước Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
