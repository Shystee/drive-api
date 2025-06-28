using Drive.Api.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wolverine.Http;

namespace Drive.Api;

public sealed record FileResponse(Ulid Id, string Name, string FileHref);

public sealed record AlbumResponse(Ulid Id, string Title, List<FileResponse> Files);

public class GetAlbumsEndpoint
{
	[WolverineGet("/api/albums"), Authorize, Tags("Drive")]
	public static async Task<List<AlbumResponse>> GetAlbums(ApplicationDbContext context, ClaimsPrincipal user)
	{
		var userId = user.GetUserId();
		var albums = await context.Albums.Include(x => x.Files)
			.Where(x => x.CreatedBy == userId)
			.ToListAsync();

		return albums.Select(a => new AlbumResponse(
				a.Id,
				a.Title,
				a.Files.Select(f => new FileResponse(f.Id, f.Name, f.S3Key)).ToList()
			))
			.ToList();
	}
}