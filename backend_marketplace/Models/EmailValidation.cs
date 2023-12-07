namespace backend_marketplace.Models
{
    public class EmailValidation : BaseModel
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
