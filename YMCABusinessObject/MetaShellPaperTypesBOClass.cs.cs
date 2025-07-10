using System;
using System.Data;
using Infotech.YmcaDataAccessObject;
using System.Security.Permissions;


namespace Infotech.YmcaBusinessObject
{
    public sealed class MetaShellPaperTypesBOClass
    {
        public static DataSet LookupPaperTypes()
        {
            try
            {
                return (Infotech.YmcaDataAccessObject.MetaShellPaperTypesDAClass.LookupPaperTypes());
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
                Infotech.YmcaDataAccessObject.MetaShellPaperTypesDAClass.InsertPaperType(parameterPrinters);
            }
            catch
            {
                throw;
            }
        }
    }
}
