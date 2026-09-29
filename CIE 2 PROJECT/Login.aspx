<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CIE_2_PROJECT.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Login</title>
    <link rel="stylesheet" href="Content/Css/Login.css" />

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
                                ◉
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