<%@ Page Title="Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payments.aspx.cs" Inherits="CIE_2_PROJECT.Payments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Payments.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="payments-page">

        <div class="page-header">

            <div>
                <h1>Payments &amp; Escrow</h1>
                <p>Manage your payments, escrow funds and transaction history.</p>
            </div>

            <button type="button" class="escrow-info-btn">
                <i class="fa-solid fa-circle-info"></i>
                How Escrow Works?
            </button>

        </div>

        <div class="summary-cards">

            <div class="summary-card escrow-card">
                <div class="summary-icon">
                    <i class="fa-solid fa-wallet"></i>
                </div>

                <div class="summary-content">
                    <span>Total Escrow Held</span>
                    <strong>&#8377;45,000</strong>
                    <small>Across 3 active projects</small>
                </div>
            </div>

            <div class="summary-card paid-card">
                <div class="summary-icon">
                    <i class="fa-solid fa-paper-plane"></i>
                </div>

                <div class="summary-content">
                    <span>Total Paid</span>
                    <strong>&#8377;72,500</strong>
                    <small>To freelancers</small>
                </div>
            </div>

            <div class="summary-card pending-card">
                <div class="summary-icon">
                    <i class="fa-regular fa-clock"></i>
                </div>

                <div class="summary-content">
                    <span>Pending Payments</span>
                    <strong>&#8377;25,000</strong>
                    <small>2 payments pending</small>
                </div>
            </div>

            <div class="summary-card completed-card">
                <div class="summary-icon">
                    <i class="fa-regular fa-file-lines"></i>
                </div>

                <div class="summary-content">
                    <span>Completed Payments</span>
                    <strong>&#8377;62,500</strong>
                    <small>5 transactions</small>
                </div>
            </div>

        </div>

        <div class="payment-container">

            <div class="payment-tabs">

                <asp:LinkButton
                    ID="lnkPending"
                    runat="server"
                    CssClass="payment-tab active"
                    CommandArgument="Pending"
                    OnClick="PaymentTab_Click">

                    Pending Payments
                    <span class="tab-count">2</span>

                </asp:LinkButton>

                <asp:LinkButton
                    ID="lnkCompleted"
                    runat="server"
                    CssClass="payment-tab"
                    CommandArgument="Completed"
                    OnClick="PaymentTab_Click">

                    Completed Payments
                    <span class="tab-count">3</span>

                </asp:LinkButton>

            </div>

            <asp:Panel
                ID="pnlPending"
                runat="server"
                CssClass="payment-table">

                <div class="table-header pending-header">

                    <div>PROJECT</div>
                    <div>FREELANCER</div>
                    <div>ESCROW AMOUNT</div>
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
                                    <strong><%# Eval("Freelancer") %></strong>
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

                                <span class="status-badge status-pending">
                                    <span class="status-dot"></span>
                                    Pending
                                </span>

                            </div>

                            <div class="action-cell">

                                <asp:LinkButton
                                    ID="btnRelease"
                                    runat="server"
                                    CssClass="release-btn"
                                    CommandArgument='<%# Eval("Id") %>'
                                    OnClick="ReleasePayment_Click">
                                    Release Payment
                                </asp:LinkButton>

                                <asp:LinkButton
                                    ID="btnCancel"
                                    runat="server"
                                    CssClass="cancel-btn"
                                    CommandArgument='<%# Eval("Id") %>'
                                    OnClick="CancelProject_Click">
                                    Cancel Project
                                </asp:LinkButton>

                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>

            </asp:Panel>

            <asp:Panel
                ID="pnlCompleted"
                runat="server"
                CssClass="payment-table"
                Visible="false">

                <div class="table-header completed-header">

                    <div>PROJECT</div>
                    <div>FREELANCER</div>
                    <div>AMOUNT</div>
                    <div>DETAILS</div>
                    <div>PAID ON</div>
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
                                    <strong><%# Eval("Freelancer") %></strong>
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

                                <span class="status-badge status-completed">
                                    <span class="status-dot"></span>
                                    Completed
                                </span>

                            </div>

                            <div class="action-cell">

                                <asp:LinkButton
                                    ID="btnDetails"
                                    runat="server"
                                    CssClass="details-btn"
                                    CommandArgument='<%# Eval("Id") %>'
                                    OnClick="ViewDetails_Click">
                                    View Details
                                </asp:LinkButton>

                            </div>

                        </div>

                    </ItemTemplate>

                </asp:Repeater>

            </asp:Panel>

        </div>

        <div class="cancellation-policy">

            <div class="policy-icon">
                <i class="fa-solid fa-circle-info"></i>
            </div>

            <div class="policy-content">

                <strong>Project Cancellation Policy</strong>

                <p>
                    If a project is cancelled after funds are placed in escrow,
                    a 25% cancellation deduction will apply. A portion of the
                    remaining amount may be paid to the freelancer based on the
                    work completed.
                </p>

            </div>

        </div>

    </div>

</asp:Content>