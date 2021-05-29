using System;

namespace API.Models
{
    public enum Position
    {
        Developer,
        HR,
        QA,
        DataAnalyst
    }

    public class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public float Salary { get; set; }
        public Position Position { get; set; }
        public float MonthsOfExperience { get; set; }
    }
}