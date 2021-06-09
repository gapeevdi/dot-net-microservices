namespace Discount.API
{
    public static class Constants
    {
        public static class Postgres
        {

            public static class Environment
            {
                public const string Host = "POSTGRES_HOST";
                public const string Port = "POSTGRES_PORT";
                public const string Database = "POSTGRES_DATABASE";
                public const string User = "POSTGRES_USER";
                public const string Password = "POSTGRES_PASSWORD";
            }

            public const string DefaultHost = "localhost";
            public const int DefaultPort = 6379;
        }
    }
}
