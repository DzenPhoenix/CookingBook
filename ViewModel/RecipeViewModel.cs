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
    }
}
