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
			var account = Fixture.Create<Account>();
			// Arrange
			var creditEntry = CreateRandom(EntryType.Credit);
			var debitEntry = CreateRandom(EntryType.Debit);
			var entries = new[] { creditEntry, creditEntry, debitEntry };
			var startDate = entries.Min(e => e.Date);
			var endDate = entries.Max(e => e.Date);
			EntryRepository
				.GetEntriesAsync(account, startDate, endDate)
				.Returns(entries);

			// Act
			var result = await Target.GetBalanceAsync(account, startDate, endDate);

			// Assert
			result.Amount.ShouldBe(creditEntry.Amount + creditEntry.Amount - debitEntry.Amount);
			result.StartDate.ShouldBe(startDate);
			result.EndDate.ShouldBe(endDate);
		}

		private Entry CreateRandom(EntryType type, Account? account = null)
			=> Fixture
				.Build<Entry>()
				.With(e => e.Account, account ?? ExistingAccount)
				.With(e => e.Type, type)
				.Create();

	}
}