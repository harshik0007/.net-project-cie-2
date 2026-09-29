<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CIE_2_PROJECT.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Forgot Password</title>

   <link rel="stylesheet" href="Content/Css/ForgotPassword.css" />

</head>

<body>

    <form id="form1" runat="server">

        <div class="page">

            <header class="header">

                <div class="logo">

                    <div class="logo-icon">
                        S
                    </div>

                    <span>SkillLink</span>

                </div>

            </header>


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
                    © 2025 SkillLink. All rights reserved.
                </div>

                <div class="footer-links">

                    <a href="#">
                        Privacy Policy
                    </a>

                    <a href="#">
                        Terms of Service
                    </a>

                    <a href="#">
                        Help Center
                    </a>

                </div>

            </footer>

        </div>

    </form>

</body>

</html>