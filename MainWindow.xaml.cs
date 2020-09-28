using CookingBook.DataLayer.Contexts;
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
            using (CookingBookContext db = new CookingBookContext())
            {
                var categoryList = from category in db.Categories
                                   select category.Name;
                List<string> ctegories = categoryList.ToList();
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
    }
}
