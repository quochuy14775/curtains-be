using curtains_be.Common;
using curtains_be.DTOs.Requests;
using curtains_be.DTOs.Responses;
using curtains_be.Extensions;
using curtains_be.Models;
using curtains_be.Repository;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace curtains_be.Services;

public static class ProductService
{
    public static async Task<ODataResponse<ProductResponse>> GetAllAsync(
        IRepository<Product> repo, ODataQueryOptions<Product> queryOptions)
    {
        var query = repo.Query()
            .AsNoTracking()
            .Where(p => p.IsActived)
            .Include(p => p.Category);

        var (count, items) = await query.AppendQueryOptionsAsync(queryOptions);

        return new ODataResponse<ProductResponse>
        {
            Count = count,
            Value = items.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Material = p.Material,
                Price = p.Price,
                Tag = p.Tag,
                ColorHex = p.ColorHex,
                ColorGroup = p.ColorGroup,
                ImageFront  = p.ImageFront,
                ImageLeft   = p.ImageLeft,
                ImageRight  = p.ImageRight,
                ImageDetail = p.ImageDetail,
                CategoryId = p.CategoryId,
                CategoryTitle = p.Category?.Title,
                IsActived = p.IsActived
            })
        };
    }

    public static async Task<ODataResponse<ProductResponse>> GetAllAdminAsync(
        IRepository<Product> repo, ODataQueryOptions<Product> queryOptions)
    {
        var query = repo.Query()
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Include(p => p.Category);

        var (count, items) = await query.AppendQueryOptionsAsync(queryOptions);

        return new ODataResponse<ProductResponse>
        {
            Count = count,
            Value = items.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Material = p.Material,
                Price = p.Price,
                Tag = p.Tag,
                ColorHex = p.ColorHex,
                ColorGroup = p.ColorGroup,
                ImageFront  = p.ImageFront,
                ImageLeft   = p.ImageLeft,
                ImageRight  = p.ImageRight,
                ImageDetail = p.ImageDetail,
                CategoryId = p.CategoryId,
                CategoryTitle = p.Category?.Title,
                IsActived = p.IsActived
            })
        };
    }

    public static async Task<ProductResponse?> GetByIdAsync(IRepository<Product> repo, int id)
    {
        return await repo.Query()
            .AsNoTracking()
            .Where(p => p.IsActived)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Material = p.Material,
                Price = p.Price,
                Tag = p.Tag,
                ColorHex = p.ColorHex,
                ColorGroup = p.ColorGroup,
                ImageFront  = p.ImageFront,
                ImageLeft   = p.ImageLeft,
                ImageRight  = p.ImageRight,
                ImageDetail = p.ImageDetail,
                CategoryId = p.CategoryId,
                CategoryTitle = p.Category!.Title,
                IsActived = p.IsActived
            })
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public static async Task<int> CreateAsync(IRepository<Product> repo, ProductRequest req, string? createdBy)
    {
        var product = new Product
        {
            Name = req.Name,
            Material = req.Material,
            Price = req.Price,
            Tag = req.Tag,
            ColorHex = req.ColorHex,
            ColorGroup = req.ColorGroup,
            ImageFront  = req.ImageFront,
            ImageLeft   = req.ImageLeft,
            ImageRight  = req.ImageRight,
            ImageDetail = req.ImageDetail,
            CategoryId  = req.CategoryId,
            CreatedBy = createdBy
        };

        await repo.AddAsync(product);
        await repo.SaveChangesAsync();

        return product.Id;
    }

    public static async Task<bool> UpdateAsync(IRepository<Product> repo, int id, ProductRequest req, string? updatedBy)
    {
        var product = await repo.FindAsync(id);
        if (product is null) return false;

        product.Name = req.Name;
        product.Material = req.Material;
        product.Price = req.Price;
        product.Tag = req.Tag;
        product.ColorHex = req.ColorHex;
        product.ColorGroup = req.ColorGroup;
        product.ImageFront  = req.ImageFront;
        product.ImageLeft   = req.ImageLeft;
        product.ImageRight  = req.ImageRight;
        product.ImageDetail = req.ImageDetail;
        product.CategoryId = req.CategoryId;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }

    public static async Task<bool?> ToggleActiveAsync(IRepository<Product> repo, int id, string? updatedBy)
    {
        var product = await repo.FindAsync(id);
        if (product is null) return null;

        product.IsActived = !product.IsActived;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return product.IsActived;
    }

    public static async Task<bool> DeleteAsync(IRepository<Product> repo, int id, string? updatedBy)
    {
        var product = await repo.FindAsync(id);
        if (product is null) return false;

        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        product.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }
}
