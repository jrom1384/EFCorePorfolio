using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace EFCore.Utilities.MVC
{
    public class StringTrimmingModelBinder : IModelBinder
    {
        private readonly IModelBinder FallbackBinder;

        public StringTrimmingModelBinder(IModelBinder fallbackBinder)
        {
            FallbackBinder = fallbackBinder ?? throw new ArgumentNullException(nameof(fallbackBinder));
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName;
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult != null 
                && valueProviderResult.FirstValue is string stringValue 
                && !string.IsNullOrEmpty(stringValue))
            {
                bindingContext.Result = ModelBindingResult.Success(stringValue.Trim());
                return Task.CompletedTask;
            }

            return FallbackBinder.BindModelAsync(bindingContext);
        }
    }
}
