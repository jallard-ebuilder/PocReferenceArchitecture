using Autofac;
using Autofac.Extensions.DependencyInjection;
using PPM.HealthChecks.Utilities;
using PPM.WebApi.Logging;
using PPM.WebApi.Logging.Extensions;

const string serviceRootUrl = "/api/v1/demo";
const string serviceName = "Platform Service Reference Architecture";

/*
 * NOTES
 *  - health doesn't appear in swagger
 *      https://www.codenesium.com/blog/posts/how-to-add-health-checks-asp-net-core-with-swagger-support/
 *  - add error handling around build and/or run, to make sure startup exceptions are caught and logged
 *          logging docs refers to ServiceUtility, which i can't find
 */

// DEMO: Logging
// toggle the ASPNETCORE_ENVIRONMENT environment variable between DEVELOPMENT and PRODUCTION
// to see the difference in logging. 
// PRODUCTION output is straight json.
// DEVELOPMENT output is a bit more user friendly; in rider it emits color-coded.
LoggerUtility.InitializeLogger();
var builder = WebApplication
    .CreateBuilder(args);

// DEMO: Logging
builder.Host.UsePpmSerilog();

// DEMO: use autofac instead of built in .net container
// the DI interface doesn't change, only the implementation
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(autofac =>
{
    // when registering services, you have the option to use either
    // builder.Services (.net) or ContainerBuilder (autofac). If you're doing basic
    // registrations, it all works out the same. The .net registrations will be loaded
    // into autofac.
    // but, autofac has additional capabilities such as RegisterDecorator.
    // if you want to do anything autofac specific, then you need to do it here.
    // mix and match as you see fit. it would be best to "pick one" (IServiceCollection or ContainerBuilder),
    // and only use the other when you need to.
    //
    // lifetimes
    //  .net                        autofac
    //----------------------------------------------------------------
    // Transient                    InstancePerDependency (default)
    // Singleton                    SingleInstance
    // Scoped                       InstancePerLifetimeScope
    // 
    // Autofac has additional lifetimes. But, these should be all that you need.
    // References
    //      https://autofac.readthedocs.io/en/latest/integration/index.html - not updated for .net6 as of 7/8/2022
    //      https://devblogs.microsoft.com/cesardelatorre/comparing-asp-net-core-ioc-service-life-times-and-autofac-ioc-instance-scopes/ - old, but appears to remain relevant
});

// DEMO: DI
// add your own services here.
// builder.Services.Add<>

builder.Services
    // DEMO: Swagger
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()

    // DEMO: Logging - serilog summary logging
    .AddSerilogRequestLogging()

    // DEMO: kafka
    .AddKafkaProducer(builder.Configuration)
    .AddKafkaConsumer(builder.Configuration);

// DEMO: HealthChecks - this is a built-in .net method.
// here, you will add health checks specific for your app.
// ie: if you're using sql server, you would use '.AddSqlServer(...)`
// https://github.com/e-buildernoc/package-ppm-sdks/tree/main/docs/PPM.HealthChecks#usage
// the usage of health checks is out of scope of this demo. this makes the health checks
// available; now you must add/create checks that are important for your app.
builder.Services.AddHealthChecks();

// DEMO: Logging - the e-builder logging controller 
builder.Services.AddControllers().AddLoggingController();


var app = builder.Build();

// DEMO: Swagger
var swaggerRoot = serviceRootUrl.Trim('/') + "/internals/swagger";
app
    .UseSwagger(options => { options.RouteTemplate = swaggerRoot + "/{documentName}/swagger.{json|yaml}"; })
    .UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("https://localhost:7108/api/v1/demo/internals/swagger/v1/swagger.json", serviceName);
        options.RoutePrefix = serviceRootUrl.Trim('/') + "/internals/swagger";
    });


// DEMO: if you are going to use controllers, enable this.
// this project has DemoController.
// Note that framework controllers are explicitly added, and do not
// depend on this call. IE: Logging, Health, Swagger, etc.
app.MapControllers();

// DEMO: health checks
app.MapPlatformHealthChecks(serviceRootUrl);

// DEMO: minimum api endpoint - remove or replace
// there is also an api controller class in the CONTROLLERS folder.
app.MapGet("",
    (HttpRequest request) =>
    {
        var root = request.Scheme + "://" + request.Headers.Host.First() + serviceRootUrl;
        var output = new
        {
            Links = new
            {
                DemoController = root + "/stuff",
                SwaggerJson = root + "/internals/swagger/v1/swagger.json",
                SwaggerYaml = root + "/internals/swagger/v1/swagger.yaml",
                SwaggerUi = root + "/internals/swagger",
                Health = root + "/internals/health"
            }
        };
        return Results.Json(output, contentType: "application/json");
    });

app.Run();