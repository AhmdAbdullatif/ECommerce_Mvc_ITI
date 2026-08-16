using ECommerce_Mvc.Models;

namespace ECommerce_Mvc.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();

    Task<Category?> GetByIdAsync(int id);

    Task AddAsync(Category category);

    Task<bool> UpdateAsync(
        int id,
        string name);

    void Delete(Category category);

    Task<bool> ExistsAsync(int id);

    Task SaveChangesAsync();
}