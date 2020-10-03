using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using CookingBook.Service;
using CookingBook.ViewModel.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CookingBook.ViewModel
{
    public class CookingBookViewModel : INotifyPropertyChanged
    {
        private RecipeViewModel selectedRecipe;
        public RecipeViewModel SelectedRecipe { get { return selectedRecipe; } set { selectedRecipe = value;OnPropertyChanged("SelectedRecipe"); } }
        public ObservableCollection<RecipeViewModel> FilteredRecipes { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Kitchens { get; set; }
        public ObservableCollection<IngridientViewModel> Ingridients { get; set; }

        private CookingBookCommand filterCommand;
        public CookingBookCommand FilterCommand
        {
            get
            {
                return filterCommand??(filterCommand = new CookingBookCommand(FilterAction));
            }
        }

        public Filter Filter { get; set; }


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

                HashSet<IngridientViewModel> ingridientSet = new HashSet<IngridientViewModel>();
                foreach (Recipe recipe in db.Recipes)
                {
                    List<IngridientViewModel> ingridients = JsonConvert.DeserializeObject<List<IngridientViewModel>>(recipe.SerializedIngridients);
                    foreach (IngridientViewModel ingridient in ingridients)
                    {
                        ingridientSet.Add(ingridient);
                    }
                }
                this.Ingridients = new ObservableCollection<IngridientViewModel>(ingridientSet);
                this.FilteredRecipes = new ObservableCollection<RecipeViewModel>(GetAllRecipes());
                this.SelectedRecipe = this.FilteredRecipes[0];
            }

        }


        public static List<RecipeViewModel> GetAllRecipes()
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

        private void FilterRecipes(List<RecipeViewModel> recipes,Filter filter)
        {
            foreach (RecipeViewModel recipe in recipes)
            {
                if (RequiredByCategory(recipe, filter.Categories) && RequiredByKitchen(recipe, filter.Kitchens) && RequiredByIngridients(recipe, filter.Ingridients))
                {
                    this.FilteredRecipes.Add(recipe);
                }
            }
        }

        private bool RequiredByCategory(RecipeViewModel recipe, List<string> categories)
        {
            bool res = false;
            foreach (string category in categories)
            {
                res = res || recipe.Category == category;
            }
            return res;
        }

        private bool RequiredByKitchen(RecipeViewModel recipe, List<string> kitchens)
        {
            bool res = false;
            foreach (string kitchen in kitchens)
            {
                res = res || recipe.Kitchen == kitchen;
            }
            return res;
        }

        private bool RequiredByIngridients(RecipeViewModel recipe, List<string> ingridients)
        {
            bool res = false;
            foreach (IngridientViewModel recipeIngridient in recipe.Ingridients)
            {
                foreach (string ingridient in ingridients)
                {
                    res = res || recipeIngridient.Name == ingridient;
                }
            }
            return res;
        }

       void FilterAction(object filteredRecipes)
       {
            this.FilteredRecipes.Clear();
            FilterRecipes(GetAllRecipes(),Filter);
       }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
