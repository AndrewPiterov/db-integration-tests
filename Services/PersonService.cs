using IntegrationTestFun.Data;

namespace Services
{
   public class PersonService
    {
       private readonly FunContext _context;

       public PersonService(FunContext context)
       {
           _context = context;
       }

        public void CreatePerson(string firstName, string lastName)
        {
            var newPerson = new Person();
            newPerson.FirstName = firstName;
            
            _context.Persons.Add(newPerson);
            _context.SaveChanges();
        }
    }
}
