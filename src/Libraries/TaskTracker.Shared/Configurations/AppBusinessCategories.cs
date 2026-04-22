namespace TaskTracker.Shared.Configurations
{
    public static class AppBusinessCategoryType
    {
        public readonly static int ProductUnitType = 301;
        public readonly static int ProductType = 302;
        public readonly static int Month = 304;
    }
    public static class AppBusinessCategories
    {
        public static class ProductUnitType
        {
            public readonly static int Piece = 30101;
            public readonly static int Kg = 30102;
            public readonly static int Litre = 30103;
        }

        public static class ProductType
        {
            public readonly static int Food = 30201;
            public readonly static int Drink = 30202;
        }
        public static class Month
        {
            public readonly static int January = 30401;
            public readonly static int February = 30402;
            public readonly static int March = 30403;
            public readonly static int April = 30404;
            public readonly static int May = 30405;
            public readonly static int June = 30406;
            public readonly static int July = 30407;
            public readonly static int August = 30408;
            public readonly static int September = 30409;
            public readonly static int October = 30410;
            public readonly static int November = 30411;
            public readonly static int December = 30412;
        }

        public static class WeekDay
        {

            public readonly static int Sunday = 21001;
            public readonly static int Monday = 21002;
            public readonly static int Tuesday = 21003;
            public readonly static int Wednessday = 21004;
            public readonly static int Thursday = 21005;
            public readonly static int Friday = 21006;
            public readonly static int Saturday = 21007;
        }
    }

    //if (item.ProductType == ProductType.Drink) i have to use like this
}
