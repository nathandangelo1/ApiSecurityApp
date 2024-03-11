global using Policies = WebApi.Constants.PolicyConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApi.Builders;
//using ApiSecurity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//var securityScheme = new OpenApiSecurityScheme()
//{
//    Name = "Authorization",
//    Description = "JWT Authorization bearer token",
//    In = ParameterLocation.Header,
//    Type = SecuritySchemeType.Http,
//    Scheme = "bearer",
//    BearerFormat = "JWT"
//};

//var securityRequirement = new OpenApiSecurityRequirement
//{
//    {
//        new OpenApiSecurityScheme
//        {
//            Reference = new OpenApiReference
//            {
//                Type = ReferenceType.SecurityScheme,
//                Id = "bearerAuth"
//            }
//        },
//        new string[]{ }
//    }
//};

builder.Services.AddSwaggerGen(options =>
{
    var title = "Our Versioned API";
    var description = "This is a description";
    var terms = new Uri("https://localhost:7276/terms");
    var contact = new OpenApiContact()
    {
        Name = "Helpdesk",
        Email = "help@helpdesk.com",
        Url = new Uri("https://www.helpdesk.com")
    };

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = $"{title} v1",
        Description = description,
        Contact = contact,
        TermsOfService = terms,
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = $"{title} v2",
        Description = description,
        Contact = contact,
        TermsOfService = terms,
    });

    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Description = "JWT Authorization bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            new string[]{ }
        }
    });

});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

Authorization.AddAuthorization(builder);

Authentication.AddAuthentication(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2"); // default version chosen first
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

