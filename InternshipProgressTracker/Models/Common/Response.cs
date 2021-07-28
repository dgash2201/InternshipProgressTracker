namespace InternshipProgressTracker.Models.Common
{
    public class Response<TModel>
    {
        public bool Success { get; set; }

        public TModel Model { get; set; }
    }
}
