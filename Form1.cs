using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Globalization;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Linq;
//using System.Windows.Forms.DataVisualization.Charting;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Themes;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Library_system;


//TODO:
// SETTINGS
// Change default timeframe. Give choices such ddaily, weekly, monthly 

//DASHBOARD:
// FILL UP borrowed books, returnd books, overdue books, misssing books,
// total books, visitors, and new members depending on the chosen date.

// FINISH STATISTICS

// BORROWED BOOKS:
// Each table row items should be clickable and enables the user to set the book as returned, missing, or overdue.

namespace Library_system
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            branch.Text = admin.branch;
            dashboard_panel.Visible = true;
            user_panel.Visible = false;
            book_panel.Visible = false;
            borrow_record_panel.Visible = false;

            dashboard_from_dtp.MaxDate = DateTime.Now.AddDays(-1);
            dashboard_from_dtp.Value = DateTime.Now.AddDays(-7);

            dashboard_to_dtp.MaxDate = DateTime.Now.AddSeconds(1);
            dashboard_to_dtp.Value = DateTime.Now;

            addborrow_returnDate_dtp.Value = DateTime.Now.AddDays(7);

            language_cms.Items.Add("English");
            language_cms.Items.Add("Filipino");

            loadDashboard(); // Load dashboard data from the database
        }

        //DASHBOARD CLICK
        private void dashboard_click(object sender, EventArgs e)
        {
            dashboard_panel.Visible = true;
            user_panel.Visible = false;
            book_panel.Visible = false;
            borrow_record_panel.Visible = false;

            dashboard_from_dtp.MaxDate = DateTime.Now.AddDays(-1);
            dashboard_from_dtp.Value = DateTime.Now.AddDays(-7);

            dashboard_to_dtp.MaxDate = DateTime.Now.AddSeconds(1);
            dashboard_to_dtp.Value = DateTime.Now;

            //DASHBOARD LOAD QUERY
            //note: visitors missing, waiting for check in and check out table
            //note: change to date depending on the user's choice (dashboard_from_dtp and dashboard_to_dtp)

            loadDashboard(); // Load dashboard data from the database
        }
        private void dashboard_date_changed(object sender, EventArgs e)
        {
            loadDashboard(); // Load dashboard data from the database
            InitializeChart(); // Update the chart with new data
        }

        //USERS CLICK
        private void user_click(object sender, EventArgs e)
        {
            dashboard_panel.Visible = false;
            user_panel.Visible = true;
            book_panel.Visible = false;
            borrow_record_panel.Visible = false;

            loadUsers(); // Load users from the database
        }
        private void user_search_text_changed(object sender, EventArgs e)
        {
            string searchText = user_search_txtbox.Text.ToLower();

            foreach (DataGridViewRow row in users_dgv.Rows)
            {
                bool rowVisible = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                    {
                        rowVisible = true;
                        break;
                    }
                }

                row.Visible = rowVisible;
            }
        }

        //BORROW RECORDS CLICK
        private void borrow_record_click(object sender, EventArgs e)
        {
            dashboard_panel.Visible = false;
            user_panel.Visible = false;
            book_panel.Visible = false;
            borrow_record_panel.Visible = true;

            loadBorrow(); // Load borrowed books from the database
        }

        private void borrow_record_search_text_change(object sender, EventArgs e)
        {
            string searchText = borrow_record_search_txtbox.Text.ToLower();

            foreach (DataGridViewRow row in borrow_dgv.Rows)
            {
                bool rowVisible = false;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                    {
                        rowVisible = true;
                        break;
                    }
                }

                row.Visible = rowVisible;
            }
        }

        private void addborrow_popup(object sender, EventArgs e)
        {
            disableall(addborrow_panel);
        }

        private void addborrow_popup_exit(object sender, EventArgs e)
        {
            enableall(addborrow_panel);

            addborrow_studId_txtbox.Text = "";
            addborrow_title_txtbox.Text = "";
            addborrow_returnDate_dtp.Value = DateTime.Now.AddDays(7);
        }

        // Modify addborrow method to include branch information
        private void addborrow(object sender, EventArgs e) //INPUT BORROW DATA TO THE DATABASE
        {
            if (string.IsNullOrWhiteSpace(addborrow_user_type_txt.Text) ||
                string.IsNullOrWhiteSpace(addborrow_email_txt.Text) ||
                string.IsNullOrWhiteSpace(addborrow_title_txtbox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (addborrow_user_type_txt.Text == "Student" &&
                string.IsNullOrWhiteSpace(addborrow_studId_txtbox.Text))
            {
                MessageBox.Show("Student ID is required.");
                return;
            }

            string borrowerId = (addborrow_user_type_txt.Text == "Student") ? addborrow_studId_txtbox.Text :addborrow_user_type_txt.Text;
            string email = addborrow_email_txt.Text;
            string bookTitle = addborrow_title_txtbox.Text;
            DateTime returnDate = addborrow_returnDate_dtp.Value;
            string currentBranch = admin.branch;

            try
            {
                string connectionString = DatabaseConnectionStr.connStr;
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    // Validate user
                    string validateUserQuery = @"
                SELECT STUDENT_ID, FIRST_NAME, LAST_NAME
                FROM USERS
                WHERE EMAIL = :email AND BRANCH = :branch";

                    string borrowerFirstName = null;
                    string borrowerLastName = null;

                    using (OracleCommand validateUserCmd = new OracleCommand(validateUserQuery, conn))
                    {
                        validateUserCmd.Parameters.Add(new OracleParameter("email", email));
                        validateUserCmd.Parameters.Add(new OracleParameter("branch", currentBranch));

                        using (OracleDataReader reader = validateUserCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                borrowerId = reader["STUDENT_ID"].ToString();
                                borrowerFirstName = reader["FIRST_NAME"].ToString();
                                borrowerLastName = reader["LAST_NAME"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("User not found in this branch.");
                                return;
                            }
                        }
                    }

                    // Validate book
                    string validateBookQuery = @"
                SELECT BOOK_ID
                FROM BOOKS
                WHERE TITLE = :title AND BRANCH = :branch";

                    int bookId;

                    using (OracleCommand validateBookCmd = new OracleCommand(validateBookQuery, conn))
                    {
                        validateBookCmd.Parameters.Add(new OracleParameter("title", bookTitle));
                        validateBookCmd.Parameters.Add(new OracleParameter("branch", currentBranch));

                        object result = validateBookCmd.ExecuteScalar();
                        if (result != null)
                        {
                            bookId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Book not found in this branch.");
                            return;
                        }
                    }

                    // Insert borrow record
                    string insertBorrowQuery = @"
                INSERT INTO BORROWED_BOOKS (
                    BORROWER_ID, BORROWER_LN, BORROWER_FN, EMAIL, BOOK_ID, BORROW_DUE, BORROW_DATE, STATUS, BRANCH
                ) VALUES (
                    :borrowerId, :borrowerLastName, :borrowerFirstName, :email, :bookId, :borrowDue, SYSDATE, 'Borrowed', :branch
                )";

                    using (OracleCommand insertBorrowCmd = new OracleCommand(insertBorrowQuery, conn))
                    {
                        insertBorrowCmd.Parameters.Add(new OracleParameter("borrowerId", borrowerId));
                        insertBorrowCmd.Parameters.Add(new OracleParameter("borrowerLastName", borrowerLastName));
                        insertBorrowCmd.Parameters.Add(new OracleParameter("borrowerFirstName", borrowerFirstName));
                        insertBorrowCmd.Parameters.Add(new OracleParameter("email", email));
                        insertBorrowCmd.Parameters.Add(new OracleParameter("bookId", bookId));
                        insertBorrowCmd.Parameters.Add(new OracleParameter("borrowDue", returnDate));
                        insertBorrowCmd.Parameters.Add(new OracleParameter("branch", currentBranch));

                        insertBorrowCmd.ExecuteNonQuery();
                        MessageBox.Show("Borrow record added successfully.");
                    }

                    // Reload borrow records
                    loadBorrow();

                    // Reset form fields
                    addborrow_studId_txtbox.Text = "";
                    addborrow_email_txt.Text = "";
                    addborrow_title_txtbox.Text = "";
                    addborrow_returnDate_dtp.Value = DateTime.Now.AddDays(7);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding borrow record: " + ex.Message);
            }
        }
        private void status_filter(object sender, ToolStripItemClickedEventArgs e)
        {
            // FILTER STATUS FROM borrow_dgv TABLE

            string selectedStatus = e.ClickedItem.Text;
            foreach (DataGridViewRow row in borrow_dgv.Rows)
            {
                if (row.Cells[6].Value.ToString().Contains(selectedStatus))
                {
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }
        }

        //ADD BOOK CLICK
        private void add_books_click(object sender, EventArgs e)
        {
            dashboard_panel.Visible = false;
            user_panel.Visible = false;
            book_panel.Visible = true;
            borrow_record_panel.Visible = false;

            loadBooks(); // Load books from the database
        }

        private void addbook_popup(object sender, EventArgs e)
        {
            disableall(addbook_panel);
        }

        private void addbook_popup_exit(object sender, EventArgs e)
        {
            enableall(addbook_panel);

            addbook_title_txtbox.Text = "";
            addbook_author_txtbox.Text = "";
            addbook_publisher_txtbox.Text = "";
            addbook_publicationDate_dtp.Value = DateTime.Now.AddMinutes(-1);
            addbook_genre_txtbox.Text = "";
            addbook_language_txtbox.Text = "";
            addbook_pagecount_num.Value = 0;
            addbook_quantity_num.Value = 1;
        }

        // Modify addbook method to include branch information
        private void addbook(object sender, EventArgs e) //INPUTS BOOK DATA TO THE DATABASE
        {
            if (string.IsNullOrWhiteSpace(addbook_title_txtbox.Text) ||
                string.IsNullOrWhiteSpace(addbook_author_txtbox.Text) ||
                string.IsNullOrWhiteSpace(addbook_publisher_txtbox.Text) ||
                string.IsNullOrWhiteSpace(addbook_genre_txtbox.Text) ||
                string.IsNullOrWhiteSpace(addbook_language_txtbox.Text) ||
                string.IsNullOrWhiteSpace(addbook_quantity_num.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string title = addbook_title_txtbox.Text;
            string author = addbook_author_txtbox.Text;
            string publisher = addbook_publisher_txtbox.Text;
            DateTime publication_date = addbook_publicationDate_dtp.Value;
            string genre = addbook_genre_txtbox.Text;
            string book_language = addbook_language_txtbox.Text;
            int page_count = (int)addbook_pagecount_num.Value;
            int quantity = (int)addbook_quantity_num.Value;
            string currentBranch = admin.branch; // Get the current branch of the admin

            DialogResult result = MessageBox.Show("Do you confirm the data that you have entered?", "Confirm Book Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Insert data into the database
                try
                {
                    string connectionString = DatabaseConnectionStr.connStr;
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        // Updated query to include branch parameter
                        string query = "INSERT INTO books (title, author, publisher, publication_date, genre, book_language, page_count, quantity, branch) " +
                                        "VALUES(:title, :author, :publisher, :publication_date, :genre, :book_language, :page_count, :quantity, :branch)";

                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("title", title));
                            cmd.Parameters.Add(new OracleParameter("author", author));
                            cmd.Parameters.Add(new OracleParameter("publisher", publisher));
                            cmd.Parameters.Add(new OracleParameter("publication_date", publication_date));
                            cmd.Parameters.Add(new OracleParameter("genre", genre));
                            cmd.Parameters.Add(new OracleParameter("book_language", book_language));
                            cmd.Parameters.Add(new OracleParameter("page_count", page_count));
                            cmd.Parameters.Add(new OracleParameter("quantity", quantity));
                            cmd.Parameters.Add(new OracleParameter("branch", currentBranch));

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Book data inserted successfully.");
                        }

                        loadBooks(); // Reload books after insertion

                        enableall(addbook_panel); // Enable all panels after insertion

                        addbook_title_txtbox.Text = "";
                        addbook_author_txtbox.Text = "";
                        addbook_publisher_txtbox.Text = "";
                        addbook_publicationDate_dtp.Value = DateTime.Now.AddMinutes(-1);
                        addbook_genre_txtbox.Text = "";
                        addbook_language_txtbox.Text = "";
                        addbook_pagecount_num.Value = 0;
                        addbook_quantity_num.Value = 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Book data insertion canceled.");
            }
        }

        //FUNCTIONS 
        private void disableall(Panel enabledPanel) //DISABLE ALL PANELS EXCEPT THE ONE THAT IS ENABLED
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    panel.Enabled = false;
                }
            }

            foreach (Control control in enabledPanel.Parent.Controls)
            {
                if (enabledPanel.Parent != null)
                {
                    enabledPanel.Parent.Enabled = true;

                    if (control == enabledPanel)
                    {
                        control.Enabled = true;
                        control.Visible = true;
                        control.Focus();
                    }
                    else
                    {
                        control.Enabled = false;
                    }
                }
            }
        }

        public void enableall(Panel disabledPanel) //ENABLE ALL PANELS
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    panel.Enabled = true;
                }
            }

            foreach (Control control in disabledPanel.Parent.Controls)
            {
                if (disabledPanel.Parent != null)
                {
                    //disabledPanel.Parent.Enabled = true;

                    if (control == disabledPanel)
                    {
                        control.Enabled = false;
                        control.Visible = false;
                    }
                    else
                    {
                        control.Enabled = true;
                    }
                }
            }
        }

        // Modify loadDashboard method
        public void loadDashboard()
        {
            string connectionString = DatabaseConnectionStr.connStr;
            string currentBranch = admin.branch; // Get the current branch of the admin

            //COUNTERS
            //COUNTERS
            //COUNTERS
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string query = @"
                               SELECT 
                                   (SELECT COUNT(*) 
                                    FROM borrowed_books 
                                    WHERE BORROW_DATE BETWEEN :from_date AND :to_date 
                                      AND status = 'Borrowed'
                                      AND BRANCH = :branch) AS borrowed_books,

                                   (SELECT COUNT(*) 
                                    FROM borrowed_books 
                                    WHERE BORROW_DATE BETWEEN :from_date AND :to_date 
                                      AND status = 'Returned'
                                      AND BRANCH = :branch) AS returned_books,

                                   (SELECT COUNT(*) 
                                    FROM borrowed_books 
                                    WHERE BORROW_DATE BETWEEN :from_date AND :to_date 
                                      AND SYSDATE > BORROW_DUE
                                      AND status != 'Returned'
                                      AND BRANCH = :branch) AS overdue_books,

                                   (SELECT COUNT(*) 
                                    FROM borrowed_books 
                                    WHERE BORROW_DATE BETWEEN :from_date AND :to_date 
                                      AND status = 'Missing'
                                      AND BRANCH = :branch) AS missing_books,

                                   (SELECT SUM(QUANTITY) FROM books WHERE BRANCH = :branch) AS total_books,

                                    (SELECT COUNT(*) FROM books WHERE BRANCH = :branch) AS total_unique_books,

                                   (SELECT COUNT(*)
                                    FROM users
                                    WHERE DATE_CREATED BETWEEN :from_date AND :to_date
                                    AND BRANCH = :branch) AS new_members
                               FROM dual";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("from_date", dashboard_from_dtp.Value));
                    cmd.Parameters.Add(new OracleParameter("to_date", dashboard_to_dtp.Value));
                    cmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            borrow_label.Text = reader["borrowed_books"].ToString();
                            returned_label.Text = reader["returned_books"].ToString();
                            overdue_label.Text = reader["overdue_books"].ToString();
                            missing_label.Text = reader["missing_books"].ToString();
                            total_label.Text = reader["total_books"].ToString();
                            unique_books_label.Text = reader["total_unique_books"].ToString();
                            member_label.Text = reader["new_members"].ToString();
                        }
                    }
                }
            }
            //COUNTERS
            //COUNTERS
            //COUNTERS

            //OVERDUE HISTORY
            //OVERDUE HISTORY
            //OVERDUE HISTORY
            overview_history_dgv.Rows.Clear();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                string query = @"
                                SELECT 
                                    bb.borrower_id,
                                    b.title,
                                    bb.borrow_due,
                                    bb.borrow_date
                                FROM borrowed_books bb
                                JOIN books b
                                    ON b.book_id = bb.book_id
                                WHERE SYSDATE > borrow_due 
                                    AND STATUS != 'Returned'
                                    AND bb.BRANCH = :branch";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            overview_history_dgv.Rows.Add(reader["borrower_id"].ToString(),
                                                            reader["title"].ToString(),
                                                            Convert.ToDateTime(reader["borrow_due"]).ToString("MMMM d, yyyy"),
                                                            Convert.ToDateTime(reader["borrow_date"]).ToString("MMMM d, yyyy"));
                        }
                    }
                }
            }
            //OVERDUE HISTORY
            //OVERDUE HISTORY
            //OVERDUE HISTORY

            loadVisitLogs();
        }

        public void loadVisitLogs()
        {
            visit_logs_dgv.Rows.Clear(); // Clear existing rows in the DataGridView

            try
            {
                string connectionString = DatabaseConnectionStr.connStr;
                string currentBranch = admin.branch; // Get the current branch of the admin

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    // Query to get logs data filtered by branch
                    string query = @"
                        SELECT l.STUDENT_ID, u.FIRST_NAME, u.LAST_NAME, l.TYPE, l.TIME_LOGGED, l.BRANCH
                        FROM LOGS l
                        LEFT JOIN users u ON l.STUDENT_ID = u.STUDENT_ID
                        WHERE l.BRANCH = :branch 
                        AND TIME_LOGGED BETWEEN :from_date AND :to_date
                        ORDER BY l.TIME_LOGGED DESC";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                        cmd.Parameters.Add(new OracleParameter("from_date", dashboard_from_dtp.Value));
                        cmd.Parameters.Add(new OracleParameter("to_date", dashboard_to_dtp.Value.AddDays(1)));

                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string studentId = reader["STUDENT_ID"].ToString();
                                string firstName = reader.IsDBNull(reader.GetOrdinal("FIRST_NAME")) ? "Guest" : reader["FIRST_NAME"].ToString();
                                string lastName = reader.IsDBNull(reader.GetOrdinal("LAST_NAME")) ? "" : reader["LAST_NAME"].ToString();
                                string type = reader["TYPE"].ToString();
                                DateTime timeLogged = Convert.ToDateTime(reader["TIME_LOGGED"]);

                                // Add row to the DataGridView
                                visit_logs_dgv.Rows.Add(
                                    studentId,
                                    firstName,
                                    lastName,
                                    type,
                                    timeLogged.ToString("MMM dd, yyyy hh:mm tt")
                                );
                            }
                        }
                    }

                    // Update visitor count
                    using (OracleCommand countCmd = new OracleCommand(@"
                SELECT COUNT(DISTINCT STUDENT_ID) AS visitor_count 
                FROM LOGS 
                WHERE BRANCH = :branch 
                AND TIME_LOGGED BETWEEN :from_date AND :to_date
                AND TYPE = 'Check In'", conn))
                    {
                        countCmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                        countCmd.Parameters.Add(new OracleParameter("from_date", dashboard_from_dtp.Value));
                        countCmd.Parameters.Add(new OracleParameter("to_date", dashboard_to_dtp.Value.AddDays(1)));

                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        var visitorCount = countCmd.ExecuteScalar();
                        visitor_label.Text = visitorCount != DBNull.Value ? visitorCount.ToString() : "0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading visit logs: " + ex.Message);
            }
        }

        // Modify loadUsers method
        public void loadUsers() //LOAD USER
        {
            //REFERENCE TABLE:
            //CREATE TABLE users
            // ("STUDENT_ID" VARCHAR2(10),
            //  "FIRST_NAME" VARCHAR2(25),
            //  "LAST_NAME" VARCHAR2(25),
            //  "EMAIL" VARCHAR2(50),
            //  "DATE_CREATED" DATE
            // )
            users_dgv.Rows.Clear(); // Clear existing rows in the DataGridView
            try
            {
                string connectionString = DatabaseConnectionStr.connStr;
                string currentBranch = admin.branch; // Get the current branch of the admin

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = "SELECT STUDENT_ID, FIRST_NAME, LAST_NAME, EMAIL FROM users WHERE BRANCH = :branch";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users_dgv.Rows.Add(
                                    reader["STUDENT_ID"].ToString(),
                                    reader["FIRST_NAME"].ToString() + " " + reader["LAST_NAME"].ToString(),
                                    reader["EMAIL"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Modify loadBooks method
        public void loadBooks() //LOAD BOOKS
        {
            //REFERENCE TABLE:
            //CREATE TABLE "XEROJ"."BOOKS"
            // ("TITLE" VARCHAR2(255 BYTE),
            //  "AUTHOR" VARCHAR2(255 BYTE),
            //  "PUBLISHER" VARCHAR2(255 BYTE),
            //  "PUBLICATION_DATE" DATE,
            //  "GENRE" VARCHAR2(255 BYTE),
            //  "BOOK_LANGUAGE" VARCHAR2(50 BYTE),
            //  "PAGE_COUNT" NUMBER(*, 0),
            //  "QUANTITY" NUMBER(*, 0),
            //  "LAST_UPDATED" TIMESTAMP(6),
            //  "BOOK_ID" NUMBER,
            //  "BRANCH" VARCHAR2(20 BYTE)
            // )
            books_dgv.Rows.Clear(); // Clear existing rows in the DataGridView
            try
            {
                string connectionString = DatabaseConnectionStr.connStr;
                string currentBranch = admin.branch; // Get the current branch of the admin

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                                        SELECT 
                                            book_id,
                                            title,
                                            author,
                                            publication_date,
                                            genre,
                                            quantity,
                                            (quantity - (SELECT COUNT(*) 
                                                         FROM borrowed_books bb
                                                         WHERE b.book_id = bb.book_id
                                                         AND bb.BRANCH = :branch)) AS available
                                        FROM books b
                                        WHERE b.BRANCH = :branch";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                        conn.Open();
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                books_dgv.Rows.Add(
                                    Convert.ToInt32(reader["BOOK_ID"]),
                                    reader["TITLE"].ToString(),
                                    reader["AUTHOR"].ToString(),
                                    Convert.ToDateTime(reader["PUBLICATION_DATE"].ToString()).ToString("MMMM d, yyyy"),
                                    reader["GENRE"].ToString(),
                                    Convert.ToInt32(reader["QUANTITY"]),
                                    Convert.ToInt32(reader["AVAILABLE"])
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        // Modify loadBorrow method
        public void loadBorrow() //LOAD BORROWED BOOKS
        {
            borrow_dgv.Rows.Clear(); // Clear existing rows in the DataGridView

            try
            {
                string connectionString = DatabaseConnectionStr.connStr;
                string currentBranch = admin.branch; // Get the current branch of the admin

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    bb.BORROW_ID,
                    u.TYPE AS BORROWER_TYPE,
                    u.EMAIL,
                    b.TITLE AS BOOK_TITLE,
                    bb.BORROW_DATE,
                    bb.BORROW_DUE,
                    bb.STATUS
                FROM BORROWED_BOOKS bb
                JOIN USERS u ON bb.BORROWER_ID = u.STUDENT_ID
                JOIN BOOKS b ON bb.BOOK_ID = b.BOOK_ID
                WHERE bb.BRANCH = :branch
                ORDER BY bb.BORROW_DATE DESC";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("branch", currentBranch));
                        conn.Open();

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                borrow_dgv.Rows.Add(
                                    reader["BORROW_ID"].ToString(),
                                    reader["BORROWER_TYPE"].ToString(),
                                    reader["EMAIL"].ToString(),
                                    reader["BOOK_TITLE"].ToString(),
                                    Convert.ToDateTime(reader["BORROW_DATE"]).ToString("MMMM d, yyyy"),
                                    Convert.ToDateTime(reader["BORROW_DUE"]).ToString("MMMM d, yyyy"),
                                    reader["STATUS"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading borrowed books: " + ex.Message);
            }
        }
        //VALIDATION FUNCTIONS TO BE RECYCLED IN ACCOUNT REGISTRATION
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

        //SMALL FUNCTIONS
        private void languages_cms_dropdown(object sender, EventArgs e)
        {
            language_cms.Width = addbook_language_txtbox.Width;
            language_cms.Show(addbook_language_txtbox, new Point(0, addbook_language_txtbox.Height));
        }

        private void language_cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem clickedItem = e.ClickedItem as ToolStripMenuItem;
            if (clickedItem != null)
            {
                addbook_language_txtbox.Text = clickedItem.Text;
            }
        }

        private void status_btn_Click(object sender, EventArgs e)
        {
            status_cms.Show(status_btn, new Point(0, status_btn.Height));
        }

        private void statistics_dropdown(object sender, EventArgs e)
        {
            statistics_cms.Show(statistics_txtbox, new Point(0, statistics_txtbox.Height));
        }
        private void statistics_cms_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                statistics_txtbox.Text = e.ClickedItem.Text;
            }
        }
        private void statistics_chart_changed(object sender, EventArgs e)
        {
            statistics_cms.Hide();
            InitializeChart();
        }

        // Modify InitializeChart method to filter by branch
        private void InitializeChart()
        {
            // Clear any existing chart
            chart_panel.Controls.Clear();

            var cartesianChart = new CartesianChart
            {
                Dock = DockStyle.Fill,
                BackColor = System.Drawing.Color.White
            };

            // Set tooltip configuration for smaller appearance
            cartesianChart.TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Top;
            cartesianChart.TooltipBackgroundPaint = new SolidColorPaint(SKColors.White.WithAlpha(230));
            cartesianChart.TooltipTextSize = 9; // Smaller text size
            cartesianChart.TooltipTextPaint = new SolidColorPaint(SKColors.Black);

            // Modern gradient colors
            var gradientPaint = new LinearGradientPaint(
                new[] {
                    new SKColor(48, 63, 159, 180),  // Dark blue with transparency
                    new SKColor(41, 182, 246, 150)  // Light blue with transparency
                },
                new SKPoint(0, 0),
                new SKPoint(0, 1)
            );

            string selectedStatistic = statistics_txtbox.Text;
            string chartTitle = "Library Statistics";
            string yAxisName = "Count";
            string seriesName = selectedStatistic;
            string currentBranch = admin.branch; // Get the current branch of the admin

            List<double> values = new List<double>();
            List<string> dates = new List<string>();
            DateTime fromDate = dashboard_from_dtp.Value;
            DateTime toDate = dashboard_to_dtp.Value;

            // Set up the database connection
            string connectionString = DatabaseConnectionStr.connStr;
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "";

                    switch (selectedStatistic)
                    {
                        case "Visitors":
                            // Create visitor query with branch filtering based on LOGS table
                            query = @"
                                    SELECT COUNT(*) AS count, TRUNC(DATE_LOGGED) AS created_date
                                    FROM LOGS
                                    WHERE DATE_LOGGED BETWEEN :from_date AND :to_date
                                    AND TYPE = 'Check In'
                                    AND BRANCH = :branch
                                    GROUP BY TRUNC(DATE_LOGGED)
                                    ORDER BY TRUNC(DATE_LOGGED)";
                            chartTitle = "Visitor Statistics";
                            seriesName = "Visitors";
                            break;

                        case "New Members":
                            query = @"
                                    SELECT COUNT(*) AS count, TRUNC(date_created) AS created_date
                                    FROM users
                                    WHERE date_created BETWEEN :from_date AND :to_date
                                    AND BRANCH = :branch
                                    GROUP BY TRUNC(date_created)
                                    ORDER BY TRUNC(date_created)";
                            chartTitle = "New Member Registrations";
                            seriesName = "New Members";
                            break;

                        case "Borrowed Books":
                            query = @"
                                    SELECT COUNT(*) AS count, TRUNC(borrow_date) AS created_date
                                    FROM borrowed_books
                                    WHERE status = 'Borrowed' 
                                        AND borrow_date BETWEEN :from_date AND :to_date
                                        AND BRANCH = :branch
                                    GROUP BY TRUNC(borrow_date)
                                    ORDER BY TRUNC(borrow_date)";
                            chartTitle = "Books Borrowed Over Time";
                            seriesName = "Borrowed Books";
                            break;

                        case "Returned Books":
                            query = @"
                                    SELECT COUNT(*) AS count, TRUNC(borrow_date) AS created_date
                                    FROM borrowed_books
                                    WHERE status = 'Returned' 
                                        AND borrow_date BETWEEN :from_date AND :to_date
                                        AND BRANCH = :branch
                                    GROUP BY TRUNC(borrow_date)
                                    ORDER BY TRUNC(borrow_date)";
                            chartTitle = "Books Returned Over Time";
                            seriesName = "Returned Books";
                            break;

                        case "Overdue Books":
                            query = @"
                                    SELECT COUNT(*) AS count, TRUNC(borrow_due) AS created_date
                                    FROM borrowed_books
                                    WHERE SYSDATE > borrow_due 
                                        AND status != 'Returned'
                                        AND borrow_due BETWEEN :from_date AND :to_date
                                        AND BRANCH = :branch
                                    GROUP BY TRUNC(borrow_due)
                                    ORDER BY TRUNC(borrow_due)";
                            chartTitle = "Overdue Books Over Time";
                            seriesName = "Overdue Books";
                            break;

                        case "Missing Books":
                            query = @"
                                    SELECT COUNT(*) AS count, TRUNC(borrow_date) AS created_date
                                    FROM borrowed_books
                                    WHERE status = 'Missing'
                                        AND borrow_date BETWEEN :from_date AND :to_date
                                        AND BRANCH = :branch
                                    GROUP BY TRUNC(borrow_date)
                                    ORDER BY TRUNC(borrow_date)";
                            chartTitle = "Missing Books Over Time";
                            seriesName = "Missing Books";
                            break;

                        case "Total Books":
                            query = @"
                                    SELECT SUM(QUANTITY) AS count, TRUNC(LAST_UPDATED) AS created_date
                                    FROM books
                                    WHERE LAST_UPDATED BETWEEN :from_date AND :to_date
                                    AND BRANCH = :branch
                                    GROUP BY TRUNC(LAST_UPDATED)
                                    ORDER BY TRUNC(LAST_UPDATED)";
                            chartTitle = "Total Book Inventory Over Time";
                            seriesName = "Total Books";
                            break;

                        default:
                            values = Enumerable.Repeat(0.0, 10).ToList();
                            dates = Enumerable.Range(0, 10)
                                .Select(i => fromDate.AddDays(i).ToString("MMM dd"))
                                .ToList();
                            chartTitle = "Select a Statistic";
                            seriesName = "No Data";
                            break;
                    }

                    if (!string.IsNullOrEmpty(query))
                    {
                        var dataByDate = new Dictionary<DateTime, double>();

                        for (var day = fromDate.Date; day <= toDate.Date; day = day.AddDays(1))
                        {
                            dataByDate[day] = 0;
                        }

                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("from_date", fromDate));
                            cmd.Parameters.Add(new OracleParameter("to_date", toDate));
                            cmd.Parameters.Add(new OracleParameter("branch", currentBranch));

                            using (OracleDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DateTime date = Convert.ToDateTime(reader["created_date"]);
                                    double count = Convert.ToDouble(reader["count"]);

                                    dataByDate[date.Date] = count;
                                }
                            }
                        }

                        var sortedData = dataByDate.OrderBy(kvp => kvp.Key).ToList();
                        values = sortedData.Select(kvp => kvp.Value).ToList();
                        dates = sortedData.Select(kvp => kvp.Key.ToString("MMM dd")).ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading chart data: {ex.Message}");

                    values = Enumerable.Repeat(0.0, 10).ToList();
                    dates = Enumerable.Range(0, 10)
                        .Select(i => fromDate.AddDays(i).ToString("MMM dd"))
                        .ToList();
                }
            }

            cartesianChart.Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Name = seriesName,
                    Values = values.ToArray(),
                    Stroke = new SolidColorPaint(SKColors.DodgerBlue) { StrokeThickness = 3 },
                    Fill = gradientPaint,
                    GeometrySize = 2,
                    LineSmoothness = 0.5
                }
            };

            cartesianChart.XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Date",
                    Labels = dates.ToArray(),
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    TicksPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    LabelsPaint = new SolidColorPaint(SKColors.Gray)
                }
            };

            cartesianChart.YAxes = new Axis[]
            {
                new Axis
                {
                    Name = yAxisName,
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    TicksPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 1 },
                    LabelsPaint = new SolidColorPaint(SKColors.Gray),
                    MinLimit = 0
                }
            };

            cartesianChart.Title = new LabelVisual
            {
                Text = chartTitle + " - " + currentBranch + " Branch",
                TextSize = 16,
                Paint = new SolidColorPaint(SKColors.DarkSlateBlue)
            };

            cartesianChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Top;
            cartesianChart.LegendTextPaint = new SolidColorPaint(SKColors.DarkSlateBlue);
            cartesianChart.LegendTextSize = 6;

            chart_panel.Controls.Add(cartesianChart);
        }

        private void log_out(object sender, EventArgs e)
        {
            this.Hide();
            WelcomePage wc = new WelcomePage();
            wc.Show();
        }

        private void user_type(object sender, EventArgs e)
        {
            user_type_cms.Show(addborrow_user_type_txt, new Point(0, addborrow_user_type_txt.Height));
        }

        private void user_type_click(object sender, ToolStripItemClickedEventArgs e)
        {
            addborrow_user_type_txt.Text = e.ClickedItem.Text;

            student_id_pnl.Visible = addborrow_user_type_txt.Text == "Student";
        }
    }
}
