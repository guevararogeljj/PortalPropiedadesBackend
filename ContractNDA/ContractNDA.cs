using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ContractNDA
{
    public class ContractNDA
    {
        //private PdfDocument document = null;

        public ContractNDA()
        {
            //this.document = new PdfDocument();
        }


        public static Stream CreateContent(string content)
        {
            using (var stream = new MemoryStream())
            {
                var document = PdfGenerator.GeneratePdf(content, PdfSharp.PageSize.Letter);
                document.Save(stream);

                return stream;

            }


        }
    }
}
