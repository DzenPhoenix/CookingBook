using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using CookingBook.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.DataLayer.CookingBookInitializers
{
    public class CookingBookInitializer: CreateDatabaseIfNotExists<CookingBookContext>
    {
        protected override void Seed(CookingBookContext db)
        {
            Kitchen ukr = new Models.Kitchen { Name = "Украинская" };//1
            Kitchen rus = new Models.Kitchen { Name = "Русская" };//2
            Kitchen italic = new Models.Kitchen { Name = "Итальянская" };//3
            Kitchen franch = new Models.Kitchen { Name = "Француская" };//4
            Kitchen japan = new Models.Kitchen { Name = "Японская" };//5
            Kitchen folk = new Models.Kitchen { Name = "Народная" };//6

            db.Kitchens.AddRange(new List<Models.Kitchen>() { ukr, rus, italic, franch, japan, folk });
            db.SaveChanges();

            Category soup = new Models.Category { Name = "Супы" };//1
            Category baking = new Models.Category { Name = "Выпечка" };//2
            Category salate = new Models.Category { Name = "Салат" };//3
            Category dessert = new Models.Category { Name = "Десерт" };//4
            Category oatmeal = new Models.Category { Name = "Каша" };//5
            Category snacks = new Models.Category { Name = "Закуски" };//6

            db.Categories.AddRange(new List<Category>() { soup, baking, salate, dessert, oatmeal, snacks });
            db.SaveChanges();

            RecipeViewModel recipeView = new RecipeViewModel();
            recipeView.Name = "Тарт со сливами";
            recipeView.Category = "Выпечка";
            recipeView.Kitchen = "Народная";
            recipeView.MainPictureAdress = @".\Res\pic\TestRecipe\MainPicTest.png";
            recipeView.Description = "Сливовый тарт, очень вкусный, сочный, нежный, ароматный, простой в приготовлении и который выглядит эффектно.";

            IngridientViewModel ing_1 = new IngridientViewModel { Name = "Мука пшеничная", Comment = "— 200 г" };
            IngridientViewModel ing_2 = new IngridientViewModel { Name = "Пудинг ванильный", Comment = "(40г) — 1 пач." };
            IngridientViewModel ing_3 = new IngridientViewModel { Name = "Сахар", Comment = "— 30 г" };
            IngridientViewModel ing_4 = new IngridientViewModel { Name = "Масло сливочное", Comment = "— 150 г" };
            IngridientViewModel ing_5 = new IngridientViewModel { Name = "Яйцо куриное", Comment = "— 1 шт" };
            IngridientViewModel ing_6 = new IngridientViewModel { Name = "Ванильная эссенция", Comment = "(Можно использовать ванильный сахар) — 1 ч. л" };
            IngridientViewModel ing_7 = new IngridientViewModel { Name = "Слива", Comment = "(10 - зависит от размера слив) — 8 шт" };
            IngridientViewModel ing_8 = new IngridientViewModel { Name = "Корица", Comment = "— 1/2 ч. л." };
            recipeView.Ingridients.AddRange(new List<IngridientViewModel>() { ing_1, ing_2, ing_3, ing_4, ing_5, ing_6, ing_7, ing_8 });

            InstructionViewModel ins_1 = new InstructionViewModel { Name = "В миске соединить холодное масло порезанное на кусочки, пудинг, сахар и муку. Всё перетереть руками в крошку.", ImageSource = @".\res\pic\TestRecipe\1.jpg" };
            InstructionViewModel ins_2 = new InstructionViewModel { Name = "Добавить ваниль и яйцо.", ImageSource = @".\res\pic\TestRecipe\2.jpg" };
            InstructionViewModel ins_3 = new InstructionViewModel { Name = "Начать замешивать тесто пока оно не начнёт собираться в шар. Завернуть тесто в пищевую плёнку и убрать на пол часа в холодильник.", ImageSource = @".\res\pic\TestRecipe\3.jpg" };
            InstructionViewModel ins_4 = new InstructionViewModel { Name = "Форму (у меня 25 см в диаметре) слегка присыпать мукой. Поверхность, на которой будете раскатывать тесто присыпать мукой. Тесто раскатать в круг чуть больше чем форма. Перенести в форму и равномерно распределить, сделать бортики. Наколоть вилкой и выпекать 10 минут при 180*C (350*F).", ImageSource = @".\res\pic\TestRecipe\4.jpg" };
            InstructionViewModel ins_5 = new InstructionViewModel { Name = "Сливы помыть, обсушить, разрезать на половинки и удалить косточку. Порезать каждую половинку на слайсы острым ножом что бы половинки сохранили форму.", ImageSource = @".\res\pic\TestRecipe\5.jpg" };
            InstructionViewModel ins_6 = new InstructionViewModel { Name = "Выложить порезанные половинки слив срезом вниз на тарт чередуя вертикальный разрез с горизонтальным. Сахар смешать с корицей. Холодное масло порезать на маленькие кубики.", ImageSource = @".\res\pic\TestRecipe\6.jpg" };
            InstructionViewModel ins_7 = new InstructionViewModel { Name = "Посыпать сливы сахаром с корицей и сверху разложить кубики масла.", ImageSource = @".\res\pic\TestRecipe\7.jpg" };
            InstructionViewModel ins_8 = new InstructionViewModel { Name = "Выпекать в предварительно разогретой до 180*C (350*F) примерно 25 минут. Дать остыть и вынуть из формы.", ImageSource = @".\res\pic\TestRecipe\8.jpg" };
            recipeView.Instructions.AddRange(new List<InstructionViewModel>() { ins_1, ins_2, ins_3, ins_4, ins_5, ins_6, ins_7, ins_8 });

            string serializedIngridients = JsonConvert.SerializeObject(recipeView.Ingridients);
            string serializedInstructions = JsonConvert.SerializeObject(recipeView.Instructions);

            Recipe recipe = new Recipe
            {
                Name = recipeView.Name,
                CategoryId = 2,
                KitchenId = 6,
                MainPictureAdress = recipeView.MainPictureAdress,
                Description=recipeView.Description,
                SerializedIngridients = serializedIngridients,
                SerializedInstructions = serializedInstructions
            };
            db.Recipes.Add(recipe);
            db.SaveChanges();
        }
    }
}
