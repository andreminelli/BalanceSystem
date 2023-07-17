using BalanceSystem.Core;
using Microsoft.EntityFrameworkCore;

namespace BalanceSystem.DataAccess.PostgreSql
{
	public class BalanceDbContext : DbContext
	{
		public DbSet<Entry> Entries { get; set; }

		public BalanceDbContext(DbContextOptions<BalanceDbContext> options)
			: base(options)
		{
		}
	}
}
