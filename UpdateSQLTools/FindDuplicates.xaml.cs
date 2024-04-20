/* Title:           Find Duplicates
 * Date:            1-4-18
 * Author:          Terry Holmes
 * 
 * Description:     This will find duplicates in Access Tools */

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

namespace UpdateSQLTools
{
    /// <summary>
    /// Interaction logic for FindDuplicates.xaml
    /// </summary>
    public partial class FindDuplicates : Window
    {
        //setting the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        AccessToolClass TheAccessToolClass = new AccessToolClass();
        EventLogClass TheEventLogClass = new EventLogClass();

        AccessToolsDataSet TheAccessToolsDataSet;
        DuplicateToolsDataSet TheDuplicateToolsDataSet = new DuplicateToolsDataSet();

        int gintDupCounter;
        int gintDupUpperLimit;

        public FindDuplicates()
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

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainMenu MainMenu = new MainMenu();
            MainMenu.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //setting local variables
            int intCounter;
            int intNumberOfRecords;
            string strToolID;
            int intToolKey;
            DateTime datCreationDate;
            int intDupCounter;
            int intSecondCounter;
            int intSecondToolKey;
            bool blnAddItem;

            PleaseWait PleaseWait = new PleaseWait();
            PleaseWait.Show();

            try
            {
                TheAccessToolsDataSet = TheAccessToolClass.GetAccessTools();
                intNumberOfRecords = TheAccessToolsDataSet.tools.Rows.Count - 1;
                gintDupCounter = 0;
                gintDupUpperLimit = 0;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    strToolID = TheAccessToolsDataSet.tools[intCounter].ToolID;
                    intToolKey = TheAccessToolsDataSet.tools[intCounter].ToolKey;
                    datCreationDate = TheAccessToolsDataSet.tools[intCounter].Date;

                    for(intSecondCounter = 0; intSecondCounter <= intNumberOfRecords; intSecondCounter++)
                    {
                        if(strToolID == TheAccessToolsDataSet.tools[intSecondCounter].ToolID)
                        {
                            if(intToolKey != TheAccessToolsDataSet.tools[intSecondCounter].ToolKey)
                            {
                                blnAddItem = true;
                                intSecondToolKey = TheAccessToolsDataSet.tools[intSecondCounter].ToolKey;

                                if(gintDupCounter > 0)
                                {
                                    for (intDupCounter = 0; intDupCounter <= gintDupUpperLimit; intDupCounter++)
                                    {
                                        if (intSecondToolKey == TheDuplicateToolsDataSet.duplicatetools[intDupCounter].ToolKey)
                                        {
                                            blnAddItem = false;
                                        }
                                    }
                                }

                                if (blnAddItem == true)
                                {
                                    DuplicateToolsDataSet.duplicatetoolsRow NewDuplicateRow = TheDuplicateToolsDataSet.duplicatetools.NewduplicatetoolsRow();

                                    if (TheAccessToolsDataSet.tools[intSecondCounter].Active == "YES")
                                    {
                                        NewDuplicateRow.Active = true;
                                    }
                                    else
                                    {
                                        NewDuplicateRow.Active = false;
                                    }
                                    if (TheAccessToolsDataSet.tools[intSecondCounter].Available== "YES")
                                    {
                                        NewDuplicateRow.Available = true;
                                    }
                                    else
                                    {
                                        NewDuplicateRow.Available = false;
                                    }
                                    NewDuplicateRow.Description = TheAccessToolsDataSet.tools[intSecondCounter].Description;
                                    NewDuplicateRow.ToolID = strToolID;
                                    NewDuplicateRow.ToolKey = intSecondToolKey;

                                    TheDuplicateToolsDataSet.duplicatetools.Rows.Add(NewDuplicateRow);
                                    gintDupUpperLimit = gintDupCounter;
                                    gintDupCounter++;
                                    
                                }
                            }
                        }
                    }
                }

                dgrResults.ItemsSource = TheDuplicateToolsDataSet.duplicatetools;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Find Duplicates // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            PleaseWait.Close();
        }
    }
}
