<%@ Page Title="Payment History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WorkHistory.aspx.cs" Inherits="CIE_2_PROJECT.WorkHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/WorkHistory.css?v=4" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="work-history-page payment-history-page">
        <div class="work-history-header">
            <div>
                <h1>Payment History</h1>
                <p>Review funding, released payments, and refunds for your contracts.</p>
            </div>
            <a class="payment-history-back" href="ActiveWork.aspx">
                <i class="fa-solid fa-arrow-left"></i>
                Active Work
            </a>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="work-history-message" Visible="false" />
        <div class="payment-history-summary">
            <div class="payment-history-summary-icon"><i class="fa-solid fa-receipt"></i></div>
            <div class="payment-history-summary-item">
                <span>Transactions</span>
                <strong><asp:Label ID="lblTransactionCount" runat="server" Text="0" /></strong>
            </div>
            <div class="payment-history-summary-divider"></div>
            <div class="payment-history-summary-item">
                <span>Total received by you</span>
                <strong><asp:Label ID="lblTotalReleased" runat="server" Text="&#8377;0" /></strong>
            </div>
        </div>
        <asp:Panel ID="pnlNoPayments" runat="server" CssClass="work-history-empty" Visible="false">
            <div class="payment-history-empty-icon"><i class="fa-regular fa-credit-card"></i></div>
            <h2>No payment history yet</h2>
            <p>Contract funding, released payments, and refunds will appear here.</p>
        </asp:Panel>
        <asp:Repeater ID="rptPayments" runat="server">
            <ItemTemplate>
                <article class="payment-history-card">
                    <div class='payment-history-icon <%#: Eval("StatusClass") %>'>
                        <i class='<%#: Eval("StatusIcon") %>'></i>
                    </div>
                    <div class="payment-history-main">
                        <div class="payment-history-title-row">
                            <h2 title='<%#: Eval("ProjectTitle") %>'><%#: Eval("ProjectTitle") %></h2>
                            <span class='payment-history-status <%#: Eval("StatusClass") %>'>
                                <i class='<%#: Eval("StatusIcon") %>'></i>
                                <%#: Eval("Status") %>
                            </span>
                        </div>
                        <div class="payment-history-details">
                            <span><i class="fa-regular fa-calendar"></i> <%#: Eval("ActivityDate") %></span>
                            <span><i class="fa-regular fa-file-lines"></i> <%#: Eval("ActivityLabel") %></span>
                        </div>
                        <p class="payment-history-note"><%#: Eval("Details") %></p>
                    </div>
                    <div class="payment-history-amount">
                        <span>Your payout</span>
                        <strong><%#: Eval("Amount") %></strong>
                    </div>
                </article>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
