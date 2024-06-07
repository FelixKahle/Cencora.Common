// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cencora.Common.Core.Swagger
{
    public class TemperatureSchema : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(Temperature))
            {
                schema.Type = "object";
                schema.Properties.Add("value", new OpenApiSchema { Type = "number", Format = "double" });
                schema.Properties.Add("unit", new OpenApiSchema { Type = "string" });
            }
        }
    }
}