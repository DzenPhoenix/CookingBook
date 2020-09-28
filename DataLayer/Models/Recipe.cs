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
    }

}
