using Microsoft.Data.Sqlite;
using zjq.Models;
using zjq.Services;

namespace zjq.Repositories
{
    public class MaintenanceRecordRepository
    {
        private readonly DatabaseService _dbService;

        public MaintenanceRecordRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<MaintenanceRecord>> GetAllAsync()
        {
            var records = new List<MaintenanceRecord>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM MaintenanceRecords";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        records.Add(new MaintenanceRecord
                        {
                            Id = reader.GetInt32(0),
                            SelfRescuerId = reader.GetInt32(1),
                            MaintenanceDate = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            MaintenanceType = reader.GetString(3),
                            Technician = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Description = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Notes = reader.IsDBNull(6) ? null : reader.GetString(6),
                            CreatedAt = reader.IsDBNull(7) ? DateTime.Now : DateTime.Parse(reader.GetString(7))
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing other records
                        System.Diagnostics.Debug.WriteLine($"Error parsing maintenance record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                System.Diagnostics.Debug.WriteLine($"Error getting maintenance records: {ex.Message}");
            }

            return records;
        }

        public List<MaintenanceRecord> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<List<MaintenanceRecord>> GetBySelfRescuerIdAsync(int selfRescuerId)
        {
            var records = new List<MaintenanceRecord>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM MaintenanceRecords WHERE SelfRescuerId = @SelfRescuerId";
                command.Parameters.AddWithValue("@SelfRescuerId", selfRescuerId);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        records.Add(new MaintenanceRecord
                        {
                            Id = reader.GetInt32(0),
                            SelfRescuerId = reader.GetInt32(1),
                            MaintenanceDate = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            MaintenanceType = reader.GetString(3),
                            Technician = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Description = reader.IsDBNull(5) ? null : reader.GetString(5),
                            Notes = reader.IsDBNull(6) ? null : reader.GetString(6),
                            CreatedAt = reader.IsDBNull(7) ? DateTime.Now : DateTime.Parse(reader.GetString(7))
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing other records
                        System.Diagnostics.Debug.WriteLine($"Error parsing maintenance record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                System.Diagnostics.Debug.WriteLine($"Error getting maintenance records by self rescuer id: {ex.Message}");
            }

            return records;
        }

        public List<MaintenanceRecord> GetBySelfRescuerId(int selfRescuerId)
        {
            return GetBySelfRescuerIdAsync(selfRescuerId).Result;
        }

        public async Task<int> AddAsync(MaintenanceRecord record)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO MaintenanceRecords (
                        SelfRescuerId, MaintenanceDate, MaintenanceType,
                        Technician, Description, Notes, CreatedAt
                    ) VALUES (
                        @SelfRescuerId, @MaintenanceDate, @MaintenanceType,
                        @Technician, @Description, @Notes, @CreatedAt
                    )";
            
                command.Parameters.AddWithValue("@SelfRescuerId", record.SelfRescuerId);
                command.Parameters.AddWithValue("@MaintenanceDate", record.MaintenanceDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@MaintenanceType", record.MaintenanceType);
                command.Parameters.AddWithValue("@Technician", string.IsNullOrEmpty(record.Technician) ? (object)DBNull.Value : record.Technician);
                command.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(record.Description) ? (object)DBNull.Value : record.Description);
                command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(record.Notes) ? (object)DBNull.Value : record.Notes);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                await command.ExecuteNonQueryAsync();
            
                // 获取最后插入的ID
                var lastIdCommand = connection.CreateCommand();
                lastIdCommand.CommandText = "SELECT last_insert_rowid()";
                return Convert.ToInt32(await lastIdCommand.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                // Log the error and return -1
                System.Diagnostics.Debug.WriteLine($"Error adding maintenance record: {ex.Message}");
                return -1;
            }
        }

        public int Add(MaintenanceRecord record)
        {
            return AddAsync(record).Result;
        }
    }
}