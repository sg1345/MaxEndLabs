using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using static MaxEndLabs.GCommon.EntityValidation.OrderItem;

namespace MaxEndLabs.Data.Models
{
	public class OrderItem
	{
		[ForeignKey(nameof(Order))]
		public int OrderId { get; set; }
		public virtual Order Order { get; set; } = null!;

		[ForeignKey(nameof(Product))]
		public int ProductId { get; set; }
		public virtual Product Product { get; set; } = null!;

		[ForeignKey(nameof(ProductVariant))]
		public int ProductVariantId { get; set; }

		public virtual ProductVariant ProductVariant { get; set; } = null!;

		[Required]
		public int Quantity { get; set; }

		[Required]
		[Column(TypeName = UnitPriceColumnType)]
		public decimal UnitPrice { get; set; }

		[Required]
		[Column(TypeName = LineTotalColumnType)]
		public decimal LineTotal { get; set; }
	}
}
