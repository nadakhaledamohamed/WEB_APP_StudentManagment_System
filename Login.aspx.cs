using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Controller
{
    public partial class Login : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentRegistrationDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (email == "olivia.nelson@university.com" && password == "000")
            {
                // Redirect to Students.aspx
                Response.Redirect("Students.aspx");
                return;
            }
            // Hardcoded password validation
           //else if (password != "123")
           // {
           //     lblErrorMessage.Text = "Invalid password.";
           //     return;
           // }

            // Validate email from the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Students WHERE email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();

                int count = (int)cmd.ExecuteScalar();
                if (count > 0)
                {
                    // Redirect to another page (e.g., dashboard)
                    // Response.Redirect("Dashboard.aspx");
                    Response.Redirect("Dashboard.aspx?email=" + txtEmail.Text);

                }
                else
                {
                    lblErrorMessage.Text = "Invalid email.";
                }
            }
        }
    }
}