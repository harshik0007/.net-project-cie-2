<%@ Page Title="How Escrow Works" Language="C#" MasterPageFile="~/Site.Master" %>

<asp:Content ID="EscrowGuideHead" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/EscrowGuide.css?v=2" />
</asp:Content>

<asp:Content ID="EscrowGuideContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="escrow-guide-page">
        <a class="escrow-guide-back" href="../Client/Payments.aspx"><i class="fa-solid fa-arrow-left"></i> Back to payments</a>

        <header class="escrow-guide-hero">
            <span class="escrow-guide-eyebrow"><i class="fa-solid fa-shield-halved"></i> Payment protection</span>
            <h1>How escrow works</h1>
            <p>Funds are recorded as held while work is in progress. When the client approves the completed work, the agreed contract payout is released to the freelancer.</p>
            <span class="escrow-simulation-tag"><i class="fa-solid fa-circle-info"></i> Academic simulation &mdash; no real money is moved</span>
        </header>

        <section class="escrow-guide-card">
            <div class="escrow-guide-section-heading">
                <span class="escrow-guide-section-icon"><i class="fa-solid fa-route"></i></span>
                <div><h2>Four steps from funding to completion</h2><p>Both sides can see the contract status and payment history.</p></div>
            </div>
            <ol class="escrow-steps">
                <li><span class="escrow-step-number">1</span><div><h3>Client funds the contract</h3><p>The client total includes the agreed amount, a separate 5% client platform fee, and 18% demo GST on the agreed amount. The freelancer can then submit work.</p></div></li>
                <li><span class="escrow-step-number">2</span><div><h3>Freelancer delivers the work</h3><p>The freelancer submits project files and a completion note for the client to review.</p></div></li>
                <li><span class="escrow-step-number">3</span><div><h3>Client reviews the submission</h3><p>The client can request a revision with written feedback, or approve the submitted work.</p></div></li>
                <li><span class="escrow-step-number">4</span><div><h3>Approval releases payment</h3><p>Approving the work completes the contract. A 5% freelancer platform fee is deducted from the agreed amount, and the remaining 95% is released to the freelancer.</p></div></li>
            </ol>
        </section>

        <section class="escrow-guide-card">
            <div class="escrow-guide-section-heading">
                <span class="escrow-guide-section-icon"><i class="fa-solid fa-receipt"></i></span>
                <div><h2>Payment breakdown in this demo</h2><p>For normal completion, the client pays the contract amount plus a 5% client platform fee and 18% demo GST. The freelancer receives 95% of the contract amount after a separate 5% freelancer platform fee.</p></div>
            </div>
            <div class="escrow-split-grid">
                <article class="escrow-split-item"><span>Agreed contract amount</span><strong>&#8377;10,000</strong><small>Base before platform fees</small></article>
                <article class="escrow-split-item"><span>Demo GST at 18%</span><strong>&#8377;1,800</strong><small>Calculated on contract amount</small></article>
                <article class="escrow-split-item platform-fee"><span>Client platform fee at 5%</span><strong>&#8377;500</strong><small>Separate simulated fee</small></article>
                <article class="escrow-split-item freelancer-payout"><span>Freelancer platform fee at 5%</span><strong>&#8377;500</strong><small>Deducted from contract amount</small></article>
                <article class="escrow-split-item client-refund"><span>Client total</span><strong>&#8377;12,300</strong><small>Contract amount, demo GST, and client fee</small></article>
            </div>
            <p class="escrow-cancellation-note">The 18% rate and 5% platform fees are fixed academic demonstration values. GST is calculated only on the agreed contract amount in this demo. They are not a tax determination, GST invoice, or amounts collected or remitted to a tax authority. Actual GST treatment and rates depend on the real supply and applicable rules.</p>
        </section>

        <section class="escrow-guide-card escrow-cancellation-card">
            <div class="escrow-guide-section-heading">
                <span class="escrow-guide-section-icon cancellation-icon"><i class="fa-solid fa-arrow-rotate-left"></i></span>
                <div><h2>Cancellation before work is submitted</h2><p>If a client cancels a funded contract before any work is submitted, the demo GST and 5% client platform fee are returned in full. The pre-GST contract amount is allocated as follows.</p></div>
            </div>
            <div class="escrow-split-grid">
                <article class="escrow-split-item client-refund"><span>Contract price refund</span><strong>75%</strong><small>Plus full demo GST and client fee</small></article>
                <article class="escrow-split-item freelancer-payout"><span>Freelancer payout</span><strong>10%</strong><small>Of pre-GST contract amount</small></article>
                <article class="escrow-split-item platform-fee"><span>Platform fee</span><strong>15%</strong><small>Of pre-GST contract amount</small></article>
            </div>
            <div class="escrow-example"><i class="fa-solid fa-calculator"></i><p><strong>Normal completion: &#8377;10,000 contract + &#8377;1,800 demo GST + &#8377;500 client fee = &#8377;12,300 client total</strong><br />Freelancer payout: &#8377;9,500 after a &#8377;500 freelancer fee.<br /><strong>Pre-submission cancellation:</strong> Client refund: &#8377;7,500 contract refund + &#8377;1,800 GST + &#8377;500 client fee = &#8377;9,800 &nbsp;&middot;&nbsp; Freelancer payout: &#8377;1,000 &nbsp;&middot;&nbsp; Cancellation platform fee: &#8377;1,500</p></div>
            <p class="escrow-cancellation-note">This cancellation split is available only before any work submission. Once work has been submitted, the contract follows the review and revision process instead.</p>
        </section>

        <div class="escrow-guide-footer-note"><i class="fa-solid fa-circle-info"></i><span>SkillLink currently records simulated payments for academic use. It does not collect card, bank, UPI, GST registration details, or payment-provider credentials, and no real payment, tax collection, transfer, or refund takes place.</span></div>
    </main>
</asp:Content>
