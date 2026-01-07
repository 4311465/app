using System.ComponentModel.DataAnnotations;

namespace zjq.Models;

public class Department
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}