<%@ Page Title="Create Project" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateProject.aspx.cs" Inherits="CIE_2_PROJECT.CreateProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../Content/Css/CreateProject.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="create-project-page">

        <div class="page-header">
            <div>
                <a href="MyProjects.aspx" class="back-link">
                    <i class="fa-solid fa-arrow-left"></i>
                    Back to My Projects
                </a>

                <h1>Create a Project</h1>

                <p>
                    Tell us what you need and find the right freelancer for your project.
                </p>
            </div>
        </div>


        <div class="form-layout">

            <div class="form-card">

                <div class="form-section">

                    <div class="section-title">
                        <div class="section-icon">
                            <i class="fa-solid fa-file-lines"></i>
                        </div>

                        <div>
                            <h2>Project Details</h2>
                            <p>Provide the basic information about your project.</p>
                        </div>
                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblProjectTitle"
                            runat="server"
                            AssociatedControlID="txtProjectTitle"
                            CssClass="form-label">
                            Project Title
                            <span>*</span>
                        </asp:Label>

                        <asp:TextBox
                            ID="txtProjectTitle"
                            runat="server"
                            CssClass="form-input"
                            MaxLength="100"
                            placeholder="e.g. Build a responsive e-commerce website">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvProjectTitle"
                            runat="server"
                            ControlToValidate="txtProjectTitle"
                            ErrorMessage="Project title is required."
                            CssClass="validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvProjectTitle"
                            runat="server"
                            ControlToValidate="txtProjectTitle"
                            ErrorMessage="Project title must be between 5 and 100 characters."
                            CssClass="validation-error"
                            EnableClientScript="false"
                            OnServerValidate="cvProjectTitle_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblDescription"
                            runat="server"
                            AssociatedControlID="txtDescription"
                            CssClass="form-label">
                            Project Description
                            <span>*</span>
                        </asp:Label>

                        <asp:TextBox
                            ID="txtDescription"
                            runat="server"
                            TextMode="MultiLine"
                            CssClass="form-textarea"
                            placeholder="Describe what you need, your goals, requirements and expected outcome.">
                        </asp:TextBox>

                        <asp:RequiredFieldValidator
                            ID="rfvDescription"
                            runat="server"
                            ControlToValidate="txtDescription"
                            ErrorMessage="Project description is required."
                            CssClass="validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                        <asp:CustomValidator
                            ID="cvDescription"
                            runat="server"
                            ControlToValidate="txtDescription"
                            ErrorMessage="Description must be between 20 and 2000 characters."
                            CssClass="validation-error"
                            EnableClientScript="false"
                            OnServerValidate="cvDescription_ServerValidate">
                        </asp:CustomValidator>

                    </div>


                    <div class="form-row">

                        <div class="form-group">

                            <asp:Label
                                ID="lblCategory"
                                runat="server"
                                AssociatedControlID="ddlCategory"
                                CssClass="form-label">
                                Category
                                <span>*</span>
                            </asp:Label>

                            <asp:DropDownList
                                ID="ddlCategory"
                                runat="server"
                                CssClass="form-select">

                                <asp:ListItem Text="Select Category" Value=""></asp:ListItem>
                                <asp:ListItem Text="Web Development" Value="Web Development"></asp:ListItem>
                                <asp:ListItem Text="Mobile Development" Value="Mobile Development"></asp:ListItem>
                                <asp:ListItem Text="UI/UX Design" Value="UI/UX Design"></asp:ListItem>
                                <asp:ListItem Text="Graphic Design" Value="Graphic Design"></asp:ListItem>
                                <asp:ListItem Text="WordPress" Value="WordPress"></asp:ListItem>
                                <asp:ListItem Text="Software Development" Value="Software Development"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>

                            </asp:DropDownList>

                            <asp:CustomValidator
                                ID="cvCategory"
                                runat="server"
                                ErrorMessage="Please select a category."
                                CssClass="validation-error"
                                EnableClientScript="false"
                                OnServerValidate="cvCategory_ServerValidate">
                            </asp:CustomValidator>

                        </div>


                        <div class="form-group">

                            <asp:Label
                                ID="lblExperience"
                                runat="server"
                                AssociatedControlID="ddlExperience"
                                CssClass="form-label">
                                Experience Required
                                <span>*</span>
                            </asp:Label>

                            <asp:DropDownList
                                ID="ddlExperience"
                                runat="server"
                                CssClass="form-select">

                                <asp:ListItem Text="Select Experience" Value=""></asp:ListItem>
                                <asp:ListItem Text="Entry Level" Value="Entry"></asp:ListItem>
                                <asp:ListItem Text="Intermediate" Value="Intermediate"></asp:ListItem>
                                <asp:ListItem Text="Expert" Value="Expert"></asp:ListItem>

                            </asp:DropDownList>

                            <asp:CustomValidator
                                ID="cvExperience"
                                runat="server"
                                ErrorMessage="Please select the required experience."
                                CssClass="validation-error"
                                EnableClientScript="false"
                                OnServerValidate="cvExperience_ServerValidate">
                            </asp:CustomValidator>

                        </div>

                    </div>


                    <div class="form-group">

                        <asp:Label
                            ID="lblSkills"
                            runat="server"
                            AssociatedControlID="txtSkills"
                            CssClass="form-label">
                            Required Skills
                            <span>*</span>
                        </asp:Label>

                        <asp:TextBox
                            ID="txtSkills"
                            runat="server"
                            CssClass="form-input"
                            placeholder="e.g. React.js, Node.js, MongoDB">
                        </asp:TextBox>

                        <div class="field-help">
                            Separate multiple skills with commas.
                        </div>

                        <asp:RequiredFieldValidator
                            ID="rfvSkills"
                            runat="server"
                            ControlToValidate="txtSkills"
                            ErrorMessage="Required skills are required."
                            CssClass="validation-error"
                            EnableClientScript="false">
                        </asp:RequiredFieldValidator>

                    </div>

                </div>


                <div class="form-divider"></div>


                <div class="form-section">

                    <div class="section-title">
                        <div class="section-icon">
                            <i class="fa-solid fa-indian-rupee-sign"></i>
                        </div>

                        <div>
                            <h2>Budget & Timeline</h2>
                            <p>Set your project budget and expected delivery date.</p>
                        </div>
                    </div>


                    <div class="form-row">

                        <div class="form-group">

                            <asp:Label
                                ID="lblMinBudget"
                                runat="server"
                                AssociatedControlID="txtMinBudget"
                                CssClass="form-label">
                                Minimum Budget
                                <span>*</span>
                            </asp:Label>

                            <div class="input-prefix">
                                <span>&#8377;</span>

                                <asp:TextBox
                                    ID="txtMinBudget"
                                    runat="server"
                                    CssClass="prefix-input"
                                    placeholder="15000">
                                </asp:TextBox>
                            </div>

                            <asp:RequiredFieldValidator
                                ID="rfvMinBudget"
                                runat="server"
                                ControlToValidate="txtMinBudget"
                                ErrorMessage="Minimum budget is required."
                                CssClass="validation-error"
                                EnableClientScript="false">
                            </asp:RequiredFieldValidator>

                            <asp:CustomValidator
                                ID="cvMinBudget"
                                runat="server"
                                ControlToValidate="txtMinBudget"
                                ErrorMessage="Enter a valid minimum budget."
                                CssClass="validation-error"
                                EnableClientScript="false"
                                OnServerValidate="cvMinBudget_ServerValidate">
                            </asp:CustomValidator>

                        </div>


                        <div class="form-group">

                            <asp:Label
                                ID="lblMaxBudget"
                                runat="server"
                                AssociatedControlID="txtMaxBudget"
                                CssClass="form-label">
                                Maximum Budget
                                <span>*</span>
                            </asp:Label>

                            <div class="input-prefix">
                                <span>&#8377;</span>

                                <asp:TextBox
                                    ID="txtMaxBudget"
                                    runat="server"
                                    CssClass="prefix-input"
                                    placeholder="25000">
                                </asp:TextBox>
                            </div>

                            <asp:RequiredFieldValidator
                                ID="rfvMaxBudget"
                                runat="server"
                                ControlToValidate="txtMaxBudget"
                                ErrorMessage="Maximum budget is required."
                                CssClass="validation-error"
                                EnableClientScript="false">
                            </asp:RequiredFieldValidator>

                            <asp:CustomValidator
                                ID="cvMaxBudget"
                                runat="server"
                                ControlToValidate="txtMaxBudget"
                                ErrorMessage="Enter a valid maximum budget greater than minimum budget."
                                CssClass="validation-error"
                                EnableClientScript="false"
                                OnServerValidate="cvMaxBudget_ServerValidate">
                            </asp:CustomValidator>

                        </div>

                    </div>


                    <div class="form-row">

                        <div class="form-group">

                            <asp:Label
                                ID="lblDeadline"
                                runat="server"
                                AssociatedControlID="txtDeadline"
                                CssClass="form-label">
                                Project Deadline
                                <span>*</span>
                            </asp:Label>

                            <asp:TextBox
                                ID="txtDeadline"
                                runat="server"
                                TextMode="Date"
                                CssClass="form-input">
                            </asp:TextBox>

                            <asp:RequiredFieldValidator
                                ID="rfvDeadline"
                                runat="server"
                                ControlToValidate="txtDeadline"
                                ErrorMessage="Project deadline is required."
                                CssClass="validation-error"
                                EnableClientScript="false">
                            </asp:RequiredFieldValidator>

                            <asp:CustomValidator
                                ID="cvDeadline"
                                runat="server"
                                ControlToValidate="txtDeadline"
                                ErrorMessage="Project deadline must be a future date."
                                CssClass="validation-error"
                                EnableClientScript="false"
                                OnServerValidate="cvDeadline_ServerValidate">
                            </asp:CustomValidator>

                        </div>


                        <div class="form-group">

                            <asp:Label
                                ID="lblLocation"
                                runat="server"
                                AssociatedControlID="txtLocation"
                                CssClass="form-label">
                                Preferred Location
                                <span class="optional">(Optional)</span>
                            </asp:Label>

                            <asp:TextBox
                                ID="txtLocation"
                                runat="server"
                                CssClass="form-input"
                                placeholder="Remote / India / Specific location">
                            </asp:TextBox>

                        </div>

                    </div>

                </div>


                <div class="form-divider"></div>


                <div class="form-section">

                    <div class="section-title">
                        <div class="section-icon">
                            <i class="fa-solid fa-paperclip"></i>
                        </div>

                        <div>
                            <h2>Project Files</h2>
                            <p>Attach documents or reference files if required.</p>
                        </div>
                    </div>


                    <div class="upload-box">

                        <i class="fa-solid fa-cloud-arrow-up"></i>

                        <h3>Upload Project Files</h3>

                        <p>
                            Add requirements, references, designs or other useful files.
                        </p>

                        <asp:FileUpload
                            ID="fuProjectFiles"
                            runat="server"
                            CssClass="file-upload" />

                        <span>
                            PDF, DOC, DOCX, ZIP, PNG, JPG
                        </span>

                    </div>

                </div>


                <div class="form-actions">

                    <a href="MyProjects.aspx" class="cancel-button">
                        Cancel
                    </a>

                    <asp:Button
                        ID="btnPostProject"
                        runat="server"
                        Text="Post Project"
                        CssClass="post-button"
                        OnClick="btnPostProject_Click" />

                </div>


                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="form-message">
                </asp:Label>

            </div>


            <aside class="project-sidebar">

                <div class="tips-card">

                    <h2>Tips for a Great Project</h2>

                    <div class="tip">
                        <i class="fa-solid fa-check"></i>
                        <span>Use a clear and specific project title.</span>
                    </div>

                    <div class="tip">
                        <i class="fa-solid fa-check"></i>
                        <span>Clearly describe your requirements and goals.</span>
                    </div>

                    <div class="tip">
                        <i class="fa-solid fa-check"></i>
                        <span>Add the skills you expect from the freelancer.</span>
                    </div>

                    <div class="tip">
                        <i class="fa-solid fa-check"></i>
                        <span>Set a realistic budget and deadline.</span>
                    </div>

                </div>


                <div class="workflow-card">

                    <div class="workflow-icon">
                        <i class="fa-solid fa-route"></i>
                    </div>

                    <h2>What happens next?</h2>

                    <div class="workflow-step">
                        <span>1</span>
                        <p>Your project is posted.</p>
                    </div>

                    <div class="workflow-step">
                        <span>2</span>
                        <p>Freelancers submit proposals.</p>
                    </div>

                    <div class="workflow-step">
                        <span>3</span>
                        <p>You review and select a freelancer.</p>
                    </div>

                    <div class="workflow-step">
                        <span>4</span>
                        <p>Payment is secured in escrow after hiring.</p>
                    </div>

                </div>

            </aside>

        </div>

    </div>

</asp:Content>