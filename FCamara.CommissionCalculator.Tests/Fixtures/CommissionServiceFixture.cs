using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.CommissionCalculator.Tests.Fixtures
{
    public class CommissionServiceFixture
    {
        public Mock<ICommissionService> MockCommissionService { get; }

        public CommissionServiceFixture()
        {
            MockCommissionService = new Mock<ICommissionService>();
            SetupDefaultBehavior();
        }

        private void SetupDefaultBehavior()
        {
            MockCommissionService.Setup(service => service.CalculateCommissions(It.IsAny<CommissionCalculationRequest>()))
                .Returns((CommissionCalculationRequest request) => new CommissionCalculationResponse
                {
                    FCamaraCommissionAmount = request.LocalSalesCount * request.AverageSaleAmount * 0.20m
                                             + request.ForeignSalesCount * request.AverageSaleAmount * 0.35m,
                    CompetitorCommissionAmount = request.LocalSalesCount * request.AverageSaleAmount * 0.02m
                                                 + request.ForeignSalesCount * request.AverageSaleAmount * 0.0755m
                });
        }
    }
}
