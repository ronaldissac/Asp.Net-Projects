﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Omegashipping.com.Login
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void TrackButton_Click(object sender, EventArgs e)
        {
            string trackID = Track.Text;

            // Check if the track exists in the Export_Status table
            if (!IsTrackExists(trackID))
            {
                // Display alert message for non-existing track
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('TrackID does not exist.');", true);
                return;
            }

            // Fetch distinct data from the database based on the track column
            DataTable dt = GetDistinctDataFromDatabase(trackID);

            // Bind the fetched data to the GridView
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private bool IsTrackExists(string trackID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            int count = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string checkQuery = "SELECT COUNT(*) FROM Export_Status WHERE Track = @TrackID";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@TrackID", trackID);
                    count = (int)checkCommand.ExecuteScalar();
                }
            }

            return count > 0;
        }

        private DataTable GetDistinctDataFromDatabase(string trackID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT FromLocation, ToLocation, Status, Location FROM Export_Status WHERE Track = @TrackID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TrackID", trackID);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        private void BindGridView()
        {
            // Fetch initial data for GridView (if needed)
            DataTable dt = new DataTable(); // Replace with your logic to fetch data

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string userType = UserTypeDropdown.SelectedValue;
            string userID = UserID.Text;
            string password = Password.Text;
            bool isValid = false;

            // Call the stored procedure to validate the login
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["dis"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("ValidateLogin", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    command.Parameters.AddWithValue("@UserType", userType);
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@Password", password);

                    // Output parameter
                    SqlParameter isValidParam = new SqlParameter("@IsValid", SqlDbType.Bit);
                    isValidParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(isValidParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Retrieve the value of the output parameter
                    isValid = (bool)isValidParam.Value;
                }
            }

            if (isValid)
            {
                // Login successful
                if (userType == "Admin")
                {
                    // Redirect to admin export page
                    // Show credentials message using ScriptManager
                    string script = "alert('Login Successfull');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showInvalidCredentials", script, true);
                    Response.Redirect("~/Admin/ExportAdmin.aspx");
                }
                else if (userType == "User")
                {
                    // Redirect to user export page
                    string script = "alert('Login Successfull');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showInvalidCredentials", script, true);
                    Response.Redirect("~/Export.aspx");
                }
            }
            else
            {
                // Show invalid credentials message using ScriptManager
                string script = "alert('Invalid credentials. Please try again.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "showInvalidCredentials", script, true);
            }
        }



    }
}