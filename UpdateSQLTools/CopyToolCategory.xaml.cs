/* Title:           Copy Tool Category
 * Date:            1-3-18
 * Author:          Terry Holmes
 * 
 * Description:     This is used to copy access tool categories over */

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
using NewEventLogDLL;
using ToolCategoryDLL;

namespace UpdateSQLTools
{
    /// <summary>
    /// Interaction logic for CopyToolCategory.xaml
    /// </summary>
    public partial class CopyToolCategory : Window
    {
        //setting up the classes
        WPFMessagesClass TheMesssagesClass = new WPFMessagesClass();
        AccessToolClass TheAccessToolClass = new AccessToolClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        ToolCategoryClass TheToolCategoryClass = new ToolCategoryClass();

        //setting up the data
        FindToolCategoryByCategoryDataSet TheFindToolCategoryByCategoryDataSet = new FindToolCategoryByCategoryDataSet();
        AccessToolCategoryDataSet TheAccessToolCategoryDataSet;

        public CopyToolCategory()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMesssagesClass.CloseTheProgram();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TheAccessToolCategoryDataSet = TheAccessToolClass.GetAccessToolCategory();

            dgrResults.ItemsSource = TheAccessToolCategoryDataSet.toolcategory;
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            int intRecordsReturned;
            bool blnFatalError;
            string strToolCategory;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                intNumberOfRecords = TheAccessToolCategoryDataSet.toolcategory.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strToolCategory = TheAccessToolCategoryDataSet.toolcategory[intCounter].ToolCategory;

                    TheFindToolCategoryByCategoryDataSet = TheToolCategoryClass.FindToolCategoryByCategory(strToolCategory);

                    intRecordsReturned = TheFindToolCategoryByCategoryDataSet.FindToolCategoryByCategory.Rows.Count;

                    if(intRecordsReturned == 0)
                    {
                        blnFatalError = TheToolCategoryClass.InsertToolCategory(strToolCategory);

                        if (blnFatalError == true)
                            throw new Exception();
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Copy Tool Category // Process Button " + Ex.Message);
            }

            PleaseWait.Close();
        }
    }
}
