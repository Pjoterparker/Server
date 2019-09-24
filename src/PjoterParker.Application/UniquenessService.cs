using System;
using System.Linq;
using PjoterParker.Api.Database;
using PjoterParker.Api.Database.Entities;
using PjoterParker.Core.Application;
using PjoterParker.Core.Extensions;

namespace PjoterParker.Application
{
    public class UniquenessService : IUniquenessService
    {
        private readonly IUniquenessContext _context;

        public UniquenessService(IUniquenessContext context)
        {
            _context = context;
        }

        public bool IsUnique(Guid aggrageteId, string key, string value)
        {
            var result = _context.UniquenessTable.FirstOrDefault(u => u.Key == key && u.Value == value);
            if (result.IsNull())
            {
                _context.UniquenessTable.Add(new UniquenessTable(key, value, aggrageteId));
                _context.SaveChanges();
                return true;
            }

            return result.AggrageteId == aggrageteId;
        }

        public bool Remove(Guid aggrageteId, string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}