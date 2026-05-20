using MenuNews.Domain.Common;
using MenuNews.Domain.Interfaces;
using MenuNews.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MenuNews.Api.Controllers;


// Endpoint hỗ trợ kiểm tra cấu hình &amp; MongoDB audit.

[ApiController]
[Route("api/[controller]")]
public class SystemController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuditLogQueryService _auditLogQuery;

    public SystemController(IConfiguration configuration, IAuditLogQueryService auditLogQuery)
    {
        _configuration = configuration;
        _auditLogQuery = auditLogQuery;
    }

    //Trạng thái bật/tắt các công nghệ trong project
    [HttpGet("status")]
    public ActionResult<ApiResponse<object>> GetStatus()
    {
        var mongo = _configuration.GetSection(MongoDbSettings.SectionName);
        var rabbit = _configuration.GetSection(RabbitMqSettings.SectionName);

        return Ok(ApiResponse<object>.Ok(new
        {
            sqlServer = new
            {
                enabled = true,
                connection = MaskConnectionString(_configuration.GetConnectionString("DefaultConnection"))
            },
            mongoDb = new
            {
                enabled = mongo.GetValue("Enabled", true),
                connectionString = mongo["ConnectionString"],
                database = mongo["DatabaseName"],
                collection = mongo["AuditCollectionName"]
            },
            rabbitMq = new
            {
                enabled = rabbit.GetValue("Enabled", true),
                host = rabbit["HostName"],
                port = rabbit.GetValue("Port", 5672),
                queue = rabbit["QueueName"]
            },
            features = new
            {
                cleanArchitecture = true,
                cqrsMediatR = true,
                fluentValidation = true,
                efCoreDbFirst = true,
                repositoryPattern = true,
                grpc = true,
                pagination = true,
                softDelete = true,
                manyToMany = true
            }
        }));
    }

    //Xem audit log gần nhất (MongoDB) — sau khi CRUD Menu/News.
    [HttpGet("audit-logs")]
    public async Task<ActionResult<ApiResponse<object>>> GetAuditLogs([FromQuery] int limit = 20)
    {
        var mongoEnabled = _configuration.GetValue($"{MongoDbSettings.SectionName}:Enabled", true);
        if (!mongoEnabled)
        {
            return Ok(ApiResponse<object>.Ok(
                Array.Empty<object>(),
                "MongoDb.Enabled = false — bật MongoDB trong appsettings để xem audit thật."));
        }

        var logs = await _auditLogQuery.GetRecentAsync(limit);
        return Ok(ApiResponse<object>.Ok(logs));
    }

    private static string? MaskConnectionString(string? cs)
    {
        if (string.IsNullOrEmpty(cs)) return cs;
        return System.Text.RegularExpressions.Regex.Replace(cs, @"Password=[^;]+", "Password=***", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
}
