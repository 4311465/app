using Microsoft.Data.Sqlite;
using zjq.Models;
using zjq.Services;

namespace zjq.Repositories
{
    public class StatusTypeRepository
    {
        private readonly DatabaseService _dbService;

        public StatusTypeRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public async Task<List<StatusType>> GetAllAsync()
        {
            var statusTypes = new List<StatusType>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM StatusTypes";

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        statusTypes.Add(new StatusType
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        });
                    }
                    catch (Exception ex)
                    {
                        // Log the error and continue processing other records
                        System.Diagnostics.Debug.WriteLine($"Error parsing status type record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return an empty list
                System.Diagnostics.Debug.WriteLine($"Error getting status types: {ex.Message}");
            }

            return statusTypes;
        }

        public List<StatusType> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<StatusType> GetByIdAsync(int id)
        {
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM StatusTypes WHERE Id = @Id";
                command.Parameters.AddWithValue("@Id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    try
                    {
                        return new StatusType
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        };
                    }
                    catch (Exception ex)
                    {
                        // Log the error and return null
                        System.Diagnostics.Debug.WriteLine($"Error parsing status type record: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error and return null
                System.Diagnostics.Debug.WriteLine($"Error getting status type by id: {ex.Message}");
            }

            return null;
        }

        public StatusType GetById(int id)
        {
            return GetByIdAsync(id).Result;
        }
    }
}