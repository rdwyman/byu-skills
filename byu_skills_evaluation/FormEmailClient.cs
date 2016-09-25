using System.Collections.Generic;
using System.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;

namespace byu_skills_evaluation
{
    internal class FormEmailClient
    {
        private readonly string smtpEmail;
        private readonly SmtpClient client;

        /// <summary>
        /// An Smtp based client to send form emails. Must be configured in App.config
        /// </summary>
        internal FormEmailClient()
        {
            SmtpSection smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
            smtpEmail = smtpSection.From;

            client = new SmtpClient();
        }

        /// <summary>
        /// sends form emails subbing in tokens to personalize each one
        /// </summary>
        /// <param name="subject">Each email's subject</param>
        /// <param name="formEmail">A string that is subbed into using String.Format() with tokens used for {#} substitutions</param>
        /// <param name="emailAddresses">A list of each address to send an email to</param>
        /// <param name="tokens">A list of string arrays. Each array is subbed into the formEmail parameter to make personalized emails</param>
        /// <returns>A list of booleans specifying which emails were successfully sent</returns>
        internal List<bool> SendFormEmail(string subject, string formEmail, IList<string> emailAddresses, IList<string[]> tokens)
        {
            List<bool> emailSentList = new List<bool>();
            for (int i = 0; i < emailAddresses.Count; i++)
            {
                bool emailSent = false;
                string curEmail = emailAddresses[i];
                if (curEmail != null)
                {
                    string[] curTokenArray = tokens[i];

                    // code from 
                    // http://stackoverflow.com/questions/757987/send-email-via-c-sharp-through-google-apps-account
                    // answer by Achilles
                    MailAddress maFrom = new MailAddress(smtpEmail, "Sender's Name", Encoding.UTF8);
                    MailAddress maTo = new MailAddress(curEmail, "Recipient's Name", Encoding.UTF8);
                    MailMessage mmsg = new MailMessage(maFrom.Address, maTo.Address);
                    mmsg.Body = string.Format(formEmail, tokens[i]);
                    mmsg.BodyEncoding = Encoding.UTF8;
                    mmsg.IsBodyHtml = true;
                    mmsg.Subject = subject;
                    mmsg.SubjectEncoding = Encoding.UTF8;

                    client.Send(mmsg);
                    emailSent = true;
                }
                emailSentList.Add(emailSent);
            }

            return emailSentList;
        }
    }
}
