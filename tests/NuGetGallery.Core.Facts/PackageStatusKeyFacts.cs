// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Xunit;

namespace NuGetGallery
{
    public class PackageStatusKeyFacts
    {
        public void AssertAvailableNotChanged()
        {
            Assert.Null(PackageStatusKey.Available);
        }

        public void AssertDeletedNotChanged()
        {
            Assert.Equal(1, PackageStatusKey.Deleted);
        }

        [Theory]
        [MemberData(nameof(PackageStatusKeyIsAvailable))]
        public void IsAvailable(int? packageStatusKey, bool isAvailable)
        {
            // Arrange
            var package = new Package { PackageStatusKey = packageStatusKey };
            var compiled = PackageStatusKey.IsAvailable().Compile();

            // Act
            var actual = compiled(package);

            // Assert
            Assert.Equal(isAvailable, actual);
        }

        [Theory]
        [MemberData(nameof(PackageStatusKeyIsDeleted))]
        public void IsNotDeleted(int? packageStatusKey, bool isDeleted)
        {
            // Arrange
            var package = new Package { PackageStatusKey = packageStatusKey };
            var compiled = PackageStatusKey.IsNotDeleted().Compile();

            // Act
            var actual = compiled(package);

            // Assert
            Assert.Equal(!isDeleted, actual);
        }

        [Theory]
        [MemberData(nameof(PackageStatusKeyIsDeleted))]
        public void IsDeleted(int? packageStatusKey, bool isDeleted)
        {
            // Arrange
            var package = new Package { PackageStatusKey = packageStatusKey };

            // Act
            var actual = package.IsDeleted();

            // Assert
            Assert.Equal(isDeleted, actual);
        }

        public static IEnumerable<object[]> PackageStatusKeyIsDeleted => new[]
        {
            new object[] { null, false },
            new object[] { -1, false },
            new object[] { 0, false },
            new object[] { 1, true },
            new object[] { 2, false },
        };

        public static IEnumerable<object[]> PackageStatusKeyIsAvailable => new[]
        {
            new object[] { null, true },
            new object[] { -1, false },
            new object[] { 0, false },
            new object[] { 1, false },
            new object[] { 2, false },
        };
    }
}
