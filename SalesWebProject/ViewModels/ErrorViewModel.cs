namespace SalesWebProject.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string Message { get; set; }

        public bool ShowRequestId { get { return !string.IsNullOrEmpty(RequestId); } }
    }
}