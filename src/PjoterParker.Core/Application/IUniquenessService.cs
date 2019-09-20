namespace PjoterParker.Core.Application
{
    public interface IUniquenessService
    {
        void Add(string key, string value);

        bool IsUnique(string key, string value);

        void Remove(string key, string value);
    }
}