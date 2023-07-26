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
    }
}