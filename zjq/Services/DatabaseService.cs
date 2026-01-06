using Microsoft.Data.Sqlite;
using Microsoft.Maui.Storage;
using zjq.Models;

namespace zjq.Services
{
    public class DatabaseService
    {
        private readonly string _databasePath;

        public DatabaseService()
        {
            _databasePath = Path.Combine(FileSystem.AppDataDirectory, "self_rescuer.db");
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqliteConnection($"Filename={_databasePath}");
                connection.Open();

                // 创建状态类型表
                var statusTableCommand = connection.CreateCommand();
                statusTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS StatusTypes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT
                    )";
                statusTableCommand.ExecuteNonQuery();

                // 创建自救器表
                var rescuerTableCommand = connection.CreateCommand();
                rescuerTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS SelfRescuers (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SerialNumber TEXT NOT NULL UNIQUE,
                        Model TEXT,
                        Manufacturer TEXT,
                        ManufactureDate TEXT,
                        ExpiryDate TEXT,
                        StatusId INTEGER,
                        Location TEXT,
                        Notes TEXT,
                        CreatedAt TEXT,
                        UpdatedAt TEXT,
                        FOREIGN KEY (StatusId) REFERENCES StatusTypes (Id)
                    )";
                rescuerTableCommand.ExecuteNonQuery();

                // 创建维护记录表
                var maintenanceTableCommand = connection.CreateCommand();
                maintenanceTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS MaintenanceRecords (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SelfRescuerId INTEGER,
                        MaintenanceDate TEXT,
                        MaintenanceType TEXT,
                        Technician TEXT,
                        Description TEXT,
                        Notes TEXT,
                        CreatedAt TEXT,
                        FOREIGN KEY (SelfRescuerId) REFERENCES SelfRescuers (Id)
                    )";
                maintenanceTableCommand.ExecuteNonQuery();

                // 创建使用记录表
                var usageTableCommand = connection.CreateCommand();
                usageTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS UsageRecords (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        SelfRescuerId INTEGER,
                        UsageDate TEXT,
                        UserName TEXT,
                        Purpose TEXT,
                        ReturnDate TEXT,
                        Condition TEXT,
                        Notes TEXT,
                        CreatedAt TEXT,
                        FOREIGN KEY (SelfRescuerId) REFERENCES SelfRescuers (Id)
                    )";
                usageTableCommand.ExecuteNonQuery();

                // 初始化状态类型数据
                SeedStatusTypes(connection);
            }
            catch (Exception ex)
            {
                // Log the error but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Error initializing database: {ex.Message}");
            }
        }

        private void SeedStatusTypes(SqliteConnection connection)
        {
            try
            {
                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = "SELECT COUNT(*) FROM StatusTypes";
                var count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count == 0)
                {
                    var insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = @"
                        INSERT INTO StatusTypes (Name, Description) VALUES
                        ('正常', '正常可用状态'),
                        ('需要维护', '需要进行维护'),
                        ('过期', '已超过有效期'),
                        ('损坏', '已损坏，不可用'),
                        ('在使用中', '正在被使用')
                    ";
                    insertCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Log the error but don't crash the app
                System.Diagnostics.Debug.WriteLine($"Error seeding status types: {ex.Message}");
            }
        }

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Filename={_databasePath}");
        }
    }
}