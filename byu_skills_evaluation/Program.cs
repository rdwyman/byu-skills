using System;
using System.Collections.Generic;
using Octokit;

namespace byu_skills_evaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting");

                // set up data
                string orgName = "BYUMicrostructureResearch";
                Credentials githubCredentials = new Credentials("rdwyman", "FAKE");
                
                // get all users of an organization whose name is null
                Console.WriteLine("Getting GitHub users for \"" + orgName + "\"");
                GithubOrgUsers goc = new GithubOrgUsers(orgName, githubCredentials);
                IEnumerable<User> noNameUsers = goc.FindMatchingUsers(x => x.Name == null);

                // send email to users to tell them to add their name



            }
            catch (Exception e)
            {
                Console.WriteLine("\tFATAL ERROR: \"" + e.Message + "\"");
            }

            Console.WriteLine("Finished");
            Console.ReadKey();

        }
    }
}
