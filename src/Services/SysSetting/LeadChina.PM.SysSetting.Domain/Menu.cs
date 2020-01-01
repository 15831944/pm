using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.PM.SysSetting.Domain
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Table("base_menu")]
    public class Menu : RootEntity<int>
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Column("menu_name", TypeName = "varchar(50)")]
        public string MenuName { get; set; }

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
