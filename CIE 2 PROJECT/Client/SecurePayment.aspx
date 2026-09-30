<%@ Page Title="Secure Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurePayment.aspx.cs" Inherits="CIE_2_PROJECT.SecurePayment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/SecurePayment.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="payment-page">

        <div class="page-header">
            <div>
                <a href="HireFreelancer.aspx?projectId=1" class="back-link">
                    <i class="fa-solid fa-arrow-left"></i>
                    Back to Proposals
                </a>

                <h1>Secure Payment</h1>

                <p>
                    Your payment is securely held by SkillLink and released
                    only after you approve the completed work.
                </p>
            </div>
        </div>

        <div class="payment-layout">

            <div class="payment-main">

                <div class="section-card">

                    <div class="section-title">
                        <i class="fa-solid fa-file-lines"></i>
                        <h2>Project & Freelancer Details</h2>
                    </div>

                    <div class="project-freelancer">

                        <div class="project-details">
                            <span class="label">PROJECT</span>
                            <h3>Unity 2D Game Development</h3>
                            <p>Development of a 2D Unity game with game mechanics and levels.</p>
                        </div>

                        <div class="freelancer-details">
                            <div class="freelancer-avatar">RS</div>

                            <div>
                                <span class="label">FREELANCER</span>
                                <h3>Rahul Sharma</h3>
                                <p>Full Stack Developer</p>
                            </div>
                        </div>

                    </div>

                </div>


                <div class="section-card">

                    <div class="section-title">
                        <i class="fa-solid fa-indian-rupee-sign"></i>
                        <h2>Payment Amount</h2>
                    </div>

                    <div class="amount-row">
                        <span>Agreed Project Amount</span>
                        <strong>&#8377;15,000</strong>
                    </div>

                    <div class="amount-row">
                        <span>Project Amount</span>
                        <strong>&#8377;15,000</strong>
                    </div>

                    <div class="amount-row">
                        <span>Platform Fee (2%)</span>
                        <strong>&#8377;300</strong>
                    </div>

                    <div class="amount-divider"></div>

                    <div class="amount-total">
                        <span>Total Amount</span>
                        <strong>&#8377;15,300</strong>
                    </div>

                    <div class="escrow-note">
                        <i class="fa-solid fa-shield-halved"></i>

                        <div>
                            <strong>Your payment is protected by escrow</strong>
                            <p>
                                The payment will be securely held until you
                                review and approve the completed work.
                            </p>
                        </div>
                    </div>

                </div>


                <div class="section-card">

                    <div class="section-title">
                        <i class="fa-solid fa-shield-halved"></i>
                        <h2>How Escrow Protection Works</h2>
                    </div>

                    <div class="escrow-steps">

                        <div class="escrow-step">
                            <div class="step-number">1</div>
                            <div>
                                <h3>Payment Secured</h3>
                                <p>Your payment is securely held in escrow.</p>
                            </div>
                        </div>

                        <div class="step-line"></div>

                        <div class="escrow-step">
                            <div class="step-number">2</div>
                            <div>
                                <h3>Freelancer Works</h3>
                                <p>The freelancer starts working on your project.</p>
                            </div>
                        </div>

                        <div class="step-line"></div>

                        <div class="escrow-step">
                            <div class="step-number">3</div>
                            <div>
                                <h3>Work Submission</h3>
                                <p>The freelancer submits the completed work.</p>
                            </div>
                        </div>

                        <div class="step-line"></div>

                        <div class="escrow-step">
                            <div class="step-number">4</div>
                            <div>
                                <h3>You Approve</h3>
                                <p>You review the submitted work and approve it.</p>
                            </div>
                        </div>

                        <div class="step-line"></div>

                        <div class="escrow-step">
                            <div class="step-number">5</div>
                            <div>
                                <h3>Payment Released</h3>
                                <p>The escrow payment is released to the freelancer.</p>
                            </div>
                        </div>

                    </div>

                </div>


                <div class="section-card">

                    <div class="section-title">
                        <i class="fa-regular fa-credit-card"></i>
                        <h2>Select Payment Method</h2>
                    </div>

                    <div class="payment-method">

                        <div class="payment-method-header">
                            <div class="method-icon">
                                <i class="fa-solid fa-mobile-screen-button"></i>
                            </div>

                            <div>
                                <h3>UPI</h3>
                                <p>Pay securely using your UPI ID</p>
                            </div>
                        </div>

                        <div class="upi-form">

                            <asp:Label ID="lblUpi" runat="server"
                                AssociatedControlID="txtUpi"
                                CssClass="input-label">
                                UPI ID
                            </asp:Label>

                            <asp:TextBox ID="txtUpi" runat="server"
                                CssClass="payment-input"
                                placeholder="example@upi">
                            </asp:TextBox>

                            <asp:RequiredFieldValidator
                                ID="rfvUpi"
                                runat="server"
                                ControlToValidate="txtUpi"
                                ErrorMessage="UPI ID is required."
                                CssClass="validation-error"
                                EnableClientScript="false">
                            </asp:RequiredFieldValidator>

                            <asp:CustomValidator
                                ID="cvUpi"
                                runat="server"
                                ControlToValidate="txtUpi"
                                ErrorMessage="Enter a valid UPI ID."
                                CssClass="validation-error"
                                EnableClientScript="false"
                                OnServerValidate="cvUpi_ServerValidate">
                            </asp:CustomValidator>

                            <p class="upi-info">
                                You will be redirected to your UPI app to complete
                                the payment securely.
                            </p>

                        </div>

                    </div>

                </div>


                <div class="security-box">

                    <div class="security-icon">
                        <i class="fa-solid fa-lock"></i>
                    </div>

                    <div>
                        <h3>Safe & Secure with SkillLink</h3>

                        <div class="security-points">

                            <span>
                                <i class="fa-solid fa-check"></i>
                                Payment Secured
                            </span>

                            <span>
                                <i class="fa-solid fa-check"></i>
                                Work Approval
                            </span>

                            <span>
                                <i class="fa-solid fa-check"></i>
                                Escrow Protection
                            </span>

                        </div>
                    </div>

                </div>

            </div>


            <aside class="payment-sidebar">

                <div class="summary-card">

                    <h2>Payment Summary</h2>

                    <div class="summary-project">
                        <span>Project</span>
                        <strong>Unity 2D Game Development</strong>
                    </div>

                    <div class="summary-freelancer">
                        <span>Freelancer</span>
                        <strong>Rahul Sharma</strong>
                    </div>

                    <div class="summary-divider"></div>

                    <div class="summary-row">
                        <span>Project Amount</span>
                        <span>&#8377;15,000</span>
                    </div>

                    <div class="summary-row">
                        <span>Platform Fee</span>
                        <span>&#8377;300</span>
                    </div>

                    <div class="summary-divider"></div>

                    <div class="summary-total">
                        <span>Total</span>
                        <strong>&#8377;15,300</strong>
                    </div>

                    <asp:Button
                        ID="btnMakePayment"
                        runat="server"
                        Text="Pay & Secure &#8377;15,300"
                        CssClass="payment-button"
                        OnClick="btnMakePayment_Click" />

                    <p class="payment-security">
                        <i class="fa-solid fa-lock"></i>
                        Secure payment protected by SkillLink
                    </p>

                </div>


                <div class="info-card">

                    <h3>Why Platform Fee?</h3>

                    <p>
                        The platform fee supports SkillLink services,
                        payment processing and secure escrow management.
                    </p>

                </div>


                <div class="help-card">

                    <div class="help-icon">
                        <i class="fa-regular fa-circle-question"></i>
                    </div>

                    <div>
                        <h3>Need Help?</h3>
                        <p>Our support team is here to help.</p>

                        <a href="../Guidelines/SupportCenter.aspx">
                            Contact Support
                        </a>
                    </div>

                </div>

            </aside>

        </div>

    </div>

</asp:Content>
