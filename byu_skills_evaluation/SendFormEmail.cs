using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace byu_skills_evaluation
{
    class FormEmailSender
    {
        private readonly string sftpEmail;
        private readonly SmtpClient client;

        public FormEmailSender(string sftpEmail, NetworkCredential sftpCredentials)
        {
            this.sftpEmail = sftpEmail;

            client = new SmtpClient();
            client.Credentials = sftpCredentials;
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

        }

        internal List<bool> SendFormEmail(string subject, string formEmail, List<string> emailAddresses, List<string[]> tokens)
        {
            // http://stackoverflow.com/questions/757987/send-email-via-c-sharp-through-google-apps-account answer by Achilles

            for (int i = 0; i < emailAddresses.Count; i++)
            {
                try
                {
                    string curEmail = emailAddresses[i];
                    string[] curTokenArray = tokens[i];
                    if (curEmail != null)
                    {
                        MailAddress maFrom = new MailAddress(sftpEmail, "Sender's Name", Encoding.UTF8);
                        MailAddress maTo = new MailAddress(curEmail, "Recipient's Name", Encoding.UTF8);
                        MailMessage mmsg = new MailMessage(maFrom.Address, maTo.Address);
                        mmsg.Body = string.Format(formEmail, tokens[i]);
                        mmsg.BodyEncoding = Encoding.UTF8;
                        mmsg.IsBodyHtml = true;
                        mmsg.Subject = subject;
                        mmsg.SubjectEncoding = Encoding.UTF8;

                        client.Send(mmsg);
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString(), ex.Message);
                }
            }

            return null;
        }
    }
}
