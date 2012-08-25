using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


using NHibernateFetchProblems.Models;

namespace NHibernateFetchProblems.DbMapping
{
    public class EfMapping : DbContext
    {
        public EfMapping() 
        {
            this.Configuration.ProxyCreationEnabled = true;


        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            
        }
    }
}
