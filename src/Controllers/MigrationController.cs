namespace BasicConnectApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicConnectApi.Data;
using BasicConnectApi.Models;

[ApiController]
[Route("api/v1/[controller]")]
public class MigrationController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationController> _logger;

    public MigrationController(IServiceProvider serviceProvider, ILogger<MigrationController> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    [HttpGet("run-migrations")]
    public IActionResult RunMigrations()
    {
        try
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            return Ok(new BaseResponse(true));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Run migrations exception");
            return StatusCode(500, new BaseResponse(false));

        }
    }
}
