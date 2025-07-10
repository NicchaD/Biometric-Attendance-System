//**************************************************************************************************************/
// Author: Manthan Rajguru
// Created on: 09/13/2017
// Summary of Functionality: Includes all business logic for all actions performed under DC Tools module
// Declared in Version: 20.4.0 | YRS-AT-3665 -  YRS enh: Data Corrections Tool - Admin screen option to create a manual credit

//**************************************************************************************************************
// REVISION HISTORY:
// ------------------------------------------------------------------------------------------------------
// Developer Name              | Date         | Version No      | Ticket
// ------------------------------------------------------------------------------------------------------
// Pramod Prakash Pokale       | 09/18/2017   |	  20.4.0        | YRS-AT-3631 - YRS enh: Data Corrections Tool -Admin screen function to allow manual update of Fund status from WD or WP to TM or PT (TrackIT 30950) 
// Sanjay GS Rawat             | 09/18/2017   |	  20.4.0        | YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
// Santosh Bura		           | 09/18/2017   |	  20.4.0        | YRS-AT-3541 -  YRS enh: Data Corrections Tool -Admin screen function to allow modification of RDB amounts (excessive data corrections) 
// ------------------------------------------------------------------------------------------------------
//**************************************************************************************************************

using System.Data;

namespace YMCARET.YmcaBusinessObject
{
    public class DCToolsBO
    {

        #region "YMCA Credit"
       
        /// <summary>
        /// Provides YMCA credit details such as YMCA No, name and credit amount available with YMCA
        /// </summary>
        /// <param name="ymcaID">Ymca ID</param>
        /// <returns>Ymca Credit Details</returns>
        public static DataTable GetYMCACreditDetails(string ymcaID)
        {
            return YMCARET.YmcaDataAccessObject.DCToolsDA.GetYMCACreditDetails(ymcaID);
        }

        /// <summary>
        /// Saves given Ymca credit amount in data source
        /// </summary>
        /// <param name="data">Ymca Credit Details</param>
        /// <param name="activityLogEntry">Activity Log Details</param>
        /// <returns></returns>
        public static bool SaveYMCACredits(YMCAObjects.DCTools.YMCACredit data, YMCAObjects.YMCAActionEntry activityLogEntry)
        {
            return (YMCARET.YmcaDataAccessObject.DCToolsDA.SaveYMCACredits(data, activityLogEntry));
        }

        /// <summary>
        /// Validating credit amount and return error message no and threshold amount to be displayed in message .
        /// </summary>
        /// <param name="creditAmount">Credit Amount</param>
        /// <returns> Error message No and Threshold amount in datatable</returns>
        public static DataTable ValidateCreditAmount(decimal creditAmount)
        {
            return YMCARET.YmcaDataAccessObject.DCToolsDA.ValidateCreditAmount(creditAmount);
        }
        #endregion

        #region "Fund Status Change"
        //START: PPP | 09/18/2017 | YRS-AT-3631 | DC Tools - change fund event's Get and Save methods
        /// <summary>
        /// Provides details of fund status and balance for the given Fund Event ID
        /// </summary>
        /// <param name="fundEventID">Fund Event ID</param>
        /// <returns>Details of Fund and list of available fund status</returns>
        public static DataSet GetFundStatusDetails(string fundEventID)
        {
            return YMCARET.YmcaDataAccessObject.DCToolsDA.GetFundStatusDetails(fundEventID);
        }

        /// <summary>
        /// Updates fund status from old to new for the given person details
        /// </summary>
        /// <param name="data">Person Details</param>
        /// <param name="activityLogDetails">Log Details</param>
        /// <returns>Success Status (True / False)</returns>
        public static bool SaveFundStatusChanges(YMCAObjects.DCTools.FundStatus data, YMCAObjects.YMCAActionEntry activityLogDetails)
        {
            return YMCARET.YmcaDataAccessObject.DCToolsDA.SaveFundStatusChanges(data, activityLogDetails);
        }
        //END: PPP | 09/18/2017 | YRS-AT-3631 | DC Tools - change fund event's Get and Save methods
        #endregion

        #region "Reverse Annuity"
        // START | SR  | 09/18/2017   |	 YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
        /// <summary>
        /// Method to validate selected annuities for following
        /// '1. Annuity is void & reversed
        /// '2. QDRO split has not been performed using Annuity.
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static string ValidateAnnuity(string DisbursementId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DCToolsDA.ValidateAnnuity(DisbursementId);
            }
            catch
            {
                throw;
            }
        
        }

        /// <summary>
        /// This method will return annuitants (1) all annuities (group by disbursement) which are not in paid status. (2) All annuities without any grouping        
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static DataSet LookUpAnnuities(string fundEventId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DCToolsDA.LookUpAnnuities(fundEventId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method will return recommended fundstatus based on annuitant retiree & transaction status.
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static string RecommendedFundEventStatus(string DisbursementId)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DCToolsDA.RecommendedFundEventStatus(DisbursementId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method will reverse selected annuity, insert Notes & activity Logs.
        /// </summary>
        /// <param name="YMCAId"></param>
        /// <returns></returns>
        public static string ReverseAnnuity(string DisbursementId, string FundStatus, string Notes, string PersId, YMCAObjects.YMCAActionEntry activityLogEntry)
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.DCToolsDA.ReverseAnnuity(DisbursementId, FundStatus, Notes, PersId, activityLogEntry);
            }
            catch
            {
                throw;
            }
        }
        // END | SR  | 09/18/2017   |	 YRS-AT-3666 - YRS enh: Data Corrections Tool) -Reverse Annuity
        #endregion

        #region "Remaining Death Benefit"
        // START | SB | 09/18/2017 | YRS-AT-3541 - YRS enh: Data Corrections Tool -Edit Remaining Death Benefit amount
        /// <summary>
        ///Provides purchased annuity details along with Personal details, remaining death benefit total and system note.
        /// </summary>
        /// <param name="FundEventId"> Fund Event Id </param>
        /// <param name="errorCode"> Error Code </param>
        /// <returns>Annuities list, participant's information, remaining death benefit total and system note.</returns>
        public static DataSet GetAnnuitiesInformation(string fundeventID,out int errorCode)
        {
                return YMCARET.YmcaDataAccessObject.DCToolsDA.GetAnnuitiesInformation(fundeventID,out errorCode);
        }

        /// <summary>
        /// Edits remaining death benefit amount with notes and logs the activity in data source.
        /// </summary>
        /// <param name="data">Remaining death benefit details</param>
        /// <param name="activityLogEntry">Activity log details</param>
        /// <returns></returns>
        public static bool EditRemainingDeathBenefitAmount(YMCAObjects.DCTools.YMCARemainingDeathBenefitAmount data, YMCAObjects.YMCAActionEntry activityLogEntry)
        {
            return (YMCARET.YmcaDataAccessObject.DCToolsDA.EditRemainingDeathBenefitAmount(data, activityLogEntry));
        }
        // END | SB | 09/18/2017 | YRS-AT-3541 - YRS enh: Data Corrections Tool -Edit Remaining Death Benefit amount
        #endregion
    }
}
