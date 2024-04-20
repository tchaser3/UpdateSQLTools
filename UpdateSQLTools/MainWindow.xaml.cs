/* Title:           Update SQL Tools - Main Menu
 * Date:            1-3-18
 * Author:          Terry Holmes
 * 
 * Description:     This is the main window to bring over the tools into SQL */

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
using DataValidationDLL;
using NewEmployeeDLL;
using NewEventLogDLL;

namespace UpdateSQLTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        DataValidationClass TheDataValidationClass = new DataValidationClass();
        EmployeeClass TheEmployeeClass = new EmployeeClass();
        EventLogClass TheEventLogClass = new EventLogClass();

        public static VerifyLogonDataSet TheVerifyLogonDataSet = new VerifyLogonDataSet();

        int gintNoOfMisses;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void btnSignOn_Click(object sender, RoutedEventArgs e)
        {
            //setting local variables
            string strValueForValidation;
            int intEmployeeID = 0;
            string strLastName;
            string strErrorMessage = "";
            bool blnThereIsAProblem = false;
            bool blnFatalError = false;
            int intRecordsReturned;

            try
            {
                //data validation
                strValueForValidation = pbxEmployeeID.Password;
                blnThereIsAProblem = TheDataValidationClass.VerifyIntegerData(strValueForValidation);
                if (blnThereIsAProblem == true)
                {
                    blnFatalError = true;
                    strErrorMessage += "The Employee ID Is Not An Integer\n";
                }
                else
                {
                    intEmployeeID = Convert.ToInt32(strValueForValidation);
                }
                strLastName = txtLastName.Text;
                if (strLastName == "")
                {
                    blnFatalError = true;
                    strErrorMessage += "The Last Name Was Not Entered";
                }
                if (blnFatalError == true)
                {
                    TheMessagesClass.ErrorMessage(strErrorMessage);
                    return;
                }

                //getting the data
                TheVerifyLogonDataSet = TheEmployeeClass.VerifyLogon(intEmployeeID, strLastName);

                intRecordsReturned = TheVerifyLogonDataSet.VerifyLogon.Rows.Count;

                if (intRecordsReturned == 0)
                {
                    LogonFailed();
                }
                else
                {
                    if ((TheVerifyLogonDataSet.VerifyLogon[0].EmployeeGroup != "ADMIN") && (TheVerifyLogonDataSet.VerifyLogon[0].EmployeeGroup != "IT"))
                    {
                        LogonFailed();
                    }
                    else
                    {
                        MainMenu MainMenu = new MainMenu();
                        MainMenu.Show();
                        Hide();
                    }
                }
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Main Window // Sign On Button // " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            TheMessagesClass.CloseTheProgram();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gintNoOfMisses = 0;

            pbxEmployeeID.Focus();
        }
        private void LogonFailed()
        {
            gintNoOfMisses++;

            if (gintNoOfMisses == 3)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "There Have Been Three Attempts to Sign Into Update SQL Tools");

                TheMessagesClass.ErrorMessage("There Have Been Three Attempts To Sign In\nThe Program Will Shut Down");

                Application.Current.Shutdown();
            }
            else
            {
                TheMessagesClass.InformationMessage("You Have Failed The Sign In Process");
                return;
            }
        }
    }
}
