using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.PM.SysSetting.Domain
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("base_role")]
    public class Role : RootEntity<int>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Column("role_name", TypeName = "varchar(50)")]
        public string RoleName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status", TypeName = "bit")]
        public bool Status { get; set; }
    }
}
