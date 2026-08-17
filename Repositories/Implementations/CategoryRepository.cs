using ECommerce_Mvc.Data;
using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_Mvc.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public async Task<bool> UpdateAsync(
        int id,
        string name)
    {
        int affectedRows =
            await _context.Categories
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(
                        c => c.Name,
                        name));

        return affectedRows > 0;
    }

    public void Delete(Category category)
    {
        _context.Categories.Remove(category);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}