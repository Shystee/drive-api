using Drive.Api.Domain;
using Microsoft.AspNetCore.Authorization;
using Wolverine.Http;

namespace Drive.Api;

public sealed record CreateAlbumRequest(string Title);

public record CreateAlbumResponse(Ulid Id) : CreationResponse("/api/albums/" + Id);

public class CreateAlbumEndpoint
{
	[Authorize]
	[WolverinePost("/api/album")]
	public static CreateAlbumResponse CreateAlbum(CreateAlbumRequest request, ApplicationDbContext context, HttpContext httpContext)
	{
		var userId = httpContext.User.GetUserId();
		var album = new Album(request.Title, userId);
		context.Albums.Add(album);
		return new CreateAlbumResponse(album.Id);
	}
}