# Package cho JwtBearer
- Microsoft.AspNetCore.Authentication.JwtBearer
- Configure tại Program.cs
```
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
```
- Lưu ý: các DI chỉ có thể sử dụng khi nó được add trước dòng lệnh ```builder.Build()``` trong file ```Program.cs```. Nếu sau khi Build() mà đi add DI thì sẽ bị báo lỗi ngày về lỗi ```read-only```
- Các API có attribute là [Authorize] nên chỉ có đăng nhập
thành công thì mới dùng API này được vì nó yêu cầu 1 token mà token này chỉ được
generate sau khi login thành công. Và chú ý rằng token sẽ có thời hạn và sau khoảng thời gian này sẽ không còn dùng được nữa.

# Cách để tự động map các thuôc tính của 1 biến trong appsetting.json
- Vd trong appsetting.json có:
```
"AppSettings": {
    "SecretKey": "jojflulkumfrguvjdjiiquayyndxtzdf"
}
```
- Để có thể tự động map giá trị trong biến ```AppSettings``` ở trên vào 1 đối tượng thì ta cần thêm DI vào ```program.cs``` như sau:
```
// Để tự động map AppSettings có SecretKey vào đối tượng AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
```
- Và lưu ý rằng trong class ```AppSettings.cs``` phải có các thuộc tính có tên tương ứng với các thuộc tính trong biến ```AppSettings``` lưu trong file ```appsettin.json```