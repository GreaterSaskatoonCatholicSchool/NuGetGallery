// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;

namespace NuGetGallery
{
    public static class PackageStatusKey
    {
        /// <summary>
        /// The package is available and visible on the feed.
        /// </summary>
        public static readonly int? Available = null;

        /// <summary>
        /// The package has been soft deleted. This means that the package is not available but the package ID and
        /// version are still reserved.
        /// </summary>
        public const int Deleted = 1;

        private static Lazy<Expression<Func<Package, bool>>> _isAvailable = new Lazy<Expression<Func<Package, bool>>>(() =>
        {
            /// Since <see cref="Available"/> is not a constant, we must explicitly construct the expression to force
            /// that value to be considered a constant. If <see cref="Available"/> is not considered a constant, the
            /// expression that is converted to SQL by Entity Framework is less efficient.
            var parameter = Expression.Parameter(typeof(Package), "p");
            var property = Expression.Property(parameter, nameof(Package.PackageStatusKey));
            return Expression.Lambda<Func<Package, bool>>(
                Expression.Equal(
                    property,
                    Expression.Constant(Available)),
                parameter);
        });

        public static Expression<Func<Package, bool>> IsNotDeleted()
        {
            return p => p.PackageStatusKey != Deleted;
        }

        public static Expression<Func<Package, bool>> IsAvailable()
        {
            return _isAvailable.Value;
        }

        public static bool IsDeleted(this Package package)
        {
            return package.PackageStatusKey == Deleted;
        }
    }
}
