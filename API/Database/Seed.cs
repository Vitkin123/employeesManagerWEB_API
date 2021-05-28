using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Database
{
    public class Seed
    {
        public static async Task SeedData(DataContext context)
        {
            if (context.Employees.Any()) return;
            var employees = new List<Employee>
            {
                new()
                {
                    Name = "Ivan",
                    LastName = "Ivanov",
                    BirthDate = new DateTime(1995, 12, 7),
                    Salary = 1000,
                    StartWorkingDate = new DateTime(2013, 10, 10)
                },
                new()
                {
                    Name = "Petr",
                    LastName = "Petrov",
                    BirthDate = new DateTime(1999, 9, 1),
                    Salary = 1000,
                    StartWorkingDate = new DateTime(2014, 1, 10)
                }
            };
            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}