using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Application.DTOs
{
    // The manager will send the WhatsApp number only
    public class CreateInvitationDto
    {
        public string Phone { get; set; }
    }
}
