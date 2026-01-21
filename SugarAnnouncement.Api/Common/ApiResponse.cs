namespace SugarAnnouncement.Api.Common{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMsg { get; set; } = string.Empty;

        public static ApiResponse<T> Success(T data, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Data = data,
                StatusCode = statusCode,
                ErrorMsg = string.Empty
            };
        }

        public static ApiResponse<T> Failure(string errorMsg, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                Data = default!,
                StatusCode = statusCode,
                ErrorMsg = errorMsg
            };
        }
    }
}
