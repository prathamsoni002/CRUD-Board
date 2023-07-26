using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Resume_Project_CRUD_Board
{
    public partial class board : System.Web.UI.Page
    {
        string connectionString = "Data Source=.;Initial Catalog=CRUDBoardDB;Integrated Security=True";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["boardID"] != null)
                {
                    string boardID = Request.QueryString["boardID"].ToString();
                    boardIdLabel.Text = boardID;

                    BindStatements(boardID);
                }
            }
        }

        protected void BindStatements(string boardID)
        {
            string tableName = "BOARD_" + boardID + GetSecretKey(boardID) + "sabd";
            string query = $"SELECT Statement, Details, StatementBy, Timestamp, updated_by FROM {tableName} ORDER BY Timestamp DESC";

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

                        statementRepeater.DataSource = dataTable;
                        statementRepeater.DataBind();
                    }
                    else
                    {
                        noStatementsPanel.Visible = true;
                    }
                }
            }
        }

        protected string GetSecretKey(string boardID)
        {
            string query = "SELECT Secret_Key FROM BoardDetailsDB WHERE Board_ID = @BoardID";
            string secretKey = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BoardID", boardID);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        secretKey = result.ToString();
                    }
                }
            }

            return secretKey;
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            string statement = Request.Form["statementInput"];
            string description = Request.Form["descriptionInput"];
            string statementID = Session["username"].ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");
            string statementBy = Session["username"].ToString();
            DateTime timestamp = DateTime.Now;

            string tableName = "BOARD_" + boardIdLabel.Text + GetSecretKey(boardIdLabel.Text) + "sabd";
            string query = $"INSERT INTO {tableName} (StatementID, Statement, Details, StatementBy, Timestamp) VALUES (@StatementID, @Statement, @Details, @StatementBy, @Timestamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatementID", statementID);
                    command.Parameters.AddWithValue("@Statement", statement);
                    command.Parameters.AddWithValue("@Details", description);
                    command.Parameters.AddWithValue("@StatementBy", statementBy);
                    command.Parameters.AddWithValue("@Timestamp", timestamp);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            Response.Redirect(Request.RawUrl);
        }

        protected void deleteYesButton_Click(object sender, EventArgs e)
        {
            string statementBy = statementByHiddenField.Value;
            string timestamp = timestampHiddenField.Value;
            DateTime dateTime = DateTime.ParseExact(timestamp, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string formattedTimestamp = dateTime.ToString("yyyyMMddHHmmss");
            string statementID = statementBy + formattedTimestamp;

            string tableName = "BOARD_" + boardIdLabel.Text + GetSecretKey(boardIdLabel.Text) + "sabd";
            string query = $"DELETE FROM {tableName} WHERE StatementID = @StatementID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StatementID", statementID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            Response.Redirect(Request.RawUrl);
        }

        protected void editButton_Click(object sender, EventArgs e)
        {
            string statement = Request.Form["editStatementInput"];
            string description = Request.Form["editDescriptionInput"];
            string statementBy = statementByHiddenField.Value;
            string timestamp = timestampHiddenField.Value;
            DateTime dateTime = DateTime.ParseExact(timestamp, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            string formattedTimestamp = dateTime.ToString("yyyyMMddHHmmss");
            string statementID = statementBy + formattedTimestamp;

            string tableName = "BOARD_" + boardIdLabel.Text + GetSecretKey(boardIdLabel.Text) + "sabd";
            string query = $"UPDATE {tableName} SET Statement = @Statement, Details = @Details, updated_by = @UpdatedBy WHERE StatementID = @StatementID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Statement", statement);
                    command.Parameters.AddWithValue("@Details", description);
                    command.Parameters.AddWithValue("@UpdatedBy", Session["username"].ToString() + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@StatementID", statementID);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            Response.Redirect(Request.RawUrl);
        }
    }
}
