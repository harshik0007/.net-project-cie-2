<%@ Page Title="Password Assistance" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CIE_2_PROJECT.ForgotPassword" %>

<asp:Content ID="AuthHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ForgotPassword.css?v=3" />
</asp:Content>

<asp:Content ID="AuthContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="password-help-page">
        <section class="password-help-card" aria-labelledby="password-help-title">
            <div class="password-help-icon" aria-hidden="true">
                <i class="fa-solid fa-shield-halved"></i>
            </div>
            <p class="password-help-eyebrow">ACCOUNT ACCESS</p>
            <h1 id="password-help-title">Forgot or reset your password?</h1>
            <p class="password-help-description">
                Email-based password reset is not implemented yet. For help accessing your SkillLink account, contact our development team.
            </p>

            <div class="password-help-contact">
                <span class="password-help-contact-label">Contact the development team</span>
                <a href="mailto:htanti774@rku.ac.in">htanti774@rku.ac.in</a>
            </div>

            <p class="password-help-privacy">
                For your security, do not include your current password in your email.
            </p>

            <div class="password-help-actions">
                <a class="password-help-primary" href="Login.aspx">Back to login</a>
                <a class="password-help-secondary" runat="server" href="~/Guidelines/SupportCenter.aspx">Visit Support Center</a>
            </div>
        </section>

        <footer class="password-help-footer">
            <span>&copy; 2026 SkillLink Global.</span>
            <a runat="server" href="~/Guidelines/PrivacyPolicy.aspx">Privacy Policy</a>
            <a runat="server" href="~/Guidelines/TermsOfService.aspx">Terms of Service</a>
            <a runat="server" href="~/Guidelines/SupportCenter.aspx">Support Center</a>
        </footer>
    </main>
</asp:Content>
