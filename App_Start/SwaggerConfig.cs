using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using Swashbuckle.Application;
using Swashbuckle.Swagger;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using RESTapiPruebaTeen;
using System.Web.Http.Description;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace RESTapiPruebaTeen
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "My API");
                    // Elimina la inclusión de comentarios de XML
                    // Agrega un filtro personalizado para cargar comentarios de YAML
                    c.DocumentFilter<YamlDocumentFilter>();
                })
                .EnableSwaggerUi();
        }
    }

    public class YamlDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Start", "swagger.yaml");
            if (File.Exists(filePath))
            {
                using (var reader = File.OpenText(filePath))
                {
                    var deserializer = new DeserializerBuilder().Build();
                    var yamlObject = deserializer.Deserialize(reader);
                    var json = JsonConvert.SerializeObject(yamlObject);
                    var newDoc = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    foreach (var item in newDoc)
                    {
                        swaggerDoc.vendorExtensions.Add(item.Key, item.Value);
                    }
                }
            }
        }
    }
}
