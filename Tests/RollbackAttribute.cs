using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Tests
{
    /// <summary>
    /// Rollback Attribute wraps test execution into a transaction and cancels the transaction once the test is finished.
    /// You can use this attribute on single test methods or test classes/suites
    /// </summary>
    public class RollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope _transaction;


        public void BeforeTest(ITest test)
        {
            _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        public void AfterTest(ITest test)
        {
            _transaction.Dispose();
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}