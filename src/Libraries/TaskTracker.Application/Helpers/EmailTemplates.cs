using Microsoft.Extensions.Configuration;

namespace TaskTracker.Application.Helpers
{
    public static class EmailTemplates
    {
        public static string GetVerificationEmail(string userName, string verificationLink, IConfiguration configuration)
        {
            var frontendUrl = configuration["Frontend:Url"];
            var logoUrl = $"{frontendUrl}/Content/adminlte/dist/img/logo.png";

            return $@"
<!DOCTYPE html>
<html>
<head>
  <meta charset='UTF-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
  <title>Email Verification - TaskTracker</title>
</head>
<body style='margin:0; padding:0; font-family: Arial, sans-serif; background-color:#f8fafb; color:#333;'>

  <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f8fafb; padding:40px 0;'>
    <tr>
      <td align='center'>
        <table width='600' cellpadding='0' cellspacing='0' style='background:#ffffff; border-radius:12px; box-shadow:0 4px 16px rgba(0,0,0,0.05); padding:40px;'>
          
          <!-- Logo -->
          <tr>
            <td align='center' style='padding-bottom:20px;'>
              <img src='{logoUrl}' alt='TaskTracker' width='80' style='border-radius:10px;'>
            </td>
          </tr>

          <!-- Title -->
          <tr>
            <td align='center'>
              <h2 style='color:#2E7D32; margin-bottom:8px;'>Verify Your Email Address</h2>
              <p style='color:#666; margin:0;'>Hi {userName}, please confirm your email to activate your TaskTracker account.</p>
            </td>
          </tr>

          <!-- Verify Button -->
          <tr>
            <td align='center' style='padding:30px 0;'>
              <a href='{verificationLink}' 
                 style='background-color:#2E7D32; color:#ffffff; padding:12px 28px; border-radius:6px; text-decoration:none; font-weight:bold; font-size:15px;'>
                 Verify Email
              </a>
            </td>
          </tr>

          <!-- Expiry Note -->
          <tr>
            <td align='center'>
              <p style='font-size:13px; color:#888;'>
                This link will expire in 24 hours.<br>
                If you did not request this, please ignore this email.
              </p>
            </td>
          </tr>

          <!-- Divider -->
          <tr>
            <td style='padding-top:30px;'>
              <hr style='border:none; border-top:1px solid #e0e0e0;'>
            </td>
          </tr>

          <!-- Signature -->
          <tr>
            <td style='padding-top:20px;'>
              <table cellpadding='0' cellspacing='0' width='100%' style='font-size:14px; color:#444;'>
                <tr>
                  <td width='70' style='vertical-align:top; padding-right:12px;'>
                    <img src='{logoUrl}' alt='TaskTracker Logo' width='60' style='border-radius:8px;'>
                  </td>
                  <td style='vertical-align:top;'>
                    <p style='margin:0; font-weight:bold; color:#2E7D32;'>TaskTracker Team</p>
                    <p style='margin:3px 0 6px; color:#555; font-size:13px;'>
                      Smart Food, Smarter Living.<br>
                      <a href='{frontendUrl}' style='color:#2E7D32; text-decoration:none;'>{frontendUrl.Replace("https://", "").Replace("http://", "")}</a><br>
                      📞 +880177-1225965| ✉️ 
                      <a href='mailto:info@digantafood.com' style='color:#2E7D32; text-decoration:none;'>info@digantafood.com</a>
                    </p>
                    <p style='font-size:12px; color:#888; margin-top:6px;'>
                      © {DateTime.Now.Year} TaskTracker. All Rights Reserved.
                    </p>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <!-- Footer Note -->
          <tr>
            <td style='padding-top:15px;'>
              <p style='font-size:11px; color:#aaa; text-align:center;'>
                This email was generated automatically by the TaskTracker system. Please do not reply directly.<br>
                <a href='{frontendUrl}/terms' style='color:#aaa; text-decoration:none;'>Terms & Conditions</a> |
                <a href='{frontendUrl}/privacy' style='color:#aaa; text-decoration:none;'>Privacy Policy</a>
              </p>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>

</body>
</html>";
        }
    }
}
