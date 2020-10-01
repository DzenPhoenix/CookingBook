using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using CookingBook.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CookingBook.View
{
    /// <summary>
    /// Логика взаимодействия для AddRecipe.xaml
    /// </summary>
    public partial class AddRecipe : Window
    {
        private int tagOfIngridient;
        private int tagOfInstruction;
        private FileInfo mainPic;
        private DirectoryInfo recipeDir;
        public AddRecipe()
        {
            InitializeComponent();
            this.Height = 800;
            tagOfIngridient = 0;
            tagOfInstruction = 0;
            InitComboBoxCategory();
            InitComboBoxKitchen();
            InitComboBoxIngridient(this.comboBoxIngridient);
            this.mainPic = new FileInfo(@".\Res\pic\NoPic.png");
        }

        private void InitComboBoxCategory()
        {
            using (CookingBookContext db = new CookingBookContext())
            {
                List<String> categories = (from category in db.Categories
                                           select category.Name).ToList<String>();
                this.comboBoxCategory.ItemsSource = categories;
            }
        }

        private void InitComboBoxKitchen()
        {
            using (CookingBookContext db = new CookingBookContext())
            {
                List<String> kitchens = (from kitchen in db.Kitchens
                                         select kitchen.Name).ToList<String>();
                this.comboBoxKitchen.ItemsSource = kitchens;
            }
        }

        private void InitComboBoxIngridient(ComboBox box)
        {
            using (CookingBookContext db = new CookingBookContext())
            {
                HashSet<string> ingridientSet = new HashSet<string>();
                foreach (Recipe recipe in db.Recipes)
                {
                    List<IngridientViewModel> ingridients = JsonConvert.DeserializeObject<List<IngridientViewModel>>(recipe.SerializedIngridients);
                    foreach (IngridientViewModel ingridient in ingridients)
                    {
                        ingridientSet.Add(ingridient.Name);
                    }
                }
                box.ItemsSource = new List<string>(ingridientSet);
            }
        }
        private void AddPicture(Image image)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "Image Files| *.jpg; *.bmp; *.png; *.tiff; *.gif";
            dialog.AddExtension = true;
            dialog.Multiselect = true;

            if (dialog.ShowDialog() != null)
            {
                string[] fileNames = dialog.FileNames;
                //set image source
                if (fileNames.Length > 0)
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit();
                    FileInfo file = new FileInfo(fileNames[0]);
                    bmp.UriSource = new System.Uri(file.FullName);
                    bmp.EndInit();
                    image.Source = bmp;
                    this.mainPic = file;
                }
            }

        }

        private void ButtonAddMainPictureClick(object sender, RoutedEventArgs e)
        {
            AddPicture(this.imageMainPic);
        }

        private void ButtonAddPictureClick(object sender, RoutedEventArgs e)
        {
            AddPicture((sender as Button).Tag as Image);
        }

        private void ButtonIngridientClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content as string == "+")
            {
                tagOfIngridient++;
                (sender as Button).Content = "-";
                DockPanel dockPanel = new DockPanel() { LastChildFill = true };
                ComboBox comboBox = new ComboBox() { FontSize = 30, Margin = new Thickness(10), BorderThickness = new Thickness(2), Width = 300, IsEditable = true };
                InitComboBoxIngridient(comboBox);
                DockPanel.SetDock(comboBox, Dock.Left);
                dockPanel.Children.Add(comboBox);

                Button button = new Button() { Content = "+", FontSize = 30, Width = 50, Margin = new Thickness(10), Tag = tagOfIngridient };
                button.Click += ButtonIngridientClick;
                DockPanel.SetDock(button, Dock.Right);
                dockPanel.Children.Add(button);

                TextBox textBox = new TextBox() { FontSize = 30, Margin = new Thickness(10), BorderThickness = new Thickness(2) };
                dockPanel.Children.Add(textBox);

                this.stackPanelIngridients.Children.Add(dockPanel);
            }
            else
            {
                int indexRemove = 0;
                for (int i = 0; i < this.stackPanelIngridients.Children.Count; ++i)
                {
                    if (((this.stackPanelIngridients.Children[i] as DockPanel).Children[1] as Button).Tag == (sender as Button).Tag)
                    {
                        indexRemove = i; break;
                    }
                }
                this.stackPanelIngridients.Children.RemoveAt(indexRemove);
            }
        }

        private void ButtonInstructionsClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content as string == "+")
            {
                tagOfInstruction++;
                (sender as Button).Content = "-";

                DockPanel dockPanel = new DockPanel() { LastChildFill = true };
                Border border = new Border() { BorderThickness = new Thickness(2), BorderBrush = Brushes.AntiqueWhite, Margin = new Thickness(10) };
                DockPanel.SetDock(border, Dock.Left);


                StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical };
                border.Child = panel;

                Image image = new Image() { MaxWidth = 300, Stretch = Stretch.UniformToFill };
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new System.Uri(@"..\Res\pic\NoPic.png", UriKind.Relative);
                bmp.EndInit();
                image.Source = bmp;
                panel.Children.Add(image);

                Button buttonAdd = new Button() { Content = "Add", FontSize = 20, Tag = image };
                buttonAdd.Click += ButtonAddPictureClick;
                panel.Children.Add(buttonAdd);

                Button button = new Button() { Content = "+", FontSize = 30, Width = 50, Margin = new Thickness(10), Tag = tagOfInstruction };
                button.Click += ButtonInstructionsClick;
                DockPanel.SetDock(button, Dock.Right);

                TextBox textBox = new TextBox() { FontSize = 30, Margin = new Thickness(10), BorderThickness = new Thickness(2) };

                dockPanel.Children.Add(border);
                dockPanel.Children.Add(button);
                dockPanel.Children.Add(textBox);

                this.stackPanelInstrucions.Children.Add(dockPanel);
            }

            else
            {
                int indexRemove = 0;
                for (int i = 0; i < this.stackPanelInstrucions.Children.Count; ++i)
                {
                    if (((this.stackPanelInstrucions.Children[i] as DockPanel).Children[1] as Button).Tag == (sender as Button).Tag)
                    {
                        indexRemove = i; break;
                    }
                }
                this.stackPanelInstrucions.Children.RemoveAt(indexRemove);
            }
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSaveRecipeClick(object sender, RoutedEventArgs e)
        {
            RecipeViewModel recipeView = new RecipeViewModel();
            recipeView.Name = this.textBoxRecipeName.Text;
            recipeView.Category = this.comboBoxCategory.Text;
            recipeView.Kitchen = this.comboBoxKitchen.Text;

            DirectoryInfo parentDir = new DirectoryInfo(@".\Res\pic\");
            DirectoryInfo directory = new DirectoryInfo(@".\Res\pic\0");
            int count = parentDir.GetDirectories().Length;
            for (int i = 1; i <= count + 1; i++)
            {
                directory = new DirectoryInfo(parentDir.FullName + i.ToString());
                if (directory.Exists)
                {
                    continue;
                }
                else
                {
                    directory.Create();
                    this.recipeDir = directory;
                    break;
                }
            }
            this.mainPic = this.mainPic.CopyTo(directory.FullName + "\\" + this.mainPic.Name);
            recipeView.MainPictureAdress = this.mainPic.FullName;
            recipeView.Description = this.textBoxRecipeDesription.Text;

            List<IngridientViewModel> ingridientViews = new List<IngridientViewModel>();
            foreach (DockPanel panel in this.stackPanelIngridients.Children)
            {
                IngridientViewModel ingridient = new IngridientViewModel()
                {
                    Name = (panel.Children[0] as ComboBox).Text,
                    Comment = "--"+(panel.Children[2] as TextBox).Text
                };
                ingridientViews.Add(ingridient);
            }
            recipeView.Ingridients = ingridientViews;

            List<InstructionViewModel> instructionViews = new List<InstructionViewModel>();
            int fileName = 0;
            foreach (DockPanel dock in this.stackPanelInstrucions.Children)
            {
                fileName++;
                Image image = ((dock.Children[0] as Border).Child as StackPanel).Children[0] as Image;
                FileInfo imageFile = GetImageFile(image);
                imageFile = imageFile.CopyTo(recipeDir + "\\" + fileName.ToString() +"."+ imageFile.Extension);

                InstructionViewModel instruction = new InstructionViewModel()
                {
                    Name = (dock.Children[2] as TextBox).Text,
                    ImageSource = imageFile.FullName
                };
                instructionViews.Add(instruction);
            }
            recipeView.Instructions = instructionViews;

            Recipe recipe = new Recipe(recipeView);
            using (CookingBookContext db = new CookingBookContext())
            {
                db.Recipes.Add(recipe);
                db.SaveChanges(); 
            }
            this.Close();
        }

        private FileInfo GetImageFile(Image image)
        {
            try
            {
                string path = image.Source.ToString().Substring(8);
                return new FileInfo(path);
            }
            catch (Exception)
            { 
                return new FileInfo(@".\Res\pic\NoPic.png");
            }
        }
    }
}
