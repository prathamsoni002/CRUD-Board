using CRUDBoadr_logs;
using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Resume_Project_CRUD_Board
{
    public partial class createboard : System.Web.UI.Page
    {
        private static readonly ILog log = LoggerHelper.GetLogger();
        string strcon = "Data Source=.;Initial Catalog = CRUDBoardDB; Integrated Security = True";

        protected void Page_Load(object sender, EventArgs e)
        {
            create_board_button.Click += new EventHandler(create_board_button_Click);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterForEventValidation(create_board_button.UniqueID);
            base.Render(writer);
        }

        protected void create_board_button_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    if (!checkBoardExists())
                    {
                        createNewBoard();
                        string Message = TextBox1.Text.ToString() + " created successfully.";
                        string script = "alert('" + Message + "'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    }
                    else
                    {
                        string script = "alert('Board with the same Board ID exists already. Please choose another ID.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    }
                }
                catch (Exception ex)
                {
                    string script = "alert('Error in 'MainFunction' while creating the Board.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                }
            }
        }

        //Check if the board exists:
        bool checkBoardExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                SqlCommand cmd = new SqlCommand("select * from Board where Name='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    con.Close();
                    return true;
                }
                else
                {
                    con.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Error While checking the existence of the board.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return false;
            }
        }

        // Creating Board and adding the creator to UserBoard table
        void createNewBoard()
        {
            log.Info("The code is entered into the function to create a new board.");
            try
            {
                if (TextBox2.Text.ToString() == TextBox3.Text.ToString())
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                    // Get the user's UserID from the session
                    if (Guid.TryParse(Session["userID"].ToString(), out Guid createdByUserID))
                    {
                        // Generate a new GUID for the board ID
                        Guid boardID = Guid.NewGuid();

                        SqlCommand cmd = new SqlCommand("INSERT INTO Board (BoardID, Name, SecretKey, CreatedBy, CreatedAt, UpdatedAt) " +
                                                        "VALUES (@boardID, @name, @secretKey, @createdBy, GETDATE(), GETDATE())", con);

                        cmd.Parameters.AddWithValue("@boardID", boardID);
                        cmd.Parameters.AddWithValue("@name", TextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@secretKey", TextBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@createdBy", createdByUserID);

                        cmd.ExecuteNonQuery();

                        // Add the creator to the UserBoard table
                        SqlCommand addUserToUserBoardCmd = new SqlCommand("INSERT INTO UserBoard (UserBoardID, UserID, BoardID, JoinedAt) VALUES (NEWID(), @userID, @boardID, GETDATE())", con);
                        addUserToUserBoardCmd.Parameters.AddWithValue("@userID", createdByUserID);
                        addUserToUserBoardCmd.Parameters.AddWithValue("@boardID", boardID);

                        addUserToUserBoardCmd.ExecuteNonQuery();

                        // Close the connection after executing the queries
                        con.Close();
                    }
                    else
                    {
                        string script = "alert('Invalid user ID.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                        return;
                    }
                }
                else
                {
                    string script = "alert('The Secret Keys do not match.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    return;
                }
            }
            catch (Exception ex)
            {
                string script = "alert('Exception while creating the Board.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
            }
        }
    }
}
