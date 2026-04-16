using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Tamkeen.Application.DTOs;

namespace Tamkeen.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianController : ControllerBase
    {
        private readonly string _excelPath = "Technicians.xlsx";

        [HttpPost("register")]
        public IActionResult Register([FromBody] TechnicianDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.FullName))
                return BadRequest("بيانات غير صحيحة");

            // EPPlus license (required for v5+)
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo file = new FileInfo(_excelPath);
            using var package = new ExcelPackage(file);

            // لو الفايل جديد، نعمل headers
            ExcelWorksheet sheet;
            if (package.Workbook.Worksheets.Count == 0)
            {
                sheet = package.Workbook.Worksheets.Add("Technicians");
                sheet.Cells[1, 1].Value = "الاسم الكامل";
                sheet.Cells[1, 2].Value = "رقم التليفون";
                sheet.Cells[1, 3].Value = "التخصص";
                sheet.Cells[1, 4].Value = "سنوات الخبرة";
                sheet.Cells[1, 5].Value = "نبذة عنك";
                sheet.Cells[1, 6].Value = "تاريخ التسجيل";
            }
            else
            {
                sheet = package.Workbook.Worksheets[0];
            }

            // آخر row موجود + 1
            int newRow = sheet.Dimension?.Rows + 1 ?? 2;

            sheet.Cells[newRow, 1].Value = dto.FullName;
            sheet.Cells[newRow, 2].Value = dto.PhoneNumber;
            sheet.Cells[newRow, 3].Value = dto.Specialization;
            sheet.Cells[newRow, 4].Value = dto.YearsOfExperience;
            sheet.Cells[newRow, 5].Value = dto.About;
            sheet.Cells[newRow, 6].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            package.Save();

            return Ok(new { message = "تم التسجيل بنجاح ✅" });
        }
    }
}
