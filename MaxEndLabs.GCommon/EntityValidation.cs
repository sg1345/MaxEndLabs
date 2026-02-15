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
            public const int SlugMinLength = 3;

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

        public static class Review
        {
            public const int TitleMaxLength = 150;
            public const int RatingMinValue = 1;
            public const int RatingMaxValue = 5;
        }

        public static class ShoppingCart
        {
            public const int UserIdMaxLength = 450;
        }

    }
}
