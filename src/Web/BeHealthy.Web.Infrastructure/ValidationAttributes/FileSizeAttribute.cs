namespace BeHealthy.Web.Infrastructure.ValidationAttributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly long fileSize;
        private readonly string errorMessage;

        public FileSizeAttribute(long fileSize, string errorMessage = "{0} must not be more than {1} bytes.")
        {
            this.fileSize = fileSize;
            this.errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > this.fileSize)
            {
                return new ValidationResult(string.Format(this.errorMessage, validationContext.DisplayName, this.fileSize));
            }

            return ValidationResult.Success;
        }
    }
}
