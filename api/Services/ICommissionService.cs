using FCamara.CommissionCalculator.Models;

namespace FCamara.CommissionCalculator.Services
{
    public interface ICommissionService
    {
        CommissionCalculationResponse CalculateCommissions(CommissionCalculationRequest request);
    }
}
