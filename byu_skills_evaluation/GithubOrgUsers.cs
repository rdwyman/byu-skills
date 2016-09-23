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

        public GithubOrgUsers(string orgName, Credentials githubCredentials)
        {
            // set up a client
            try
            {
                this.orgName = orgName;
                GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("git-hub-client-for-byu-skills-test"));
                ghClient.Credentials = githubCredentials;
                                
                // get the organization members
                IOrganizationMembersClient omc = ghClient.Organization.Member;
                users = omc.GetAll(this.orgName).Result;
            }
            catch (Exception e) when (e.InnerException is AuthorizationException)
            {
                throw new Exception("Credentials for Github were invalid", e);
            }
            catch (Exception e) when (e.InnerException is NotFoundException)
            {
                throw new Exception("The Github group \"" + orgName + "\" does not exist", e);
            }
            catch (Exception e)
            {
                throw new Exception("Unprocessed error occurred when attempting to query Github", e);
            }
        }

        public IEnumerable<User> FindMatchingUsers(Func<User, bool> testFunc)
        {
            return users.Where(testFunc);
        }

    }
}