using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using MimeKit;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Utilities
{
    public class GmailApi
    {
        private static string[] Scopes = { GmailService.Scope.MailGoogleCom };

        public static GmailService CreateService(string pathSettings, string appName, string accountSender)
        {
            GoogleCredential credential;


            using (var stream = new FileStream(pathSettings, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                credential = GoogleCredential.FromStream(stream).
                    CreateScoped(Scopes).
                    CreateWithUser(accountSender);
            }

            var service = new GmailService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = appName
            });

            return service;
        }

        public static Message CreatePackage(IEnumerable<string> to, IEnumerable<string> cc, string subject, string from, string body, bool html)
        {
            MailAddress origin = new MailAddress(from);

            var rawMail = new MailMessage
            {
                Subject = subject,
                From = origin,
                Body = body,
                IsBodyHtml = html,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };

            AddRecipients(to, rawMail.To);
            AddRecipients(cc, rawMail.CC);

            var mail = MimeMessage.CreateFromMailMessage(rawMail);

            var message = new Message();
            message.Raw = Base64UrlEncode(mail.ToString());

            return message;
        }



        public static MailMessage CreateMailBase(IEnumerable<string> to, IEnumerable<string> cc, string subject, string from, bool html)
        {
            MailAddress origin = new MailAddress(from);
            var mailstring = new StringWriter();

            var package = new MailMessage
            {
                Subject = subject,
                From = origin,
                IsBodyHtml = html,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8
            };


            AddRecipients(to, package.To);
            AddRecipients(cc, package.CC);

            return package;

            //package.AlternateViews.Add((AlternateView)body);
            //var mail = MimeMessage.CreateFromMailMessage(package);

            //var message = new Message();
            //message.Raw = Base64UrlEncode(mail.ToString());

            //return message;
        }

        public static AlternateView CreateAlterview(string body)
        {
            var view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MediaTypeNames.Text.Html);

            return view;
        }


        public static LinkedResource CreateLinkResouse(Stream stream, string id)
        {
            var link = new LinkedResource(stream);
            link.ContentId = id;

            return link;
        }

        public static LinkedResource CreateLinkResouse(string filepath, string id)
        {
            var link = new LinkedResource(filepath);
            link.ContentId = id;

            return link;
        }

        public static MailMessage AddAttachment(string attachtment, MailMessage mail)
        {
            mail.Attachments.Add(new Attachment(attachtment));
            return mail;
        }

        public static MailMessage AddAlterviewToMail(MailMessage mail, AlternateView view)
        {
            mail.AlternateViews.Add(view);

            return mail;
        }

        public static Message CreateGoogleMail(MailMessage rawmail)
        {
            Message message = new Message();
            var mail = MimeMessage.CreateFromMailMessage(rawmail);

            message.Raw = Base64UrlEncode(mail.ToString());

            return message;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);

            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        private static void AddRecipients(IEnumerable<string> mails, ICollection<MailAddress> addresses)
        {
            if (mails != null)
            {
                foreach (var item in mails)
                {
                    addresses.Add(new MailAddress(item));
                }
            }
        }
    }
}
