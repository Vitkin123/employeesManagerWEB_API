using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Database;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeesController : ControllerBase
    {
        private DataContext _dbContext;
        private IMapper _mapper;

        public EmployeesController(DataContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }


        [HttpGet]
        [Authorize]
        public async Task<ICollection> GetEmployees()
        {
            var allEmployees = await _dbContext.Employees.ToListAsync();
            var employeesToReturn = allEmployees
                .Select(employee => new // Here we making anon type to return specific fields available to employee 
                {
                    employee.Id,
                    employee.Name,
                    employee.LastName,
                })
                .ToList();
            return employeesToReturn;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<List<Employee>> GetAllEmployeesData()
        {
            return await _dbContext.Employees.ToListAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
                return NotFound("Employee not found.");


            return Ok(employee);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeData(Guid id, Employee employeeObject)
        {
            var employee = await _dbContext.Employees.FindAsync(id);
            if (employee == null)
                return NotFound("Employee not found");

            //Automapping all props to updated object in db

            _mapper.Map(employeeObject, employee);
            await _dbContext.SaveChangesAsync();
            return Ok("Record updated successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employeeToDelete = await _dbContext.Employees.FindAsync(id);

            if (employeeToDelete == null)
                return NotFound("Employee not found.");

            _dbContext.Employees.Remove(employeeToDelete);
            await _dbContext.SaveChangesAsync();
            return Ok("Employee deleted.");
        }
    }
}