namespace Drive.Api.Domain;

public class Album
{
	public Album(string title, string createdBy)
	{
		Id = Ulid.NewUlid();
		Title = title;
		CreatedOn = DateTimeOffset.UtcNow;
		CreatedBy = createdBy;
	}

	public Album(Ulid id, string title, DateTimeOffset createdOn, string createdBy)
	{
		Id = id;
		Title = title;
		CreatedOn = createdOn;
		CreatedBy = createdBy;
	}

	public DateTimeOffset CreatedOn { get; }

	public Ulid Id { get; }

	public string Title { get; }
	
	public string CreatedBy { get; }
}