using CRUDBoadr_logs;
using log4net;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resume_Project_CRUD_Board
{
    public partial class joinboard : System.Web.UI.Page
    {
        private static readonly ILog log = LoggerHelper.GetLogger();
        string strcon = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            join_board_button.Click += new EventHandler(join_board_button_Click);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterForEventValidation(join_board_button.UniqueID);
            base.Render(writer);
        }

        protected void join_board_button_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    if (checkBoardExists() && checkSecretKey())
                    {
                        JoinNewBoard();

                        return;
                    }
                    else
                    {
                        string script = "alert('Invalid Board ID or Secret Key'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                        return;
                    }
                }
                catch
                {
                    string script = "alert('Error in 'MainFunction' while Joining the Board.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    return;
                }
            }
        }

        bool checkBoardExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Board WHERE Name = @boardName", con))
                    {
                        cmd.Parameters.AddWithValue("@boardName", TextBox1.Text.Trim());
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            return dt.Rows.Count >= 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Error While checking the existence of the board and the validity of the password.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return false;
            }
        }

        bool checkSecretKey()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Board WHERE Name = @boardName AND SecretKey = @secretKey", con))
                    {
                        cmd.Parameters.AddWithValue("@boardName", TextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@secretKey", TextBox2.Text.Trim());
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            return dt.Rows.Count >= 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Error While checking the existence of the board and the validity of the password.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return false;
            }
        }

        void JoinNewBoard()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                    // Get the board name and secret key entered by the user in TextBox1 and TextBox2
                    string boardName = TextBox1.Text.Trim();
                    string secretKey = TextBox2.Text.Trim();

                    // Assuming you have the user's UserID stored in a session variable named "userID"
                    if (Guid.TryParse(Session["userID"].ToString(), out Guid userID))
                    {
                        // Step 1: Get the Board ID by Name
                        using (SqlCommand getBoardIDCmd = new SqlCommand("SELECT BoardID FROM Board WHERE Name = @boardName", con))
                        {
                            getBoardIDCmd.Parameters.AddWithValue("@boardName", boardName);
                            Guid boardID = (Guid)getBoardIDCmd.ExecuteScalar();

                            if (boardID != Guid.Empty)
                            {
                                // Step 2: Check User Membership
                                using (SqlCommand checkMembershipCmd = new SqlCommand("SELECT * FROM UserBoard WHERE UserID = @userID AND BoardID = @boardID", con))
                                {
                                    checkMembershipCmd.Parameters.AddWithValue("@userID", userID);
                                    checkMembershipCmd.Parameters.AddWithValue("@boardID", boardID);

                                    using (SqlDataAdapter da = new SqlDataAdapter(checkMembershipCmd))
                                    {
                                        DataTable dt = new DataTable();
                                        da.Fill(dt);

                                        if (dt.Rows.Count == 0)
                                        {
                                            // User is not a member of the board, so add them to the UserBoard table
                                            using (SqlCommand addUserToBoardCmd = new SqlCommand("INSERT INTO UserBoard (UserBoardID, UserID, BoardID, JoinedAt) VALUES (@userBoardID, @userID, @boardID, GETDATE())", con))
                                            {
                                                addUserToBoardCmd.Parameters.AddWithValue("@userBoardID", Guid.NewGuid()); // Provide a new GUID for UserBoardID
                                                addUserToBoardCmd.Parameters.AddWithValue("@userID", userID);
                                                addUserToBoardCmd.Parameters.AddWithValue("@boardID", boardID);

                                                addUserToBoardCmd.ExecuteNonQuery();
                                            }
                                            string Message = TextBox1.Text.ToString() + " Joined successfully.";
                                            string script = "alert('" + Message + "'); ";
                                            ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                                        }
                                        else
                                        {
                                            string script = "alert('You are Already member of this Board.'); ";
                                            ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                                            return;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Handle the case where the board name does not exist
                                string script = "alert('Board does not exist.'); ";
                                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                            }
                        }
                    }
                    else
                    {
                        // Handle the case where Session["userID"] cannot be parsed into a Guid
                        string script = "alert('Invalid user ID.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    }
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Exception in joining the board.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
            }
        }
    }
}
