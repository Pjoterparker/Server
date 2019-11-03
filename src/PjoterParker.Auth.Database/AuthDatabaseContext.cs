using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PjoterParker.Entities;

namespace PjoterParker.Auth.Database
{
    public class AuthDatabaseContext : IdentityDbContext<User, Role, long>
    {
        private readonly AuthDatabaseCredentials _credentials;

        public AuthDatabaseContext(AuthDatabaseCredentials credentials)
        {
            _credentials = credentials;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_credentials.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
