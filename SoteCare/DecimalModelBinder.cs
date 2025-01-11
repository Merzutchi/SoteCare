using System.Globalization;
using System.Web.Mvc;

namespace SoteCare
{
    internal class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null)
                return null;

            var value = valueProviderResult.AttemptedValue;
            if (string.IsNullOrWhiteSpace(value))
                return null;

            decimal decimalValue;
            // Yritetään parsia desimaaliluku, huomioidaan kulttuuri.
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out decimalValue))
            {
                return decimalValue;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid decimal value");
            return null;
        }
    }
}