// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using Xunit.Abstractions;

namespace Cencora.Common.Core.Tests
{
    public class ApiResponseTests
    {
        private readonly ITestOutputHelper output;

        public ApiResponseTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ApiResponse_Error_Throws_IfStatusCodeIsNotError()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse.Error(200, "OK"));
        }

        [Fact]
        public void ApiResponse_Payload_Error_Throws_IfStatusCodeIsNotError()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse<int>.Error(200, "OK"));
        }

        [Fact]
        public void ApiResponse_Success_Throws_IfStatusCodeIsError()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse.Success(404));
        }

        [Fact]
        public void ApiResponse_Success_SerializeAndDeserialize_Correctly()
        {
            var response = ApiResponse.Success(200);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(response, options);
            output.WriteLine(json);
            var deserialized = JsonSerializer.Deserialize<ApiResponse>(json, options);
            Assert.Equal(response, deserialized);
        }

        [Fact]
        public void ApiResponse_Payload_Success_SerializeAndDeserialize_Correctly()
        {
            var response = ApiResponse<int>.Success(20000, 200);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(response, options);
            output.WriteLine(json);
            var deserialized = JsonSerializer.Deserialize<ApiResponse<int>>(json, options);
            Assert.Equal(response, deserialized);
        }

        [Fact]
        public void ApiResponse_Error_SerializeAndDeserialize_Correctly()
        {
            var response = ApiResponse.Error(404, "Not Found");

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(response, options);
            output.WriteLine(json);
            var deserialized = JsonSerializer.Deserialize<ApiResponse>(json, options);
            Assert.Equal(response, deserialized);
        }

        [Fact]
        public void ApiResponse_Payload_Error_SerializeAndDeserialize_Correctly()
        {
            var response = ApiResponse<int>.Error(404, "Not Found");

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string json = JsonSerializer.Serialize(response, options);
            output.WriteLine(json);
            var deserialized = JsonSerializer.Deserialize<ApiResponse<int>>(json, options);
            Assert.Equal(response, deserialized);
        }
    }
}