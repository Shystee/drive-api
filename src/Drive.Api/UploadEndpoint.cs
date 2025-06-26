using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using NanoidDotNet;
using Wolverine.Http;

namespace Drive.Api;

public record InitializeUploadRequest(string FileName);

public record InitializeUploadResponse(string UploadUrl, string FileUri);

public class UploadEndpoint
{
	private static readonly FileExtensionContentTypeProvider Provider = new();

	[Authorize]
	[WolverinePost("/api/file")]
	public static async Task<(InitializeUploadResponse, StartUpload)> InitializeUpload(
		InitializeUploadRequest request,
		IAmazonS3 s3Service)
	{
		var extension = Path.GetExtension(request.FileName);
		var contentType = Provider.Mappings[extension];
		var validFor = TimeSpan.FromMinutes(30);
		var initiatedDate = DateTime.UtcNow;
		var expirationDate = initiatedDate.Add(validFor);
		var fileId = await Nanoid.GenerateAsync("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 8);
		var s3Key = $"files/{fileId}{extension}";
		var fileUri = $"https://d2znyj7nafxno9.cloudfront.net/{s3Key}";
		var presignedUrl = await s3Service.GetPreSignedURLAsync(new GetPreSignedUrlRequest
		{
			BucketName = "drive-api-test-bucket",
			Key = s3Key,
			Verb = HttpVerb.PUT,
			Expires = expirationDate,
			ContentType = contentType,
		});

		return (
			new InitializeUploadResponse(presignedUrl, fileUri),
			new StartUpload(fileId, s3Key, request.FileName, contentType, initiatedDate, validFor)
		);
	}
}