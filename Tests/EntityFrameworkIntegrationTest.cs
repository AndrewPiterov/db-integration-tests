using System.Transactions;
using IntegrationTestFun.Data;
using NUnit.Framework;

namespace Tests
{
    public abstract class EntityFrameworkIntegrationTest
    {
        protected FunContext DbContext;

        protected TransactionScope TransactionScope;

        [SetUp]
        public void TestSetup()
        {
            DbContext = new FunContext(new StubConfiguration().GetDatabaseConnectionString());
            //DbContext = new FunContext(TestInit.TestDatabaseName);
            //DbContext.Database.CreateIfNotExists();
            TransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        [TearDown]
        public void TestCleanup()
        {
            TransactionScope.Dispose();
        }
    }
}