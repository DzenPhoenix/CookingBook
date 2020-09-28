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
            recipeView.MainPictureAdress = @".\Res\pic\1\MainPic.png";
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
            InstructionViewModel ins_2 = new InstructionViewModel { Name = "Добавить ваниль и яйцо.", ImageSource = @".\res\pic\1\2.jpg" };
            InstructionViewModel ins_3 = new InstructionViewModel { Name = "Начать замешивать тесто пока оно не начнёт собираться в шар. Завернуть тесто в пищевую плёнку и убрать на пол часа в холодильник.", ImageSource = @".\res\pic\1\3.jpg" };
            InstructionViewModel ins_4 = new InstructionViewModel { Name = "Форму (у меня 25 см в диаметре) слегка присыпать мукой. Поверхность, на которой будете раскатывать тесто присыпать мукой. Тесто раскатать в круг чуть больше чем форма. Перенести в форму и равномерно распределить, сделать бортики. Наколоть вилкой и выпекать 10 минут при 180*C (350*F).", ImageSource = @".\res\pic\1\4.jpg" };
            InstructionViewModel ins_5 = new InstructionViewModel { Name = "Сливы помыть, обсушить, разрезать на половинки и удалить косточку. Порезать каждую половинку на слайсы острым ножом что бы половинки сохранили форму.", ImageSource = @".\res\pic\1\5.jpg" };
            InstructionViewModel ins_6 = new InstructionViewModel { Name = "Выложить порезанные половинки слив срезом вниз на тарт чередуя вертикальный разрез с горизонтальным. Сахар смешать с корицей. Холодное масло порезать на маленькие кубики.", ImageSource = @".\res\pic\1\6.jpg" };
            InstructionViewModel ins_7 = new InstructionViewModel { Name = "Посыпать сливы сахаром с корицей и сверху разложить кубики масла.", ImageSource = @".\res\pic\1\7.jpg" };
            InstructionViewModel ins_8 = new InstructionViewModel { Name = "Выпекать в предварительно разогретой до 180*C (350*F) примерно 25 минут. Дать остыть и вынуть из формы.", ImageSource = @".\res\pic\1\8.jpg" };
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

            RecipeViewModel recipeView_1 = new RecipeViewModel();
            recipeView_1.Name = "Булочки-рулеты для пикника";
            recipeView_1.Category = "Выпечка";
            recipeView_1.Kitchen = "Народная";
            recipeView_1.MainPictureAdress = @".\Res\pic\2\MainPic.jpg";
            recipeView_1.Description = "Хочу предложить вкусный и сытный перекус для пикника. Такие булочки очень удобно брать собой. Отправляясь на природу, на дачу, в лес, они всегда будут кстати. А ещё их очень удобно давать ребёнку в школу.";

            IngridientViewModel ing_2_1 = new IngridientViewModel { Name = "Мука пшеничная", Comment = "— 500 г" };
            IngridientViewModel ing_2_2 = new IngridientViewModel { Name = "Вода", Comment = "300 мл" };
            IngridientViewModel ing_2_3 = new IngridientViewModel { Name = "Масло растительное", Comment = " — 5 ст. л" };
            IngridientViewModel ing_2_4 = new IngridientViewModel { Name = "Дрожжи", Comment = "(сухие) — 1 пач." };
            IngridientViewModel ing_2_5 = new IngridientViewModel { Name = "Сахар ", Comment = " 2 ст. л." };
            IngridientViewModel ing_2_6 = new IngridientViewModel { Name = "Соль ", Comment = "1 щепот." };
            IngridientViewModel ing_2_7 = new IngridientViewModel { Name = "Индейка", Comment = "— 800 г" };
            IngridientViewModel ing_2_8 = new IngridientViewModel { Name = "Перец черный ", Comment = "по вкусу" };
            IngridientViewModel ing_2_9 = new IngridientViewModel { Name = "Яблоко ", Comment = "— 1 шт" };
            IngridientViewModel ing_2_10 = new IngridientViewModel { Name = "Лук-порей", Comment = "— 1 шт" };
            IngridientViewModel ing_2_11= new IngridientViewModel { Name = "Яйцо куриное ", Comment = "(взбейте, для смазывания) — 1 шт" };
            recipeView.Ingridients.AddRange(new List<IngridientViewModel>() { ing_2_1, ing_2_2, ing_2_3, ing_2_4, ing_2_5, ing_2_6, ing_2_7, ing_2_8, ing_2_9, ing_2_10, ing_2_11 });

            InstructionViewModel ins_2_1 = new InstructionViewModel { Name = "Бедро индейки нужно предварительно запечь. Я обычно ставлю запекаться на ночь. Бедро индейки посолите и поперчите, приправьте итальянскими травами, хорошо заверните в фольгу и запекайте при температуре 200 градусов 2 часа.", ImageSource = @".\res\pic\2\1.jpg" };
            InstructionViewModel ins_2_2 = new InstructionViewModel { Name = "Дрожжи растворите в тёплой воде, добавьте сахар, 1 ст. л. муки, хорошо перемешайте и дайте постоять 10 мин.", ImageSource = @".\res\pic\2\2.jpg" };
            InstructionViewModel ins_2_3 = new InstructionViewModel { Name = "Добавьте муку, соль, растительное масло и замесите тесто. Накройте чтобы не обветрилось и поставьте в тёплое место на 1,5 часа.", ImageSource = @".\res\pic\2\3.jpg" };
            InstructionViewModel ins_2_4 = new InstructionViewModel { Name = "Приготовим начинку: индейку нарежьте маленькими кусочками.", ImageSource = @".\res\pic\2\4.jpg" };
            InstructionViewModel ins_2_5 = new InstructionViewModel { Name = "Яблоко очистите от кожуры, удалите сердцевину и нарежьте маленькими тонкими кусочками.", ImageSource = @".\res\pic\2\5.jpg" };
            InstructionViewModel ins_2_6 = new InstructionViewModel { Name = "Лук-порей нарежьте тонкими кольцами.", ImageSource = @".\res\pic\2\6.jpg" };
            InstructionViewModel ins_2_7 = new InstructionViewModel { Name = "Смешайте в миске индейку, яблоки, лук-порей, горчицу и листики тимьяна.", ImageSource = @".\res\pic\2\7.jpg" };
            InstructionViewModel ins_2_8 = new InstructionViewModel { Name = "Раскатайте половину теста в пласт толщиной 1 см.", ImageSource = @".\res\pic\2\8.jpg" };
            InstructionViewModel ins_2_9 = new InstructionViewModel { Name = "Равномерно выложите половину начинки, края смажьте яйцом и плотно заверните в рулет, разрежьте на три части. Также заверните второй рулет.", ImageSource = @".\res\pic\2\9.jpg" };
            InstructionViewModel ins_2_10 = new InstructionViewModel { Name = "Выложите рулетики на противень застеленной пергаментом или силиконовым ковриком. Смажьте рулеты оставшимся яйцом. Запекайте в предварительно нагретой до 180 градусов духовке 25", ImageSource = @".\res\pic\2\10.jpg" };
            recipeView.Instructions.AddRange(new List<InstructionViewModel>() { ins_2_1, ins_2_2, ins_2_3, ins_2_4, ins_2_5, ins_2_6, ins_2_7, ins_2_8, ins_2_9, ins_2_10 });

            serializedIngridients = JsonConvert.SerializeObject(recipeView_1.Ingridients);
            serializedInstructions = JsonConvert.SerializeObject(recipeView_1.Instructions);

            Recipe recipe_1 = new Recipe
            {
                Name = recipeView_1.Name,
                CategoryId = 2,
                KitchenId = 6,
                MainPictureAdress = recipeView_1.MainPictureAdress,
                Description = recipeView_1.Description,
                SerializedIngridients = serializedIngridients,
                SerializedInstructions = serializedInstructions
            };
            db.Recipes.Add(recipe_1);
            db.SaveChanges();
        }
    }
}
