<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="CIE_2_PROJECT.ResetPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>SkillLink - Reset Password</title>
    <link rel="stylesheet" href="../Content/Css/Resetpassword.css" />
    
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
                        Back to Login
                    </asp:HyperLink>

                </div>

            </main>


            <footer class="footer">

                <div class="copyright">
                    &copy; 2025 SkillLink. All rights reserved.
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