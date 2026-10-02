<%@ Page Title="Forgot Password" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CIE_2_PROJECT.ForgotPassword" %>

<asp:Content ID="AuthHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ForgotPassword.css?v=2" />
</asp:Content>

<asp:Content ID="AuthContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page">




            <main class="main">

                <div class="forgot-card">

                    <h1 class="forgot-title">
                        Forgot Your Password?
                    </h1>

                    <p class="forgot-subtitle">
                        No worries! Enter your email address and we'll send
                        <br />
                        you a link to reset your password.
                    </p>


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


                    <asp:Button
                        ID="btnReset"
                        runat="server"
                        Text="Send Reset Link"
                        CssClass="reset-button"
                        OnClick="btnReset_Click" />


                    <p class="login-text">

                        Remember your password?

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
