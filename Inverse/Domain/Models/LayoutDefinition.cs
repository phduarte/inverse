namespace Inverse.Domain.Models
{
    public static class LayoutDefinition
    {
        public static class Tables
        {
            public const int WIDTH = 100;
            public const int MARGIN = 50;
        }

        public static class Columns
        {
            public const int HEIGHT = 30;
            public const int TITLE_MARGIN = 30;
            public const string PRIMARY_KEY_PREFIX = "PK";
            public const string FOREIGN_KEY_PREFIX = "FK";
            public const int PREFIX_WIDTH = 40;
            public const int TYPE_WIDTH = 90;
        }

        public static class Chars
        {
            public const int WIDTH = 9;
        }
    }
}
