using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Omegashipping.com
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate the dropdown list with data from the "export" table
                PopulateDropdownList();

                // Check if a track ID is selected in the dropdown list
                if (DropDownList.SelectedIndex > -1)
                {
                    // Retrieve the selected track ID from the dropdown list
                    string selectedTrackID = DropDownList.SelectedValue;

                    // Fetch the data from the "export" table based on the selected track ID
                    FetchData(selectedTrackID);
                }
            }
        }

        private void FetchData(string selectedTrackID)
        {
            // Fetch the data from the "export" table based on the selected track ID
            string productName = "";
            string quantity = "";
            string fromAddress = "";
            string toAddress = "";
            byte[] imageData = null; // assuming the filedata column is of type varbinary(max)
            string date = "";

            // Use your preferred method to fetch data from the "export" table based on the selected track ID
            // Example: using ADO.NET and SQLDataReader

            // Assuming you have a table named "export" with the required columns

            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM export WHERE track = @TrackID", connection))
                {
                    command.Parameters.AddWithValue("@TrackID", selectedTrackID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Retrieve the data from each column
                            productName = reader["Productname"].ToString();
                            quantity = reader["Quantity"].ToString();
                            fromAddress = reader["FromLocation"].ToString();
                            toAddress = reader["ToLocation"].ToString();
                            imageData = (byte[])reader["filedata"];
                            date = reader["CreatedDate"].ToString();
                        }
                    }

                    reader.Close();
                }
            }

            // Display the fetched data on the page
            productname.Text = productName;
            Quantity.Text = quantity;
            from.Text = fromAddress;
            To.Text = toAddress;
            Image1.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(imageData);
            date1.Text = date;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Retrieve the selected track ID from the dropdown list
            string selectedTrackID = DropDownList.SelectedValue;

            // Redirect to the "StatusAdmin.aspx" page with the selected track ID as a query parameter
            Response.Redirect("StatusAdmin.aspx?trackID=" + selectedTrackID);
        }

        private void PopulateDropdownList()
        {
            // Fetch the track IDs from the "export" table and populate the dropdown list
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT track FROM export", connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            // Add each track ID to the dropdown list
                            DropDownList.Items.Add(reader["track"].ToString());
                        }
                    }

                    reader.Close();
                }
            }
        }

        protected void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Retrieve the selected track ID from the dropdown list
            string selectedTrackID = DropDownList.SelectedValue;

            // Fetch the data from the "export" table based on the selected track ID
            FetchData(selectedTrackID);
        }
    }
}
