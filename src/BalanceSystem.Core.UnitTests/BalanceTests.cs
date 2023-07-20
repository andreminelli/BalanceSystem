using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Linq;

namespace BalanceSystem.Core.UnitTests
{
	[TestClass]
	public class BalanceTests
	{
		private static readonly Fixture Fixture = new Fixture();

		[TestMethod]
		public void Create_WithNullEntries_ShouldBeSuccessfull()
		{
			// Arrange
			// Act
			var target = new Balance(default);

			// Assert
			target.Amount.ShouldBe(0);
		}

		[TestMethod]
		public void Create_WithEmpyEntries_ShouldBeSuccessfull()
		{
			// Arrange
			// Act
			var target = new Balance(Enumerable.Empty<Entry>());

			// Assert
			target.Amount.ShouldBe(0);
		}

		[TestMethod]
		public void Create_WithSingleCredit_ShouldReturnPositiveBalance()
		{
			// Arrange
			var creditEntry = CreateRandom(EntryType.Credit);
			var entries = new [] { creditEntry };

			// Act
			var target = new Balance(entries);

			// Assert
			target.Amount.ShouldBe(creditEntry.Amount);
			target.StartDate.ShouldBe(creditEntry.Date);
			target.EndDate.ShouldBe(creditEntry.Date);
		}

		[TestMethod]
		public void Create_WithSingleDebit_ShouldReturnNegativeBalance()
		{
			// Arrange
			var debitEntry = CreateRandom(EntryType.Debit);
			var entries = new [] { debitEntry };

			// Act
			var target = new Balance(entries);

			// Assert
			target.Amount.ShouldBe(-debitEntry.Amount);
			target.StartDate.ShouldBe(debitEntry.Date);
			target.EndDate.ShouldBe(debitEntry.Date);
		}

		[TestMethod]
		public void Create_WithCreditAndDebit_ShouldReturnTheDifference()
		{
			// Arrange
			var creditEntry = CreateRandom(EntryType.Credit);
			var debitEntry = CreateRandom(EntryType.Debit);
			var entries = new [] { creditEntry, debitEntry };

			// Act
			var target = new Balance(entries);

			// Assert
			target.Amount.ShouldBe(creditEntry.Amount - debitEntry.Amount);
			target.StartDate.ShouldBe(entries.Min(e => e.Date));
			target.EndDate.ShouldBe(entries.Max(e => e.Date));
		}

		private static Entry CreateRandom(EntryType type)
			=> Fixture.Build<Entry>().With(e => e.Type, type).Create();
	}
}