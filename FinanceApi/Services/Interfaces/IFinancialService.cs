﻿using FinanceApi.Models;

namespace FinanceApi.Services.Interfaces
{
    public interface IFinancialService
    {
        bool tryGetSavingsRate(string userId, out decimal savingsRate, DateTime? startDate, DateTime? endDate, out int errorCode, out string errorMessage);
        bool TryGetNetIncomeInTimePeriod(string userId, DateTime? startDate, DateTime? endDate, out decimal netIncome, out int errorCode, out string errorMessage);
    }
}
