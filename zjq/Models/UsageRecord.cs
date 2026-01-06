namespace zjq.Models
{
    public class UsageRecord
    {
        public int Id { get; set; }
        public int SelfRescuerId { get; set; }
        public DateTime UsageDate { get; set; }
        public string UserName { get; set; }
        public string Purpose { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Condition { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}