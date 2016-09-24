using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace byu_skills_evaluation
{
    internal class GithubOrgUsers
    {
        private string orgName;
        IList<User> users;

        public GithubOrgUsers(string orgName, Credentials githubCredentials)
        {
            // set up a client
            try
            {
                this.orgName = orgName;
                Task<User[]> task = InitUsers(githubCredentials);
                task.Wait();
                users = task.Result;
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

        private async Task<User[]> InitUsers(Credentials githubCredentials)
        {
            GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("git-hub-client-for-byu-skills-test"));
            ghClient.Credentials = githubCredentials;

            // get the organization members
            IOrganizationMembersClient omc = ghClient.Organization.Member;
            IReadOnlyList<User> diminishedUsers = await omc.GetAll(this.orgName);

            // the users returned from an organization do not have all meta data. Get
            // the complete users
            IUsersClient uc = ghClient.User;
            return await Task.WhenAll(diminishedUsers.Select(x => uc.Get(x.Login)));
        }

        public IEnumerable<User> FindMatchingUsers(Func<User, bool> testFunc)
        {
            return users.Where(testFunc);
        }

    }
}