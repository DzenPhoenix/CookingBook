using CookingBook.DataLayer.Contexts;
using CookingBook.Service;
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
                CheckBox box = new CheckBox() { Content =category, IsChecked = false,Foreground= Brushes.Blue };
                this.listBoxCategory.Items.Add(box);
            }
            foreach (string kitchen in (this.DataContext as CookingBookViewModel).Kitchens)
            {
                CheckBox box = new CheckBox() { Content = kitchen, IsChecked = false, Foreground = Brushes.Blue };
                this.listBoxKitchen.Items.Add(box);
            }

            foreach (string ingridient in (this.DataContext as CookingBookViewModel).Ingridients)
            {
                CheckBox box = new CheckBox() { Content = ingridient, IsChecked = false, Foreground = Brushes.Blue };
                this.listBoxIngridients.Items.Add(box);
            }

            foreach (RecipeViewModel recipeView in (this.DataContext as CookingBookViewModel).FilteredRecipes)
            {
                ListBoxItem item = new ListBoxItem() { Content = recipeView.Name };
                this.listBoxFilteredRecipes.Items.Add(item);
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
                    filter.Categories.Add(box.Name);
                }
            }

            foreach (CheckBox box in this.listBoxKitchen.Items)
            {
                if (box.IsChecked == true)
                {
                    filter.Kitchens.Add(box.Name);
                }
            }

            foreach (CheckBox box in this.listBoxIngridients.Items)
            {
                if (box.IsChecked == true)
                {
                    filter.Ingridients.Add(box.Name);
                }
            }
            (this.DataContext as CookingBookViewModel).Filter = filter;
        }
    }
}
