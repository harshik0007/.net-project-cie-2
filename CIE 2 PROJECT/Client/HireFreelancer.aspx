<%@ Page Title="Hire Freelancer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HireFreelancer.aspx.cs" Inherits="CIE_2_PROJECT.HireFreelancer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/HireFreelancer.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="hire-page">

        <a href="ProjectProposal.aspx" class="back-link">
            <i class="fa-solid fa-arrow-left"></i>
            Back to Proposals
        </a>

        <div class="page-heading">
            <h1>Hire Freelancer</h1>
            <p>Review the proposal and confirm hiring this freelancer for your project.</p>
        </div>

        <div class="project-card">

            <div class="project-icon">
                <i class="fa-solid fa-gamepad"></i>
            </div>

            <div class="project-info">

                <h2>Unity 2D Game Development</h2>

                <p>
                    Create a 2D shooting game with multiple levels, enemies,
                    in-game store and leaderboard system.
                </p>

                <div class="project-skills">
                    <span>Unity</span>
                    <span>C#</span>
                    <span>Game Design</span>
                </div>

            </div>

            <div class="project-meta">

                <div>
                    <span>Budget</span>
                    <strong>₹15,000 - ₹25,000</strong>
                </div>

                <div>
                    <span>Deadline</span>
                    <strong>30 May, 2026</strong>
                </div>

            </div>

        </div>

        <div class="freelancer-card">

            <div class="freelancer-header">

                <div class="freelancer-main">

                    <div class="freelancer-avatar">
                        <i class="fa-solid fa-user"></i>
                        <span class="online-dot"></span>
                    </div>

                    <div class="freelancer-info">

                        <h2>Rahul Sharma</h2>

                        <span class="freelancer-title">
                            Full Stack Developer
                        </span>

                        <div class="freelancer-meta">

                            <span class="rating">
                                <i class="fa-solid fa-star"></i>
                                4.8
                                <span>(26 reviews)</span>
                            </span>

                            <span>
                                <i class="fa-solid fa-location-dot"></i>
                                India
                            </span>

                        </div>

                    </div>

                </div>

                <div class="freelancer-side">

                    <div>
                        <span>Skills</span>

                        <div class="skill-list">
                            <span>Unity</span>
                            <span>C#</span>
                            <span>2D Animation</span>
                        </div>
                    </div>

                    <div class="side-info">
                        <span>Member Since</span>
                        <strong>Jan 2024</strong>
                    </div>

                    <div class="side-info">
                        <span>Total Earnings</span>
                        <strong>₹1,25,000+</strong>
                    </div>

                </div>

            </div>

            <div class="proposal-details">

                <h3>Proposal Details</h3>

                <div class="proposal-stats">

                    <div class="proposal-stat">

                        <div class="stat-icon price-icon">
                            <i class="fa-solid fa-indian-rupee-sign"></i>
                        </div>

                        <div>
                            <span>Proposed Price</span>
                            <strong>₹20,000</strong>
                        </div>

                    </div>

                    <div class="proposal-stat">

                        <div class="stat-icon delivery-icon">
                            <i class="fa-regular fa-calendar"></i>
                        </div>

                        <div>
                            <span>Delivery Time</span>
                            <strong>15 days</strong>
                        </div>

                    </div>

                    <div class="proposal-stat">

                        <div class="stat-icon revision-icon">
                            <i class="fa-regular fa-file-lines"></i>
                        </div>

                        <div>
                            <span>Revision</span>
                            <strong>2 Revisions</strong>
                        </div>

                    </div>

                </div>

                <div class="proposal-message">

                    <h3>Proposal Message</h3>

                    <div class="message-box">

                        <i class="fa-solid fa-quote-left quote-left"></i>

                        <p>
                            Hi! I have 4+ years of experience in Unity game development.
                            I can build your 2D shooting game with clean code and smooth
                            gameplay. I will deliver high quality work with all the
                            features you mentioned.
                        </p>

                        <i class="fa-solid fa-quote-right quote-right"></i>

                    </div>

                </div>

            </div>

        </div>

        <div class="important-note">

            <div class="note-icon">
                <i class="fa-solid fa-circle-info"></i>
            </div>

            <div>
                <strong>Important Note</strong>

                <p>
                    Once you hire this freelancer, other proposals will be
                    declined automatically and your project will move to
                    <strong>In Progress</strong>.
                </p>
            </div>

        </div>

        <div class="payment-note">

            <div class="payment-note-icon">
                <i class="fa-solid fa-lock"></i>
            </div>

            <div>
                <strong>Payment &amp; Escrow</strong>

                <p>
                    The proposed amount will be secured in escrow after hiring.
                    Payment will be released to the freelancer only after the
                    submitted work is reviewed and the project is marked completed.
                </p>
            </div>

        </div>

        <div class="cancellation-note">

            <div class="cancellation-icon">
                <i class="fa-solid fa-circle-info"></i>
            </div>

            <div>
                <strong>Cancellation Policy</strong>

                <p>
                    If the project is cancelled after funds are placed in escrow,
                    a 25% cancellation deduction will apply. A portion of the
                    remaining amount may go to the freelancer based on work completed.
                </p>
            </div>

        </div>

        <div class="hire-actions">

            <a href="ProjectProposal.aspx" class="back-proposals-btn">
                <i class="fa-solid fa-arrow-left"></i>
                Back to Proposals
            </a>

            <div class="right-actions">

                <a href="ProjectProposal.aspx" class="cancel-btn">
                    Cancel
                </a>

                <asp:LinkButton
                    ID="btnConfirmHire"
                    runat="server"
                    CssClass="confirm-hire-btn"
                    OnClick="btnConfirmHire_Click">

                    <i class="fa-solid fa-check"></i>
                    Confirm Hire

                </asp:LinkButton>

            </div>

        </div>

        <div class="security-message">

            <i class="fa-solid fa-lock"></i>

            <span>
                Your payment is secured in escrow until the project is completed.
            </span>

        </div>

    </div>

</asp:Content>