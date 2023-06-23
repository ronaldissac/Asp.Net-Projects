using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Omegashipping.com
{
    public partial class WebForm2 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["track"] != null)
                {
                    string selectedTrackID = Request.QueryString["track"];
                    Track.Text = selectedTrackID;
                    FetchData(selectedTrackID);
                }
            }
        }

        private void FetchData(string selectedTrackID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT FromLocation, ToLocation, Payment, Estimation, Location, Status, Vessel FROM Export WHERE Track = @TrackID", connection))
                {
                    command.Parameters.AddWithValue("@TrackID", selectedTrackID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            From.Text = reader["FromLocation"].ToString();
                            To.Text = reader["ToLocation"].ToString();
                            Payment.Text = reader["Payment"].ToString();
                            Estimation.Text = reader["Estimation"].ToString();
                            Location.Text = reader["Location"].ToString();
                            Status.Text = reader["Status"].ToString();
                            Vessel.Text = reader["Vessel"].ToString();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Status is not yet updated');", true);
                    }

                    reader.Close();
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string track = Track.Text;

            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Fetch data from the Export table based on the track ID
                string selectQuery = "SELECT * FROM Export WHERE Track = @TrackID";
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@TrackID", track);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            From.Text = GetColumnValue(reader, "FromLocation");
                            To.Text = GetColumnValue(reader, "ToLocation");
                            Payment.Text = GetColumnValue(reader, "Payment");
                            Estimation.Text = GetColumnValue(reader, "Estimation");
                            Location.Text = GetColumnValue(reader, "Location");
                            Status.Text = GetColumnValue(reader, "Status");
                            Vessel.Text = GetColumnValue(reader, "Vessel");
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", $"alert('Status is not yet updated');", true);
                    }

                    reader.Close();
                }
            }
        }

        private string GetColumnValue(SqlDataReader reader, string columnName)
        {
            if (reader[columnName] != DBNull.Value)
            {
                return reader[columnName].ToString();
            }
            else
            {
                return "Not Yet Updated";
            }
        }

    }
}
