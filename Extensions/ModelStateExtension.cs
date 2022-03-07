using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Livraria.Extensions
{
    public static class ModelStateExtension
    {
        public static List<string> GetErrors(this ModelStateDictionary state)
        {
            var errors = new List<string>();

            foreach (var value in state.Values)
                errors.AddRange(value.Errors.Select(x => x.ErrorMessage));

            return errors;
        }
    }
}
