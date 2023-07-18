using System.Diagnostics.CodeAnalysis;

namespace BalanceSystem.Core
{
	public struct EntryId : IEquatable<EntryId>, IEqualityComparer<EntryId>
	{
		private readonly Guid _entryId;

        public EntryId(Guid id)
		{
			_entryId = id;
		}

		public bool Equals(EntryId other)
			=> _entryId.Equals(other._entryId);

		public bool Equals(EntryId x, EntryId y)
			=> x.Equals(y);

		public int GetHashCode([DisallowNull] EntryId obj)
			=> obj._entryId.GetHashCode();

		public override bool Equals([NotNullWhen(true)] object? obj)
			=> obj is EntryId entryId ? Equals(entryId) : false;

		public override int GetHashCode()
			=> _entryId.GetHashCode();

		public static explicit operator EntryId(Guid Id)
			=> new EntryId(Id);

		public static explicit operator Guid(EntryId entryId)
			=> entryId._entryId;
	}
}