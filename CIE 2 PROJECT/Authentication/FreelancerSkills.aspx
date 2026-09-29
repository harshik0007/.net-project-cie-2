<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreelancerSkills.aspx.cs" Inherits="CIE_2_PROJECT.FreelancerSkills" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SkillLink - Select Skills</title>
    <link rel="stylesheet" href="../Content/Css/Freelancerskills.css" />
</head>

<body>

    <form id="form1" runat="server">

        <div class="header">
            <div class="brand">
                <span class="brand-logo">S</span>
                <span>SkillLink</span>
            </div>
        </div>

        <div class="page">

            <div class="card">

                <h1>Add Your Skills</h1>

                <div class="subtitle">
                    Select the skills you're confident in. These skills will help us
                    match you with relevant projects.
                </div>

                <div class="section-title">
                    Important Freelancer Skills
                </div>

                <div class="skills-grid">

                    <div class="skill">
                        <asp:CheckBox ID="chkWebDevelopment" runat="server" Text="Web Development" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkFrontend" runat="server" Text="Frontend Development" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkBackend" runat="server" Text="Backend Development" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkFullStack" runat="server" Text="Full Stack Development" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkReact" runat="server" Text="React" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkNode" runat="server" Text="Node.js" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkJava" runat="server" Text="Java" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkPython" runat="server" Text="Python" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkDotNet" runat="server" Text="C# / .NET" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkSQL" runat="server" Text="SQL / Database" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkMobile" runat="server" Text="Mobile App Development" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkUIUX" runat="server" Text="UI / UX Design" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkGraphic" runat="server" Text="Graphic Design" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkWordPress" runat="server" Text="WordPress" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkDataAnalysis" runat="server" Text="Data Analysis" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkML" runat="server" Text="Machine Learning" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkTesting" runat="server" Text="Software Testing / QA" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkContentWriting" runat="server" Text="Content Writing" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkMarketing" runat="server" Text="Digital Marketing" />
                    </div>

                    <div class="skill">
                        <asp:CheckBox ID="chkVideoEditing" runat="server" Text="Video Editing" />
                    </div>

                </div>

                <div class="selected">
                    Select Your Skills (Maximum 10)
                </div>

                <div class="info-box">
                    Select the skills that best describe your expertise.
                </div>

                <asp:CustomValidator
                    ID="cvSkills"
                    runat="server"
                    ErrorMessage="Please select at least one skill and no more than 10 skills."
                    CssClass="validation-error"
                    Display="Dynamic"
                    EnableClientScript="false"
                    OnServerValidate="cvSkills_ServerValidate">
                </asp:CustomValidator>

                <div class="buttons">

                    <asp:Button
                        ID="btnContinue"
                        runat="server"
                        Text="Continue →"
                        CssClass="continue-button"
                        OnClick="btnContinue_Click" />

                    <asp:Button
                        ID="btnSkip"
                        runat="server"
                        Text="Skip for now"
                        CssClass="skip-button"
                        OnClick="btnSkip_Click" />

                </div>

            </div>

        </div>

    </form>

</body>
</html>