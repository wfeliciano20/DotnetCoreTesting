using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class PeopleDbContext : DbContext
    {

        public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options)
        {

        }
        public DbSet<Person> People { get; private set; }

        public DbSet<Country> Countries { get; private set; }

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
            return People.FromSqlRaw("EXECUTE [dbo].[GetAllPeople]").ToList();
        }
    }
}