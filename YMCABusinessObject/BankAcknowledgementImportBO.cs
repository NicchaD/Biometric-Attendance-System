//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	BankAcknowledgementImportBO.cs
// Author Name		:	Santosh Bura
// Created on       :   11/13/2019
// Summary of Functionality: Includes all business logic for all actions performed from importing DAR and Reverse seed files from Bank
// Declared in Version: 20.7.0 | YRS-AT-4599 -  YRS enh: State Tax Withholding - New import screen for "First Annuity Payroll Processing" (DAR File)

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
 //------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
    public class BankAcknowledgementImportBO
    {

        public BankAcknowledgementImportBO()
		{
			//
			// TODO: Add constructor logic here
			//
		}
        
        #region "DAR File Import"

        public static String PrepareImportBaseHeaderRecord(YMCAObjects.BankAcknowledgementHeaderImport DARImportFIleData)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.BankAcknowledgementImportDA.PrepareImportBaseHeaderRecord(DARImportFIleData));
            }
            catch
            {
                throw;
            }

        }

        public static DataSet InsertImportBaseRecordsWithErrors(DataSet dsDARFileData, int ImportBaseHeaderID)
        {
            try
            {
               // string para_out_string_ExeceptionReason = "";
               // DataSet l_dataset_ImportBaseRecordsWithErrors = null;
              // l_dataset_ImportBaseRecordsWithErrors =  
                 return  YMCARET.YmcaDataAccessObject.BankAcknowledgementImportDA.InsertImportBaseRecordsWithErrors(dsDARFileData, ImportBaseHeaderID);//, out para_out_string_ExeceptionReason);
               // return l_dataset_ImportBaseRecordsWithErrors;
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetErrorDetailsForEachRecord(string requestedImportBaseId, string requestedImportBaseDetailID)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.BankAcknowledgementImportDA.GetErrorDetailsForEachRecord(requestedImportBaseId, requestedImportBaseDetailID);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get pending records imported through DAR file for funding
        /// </summary>
        /// <returns>Get pending records else null</returns>
        public static DataSet GetPendingImportFilePresent()
        {
             return YmcaDataAccessObject.BankAcknowledgementImportDA.GetPendingImportFilePresent();
        }

        public static bool DiscardImportedDARFile( int ImportBaseHeaderID)
        {
            return (YMCARET.YmcaDataAccessObject.BankAcknowledgementImportDA.DiscardImportedDARFile(ImportBaseHeaderID));
        }

        public static bool ProcessImportedDARFile(int ImportBaseHeaderID)
        {
            return (YMCARET.YmcaDataAccessObject.BankAcknowledgementImportDA.ProcessImportedDARFile(ImportBaseHeaderID));
        }
        public static DataSet GetDARFileImportReport(int ImportBaseHeaderID)
        {
            return (YMCARET.YmcaDataAccessObject.BankAcknowledgementImportDA.GetDARFileImportReport(ImportBaseHeaderID));
        }
        #endregion  
    }

}
