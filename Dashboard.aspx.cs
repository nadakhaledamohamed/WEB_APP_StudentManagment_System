using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Controller
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentRegistrationDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Assume the student's email is passed via query string after login
                string email = Request.QueryString["email"];
                if (string.IsNullOrEmpty(email))
                {
                    Response.Redirect("Login.aspx");
                }

                LoadStudentInfo(email);
                LoadEnrolledCourses(email);
            }
        }

        private void LoadStudentInfo(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT first_name + ' ' + last_name AS FullName, email, phone_number, major, enrollment_year FROM Students WHERE email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    lblStudentName.Text = reader["FullName"].ToString();
                    lblEmail.Text = reader["email"].ToString();
                    lblPhoneNumber.Text = reader["phone_number"].ToString();
                    lblMajor.Text = reader["major"].ToString();
                    lblEnrollmentYear.Text = reader["enrollment_year"].ToString();
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        private void LoadEnrolledCourses(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
                    SELECT c.course_name AS CourseName, c.course_code AS CourseCode, c.credit_hours AS CreditHours,
                           i.first_name + ' ' + i.last_name AS InstructorName, co.schedule AS Schedule
                    FROM Students s
                    JOIN Registrations r ON s.student_id = r.student_id
                    JOIN CourseOfferings co ON r.offering_id = co.offering_id
                    JOIN Courses c ON co.course_id = c.course_id
                    JOIN Instructors i ON co.instructor_id = i.instructor_id
                    WHERE s.email = @Email", conn);

                cmd.Parameters.AddWithValue("@Email", email);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewCourses.DataSource = dt;
                GridViewCourses.DataBind();
            }
        }
    }
}
