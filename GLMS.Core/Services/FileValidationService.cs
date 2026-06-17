using Microsoft.AspNetCore.Http;

namespace GLMS.Core.Services
{
    public class FileValidationService
    {
        private readonly string[] permittedExtensions = { ".pdf" };

        public bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return permittedExtensions.Contains(extension);
        }
    }
}
