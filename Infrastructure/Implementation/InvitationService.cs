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

        public async Task<(bool Success, string Message, string? Token, string? ProfileImageUrl)> RegisterVendorAsync(VendorRegisterDto dto)
        {
            // ✅ 1. تحقق من اللينك
            var invitation = await _context.vendorInvitations
                .FirstOrDefaultAsync(x => x.Token == dto.Token);

            if (invitation == null)
                return (false, "Invalid invitation link.", null, null);

            if (invitation.IsUsed)
                return (false, "This invitation has already been used.", null, null);

            if (invitation.ExpiresAt < DateTime.UtcNow)
                return (false, "Invitation link has expired.", null, null);

            // ✅ 2. تحقق من الإيميل
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return (false, "Email already exists.", null, null);

            // ✅ 3. Validation للصورة
            string? imagePath = null;

            if (dto.ImageUrl != null && dto.ImageUrl.Length > 0)
            {
                if (dto.ImageUrl.Length > 2 * 1024 * 1024)
                    return (false, "Image size must be less than 2MB.", null, null);

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(dto.ImageUrl.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return (false, "Only JPG and PNG images are allowed.", null, null);

                if (!dto.ImageUrl.ContentType.StartsWith("image/"))
                    return (false, "Invalid image file.", null, null);

                try
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/vendors");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var fileName = Guid.NewGuid().ToString() + extension;
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await dto.ImageUrl.CopyToAsync(stream);
                    }

                    imagePath = "images/vendors/" + fileName; // ✅ بدون / في الأول عشان الفرونت يضيفها
                }
                catch
                {
                    return (false, "Error while uploading image.", null, null);
                }
            }

            // ✅ 4. إنشاء المستخدم
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                ImageUrl = imagePath,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)), null, null);

            // ✅ 5. إضافة رول
            await _userManager.AddToRoleAsync(user, UserRole.Vendor.ToString());

            // ✅ 6. تحديث الدعوة
            invitation.IsUsed = true;
            await _context.SaveChangesAsync();

            // ✅ 7. إنشاء التوكن
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user.Id, user.Email!, roles);

            return (true, "Vendor registered successfully.", token, imagePath);
        }
    }
}
