//**************************************************************************************************************/
// Author: Manthan Rajguru
// Created on: 04/27/2020
// Summary of Functionality: Includes all business logic for all actions performed under withdrawals module
// Declared in Version: 20.8.1.2 | YRS-AT-4854 -  COVID - Special withdrawal functionality needed due to CARE Act/COVID-19 (Track IT - 41688) 

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// Shiny C.                    | 05/05/2020   | 20.8.1.2        | YRS-AT-4874 -  COVID - Special withdrawal functionality needed in YRS processing screen due to CARE Act/COVID-19 (Track IT - 41688) 
// Megha Lad                   | 25/05/2020   | 20.8.1.2        | YRS-AT-4874 -  Save refund request processing (Resolve precision issue)
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for RefundRequest.
    /// </summary>
    public class RefundRequest_C19
    {


        private RefundRequest_C19()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static Decimal GetCovidAmountUsed(int fundNo)
        {
            try
            {
                return RefundRequest_C19DAClass.GetCovidAmountUsed(fundNo);
            }
            catch
            {
                throw;
            }
        }

        //START : SC | 2020.04.05 | YRS-AT-4874 | Added COVID related methods for Refund Processing screen
        /// <summary>
        /// This procedure will fetch following COVID related amounts
        /// 1. Total Covid amount exhausted by the partipant
        /// 2. Covid amount requested at the time of request
        /// 3. Tax Rate requested at the time of request
        /// 4. Max Covid Limit which participant can avail
        /// </summary>
        public static DataTable GetCovidAmountsForProcessing(string parameterRefundRequestID)
        {
            try
            {
                return RefundRequest_C19DAClass.GetCovidAmountsForProcessing(parameterRefundRequestID);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Updates Taxable, Non-taxable and Tax rate in the Covid Transactions Table after save refund processing
        /// </summary>
        public static void UpdateCovidTransactactionAfterProcessing(string parameterRefundRequestID, decimal parameterCovidTaxableAmount, decimal parameterCovidNonTaxableAmount, decimal parameterCovidTaxRate)
        {
            try
            {
                RefundRequest_C19DAClass.UpdateCovidTransactactionAfterProcessing(parameterRefundRequestID, parameterCovidTaxableAmount, parameterCovidNonTaxableAmount, parameterCovidTaxRate);
            }
            catch
            {
                throw;
            }
        }
        //END : SC | 2020.04.05 | YRS-AT-4874 | Added COVID related methods for Refund Processing screen 

     


        //START | ML | 2020.05.25 | YRS-AT-4874 | Save refund request processing 
        /// <summary>
        /// Resolve precision issue for column intTaxRate for atsRefRequests, atsRefRequestPerPlan
        /// </summary>
        public static bool SaveRefundRequestProcess(DataSet parameterDataSet, string parameterPersonID, string parameterFundID, string paramterRefundType, bool parameterVested, bool parameterTerminated, bool parameterTookTDAccount, string parameterFundEventStatusType, DataSet parameterDataSetForNewRequests, bool parameterNeedsNewRequests, string parameterInitialRefRequestId, bool parameterIsMarket, decimal parameterTaxRate, string parameterPayeeName, string parameterRolloverinstitutionId, decimal parameterRollOverAmount, bool parameterIsActive, decimal parameterTaxable, decimal parameterNonTaxable, bool IRSOverride = false)
        {
            try
            {
                return (RefundRequest_C19DAClass.SaveRefundRequestProcessRefunds(parameterDataSet, parameterPersonID, parameterFundID, paramterRefundType, parameterVested, parameterTerminated, parameterTookTDAccount, parameterFundEventStatusType, parameterDataSetForNewRequests, parameterNeedsNewRequests, parameterInitialRefRequestId, parameterIsMarket, parameterTaxRate, parameterPayeeName, parameterRolloverinstitutionId, parameterRollOverAmount, parameterIsActive, parameterTaxable, parameterNonTaxable, IRSOverride));

            }
            catch
            {
                throw;
            }
        }
        //END | ML | 2020.05.25 | YRS-AT-4874 | Save refund request processing 
    }
}