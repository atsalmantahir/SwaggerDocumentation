using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using SwaggerDocumentation.Entities;

namespace SwaggerDocumentation.OData
{
    public class ODataEntityDataModel
    {
        public IEdmModel GetEntityDataModel() 
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "SwaggerDocumentation";
            builder.ContainerName = "SwaggerDocumentationContainer";

            builder.EntitySet<Book>("Book");
            return builder.GetEdmModel();
        }
    }
}
