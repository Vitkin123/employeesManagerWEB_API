using System;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Database;
using API.Models;
using API.Services.Interfaces;
using AuthenticationPlugin;
using AutoMapper;
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
        private IMapper _mapper;
        private ISalaryCalculationService _salaryCalculationService;

        public AuthController(DataContext context, IConfiguration configuration, IMapper mapper,
            ISalaryCalculationService calculationService)
        {
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _mapper = mapper;
            _salaryCalculationService = calculationService;
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

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, employee.Email),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employeeInDatabase.Role)
            };
            var token = _auth.GenerateAccessToken(claims);

            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                employee_id = employeeInDatabase.Id,
                role = employeeInDatabase.Role
            });
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] Employee employee)
        {
            var employeeWithTheSameEmail = await _context.Employees
                .SingleOrDefaultAsync(e => e.Email == employee.Email);

            if (employeeWithTheSameEmail != null)
                return BadRequest("Employee with the same email already exists.");


            var newEmployeeObj = new Employee();

            _mapper.Map(employee, newEmployeeObj);

            newEmployeeObj.Salary =
                _salaryCalculationService.CalculateSalary(employee.MonthsOfExperience, employee.Position);

            await _context.Employees.AddAsync(newEmployeeObj);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}