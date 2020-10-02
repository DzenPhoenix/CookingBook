using CookingBook.DataLayer.Contexts;
using CookingBook.Service;
using CookingBook.View;
using CookingBook.ViewModel;
using System;
using System.Collections.Generic;
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

        private void AddRecipeClick(object sender, RoutedEventArgs e)
        {
            AddRecipe addForm = new AddRecipe();
            addForm.ShowDialog();
            this.DataContext = new CookingBookViewModel();
            this.listBoxCategory.Items.Clear();
            this.listBoxKitchen.Items.Clear();
            this.listBoxIngridients.Items.Clear();
            InitializeDynamicComponent();
        }

        
        private void EditRecipeClick(object sender, RoutedEventArgs e)
        {
            EditRecipe editForm = new EditRecipe((this.DataContext as CookingBookViewModel).SelectedRecipe);
            editForm.ShowDialog();
            this.DataContext = new CookingBookViewModel();
            this.listBoxCategory.Items.Clear();
            this.listBoxKitchen.Items.Clear();
            this.listBoxIngridients.Items.Clear();
            InitializeDynamicComponent();
        }
    }
}
