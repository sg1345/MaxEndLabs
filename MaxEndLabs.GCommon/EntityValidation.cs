namespace MaxEndLabs.GCommon
{
	public class EntityValidation
	{
		public static class Category
		{
			public const int NameMaxLength = 150;
			public const int SlugMaxLength = 150;
		}

		public static class Product
		{
			public const int NameMaxLength = 200;
			public const int NameMinLength = 3;
			public const int SlugMaxLength = 200;

			public const int DescriptionMaxLength = 500;

			public const string PriceColumnType = "decimal(10, 2)";
			public const double PriceMinValue = 0.01;
			public const double PriceMaxValue = 9_999_999.99;

			public const int CategoryIdMinValue = 1;
			public const int CategoryIdMaxValue = 5;
		}

		public static class ProductVariant
		{
			public const int VariantNameMaxLength = 150;
			public const int VariantNameMinLength = 1;

			public const string PriceColumnType = "decimal(10, 2)";

			public const double PriceMinValue = 0.01;
			public const double PriceMaxValue = 9_999_999.99;
		}

		public static class ShoppingCart
		{
			public const int UserIdMaxLength = 450;
		}

		public static class CartItem
		{
			public const int QuantityMinValue = 1;
			public const int QuantityMaxValue = 99;
			public const int ProductVariantIdMinValue = 1;
		}

		public static class Order
		{
			public const int OrderNumberMaxLength = 19;

			public const int StreetAddressMaxLength = 250;
            public const int StreetAddressMinLength = 3;

            public const int CityMaxLength = 100;
            public const int CityMinLength = 2;

            public const int PostcodeMaxLength = 15;
			public const int PostcodeMinLength = 3;

			public const string TotalAmountColumnType = "decimal(18, 2)";
		}

		public static class OrderItem
		{
			public const string UnitPriceColumnType = "decimal(10, 2)";
			public const string LineTotalColumnType = "decimal(10, 2)";
		}

        public static class ApplicationUser
        {
			public const int FullNameMaxLength = 250;
        }

        public static class NewsArticle
        {
            public const int TeaserTittleMaxLength = 100;
            public const int ContentTitleMaxLength = 200;
            public const int SummaryMaxLength = 500;
        }
	}
}
