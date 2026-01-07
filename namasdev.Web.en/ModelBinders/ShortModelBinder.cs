using System;
using System.Globalization;
using System.Web.Mvc;

using namasdev.Core.Validation;

namespace namasdev.Web.ModelBinders
{
    public class ShortModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null
                || String.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return bindingContext.ModelType.IsGenericType
                    ? (short?)null
                    : default(short);
            }

            short valor;
            if (!short.TryParse(value.AttemptedValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.ShortInvalid(bindingContext.ModelName));
                return default(short);
            }

            return valor;
        }
    }
}