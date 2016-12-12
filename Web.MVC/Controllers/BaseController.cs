using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using Elmah;
using HealthCatalyst.DataLayer;
using HealthCatalyst.LogicLayer;
using HealthCatalyst.Models;

namespace HealthCatalyst.Controllers
{
    /// <summary>
    /// Base Api Controller Implementation.
    /// </summary>
    public class BaseController : Controller
    {
        private const string ErrorMessage = "Unhandled exception occured in the system.";
        protected static readonly LightInject.ServiceContainer DependencyContainer;

        /// <summary>
        /// Initialize controller common settings. Only called once.
        /// </summary>
        static BaseController()
        {
            // Inject dependencies here.
            DependencyContainer = new LightInject.ServiceContainer();
            DependencyContainer.Register<IPeopleLogic, PeopleLogic>(new LightInject.PerRequestLifeTime());

            // Set mapping settings
            Mapper.Initialize(config =>
            {
                config.CreateMissingTypeMaps = true;
                config.CreateMap<Person, PersonModel>();
                config.CreateMap<PersonModel, Person>();
                config.CreateMap<PersonInterest, PersonInterestModel>();
                config.CreateMap<PersonInterestModel, PersonInterest>();
            });
        }

        /// <summary>
        /// Handles controller un-expected exceptions.
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as DbEntityValidationException;
            if (exception == null)
                ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(filterContext.Exception));
            else
            {
                var sb = new StringBuilder();
                foreach (var error in exception.EntityValidationErrors)
                {
                    if (!error.IsValid)
                    {
                        foreach (var dbValidationError in error.ValidationErrors)
                        {
                            sb.AppendLine($"Property Name: {dbValidationError.PropertyName}");
                            sb.AppendLine($"Error Message: {dbValidationError.ErrorMessage}");
                        }
                    }
                }
                var customerException = new Exception(sb.ToString(), filterContext.Exception);
                ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(customerException));
            }

            filterContext.ExceptionHandled = true;
            ActionResult actionResult;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
                actionResult = HandleAjaxRequests();
            else if (ControllerContext.HttpContext.Request.HttpMethod.ToUpper() == "POST")
                actionResult = HandlePostRequests(filterContext);
            else actionResult = HandleGetRequests(filterContext);

            filterContext.Result = actionResult;
        }

        private static ActionResult HandleAjaxRequests()
        {
            var validationResult = new LogicValidationResult
            {
                ErrorMessage = ErrorMessage
            };
            return JsonStringResult.Build(validationResult.AjaxResult);
        }

        private ActionResult HandlePostRequests(ExceptionContext filterContext)
        {
            ((Controller) filterContext.Controller).ModelState.AddModelError("", ErrorMessage);

            return new RedirectResult(ControllerContext.HttpContext.Request.FilePath);
        }

        private static ActionResult HandleGetRequests(ExceptionContext filterContext)
        {
            var model = new HandleErrorInfo(filterContext.Exception,
                filterContext.RouteData.Values["action"].ToString(),
                filterContext.RouteData.Values["controller"].ToString());
            var controller = new HomeController
            {
                ViewData = new ViewDataDictionary(model)
            };
            return controller.View("~/Views/Shared/Error.cshtml");
        }
    }
}