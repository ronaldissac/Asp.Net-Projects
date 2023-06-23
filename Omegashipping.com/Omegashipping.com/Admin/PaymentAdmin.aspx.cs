using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Omegashipping.com
{
    public partial class WebForm7 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTrackingIDs();
            }
        }

        protected void Trackbtn_Click(object sender, EventArgs e)
        {
            string selectedTrack = DropDownList1.SelectedItem.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Estimation,Payment,Amount FROM Export WHERE Track = @TrackingID";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrackingID", selectedTrack);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                    else
                    {
                        // Track not found in the table
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Handle the exception
                }
            }
        }
        private void BindTrackingIDs()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT Track FROM Export";

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable trackingTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(trackingTable);

                    if (trackingTable.Rows.Count > 0)
                    {
                        DropDownList1.DataSource = trackingTable;
                        DropDownList1.DataTextField = "Track";
                        DropDownList1.DataValueField = "Track";
                        DropDownList1.DataBind();
                    }
                    else
                    {
                        // No tracking IDs found in the table
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception
                }
            }
        }
    }

}