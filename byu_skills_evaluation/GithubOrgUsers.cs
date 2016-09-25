using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace byu_skills_evaluation
{
    internal class GithubOrgUsersClient
    {
        private string orgName;
        IList<User> users;

        /// <summary>
        /// A client that uses Octokit to get users in an organization
        /// </summary>
        /// <param name="orgName">the organization to get users from</param>
        /// <param name="credentials">Credentials to your personal GitHub account</param>
        internal GithubOrgUsersClient(string orgName, Credentials credentials)
        {
            try
            {
                this.orgName = orgName;
                Task<User[]> task = InitUsers(credentials);
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

        /// <summary>
        /// Query GitHub to get users in organization. This is not done 
        /// in the constructor so that async capabilities can be used
        /// </summary>
        /// <param name="githubCredentials"></param>
        /// <returns></returns>
        private async Task<User[]> InitUsers(Credentials githubCredentials)
        {
            GitHubClient ghClient = new GitHubClient(new ProductHeaderValue("git-hub-client-for-byu-skills-test"));
            ghClient.Credentials = githubCredentials;

            // get the organization members
            IOrganizationMembersClient omc = ghClient.Organization.Member;
            IReadOnlyList<User> diminishedUsers = await omc.GetAll(orgName);

            // the users returned from an organization do not have all meta data. Get
            // the complete users using the direct user client
            IUsersClient uc = ghClient.User;
            return await Task.WhenAll(diminishedUsers.Select(x => uc.Get(x.Login)));
        }

        /// <summary>
        /// Returns users matching a particular test function
        /// </summary>
        /// <param name="testFunc">the test function to determine if a user should be returned</param>
        /// <returns>users passing the test function</returns>
        internal IEnumerable<User> FindMatchingUsers(Func<User, bool> testFunc)
        {
            return users.Where(testFunc);
        }
    }
}