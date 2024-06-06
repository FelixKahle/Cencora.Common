// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;

namespace Cencora.Common.Core.Tests
{
    public class ApiResponseTests
    {
        [Fact]
        public void Success_WithoutParameters_ShouldSetStatusCodeTo200()
        {
            var response = ApiResponse.Success();
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public void Success_WithParameters_ShouldCreateInstance()
        {
            var response = ApiResponse.Success(200);
            Assert.Equal(200, response.StatusCode);

            response = ApiResponse.Success(System.Net.HttpStatusCode.OK);
            Assert.Equal(200, response.StatusCode);
        }

        [Fact]
        public void Success_WithInvalidStatusCode_ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse.Success(500));
        }

        [Fact]
        public void Error_WithValidParameters_ShouldCreateInstance()
        {
            var response = ApiResponse.Error(500, "Internal Server Error");
            Assert.Equal(500, response.StatusCode);

            response.Match(
                success: (code) => Assert.Fail("Should not be success"),
                error: (code, message) =>
                {
                    Assert.Equal(500, code);
                    Assert.Equal("Internal Server Error", message);
                }
            );
        }

        [Fact]
        public void Error_WithInvalidStatusCode_ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse.Error(200, "OK"));
        }

        [Fact]
        public void Match_WithSuccess_ShouldCallSuccess()
        {
            var response = ApiResponse.Success();
            response.Match(
                success: (code) => Assert.Equal(200, code),
                error: (code, message) => Assert.Fail("Should not be error")
            );
        }

        [Fact]
        public void Match_WithError_ShouldCallError()
        {
            var response = ApiResponse.Error(500, "Internal Server Error");
            response.Match(
                success: (code) => Assert.Fail("Should not be success"),
                error: (code, message) =>
                {
                    Assert.Equal(500, code);
                    Assert.Equal("Internal Server Error", message);
                }
            );
        }

        [Fact]
        public void Match_WithNull_ShouldThrowException()
        {
            var response = ApiResponse.Success();
            Assert.Throws<ArgumentNullException>(() => response.Match(
                success: (code) => Assert.Fail("Should not be reached"),
                null!
            ));
            Assert.Throws<ArgumentNullException>(() => response.Match(
                null!,
                error: (code, message) => Assert.Fail("Should not be reached")
            ));
        }

        [Fact]
        public void MatchWithReturn_WithSuccess_ShouldCallSuccess()
        {
            var response = ApiResponse.Success();
            var result = response.Match(
                success: (code) => code,
                error: (code, message) => 
                {
                    Assert.Fail("Should not be error");
                    return 0;
                }
            );
            Assert.Equal(200, result);
        }

        [Fact]
        public void MatchWithReturn_WithError_ShouldCallError()
        {
            var response = ApiResponse.Error(500, "Internal Server Error");
            var result = response.Match(
                success: (code) => 
                {
                    Assert.Fail("Should not be success");
                    return 0;
                },
                error: (code, message) => code
            );
            Assert.Equal(500, result);
        }

        [Fact]
        public void MatchWithReturn_WithNull_ShouldThrowException()
        {
            var response = ApiResponse.Success();
            Assert.Throws<ArgumentNullException>(() => response.Match(
                success: (code) => 
                {
                    Assert.Fail("Should not be reached");
                    return 0;
                },
                null!
            ));
            Assert.Throws<ArgumentNullException>(() => response.Match(
                null!,
                error: (code, message) => 
                {
                    Assert.Fail("Should not be reached");
                    return 0;
                }
            ));
        }

        [Fact]
        public void Equals_WithEqualInstances_ShouldReturnTrue()
        {
            var response1 = ApiResponse.Success();
            var response2 = ApiResponse.Success();
            Assert.Equal(response1, response2);
            Assert.True(response1 == response2);
            Assert.False(response1 != response2);
        }

        [Fact]
        public void Equals_WithDifferentInstances_ShouldReturnFalse()
        {
            var response1 = ApiResponse.Success();
            var response2 = ApiResponse.Error(500, "Internal Server Error");
            Assert.NotEqual(response1, response2);
            Assert.False(response1 == response2);
            Assert.True(response1 != response2);
        }

        [Fact]
        public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
        {
            var response1 = ApiResponse.Success();
            var response2 = ApiResponse.Success();
            Assert.Equal(response1.GetHashCode(), response2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
        {
            var response1 = ApiResponse.Success();
            var response2 = ApiResponse.Error(500, "Internal Server Error");
            Assert.NotEqual(response1.GetHashCode(), response2.GetHashCode());
        }

        [Fact]
        public void Json_Serialize_ShouldReturnCorrectJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string expected = "{\"statusCode\":201}";

            var response = ApiResponse.Success(201);
            var json = JsonSerializer.Serialize(response, options);

            Assert.Equal(expected, json, true, true, true, true);
        }

        [Fact]
        public void Json_Deserialize_ShouldReturnCorrectInstance()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            string json = "{\"statusCode\":200}";
            var response = JsonSerializer.Deserialize<ApiResponse>(json, options);

            var expected = ApiResponse.Success();
            Assert.Equal(expected, response);
        }
    }

    public class GenericApiResponseTests
    {
        [Fact]
        public void Success_WithoutParameters_ShouldSetStatusCodeTo200()
        {
            var response = ApiResponse<bool>.Success(true);
            Assert.Equal(200, response.StatusCode);
            response.Match(
                success: (val, code) => Assert.True(val),
                error: (code, message) => Assert.Fail("Should not be error")
            );
        }

        [Fact]
        public void Success_WithParameters_ShouldCreateInstance()
        {
            var response = ApiResponse<bool>.Success(true, 200);
            Assert.Equal(200, response.StatusCode);
            response.Match(
                success: (val, code) => Assert.True(val),
                error: (code, message) => Assert.Fail("Should not be error")
            );

            response = ApiResponse<bool>.Success(true, System.Net.HttpStatusCode.OK);
            Assert.Equal(200, response.StatusCode);
            response.Match(
                success: (val, code) => Assert.True(val),
                error: (code, message) => Assert.Fail("Should not be error")
            );
        }

        [Fact]
        public void Success_WithInvalidStatusCode_ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse<bool>.Success(true, 500));
        }

        [Fact]
        public void Success_WithNullPayload_ShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ApiResponse<bool?>.Success(null!));
        }

        [Fact]
        public void Error_WithValidParameters_ShouldCreateInstance()
        {
            var response = ApiResponse<bool>.Error(500, "Internal Server Error");
            Assert.Equal(500, response.StatusCode);
            response.Match(
                success: (val, code) => Assert.Fail("Should not be success"),
                error: (code, message) =>
                {
                    Assert.Equal(500, code);
                    Assert.Equal("Internal Server Error", message);
                }
            );
        }

        [Fact]
        public void Error_WithInvalidStatusCode_ShouldThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ApiResponse<bool>.Error(200, "OK"));
        }

        [Fact]
        public void Match_WithSuccess_ShouldCallSuccess()
        {
            var response = ApiResponse<bool>.Success(true);
            response.Match(
                success: (val, code) => 
                {
                    Assert.True(val);
                    Assert.Equal(200, code);
                },
                error: (code, message) => Assert.Fail("Should not be error")
            );
        }

        [Fact]
        public void Match_WithError_ShouldCallError()
        {
            var response = ApiResponse<bool>.Error(500, "Internal Server Error");
            response.Match(
                success: (val, code) => Assert.Fail("Should not be success"),
                error: (code, message) =>
                {
                    Assert.Equal(500, code);
                    Assert.Equal("Internal Server Error", message);
                }
            );
        }

        [Fact]
        public void Match_WithNull_ShouldThrowException()
        {
            var response = ApiResponse<bool>.Success(true);
            Assert.Throws<ArgumentNullException>(() => response.Match(
                success: (val, code) => Assert.Fail("Should not be reached"),
                null!
            ));
            Assert.Throws<ArgumentNullException>(() => response.Match(
                null!,
                error: (code, message) => Assert.Fail("Should not be reached")
            ));
        }

        [Fact]
        public void MatchWithReturn_WithSuccess_ShouldCallSuccess()
        {
            var response = ApiResponse<bool>.Success(true);
            var result = response.Match(
                success: (val, code) => 
                {
                    Assert.True(val);
                    Assert.Equal(200, code);
                    return val;
                },
                error: (code, message) => 
                {
                    Assert.Fail("Should not be error");
                    return false;
                }
            );
            Assert.True(result);
        }

        [Fact]
        public void MatchWithReturn_WithError_ShouldCallError()
        {
            var response = ApiResponse<bool>.Error(500, "Internal Server Error");
            var result = response.Match(
                success: (val, code) => 
                {
                    Assert.Fail("Should not be success");
                    return false;
                },
                error: (code, message) => false
            );
            Assert.False(result);
        }

        [Fact]
        public void MatchWithReturn_WithNull_ShouldThrowException()
        {
            var response = ApiResponse<bool>.Success(true);
            Assert.Throws<ArgumentNullException>(() => response.Match(
                success: (val, code) => 
                {
                    Assert.Fail("Should not be reached");
                    return false;
                },
                null!
            ));
            Assert.Throws<ArgumentNullException>(() => response.Match(
                null!,
                error: (code, message) => 
                {
                    Assert.Fail("Should not be reached");
                    return false;
                }
            ));
        }

        [Fact]
        public void Into_WithSuccessAndValidConverter_ShouldConvert()
        {
            var response = ApiResponse<bool>.Success(true);
            var result = response.Into<int>(val => val ? 1 : 0);
            result.Match(
                success: (val, code) => 
                {
                    Assert.Equal(1, val);
                    Assert.Equal(200, code);
                },
                error: (code, message) => Assert.Fail("Should not be error")
            );
        }

        [Fact]
        public void Into_WithErrorAndValidConerter_ShouldConvert()
        {
            var response = ApiResponse<bool>.Error(500, "Internal Server Error");
            var result = response.Into<int>(val => val ? 1 : 0);
            result.Match(
                success: (val, code) => Assert.Fail("Should not be success"),
                error: (code, message) =>
                {
                    Assert.Equal(500, code);
                    Assert.Equal("Internal Server Error", message);
                }
            );
        }

        [Fact]
        public void Into_WithNullConverter_ShouldThrowException()
        {
            var response = ApiResponse<bool>.Success(true);
            Assert.Throws<ArgumentNullException>(() => response.Into<int>(null!));
        }

        [Fact]
        public void Equals_WithEqualInstances_ShouldReturnTrue()
        {
            var response1 = ApiResponse<bool>.Success(true);
            var response2 = ApiResponse<bool>.Success(true);
            Assert.Equal(response1, response2);
            Assert.True(response1 == response2);
            Assert.False(response1 != response2);
        }

        [Fact]
        public void Equals_WithDifferentInstances_ShouldReturnFalse()
        {
            var response1 = ApiResponse<bool>.Success(true);
            var response2 = ApiResponse<bool>.Error(500, "Internal Server Error");
            Assert.NotEqual(response1, response2);
            Assert.False(response1 == response2);
            Assert.True(response1 != response2);
        }

        [Fact]
        public void GetHashCode_WithEqualInstances_ShouldReturnEqualHashCodes()
        {
            var response1 = ApiResponse<bool>.Success(true);
            var response2 = ApiResponse<bool>.Success(true);
            Assert.Equal(response1.GetHashCode(), response2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentInstances_ShouldReturnDifferentHashCodes()
        {
            var response1 = ApiResponse<bool>.Success(true);
            var response2 = ApiResponse<bool>.Error(500, "Internal Server Error");
            Assert.NotEqual(response1.GetHashCode(), response2.GetHashCode());
        }

        [Fact]
        public void Json_Serialize_ShouldReturnCorrectJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string expected = "{\"statusCode\":201,\"payload\":true}";

            var response = ApiResponse<bool>.Success(true, 201);
            var json = JsonSerializer.Serialize(response, options);

            Assert.Equal(expected, json, true, true, true, true);
        }

        [Fact]
        public void Json_Deserialize_ShouldReturnCorrectInstance()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            string json = "{\"statusCode\":200}";
            var response = JsonSerializer.Deserialize<ApiResponse>(json, options);

            var expected = ApiResponse.Success();
            Assert.Equal(expected, response);
        }
    }

    public class ApiResponseConverterTests
    {
        [Fact]
        public void Read_WithApiResponse_ShouldReturnCorrectJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string expected = "{\"statusCode\":201}";

            var response = ApiResponse.Success(201);
            var json = JsonSerializer.Serialize(response, options);

            Assert.Equal(expected, json, true, true, true, true);
        }

        [Fact]
        public void Read_WithApiResponse_ShouldReturnCorrectInstance()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            string json = "{\"statusCode\":200}";
            var response = JsonSerializer.Deserialize<ApiResponse>(json, options);

            var expected = ApiResponse.Success();
            Assert.Equal(expected, response);
        }
    }

    public class GenericApiResponseConverterTests
    {
        [Fact]
        public void Read_WithApiResponse_ShouldReturnCorrectJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string expected = "{\"statusCode\":201,\"payload\":true}";

            var response = ApiResponse<bool>.Success(true, 201);
            var json = JsonSerializer.Serialize(response, options);

            Assert.Equal(expected, json, true, true, true, true);
        }

        [Fact]
        public void Read_WithApiResponse_ShouldReturnCorrectInstance()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            string json = "{\"statusCode\":200,\"payload\":true}";
            var response = JsonSerializer.Deserialize<ApiResponse<bool>>(json, options);

            var expected = ApiResponse<bool>.Success(true);
            Assert.Equal(expected, response);
        }
    }
}