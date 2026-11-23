# SkillLink

SkillLink is a server-rendered freelance marketplace built with ASP.NET Web Forms and C# on .NET Framework 4.7.2. Clients can post projects, review proposals, fund simulated contracts, and approve submitted work. Freelancers can find projects, submit proposals and work, and manage their profiles. Both roles can message, review, and receive notifications.

## Technology

- ASP.NET Web Forms with C# code-behind
- SQL Server with ADO.NET repositories
- HTML and CSS for the interface
- No MVC or Web API framework; server requests are handled by Web Forms pages and C# code-behind

## Open the project

Open `CIE 2 PROJECT.slnx` in Visual Studio with the .NET Framework 4.7.2 targeting pack installed. Configure the `SkillLinkDBConnection` connection string in `CIE 2 PROJECT/Web.config`. The selected database must already have the SkillLink schema and required migrations applied.
