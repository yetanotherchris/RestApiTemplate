# RestApiTemplate
A .NET Core RESTful API example to demonstrate RESTful conventions, versioning, Swagger, errors that ASP.NET Core support.

## What the repository demonstrates

The repository has two projects, one using Swashbuckle for Swagger integration, and the other using NSwag.
The goal is to demonstrate the following common functionality you have with an API:

### Packages

The base `.csproj` file has one package reference: `<PackageReference Include="Microsoft.AspNetCore.App" />`. All other references in the `.csproj` file are additions.

### API and Swagger versioning

Route-based API Versioning and Swagger versioning support with both Swashbuckle and NSwag. 
e.g. `/v1/home/`. (_note:_ minor versions are optional - [docs](https://github.com/Microsoft/aspnet-api-versioning/wiki/Version-Format))

### Swagger documentation

Adding documentation (XML docs) to Swashbuckle and NSwag. This is achieved in part by adding the following to the `.csproj` files:

```
    <PropertyGroup>
        ...standard bits of csproj file
        <GenerateDocumentationFile>true</GenerateDocumentationFile>
        <NoWarn>$(NoWarn);1591</NoWarn>
    </PropertyGroup>

    <PropertyGroup Condition=" '$(Configuration)' == 'Debug' ">
        <DocumentationFile>bin\Debug\SwashbuckleExample.xml</DocumentationFile>
    </PropertyGroup>
    <PropertyGroup Condition=" '$(Configuration)' == 'Release' ">
        <DocumentationFile>bin\Release\SwashbuckleExample.xml</DocumentationFile>
    </PropertyGroup>
```

### JSON error messages instead of HTML

This is done via the `JsonExceptionMiddleware` class. All 500s are caught and the error message output as a JSON object for better readability.

### Validation of models using DataAnnotations

This is added by a call to `services.AddDataAnnotations()`. When validation fails, a BadRequest is returned with an error JSON object.

### HTTP status code conventions

e.g.OK, NotFound, BadRequest. Your controller actions need to return the appropriate status code, however Swagger picks up the documentation that is generated via a 

`[assembly: ApiConventionType(typeof(DefaultApiConventions))]` 

[Documentation](https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-2.2)

### ProblemDetails

This comes out-of-the-box since ASP.NET Core 2.2. _However_ there are some caveats:

* 404s for missing routes don't return a ProblemDetails object
* Returning null from an action doesn't return a BadRequest ProblemDetails, just a NoContent.
* If you return NotFound("message") it doesn't translate it into a ProblemDetails, you get a plain 404.

## Further reading

[HATEAOS and Ion example Restful API](https://github.com/nbarbettini/BeautifulRestApi)