<%@ Page Title="Simulated Contract Funding" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurePayment.aspx.cs" Inherits="CIE_2_PROJECT.SecurePayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/SecurePayment.css?v=6" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="payment-page">
        <div class="page-header">
            <div>
                <a href="Payments.aspx" class="back-link">Back to Payments</a>
                <h1>Fund Contract</h1>
                <p>This is an academic payment simulation. It does not collect payment details or move real money.</p>
            </div>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="payment-message" Visible="false" />

        <div class="payment-layout">
            <div class="payment-main">
                <div class="section-card">
                    <div class="section-title"><i class="fa-solid fa-file-lines"></i><h2>Contract details</h2></div>
                    <div class="project-details">
                        <span class="label">PROJECT</span>
                        <h3><asp:Label ID="lblProjectTitle" runat="server" /></h3>
                        <p><asp:Label ID="lblProjectDescription" runat="server" /></p>
                    </div>
                    <div class="freelancer-details">
                        <span class="label">FREELANCER</span>
                        <h3><asp:HyperLink ID="lnkFreelancerProfile" runat="server" /></h3>
                    </div>
                </div>

                <div class="section-card">
                    <div class="section-title"><i class="fa-solid fa-indian-rupee-sign"></i><h2>Payment breakdown</h2></div>
                    <div class="amount-row"><span>Agreed contract amount</span><strong><asp:Label ID="lblAmount" runat="server" /></strong></div>
                    <div class="amount-row"><span>GST (<asp:Label ID="lblGstRate" runat="server" />% demo rate)</span><strong><asp:Label ID="lblGstAmount" runat="server" /></strong></div>
                    <div class="amount-row"><span>Client platform fee (<asp:Label ID="lblClientFeeRate" runat="server" />%)</span><strong><asp:Label ID="lblClientFeeAmount" runat="server" /></strong></div>
                    <div class="amount-divider"></div>
                    <div class="amount-total"><span>Total simulated charge</span><strong><asp:Label ID="lblClientTotal" runat="server" /></strong></div>
                    <p>After the <asp:Label ID="lblFreelancerFeeRate" runat="server" />% freelancer platform fee of <strong><asp:Label ID="lblFreelancerFeeAmount" runat="server" /></strong>, the freelancer receives <strong><asp:Label ID="lblFreelancerNet" runat="server" /></strong> on approval.</p>
                    <p>Funding creates one Held payment record. The freelancer can submit work after funding succeeds.</p>
                    <div class="funding-confirm-note"><i class="fa-solid fa-circle-info"></i><span>Review the details, then confirm the simulated payment to hire this freelancer.</span></div>
                </div>

                <div class="escrow-note">
                    <i class="fa-solid fa-circle-info"></i>
                    <div><strong>No real payment processing</strong><p>No UPI, card, bank, or gateway details are requested.</p></div>
                </div>
            </div>

            <aside class="payment-sidebar">
                <div class="summary-card">
                    <h2>Funding summary</h2>
                    <div class="summary-project"><span>Project</span><strong><asp:Label ID="lblSummaryProject" runat="server" /></strong></div>
                    <div class="summary-row"><span>Freelancer</span><strong><asp:HyperLink ID="lnkSummaryFreelancerProfile" runat="server" /></strong></div>
                    <div class="summary-row"><span>Agreed amount</span><strong><asp:Label ID="lblSummaryAmount" runat="server" /></strong></div>
                    <div class="summary-row"><span>GST (<asp:Label ID="lblSummaryGstRate" runat="server" />%)</span><strong><asp:Label ID="lblSummaryGst" runat="server" /></strong></div>
                    <div class="summary-row"><span>Client platform fee (<asp:Label ID="lblSummaryClientFeeRate" runat="server" />%)</span><strong><asp:Label ID="lblSummaryClientFee" runat="server" /></strong></div>
                    <div class="summary-row"><span>Freelancer payout after fee</span><strong><asp:Label ID="lblSummaryFreelancerNet" runat="server" /></strong></div>
                    <div class="summary-divider"></div>
                    <div class="summary-total"><span>Total due</span><strong><asp:Label ID="lblSummaryTotal" runat="server" /></strong></div>
                    <asp:Button ID="btnFundContract" runat="server" Text="Fund Contract (Simulation)"
                        CssClass="payment-button" OnClick="FundContract_Click" />
                    <a href="Payments.aspx" class="payment-history-link">View payment history</a>
                </div>
            </aside>
        </div>
    </div>
</asp:Content>
