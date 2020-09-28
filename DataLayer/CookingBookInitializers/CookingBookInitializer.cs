using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.DataLayer.CookingBookInitializers
{
    public class CookingBookInitializer: CreateDatabaseIfNotExists<CookingBookContext>
    {
        protected override void Seed(CookingBookContext db)
        {
            Kitchen ukr = new Models.Kitchen { Name = "Украинская" };//1
            Kitchen rus = new Models.Kitchen { Name = "Русская" };//2
            Kitchen italic = new Models.Kitchen { Name = "Итальянская" };//3
            Kitchen franch = new Models.Kitchen { Name = "Француская" };//4
            Kitchen japan = new Models.Kitchen { Name = "Японская" };//5
            Kitchen folk = new Models.Kitchen { Name = "Народная" };//6

            db.Kitchens.AddRange(new List<Models.Kitchen>() { ukr, rus, italic, franch, japan, folk });
            db.SaveChanges();

            Category soup = new Models.Category { Name = "Супы" };//1
            Category baking = new Models.Category { Name = "Выпечка" };//2
            Category salate = new Models.Category { Name = "Салат" };//3
            Category dessert = new Models.Category { Name = "Десерт" };//4
            Category oatmeal = new Models.Category { Name = "Каша" };//5
            Category snacks = new Models.Category { Name = "Закуски" };//6

            db.Categories.AddRange(new List<Category>() { soup, baking, salate, dessert, oatmeal, snacks });
            db.SaveChanges();
        }
    }
}
