<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="CIE_2_PROJECT.SignUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Sign Up</title>

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
            padding: 25px 0;
        }

        .signup-card {
            width: 365px;
            padding: 22px 20px;
            background-color: #ffffff;
            border: 1px solid #e7e9ee;
            box-shadow: 0 3px 18px rgba(0, 0, 0, 0.06);
            border-radius: 5px;
        }

        .signup-title {
            text-align: center;
            font-size: 21px;
            font-weight: bold;
            margin-bottom: 7px;
        }

        .signup-subtitle {
            text-align: center;
            font-size: 11px;
            color: #666b75;
            margin-bottom: 25px;
        }

        .row {
            display: flex;
            gap: 12px;
        }

        .half {
            width: 50%;
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

        .password-hint {
            font-size: 8px;
            color: #555;
            margin-top: 5px;
        }

        .validation-error {
            display: block;
            margin-top: 4px;
            font-size: 9px;
            color: #dc3545;
        }

        .role-title {
            font-size: 9px;
            font-weight: bold;
            margin-bottom: 7px;
        }

        .role-row {
            display: flex;
            gap: 12px;
        }

        .role-box {
            width: 50%;
            min-height: 59px;
            border: 1px solid #cfd4df;
            padding: 9px;
            display: flex;
            align-items: center;
        }

        .role-radio {
            margin-right: 8px;
        }

        .role-content {
            font-size: 9px;
            color: #333842;
        }

        .role-name {
            font-weight: bold;
            margin-bottom: 3px;
        }

        .role-description {
            font-size: 8px;
            line-height: 11px;
        }

        .terms-row {
            display: flex;
            align-items: flex-start;
            margin-top: 18px;
            font-size: 8px;
            color: #555;
        }

        .terms-checkbox {
            margin-right: 7px;
            margin-top: 1px;
        }

        .terms-link {
            color: #075cff;
            text-decoration: none;
        }

        .terms-link:hover {
            text-decoration: underline;
        }

        .create-button {
            width: 100%;
            height: 34px;
            margin-top: 12px;
            border: none;
            background-color: #075cff;
            color: white;
            font-size: 9px;
            font-weight: bold;
            cursor: pointer;
        }

        .create-button:hover {
            background-color: #004bd6;
        }

        .login-text {
            text-align: center;
            margin-top: 17px;
            font-size: 8px;
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

            .signup-card {
                width: 92%;
            }

            .row {
                flex-direction: column;
                gap: 0;
            }

            .half {
                width: 100%;
            }

            .role-row {
                flex-direction: column;
            }

            .role-box {
                width: 100%;
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

                                    <div class="role-name">
                                        Freelancer
                                    </div>

                                    <div class="role-description">
                                        I want to work and learn
                                    </div>

                                </div>

                            </div>


                            <div class="role-box">

                                <asp:RadioButton
                                    ID="rbClient"
                                    runat="server"
                                    GroupName="UserRole"
                                    CssClass="role-radio" />

                                <div class="role-content">

                                    <div class="role-name">
                                        Client
                                    </div>

                                    <div class="role-description">
                                        I want to hire and get work done
                                    </div>

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
                            <a href="#" class="terms-link">Terms of Service</a>
                            and
                            <a href="#" class="terms-link">Privacy Policy</a>
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