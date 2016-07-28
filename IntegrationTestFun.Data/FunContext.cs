using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;

namespace IntegrationTestFun.Data
{
    public class FunContext : DbContext
    {
        public FunContext():base("DefaultConnection")
        {

        }

        public FunContext(string conn) : base(conn)
        {

        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().HasKey(x => x.PersonId);

            modelBuilder
                .Entity<Person>().Property(x => x.PersonId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder
                .Entity<Person>()
                .Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_FirstNameLastName", 1) { IsUnique = true }));

            modelBuilder
                .Entity<Person>()
                .Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(60)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_FirstNameLastName", 2) { IsUnique = true }));
        }
    }
}
