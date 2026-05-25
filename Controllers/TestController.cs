using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok("Server is working");
    }
}