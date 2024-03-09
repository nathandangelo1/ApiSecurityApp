using Microsoft.AspNetCore.Authorization;

namespace WebApi.Builders;

public class Authorization
{
    public static void AddAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.MustHaveEmployeeId, policy =>
            {
                //policy.RequireAuthenticatedUser() only in Fallback policy bc in order to get a claim you must get a token, you
                // cant get a token unless you have properly authenticated. RequireAuthenticatedUser() therefore only in fallback policy
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
    }
}
