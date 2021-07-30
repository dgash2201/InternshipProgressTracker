namespace InternshipProgressTracker.Models.Common
{
    public class ResponseWithModel<TModel>
    {
        public bool Success { get; set; }

        public TModel Model { get; set; }
    }
}
