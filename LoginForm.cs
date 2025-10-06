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
    public partial class LoginForm : Form
    {
        // Connection string for the service-based database
        private readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\btech\dotnet\ContactManagement\ContactManagerDB.mdf;Integrated Security=True";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Username and password are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT UserId, Username FROM Users WHERE Username = @Username AND Password = @Password";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    // IMPORTANT: In a real-world application, ALWAYS hash passwords.
                    // This is a simplified example for clarity.
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Store logged in user's info
                        CurrentUser.UserId = Convert.ToInt32(reader["UserId"]);
                        CurrentUser.Username = reader["Username"].ToString();

                        // Hide the login form and show the main application form
                        this.Hide();
                        MainForm mainForm = new MainForm();
                        mainForm.FormClosed += (s, args) => this.Close(); // Close login form when main form closes
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred during login: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Show the registration form
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }
    }
}
