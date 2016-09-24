using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
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

            // set up data
            string orgName = "BYUMicrostructureResearch";
            //string orgName = "test-ricky-byu-skills";
            Credentials githubCredentials = new Credentials("rdwyman", "FAKE_PASSWORD");
            string sftpEmail = "chessofnerd@gmail.com";
            NetworkCredential sftpCredentials = new NetworkCredential("chessofnerd", "FAKE_PASSWORD");
            string valediction = "Yours,\nRicky Wyman";

            // get all users of an organization whose name is null
            Console.WriteLine("Getting GitHub users for \"" + orgName + "\"");
            GithubOrgUsers goc = new GithubOrgUsers(orgName, githubCredentials);
            List<User> noNameUsers = goc.FindMatchingUsers(x => x.Name == null).ToList();

            // send email to users to tell them to add their name
            Console.WriteLine("Sending emails to users without name via \"" + sftpEmail + "\"");
            List<string> noNameUserEmails = noNameUsers.ConvertAll(x => x.Email);
            List<string[]> noNameUserTokens = noNameUsers.ConvertAll(x => new string[] { x.Login, x.HtmlUrl });
            FormEmailSender fes = new FormEmailSender(sftpEmail, sftpCredentials);
            List<bool> emailSuccess = fes.SendFormEmail(
                "About your Github account","Hi, {0}! You are part of the Github organization " + orgName
                + ". I noticed you haven't added your name to your Github profile. Please use the link {1}"
                + " to update your profile!\n" + (valediction == null ? "Thanks!" : valediction)
                , noNameUserEmails, noNameUserTokens);


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
