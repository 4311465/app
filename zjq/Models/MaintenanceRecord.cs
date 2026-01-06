namespace zjq.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public int SelfRescuerId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; }
        public string Technician { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}