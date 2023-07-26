using CRUDBoadr_logs;
using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Resume_Project_CRUD_Board
{
    public partial class registration : System.Web.UI.Page
    {
        private static readonly ILog log = LoggerHelper.GetLogger();
        string strcon = "Data Source=.;Initial Catalog = CRUDBoardDB; Integrated Security = True";

        protected void Page_Load(object sender, EventArgs e)
        {
            Sign_up_button.Click += new EventHandler(Sign_up_button_Click);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterForEventValidation(Sign_up_button.UniqueID);
            base.Render(writer);
        }
        //sign up button is clicked.
        protected void Sign_up_button_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    if (checkMemberExists())
                    {
                        string script = "alert('Username is already taken.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                        return;
                    }
                    else
                    {
                        signUpNewUser();

                        string success_sign_up = "alert('New User Created.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", success_sign_up, true);
                    }
                }
                catch (Exception ex)
                {
                    string Message = "Error while Sign " + ex.Message + ex.StackTrace;
                    labelMessage.Text = Message;
                    Response.Redirect("homepage.aspx");
                }
                Response.Redirect("loginpage.aspx");
            }

        }

        // user defined methods


        // Checking if the username existis.

        bool checkMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                SqlCommand cmd = new SqlCommand("select * from user_information where User_Name='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

                con.Close();

            }
            catch (Exception ex)
            {
                string message = "This is a message." + ex.Message + ex.StackTrace;
                labelMessage.Text = message;
                return false;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        //Creating User
        void signUpNewUser()
        {
            log.Info("The code is entered the on click button");
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                SqlCommand cmd = new SqlCommand("insert into user_information (Name, Organization, Department, User_Name, Password) values(@fname, @orgz, @depart, @username, @pass)", con);

                cmd.Parameters.AddWithValue("@fname", TextBox4.Text.ToString());
                cmd.Parameters.AddWithValue("@orgz", TextBox5.Text.ToString());
                cmd.Parameters.AddWithValue("@depart", TextBox3.Text.ToString());
                cmd.Parameters.AddWithValue("@username", TextBox1.Text.ToString());
                cmd.Parameters.AddWithValue("@pass", TextBox2.Text.ToString());

                cmd.ExecuteNonQuery();

                // Creating the User Specific Active Board Details
                string tableName = "USER_"+ TextBox1.Text.ToString() + TextBox2.Text.ToString() + "sabd";
                SqlCommand createTableCmd = new SqlCommand($"CREATE TABLE {tableName} ([Board ID] varchar(30) not null, [Board Secret Key] varchar(30) not null, Members int null, Notifications int null)", con);
                createTableCmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception ex)
            {
                string message = "This is a message." + ex.Message + ex.StackTrace;
                labelMessage.Text = message;
                return;
                //Response.Write("<script>alert('" + ex.Message + "');</script>");
            }

            
        }
    }
}
