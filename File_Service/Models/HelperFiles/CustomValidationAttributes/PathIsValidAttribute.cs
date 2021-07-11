using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
