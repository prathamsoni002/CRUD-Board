using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resume_Project_CRUD_Board
{
    public partial class loginpage : System.Web.UI.Page
    {
        string strcon = "Data Source=.;Initial Catalog = CRUDBoardDB; Integrated Security = True";
        protected void Page_Load(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(Button1_Click);
        }
        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager cs = Page.ClientScript;
            cs.RegisterForEventValidation(Button1.UniqueID);
            base.Render(writer);
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                try
                {
                    SqlConnection con = new SqlConnection(strcon);
                    if (con.State == System.Data.ConnectionState.Closed) { con.Open(); }

                    SqlCommand cmd = new SqlCommand("select * from Users where Username='" + TextBox1.Text.Trim() + "' AND Password='" + TextBox2.Text.Trim() + "'", con);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Session["username"] = dr.GetValue(4).ToString();
                            Session["password"] = dr.GetValue(5).ToString();
                            Session["fname"] = dr.GetValue(1).ToString();
                            Session["userID"] = dr.GetValue(0).ToString();
                        }
                    }
                    else
                    {
                        string script = "alert('Invalid Username or Password.'); ";
                        ClientScript.RegisterStartupScript(this.GetType(), "AlertScript", script, true);
                        return;
                    }
                }
                catch
                {

                }
                Response.Redirect("Userhomepage.aspx");
            }
         }

        protected void Button2a_Click(object sender, EventArgs e)
        {
            Response.Redirect("registration.aspx");
        }
    }
}