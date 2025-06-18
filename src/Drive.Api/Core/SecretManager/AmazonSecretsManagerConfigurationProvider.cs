using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System.Text;
using System.Text.Json;

namespace Drive.Api.Core.SecretManager;

public class AmazonSecretsManagerConfigurationProvider(string region, string secretName) : ConfigurationProvider
{
	public override void Load()
	{
		var secret = GetSecret();
		Data = JsonSerializer.Deserialize<Dictionary<string, string?>>(secret) ?? throw new NullReferenceException("Secret not found");
	}

	private string GetSecret()
	{
		var request = new GetSecretValueRequest
		{
			SecretId = secretName,
			VersionStage = "AWSCURRENT"
		};

		using var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
		var response = client.GetSecretValueAsync(request).Result;
		if (response.SecretString != null)
		{
			return response.SecretString;
		}

		var memoryStream = response.SecretBinary;
		var reader = new StreamReader(memoryStream);
		return Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
	}
}