using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tamkeen.Application.DTOs
{
    // الـ Vendor بيملا الفورم
    public class VendorRegisterDto
    {
        public string Token { get; set; }   // جاي من الـ URL
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }   // جديد
        public IFormFile ImageUrl { get; set; }
    }
}
