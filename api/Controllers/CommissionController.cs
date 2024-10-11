using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Services;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.CommissionCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommissionController : ControllerBase
    {
        private readonly ILogger<CommissionController> _logger;
        private readonly ICommissionService _commissionService;

        public CommissionController(ILogger<CommissionController> logger, ICommissionService commissionService)
        {
            _logger = logger;
            _commissionService = commissionService;
        }

        [ProducesResponseType(typeof(CommissionCalculationResponse), 200)]
        [HttpPost]
        [Route("CalculateCommission")]
        public IActionResult CalculateCommission([FromBody] CommissionCalculationRequest request)
        {
            _logger.LogInformation("CalculateCommission with LocalSalesCount: {LocalSalesCount}, ForeignSalesCount: {ForeignSalesCount}, AverageSalesAmount: {AverageSalesAmount}",
                request.LocalSalesCount, request.ForeignSalesCount, request.AverageSaleAmount);

            if (request == null || request.LocalSalesCount < 0 || request.ForeignSalesCount < 0 || request.AverageSaleAmount <= 0)
            {
                _logger.LogWarning("Invalid input data received.");
                return BadRequest("Invalid input data.");
            }

            try
            {
                var result = _commissionService.CalculateCommissions(request);
                _logger.LogInformation("Commission calculation successful. FCamaraTotalCommission: {FCamaraTotalCommission}, CompetitorTotalCommission: {CompetitorTotalCommission}",
                    result.FCamaraCommissionAmount, result.CompetitorCommissionAmount);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, "Error occurred while calculating commissions.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
