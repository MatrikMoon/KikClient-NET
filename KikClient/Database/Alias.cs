using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * Created by Moon on 1/13/2021
 * The Model class for an alias database entry
 */

namespace KikClient.Database
{
    [Table("Aliases")]
    public class Alias
    {
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }

        [Column("AliasJid")]
        public string AliasJid { get; set; }

        [Column("ResolvedJid")]
        public string ResolvedJid { get; set; }

        [Column("GroupJid")]
        public string GroupJid { get; set; }

        [Column("DisplayName")]
        public string DisplayName { get; set; }

        [Column("ProfilePic")]
        public string ProfilePic { get; set; }

        [Column("ProfilePicLastUpdated")]
        public long ProfilePicLastUpdated { get; set; }

        [Column("Old")]
        public bool Old { get; set; }
    }
}
