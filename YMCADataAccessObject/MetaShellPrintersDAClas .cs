//****************************************************
//Modification History
//****************************************************
//Modified by         Date             Description
//****************************************************
//Manthan Rajguru     2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace YMCARET.YmcaDataAccessObject
{
    public sealed class MetaShellPrintersDAClass
    {
        public static DataSet GetUserShellPrinters(Int64 IntUserId, string reportName)
        {
            DataSet dsGetUserShellPrinters = null;
            Database db = null;
            DbCommand commandGetUserShellPrinters = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandGetUserShellPrinters = db.GetStoredProcCommand("yrs_usp_GetShellPrinters");

                if (commandGetUserShellPrinters == null) return null;
                dsGetUserShellPrinters = new DataSet();
                db.AddInParameter(commandGetUserShellPrinters, "@IntUserID", DbType.Int64, IntUserId);
                db.AddInParameter(commandGetUserShellPrinters, "@ReportName", DbType.String, reportName);
                dsGetUserShellPrinters.Locale = CultureInfo.InvariantCulture;
                l_TableNames = new string[] { "Printers","UserPrinter"};
                db.LoadDataSet(commandGetUserShellPrinters, dsGetUserShellPrinters, l_TableNames);

                return dsGetUserShellPrinters;
            }
            catch
            {
                throw;
            }

        }
        public static void InsertUserPrinter(Int64 IntUserId, Int64 IntReportId, Int64 IntPrintId)
        {
            Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;

            try
            {
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                if (l_DataBase == null) return;

                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("yrs_usp_InsertUserPrinter");

                if (l_DBCommandWrapper == null) return;

                l_DataBase.AddInParameter(l_DBCommandWrapper, "@IntUserID", DbType.Int64, IntUserId);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@IntReportId", DbType.Int64, IntReportId);
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@IntPrintID", DbType.Int64, IntPrintId);
                
               
                l_DataBase.ExecuteNonQuery(l_DBCommandWrapper);

            }
            catch (Exception ex)
            {
                throw ex;
            }



        }
        public static DataSet LookupPrinters()
        {
            DataSet dsLookUpPrinters = null;
            Database db = null;
            DbCommand commandLookUpOPrinters = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpOPrinters = db.GetStoredProcCommand("yrs_usp_MetaShell_LookupPrinters");

                if (commandLookUpOPrinters == null) return null;
                
                dsLookUpPrinters = new DataSet("Printers Details");
                l_TableNames = new string[] { "Printers", "PrinterPaperTypes","PaperTypes"};
                dsLookUpPrinters.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(commandLookUpOPrinters, dsLookUpPrinters, l_TableNames);

                return dsLookUpPrinters;
            }
            catch
            {
                throw;
            }

        }
        public static string InsertPrinter(DataSet parameterPrinters, string[] parameterListPaperId)
        {
            Database db = null;
            string l_string_Output = "";
            Int64 l_NewPrinterID=0;
            DbCommand insertCommandWrapper = null;
            //DbCommand updateCommandWrapper = null;
            //DbCommand deleteCommandWrapper = null;

            DbTransaction l_IDbTransaction = null;
            DbConnection l_IDbConnection = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");


                if (db == null) return null;


                l_IDbConnection = db.CreateConnection();
                l_IDbConnection.Open();
                if (l_IDbConnection == null) return null;

                l_IDbTransaction = l_IDbConnection.BeginTransaction();




                foreach (DataRow l_DataRow in parameterPrinters.Tables["Printers"].Rows)
                {


                    if (l_DataRow.RowState.ToString() == "Added")
                    {
                        insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_InsertPrinters");
                        db.AddInParameter(insertCommandWrapper, "@IntPrintID", DbType.Int64, 0);
                        db.AddInParameter(insertCommandWrapper, "@varchar_PrinterName", DbType.String, Convert.ToString(l_DataRow["PrinterName"]));
                        db.AddInParameter(insertCommandWrapper, "@varchar_PrinterDescription", DbType.String, Convert.ToString(l_DataRow["PrinterDescription"]));
                        db.AddOutParameter(insertCommandWrapper, "@sOutput", DbType.String, 1000);
                        db.AddOutParameter(insertCommandWrapper, "@IntPrintIDOut", DbType.Int64, 0);

                        db.ExecuteNonQuery(insertCommandWrapper, l_IDbTransaction);

                        l_string_Output = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@sOutput"));
                        l_NewPrinterID = Convert.ToInt64(db.GetParameterValue(insertCommandWrapper, "@IntPrintIDOut"));

                        if (l_string_Output != String.Empty)
                        {
                            l_IDbTransaction.Rollback();
                            return l_string_Output;
                        }
                    }

                    if (l_DataRow.RowState.ToString() == "Modified")
                    {
                        insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_InsertPrinters");
                        db.AddInParameter(insertCommandWrapper, "@IntPrintID", DbType.Int64, Convert.ToString(l_DataRow["IntUniqueid"]));
                        db.AddInParameter(insertCommandWrapper, "@varchar_PrinterName", DbType.String, Convert.ToString(l_DataRow["PrinterName"]));
                        db.AddInParameter(insertCommandWrapper, "@varchar_PrinterDescription", DbType.String, Convert.ToString(l_DataRow["PrinterDescription"]));
                        db.AddOutParameter(insertCommandWrapper, "@sOutput", DbType.String, 1000);
                        db.AddOutParameter(insertCommandWrapper, "@IntPrintIDOut", DbType.Int64, 0);

                        db.ExecuteNonQuery(insertCommandWrapper, l_IDbTransaction);

                        l_string_Output = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@sOutput"));
                        l_NewPrinterID = Convert.ToInt64(db.GetParameterValue(insertCommandWrapper, "@IntPrintIDOut"));

                        if (l_string_Output != String.Empty)
                        {
                            l_IDbTransaction.Rollback();
                            return l_string_Output;
                        }

                    }
                    if (l_DataRow.RowState.ToString() == "Deleted")
                    {
                        insertCommandWrapper = null;
                        insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_DeletePrinter");
                        if (insertCommandWrapper == null) return null;

                        db.AddInParameter(insertCommandWrapper, "@IntPrintID", DbType.Int64, parameterListPaperId.GetValue(0));
                        db.AddOutParameter(insertCommandWrapper, "@sOutput", DbType.String, 1000);
                        db.ExecuteNonQuery(insertCommandWrapper, l_IDbTransaction);
                        l_string_Output = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@sOutput"));

                        if (l_string_Output != String.Empty)
                        {
                            l_IDbTransaction.Rollback();
                            return l_string_Output;
                        }
                        

                    }

                }

                if (l_NewPrinterID > 0)
                {

                    insertCommandWrapper = null;
                    insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_DeletePrinterPaperTypes");
                    if (insertCommandWrapper == null) return null;

                    db.AddInParameter(insertCommandWrapper, "@IntPrintID", DbType.Int64, l_NewPrinterID);
                    db.AddOutParameter(insertCommandWrapper, "@sOutput", DbType.String, 1000);
                    db.ExecuteNonQuery(insertCommandWrapper, l_IDbTransaction);
                    l_string_Output = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@sOutput"));

                    if (l_string_Output != String.Empty)
                    {
                        l_IDbTransaction.Rollback();
                        return l_string_Output;
                    }

                    for (int i = 1; i < parameterListPaperId.Length; i++)
                    {
                        insertCommandWrapper = null;
                        insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_InsertPrinterPaperTypes");
                        if (insertCommandWrapper == null) return null;

                        db.AddInParameter(insertCommandWrapper, "@IntPrintID", DbType.Int64, l_NewPrinterID);
                        db.AddInParameter(insertCommandWrapper, "@IntPaperTypeID", DbType.Int64, parameterListPaperId.GetValue(i));
                        db.AddOutParameter(insertCommandWrapper, "@sOutput", DbType.String, 1000);
                        db.ExecuteNonQuery(insertCommandWrapper, l_IDbTransaction);

                        l_string_Output = Convert.ToString(db.GetParameterValue(insertCommandWrapper, "@sOutput"));

                        if (l_string_Output != String.Empty)
                        {
                            l_IDbTransaction.Rollback();
                            return l_string_Output;
                        }

                    }

                }
                l_IDbTransaction.Commit();

                return l_string_Output;



               

            }
            catch
            {
                throw;
            }
        }
        public static DataSet SearchPrinter(string parameterSearchPrinter)
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

                db.AddInParameter(SearchCommandWrapper, "@varchar_PrinterName", DbType.String, parameterSearchPrinter);
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
