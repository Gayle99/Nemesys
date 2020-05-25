using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class CreateReportViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A title is required")]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; }

        [Required(ErrorMessage = "A location is required")]
        [StringLength(50, MinimumLength = 1)]
        public string Location { get; set; }

        public DateTime DateSpotted { get; set; }
        public string TypeOfHazard { get; set; }

        [Required(ErrorMessage = "Report description is required")]
        [StringLength(1500, MinimumLength = 1, ErrorMessage = "Report description cannot be longer than 1500 characters")]
        public string Description { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        [MaxFileSize(6 * 1024 *1024)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg"})]
        public IFormFile ImageToUpload { get; set; }

    }

    public class AllowedExtensionsAttribute:ValidationAttribute
    {
        private readonly string[] _extensions;
        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            var extension = Path.GetExtension(file.FileName);
            if (file != null)
            {
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }
        public string GetErrorMessage()
        {
            return $"This photo extension is not allowed!";
        }
    }

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            return $"Maximum allowed file size is { _maxFileSize} bytes.";
        }
    }

}
