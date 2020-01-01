using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.PM.SysSetting.Domain
{
    /// <summary>
    /// 用户组织
    /// </summary>
    [Table("base_account_org")]
    public class AccountOrg : RootEntity<int>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column("account_id", TypeName = "int")]
        public int AccountId { get; set; }

        /// <summary>
        /// 组织Id
        /// </summary>
        [Column("org_id", TypeName = "int")]
        public int OrgId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status", TypeName = "bit")]
        public bool Status { get; set; }
    }
}
