using System.Diagnostics.CodeAnalysis;

namespace BalanceSystem.Core
{
	public struct AccountId : IEquatable<AccountId>, IEqualityComparer<AccountId>
	{
		private readonly string _accountId;

        public AccountId(string id)
        {
			_accountId = id;
        }

		public bool Equals(AccountId other)
			=> _accountId.Equals(other._accountId);

		public bool Equals(AccountId x, AccountId y)
			=> x.Equals(y);

		public int GetHashCode([DisallowNull] AccountId obj)
			=> obj._accountId.GetHashCode();

		public static explicit operator AccountId(string Id)
			=> new AccountId(Id);

		public static explicit operator string(AccountId accountId)
			=> accountId._accountId;
	}
}