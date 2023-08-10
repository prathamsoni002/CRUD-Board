using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resume_Project_CRUD_Board
{
    public partial class Userhomepage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (!IsPostBack)
            {*/
            BindBoards();
            username_link.Visible = true;
            username_link.Text = Session["fname"].ToString();
            //}
        }


        protected void joinButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("joinboard.aspx");
        }

        protected void logoutButton_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    // Clear session and redirect to login page
                    Session.Abandon();
                    Session.Clear();
                    Response.Cookies.Clear();
                    string script = "alert('Logged Out Successfully.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                }
                catch (Exception ex)
                {
                    string script = "alert('There was an Exception while logging out.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                }

                Response.Redirect("loginpage.aspx");
            }
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
            // Get the board IDs and notifications for the current user from the user-specific table
            string userTableName = "USER_" + Session["username"] + Session["password"] + "sabd";
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = $"SELECT [Board ID], Notifications FROM {userTableName}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Add the "MemberNumber" column if it doesn't exist
                        if (!dataTable.Columns.Contains("MemberNumber"))
                        {
                            dataTable.Columns.Add("MemberNumber");
                        }

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string boardID = row["Board ID"].ToString();
                            string notifications = row["Notifications"].ToString();

                            // Get the member number from the BoardDetailsDB table
                            int memberNumber = GetMemberNumber(boardID);

                            row["MemberNumber"] = memberNumber;
                        }

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


        protected int GetMemberNumber(string boardID)
        {
            // Get the member number from the BoardDetailsDB table based on the board ID
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = "SELECT Members FROM BoardDetailsDB WHERE Board_ID = @BoardID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardID", boardID);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int memberNumber))
                    {
                        return memberNumber;
                    }
                }
            }

            return 0; // Default member number if not found or an error occurred
        }

        protected void boardNameLink_Click(object sender, EventArgs e)
        {
            LinkButton boardNameLink = (LinkButton)sender;
            string boardID = boardNameLink.Text;

            Response.Redirect($"board.aspx?boardID={boardID}");
        }

        protected void leaveBtn_Click(object sender, EventArgs e)
        {
            try
            {

                // Get the board ID and secret key from the popup
                string boardID = boardNameHiddenField.Value;
                string secretKey = deleteBoardSecretKeyInput.Text.Trim();


                // Check if the secret key matches the one in BoardDetailsDB table
                bool isSecretKeyValid = CheckSecretKey(boardID, secretKey);

                if (isSecretKeyValid)
                {
                    // Get the member number for the board
                    int memberNumber = GetMemberNumber(boardID);

                    // If member number is 1, delete the entire board
                    if (memberNumber == 1)
                    {
                        // Delete the board details from the user-specific table
                        DeleteBoardFromUserTable(boardID);

                        // Delete the table for the board from the database
                        string tableName = "BOARD_" + boardID + secretKey + "sabd";
                        DeleteBoardTable(tableName);

                        // Delete the row from BoardDetailsDB table
                        DeleteBoardFromBoardDetailsDB(boardID);

                        string successMessage = $"Successful Exit from {boardID}";
                        string script = $"alert('{successMessage}'); window.location.href = 'Userhomepage.aspx';";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    }
                    else
                    {
                        // Subtract 1 from the member number in BoardDetailsDB table
                        UpdateMemberNumber(boardID, memberNumber - 1);

                        // Delete the row from the user-specific table
                        DeleteBoardFromUserTable(boardID);

                        string successMessage = $"Successful Exit from {boardID}";
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
                string errorMessage = "Exception while removing Board. Error: " +ex.Message;
                string script = $"alert('{errorMessage}');";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
            }
        }

        private bool CheckSecretKey(string boardID, string secretKey)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = "SELECT 1 FROM BoardDetailsDB WHERE Board_ID = @BoardID AND Secret_Key = @SecretKey";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardID", boardID);
                    command.Parameters.AddWithValue("@SecretKey", secretKey);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int exists))
                    {
                        return true;
                    }
                }
            }

            return false; // If secret key does not match
        }

        private void DeleteBoardFromUserTable(string boardID)
        {
            string userTableName = "USER_" + Session["username"] + Session["password"] + "sabd";
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = $"DELETE FROM {userTableName} WHERE [Board ID] = @BoardID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardID", boardID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteBoardTable(string tableName)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = $"DROP TABLE {tableName}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteBoardFromBoardDetailsDB(string boardID)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = $"DELETE FROM BoardDetailsDB WHERE Board_ID = @BoardID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardID", boardID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateMemberNumber(string boardID, int memberNumber)
        {
            string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
            string query = $"UPDATE BoardDetailsDB SET Members = @MemberNumber WHERE Board_ID = @BoardID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberNumber", memberNumber);
                    command.Parameters.AddWithValue("@BoardID", boardID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }



        protected void cancelBtn_Click(object sender, EventArgs e)
        {

        }
    }
}