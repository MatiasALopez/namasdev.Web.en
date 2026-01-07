using System;
using System.Globalization;
using System.Web.Mvc;

using namasdev.Core.Validation;

namespace namasdev.Web.ModelBinders
{
    public class DoubleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null
                || String.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return bindingContext.ModelType.IsGenericType
                    ? (double?)null
                    : default(double);
            }

            double valor;
            if (!double.TryParse(value.AttemptedValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.NumberInvalid(bindingContext.ModelName));
                return default(double);
            }

            return valor;
        }
    }
}