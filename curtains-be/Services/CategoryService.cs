using curtains_be.Common;
using curtains_be.DTOs.Requests;
using curtains_be.DTOs.Responses;
using curtains_be.Extensions;
using curtains_be.Models;
using curtains_be.Repository;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace curtains_be.Services;

public static class CategoryService
{
    public static async Task<ODataResponse<CategoryResponse>> GetAllAsync(
        IRepository<Category> repo, ODataQueryOptions<Category> queryOptions)
    {
        var query = repo.Query()
            .AsNoTracking()
            .Include(c => c.Products);

        var (count, items) = await query.AppendQueryOptionsAsync(queryOptions);

        return new ODataResponse<CategoryResponse>
        {
            Count = count,
            Value = items.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Title = c.Title,
                Subtitle = c.Subtitle,
                ProductCount = c.Products?.Count(p => !p.IsDeleted && p.IsActived) ?? 0,
                ImageUrl = c.ImageUrl,
                CoverImage = c.Products!
                    .Where(p => !p.IsDeleted && p.IsActived && p.ImageDetail != null)
                    .Select(p => p.ImageDetail)
                    .FirstOrDefault()
            })
        };
    }

    public static async Task<CategoryResponse?> GetByIdAsync(IRepository<Category> repo, int id)
    {
        return await repo.Query()
            .AsNoTracking()
            .Include(c => c.Products)
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Title = c.Title,
                Subtitle = c.Subtitle,
                ProductCount = c.Products != null ? c.Products.Count(p => !p.IsDeleted && p.IsActived) : 0
            })
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public static async Task<int> CreateAsync(IRepository<Category> repo, CategoryRequest req, string? createdBy)
    {
        var category = new Category
        {
            Title = req.Title,
            Subtitle = req.Subtitle,
            ImageUrl = req.ImageUrl,
            CreatedBy = createdBy
        };

        await repo.AddAsync(category);
        await repo.SaveChangesAsync();

        return category.Id;
    }

    public static async Task<bool> UpdateAsync(IRepository<Category> repo, int id, CategoryRequest req, string? updatedBy)
    {
        var category = await repo.FindAsync(id);
        if (category is null) return false;

        category.Title = req.Title;
        category.Subtitle = req.Subtitle;
        category.ImageUrl = req.ImageUrl;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }

    public static async Task<bool> DeleteAsync(IRepository<Category> repo, int id, string? updatedBy)
    {
        var category = await repo.FindAsync(id);
        if (category is null) return false;

        category.IsDeleted = true;
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }
}
