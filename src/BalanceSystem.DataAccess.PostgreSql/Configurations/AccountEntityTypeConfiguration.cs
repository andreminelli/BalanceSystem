using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BalanceSystem.DataAccess.PostgreSql.Configurations
{
	public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
	{
		public void Configure(EntityTypeBuilder<Account> builder)
		{
			builder
				.ToTable("Accounts")
				.HasKey(account => account.Id);

			builder
				.Property(account => account.Id)
				.HasConversion(
					accountId => (string)accountId,
					id => new AccountId(id))
				.ValueGeneratedNever();
		}
	}
}
