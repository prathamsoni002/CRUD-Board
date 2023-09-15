using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resume_Project_CRUD_Board
{
    public partial class Userhomepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBoards();
                username_link.Visible = true;
                username_link.Text = Session["fname"].ToString();
            }
        }
        protected void joinButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("joinboard.aspx");
        }
        protected void logoutButton_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                Response.Cookies.Clear();
                string script = "alert('Logged Out Successfully.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
            }
            catch (Exception ex)
            {
                string script = "alert('There was an Exception while logging out. Error: " + ex.Message + "'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
            }

            Response.Redirect("loginpage.aspx");
        }
        protected void createBoardButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("createboard.aspx");
        }
        protected void username_link_Click(object sender, EventArgs e)
        {
            Response.Redirect("userInformations.aspx");
        }
        protected void BindBoards()
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = @"
                SELECT UB.BoardID, B.Name, B.SecretKey
                FROM UserBoard UB
                INNER JOIN Board B ON UB.BoardID = B.BoardID
                WHERE UB.UserID = @UserID
                ORDER BY UB.JoinedAt DESC
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", Session["UserID"]);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        boardRepeater.DataSource = dataTable;
                        boardRepeater.DataBind();
                    }
                    else
                    {
                        noBoardPanel.Visible = true;
                    }
                }
            }
        }

        protected void boardNameLink_Click(object sender, EventArgs e)
        {
            LinkButton boardNameLink = (LinkButton)sender;
            string boardName = boardNameLink.Text;

            Response.Redirect($"board.aspx?boardName={Server.UrlEncode(boardName)}");
        }

        protected void leaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string boardName = boardNameHiddenField.Value;
                string secretKey = deleteBoardSecretKeyInput.Text.Trim();

                bool isSecretKeyValid = CheckSecretKey(boardName, secretKey);

                if (isSecretKeyValid)
                {
                    RemoveUserFromUserBoard(boardName);

                    int memberCount = GetMemberCount(boardName);

                    if (memberCount == 0)
                    {
                        DeleteBoard(boardName);

                        string successMessage = $"Successfully left and deleted the board {boardName}";
                        string script = $"alert('{successMessage}'); window.location.href = 'Userhomepage.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    }
                    else
                    {
                        string successMessage = $"Successfully left the board {boardName}";
                        string script = $"alert('{successMessage}'); window.location.href = 'Userhomepage.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    }
                }
                else
                {
                    string errorMessage = "Wrong Secret Key! Please try again.";
                    string script = $"alert('{errorMessage}');";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Exception while leaving the board. Error: " + ex.Message;
                string script = $"alert('{errorMessage}');";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
            }
        }

        private bool CheckSecretKey(string boardName, string secretKey)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = "SELECT 1 FROM Board WHERE Name = @BoardName AND SecretKey = @SecretKey";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardName", boardName);
                    command.Parameters.AddWithValue("@SecretKey", secretKey);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int exists))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void RemoveUserFromUserBoard(string boardName)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = "DELETE FROM UserBoard WHERE UserID = @UserID AND BoardID = (SELECT BoardID FROM Board WHERE Name = @BoardName)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", Session["UserID"]);
                    command.Parameters.AddWithValue("@BoardName", boardName);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private int GetMemberCount(string boardName)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = @"
                SELECT COUNT(*) AS MemberCount
                FROM UserBoard
                WHERE BoardID = (SELECT BoardID FROM Board WHERE Name = @BoardName)
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardName", boardName);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int memberCount))
                    {
                        return memberCount;
                    }
                }
            }

            return 0;
        }

        private string GetBoardID(string boardName)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
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

        private void DeleteBoard(string boardName)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string boardID = GetBoardID(boardName);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string deleteStatementsQuery = "DELETE FROM Statement WHERE BoardID = @BoardID";
                    using (SqlCommand deleteStatementsCommand = new SqlCommand(deleteStatementsQuery, connection, transaction))
                    {
                        deleteStatementsCommand.Parameters.AddWithValue("@BoardID", boardID);
                        deleteStatementsCommand.ExecuteNonQuery();
                    }

                    string deleteBoardQuery = "DELETE FROM Board WHERE BoardID = @BoardID";
                    using (SqlCommand deleteBoardCommand = new SqlCommand(deleteBoardQuery, connection, transaction))
                    {
                        deleteBoardCommand.Parameters.AddWithValue("@BoardID", boardID);
                        deleteBoardCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        protected void cancelBtn_Click(object sender, EventArgs e)
        {
            // The decision to display an alert here is left to your discretion.
        }
    }
}
