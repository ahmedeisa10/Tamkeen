using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.Application.DTOs.Ticket_DTOs;

namespace Tamkeen.Application.Interfaces.Ticket_Interface
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponseDto>> GetPendingAsync(string? governorate = null, string? city = null);
        Task ApplyAsync(Guid ticketId, string vendorId);
        Task AcceptApplicationAsync(Guid applicationId, string tenantId);
        Task<TicketResponseDto> CreateAsync(CreateTicketDto dto, string tenantId);
        Task<TicketResponseDto> GetByIdAsync(Guid id, string userId, string role);
        Task<IEnumerable<TicketResponseDto>> GetAllAsync(string userId, string role, string? governorate = null, string? city = null);
        //Task AssignVendorAsync(Guid id, AssignTicketDto dto);         // Manager
        //Task AcceptAsync(Guid id, string vendorId);                   // Vendor قبل
        //Task RejectAsync(Guid id, string vendorId);                   // Vendor رفض
        Task<List<ImageResponseDto>> CompleteWithImagesAsync(Guid ticketId, CompleteTicketDto dto, string vendorId);
        Task CloseAsync(Guid id, string tenantId);                    // Tenant تمام

        //Images


    }
}
