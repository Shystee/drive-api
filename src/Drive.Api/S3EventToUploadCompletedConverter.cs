using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drive.Api;

public class S3EventToUploadCompletedConverter : JsonConverter<S3UploadCompleted>
{
	public override S3UploadCompleted? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var doc = JsonDocument.ParseValue(ref reader);
		var root = doc.RootElement;

		// Handle both single S3UploadCompleted and S3 event notification
		if (root.TryGetProperty("S3Key", out _))
		{
			// Direct S3UploadCompleted message
			return JsonSerializer.Deserialize<S3UploadCompleted>(root.GetRawText(), options);
		}

		// S3 Event Notification - extract the key
		if (root.TryGetProperty("Records", out var records) && records.GetArrayLength() > 0)
		{
			var key = records[0]
				.GetProperty("s3")
				.GetProperty("object")
				.GetProperty("key")
				.GetString();

			return new S3UploadCompleted(key!);
		}

		throw new JsonException("Cannot deserialize to S3UploadCompleted");
	}

	public override void Write(Utf8JsonWriter writer, S3UploadCompleted value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, value, options);
	}
}