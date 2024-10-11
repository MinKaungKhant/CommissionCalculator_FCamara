using FCamara.CommissionCalculator.Models;

namespace FCamara.CommissionCalculator.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly ILogger<CommissionService> _logger;
        private readonly IConfiguration _configuration;

        public CommissionService(ILogger<CommissionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public CommissionCalculationResponse CalculateCommissions(CommissionCalculationRequest request)
        {
            _logger.LogDebug("Starting commission calculations.");

            var fcamaraLocalRate = _configuration.GetValue<decimal>("CommissionRates:FCamaraLocal");
            var fcamaraForeignRate = _configuration.GetValue<decimal>("CommissionRates:FCamaraForeign");
            var competitorLocalRate = _configuration.GetValue<decimal>("CommissionRates:CompetitorLocal");
            var competitorForeignRate = _configuration.GetValue<decimal>("CommissionRates:CompetitorForeign");

            var fcamaraLocalCommission = request.LocalSalesCount * request.AverageSaleAmount * fcamaraLocalRate;
            var fcamaraForeignCommission = request.ForeignSalesCount * request.AverageSaleAmount * fcamaraForeignRate;

            var competitorLocalCommission = request.LocalSalesCount * request.AverageSaleAmount * competitorLocalRate;
            var competitorForeignCommission = request.ForeignSalesCount * request.AverageSaleAmount * competitorForeignRate;

            _logger.LogDebug("FCamara Local Commission: {FCamaraLocalCommission}, FCamara Foreign Commission: {FCamaraForeignCommission}",
                fcamaraLocalCommission, fcamaraForeignCommission);
            _logger.LogDebug("Competitor Local Commission: {CompetitorLocalCommission}, Competitor Foreign Commission: {CompetitorForeignCommission}",
                competitorLocalCommission, competitorForeignCommission);

            var response = new CommissionCalculationResponse
            {
                FCamaraCommissionAmount = fcamaraLocalCommission + fcamaraForeignCommission,
                CompetitorCommissionAmount = competitorLocalCommission + competitorForeignCommission
            };

            _logger.LogInformation("Commission calculations completed successfully.");

            return response;
        }
    }
}
