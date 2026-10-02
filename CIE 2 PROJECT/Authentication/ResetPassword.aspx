<%@ Page Title="Reset Password" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="CIE_2_PROJECT.ResetPassword" %>

<asp:Content ID="AuthHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Resetpassword.css?v=2" />
</asp:Content>

<asp:Content ID="AuthContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page">




            <main class="main">

                <div class="reset-card">

                    <div class="reset-logo">
                        S
                    </div>

                    <h1 class="reset-title">
                        Create New Password
                    </h1>

                    <p class="reset-subtitle">
                        Your identity has been verified. Create a new
                        <br />
                        password for your account.
                    </p>


                    <div class="form-group">

                        <label class="form-label">
                            New Password
                        </label>

                        <asp:TextBox
                            ID="txtNewPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="input-box"
                            placeholder="Enter your new password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvNewPassword"
                            runat="server"
                            ControlToValidate="txtNewPassword"
                            ErrorMessage="New password is required."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvNewPassword"
                            runat="server"
                            ControlToValidate="txtNewPassword"
                            ErrorMessage="Password must be at least 8 characters with letters, numbers and symbols."
                            CssClass="validation-error"
                            Display="Dynamic"
                            EnableClientScript="false"
                            OnServerValidate="cvNewPassword_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-group">

                        <label class="form-label">
                            Confirm New Password
                        </label>

                        <asp:TextBox
                            ID="txtConfirmPassword"
                            runat="server"
                            TextMode="Password"
                            CssClass="input-box"
                            placeholder="Confirm your new password">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvConfirmPassword"
                            runat="server"
                            ControlToValidate="txtConfirmPassword"
                            ErrorMessage="Please confirm your new password."
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


                    <asp:Button
                        ID="btnResetPassword"
                        runat="server"
                        Text="Reset Password"
                        CssClass="reset-button"
                        OnClick="btnResetPassword_Click" />


                    <asp:HyperLink
                        ID="lnkLogin"
                        runat="server"
                        NavigateUrl="Login.aspx"
                        CssClass="back-link">
                        Back to Login
                    </asp:HyperLink>

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
