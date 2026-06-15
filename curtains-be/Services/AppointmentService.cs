using curtains_be.Common;
using curtains_be.DTOs.Requests;
using curtains_be.DTOs.Responses;
using curtains_be.Enum;
using curtains_be.Extensions;
using curtains_be.Models;
using curtains_be.Repository;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace curtains_be.Services;

public static class AppointmentService
{
    public static async Task<ODataResponse<AppointmentResponse>> GetAllAsync(
        IRepository<Appointment> repo, ODataQueryOptions<Appointment> queryOptions)
    {
        var query = repo.Query()
            .AsNoTracking()
            .Include(a => a.Customer);

        var (count, items) = await query.AppendQueryOptionsAsync(queryOptions);

        return new ODataResponse<AppointmentResponse>
        {
            Count = count,
            Value = items.Select(a => new AppointmentResponse
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer?.Name,
                Phone = a.Customer?.Phone,
                Address = a.Customer?.Address,
                Note = a.Note,
                StaffNote = a.StaffNote,
                Status = a.Status,
                ScheduledAt = a.ScheduledAt,
                CreatedAt = a.CreatedAt
            })
        };
    }

    public static async Task<AppointmentResponse?> GetByIdAsync(IRepository<Appointment> repo, int id)
    {
        return await repo.Query()
            .AsNoTracking()
            .Include(a => a.Customer)
            .Select(a => new AppointmentResponse
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                CustomerName = a.Customer != null ? a.Customer.Name : null,
                Phone = a.Customer != null ? a.Customer.Phone : null,
                Address = a.Customer != null ? a.Customer.Address : null,
                Note = a.Note,
                StaffNote = a.StaffNote,
                Status = a.Status,
                ScheduledAt = a.ScheduledAt,
                CreatedAt = a.CreatedAt
            })
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public static async Task<int> CreateAsync(
        IRepository<Appointment> repo,
        IRepository<Customer> customerRepo,
        AppointmentRequest req,
        string? createdBy)
    {
        var phone = req.Phone.Trim();

        // Tìm customer theo SĐT khi điền form lần 2 — bỏ qua filter soft-delete
        // để không vướng unique index trên Phone
        var customer = await customerRepo.Query()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Phone == phone);

        if (customer is null)
        {
            customer = new Customer
            {
                Name = req.Name.Trim(),
                Phone = phone,
                Address = req.Address?.Trim(),
                CreatedBy = createdBy
            };
            await customerRepo.AddAsync(customer);
        }
        else
        {
            customer.Name = req.Name.Trim();
            if (!string.IsNullOrWhiteSpace(req.Address))
                customer.Address = req.Address.Trim();
            customer.IsDeleted = false;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = createdBy;
        }

        var appointment = new Appointment
        {
            Customer = customer,
            Note = req.Note?.Trim(),
            Status = AppointmentStatus.Pending,
            CreatedBy = createdBy
        };

        await repo.AddAsync(appointment);
        await repo.SaveChangesAsync();

        return appointment.Id;
    }

    public static async Task<bool> UpdateAsync(
        IRepository<Appointment> repo, int id, AppointmentUpdateRequest req, string? updatedBy)
    {
        var appointment = await repo.FindAsync(id);
        if (appointment is null) return false;

        appointment.Status = req.Status;
        appointment.StaffNote = req.StaffNote;
        appointment.ScheduledAt = AsUtc(req.ScheduledAt);
        appointment.UpdatedAt = DateTime.UtcNow;
        appointment.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }

    public static async Task<bool> UpdateStatusAsync(
        IRepository<Appointment> repo, int id, AppointmentStatus status, string? updatedBy)
    {
        var appointment = await repo.FindAsync(id);
        if (appointment is null) return false;

        appointment.Status = status;
        appointment.UpdatedAt = DateTime.UtcNow;
        appointment.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }

    public static async Task<AppointmentStatsResponse> GetStatsAsync(
        IRepository<Appointment> repo, IRepository<Customer> customerRepo)
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var query = repo.Query().AsNoTracking();

        var statusCounts = await query
            .GroupBy(a => a.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        int CountOf(AppointmentStatus status) =>
            statusCounts.FirstOrDefault(s => s.Status == status)?.Count ?? 0;

        var total = statusCounts.Sum(s => s.Count);
        var completed = CountOf(AppointmentStatus.Completed);

        var completedThisMonth = await query.CountAsync(a =>
            a.Status == AppointmentStatus.Completed && a.CreatedAt >= startOfMonth);

        var totalCustomers = await customerRepo.Query().AsNoTracking().CountAsync();

        var upcoming = await query
            .Include(a => a.Customer)
            .Where(a => a.ScheduledAt != null && a.ScheduledAt >= now
                     && a.Status != AppointmentStatus.Cancelled
                     && a.Status != AppointmentStatus.Completed)
            .OrderBy(a => a.ScheduledAt)
            .Take(5)
            .ToListAsync();

        var recent = await query
            .Include(a => a.Customer)
            .OrderByDescending(a => a.CreatedAt)
            .Take(5)
            .ToListAsync();

        return new AppointmentStatsResponse
        {
            Total = total,
            Pending = CountOf(AppointmentStatus.Pending),
            Confirmed = CountOf(AppointmentStatus.Confirmed),
            Completed = completed,
            Cancelled = CountOf(AppointmentStatus.Cancelled),
            CompletedThisMonth = completedThisMonth,
            CompletionRate = total == 0 ? 0 : (int)Math.Round(completed * 100.0 / total),
            TotalCustomers = totalCustomers,
            Upcoming = upcoming.Select(ToResponse),
            Recent = recent.Select(ToResponse)
        };
    }

    public static async Task<bool> DeleteAsync(IRepository<Appointment> repo, int id, string? updatedBy)
    {
        var appointment = await repo.FindAsync(id);
        if (appointment is null) return false;

        appointment.IsDeleted = true;
        appointment.UpdatedAt = DateTime.UtcNow;
        appointment.UpdatedBy = updatedBy;

        await repo.SaveChangesAsync();
        return true;
    }

    private static AppointmentResponse ToResponse(Appointment a) => new()
    {
        Id = a.Id,
        CustomerId = a.CustomerId,
        CustomerName = a.Customer?.Name,
        Phone = a.Customer?.Phone,
        Address = a.Customer?.Address,
        Note = a.Note,
        StaffNote = a.StaffNote,
        Status = a.Status,
        ScheduledAt = a.ScheduledAt,
        CreatedAt = a.CreatedAt
    };

    // Npgsql yêu cầu DateTime Kind=Utc cho cột timestamptz
    private static DateTime? AsUtc(DateTime? dt) =>
        dt is null ? null
        : dt.Value.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(dt.Value, DateTimeKind.Utc)
            : dt.Value.ToUniversalTime();
}
