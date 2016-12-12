namespace HealthCatalyst.LogicLayer
{
    /// <summary>
    /// Encapsulates required fields to validate a model and handle client validation.
    /// </summary>
    public class LogicValidationResult
    {
        /// <summary>
        /// Indicates whether model has an error or not.
        /// </summary>
        public bool HasError { get; private set; }

        /// <summary>
        /// If the model is a new entity, the new generated Id in string format will be stored here.
        /// </summary>
        public string NewId { get; set; }

        private string _errorMessage;
        /// <summary>
        /// The error message to display to the client.
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                HasError = true;
            }
        }

        /// <summary>
        /// Dynamic model to be used for Ajax results.
        /// </summary>
        public dynamic AjaxResult => new
        {
            HasError,
            ErrorMessage,
            NewId
        };
    }
}