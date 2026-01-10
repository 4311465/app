using zjq.Models;
using zjq.Repositories;

namespace zjq.Services
{
    public class SelfRescuerService
    {
        private readonly SelfRescuerRepository _selfRescuerRepo;
        private readonly MaintenanceRecordRepository _maintenanceRepo;
        private readonly UsageRecordRepository _usageRepo;
        private readonly StatusTypeRepository _statusRepo;

        public SelfRescuerService(
            SelfRescuerRepository selfRescuerRepo,
            MaintenanceRecordRepository maintenanceRepo,
            UsageRecordRepository usageRepo,
            StatusTypeRepository statusRepo)
        {
            _selfRescuerRepo = selfRescuerRepo;
            _maintenanceRepo = maintenanceRepo;
            _usageRepo = usageRepo;
            _statusRepo = statusRepo;
        }

        // 获取所有自救器
        public List<SelfRescuer> GetAllSelfRescuers()
        {
            try
            {
                return _selfRescuerRepo.GetAll();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting all self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 获取所有自救器（异步）
        public async Task<List<SelfRescuer>> GetAllSelfRescuersAsync()
        {
            try
            {
                return await _selfRescuerRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting all self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 根据ID获取自救器
        public SelfRescuer GetSelfRescuerById(int id)
        {
            try
            {
                return _selfRescuerRepo.GetById(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuer by id: {ex.Message}");
                return null;
            }
        }

        // 根据ID获取自救器（异步）
        public async Task<SelfRescuer> GetSelfRescuerByIdAsync(int id)
        {
            try
            {
                return await _selfRescuerRepo.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuer by id: {ex.Message}");
                return null;
            }
        }

        // 添加自救器
        public int AddSelfRescuer(SelfRescuer rescuer)
        {
            try
            {
                return _selfRescuerRepo.Add(rescuer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding self rescuer: {ex.Message}");
                return -1;
            }
        }

        // 添加自救器（异步）
        public async Task<int> AddSelfRescuerAsync(SelfRescuer rescuer)
        {
            try
            {
                return await _selfRescuerRepo.AddAsync(rescuer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding self rescuer: {ex.Message}");
                return -1;
            }
        }

        // 更新自救器
        public void UpdateSelfRescuer(SelfRescuer rescuer)
        {
            try
            {
                _selfRescuerRepo.Update(rescuer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating self rescuer: {ex.Message}");
            }
        }

        // 更新自救器（异步）
        public async Task UpdateSelfRescuerAsync(SelfRescuer rescuer)
        {
            try
            {
                await _selfRescuerRepo.UpdateAsync(rescuer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating self rescuer: {ex.Message}");
            }
        }

        // 删除自救器
        public void DeleteSelfRescuer(int id)
        {
            try
            {
                _selfRescuerRepo.Delete(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting self rescuer: {ex.Message}");
            }
        }

        // 删除自救器（异步）
        public async Task DeleteSelfRescuerAsync(int id)
        {
            try
            {
                await _selfRescuerRepo.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting self rescuer: {ex.Message}");
            }
        }

        // 获取自救器的维护记录
        public List<MaintenanceRecord> GetMaintenanceRecords(int selfRescuerId)
        {
            try
            {
                return _maintenanceRepo.GetBySelfRescuerId(selfRescuerId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting maintenance records: {ex.Message}");
                return new List<MaintenanceRecord>();
            }
        }

        // 获取自救器的维护记录（异步）
        public async Task<List<MaintenanceRecord>> GetMaintenanceRecordsAsync(int selfRescuerId)
        {
            try
            {
                return await _maintenanceRepo.GetBySelfRescuerIdAsync(selfRescuerId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting maintenance records: {ex.Message}");
                return new List<MaintenanceRecord>();
            }
        }

        // 添加维护记录
        public int AddMaintenanceRecord(MaintenanceRecord record)
        {
            try
            {
                var result = _maintenanceRepo.Add(record);
                
                // 如果维护成功，更新自救器状态为正常
                var rescuer = _selfRescuerRepo.GetById(record.SelfRescuerId);
                if (rescuer != null)
                {
                    // 注意：SelfRescuer 模型中没有 StatusId 属性
                    // 这里可以根据需要添加其他逻辑，比如更新检查时间等
                    _selfRescuerRepo.Update(rescuer);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding maintenance record: {ex.Message}");
                return -1;
            }
        }

        // 添加维护记录（异步）
        public async Task<int> AddMaintenanceRecordAsync(MaintenanceRecord record)
        {
            try
            {
                var result = await _maintenanceRepo.AddAsync(record);
                
                // 如果维护成功，更新自救器状态为正常
                var rescuer = await _selfRescuerRepo.GetByIdAsync(record.SelfRescuerId);
                if (rescuer != null)
                {
                    // 注意：SelfRescuer 模型中没有 StatusId 属性
                    // 这里可以根据需要添加其他逻辑，比如更新检查时间等
                    await _selfRescuerRepo.UpdateAsync(rescuer);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding maintenance record: {ex.Message}");
                return -1;
            }
        }

        // 获取自救器的使用记录
        public List<UsageRecord> GetUsageRecords(int selfRescuerId)
        {
            try
            {
                return _usageRepo.GetBySelfRescuerId(selfRescuerId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting usage records: {ex.Message}");
                return new List<UsageRecord>();
            }
        }

        // 获取自救器的使用记录（异步）
        public async Task<List<UsageRecord>> GetUsageRecordsAsync(int selfRescuerId)
        {
            try
            {
                return await _usageRepo.GetBySelfRescuerIdAsync(selfRescuerId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting usage records: {ex.Message}");
                return new List<UsageRecord>();
            }
        }

        // 借出自救器
        public int BorrowSelfRescuer(UsageRecord record)
        {
            try
            {
                var result = _usageRepo.Add(record);
                
                // 更新自救器状态为在使用中
                var rescuer = _selfRescuerRepo.GetById(record.SelfRescuerId);
                if (rescuer != null)
                {
                    // 注意：SelfRescuer 模型中没有 StatusId 属性
                    // 这里可以根据需要添加其他逻辑，比如更新使用状态等
                    _selfRescuerRepo.Update(rescuer);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error borrowing self rescuer: {ex.Message}");
                return -1;
            }
        }

        // 借出自救器（异步）
        public async Task<int> BorrowSelfRescuerAsync(UsageRecord record)
        {
            try
            {
                var result = await _usageRepo.AddAsync(record);
                
                // 更新自救器状态为在使用中
                var rescuer = await _selfRescuerRepo.GetByIdAsync(record.SelfRescuerId);
                if (rescuer != null)
                {
                    // 注意：SelfRescuer 模型中没有 StatusId 属性
                    // 这里可以根据需要添加其他逻辑，比如更新使用状态等
                    await _selfRescuerRepo.UpdateAsync(rescuer);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error borrowing self rescuer: {ex.Message}");
                return -1;
            }
        }

        // 归还自救器
        public void ReturnSelfRescuer(int usageRecordId, DateTime returnDate, string condition)
        {
            try
            {
                var records = _usageRepo.GetAll();
                var record = records.FirstOrDefault(r => r.Id == usageRecordId);
                
                if (record != null)
                {
                    record.ReturnDate = returnDate;
                    record.Condition = condition;
                    _usageRepo.Update(record);
                    
                    // 更新自救器状态
                    var rescuer = _selfRescuerRepo.GetById(record.SelfRescuerId);
                    if (rescuer != null)
                    {
                        // 根据归还状态设置自救器状态
                        // 注意：SelfRescuer 模型中没有 StatusId 属性
                        // 这里可以根据需要添加其他逻辑，比如更新检查时间等
                        _selfRescuerRepo.Update(rescuer);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error returning self rescuer: {ex.Message}");
            }
        }

        // 归还自救器（异步）
        public async Task ReturnSelfRescuerAsync(int usageRecordId, DateTime returnDate, string condition)
        {
            try
            {
                var records = await _usageRepo.GetAllAsync();
                var record = records.FirstOrDefault(r => r.Id == usageRecordId);
                
                if (record != null)
                {
                    record.ReturnDate = returnDate;
                    record.Condition = condition;
                    await _usageRepo.UpdateAsync(record);
                    
                    // 更新自救器状态
                    var rescuer = await _selfRescuerRepo.GetByIdAsync(record.SelfRescuerId);
                    if (rescuer != null)
                    {
                        // 根据归还状态设置自救器状态
                        // 注意：SelfRescuer 模型中没有 StatusId 属性
                        // 这里可以根据需要添加其他逻辑，比如更新检查时间等
                        await _selfRescuerRepo.UpdateAsync(rescuer);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error returning self rescuer: {ex.Message}");
            }
        }

        // 获取所有状态类型
        public List<StatusType> GetAllStatusTypes()
        {
            try
            {
                return _statusRepo.GetAll();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting all status types: {ex.Message}");
                return new List<StatusType>();
            }
        }

        // 获取所有状态类型（异步）
        public async Task<List<StatusType>> GetAllStatusTypesAsync()
        {
            try
            {
                return await _statusRepo.GetAllAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting all status types: {ex.Message}");
                return new List<StatusType>();
            }
        }

        // 获取状态名称
        public string GetStatusName(int statusId)
        {
            try
            {
                var status = _statusRepo.GetById(statusId);
                return status?.Name ?? "未知";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting status name: {ex.Message}");
                return "未知";
            }
        }

        // 获取状态名称（异步）
        public async Task<string> GetStatusNameAsync(int statusId)
        {
            try
            {
                var status = await _statusRepo.GetByIdAsync(statusId);
                return status?.Name ?? "未知";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting status name: {ex.Message}");
                return "未知";
            }
        }

        // 根据URL获取自救器
        public async Task<List<SelfRescuer>> GetSelfRescuersByUrlAsync(string url)
        {
            try
            {
                return await _selfRescuerRepo.GetByUrlAsync(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuers by URL: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 检查过期的自救器
        public List<SelfRescuer> GetExpiredSelfRescuers()
        {
            try
            {
                var allRescuers = _selfRescuerRepo.GetAll();
                var today = DateTime.Now;
                var expiredRescuers = new List<SelfRescuer>();
                
                // 检查过期状态
                foreach (var rescuer in allRescuers)
                {
                    bool isExpired = false;
                    
                    // 优先使用 SelfRescueValidEnd 如果存在
                    if (rescuer.SelfRescueValidEnd.HasValue)
                    {
                        isExpired = rescuer.SelfRescueValidEnd.Value < today;
                    }
                    // 否则尝试从 SelfRescueId 解析出厂日期
                    else if (!string.IsNullOrWhiteSpace(rescuer.SelfRescueId) && rescuer.SelfRescueId.Length >= 8)
                    {
                        try
                        {
                            // 出厂编号的前8位数字表示日期，格式为YYYYMMDD
                            string dateStr = rescuer.SelfRescueId.Substring(0, 8);
                            
                            // 检查是否都是数字
                            if (System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                            {
                                // 解析日期
                                if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                {
                                    // 判断是否过期（3年内有效）
                                    isExpired = parsedDate.AddYears(3) < today;
                                }
                            }
                        }
                        catch
                        {
                            // 解析失败，视为未过期
                        }
                    }
                    
                    if (isExpired)
                    {
                        expiredRescuers.Add(rescuer);
                    }
                }
                
                return expiredRescuers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting expired self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 检查过期的自救器（异步）
        public async Task<List<SelfRescuer>> GetExpiredSelfRescuersAsync()
        {
            try
            {
                var allRescuers = await _selfRescuerRepo.GetAllAsync();
                var today = DateTime.Now;
                var expiredRescuers = new List<SelfRescuer>();
                
                // 检查过期状态
                foreach (var rescuer in allRescuers)
                {
                    bool isExpired = false;
                    
                    // 优先使用 SelfRescueValidEnd 如果存在
                    if (rescuer.SelfRescueValidEnd.HasValue)
                    {
                        isExpired = rescuer.SelfRescueValidEnd.Value < today;
                    }
                    // 否则尝试从 SelfRescueId 解析出厂日期
                    if (!string.IsNullOrWhiteSpace(rescuer.SelfRescueId) && rescuer.SelfRescueId.Length >= 8)
                    {
                        try
                        {
                            // 出厂编号的前8位数字表示日期，格式为YYYYMMDD
                            string dateStr = rescuer.SelfRescueId.Substring(0, 8);
                            
                            // 检查是否都是数字
                            if (System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                            {
                                // 解析日期
                                if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                {
                                    // 判断是否过期（3年内有效）
                                    isExpired = parsedDate.AddYears(3) < today;
                                }
                            }
                        }
                        catch
                        {
                            // 解析失败，视为未过期
                        }
                    }
                    
                    if (isExpired)
                    {
                        expiredRescuers.Add(rescuer);
                    }
                }
                
                return expiredRescuers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting expired self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 获取临期的自救器
        public List<SelfRescuer> GetExpiringSelfRescuers()
        {
            try
            {
                var allRescuers = _selfRescuerRepo.GetAll();
                var currentDate = DateTime.Now;
                
                // 先获取所有符合临期条件的记录
                var expiringCandidates = allRescuers.Where(c => 
                    c.CheckTime.HasValue && 
                    ( 
                        // 压缩氧：160天到180天
                        (c.DeviceType == 1 && 
                         c.CheckTime.Value.AddDays(160) <= currentDate && 
                         c.CheckTime.Value.AddDays(180) >= currentDate)
                        ||
                        // 化学氧：80天到90天
                        (c.DeviceType == 0 && 
                         c.CheckTime.Value.AddDays(80) <= currentDate && 
                         c.CheckTime.Value.AddDays(90) >= currentDate)
                    ) 
                ).ToList();
                
                // 在内存中过滤未过期的自救器
                var validExpiringSelfRescuers = expiringCandidates.Where(c => 
                {
                    // 尝试从SelfRescueId解析出厂日期
                    if (string.IsNullOrWhiteSpace(c.SelfRescueId) || c.SelfRescueId.Length < 8)
                        return true;
                    
                    try
                    {
                        // 出厂编号的前8位数字表示日期，格式为YYYYMMDD
                        string dateStr = c.SelfRescueId.Substring(0, 8);
                        
                        // 检查是否都是数字
                        if (!System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                            return true; // 格式不正确，视为未过期
                        
                        // 解析日期
                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                        {
                            // 判断是否未过期（3年内有效）
                            return parsedDate.AddYears(3) >= currentDate;
                        }
                        return true; // 解析失败，视为未过期
                    }
                    catch
                    {
                        return true; // 任何异常都视为未过期
                    }
                }).ToList();
                
                return validExpiringSelfRescuers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting expiring self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 获取临期的自救器（异步）
        public async Task<List<SelfRescuer>> GetExpiringSelfRescuersAsync()
        {
            try
            {
                var allRescuers = await _selfRescuerRepo.GetAllAsync();
                var currentDate = DateTime.Now;
                
                // 先获取所有符合临期条件的记录
                var expiringCandidates = allRescuers.Where(c => 
                    c.CheckTime.HasValue && 
                    ( 
                        // 压缩氧：160天到180天
                        (c.DeviceType == 1 && 
                         c.CheckTime.Value.AddDays(160) <= currentDate && 
                         c.CheckTime.Value.AddDays(180) >= currentDate)
                        ||
                        // 化学氧：80天到90天
                        (c.DeviceType == 0 && 
                         c.CheckTime.Value.AddDays(80) <= currentDate && 
                         c.CheckTime.Value.AddDays(90) >= currentDate)
                    ) 
                ).ToList();
                
                // 在内存中过滤未过期的自救器
                var validExpiringSelfRescuers = expiringCandidates.Where(c => 
                {
                    // 尝试从SelfRescueId解析出厂日期
                    if (string.IsNullOrWhiteSpace(c.SelfRescueId) || c.SelfRescueId.Length < 8)
                        return true;
                    
                    try
                    {
                        // 出厂编号的前8位数字表示日期，格式为YYYYMMDD
                        string dateStr = c.SelfRescueId.Substring(0, 8);
                        
                        // 检查是否都是数字
                        if (!System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                            return true; // 格式不正确，视为未过期
                        
                        // 解析日期
                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                        {
                            // 判断是否未过期（3年内有效）
                            return parsedDate.AddYears(3) >= currentDate;
                        }
                        return true; // 解析失败，视为未过期
                    }
                    catch
                    {
                        return true; // 任何异常都视为未过期
                    }
                }).ToList();
                
                return validExpiringSelfRescuers;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting expiring self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 获取需要维护的自救器
        public List<SelfRescuer> GetSelfRescuersNeedingMaintenance()
        {
            try
            {
                var allRescuers = _selfRescuerRepo.GetAll();
                // 注意：SelfRescuer 模型中没有 StatusId 属性
                // 这里可以根据需要添加其他逻辑来判断哪些自救器需要维护
                // 例如，根据检查时间、校验结果等
                return new List<SelfRescuer>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuers needing maintenance: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 获取需要维护的自救器（异步）
        public async Task<List<SelfRescuer>> GetSelfRescuersNeedingMaintenanceAsync()
        {
            try
            {
                var allRescuers = await _selfRescuerRepo.GetAllAsync();
                // 注意：SelfRescuer 模型中没有 StatusId 属性
                // 这里可以根据需要添加其他逻辑来判断哪些自救器需要维护
                // 例如，根据检查时间、校验结果等
                return new List<SelfRescuer>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuers needing maintenance: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 根据状态获取自救器
        public async Task<List<SelfRescuer>> GetSelfRescuersByStatusAsync(string status)
        {
            try
            {
                var allRescuers = await _selfRescuerRepo.GetAllAsync();
                var query = allRescuers.AsQueryable();
                var currentDate = DateTime.Now;

                switch (status)
                {
                    case "expiring": // 临期自救器
                        // 先获取所有符合临期条件的记录
                        var expiringCandidates = await Task.FromResult(query.Where(c => 
                            c.CheckTime.HasValue && 
                            ( 
                                // 压缩氧：160天到180天
                                (c.DeviceType == 1 && 
                                 c.CheckTime.Value.AddDays(160) <= currentDate && 
                                 c.CheckTime.Value.AddDays(180) >= currentDate) 
                                || 
                                // 化学氧：80天到90天
                                (c.DeviceType == 0 && 
                                 c.CheckTime.Value.AddDays(80) <= currentDate && 
                                 c.CheckTime.Value.AddDays(90) >= currentDate) 
                            ) 
                        ).ToList());
                        
                        // 在内存中过滤未过期的自救器
                        var validExpiringSelfRescuers = expiringCandidates.Where(c => 
                        { 
                            // 尝试从SelfRescueId解析出厂日期
                            if (string.IsNullOrWhiteSpace(c.SelfRescueId) || c.SelfRescueId.Length < 8) 
                                return true; 
                            
                            try 
                            { 
                                // 出厂编号的前8位数字表示日期，格式为YYYYMMDD
                                string dateStr = c.SelfRescueId.Substring(0, 8); 
                                
                                // 检查是否都是数字
                                if (!System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$")) 
                                    return true; // 格式不正确，视为未过期
                                
                                // 解析日期
                                if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate)) 
                                { 
                                    // 判断是否未过期（3年内有效）
                                    return parsedDate.AddYears(3) >= currentDate; 
                                } 
                                return true; // 解析失败，视为未过期
                            } 
                            catch 
                            { 
                                return true; // 任何异常都视为未过期
                            } 
                        }).ToList();
                        
                        // 重新构造查询
                        query = validExpiringSelfRescuers.AsQueryable();
                        break;
                    default:
                        // 默认返回所有自救器
                        break;
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuers by status: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }
    }
}