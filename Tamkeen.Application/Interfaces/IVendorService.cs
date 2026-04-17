using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.Application.DTOs;
using Tamkeen.Application.DTOs.Vendor;

namespace Tamkeen.Application.Interfaces
{
    public interface IVendorService
    {
        Task CreateProfileAsync(CreateVendorProfileDto dto);
        Task<IEnumerable<VendorResponseDto>> GetAllVendorsAsync();
    }
}
