using Microsoft.EntityFrameworkCore;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Services;

namespace PjoterParker.Database
{
    public interface IApiDatabaseContext : IDbContext
    {
        DbSet<Location> Location { get; set; }
    }
}