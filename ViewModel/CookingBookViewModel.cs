using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.ViewModel
{
    public class CookingBookViewModel
    {
        public RecipeViewModel SelectedRecipe { get; set; }
        public ObservableCollection<string> FilteredRecipes { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Kitchens { get; set; }
        public ObservableCollection<string> Ingridients { get; set; }

        public CookingBookViewModel()
        {
            using (CookingBookContext db = new CookingBookContext())
            {
                var categoryList = from category in db.Categories
                                   select category.Name;     
                this.Categories = new ObservableCollection<string>(categoryList.ToList<string>());

                var kitchenList = from kitchen in db.Kitchens
                                   select kitchen.Name;
                this.Kitchens = new ObservableCollection<string>(kitchenList.ToList<string>());

            }
        }

    }
}
