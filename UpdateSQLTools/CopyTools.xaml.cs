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
using NewToolsDLL;
using ToolCategoryDLL;
using NewEventLogDLL;

namespace UpdateSQLTools
{
    /// <summary>
    /// Interaction logic for CopyTools.xaml
    /// </summary>
    public partial class CopyTools : Window
    {
        //setting the class
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        ToolsClass TheToolsClass = new ToolsClass();
        ToolCategoryClass TheToolCategoryClass = new ToolCategoryClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        AccessToolClass TheAccessToolClass = new AccessToolClass();

        //setting up data
        AccessToolsDataSet TheAccessToolsDataSet;
        FindSpecificToolByToolIDDataSet TheFindSpecificToolByToolIDDataSet = new FindSpecificToolByToolIDDataSet();
        FindToolCategoryByCategoryDataSet TheFindToolCategoryByCategoryDataSet = new FindToolCategoryByCategoryDataSet();
        ToolsDataSet TheToolsDataSet;

        ManuallyInsertToolsEntryTableAdapters.QueriesTableAdapter aManullyInsertToolsTableAdapter;

        public CopyTools()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                TheAccessToolsDataSet = TheAccessToolClass.GetAccessTools();

                dgrResults.ItemsSource = TheAccessToolsDataSet.tools;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Copy Tools // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
            
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private bool ManuallyInsertTools(string strToolID, int intEmployeeID, DateTime datCreationDate, string strPartNumber, int intCategoryID, string strDescription, decimal decToolCost, bool blnAvailable, bool blnActive, string strNotes)
        {
            bool blnFatalError = false;

            try
            {
                aManullyInsertToolsTableAdapter = new ManuallyInsertToolsEntryTableAdapters.QueriesTableAdapter();
                aManullyInsertToolsTableAdapter.InsertTools(strToolID, intEmployeeID, datCreationDate, strPartNumber, intCategoryID, strDescription, DateTime.Now, decToolCost, blnAvailable, blnActive, 0, strNotes);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Copy Tools // Manually Insert Tools " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());

                blnFatalError = true;
            }

            return blnFatalError;
        }
        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            int intNumberOfRecords;
            int intCounter;
            int intRecordsReturned;
            string strToolCategory;
            int intCategoryID;
            string strToolID;
            int intEmployeeID;
            DateTime datCreationDate;
            DateTime datTransactionDate;
            string strPartNumber;
            string strDescription;
            decimal decToolCost;
            bool blnAvailable = true;
            bool blnActive = true;
            int intCurrentLocation;
            string strToolNotes;
            int intSecondCounter;
            bool blnItemFound;
            int intToolKey;
            bool blnFatalError;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                intNumberOfRecords = TheAccessToolsDataSet.tools.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strToolID = TheAccessToolsDataSet.tools[intCounter].ToolID;
                    strToolCategory = TheAccessToolsDataSet.tools[intCounter].Type;
                    blnItemFound = false;

                    //getting the id
                    TheFindToolCategoryByCategoryDataSet = TheToolCategoryClass.FindToolCategoryByCategory(strToolCategory);

                    intCategoryID = TheFindToolCategoryByCategoryDataSet.FindToolCategoryByCategory[0].CategoryID;
                    intEmployeeID = TheAccessToolsDataSet.tools[intCounter].EmployeeID;
                    datCreationDate = TheAccessToolsDataSet.tools[intCounter].Date;
                    datTransactionDate = DateTime.Now;
                    strPartNumber = TheAccessToolsDataSet.tools[intCounter].PartNumber;
                    strDescription = TheAccessToolsDataSet.tools[intCounter].Description;
                    decToolCost = TheAccessToolsDataSet.tools[intCounter].Value;
                    if(TheAccessToolsDataSet.tools[intCounter].Available == "YES")
                    {
                        blnAvailable = true;
                    }
                    else
                    {
                        blnAvailable = false;
                    }
                    if(TheAccessToolsDataSet.tools[intCounter].Active == "YES")
                    {
                        blnActive = true;
                    }
                    else
                    {
                        blnAvailable = false;
                    }
                    intCurrentLocation = 0;
                    if(TheAccessToolsDataSet.tools[intCounter].IsNotesNull() == true)
                    {
                        strToolNotes = "NO NOTES FOUND";
                    }
                    else
                    {
                        strToolNotes = TheAccessToolsDataSet.tools[intCounter].Notes;
                    }

                    TheFindSpecificToolByToolIDDataSet = TheToolsClass.FindSpecificToolByToolID(strToolID);

                    intRecordsReturned = TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID.Rows.Count - 1;

                    if(blnActive == false)
                    {
                        blnItemFound = true;
                    }

                    if(intRecordsReturned > -1)
                    {
                        for(intSecondCounter = 0; intSecondCounter <= intRecordsReturned; intSecondCounter++)
                        {
                            if(strToolID == TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[intSecondCounter].ToolID)
                            {
                                if(TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[intSecondCounter].CreationDate == datCreationDate)
                                {
                                    blnItemFound = true;
                                    intToolKey = TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[intSecondCounter].ToolKey;

                                    if(blnAvailable != TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[intSecondCounter].Available)
                                    {
                                        blnFatalError = TheToolsClass.UpdateToolAvailability(intToolKey, blnAvailable);

                                        if (blnFatalError == true)
                                            throw new Exception();
                                    }

                                    if(intEmployeeID != TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[intSecondCounter].EmployeeID)
                                    {
                                        if (blnAvailable == false)
                                            strToolNotes = "TOOL SIGNED OUT";
                                        else if (blnAvailable == true)
                                            strToolNotes = "TOOL SIGNED IN";

                                        blnFatalError = TheToolsClass.UpdateToolSignOut(intToolKey, intEmployeeID, blnAvailable, strToolNotes);

                                        if (blnFatalError == true)
                                            throw new Exception();
                                    }

                                    if(blnActive != TheFindSpecificToolByToolIDDataSet.FindSpecificToolByToolID[intSecondCounter].ToolActive)
                                    {
                                        blnFatalError = TheToolsClass.UpdateToolActive(intToolKey, blnActive);
                                    }
                                }
                                else
                                {
                                    blnItemFound = true;
                                }
                            }
                        }
                    }

                    if(blnItemFound == false)
                    {
                        blnFatalError = ManuallyInsertTools(strToolID, intEmployeeID, datCreationDate, strPartNumber, intCategoryID, strDescription, decToolCost, blnAvailable, blnActive, strToolNotes);

                        if (blnFatalError == true)
                            throw new Exception();

                    }
                }

                TheToolsDataSet = TheToolsClass.GetToolsInfo();

                dgrResults.ItemsSource = TheToolsDataSet.tools;

            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Copy Tools // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
    }
}
