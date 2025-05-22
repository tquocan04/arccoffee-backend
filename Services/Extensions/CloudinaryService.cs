using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Entities;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace Services.Extensions
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private static readonly Random _random = new();
        private static readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        private static string GenerateRandomString(int length = 5)
        {
            return new string(Enumerable.Repeat(_chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }

        public async Task<string> UploadImageProductAsync(Product product, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            if (file.Length > 5 * 1024 * 1024) // Giới hạn 5MB
            {
                throw new ArgumentException("File size exceeds the maximum limit of 5MB.");
            }

            try
            {
                var random = GenerateRandomString();

                // Tách tên file và phần mở rộng
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);

                // Loại bỏ mọi mở rộng kép (nếu có)
                while (Path.GetExtension(fileNameWithoutExtension) != "")
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
                }

                // Tạo upload parameters với Stream từ IFormFile
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = $"product_{fileNameWithoutExtension}_{random}",
                    Folder = "arc"
                };

                // Upload lên Cloudinary
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Kiểm tra trạng thái upload
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException($"Failed to upload image: {uploadResult.Error?.Message}");
                }

                // Lấy URL của ảnh
                var imageUrl = uploadResult.SecureUrl?.AbsoluteUri;
                if (string.IsNullOrEmpty(imageUrl))
                {
                    throw new InvalidOperationException("Image URL is empty.");
                }

                return imageUrl;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                throw new InvalidOperationException($"Error uploading image to Cloudinary: {ex.Message}", ex);
            }
        }
        
        public async Task<string> UploadImageCustomerAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
            }

            if (file.Length > 5 * 1024 * 1024) // Giới hạn 5MB
            {
                throw new ArgumentException("File size exceeds the maximum limit of 5MB.");
            }

            try
            {
                var random = GenerateRandomString();

                // Tách tên file và phần mở rộng
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExtension = Path.GetExtension(file.FileName);

                // Loại bỏ mọi mở rộng kép (nếu có)
                while (Path.GetExtension(fileNameWithoutExtension) != "")
                {
                    fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
                }

                // Tạo upload parameters với Stream từ IFormFile
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    PublicId = $"user_{fileNameWithoutExtension}_{random}",
                    Folder = "arc"
                };

                // Upload lên Cloudinary
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Kiểm tra trạng thái upload
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new InvalidOperationException($"Failed to upload image: {uploadResult.Error?.Message}");
                }

                // Lấy URL của ảnh
                var imageUrl = uploadResult.SecureUrl?.AbsoluteUri;
                if (string.IsNullOrEmpty(imageUrl))
                {
                    throw new InvalidOperationException("Image URL is empty.");
                }

                return imageUrl;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                throw new InvalidOperationException($"Error uploading image to Cloudinary: {ex.Message}", ex);
            }
        }
    }
}
