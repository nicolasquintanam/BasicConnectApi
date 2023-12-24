using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicConnectApi.Data;

namespace BasicConnectApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MigrationController : ControllerBase
{
    private readonly IServiceProvider _serviceProvider;

    public MigrationController(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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

            return Ok("Migrations executed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error executing migrations: {ex.Message}");
        }
    }
}
