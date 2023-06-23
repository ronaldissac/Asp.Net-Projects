using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Omegashipping.com.Admin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if a track ID is provided in the query string
                if (Request.QueryString["trackID"] != null)
                {
                    string selectedTrackID = Request.QueryString["trackID"];
                    selectedTrackID = PopulateTrackDropdown(selectedTrackID); // Update the selectedTrackID variable with the returned value
                    FetchData(selectedTrackID);
                }
                else
                {
                    string selectedTrackID = PopulateTrackDropdown(null); // Pass null when no track ID is provided
                    FetchData(selectedTrackID);
                }
            }
        }

        protected string PopulateTrackDropdown(string selectedTrackID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            string query = "SELECT Track FROM Export ORDER BY CreatedDate DESC";
            string selectedTrack = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Clear existing items in the dropdown list
                        Track.Items.Clear();

                        // Loop through the data reader and add items to the dropdown list
                        while (reader.Read())
                        {
                            string trackName = reader["Track"].ToString();
                            ListItem item = new ListItem(trackName);

                            if (selectedTrackID != null && trackName == selectedTrackID)
                            {
                                item.Selected = true; // Set the item as selected if it matches the selected track ID
                                selectedTrack = selectedTrackID; // Set the selectedTrack variable
                            }

                            Track.Items.Add(item);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            if (selectedTrack == null)
            {
                // If no track is selected, set the selectedTrack to the first item in the dropdown list
                selectedTrack = Track.SelectedValue;
            }

            return selectedTrack; // Return the selected track ID
        }




        private void FetchData(string selectedTrack)
        {
            string fromAddress = "";
            string toAddress = "";
            string payment = "";

            // Fetch data from the "Export" table based on the selected track
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            string exportQuery = "SELECT FromLocation, ToLocation ,Payment FROM Export WHERE Track = @Track";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(exportQuery, connection))
                {
                    command.Parameters.AddWithValue("@Track", selectedTrack);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // Since the query returns a single row, you can use if instead of while loop
                        if (reader.Read())
                        {
                            fromAddress = reader["FromLocation"].ToString();
                            toAddress = reader["ToLocation"].ToString();
                            payment = reader["Payment"].ToString();
                        }
                    }

                    reader.Close();
                }
            }

            From.Text = fromAddress;
            To.Text = toAddress;
            Payment.Text = payment;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            string track = Track.SelectedValue;
            string status = Status.SelectedItem.Text;
            string fromLocation = From.Text;
            string toLocation = To.Text;
            string location = Location.Text;
            string vessel = Vessel.SelectedItem.Text;
           


            // Validate Estimation as integer
            int estimation;
            if (!int.TryParse(Estimation.Text, out estimation))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Estimation must be an integer.');", true);
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if the track exists in the Export table
                string checkQuery = "SELECT COUNT(*) FROM Export WHERE Track = @Track";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@Track", track);
                    int count = (int)checkCommand.ExecuteScalar();
                    string updateQuery;
                     if (count == 1)
                    {
                        updateQuery = "UPDATE Export SET Location = @Location, Estimation = @Estimation, Status = @Status, Vessel = @Vessel WHERE Track = @Track";


                    }
                    else
                    {
                        updateQuery = "UPDATE Export SET Location = @Location, Estimation = @Estimation, Status = @Status, Vessel = @Vessel WHERE Track = @Track";


                    }

                    // Execute the update query based on the track existence

                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                    {

                        updateCommand.Parameters.AddWithValue("@Track", track);
                        updateCommand.Parameters.AddWithValue("@Location", location);
                        updateCommand.Parameters.AddWithValue("@Estimation", estimation);
                        updateCommand.Parameters.AddWithValue("@Status", status);
                        updateCommand.Parameters.AddWithValue("@Vessel", vessel);

                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Status Updated Successfully');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Error From server');", true);
                        }
                    }
                }
            }
        }

        protected void Track_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Retrieve the selected track from the dropdown list
            string selectedTrack = Track.SelectedValue;

            // Fetch the data from the "Export" table based on the selected track
            FetchData(selectedTrack);
        }
    }
}
