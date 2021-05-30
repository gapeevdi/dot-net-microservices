
namespace Catalog.API
{
    public static class Constants
    {
        public static class Mongo
        {
            public static class Environment
            {
                public const string Host = "MONGO_HOST";
                public const string Port = "MONGO_PORT";
                public const string ProductDatabase = "MONGO_DATABASE";
                public const string ProductCollection = "MONGO_COLLECTION";
            }

            public const string DefaultHost = "localhost";
            public const int DefaultPort = 27017;
        }
    }
}
