using System.Diagnostics.CodeAnalysis;

namespace BalanceSystem.Core
{
	public class Account : IEquatable<Account>, IEqualityComparer<Account>
	{
		public AccountId Id { get; init; }
		public string Name { get; init; }

		public bool Equals(Account? other) 
			=> Id.Equals(other?.Id);

		public bool Equals(Account? x, Account? y)
			=> x?.Equals(y) == true;

		public int GetHashCode([DisallowNull] Account obj)
			=> obj.Id.GetHashCode();
	}
}
