using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.PM.SysSetting.Domain
{
    /// <summary>
    /// 组织
    /// </summary>
    [Table("base_org")]
    public class Org : RootEntity<int>
    {
        /// <summary>
        /// 组织名
        /// </summary>
        [Column("org_name", TypeName = "varchar(50)")]
        public string OrgName { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Column("level", TypeName = "tinyint")]
        public short Level { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [Column("parentId", TypeName = "int")]
        public int ParentId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status", TypeName = "bit")]
        public bool Status { get; set; }
    }
}
