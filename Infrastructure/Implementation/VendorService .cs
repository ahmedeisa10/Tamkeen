using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamkeen.Application.DTOs;
using Tamkeen.Application.DTOs.Vendor;
using Tamkeen.Application.Interfaces;
using Tamkeen.Domain.Entities;
using Tamkeen.Infrastructure.Data;

namespace Tamkeen.Infrastructure.Implementation
{
    public class VendorService : IVendorService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public VendorService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreateProfileAsync( CreateVendorProfileDto dto)
        {
           

            var profile = new VendorProfile
            {
                fullName = dto.FullName,
                phone = dto.PhoneNumber,
                specialty = dto.Specialization,
                yearsExperience = dto.YearsOfExperience,
                bio = dto.Bio
            };

            _context.vendorProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<VendorResponseDto>> GetAllVendorsAsync()
        {
            return await _context.vendorProfiles
                .OrderByDescending(v => v.CreatedAt)
                .Select(v => new VendorResponseDto
                {
                    fullName = v.fullName,
                    phone = v.phone,
                    specialty = v.specialty,
                    yearsExperience = v.yearsExperience,
                    bio = v.bio,
                    CreatedAt = v.CreatedAt
                })
                .ToListAsync();
        }

    }
}
