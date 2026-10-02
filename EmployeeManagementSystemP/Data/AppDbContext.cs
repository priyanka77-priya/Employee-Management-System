using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystemP.Models;

namespace EmployeeManagementSystemP.Data
{
    public class AppDbContext : DbContext
    {
        //constructor to accept the options
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<EmployeeLanguage> EmployeesLanguages { get; set; }
        public DbSet<State> states { get; set; }
        public DbSet<Language> Languages { get; set; }

        /*
         onmodelcreating is used to specify the Ef how to create database by  manually assigning it constrins
         here we are overrdiing the EF OnModelCreation Method
        We are setting it as protected bcz only with in the dbcontext class or class inherting it can change it 
        so that the actual flow of EF is not interrupted.
       Modelbuikder is the method defined by EF to define table , set relations, set primary and foreign key
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            HasKey - primary key
            Hasone - one in one to many
            /WithMany - many
            Hasforignkey - used to set the foreign key
            Entity is the table
            */
            
            //these are all fluent Api
            //we can add the indexing manually for other properties using these fluent api 
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Country)
        .WithMany(c => c.Employees)
        .HasForeignKey(e => e.CountryId)
        .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.State)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.StateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.City)
                .WithMany(c => c.Employees)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite key for EmployeeLanguage
            modelBuilder.Entity<EmployeeLanguage>()
             .HasKey(el => new { el.EmployeeId, el.LanguageId });

            modelBuilder.Entity<EmployeeLanguage>()
                .HasOne(el => el.Employee)
                .WithMany(e => e.EmployeeLanguages)
                .HasForeignKey(el => el.EmployeeId);

            modelBuilder.Entity<EmployeeLanguage>()
                .HasOne(el => el.Language)
                .WithMany(lang => lang.EmployeeLanguages)
                .HasForeignKey(el => el.LanguageId);
           //modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.isDeleted);
        }

    }
}
