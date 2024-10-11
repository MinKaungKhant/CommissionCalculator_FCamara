using FCamara.CommissionCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCamara.CommissionCalculator.Tests.Helpers
{
    public static class MockDataHelper
    {
        public static CommissionCalculationRequest CreateValidRequest(int localSales, int foreignSales, decimal averageAmount)
        {
            return new CommissionCalculationRequest
            {
                LocalSalesCount = localSales,
                ForeignSalesCount = foreignSales,
                AverageSaleAmount = averageAmount
            };
        }
    }
}
