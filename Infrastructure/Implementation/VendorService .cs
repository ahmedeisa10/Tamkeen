using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Tamkeen.Application.DTOs;
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

        public async Task CreateProfileAsync(Guid userId, CreateVendorProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("User not found");

            // تأكد إنه Vendor
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Vendor"))
                throw new Exception("User is not a vendor");

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
    }
}
