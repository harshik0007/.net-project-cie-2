<%@ Page Title="Find the right people" Language="C#" MasterPageFile="~/Site2.master" AutoEventWireup="true" CodeBehind="Landing.aspx.cs" Inherits="CIE_2_PROJECT.Landing" %>

<asp:Content ID="LandingHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/Landing.css" />
</asp:Content>

<asp:Content ID="LandingContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="landing-page">
        <section class="landing-hero">
            <div class="hero-copy">
                <p class="eyebrow">FREELANCE. COLLABORATE. GROW.</p>
                <h1>Find the right people<br /><span>to bring your ideas to life.</span></h1>
                <p class="hero-description">SkillLink connects clients and freelancers to work on real projects, build skills and create opportunities together.</p>
                <div class="hero-actions">
                    <div class="hero-action-item">
                        <a runat="server" href="~/Client/ClientHome.aspx" class="hero-button button-primary">I am a Client</a>
                        <span>Find talented freelancers</span>
                    </div>
                    <div class="hero-action-item">
                        <a runat="server" href="~/Freelancer/FreelancerHome.aspx" class="hero-button button-secondary">I am a Freelancer</a>
                        <span>Find projects and earn</span>
                    </div>
                </div>
                <p class="access-note">Sign in or create an account to unlock the full SkillLink experience.</p>
            </div>
            <div class="hero-visual">
                <img src="../Content/Images/landing-collaboration.png" alt="A freelancer and client collaborating on a project at a laptop" />
            </div>
        </section>

        <section class="category-section" aria-labelledby="categories-title">
            <div class="section-heading">
                <p class="eyebrow">EXPLORE OPPORTUNITIES</p>
                <h2 id="categories-title">Find work across many skills</h2>
            </div>
            <div class="category-grid">
                <div class="category-item"><i class="fa-solid fa-code"></i><span>Web Development</span></div>
                <div class="category-item"><i class="fa-solid fa-mobile-screen-button"></i><span>Mobile Development</span></div>
                <div class="category-item"><i class="fa-solid fa-pen-ruler"></i><span>UI/UX &amp; Graphic Design</span></div>
                <div class="category-item"><i class="fa-solid fa-gamepad"></i><span>Game Development</span></div>
                <div class="category-item"><i class="fa-solid fa-file-lines"></i><span>Writing &amp; Content</span></div>
                <div class="category-item"><i class="fa-solid fa-bullhorn"></i><span>Digital Marketing</span></div>
                <div class="category-item"><i class="fa-solid fa-chart-line"></i><span>Data Science &amp; AI</span></div>
                <div class="category-item"><i class="fa-solid fa-film"></i><span>Video &amp; Animation</span></div>
            </div>
        </section>

        <section class="benefits-section" aria-label="Why SkillLink">
            <article class="benefit-item">
                <span class="benefit-icon"><i class="fa-solid fa-user-group"></i></span>
                <div><h3>Talented Freelancers</h3><p>Connect with skilled professionals across different domains.</p></div>
            </article>
            <article class="benefit-item">
                <span class="benefit-icon"><i class="fa-solid fa-folder-open"></i></span>
                <div><h3>Real Projects</h3><p>Work on meaningful projects and practical opportunities.</p></div>
            </article>
            <article class="benefit-item">
                <span class="benefit-icon"><i class="fa-solid fa-shield-halved"></i></span>
                <div><h3>Safe &amp; Trusted</h3><p>Manage projects through a structured and secure workflow.</p></div>
            </article>
        </section>

        <section class="steps-section" aria-labelledby="steps-title">
            <div class="section-heading">
                <p class="eyebrow">A CLEAR WAY TO WORK TOGETHER</p>
                <h2 id="steps-title">How SkillLink Works</h2>
            </div>
            <div class="steps-grid">
                <article class="step-item"><span class="step-number">1</span><div><h3>Post or Browse</h3><p>Clients post projects or freelancers browse opportunities.</p></div></article>
                <article class="step-item"><span class="step-number">2</span><div><h3>Connect</h3><p>Discuss requirements, share ideas and agree on the work.</p></div></article>
                <article class="step-item"><span class="step-number">3</span><div><h3>Get It Done</h3><p>Work together and complete the project.</p></div></article>
            </div>
        </section>

        <section class="final-cta">
            <h2>Ready to get started?</h2>
            <p>Join SkillLink and connect with the right people for your next project.</p>
            <div class="hero-actions cta-actions">
                <a runat="server" href="~/Client/ClientHome.aspx" class="hero-button button-primary">I am a Client</a>
                <a runat="server" href="~/Freelancer/FreelancerHome.aspx" class="hero-button button-secondary">I am a Freelancer</a>
            </div>
        </section>

        <footer class="landing-footer">
            <div class="landing-footer-content">
                <a runat="server" href="~/Default.aspx" class="footer-brand">SkillLink</a>
                <div class="important-links">
                    <a runat="server" href="~/Guidelines/PrivacyPolicy.aspx">Privacy Policy</a>
                    <a runat="server" href="~/Guidelines/TermsOfService.aspx">Terms of Service</a>
                    <a runat="server" href="~/Guidelines/SupportCenter.aspx">Support Center</a>
                    <a runat="server" href="~/Guidelines/CommunityGuidelines.aspx">Community Guidelines</a>
                </div>
                <p class="copyright">&copy;2026 SkillLink Global. All rights reserved.</p>
            </div>
        </footer>
    </main>
</asp:Content>
