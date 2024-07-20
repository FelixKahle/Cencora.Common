// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements;

/// <summary>
/// Converts a <see cref="Temperature"/> object to and from JSON using custom logic.
/// Saves the temperature value in Kelvin.
/// </summary>
public class TemperatureConverter : JsonConverter<Temperature>
{
    /// <inheritdoc/>
    public override Temperature Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
        var unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        double value = 0;
        var unit = TemperatureUnit.Kelvin;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Temperature(value, unit);
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }

            var propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

            if (propertyName == valuePropertyName)
            {
                reader.Read();
                value = reader.GetDouble();
            }
            else if (propertyName == unitPropertyName)
            {
                reader.Read();
                unit = TemperatureUnitExtensions.FromString(reader.GetString() ?? throw new JsonException("Expected unit"));
            }
            else
            {
                throw new JsonException($"Unknown property: {propertyName}");
            }
        }

        throw new JsonException("Unexpected end of JSON");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Temperature value, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
        var unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

        writer.WriteStartObject();
        writer.WriteNumber(valuePropertyName, value.Kelvin);
        writer.WriteString(unitPropertyName, TemperatureUnit.Kelvin.ToUnitString());
        writer.WriteEndObject();
    }
}