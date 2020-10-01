using CookingBook.DataLayer.Contexts;
using CookingBook.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.DataLayer.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string MainPictureAdress { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public string SerializedIngridients { get; set; }
        public string SerializedInstructions { get; set; }
        public int? KitchenId { get; set; }
        public Kitchen Kitchen { get; set; }

        public Recipe(){}
        public Recipe(RecipeViewModel recipeView)
        {
            string serializedIngridients = JsonConvert.SerializeObject(recipeView.Ingridients);
            string serializedInstructions = JsonConvert.SerializeObject(recipeView.Instructions);

            this.Name = recipeView.Name;

            int categoryId = 0;
            int kitchenId = 0;
            using (CookingBookContext db = new CookingBookContext())
            {
                categoryId = (from category in db.Categories
                              where category.Name == recipeView.Category
                              select category.CategoryId).FirstOrDefault();

                kitchenId = (from kitchen in db.Kitchens
                             where kitchen.Name == recipeView.Kitchen
                             select kitchen.KitchenId).FirstOrDefault();
            }
            if (categoryId != 0)
            {
                this.CategoryId = categoryId;
            }
            else
            {
                this.Category = new Category() { Name = recipeView.Category };
            }
            if (kitchenId != 0)
            {
                this.KitchenId = kitchenId;
            }
            else { this.Kitchen = new Kitchen() { Name = recipeView.Kitchen }; }
            this.MainPictureAdress = recipeView.MainPictureAdress;
            this.Description = recipeView.Description;
            this.SerializedIngridients = serializedIngridients;
            this.SerializedInstructions = serializedInstructions;
        }
    }

}
