using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySqlDbStructureClone
{
    public partial class Create_DB : Form
    {
        private readonly string mysqlBinPath =
            @"C:\Program Files\MySQL\MySQL Server 8.0\bin";

        public Create_DB()
        {
            InitializeComponent();
        }

        // ================= LOAD DATABASES =================
        private void btnLoadDb_Click(object sender, EventArgs e)
        {
            try
            {
                cmbOldDb.Items.Clear();
                checkedTables.Items.Clear();

                using (MySqlConnection con =
                    new MySqlConnection(
                        $"Server={txtServer.Text};Uid={txtUser.Text};Pwd={txtPassword.Text};"))
                {
                    con.Open();

                    using (MySqlCommand cmd = new MySqlCommand("SHOW DATABASES", con))
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            cmbOldDb.Items.Add(dr.GetString(0));
                    }
                }

                MessageBox.Show("Databases loaded successfully");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        // ================= LOAD TABLES =================
        private void cmbOldDb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                checkedTables.Items.Clear();

                using (MySqlConnection con =
                    new MySqlConnection(
                        $"Server={txtServer.Text};Database={cmbOldDb.Text};Uid={txtUser.Text};Pwd={txtPassword.Text};"))
                {
                    con.Open();

                    string query =
                        @"SELECT table_name, table_rows
                          FROM information_schema.tables
                          WHERE table_schema = @db";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@db", cmbOldDb.Text);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string table = dr.GetString(0);
                                long rows = 0;

                                if (dr["table_rows"] != DBNull.Value)
                                    long.TryParse(dr["table_rows"].ToString(), out rows);

                                checkedTables.Items.Add(
                                    $"{table}  (Rows: {rows})", false);
                            }
                        }
                    }
                }

                txtLog.AppendText("✔ Tables loaded successfully\r\n");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        // ================= CLONE BUTTON =================
        private async void btnClone_Click(object sender, EventArgs e)
        {
            try
            {
                txtLog.Clear();

                string server = txtServer.Text.Trim();
                string user = txtUser.Text.Trim();
                string pwd = txtPassword.Text;
                string oldDb = cmbOldDb.Text.Trim();
                string newDb = txtNewDb.Text.Trim();

                ValidateDbName(newDb);

                using (MySqlConnection con =
                    new MySqlConnection($"Server={server};Uid={user};Pwd={pwd};"))
                {
                    con.Open();
                    new MySqlCommand(
                        $"CREATE DATABASE IF NOT EXISTS `{newDb}`", con)
                        .ExecuteNonQuery();
                }

                txtLog.AppendText("✔ New database ready\r\n");

                List<string> structureOnlyTables = new List<string>();
                List<string> fullCopyTables = new List<string>();

                foreach (var item in checkedTables.Items)
                {
                    string text = item.ToString();
                    int index = text.IndexOf("  (Rows:");
                    string tableName = index > 0
                        ? text.Substring(0, index).Trim()
                        : text.Trim();

                    if (checkedTables.CheckedItems.Contains(item))
                        structureOnlyTables.Add(tableName);
                    else
                        fullCopyTables.Add(tableName);
                }

                await Task.Run(() =>
                    CloneDatabase(
                        server, user, pwd,
                        oldDb, newDb,
                        structureOnlyTables,
                        fullCopyTables));

                MessageBox.Show("Database Created Successfully",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                Application.Exit();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        // ================= STREAM BASED CLONE =================
        private void CloneDatabase(
            string server, string user, string pwd,
            string oldDb, string newDb,
            List<string> structureOnlyTables,
            List<string> fullCopyTables)
        {
            string mysqldumpExe = Path.Combine(mysqlBinPath, "mysqldump.exe");
            string mysqlExe = Path.Combine(mysqlBinPath, "mysql.exe");

            if (!File.Exists(mysqldumpExe) || !File.Exists(mysqlExe))
                throw new Exception("MySQL tools not found.");

            string authFile = CreateMySqlConfigFile(user, pwd);

            try
            {
                foreach (string table in structureOnlyTables)
                {
                    SafeLog($"Processing (Structure Only) {table}...");
                    PipeDumpToMysql(
                        mysqldumpExe, mysqlExe,
                        authFile, server,
                        oldDb, newDb,
                        $"--no-data {oldDb} {table}",
                        true);
                    SafeLog($"✔ Structure created for {table}");
                }

                foreach (string table in fullCopyTables)
                {
                    SafeLog($"Processing (Full Copy) {table}...");
                    PipeDumpToMysql(
                        mysqldumpExe, mysqlExe,
                        authFile, server,
                        oldDb, newDb,
                        $"{oldDb} {table}",
                        false);
                    SafeLog($"✔ Copied {table} with data");
                }
            }
            finally
            {
                if (File.Exists(authFile))
                    File.Delete(authFile);
            }
        }

        // ================= PIPE METHOD (NO MEMORY LOAD) =================
        private void PipeDumpToMysql(
            string dumpExe,
            string mysqlExe,
            string authFile,
            string server,
            string oldDb,
            string newDb,
            string dumpArgs,
            bool removeAutoIncrement)
        {
            var dumpProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = dumpExe,
                    Arguments = $"--defaults-extra-file=\"{authFile}\" -h{server} {dumpArgs}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var importProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = mysqlExe,
                    Arguments = $"--defaults-extra-file=\"{authFile}\" -h{server} {newDb}",
                    RedirectStandardInput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            dumpProcess.Start();
            importProcess.Start();

            using (var reader = dumpProcess.StandardOutput)
            using (var writer = importProcess.StandardInput)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (removeAutoIncrement)
                        line = Regex.Replace(line,
                            @"AUTO_INCREMENT=\d+\s*",
                            "",
                            RegexOptions.IgnoreCase);

                    writer.WriteLine(line);
                }
            }

            dumpProcess.WaitForExit();
            importProcess.WaitForExit();

            if (dumpProcess.ExitCode != 0)
                throw new Exception(dumpProcess.StandardError.ReadToEnd());

            if (importProcess.ExitCode != 0)
                throw new Exception(importProcess.StandardError.ReadToEnd());
        }

        // ================= HELPERS =================
        private void ValidateDbName(string dbName)
        {
            if (!Regex.IsMatch(dbName, @"^[a-zA-Z0-9_]+$"))
                throw new Exception("Invalid database name.");
        }

        private string CreateMySqlConfigFile(string user, string password)
        {
            string filePath = Path.Combine(
                Path.GetTempPath(),
                $"mysql_auth_{Guid.NewGuid()}.cnf");

            File.WriteAllText(filePath,
$@"[client]
user={user}
password={password}");

            return filePath;
        }

        private void SafeLog(string message)
        {
            if (txtLog.InvokeRequired)
                txtLog.Invoke(new Action(() =>
                    txtLog.AppendText(message + "\r\n")));
            else
                txtLog.AppendText(message + "\r\n");
        }

        private void LogError(Exception ex)
        {
            SafeLog("ERROR: " + ex.Message);

            MessageBox.Show(ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}