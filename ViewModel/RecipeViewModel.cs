using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.ViewModel
{
    public class RecipeViewModel: INotifyPropertyChanged
    {
        private string name;
        private string mainPictureAdress;
        private string description;
        private string category;
        private string kitchen;
        private List<IngridientViewModel> ingridients;
        private List<InstructionViewModel> instructions;

        public string Name { get { return name; } set { name = value;OnPropertyChanged("Name"); } }
        public string MainPictureAdress { get { return mainPictureAdress; } set { mainPictureAdress = value; OnPropertyChanged("MainPictureAdress"); } }
        public string Description { get { return description; } set { description = value; OnPropertyChanged("Description"); } }
        public string Category { get { return category; } set { category = value; OnPropertyChanged("Category"); } }
        public string Kitchen { get { return kitchen; } set { kitchen = value; OnPropertyChanged("Kitchen"); } }
        public List<IngridientViewModel> Ingridients { get { return ingridients; } set { ingridients = value; OnPropertyChanged("Ingridients"); } }
        public List<InstructionViewModel> Instructions { get { return instructions; } set { instructions = value; OnPropertyChanged("Instructions"); } }

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
