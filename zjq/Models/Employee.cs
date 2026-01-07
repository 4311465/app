using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace zjq.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string EmployeeNumber { get; set; } = null!;

    [MaxLength(100)]
    public string? Email { get; set; }

    // 自救器编号
    [MaxLength(50)]
    public string? SelfRescueId { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public DateTime HireDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 外键
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    // 导航属性
    public Department? Department { get; set; }
    public Position? Position { get; set; }



    // 关联的检查信息
    public ICollection<SelfRescuer> SelfRescuers { get; set; } = new List<SelfRescuer>();
}