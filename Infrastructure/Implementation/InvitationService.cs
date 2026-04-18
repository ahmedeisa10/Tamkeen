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
            var token = Guid.NewGuid().ToString("N");

            var invitation = new VendorInvitation
            {
                Phone = phone,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(3)
            };

            _context.vendorInvitations.Add(invitation);
            await _context.SaveChangesAsync();

            // لينك التسجيل
            var registrationLink = $"http://localhost:4200/vendor-register?token={token}";

            // رسالة واتساب جميلة
            var message = $"🎉 مبارك انضمامك لينا!\n\n" +
                          $"أهلاً بيك في فريقنا 👋\n" +
                          $"يسعدنا وجودك معانا 💙\n\n" +
                          $"سجل من هنا 👇\n{registrationLink}";

            // encode الرسالة
            var encodedText = Uri.EscapeDataString(message);

            // لينك واتساب
            var whatsappLink = $"https://wa.me/{phone}?text={encodedText}";

            return whatsappLink;
        }

        // ===== الـ Vendor بيسجل عن طريق اللينك =====
        public async Task<(bool Success, string Message, string? Token)> RegisterVendorAsync(VendorRegisterDto dto)
        {
            var invitation = await _context.vendorInvitations
                .FirstOrDefaultAsync(x => x.Token == dto.Token);

            if (invitation == null)
                return (false, "Invalid invitation link.", null);

            if (invitation.IsUsed)
                return (false, "This invitation has already been used.", null);

            if (invitation.ExpiresAt < DateTime.UtcNow)
                return (false, "Invitation link has expired.", null);

            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "Email already exists.", null);

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)), null);

            await _userManager.AddToRoleAsync(user, UserRole.Vendor.ToString());

            invitation.IsUsed = true;
            await _context.SaveChangesAsync();

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

            return (true, "Vendor registered successfully.", token);
        }
    }
}
