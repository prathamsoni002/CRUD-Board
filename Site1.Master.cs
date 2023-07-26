using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CRUDBoadr_logs;

namespace Resume_Project_CRUD_Board
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private static readonly ILog log = LoggerHelper.GetLogger();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
       
        protected void LinkButton6_Click(object sender, EventArgs e)
        {
           log.Debug("The login option on the footer is going to redierct");
           
           Response.Redirect("loginpage.aspx");
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            Response.Redirect("registration.aspx");
        }
    }
}