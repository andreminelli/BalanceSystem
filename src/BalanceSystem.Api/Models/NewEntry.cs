using BalanceSystem.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BalanceSystem.Api.Models
{
	/// <summary>
	/// Define as propriedade de um novo lançamento 
	/// </summary>
	public class NewEntry
	{
		/// <summary>
		/// A data do lançamento
		/// </summary>
		[Required]
		public DateTimeOffset Date { get; init; }
		
		/// <summary>
		/// O tipo do lançamento (crédito ou débito)
		/// </summary>
		[Required]
		public EntryType Type { get; init; }

		/// <summary>
		/// Uma breve descrição para o lançamento
		/// </summary>
		[DefaultValue("")]
		public string Description { get; init; }
		
		/// <summary>
		/// O valor (absoluto) do lançamento
		/// </summary>
		[Required]
		[Range(0.0, 1_000_000_000_000)]
		public decimal Amount { get; init; }

		public Entry ToEntry()
		{
			return new Entry
			{
				Date = Date,
				Type = Type,
				Description = Description,
				Amount = Amount
			};
		}
	}
}
