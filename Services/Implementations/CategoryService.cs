using ECommerce_Mvc.Models;
using ECommerce_Mvc.Repositories.Interfaces;
using ECommerce_Mvc.Services.Interfaces;

namespace ECommerce_Mvc.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Category category)
    {
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        int id,
        string name)
    {
        bool updated =
            await _categoryRepository.UpdateAsync(
                id,
                name);

        if (!updated)
        {
            throw new InvalidOperationException(
                "Category not found.");
        }
    }

    public async Task DeleteAsync(int id)
    {
        Category? category =
            await _categoryRepository.GetByIdAsync(id);

        if (category is null)
        {
            throw new InvalidOperationException(
                "Category not found.");
        }

        _categoryRepository.Delete(category);

        await _categoryRepository.SaveChangesAsync();
    }
}