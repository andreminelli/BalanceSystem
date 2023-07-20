using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace BalanceSystem.Core.UnitTests
{
	[TestClass]
	public class EntryTests
	{
		[TestMethod]
		public void Amount_ShouldBePositive()
		{
			Should.Throw<ArgumentException>(() => new Entry { Amount = -1 });
		}

		[TestMethod]
		[DataRow(-10, EntryType.Credit, 25, 15)]
		[DataRow(0, EntryType.Credit, 25, 25)]
		[DataRow(500, EntryType.Credit, 25, 525)]
		[DataRow(-10, EntryType.Debit, 25, -35)]
		[DataRow(0, EntryType.Debit, 25, -25)]
		[DataRow(500, EntryType.Debit, 25, 475)]
		public void SumTo_ShouldProduceCorrectAmount(double initialAmount, EntryType entryType, double entryAmount, double finalAmount)
		{
			// Arrange
			var target = new Entry
			{
				Amount = Convert.ToDecimal(entryAmount),
				Date = DateTimeOffset.Now,
				Type = entryType,
				Description = "Some info"
			};

			// Act
			var amount = target.SumTo(Convert.ToDecimal(initialAmount));

			// Assert
			amount.ShouldBe(Convert.ToDecimal(finalAmount));
		}
	}
}