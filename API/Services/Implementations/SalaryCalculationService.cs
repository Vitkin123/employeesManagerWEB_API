using System;
using API.Models;
using API.Services.Interfaces;

namespace API.Core
{
    public class SalaryCalculationService : ISalaryCalculationService
    {
        public float CalculateSalary(float experience, Position position)
        {
            var coefficient = position switch
            {
                Position.Developer => 450,
                Position.QA => 300,
                Position.DataAnalyst => 400,
                Position.HR => 300,
                _ => throw new ArgumentOutOfRangeException(nameof(position), position, "Wrong position.")
            };

            return experience * coefficient;
        }
    }
}