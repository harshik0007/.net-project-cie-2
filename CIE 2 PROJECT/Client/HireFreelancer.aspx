<%@ Page Title="Hire Freelancer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HireFreelancer.aspx.cs" Inherits="CIE_2_PROJECT.HireFreelancer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/HireFreelancer.css?v=3" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hire-page">
        <asp:HyperLink ID="lnkBack" runat="server" CssClass="back-link" Text="Back to Proposals" />

        <div class="page-heading">
            <h1>Hire Freelancer</h1>
            <p>Review the selected proposal before accepting it for your project.</p>
        </div>

        <asp:Label ID="lblMessage" runat="server" CssClass="form-message" Visible="false" />

        <asp:Panel ID="pnlConfirmation" runat="server" Visible="false">
            <div class="project-card">
                <div class="project-icon"><i class="fa-solid fa-briefcase"></i></div>
                <div class="project-info">
                    <h2><asp:Label ID="lblProjectTitle" runat="server" /></h2>
                    <p><asp:Label ID="lblProjectDescription" runat="server" /></p>
                    <div class="project-skills"><asp:Label ID="lblProjectSkills" runat="server" /></div>
                </div>
                <div class="project-meta">
                    <div><span>Budget</span><strong><asp:Label ID="lblProjectBudget" runat="server" /></strong></div>
                    <div><span>Deadline</span><strong><asp:Label ID="lblProjectDeadline" runat="server" /></strong></div>
                </div>
            </div>

            <div class="freelancer-card">
                <div class="freelancer-header">
                    <div class="freelancer-main">
                        <div class="freelancer-avatar"><i class="fa-solid fa-user"></i></div>
                        <div class="freelancer-info">
                            <h2><asp:HyperLink ID="lnkFreelancerProfile" runat="server" CssClass="profile-name-link" /></h2>
                            <span class="freelancer-title">Freelancer</span>
                        </div>
                    </div>
                </div>

                <div class="proposal-details">
                    <h3>Proposal Details</h3>
                    <div class="proposal-stats">
                        <div class="proposal-stat">
                            <div><span>Proposed Price</span><strong><asp:Label ID="lblProposedPrice" runat="server" /></strong></div>
                        </div>
                        <div class="proposal-stat">
                            <div><span>Delivery Time</span><strong><asp:Label ID="lblDeliveryTime" runat="server" /></strong></div>
                        </div>
                        <div class="proposal-stat">
                            <div><span>Proposal Status</span><strong><asp:Label ID="lblProposalStatus" runat="server" /></strong></div>
                        </div>
                    </div>
                    <div class="proposal-message">
                        <h3>Proposal Message</h3>
                        <div class="message-box"><asp:Label ID="lblCoverLetter" runat="server" /></div>
                    </div>

                    <div class="proposal-attachment">
                        <div class="proposal-attachment-icon"><i class="fa-solid fa-paperclip"></i></div>
                        <div class="proposal-attachment-info">
                            <h3>Proposal attachment</h3>
                            <asp:Label ID="lblProposalAttachmentDetails" runat="server"
                                CssClass="proposal-attachment-details" />
                        </div>
                        <asp:HyperLink ID="lnkProposalAttachment" runat="server"
                            CssClass="proposal-attachment-link"
                            Text="Open attachment"
                            Target="_blank"
                            Visible="false" />
                    </div>
                </div>
            </div>

            <div class="important-note">
                <div class="note-icon"><i class="fa-solid fa-circle-info"></i></div>
                <div>
                    <strong>What happens when you confirm?</strong>
                    <p>Confirm Hire takes you to the payment simulation. The freelancer is selected and the project starts only after you confirm the simulated payment.</p>
                </div>
            </div>

            <div class="hire-actions">
                <asp:HyperLink ID="lnkCancel" runat="server" CssClass="cancel-btn" Text="Cancel" />
                <asp:LinkButton ID="btnConfirmHire" runat="server" CssClass="confirm-hire-btn"
                    OnClick="btnConfirmHire_Click" CausesValidation="false">
                    <i class="fa-solid fa-check"></i> Confirm Hire
                </asp:LinkButton>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
