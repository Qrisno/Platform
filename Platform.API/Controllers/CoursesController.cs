using Microsoft.AspNetCore.Mvc;
using Platform.Application.DTOs;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Domain.Entities.Models;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class CoursesController : ControllerBase
{
    private CourseService _courseService;
    public CoursesController(CourseService courseService)
    {
        _courseService = courseService;
    }
    [HttpGet("GetCoursesByAuthorId/{authorId}")]
    public async Task<IActionResult> GetCoursesByAuthorId([FromQuery] int id)
    {
        CourseResponse result = await _courseService.GetCoursesByAuthor(id);
        if (result.courses.Count == 0)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        return Ok(result);
    }

    [HttpGet("GetCoursesByStudentId/{StudentId}")]
    public async Task<IActionResult> GetCoursesByStudentId([FromQuery] int id)
    {
        CourseResponse result = await _courseService.GetCoursesByStudent(id);
        if (result.courses.Count == 0)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        return Ok(result);
    }

    [HttpPost("AddCourse")]
    public async Task<IActionResult> AddCourse([FromBody] AddCourseDTO course)
    {
        CourseResponse result = await _courseService.AddCourse(course);
        if (result.result == CourseSearchResultEnum.UserNotFound)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        if (result.result == CourseSearchResultEnum.UserNotAuthorized)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        return Ok(result);
    }


    [HttpGet("Search")]
    public IActionResult SearchCourses()
    {
        return Ok();
    }

    [HttpPost("Enroll")]
    public async Task<IActionResult> Enroll(CourseToEnrollDTO course)
    {
        CourseResponse result = await _courseService.Enroll(course);
        if (result.result == CourseSearchResultEnum.UserNotFound || result.result == CourseSearchResultEnum.NotFound)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        return Ok(result);
    }
}