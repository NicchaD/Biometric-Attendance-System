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
    public sealed class MetaShellPaperTypesDAClass
    {
        public static DataSet LookupPaperTypes()
        {
            DataSet dsLookUpPrinters = null;
            Database db = null;
            DbCommand commandLookUpOPrinters = null;
            string[] l_TableNames;
            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                commandLookUpOPrinters = db.GetStoredProcCommand("yrs_usp_MetaShell_LookupPaperTypes");

                if (commandLookUpOPrinters == null) return null;

                dsLookUpPrinters = new DataSet("PaperType Details");
                l_TableNames = new string[] { "PaperTypes", "Reports","Printers" };
                dsLookUpPrinters.Locale = CultureInfo.InvariantCulture;
                db.LoadDataSet(commandLookUpOPrinters, dsLookUpPrinters, l_TableNames);

                return dsLookUpPrinters;
            }
            catch
            {
                throw;
            }

        }
        public static void InsertPaperType(DataSet parameterReports)
        {
            Database db = null;
            DbCommand insertCommandWrapper = null;
            DbCommand updateCommandWrapper = null;
            DbCommand deleteCommandWrapper = null;

            try
            {
                //DataRowVersion.Current and DataRowVersion.Original differenitiates when is Insertion and when is updation.
                db = DatabaseFactory.CreateDatabase("YRS");

                insertCommandWrapper = db.GetStoredProcCommand("yrs_usp_MetaShell_InsertPaperTypes");

                //db.AddInParameter(insertCommandWrapper, "@int_ReportID", DbType.Int64, "ReportID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@int_PaperTypeID", DbType.Int64, "PaperTypeID", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_PaperType", DbType.String, "PaperType", DataRowVersion.Current);
                db.AddInParameter(insertCommandWrapper, "@varchar_Description", DbType.String, "Description", DataRowVersion.Current);


                updateCommandWrapper = db.GetStoredProcCommand("yrs_usp_UpdatePaperTypes");


                db.AddInParameter(updateCommandWrapper, "@int_PaperTypeID", DbType.Int64, "PaperTypeID", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_PaperType", DbType.String, "PaperType", DataRowVersion.Current);
                db.AddInParameter(updateCommandWrapper, "@varchar_Description", DbType.String, "Description", DataRowVersion.Current);

                deleteCommandWrapper = db.GetStoredProcCommand("yrs_usp_DeletePaperTypes");

                db.AddInParameter(deleteCommandWrapper, "@int_PaperTypeID", DbType.Int64, "PaperTypeID", DataRowVersion.Original);




                if (parameterReports != null)
                {
                    db.UpdateDataSet(parameterReports, "PaperTypes", insertCommandWrapper, updateCommandWrapper, deleteCommandWrapper, UpdateBehavior.Standard);
                }

            }
            catch
            {
                throw;
            }
        }
        
    }
}
