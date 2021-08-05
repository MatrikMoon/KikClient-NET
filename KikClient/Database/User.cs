using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * Created by Moon on 1/13/2020
 * The Model class for a user database entry
 */

namespace KikClient.Database
{
    [Table("Users")]
    public class User
    {
        [Column("ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ID { get; set; }

        [Column("UserJid")]
        public string UserJid { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("DisplayName")]
        public string DisplayName { get; set; }

        [Column("ProfilePic")]
        public string ProfilePic { get; set; }

        [Column("ProfilePicLastUpdated")]
        public long ProfilePicLastUpdated { get; set; }

        [Column("AccountCreationDate")]
        public long AccountCreationDate { get; set; }

        [Column("Old")]
        public bool Old { get; set; }
    }
}
