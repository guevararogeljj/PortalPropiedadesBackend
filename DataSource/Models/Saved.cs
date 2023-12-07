namespace DataSource.Models
{
    public class Saved
    {
        public Saved(bool v)
        {
            this.Success = v;
        }

        public Saved(bool v, string message) : this(v)
        {
            this.Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
