using ECommerce_Mvc.Models;

namespace ECommerce_Mvc.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task AddAsync(Category category);

    Task UpdateAsync(
        int id,
        string name);

    Task DeleteAsync(int id);
}