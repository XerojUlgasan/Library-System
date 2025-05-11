using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Library_system
{
    public partial class UserInterface : Form
    {
        public UserInterface()
        {
            InitializeComponent();

            // Set user details in the UI when form loads
            if (Student.firstName != null && Student.lastName != null)
            {
                label1.Text = Student.email;
                label3.Text = Student.studentId;
            }

            loadBooks();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            statistics_cms2.Show(statistics_txt, new Point(0, statistics_txt.Height));
        }

        private void statistics_cms2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            statistics_txt.Text = e.ClickedItem.Text;
            statistics_cms2.Hide();
            initializeChart(); // Re-initialize the chart with the new selection
        }

        private void userInterface_load(object sender, EventArgs e)
        {
            // Set default selection
            if (string.IsNullOrEmpty(statistics_txt.Text))
            {
                statistics_txt.Text = "My visits";
            }

            initializeChart();
        }

        void initializeChart()
        {
            // Clear any existing chart
            chart_panel2.Controls.Clear();

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

            // Default values if no statistics selected
            string selectedStatistic = statistics_txt.Text;
            string chartTitle = "My Library Statistics";
            string yAxisName = "Count";
            string seriesName = selectedStatistic;
            string currentBranch = Student.targetBranch; // Get the branch of the student
            string studentId = Student.studentId;
            string email = Student.email;

            List<double> values = new List<double>();
            List<string> dates = new List<string>();

            // Default date range: last 7 days to today
            DateTime fromDate = DateTime.Now.AddDays(-7);
            DateTime toDate = DateTime.Now;

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
                        case "My visits":
                            // Query to get the user's check-in records
                            query = @"
                                SELECT COUNT(*) AS count, TRUNC(DATE_LOGGED) AS created_date
                                FROM LOGS
                                WHERE EMAIL = :email
                                AND TYPE = 'Check In'
                                AND DATE_LOGGED BETWEEN :from_date AND :to_date
                                AND BRANCH = :branch   
                                GROUP BY TRUNC(DATE_LOGGED)
                                ORDER BY TRUNC(DATE_LOGGED)";
                            chartTitle = "My Visit History";
                            seriesName = "Visits";
                            break;

                        case "My borrows":
                            // Query to get the user's borrowed books
                            query = @"
                                SELECT COUNT(*) AS count, TRUNC(BORROW_DATE) AS created_date
                                FROM BORROWED_BOOKS
                                WHERE EMAIL = :email
                                AND BORROW_DATE BETWEEN :from_date AND :to_date
                                AND BRANCH = :branch  
                                GROUP BY TRUNC(BORROW_DATE)
                                ORDER BY TRUNC(BORROW_DATE)";
                            chartTitle = "My Borrow History";
                            seriesName = "Borrows";
                            break;

                        case "My dues":
                            // Query to get the user's overdue books
                            query = @"
                                SELECT COUNT(*) AS count, TRUNC(BORROW_DUE) AS created_date
                                FROM BORROWED_BOOKS
                                WHERE EMAIL = :email
                                AND SYSDATE > BORROW_DUE
                                AND STATUS != 'Returned'
                                AND BRANCH = :branch  
                                AND BORROW_DUE BETWEEN :from_date AND :to_date
                                GROUP BY TRUNC(BORROW_DUE)
                                ORDER BY TRUNC(BORROW_DUE)";
                            chartTitle = "My Due Dates";
                            seriesName = "Dues";
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
                            cmd.Parameters.Add(new OracleParameter("email", email));

                            OracleParameter fromParam = new OracleParameter("from_date", OracleDbType.Date);
                            fromParam.Value = fromDate;
                            cmd.Parameters.Add(fromParam);

                            OracleParameter toParam = new OracleParameter("to_date", OracleDbType.Date);
                            toParam.Value = toDate;
                            cmd.Parameters.Add(toParam);

                            cmd.Parameters.Add("branch", currentBranch);

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
                Text = chartTitle,
                TextSize = 16,
                Paint = new SolidColorPaint(SKColors.DarkSlateBlue)
            };

            cartesianChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Top;
            cartesianChart.LegendTextPaint = new SolidColorPaint(SKColors.DarkSlateBlue);
            cartesianChart.LegendTextSize = 6;

            chart_panel2.Controls.Add(cartesianChart);
        }

        private void check_in(object sender, EventArgs e)
        {
            try
            {
                // Get current user information from Student class
                string email = Student.email;
                string studentId = Student.studentId;
                string branch = Student.targetBranch; // Get the branch information

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(branch))
                {
                    MessageBox.Show("User information is not available. Please log in again.");
                    return;
                }

                using (OracleConnection conn = new OracleConnection(DatabaseConnectionStr.connStr))
                {
                    conn.Open();

                    // Check if the user already has a check-in record for today without a check-out
                    string checkQuery = @"
                    SELECT COUNT(*) 
                    FROM LOGS 
                    WHERE EMAIL = :email 
                    AND TRUNC(DATE_LOGGED) = TRUNC(SYSDATE)
                    AND TYPE = 'Check In'
                    AND NOT EXISTS (
                        SELECT 1 
                        FROM LOGS L2 
                        WHERE L2.EMAIL = LOGS.EMAIL 
                        AND TRUNC(L2.DATE_LOGGED) = TRUNC(LOGS.DATE_LOGGED)
                        AND L2.TYPE = 'Check Out'
                        AND L2.TIME_LOGGED > LOGS.TIME_LOGGED
                    )";

                    using (OracleCommand checkCmd = new OracleCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add(new OracleParameter("email", email));
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("You have already checked in today and haven't checked out yet.");
                            return;
                        }
                    }

                    // Insert the check-in record with branch information
                    string insertQuery = @"
                    INSERT INTO LOGS (EMAIL, STUDENT_ID, DATE_LOGGED, TIME_LOGGED, TYPE, BRANCH)
                    VALUES (:email, :student_id, SYSDATE, SYSTIMESTAMP, 'Check In', :branch)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("email", email));
                        cmd.Parameters.Add(new OracleParameter("student_id", studentId));
                        cmd.Parameters.Add(new OracleParameter("branch", branch)); // Add branch parameter

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Check-in successful!");
                        }
                        else
                        {
                            MessageBox.Show("Check-in failed. Please try again.");
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

        private void check_out(object sender, EventArgs e)
        {
            try
            {
                // Get current user information from Student class
                string email = Student.email;
                string studentId = Student.studentId;
                string branch = Student.branch; // Get the branch information

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(branch))
                {
                    MessageBox.Show("User information is not available. Please log in again.");
                    return;
                }

                using (OracleConnection conn = new OracleConnection(DatabaseConnectionStr.connStr))
                {
                    conn.Open();

                    // Check if the user has checked in today before checking out
                    string checkQuery = @"
                    SELECT COUNT(*) 
                    FROM LOGS 
                    WHERE EMAIL = :email 
                    AND TRUNC(DATE_LOGGED) = TRUNC(SYSDATE)
                    AND TYPE = 'Check In'";

                    using (OracleCommand checkCmd = new OracleCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add(new OracleParameter("email", email));
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("You need to check in first before checking out.");
                            return;
                        }
                    }

                    // Check if the user already has a check-out record for today after their most recent check-in
                    string checkOutQuery = @"
                    SELECT COUNT(*) 
                    FROM LOGS L1
                    WHERE L1.EMAIL = :email 
                    AND TRUNC(L1.DATE_LOGGED) = TRUNC(SYSDATE)
                    AND L1.TYPE = 'Check Out'
                    AND L1.TIME_LOGGED > (
                        SELECT MAX(L2.TIME_LOGGED) 
                        FROM LOGS L2 
                        WHERE L2.EMAIL = L1.EMAIL 
                        AND TRUNC(L2.DATE_LOGGED) = TRUNC(SYSDATE)
                        AND L2.TYPE = 'Check In'
                    )";

                    using (OracleCommand checkOutCmd = new OracleCommand(checkOutQuery, conn))
                    {
                        checkOutCmd.Parameters.Add(new OracleParameter("email", email));
                        int count = Convert.ToInt32(checkOutCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("You have already checked out after your most recent check-in.");
                            return;
                        }
                    }

                    // Insert the check-out record with branch information
                    string insertQuery = @"
                    INSERT INTO LOGS (EMAIL, STUDENT_ID, DATE_LOGGED, TIME_LOGGED, TYPE, BRANCH)
                    VALUES (:email, :student_id, SYSDATE, SYSTIMESTAMP, 'Check Out', :branch)";

                    using (OracleCommand cmd = new OracleCommand(insertQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("email", email));
                        cmd.Parameters.Add(new OracleParameter("student_id", studentId));
                        cmd.Parameters.Add(new OracleParameter("branch", branch)); // Add branch parameter

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Check-out successful!");
                        }
                        else
                        {
                            MessageBox.Show("Check-out failed. Please try again.");
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

        private void log_out(object sender, EventArgs e)
        {
            Student.clear();


            // User is logging out, show login form
            WelcomePage welcomePage = new WelcomePage();
            welcomePage.Show();
            this.Close();
        }

        private void show_dashboard(object sender, EventArgs e)
        {
            dashboard_panel.Visible = true;
            books_panel.Visible = false;
        }

        private void show_books(object sender, EventArgs e)
        {
            books_panel.Visible = true;
            dashboard_panel.Visible = false;
            loadBooks(); // Load books when switching to the books panel
        }

        // Add this method to your UserInterface class
        private void loadBooks()
        {
            books_dgv.Rows.Clear(); // Clear existing rows in the DataGridView

            try
            {
                string connectionString = DatabaseConnectionStr.connStr;
                string currentBranch = Student.targetBranch; // Get the branch of the student

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    b.book_id,
                    b.title,
                    b.author,
                    b.publication_date,
                    b.genre,
                    b.quantity,
                    (b.quantity - (SELECT COUNT(*) 
                                   FROM borrowed_books bb
                                   WHERE b.book_id = bb.book_id
                                   AND bb.status = 'Borrowed'
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
                                    Convert.ToDateTime(reader["PUBLICATION_DATE"]).ToString("MMMM d, yyyy"),
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
                MessageBox.Show("Error loading books: " + ex.Message);
            }
        }
    }
}
