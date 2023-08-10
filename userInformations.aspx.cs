using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Resume_Project_CRUD_Board
{
    public partial class userInformations : Page
    {
        private string originalName;
        private string originalOrganization;
        private string originalDepartment;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["username"] != null)
                {
                    // Get the user information from the database based on the session username
                    string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
                    string query = "SELECT [Name], [Organization], [Department], [User_Name], [Password] FROM user_information WHERE [User_Name] = @UserName";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@UserName", Session["username"].ToString());

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
                                TextBox1.Text = Session["username"].ToString();
                                TextBox2.Text = "********"; // Or any placeholder for the password
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
                    // Redirect to login page if session username is null
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
                        string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
                        string query = "UPDATE user_information SET [Name] = @Name, [Organization] = @Organization, [Department] = @Department WHERE [User_Name] = @UserName";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Name", name);
                                command.Parameters.AddWithValue("@Organization", organization);
                                command.Parameters.AddWithValue("@Department", department);
                                command.Parameters.AddWithValue("@UserName", Session["username"].ToString());

                                connection.Open();
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Show a success message and refresh the page
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
