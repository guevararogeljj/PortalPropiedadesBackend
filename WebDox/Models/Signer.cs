namespace WebDox.Models
{
    public class Signer
    {
        public string kind { get; set; } = "external";
        public string user_id { get; set; } = null;
        public string country_code { get; set; } = "MEX";
        public string name { get; set; }
        public string email { get; set; }
        public string national_identification_number { get; set; }
        public string national_identification_kind_id { get; set; } = null;
    }
}
