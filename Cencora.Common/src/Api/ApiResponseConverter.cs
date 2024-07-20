// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Api;

/// <summary>
/// Provides a custom JSON converter for the <see cref="ApiResponse"/> struct.
/// </summary>
public class ApiResponseConverter : JsonConverter<ApiResponse>
{
    /// <inheritdoc/>
    public override ApiResponse Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
        var errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";
        var comparison = options.PropertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        var statusCode = 0;
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

            var propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

            if (string.Equals(propertyName, statusPropertyName, comparison))
            {
                reader.Read();
                statusCode = reader.GetInt32();
            }
            else if (string.Equals(propertyName, errorMessagePropertyName, comparison))
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
        var namingPolicy = options.PropertyNamingPolicy;
        var statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
        var errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";

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

        var types = typeToConvert.GetGenericArguments() ?? throw new InvalidOperationException("Expected generic arguments.");
        if (types.Length != 1)
        {
            throw new InvalidOperationException("Expected exactly one generic argument.");
        }

        var payloadType = types.First();
        var converterType = typeof(PayloadApiResponseConverter<>).MakeGenericType(payloadType);
        var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
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
        var namingPolicy = options.PropertyNamingPolicy;
        var statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
        var errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";
        var payloadPropertyName = namingPolicy?.ConvertName("Payload") ?? "Payload";
        var comparison = options.PropertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        var statusCode = 0;
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

            var propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

            if (string.Equals(propertyName, statusPropertyName, comparison))
            {
                reader.Read();
                statusCode = reader.GetInt32();
            }
            else if (string.Equals(propertyName, errorMessagePropertyName, comparison))
            {
                reader.Read();
                errorMessage = reader.GetString();
            }
            else if (string.Equals(propertyName, payloadPropertyName, comparison))
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
        var namingPolicy = options.PropertyNamingPolicy;
        var statusPropertyName = namingPolicy?.ConvertName("StatusCode") ?? "StatusCode";
        var errorMessagePropertyName = namingPolicy?.ConvertName("ErrorMessage") ?? "ErrorMessage";
        var payloadPropertyName = namingPolicy?.ConvertName("Payload") ?? "Payload";

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