using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tamkeen.Application.DTOs;
using Tamkeen.Application.Interfaces;

namespace Tamkeen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _service;

        public VendorsController(IVendorService service)
        {
            _service = service;
        }

        [HttpPost("profile")]
        public async Task<IActionResult> CreateProfile(CreateVendorProfileDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;

            await _service.CreateProfileAsync(dto);

            return Ok(new { message = "Profile created successfully" });
        }
    }
}
