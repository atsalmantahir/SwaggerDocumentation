using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SwaggerDocumentation.Data;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData;
using SwaggerDocumentation.OData;
using OData.Swagger.Services;

namespace SwaggerDocumentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt => 
            {
                opt.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                opt.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                opt.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            });

            services.AddControllers()
                .AddOData(c =>
                c.AddRouteComponents("v{version}", new ODataEntityDataModel().GetEntityDataModel())
                .Select().Expand().Filter().OrderBy().SetMaxTop(100).SkipToken());
            services.AddDbContext<SwaggerDocumentationContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SwaggerDocumentationContext")));

            services.AddAutoMapper(typeof(Startup));

            services.AddVersionedApiExplorer(c=> 
            {
                c.GroupNameFormat = "'v'VV";
            });
            services.AddApiVersioning(c=> 
            {
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.DefaultApiVersion = new ApiVersion(1, 0);
                c.ReportApiVersions = false;
                    
            });


            var apiVersionDescription =
                services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(c =>
            {
                foreach (var item in apiVersionDescription.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        $"SwaggerDocumentationOpenAPI{item.GroupName}",
                        new OpenApiInfo
                        {
                            Title = "SwaggerDocumentation",
                            Version = item.ApiVersion.ToString(),
                            Description = "Through this API you can access books",
                            Contact = new OpenApiContact
                            {
                                Email = "stahir@rdt.co.uk",
                                Name = "Salman Tahir",
                                Url = new Uri("https://www.google.com.pk/"),
                            },
                            License = new OpenApiLicense
                            {
                                Name = "MIT License",
                                Url = new Uri("https://wwww.google.com.pk/"),
                            }
                        });
                }

                c.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var actionApiVersionModel = apiDescription.ActionDescriptor
                    .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

                    if (actionApiVersionModel == null) 
                    {
                        return true;
                    }

                    if (actionApiVersionModel.DeclaredApiVersions.Any()) 
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v =>
                        $"SwaggerDocumentationOpenAPIv{v.ToString()}" == documentName);
                    }

                    return actionApiVersionModel.ImplementedApiVersions.Any(v =>
                    $"SwaggerDocumentationOpenAPIv{v.ToString()}" == documentName);
                });
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                c.IncludeXmlComments(xmlCommentsFullPath);
            });
            services.AddOdataSwaggerSupport();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescription)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                {
                    foreach (var item in apiVersionDescription.ApiVersionDescriptions) 
                    {
                        c.SwaggerEndpoint($"/swagger/" + $"SwaggerDocumentationOpenAPI{item.GroupName}/swagger.json", item.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
