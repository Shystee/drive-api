using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using NanoidDotNet;

namespace Drive.Api;

public class NanoIdGenerator : ValueGenerator<string>
{
	public override bool GeneratesTemporaryValues => false;

	public override string Next(EntityEntry entry)
	{
		return Nanoid.Generate("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz", 8);
	}
}