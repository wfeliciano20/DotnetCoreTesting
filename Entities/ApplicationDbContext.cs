using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Person> People { get; private set; }

        public virtual DbSet<Country> Countries { get; private set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("countries");
            modelBuilder.Entity<Person>().ToTable("people");

            string countriesJson = System.IO.File.ReadAllText("countries.json");

            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            if (countries != null)
            {
                foreach (var country in countries)
                {
                    modelBuilder.Entity<Country>().HasData(country);
                }
            }
            else
            {
                Console.WriteLine("Error: Seeding Country Data");
            }

            string peopleJson = System.IO.File.ReadAllText("people.json");

            List<Person>? people = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(peopleJson);


            if (people != null)
            {
                foreach (var person in people)
                {
                    modelBuilder.Entity<Person>().HasData(person);
                }
            }
            else
            {
                Console.WriteLine("Error: Seeding People Data");
            }
        }

        public List<Person> sp_GetALLPeople()
        {
            return People.FromSqlRaw("EXEC [dbo].[GetAllPeople]").ToList();
        }

        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@PersonID", person.PersonID),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryID", person.CountryID),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveNewsLetter", person.ReceiveNewsLetter),
            };
            return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetter", parameters);
        }
    }
}