<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CIE_2_PROJECT.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Login</title>

    <style>

        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        body {
            font-family: Arial, Helvetica, sans-serif;
            background-color: #ffffff;
            color: #172033;
        }

        .page {
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }

        .header {
            height: 60px;
            display: flex;
            align-items: center;
            padding-left: 30px;
            border-top: 1px solid #222;
        }

        .logo {
            display: flex;
            align-items: center;
            gap: 7px;
            font-size: 14px;
            font-weight: bold;
            color: #075cff;
        }

        .logo-icon {
            width: 14px;
            height: 14px;
            background-color: #075cff;
            color: white;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 9px;
            font-weight: bold;
        }

        .main {
            flex: 1;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .login-card {
            width: 340px;
            min-height: 350px;
            padding: 25px 20px;
            background-color: #ffffff;
            box-shadow: 0 3px 18px rgba(0, 0, 0, 0.08);
            border-radius: 4px;
        }

        .login-title {
            text-align: center;
            font-size: 20px;
            font-weight: bold;
            margin-bottom: 8px;
            color: #172033;
        }

        .login-subtitle {
            text-align: center;
            font-size: 11px;
            color: #666b75;
            margin-bottom: 25px;
        }

        .form-group {
            margin-bottom: 12px;
        }

        .form-label {
            display: block;
            font-size: 9px;
            font-weight: bold;
            color: #333842;
            margin-bottom: 6px;
        }

        .input-box {
            width: 100%;
            height: 32px;
            border: 1px solid #d8dce5;
            padding: 0 10px;
            font-size: 10px;
            color: #333;
            outline: none;
        }

        .input-box:focus {
            border-color: #075cff;
        }

        .password-wrapper {
            position: relative;
        }

        .password-wrapper .input-box {
            padding-right: 35px;
        }

        .eye-icon {
            position: absolute;
            right: 10px;
            top: 8px;
            font-size: 12px;
            color: #777;
        }

        .validation-error {
            display: block;
            margin-top: 4px;
            font-size: 9px;
            color: #dc3545;
        }

        .forgot-password {
            display: block;
            margin-top: 3px;
            margin-bottom: 17px;
            font-size: 8px;
            color: #075cff;
            text-decoration: none;
        }

        .forgot-password:hover {
            text-decoration: underline;
        }

        .login-button {
            width: 100%;
            height: 31px;
            border: none;
            background-color: #075cff;
            color: white;
            font-size: 9px;
            cursor: pointer;
        }

        .login-button:hover {
            background-color: #004bd6;
        }

        .signup-text {
            text-align: center;
            margin-top: 20px;
            font-size: 8px;
            color: #555;
        }

        .signup-link {
            color: #075cff;
            text-decoration: none;
        }

        .signup-link:hover {
            text-decoration: underline;
        }

        .footer {
            height: 75px;
            text-align: center;
            padding-top: 15px;
            color: #666;
        }

        .copyright {
            font-size: 8px;
            margin-bottom: 7px;
        }

        .footer-links {
            font-size: 7px;
        }

        .footer-links a {
            color: #555;
            text-decoration: none;
            margin: 0 7px;
        }

        .footer-links a:hover {
            text-decoration: underline;
        }

        @media (max-width: 500px) {

            .header {
                padding-left: 20px;
            }

            .login-card {
                width: 90%;
                max-width: 340px;
            }

        }

    </style>

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
                        NavigateUrl="#">
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
                            NavigateUrl="#">
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