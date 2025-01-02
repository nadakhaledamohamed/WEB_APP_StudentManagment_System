
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Students.aspx.cs" Inherits="WebApplication1.Controller.Students" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Management</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(to right, #1d2b64, #f8cdda);
            min-height: 100vh;
        }

        .dashboard-container {
            max-width: 900px;
            margin: 2rem auto;
            background: rgba(255, 255, 255, 0.95);
            border-radius: 10px;
            padding: 2rem;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2);
            border: 1px solid #ddd;
        }

        .dashboard-container h1 {
            text-align: center;
            color: #2c3e50;
            margin-bottom: 1.5rem;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 1rem;
        }

        th, td {
            text-align: left;
            padding: 0.8rem;
            border-bottom: 1px solid #ddd;
        }

        th {
            background-color: #f4f4f4;
            font-weight: bold;
        }

        tr:hover {
            background-color: #f1f1f1;
        }

        .form-container {
            margin-top: 2rem;
        }

        .form-container input {
            width: calc(100% - 20px);
            padding: 10px;
            margin: 5px 0;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .form-container button {
            background-color: #2575fc;
            color: #fff;
            padding: 10px 15px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .form-container button:hover {
            background-color: #1f4db5;
        }
    </style>
    <script type="text/javascript">
        // JavaScript function to confirm actions
        function confirmAction(action) {
            return confirm("Are you sure you want to " + action + "?");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <h1>Student Management</h1>
           <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="student_id"
    OnRowEditing="GridView1_RowEditing" 
    OnRowUpdating="GridView1_RowUpdating"
    OnRowCancelingEdit="GridView1_RowCancelingEdit" 
    OnRowDeleting="GridView1_RowDeleting">
    <Columns>
        <asp:BoundField DataField="student_id" HeaderText="Student ID" ReadOnly="True" />
        <asp:BoundField DataField="FullName" HeaderText="Student Name" />
        <asp:BoundField DataField="course_name" HeaderText="Course Name" />
        <asp:BoundField DataField="course_code" HeaderText="Course Code" />
        <asp:BoundField DataField="credit_hours" HeaderText="Credit Hours" />
        <asp:BoundField DataField="InstructorName" HeaderText="Instructor Name" />
        <asp:BoundField DataField="schedule" HeaderText="Schedule" />
        <asp:TemplateField HeaderText="Actions">
            <ItemTemplate>
                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" 
                    CssClass="edit-button" OnClientClick="return confirmAction('Edit');"></asp:LinkButton>
                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" 
                    CssClass="delete-button" OnClientClick="return confirmAction('Delete');"></asp:LinkButton>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" 
                    CssClass="update-button" OnClientClick="return confirmAction('Update');"></asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" 
                    CssClass="cancel-button" OnClientClick="return confirmAction('Cancel Edit');"></asp:LinkButton>
            </EditItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

            <div class="form-container">
                <asp:TextBox ID="txtStudentName" runat="server" Placeholder="Student Name"></asp:TextBox>
                <asp:TextBox ID="txtCourseName" runat="server" Placeholder="Course Name"></asp:TextBox>
                <asp:TextBox ID="txtCourseCode" runat="server" Placeholder="Course Code"></asp:TextBox>
                <asp:TextBox ID="txtCreditHours" runat="server" Placeholder="Credit Hours"></asp:TextBox>
                <asp:TextBox ID="txtInstructorName" runat="server" Placeholder="Instructor Name"></asp:TextBox>
                <asp:TextBox ID="txtSchedule" runat="server" Placeholder="Schedule"></asp:TextBox>
                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" OnClientClick="return confirmAction('Add');" />
            </div>
        </div>
    </form>
</body>
</html>
