/* Title:           Access Tool Class
 * Date:            1-3-18
 * Author:          Terry Holmes
 * 
 * Description:     This is used to bring the access in */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewEventLogDLL;

namespace UpdateSQLTools
{
    class AccessToolClass
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();

        AccessToolCategoryDataSet aAccessToolCategoryDataSet;
        AccessToolCategoryDataSetTableAdapters.toolcategoryTableAdapter aAccessToolCategoryTableAdapter;

        AccessToolsDataSet aAccessToolsDataSet;
        AccessToolsDataSetTableAdapters.toolsTableAdapter aAccessToolsTableAdapter;

        AccessToolHistoryDataSet aAccessToolHistoryDataSet;
        AccessToolHistoryDataSetTableAdapters.toolhistoryTableAdapter aAccessToolHistoryTableAdatper;

        public AccessToolHistoryDataSet GetAccessToolHistory()
        {
            try
            {
                aAccessToolHistoryDataSet = new AccessToolHistoryDataSet();
                aAccessToolHistoryTableAdatper = new AccessToolHistoryDataSetTableAdapters.toolhistoryTableAdapter();
                aAccessToolHistoryTableAdatper.Fill(aAccessToolHistoryDataSet.toolhistory);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Access Tools Class // Get Access Tool History " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            return aAccessToolHistoryDataSet;
        }
        public AccessToolsDataSet GetAccessTools()
        {
            try
            {
                aAccessToolsDataSet = new AccessToolsDataSet();
                aAccessToolsTableAdapter = new AccessToolsDataSetTableAdapters.toolsTableAdapter();
                aAccessToolsTableAdapter.Fill(aAccessToolsDataSet.tools);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Access Tools Class // Get Access Tools " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            return aAccessToolsDataSet;
        }
        public AccessToolCategoryDataSet GetAccessToolCategory()
        {
            try
            {
                aAccessToolCategoryDataSet = new AccessToolCategoryDataSet();
                aAccessToolCategoryTableAdapter = new AccessToolCategoryDataSetTableAdapters.toolcategoryTableAdapter();
                aAccessToolCategoryTableAdapter.Fill(aAccessToolCategoryDataSet.toolcategory);
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Update SQL Tools // Access Tool Class // Get Access Tool Category " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }

            return aAccessToolCategoryDataSet;
        }

    }
}
