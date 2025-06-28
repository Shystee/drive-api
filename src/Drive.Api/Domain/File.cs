namespace Drive.Api.Domain;

public class File
{
	public File(Ulid id, string name, string s3Key, string contentType, long size)
	{
		Id = id;
		Name = name;
		S3Key = s3Key;
		ContentType = contentType;
		Size = size;
	}

	public string ContentType { get; }

	public Ulid Id { get; }

	public string Name { get; }

	public string S3Key { get; }

	public long Size { get; }
}