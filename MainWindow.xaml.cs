using CookingBook.DataLayer.Contexts;
using CookingBook.DataLayer.Models;
using CookingBook.Service;
using CookingBook.View;
using CookingBook.ViewModel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CookingBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new CookingBookViewModel();
            InitializeDynamicComponent();
        }

        private void InitializeDynamicComponent()
        {
            foreach (string category in (this.DataContext as CookingBookViewModel).Categories)
            {
                CheckBox box = new CheckBox() { Content =category, IsChecked = true,Foreground= Brushes.Blue };
                this.listBoxCategory.Items.Add(box);
            }
            foreach (string kitchen in (this.DataContext as CookingBookViewModel).Kitchens)
            {
                CheckBox box = new CheckBox() { Content = kitchen, IsChecked = true, Foreground = Brushes.Blue };
                this.listBoxKitchen.Items.Add(box);
            }

            foreach (IngridientViewModel ingridient in (this.DataContext as CookingBookViewModel).Ingridients)
            {
                CheckBox box = new CheckBox() { Content = ingridient.Name, IsChecked = true, Foreground = Brushes.Blue };
                this.listBoxIngridients.Items.Add(box);
            }

            foreach (InstructionViewModel instruction in (this.DataContext as CookingBookViewModel).SelectedRecipe.Instructions)
            {
                DockPanel panel = new DockPanel();
                Image image = new Image()
                {
                    MaxWidth = 300,
                    Stretch = Stretch.UniformToFill,
                    Source = (new ImageSourceConverter().ConvertFromString(instruction.ImageSource) as ImageSource)
                };
                DockPanel.SetDock(image, Dock.Left);
                panel.Children.Add(image);
                panel.Children.Add(new TextBlock { Text = instruction.Name, TextWrapping = TextWrapping.Wrap });
                this.stackPanelInstruction.Children.Add(panel);
                this.stackPanelInstruction.Children.Add(new Separator());
            }

        }

        private void ButtonExpanderLeftClick(object sender, RoutedEventArgs e)
        {
            if (this.LeftPanel.Visibility == Visibility.Visible)
            {
                this.LeftPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.LeftPanel.Visibility = Visibility.Visible;
            }
        }

        private void ButtonExpanderRightClick(object sender, RoutedEventArgs e)
        {
            if (this.RightPanel.Visibility == Visibility.Visible)
            {
                this.RightPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.RightPanel.Visibility = Visibility.Visible;
            }
        }

        private void GetFilter()
        {
            Filter filter = new Filter();
            foreach (CheckBox box in this.listBoxCategory.Items)
            {
                if (box.IsChecked == true)
                {
                    filter.Categories.Add(box.Content as string);
                }
            }

            foreach (CheckBox box in this.listBoxKitchen.Items)
            {
                if (box.IsChecked == true)
                {
                    filter.Kitchens.Add(box.Content as string);
                }
            }

            foreach (CheckBox box in this.listBoxIngridients.Items)
            {
                if (box.IsChecked == true)
                {
                    filter.Ingridients.Add(box.Content as string);
                }
            }
            (this.DataContext as CookingBookViewModel).Filter = filter;
        }

        private void ButtonFilterClick(object sender, RoutedEventArgs e)
        {
            GetFilter();
        }

        private void CheckBoxClickAllCategory(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                foreach (CheckBox box in this.listBoxCategory.Items)
                {
                    box.IsChecked = true;
                }
            }
            else
            {
                foreach (CheckBox box in this.listBoxCategory.Items)
                {
                    box.IsChecked = false;
                }
            }
        }

        private void CheckBoxClickAllKitchen(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                foreach (CheckBox box in this.listBoxKitchen.Items)
                {
                    box.IsChecked = true;
                }
            }
            else
            {
                foreach (CheckBox box in this.listBoxKitchen.Items)
                {
                    box.IsChecked = false;
                }
            }
        }

        private void CheckBoxClickAllIngridients(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                foreach (CheckBox box in this.listBoxIngridients.Items)
                {
                    box.IsChecked = true;
                }
            }
            else
            {
                foreach (CheckBox box in this.listBoxIngridients.Items)
                {
                    box.IsChecked = false;
                }
            }
        }

        private void ListBoxFilteredRecipesMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecipeViewModel selectedRecipe = (sender as ListBox).SelectedItem as RecipeViewModel;
            ShowRecipe(selectedRecipe);     
        }

        private void ShowRecipe(RecipeViewModel selectedRecipe)
        {
            if (selectedRecipe != null)
            {
                (this.DataContext as CookingBookViewModel).SelectedRecipe = selectedRecipe;
                this.stackPanelInstruction.Children.Clear();
                foreach (InstructionViewModel instruction in selectedRecipe.Instructions)
                {
                    DockPanel panel = new DockPanel();
                    Image image = new Image()
                    {
                        MaxWidth = 300,
                        Stretch = Stretch.UniformToFill,
                        Source = (new ImageSourceConverter().ConvertFromString(instruction.ImageSource) as ImageSource)
                    };
                    DockPanel.SetDock(image, Dock.Left);
                    panel.Children.Add(image);
                    panel.Children.Add(new TextBlock { Text = instruction.Name, TextWrapping = TextWrapping.Wrap });
                    this.stackPanelInstruction.Children.Add(panel);
                    this.stackPanelInstruction.Children.Add(new Separator());
                }
            }
        }

        private void AddRecipeClick(object sender, RoutedEventArgs e)
        {
            AddRecipe addForm = new AddRecipe();
            addForm.ShowDialog();
            RecipeViewModel selectedRecipe = (this.DataContext as CookingBookViewModel).SelectedRecipe;
            RefreshDataContext(selectedRecipe);
        }

        
        private void EditRecipeClick(object sender, RoutedEventArgs e)
        {
            RecipeViewModel selectedRecipe = (this.DataContext as CookingBookViewModel).SelectedRecipe;
            EditRecipe editForm = new EditRecipe(selectedRecipe);
            editForm.ShowDialog();
            RefreshDataContext(selectedRecipe);
        }

        private RecipeViewModel FindRecipe(RecipeViewModel recipeView)
        {
            int index = 0;
            for ( int i=0;i< this.listBoxFilteredRecipes.Items.Count;i++)
            {
                if ((this.listBoxFilteredRecipes.Items[i] as RecipeViewModel).Name == recipeView.Name)
                {
                    index = i;break;
                }
            }
            return this.listBoxFilteredRecipes.Items[index] as RecipeViewModel;
        }

        private void DeleteRecipeClick(object sender, RoutedEventArgs e)
        {
            RecipeViewModel selectedRecipe = (this.DataContext as CookingBookViewModel).SelectedRecipe;
            string name = selectedRecipe.Name;
            
            if (MessageBox.Show($"You really want delete {name}","Warning",MessageBoxButton.YesNo,MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (this.listBoxFilteredRecipes.Items.Count > 1)
                {
                    using (CookingBookContext db = new CookingBookContext())
                    {
                        Recipe recipe = (from rec in db.Recipes
                                         where rec.Name == name
                                         select rec).FirstOrDefault();

                        db.Recipes.Remove(recipe);
                        db.SaveChanges();
                    }
                }
                else
                {
                    MessageBox.Show("You cant delete last recipe", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            RefreshDataContext(selectedRecipe);
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RefreshDataContext(RecipeViewModel selectedRecipe)
        {
            this.DataContext = new CookingBookViewModel();
            this.listBoxCategory.Items.Clear();
            CheckBox box = new CheckBox() { Content = "All/Clear", IsChecked = true, Foreground = Brushes.Blue }; box.Click += CheckBoxClickAllCategory;
            this.listBoxCategory.Items.Add(box);
            this.listBoxKitchen.Items.Clear();
            box = new CheckBox() { Content = "All/Clear", IsChecked = true, Foreground = Brushes.Blue }; box.Click += CheckBoxClickAllKitchen;
            this.listBoxKitchen.Items.Add(box);
            this.listBoxIngridients.Items.Clear();
            box = new CheckBox() { Content = "All/Clear", IsChecked = true, Foreground = Brushes.Blue }; box.Click += CheckBoxClickAllIngridients;
            this.listBoxIngridients.Items.Add(box);
            InitializeDynamicComponent();
            RecipeViewModel newSelectedRecipe = FindRecipe(selectedRecipe);
            ShowRecipe(newSelectedRecipe);
        }

        private void SaveToPdfClick(object sender, RoutedEventArgs e)
        {
            
            Document document = CreateDocument();
            document.UseCmykColor = true;
            const bool unicode = true;
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always;
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();
            const string filename ="Recipe.pdf";
            pdfRenderer.PdfDocument.Save(filename);
            Process.Start(filename);
        }

        private Document CreateDocument()
        {
            RecipeViewModel selectedRecipe = (this.DataContext as CookingBookViewModel).SelectedRecipe;
            Document document = new Document();
            MigraDoc.DocumentObjectModel.Section section = document.AddSection();
            section.PageSetup.TopMargin = 10;
            section.PageSetup.BottomMargin = 10;
            MigraDoc.DocumentObjectModel.Paragraph paragraphName = section.AddParagraph();
            paragraphName.AddFormattedText(selectedRecipe.Name);
            paragraphName.Format.Font.Size = 30;
            paragraphName.Format.Alignment = ParagraphAlignment.Center;

            MigraDoc.DocumentObjectModel.Paragraph paragraphComment = section.AddParagraph();
            paragraphComment.AddFormattedText(selectedRecipe.Category + "," + selectedRecipe.Kitchen);
            paragraphComment.Format.Alignment = ParagraphAlignment.Center;
            paragraphComment.Format.Font.Size = 10;
            paragraphComment.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Blue;

            MigraDoc.DocumentObjectModel.Shapes.Image mainImage = section.AddImage(selectedRecipe.MainPictureAdress);
            mainImage.Width = 500;


            MigraDoc.DocumentObjectModel.Paragraph paragraphDescriptions = section.AddParagraph();
            paragraphDescriptions.AddFormattedText(selectedRecipe.Description);
            paragraphDescriptions.AddLineBreak();

            MigraDoc.DocumentObjectModel.Paragraph paragraphIngridients = section.AddParagraph();
            paragraphIngridients.Format.Alignment = ParagraphAlignment.Left;
            paragraphIngridients.Format.Font.Italic = true;
            paragraphIngridients.Format.Font.Color = MigraDoc.DocumentObjectModel.Colors.Blue;
            paragraphIngridients.AddLineBreak();
            paragraphIngridients.AddFormattedText("Ingridients: ");
            paragraphIngridients.AddLineBreak();
            foreach (IngridientViewModel ingridientView in selectedRecipe.Ingridients)
            {
                paragraphIngridients.AddLineBreak();
                paragraphIngridients.AddFormattedText(ingridientView.ToString());
            }
            paragraphIngridients.AddLineBreak();

            MigraDoc.DocumentObjectModel.Paragraph InstructionParagraph = section.AddParagraph();
            InstructionParagraph.Format.KeepTogether = false;
            MigraDoc.DocumentObjectModel.Shapes.Image instructionImage;
            
            InstructionParagraph.AddLineBreak();
            InstructionParagraph.AddFormattedText("Istructions:",TextFormat.Italic);
            InstructionParagraph.AddLineBreak();
            for (int i = 0; i < selectedRecipe.Instructions.Count; i++)
            {
                InstructionParagraph.AddFormattedText($"{ i + 1}) " + selectedRecipe.Instructions[i].Name);
                InstructionParagraph.AddLineBreak();
                instructionImage = InstructionParagraph.AddImage(selectedRecipe.Instructions[i].ImageSource);
                instructionImage.Height = 200;
                InstructionParagraph.AddLineBreak();
            }

            return document;
        }
    }
}
