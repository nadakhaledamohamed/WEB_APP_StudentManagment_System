<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebApplication1.Controller.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Dashboard</title>
    <link href="../Styles/Dashboard_style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <h1>Welcome, <asp:Label ID="lblStudentName" runat="server" Text=""></asp:Label>!</h1>

            <div class="student-info">
                <h2>Student Information</h2>
                <p><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></p>
                <p><strong>Phone:</strong> <asp:Label ID="lblPhoneNumber" runat="server" Text=""></asp:Label></p>
                <p><strong>Major:</strong> <asp:Label ID="lblMajor" runat="server" Text=""></asp:Label></p>
                <p><strong>Enrollment Year:</strong> <asp:Label ID="lblEnrollmentYear" runat="server" Text=""></asp:Label></p>
            </div>

            <div class="course-info">
                <h2>Enrolled Courses</h2>
                <asp:GridView ID="GridViewCourses" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="CourseName" HeaderText="Course Name" />
                        <asp:BoundField DataField="CourseCode" HeaderText="Course Code" />
                        <asp:BoundField DataField="CreditHours" HeaderText="Credit Hours" />
                        <asp:BoundField DataField="InstructorName" HeaderText="Instructor" />
                        <asp:BoundField DataField="Schedule" HeaderText="Schedule" />
                    </Columns>
                </asp:GridView>
            </div>

            <a href="Login.aspx" class="logout-btn">Logout</a>
        </div>
    </form>
</body>
</html>
