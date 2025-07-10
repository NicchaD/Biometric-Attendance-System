//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;
using System.Security.Permissions;

namespace YMCARET.YmcaBusinessObject
{
    public sealed class MetaShellPrintersBOClass
    {
        public static DataSet GetUserShellPrinters(Int64 IntUserId, string reportName)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellPrintersDAClass.GetUserShellPrinters(IntUserId, reportName));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet LookupPrinters()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellPrintersDAClass.LookupPrinters());
            }
            catch
            {
                throw;
            }
        }
        public static string InsertPrinter(DataSet parameterPrinters, string[] parameterListPaperId)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellPrintersDAClass.InsertPrinter(parameterPrinters, parameterListPaperId));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet SearchPrinter(string parameterPrinter)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellPrintersDAClass.SearchPrinter(parameterPrinter));
            }
            catch
            {
                throw;
            }

        }
        public static void InsertUserPrinter(Int64 IntUserId, Int64 IntReportId, Int64 IntPrintId)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.MetaShellPrintersDAClass.InsertUserPrinter(IntUserId, IntReportId, IntPrintId);
            }
            catch
            {
                throw;
            }
        }
    }
}
