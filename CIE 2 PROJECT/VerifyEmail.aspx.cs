using System;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CIE_2_PROJECT
{
    public partial class VerifyEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvOtp_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            string otp =
                txtOtp1.Text.Trim() +
                txtOtp2.Text.Trim() +
                txtOtp3.Text.Trim() +
                txtOtp4.Text.Trim() +
                txtOtp5.Text.Trim() +
                txtOtp6.Text.Trim();

            args.IsValid =
                Regex.IsMatch(otp, @"^[0-9]{6}$");
        }

        protected void btnVerify_Click(
            object sender,
            EventArgs e)
        {
            Page.Validate();

            if (!Page.IsValid)
            {
                return;
            }

            string otp =
                txtOtp1.Text.Trim() +
                txtOtp2.Text.Trim() +
                txtOtp3.Text.Trim() +
                txtOtp4.Text.Trim() +
                txtOtp5.Text.Trim() +
                txtOtp6.Text.Trim();

            // OTP verification logic will be added later.
        }

        protected void btnResend_Click(
            object sender,
            EventArgs e)
        {
            // Resend OTP logic will be added later.
        }
    }
}