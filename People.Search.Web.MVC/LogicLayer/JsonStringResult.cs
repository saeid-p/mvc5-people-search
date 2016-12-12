using System;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HealthCatalyst.LogicLayer
{
    public class JsonStringResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the data that needs to be encapsulated.
        /// </summary>
        public object Data { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether HTTP GET requests from the client are allowed.
        /// </summary>
        /// <returns>A value that indicates whether HTTP GET requests from the client are allowed.</returns>
        private readonly JsonRequestBehavior _requestBehavior;

        protected JsonStringResult(object data)
        {
            Data = data;
            _requestBehavior = JsonRequestBehavior.AllowGet;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="T:HealthCatalyst.LogicLayer.JsonStringResult" /> class.
        /// </summary>
        public static JsonStringResult Build(object data) => new JsonStringResult(data);

        /// <summary>
        /// Enables processing the result of an action method by a custom type 
        /// that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context within which the result is executed.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            // If the request behavior set to deny get requests, the action should raise an exception.
            if (_requestBehavior == JsonRequestBehavior.DenyGet 
                && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("JSON Get Result Not Allowed");

            // The client retrieves null only if the get request is allowed.
            if (Data == null) return;

            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            response.Write(JsonConvert.SerializeObject(Data));
        }
    }
}