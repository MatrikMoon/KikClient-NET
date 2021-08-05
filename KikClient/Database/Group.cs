using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * Created by Moon on 1/24/2020
 * The Model class for a user database entry
 */

namespace KikClient.Database
{
    [Table("Groups")]
    public class Group
    {
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }

        [Column("GroupJid")]
        public string GroupJid { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Code")]
        public string Code { get; set; }

        [Column("Pic")]
        public string Pic { get; set; }

        [Column("PicTimestamp")]
        public long PicTimestamp { get; set; }

        [Column("Old")]
        public bool Old { get; set; }
    }
}
