using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace byu_skills_evaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            //try
            //{
            Console.WriteLine("Starting");

            // GitHub info
            string gitOrgName = "BYUMicrostructureResearch";
            //string gitOrgName = "test-ricky-byu-skills";
            string gitLogin = "rdwyman";
            string gitPassword = "FAKE";

            // email info
            string sftpEmail = "chessofnerd@gmail.com";
            string sftpHostAddress = "sftp.gmail.com";
            string sftpUserName = "chessofnerd";
            string sftpPassword = "FAKE_PASSWORD";
            string subject = "About your Git hub profile";
            string formMessage = "Hi, {0}! You are part of the Github organization " + gitOrgName
                + ". I noticed you haven't added your name to your Github profile. Please use the link {1}"
                + " to update your profile!\n";

            // aws info
            string awsAccessKey = "FAKE";
            string awsSecretKey = "FAKE";
            string awsBucket = "this-is-rickys-test-bucket";
            string awsLogName = "MyLogFile";

            // get all users of an organization whose name is null
            Console.WriteLine("Getting GitHub users for \"" + gitOrgName + "\"");
            GithubOrgUsersClient goc = new GithubOrgUsersClient(gitOrgName, new Credentials(gitLogin, gitPassword));
            List<User> noNameUsers = goc.FindMatchingUsers(x => x.Name == null).ToList();

            // send email to users to tell them to add their name
            Console.WriteLine("Sending emails to users without name via \"" + sftpEmail + "\"");
            List<string> noNameUserEmails = noNameUsers.ConvertAll(x => x.Email);
            List<string[]> noNameUserTokens = noNameUsers.ConvertAll(x => new string[] { x.Login, x.HtmlUrl });
            FormEmailClient fec = new FormEmailClient(sftpEmail, sftpHostAddress, new NetworkCredential(sftpUserName, sftpPassword));
            List<bool> emailSuccess = fec.SendFormEmail(
                subject, formMessage, noNameUserEmails, noNameUserTokens);

            // store users with no name field in an AWS bucket
            LogAwsClient lac = new LogAwsClient(awsAccessKey, awsSecretKey);
            lac.Log(awsBucket, awsLogName);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("\tFATAL ERROR: \"" + e.Message + "\"");
            //}

            Console.WriteLine("Finished");
            Console.ReadKey();

        }
    }
}
