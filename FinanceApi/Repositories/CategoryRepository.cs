﻿using FinanceApi.Data;
using FinanceApi.Models;
using FinanceApi.Repositories.Interfaces;

namespace FinanceApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext dataContext;

        public CategoryRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public bool Create(Category category)
        {
            dataContext.Categories.Add(category);
            return Save();
        }

        public bool Delete(Category category)
        {
            dataContext.Categories.Remove(category);
            return Save();
        }

        public bool ExistsById(int id)
        {
            return dataContext.Categories.Any(c => c.Id == id);
        }

        public bool ExistsBytitle(string title)
        {
            return dataContext.Categories.Any(c => c.Title.Equals(title));
        }

        public ICollection<Category> GetAllOfUser(string userId)
        {
            return dataContext.Categories.Where(c => c.User.Id.Equals(userId)).ToList();
        }

        public Category GetById(int categoryId)
        {
            return dataContext.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0;
        }

        public bool Update(Category category)
        {
            dataContext.Categories.Update(category);
            return Save();
        }
    }
}
