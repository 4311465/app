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
                        SelfRescueId TEXT NOT NULL UNIQUE,
                        CreateTime TEXT,
                        CheckTime TEXT,
                        VerifyResult INTEGER,
                        Temp REAL,
                        Hs REAL,
                        SelfRescueInfo TEXT NOT NULL,
                        SelfRescueUrl TEXT NOT NULL,
                        SelfRescueModel TEXT NOT NULL,
                        SelfRescueSafeCode TEXT NOT NULL,
                        SelfRescueName TEXT NOT NULL,
                        SelfRescueIsValid TEXT NOT NULL,
                        SelfRescueCompany TEXT NOT NULL,
                        SelfRescueValidDate TEXT NOT NULL,
                        SelfRescueValidStart TEXT,
                        SelfRescueValidEnd TEXT,
                        ProcessingStatus INTEGER,
                        ProcessingCount INTEGER,
                        EmployeeId INTEGER,
                        DeviceType INTEGER DEFAULT 0,
                        InspectorName TEXT,
                        PositivePressure REAL,
                        PositivePressureTime TEXT,
                        NegativePressure REAL,
                        NegativePressureTime TEXT,
                        ExhaustPressure REAL,
                        ExhaustPressureTime TEXT,
                        QuantitativeOxygen REAL,
                        QuantitativeOxygenTime TEXT,
                        ManualOxygen REAL,
                        ManualOxygenTime TEXT
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

                // 创建部门表
                var departmentTableCommand = connection.CreateCommand();
                departmentTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Departments (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        CreatedAt TEXT
                    )";
                departmentTableCommand.ExecuteNonQuery();

                // 创建职位表
                var positionTableCommand = connection.CreateCommand();
                positionTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Positions (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        CreatedAt TEXT
                    )";
                positionTableCommand.ExecuteNonQuery();

                // 创建员工表
                var employeeTableCommand = connection.CreateCommand();
                employeeTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Employees (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        EmployeeNumber TEXT NOT NULL,
                        Email TEXT,
                        SelfRescueId TEXT,
                        Phone TEXT,
                        HireDate TEXT,
                        CreatedAt TEXT,
                        DepartmentId INTEGER,
                        PositionId INTEGER,
                        FOREIGN KEY (DepartmentId) REFERENCES Departments (Id),
                        FOREIGN KEY (PositionId) REFERENCES Positions (Id)
                    )";
                employeeTableCommand.ExecuteNonQuery();

                // 创建检查信息表
                var checkInfoTableCommand = connection.CreateCommand();
                checkInfoTableCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS CheckInfos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CheckDate TEXT NOT NULL,
                        CheckResult TEXT NOT NULL,
                        Notes TEXT,
                        EmployeeId INTEGER,
                        FOREIGN KEY (EmployeeId) REFERENCES Employees (Id)
                    )";
                checkInfoTableCommand.ExecuteNonQuery();

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