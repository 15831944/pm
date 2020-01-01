using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.PM.SysSetting.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("base_account")]
    public class Account: RootEntity<int>
    {
        /// <summary>
        /// 工号
        /// </summary>
        [Column("account_no", TypeName = "varchar(20)")]
        public string AccountNo { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column("password", TypeName = "varchar(50)")]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column("account_name", TypeName = "varchar(50)")]
        public string AccountName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column("email", TypeName = "varchar(200)")]
        public string Email { get; set; }

        /// <summary>
        /// 状态：
        ///   1:正常 2:已离职 3:已删除
        /// </summary>
        [Column("status", TypeName = "tinyint")]
        public short Status { get; set; }
    }
}
