using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace BalanceSystem.DataAccess.PostgreSql.Configurations
{
	public class EntryIdValueGenerator : ValueGenerator<EntryId>
	{
		private static readonly SequentialGuidValueGenerator _guidGenerator = new SequentialGuidValueGenerator();

		public override bool GeneratesTemporaryValues => false;

		public override EntryId Next(EntityEntry entry)
			=> (EntryId)_guidGenerator.Next(entry);
	}
}