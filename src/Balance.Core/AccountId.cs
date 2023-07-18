using System.Diagnostics.CodeAnalysis;

namespace BalanceSystem.Core
{
	public class AccountId : IEquatable<AccountId>, IEqualityComparer<AccountId>
	{
		private readonly string _accountId;

        public AccountId(string id)
        {
			_accountId = id ?? throw new ArgumentNullException(nameof(id));
        }

		public bool Equals(AccountId? other)
			=> _accountId.Equals(other?._accountId);

		public bool Equals(AccountId? x, AccountId? y)
			=> x?.Equals(y) == true;

		public int GetHashCode([DisallowNull] AccountId obj)
			=> obj._accountId.GetHashCode();

		public override bool Equals([NotNullWhen(true)] object? obj)
			=> obj is AccountId accountId && accountId?._accountId is not null ? Equals(accountId) : false;

		public override int GetHashCode()
			=> _accountId.GetHashCode();

		public override string ToString()
			=> _accountId;

		public static explicit operator AccountId(string Id)
			=> new AccountId(Id);

		public static explicit operator string(AccountId accountId)
			=> accountId._accountId;

		public static bool operator ==(AccountId accountId1, AccountId accountId2)
			=> accountId1?.Equals(accountId2) == true;

		public static bool operator !=(AccountId accountId1, AccountId accountId2)
			=> !(accountId1 == accountId2);
	}
}