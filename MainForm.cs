using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContactManagement
{
    public partial class MainForm : Form
    {
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\btech\dotnet\ContactManagement\ContactManagerDB.mdf;Integrated Security=True";

        public MainForm()
        {
            InitializeComponent();
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count > 0)
            {
                int contactId = Convert.ToInt32(dgvContacts.SelectedRows[0].Cells["ContactId"].Value);
                ContactForm contactForm = new ContactForm(this, contactId);
                contactForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a contact to edit.", "No Contact Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvContacts.SelectedRows.Count > 0)
            {
                var confirmResult = MessageBox.Show("Are you sure you want to delete this contact?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        int contactId = Convert.ToInt32(dgvContacts.SelectedRows[0].Cells["ContactId"].Value);
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            string query = "DELETE FROM Contacts WHERE ContactId = @ContactId";
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@ContactId", contactId);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                        LoadContacts(); // Refresh the list
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to delete contact: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a contact to delete.", "No Contact Selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            ProfileForm profileForm = new ProfileForm();
            profileForm.ShowDialog();
        }
    }
}
