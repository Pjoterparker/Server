using System;

namespace PjoterParker.Core.Application
{
    public interface IUniquenessService
    {
        bool IsUnique(Guid aggrageteId, string key, string value);

        bool Remove(Guid aggrageteId, string key, string value);
    }
}
