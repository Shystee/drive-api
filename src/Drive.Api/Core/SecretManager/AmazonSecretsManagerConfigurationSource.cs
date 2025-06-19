namespace Drive.Api.Core.SecretManager;

public class AmazonSecretsManagerConfigurationSource(string region, string secretName) : IConfigurationSource
{
	public IConfigurationProvider Build(IConfigurationBuilder builder) =>
		new AmazonSecretsManagerConfigurationProvider(region, secretName);
}