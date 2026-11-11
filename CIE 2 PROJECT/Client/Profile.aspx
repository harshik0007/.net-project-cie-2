<%@ Page Title="Client Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CIE_2_PROJECT.ClientProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ClientProfile.css?v=7" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="profile-page">
        <asp:Panel ID="pnlProfileError" runat="server" CssClass="profile-error" Visible="false">
            <asp:Literal ID="litProfileError" runat="server" Mode="Encode" />
        </asp:Panel>

        <asp:Panel ID="pnlProfileContent" runat="server">
            <div class="profile-header">
                <div class="profile-main">
                    <asp:Image ID="imgProfileImage" runat="server" CssClass="profile-avatar profile-avatar-image" AlternateText="Profile photo" Visible="false" />
                    <asp:Panel ID="pnlAvatarInitials" runat="server" CssClass="profile-avatar"><asp:Literal ID="litInitials" runat="server" Mode="Encode" /></asp:Panel>
                    <div class="profile-heading">
                        <h1><asp:Literal ID="litFullName" runat="server" Mode="Encode" /></h1>
                        <span class="profile-role"><i class="fa-solid fa-building"></i><asp:Literal ID="litRole" runat="server" Mode="Encode" /></span>
                        <div class="profile-location"><i class="fa-solid fa-location-dot"></i><asp:Literal ID="litLocation" runat="server" Mode="Encode" /></div>
                        <div class="member-since">Member since <asp:Literal ID="litMemberSince" runat="server" Mode="Encode" /></div>
                    </div>
                </div>
                <asp:LinkButton ID="btnEditProfile" runat="server" CssClass="edit-profile-btn" CausesValidation="false" OnClick="btnEditProfile_Click">
                    <i class="fa-solid fa-pen"></i> Edit Profile
                </asp:LinkButton>
            </div>

            <asp:Panel ID="pnlEditProfile" runat="server" CssClass="profile-card profile-edit-form" Visible="false">
                <h2>Edit profile</h2>
                <div class="profile-edit-grid">
                    <label>Full name<asp:TextBox ID="txtFullName" runat="server" MaxLength="100" /></label>
                    <label>Location<asp:TextBox ID="txtLocation" runat="server" MaxLength="100" /></label>
                    <label>Education<asp:TextBox ID="txtEducation" runat="server" MaxLength="250" placeholder="School, degree, or qualification" /></label>
                    <label>Company / organization<asp:TextBox ID="txtOrganization" runat="server" MaxLength="150" /></label>
                    <label>Contact email<asp:TextBox ID="txtContactEmail" runat="server" MaxLength="100" TextMode="Email" /></label>
                    <label>Phone number<asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="30" TextMode="Phone" /></label>
                    <label class="profile-edit-wide">Website or portfolio<asp:TextBox ID="txtPortfolioUrl" runat="server" MaxLength="500" TextMode="Url" placeholder="https://example.com" /></label>
                    <label class="profile-edit-wide">About you<asp:TextBox ID="txtBio" runat="server" TextMode="MultiLine" MaxLength="500" Rows="3" /></label>
                    <label>Preferred payment method<asp:DropDownList ID="ddlPreferredPaymentMethod" runat="server" CssClass="profile-edit-select"><asp:ListItem Text="Not set" Value="" /><asp:ListItem Text="UPI" Value="UPI" /><asp:ListItem Text="Bank transfer" Value="Bank transfer" /><asp:ListItem Text="PayPal" Value="PayPal" /><asp:ListItem Text="Other" Value="Other" /></asp:DropDownList></label>
                    <label>Payment reference<asp:TextBox ID="txtPaymentAccountLabel" runat="server" MaxLength="120" placeholder="UPI ID or masked account reference" /></label>
                    <label class="profile-edit-wide">Profile photo<asp:FileUpload ID="fuProfilePhoto" runat="server" CssClass="profile-file-input" accept=".jpg,.jpeg,.png" /></label>
                </div>
                <p class="profile-edit-note">Payment details are for this simulation. Use a UPI ID or masked account reference only; never enter a card number, password, or full bank credentials.</p>
                <asp:Literal ID="litEditMessage" runat="server" Mode="Encode" />
                <div class="profile-edit-actions">
                    <asp:Button ID="btnSaveProfile" runat="server" Text="Save changes" CssClass="profile-save-btn" OnClick="btnSaveProfile_Click" />
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" CssClass="profile-cancel-btn" CausesValidation="false" OnClick="btnCancelEdit_Click" />
                </div>
            </asp:Panel>

            <div class="profile-grid">
                <div class="profile-card">
                    <div class="card-title"><i class="fa-regular fa-user"></i><h2>About</h2></div>
                    <div class="info-list">
                        <div class="info-item"><span class="info-label">Full Name</span><span class="info-value"><asp:Literal ID="litAboutName" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Location</span><span class="info-value"><asp:Literal ID="litAboutLocation" runat="server" Mode="Encode" /></span></div>
                    </div>
                    <div class="about-text"><asp:Literal ID="litBio" runat="server" Mode="Encode" /></div>
                </div>

                <asp:Panel ID="pnlAccountInfo" runat="server" CssClass="profile-card">
                    <div class="card-title"><i class="fa-regular fa-id-card"></i><h2>Account Information</h2></div>
                    <div class="info-list">
                        <div class="info-item"><span class="info-label">Email Address</span><span class="info-value"><asp:Literal ID="litEmail" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Username</span><span class="info-value"><asp:Literal ID="litUsername" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Account Type</span><span class="info-value"><asp:Literal ID="litAccountRole" runat="server" Mode="Encode" /></span></div>
                    </div>
                </asp:Panel>
            </div>

            <asp:Panel ID="pnlProfileExtraGrid" runat="server" CssClass="profile-grid profile-extra-grid">
                <div class="profile-card profile-extra-card">
                    <div class="card-title"><i class="fa-solid fa-graduation-cap"></i><h2>Education &amp; Organization</h2></div>
                    <div class="info-list">
                        <div class="info-item"><span class="info-label">Education</span><span class="info-value"><asp:Literal ID="litEducation" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Organization</span><span class="info-value"><asp:Literal ID="litOrganization" runat="server" Mode="Encode" /></span></div>
                    </div>
                    <asp:HyperLink ID="lnkPortfolio" runat="server" CssClass="profile-portfolio-link" Text="Visit website or portfolio &#8594;" Visible="false" Target="_blank" />
                    <asp:LinkButton ID="btnAddEducation" runat="server" CssClass="profile-inline-edit" CausesValidation="false" OnClick="btnEditProfile_Click" Visible="false">Add or edit profile details</asp:LinkButton>
                </div>

                <asp:Panel ID="pnlPrivateContactAndPayment" runat="server" CssClass="profile-card profile-extra-card">
                    <div class="card-title"><i class="fa-regular fa-address-card"></i><h2>Contact &amp; Payment Preferences</h2></div>
                    <div class="info-list">
                        <div class="info-item"><span class="info-label">Contact email</span><span class="info-value"><asp:Literal ID="litContactEmailValue" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Phone number</span><span class="info-value"><asp:Literal ID="litPhoneNumberValue" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Payment method</span><span class="info-value"><asp:Literal ID="litPreferredPaymentMethod" runat="server" Mode="Encode" /></span></div>
                        <div class="info-item"><span class="info-label">Payment reference</span><span class="info-value"><asp:Literal ID="litPaymentAccountLabel" runat="server" Mode="Encode" /></span></div>
                    </div>
                    <asp:LinkButton ID="btnEditContactPayment" runat="server" CssClass="profile-inline-edit" CausesValidation="false" OnClick="btnEditProfile_Click">Edit contact and payment details</asp:LinkButton>
                </asp:Panel>
            </asp:Panel>

            <div class="profile-card statistics-card">
                <div class="card-title"><i class="fa-solid fa-chart-simple"></i><h2>Client Statistics</h2></div>
                <div class="statistics">
                    <div class="stat-item"><strong><asp:Literal ID="litProjectsPosted" runat="server" Mode="Encode" /></strong><span>Projects Posted</span></div>
                    <div class="stat-item"><strong><asp:Literal ID="litProjectsCompleted" runat="server" Mode="Encode" /></strong><span>Projects Completed</span></div>
                    <div class="stat-item"><strong><asp:Literal ID="litActiveProjects" runat="server" Mode="Encode" /></strong><span>Active Projects</span></div>
                    <div class="stat-item"><strong><asp:Literal ID="litTotalSpent" runat="server" Mode="Encode" /></strong><span>Payments Released</span></div>
                </div>
            </div>

            <div class="profile-grid">
                <div class="profile-card reputation-card">
                    <div class="card-title"><i class="fa-regular fa-star"></i><h2>Client Reputation</h2></div>
                    <div class="rating-section">
                        <div class="rating-number"><asp:Literal ID="litAverageRating" runat="server" Mode="Encode" /></div>
                        <div class="rating-details"><div class="stars"><asp:Literal ID="litRatingStars" runat="server" /></div><span>Based on <asp:Literal ID="litReviewCount" runat="server" Mode="Encode" /> freelancer reviews</span></div>
                    </div>
                    <div class="reputation-info">
                        <div><strong><asp:Literal ID="litCompletedCount" runat="server" Mode="Encode" /></strong><span>Completed Projects</span></div>
                        <div><strong><asp:Literal ID="litReviewCountSummary" runat="server" Mode="Encode" /></strong><span>Reviews Received</span></div>
                    </div>
                </div>

                <div class="profile-card client-reviews-card">
                    <div class="section-heading">
                        <div class="card-title"><i class="fa-regular fa-star"></i><h2>Freelancer Reviews</h2></div>
                        <asp:HyperLink ID="lnkAllClientReviews" runat="server" CssClass="view-all" Text="See all reviews &#8594;" />
                    </div>
                    <asp:Panel ID="pnlNoClientReviews" runat="server" CssClass="profile-empty-state" Visible="false">Reviews from freelancers will appear here after completed contracts.</asp:Panel>
                    <asp:Repeater ID="rptClientReviews" runat="server">
                        <ItemTemplate>
                            <article class="client-review-item">
                                <div class="client-review-heading">
                                    <div><strong><a href='<%#: Eval("ReviewerProfileUrl") %>'><%#: Eval("ReviewerName") %></a></strong><span><%#: Eval("CreatedDate") %></span></div>
                                    <span class="client-review-rating"><i class="fa-solid fa-star"></i> <%#: Eval("Rating") %> / 5</span>
                                </div>
                                <p><%#: Eval("ReviewText") %></p>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <asp:Panel ID="pnlPaymentSummary" runat="server" CssClass="profile-card payment-card">
                    <div class="card-title"><i class="fa-solid fa-wallet"></i><h2>Payment Summary</h2></div>
                    <div class="payment-summary">
                        <div class="payment-item"><span>Total Released</span><strong><asp:Literal ID="litReleasedAmount" runat="server" Mode="Encode" /></strong></div>
                        <div class="payment-item"><span>Escrow Held</span><strong><asp:Literal ID="litEscrowHeld" runat="server" Mode="Encode" /></strong></div>
                        <div class="payment-item"><span>Payments Made</span><strong><asp:Literal ID="litPaymentCount" runat="server" Mode="Encode" /></strong></div>
                    </div>
                    <a href="Payments.aspx" class="payment-link">View Payments <i class="fa-solid fa-arrow-right"></i></a>
                </asp:Panel>
            </div>

            <div class="profile-card recent-projects-card">
                <div class="section-heading">
                    <div class="card-title"><i class="fa-solid fa-briefcase"></i><h2>Recent Projects</h2></div>
                    <a href="MyProjects.aspx" class="view-all">View All <i class="fa-solid fa-arrow-right"></i></a>
                </div>
                <asp:Panel ID="pnlNoProjects" runat="server" CssClass="profile-empty-state" Visible="false">No projects have been posted yet.</asp:Panel>
                <asp:Repeater ID="rptRecentProjects" runat="server">
                    <ItemTemplate>
                        <div class="recent-project">
                            <div class="recent-project-info"><strong><%#: Eval("Title") %></strong><span><%#: Eval("Category") %></span></div>
                            <span class='project-status <%# Eval("StatusClass") %>'><%#: Eval("Status") %></span>
                            <a href='<%#: Eval("Url") %>' class="project-view">View Project</a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <asp:Panel ID="pnlSecurityCard" runat="server" CssClass="profile-card profile-security-card">
                <div class="profile-security-copy">
                    <h2><i class="fa-solid fa-shield-halved"></i> Security &amp; account</h2>
                    <p>Keep your account access up to date.</p>
                </div>
                <a href="../Authentication/ForgotPassword.aspx" class="profile-password-link"><i class="fa-solid fa-key"></i> Reset password</a>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>
