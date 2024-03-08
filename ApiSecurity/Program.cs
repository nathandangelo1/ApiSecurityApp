global using Policies = ApiSecurity.Constants.PolicyConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.MustHaveEmployeeId, policy =>
    {
        policy.RequireClaim("employeeId");
    });
    options.AddPolicy(Policies.MustBeTheOwner, policy =>
    {
        //policy.RequireUserName("ndangelo");
        policy.RequireClaim("title", "BusinessOwner");
    });
    options.AddPolicy(Policies.MustBeAVeteranEmployee, policy =>
    {
        //policy.RequireUserName("ndangelo");
        policy.RequireClaim("employeeId", "E001", "E002", "E003");
    });
    // Sets Fallback policy, applies to all endpoints unless specific rules overwrite it (such as [AllowAnonymous] on api/Authentication/token endpoint to allow users access to authenticate
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opts =>
    {
        opts.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                builder.Configuration.GetValue<string>("Authentication:SecretKey")))

        };
    });
    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
