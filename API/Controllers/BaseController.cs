using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public abstract class BaseController(ILogger<BaseController> logger) : ControllerBase
{
    protected int GetUserId()
    {
        if (User == null || !User.Claims.Any())
        {
            logger.LogError("User claims are not available.");
            throw new InvalidOperationException("User claims are not available.");
        }

        var userIdClaim = User.Claims.FirstOrDefault(i => i.Type == "UserId");
        if (userIdClaim == null)
        {
            logger.LogError("UserId claim is missing.");
            throw new InvalidOperationException("UserId claim is missing.");
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            logger.LogError("UserId claim is not a valid integer.");
            throw new InvalidOperationException("UserId claim is not a valid integer.");
        }

        return userId;
    }
}