using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Linq;

namespace EFCore.Utilities.MVC
{
    public static class MVCOptionExtensions
    {
        public static void AddStringTrimmingProvider(this MvcOptions option)
        {
            var binderToFind = option.ModelBinderProviders
              .FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));

            if (binderToFind == null)
            {
                return;
            }

            var index = option.ModelBinderProviders.IndexOf(binderToFind);
            option.ModelBinderProviders.Insert(index, new StringTrimmingModelBinderProvider());
        }
    }
}
