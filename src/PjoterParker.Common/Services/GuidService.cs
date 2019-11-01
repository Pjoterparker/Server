using System;
using PjoterParker.Core.Services;

namespace PjoterParker.Common.Services
{
    public class GuidService : IGuidService
    {
        public Guid New()
        {
            return Guid.NewGuid();
        }
    }
}