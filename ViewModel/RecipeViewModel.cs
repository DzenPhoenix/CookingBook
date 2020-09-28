using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.ViewModel
{
    public class RecipeViewModel
    {
        public string Name { get; set; }
        public string MainPictureAdress { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Kitchen { get; set; }
        public List<IngridientViewModel> Ingridients { get; set; }
        public List<InstructionViewModel> Instructions { get; set; }

        public RecipeViewModel()
        {
            this.Ingridients = new List<IngridientViewModel>();
            this.Instructions = new List<InstructionViewModel>();
        }

        public RecipeViewModel(Recipe recipe)
        {
            this.Name = recipe.Name;
            this.MainPictureAdress = recipe.MainPictureAdress;
            this.Description = recipe.Description;
            
            using (CookingBookContext db = new CookingBookContext())
            {
                string categoryName = (from category in db.Categories
                               where recipe.CategoryId==category.CategoryId
                               select category.Name).FirstOrDefault();
                this.Category = categoryName;

                string kitchenName = (from kitchen in db.Kitchens
                                      where recipe.KitchenId == kitchen.KitchenId
                                      select kitchen.Name).FirstOrDefault();
                this.Kitchen = kitchenName;
            }
            this.Ingridients = JsonConvert.DeserializeObject<List<IngridientViewModel>>(recipe.SerializedIngridients);
            this.Instructions = JsonConvert.DeserializeObject<List<InstructionViewModel>>(recipe.SerializedInstructions);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
