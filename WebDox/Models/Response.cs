namespace WebDox.Models
{
    public class Response
    {
        public Response()
        {

        }
        public Response(bool success)
        {
            this.Success = success;
        }

        public Response(bool success, string message) : this(success)
        {
            this.Message = message;
        }

        public Response(bool success, string message, object result) : this(success, message)
        {
            this.Result = result;
        }
        public bool Success { get; set; }
        public object Result { get; set; }
        public string Message { get; set; }
    }
}
