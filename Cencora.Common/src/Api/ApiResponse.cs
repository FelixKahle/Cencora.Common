// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Api
{
    /// <summary>
    /// Represents an API response without a payload.
    /// </summary>
    [JsonConverter(typeof(ApiResponseConverter))]
    public readonly struct ApiResponse : IEquatable<ApiResponse>
    {
        /// <summary>
        /// The status code of the response.
        /// </summary>
        private readonly int _statusCode;

        /// <summary>
        /// The error message of the response.
        /// </summary>
        private readonly string? _errorMessage;

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        public int StatusCode => _statusCode;

        /// <summary>
        /// Gets the error message of the response.
        /// </summary>
        public string? ErrorMessage => _errorMessage;

        /// <summary>
        /// Determines whether the response is successful.
        /// </summary>
        public bool IsSuccess => HttpUtils.IsSuccessStatusCode(_statusCode);

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> struct.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="errorMessage">The error message of the response.</param>
        /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
        private ApiResponse(int statusCode, string? errorMessage)
        {
            if (!HttpUtils.IsValidHttpStatusCode(statusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code is not a valid HTTP status code.");
            }

            _statusCode = statusCode;
            _errorMessage = errorMessage;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents a successful response.
        /// </summary>
        /// <param name="statusCode">The status code of the response. Default to 200</param>
        /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents a successful response.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The status code does not indicate success.</exception>
        public static ApiResponse Success(int statusCode = 200)
        {
            if (!HttpUtils.IsSuccessStatusCode(statusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code does not indicate success.");
            }

            return new ApiResponse(statusCode, null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents a successful response.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents a successful response.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The status code does not indicate success.</exception>
        public static ApiResponse Success(HttpStatusCode statusCode)
        {
            return Success(HttpUtils.HttpStatusCodeToInt(statusCode));
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents an error response.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="errorMessage">The error message of the response.</param>
        /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents an error response.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
        public static ApiResponse Error(int statusCode, string errorMessage)
        {
            if (HttpUtils.IsSuccessStatusCode(statusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code indicates success.");
            }

            return new ApiResponse(statusCode, errorMessage);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse"/> struct that represents an error response.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="errorMessage">The error message of the response.</param>
        /// <returns>A new instance of the <see cref="ApiResponse"/> struct that represents an error response.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
        public static ApiResponse Error(HttpStatusCode statusCode, string errorMessage)
        {
            return Error(HttpUtils.HttpStatusCodeToInt(statusCode), errorMessage);
        }

        /// <summary>
        /// Matches the response against the specified actions.
        /// </summary>
        /// <param name="success">The action to execute if the response is successful.</param>
        /// <param name="error">The action to execute if the response is an error.</param>
        /// <exception cref="ArgumentNullException">The success action is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The error action is <c>null</c>.</exception>
        public void Match(Action<int> success, Action<int, string> error)
        {
            if (success == null)
            {
                throw new ArgumentNullException(nameof(success));
            }
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            if (IsSuccess)
            {
                success(StatusCode);
            }
            else
            {
                error(StatusCode, _errorMessage ?? string.Empty);
            }
        }

        /// <summary>
        /// Matches the response against the specified functions.
        /// </summary>
        /// <typeparam name="TR">The return type of the functions.</typeparam>
        /// <param name="success">The function to execute if the response is successful.</param>
        /// <param name="error">The function to execute if the response is an error.</param>
        /// <returns>The result of the executed function.</returns>
        /// <exception cref="ArgumentNullException">The success function is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The error function is <c>null</c>.</exception>
        public TR Match<TR>(Func<int, TR> success, Func<int, string, TR> error)
        {
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(error);

            return IsSuccess ? success(StatusCode) : error(StatusCode, _errorMessage ?? string.Empty);
        }

        /// <inheritdoc/>
        public bool Equals(ApiResponse other)
        {
            return _statusCode == other._statusCode && _errorMessage == other._errorMessage;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is ApiResponse other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(_statusCode, _errorMessage);
        }

        public static bool operator ==(ApiResponse left, ApiResponse right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ApiResponse left, ApiResponse right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Provides a custom JSON converter for the <see cref="ApiResponse"/> struct.
    /// </summary>
    public class ApiResponseConverter : JsonConverter<ApiResponse>
    {
        /// <inheritdoc/>
        public override ApiResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
            string errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            int statusCode = 0;
            string? errorMessage = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return HttpUtils.IsSuccessStatusCode(statusCode)
                        ? ApiResponse.Success(statusCode)
                        : ApiResponse.Error(statusCode, errorMessage ?? string.Empty);
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name");
                }

                string propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

                if (propertyName == statusPropertyName)
                {
                    reader.Read();
                    statusCode = reader.GetInt32();
                }
                else if (propertyName == errorMessagePropertyName)
                {
                    reader.Read();
                    errorMessage = reader.GetString();
                }
                else
                {
                    throw new JsonException($"Unknown property: {propertyName}");
                }
            }

            throw new JsonException("Unexpected end of JSON");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ApiResponse value, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
            string errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";

            writer.WriteStartObject();
            value.Match(
                success: statusCode => writer.WriteNumber(statusPropertyName, statusCode),
                error: (statusCode, errorMessage) =>
                {
                    writer.WriteNumber(statusPropertyName, statusCode);
                    writer.WriteString(errorMessagePropertyName, errorMessage);
                }
            );
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// Represents an API response with a payload.
    /// </summary>
    [JsonConverter(typeof(PayloadApiResponseConverterFactory))]
    public readonly struct ApiResponse<T> : IEquatable<ApiResponse<T>>
    {
        /// <summary>
        /// The status code of the response.
        /// </summary>
        private readonly int _statusCode;

        /// <summary>
        /// The error message of the response.
        /// </summary>
        private readonly string? _errorMessage;

        /// <summary>
        /// The payload of the response.
        /// </summary>
        private readonly T? _payload;

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        public int StatusCode => _statusCode;

        /// <summary>
        /// Determines whether the response is successful.
        /// </summary>
        public bool IsSuccess => HttpUtils.IsSuccessStatusCode(_statusCode) && _payload != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> struct.
        /// </summary>
        /// <param name="payload">The payload of the response.</param>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="errorMessage">The error message of the response.</param>
        /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
        private ApiResponse(T? payload, int statusCode, string? errorMessage)
        {
            if (!HttpUtils.IsValidHttpStatusCode(statusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code is not a valid HTTP status code.");
            }

            _statusCode = statusCode;
            _errorMessage = errorMessage;
            _payload = payload;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse{T}"/> struct that represents a successful response.
        /// </summary>
        /// <param name="payload">The payload of the response.</param>
        /// <param name="statusCode">The status code of the response.</param>
        /// <returns>A new instance of the <see cref="ApiResponse{T}"/> struct that represents a successful response.</returns>
        /// <exception cref="ArgumentNullException">The payload is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The status code does not indicate success.</exception>
        public static ApiResponse<T> Success(T payload, int statusCode = 200)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }

            if (!HttpUtils.IsSuccessStatusCode(statusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code does not indicate success.");
            }

            return new ApiResponse<T>(payload, statusCode, null);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="errorMessage">The error message of the response.</param>
        /// <returns>A new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
        public static ApiResponse<T> Error(int statusCode, string errorMessage)
        {
            if (HttpUtils.IsSuccessStatusCode(statusCode))
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "The status code indicates success.");
            }

            return new ApiResponse<T>(default, statusCode, errorMessage);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.
        /// </summary>
        /// <param name="statusCode">The status code of the response.</param>
        /// <param name="errorMessage">The error message of the response.</param>
        /// <returns>A new instance of the <see cref="ApiResponse{T}"/> struct that represents an error response.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The status code is not a valid HTTP status code.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The status code indicates success.</exception>
        public static ApiResponse<T> Error(HttpStatusCode statusCode, string errorMessage)
        {
            return Error(HttpUtils.HttpStatusCodeToInt(statusCode), errorMessage);
        }

        /// <summary>
        /// Gets the payload of the response.
        /// </summary>
        /// <returns>The payload of the response.</returns>
        /// <exception cref="InvalidOperationException">The payload is <c>null</c>.</exception>
        public T Value
        {
            get
            {
                if (_payload == null)
                {
                    throw new InvalidOperationException("The payload is null.");
                }
                return _payload;
            }
        }

        /// <summary>
        /// Gets the error message of the response.
        /// </summary>
        public string? ErrorMessage => _errorMessage;

        /// <summary>
        /// Matches the response against the specified actions.
        /// </summary>
        /// <param name="success">The action to execute if the response is successful.</param>
        /// <param name="error">The action to execute if the response is an error.</param>
        /// <exception cref="ArgumentNullException">The success action is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The error action is <c>null</c>.</exception>
        public void Match(Action<T, int> success, Action<int, string> error)
        {
            if (success == null)
            {
                throw new ArgumentNullException(nameof(success));
            }
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            if (IsSuccess)
            {
                if (_payload == null)
                {
                    throw new InvalidOperationException("The payload is null.");
                }
                success(_payload, StatusCode);
            }
            else
            {
                error(StatusCode, _errorMessage ?? string.Empty);
            }
        }

        /// <summary>
        /// Matches the response against the specified functions.
        /// </summary>
        /// <typeparam name="TR">The return type of the functions.</typeparam>
        /// <param name="success">The function to execute if the response is successful.</param>
        /// <param name="error">The function to execute if the response is an error.</param>
        /// <returns>The result of the executed function.</returns>
        /// <exception cref="ArgumentNullException">The success function is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">The error function is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The payload is <c>null</c>.</exception>
        public TR Match<TR>(Func<T, int, TR> success, Func<int, string, TR> error)
        {
            ArgumentNullException.ThrowIfNull(success);
            ArgumentNullException.ThrowIfNull(error);

            if (IsSuccess)
            {
                // Should never happen, because IsSuccess checks for _payload != null
                // But we need to check it here to satisfy the compiler and to be sure.
                if (_payload == null)
                {
                    throw new InvalidOperationException("The payload is null.");
                }
                return success(_payload, StatusCode);
            }
            else
            {
                return error(StatusCode, _errorMessage ?? string.Empty);
            }
        }

        /// <summary>
        /// Converts the payload of the current <see cref="ApiResponse{T}"/> to a new type using the specified payload converter function.
        /// </summary>
        /// <typeparam name="TR">The type to convert the payload to.</typeparam>
        /// <param name="payloadConverter">The function that converts the payload to the new type.</param>
        /// <returns>An <see cref="ApiResponse{R}"/> with the converted payload.</returns>
        /// <exception cref="InvalidOperationException">The payload converter is <c>null</c>. Only if the Response is success and the converter is null.</exception>
        public ApiResponse<TR> Into<TR>(Func<T, TR> payloadConverter)
        {
            if (payloadConverter == null)
            {
                throw new ArgumentNullException(nameof(payloadConverter));
            }

            return Match(
                success: (payload, statusCode) => ApiResponse<TR>.Success(payloadConverter(payload), statusCode),
                error: ApiResponse<TR>.Error
            );
        }

        /// <summary>
        /// Unwraps the payload of the current <see cref="ApiResponse{T}"/>.
        /// </summary>
        /// <returns>The payload of the current <see cref="ApiResponse{T}"/>.</returns>
        /// <exception cref="InvalidOperationException">The payload is <c>null</c>.</exception>
        public T Unwrap()
        {
            if (_payload == null)
            {
                throw new InvalidOperationException("The payload is null.");
            }
            return _payload;
        }

        /// <inheritdoc/>
        public bool Equals(ApiResponse<T> other)
        {
            return _statusCode == other._statusCode
                && _errorMessage == other._errorMessage
                && EqualityComparer<T>.Default.Equals(_payload, other._payload);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is ApiResponse<T> other && Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(_statusCode, _errorMessage, _payload);
        }

        public static bool operator ==(ApiResponse<T> left, ApiResponse<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ApiResponse<T> left, ApiResponse<T> right)
        {
            return !left.Equals(right);
        }
    }

    /// <summary>
    /// Converts an <see cref="ApiResponse{T}"/> to JSON and vice versa, where T is the payload type.
    /// </summary>
    public class PayloadApiResponseConverterFactory : JsonConverterFactory
    {
        /// <summary>
        /// Determines whether the specified type can be converted.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <returns><c>true</c> if the type can be converted; otherwise, <c>false</c>.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ApiResponse<>);
        }

        /// <summary>
        /// Creates a converter for the specified type.
        /// </summary>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">The serializer options.</param>
        /// <returns>A converter for the specified type.</returns>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            // Even though we checked in CanConvert that the type is ApiResponse<T>, we check here again.
            // The function looks like it can fail often, but it will only do so if the type is not ApiResponse<T>.

            Type[] types = typeToConvert.GetGenericArguments() ?? throw new InvalidOperationException("Expected generic arguments.");
            if (types.Length != 1)
            {
                throw new InvalidOperationException("Expected exactly one generic argument.");
            }

            Type payloadType = types.First();
            Type converterType = typeof(PayloadApiResponseConverter<>).MakeGenericType(payloadType);
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(converterType)!;
            if (converter == null)
            {
                throw new InvalidOperationException("Failed to create converter.");
            }
            return converter;
        }
    }

    /// <summary>
    /// Provides a custom JSON converter for the <see cref="ApiResponse{T}"/> struct.
    /// </summary>
    public class PayloadApiResponseConverter<T> : JsonConverter<ApiResponse<T>>
    {
        /// <inheritdoc/>
        public override ApiResponse<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
            string errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";
            string payloadPropertyName = namingPolicy?.ConvertName("Payload") ?? "Payload";

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Expected start of object");
            }

            int statusCode = 0;
            string? errorMessage = null;
            T? payload = default;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (payload == null)
                    {
                        throw new JsonException("Payload is null");
                    }

                    return HttpUtils.IsSuccessStatusCode(statusCode)
                        ? ApiResponse<T>.Success(payload, statusCode)
                        : ApiResponse<T>.Error(statusCode, errorMessage ?? string.Empty);
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException("Expected property name");
                }

                string propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

                if (propertyName == statusPropertyName)
                {
                    reader.Read();
                    statusCode = reader.GetInt32();
                }
                else if (propertyName == errorMessagePropertyName)
                {
                    reader.Read();
                    errorMessage = reader.GetString();
                }
                else if (propertyName == payloadPropertyName)
                {
                    reader.Read();
                    payload = JsonSerializer.Deserialize<T>(ref reader, options);
                }
                else
                {
                    throw new JsonException($"Unknown property: {propertyName}");
                }
            }

            throw new JsonException("Unexpected end of JSON");
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, ApiResponse<T> value, JsonSerializerOptions options)
        {
            JsonNamingPolicy? namingPolicy = options.PropertyNamingPolicy;
            string statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
            string errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";
            string payloadPropertyName = namingPolicy?.ConvertName("Payload") ?? "Payload";

            writer.WriteStartObject();
            value.Match(
                success: (payload, statusCode) =>
                {
                    writer.WriteNumber(statusPropertyName, statusCode);
                    writer.WritePropertyName(payloadPropertyName);
                    JsonSerializer.Serialize(writer, payload, options);
                },
                error: (statusCode, errorMessage) =>
                {
                    writer.WriteNumber(statusPropertyName, statusCode);
                    writer.WriteString(errorMessagePropertyName, errorMessage);
                }
            );
            writer.WriteEndObject();
        }
    }
}