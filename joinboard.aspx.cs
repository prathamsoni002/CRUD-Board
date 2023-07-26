using CRUDBoadr_logs;
using log4net;
using System;
using System.Collections.Generic;
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
        string strcon = "Data Source=.;Initial Catalog = CRUDBoardDB; Integrated Security = True";
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
                        if (!checkAlreadyJoined())
                        {
                            joinNewBoard();
                            string Message = TextBox1.Text.ToString() + " Joined successfully.";
                            string script = "alert('" + Message + "'); ";
                            ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                            return;
                        }
                        else
                        {
                            string script = "alert('You are already a member of this Board.'); ";
                            ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                            return;
                        }
                            
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
        // Following are the two different code for checking Board existance and validating the password. This tow seperate functions could be merged into one single function also.
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
                string script = "alert('Error While checking the existance of the board and the validity of the password.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return false;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        bool checkSecretKey()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                SqlCommand cmd = new SqlCommand("select * from BoardDetailsDB where Board_ID='" + TextBox1.Text.Trim() + "'AND Secret_Key='" + TextBox2.Text.Trim() + "'", con);
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
                string script = "alert('Error While checking the existance of the board and the validity of the password1.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return false;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        bool checkAlreadyJoined()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                string usersabd_table_name = "USER_" + Session["username"].ToString() + Session["password"].ToString() + "sabd";
                SqlCommand cmd = new SqlCommand($"select * from {usersabd_table_name} where [Board ID]='" + TextBox1.Text.Trim() + "'AND [Board Secret Key]='" + TextBox2.Text.Trim() + "'", con);
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
                string script = "alert('Error While checking wether the Board already Joined or not by the user.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return true;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        void joinNewBoard()
        {
            log.Info("The code is entered the in the function to join new board.");
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                string usersabd_table_name = "USER_" + Session["username"].ToString() + Session["password"].ToString() + "sabd";
                string query = $"insert into [{usersabd_table_name}] ([Board ID], [Board Secret Key], Members, Notifications) values (@boardID, @secretkey, 0, 0)";
                SqlCommand userSabd = new SqlCommand(query, con);

                userSabd.Parameters.AddWithValue("@boardID", TextBox1.Text.ToString());
                userSabd.Parameters.AddWithValue("@secretkey", TextBox2.Text.ToString());

                userSabd.ExecuteNonQuery();

                SqlCommand members_update = new SqlCommand($"UPDATE BoardDetailsDB SET Members = Members + 1 WHERE Board_ID= '{TextBox1.Text.ToString()}'", con);
                members_update.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                string script = "alert('Exception in the BoardID and Password validating Function for the given Board details.'); ";
                ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                return;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }

        }
    }
}