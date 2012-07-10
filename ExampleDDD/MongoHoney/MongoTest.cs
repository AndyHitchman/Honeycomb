using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace MongoHoney
{
    public class MongoTest
    {
        private string connectionString = "mongodb://localhost/?safe=true";
        private string databaseName = "test";
        protected MongoServer server;
        protected MongoDatabase database;

        public MongoTest()
        {
            server = MongoServer.Create(connectionString);
            database = server.GetDatabase(databaseName);


        }
    }
}
