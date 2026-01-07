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
                            SelfRescueId = reader.GetString(1),
                            CreateTime = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            CheckTime = reader.IsDBNull(3) ? null : DateTime.Parse(reader.GetString(3)),
                            VerifyResult = reader.IsDBNull(4) ? null : (byte?)reader.GetInt32(4),
                            Temp = reader.IsDBNull(5) ? null : (float?)reader.GetDouble(5),
                            Hs = reader.IsDBNull(6) ? null : (float?)reader.GetDouble(6),
                            SelfRescueInfo = reader.GetString(7),
                            SelfRescueUrl = reader.GetString(8),
                            SelfRescueModel = reader.GetString(9),
                            SelfRescueSafeCode = reader.GetString(10),
                            SelfRescueName = reader.GetString(11),
                            SelfRescueIsValid = reader.GetString(12),
                            SelfRescueCompany = reader.GetString(13),
                            SelfRescueValidDate = reader.GetString(14),
                            SelfRescueValidStart = reader.IsDBNull(15) ? null : DateTime.Parse(reader.GetString(15)),
                            SelfRescueValidEnd = reader.IsDBNull(16) ? null : DateTime.Parse(reader.GetString(16)),
                            ProcessingStatus = reader.IsDBNull(17) ? null : (byte?)reader.GetInt32(17),
                            ProcessingCount = reader.IsDBNull(18) ? null : reader.GetInt32(18),
                            EmployeeId = reader.IsDBNull(19) ? null : reader.GetInt32(19),
                            DeviceType = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                            InspectorName = reader.IsDBNull(21) ? null : reader.GetString(21),
                            PositivePressure = reader.IsDBNull(22) ? null : (float?)reader.GetDouble(22),
                            PositivePressureTime = reader.IsDBNull(23) ? null : DateTime.Parse(reader.GetString(23)),
                            NegativePressure = reader.IsDBNull(24) ? null : (float?)reader.GetDouble(24),
                            NegativePressureTime = reader.IsDBNull(25) ? null : DateTime.Parse(reader.GetString(25)),
                            ExhaustPressure = reader.IsDBNull(26) ? null : (float?)reader.GetDouble(26),
                            ExhaustPressureTime = reader.IsDBNull(27) ? null : DateTime.Parse(reader.GetString(27)),
                            QuantitativeOxygen = reader.IsDBNull(28) ? null : (float?)reader.GetDouble(28),
                            QuantitativeOxygenTime = reader.IsDBNull(29) ? null : DateTime.Parse(reader.GetString(29)),
                            ManualOxygen = reader.IsDBNull(30) ? null : (float?)reader.GetDouble(30),
                            ManualOxygenTime = reader.IsDBNull(31) ? null : DateTime.Parse(reader.GetString(31))
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
                            SelfRescueId = reader.GetString(1),
                            CreateTime = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            CheckTime = reader.IsDBNull(3) ? null : DateTime.Parse(reader.GetString(3)),
                            VerifyResult = reader.IsDBNull(4) ? null : (byte?)reader.GetInt32(4),
                            Temp = reader.IsDBNull(5) ? null : (float?)reader.GetDouble(5),
                            Hs = reader.IsDBNull(6) ? null : (float?)reader.GetDouble(6),
                            SelfRescueInfo = reader.GetString(7),
                            SelfRescueUrl = reader.GetString(8),
                            SelfRescueModel = reader.GetString(9),
                            SelfRescueSafeCode = reader.GetString(10),
                            SelfRescueName = reader.GetString(11),
                            SelfRescueIsValid = reader.GetString(12),
                            SelfRescueCompany = reader.GetString(13),
                            SelfRescueValidDate = reader.GetString(14),
                            SelfRescueValidStart = reader.IsDBNull(15) ? null : DateTime.Parse(reader.GetString(15)),
                            SelfRescueValidEnd = reader.IsDBNull(16) ? null : DateTime.Parse(reader.GetString(16)),
                            ProcessingStatus = reader.IsDBNull(17) ? null : (byte?)reader.GetInt32(17),
                            ProcessingCount = reader.IsDBNull(18) ? null : reader.GetInt32(18),
                            EmployeeId = reader.IsDBNull(19) ? null : reader.GetInt32(19),
                            DeviceType = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                            InspectorName = reader.IsDBNull(21) ? null : reader.GetString(21),
                            PositivePressure = reader.IsDBNull(22) ? null : (float?)reader.GetDouble(22),
                            PositivePressureTime = reader.IsDBNull(23) ? null : DateTime.Parse(reader.GetString(23)),
                            NegativePressure = reader.IsDBNull(24) ? null : (float?)reader.GetDouble(24),
                            NegativePressureTime = reader.IsDBNull(25) ? null : DateTime.Parse(reader.GetString(25)),
                            ExhaustPressure = reader.IsDBNull(26) ? null : (float?)reader.GetDouble(26),
                            ExhaustPressureTime = reader.IsDBNull(27) ? null : DateTime.Parse(reader.GetString(27)),
                            QuantitativeOxygen = reader.IsDBNull(28) ? null : (float?)reader.GetDouble(28),
                            QuantitativeOxygenTime = reader.IsDBNull(29) ? null : DateTime.Parse(reader.GetString(29)),
                            ManualOxygen = reader.IsDBNull(30) ? null : (float?)reader.GetDouble(30),
                            ManualOxygenTime = reader.IsDBNull(31) ? null : DateTime.Parse(reader.GetString(31))
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

        public async Task<List<SelfRescuer>> GetByUrlAsync(string url)
        {
            var rescuers = new List<SelfRescuer>();
            try
            {
                using var connection = _dbService.GetConnection();
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM SelfRescuers WHERE SelfRescueUrl = @Url";
                command.Parameters.AddWithValue("@Url", url);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    try
                    {
                        rescuers.Add(new SelfRescuer
                        {
                            Id = reader.GetInt32(0),
                            SelfRescueId = reader.GetString(1),
                            CreateTime = reader.IsDBNull(2) ? DateTime.Now : DateTime.Parse(reader.GetString(2)),
                            CheckTime = reader.IsDBNull(3) ? null : DateTime.Parse(reader.GetString(3)),
                            VerifyResult = reader.IsDBNull(4) ? null : (byte?)reader.GetInt32(4),
                            Temp = reader.IsDBNull(5) ? null : (float?)reader.GetDouble(5),
                            Hs = reader.IsDBNull(6) ? null : (float?)reader.GetDouble(6),
                            SelfRescueInfo = reader.GetString(7),
                            SelfRescueUrl = reader.GetString(8),
                            SelfRescueModel = reader.GetString(9),
                            SelfRescueSafeCode = reader.GetString(10),
                            SelfRescueName = reader.GetString(11),
                            SelfRescueIsValid = reader.GetString(12),
                            SelfRescueCompany = reader.GetString(13),
                            SelfRescueValidDate = reader.GetString(14),
                            SelfRescueValidStart = reader.IsDBNull(15) ? null : DateTime.Parse(reader.GetString(15)),
                            SelfRescueValidEnd = reader.IsDBNull(16) ? null : DateTime.Parse(reader.GetString(16)),
                            ProcessingStatus = reader.IsDBNull(17) ? null : (byte?)reader.GetInt32(17),
                            ProcessingCount = reader.IsDBNull(18) ? null : reader.GetInt32(18),
                            EmployeeId = reader.IsDBNull(19) ? null : reader.GetInt32(19),
                            DeviceType = reader.IsDBNull(20) ? 0 : reader.GetInt32(20),
                            InspectorName = reader.IsDBNull(21) ? null : reader.GetString(21),
                            PositivePressure = reader.IsDBNull(22) ? null : (float?)reader.GetDouble(22),
                            PositivePressureTime = reader.IsDBNull(23) ? null : DateTime.Parse(reader.GetString(23)),
                            NegativePressure = reader.IsDBNull(24) ? null : (float?)reader.GetDouble(24),
                            NegativePressureTime = reader.IsDBNull(25) ? null : DateTime.Parse(reader.GetString(25)),
                            ExhaustPressure = reader.IsDBNull(26) ? null : (float?)reader.GetDouble(26),
                            ExhaustPressureTime = reader.IsDBNull(27) ? null : DateTime.Parse(reader.GetString(27)),
                            QuantitativeOxygen = reader.IsDBNull(28) ? null : (float?)reader.GetDouble(28),
                            QuantitativeOxygenTime = reader.IsDBNull(29) ? null : DateTime.Parse(reader.GetString(29)),
                            ManualOxygen = reader.IsDBNull(30) ? null : (float?)reader.GetDouble(30),
                            ManualOxygenTime = reader.IsDBNull(31) ? null : DateTime.Parse(reader.GetString(31))
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
                System.Diagnostics.Debug.WriteLine($"Error getting self-rescuers by URL: {ex.Message}");
            }

            return rescuers;
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
                        SelfRescueId, CreateTime, CheckTime, VerifyResult, Temp, Hs,
                        SelfRescueInfo, SelfRescueUrl, SelfRescueModel, SelfRescueSafeCode,
                        SelfRescueName, SelfRescueIsValid, SelfRescueCompany, SelfRescueValidDate,
                        SelfRescueValidStart, SelfRescueValidEnd, ProcessingStatus, ProcessingCount,
                        EmployeeId, DeviceType, InspectorName, PositivePressure, PositivePressureTime,
                        NegativePressure, NegativePressureTime, ExhaustPressure, ExhaustPressureTime,
                        QuantitativeOxygen, QuantitativeOxygenTime, ManualOxygen, ManualOxygenTime
                    ) VALUES (
                        @SelfRescueId, @CreateTime, @CheckTime, @VerifyResult, @Temp, @Hs,
                        @SelfRescueInfo, @SelfRescueUrl, @SelfRescueModel, @SelfRescueSafeCode,
                        @SelfRescueName, @SelfRescueIsValid, @SelfRescueCompany, @SelfRescueValidDate,
                        @SelfRescueValidStart, @SelfRescueValidEnd, @ProcessingStatus, @ProcessingCount,
                        @EmployeeId, @DeviceType, @InspectorName, @PositivePressure, @PositivePressureTime,
                        @NegativePressure, @NegativePressureTime, @ExhaustPressure, @ExhaustPressureTime,
                        @QuantitativeOxygen, @QuantitativeOxygenTime, @ManualOxygen, @ManualOxygenTime
                    )";
        
                var now = DateTime.Now;
                command.Parameters.AddWithValue("@SelfRescueId", rescuer.SelfRescueId);
                command.Parameters.AddWithValue("@CreateTime", rescuer.CreateTime.ToString());
                command.Parameters.AddWithValue("@CheckTime", rescuer.CheckTime.HasValue ? (object)rescuer.CheckTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@VerifyResult", rescuer.VerifyResult.HasValue ? (object)rescuer.VerifyResult.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Temp", rescuer.Temp.HasValue ? (object)rescuer.Temp.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Hs", rescuer.Hs.HasValue ? (object)rescuer.Hs.Value : DBNull.Value);
                command.Parameters.AddWithValue("@SelfRescueInfo", rescuer.SelfRescueInfo);
                command.Parameters.AddWithValue("@SelfRescueUrl", rescuer.SelfRescueUrl);
                command.Parameters.AddWithValue("@SelfRescueModel", rescuer.SelfRescueModel);
                command.Parameters.AddWithValue("@SelfRescueSafeCode", rescuer.SelfRescueSafeCode);
                command.Parameters.AddWithValue("@SelfRescueName", rescuer.SelfRescueName);
                command.Parameters.AddWithValue("@SelfRescueIsValid", rescuer.SelfRescueIsValid);
                command.Parameters.AddWithValue("@SelfRescueCompany", rescuer.SelfRescueCompany);
                command.Parameters.AddWithValue("@SelfRescueValidDate", rescuer.SelfRescueValidDate);
                command.Parameters.AddWithValue("@SelfRescueValidStart", rescuer.SelfRescueValidStart.HasValue ? (object)rescuer.SelfRescueValidStart.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@SelfRescueValidEnd", rescuer.SelfRescueValidEnd.HasValue ? (object)rescuer.SelfRescueValidEnd.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@ProcessingStatus", rescuer.ProcessingStatus.HasValue ? (object)rescuer.ProcessingStatus.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ProcessingCount", rescuer.ProcessingCount.HasValue ? (object)rescuer.ProcessingCount.Value : DBNull.Value);
                command.Parameters.AddWithValue("@EmployeeId", rescuer.EmployeeId.HasValue ? (object)rescuer.EmployeeId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@DeviceType", rescuer.DeviceType);
                command.Parameters.AddWithValue("@InspectorName", string.IsNullOrEmpty(rescuer.InspectorName) ? (object)DBNull.Value : rescuer.InspectorName);
                command.Parameters.AddWithValue("@PositivePressure", rescuer.PositivePressure.HasValue ? (object)rescuer.PositivePressure.Value : DBNull.Value);
                command.Parameters.AddWithValue("@PositivePressureTime", rescuer.PositivePressureTime.HasValue ? (object)rescuer.PositivePressureTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@NegativePressure", rescuer.NegativePressure.HasValue ? (object)rescuer.NegativePressure.Value : DBNull.Value);
                command.Parameters.AddWithValue("@NegativePressureTime", rescuer.NegativePressureTime.HasValue ? (object)rescuer.NegativePressureTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@ExhaustPressure", rescuer.ExhaustPressure.HasValue ? (object)rescuer.ExhaustPressure.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ExhaustPressureTime", rescuer.ExhaustPressureTime.HasValue ? (object)rescuer.ExhaustPressureTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@QuantitativeOxygen", rescuer.QuantitativeOxygen.HasValue ? (object)rescuer.QuantitativeOxygen.Value : DBNull.Value);
                command.Parameters.AddWithValue("@QuantitativeOxygenTime", rescuer.QuantitativeOxygenTime.HasValue ? (object)rescuer.QuantitativeOxygenTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@ManualOxygen", rescuer.ManualOxygen.HasValue ? (object)rescuer.ManualOxygen.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ManualOxygenTime", rescuer.ManualOxygenTime.HasValue ? (object)rescuer.ManualOxygenTime.Value.ToString() : DBNull.Value);

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
                        SelfRescueId = @SelfRescueId,
                        CreateTime = @CreateTime,
                        CheckTime = @CheckTime,
                        VerifyResult = @VerifyResult,
                        Temp = @Temp,
                        Hs = @Hs,
                        SelfRescueInfo = @SelfRescueInfo,
                        SelfRescueUrl = @SelfRescueUrl,
                        SelfRescueModel = @SelfRescueModel,
                        SelfRescueSafeCode = @SelfRescueSafeCode,
                        SelfRescueName = @SelfRescueName,
                        SelfRescueIsValid = @SelfRescueIsValid,
                        SelfRescueCompany = @SelfRescueCompany,
                        SelfRescueValidDate = @SelfRescueValidDate,
                        SelfRescueValidStart = @SelfRescueValidStart,
                        SelfRescueValidEnd = @SelfRescueValidEnd,
                        ProcessingStatus = @ProcessingStatus,
                        ProcessingCount = @ProcessingCount,
                        EmployeeId = @EmployeeId,
                        DeviceType = @DeviceType,
                        InspectorName = @InspectorName,
                        PositivePressure = @PositivePressure,
                        PositivePressureTime = @PositivePressureTime,
                        NegativePressure = @NegativePressure,
                        NegativePressureTime = @NegativePressureTime,
                        ExhaustPressure = @ExhaustPressure,
                        ExhaustPressureTime = @ExhaustPressureTime,
                        QuantitativeOxygen = @QuantitativeOxygen,
                        QuantitativeOxygenTime = @QuantitativeOxygenTime,
                        ManualOxygen = @ManualOxygen,
                        ManualOxygenTime = @ManualOxygenTime
                    WHERE Id = @Id";
        
                command.Parameters.AddWithValue("@Id", rescuer.Id);
                command.Parameters.AddWithValue("@SelfRescueId", rescuer.SelfRescueId);
                command.Parameters.AddWithValue("@CreateTime", rescuer.CreateTime.ToString());
                command.Parameters.AddWithValue("@CheckTime", rescuer.CheckTime.HasValue ? (object)rescuer.CheckTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@VerifyResult", rescuer.VerifyResult.HasValue ? (object)rescuer.VerifyResult.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Temp", rescuer.Temp.HasValue ? (object)rescuer.Temp.Value : DBNull.Value);
                command.Parameters.AddWithValue("@Hs", rescuer.Hs.HasValue ? (object)rescuer.Hs.Value : DBNull.Value);
                command.Parameters.AddWithValue("@SelfRescueInfo", rescuer.SelfRescueInfo);
                command.Parameters.AddWithValue("@SelfRescueUrl", rescuer.SelfRescueUrl);
                command.Parameters.AddWithValue("@SelfRescueModel", rescuer.SelfRescueModel);
                command.Parameters.AddWithValue("@SelfRescueSafeCode", rescuer.SelfRescueSafeCode);
                command.Parameters.AddWithValue("@SelfRescueName", rescuer.SelfRescueName);
                command.Parameters.AddWithValue("@SelfRescueIsValid", rescuer.SelfRescueIsValid);
                command.Parameters.AddWithValue("@SelfRescueCompany", rescuer.SelfRescueCompany);
                command.Parameters.AddWithValue("@SelfRescueValidDate", rescuer.SelfRescueValidDate);
                command.Parameters.AddWithValue("@SelfRescueValidStart", rescuer.SelfRescueValidStart.HasValue ? (object)rescuer.SelfRescueValidStart.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@SelfRescueValidEnd", rescuer.SelfRescueValidEnd.HasValue ? (object)rescuer.SelfRescueValidEnd.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@ProcessingStatus", rescuer.ProcessingStatus.HasValue ? (object)rescuer.ProcessingStatus.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ProcessingCount", rescuer.ProcessingCount.HasValue ? (object)rescuer.ProcessingCount.Value : DBNull.Value);
                command.Parameters.AddWithValue("@EmployeeId", rescuer.EmployeeId.HasValue ? (object)rescuer.EmployeeId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@DeviceType", rescuer.DeviceType);
                command.Parameters.AddWithValue("@InspectorName", string.IsNullOrEmpty(rescuer.InspectorName) ? (object)DBNull.Value : rescuer.InspectorName);
                command.Parameters.AddWithValue("@PositivePressure", rescuer.PositivePressure.HasValue ? (object)rescuer.PositivePressure.Value : DBNull.Value);
                command.Parameters.AddWithValue("@PositivePressureTime", rescuer.PositivePressureTime.HasValue ? (object)rescuer.PositivePressureTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@NegativePressure", rescuer.NegativePressure.HasValue ? (object)rescuer.NegativePressure.Value : DBNull.Value);
                command.Parameters.AddWithValue("@NegativePressureTime", rescuer.NegativePressureTime.HasValue ? (object)rescuer.NegativePressureTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@ExhaustPressure", rescuer.ExhaustPressure.HasValue ? (object)rescuer.ExhaustPressure.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ExhaustPressureTime", rescuer.ExhaustPressureTime.HasValue ? (object)rescuer.ExhaustPressureTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@QuantitativeOxygen", rescuer.QuantitativeOxygen.HasValue ? (object)rescuer.QuantitativeOxygen.Value : DBNull.Value);
                command.Parameters.AddWithValue("@QuantitativeOxygenTime", rescuer.QuantitativeOxygenTime.HasValue ? (object)rescuer.QuantitativeOxygenTime.Value.ToString() : DBNull.Value);
                command.Parameters.AddWithValue("@ManualOxygen", rescuer.ManualOxygen.HasValue ? (object)rescuer.ManualOxygen.Value : DBNull.Value);
                command.Parameters.AddWithValue("@ManualOxygenTime", rescuer.ManualOxygenTime.HasValue ? (object)rescuer.ManualOxygenTime.Value.ToString() : DBNull.Value);

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