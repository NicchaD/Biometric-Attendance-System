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
    public sealed class MetaShellReportsBOClass
    {
        public static DataSet LookupReports()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellReportsDAClass.LookupReports());
            }
            catch
            {
                throw;
            }
        }
        public static void InsertReport(DataSet parameterPrinters)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.MetaShellReportsDAClass.InsertReport(parameterPrinters);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet SearchReport(string parameterReport)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellReportsDAClass.SearchReport(parameterReport));
            }
            catch
            {
                throw;
            }

        }
    }
}
