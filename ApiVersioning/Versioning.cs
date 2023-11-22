using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;

namespace ApiVersioning;

public static class Versioning
{
    public static ApiVersionSet VersionSet { get; private set; } = null!;

    public static void CreateApiVersionSet(this IEndpointRouteBuilder app)
    {
        VersionSet = app.NewApiVersionSet()
            .HasApiVersion(1.0)
            .ReportApiVersions()
            .Build();
    }
}