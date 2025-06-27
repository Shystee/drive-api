using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Drive.Api.Persistence.Converters;

public class UlidToBytesConverter(ConverterMappingHints? mappingHints) : ValueConverter<Ulid, byte[]>(x => x.ToByteArray(),
	x => new Ulid(x),
	DefaultHints.With(mappingHints))
{
	private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(16);

	public UlidToBytesConverter()
		: this(null)
	{
	}
}

public class UlidToStringConverter(ConverterMappingHints? mappingHints) : ValueConverter<Ulid, string>(x => x.ToString(),
	x => Ulid.Parse(x),
	DefaultHints.With(mappingHints))
{
	private static readonly ConverterMappingHints DefaultHints = new ConverterMappingHints(26);

	public UlidToStringConverter()
		: this(null)
	{
	}
}