using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Omegashipping.com
{
    public partial class WebForm4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["track"] != null)
                {
                    string selectedTrackID = Request.QueryString["track"];
                    Track.Text = selectedTrackID;
                }
            }
        }
            protected void Trackbtn_Click(object sender, EventArgs e)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
                string trackID = Track.Text;
                string query = "SELECT Estimation FROM Export WHERE Track = @trackID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@trackID", trackID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                   if (reader.Read())
{
    if (reader["Estimation"] != DBNull.Value)
    {
        string estimation = reader["Estimation"].ToString();
        Estimation.Text = estimation;
        payment.Text = estimation;
    }
    else
    {
        // Handle case when estimation is not yet updated for the given track ID
        Estimation.Text = "Estimation is not yet updated";
        payment.Text = string.Empty;
    }
}
else
{
    // Handle case when the track ID is invalid
    Estimation.Text = string.Empty;
    payment.Text = string.Empty;
    ScriptManager.RegisterStartupScript(this, GetType(), "InvalidTrackID", "alert('Track ID is invalid');", true);
}


                    reader.Close();
                }
            }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Get the connection string from web.config
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            // Get the payment amount from the textbox
            string paymentAmount = payment.Text;

            // Get the track ID from the textbox
            string trackID = Track.Text;
                try
                {
                    // Update the Export_Status table with the payment information
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Export SET Amount = @Amount, Payment = 'Payment Successful' WHERE Track = @TrackingID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Amount", paymentAmount);
                        command.Parameters.AddWithValue("@TrackingID", trackID);

                        command.ExecuteNonQuery();
                    }

                    // Display success message
                    ClientScript.RegisterStartupScript(GetType(), "PaymentSuccess", "alert('Payment successful.');", true);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occurred during the update process
                    // Display error message or perform necessary error handling
                    ClientScript.RegisterStartupScript(GetType(), "PaymentError", $"alert('An error occurred while updating the payment: {ex.Message}');", true);
                }
           
        }


    }
}