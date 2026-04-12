using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tamkeen.Domain.Enums;

namespace Tamkeen.Application.DTOs.Ticket_DTOs
{
    public class UploadTicketImagesDto
    {
        public List<IFormFile> Images { get; set; }
        public ImageType Type { get; set; } // Before or After
    }
}
