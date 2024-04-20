/* Title:           Main Menu
 * Date:            1-3-18
 * Author:          Terry Holmes
 * 
 * Description:     This the main menu */

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
using System.Windows.Shapes;

namespace UpdateSQLTools
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        //setting up the class
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnCopyToolCategory_Click(object sender, RoutedEventArgs e)
        {
            CopyToolCategory CopyToolCategory = new CopyToolCategory();
            CopyToolCategory.Show();
            Close();
        }

        private void btnCopyTools_Click(object sender, RoutedEventArgs e)
        {
            CopyTools CopyTools = new CopyTools();
            CopyTools.Show();
            Close();
        }

        private void btnFindDuplicates_Click(object sender, RoutedEventArgs e)
        {
            FindDuplicates FindDuplicates = new FindDuplicates();
            FindDuplicates.Show();
            Close();
        }

        private void btnCopyToolHistory_Click(object sender, RoutedEventArgs e)
        {
            CopyToolHistory CopyToolHistory = new CopyToolHistory();
            CopyToolHistory.Show();
            Close();
        }
    }
}
