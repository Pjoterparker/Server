using Microsoft.EntityFrameworkCore;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Database;

namespace PjoterParker.Api.Database
{
    public interface IApiDatabaseContext : IDbContext
    {
        DbSet<Location> Location { get; set; }

        DbSet<Spot> Spot { get; set; }

        //DbSet<Reservation> Reservation { get; set; }
    }
}