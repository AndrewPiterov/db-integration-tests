using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using FluentAssertions;
using IntegrationTestFun.Data;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class PersonTest : EntityFrameworkIntegrationTest
    {
        private Person p;
        private string _first;
        private string _last;

        [SetUp]
        public void Setup()
        {

            _first = "Andrey";
            _last = "Piterov";
            p = new Person
            {
                FirstName = _first,
                LastName = _last
            };

            DbContext.Persons.Add(p);
            DbContext.SaveChanges();

            DbContext.Entry(p).Reload();

        }
        [Test]
        public void AllRequired_SavePerson()
        {
            var p = DbContext.Persons.Find(this.p.PersonId);
            p.LastName.Should().Be(_last);
        }

        [Test]
        public void AllRequired2_SavePerson()
        {
            var p = DbContext.Persons.Find(this.p.PersonId);
            p.LastName.Should().Be(_last);
        }

    }

    [TestFixture]
    public class InvalidPersonTests : EntityFrameworkIntegrationTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void DuplicateFirstLastName_ShouldThrowException()
        {
            var first = "Andrey";
            var last = "Piterov";

            var person1 = new Person
            {
                FirstName = first,
                LastName = last
            };

            DbContext.Persons.Add(person1);
            DbContext.SaveChanges();

            var cid = person1.PersonId;

            var person2 = new Person
            {
                FirstName = first,
                LastName = last
            };

            DbContext.Persons.Add(person2);

            var ex = Assert.Throws<DbUpdateException>(() => DbContext.SaveChanges());
            var inner = GetInnerException(ex);
            inner.Message.Should().Contain("duplicate key");
        }

        private static Exception GetInnerException(Exception ex)
        {
            return ex.InnerException == null ? ex : GetInnerException(ex.InnerException);
        }

        [Test]
        public void NoFirst_ShouldThrowException()
        {
            DbContext.Persons.Add(new Person());
            Action act = () => DbContext.SaveChanges();
            act.ShouldThrow<DbEntityValidationException>();
        }
    }
}
