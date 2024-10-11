using FCamara.CommissionCalculator.Controllers;
using FCamara.CommissionCalculator.Models;
using FCamara.CommissionCalculator.Services;
using FCamara.CommissionCalculator.Tests.Fixtures;
using FCamara.CommissionCalculator.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace FCamara.CommissionCalculator.Tests.Controllers
{
    public class CommissionCalculatorControllerTests : IClassFixture<CommissionServiceFixture>
    {
        private readonly CommissionController _controller;
        private readonly CommissionServiceFixture _fixture;

        public CommissionCalculatorControllerTests(CommissionServiceFixture fixture)
        {
            _fixture = fixture;
            var mockLogger = new Mock<ILogger<CommissionController>>();
            _controller = new CommissionController(mockLogger.Object, _fixture.MockCommissionService.Object);
        }

        [Fact]
        public void CalculateCommission_ValidInput_ReturnsCorrectCommissions()
        {
            // Arrange
            var request = MockDataHelper.CreateValidRequest(10, 10, 100m);

            // Act
            var result = _controller.CalculateCommission(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseValue = result.Value as CommissionCalculationResponse;
            Assert.NotNull(responseValue);
            Assert.Equal(550m, responseValue.FCamaraCommissionAmount);
            Assert.Equal(95.5m, responseValue.CompetitorCommissionAmount);
        }

        [Theory]
        [InlineData(-1, 10, 100, "Invalid input data.")]
        [InlineData(10, -5, 100, "Invalid input data.")]
        [InlineData(10, 10, -100, "Invalid input data.")]
        public void CalculateCommission_InvalidInput_ReturnsBadRequest(int localSales, int foreignSales, decimal avgAmount, string expectedError)
        {
            // Arrange
            var request = new CommissionCalculationRequest
            {
                LocalSalesCount = localSales,
                ForeignSalesCount = foreignSales,
                AverageSaleAmount = avgAmount
            };

            // Act
            var result = _controller.CalculateCommission(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(expectedError, result.Value);
        }

        [Fact]
        public void CalculateCommission_ZeroSales_ReturnsZeroCommissions()
        {
            // Arrange
            var request = MockDataHelper.CreateValidRequest(0, 0, 100m);

            // Act
            var result = _controller.CalculateCommission(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseValue = result.Value as CommissionCalculationResponse;
            Assert.NotNull(responseValue);
            Assert.Equal(0m, responseValue.FCamaraCommissionAmount);
            Assert.Equal(0m, responseValue.CompetitorCommissionAmount);
        }

        [Fact]
        public void CalculateCommission_HighValues_ReturnsCorrectCommissions()
        {
            // Arrange
            var request = MockDataHelper.CreateValidRequest(1000, 500, 1000m);

            // Act
            var result = _controller.CalculateCommission(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var responseValue = result.Value as CommissionCalculationResponse;
            Assert.NotNull(responseValue);


            // FCamara Commissions
            decimal expectedFCamaraLocal = 1000 * 1000m * 0.20m;
            decimal expectedFCamaraForeign = 500 * 1000m * 0.35m;
            Assert.Equal(expectedFCamaraLocal + expectedFCamaraForeign, responseValue.FCamaraCommissionAmount);

            // Competitor Commissions
            decimal expectedCompetitorLocal = 1000 * 1000m * 0.02m;
            decimal expectedCompetitorForeign = 500 * 1000m * 0.0755m;
            Assert.Equal(expectedCompetitorLocal + expectedCompetitorForeign, responseValue.CompetitorCommissionAmount);
        }
    }
}
