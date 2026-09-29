using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class FreelancerSkills : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvSkills_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            int count = GetSelectedSkillCount();

            args.IsValid = count >= 1 && count <= 10;
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string selectedSkills = GetSelectedSkills();

            // Skill saving logic will be added later.
            Response.Redirect("~/Freelancer/FreelancerHome.aspx");
        }

        protected void btnSkip_Click(object sender, EventArgs e)
        {
            // Skip logic will be added later.
            Response.Redirect("~/Freelancer/FreelancerHome.aspx");
        }

        private int GetSelectedSkillCount()
        {
            int count = 0;

            if (chkWebDevelopment.Checked) count++;
            if (chkFrontend.Checked) count++;
            if (chkBackend.Checked) count++;
            if (chkFullStack.Checked) count++;
            if (chkReact.Checked) count++;
            if (chkNode.Checked) count++;
            if (chkJava.Checked) count++;
            if (chkPython.Checked) count++;
            if (chkDotNet.Checked) count++;
            if (chkSQL.Checked) count++;
            if (chkMobile.Checked) count++;
            if (chkUIUX.Checked) count++;
            if (chkGraphic.Checked) count++;
            if (chkWordPress.Checked) count++;
            if (chkDataAnalysis.Checked) count++;
            if (chkML.Checked) count++;
            if (chkTesting.Checked) count++;
            if (chkContentWriting.Checked) count++;
            if (chkMarketing.Checked) count++;
            if (chkVideoEditing.Checked) count++;

            return count;
        }

        private string GetSelectedSkills()
        {
            string skills = "";

            if (chkWebDevelopment.Checked) skills += "Web Development, ";
            if (chkFrontend.Checked) skills += "Frontend Development, ";
            if (chkBackend.Checked) skills += "Backend Development, ";
            if (chkFullStack.Checked) skills += "Full Stack Development, ";
            if (chkReact.Checked) skills += "React, ";
            if (chkNode.Checked) skills += "Node.js, ";
            if (chkJava.Checked) skills += "Java, ";
            if (chkPython.Checked) skills += "Python, ";
            if (chkDotNet.Checked) skills += "C# / .NET, ";
            if (chkSQL.Checked) skills += "SQL / Database, ";
            if (chkMobile.Checked) skills += "Mobile App Development, ";
            if (chkUIUX.Checked) skills += "UI / UX Design, ";
            if (chkGraphic.Checked) skills += "Graphic Design, ";
            if (chkWordPress.Checked) skills += "WordPress, ";
            if (chkDataAnalysis.Checked) skills += "Data Analysis, ";
            if (chkML.Checked) skills += "Machine Learning, ";
            if (chkTesting.Checked) skills += "Software Testing / QA, ";
            if (chkContentWriting.Checked) skills += "Content Writing, ";
            if (chkMarketing.Checked) skills += "Digital Marketing, ";
            if (chkVideoEditing.Checked) skills += "Video Editing, ";

            return skills.TrimEnd(' ', ',');
        }
    }
}