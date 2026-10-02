<%@ Page Title="Sign Up" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="CIE_2_PROJECT.SignUp" %>

<asp:Content ID="AuthHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Signup.css?v=2" />
</asp:Content>

<asp:Content ID="AuthContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page">




            <main class="main">

                <div class="signup-card">

                    <h1 class="signup-title">
                        Create Your Account
                    </h1>

                    <p class="signup-subtitle">
                        Join SkillLink and start your freelancing journey
                    </p>


                    <div class="row">

                        <div class="half">

                            <div class="form-group">

                                <label class="form-label">
                                    Full Name
                                </label>

                                <asp:TextBox
                                    ID="txtFullName"
                                    runat="server"
                                    CssClass="input-box"
                                    placeholder="Enter your full name">
                                </asp:TextBox>

                                <asp:RequiredFieldValidator
                                    ID="rfvFullName"
                                    runat="server"
                                    ControlToValidate="txtFullName"
                                    ErrorMessage="Full name is required."
                                    CssClass="validation-error"
                                    Display="Dynamic"
                                    EnableClientScript="false">
                                </asp:RequiredFieldValidator>

                                <asp:CustomValidator
                                    ID="cvFullName"
                                    runat="server"
                                    ControlToValidate="txtFullName"
                                    ErrorMessage="Full name must contain at least 3 characters and only letters or spaces."
                                    CssClass="validation-error"
                                    Display="Dynamic"
                                    EnableClientScript="false"
                                    OnServerValidate="cvFullName_ServerValidate">
                                </asp:CustomValidator>

                            </div>

                        </div>


                        <div class="half">

                            <div class="form-group">

                                <label class="form-label">
                                    Username
                                </label>

                                <asp:TextBox
                                    ID="txtUsername"
                                    runat="server"
                                    CssClass="input-box"
                                    placeholder="Choose a username">
                                </asp:TextBox>

                                <asp:RequiredFieldValidator
                                    ID="rfvUsername"
                                    runat="server"
                                    ControlToValidate="txtUsername"
                                    ErrorMessage="Username is required."
                                    CssClass="validation-error"
                                    Display="Dynamic"
                                    EnableClientScript="false">
                                </asp:RequiredFieldValidator>

                                <asp:CustomValidator
                                    ID="cvUsername"
                                    runat="server"
                                    ControlToValidate="txtUsername"
                                    ErrorMessage="Username must have at least 3 characters and contain only letters, numbers or underscore."
                                    CssClass="validation-error"
                                    Display="Dynamic"
                                    EnableClientScript="false"
                                    OnServerValidate="cvUsername_ServerValidate">
                                </asp:CustomValidator>

                            </div>

                        </div>

                    </div>


                    <div class="form-group">

                        <label class="form-label">
                            Email Address
                        </label>

                        <asp:TextBox
                            ID="txtEmail"
                            runat="server"
                            CssClass="input-box"
                            placeholder="Enter your email address">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvEmail"
                            runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Email address is required."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvEmail"
                            runat="server"
                            ControlToValidate="txtEmail"
                            ErrorMessage="Enter a valid email address."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvEmail_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-group">

                        <label class="form-label">
                            Password
                        </label>

                        <asp:TextBox
                            ID="txtPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="input-box"
                            placeholder="Create a password">
                        </asp:TextBox>

                        <p class="password-hint">
                            At least 8 characters with letters, numbers &amp; symbols
                        </p>

                        <asp:RequiredFieldValidator
                            ID="rfvPassword"
                            runat="server"
                            ControlToValidate="txtPassword"
                            ErrorMessage="Password is required."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvPassword"
                            runat="server"
                            ControlToValidate="txtPassword"
                            ErrorMessage="Password must have at least 8 characters, a letter, a number and a symbol."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvPassword_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-group">

                        <label class="form-label">
                            Confirm Password
                        </label>

                        <asp:TextBox
                            ID="txtConfirmPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="input-box"
                            placeholder="Confirm your password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvConfirmPassword"
                            runat="server"
                            ControlToValidate="txtConfirmPassword"
                            ErrorMessage="Please confirm your password."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvConfirmPassword"
                            runat="server"
                            ControlToValidate="txtConfirmPassword"
                            ErrorMessage="Passwords do not match."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvConfirmPassword_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-group">

                        <div class="role-title">
                            I am a
                        </div>

                        <div class="role-row">

                            <div class="role-box">

                                <asp:RadioButton
                                    ID="rbFreelancer"
                                    runat="server"
                                    GroupName="UserRole"
                                    CssClass="role-radio" />

                                <div class="role-content">

                                    <label class="role-name" for="<%= rbFreelancer.ClientID %>">
                                        Freelancer
                                    </label>

                                    <label class="role-description" for="<%= rbFreelancer.ClientID %>">
                                        I want to work and learn
                                    </label>

                                </div>

                            </div>


                            <div class="role-box">

                                <asp:RadioButton
                                    ID="rbClient"
                                    runat="server"
                                    GroupName="UserRole"
                                    CssClass="role-radio" />

                                <div class="role-content">

                                    <label class="role-name" for="<%= rbClient.ClientID %>">
                                        Client
                                    </label>

                                    <label class="role-description" for="<%= rbClient.ClientID %>">
                                        I want to hire and get work done
                                    </label>

                                </div>

                            </div>

                        </div>

                        <asp:CustomValidator
                            ID="cvRole"
                            runat="server"
                            ErrorMessage="Please select Freelancer or Client."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvRole_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-group">

                        <label class="form-label">
                            Phone Number
                        </label>

                        <asp:TextBox
                            ID="txtPhone"
                            runat="server"
                            CssClass="input-box"
                            placeholder="Enter your phone number">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvPhone"
                            runat="server"
                            ControlToValidate="txtPhone"
                            ErrorMessage="Phone number is required."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvPhone"
                            runat="server"
                            ControlToValidate="txtPhone"
                            ErrorMessage="Phone number must contain exactly 10 digits."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvPhone_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="terms-row">

                        <asp:CheckBox
                            ID="chkTerms"
                            runat="server"
                            CssClass="terms-checkbox" />

                        <span>
                            I agree to the
                            <a runat="server" href="~/Guidelines/TermsOfService.aspx" class="terms-link">Terms of Service</a>
                            and
                            <a runat="server" href="~/Guidelines/PrivacyPolicy.aspx" class="terms-link">Privacy Policy</a>
                        </span>

                    </div>

                    <asp:CustomValidator
                        ID="cvTerms"
                        runat="server"
                        ErrorMessage="You must agree to the Terms of Service and Privacy Policy."
                        CssClass="validation-error"
                        Display="Dynamic"
                        EnableClientScript="false"
                        OnServerValidate="cvTerms_ServerValidate">
                    </asp:CustomValidator>


                    <asp:Button
                        ID="btnCreateAccount"
                        runat="server"
                        Text="Create Account"
                        CssClass="create-button"
                        OnClick="btnCreateAccount_Click" />


                    <p class="login-text">

                        Already have an account?

                        <asp:HyperLink
                            ID="lnkLogin"
                            runat="server"
                            CssClass="login-link"
                            NavigateUrl="Login.aspx">
                            Login
                        </asp:HyperLink>

                    </p>

                </div>

            </main>


            <footer class="footer">

                <div class="copyright">
                    &copy; 2026 SkillLink Global. All rights reserved.
                </div>

                <div class="footer-links">

                    <a runat="server" href="~/Guidelines/PrivacyPolicy.aspx">Privacy Policy</a>
                    <a runat="server" href="~/Guidelines/TermsOfService.aspx">Terms of Service</a>
                    <a runat="server" href="~/Guidelines/SupportCenter.aspx">Support Center</a>
                    <a runat="server" href="~/Guidelines/CommunityGuidelines.aspx">Community Guidelines</a>

                </div>

            </footer>

        </div>
</asp:Content>
