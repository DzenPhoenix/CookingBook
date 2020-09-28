using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.DataLayer.Models
{ 
    
    public class Kitchen
    {
        public int KitchenId { get; set; }
        public string Name { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
        public Kitchen() { this.Recipes = new List<Recipe>(); }
    }
}
