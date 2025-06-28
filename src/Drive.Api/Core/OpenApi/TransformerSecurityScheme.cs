using Drive.Api.Core.Clerk;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Drive.Api.Core.OpenApi;

public class TransformerSecurityScheme(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
	public async Task TransformAsync(
		OpenApiDocument document,
		OpenApiDocumentTransformerContext context,
		CancellationToken cancellationToken)
	{
		document.Components ??= new OpenApiComponents();
		document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();
		var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();
		foreach (var authenticationScheme in authenticationSchemes)
		{
			if (authenticationScheme.Name != ClerkAuthenticationDefaults.AuthenticationScheme) continue;
			
			var scheme = new OpenApiSecurityScheme
			{
				Description = "Bearer scheme; see https://learn.openapis.org/specification/security.html#http-authentication",
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				In = ParameterLocation.Header,
				BearerFormat = "JWT",
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = JwtBearerDefaults.AuthenticationScheme
				}
			};

			document.Components.SecuritySchemes[scheme.Reference.Id] = scheme;
			document.SecurityRequirements.Add(new OpenApiSecurityRequirement
			{
				{
					scheme, []
				}
			});
		}
	}
}