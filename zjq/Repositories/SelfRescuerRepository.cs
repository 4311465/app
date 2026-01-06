using Microsoft.Data.Sqlite;
using System.Globalization;
using zjq.Models;
using zjq.Services;

namespace zjq.Repositories
{
    public class SelfRescuerRepository
    {
        private readonly DatabaseService _dbService;

        public SelfRescuerRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public List<SelfRescuer> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<List<SelfRescuer>> GetAllAsync()
        {
            var rescuers = new List<SelfRescuer>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM SelfRescuers";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        rescuers.Add(new SelfRescuer
                        {
                            Id = reader.GetInt32(0),
                            SerialNumber = reader.GetString(1),
                            Model = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Manufacturer = reader.IsDBNull(3) ? null : reader.GetString(3),
                            ManufactureDate = reader.IsDBNull(4) ? DateTime.Now : DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ExpiryDate = reader.IsDBNull(5) ? DateTime.Now : DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            StatusId = reader.GetInt32(6),
                            Location = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Notes = reader.IsDBNull(8) ? null : reader.GetString(8),
                            CreatedAt = reader.IsDBNull(9) ? DateTime.Now : DateTime.ParseExact(reader.GetString(9), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            UpdatedAt = reader.IsDBNull(10) ? DateTime.Now : DateTime.ParseExact(reader.GetString(10), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing other records
                        System.Diagnostics.Debug.WriteLine($"Error parsing self-rescuer record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                System.Diagnostics.Debug.WriteLine($"Error getting self-rescuers: {ex.Message}");
            }

            return rescuers;
        }

        public SelfRescuer GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }

        public async Task<SelfRescuer> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM SelfRescuers WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    try
                    {
                        return new SelfRescuer
                        {
                            Id = reader.GetInt32(0),
                            SerialNumber = reader.GetString(1),
                            Model = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Manufacturer = reader.IsDBNull(3) ? null : reader.GetString(3),
                            ManufactureDate = reader.IsDBNull(4) ? DateTime.Now : DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            ExpiryDate = reader.IsDBNull(5) ? DateTime.Now : DateTime.ParseExact(reader.GetString(5), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            StatusId = reader.GetInt32(6),
                            Location = reader.IsDBNull(7) ? null : reader.GetString(7),
                            Notes = reader.IsDBNull(8) ? null : reader.GetString(8),
                            CreatedAt = reader.IsDBNull(9) ? DateTime.Now : DateTime.ParseExact(reader.GetString(9), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                            UpdatedAt = reader.IsDBNull(10) ? DateTime.Now : DateTime.ParseExact(reader.GetString(10), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                        };
                    }
                    catch (Exception ex)
                    {
                        // Log the error and return null
                        System.Diagnostics.Debug.WriteLine($"Error parsing self-rescuer record: {ex.Message}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return null
                System.Diagnostics.Debug.WriteLine($"Error getting self-rescuer by id: {ex.Message}");
            }

            return null;
        }

        public int Add(SelfRescuer rescuer)
        {
            return AddAsync(rescuer).Result;
        }

        public async Task<int> AddAsync(SelfRescuer rescuer)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO SelfRescuers (
                        SerialNumber, Model, Manufacturer, ManufactureDate, ExpiryDate,
                        StatusId, Location, Notes, CreatedAt, UpdatedAt
                    ) VALUES (
                        @SerialNumber, @Model, @Manufacturer, @ManufactureDate, @ExpiryDate,
                        @StatusId, @Location, @Notes, @CreatedAt, @UpdatedAt
                    )";
            
                var now = DateTime.Now;
                command.Parameters.AddWithValue("@SerialNumber", rescuer.SerialNumber);
                command.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(rescuer.Model) ? (object)DBNull.Value : rescuer.Model);
                command.Parameters.AddWithValue("@Manufacturer", string.IsNullOrEmpty(rescuer.Manufacturer) ? (object)DBNull.Value : rescuer.Manufacturer);
                command.Parameters.AddWithValue("@ManufactureDate", rescuer.ManufactureDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ExpiryDate", rescuer.ExpiryDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@StatusId", rescuer.StatusId);
                command.Parameters.AddWithValue("@Location", string.IsNullOrEmpty(rescuer.Location) ? (object)DBNull.Value : rescuer.Location);
                command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(rescuer.Notes) ? (object)DBNull.Value : rescuer.Notes);
                command.Parameters.AddWithValue("@CreatedAt", now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@UpdatedAt", now.ToString("yyyy-MM-dd HH:mm:ss"));

                await command.ExecuteNonQueryAsync();
            
                // 获取最后插入的ID
                var lastIdCommand = connection.CreateCommand();
                lastIdCommand.CommandText = "SELECT last_insert_rowid()";
                return Convert.ToInt32(await lastIdCommand.ExecuteScalarAsync());
            }
            catch (Exception ex)
            {
                // Log the error and return -1
                System.Diagnostics.Debug.WriteLine($"Error adding self-rescuer: {ex.Message}");
                return -1;
            }
        }

        public void Update(SelfRescuer rescuer)
        {
            UpdateAsync(rescuer).Wait();
        }

        public async Task UpdateAsync(SelfRescuer rescuer)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE SelfRescuers SET
                        SerialNumber = @SerialNumber,
                        Model = @Model,
                        Manufacturer = @Manufacturer,
                        ManufactureDate = @ManufactureDate,
                        ExpiryDate = @ExpiryDate,
                        StatusId = @StatusId,
                        Location = @Location,
                        Notes = @Notes,
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id";
            
                command.Parameters.AddWithValue("@Id", rescuer.Id);
                command.Parameters.AddWithValue("@SerialNumber", rescuer.SerialNumber);
                command.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(rescuer.Model) ? (object)DBNull.Value : rescuer.Model);
                command.Parameters.AddWithValue("@Manufacturer", string.IsNullOrEmpty(rescuer.Manufacturer) ? (object)DBNull.Value : rescuer.Manufacturer);
                command.Parameters.AddWithValue("@ManufactureDate", rescuer.ManufactureDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@ExpiryDate", rescuer.ExpiryDate.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@StatusId", rescuer.StatusId);
                command.Parameters.AddWithValue("@Location", string.IsNullOrEmpty(rescuer.Location) ? (object)DBNull.Value : rescuer.Location);
                command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(rescuer.Notes) ? (object)DBNull.Value : rescuer.Notes);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error updating self-rescuer: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            DeleteAsync(id).Wait();
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM SelfRescuers WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                // Log the error
                System.Diagnostics.Debug.WriteLine($"Error deleting self-rescuer: {ex.Message}");
            }
        }
    }
}