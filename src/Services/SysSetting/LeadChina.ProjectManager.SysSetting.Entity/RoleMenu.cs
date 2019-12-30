using LeadChina.ProjectManager.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.ProjectManager.SysSetting.Entity
{
    /// <summary>
    /// 角色菜单
    /// </summary>
    [Table("base_role_menu")]
    public class RoleMenu : RootEntity<int>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Column("role_id", TypeName = "int")]
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [Column("menu_id", TypeName = "int")]
        public int MenuId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status", TypeName = "bit")]
        public bool Status { get; set; }
    }
}
