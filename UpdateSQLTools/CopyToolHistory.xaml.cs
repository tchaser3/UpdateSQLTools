/* Title:           Copy Tool History
 * Date:            1-5-18
 * Author:          Terry Holmes
 * 
 * Description:     This form is used to copy tool history */

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
using NewToolsDLL;
using ToolHistoryDLL;


namespace UpdateSQLTools
{
    /// <summary>
    /// Interaction logic for CopyToolHistory.xaml
    /// </summary>
    public partial class CopyToolHistory : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        AccessToolClass TheAccessToolClass = new AccessToolClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        ToolsClass TheToolsClass = new ToolsClass();
        ToolHistoryClass TheToolHistoryClass = new ToolHistoryClass();

        //setting up stored procedure
        ManuallyInsertToolHistoryEntryTableAdapters.QueriesTableAdapter aManuallyInsertToolHistoryTableAdapter;

        //setting up data
        AccessToolHistoryDataSet TheAccessToolHistoryDataSet;
        FindSpecificToolByToolIDDataSet TheFindSpecificToolByToolIDDataSet = new FindSpecificToolByToolIDDataSet();
        ToolHistoryDataSet TheToolHistoryDataSet = new ToolHistoryDataSet();

        int gintHistoryCounter;
        int gintHistoryUpperLimit;

        public CopyToolHistory()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private bool ManuallyInsertToolHistory(int intToolKey, DateTime datTransactionDate, int intEmployeeID, int intWarehouseEmployeeID, string strNotes)
        {
            bool blnFatalError = false;

            try
            {
                aManuallyInsertToolHistoryTableAdapter = new ManuallyInsertToolHistoryEntryTableAdapters.QueriesTableAdapter();
                aManuallyInsertToolHistoryTableAdapter.InsertToolHistory(intToolKey, datTransactionDate, intEmployeeID, intWarehouseEmployeeID, strNotes);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Table // Copy Tool History // Manually Insert Tool History " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());

                blnFatalError = true;
            }

            return blnFatalError;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TheAccessToolHistoryDataSet = TheAccessToolClass.GetAccessToolHistory();

            dgrResults.ItemsSource = TheAccessToolHistoryDataSet.toolhistory;
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            int intCounter;
            int intNumberOfRecords;
            int intToolKey;
            string strToolID;
            DateTime datTransactionDate;
            int intEmployeeID;
            int intWarehouseEmployeeID;
            string strNotes;
            int intRecordsReturned;
            bool blnItemFound;
            bool blnFatalError;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                intNumberOfRecords = TheAccessToolHistoryDataSet.toolhistory.Rows.Count - 1;
               
                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strToolID = TheAccessToolHistoryDataSet.toolhistory[intCounter].ToolID;

                    TheFindSpecificToolByToolIDDataSet = TheToolsClass.FindSpecificToolByToolID(strToolID);

                    intRecordsReturned = TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID.Rows.Count;
                        
                    if(intRecordsReturned > 0)
                    {
                        intToolKey = TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[0].ToolKey;
                        datTransactionDate = TheAccessToolHistoryDataSet.toolhistory[intCounter].Date;
                        intEmployeeID = TheAccessToolHistoryDataSet.toolhistory[intCounter].EmployeeID;
                        intWarehouseEmployeeID = TheAccessToolHistoryDataSet.toolhistory[intCounter].WarehouseEmployeeID;
                        strNotes = TheAccessToolHistoryDataSet.toolhistory[intCounter].Availablity;

                        blnItemFound = CheckForHistory(intToolKey, datTransactionDate);

                        if(blnItemFound == false)
                        {
                            blnFatalError = ManuallyInsertToolHistory(intToolKey, datTransactionDate, intEmployeeID, intWarehouseEmployeeID, strNotes);

                            if (blnFatalError == true)
                                throw new Exception();
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Copy Tool History // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
        private bool CheckForHistory(int intToolKey, DateTime datTransactionDate)
        {
            bool blnItemFound = false;
            int intCounter;
            int intNumberOfRecords;

            TheToolHistoryDataSet = TheToolHistoryClass.GetToolHistoryInfo();

            intNumberOfRecords = TheToolHistoryDataSet.toolhistory.Rows.Count - 1;

            for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
            {
                if(intToolKey == TheToolHistoryDataSet.toolhistory[intCounter].ToolKey)
                {
                    if(datTransactionDate == TheToolHistoryDataSet.toolhistory[intCounter].TransactionDate)
                    {
                        blnItemFound = true;
                    }
                }
            }


            return blnItemFound;
        }
    }
}
