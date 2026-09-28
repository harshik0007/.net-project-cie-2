<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CIE_2_PROJECT.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Forgot Password</title>

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

        .forgot-card {
            width: 365px;
            min-height: 315px;
            padding: 30px 24px;
            background-color: #ffffff;
            border: 1px solid #e7e9ee;
            box-shadow: 0 3px 18px rgba(0, 0, 0, 0.07);
            border-radius: 5px;
        }

        .forgot-title {
            text-align: center;
            font-size: 21px;
            font-weight: bold;
            margin-bottom: 10px;
            color: #172033;
        }

        .forgot-subtitle {
            text-align: center;
            font-size: 11px;
            line-height: 17px;
            color: #666b75;
            margin-bottom: 27px;
        }

        .form-group {
            margin-bottom: 16px;
        }

        .form-label {
            display: block;
            font-size: 9px;
            font-weight: bold;
            color: #333842;
            margin-bottom: 7px;
        }

        .input-box {
            width: 100%;
            height: 38px;
            border: 1px solid #cfd4df;
            padding: 0 10px;
            font-size: 10px;
            color: #333;
            outline: none;
        }

        .input-box:focus {
            border-color: #075cff;
        }

        .validation-error {
            display: block;
            margin-top: 5px;
            font-size: 9px;
            color: #dc3545;
        }

        .reset-button {
            width: 100%;
            height: 37px;
            border: none;
            background-color: #075cff;
            color: white;
            font-size: 9px;
            font-weight: bold;
            cursor: pointer;
            border-radius: 4px;
        }

        .reset-button:hover {
            background-color: #004bd6;
        }

        .login-text {
            text-align: center;
            margin-top: 30px;
            font-size: 9px;
            color: #555;
        }

        .login-link {
            color: #075cff;
            text-decoration: none;
        }

        .login-link:hover {
            text-decoration: underline;
        }

        .footer {
            height: 60px;
            text-align: center;
            padding-top: 10px;
            color: #666;
        }

        .copyright {
            font-size: 8px;
            margin-bottom: 6px;
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

            .forgot-card {
                width: 92%;
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