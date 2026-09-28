<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FreelancerSkills.aspx.cs" Inherits="CIE_2_PROJECT.FreelancerSkills" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SkillLink - Select Skills</title>

    <style>
        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
            background: #ffffff;
            color: #14213d;
        }

        .header {
            height: 60px;
            display: flex;
            align-items: center;
            padding: 0 24px;
        }

        .brand {
            display: flex;
            align-items: center;
            gap: 8px;
            color: #075cff;
            font-size: 15px;
            font-weight: bold;
        }

        .brand-logo {
            width: 22px;
            height: 22px;
            border-radius: 50%;
            background: #075cff;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 12px;
            font-weight: bold;
        }

        .page {
            min-height: calc(100vh - 60px);
            display: flex;
            justify-content: center;
            padding: 34px 20px 40px;
        }

        .card {
            width: 725px;
            max-width: 100%;
            border: 1px solid #e5e7eb;
            border-radius: 10px;
            padding: 42px 42px 36px;
            box-shadow: 0 3px 12px rgba(0, 0, 0, 0.05);
            height: fit-content;
        }

        .page-logo {
            width: 18px;
            height: 18px;
            margin: 0 auto 10px;
            border-radius: 50%;
            background: #075cff;
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 10px;
            font-weight: bold;
        }

        h1 {
            text-align: center;
            font-size: 24px;
            margin: 0 0 10px;
        }

        .subtitle {
            text-align: center;
            color: #4b5563;
            font-size: 14px;
            line-height: 1.5;
            margin: 0 auto 32px;
            max-width: 600px;
        }

        .section-title {
            font-size: 14px;
            font-weight: bold;
            margin-bottom: 12px;
        }

        .skills-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 10px;
        }

        .skill {
            border: 1px solid #d1d5db;
            border-radius: 7px;
            padding: 11px 10px;
            min-height: 58px;
            display: flex;
            align-items: center;
            background: #ffffff;
        }

        .skill:hover {
            border-color: #075cff;
        }

        .skill input {
            margin-right: 9px;
            width: 17px;
            height: 17px;
            accent-color: #075cff;
        }

        .skill label {
            font-size: 12px;
            line-height: 1.25;
            color: #172033;
            cursor: pointer;
        }

        .selected {
            margin-top: 24px;
            font-size: 13px;
            font-weight: bold;
        }

        .info-box {
            margin-top: 10px;
            padding: 14px;
            border: 1px dashed #cbd5e1;
            border-radius: 7px;
            background: #f8faff;
            color: #4b5563;
            font-size: 12px;
            text-align: center;
        }

        .validation-error {
            display: block;
            color: #dc3545;
            font-size: 12px;
            margin-top: 8px;
        }

        .buttons {
            margin-top: 28px;
        }

        .continue-button {
            width: 100%;
            height: 38px;
            border: none;
            border-radius: 5px;
            background: #075cff;
            color: white;
            font-size: 13px;
            font-weight: bold;
            cursor: pointer;
        }

        .skip-button {
            width: 100%;
            height: 38px;
            margin-top: 9px;
            border: 1px solid #cbd5e1;
            border-radius: 5px;
            background: white;
            color: #374151;
            font-size: 13px;
            font-weight: bold;
            cursor: pointer;
        }

        @media (max-width: 700px) {
            .card {
                padding: 30px 20px;
            }

            .skills-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 450px) {
            .skills-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
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