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
    public sealed class MetaShellPaperTypesBOClass
    {
        public static DataSet LookupPaperTypes()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.MetaShellPaperTypesDAClass.LookupPaperTypes());
            }
            catch
            {
                throw;
            }
        }
        public static void InsertPaperType(DataSet parameterPrinters)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.MetaShellPaperTypesDAClass.InsertPaperType(parameterPrinters);
            }
            catch
            {
                throw;
            }
        }
    }
}
