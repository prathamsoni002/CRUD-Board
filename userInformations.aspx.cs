using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace Resume_Project_CRUD_Board
{
    public partial class userInformations : Page
    {
        // Extracted the connection string into a separate method for reusability
        private string GetConnectionString()
        {
            return "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
        }

        private string originalName;
        private string originalOrganization;
        private string originalDepartment;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userID"] != null)
                {
                    string query = "SELECT [Name], [Organization], [Department], [Username] FROM Users WHERE [UserID] = @UserID";

                    using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@UserID", Session["userID"].ToString());

                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.HasRows)
                            {
                                reader.Read();
                                originalName = reader["Name"].ToString();
                                originalOrganization = reader["Organization"].ToString();
                                originalDepartment = reader["Department"].ToString();

                                TextBox4.Text = originalName;
                                TextBox5.Text = originalOrganization;
                                TextBox3.Text = originalDepartment;
                                TextBox1.Text = reader["Username"].ToString();
                            }
                            else
                            {
                                // User not found in the database, handle the scenario
                                // ...
                            }
                        }
                    }
                }
                else
                {
                    // Redirect to login page if session userID is null
                    Response.Redirect("loginpage.aspx");
                }
            }
        }

        protected void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Get the user input from the text fields
                string name = TextBox4.Text.Trim();
                string organization = TextBox5.Text.Trim();
                string department = TextBox3.Text.Trim();

                // Validate that the input fields are not blank
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(organization) || string.IsNullOrEmpty(department))
                {
                    // Show an alert if any of the fields are blank
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Blank details are not allowed.');", true);
                }
                else
                {
                    // Check if there are any changes in the user input
                    if (name == originalName && organization == originalOrganization && department == originalDepartment)
                    {
                        // Show an alert if there are no changes
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No changes detected.');", true);
                    }
                    else
                    {
                        // Update the user information in the database
                        string query = "UPDATE Users SET [Name] = @Name, [Organization] = @Organization, [Department] = @Department WHERE [UserID] = @UserID";

                        using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Name", name);
                                command.Parameters.AddWithValue("@Organization", organization);
                                command.Parameters.AddWithValue("@Department", department);
                                command.Parameters.AddWithValue("@UserID", Session["userID"].ToString());

                                connection.Open();
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Update the original values for future comparisons
                                    originalName = name;
                                    originalOrganization = organization;
                                    originalDepartment = department;

                                    // Update Session variables with new values
                                    Session["username"] = TextBox1.Text;
                                    Session["fname"] = name;

                                    // Show a success message and redirect to userInformations.aspx
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Updated successfully.'); window.location.href='userInformations.aspx';", true);
                                }
                                else
                                {
                                    // Show an error message if the update fails
                                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to update.');", true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the update process
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('An error occurred while updating.');", true);
            }
        }
    }
}
