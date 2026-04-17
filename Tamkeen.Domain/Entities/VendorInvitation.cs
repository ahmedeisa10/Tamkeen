using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tamkeen.Domain.Entities
{
    public class VendorInvitation
    {
        public int Id { get; set; }
        public string Phone { get; set; }        // بتاع الـ Vendor
        public string Token { get; set; }        // GUID عشوائي
        public DateTime ExpiresAt { get; set; }  // صلاحية اللينك
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
