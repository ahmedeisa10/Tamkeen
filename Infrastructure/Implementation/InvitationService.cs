using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tamkeen.Application.DTOs;
using Tamkeen.Application.Interfaces;
using Tamkeen.Domain.Entities;
using Tamkeen.Domain.Enums;
using Tamkeen.Infrastructure.Data;

namespace Tamkeen.Infrastructure.Implementation
{
    public class InvitationService : IInvitationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;

        public InvitationService(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            AppDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }

        // ===== المانجر بيعمل Invitation =====
        public async Task<string> CreateInvitationAsync(string phone)
        {
            var token = Guid.NewGuid().ToString("N"); // مثلا: a1b2c3d4...

            var invitation = new VendorInvitation
            {
                Phone = phone,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(3) // اللينك صالح 3 أيام
            };

            _context.vendorInvitations.Add(invitation);
            await _context.SaveChangesAsync();

            // اللينك اللي هيتبعت على الواتس
            var link = $"https://yoursite.com/vendor-register?token={token}";
            return link;
        }

        // ===== الـ Vendor بيسجل عن طريق اللينك =====
        public async Task<(bool Success, string Message)> RegisterVendorAsync(VendorRegisterDto dto)
        {
            // 1. ابحث عن الـ Token
            var invitation = await _context.vendorInvitations
                .FirstOrDefaultAsync(x => x.Token == dto.Token);

            if (invitation == null)
                return (false, "Invalid invitation link.");

            if (invitation.IsUsed)
                return (false, "This invitation has already been used.");

            if (invitation.ExpiresAt < DateTime.UtcNow)
                return (false, "Invitation link has expired.");

            // 2. تحقق الـ Email مش موجود
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "Email already exists.");

            // 3. انشئ الـ User
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                EmailConfirmed = true  // مش محتاج confirm لأن المانجر اتواصل معاه
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            // 4. ضيفه كـ Vendor
            await _userManager.AddToRoleAsync(user, UserRole.Vendor.ToString());

            // 5. خلي التوكن مستخدم
            invitation.IsUsed = true;
            await _context.SaveChangesAsync();

            return (true, "Vendor registered successfully.");
        }
    }
}
