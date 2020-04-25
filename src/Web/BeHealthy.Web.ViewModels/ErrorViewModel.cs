namespace BeHealthy.Web.Dtos
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        public int StatusCode { get; set; }
    }
}
