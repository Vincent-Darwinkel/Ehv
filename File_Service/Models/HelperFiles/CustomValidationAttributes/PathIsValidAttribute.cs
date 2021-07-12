using System.ComponentModel.DataAnnotations;

namespace File_Service.Models.HelperFiles.CustomValidationAttributes
{
    public class PathIsValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return ValidFilePaths.FilePathIsValid((string)value);
        }
    }
}
