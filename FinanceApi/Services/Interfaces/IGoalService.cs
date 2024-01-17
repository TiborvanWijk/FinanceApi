﻿using FinanceApi.Data.Dtos;
using FinanceApi.Models;

namespace FinanceApi.Services.Interfaces
{
    public interface IGoalService
    {
        Goal GetById(int goalId, bool tracking);
        bool TryGetAllOrderedOrDefault(string userId, out ICollection<Goal> goals, out int errorCode,
            out string errorMessage, DateTime? startDate, DateTime? endDate, string? listOrderBy, string? listDir);
        decimal GetProgressAmountOfGoal(string userId, int goalId);
        bool HasGoals(string userId);
        bool ExistsById(string userId, int goalId);
        bool ExistsByTitle(string userId, string title);
        bool ValidateGoal(GoalManageDto goalDto, out int errorCode, out string errorMessage);
        bool AddCategory(GoalCategory goalCategory);
        bool Delete(Goal goal);
        bool Update(User user, GoalManageDto goalDto, out int errorCode, out string errorMessage);
        bool Create(User user, GoalManageDto goalDto, out int errorCode, out string errorMessage);
        bool AddCategories(string userId, int goalId, ICollection<int> categoryIds, out string errorMessage, out int errorCode);
        bool TryGetGoalsByCategoryId(User user, int categoryId, out ICollection<Goal> goals, out int errorCode, out string errorMessage);
        bool TryDeleteGoal(User user, int goalId, out int errorCode, out string errorMessage);
        bool TryRemoveCategories(User user, int goalId, ICollection<int> categoryIds, out int errorCode, out string errorMessage);
    }
}
