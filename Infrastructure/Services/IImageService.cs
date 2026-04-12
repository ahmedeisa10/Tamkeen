using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tamkeen.Infrastructure.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file, string folder);
        void DeleteImage(string url);
    }
}
