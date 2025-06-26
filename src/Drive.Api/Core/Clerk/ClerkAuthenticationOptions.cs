namespace Drive.Api.Core.Clerk;

public sealed class ClerkAuthenticationOptions
{
	public string Authority { get; set; } = null!;

	public string? AuthorizedParty { get; set; }
}