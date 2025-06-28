using Drive.Api.Domain;
using Drive.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Wolverine.Http;

namespace Drive.Api;

public class GetAlbumEndpoint
{
	[WolverineGet("/api/albums/{id}"), Tags("Drive")]
	public static AlbumResponse GetAlbum(Album album)
	{
		return new AlbumResponse(
			album.Id,
			album.Title,
			album.Files.Select(f => new FileResponse(f.Id, f.Name, f.S3Key)).ToList()
		);
	}

	public static async Task<(Album? album, IResult result)> LoadAsync(string id, ApplicationDbContext context)
	{
		if (!Ulid.TryParse(id, out var albumId))
		{
			return (null, Results.NotFound());
		}

		var album = await context.Albums.Include(x => x.Files)
			.FirstOrDefaultAsync(x => x.Id == albumId);

		return album is null
			? (album, Results.NotFound())
			: (album, WolverineContinue.Result());
	}
}