using Microsoft.EntityFrameworkCore;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Services;

namespace PjoterParker.Api.Database
{
    public interface IUniquenessContext : IDbContext
    {
        DbSet<UniquenessTable> UniquenessTable { get; set; }
    }
}
