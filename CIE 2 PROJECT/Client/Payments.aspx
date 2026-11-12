<%@ Page Title="Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="CIE_2_PROJECT.Payments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Payments.css?v=5" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="payments-page">

        <div class="page-header">

            <div>
                <h1>Payments &amp; Escrow</h1>
                <p>Manage your payments, escrow funds and transaction history.</p>
            </div>

            <a href="../Guidelines/EscrowGuide.aspx" class="escrow-info-btn">
                <i class="fa-solid fa-circle-info"></i>
                How Escrow Works?
            </a>

        </div>

        <div class="summary-cards">

            <div class="summary-card escrow-card">
                <div class="summary-icon">
                    <i class="fa-solid fa-wallet"></i>
                </div>

                <div class="summary-content">
                    <span>Total Escrow Held</span>
                    <strong><asp:Label ID="lblEscrowHeld" runat="server" Text="&#8377;0" /></strong>
                    <small>Across <asp:Label ID="lblEscrowProjects" runat="server" Text="0" /> active contracts</small>
                </div>
            </div>

            <div class="summary-card paid-card">
                <div class="summary-icon">
                    <i class="fa-solid fa-paper-plane"></i>
                </div>

                <div class="summary-content">
                    <span>Total Paid to Freelancers</span>
                    <strong><asp:Label ID="lblTotalPaid" runat="server" Text="&#8377;0" /></strong>
                    <small>Released payouts and cancellation settlements</small>
                </div>
            </div>

            <div class="summary-card pending-card">
                <div class="summary-icon">
                    <i class="fa-regular fa-clock"></i>
                </div>

                <div class="summary-content">
                    <span>Pending Payments</span>
                    <strong><asp:Label ID="lblPendingAmount" runat="server" Text="&#8377;0" /></strong>
                    <small><asp:Label ID="lblPendingSummary" runat="server" Text="0 payments not yet funded" /></small>
                </div>
            </div>

            <div class="summary-card completed-card">
                <div class="summary-icon">
                    <i class="fa-regular fa-file-lines"></i>
                </div>

                <div class="summary-content">
                    <span>Completed Payments</span>
                    <strong><asp:Label ID="lblCompletedAmount" runat="server" Text="&#8377;0" /></strong>
                    <small><asp:Label ID="lblCompletedSummary" runat="server" Text="0 released payments" /></small>
                </div>
            </div>

        </div>

        <div class="payment-container">

            <div class="payment-tabs">

                <a id="lnkPending" runat="server" class="payment-tab active" href="Payments.aspx?tab=Pending">
                    Pending Payments
                    <span class="tab-count"><asp:Label ID="lblPendingCount" runat="server" Text="0" /></span>
                </a>

                <a id="lnkCompleted" runat="server" class="payment-tab" href="Payments.aspx?tab=Completed">
                    Completed Payments
                    <span class="tab-count"><asp:Label ID="lblCompletedCount" runat="server" Text="0" /></span>
                </a>

            </div>

            <asp:Label ID="lblPageMessage" runat="server" Visible="false" CssClass="payment-message" />

            <asp:Panel
                ID="pnlPending"
                runat="server"
                CssClass="payment-table">

                <div class="table-header pending-header">

                    <div>PROJECT</div>
                    <div>FREELANCER</div>
                    <div>CLIENT TOTAL</div>
                    <div>MILESTONE / DETAILS</div>
                    <div>DUE DATE</div>
                    <div>STATUS</div>
                    <div>ACTION</div>

                </div>

                <asp:Repeater ID="rptPending" runat="server">

                    <ItemTemplate>

                        <div class="payment-row pending-row">

                            <div class="project-cell">

                                <strong><%# Eval("Project") %></strong>

                                <span>
                                    <%# Eval("ProjectDescription") %>
                                </span>

                            </div>

                            <div class="freelancer-cell">

                                <div class="freelancer-avatar">
                                    <%# Eval("Initials") %>
                                </div>

                                <div>
                                    <strong><a href='<%# Eval("FreelancerProfileUrl") %>'><%# Eval("Freelancer") %></a></strong>
                                    <span><%# Eval("Role") %></span>
                                </div>

                            </div>

                            <div class="amount-cell">
                                <%# Eval("Amount") %>
                            </div>

                            <div class="milestone-cell">

                                <strong><%# Eval("Milestone") %></strong>

                                <span>
                                    <%# Eval("MilestoneDetails") %>
                                </span>

                            </div>

                            <div class="date-cell">
                                <%# Eval("DueDate") %>
                            </div>

                            <div class="status-cell">

                                <span class='status-badge <%# GetPaymentStatusClass(Eval("PaymentStatus")) %>'>
                                    <span class="status-dot"></span>
                                    <%#: Eval("PaymentStatus") %>
                                </span>

                            </div>

                            <div class="action-cell">

                                <asp:HyperLink
                                    ID="lnkProposalReview"
                                    runat="server"
                                    CssClass="release-btn"
                                    Visible='<%# Convert.ToBoolean(Eval("CanReviewProposals")) %>'
                                    Text="Review proposals"
                                    NavigateUrl='<%# Eval("ProposalReviewUrl") %>' />

                                <asp:LinkButton
                                    ID="btnRelease"
                                    runat="server"
                                    CssClass="release-btn"
                                    Visible='<%# Convert.ToBoolean(Eval("CanRelease")) %>'
                                    Text="Release Payment"
                                    CommandArgument='<%# Eval("ContractID") %>'
                                    OnClick="ReleasePayment_Click">
                                </asp:LinkButton>

                                <asp:Label ID="lblPaymentWait" runat="server"
                                    Visible='<%# !Convert.ToBoolean(Eval("CanRelease")) && !Convert.ToBoolean(Eval("CanReviewProposals")) %>'
                                    Text='<%# Eval("ActionText") %>' />

                                <asp:LinkButton
                                    ID="btnCancel"
                                    runat="server"
                                    CssClass="cancel-btn"
                                    Visible='<%# Convert.ToBoolean(Eval("CanCancel")) %>'
                                    CommandArgument='<%# Eval("ContractID") %>'
                                    OnClick="CancelProject_Click">
                                    Cancel Project
                                </asp:LinkButton>

                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>

                <asp:Panel ID="pnlPendingEmpty" runat="server" CssClass="payment-empty" Visible="false">
                    No pending payments. New contracts awaiting payment will appear here.
                </asp:Panel>

            </asp:Panel>

            <asp:Panel
                ID="pnlCompleted"
                runat="server"
                CssClass="payment-table"
                Visible="false">

                <div class="table-header completed-header">

                    <div>PROJECT</div>
                    <div>FREELANCER</div>
                    <div>CLIENT CHARGE / REFUND</div>
                    <div>DETAILS</div>
                    <div>DATE</div>
                    <div>STATUS</div>
                    <div>ACTION</div>

                </div>

                <asp:Repeater ID="rptCompleted" runat="server">

                    <ItemTemplate>

                        <div class="payment-row completed-row">

                            <div class="project-cell">

                                <strong><%# Eval("Project") %></strong>

                                <span>
                                    <%# Eval("ProjectDescription") %>
                                </span>

                            </div>

                            <div class="freelancer-cell">

                                <div class="freelancer-avatar">
                                    <%# Eval("Initials") %>
                                </div>

                                <div>
                                    <strong><a href='<%# Eval("FreelancerProfileUrl") %>'><%# Eval("Freelancer") %></a></strong>
                                    <span><%# Eval("Role") %></span>
                                </div>

                            </div>

                            <div class="amount-cell">
                                <%# Eval("Amount") %>
                            </div>

                            <div class="milestone-cell">

                                <strong><%# Eval("Milestone") %></strong>

                                <span>
                                    <%# Eval("MilestoneDetails") %>
                                </span>

                            </div>

                            <div class="date-cell">
                                <%# Eval("PaidOn") %>
                            </div>

                            <div class="status-cell">

                                <span class='status-badge <%# GetPaymentStatusClass(Eval("PaymentStatus")) %>'>
                                    <span class="status-dot"></span>
                                    <%#: Eval("PaymentStatus") %>
                                </span>

                            </div>

                            <div class="action-cell">

                                <asp:LinkButton
                                    ID="btnDetails"
                                    runat="server"
                                    CssClass="details-btn"
                                    CommandArgument='<%# Eval("ContractID") %>'
                                    OnClick="ViewDetails_Click">
                                    View Details
                                </asp:LinkButton>

                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>

                <asp:Panel ID="pnlCompletedEmpty" runat="server" CssClass="payment-empty" Visible="false">
                    No completed payments yet. Released payments and completed refunds will appear here.
                </asp:Panel>

            </asp:Panel>

        </div>

        <div class="cancellation-policy">

            <div class="policy-icon">
                <i class="fa-solid fa-circle-info"></i>
            </div>

            <div class="policy-content">

                <strong>Project Cancellation Policy</strong>

                <p>
                    Before work is submitted, cancellation returns all simulated GST and the separate client platform fee.
                    The pre-GST contract amount remains split 75% to the client, 10% to the freelancer, and 15% as the platform fee.
                    The normal freelancer fee does not stack on top of this cancellation split. This is a simulated settlement.
                    After work is submitted, the contract must follow review and payment release.
                </p>

            </div>

        </div>

    </div>

</asp:Content>
