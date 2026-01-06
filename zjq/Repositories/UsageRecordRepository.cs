using Microsoft.Data.Sqlite;
using zjq.Models;
using zjq.Services;

namespace zjq.Repositories
{
    public class UsageRecordRepository
    {
        private readonly DatabaseService _dbService;

        public UsageRecordRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<UsageRecord>> GetAllAsync()
        {
            var records = new List<UsageRecord>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM UsageRecords";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        records.Add(new UsageRecord
                        {
                            Id = reader.GetInt32(0),
                            SelfRescuerId = reader.GetInt32(1),
                            UsageDate = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            UserName = reader.GetString(3),
                            Purpose = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ReturnDate = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.Parse(reader.GetString(5)),
                            Condition = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Notes = reader.IsDBNull(7) ? null : reader.GetString(7),
                            CreatedAt = reader.IsDBNull(8) ? DateTime.Now : DateTime.Parse(reader.GetString(8))
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing other records
                        System.Diagnostics.Debug.WriteLine($"Error parsing usage record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                System.Diagnostics.Debug.WriteLine($"Error getting usage records: {ex.Message}");
            }

            return records;
        }

        public List<UsageRecord> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<List<UsageRecord>> GetBySelfRescuerIdAsync(int selfRescuerId)
        {
            var records = new List<UsageRecord>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM UsageRecords WHERE SelfRescuerId = @SelfRescuerId";
                command.Parameters.AddWithValue("@SelfRescuerId", selfRescuerId);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        records.Add(new UsageRecord
                        {
                            Id = reader.GetInt32(0),
                            SelfRescuerId = reader.GetInt32(1),
                            UsageDate = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            UserName = reader.GetString(3),
                            Purpose = reader.IsDBNull(4) ? null : reader.GetString(4),
                            ReturnDate = reader.IsDBNull(5) ? DateTime.MinValue : DateTime.Parse(reader.GetString(5)),
                            Condition = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Notes = reader.IsDBNull(7) ? null : reader.GetString(7),
                            CreatedAt = reader.IsDBNull(8) ? DateTime.Now : DateTime.Parse(reader.GetString(8))
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing other records
                        System.Diagnostics.Debug.WriteLine($"Error parsing usage record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                System.Diagnostics.Debug.WriteLine($"Error getting usage records by self rescuer id: {ex.Message}");
            }

            return records;
        }

        public List<UsageRecord> GetBySelfRescuerId(int selfRescuerId)
        {
            return GetBySelfRescuerIdAsync(selfRescuerId).Result;
        }

        public async Task<int> AddAsync(UsageRecord record)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO UsageRecords (
                        SelfRescuerId, UsageDate, UserName, Purpose,
                        ReturnDate, Condition, Notes, CreatedAt
                    ) VALUES (
                        @SelfRescuerId, @UsageDate, @UserName, @Purpose,
                        @ReturnDate, @Condition, @Notes, @CreatedAt
                    )";
            
                command.Parameters.AddWithValue("@SelfRescuerId", record.SelfRescuerId);
                command.Parameters.AddWithValue("@UsageDate", record.UsageDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@UserName", record.UserName);
                command.Parameters.AddWithValue("@Purpose", string.IsNullOrEmpty(record.Purpose) ? (object)DBNull.Value : record.Purpose);
                command.Parameters.AddWithValue("@ReturnDate", record.ReturnDate == DateTime.MinValue ? (object)DBNull.Value : record.ReturnDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Condition", string.IsNullOrEmpty(record.Condition) ? (object)DBNull.Value : record.Condition);
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
                System.Diagnostics.Debug.WriteLine($"Error adding usage record: {ex.Message}");
                return -1;
            }
        }

        public int Add(UsageRecord record)
        {
            return AddAsync(record).Result;
        }

        public async Task UpdateAsync(UsageRecord record)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE UsageRecords SET
                        ReturnDate = @ReturnDate,
                        Condition = @Condition,
                        Notes = @Notes
                    WHERE Id = @Id";
            
                command.Parameters.AddWithValue("@Id", record.Id);
                command.Parameters.AddWithValue("@ReturnDate", record.ReturnDate == DateTime.MinValue ? (object)DBNull.Value : record.ReturnDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@Condition", string.IsNullOrEmpty(record.Condition) ? (object)DBNull.Value : record.Condition);
                command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(record.Notes) ? (object)DBNull.Value : record.Notes);

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error updating usage record: {ex.Message}");
            }
        }

        public void Update(UsageRecord record)
        {
            UpdateAsync(record).Wait();
        }
    }
}