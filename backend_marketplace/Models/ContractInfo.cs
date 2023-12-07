using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class ContractInfo : BaseModel
    {
        private string email;

        [Required]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$")]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email
        {
            get { return email; }
            set { email = value.Trim(); }
        }


        private int occupation;

        [Required]
        //[Range(0, int.MaxValue)]
        [JsonPropertyName("ocuppation")]
        public int Occupation
        {
            get { return occupation; }
            set { occupation = value; }
        }

        private string address;

        [DataType(DataType.Text)]
        [JsonPropertyName("Address")]
        public string Address
        {
            get { return address; }
            set { address = value.Trim(); }
        }

        private string rfc;

        [Required]
        [DataType(DataType.Text)]
        [JsonPropertyName("rfc")]
        public string Rfc
        {
            get { return rfc; }
            set { rfc = value.Trim(); }
        }

        private int maritalstatus;

        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("maritalstatus")]
        public int MaritalStatus
        {
            get { return maritalstatus; }
            set { maritalstatus = value; }
        }

        private string rfcdoc;
        [JsonPropertyName("rfcdoc")]
        public string RfcDoc
        {
            get { return rfcdoc; }
            set { rfcdoc = value.Trim(); }
        }


        private string typefiledoc;

        [JsonPropertyName("typefiledoc")]
        public string TypeFileDoc
        {
            get { return typefiledoc; }
            set { typefiledoc = value.Trim(); }
        }


        private string hometown;

        [Required]
        [DataType(DataType.Text)]
        [JsonPropertyName("hometown")]
        public string Hometown
        {
            get { return hometown; }
            set { hometown = value.Trim(); }
        }

        private bool rfctype;

        [Required]
        [JsonPropertyName("rfctype")]
        public bool RfcType
        {
            get { return rfctype; }
            set { rfctype = value; }
        }

        private string cellphone;

        [Required]
        [Phone]
        [RegularExpression(@"\d{10}$")]
        [JsonPropertyName("cellphone")]
        public string Cellphone
        {
            get { return cellphone; }
            set { cellphone = value; }
        }


    }
}
