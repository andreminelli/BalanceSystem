using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BalanceSystem.DataAccess.PostgreSql.Configurations
{
	public class EntryEntityTypeConfiguration : IEntityTypeConfiguration<Entry>
	{
		public void Configure(EntityTypeBuilder<Entry> builder)
		{
			builder
				.ToTable("Entries")
				.HasKey(entry => entry.Id);

			builder
				.Property(entry => entry.Id)
				.HasConversion(
					entryId => (Guid)entryId,
					id => new EntryId(id))
				.ValueGeneratedOnAdd().HasValueGenerator<EntryIdValueGenerator>();

			builder
				.Property(entry => entry.Type)
				.HasConversion<string>();

			builder
				.Property(entry => entry.AccountId)
				.HasConversion(
					accountId => (string)accountId,
					id => new AccountId(id));

			builder
				.HasOne(entry => entry.Account);
		}
	}
}
