using FluentValidation.Results;

namespace App.API.Extensions
{
    public static class ValidationResultExtension
    {
        public static string[] GetMessages(this ValidationResult validationResult)
        {
            return validationResult.Errors.Select(x => x.ErrorMessage).ToArray();
        }
    }
}
