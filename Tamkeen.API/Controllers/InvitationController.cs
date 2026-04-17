using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tamkeen.Application.DTOs;
using Tamkeen.Application.Interfaces;
using Tamkeen.Infrastructure.Data;

namespace Tamkeen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly AppDbContext context;

        public InvitationController(IInvitationService invitationService,AppDbContext context)
        {
            _invitationService = invitationService;
            this.context = context;
        }

        // ===== Manager Only =====
        [HttpPost("create")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateInvitation([FromBody] CreateInvitationDto dto)
        {
            var link = await _invitationService.CreateInvitationAsync(dto.Phone);
            return Ok(new { link });
            // المانجر هياخد اللينك ده ويبعته على الواتس
        }

        // ===== التحقق من التوكن قبل ما يفتح الفورم =====
        [HttpGet("validate/{token}")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            var invitation = await context.vendorInvitations
                .FirstOrDefaultAsync(x => x.Token == token && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow);

            if (invitation == null)
                return BadRequest(new { message = "Invalid or expired link." });

            return Ok(new { valid = true });
        }

        // ===== الـ Vendor يسجل =====
        [HttpPost("register-vendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] VendorRegisterDto dto)
        {
            var result = await _invitationService.RegisterVendorAsync(dto);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }
    }
}
