using CRUDBoadr_logs;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                        string script = "alert('"+Message+"'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                        return;
                    }
                    else
                    {
                       string script = "alert('Board with the same Board ID exists already. Please choose another ID.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                        return;
                    }
                }
                catch
                {
                    string script = "alert('Error in 'MainFunction' while creating the Board.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    return;
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

                SqlCommand cmd = new SqlCommand("select * from BoardDetailsDB where Board_ID='" + TextBox1.Text.Trim() + "';", con);
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
                string script = "alert('Error While checking the existance of the board.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return false;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        //Creating Board
        void createNewBoard()
        {
            log.Info("The code is entered the in the function to create new board.");
            try
            {
                if(TextBox2.Text.ToString() == TextBox3.Text.ToString())
                {
                    //Adding the Board Detains to BoardDetailsDB:
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                    SqlCommand cmd = new SqlCommand("insert into BoardDetailsDB (Board_ID, Secret_Key, Members) values(@boardID, @secretkey, 1)", con);

                    cmd.Parameters.AddWithValue("@boardID", TextBox1.Text.ToString());
                    cmd.Parameters.AddWithValue("@secretkey", TextBox3.Text.ToString());

                    cmd.ExecuteNonQuery();

                    //adding details in the user sabd:
                    string usersabd_table_name = "USER_"+ Session["username"].ToString() + Session["password"].ToString() + "sabd";
                    string query = $"insert into [{usersabd_table_name}] ([Board ID], [Board Secret Key], Members, Notifications) values (@boardID, @secretkey, 0, 0)";
                    SqlCommand userSabd = new SqlCommand(query, con);

                    userSabd.Parameters.AddWithValue("@boardID", TextBox1.Text.ToString());
                    userSabd.Parameters.AddWithValue("@secretkey", TextBox3.Text.ToString());

                    userSabd.ExecuteNonQuery();

                    // Creating the Board Specific Active Board Details:
                    string tableName = "BOARD_" + TextBox1.Text.ToString() + TextBox3.Text.ToString() + "sabd";
                    SqlCommand createTableCmd = new SqlCommand($"CREATE TABLE {tableName} ([Statement] varchar(50) not null, [Details] varchar(1000) not null, StatementID varchar(30) not null, StatementBy varchar(60) not null, [Timestamp] datetime not null, [updated_by] varchar(255) null)", con);
                    createTableCmd.ExecuteNonQuery();

                    con.Close();

                }
                else
                {
                    string script = "alert('The Secret Keys do no match.'); ";
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                    return;
                }

            }
            catch (Exception ex)
            {
                string script = "alert('Exception while creating the Board.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }


        }
    }
}