using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Drive.Api.Core.Clerk;

public static class ClerkAuthenticationExtensions
{
	public static AuthenticationBuilder AddClerkAuthentication(
		this AuthenticationBuilder builder,
		string authenticationScheme,
		Action<ClerkAuthenticationOptions> configureOptions)
	{
		var optionsObj = new ClerkAuthenticationOptions();
		configureOptions(optionsObj);

		if (string.IsNullOrEmpty(optionsObj.Authority))
		{
			throw new InvalidOperationException("Clerk Authority cannot be empty or null");
		}

		return builder.AddJwtBearer(authenticationScheme, x =>
		{
			x.MapInboundClaims = false;
			x.Authority = optionsObj.Authority;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = optionsObj.Authority,
				ValidateAudience = false,
				ValidateLifetime = true,
				RequireExpirationTime = true,
				ClockSkew = TimeSpan.FromMinutes(1),
				ValidateIssuerSigningKey = true,
				NameClaimType = ClaimTypes.NameIdentifier
			};

			x.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					context.Token = context.Request.Cookies["__session"];
					return Task.CompletedTask;
				},
				OnTokenValidated = context =>
				{
					var azp = context.Principal?.FindFirstValue("azp");
					if (!string.IsNullOrEmpty(azp) && !azp.Equals(optionsObj.AuthorizedParty))
					{
						context.Fail("AZP Claim is invalid or missing");
					}

					return Task.CompletedTask;
				},
				OnAuthenticationFailed = context =>
				{
					if (context.Exception is SecurityTokenExpiredException)
					{
						context.Response.Headers.Append("Token-Expired", "true");
					}

					return Task.CompletedTask;
				}
			};
		});
	}
}