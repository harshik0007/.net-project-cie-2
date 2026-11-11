<%@ Page Title="Freelancer Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CIE_2_PROJECT.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/FProfile.css?v=8" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="profile-page">
        <asp:Panel ID="pnlProfileError" runat="server" CssClass="profile-error" Visible="false">
            <asp:Literal ID="litProfileError" runat="server" Mode="Encode" />
        </asp:Panel>

        <asp:Panel ID="pnlProfileContent" runat="server">
            <div class="page-header">
                <h1 class="page-title">Profile</h1>
                <p class="page-subtitle">Your profile details, skills, work history, and client feedback.</p>
            </div>

            <div class="profile-layout">
                <div class="profile-left">
                    <div class="profile-card">
                        <div class="profile-overview">
                            <div class="profile-photo-wrap">
                                <asp:Image ID="imgProfileImage" runat="server" CssClass="profile-photo profile-photo-image" AlternateText="Profile photo" Visible="false" />
                                <asp:Panel ID="pnlInitials" runat="server" CssClass="profile-photo"><asp:Literal ID="litInitials" runat="server" Mode="Encode" /></asp:Panel>
                            </div>
                            <div class="profile-details">
                                <div class="profile-name-row"><h2><asp:Literal ID="litFullName" runat="server" Mode="Encode" /></h2></div>
                                <div class="profile-role"><asp:Literal ID="litRole" runat="server" Mode="Encode" /></div>
                                <div class="profile-location">
                                    <span><i class="fa-solid fa-location-dot"></i><asp:Literal ID="litLocation" runat="server" Mode="Encode" /></span>
                                    <span class="profile-separator">&#8226;</span>
                                    <span><i class="fa-regular fa-clock"></i>Member since <asp:Literal ID="litMemberSince" runat="server" Mode="Encode" /></span>
                                </div>
                                <asp:Panel ID="pnlSlowWorkerBadge" runat="server" CssClass="slow-worker-badge" Visible="false">
                                    <i class="fa-solid fa-triangle-exclamation"></i>
                                    Slow worker <strong>&times;<asp:Literal ID="litSlowWorkerCount" runat="server" Mode="Encode" /></strong>
                                    <span>Complete on time to reduce this count.</span>
                                </asp:Panel>
                                <p class="profile-bio"><asp:Literal ID="litBio" runat="server" Mode="Encode" /></p>
                                <asp:LinkButton ID="btnEditProfile" runat="server" CssClass="edit-profile-button" CausesValidation="false" OnClick="btnEditProfile_Click"><i class="fa-regular fa-pen-to-square"></i> Edit Profile</asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnlEditProfile" runat="server" CssClass="profile-card profile-edit-card" Visible="false">
                        <h2 class="card-title">Edit profile</h2>
                        <div class="profile-edit-grid">
                            <label>Full name<asp:TextBox ID="txtFullName" runat="server" MaxLength="100" /></label>
                            <label>Location<asp:TextBox ID="txtLocation" runat="server" MaxLength="100" /></label>
                            <label>Education<asp:TextBox ID="txtEducation" runat="server" MaxLength="250" placeholder="School, degree, or qualification" /></label>
                            <label>Company / organization<asp:TextBox ID="txtOrganization" runat="server" MaxLength="150" /></label>
                            <label>Contact email<asp:TextBox ID="txtContactEmail" runat="server" MaxLength="100" TextMode="Email" /></label>
                            <label>Phone number<asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="30" TextMode="Phone" /></label>
                            <label class="profile-edit-wide">Portfolio or website<asp:TextBox ID="txtPortfolioUrl" runat="server" MaxLength="500" TextMode="Url" placeholder="https://example.com" /></label>
                            <label class="profile-edit-wide">About you<asp:TextBox ID="txtBio" runat="server" TextMode="MultiLine" MaxLength="500" Rows="3" /></label>
                            <label class="profile-edit-wide">Skills, separated by commas<asp:TextBox ID="txtSkills" runat="server" MaxLength="500" /></label>
                            <label>Preferred payout method<asp:DropDownList ID="ddlPreferredPaymentMethod" runat="server" CssClass="profile-edit-select"><asp:ListItem Text="Not set" Value="" /><asp:ListItem Text="UPI" Value="UPI" /><asp:ListItem Text="Bank transfer" Value="Bank transfer" /><asp:ListItem Text="PayPal" Value="PayPal" /><asp:ListItem Text="Other" Value="Other" /></asp:DropDownList></label>
                            <label>Payout reference<asp:TextBox ID="txtPaymentAccountLabel" runat="server" MaxLength="120" placeholder="UPI ID or masked account reference" /></label>
                            <label class="profile-edit-wide">Profile photo<asp:FileUpload ID="fuProfilePhoto" runat="server" CssClass="profile-file-input" accept=".jpg,.jpeg,.png" /></label>
                        </div>
                        <p class="profile-edit-note">Payment details are for this simulation. Use a UPI ID or masked account reference only; never enter a card number, password, or full bank credentials.</p>
                        <asp:Literal ID="litEditMessage" runat="server" Mode="Encode" />
                        <div class="profile-edit-actions">
                            <asp:Button ID="btnSaveProfile" runat="server" Text="Save changes" CssClass="profile-save-btn" OnClick="btnSaveProfile_Click" />
                            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="profile-cancel-btn" CausesValidation="false" OnClick="btnCancelEdit_Click" />
                        </div>
                    </asp:Panel>

                    <div class="profile-card skills-card">
                        <h2 class="card-title">Skills</h2>
                        <div class="skills-list">
                            <asp:Repeater ID="rptSkills" runat="server"><ItemTemplate><span class="skill-tag"><%#: Container.DataItem %></span></ItemTemplate></asp:Repeater>
                        </div>
                        <asp:Panel ID="pnlNoSkills" runat="server" CssClass="profile-empty-state" Visible="false">Add your skills to help clients understand what you do.</asp:Panel>
                    </div>

                    <div class="profile-card education-card">
                        <div class="profile-card-heading"><h2 class="card-title">Education &amp; Organization</h2><asp:LinkButton ID="btnAddEducation" runat="server" CssClass="profile-inline-edit" CausesValidation="false" OnClick="btnEditProfile_Click" Visible="false">Add details</asp:LinkButton></div>
                        <div class="profile-detail-row"><span>Education</span><strong><asp:Literal ID="litEducation" runat="server" Mode="Encode" /></strong></div>
                        <div class="profile-detail-row"><span>Organization</span><strong><asp:Literal ID="litOrganization" runat="server" Mode="Encode" /></strong></div>
                        <asp:HyperLink ID="lnkPortfolio" runat="server" CssClass="profile-portfolio-link" Text="View portfolio or website &#8594;" Visible="false" Target="_blank" />
                    </div>

                    <div class="profile-card experience-card">
                        <h2 class="card-title"><i class="fa-solid fa-briefcase card-title-icon"></i>Work History</h2>
                        <asp:Repeater ID="rptWorkHistory" runat="server">
                            <ItemTemplate>
                                <div class="timeline-item">
                                    <div class="timeline-date"><strong><%#: Eval("CompletedDate") %></strong><span>Net payout: <%#: Eval("Amount") %></span></div>
                                    <div class="timeline-content"><h3><%#: Eval("Title") %></h3><span class="company-link">Client: <a href='<%#: Eval("ClientProfileUrl") %>'><%#: Eval("ClientName") %></a></span></div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlNoWorkHistory" runat="server" CssClass="profile-empty-state" Visible="false">Completed contracts will appear here.</asp:Panel>
                    </div>
                </div>

                <div class="profile-right">
                    <div class="profile-card earnings-card">
                        <h2 class="side-card-title">Earnings</h2>
                        <div class="earnings-total"><asp:Literal ID="litReleasedEarnings" runat="server" Mode="Encode" /></div>
                        <div class="earnings-caption">Total received from completed work and cancellation payouts</div>
                        <div class="earnings-count"><i class="fa-solid fa-circle-check"></i> <asp:Literal ID="litReleasedPaymentCount" runat="server" Mode="Encode" /> received payments</div>
                    </div>

                    <div class="profile-card reputation-card">
                        <h2 class="side-card-title">Reputation Overview</h2>
                        <div class="reputation-score">
                            <div class="reputation-label">Average client rating</div>
                            <div class="score-row"><strong><asp:Literal ID="litAverageRating" runat="server" Mode="Encode" /></strong><span>/ 5.0</span><span class="stars"><asp:Literal ID="litRatingStars" runat="server" /></span></div>
                            <div class="reputation-status"><asp:Literal ID="litReputationStatus" runat="server" Mode="Encode" /></div>
                            <div class="reputation-caption">From <asp:Literal ID="litReviewCount" runat="server" Mode="Encode" /> client reviews</div>
                        </div>
                        <div class="reputation-meter">
                            <div class="meter-header"><span>Rating</span><strong><asp:Literal ID="litRatingPercent" runat="server" Mode="Encode" /></strong></div>
                            <div class="meter-track"><asp:Panel ID="pnlRatingFill" runat="server" CssClass="meter-fill" /></div>
                        </div>
                        <div class="reputation-stats">
                            <div class="reputation-stat"><i class="fa-regular fa-star"></i><strong><asp:Literal ID="litAverageRatingStat" runat="server" Mode="Encode" /></strong><span>Average Rating</span></div>
                            <div class="reputation-stat"><i class="fa-solid fa-briefcase"></i><strong><asp:Literal ID="litCompletedProjects" runat="server" Mode="Encode" /></strong><span>Projects Completed</span></div>
                            <div class="reputation-stat"><i class="fa-regular fa-thumbs-up"></i><strong><asp:Literal ID="litPositiveReviews" runat="server" Mode="Encode" /></strong><span>Positive Reviews</span></div>
                            <div class="reputation-stat"><i class="fa-regular fa-folder-open"></i><strong><asp:Literal ID="litActiveContracts" runat="server" Mode="Encode" /></strong><span>Active Contracts</span></div>
                        </div>
                    </div>

                    <div class="profile-card reviews-card">
                        <div class="side-card-heading"><h2 class="side-card-title">Client Reviews</h2><asp:HyperLink ID="lnkAllReviews" runat="server" CssClass="view-link" Text="View all" /></div>
                        <asp:Repeater ID="rptReviews" runat="server">
                            <ItemTemplate>
                                <div class="review-item">
                                    <div class="review-header">
                                        <div class="review-user"><div class="review-avatar"><%#: Eval("Initials") %></div><div><strong><a href='<%#: Eval("ReviewerProfileUrl") %>'><%#: Eval("ReviewerName") %></a></strong><div class="review-stars"><%#: Eval("Rating") %> / 5</div></div></div>
                                        <div class="review-rating"><span><%#: Eval("CreatedDate") %></span></div>
                                    </div>
                                    <p><%#: Eval("ReviewText") %></p>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlNoReviews" runat="server" CssClass="profile-empty-state" Visible="false">Client reviews will appear here after completed work.</asp:Panel>
                        <asp:HyperLink ID="lnkAllReviewsBottom" runat="server" CssClass="reviews-bottom-link" Text="View all reviews &#8594;" />
                    </div>

                    <asp:Panel ID="pnlPrivateContact" runat="server" CssClass="profile-card contact-card">
                        <h2 class="side-card-title">Contact &amp; Payout Preferences</h2>
                        <div class="contact-item"><i class="fa-regular fa-envelope"></i><span><asp:Literal ID="litEmail" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-regular fa-paper-plane"></i><span><asp:Literal ID="litContactEmail" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-solid fa-phone"></i><span><asp:Literal ID="litPhoneNumber" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-solid fa-wallet"></i><span><asp:Literal ID="litPreferredPaymentMethod" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-regular fa-credit-card"></i><span><asp:Literal ID="litPaymentAccountLabel" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-solid fa-at"></i><span>@<asp:Literal ID="litUsername" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-solid fa-location-dot"></i><span><asp:Literal ID="litContactLocation" runat="server" Mode="Encode" /></span></div>
                        <div class="contact-item"><i class="fa-regular fa-calendar-check"></i><span>Joined <asp:Literal ID="litJoinedDate" runat="server" Mode="Encode" /></span></div>
                        <asp:LinkButton ID="btnEditContactPayment" runat="server" CssClass="profile-inline-edit" CausesValidation="false" OnClick="btnEditProfile_Click">Edit contact and payout details</asp:LinkButton>
                    </asp:Panel>
                </div>
            </div>

            <asp:Panel ID="pnlProfileSecurity" runat="server" CssClass="profile-card profile-security-card">
                <div class="profile-security-copy">
                    <h2><i class="fa-solid fa-shield-halved"></i> Security &amp; account</h2>
                    <p>Keep your account access up to date.</p>
                </div>
                <a href="../Authentication/ForgotPassword.aspx" class="profile-password-link"><i class="fa-solid fa-key"></i> Reset password</a>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>
