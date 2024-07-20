// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cencora.Common.Measurements;

/// <summary>
/// Provides a custom JSON converter for the <see cref="Weight"/> struct.
/// Saves the weight in grams.
/// </summary>
public class WeightConverter : JsonConverter<Weight>
{
    /// <inheritdoc/>
    public override Weight Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
        var unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";
        var comparison = options.PropertyNameCaseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object");
        }

        double value = 0;
        var unit = WeightUnit.Gram;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return new Weight(value, unit);
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }

            var propertyName = reader.GetString() ?? throw new JsonException("Expected property name");

            if (string.Equals(propertyName, valuePropertyName, comparison))
            {
                reader.Read();
                value = reader.GetDouble();
            }
            else if (string.Equals(propertyName, unitPropertyName, comparison))
            {
                reader.Read();
                unit = WeightUnitExtensions.FromString(reader.GetString() ?? throw new JsonException("Expected unit"));
            }
            else
            {
                throw new JsonException($"Unknown property: {propertyName}");
            }
        }

        throw new JsonException("Unexpected end of JSON");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Weight value, JsonSerializerOptions options)
    {
        var namingPolicy = options.PropertyNamingPolicy;
        var valuePropertyName = namingPolicy?.ConvertName("Value") ?? "Value";
        var unitPropertyName = namingPolicy?.ConvertName("Unit") ?? "Unit";

        writer.WriteStartObject();
        writer.WriteNumber(valuePropertyName, value.Grams);
        writer.WriteString(unitPropertyName, WeightUnit.Gram.ToUnitString());
        writer.WriteEndObject();
    }
}