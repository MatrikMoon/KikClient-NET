using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

/**
 * Created by Moon on 5/18/2019
 * The base database context for the EF database
 */

namespace KikClient.Database
{
    public class DatabaseContext : DbContext
    {
        private string location;

        public DatabaseContext(string location) : base()
        {
            this.location = location;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(new SqliteConnection()
            {
                ConnectionString = new SqliteConnectionStringBuilder() { DataSource = location }.ConnectionString
            });
        }

        public DbSet<Alias> Aliases { get; set; }
        public DbSet<GroupActivity> GroupActivity { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
    }
}
