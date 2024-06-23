// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

namespace Cencora.Common.Core.Tests
{
    public class LuidTests
    {
        [Fact]
        public void Constructor_WithString_CreatesLuid()
        {
            string luid = "12345678";
            Luid result = new Luid(luid);
            Assert.Equal("12345678", luid.ToString());
        }
    }
}