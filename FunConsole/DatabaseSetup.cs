using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;

namespace FunConsole
{
    public class DatabaseSetup
    {
        public string GetDatabaseConnectionString()
         => ConfigurationManager.AppSettings["ConnectionString"];

        public void Wipe_And_Create_Database()
        {
            var connectionString = GetDatabaseConnectionString();

            // drop database first
            ReallyDropDatabase(connectionString);

            // Now time to create the database from migrations
            // MyApp.Data.Migrations.Configuration is migration configuration class 
            // this class is crated for you automatically when you enable migrations
            var initializer = new MigrateDatabaseToLatestVersion<IntegrationTestFun.Data.FunContext, IntegrationTestFun.Data.Migrations.Configuration>();

            // set DB initialiser to execute migrations
            Database.SetInitializer(initializer);

            // now actually force the initialisation to happen
            using (var domainContext = new IntegrationTestFun.Data.FunContext(connectionString))
            {
                Console.WriteLine("Starting creating database");
                domainContext.Database.Initialize(true);
                Console.WriteLine("Database is created");
            }

            // And after the DB is created, you can put some initial base data 
            // for your tests to use
            // usually this data represents lookup tables, like Currencies, Countries, Units of Measure, etc
            using (var domainContext = new IntegrationTestFun.Data.FunContext(connectionString))
            {
                Console.WriteLine("Seeding test data into database");
                // discussion for that to follow
                SeedContextForTests.Seed(domainContext);
                Console.WriteLine("Seeding test data is complete");
            }
        }

        public void Update_Database()
        {
            var connectionString = GetDatabaseConnectionString();

            var migrationConfiguration = new IntegrationTestFun.Data.Migrations.Configuration();

            migrationConfiguration.TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient");

            var migrator = new DbMigrator(migrationConfiguration);

            migrator.Update();
        }

        /// <summary>
        /// Drops the database that is specified in the connection string.
        /// 
        /// Drops the database even if the connection is open. Sql is stolen from here:
        /// http://daniel.wertheim.se/2012/12/02/entity-framework-really-do-drop-create-database-if-model-changes-and-db-is-in-use/
        /// </summary>
        /// <param name="connectionString"></param>
        private static void ReallyDropDatabase(String connectionString)
        {
            const string DropDatabaseSql =
                "if (select DB_ID('{0}')) is not null\r\n"
                + "begin\r\n"
                + "alter database [{0}] set offline with rollback immediate;\r\n"
                + "alter database [{0}] set online;\r\n"
                + "drop database [{0}];\r\n"
                + "end";

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var sqlToExecute = String.Format(DropDatabaseSql, connection.Database);

                    var command = new SqlCommand(sqlToExecute, connection);

                    Console.WriteLine("Dropping database");
                    command.ExecuteNonQuery();
                    Console.WriteLine("Database is dropped");
                }
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Message.StartsWith("Cannot open database"))
                {
                    Console.WriteLine("Database does not exist.");
                    return;
                }
                throw;
            }
        }
    }

    public class SeedContextForTests
    {
        public static void Seed(IntegrationTestFun.Data.FunContext domainContext)
        {


        }
    }
}
