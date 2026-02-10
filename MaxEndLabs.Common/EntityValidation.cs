namespace MaxEndLabs.Common
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
            public const int SlugMaxLength = 200;

            public const int DescriptionMaxLength = 500;

            public const string PriceColumnType = "decimal(10, 2)";
        }

        public static class ProductVariant
        {
            public const int VariantNameMaxLength = 150;

            public const string PriceColumnType = "decimal(10, 2)";
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
