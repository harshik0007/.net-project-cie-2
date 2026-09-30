<%@ Page Title="Client Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CIE_2_PROJECT.ClientProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/ClientProfile.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page">

        <div class="profile-header">

            <div class="profile-main">

                <div class="profile-avatar">
                    <i class="fa-regular fa-user"></i>
                </div>

                <div class="profile-heading">

                    <h1>XYZ ABC</h1>

                    <span class="profile-role">
                        <i class="fa-solid fa-building"></i>
                        Client
                    </span>

                    <div class="profile-location">
                        <i class="fa-solid fa-location-dot"></i>
                        Ahmedabad, India
                    </div>

                    <div class="member-since">
                        Member since January 2026
                    </div>

                </div>

            </div>

            <a href="#" class="edit-profile-btn">
                <i class="fa-solid fa-pen"></i>
                Edit Profile
            </a>

        </div>

        <div class="profile-grid">

            <div class="profile-card">

                <div class="card-title">
                    <i class="fa-regular fa-user"></i>
                    <h2>About</h2>
                </div>

                <div class="info-list">

                    <div class="info-item">
                        <span class="info-label">Full Name</span>
                        <span class="info-value">XYZ ABC</span>
                    </div>

                    <div class="info-item">
                        <span class="info-label">Company</span>
                        <span class="info-value">SkillLink Technologies</span>
                    </div>

                    <div class="info-item">
                        <span class="info-label">Industry</span>
                        <span class="info-value">Information Technology</span>
                    </div>

                    <div class="info-item">
                        <span class="info-label">Location</span>
                        <span class="info-value">Ahmedabad, India</span>
                    </div>

                </div>

                <div class="about-text">
                    Looking for talented freelancers to build quality digital
                    products and complete projects efficiently.
                </div>

            </div>

            <div class="profile-card">

                <div class="card-title">
                    <i class="fa-regular fa-id-card"></i>
                    <h2>Account Information</h2>
                </div>

                <div class="info-list">

                    <div class="info-item">
                        <span class="info-label">Email Address</span>
                        <span class="info-value">xyz@example.com</span>
                    </div>

                    <div class="info-item">
                        <span class="info-label">Phone Number</span>
                        <span class="info-value">+91 98765 43210</span>
                    </div>

                    <div class="info-item">
                        <span class="info-label">Username</span>
                        <span class="info-value">xyzabc</span>
                    </div>

                    <div class="info-item">
                        <span class="info-label">Account Status</span>

                        <span class="verified-status">
                            <i class="fa-solid fa-circle-check"></i>
                            Verified
                        </span>
                    </div>

                </div>

            </div>

        </div>

        <div class="profile-card statistics-card">

            <div class="card-title">
                <i class="fa-solid fa-chart-simple"></i>
                <h2>Client Statistics</h2>
            </div>

            <div class="statistics">

                <div class="stat-item">
                    <strong>8</strong>
                    <span>Projects Posted</span>
                </div>

                <div class="stat-item">
                    <strong>5</strong>
                    <span>Projects Completed</span>
                </div>

                <div class="stat-item">
                    <strong>2</strong>
                    <span>Active Projects</span>
                </div>

                <div class="stat-item">
                    <strong>₹72,500</strong>
                    <span>Total Spent</span>
                </div>

            </div>

        </div>

        <div class="profile-grid">

            <div class="profile-card reputation-card">

                <div class="card-title">
                    <i class="fa-regular fa-star"></i>
                    <h2>Client Reputation</h2>
                </div>

                <div class="rating-section">

                    <div class="rating-number">
                        4.8
                    </div>

                    <div class="rating-details">

                        <div class="stars">
                            <i class="fa-solid fa-star"></i>
                            <i class="fa-solid fa-star"></i>
                            <i class="fa-solid fa-star"></i>
                            <i class="fa-solid fa-star"></i>
                            <i class="fa-solid fa-star"></i>
                        </div>

                        <span>Based on 12 freelancer reviews</span>

                    </div>

                </div>

                <div class="reputation-info">

                    <div>
                        <strong>5</strong>
                        <span>Completed Projects</span>
                    </div>

                    <div>
                        <strong>12</strong>
                        <span>Reviews Received</span>
                    </div>

                </div>

            </div>

            <div class="profile-card payment-card">

                <div class="card-title">
                    <i class="fa-solid fa-wallet"></i>
                    <h2>Payment Summary</h2>
                </div>

                <div class="payment-summary">

                    <div class="payment-item">
                        <span>Total Spent</span>
                        <strong>₹72,500</strong>
                    </div>

                    <div class="payment-item">
                        <span>Escrow Held</span>
                        <strong>₹45,000</strong>
                    </div>

                    <div class="payment-item">
                        <span>Completed Payments</span>
                        <strong>₹62,500</strong>
                    </div>

                </div>

                <a href="Payments.aspx" class="payment-link">
                    View Payments
                    <i class="fa-solid fa-arrow-right"></i>
                </a>

            </div>

        </div>

        <div class="profile-card recent-projects-card">

            <div class="section-heading">

                <div class="card-title">
                    <i class="fa-solid fa-briefcase"></i>
                    <h2>Recent Projects</h2>
                </div>

                <a href="MyProjects.aspx" class="view-all">
                    View All
                    <i class="fa-solid fa-arrow-right"></i>
                </a>

            </div>

            <div class="recent-project">

                <div class="recent-project-info">

                    <strong>Unity 2D Game Development</strong>

                    <span>
                        Game Development
                    </span>

                </div>

                <span class="project-status status-progress">
                    In Progress
                </span>

                <a href="ProjectDetails.aspx?id=1" class="project-view">
                    View Project
                </a>

            </div>

            <div class="recent-project">

                <div class="recent-project-info">

                    <strong>Brand Logo Design</strong>

                    <span>
                        Graphic Design
                    </span>

                </div>

                <span class="project-status status-completed">
                    Completed
                </span>

                <a href="ProjectDetails.aspx?id=3" class="project-view">
                    View Project
                </a>

            </div>

            <div class="recent-project">

                <div class="recent-project-info">

                    <strong>Portfolio Website</strong>

                    <span>
                        Web Development
                    </span>

                </div>

                <span class="project-status status-completed">
                    Completed
                </span>

                <a href="ProjectDetails.aspx?id=5" class="project-view">
                    View Project
                </a>

            </div>

        </div>

        <div class="profile-card security-card">

            <div class="card-title">
                <i class="fa-solid fa-shield-halved"></i>
                <h2>Security &amp; Account</h2>
            </div>

            <div class="security-actions">

                <a href="../Authentication/ForgotPassword.aspx" class="security-btn">
                    <i class="fa-solid fa-lock"></i>
                    Change Password
                </a>

                <a href="../Authentication/Login.aspx" class="logout-btn">
                    <i class="fa-solid fa-right-from-bracket"></i>
                    Logout
                </a>

            </div>

        </div>

    </div>

</asp:Content>