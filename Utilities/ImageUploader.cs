using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace EquipShare.Utilities
{
    public static class ImageUploader
    {
        public static string UploadImage(IFormFile imageFile, string webRootPath)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");

            // Validate file size (max 5MB)
            if (imageFile.Length > 5 * 1024 * 1024)
                throw new Exception("File size cannot exceed 5MB.");

            // Create upload directory if it doesn't exist
            var uploadsDir = Path.Combine(webRootPath, "images", "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsDir, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return $"/images/uploads/{fileName}";
        }

        public static bool DeleteImage(string imageUrl, string webRootPath)
        {
            if (string.IsNullOrEmpty(imageUrl) || !imageUrl.StartsWith("/images/uploads/"))
                return false;

            var filePath = Path.Combine(webRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}