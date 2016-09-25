# byu-skills
Skills Evaluation for BYU Software Engineer Position (Job ID 56635) for Ricky Wyman.

The program will query a Github organization and attempt to send an email to any user who hasn't added
their name to their Github profile. It will then store these user's logins in an AWS S3 bucket along 
with if an email could be sent or not.

# Getting Started and Running
Clone the project and then open the byu_skills_test.sln file with Visual Studio. Resolve any dependency 
issues (see the Dependencies section). Alter the App.config file to add your particular information. The
original App.config file's fields that must be changed are prefixed with a colon (example ":yourPassword"). 
Other fields are suggestions that may or may not need changing. They are meant as defaults and guides. 

Compile and run the program by pushing F5. The first time you compile will take a long time, mainly because 
the Octokit library is large (see the Developer Notes section for a defense of using Octokit).

# Assumptions
The following assumptions were made:
* I assumed I could use 3rd party libraries (like Octokit)
* I assumed I didn't have to email users who hadn't listed an email (I hope this is tautologically clear); 
however, I _did_ choose to note which users I successfully emailed in the AWS log
* I assumed I could use an SMTP server to handle sending email

# Developer Notes
I chose to use the large and verbose Octokit library. I likely could have programmed my own much thinner JSON
client to just read the organization users I needed. I justify using Octokit because it is heavily supported
by Github client experts. I don't need the domain experience they have and decided to build on their existing
knowledge. My apologies if this was not your intention for me in this exercise.

You will note that I use [LINQ](https://msdn.microsoft.com/en-us/library/bb397906.aspx) extensively. Some might 
argue I use it too much. I've always been a fan of LINQ as I feel it declutters code. Where a `for` loop 
generally takes 3+ lines, a LINQ statement is only one!

I attempted to compartmentalize technologies to a single class and make them as generic as possible. I believe
this encourages code reuse as a class can be used for different purposes. For example, I designed the 
`LogAwsClient` to log _any_ data, not just user names. A debatably negative result of this is that a lot of 
business logic exists in the main function. In a heavier application, this could easily be solved by adding a
class that either inherits or, [better yet, is composed](https://en.wikipedia.org/wiki/Composition_over_inheritance)
with the client.

# Dependencies
This project has the following dependencies

* AWSSDK.Core - Available via NuGet
* AWSSDK.S3 - Available via NuGet
* Octokit.net - Available at https://github.com/octokit/octokit.net

You may have to manually add the AWS dependencies via NuGet. In Visual Studio, this can be 
done by going to "Tools>NuGet Package Manager>Manage NuGet Packages for Solution". The 
Octokit.net library is a [submodule](https://github.com/blog/2104-working-with-submodules) of 
this project and should automatically download when you clone it via git.

# Known Issues
The following issues are known and outstanding

* If the SMTP client is incorrectly configured, the program may hang
* If the SMTP client is given bad credentials, the program may hang

See [this](http://lammps.sandia.gov/unknown.html) for a list of unknown issues.
