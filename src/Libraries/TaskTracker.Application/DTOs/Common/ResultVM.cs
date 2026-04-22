namespace TaskTracker.Application.DTOs.Common
{
    public class ResultVM
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string ExMessage { get; set; }
        public string Id { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public string?[] IDs { get; set; }
        public object Data { get; set; }
        public object DataVM { get; set; }
        public HttpResponseMessage respone { get; set; }
        public string DetailId { get; set; }

        // Add this static factory method
        public static ResultVM Fail(string message, string exMessage = "")
        {
            return new ResultVM
            {
                Status = "Fail",
                Message = message,
                ExMessage = exMessage
            };
        }

        public static ResultVM Success(string message = "Success")
        {
            return new ResultVM
            {
                Status = "Success",
                Message = message
            };
        }
    }
}
