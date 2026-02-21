namespace MySqlDbStructureClone
{
    partial class Create_DB
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblOldDb;
        private System.Windows.Forms.Label lblNewDb;
        private System.Windows.Forms.Label lblLog;

        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cmbOldDb;
        private System.Windows.Forms.TextBox txtNewDb;
        private System.Windows.Forms.Button btnLoadDb;
        private System.Windows.Forms.Button btnClone;
        private System.Windows.Forms.TextBox txtLog;

        // ✅ Added
        private System.Windows.Forms.CheckedListBox checkedTables;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Create_DB));

            lblTitle = new Label();
            lblServer = new Label();
            lblUser = new Label();
            lblPassword = new Label();
            lblOldDb = new Label();
            lblNewDb = new Label();
            lblLog = new Label();
            txtServer = new TextBox();
            txtUser = new TextBox();
            txtPassword = new TextBox();
            cmbOldDb = new ComboBox();
            txtNewDb = new TextBox();
            btnLoadDb = new Button();
            btnClone = new Button();
            txtLog = new TextBox();
            checkedTables = new CheckedListBox(); // ✅

            SuspendLayout();

            // Title
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.Location = new Point(161, 10);
            lblTitle.Text = "MYSQL Database Structure Tool";

            // Server
            lblServer.AutoSize = true;
            lblServer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblServer.Location = new Point(30, 55);
            lblServer.Text = "Server";

            txtServer.Location = new Point(30, 72);
            txtServer.Size = new Size(250, 31);
            txtServer.Text = "localhost";

            // User
            lblUser.AutoSize = true;
            lblUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblUser.Location = new Point(30, 105);
            lblUser.Text = "Username";

            txtUser.Location = new Point(30, 122);
            txtUser.Size = new Size(250, 31);
            txtUser.Text = "root";

            // Password
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPassword.Location = new Point(30, 155);
            lblPassword.Text = "Password";

            txtPassword.Location = new Point(30, 172);
            txtPassword.Size = new Size(250, 31);
            txtPassword.PasswordChar = '*';

            // Load DB Button
            btnLoadDb.Location = new Point(30, 205);
            btnLoadDb.Size = new Size(250, 28);
            btnLoadDb.Text = "Load Databases";
            btnLoadDb.Click += btnLoadDb_Click;

            // Old DB
            lblOldDb.AutoSize = true;
            lblOldDb.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblOldDb.Location = new Point(30, 255);
            lblOldDb.Text = "Select Old Database";

            cmbOldDb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbOldDb.Location = new Point(30, 272);
            cmbOldDb.Size = new Size(250, 33);

            // ✅ Important Event
            cmbOldDb.SelectedIndexChanged += cmbOldDb_SelectedIndexChanged;

            // New DB
            lblNewDb.AutoSize = true;
            lblNewDb.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblNewDb.Location = new Point(30, 305);
            lblNewDb.Text = "New Database Name";

            txtNewDb.Location = new Point(30, 322);
            txtNewDb.Size = new Size(250, 31);

            // Clone Button
            btnClone.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClone.Location = new Point(30, 355);
            btnClone.Size = new Size(250, 32);
            btnClone.Text = "Create Database";
            btnClone.Click += btnClone_Click;

            // Log
            lblLog.AutoSize = true;
            lblLog.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblLog.Location = new Point(320, 55);
            lblLog.Text = "Process Log";

            txtLog.Location = new Point(320, 72);
            txtLog.Multiline = true;
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(320, 315);

            // ✅ Checked Tables List
            checkedTables.Location = new Point(30, 400);
            checkedTables.Size = new Size(610, 140);
            checkedTables.CheckOnClick = true;

            // Form
            ClientSize = new Size(670, 560);

            Controls.Add(lblTitle);
            Controls.Add(lblServer);
            Controls.Add(lblUser);
            Controls.Add(lblPassword);
            Controls.Add(lblOldDb);
            Controls.Add(lblNewDb);
            Controls.Add(lblLog);
            Controls.Add(txtServer);
            Controls.Add(txtUser);
            Controls.Add(txtPassword);
            Controls.Add(btnLoadDb);
            Controls.Add(cmbOldDb);
            Controls.Add(txtNewDb);
            Controls.Add(btnClone);
            Controls.Add(txtLog);
            Controls.Add(checkedTables); // ✅ Added

            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Create_DB";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DB Tool";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}
