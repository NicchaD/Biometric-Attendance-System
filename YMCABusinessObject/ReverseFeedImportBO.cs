//**************************************************************************************************************/
// Copyright YMCA Retirement Fund All Rights Reserved. 
// Project Name		:	YMCA-YRS
// FileName			:	ReverseFeedImportBO.cs
// Author Name		:	Pooja Kumkar
// Employee ID		:	
// Email	    	:	
// Contact No		:	
// Creation Time	:	12/27/2019
// Description	    :	Business Class for reverse feed import
// Declared in Version : 20.7.2 | YRS-AT-4641 -  YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
//**************************************************************************************************************
// MODIFICATION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------

// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
    public class ReverseFeedImportBO
    {
        public static DataSet GetDataInGrid(int exportBaseProcessId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.GetDataInGrid(exportBaseProcessId);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetDataInExceptionTabGrid(int importBaseProcessId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.GetDataInExceptionTabGrid(importBaseProcessId);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetHeaderBaseID()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.GetHeaderBaseID();
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetDataExceptionList(int importBaseProcessID, int baseId,int FundNo)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.GetDataExceptionList(importBaseProcessID, baseId, FundNo);
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetDifferenceData(int exportBaseProcessId, int importedHeaderId, int FundId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.GetDifferenceData(exportBaseProcessId, importedHeaderId, FundId);
            }
            catch
            {
                throw;
            }
        }
        public static int UpdateImportBaseHeaderStatus(int importedHeaderId, string status)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.UpdateImportBaseHeaderStatus(importedHeaderId, status);
            }
            catch
            {
                throw;
            }
        }
        public static bool DiscardReverseFeed(int ImportBaseHeaderID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.DiscardReverseFeed(ImportBaseHeaderID));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet GetDataForPrintList(int ImportBaseHeaderID)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.ReverseFeedImportDA.GetDataForPrintList(ImportBaseHeaderID);
            }
            catch
            {
                throw;
            }
        }

    }
}
