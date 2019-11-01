using System;
using PjoterParker.Common.Extensions;

namespace PjoterParker.Common.Helpers
{
    public static class CompareHelper
    {
        public static bool ClassEquals<T>(T left, T right) where T : class, IEquatable<T>
        {
            if (left.IsNull() && right.IsNull())
            {
                return true;
            }

            if (left.IsNull() && right.IsNotNull())
            {
                return false;
            }

            if (left.IsNotNull() && right.IsNull())
            {
                return false;
            }

            return left.Equals(right);
        }

        public static bool StructEquals<T>(T? left, T? right) where T : struct
        {
            if (!left.HasValue && !right.HasValue)
            {
                return true;
            }

            if (!left.HasValue && right.HasValue)
            {
                return false;
            }

            if (left.HasValue && !right.HasValue)
            {
                return false;
            }

            return left.Equals(right);
        }
    }
}
