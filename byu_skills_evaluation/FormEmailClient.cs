using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace byu_skills_evaluation
{
    internal class FormEmailClient
    {
        private readonly string sftpEmail;
        private readonly SmtpClient client;

        internal FormEmailClient(string sftpEmail, string sftpHostAddress, NetworkCredential sftpCredentials)
        {
            this.sftpEmail = sftpEmail;

            client = new SmtpClient();
            client.Credentials = sftpCredentials;
            client.Port = 587;
            client.Host = sftpHostAddress;
            client.EnableSsl = true;
        }

        internal List<bool> SendFormEmail(string subject, string formEmail, List<string> emailAddresses, List<string[]> tokens)
        {
            List<bool> emailSentList = new List<bool>();
            for (int i = 0; i < emailAddresses.Count; i++)
            {
                bool emailSent = false;
                string curEmail = emailAddresses[i];
                if (curEmail != null)
                {
                    try
                    {
                        string[] curTokenArray = tokens[i];

                        // code from 
                        // http://stackoverflow.com/questions/757987/send-email-via-c-sharp-through-google-apps-account
                        // answer by Achilles
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
                    catch
                    {
                        emailSent = false;
                    }
                }
                emailSentList.Add(emailSent);
            }

            return emailSentList;
        }
    }
}
