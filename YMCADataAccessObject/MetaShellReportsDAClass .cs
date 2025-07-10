//****************************************************
//Modification History
//****************************************************
//Modified by         Date            Description
//****************************************************
//Manthan Rajguru     2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
     public sealed class MetaShellReportsDAClass
    {
        public static DataSet LookupReports()
        {
            DataSet dsLookUpPrinters = null;
            Database db = null;
            DbCommand commandLookUpOPrinters = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpOPrinters = db.GetStoredProcCommand("yrs_usp_MetaShell_LookupReports");

                if (commandLookUpOPrinters == null) return null;

                dsLookUpPrinters = new DataSet("Report Details");
                l_TableNames = new string[] { "Reports", "PaperTypes" };
                dsLookUpPrinters.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(commandLookUpOPrinters, dsLookUpPrinters, l_TableNames);

                return dsLookUpPrinters;
            }
            catch
            {
                throw;
            }

        }
        public static void InsertReport(DataSet parameterReports)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_InsertReports");

                db.AddInParameter(insertCommandWrapper, "@int_ReportID", DbType.Int64, "ReportID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@int_PaperTypeID", DbType.Int64, "PaperTypeID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_ReportName", DbType.String, "ReportName", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_ModuleName", DbType.String, "ModuleName", DataRowVersion.Current);


                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_UpdateReports");

                db.AddInParameter(updateCommandWrapper, "@int_ReportID", DbType.Int64, "ReportID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@int_PaperTypeID", DbType.Int64, "PaperTypeID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_ReportName", DbType.String, "ReportName", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_ModuleName", DbType.String, "ModuleName", DataRowVersion.Current);

                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_DeleteReports");

                db.AddInParameter(deleteCommandWrapper, "@int_ReportID", DbType.Int64, "ReportID", DataRowVersion.Original);




                if (parameterReports != null)
                {
                    db.UpdateDataSet(parameterReports, "Reports", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }
        public static DataSet SearchReport(string parameterSearchReport)
        {
            DataSet dsSearchCountryTypes = null;
            Database db = null;
            DbCommand SearchCommandWrapper = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                SearchCommandWrapper = db.GetStoredProcCommand("yrs_usp_SearchPrinters");

                if (SearchCommandWrapper == null) return null;

                db.AddInParameter(SearchCommandWrapper, "@varchar_PrinterName", DbType.String, parameterSearchReport);
                //							
                dsSearchCountryTypes = new DataSet();
                dsSearchCountryTypes.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(SearchCommandWrapper, dsSearchCountryTypes, "Printer");

                return dsSearchCountryTypes;
            }
            catch
            {
                throw;
            }
        }
    }
}
