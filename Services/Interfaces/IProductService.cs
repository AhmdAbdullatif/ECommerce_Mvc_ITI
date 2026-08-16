using ECommerce_Mvc.Models;

namespace ECommerce_Mvc.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(
        string? searchText = null,
        int? categoryId = null,
        string? sortOrder = null);

    Task<Product?> GetByIdAsync(int id);

    Task<Product?> GetSellerProductAsync(
        int id,
        string userId);

    Task AddAsync(Product product);

    Task UpdateAsync(
        int id,
        string userId,
        int categoryId,
        string name,
        string description,
        int quantity,
        decimal price,
        string pictureUri);

    Task DeleteAsync(
        int id,
        string userId);
}