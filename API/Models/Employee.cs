using System;

namespace API.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public float Salary { get; set; }
    }
}