namespace WebDox.Models
{
    public class InitiateSigners
    {
        public string comment { get; set; }

        public InitiateSigners(string comments)
        {
            this.comment = comments;
        }

        public InitiateSigners()
        {
            this.comment = "Se inicializa firmas con los interesados atravez de portal de propiedades.";
        }
    }
}
