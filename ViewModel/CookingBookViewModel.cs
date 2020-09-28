using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using Newtonsoft.Json;
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

        //public Service.Filter Filter{get;set;}


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

                HashSet<string> ingridientSet = new HashSet<string>() ;
                foreach (Recipe recipe in db.Recipes)
                {
                    List<IngridientViewModel> ingridients = JsonConvert.DeserializeObject<List<IngridientViewModel>>(recipe.SerializedIngridients);
                    foreach (IngridientViewModel ingridient in ingridients)
                    {
                        ingridientSet.Add(ingridient.Name);
                    } 
                }
                this.Ingridients = new ObservableCollection<string>(ingridientSet);
            }
            //this.FilteredRecipes = 
        }

        private List<string> FilterByCategory(List<string> categories)
        {
            List<string> res =new List<string>();
            using (CookingBookContext db = new CookingBookContext())
            {
                foreach (Recipe recipe in db.Recipes)
                {
                    foreach (string category in categories)
                    {
                        if (recipe.Category.Name == category)
                        {
                            res.Add(recipe.Name);break;
                        }
                    }
                }
            }
            return res;
        }

        private List<string> FilterByKitchen(List<string> kitchens)
        {
            List<string> res = new List<string>();
            using (CookingBookContext db = new CookingBookContext())
            {
                foreach (Recipe recipe in db.Recipes)
                {
                    foreach (string kitchen in kitchens)
                    {
                        if (recipe.Kitchen.Name == kitchen)
                        {
                            res.Add(recipe.Name); break;
                        }
                    }
                }
            }
            return res;
        }

        private List<string> FilterByIngridients(List<string> ingridients)
        {
            List<string> res = new List<string>();
            using (CookingBookContext db = new CookingBookContext())
            {
                foreach (Recipe recipe in db.Recipes)
                {
                    List<IngridientViewModel> recipeIngridients =  JsonConvert.DeserializeObject<List<IngridientViewModel>>(recipe.SerializedIngridients);
                    foreach (IngridientViewModel recipeIngridient in recipeIngridients)
                    {
                        foreach (string ingridient in ingridients)
                        {
                            if (ingridient == recipeIngridient.Name)
                            {
                                res.Add(recipe.Name); break;
                            }
                        }
                    }
                }
            }
            return res;
        }      
    }
}
