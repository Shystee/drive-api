namespace Drive.Api.Domain;

public class File
{
	public File(string id, string originalFileName, string s3Key, string contentType, long sizeInBytes)
	{
		Id = id;
		OriginalFileName = originalFileName;
		S3Key = s3Key;
		ContentType = contentType;
		SizeInBytes = sizeInBytes;
	}

	public string ContentType { get; }

	public string Id { get; }

	public string OriginalFileName { get; }

	public string S3Key { get; }

	public long SizeInBytes { get; }
}