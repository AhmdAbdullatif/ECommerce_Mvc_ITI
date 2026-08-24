using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync(
        string? searchText = null,
        int? categoryId = null,
        string? sortOrder = null)
    {
        IQueryable<Product> query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(
                p => p.Name.Contains(searchText));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(
                p => p.CategoryId == categoryId.Value);
        }

        query = sortOrder switch
        {
            "price_asc" =>
                query.OrderBy(p => p.Price),

            "price_desc" =>
                query.OrderByDescending(p => p.Price),

            _ =>
                query.OrderBy(p => p.Name)
        };

        return await query.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetByIdAndUserAsync(
        int id,
        string userId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(
                p => p.Id == id &&
                     p.UserId == userId);
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string userId,
        int categoryId,
        string name,
        string description,
        int quantity,
        decimal price,
        string pictureUri)
    {
        int affectedRows =
            await _context.Products
                .Where(p =>
                    p.Id == id &&
                    p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(
                        p => p.CategoryId,
                        categoryId)
                    .SetProperty(
                        p => p.Name,
                        name)
                    .SetProperty(
                        p => p.Description,
                        description)
                    .SetProperty(
                        p => p.Quantity,
                        quantity)
                    .SetProperty(
                        p => p.Price,
                        price)
                    .SetProperty(
                        p => p.PictureUri,
                        pictureUri));

        return affectedRows > 0;
    }

    public void Delete(Product product)
    {
        _context.Products.Remove(product);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}