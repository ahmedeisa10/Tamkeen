using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.Application.DTOs.Feedback;

namespace Tamkeen.Application.Interfaces.Feedback
{
    public interface IFeedbackService
    {
        Task<FeedbackDto> CreateAsync(CreateFeedbackDto dto, string tenantId);
        Task<IEnumerable<FeedbackDto>> GetByTicketAsync(Guid ticketId);
    }
}
