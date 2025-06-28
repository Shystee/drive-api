namespace Drive.Api.Domain;

public class Album
{
	private readonly List<File> _files;

	public Album(string title, string createdBy)
	{
		Id = Ulid.NewUlid();
		Title = title;
		CreatedOn = DateTimeOffset.UtcNow;
		CreatedBy = createdBy;
		_files = [];
	}

	public Album(Ulid id, string title, List<File> files, DateTimeOffset createdOn, string createdBy)
	{
		Id = id;
		Title = title;
		_files = files;
		CreatedOn = createdOn;
		CreatedBy = createdBy;
	}

	public string CreatedBy { get; }

	public DateTimeOffset CreatedOn { get; }

	public IReadOnlyList<File> Files => _files.AsReadOnly();

	public Ulid Id { get; }

	public string Title { get; }

	public void AddFile(File file)
	{
		_files.Add(file);
	}
}