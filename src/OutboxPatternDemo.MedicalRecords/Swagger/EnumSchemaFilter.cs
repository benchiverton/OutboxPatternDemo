using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace OutboxPatternDemo.MedicalRecords.Swagger;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            model.Enum.Clear();
            Enum.GetNames(context.Type)
                .ToList()
                .ForEach(n => model.Enum.Add(new OpenApiString(n)));
        }
    }
}
