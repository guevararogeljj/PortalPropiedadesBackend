using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backend_marketplace.Models
{
    public class User : BaseModel
    {
        private string? name;
        private string? lastname;
        private string? lastname2;
        private string? password;
        private string? cellphone;

        [JsonPropertyName("email")]
        [Required]
        [EmailAddress]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]       
        public string? Email { get; set; }

        [JsonPropertyName("name")]
        [Required]
        [StringLength(50, ErrorMessage = "Nombre demasiado largo, 50 caracteres maximo")]
        [RegularExpression(@"^[a-zñA-ZÑ\s]+$", ErrorMessage = "El nombre ingresado no tiene un formato valido")]
        public string? Name { get => name.Trim(); set => name = value; }

        [JsonPropertyName("lastname")]
        [Required]
        [StringLength(50, ErrorMessage = "Apellido demasiado largo, 50 caracteres maximo")]
        [RegularExpression(@"[a-zñA-ZÑ]+$", ErrorMessage = "El nombre ingresado no tiene un formato valido")]
        public string? Lastname { get => lastname.Trim(); set => lastname = value; }

        [JsonPropertyName("lastname2")]
        [Required]
        [StringLength(50, ErrorMessage = "Apellido demasiado largo, 50 caracteres maximo")]
        [RegularExpression(@"[a-zñA-ZÑ]+$", ErrorMessage = "El nombre ingresado no tiene un formato valido")]
        public string? Lastname2 { get => lastname2.Trim(); set => lastname2 = value; }

        [JsonPropertyName("password")]
        [Required]
        //[StringLength(12, MinimumLength = 8)]
        //[RegularExpression(@"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,12}$")]
        public string? Password { get => password; set => password = value; }

        [JsonPropertyName("cellphone")]
        [Required]
        [Phone]
        [RegularExpression(@"\d{10}$")]
        public string? Cellphone { get => cellphone.Trim(); set => cellphone = value; }

        [JsonPropertyName("terms")]
        [Required]
        public Boolean? Terms { get; set; }

        public override string ToString()
        {
            return $"Cellphone: {this.Cellphone}, Nombre: {this.Name} {this.Lastname} {this.Lastname2}";
        }

    }
}
