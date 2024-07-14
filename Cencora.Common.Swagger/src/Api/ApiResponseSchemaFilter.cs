// Copyright 2024 Cencora. All rights reserved.
//
// Written by Felix Kahle, A123234, felix.kahle@worldcourier.de

using Cencora.Common.Api;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cencora.Common.Swagger.Api
{
    public class ApiResponseSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ApiResponse))
            {
                schema.Type = "object";
                schema.Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "statusCode", new OpenApiSchema { Type = "number", Format = "int" } },
                    { "errorMessage", new OpenApiSchema { Type = "string" } },
                };
            }
            else if (context.Type.IsGenericType && context.Type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
            {
                var genericArguments = context.Type.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
                    var responseType = genericArguments[0];
                    var responseSchema = context.SchemaGenerator.GenerateSchema(responseType, context.SchemaRepository);
                    schema.Type = "object";
                    schema.Properties = new Dictionary<string, OpenApiSchema>
                    {
                        { "statusCode", new OpenApiSchema { Type = "number", Format = "int" } },
                        { "errorMessage", new OpenApiSchema { Type = "string" } },
                        { "payload", responseSchema },
                    };
                }
            }
        }
    }
}