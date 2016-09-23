using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace byu_skills_evaluation
{
    internal class GithubOrgUsers
    {
        private string orgName;
        IReadOnlyList<User> users;

        public GithubOrgUsers(string orgName)
        {
            // set up a client
            this.orgName = orgName;
            GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("Hello!"));
            ghClient.Credentials = new Credentials("rdwyman", "FAKE");

            // get the organization members
            IOrganizationMembersClient omc = ghClient.Organization.Member;
            users = omc.GetAll(this.orgName).Result;
        }

        public IEnumerable<User> FindMatchingUsers(Func<User,bool> testFunc)
        {
            return users.Where(testFunc);
        }


    }
}