using System.Linq;
using PjoterParker.Api.Database;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Application;

namespace PjoterParker.Application
{
    public class UniquenessService : IUniquenessService
    {
        private readonly IUniquenessContext _context;

        public UniquenessService(IUniquenessContext context)
        {
            _context = context;
        }

        public void Add(string key, string value)
        {
            _context.UniquenessTable.Add(new UniquenessTable(key, value));
            _context.SaveChanges();
        }

        public bool IsUnique(string key, string value)
        {
            return _context.UniquenessTable.Any(u => u.Key == key && u.Value == value);
        }

        public void Remove(string key, string value)
        {
        }
    }
}