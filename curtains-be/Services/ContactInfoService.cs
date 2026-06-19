using curtains_be.DTOs.Requests;
using curtains_be.DTOs.Responses;
using curtains_be.Models;
using curtains_be.Repository;
using Microsoft.EntityFrameworkCore;

namespace curtains_be.Services;

public static class ContactInfoService
{
    private const int SingletonId = 1;

    public static async Task<ContactInfoResponse> GetAsync(IRepository<ContactInfo> repo)
    {
        var entity = await repo.Query().AsNoTracking().FirstOrDefaultAsync();

        return new ContactInfoResponse
        {
            CompanyName = entity?.CompanyName,
            Phone = entity?.Phone,
            Email = entity?.Email,
            AddressLine1 = entity?.AddressLine1,
            AddressLine2 = entity?.AddressLine2,
            ZaloUrl = entity?.ZaloUrl,
            WhatsappUrl = entity?.WhatsappUrl,
            FacebookUrl = entity?.FacebookUrl,
            UpdatedAt = entity?.UpdatedAt
        };
    }

    public static async Task UpdateAsync(IRepository<ContactInfo> repo, ContactInfoRequest req, string? updatedBy)
    {
        var entity = await repo.FindAsync(SingletonId);
        var isNew = entity is null;

        entity ??= new ContactInfo { Id = SingletonId };

        entity.CompanyName = req.CompanyName;
        entity.Phone = req.Phone;
        entity.Email = req.Email;
        entity.AddressLine1 = req.AddressLine1;
        entity.AddressLine2 = req.AddressLine2;
        entity.ZaloUrl = req.ZaloUrl;
        entity.WhatsappUrl = req.WhatsappUrl;
        entity.FacebookUrl = req.FacebookUrl;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = updatedBy;

        if (isNew)
            await repo.AddAsync(entity);

        await repo.SaveChangesAsync();
    }
}
