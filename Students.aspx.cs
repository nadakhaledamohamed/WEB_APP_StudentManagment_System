using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;

namespace WebApplication1.Controller
{
    public partial class Students : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentRegistrationDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                  SELECT 
    s.student_id,
    CONCAT(s.first_name, ' ', s.last_name) AS FullName,
    c.course_name,
    c.course_code,
    c.credit_hours,
    CONCAT(i.first_name, ' ', i.last_name) AS InstructorName,
    co.schedule
FROM Students s
INNER JOIN Registrations r ON s.student_id = r.student_id
INNER JOIN CourseOfferings co ON r.offering_id = co.offering_id
INNER JOIN Courses c ON co.course_id = c.course_id
INNER JOIN Instructors i ON co.instructor_id = i.instructor_id;";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string[] studentParts = txtStudentName.Text.Split(' ');
            string firstName = studentParts[0];
            string lastName = studentParts.Length > 1 ? studentParts[1] : "";
            string courseName = txtCourseName.Text;
            string courseCode = txtCourseCode.Text;
            int creditHours = int.TryParse(txtCreditHours.Text, out int credits) ? credits : 0;
            string[] instructorParts = txtInstructorName.Text.Split(' ');
            string instructorFirstName = instructorParts[0];
            string instructorLastName = instructorParts.Length > 1 ? instructorParts[1] : "";
            string schedule = txtSchedule.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertStudentCourseOffering", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters for student
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", $"{firstName.ToLower()}.{lastName.ToLower()}@university.com");
                cmd.Parameters.AddWithValue("@Password", "hashed_password");
                cmd.Parameters.AddWithValue("@PhoneNumber", "1234567890");
                cmd.Parameters.AddWithValue("@Major", "Undeclared");
                cmd.Parameters.AddWithValue("@EnrollmentYear", DateTime.Now.Year);

                // Add parameters for course
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                cmd.Parameters.AddWithValue("@CreditHours", creditHours);

                // Add parameters for instructor and schedule
                cmd.Parameters.AddWithValue("@InstructorFirstName", instructorFirstName);
                cmd.Parameters.AddWithValue("@InstructorLastName", instructorLastName);
                cmd.Parameters.AddWithValue("@Schedule", schedule);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            // Extract updated values from the GridView
            string fullName = ((TextBox)GridView1.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string courseName = ((TextBox)GridView1.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string courseCode = ((TextBox)GridView1.Rows[e.RowIndex].Cells[3].Controls[0]).Text;
            int creditHours = int.TryParse(((TextBox)GridView1.Rows[e.RowIndex].Cells[4].Controls[0]).Text, out int credits) ? credits : 0;
            string[] instructorParts = ((TextBox)GridView1.Rows[e.RowIndex].Cells[5].Controls[0]).Text.Split(' ');
            string instructorFirstName = instructorParts[0];
            string instructorLastName = instructorParts.Length > 1 ? instructorParts[1] : "";
            string schedule = ((TextBox)GridView1.Rows[e.RowIndex].Cells[6].Controls[0]).Text;

            // Split fullName into first and last name
            string[] nameParts = fullName.Split(' ');
            string firstName = nameParts[0];
            string lastName = nameParts.Length > 1 ? nameParts[1] : "";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            -- Update Students
            UPDATE Students
            SET 
                first_name = @FirstName,
                last_name = @LastName
            WHERE student_id = @StudentId;

            -- Update Courses
            UPDATE Courses
            SET 
                course_name = @CourseName,
                course_code = @CourseCode,
                credit_hours = @CreditHours
            WHERE course_id = (
                SELECT co.course_id 
                FROM Registrations r
                INNER JOIN CourseOfferings co ON r.offering_id = co.offering_id
                WHERE r.student_id = @StudentId
            );

            -- Update Instructors
            UPDATE Instructors
            SET 
                first_name = @InstructorFirstName,
                last_name = @InstructorLastName
            WHERE instructor_id = (
                SELECT co.instructor_id
                FROM Registrations r
                INNER JOIN CourseOfferings co ON r.offering_id = co.offering_id
                WHERE r.student_id = @StudentId
            );

            -- Update CourseOfferings
            UPDATE CourseOfferings
            SET schedule = @Schedule
            WHERE offering_id = (
                SELECT r.offering_id
                FROM Registrations r
                WHERE r.student_id = @StudentId
            );
        ";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters for Students
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                // Add parameters for Courses
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@CourseCode", courseCode);
                cmd.Parameters.AddWithValue("@CreditHours", creditHours);

                // Add parameters for Instructors
                cmd.Parameters.AddWithValue("@InstructorFirstName", instructorFirstName);
                cmd.Parameters.AddWithValue("@InstructorLastName", instructorLastName);

                // Add parameter for CourseOfferings
                cmd.Parameters.AddWithValue("@Schedule", schedule);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            BindGrid();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to delete related registrations and then the student record
                string query = @"
            DELETE FROM Registrations WHERE student_id = @StudentId;
            DELETE FROM Students WHERE student_id = @StudentId;
        ";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid();
        }

    }

}
