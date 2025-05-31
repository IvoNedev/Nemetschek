using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Nemetschek.API.Extensions
{
    public class ClearStringExampleFilter : ISchemaFilter
    {
        ///FIx for swagger defaulting string input fields to "string"
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Type == "string")
            {
                schema.Example = new OpenApiString(string.Empty);
                schema.Default = new OpenApiString(string.Empty);
            }
        }
    }

}
