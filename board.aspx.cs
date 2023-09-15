using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Resume_Project_CRUD_Board
{
    public partial class board : System.Web.UI.Page
    {
        private string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["boardName"] != null)
                {
                    string boardName = Request.QueryString["boardName"].ToString();
                    boardIdLabel.Text = boardName;

                    BindStatements(boardName);
                }
            }
        }

        protected void BindStatements(string boardName)
        {
            string query = @"
            SELECT 
                S.StatementID, 
                S.Title, 
                S.Description, 
                UC.Username as CreatedBy, 
                UE.Username as UpdatedBy, 
                S.CreatedAt, 
                S.UpdatedAt
            FROM Statement S
            INNER JOIN Users UC ON S.CreatedBy = UC.UserID
            LEFT JOIN Users UE ON S.UpdatedBy = UE.UserID
            WHERE S.BoardID = (SELECT BoardID FROM Board WHERE Name = @BoardName)
            ORDER BY S.CreatedAt DESC
    ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardName", boardName);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        statementRepeater.DataSource = dataTable;
                        statementRepeater.DataBind();
                    }
                    else
                    {
                        noStatementsPanel.Visible = true;
                    }
                }
            }
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                string title = Request.Form["statementInput"];
                string description = Request.Form["descriptionInput"];

                if (title == "" || description == "")
                {
                    string script = "alert('The Statement or Description cannot be empty.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    return;
                }

                // Get the user's UserID from the session
                if (Guid.TryParse(Session["userID"].ToString(), out Guid createdByUserID))
                {
                    string boardName = boardIdLabel.Text;
                    string boardID = GetBoardID(boardName);

                    // Generate a new GUID for the statement ID
                    Guid statementID = Guid.NewGuid();

                    string query = @"
                INSERT INTO Statement (StatementID, BoardID, Title, Description, CreatedBy, CreatedAt, UpdatedAt)
                VALUES (@StatementID, @BoardID, @Title, @Description, @CreatedBy, @CreatedAt, @UpdatedAt)
            ";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@StatementID", statementID);
                            command.Parameters.AddWithValue("@BoardID", boardID);
                            command.Parameters.AddWithValue("@Title", title);
                            command.Parameters.AddWithValue("@Description", description);
                            command.Parameters.AddWithValue("@CreatedBy", createdByUserID);
                            command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                            command.Parameters.AddWithValue("@UpdatedAt", DBNull.Value);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    string script = "alert('Invalid user ID.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Exception while adding the statement.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
            }
        }

        protected void deleteYesButton_Click(object sender, EventArgs e)
        {
            string statementID = statementIDHiddenField.Value; // Use the function to get the statement ID

            string query = "DELETE FROM Statement WHERE StatementID = @StatementID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatementID", statementID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            Response.Redirect(Request.RawUrl);
        }

        // Add a new method to get the username based on UserID
        private string GetUsername(string userID)
        {
            string query = "SELECT Username FROM Users WHERE UserID = @UserID";
            string username = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        username = result.ToString();
                    }
                }
            }

            return username;
        }

        protected void editButton_Click(object sender, EventArgs e)
        {
            string statementID = statementIDHiddenField.Value;
            string title = Request.Form["editStatementInput"];
            string description = Request.Form["editDescriptionInput"];
            string updatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (title == "" || description == "")
            {
                string script = "alert('The Statement or Description cannot be empty.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
            }
            // Get the current user's ID from the session
            if (Guid.TryParse(Session["userID"].ToString(), out Guid updatedByUserID))
            {
                string query = @"
            UPDATE Statement
            SET Title = @Title, Description = @Description, UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy
            WHERE StatementID = @StatementID
        ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@UpdatedAt", updatedAt);
                        command.Parameters.AddWithValue("@UpdatedBy", updatedByUserID);
                        command.Parameters.AddWithValue("@StatementID", statementID);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                Response.Redirect(Request.RawUrl);
            }
            else
            {
                string script = "alert('Invalid user ID.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
            }
        }

        private string GetBoardID(string boardName)
        {
            string query = "SELECT BoardID FROM Board WHERE Name = @BoardName";
            string boardID = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardName", boardName);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        boardID = result.ToString();
                    }
                }
            }

            return boardID;
        }
    }
}
