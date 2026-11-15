<%@ Page Title="Review Submitted Work" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReviewSubmittedWork.aspx.cs" Inherits="CIE_2_PROJECT.ReviewSubmittedWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ReviewSubmittedWork.css?v=8" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="review-page">
        <div class="page-top">
            <div>
                <a href="MyProjects.aspx" class="back-link">
                    <i class="fa-solid fa-arrow-left"></i> Back to My Projects
                </a>
                <h1><asp:Label ID="lblPageHeading" runat="server" Text="Review Submitted Work" /></h1>
                <p><asp:Label ID="lblProjectTitle" runat="server" /></p>
            </div>
        </div>

        <asp:Label ID="lblActionMessage" runat="server" CssClass="action-message" Visible="false" />
        <asp:Panel ID="pnlReviewPage" runat="server" Visible="false">
            <div class="review-layout">
                <div class="review-main">
                    <div class="content-card">
                        <div class="submission-header">
                            <div class="freelancer-box">
                                <div class="freelancer-avatar"><asp:Label ID="lblFreelancerInitials" runat="server" /></div>
                                <div>
                                    <span class="small-label">Freelancer</span>
                                    <div class="freelancer-name-row"><strong><asp:HyperLink ID="lnkFreelancerProfile" runat="server" /></strong></div>
                                </div>
                            </div>
                            <div class="submission-status">
                                <span class="small-label">Contract status</span>
                                <span class="pending-badge"><asp:Label ID="lblContractStatus" runat="server" /></span>
                            </div>
                        </div>

                        <div class="content-divider"></div>

                        <div class="message-section">
                            <div class="section-icon"><i class="fa-regular fa-comment-dots"></i></div>
                            <div class="message-box">
                                <h3>Latest work submission</h3>
                                <p><asp:Label ID="lblSubmissionMessage" runat="server" /></p>
                                <p class="file-date">Submitted <asp:Label ID="lblSubmissionDate" runat="server" /></p>
                                <p class="file-date">Submission status: <asp:Label ID="lblSubmissionStatus" runat="server" /></p>
                                <asp:Panel ID="pnlRevisionFeedback" runat="server" Visible="false" CssClass="revision-feedback">
                                    <strong>Your revision feedback</strong>
                                    <p><asp:Label ID="lblRevisionFeedback" runat="server" /></p>
                                </asp:Panel>
                            </div>
                        </div>

                        <asp:Panel ID="pnlNoSubmission" runat="server" Visible="false">
                            <p>No work submission is available for this contract yet.</p>
                        </asp:Panel>

                        <div class="content-divider"></div>

                        <div class="files-section">
                            <div class="section-heading">
                                <i class="fa-solid fa-paperclip"></i>
                                <div><h3>Submission history and files</h3></div>
                            </div>
                            <asp:Repeater ID="rptSubmissions" runat="server" OnItemDataBound="RptSubmissions_ItemDataBound">
                                <ItemTemplate>
                                    <div class="submission-history-item">
                                        <h4>Submission <%#: Eval("SubmissionNumber") %> &#183; <%#: Eval("Status") %></h4>
                                        <p><%#: Eval("SubmittedAt") %></p>
                                        <p><%#: Eval("Message") %></p>
                                        <asp:Panel ID="pnlHistoryFeedback" runat="server" Visible='<%# !String.IsNullOrEmpty(Convert.ToString(Eval("Feedback"))) %>'>
                                            <p><strong>Client feedback:</strong> <%#: Eval("Feedback") %></p>
                                        </asp:Panel>
                                        <asp:Repeater ID="rptSubmissionFiles" runat="server">
                                            <ItemTemplate>
                                                <div class="file-item">
                                                    <div class="file-icon zip"><i class="fa-regular fa-file-lines"></i></div>
                                                    <div class="file-info">
                                                        <strong><%#: Eval("Name") %></strong>
                                                        <span><%#: Eval("Size") %></span>
                                                    </div>
                                                    <asp:LinkButton ID="btnDownloadFile" runat="server" CssClass="download-button"
                                                        CommandArgument='<%# Eval("FileKey") %>' OnClick="DownloadFile_Click"
                                                        CausesValidation="false" ToolTip="Download file">
                                                        <i class="fa-solid fa-download"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="content-divider"></div>

                        <div class="links-section">
                            <div class="section-heading"><i class="fa-solid fa-link"></i><div><h3>Project links</h3></div></div>
                            <div class="project-links-grid">
                                <div class="project-link-card">
                                    <span class="project-link-icon"><i class="fa-solid fa-arrow-up-right-from-square"></i></span>
                                    <div class="project-link-content">
                                        <span class="project-link-label">Live demo</span>
                                        <asp:HyperLink ID="lnkDemo" runat="server" CssClass="project-link-action" Target="_blank" rel="noopener noreferrer" Visible="false" Text="Open live demo" />
                                        <asp:Label ID="lblNoDemo" runat="server" CssClass="project-link-empty" Text="No demo link included" />
                                    </div>
                                </div>
                                <div class="project-link-card">
                                    <span class="project-link-icon"><i class="fa-brands fa-github"></i></span>
                                    <div class="project-link-content">
                                        <span class="project-link-label">Source repository</span>
                                        <asp:HyperLink ID="lnkGithub" runat="server" CssClass="project-link-action" Target="_blank" rel="noopener noreferrer" Visible="false" Text="Open source repository" />
                                        <asp:Label ID="lblNoGithub" runat="server" CssClass="project-link-empty" Text="No repository link included" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="content-divider"></div>

                        <asp:Panel ID="pnlReviewActions" runat="server" Visible="false">
                            <div class="review-section">
                                <div class="section-heading">
                                    <i class="fa-regular fa-comment"></i>
                                    <div><h3>Review the submission</h3><p>Written feedback is required if you request a revision.</p></div>
                                </div>
                                <asp:TextBox ID="txtRevisionFeedback" runat="server" TextMode="MultiLine"
                                    CssClass="review-textbox" MaxLength="2000" />
                                <asp:RequiredFieldValidator ID="rfvRevisionFeedback" runat="server"
                                    ControlToValidate="txtRevisionFeedback" ValidationGroup="Revision"
                                    EnableClientScript="false" CssClass="validation-error"
                                    ErrorMessage="Write feedback before requesting a revision." />
                            </div>
                            <div class="action-area">
                                <asp:Button ID="btnRequestRevision" runat="server" Text="Request Revision"
                                    CssClass="revision-button" ValidationGroup="Revision" OnClick="RequestRevision_Click" />
                                <asp:Button ID="btnApproveWork" runat="server" Text="Approve &amp; Release Payment"
                                    CssClass="approve-button" CausesValidation="false" OnClick="ApproveWork_Click" />
                            </div>
                            <p class="file-date">Approving completes the contract and automatically releases the held simulated payment.</p>
                        </asp:Panel>

                        <asp:Panel ID="pnlLeaveReview" runat="server" Visible="false" CssClass="review-section">
                            <div class="section-heading review-form-heading">
                                <i class="fa-regular fa-star"></i>
                                <div><h3>Rate this freelancer</h3><p>Share helpful feedback about the completed work.</p></div>
                            </div>
                            <div class="review-form-fields">
                                <div class="review-field">
                                    <asp:Label ID="lblRatingField" runat="server" AssociatedControlID="ddlRating" CssClass="review-field-label" Text="Your rating" />
                                    <asp:DropDownList ID="ddlRating" runat="server" CssClass="review-rating-select">
                                        <asp:ListItem Text="Choose a rating" Value="" />
                                        <asp:ListItem Text="5 - Excellent" Value="5" />
                                        <asp:ListItem Text="4 - Good" Value="4" />
                                        <asp:ListItem Text="3 - Satisfactory" Value="3" />
                                        <asp:ListItem Text="2 - Needs improvement" Value="2" />
                                        <asp:ListItem Text="1 - Poor" Value="1" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvRating" runat="server" ControlToValidate="ddlRating"
                                        InitialValue="" ValidationGroup="Review" EnableClientScript="false"
                                        CssClass="validation-error" ErrorMessage="Choose a rating." />
                                </div>
                                <div class="review-field">
                                    <asp:Label ID="lblReviewField" runat="server" AssociatedControlID="txtReview" CssClass="review-field-label" Text="Written feedback" />
                                    <asp:TextBox ID="txtReview" runat="server" TextMode="MultiLine" CssClass="review-feedback-textarea"
                                        MaxLength="2000" Rows="4" placeholder="What went well? Share clear, constructive feedback." />
                                    <asp:RequiredFieldValidator ID="rfvReview" runat="server" ControlToValidate="txtReview"
                                        ValidationGroup="Review" EnableClientScript="false"
                                        CssClass="validation-error" ErrorMessage="Write a review before submitting." />
                                </div>
                            </div>
                            <div class="review-form-footer">
                                <span>Your review will be visible to the freelancer.</span>
                                <asp:Button ID="btnSubmitReview" runat="server" Text="Submit review"
                                    CssClass="submit-review-button" ValidationGroup="Review" OnClick="SubmitReview_Click" />
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlReviewSubmitted" runat="server" Visible="false" CssClass="review-success-state">
                            <span class="review-success-icon"><i class="fa-solid fa-check"></i></span>
                            <div><strong>Review submitted</strong><span>Your feedback has been saved for this contract.</span></div>
                        </asp:Panel>
                    </div>
                </div>

                <aside class="review-sidebar">
                    <div class="summary-card">
                        <h2>Project Summary</h2>
                        <div class="summary-project"><div class="project-icon"><i class="fa-solid fa-briefcase"></i></div>
                            <strong><asp:Label ID="lblSummaryProject" runat="server" /></strong></div>
                        <div class="summary-row"><span>Freelancer</span><strong><asp:HyperLink ID="lnkSummaryFreelancerProfile" runat="server" /></strong></div>
                        <asp:HyperLink ID="lnkMessageFreelancer" runat="server" CssClass="review-message-button">
                            <i class="fa-regular fa-comments"></i> Message freelancer
                        </asp:HyperLink>
                        <div class="summary-row"><span>Contract amount</span><strong class="price"><asp:Label ID="lblAgreedAmount" runat="server" /></strong></div>
                        <asp:Panel ID="pnlStandardPaymentBreakdown" runat="server" CssClass="payment-breakdown">
                            <div class="summary-row"><span>GST (<asp:Label ID="lblGstRate" runat="server" />% demo)</span><strong><asp:Label ID="lblGstAmount" runat="server" /></strong></div>
                            <div class="summary-row"><span>Client platform fee (<asp:Label ID="lblClientFeeRate" runat="server" />%)</span><strong><asp:Label ID="lblClientFeeAmount" runat="server" /></strong></div>
                            <div class="summary-row"><span>Client total</span><strong class="price"><asp:Label ID="lblClientTotal" runat="server" /></strong></div>
                            <div class="summary-row"><span>Freelancer fee (<asp:Label ID="lblFreelancerFeeRate" runat="server" />%)</span><strong><asp:Label ID="lblFreelancerFeeAmount" runat="server" /></strong></div>
                            <div class="summary-row"><span>Freelancer payout after fee</span><strong><asp:Label ID="lblFreelancerPayout" runat="server" /></strong></div>
                        </asp:Panel>
                        <asp:Panel ID="pnlCancellationBreakdown" runat="server" CssClass="payment-breakdown cancellation-breakdown" Visible="false">
                            <h3>Cancellation settlement</h3>
                            <div class="summary-row"><span>Client paid total</span><strong><asp:Label ID="lblClientPaidTotal" runat="server" /></strong></div>
                            <div class="summary-row settlement-total"><span>Total refund to client</span><strong class="price"><asp:Label ID="lblClientRefundTotal" runat="server" /></strong></div>
                            <asp:Panel ID="pnlCancellationComponents" runat="server" CssClass="settlement-components">
                                <div class="summary-row"><span>Contract refund (75%)</span><strong><asp:Label ID="lblClientContractRefund" runat="server" /></strong></div>
                                <div class="summary-row"><span>GST returned</span><strong><asp:Label ID="lblGstReturned" runat="server" /></strong></div>
                                <div class="summary-row"><span>Client platform fee returned</span><strong><asp:Label ID="lblClientFeeReturned" runat="server" /></strong></div>
                            </asp:Panel>
                            <asp:Panel ID="pnlCancellationAllocation" runat="server" CssClass="settlement-components">
                                <div class="summary-row"><span>Freelancer cancellation payout (10%)</span><strong><asp:Label ID="lblCancellationFreelancerPayout" runat="server" /></strong></div>
                                <div class="summary-row"><span>Platform cancellation share (15%)</span><strong><asp:Label ID="lblPlatformCancellationShare" runat="server" /></strong></div>
                            </asp:Panel>
                            <asp:Label ID="lblLegacyRefundNote" runat="server" CssClass="settlement-note" Visible="false" />
                            <asp:Label ID="lblCancellationPolicyNote" runat="server" CssClass="settlement-note" Text="The cancellation split is calculated from the contract amount. The regular freelancer fee is not charged again." />
                        </asp:Panel>
                        <div class="summary-row"><span>Deadline</span><strong><asp:Label ID="lblDeadline" runat="server" /></strong></div>
                        <div class="summary-row"><span>Contract created</span><strong><asp:Label ID="lblCreatedAt" runat="server" /></strong></div>
                        <div class="summary-row"><span>Payment status</span><strong><asp:Label ID="lblPaymentStatus" runat="server" /></strong></div>
                    </div>
                </aside>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
