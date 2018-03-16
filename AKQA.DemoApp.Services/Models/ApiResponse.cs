namespace AKQA.DomoApp.Services.Models
{
    public class ApiResponse<T>
        where T : class
    {
        public bool IsSuccess { get; set; }

        public ApiResponseError Error { get; set; }

        public T Result { get; set; }
    }
}
