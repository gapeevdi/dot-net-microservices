
namespace Basket.API
{
    public static class Constants
    {
        public static class Redis
        {

            public static class Environment
            {
                public const string Host = "REDIS_HOST";
                public const string Port = "REDIS_PORT";
            }

            public const string DefaultHost = "localhost";
            public const int DefaultPort = 6379;
        }
    }
}
