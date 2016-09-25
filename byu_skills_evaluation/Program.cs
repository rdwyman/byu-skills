using Octokit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace byu_skills_evaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            try
            {
                // get all users of an organization whose name is null
                Console.WriteLine("Getting GitHub users");
                GithubOrgUsersClient goc = new GithubOrgUsersClient(
                    ConfigurationManager.AppSettings["gitOrgName"],
                    new Credentials(ConfigurationManager.AppSettings["gitLogin"],
                    ConfigurationManager.AppSettings["gitPassword"])
                    );
                List<User> noNameUsers = goc.FindMatchingUsers(x => x.Name == null).ToList();

                // send email to users to tell them to add their name
                Console.WriteLine("Sending emails to users without name");
                List<string> noNameUserEmails = noNameUsers.ConvertAll(x => x.Email);
                List<string[]> noNameUserTokens = noNameUsers.ConvertAll(x => new string[] { x.Login, x.HtmlUrl });
                FormEmailClient fec = new FormEmailClient();
                List<bool> emailSuccess = fec.SendFormEmail(
                    ConfigurationManager.AppSettings["formEmailSubject"],
                    ConfigurationManager.AppSettings["formEmailMessage"],
                    noNameUserEmails, noNameUserTokens);

                // store users with no name field in an AWS bucket
                // see http://stackoverflow.com/questions/7975935/is-there-a-linq-
                //    function -for-getting-the-longest-string-in-a-list-of-strings
                // answer by SimonC
                Console.WriteLine("Logging to AWS");
                LogAwsClient lac = new LogAwsClient(
                    ConfigurationManager.AppSettings["awsAccessKey"],
                    ConfigurationManager.AppSettings["awsSecretKey"],
                    ConfigurationManager.AppSettings["awsEndpoint"]);
                List<string> userLogins = noNameUsers.Select(x => x.Login).ToList();
                int padUserName = userLogins.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length + 5;
                List<string> emailEntry = emailSuccess.Zip(noNameUserEmails, (x, y) => x ? y : "COULD_NOT_SEND_EMAIL").ToList();
                int padEmailField = emailEntry.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur).Length + 5;
                List<string> entries = userLogins.Zip(emailEntry, (x, y) => x.PadLeft(padUserName) + y.PadLeft(padEmailField)).ToList();
                lac.Log(
                    ConfigurationManager.AppSettings["awsBucket"],
                    ConfigurationManager.AppSettings["awsLogName"],
                    entries);
            }
            catch (Exception e)
            {
                Console.WriteLine("\tFATAL ERROR: \"" + e.Message + "\"");
            }

            Console.WriteLine("Finished");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
