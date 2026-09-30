using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class CreateProject : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvProjectTitle_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string value = args.Value.Trim();

            args.IsValid = value.Length >= 5 && value.Length <= 100;
        }

        protected void cvDescription_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string value = args.Value.Trim();

            args.IsValid = value.Length >= 20 && value.Length <= 2000;
        }

        protected void cvCategory_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(ddlCategory.SelectedValue);
        }

        protected void cvExperience_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(ddlExperience.SelectedValue);
        }

        protected void cvMinBudget_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            decimal value;

            args.IsValid =
                decimal.TryParse(
                    args.Value.Trim(),
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out value)
                && value > 0;
        }

        protected void cvMaxBudget_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            decimal minBudget;
            decimal maxBudget;

            bool minValid = decimal.TryParse(
                txtMinBudget.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out minBudget);

            bool maxValid = decimal.TryParse(
                txtMaxBudget.Text.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out maxBudget);

            args.IsValid =
                minValid &&
                maxValid &&
                maxBudget > 0 &&
                maxBudget >= minBudget;
        }

        protected void cvDeadline_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            DateTime deadline;

            bool validDate = DateTime.TryParse(
                args.Value,
                out deadline);

            args.IsValid =
                validDate &&
                deadline.Date > DateTime.Today;
        }

        protected void btnPostProject_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            lblMessage.Text =
                "Project posted successfully. Freelancers can now submit proposals.";

            lblMessage.CssClass =
                "form-message success-message";
        }
    }
}