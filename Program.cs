using LearnApiWeb.Data;
using LearnApiWeb.Extensions;
using Microsoft.EntityFrameworkCore;

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

// Extension methods
builder.Services.AddDependencyService();

// Cần đưng kí AutoMapper mới có thể sử dụng
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddDbContext<BookStoreContext>(
    options =>
    {
        options.UseSqlServer(connectionString);
    }
);

builder.AddIdentityAndAuthenticationExtension();
builder.ConfigurePolicyIdentityExtension();

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
