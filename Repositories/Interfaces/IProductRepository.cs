using ECommerce_Mvc.Models;

namespace ECommerce_Mvc.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(
        string? searchText = null,
        int? categoryId = null,
        string? sortOrder = null);

    Task<Product?> GetByIdAsync(int id);

    Task<Product?> GetByIdAndUserAsync(
        int id,
        string userId);

    Task AddAsync(Product product);

    Task<bool> UpdateAsync(
        int id,
        string userId,
        int categoryId,
        string name,
        string description,
        int quantity,
        decimal price,
        string pictureUri);

    void Delete(Product product);

    Task<bool> ExistsAsync(int id);

    Task SaveChangesAsync();
}