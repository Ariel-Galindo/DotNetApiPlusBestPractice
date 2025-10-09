using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Api.Controllers
{
    // https://localhost:portnumber/api/{version}/students
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class StudentsController : ControllerBase
    {
        // GET: https://localhost:portnumber/api/v1/students
        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult GetAllStudentsV1()
        {
            string[] studentNames = new string[] { "John", "Jane", "Jack", "Jill" };

            return Ok(studentNames);
        }

        // GET: https://localhost:portnumber/api/v1/students
        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult GetAllStudentsV2()
        {
            string[] studentNames = new string[] { "John 2", "Jane 2", "Jack 2", "Jill 2" };

            return Ok(studentNames);
        }
    }
}
