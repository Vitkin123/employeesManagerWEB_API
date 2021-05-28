using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Database;
using API.Models;
using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private DataContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }


        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] Employee employee)
        {
            var employeeInDatabase = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == employee.Email);

            if (employeeInDatabase == null)
                return NotFound("User not found");

            if (employeeInDatabase.Password != employee.Password)
                return Unauthorized();

            return Ok($"Welcome {employeeInDatabase.Name}.");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] Employee employee)
        {
            var employeeWithTheSameEmail = await _context.Employees
                .SingleOrDefaultAsync(e => e.Email == employee.Email);

            if (employeeWithTheSameEmail != null)
                return BadRequest("Employee with the same email already exists.");

            var employeeObj = new Employee
            {
                Name = employee.Name,
                Email = employee.Email,
                StartWorkingDate = DateTime.Now,
                BirthDate = employee.BirthDate,
                Salary = employee.Salary,
                LastName = employee.LastName,
                Password = employee.Password,
                Role = "Employee"
            };
            await _context.Employees.AddAsync(employeeObj);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}