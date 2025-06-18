namespace Drive.Api.Core.SecretManager;

public static class AmazonSecretsManagerExtensions
{
	public static void AddAmazonSecretsManager(this IConfigurationBuilder configurationBuilder, string region, string secretName)
	{
		var configurationSource = new AmazonSecretsManagerConfigurationSource(region, secretName);
		configurationBuilder.Add(configurationSource);
	}
}