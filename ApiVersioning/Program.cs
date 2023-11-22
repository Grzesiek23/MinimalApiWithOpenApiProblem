using ApiVersioning;
using ApiVersioning.Swagger;
using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer()
    .AddApiVersioning(
        options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new MediaTypeApiVersionReader("api-version");
        })
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

var app = builder.Build();
app.CreateApiVersionSet();

app.MapGet("/with", () => "Hello World WithOpenApi v1!").WithApiVersionSet(Versioning.VersionSet).HasApiVersion(1.0).WithOpenApi();
app.MapGet("/with", () => "Hello World WithOpenApi v2!").WithApiVersionSet(Versioning.VersionSet).HasApiVersion(2.0).WithOpenApi();

app.MapGet("/without", () => "Hello World without OpenApi v1!").WithApiVersionSet(Versioning.VersionSet).HasApiVersion(1.0);
app.MapGet("/without", () => "Hello World without OpenApi v2!").WithApiVersionSet(Versioning.VersionSet).HasApiVersion(2.0);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                var name = description.GroupName.ToUpperInvariant();
                options.SwaggerEndpoint(url, name);
            }
        });
}

app.UseHttpsRedirection();

app.Run();