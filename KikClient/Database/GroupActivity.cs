using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * Created by Moon on 1/13/2020
 * The Model class for a user database entry
 */

namespace KikClient.Database
{
    [Table("GroupActivity")]
    public class GroupActivity
    {
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }

        [Column("UserJid")]
        public string UserJid { get; set; }

        [Column("GroupJid")]
        public string GroupJid { get; set; }

        [Column("LastTalkDate")]
        public long LastTalkDate { get; set; }

        [Column("LastReadDate")]
        public long LastReadDate { get; set; }

        [Column("Old")]
        public bool Old { get; set; }
    }
}
