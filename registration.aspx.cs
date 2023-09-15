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
        string strcon = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {
            Sign_up_button.Click += Sign_up_button_Click;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterForEventValidation(Sign_up_button.UniqueID);
            base.Render(writer);
        }

        protected void Sign_up_button_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    if (checkMemberExists())
                    {
                        string script = "alert('It seems the Email or Username provided is already registered. Please try another.'); ";
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
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", Message, true);
                    Response.Redirect("homepage.aspx");
                }
                Response.Redirect("loginpage.aspx");
            }
        }

        bool checkMemberExists()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }

                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username=@username OR EmailID=@email", con))
                    {
                        cmd.Parameters.AddWithValue("@username", TextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", TextBox6.Text.Trim());
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
                string message = "Exception:: " + ex.Message + ex.StackTrace;
                labelMessage.Text = message;
                return false;
            }
        }

        void signUpNewUser()
        {
            log.Info("The code is entered the on click button");
            try
            {
                using (SqlConnection con = new SqlConnection(strcon))
                {
                    if (con.State == ConnectionState.Closed) { con.Open(); }

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Users (UserID, Name, Organization, Department, Username, Password, EmailID, CreatedAt, UpdatedAt) " +
                                    "VALUES (NEWID(), @fname, @orgz, @depart, @username, @pass, @email, GETDATE(), GETDATE())", con))
                    {
                        cmd.Parameters.AddWithValue("@fname", TextBox4.Text.ToString());
                        cmd.Parameters.AddWithValue("@orgz", TextBox5.Text.ToString());
                        cmd.Parameters.AddWithValue("@depart", TextBox3.Text.ToString());
                        cmd.Parameters.AddWithValue("@username", TextBox1.Text.ToString());
                        cmd.Parameters.AddWithValue("@pass", TextBox2.Text.ToString());
                        cmd.Parameters.AddWithValue("@email", TextBox6.Text.ToString());

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                string message = "This is a message." + ex.Message + ex.StackTrace;
                labelMessage.Text = message;
            }
        }
    }
}
