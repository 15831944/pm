using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.PM.SysSetting.Domain
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Table("base_account_role")]
    public class AccountRole : RootEntity<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column("account_id", TypeName = "int")]
        public int AccountId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Column("role_id", TypeName = "int")]
        public int RoleId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status", TypeName = "bit")]
        public bool Status { get; set; }
    }
}
