using System;
using System.Globalization;
using System.Web.Mvc;

using namasdev.Core.Validation;

namespace namasdev.Web.ModelBinders
{
    public class LongModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null
                || String.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return bindingContext.ModelType.IsGenericType
                    ? (long?)null
                    : default(long);
            }

            long valor;
            if (!long.TryParse(value.AttemptedValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.LongInvalid(bindingContext.ModelName));
                return default(long);
            }

            return valor;
        }
    }
}