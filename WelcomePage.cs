using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library_system;
using Oracle.ManagedDataAccess.Client;

namespace Library_system
{
    public partial class WelcomePage : Form
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void branch_cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            branch_txt.Text = e.ClickedItem.Text;
            branch_txt2.Text = e.ClickedItem.Text;
        }


        private void register_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            login_pnl.Visible = false;
            register_pnl.Visible = true;
        }

        private void login_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            register_pnl.Visible = false;
            login_pnl.Visible = true;
        }

        private void branch_txt_Click(object sender, EventArgs e)
        {
            branch_cms.Show(branch_txt, new Point(0, branch_txt.Height));
        }

        private void branch_txt2_Click(object sender, EventArgs e)
        {
            branch_cms.Show(branch_txt2, new Point(0, branch_txt2.Height));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(logEmail_txt.Text) ||
                string.IsNullOrEmpty(branch_txt.Text) ||
                string.IsNullOrEmpty(logPass_txt.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }

            handleLogin();
        }

        private void handleLogin()
        {
            String email = logEmail_txt.Text.Trim();
            String password = logPass_txt.Text.Trim();
            String branch = branch_txt.Text.Trim();

            // First check hardcoded admin credentials (keeping existing functionality)
            if (email == "QCUBatasanAdmin" && password == "BatasanAdmin123" && branch == "Batasan")
            {
                admin.branch = "Batasan";
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
                return;
            }
            else if (email == "QCUSanFranciscoAdmin" && password == "SanFranciscoAdmin123" && branch == "San Francisco")
            {
                admin.branch = "San Francisco";
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
                return;
            }
            else if (email == "QCUSanBartolomeAdmin" && password == "SanBartolomeAdmin123" && branch == "San Bartolome")
            {
                admin.branch = "San Bartolome";
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
                return;
            }

            // If not admin, check regular user credentials in the database
            try
            {
                using (OracleConnection conn = new OracleConnection(DatabaseConnectionStr.connStr))
                {
                    conn.Open();
                    string query = @"
                        SELECT TYPE, BRANCH, STUDENT_ID, EMAIL, LAST_NAME, FIRST_NAME
                        FROM users 
                        WHERE email = :email 
                        AND password = :password";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("email", email));
                        cmd.Parameters.Add(new OracleParameter("password", password));

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Student.type = reader.GetString(0);
                                Student.branch = reader.GetString(1);
                                Student.studentId = reader.GetString(2);
                                Student.email = reader.GetString(3);
                                Student.lastName = reader.GetString(4);
                                Student.firstName = reader.GetString(5);
                                Student.targetBranch = branch;

                                UserInterface userInterface = new UserInterface();
                                userInterface.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid email or password. Please try again.");
                            }
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // Validate all required fields are filled
            if (string.IsNullOrEmpty(userType_txt.Text) ||
                string.IsNullOrEmpty(branch_txt2.Text) ||
                string.IsNullOrEmpty(fName_txt.Text) ||
                string.IsNullOrEmpty(lName_txt.Text) ||
                string.IsNullOrEmpty(regEmail_txt.Text) ||
                string.IsNullOrEmpty(regPass_txt.Text) ||
                string.IsNullOrEmpty(regCPass_txt.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }

            // Validate student ID for students
            if (userType_txt.Text == "Student" &&
                string.IsNullOrEmpty(studentId_txt.Text))
            {
                MessageBox.Show("Student ID is required for students.");
                return;
            }

            // Validate student ID format if applicable
            if (userType_txt.Text == "Student" && !IsValidStudentNumber(studentId_txt.Text))
            {
                MessageBox.Show("Invalid student ID format. Format should be YY-NNNN (e.g. 21-1234)");
                return;
            }

            // Check if student ID already exists (if student)
            if (userType_txt.Text == "Student" && IsStudentIdExists(studentId_txt.Text.Trim()))
            {
                MessageBox.Show("Student ID already exists. Please use a different ID or contact administrator.");
                return;
            }

            // Validate email format
            if (!IsValidEmail(regEmail_txt.Text))
            {
                MessageBox.Show("Please enter a valid email address.");
                return;
            }

            // Check if email already exists
            if (IsEmailExists(regEmail_txt.Text.Trim()))
            {
                MessageBox.Show("Email already exists. Please use a different email or try to recover your password.");
                return;
            }

            // Validate passwords match
            if (regCPass_txt.Text != regPass_txt.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Register the user
            RegisterUser();
        }


        private void RegisterUser()
        {
            try
            {
                string firstName = fName_txt.Text.Trim();
                string lastName = lName_txt.Text.Trim();
                string email = regEmail_txt.Text.Trim();
                string password = regPass_txt.Text;
                string userType = userType_txt.Text;
                string branch = branch_txt2.Text.Trim(); // Get branch value
                string studentId = (userType == "Student") ? studentId_txt.Text.Trim() : userType;

                // Check if student ID already exists (if provided)
                if (!string.IsNullOrEmpty(studentId) && IsStudentIdExists(studentId))
                {
                    MessageBox.Show("Student ID already exists. Please use a different ID or contact administrator.");
                    return;
                }

                // Check if email already exists
                if (IsEmailExists(email))
                {
                    MessageBox.Show("Email already exists. Please use a different email or try to recover your password.");
                    return;
                }

                // Proceed with registration
                using (OracleConnection conn = new OracleConnection(DatabaseConnectionStr.connStr))
                {
                    conn.Open();

                    // Updated query to include BRANCH field
                    string query = @"
                    INSERT INTO users (
                        STUDENT_ID,
                        FIRST_NAME,
                        LAST_NAME,
                        EMAIL,
                        DATE_CREATED,
                        PASSWORD,
                        TYPE,
                        BRANCH
                    ) VALUES (
                        :student_id,
                        :first_name,
                        :last_name,
                        :email,
                        SYSDATE,
                        :password,
                        :user_type,
                        :branch
                    )";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        // Add parameters
                        cmd.Parameters.Add(new OracleParameter("student_id", studentId));
                        cmd.Parameters.Add(new OracleParameter("first_name", firstName));
                        cmd.Parameters.Add(new OracleParameter("last_name", lastName));
                        cmd.Parameters.Add(new OracleParameter("email", email));
                        cmd.Parameters.Add(new OracleParameter("password", password)); // Password is converted to BLOB in the query
                        cmd.Parameters.Add(new OracleParameter("user_type", userType));
                        cmd.Parameters.Add(new OracleParameter("branch", branch)); // Add branch parameter

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful! You may now login with your credentials.");

                            // Reset form fields
                            ClearRegistrationFields();

                            // Switch to login panel
                            register_pnl.Visible = false;
                            login_pnl.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.");
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private bool IsStudentIdExists(string studentId)
        {
            using (OracleConnection conn = new OracleConnection(DatabaseConnectionStr.connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM users WHERE student_id = :student_id";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("student_id", studentId));
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool IsEmailExists(string email)
        {
            using (OracleConnection conn = new OracleConnection(DatabaseConnectionStr.connStr))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM users WHERE email = :email";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("email", email));
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void ClearRegistrationFields()
        {
            fName_txt.Clear();
            lName_txt.Clear();
            regEmail_txt.Clear();
            studentId_txt.Clear();
            regPass_txt.Clear();
            regCPass_txt.Clear();
            userType_txt.Text = "";
            branch_txt2.Text = "";
            student_pnl.Visible = false;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidStudentNumber(string studnum)
        {
            // Student number format: YY-NNNN
            string pattern = @"^\d{2}-\d{4}$";
            return Regex.IsMatch(studnum, pattern);
        }

        private void usertype_dropdown(object sender, EventArgs e)
        {
            userType_cms.Show(userType_txt, new Point(0, userType_txt.Height));
        }

        private void userType_cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            userType_txt.Text = e.ClickedItem.Text;

            student_pnl.Visible = e.ClickedItem.Text == "Student";
        }
    }
}
