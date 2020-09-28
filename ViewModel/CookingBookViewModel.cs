using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using CookingBook.Service;
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
        public ObservableCollection<RecipeViewModel> FilteredRecipes { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Kitchens { get; set; }
        public ObservableCollection<string> Ingridients { get; set; }

        public Filter Filter{get;set;}


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
                this.FilteredRecipes = new ObservableCollection<RecipeViewModel>(GetAllRecipes());            
            }
            
        }

        private List<RecipeViewModel> GetAllRecipes()
        {
            List<Recipe> recipes = new List<Recipe>();
            List<RecipeViewModel> res = new List<RecipeViewModel>();
            using (CookingBookContext db = new CookingBookContext())
            {
                var recipeList = from recipe in db.Recipes
                                 select recipe;
                recipes = recipeList.ToList<Recipe>();
            }
            foreach (Recipe recipe in recipes)
            {
                res.Add(new RecipeViewModel(recipe));
            }
            return res;
        }

        private List<Recipe> FilterByCategory(List<Recipe> recipes,List<string> categories)
        {
            List<Recipe> res =new List<Recipe>();  
            foreach (Recipe recipe in recipes)
            {
                 foreach (string category in categories)
                 {
                     if (recipe.Category.Name==category)
                     {
                         res.Add(recipe);break;
                     }
                  }
            }
            return res;
        }

        private List<Recipe> FilterByKitchen(List<Recipe> recipes, List<string> kitchens)
        {
            List<Recipe> res = new List<Recipe>();
            foreach (Recipe recipe in recipes)
            {
                foreach (string kitchen in kitchens)
                {
                    if (recipe.Kitchen.Name == kitchen)
                    {
                        res.Add(recipe); break;
                    }
                }
            }
            return res;
        }

        private List<Recipe> FilterByIngridients(List<Recipe> recipes,List<string> ingridients)
        {
            List<Recipe> res = new List<Recipe>();
            foreach (Recipe recipe in recipes)
            {
                List<IngridientViewModel> recipeIngridients =  JsonConvert.DeserializeObject<List<IngridientViewModel>>(recipe.SerializedIngridients);
                foreach (IngridientViewModel recipeIngridient in recipeIngridients)
                {
                    foreach (string ingridient in ingridients)
                    {
                        if (ingridient == recipeIngridient.Name)
                        {
                            res.Add(recipe); break;
                        }
                    }
                }
            }
            return res;
        }      
    }
}
