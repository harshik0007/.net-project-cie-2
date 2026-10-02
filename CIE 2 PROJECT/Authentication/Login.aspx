<%@ Page Title="Log In" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CIE_2_PROJECT.Login" %>

<asp:Content ID="AuthHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Login.css?v=2" />
</asp:Content>

<asp:Content ID="AuthContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page">




            <main class="main">

                <div class="login-card">

                    <h1 class="login-title">
                        Welcome Back
                    </h1>

                    <p class="login-subtitle">
                        Login to continue your learning journey
                    </p>


                    <div class="form-group">

                        <label class="form-label">
                            Email or Username
                        </label>

                        <asp:TextBox
                            ID="txtUsername"
                            runat="server"
                            CssClass="input-box"
                            placeholder="Enter your email or username">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvUsername"
                            runat="server"
                            ControlToValidate="txtUsername"
                            ErrorMessage="Email or Username is required."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>


                    <div class="form-group">

                        <label class="form-label">
                            Password
                        </label>

                        <div class="password-wrapper">

                            <asp:TextBox
                                ID="txtPassword"
                                runat="server"
                                TextMode="Password"
                                CssClass="input-box"
                                placeholder="Enter your password">
                            </asp:TextBox>

                            <span class="eye-icon">
                                <i class="fa-solid fa-eye"></i>
                            </span>

                        </div>

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
                            ErrorMessage="Password must contain at least 6 characters."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvPassword_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <asp:HyperLink
                        ID="lnkForgotPassword"
                        runat="server"
                        CssClass="forgot-password"
                        NavigateUrl="ForgotPassword.aspx">
                        Forgot Password?
                    </asp:HyperLink>


                    <asp:Button
                        ID="btnLogin"
                        runat="server"
                        Text="Login"
                        CssClass="login-button"
                        OnClick="btnLogin_Click" />


                    <p class="signup-text">

                        Don't have an account?

                        <asp:HyperLink
                            ID="lnkSignup"
                            runat="server"
                            CssClass="signup-link"
                            NavigateUrl="SignUp.aspx">
                            Sign up
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
