using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace byu_skills_evaluation
{
    class FormEmailSender
    {
        private NetworkCredential sftpCredentials;
        private string sftpEmail;

        public FormEmailSender(string sftpEmail, NetworkCredential sftpCredentials)
        {
            this.sftpEmail = sftpEmail;
            this.sftpCredentials = sftpCredentials;
        }

        internal List<bool> SendFormEmail(string formEmail, List<string> emailAddresses, List<string[]> tokens)
        {
            throw new NotImplementedException();
        }
    }
}
