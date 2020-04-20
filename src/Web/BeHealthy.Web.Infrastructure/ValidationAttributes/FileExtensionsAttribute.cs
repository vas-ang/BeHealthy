namespace BeHealthy.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileExtensionsAttribute : ValidationAttribute
    {
        private readonly string errorMessage;
        private readonly string[] allowedExtensions;

        public FileExtensionsAttribute(string[] allowedExtensions, string errorMessage = "{0} cannot be of type {1}. Allowed extensions {2}.")
        {
            this.errorMessage = errorMessage;
            this.allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (this.allowedExtensions.All(x => x != extension))
                {
                    return new ValidationResult(string.Format(this.errorMessage, validationContext.DisplayName, extension, string.Join(", ", this.allowedExtensions)));
                }
            }

            return ValidationResult.Success;
        }
    }
}
