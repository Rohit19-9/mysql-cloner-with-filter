# MySQL Smart Database Cloner
This is a powerful **C# .NET** utility designed to clone existing MySQL databases. Unlike standard cloning tools, this project provides a **selective migration system**, giving you the flexibility to choose which tables should be cloned with data and which should remain empty (schema only).

## Key Features
* **Full Schema Replication:** Automatically loads and recreates all tables from a source database to a new destination.
* **Conditional Data Loading:** Built-in logic to decide which tables to 'Clear/Blank' and which to 'Keep' during the cloning process.
* **Dynamic Connection Handling:** Seamlessly connects to MySQL servers using optimized .NET connectors.
* **Automated Workflow:** Reduces manual effort in setting up staging or development environments from production backups.

## Built With
* **Language:** C#
* **Framework:** .NET Core / .NET Framework (Choose yours)
* **Database Library:** MySql.Data (or MySqlConnector)
* **IDE:** Visual Studio

## How It Works
1. **Load:** The program fetches the metadata from the source database.
2. **Filter:** The user/system defines the 'Blank Table' list.
3. **Execute:** 
   - Creates the new database.
   - Generates and executes `CREATE TABLE` scripts.
   - Selectively runs `INSERT` commands based on your filter logic.
1. Clone the repo: `git clone https://github.com`
2. Configure your connection strings in the configuration file.
3. Run the application to start the migration wizard.
