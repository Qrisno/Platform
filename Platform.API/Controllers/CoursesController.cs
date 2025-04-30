using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{
    [HttpGet("GetCoursesByAuthorId/{authorId}")]
    public IActionResult GetCoursesByAuthorId([FromBody] int id)
    {
        return Ok();
    }

    [HttpGet("GetCoursesByStudentId/{StudentId}")]
    public IActionResult GetCoursesByStudentId([FromBody] int id)
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult SearchCourses()
    {
        return Ok();
    }
}