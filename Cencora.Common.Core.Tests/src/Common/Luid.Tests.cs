// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.Common.Core.Tests
{
    public class LuidTests
    {
        [Fact]
        public void Constructor_WithString_GeneratesUniqueLuid()
        {
            var luid = new Luid("12345678");
            var luid2 = new Luid("23456789");

            Assert.NotEqual(luid, luid2);
        }

        [Fact]
        public void Constructor_WithInvalidString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new Luid("1234567"));
        }

        [Fact]
        public void Constructor_GeneratesUniqueLuid()
        {
            const int numTests = 10000000;

            var luids = new HashSet<Luid>(numTests);
            for (int i = 0; i < numTests; i++)
            {
                var luid = new Luid();
                Assert.DoesNotContain(luid, luids);
                luids.Add(luid);
            }
        }
    }
}