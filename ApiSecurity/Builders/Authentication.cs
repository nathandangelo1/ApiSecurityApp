using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi.Builders
{
    public class Authentication
    {
        public static void AddAuthentication(WebApplicationBuilder builder)
        {
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
        }
    }
}
