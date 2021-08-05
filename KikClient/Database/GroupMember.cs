using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * Created by Moon on 1/13/2021
 * The Model class for an alias database entry
 */

namespace KikClient.Database
{
    [Flags]
    public enum Role
    {
        None = 0,
        Admin = 1,
        Owner = 2
    }

    [Table("GroupMembers")]
    public class GroupMember
    {
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }

        [Column("Role")]
        public Role Role { get; set; }

        [Column("UserJid")]
        public string UserJid { get; set; }

        [Column("GroupJid")]
        public string GroupJid { get; set; }

        [Column("DisabledDms")]
        public bool DisabledDms { get; set; }

        [Column("Old")]
        public bool Old { get; set; }
    }
}
