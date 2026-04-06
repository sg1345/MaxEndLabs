using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using MaxEndLabs.Data.Models.Enum;

using static MaxEndLabs.GCommon.EntityValidation.Order;

namespace MaxEndLabs.Data.Models
{
	public class Order
	{
		[Key]
		public Guid Id { get; set; }

		[ForeignKey(nameof(User))]
		public Guid UserId { get; set; }
		public virtual ApplicationUser User { get; set; } = null!;

		[Required]
		[MaxLength(OrderNumberMaxLength)]
		public string OrderNumber { get; set; } = null!;

		[Required]
		[MaxLength(StreetAddressMaxLength)]
		public string StreetAddress { get; set; } = null!;

		[Required]
		[MaxLength(CityMaxLength)]
		public string City { get; set; } = null!;

		[Required]
		[MaxLength(PostcodeMaxLength)]
		public string Postcode { get; set; } = null!;

		[Required]
		public OrderStatus Status { get; set; }

		[Required]
		//[Column(TypeName = TotalAmountColumnType)]
		public decimal TotalAmount { get; set; }

		[Required]
		public DateTime CreatedAt { get; set; }

		[Required]
		public DateTime UpdatedAt { get; set; }

		public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
	}
}
