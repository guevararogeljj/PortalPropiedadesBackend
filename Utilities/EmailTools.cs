using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Utilities
{
    public static class EmailTools
    {
        public static AlternateView CreateAlternateView(string body)
        {
            AlternateView view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);

            return view;
        }

        public static LinkedResource CreateLinkedResource(Stream fileStream, string cid)
        {
            LinkedResource image = new LinkedResource(fileStream);
            image.ContentId = cid;
            return image;
        }

        public static LinkedResource CreateLinkedResource(string imagePath, string cid)
        {
            LinkedResource image = new LinkedResource(imagePath);
            image.ContentId = cid;
            return image;
        }
    }
}
