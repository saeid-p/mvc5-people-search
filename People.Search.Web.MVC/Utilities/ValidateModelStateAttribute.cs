using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HealthCatalyst.LogicLayer;

namespace HealthCatalyst.Utilities
{
    /// <summary>
    /// Ensures that the model state of the current action is valid.
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private const string ErrorMessage = "The provided model cannot be validated.";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.ViewData.ModelState.IsValid)
                return;

            foreach (var validation in filterContext.Controller.ViewData.ModelState)
            {
                foreach (var error in validation.Value.Errors)
                {
                    Elmah.ErrorLog.GetDefault(HttpContext.Current).Log(new Elmah.Error(
                        new Exception($@"{ErrorMessage}
                            Field Name: {validation.Key}", error.Exception)));
                }
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var validationResult = new LogicValidationResult
                {
                    ErrorMessage = ErrorMessage
                };
                filterContext.Result = JsonStringResult.Build(validationResult.AjaxResult);
            }
            else
            {
                filterContext.Controller.ViewData.ModelState.
                    AddModelError("", ErrorMessage);

                var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var action = filterContext.ActionDescriptor.ActionName;

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new {controller, action}));
            }
        }
    }
}