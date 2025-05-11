namespace Library_system
{
    partial class WelcomePage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            branch_cms = new ContextMenuStrip(components);
            sanBartolomeToolStripMenuItem = new ToolStripMenuItem();
            sanFranciscoToolStripMenuItem = new ToolStripMenuItem();
            batasanToolStripMenuItem = new ToolStripMenuItem();
            branch_txt = new TextBox();
            logEmail_txt = new TextBox();
            logPass_txt = new TextBox();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            label3 = new Label();
            register_link = new LinkLabel();
            login_pnl = new Panel();
            pictureBox14 = new PictureBox();
            pictureBox2 = new PictureBox();
            userType_txt = new TextBox();
            userType_cms = new ContextMenuStrip(components);
            professorToolStripMenuItem = new ToolStripMenuItem();
            staffToolStripMenuItem = new ToolStripMenuItem();
            studentToolStripMenuItem = new ToolStripMenuItem();
            pictureBox1 = new PictureBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            login_link = new LinkLabel();
            label4 = new Label();
            button3 = new Button();
            regCPass_txt = new TextBox();
            regPass_txt = new TextBox();
            regEmail_txt = new TextBox();
            branch_txt2 = new TextBox();
            register_pnl = new FlowLayoutPanel();
            panel1 = new Panel();
            panel2 = new Panel();
            student_pnl = new Panel();
            studentId_txt = new TextBox();
            label8 = new Label();
            panel7 = new Panel();
            fName_txt = new TextBox();
            label9 = new Label();
            panel8 = new Panel();
            lName_txt = new TextBox();
            label10 = new Label();
            panel3 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            panel6 = new Panel();
            panel9 = new Panel();
            pictureBox3 = new PictureBox();
            label11 = new Label();
            branch_cms.SuspendLayout();
            login_pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox14).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            userType_cms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            register_pnl.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            student_pnl.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // branch_cms
            // 
            branch_cms.Items.AddRange(new ToolStripItem[] { sanBartolomeToolStripMenuItem, sanFranciscoToolStripMenuItem, batasanToolStripMenuItem });
            branch_cms.Name = "branch_cms";
            branch_cms.Size = new Size(152, 70);
            branch_cms.ItemClicked += branch_cms_ItemClicked;
            // 
            // sanBartolomeToolStripMenuItem
            // 
            sanBartolomeToolStripMenuItem.Name = "sanBartolomeToolStripMenuItem";
            sanBartolomeToolStripMenuItem.Size = new Size(151, 22);
            sanBartolomeToolStripMenuItem.Text = "San Bartolome";
            // 
            // sanFranciscoToolStripMenuItem
            // 
            sanFranciscoToolStripMenuItem.Name = "sanFranciscoToolStripMenuItem";
            sanFranciscoToolStripMenuItem.Size = new Size(151, 22);
            sanFranciscoToolStripMenuItem.Text = "San Francisco";
            // 
            // batasanToolStripMenuItem
            // 
            batasanToolStripMenuItem.Name = "batasanToolStripMenuItem";
            batasanToolStripMenuItem.Size = new Size(151, 22);
            batasanToolStripMenuItem.Text = "Batasan";
            // 
            // branch_txt
            // 
            branch_txt.BackColor = SystemColors.ControlLightLight;
            branch_txt.Font = new Font("Trebuchet MS", 11.25F);
            branch_txt.Location = new Point(36, 39);
            branch_txt.Name = "branch_txt";
            branch_txt.PlaceholderText = "Branch";
            branch_txt.ReadOnly = true;
            branch_txt.Size = new Size(223, 25);
            branch_txt.TabIndex = 1;
            branch_txt.Click += branch_txt_Click;
            // 
            // logEmail_txt
            // 
            logEmail_txt.Font = new Font("Trebuchet MS", 11.25F);
            logEmail_txt.Location = new Point(35, 94);
            logEmail_txt.Name = "logEmail_txt";
            logEmail_txt.Size = new Size(224, 25);
            logEmail_txt.TabIndex = 2;
            // 
            // logPass_txt
            // 
            logPass_txt.Font = new Font("Trebuchet MS", 11.25F);
            logPass_txt.Location = new Point(35, 149);
            logPass_txt.Name = "logPass_txt";
            logPass_txt.Size = new Size(224, 25);
            logPass_txt.TabIndex = 3;
            logPass_txt.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Trebuchet MS", 11.25F);
            label1.Location = new Point(38, 72);
            label1.Name = "label1";
            label1.Size = new Size(45, 20);
            label1.TabIndex = 4;
            label1.Text = "Email";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Trebuchet MS", 11.25F);
            label2.Location = new Point(38, 127);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 5;
            label2.Text = "Password";
            // 
            // button1
            // 
            button1.Font = new Font("Trebuchet MS", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button1.Location = new Point(35, 198);
            button1.Name = "button1";
            button1.Size = new Size(224, 36);
            button1.TabIndex = 6;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(65, 247);
            label3.Name = "label3";
            label3.Size = new Size(106, 15);
            label3.TabIndex = 7;
            label3.Text = "Not registered yet?";
            // 
            // register_link
            // 
            register_link.AutoSize = true;
            register_link.Location = new Point(167, 247);
            register_link.Name = "register_link";
            register_link.Size = new Size(64, 15);
            register_link.TabIndex = 8;
            register_link.TabStop = true;
            register_link.Text = "Click Here.";
            register_link.LinkClicked += register_link_LinkClicked;
            // 
            // login_pnl
            // 
            login_pnl.BackColor = Color.White;
            login_pnl.Controls.Add(pictureBox14);
            login_pnl.Controls.Add(button1);
            login_pnl.Controls.Add(register_link);
            login_pnl.Controls.Add(label3);
            login_pnl.Controls.Add(branch_txt);
            login_pnl.Controls.Add(logEmail_txt);
            login_pnl.Controls.Add(logPass_txt);
            login_pnl.Controls.Add(label2);
            login_pnl.Controls.Add(label1);
            login_pnl.Location = new Point(252, 139);
            login_pnl.Name = "login_pnl";
            login_pnl.Size = new Size(296, 291);
            login_pnl.TabIndex = 9;
            // 
            // pictureBox14
            // 
            pictureBox14.Image = Properties.Resources.Caret_down;
            pictureBox14.Location = new Point(237, 42);
            pictureBox14.Name = "pictureBox14";
            pictureBox14.Size = new Size(19, 19);
            pictureBox14.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox14.TabIndex = 23;
            pictureBox14.TabStop = false;
            pictureBox14.Click += branch_txt_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Caret_down;
            pictureBox2.Location = new Point(221, 7);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(19, 19);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 26;
            pictureBox2.TabStop = false;
            pictureBox2.Click += usertype_dropdown;
            // 
            // userType_txt
            // 
            userType_txt.BackColor = SystemColors.ControlLightLight;
            userType_txt.ContextMenuStrip = userType_cms;
            userType_txt.Font = new Font("Trebuchet MS", 11.25F);
            userType_txt.Location = new Point(20, 4);
            userType_txt.Name = "userType_txt";
            userType_txt.PlaceholderText = "User Type";
            userType_txt.ReadOnly = true;
            userType_txt.Size = new Size(223, 25);
            userType_txt.TabIndex = 25;
            userType_txt.Click += usertype_dropdown;
            // 
            // userType_cms
            // 
            userType_cms.Items.AddRange(new ToolStripItem[] { professorToolStripMenuItem, staffToolStripMenuItem, studentToolStripMenuItem });
            userType_cms.Name = "userType_cms";
            userType_cms.Size = new Size(124, 70);
            userType_cms.ItemClicked += userType_cms_ItemClicked;
            // 
            // professorToolStripMenuItem
            // 
            professorToolStripMenuItem.Name = "professorToolStripMenuItem";
            professorToolStripMenuItem.Size = new Size(123, 22);
            professorToolStripMenuItem.Text = "Professor";
            // 
            // staffToolStripMenuItem
            // 
            staffToolStripMenuItem.Name = "staffToolStripMenuItem";
            staffToolStripMenuItem.Size = new Size(123, 22);
            staffToolStripMenuItem.Text = "Staff";
            // 
            // studentToolStripMenuItem
            // 
            studentToolStripMenuItem.Name = "studentToolStripMenuItem";
            studentToolStripMenuItem.Size = new Size(123, 22);
            studentToolStripMenuItem.Text = "Student";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Caret_down;
            pictureBox1.Location = new Point(221, 8);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(19, 18);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 24;
            pictureBox1.TabStop = false;
            pictureBox1.Click += branch_txt2_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Trebuchet MS", 11.25F);
            label7.Location = new Point(23, 4);
            label7.Name = "label7";
            label7.Size = new Size(128, 20);
            label7.TabIndex = 13;
            label7.Text = "Confirm Password";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Trebuchet MS", 11.25F);
            label6.Location = new Point(23, 4);
            label6.Name = "label6";
            label6.Size = new Size(69, 20);
            label6.TabIndex = 12;
            label6.Text = "Password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Trebuchet MS", 11.25F);
            label5.Location = new Point(23, 3);
            label5.Name = "label5";
            label5.Size = new Size(45, 20);
            label5.TabIndex = 11;
            label5.Text = "Email";
            // 
            // login_link
            // 
            login_link.AutoSize = true;
            login_link.Location = new Point(168, 52);
            login_link.Name = "login_link";
            login_link.Size = new Size(64, 15);
            login_link.TabIndex = 10;
            login_link.TabStop = true;
            login_link.Text = "Click Here.";
            login_link.LinkClicked += login_link_LinkClicked;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(31, 52);
            label4.Name = "label4";
            label4.Size = new Size(142, 15);
            label4.TabIndex = 9;
            label4.Text = "Already have an account?";
            // 
            // button3
            // 
            button3.Font = new Font("Trebuchet MS", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button3.Location = new Point(20, 6);
            button3.Name = "button3";
            button3.Size = new Size(223, 36);
            button3.TabIndex = 7;
            button3.Text = "Register";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // regCPass_txt
            // 
            regCPass_txt.Location = new Point(20, 26);
            regCPass_txt.Name = "regCPass_txt";
            regCPass_txt.Size = new Size(223, 23);
            regCPass_txt.TabIndex = 6;
            regCPass_txt.UseSystemPasswordChar = true;
            // 
            // regPass_txt
            // 
            regPass_txt.Location = new Point(20, 26);
            regPass_txt.Name = "regPass_txt";
            regPass_txt.Size = new Size(223, 23);
            regPass_txt.TabIndex = 5;
            regPass_txt.UseSystemPasswordChar = true;
            // 
            // regEmail_txt
            // 
            regEmail_txt.Location = new Point(20, 25);
            regEmail_txt.Name = "regEmail_txt";
            regEmail_txt.Size = new Size(223, 23);
            regEmail_txt.TabIndex = 4;
            // 
            // branch_txt2
            // 
            branch_txt2.BackColor = SystemColors.ControlLightLight;
            branch_txt2.Font = new Font("Trebuchet MS", 11.25F);
            branch_txt2.Location = new Point(20, 4);
            branch_txt2.Name = "branch_txt2";
            branch_txt2.PlaceholderText = "Branch";
            branch_txt2.ReadOnly = true;
            branch_txt2.Size = new Size(223, 25);
            branch_txt2.TabIndex = 3;
            branch_txt2.Click += branch_txt2_Click;
            // 
            // register_pnl
            // 
            register_pnl.AutoSize = true;
            register_pnl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            register_pnl.BackColor = Color.White;
            register_pnl.Controls.Add(panel1);
            register_pnl.Controls.Add(panel2);
            register_pnl.Controls.Add(student_pnl);
            register_pnl.Controls.Add(panel7);
            register_pnl.Controls.Add(panel8);
            register_pnl.Controls.Add(panel3);
            register_pnl.Controls.Add(panel4);
            register_pnl.Controls.Add(panel5);
            register_pnl.Controls.Add(panel6);
            register_pnl.FlowDirection = FlowDirection.TopDown;
            register_pnl.Location = new Point(134, 157);
            register_pnl.MaximumSize = new Size(0, 259);
            register_pnl.Name = "register_pnl";
            register_pnl.Size = new Size(536, 254);
            register_pnl.TabIndex = 11;
            register_pnl.Visible = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(branch_txt2);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(262, 32);
            panel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(pictureBox2);
            panel2.Controls.Add(userType_txt);
            panel2.Location = new Point(3, 41);
            panel2.Name = "panel2";
            panel2.Size = new Size(262, 32);
            panel2.TabIndex = 25;
            // 
            // student_pnl
            // 
            student_pnl.Controls.Add(studentId_txt);
            student_pnl.Controls.Add(label8);
            student_pnl.Location = new Point(3, 79);
            student_pnl.Name = "student_pnl";
            student_pnl.Size = new Size(262, 52);
            student_pnl.TabIndex = 30;
            student_pnl.Visible = false;
            // 
            // studentId_txt
            // 
            studentId_txt.Location = new Point(20, 25);
            studentId_txt.Name = "studentId_txt";
            studentId_txt.Size = new Size(223, 23);
            studentId_txt.TabIndex = 4;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Trebuchet MS", 11.25F);
            label8.Location = new Point(23, 3);
            label8.Name = "label8";
            label8.Size = new Size(78, 20);
            label8.TabIndex = 11;
            label8.Text = "Student ID";
            // 
            // panel7
            // 
            panel7.Controls.Add(fName_txt);
            panel7.Controls.Add(label9);
            panel7.Location = new Point(3, 137);
            panel7.Name = "panel7";
            panel7.Size = new Size(262, 52);
            panel7.TabIndex = 31;
            // 
            // fName_txt
            // 
            fName_txt.Location = new Point(20, 25);
            fName_txt.Name = "fName_txt";
            fName_txt.Size = new Size(223, 23);
            fName_txt.TabIndex = 4;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Trebuchet MS", 11.25F);
            label9.Location = new Point(23, 3);
            label9.Name = "label9";
            label9.Size = new Size(83, 20);
            label9.TabIndex = 11;
            label9.Text = "First Name";
            // 
            // panel8
            // 
            panel8.Controls.Add(lName_txt);
            panel8.Controls.Add(label10);
            panel8.Location = new Point(3, 195);
            panel8.Name = "panel8";
            panel8.Size = new Size(262, 52);
            panel8.TabIndex = 32;
            // 
            // lName_txt
            // 
            lName_txt.Location = new Point(20, 25);
            lName_txt.Name = "lName_txt";
            lName_txt.Size = new Size(223, 23);
            lName_txt.TabIndex = 4;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Trebuchet MS", 11.25F);
            label10.Location = new Point(23, 3);
            label10.Name = "label10";
            label10.Size = new Size(81, 20);
            label10.TabIndex = 11;
            label10.Text = "Last Name";
            // 
            // panel3
            // 
            panel3.Controls.Add(regEmail_txt);
            panel3.Controls.Add(label5);
            panel3.Location = new Point(271, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(262, 52);
            panel3.TabIndex = 26;
            // 
            // panel4
            // 
            panel4.Controls.Add(regPass_txt);
            panel4.Controls.Add(label6);
            panel4.Location = new Point(271, 61);
            panel4.Name = "panel4";
            panel4.Size = new Size(262, 52);
            panel4.TabIndex = 27;
            // 
            // panel5
            // 
            panel5.Controls.Add(label7);
            panel5.Controls.Add(regCPass_txt);
            panel5.Location = new Point(271, 119);
            panel5.Name = "panel5";
            panel5.Size = new Size(262, 52);
            panel5.TabIndex = 28;
            // 
            // panel6
            // 
            panel6.Controls.Add(login_link);
            panel6.Controls.Add(button3);
            panel6.Controls.Add(label4);
            panel6.Location = new Point(271, 177);
            panel6.Name = "panel6";
            panel6.Size = new Size(262, 74);
            panel6.TabIndex = 29;
            // 
            // panel9
            // 
            panel9.Controls.Add(pictureBox3);
            panel9.Controls.Add(label11);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(0, 0);
            panel9.Name = "panel9";
            panel9.Size = new Size(800, 51);
            panel9.TabIndex = 12;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = Properties.Resources.Library_icon;
            pictureBox3.Location = new Point(328, 9);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(32, 32);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 5;
            pictureBox3.TabStop = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Trebuchet MS", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(366, 13);
            label11.Name = "label11";
            label11.Size = new Size(107, 24);
            label11.TabIndex = 4;
            label11.Text = "Library App";
            // 
            // WelcomePage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 568);
            Controls.Add(panel9);
            Controls.Add(login_pnl);
            Controls.Add(register_pnl);
            FormBorderStyle = FormBorderStyle.None;
            Name = "WelcomePage";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WelcomePage";
            branch_cms.ResumeLayout(false);
            login_pnl.ResumeLayout(false);
            login_pnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox14).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            userType_cms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            register_pnl.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            student_pnl.ResumeLayout(false);
            student_pnl.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ContextMenuStrip branch_cms;
        private ToolStripMenuItem sanBartolomeToolStripMenuItem;
        private ToolStripMenuItem sanFranciscoToolStripMenuItem;
        private ToolStripMenuItem batasanToolStripMenuItem;
        private TextBox branch_txt;
        private TextBox logEmail_txt;
        private TextBox logPass_txt;
        private Label label1;
        private Label label2;
        private Button button1;
        private Label label3;
        private LinkLabel register_link;
        private Panel login_pnl;
        private LinkLabel login_link;
        private Label label4;
        private Button button3;
        private TextBox regCPass_txt;
        private TextBox regPass_txt;
        private TextBox regEmail_txt;
        private TextBox branch_txt2;
        private Label label7;
        private Label label6;
        private Label label5;
        private PictureBox pictureBox14;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private TextBox userType_txt;
        private ContextMenuStrip userType_cms;
        private ToolStripMenuItem professorToolStripMenuItem;
        private ToolStripMenuItem staffToolStripMenuItem;
        private ToolStripMenuItem studentToolStripMenuItem;
        private FlowLayoutPanel register_pnl;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel student_pnl;
        private TextBox studentId_txt;
        private Label label8;
        private Panel panel7;
        private TextBox fName_txt;
        private Label label9;
        private Panel panel8;
        private TextBox lName_txt;
        private Label label10;
        private Panel panel9;
        private PictureBox pictureBox3;
        private Label label11;
    }
}