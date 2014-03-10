// -----------------------------------------------------------------------
// <copyright file="DecimalModelBinder.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Mvc
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            return valueProviderResult == null ? base.BindModel(controllerContext, bindingContext) : Convert.ToDecimal(valueProviderResult.AttemptedValue);
        }
    }
}
