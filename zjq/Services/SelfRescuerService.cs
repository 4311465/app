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
                    rescuer.StatusId = 1; // 正常状态
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
                    rescuer.StatusId = 1; // 正常状态
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
                    rescuer.StatusId = 5; // 在使用中状态
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
                    rescuer.StatusId = 5; // 在使用中状态
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
                        rescuer.StatusId = condition == "正常" ? 1 : 2; // 正常或需要维护
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
                        rescuer.StatusId = condition == "正常" ? 1 : 2; // 正常或需要维护
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

        // 检查过期的自救器
        public List<SelfRescuer> GetExpiredSelfRescuers()
        {
            try
            {
                var allRescuers = _selfRescuerRepo.GetAll();
                var today = DateTime.Now;
                
                // 更新过期状态
                foreach (var rescuer in allRescuers)
                {
                    if (rescuer.ExpiryDate < today && rescuer.StatusId != 3)
                    {
                        rescuer.StatusId = 3; // 过期状态
                        _selfRescuerRepo.Update(rescuer);
                    }
                }
                
                return allRescuers.Where(r => r.ExpiryDate < today).ToList();
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
                
                // 更新过期状态
                foreach (var rescuer in allRescuers)
                {
                    if (rescuer.ExpiryDate < today && rescuer.StatusId != 3)
                    {
                        rescuer.StatusId = 3; // 过期状态
                        await _selfRescuerRepo.UpdateAsync(rescuer);
                    }
                }
                
                return allRescuers.Where(r => r.ExpiryDate < today).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting expired self rescuers: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }

        // 获取需要维护的自救器
        public List<SelfRescuer> GetSelfRescuersNeedingMaintenance()
        {
            try
            {
                var allRescuers = _selfRescuerRepo.GetAll();
                return allRescuers.Where(r => r.StatusId == 2).ToList();
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
                return allRescuers.Where(r => r.StatusId == 2).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting self rescuers needing maintenance: {ex.Message}");
                return new List<SelfRescuer>();
            }
        }
    }
}