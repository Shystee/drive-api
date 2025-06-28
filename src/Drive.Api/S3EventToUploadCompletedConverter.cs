using System.Text.Json;
using System.Text.Json.Serialization;

namespace Drive.Api;

public class S3EventToUploadCompletedConverter : JsonConverter<S3UploadCompleted>
{
	public override S3UploadCompleted? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using var doc = JsonDocument.ParseValue(ref reader);
		var root = doc.RootElement;
		if (root.TryGetProperty("S3Key", out _))
		{
			return JsonSerializer.Deserialize<S3UploadCompleted>(root.GetRawText(), options);
		}

		if (root.TryGetProperty("Records", out var records) && records.GetArrayLength() > 0)
		{
			var @object = records[0].GetProperty("s3").GetProperty("object");
			var key = @object.GetProperty("key").GetString()!;
			var size = @object.GetProperty("size").GetInt32();
			return new S3UploadCompleted(key, size);
		}

		throw new JsonException("Cannot deserialize to S3UploadCompleted");
	}

	public override void Write(Utf8JsonWriter writer, S3UploadCompleted value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, value, options);
	}
}