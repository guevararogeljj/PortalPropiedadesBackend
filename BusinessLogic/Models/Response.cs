using System.Text.Json.Serialization;

namespace BusinessLogic.Models
{
    public class Response
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("result")]
        public object? Result { get; set; }
        [JsonPropertyName("success")]
        public bool? Success { get; set; }

        public Response()
        {

        }

        public Response(bool? status)
        {
            Success = status;
        }

        public Response(bool? status, string? message) : this(status)
        {
            Message = message;
        }

        public Response(bool? status, object? result) : this(status)
        {
            Result = result;
        }

        public Response(bool? status, string? message, object? result) : this(status, message)
        {
            Result = result;
        }
    }
}
