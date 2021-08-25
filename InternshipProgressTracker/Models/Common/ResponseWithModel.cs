namespace InternshipProgressTracker.Models.Common
{
    /// <summary>
    /// Represents response with entity model
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ResponseWithModel<TModel>
    {
        public bool Success { get; set; }

        public TModel Model { get; set; }
    }
}
