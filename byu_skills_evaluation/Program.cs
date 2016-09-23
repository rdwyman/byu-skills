using System;
using System.Collections.Generic;
using Octokit;

namespace byu_skills_evaluation
{
    class Program
    {
        static void Main(string[] args)
        {
            string orgName = "BYUMicrostructureResearch";

            Console.WriteLine("Starting");

            // get all users of an organization whose name is null
            GithubOrgUsers goc = new GithubOrgUsers(orgName);
            IEnumerable<User> noNameUsers = goc.FindMatchingUsers(x => x.Name == null);
            foreach (User user in noNameUsers)
            {
                Console.WriteLine(">>>" + user.Name + "|" + user.Login + "|" + user.Id + "<<<");
            }

            Console.WriteLine("Finished");
            Console.ReadKey();

        }
    }
}
