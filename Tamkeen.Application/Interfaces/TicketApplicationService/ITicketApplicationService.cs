using Tamkeen.Application.DTOs.TicketApplication;

namespace Tamkeen.Application.Interfaces
{
    public interface ITicketApplicationService
    {
        Task<IEnumerable<TicketApplicationDto>> GetApplicationsAsync(Guid ticketId, string tenantId);
    }
}