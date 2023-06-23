using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Omegashipping.com
{
    public partial class WebForm5 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string trackId = Track.Text;

            if (!string.IsNullOrEmpty(trackId))
            {
                // Check if the track detail exists in the Export_Status table
                if (IsTrackExist(trackId))
                {
                    // Fetch the details from the Export_Status table using the track
                    DataTable detailsTable = GetExportStatusDetails(trackId);

                    if (detailsTable.Rows.Count > 0)
                    {
                        DataRow row = detailsTable.Rows[0];

                        // Display the details in the respective textboxes
                        Status.Text = row["Status"].ToString();
                        From.Text = row["FromLocation"].ToString();
                        To.Text = row["ToLocation"].ToString();
                        Payment.Text = row["Payment"].ToString();
                        Estimation.Text = row["Estimation"].ToString();
                        Vessel.Text = row["Vessel"].ToString();
                        Location.Text = row["Location"].ToString();
                    }
                }
                else
                {
                    // Track detail doesn't exist in the table, show an alert message
                    ScriptManager.RegisterStartupScript(this, GetType(), "TrackNotFound", "alert('The provided tracking ID does not exist.');", true);
                }
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string trackId = Track.Text;

            if (!string.IsNullOrEmpty(trackId))
            {
                // Check if the track detail exists in the Export_Status table
                if (IsTrackExist(trackId))
                {
                    // Fetch the details from the Export_Status table using the track
                    DataTable detailsTable = GetExportStatusDetails(trackId);

                    if (detailsTable.Rows.Count > 0)
                    {
                        DataRow row = detailsTable.Rows[0];

                        // Store the details in the BL table and generate a unique BL number
                        string blNumber = StoreInBLTable(row);
                        string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
                        // Generate a PDF to display the details
                        GeneratePDF(connectionString, blNumber);
                    }
                }
                else
                {
                    // Track detail doesn't exist in the table, show an alert message
                    ScriptManager.RegisterStartupScript(this, GetType(), "TrackNotFound", "alert('The provided tracking ID does not exist.');", true);
                }
            }
            else
            {
                // Track detail doesn't exist in the table, show an alert message
                ScriptManager.RegisterStartupScript(this, GetType(), "TrackNotFound", "alert('The provided tracking ID does not exist.');", true);
            }
        }

        private bool IsTrackExist(string trackId)
        {
            // Check if the track detail exists in the Export_Status table
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Export WHERE Track = @TrackId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrackId", trackId);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private DataTable GetExportStatusDetails(string trackId)
        {
            // Fetch the details from the Export_Status table using the track
            DataTable detailsTable = new DataTable();
            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Export WHERE Track = @TrackId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TrackId", trackId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(detailsTable);
            }

            return detailsTable;
        }

        private string StoreInBLTable(DataRow sourceRow)
        {
            // Retrieve the selected track from the sourceRow
            string selectedTrack = sourceRow["Track"].ToString();

            // Check if the BL number already exists for the selected track
            string existingBLNumber = sourceRow["BLnumber"].ToString();
            string blNumber = string.IsNullOrEmpty(existingBLNumber) ? GenerateUniqueBLNumber() : existingBLNumber;

            string connectionString = ConfigurationManager.ConnectionStrings["dis"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Export SET BLnumber = @BLNumber WHERE Track = @Track";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BLNumber", blNumber);
                command.Parameters.AddWithValue("@Track", selectedTrack);

                command.ExecuteNonQuery();
            }

            return blNumber;
        }

    private string GenerateUniqueBLNumber()
        {
            // Generate a unique BL number
            // Implement your logic to generate a unique BL number here
            return Guid.NewGuid().ToString();
        }
        private void GeneratePDF(string connectionString, string blNumber)
        {
            // Fetch data from the database
            DataTable detailsTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT CreatedDate,Track,Location,Estimation,Status,BLnumber,CustomerName,Payment,Amount,Vessel FROM Export WHERE Track = @Track", connection);
                command.Parameters.AddWithValue("@Track",Track.Text);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(detailsTable);
            }

            // Generate a PDF to display the details
            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();

            document.Add(new Paragraph("OmegaShipping PVT LTD"));
            // Add the BL number as a QR code
            BarcodeQRCode qrCode = new BarcodeQRCode(blNumber, 100, 100, null);
            Image qrCodeImage = qrCode.GetImage();
            qrCodeImage.SetAbsolutePosition(50, document.PageSize.Height - 150);
            document.Add(qrCodeImage);

            // Add some spacing
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            // Add the details to the PDF
            PdfPTable table = new PdfPTable(detailsTable.Columns.Count);
            table.WidthPercentage = 100;

            // Add table headers
            foreach (DataColumn column in detailsTable.Columns)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName));
                table.AddCell(headerCell);
            }

            // Add table rows
            foreach (DataRow row in detailsTable.Rows)
            {
                foreach (DataColumn column in detailsTable.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(row[column].ToString()));
                    table.AddCell(cell);
                }
            }

            document.Add(table);
            document.Close();

            // Provide the PDF file for download
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=BL_" + blNumber + ".pdf");
            Response.BinaryWrite(memoryStream.ToArray());
            Response.End();
        }




    }
}
