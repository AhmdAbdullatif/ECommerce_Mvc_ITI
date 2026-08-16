using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Interfaces;
using ECommerce_Mvc.Services.Interfaces;

namespace ECommerce_Mvc.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(
        IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllAsync(
        string? searchText = null,
        int? categoryId = null,
        string? sortOrder = null)
    {
        return await _productRepository.GetAllAsync(
            searchText,
            categoryId,
            sortOrder);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product?> GetSellerProductAsync(
        int id,
        string userId)
    {
        return await _productRepository
            .GetByIdAndUserAsync(id, userId);
    }

    public async Task AddAsync(Product product)
    {
        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        int id,
        string userId,
        int categoryId,
        string name,
        string description,
        int quantity,
        decimal price,
        string pictureUri)
    {
        bool updated =
            await _productRepository.UpdateAsync(
                id,
                userId,
                categoryId,
                name,
                description,
                quantity,
                price,
                pictureUri);

        if (!updated)
        {
            throw new InvalidOperationException(
                "Product not found or you are not allowed to edit it.");
        }
    }

    public async Task DeleteAsync(
        int id,
        string userId)
    {
        Product? product =
            await _productRepository
                .GetByIdAndUserAsync(id, userId);

        if (product is null)
        {
            throw new InvalidOperationException(
                "Product not found or you are not allowed to delete it.");
        }

        _productRepository.Delete(product);

        await _productRepository.SaveChangesAsync();
    }
}