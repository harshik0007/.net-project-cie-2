<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="CIE_2_PROJECT.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/FProfile.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="profile-page">

        <div class="page-header">
            <h1 class="page-title">Profile</h1>
            <p class="page-subtitle">
                Manage your profile information and showcase your skills to clients.
            </p>
        </div>


        <div class="profile-layout">

            <div class="profile-left">

                <div class="profile-card">

                    <div class="profile-overview">

                        <div class="profile-photo">
                            <i class="fa-regular fa-user"></i>
                        </div>

                        <div class="profile-details">

                            <div class="profile-name-row">
                                <h2>xyz abc</h2>
                                <i class="fa-solid fa-circle-check verified-icon"></i>
                            </div>

                            <div class="profile-role">
                                Freelancer
                            </div>

                            <div class="profile-location">

                                <span>
                                    <i class="fa-solid fa-location-dot"></i>
                                    India, Gujarat
                                </span>

                                <span class="profile-separator">•</span>

                                <span>
                                    <i class="fa-regular fa-clock"></i>
                                    Member since May 2026
                                </span>

                            </div>

                            <p class="profile-bio">
                                Passionate full-stack developer with experience building web applications
                                using .NET, JavaScript, and modern technologies. I love turning ideas into
                                real products.
                            </p>

                            <a href="#" class="edit-profile-button">
                                <i class="fa-regular fa-pen-to-square"></i>
                                Edit Profile
                            </a>

                        </div>

                    </div>

                </div>


                <div class="profile-card skills-card">

                    <h2 class="card-title">Skills</h2>

                    <div class="skills-list">

                        <span class="skill-tag">C# / .NET</span>
                        <span class="skill-tag">ASP.NET Core</span>
                        <span class="skill-tag">JavaScript</span>
                        <span class="skill-tag">SQL Server</span>
                        <span class="skill-tag">HTML</span>
                        <span class="skill-tag">CSS</span>
                        <span class="skill-tag">Bootstrap</span>
                        <span class="skill-tag">React.js</span>
                        <span class="skill-tag">Git</span>
                        <span class="skill-tag">REST API</span>

                    </div>

                </div>


                <div class="profile-card experience-card">

                    <h2 class="card-title">
                        <i class="fa-solid fa-briefcase card-title-icon"></i>
                        Experience
                    </h2>

                    <div class="timeline-item">

                        <div class="timeline-date">
                            <strong>2023 - Present</strong>
                            <span>2+ Years</span>
                        </div>

                        <div class="timeline-content">

                            <h3>Full Stack Developer</h3>

                            <a href="#" class="company-link">
                                Freelancer
                            </a>

                            <p>
                                Building web applications using ASP.NET Core, JavaScript,
                                SQL Server and other modern technologies.
                            </p>

                        </div>

                    </div>


                    <div class="timeline-item">

                        <div class="timeline-date">
                            <strong>2021 - 2023</strong>
                            <span>1.5 Years</span>
                        </div>

                        <div class="timeline-content">

                            <h3>Web Developer</h3>

                            <a href="#" class="company-link">
                                Tech Solutions
                            </a>

                            <p>
                                Developed and maintained client websites and internal web applications.
                            </p>

                        </div>

                    </div>

                </div>


                <div class="profile-card education-card">

                    <h2 class="card-title">
                        <i class="fa-solid fa-graduation-cap card-title-icon"></i>
                        Education
                    </h2>

                    <div class="timeline-item">

                        <div class="timeline-date">
                            <strong>2020 - 2024</strong>
                        </div>

                        <div class="timeline-content">

                            <h3>Bachelor of Computer Applications (BCA)</h3>

                            <a href="#" class="company-link">
                                Gujarat University
                            </a>

                            <p>
                                CGPA: 8.2 / 10
                            </p>

                        </div>

                    </div>


                    <div class="timeline-item">

                        <div class="timeline-date">
                            <strong>2018 - 2020</strong>
                        </div>

                        <div class="timeline-content">

                            <h3>Higher Secondary (12th)</h3>

                            <a href="#" class="company-link">
                                GSEB
                            </a>

                            <p>
                                Percentage: 72.40%
                            </p>

                        </div>

                    </div>

                </div>


                <div class="profile-card payment-card">

                    <div class="card-heading-row">

                        <h2 class="card-title">
                            <i class="fa-solid fa-building-columns card-title-icon"></i>
                            Payment Details
                        </h2>

                        <a href="#" class="edit-link">
                            <i class="fa-regular fa-pen-to-square"></i>
                            Edit
                        </a>

                    </div>

                    <div class="payment-grid">

                        <div class="payment-field">

                            <label>UPI ID</label>

                            <div class="payment-value">
                                user@bankname
                            </div>

                        </div>

                        <div class="payment-field">

                            <label>Bank Account Number</label>

                            <div class="payment-value">
                                •••• •••• •••• 4582
                            </div>

                        </div>

                        <div class="payment-field">

                            <label>Account Holder Name</label>

                            <div class="payment-value">
                                xyz abc
                            </div>

                        </div>

                        <div class="payment-field">

                            <label>IFSC Code</label>

                            <div class="payment-value">
                                HDFC0001234
                            </div>

                        </div>

                    </div>

                </div>

            </div>


            <div class="profile-right">

                <div class="profile-card reputation-card">

                    <h2 class="side-card-title">
                        Reputation Overview
                    </h2>

                    <div class="reputation-score">

                        <div class="reputation-label">
                            Reputation Score
                        </div>

                        <div class="score-row">

                            <strong>4.8</strong>

                            <span>/ 5.0</span>

                            <span class="stars">
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                                <i class="fa-solid fa-star"></i>
                            </span>

                        </div>

                        <div class="reputation-status">
                            Excellent
                        </div>

                        <div class="reputation-caption">
                            Top Rated Freelancer
                        </div>

                    </div>


                    <div class="reputation-meter">

                        <div class="meter-header">
                            <span>Reputation Meter</span>
                            <strong>96%</strong>
                        </div>

                        <div class="meter-track">
                            <div class="meter-fill"></div>
                        </div>

                    </div>


                    <div class="reputation-stats">

                        <div class="reputation-stat">
                            <i class="fa-regular fa-star"></i>
                            <strong>4.8</strong>
                            <span>Average Rating</span>
                        </div>

                        <div class="reputation-stat">
                            <i class="fa-solid fa-briefcase"></i>
                            <strong>12</strong>
                            <span>Projects Completed</span>
                        </div>

                        <div class="reputation-stat">
                            <i class="fa-regular fa-thumbs-up"></i>
                            <strong>10</strong>
                            <span>Positive Reviews</span>
                        </div>

                        <div class="reputation-stat">
                            <i class="fa-regular fa-clock"></i>
                            <strong>100%</strong>
                            <span>On-time Completion</span>
                        </div>

                    </div>

                </div>


                <div class="profile-card reviews-card">

                    <div class="side-card-heading">

                        <h2 class="side-card-title">
                            Client Reviews
                        </h2>

                        <a href="#" class="view-link">
                            View All Reviews
                        </a>

                    </div>


                    <div class="review-item">

                        <div class="review-header">

                            <div class="review-user">

                                <div class="review-avatar avatar-green">
                                    GS
                                </div>

                                <div>
                                    <strong>Game Studio</strong>

                                    <div class="review-stars">
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                    </div>
                                </div>

                            </div>

                            <div class="review-rating">
                                <strong>5.0</strong>
                                <span>May 25, 2026</span>
                            </div>

                        </div>

                        <p>
                            Excellent work! Delivered everything on time and communication
                            was top-notch.
                        </p>

                    </div>


                    <div class="review-item">

                        <div class="review-header">

                            <div class="review-user">

                                <div class="review-avatar avatar-blue">
                                    JD
                                </div>

                                <div>
                                    <strong>dhruv</strong>

                                    <div class="review-stars">
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                    </div>
                                </div>

                            </div>

                            <div class="review-rating">
                                <strong>5.0</strong>
                                <span>May 15, 2026</span>
                            </div>

                        </div>

                        <p>
                            Very professional and skilled. Would love to work again.
                        </p>

                    </div>


                    <div class="review-item">

                        <div class="review-header">

                            <div class="review-user">

                                <div class="review-avatar avatar-purple">
                                    CB
                                </div>

                                <div>
                                    <strong>College Blog</strong>

                                    <div class="review-stars">
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-solid fa-star"></i>
                                        <i class="fa-regular fa-star"></i>
                                    </div>
                                </div>

                            </div>

                            <div class="review-rating">
                                <strong>4.5</strong>
                                <span>May 05, 2026</span>
                            </div>

                        </div>

                        <p>
                            Good work and on-time delivery. Highly recommended.
                        </p>

                    </div>


                    <a href="#" class="reviews-bottom-link">
                        View All Reviews
                        <i class="fa-solid fa-arrow-right"></i>
                    </a>

                </div>


                <div class="profile-card completed-card">

                    <div class="side-card-heading">

                        <h2 class="side-card-title">
                            <i class="fa-regular fa-folder"></i>
                            Completed Projects
                        </h2>

                        <a href="#" class="view-link">
                            View All Projects
                            <i class="fa-solid fa-arrow-right"></i>
                        </a>

                    </div>


                    <div class="completed-project">

                        <div class="completed-icon game-completed">
                            <i class="fa-solid fa-gamepad"></i>
                        </div>

                        <div class="completed-details">

                            <strong>
                                2D Adventure Game
                            </strong>

                            <span>
                                Game Studio
                            </span>

                        </div>

                        <span class="completed-badge">
                            Completed
                        </span>

                    </div>


                    <div class="completed-project">

                        <div class="completed-icon blog-completed">
                            <i class="fa-regular fa-file-lines"></i>
                        </div>

                        <div class="completed-details">

                            <strong>
                                College Blog Platform
                            </strong>

                            <span>
                                College Blog
                            </span>

                        </div>

                        <span class="completed-badge">
                            Completed
                        </span>

                    </div>

                </div>


                <div class="profile-card contact-card">

                    <h2 class="side-card-title">
                        Contact Information
                    </h2>

                    <div class="contact-item">
                        <i class="fa-regular fa-envelope"></i>
                        <span>abcxyz@gmail.com</span>
                    </div>

                    <div class="contact-item">
                        <i class="fa-solid fa-phone"></i>
                        <span>+91 98765 43210</span>
                    </div>

                    <div class="contact-item">
                        <i class="fa-solid fa-location-dot"></i>
                        <span>Gujarat, India</span>
                    </div>

                    <div class="contact-item">
                        <i class="fa-solid fa-globe"></i>
                        <span>www.xyzabc.dev</span>
                    </div>

                    <div class="contact-item">
                        <i class="fa-regular fa-calendar-check"></i>
                        <span>Available for new projects</span>
                    </div>

                </div>

            </div>

        </div>


        <a href="#" class="logout-button">
            Log Out
        </a>

    </div>

</asp:Content>