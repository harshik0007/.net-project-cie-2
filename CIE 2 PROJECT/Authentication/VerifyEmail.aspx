<%@ Page Title="Verify Email" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="CIE_2_PROJECT.VerifyEmail" %>

<asp:Content ID="AuthHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Verifyemail.css?v=2" />
</asp:Content>

<asp:Content ID="AuthContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page">




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
