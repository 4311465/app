using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace zjq.Models
{
    public class SelfRescuer
    {
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// 出厂编号,唯一ID
        /// </summary>
        [Required]
        public string SelfRescueId { get; set; } = null!;


        /// <summary>
        /// 该设备第一次进入系统的时间
        /// </summary>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 此次的校验时间
        /// </summary>
        public DateTime? CheckTime { get; set; }


        /// <summary>
        /// 此次校验结果
        /// </summary>
        public byte? VerifyResult { get; set; }


        /// <summary>
        /// 温度
        /// </summary>
        public float? Temp { get; set; }


        /// <summary>
        /// 湿度
        /// </summary>
        public float? Hs { get; set; }


        /// <summary>
        /// 自救器url有效内容
        /// </summary>
        [Required]
        public string SelfRescueInfo { get; set; } = null!;


        /// <summary>
        /// 自救器扫码url
        /// </summary>
        [Required]
        public string SelfRescueUrl { get; set; } = null!;


        /// <summary>
        /// 自救器类型
        /// </summary>
        [Required]
        public string SelfRescueModel { get; set; } = null!;


        /// <summary>
        /// 安标
        /// </summary>
        [Required]
        public string SelfRescueSafeCode { get; set; } = null!;


        /// <summary>
        /// 自救器名称
        /// </summary>
        [Required]
        public string SelfRescueName { get; set; } = null!;


        /// <summary>
        /// 自救器官网是否有效
        /// </summary>
        [Required]
        public string SelfRescueIsValid { get; set; } = null!;


        /// <summary>
        /// 自救器生产厂家
        /// </summary>
        [Required]
        public string SelfRescueCompany { get; set; } = null!;


        /// <summary>
        /// 自救器有效时间范围
        /// </summary>
        [Required]
        public string SelfRescueValidDate { get; set; } = null!;


        /// <summary>
        /// 自救器有效期开始时间
        /// </summary>
        public DateTime? SelfRescueValidStart { get; set; }


        /// <summary>
        /// 自救器有效期结束时间
        /// </summary>
        public DateTime? SelfRescueValidEnd { get; set; }


        /// <summary>
        /// 服务器处理状态
        /// </summary>
        public byte? ProcessingStatus { get; set; }


        /// <summary>
        /// 服务器处理次数
        /// </summary>
        public int? ProcessingCount { get; set; }


        /// <summary>
        /// 员工ID（外键）
        /// </summary>
        public int? EmployeeId { get; set; }


        /// <summary>
        /// 设备类型: 0=化学氧(默认), 1=压缩氧
        /// </summary>
        public int DeviceType { get; set; } = 0;

        /// <summary>
        /// 状态ID
        /// </summary>
        public int StatusId { get; set; } = 1; // 默认状态为正常

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber { get; set; } = null!;

        /// <summary>
        /// 型号
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// 过期日期
        /// </summary>
        public DateTime ExpiryDate { get; set; }


        /// <summary>
        /// 检查人姓名（从Modbus设备自动读取）
        /// </summary>
        public string? InspectorName { get; set; }


        /// <summary>
        /// 正压气密性 (压缩氧)
        /// </summary>
        public float? PositivePressure { get; set; }


        /// <summary>
        /// 正压气密性检测时间
        /// </summary>
        public DateTime? PositivePressureTime { get; set; }


        /// <summary>
        /// 负压气密性 (压缩氧)
        /// </summary>
        public float? NegativePressure { get; set; }


        /// <summary>
        /// 负压气密性检测时间
        /// </summary>
        public DateTime? NegativePressureTime { get; set; }


        /// <summary>
        /// 排气压力 (压缩氧)
        /// </summary>
        public float? ExhaustPressure { get; set; }


        /// <summary>
        /// 排气压力检测时间
        /// </summary>
        public DateTime? ExhaustPressureTime { get; set; }


        /// <summary>
        /// 定量供氧 (压缩氧)
        /// </summary>
        public float? QuantitativeOxygen { get; set; }


        /// <summary>
        /// 定量供氧检测时间
        /// </summary>
        public DateTime? QuantitativeOxygenTime { get; set; }


        /// <summary>
        /// 手动补给 (压缩氧)
        /// </summary>
        public float? ManualOxygen { get; set; }


        /// <summary>
        /// 手动补给检测时间
        /// </summary>
        public DateTime? ManualOxygenTime { get; set; }


        /// <summary>
        /// 关联的员工
        /// </summary>
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
    }
}