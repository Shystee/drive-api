using System.Security.Claims;

namespace Drive.Api;

public static class ClaimsPrincipleExtensions
{
	public static string GetUserId(this ClaimsPrincipal principal)
	{
		return principal.FindFirstValue("sub") ?? throw new InvalidOperationException();
	}
}