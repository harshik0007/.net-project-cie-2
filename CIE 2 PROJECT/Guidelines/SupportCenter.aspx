<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SupportCenter.aspx.cs" Inherits="CIE_2_PROJECT.SupportCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/SupportCenter.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="support-page">

        <div class="support-header">
            <h1>Support Center</h1>
            <p>Find help and information about using SkillLink.</p>
        </div>

        <div class="support-card">

            <section class="support-section">
                <h2>1. Getting Started</h2>
                <p>
                    Create your SkillLink account, complete your profile, and
                    select your role as a freelancer or client to get started.
                </p>
            </section>

            <section class="support-section">
                <h2>2. Account Support</h2>
                <p>
                    If you are having trouble signing in, updating your profile,
                    verifying your email, or resetting your password, use the
                    available account support options.
                </p>
            </section>

            <section class="support-section">
                <h2>3. Freelancer Support</h2>
                <p>
                    Freelancers can find projects, submit proposals, manage
                    accepted projects, submit completed work, and maintain their
                    professional profile through SkillLink.
                </p>
            </section>

            <section class="support-section">
                <h2>4. Client Support</h2>
                <p>
                    Clients can create projects, review proposals, communicate
                    with freelancers, manage active work, and review submitted work.
                </p>
            </section>

            <section class="support-section">
                <h2>5. Project Support</h2>
                <p>
                    For project-related issues, review the project requirements,
                    agreed price, deadline, proposal information, and communication
                    between the client and freelancer.
                </p>
            </section>

            <section class="support-section">
                <h2>6. Payment Support</h2>
                <p>
                    Make sure your payment information is accurate and review the
                    agreed payment details associated with your project.
                </p>
            </section>

            <section class="support-section">
                <h2>7. Reporting an Issue</h2>
                <p>
                    If you experience a problem while using SkillLink, provide
                    clear information about the issue so that the support team
                    can understand and assist with the problem.
                </p>
            </section>

            <section class="support-section last-section">
                <h2>8. Contact Support</h2>
                <p>
                    For additional assistance, contact the SkillLink support team
                    with your account information and a description of the issue.
                </p>
            </section>

        </div>


        <div class="support-form-card">

            <div class="form-header">
                <h2>Submit a Support Request</h2>
                <p>Tell us about your issue and our support team will review your request.</p>
            </div>


            <div class="form-grid">

                <div class="form-group">

                    <label for="txtName">Full Name</label>

                    <asp:TextBox
                        ID="txtName"
                        runat="server"
                        CssClass="form-input">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvName"
                        runat="server"
                        ControlToValidate="txtName"
                        ErrorMessage="Full name is required."
                        CssClass="validation-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:CustomValidator
                        ID="cvName"
                        runat="server"
                        ControlToValidate="txtName"
                        ErrorMessage="Enter a valid name."
                        CssClass="validation-message"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvName_ServerValidate">
                    </asp:CustomValidator>

                </div>


                <div class="form-group">

                    <label for="txtEmail">Email Address</label>

                    <asp:TextBox
                        ID="txtEmail"
                        runat="server"
                        CssClass="form-input"
                        TextMode="Email">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Email address is required."
                        CssClass="validation-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:CustomValidator
                        ID="cvEmail"
                        runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="Enter a valid email address."
                        CssClass="validation-message"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvEmail_ServerValidate">
                    </asp:CustomValidator>

                </div>


                <div class="form-group">

                    <label for="ddlCategory">Issue Category</label>

                    <asp:DropDownList
                        ID="ddlCategory"
                        runat="server"
                        CssClass="form-input">

                        <asp:ListItem Value="">Select an issue category</asp:ListItem>
                        <asp:ListItem Value="Account">Account &amp; Login</asp:ListItem>
                        <asp:ListItem Value="Freelancer">Freelancer Support</asp:ListItem>
                        <asp:ListItem Value="Client">Client Support</asp:ListItem>
                        <asp:ListItem Value="Project">Project Issue</asp:ListItem>
                        <asp:ListItem Value="Proposal">Proposal Issue</asp:ListItem>
                        <asp:ListItem Value="Payment">Payment Issue</asp:ListItem>
                        <asp:ListItem Value="Technical">Technical Issue</asp:ListItem>
                        <asp:ListItem Value="Other">Other</asp:ListItem>

                    </asp:DropDownList>

                    <asp:CustomValidator
                        ID="cvCategory"
                        runat="server"
                        ControlToValidate="ddlCategory"
                        ErrorMessage="Please select an issue category."
                        CssClass="validation-message"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvCategory_ServerValidate">
                    </asp:CustomValidator>

                </div>


                <div class="form-group">

                    <label for="ddlPriority">Priority</label>

                    <asp:DropDownList
                        ID="ddlPriority"
                        runat="server"
                        CssClass="form-input">

                        <asp:ListItem Value="">Select priority</asp:ListItem>
                        <asp:ListItem Value="Low">Low</asp:ListItem>
                        <asp:ListItem Value="Medium">Medium</asp:ListItem>
                        <asp:ListItem Value="High">High</asp:ListItem>

                    </asp:DropDownList>

                    <asp:CustomValidator
                        ID="cvPriority"
                        runat="server"
                        ControlToValidate="ddlPriority"
                        ErrorMessage="Please select a priority."
                        CssClass="validation-message"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvPriority_ServerValidate">
                    </asp:CustomValidator>

                </div>


                <div class="form-group full-width">

                    <label for="txtSubject">Subject</label>

                    <asp:TextBox
                        ID="txtSubject"
                        runat="server"
                        CssClass="form-input">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvSubject"
                        runat="server"
                        ControlToValidate="txtSubject"
                        ErrorMessage="Subject is required."
                        CssClass="validation-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:CustomValidator
                        ID="cvSubject"
                        runat="server"
                        ControlToValidate="txtSubject"
                        ErrorMessage="Subject must be between 5 and 100 characters."
                        CssClass="validation-message"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvSubject_ServerValidate">
                    </asp:CustomValidator>

                </div>


                <div class="form-group full-width">

                    <label for="txtMessage">Describe Your Issue</label>

                    <asp:TextBox
                        ID="txtMessage"
                        runat="server"
                        CssClass="form-input form-textarea"
                        TextMode="MultiLine"
                        Rows="6">
                    </asp:TextBox>

                    <asp:RequiredFieldValidator
                        ID="rfvMessage"
                        runat="server"
                        ControlToValidate="txtMessage"
                        ErrorMessage="Please describe your issue."
                        CssClass="validation-message"
                        Display="Dynamic">
                    </asp:RequiredFieldValidator>

                    <asp:CustomValidator
                        ID="cvMessage"
                        runat="server"
                        ControlToValidate="txtMessage"
                        ErrorMessage="Message must be between 20 and 1000 characters."
                        CssClass="validation-message"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvMessage_ServerValidate">
                    </asp:CustomValidator>

                </div>

            </div>


            <div class="form-note">
                Please provide as much detail as possible so our support team can assist you effectively.
            </div>


            <div class="form-actions">

                <asp:Button
                    ID="btnClear"
                    runat="server"
                    Text="Clear"
                    CssClass="clear-button"
                    CausesValidation="false"
                    OnClick="btnClear_Click">
                </asp:Button>

                <asp:Button
                    ID="btnSubmit"
                    runat="server"
                    Text="Submit Request"
                    CssClass="submit-button"
                    OnClick="btnSubmit_Click">
                </asp:Button>

            </div>


            <asp:Label
                ID="lblSuccess"
                runat="server"
                CssClass="success-message"
                Visible="false">
            </asp:Label>

        </div>

    </div>

</asp:Content>