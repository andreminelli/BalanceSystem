namespace BalanceSystem.Core
{
	public struct AccountId : IEquatable<AccountId>
	{
		private readonly string _accountId;

        public AccountId(string id)
        {
			_accountId = id;
        }

		public bool Equals(AccountId other)
			=> _accountId.Equals(other._accountId);

		public static explicit operator AccountId(string Id) 
			=> new AccountId(Id);
	}
}