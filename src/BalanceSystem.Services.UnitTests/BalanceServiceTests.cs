using AutoFixture;
using BalanceSystem.Core;
using BalanceSystem.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BalanceSystem.Services.UnitTests
{
	[TestClass]
	public class BalanceServiceTests
	{
		private static readonly Fixture Fixture = new Fixture();

		private IAccountRepository AccountRepository;
		private IEntryRepository EntryRepository;
		private Account ExistingAccount;

		private BalanceService Target;

		[TestInitialize]
		public void Setup()
		{
			AccountRepository = Substitute.For<IAccountRepository>();
			EntryRepository = Substitute.For<IEntryRepository>();
			ExistingAccount = Fixture.Create<Account>();
			AccountRepository.GetByIdAsync(ExistingAccount.Id).Returns(ExistingAccount);

			Target = new BalanceService(AccountRepository, EntryRepository);
		}

		[TestMethod]
		public async Task AddEntry_ShouldAddToRepository()
		{
			// Arrange
			var entryToAdd = Fixture.Create<Entry>();

			// Act
			await Target.AddEntryAsync(ExistingAccount, entryToAdd);

			// Assert
			_ = EntryRepository.Received().AddEntryAsync(entryToAdd);
		}

		[TestMethod]
		public async Task GetBalances_ShouldThrowWhenStartDateIsLessThanEndDate()
		{
			// Arrange
			var account = Fixture.Create<Account>();
			var startDate = Fixture.Create<DateTimeOffset>();
			var endDate = startDate.AddDays(-1);

			// Act
			await Should.ThrowAsync<ArgumentException>(async() =>
				await Target.GetBalanceAsync(account, startDate, endDate));
		}

		[TestMethod]
		public async Task GetBalances_ShouldGetEntriesFromRepository()
		{
			// Arrange
			var account = Fixture.Create<Account>();
			var startDate = Fixture.Create<DateTimeOffset>();
			var endDate = startDate.AddDays(2);

			// Act
			_ = await Target.GetBalanceAsync(account, startDate, endDate);

			// Assert
			_ = EntryRepository.Received().GetEntriesAsync(account, startDate, endDate);
		}

		[TestMethod]
		public async Task GetBalances_WithCreditAndDebit_ShouldReturnCorrectBalance()
		{
			// Arrange
			var creditEntry = CreateRandom(EntryType.Credit);
			var debitEntry = CreateRandom(EntryType.Debit);
			var entries = new[] { creditEntry, creditEntry, debitEntry };
			var startDate = entries.Min(e => e.Date);
			var endDate = entries.Max(e => e.Date);
			EntryRepository
				.GetEntriesAsync(ExistingAccount, startDate, endDate)
				.Returns(entries);

			// Act
			var result = await Target.GetBalanceAsync(ExistingAccount, startDate, endDate);

			// Assert
			result.Amount.ShouldBe(creditEntry.Amount + creditEntry.Amount - debitEntry.Amount);
			result.StartDate.ShouldBe(startDate);
			result.EndDate.ShouldBe(endDate);
		}

		[TestMethod]
		public async Task GetDailyBalances_ShouldThrowWhenStartDateIsLessThanEndDate()
		{
			// Arrange
			var startDate = Fixture.Create<DateTimeOffset>();
			var endDate = startDate.AddDays(-1);

			// Act
			await Should.ThrowAsync<ArgumentException>(async () =>
				await Target.GetBalanceAsync(ExistingAccount, startDate, endDate));
		}

		[TestMethod]
		public async Task GetDailyBalances_WithoutPreviousBalance_ShouldStartFromFirstEntryDate()
		{
			// Arrange
			var creditEntry1 = CreateRandom(EntryType.Credit);
			var creditEntry2 = CreateRandom(EntryType.Credit);
			var entries = new[] { creditEntry1, creditEntry2 };

			var minEntryDate = entries.Min(e => e.Date);

			EntryRepository
				.GetEntriesAsync(ExistingAccount, DateTimeOffset.MinValue, DateTimeOffset.MinValue)
				.Returns(Enumerable.Empty<Entry>());

			EntryRepository
				.GetEntriesAsync(ExistingAccount, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
				.Returns(entries);

			// Act
			var result = await Target.GetDailyBalanceAsync(ExistingAccount);

			// Assert
			result.ShouldNotBeNull();
			result.First().Date.ShouldBe(minEntryDate.Date);
		}

		[TestMethod]
		public async Task GetDailyBalances_WithPreviousBalance_ShouldStartFromFirstDayBeforeStartDate()
		{
			// Arrange
			var creditEntry1 = CreateRandom(EntryType.Credit);
			var creditEntry2 = CreateRandom(EntryType.Credit);
			var entries = new[] { creditEntry1, creditEntry2 };

			var minEntryDate = entries.Min(e => e.Date);

			var beforeMinEntryDate = minEntryDate.AddDays(-Fixture.Create<uint>());
			var creditEntryA = CreateRandom(EntryType.Credit, date: beforeMinEntryDate);
			var previousEntries = new[] { creditEntryA };

			EntryRepository
				.GetEntriesAsync(ExistingAccount, Arg.Is<DateTimeOffset>(arg => arg == DateTimeOffset.MinValue), Arg.Is<DateTimeOffset>(arg => arg < minEntryDate))
				.Returns(previousEntries);

			EntryRepository
				.GetEntriesAsync(ExistingAccount, minEntryDate, Arg.Any<DateTimeOffset>())
				.Returns(entries);

			// Act
			var result = await Target.GetDailyBalanceAsync(ExistingAccount, startDate: minEntryDate);

			// Assert
			result.ShouldNotBeNull();
			result.First().Date.ShouldBe(minEntryDate.Date.AddDays(-1));
			result.First().Amount.ShouldBe(creditEntryA.Amount);
		}

		private Entry CreateRandom(EntryType type, Account? account = null, DateTimeOffset? date = null)
			=> Fixture
				.Build<Entry>()
				.With(e => e.Account, account ?? ExistingAccount)
				.With(e => e.Type, type)
				.With(e => e.Date, date ?? Fixture.Create<DateTimeOffset>())
				.Create();

	}
}