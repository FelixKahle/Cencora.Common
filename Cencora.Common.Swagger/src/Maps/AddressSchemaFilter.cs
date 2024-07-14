// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Maps;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cencora.Common.Swagger.Maps
{
    public class AddressSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(Address)) return;
            schema.Type = "object";
            schema.Properties = new Dictionary<string, OpenApiSchema>
            {
                { "addressLine1", new OpenApiSchema { Type = "string" } },
                { "addressLine2", new OpenApiSchema { Type = "string" } },
                { "city", new OpenApiSchema { Type = "string" } },
                { "stateOrProvince", new OpenApiSchema { Type = "string" } },
                { "postalCode", new OpenApiSchema { Type = "string" } },
                { "country", new OpenApiSchema { Type = "string" } }
            };
        }
    }
}