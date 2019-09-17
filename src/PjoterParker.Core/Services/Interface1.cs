using System;
using System.Collections.Generic;
using System.Text;

namespace PjoterParker.Core.Services
{
    public interface IDbContext
    {
        void BeginTransaction();

        void RollbackTransaction();

        void SaveChanges();
    }
}
