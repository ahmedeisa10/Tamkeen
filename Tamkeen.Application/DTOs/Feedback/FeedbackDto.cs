using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs.Feedback
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public string TenantName { get; set; }
        public string TenantId { get; set; }
    }
}
