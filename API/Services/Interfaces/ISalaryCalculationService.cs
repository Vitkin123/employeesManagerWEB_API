using API.Models;

namespace API.Services.Interfaces
{
    public interface ISalaryCalculationService
    {
        public float CalculateSalary(float experience, Position position);
    }
}