using CookingBook.DataLayer.CookingBookInitializers;
using CookingBook.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.DataLayer.Contexts 
{
    public class CookingBookContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Kitchen> Kitchens { get; set; }

        public CookingBookContext() : base("CookingBookConnection") { }
        static CookingBookContext()
        {
            Database.SetInitializer(new CookingBookInitializer());
        }
    }
}
