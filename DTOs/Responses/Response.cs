namespace DTOs.Responses
{
    public class Response<T> where T : class
    {
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
    }
}
