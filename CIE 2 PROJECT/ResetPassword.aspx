<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="CIE_2_PROJECT.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Reset Password</title>

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

        .reset-card {
            width: 425px;
            padding: 40px 45px;
            background-color: #ffffff;
            border: 1px solid #e7e9ee;
            box-shadow: 0 3px 18px rgba(0, 0, 0, 0.07);
            border-radius: 6px;
        }

        .reset-logo {
            text-align: center;
            color: #075cff;
            font-size: 32px;
            font-weight: bold;
            margin-bottom: 22px;
        }

        .reset-title {
            text-align: center;
            font-size: 23px;
            font-weight: bold;
            margin-bottom: 12px;
        }

        .reset-subtitle {
            text-align: center;
            font-size: 12px;
            line-height: 18px;
            color: #555d6d;
            margin-bottom: 30px;
        }

        .form-group {
            margin-bottom: 18px;
        }

        .form-label {
            display: block;
            font-size: 10px;
            font-weight: bold;
            margin-bottom: 7px;
        }

        .input-box {
            width: 100%;
            height: 38px;
            border: 1px solid #cfd4df;
            padding: 0 10px;
            font-size: 11px;
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
            margin-top: 8px;
            border: none;
            background-color: #075cff;
            color: white;
            font-size: 10px;
            font-weight: bold;
            cursor: pointer;
            border-radius: 4px;
        }

        .reset-button:hover {
            background-color: #004bd6;
        }

        .back-link {
            display: block;
            text-align: center;
            margin-top: 30px;
            color: #075cff;
            text-decoration: none;
            font-size: 10px;
            font-weight: 500;
        }

        .back-link:hover {
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

            .reset-card {
                width: 92%;
                padding: 35px 25px;
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
                        ← Back to Login
                    </asp:HyperLink>

                </div>

            </main>


            <footer class="footer">

                <div class="copyright">
                    © 2025 SkillLink. All rights reserved.
                </div>

                <div class="footer-links">

                    <a href="#">Privacy Policy</a>

                    <a href="#">Terms of Service</a>

                    <a href="#">Help Center</a>

                </div>

            </footer>

        </div>

    </form>

</body>

</html>