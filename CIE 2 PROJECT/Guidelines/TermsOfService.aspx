<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TermsOfService.aspx.cs" Inherits="CIE_2_PROJECT.TermsOfService" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/TermsOfService.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="terms-page">

        <div class="terms-header">
            <h1>Terms of Service</h1>
            <p>Last updated: 2026</p>
        </div>

        <div class="terms-card">

            <section class="terms-section">
                <h2>1. Acceptance of Terms</h2>
                <p>
                    By creating an account or using SkillLink, you agree to follow
                    these Terms of Service and all applicable rules of the platform.
                </p>
            </section>

            <section class="terms-section">
                <h2>2. User Accounts</h2>
                <p>
                    Users must provide accurate information when creating an account.
                    You are responsible for maintaining the security of your account
                    credentials and for all activity performed through your account.
                </p>
            </section>

            <section class="terms-section">
                <h2>3. Freelancer Responsibilities</h2>
                <p>
                    Freelancers are responsible for submitting accurate proposals,
                    completing accepted projects according to agreed requirements,
                    and communicating professionally with clients.
                </p>
            </section>

            <section class="terms-section">
                <h2>4. Client Responsibilities</h2>
                <p>
                    Clients are responsible for providing clear project requirements,
                    reviewing submitted work fairly, and communicating project
                    expectations with freelancers.
                </p>
            </section>

            <section class="terms-section">
                <h2>5. Projects and Proposals</h2>
                <p>
                    Project proposals, prices, deadlines, and other project terms
                    should be agreed upon by the involved users before work begins.
                    Users should provide truthful and relevant project information.
                </p>
            </section>

            <section class="terms-section">
                <h2>6. Payments</h2>
                <p>
                    SkillLink currently records payments through an academic simulation. For new funding records, the client is
                    charged the agreed contract amount, a separate 5% demo client platform fee, and 18% demo GST calculated on the
                    contract amount. On normal approval, a separate 5% freelancer platform fee is deducted from the contract amount;
                    the freelancer receives the remaining 95%. These fee and tax values are fixed demo assumptions and are not tax
                    advice, a determination of actual GST, or a real charge or transfer. No real payment, tax, invoice, or transfer
                    is created.
                </p>
            </section>

            <section class="terms-section">
                <h2>7. Escrow cancellation and allocation</h2>
                <p>
                    A client may cancel a funded contract under this policy only before the freelancer submits any work. The full
                    demo GST line and the separate 5% client platform fee are returned to the client. The pre-GST contract amount
                    is allocated as follows: 75% is refunded to the client, 10% is credited to the freelancer, and 15% is recorded
                    as the SkillLink platform fee. These allocations are rounded to the nearest paise and sum to the contract amount;
                    the normal 5% freelancer fee does not replace or stack on top of this cancellation allocation. After work is submitted, this
                    cancellation path is unavailable; the submission follows the review and revision process. No real funds or tax
                    are collected or transferred.
                </p>
            </section>

            <section class="terms-section">
                <h2>8. Prohibited Activities</h2>
                <p>
                    Users must not use SkillLink for fraudulent activities,
                    unauthorized access, harmful content, impersonation, or any
                    activity that violates applicable laws or platform rules.
                </p>
            </section>

            <section class="terms-section">
                <h2>9. Account Termination</h2>
                <p>
                    SkillLink may restrict or terminate accounts that violate
                    these Terms of Service or misuse the platform.
                </p>
            </section>

            <section class="terms-section last-section">
                <h2>10. Changes to These Terms</h2>
                <p>
                    SkillLink may update these Terms of Service when necessary.
                    Continued use of the platform after changes indicates acceptance
                    of the updated terms.
                </p>
            </section>

        </div>

    </div>

</asp:Content>
