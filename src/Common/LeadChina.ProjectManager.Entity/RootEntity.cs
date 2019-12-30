using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeadChina.ProjectManager.Entity
{
    /// <summary>
    /// 定义领域实体基类
    /// </summary>
    public abstract class RootEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        /// <summary>
        /// 唯一标识
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_date", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("update_date", TypeName = "datetime")]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 重写方法 相等运算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as RootEntity<T>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }
        /// <summary>
        /// 重写方法 实体比较 ==
        /// </summary>
        /// <param name="a">领域实体a</param>
        /// <param name="b">领域实体b</param>
        /// <returns></returns>
        public static bool operator ==(RootEntity<T> a, RootEntity<T> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        /// <summary>
        /// 重写方法 实体比较 !=
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(RootEntity<T> a, RootEntity<T> b)
        {
            return !(a == b);
        }
        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
        /// <summary>
        /// 输出领域对象的状态
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
