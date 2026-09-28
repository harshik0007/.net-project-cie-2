<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="CIE_2_PROJECT.VerifyEmail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Verify Email</title>

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

        .verify-card {
            width: 340px;
            min-height: 425px;
            padding: 30px 24px;
            background-color: #ffffff;
            border: 1px solid #e7e9ee;
            box-shadow: 0 3px 18px rgba(0, 0, 0, 0.07);
            border-radius: 5px;
            text-align: center;
        }

        .verify-logo {
            width: 38px;
            height: 38px;
            margin: 0 auto 25px auto;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #075cff;
            font-size: 30px;
            font-weight: bold;
        }

        .verify-title {
            font-size: 17px;
            font-weight: bold;
            margin-bottom: 10px;
            color: #172033;
        }

        .verify-subtitle {
            font-size: 10px;
            line-height: 16px;
            color: #555d6d;
            margin-bottom: 16px;
        }

        .email-box {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            padding: 6px 10px;
            background-color: #f4f6ff;
            border: 1px solid #dce2f7;
            border-radius: 15px;
            font-size: 9px;
            color: #555d6d;
            margin-bottom: 24px;
        }

        .change-link {
            color: #075cff;
            text-decoration: none;
            font-weight: bold;
            margin-left: 4px;
        }

        .change-link:hover {
            text-decoration: underline;
        }

        .otp-row {
            display: flex;
            justify-content: center;
            gap: 7px;
            margin-bottom: 22px;
        }

        .otp-box {
            width: 37px;
            height: 43px;
            border: 1px solid #cfd4df;
            border-radius: 6px;
            text-align: center;
            font-size: 16px;
            color: #172033;
            outline: none;
        }

        .otp-box:focus {
            border-color: #075cff;
        }

        .validation-error {
            display: block;
            margin-top: -15px;
            margin-bottom: 15px;
            font-size: 9px;
            color: #dc3545;
        }

        .resend-text {
            font-size: 9px;
            color: #555d6d;
            line-height: 18px;
            margin-bottom: 20px;
        }

        .resend-link {
            color: #075cff;
            text-decoration: none;
        }

        .resend-link:hover {
            text-decoration: underline;
        }

        .timer {
            color: #7a8090;
        }

        .verify-button {
            width: 100%;
            height: 35px;
            border: none;
            background-color: #075cff;
            color: white;
            font-size: 9px;
            font-weight: bold;
            cursor: pointer;
            border-radius: 4px;
        }

        .verify-button:hover {
            background-color: #004bd6;
        }

        .back-link {
            display: block;
            margin-top: 25px;
            color: #075cff;
            text-decoration: none;
            font-size: 9px;
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

            .verify-card {
                width: 92%;
            }

            .otp-row {
                gap: 5px;
            }

            .otp-box {
                width: 34px;
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

                <div class="verify-card">

                    <div class="verify-logo">
                        S
                    </div>

                    <h1 class="verify-title">
                        Verify Your Email
                    </h1>

                    <p class="verify-subtitle">
                        Enter the 6-digit code sent to your email address
                    </p>


                    <div class="email-box">

                        <span>✉</span>

                        <asp:Label
                            ID="lblEmail"
                            runat="server"
                            Text="h***@gmail.com">
                        </asp:Label>

                        <asp:HyperLink
                            ID="lnkChange"
                            runat="server"
                            NavigateUrl="ForgotPassword.aspx"
                            CssClass="change-link">
                            Change
                        </asp:HyperLink>

                    </div>


                   <div class="otp-row">

    <asp:TextBox
        ID="txtOtp1"
        runat="server"
        CssClass="otp-box"
        MaxLength="1">
    </asp:TextBox>

    <asp:TextBox
        ID="txtOtp2"
        runat="server"
        CssClass="otp-box"
        MaxLength="1">
    </asp:TextBox>

    <asp:TextBox
        ID="txtOtp3"
        runat="server"
        CssClass="otp-box"
        MaxLength="1">
    </asp:TextBox>

    <asp:TextBox
        ID="txtOtp4"
        runat="server"
        CssClass="otp-box"
        MaxLength="1">
    </asp:TextBox>

    <asp:TextBox
        ID="txtOtp5"
        runat="server"
        CssClass="otp-box"
        MaxLength="1">
    </asp:TextBox>

    <asp:TextBox
        ID="txtOtp6"
        runat="server"
        CssClass="otp-box"
        MaxLength="1">
    </asp:TextBox>

</div>

<asp:RequiredFieldValidator
    ID="rfvOtp1"
    runat="server"
    ControlToValidate="txtOtp1"
    ErrorMessage="Enter all 6 digits of the OTP."
    CssClass="validation-error"
    Display="Dynamic"
    EnableClientScript="false">
</asp:RequiredFieldValidator>

<asp:RequiredFieldValidator
    ID="rfvOtp2"
    runat="server"
    ControlToValidate="txtOtp2"
    ErrorMessage="Enter all 6 digits of the OTP."
    CssClass="validation-error"
    Display="None"
    EnableClientScript="false">
</asp:RequiredFieldValidator>

<asp:RequiredFieldValidator
    ID="rfvOtp3"
    runat="server"
    ControlToValidate="txtOtp3"
    ErrorMessage="Enter all 6 digits of the OTP."
    CssClass="validation-error"
    Display="None"
    EnableClientScript="false">
</asp:RequiredFieldValidator>

<asp:RequiredFieldValidator
    ID="rfvOtp4"
    runat="server"
    ControlToValidate="txtOtp4"
    ErrorMessage="Enter all 6 digits of the OTP."
    CssClass="validation-error"
    Display="None"
    EnableClientScript="false">
</asp:RequiredFieldValidator>

<asp:RequiredFieldValidator
    ID="rfvOtp5"
    runat="server"
    ControlToValidate="txtOtp5"
    ErrorMessage="Enter all 6 digits of the OTP."
    CssClass="validation-error"
    Display="None"
    EnableClientScript="false">
</asp:RequiredFieldValidator>

<asp:RequiredFieldValidator
    ID="rfvOtp6"
    runat="server"
    ControlToValidate="txtOtp6"
    ErrorMessage="Enter all 6 digits of the OTP."
    CssClass="validation-error"
    Display="None"
    EnableClientScript="false">
</asp:RequiredFieldValidator>

<asp:CustomValidator
    ID="cvOtp"
    runat="server"
    ErrorMessage="OTP must contain exactly 6 digits."
    CssClass="validation-error"
    Display="Dynamic"
    EnableClientScript="false"
    OnServerValidate="cvOtp_ServerValidate">
</asp:CustomValidator>


                    <div class="resend-text">

                        Didn't receive the code?

                        <asp:LinkButton
                            ID="btnResend"
                            runat="server"
                            CssClass="resend-link"
                            OnClick="btnResend_Click">
                            Resend OTP
                        </asp:LinkButton>

                        <br />

                        <span class="timer">
                            Resend in 00:45
                        </span>

                    </div>


                    <asp:Button
                        ID="btnVerify"
                        runat="server"
                        Text="Verify OTP"
                        CssClass="verify-button"
                        OnClick="btnVerify_Click" />


                    <asp:HyperLink
                        ID="lnkBack"
                        runat="server"
                        NavigateUrl="ForgotPassword.aspx"
                        CssClass="back-link">
                        ← Go Back
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