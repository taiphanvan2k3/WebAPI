using Microsoft.AspNetCore.Mvc;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadConfigureController : ControllerBase
    {
        [HttpGet("cach1")]
        public ActionResult ReadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
            var config = builder.Build();

            // Đi từ ngoài vào, ngăn cách nhau bởi dấu :
            var secretKey = config["AppSettings:SecretKey"];
            var connectionString = config["ConnectionStrings:BookStoreconnectionString"];
            var connectionStringv2 = config.GetConnectionString("BookStoreconnectionString");
            return Ok(new
            {
                SecretKey = secretKey,
                ConnectionString = connectionString,
                ConnectionStringv2 = connectionStringv2
            });
        }

        [HttpGet("cach2")]
        public ActionResult ReadConfigurationBySession()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");
            var config = builder.Build();

            /* 
            "MySetting": {
                
            }
            thì MySetting được gọi là section.
            Trong một section có thể có 
             */
            var appSettingSection = config.GetSection("AppSettings");
            var loggingSection = config.GetSection("Logging:LogLevel");
            var secretKey = appSettingSection["SecretKey"];
            return Ok(new
            {
                SecretKey = secretKey,
                Default = loggingSection["Default"]
            });
        }
    }
}