//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS Plan Split Project	
// FileName			:	RetirementClassBO.cs
// Author Name		:	Asween
// Employee ID		:	37266
// Email			:	asween.mallick@3i-infotech.com
// Contact No		:	
// Creation Time		:	5/01/2007 10:17:00 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	
// Anil				YRPS 4536			28-Jan-2008
//*******************************************************************************
//	Date		Changed By			Description
//*******************************************************************************
//	2008.05.06	Nikunj Patel		BT-417 - Issue was when no existing accounts are available for the participant, then record for RT could not be added for projection.
//	2008.09.04	Mohammed Hafiz		YRS 5.0-445 - issue description mentioned under comments in Gemini.
//	2009.01.06	Mohammed Hafiz		YRS 5.0-636 
//	11-Feb-09	Priya				YRS 5.0-688 Change the wording "Fund" to "Funds"
//	18-Apr-2009	Ashish Srivastava   Phase V changes
//	02-Jul-2009	Ashish Srivastava   resolve issue YRS 5.0-801, 
//  08-Jul-2009 Ashish Srivastava   remove equality fron additional account stop date validation 
//  15-Jul-2009 Ashish Srivastava   No additional contibution in current month, Issue YRS 5.0-801 
//  20-Jul-2009 Ashish Srivastava   Remove future salay and future salary effective date validation for AE& RA participant, it is already validate in UI ,Issue YRS 5.0-830
//  21-Jul-2009 Ashish Srivastava   Integrate Issue YRS 5.0-830 from lable version 7.0.3
//  23-Jul-2009 Ashish Srivastava   Resolve Issue YRS 5.0-835, Future salary and annual salary increment simultaneously enable for estimation
//  27-Jul-2009 Ashish Srivastava   Revert chages for RT account considered  twice  
//  03-Aug-2009 Ashish Srivastava   Resolve Issue YRS 5.0-801
//	21-Aug-2009 Ashish Srivastava	Remove valdation for unmature RT amount sutracted from saving plan balaces
//	2009.11.17	Ashish Srivastava	Calculate average salary based on fundeventid
//  2010.05.26  Ashish Srivastava   Changes required for Migration
//  2010.10.11  Ashish Srivastava   Resolved Issue  YRS 5.0-855 BT-624
//2010.11.16    Ashish Srivastava   Resolved Issue  YRS 5.0-1215 BT-666
//2010.11.17    Priya Jawale        Added validation method for exact age annuity
//2010.12.27    Ashish Srivastava       YRS 5.0-855 BT-624
//2011-01-28    Ashish Srivastava       BT-665 Disability Retirement
//2011-02-07    Sanket Vaidya           Issues No BT-665 for Disability avoiding the paid service validation for Service plan
//2011.02.16    Ashish Srivastava      Added null condition in plantype for BT-665  Disability
//2011.02.20    Ashish Srivastava       Disability changes
//2011.02.21    Ashish Srivastava       Pass fundeventid insted of persid for getting Ymca resolution
//2011.03.01    Sanket Vaidya       BT-731 : The minimum required to purchase an annutiy is $5,000.01.  The retirement process allows an annutiy purchase with $5,000.00. 
//2011.03.09    Ashish Srivastava   YRS 5.0-1271/BT-665 accruing contribution and interest on new money from termination date to retirement date
//2011.03.07    Ashish Srivastava   Start date should be retirement date.
//2011.03.24    Sanket Vaidya        For YRS 5.0-1294,BT 794  : For disability requirement 
//2011.03.24    Sanket Vaidya        For YRS 5.0-1295,BT 796  : Minor adjustment to the calculation of the RDB
//2011.04.18    Sanket vaidya        BT-816 : Disability Retirement Estimate Issues.
//2011.06.01    Sanket vaidya       YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
//2011.07.27    Sanket Vaidya       YRS 5.0-1150/BT-596 : Option C reduction amount should not include Dth Ben annty
//2011.09.21    Ashish Srivastava   YRS-1345/BT-859 change message for QDRO eligble age
//2011.08.09    Sanket Vaidya       YRS 5.0-1329:J&S options available to non-spouse beneficiaries
//2011.10.04    Ashish Srivastava   YRS 5.0-1329:J&S options available to non-spouse beneficiaries(Consider participant& beneficiary age as on calender year in which the annuity being purchased)
//2011.10.05    Ashish Srivastava   YRS 5.0-1428-Retirement estimate error on projected Savings plan.
//2011.10.13    Ashish Srivastava   BT-917 Application giving error "String was not recognized as a valid DateTime." for  RPT fund event. 
//2011.12.09    Ashish Srivastava   BT-877:Object Reference error in Retirement process on selection of Death Benefigt
//2011.12.09    Ashish Srivastava   BT-883/YRS 5.0-1353:Failing on second annutiy purchase if 1/1/2011 bal is zero
//2012.02.02    Shashank Patel      BT-602/YRS 5.0-1130 : Change in how voluntary accounts are projected
//2012.02.21    Shashank Patel      BT-1003 : Annual Contribution limits messages not showing properly
//2012.03.16	Nikunj Patel		YRS 5.0-1560:BT-1016 - Message was not being displayed when the max annual limit was being crossed in the last month of the financial year.
//2012.03.21    Shashank Patel      BT-1020 :intContributionMaxAnnual50Addl value from atsMetacontributionlimits should not consider if the participant do not have/specify TD account for estimate.
//2012.03.30    Shashank Patel      BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
//2012.05.08    Shashank Patel      BT-1032 : Additional comments added for Gemini ID YRS 5.0-1574
//2012.05.18    Shashank Patel      BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary 
//2012.06.11    Shashank Patel      BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect (Reopened)
//2012.06.28	Shashank Patel		BT-712/YRS 5.0-1246 : Handling DLIN records
//2012.07.13	Shashank Patel		BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
//2012-07.30    Sanjay R.           BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
//2012-09-24    Anudeep A           BT-1126/Additional change YRS 5.0-1246 : : Handling DLIN records 
//2013.03.14    Dinesh Kanojia(DK)  BT-943: YRS 5.0-1443:Include partial withdrawals
//2013-07-02    Anudeep A           Bt-1501:YRS 5.0-1745:Capture Beneficiary addresses 
//2013.10.10    Dinesh Kanojia(DK)  BT-943: YRS 5.0-1443:Include partial withdrawals
//2013-10-24    Dinesh.k            BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
//2014-02-20	Shashank			BT-2436 :Retirement processing observations find attachment.
//2015.09.16    Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2015.11.24    Chandra sekar.c     YRS-AT-2610 : Restriction of death benefit annuity purchase
//2015.12.16    Chandra sekar.c     YRS-AT-2479 : Partial Withdrawal Estimate is allowed or not for terminated Participant with YMCA(LEGACY) ACCOUNT Balance
//2015.12.22    Sanjay Singh        YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
//2016.01.19    Chandra sekar.c     YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
//2016.02.22    Chandra sekar.c     YRS-AT-2752 - Retirement estimate not calculating projected reserves when estimation is performed for Both plan.
//2016.04.21    Chandra sekar.c     YRS-AT-2612 - YRS enh: Annuity Estimate Calculator should include 15+ yrs of service catchup
//2016.04.21    Chandra sekar.c     YRS-AT-2891 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
//2016.05.05    Pramod P. Pokale    YRS-AT-2683 - YRS and website enhancement-iFor PRA calculator use full YTD and full projected contributions
//2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
//2016.06.22    Chandra sekar.c     YRS-AT-3010 - YRS bug-annuity estimate calculator not counting partial withdrawal against estimates(TrackIT 26458)
//2016.08.02    Sanjay Singh        YRS-AT-2382 - Capture 'Reason' for change of beneficiary SSN and Annty bene SSN(TrackIT 19856)
//2017.02.02    Pramod P. Pokale    YRS-AT-3289 - YRS bug: catchup amount incorrect (TrackIT 28570 ) 
//2017.03.01    Pramod P. Pokale    YRS-AT-3317 - YRS bug - Annuity Estimate calculator reported lowering annual amt for the same retirement dates. (TrackIT 28688) 
//2017.03.03    Manthan Rajguru     YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012)    
//2017.03.06    Pramod P. Pokale    YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 
//2017.03.06    Santosh Bura        YRS-AT-2625 -  YRS enh: change in disability Annuity calculations (TrackIT 24012) 
//2017.03.09    Sanjay GS Rawat     YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 
//2017.03.09    Pramod P. Pokale    YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 
//2017.04.07    Sanjay GS Rawat     YRS-AT-3390 - YRS bug: Annuity calculations Estimates Screen (TrackIT 24012) 
//2017.04.18    Pramod P. Pokale    YRS-AT-3390 - YRS bug: Annuity calculations Estimates Screen (TrackIT 24012) 
//2017.04.19    Sanjay GS Rawat     YRS-AT-3403 - Support Request: YRS - error "no row at position 0" cannot run annuity estimates for (QDRO)alternate payees (TrackIT 29672) 
//2017.04.26    Pramod P. Pokale    YRS-AT-3419 -  URGENT : YRS BUG: Annuity Estimate - employment date - calc bug (TrackIT 29750) 
//2017.05.30    Pramod P. Pokale    YRS-AT-2625 - YRS enh: change in disability Annuity calculations (TrackIT 24012) 
//2017.07.03    Pramod P. Pokale    YRS-AT-3289 - YRS bug: catchup amount incorrect (TrackIT 28570 ) 
//2017.11.24    Pramod P. Pokale    YRS-AT-3319 - YRS bug: Annuity estimate calculator-maximum salary limit (TrackIT 28604) 
//2017.12.28    Pramod P. Pokale    YRS-AT-3328 - YRS bug-annuity estimates not allowing exclusion of certain accounts (TrackIT 28917) 
//2018.07.23    Sanjay GS Rawat     YRS-AT-3790 - YRS enh - Annuity Estimate calculator The projection process should consider the salary of just the projection months in the year (in this case two months) instead of the whole year salary for validating against annual limits (trackIT 32189&# 
//2018.07.23    Sanjay GS Rawat     YRS-AT-3811 - YRS Enh: comp limit issue-salary counted for entire year for those working partial year (TrackIT 32254) 
//2018.11.01    Ranu patel          YRS-AT-4133 -  YRS enh: Annuity Estimate calculator & Goal Calculator: RDB Plan Rule change NEEDED by DECEMBER 2018 (TrackIT 32497) 
//2018.11.28    Dharmesh CB         YRS-AT-3837 - YRS Enh: Death Benefit Application for RDB rule change NEEDED by DECEMBER 2018 (TrackIT 32497)
//2018.08.28    Sanjay GS Rawat     YRS-AT-4106 - Annuity Estimates Calculator - must take YTD, reported and unreported salary into account, for comp limit. Track IT - 35098 
//2019.06.06    Megha Lad           YRS-AT-4458 -YRS bug-partial withdrawal not reducing estimates for active participants (TrackIT 38406)
//2019.10.24    Megha Lad           YRS-AT-4598 - YRS enh: State Withholding Project - Annuity Payroll Processing
//2019.10.24    Megha Lad           YRS-AT-4597 - YRS enh: State Withholding Project - First Annuity Payments (UI design)
//2020.02.05    Megha Lad           YRS-AT-4769 - YRS enhancement-new Secure Act rule for Annuity Estimate and Annuity Purchase screens (TrackIt- 41078)
//                                  YRS-AT-4783 - YRS enhancement-modify Batch Estimates for Secure Act (TrackIt - 41132)
//2020.05.22    Manthan Rajguru     YRS-AT-4885 - Allow 0% Interest in annuity estimate calculator
//*******************************************************************************
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using YMCARET.YmcaDataAccessObject;
using System.Security.Permissions;
using System.Collections.Generic;
using YMCAObjects;
using System.Diagnostics; // 2016.01.06 | Sanjay S. | YRS-AT-2252

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// This class encapsulates all the business logic for 
    ///		1. Calculating the annuities.
    ///		2. Retirement Estimate
    ///		3. Retirement Processing
    /// </summary>
    public class RetirementBOClass
    {
        #region Declaration
        public static string REG_EX_CURRENCY = @"^\d+(?:\.\d{0,2})?$";
        public static string MONTHLY_PAYMENTS = "MOPAY";
        public static string MONTHLY_PERCENT_SALARY = "MOPERC";
        public static string ONE_LUMP_SUM = "ONELUM";
        public static string YEARLY_LUMP_SUM_PAYMENT = "YRLUMP";
        private const string EMPTY_DATE = "01/01/1900";
        //1329
        public static string JSAnnuityUnAvailableValue = "*N/A";


        public bool RetiredOnSavingsPlan = false;
        public bool RetiredOnRetirementPlan = false;
        //Ashish BT-917 commented
        //public string RetirementDate = string.Empty;
        public string existingAnnuityPurchaseDate = string.Empty;
        private decimal retirementPlanBalance;
        private decimal savingsPlanBalance;
        //private decimal ycProjected_Basic_Pre96_Retirement_Balance = 0;
        //		private decimal ycProjected_Basic_Pst96_Retirement_Balance = 0;
        //		public decimal ycProjected_Pre96_Personal_Retirement_Balance = 0;
        //		public decimal ycProjected_Pre96_YMCA_Retirement_Balance = 0;
        //		public decimal ycProjected_Pst96_Personal_Retirement_Balance = 0;
        //		public decimal ycProjected_Pst96_YMCA_Retirement_Balance = 0;
        //ASHISH:2011-01-28 commented for BT- 665 Disability Retirment
        //public decimal ycProjected_Basic_YMCA_Age60_Balance = 0;
        //public decimal ycProjected_Basic_Personal_Age60_Balance = 0;
        //public decimal ycProjected_NonBasic_YMCA_Age60_Balance = 0;
        //public decimal ycProjected_NonBasic_Personal_Age60_Balance = 0;

        //Declarations for final Sum		
        //		private decimal ycProjected_Roll_Personal_Retirement_Balance = 0;
        //		private decimal ycProjected_Roll_YMCA_Retirement_Balance = 0;

        //Added by Ashish for phase V part Changes
        public DataTable g_dtAcctBalancesByBasisType = null;
        public DataTable g_dtAnnuityBasisTypeList = null;

        
        //2012.07.13 SP : YRS 5.0-1599:Add "Projected Final Year's Salary" to page 
        private decimal projFinalYearSal = 0;

        //START: PPP | 03/08/2017 | YRS-AT-2625 | Following datatable variables will store actual values before projection till 60 years and total values including projected balance till 60 years
        //-- This will help to calculate AtsAnnuityCurrentValues.mnyReserveReductionPercent (actualRetirementBalance / totalRetirementBalance) in case participant purchases "C" annuity
        //-- This reduction factor will be used in monthly payroll process where ReserveRemaining will be reduced by (CurrentPayment * ReductionFactor) value
        //-- ReserveRemaining are always stored = actual balance and annuity current payments are calculated based on total balance 
        //-- So to reduce reserves we need reduction factor
        private DataTable actualRetirementBalance, totalRetirementBalance;
        //END: PPP | 03/08/2017 | YRS-AT-2625 | Following datatable variables will store actual values before projection till 60 years and total values including projected balance till 60 years

        //2102.07.13 SP :YRS 5.0-1599:Add "Projected Final Year's Salary" to page (creating property)
        public decimal ProjFinalYearSal
        {
            get { return projFinalYearSal; }
        }

        //START: PPP | 03/14/2017 | YRS-AT-2625 | Following properties will be used to log details in AtsYRSActivityLog table
        
        private DataTable actualBalanceAtRetirement;
        public DataTable ActualBalanceAtRetirement
        {
            get 
            {
                return actualBalanceAtRetirement;
            }
        }

        private DataTable totalBalanceAtRetirement;
        public DataTable TotalBalanceAtRetirement
        {
            get
            {
                return totalBalanceAtRetirement;
            }
        }

        private DataSet ymcaResolution;
        public DataSet YmcaResolution 
        {
            get
            {
                return ymcaResolution;
            }
        }

        private decimal averageSalary;
        public decimal AverageSalary
        {
            get
            {
                return averageSalary;
            }
        }
        //END: PPP | 03/14/2017 | YRS-AT-2625 | Following properties will be used to log details in AtsYRSActivityLog table
        #endregion "Declaration"

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RetirementBOClass()
        {

        }
        #endregion

        #region Retirement Processing



        /// <summary>
        /// This method is not used currently
        /// </summary>
        /// <param name="dtAccountsByBasis"></param>
        /// <param name="dtAccounts"></param>
        /// <param name="ltAccountBalanceEndDate"></param>
        /// <param name="personID"></param>
        /// <param name="planType"></param>
        /// <returns></returns>
        private static bool getBasisTypeAndAccountBalance(DataTable dtAccountsByBasis, DataTable dtAccounts, string ltAccountBalanceEndDate, string personID, string fundEventID, string planType)
        {
            DataSet dsAcctBalByBasis = RetirementBOClass.SearchAccountBalances(true, "01/01/1900", ltAccountBalanceEndDate, personID, fundEventID, false, true, planType);
            if (dsAcctBalByBasis.Tables.Count != 0 && dsAcctBalByBasis.Tables[0].Rows.Count != 0)
            {
                dtAccountsByBasis = dsAcctBalByBasis.Tables[0].Copy();
                dtAccountsByBasis.AcceptChanges();
            }

            DataSet dsAccts = RetirementBOClass.SearchAccountBalances(false, "01/01/1900", ltAccountBalanceEndDate, personID, fundEventID, false, true, planType);
            if (dsAccts.Tables.Count != 0 && dsAccts.Tables[0].Rows.Count != 0)
            {
                dtAccounts = dsAccts.Tables[0].Copy();
                dtAccounts.AcceptChanges();
            }
            bool isBasisType = false;
            DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
            foreach (DataRow drActByBasis in dtAccountsByBasis.Rows)
                foreach (DataRow drMetaActTypes in l_dsMetaAccountTypes.Tables[0].Rows)
                {
                    if (drActByBasis["chrAcctType"].ToString().Trim().ToUpper() ==
                        drMetaActTypes["chrAcctType"].ToString().Trim().ToUpper())
                        isBasisType = true;
                }

            return isBasisType;
        }



        //Added by Ashish for phase V part III changes, This method is not used currently
        /// <summary>
        /// Get the additional(new) contribution details for all the accounts to which user has already made the contribution.
        /// </summary>
        /// <param name="dsElectiveAccounts"></param>
        /// <param name="dtAccountsBasisByProjection"></param>
        /// <param name="isBasisType"></param>
        private static void getElectiveAccountDetails(DataSet dsElectiveAccounts, DataTable dtAccountsBasisByProjection
            , bool isBasisType)
        {
            dtAccountsBasisByProjection.Columns.Add("chvContributionTypeCode");
            dtAccountsBasisByProjection.Columns.Add("numAddlPctg", typeof(decimal));
            dtAccountsBasisByProjection.Columns.Add("dtmAddlContribEffDate", typeof(DateTime));
            dtAccountsBasisByProjection.AcceptChanges();
            if (dsElectiveAccounts != null && dsElectiveAccounts.Tables.Count != 0
                && dsElectiveAccounts.Tables[0].Rows.Count != 0)
            {
                if (isBasisType)
                {
                    foreach (DataRow drProjection in dtAccountsBasisByProjection.Rows)
                        foreach (DataRow drElectiveAct in dsElectiveAccounts.Tables[0].Rows)
                        {
                            if (drProjection["chrAcctType"].ToString().Trim().ToUpper() == drElectiveAct["chrAcctType"].ToString().Trim().ToUpper()
                                && drProjection["chrAnnuityBasisType"].ToString().Trim().ToUpper() == "PST96")
                            {
                                //if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == "P") //commented by hafiz on 10-Oct-2008
                                if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == MONTHLY_PERCENT_SALARY)
                                {
                                    drProjection["chvContributionTypeCode"] = MONTHLY_PERCENT_SALARY;
                                    drProjection["numAddlPctg"] = Convert.ToDecimal(drElectiveAct["numAddlPctg"]);
                                    //drProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmAddlContribEffDate"]);
                                    drProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmEffDate"]);
                                    dtAccountsBasisByProjection.GetChanges(DataRowState.Modified);
                                    dtAccountsBasisByProjection.AcceptChanges();
                                }
                            }
                        }
                }
                else
                {
                    if (dtAccountsBasisByProjection.Rows.Count > 0)
                    {
                        foreach (DataRow drProjection in dtAccountsBasisByProjection.Rows)
                        {
                            foreach (DataRow drElectiveAct in dsElectiveAccounts.Tables[0].Rows)
                            {
                                if (drProjection["chrAcctType"].ToString().Trim().ToUpper() == drElectiveAct["chrAcctType"].ToString().Trim().ToUpper())
                                {
                                    //if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == "P") //commented by hafiz on 10-Oct-2008
                                    if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == MONTHLY_PERCENT_SALARY)
                                    {
                                        drProjection["chvContributionTypeCode"] = MONTHLY_PERCENT_SALARY;
                                        drProjection["numAddlPctg"] = Convert.ToDecimal(drElectiveAct["numAddlPctg"]);
                                        //drProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmAddlContribEffDate"]);
                                        drProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmEffDate"]);
                                        dtAccountsBasisByProjection.GetChanges(DataRowState.Modified);
                                        dtAccountsBasisByProjection.AcceptChanges();
                                    }
                                    //if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == "M") //commented by hafiz on 10-Oct-2008
                                    if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == MONTHLY_PAYMENTS)
                                    {
                                        drProjection["chvContributionTypeCode"] = MONTHLY_PAYMENTS;
                                        drProjection["numAddlPctg"] = Convert.ToDecimal(drElectiveAct["numAddlPctg"]);
                                        //drProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmAddlContribEffDate"]);
                                        drProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmEffDate"]);
                                        dtAccountsBasisByProjection.GetChanges(DataRowState.Modified);
                                        dtAccountsBasisByProjection.AcceptChanges();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow drElectiveAct in dsElectiveAccounts.Tables[0].Rows)
                        {
                            DataRow drAccountsBasisByProjection = dtAccountsBasisByProjection.NewRow();

                            drAccountsBasisByProjection["dtsTransactDate"] = Convert.ToDateTime(drElectiveAct["dtmEffDate"]);
                            drAccountsBasisByProjection["chrProjPeriod"] = "";
                            drAccountsBasisByProjection["intProjPeriodSequence"] = 0;
                            drAccountsBasisByProjection["chrAcctType"] = drElectiveAct["chrAcctType"].ToString().Trim().ToUpper();
                            drAccountsBasisByProjection["chrAnnuityBasisType"] = "PST96";
                            drAccountsBasisByProjection["mnyYMCAContribBalance"] = 0;
                            drAccountsBasisByProjection["mnyPersonalContribBalance"] = 0;
                            drAccountsBasisByProjection["mnyPersonalInterestBalance"] = 0;
                            drAccountsBasisByProjection["mnyYMCAInterestBalance"] = 0;
                            drAccountsBasisByProjection["mnyPersonalPreTax"] = 0;
                            drAccountsBasisByProjection["mnyYMCAPreTax"] = 0;
                            drAccountsBasisByProjection["mnyPersonalPostTax"] = 0;
                            drAccountsBasisByProjection["YMCAAmt"] = 0;
                            drAccountsBasisByProjection["PersonalAmt"] = 0;
                            drAccountsBasisByProjection["mnyBalance"] = 0;
                            drAccountsBasisByProjection["chvPlanType"] = drElectiveAct["PlanType"];
                            if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == MONTHLY_PERCENT_SALARY)
                            {
                                drAccountsBasisByProjection["chvContributionTypeCode"] = MONTHLY_PERCENT_SALARY;
                                drAccountsBasisByProjection["numAddlPctg"] = Convert.ToDecimal(drElectiveAct["numAddlPctg"]);
                                drAccountsBasisByProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmEffDate"]);
                            }
                            else if (drElectiveAct["chrAdjustmentBasisCode"].ToString().Trim().ToUpper() == MONTHLY_PAYMENTS)
                            {
                                drAccountsBasisByProjection["chvContributionTypeCode"] = MONTHLY_PAYMENTS;
                                drAccountsBasisByProjection["numAddlPctg"] = Convert.ToDecimal(drElectiveAct["numAddlPctg"]);
                                drAccountsBasisByProjection["dtmAddlContribEffDate"] = Convert.ToDateTime(drElectiveAct["dtmEffDate"]);
                            }

                            dtAccountsBasisByProjection.Rows.Add(drAccountsBasisByProjection);
                        }

                        dtAccountsBasisByProjection.AcceptChanges();
                    }
                }
            }
        }




        /// <summary>
        /// Get the balance of all the Retirement accounts.
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        public static decimal GetRetirementBalance(string fundEventID, string retirementDate)
        {
            string personID = string.Empty;
            DataSet accountBalance;
            decimal retirementBalance = 0;
            try
            {
                accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceData(false, "01/01/1900", retirementDate, personID, fundEventID, false, true, "RETIREMENT");
                if (accountBalance.Tables.Count == 2)
                {
                    if (accountBalance.Tables[1].Rows.Count > 0)
                        foreach (DataRow dr in accountBalance.Tables[1].Rows)
                        {
                            //Calculate Retirement balance
                            if (dr[0].ToString().ToUpper() == "RETIREMENT")
                                retirementBalance = Convert.ToDecimal(dr["Balance"].ToString());
                        }
                }
            }
            catch
            {
                throw;
            }

            return retirementBalance;
        }


        /// <summary>
        /// Get the balance of all the Savings accounts.
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        public static decimal GetSavingsBalance(string fundEventID, string retirementDate)
        {
            string personID = string.Empty;
            DataSet accountBalance;
            decimal savingsBalance = 0;
            try
            {
                accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceData(false, "01/01/1900", retirementDate, personID, fundEventID, false, true, "SAVINGS");
                if (accountBalance.Tables.Count == 2)
                {
                    if (accountBalance.Tables[1].Rows.Count > 0)
                        foreach (DataRow dr in accountBalance.Tables[1].Rows)
                        {
                            //Calculate Savings balance
                            if (dr[0].ToString().ToUpper() == "SAVINGS")
                                savingsBalance = Convert.ToDecimal(dr["Balance"].ToString());
                        }
                }
            }
            catch
            {
                throw;
            }

            return savingsBalance;
        }

        /// <summary>
        /// Get a clean copy of data. No modification is done.
        /// </summary>
        /// <param name="annuitiesListCursor"></param>
        /// <returns></returns>
        public static DataTable GetPaymentData(DataTable annuitiesListCursor)
        {
            DataTable dtSelectAnnuity = new DataTable();
            DataRow drSelectAnnuity;
            dtSelectAnnuity.Columns.Add("Annuity");
            dtSelectAnnuity.Columns.Add("AnnuityDescription");
            dtSelectAnnuity.Columns.Add("Amount");
            dtSelectAnnuity.Columns.Add("Before62");
            dtSelectAnnuity.Columns.Add("After62");
            dtSelectAnnuity.Columns.Add("mnyPersonalPreTaxCurrentPayment");
            dtSelectAnnuity.Columns.Add("mnyPersonalPostTaxCurrentPayment");
            dtSelectAnnuity.Columns.Add("mnyYmcaPreTaxCurrentPayment");
            dtSelectAnnuity.Columns.Add("mnyYmcaPostTaxCurrentPayment");
            dtSelectAnnuity.Columns.Add("mnySSIncrease");
            dtSelectAnnuity.Columns.Add("mnySSDecrease");
            dtSelectAnnuity.AcceptChanges();
            foreach (DataRow dr in annuitiesListCursor.Rows)
            {
                drSelectAnnuity = dtSelectAnnuity.NewRow();
                if (dr["mnyCurrentPayment"].ToString() != "0")
                {
                    drSelectAnnuity["Annuity"] = dr["chrAnnuityType"].ToString();
                    drSelectAnnuity["AnnuityDescription"] = dr["chvDescription"].ToString();
                    drSelectAnnuity["Amount"] = Math.Round(Convert.ToDecimal(dr["mnyCurrentPayment"].ToString()), 2);
                    if (dr["mnySSBefore62"] != DBNull.Value)
                    {
                        if (dr["mnySSBefore62"].ToString() != "0")
                            drSelectAnnuity["Before62"] = Math.Round(Convert.ToDecimal(dr["mnySSBefore62"].ToString()), 2);
                        else
                            drSelectAnnuity["Before62"] = string.Empty;
                    }
                    if (dr["mnySSAfter62"] != DBNull.Value)
                    {
                        if (dr["mnySSAfter62"].ToString() != "0")
                            drSelectAnnuity["After62"] = Math.Round(Convert.ToDecimal(dr["mnySSAfter62"].ToString()), 2);
                        else
                            drSelectAnnuity["After62"] = string.Empty;
                    }
                    drSelectAnnuity["mnyPersonalPreTaxCurrentPayment"] = dr["mnyPersonalPreTaxCurrentPayment"];
                    drSelectAnnuity["mnyPersonalPostTaxCurrentPayment"] = dr["mnyPersonalPostTaxCurrentPayment"];
                    drSelectAnnuity["mnyYmcaPreTaxCurrentPayment"] = dr["mnyYmcaPreTaxCurrentPayment"];
                    drSelectAnnuity["mnyYmcaPostTaxCurrentPayment"] = dr["mnyYmcaPostTaxCurrentPayment"];
                    drSelectAnnuity["mnySSIncrease"] = dr["mnySSIncrease"];
                    drSelectAnnuity["mnySSDecrease"] = dr["mnySSDecrease"];
                    dtSelectAnnuity.Rows.Add(drSelectAnnuity);
                }
            }
            dtSelectAnnuity.GetChanges(DataRowState.Added);
            dtSelectAnnuity.AcceptChanges();

            return dtSelectAnnuity;
        }

        /// <summary>
        /// This method find how many days in supplied year.  
        /// </summary>
        /// <param name="parameterYear"></param>
        /// <returns>Number of days in a Year</returns>
        private static int GetNumberOfDaysInYear(int parameterYear)
        {
            int days = 0;
            try
            {
                if (parameterYear % 4 == 0)
                {
                    if (parameterYear % 100 == 0)
                    {
                        if (parameterYear % 400 == 0)
                        {
                            days = 366;
                        }
                        else
                        {
                            days = 365;
                        }
                    }
                    else
                    {
                        days = 366;
                    }

                }
                else
                {
                    days = 365;
                }
                return days;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }


        /// <summary>
        /// This Methods Claculate Compound Interest and Return Interest Amount.
        /// </summary>
        /// <param name="parameterPrincipalAmount"></param>
        /// <param name="parameterAnualInterestRate"></param>
        /// <param name="parameterNumOfPeriodsComputed"></param>
        /// <returns> InterestAmount</returns>		 
        private static double CalculateCompoundInterest(double parameterPrincipalAmount, double parameterAnualInterestRate, int parameterNumOfPeriodsComputed, int currentYear)
        {
            double interestAmount = 0;
            int numberOfCompoundingPeriods;
            try
            {

                numberOfCompoundingPeriods = GetNumberOfDaysInYear(currentYear);
                interestAmount = (parameterPrincipalAmount * (Math.Pow(1 + (parameterAnualInterestRate / 100) / (double)(numberOfCompoundingPeriods), (double)parameterNumOfPeriodsComputed))) - parameterPrincipalAmount;

                return interestAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Commented by Ashish for phase V part III changes
        /// <summary>
        /// Calculate the balance in each account that the user has directed to use for his retirement.
        /// </summary>
        /// <param name="p_dataset_RetEstimateEmployment"></param>
        /// <param name="txtRetireeBirthday"></param>
        /// <param name="txtFutureSalary"></param>
        /// <param name="txtRetirementDate"></param>
        /// <param name="txtFutureSalaryEffDate"></param>
        /// <param name="txtModifiedSal"></param>
        /// <param name="txtEndWorkDate"></param>
        /// <param name="personID"></param>
        /// <param name="fundEventID"></param>
        /// <param name="retireType"></param>
        /// <param name="projectedInterestRate"></param>
        /// <param name="dataSetElectiveAccounts"></param>
        /// <param name="annualSalaryIncrease"></param>
        /// <param name="combinedDataSet"></param>
        /// <param name="isEstimate"></param>
        /// <param name="planType"></param>
        /// <param name="fundStatus"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        //		public bool CalculateAccountBalancesAndProjections(DataSet p_dataset_RetEstimateEmployment, string txtRetireeBirthday
        //			, string txtFutureSalary, string txtRetirementDate, string txtFutureSalaryEffDate
        //			, string txtModifiedSal, string txtEndWorkDate, string personID, string fundEventID, string retireType
        //			, double projectedInterestRate, DataSet dataSetElectiveAccounts, int annualSalaryIncrease
        //			, DataSet combinedDataSet, bool isEstimate, string planType, string fundStatus, ref string errorMessage)
        //		{
        //			bool hasNoErrors = true;
        //			DataTable dtAccountsBasisByProjection = new DataTable();
        //			DataSet dsElectiveAccounts = new DataSet();
        //			DataTable dtAccountsByBasis = new DataTable();
        //			DataSet dsRetEstimateEmployment = new DataSet();
        //			DataTable dtRetEstimateEmployment = new DataTable();
        //			DataSet l_dsContributionLimits = new DataSet();
        //			string[,] laProjectionPeriods = new string[4, 3];
        //			int lnProjectionPeriod;
        //			int lnRETIREMENT_PROJECTION_PHASE;
        //			int C_PROJECTIONPERIOD = 1;
        //			int C_EFFDATE = 2;
        //			int C_TERMDATE = 3;
        //			int l_numGuaranteedInterestRatePre96 = 0;
        //			double lnMaximumContributionSalary = 0;
        //			//double lnContributionMaxAnnualTD = 0;//not in use
        //			//double lnContributionMaxAnnual = 0;//not in use
        //			DateTime ycCalcMonth;
        //			DateTime ldCalcMonthRetire;
        //			
        //			dtRetEstimateEmployment = p_dataset_RetEstimateEmployment.Tables[0]; // Last salary details of each employment.
        //			string ltAccountBalanceEndDate; // Retirement date
        //			if(isEstimate)
        //			{
        //				if (Convert.ToDateTime(txtRetirementDate) < DateTime.Now) 
        //					ltAccountBalanceEndDate = DateTime.Now.ToString("MM/dd/yyyy");
        //				else 
        //					ltAccountBalanceEndDate = txtRetirementDate;
        //			}
        //			else
        //				ltAccountBalanceEndDate = txtRetirementDate;
        //
        //			// Step 1. - Get Maximum contribution salary
        //			ldCalcMonthRetire = Convert.ToDateTime(Convert.ToDateTime(txtRetirementDate).Month.ToString() + "/1/" + Convert.ToDateTime(txtRetirementDate).Year.ToString());
        //			string ldCalcMonthToday = Convert.ToDateTime(DateTime.Now.Month + "/1/" + DateTime.Now.Year).ToString();
        //			l_dsContributionLimits = RetirementBOClass.SearchContributionLimits();
        //			if (l_dsContributionLimits.Tables[0].Rows.Count != 0)
        //				if (l_dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"] != DBNull.Value)
        //					lnMaximumContributionSalary = Convert.ToDouble(l_dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"]);
        //			
        //
        //			// Step 2. - Get account balances
        //			// Get balances by account and by Basis
        //			DataSet dsAcctBalByBasis = RetirementBOClass.SearchAccountBalances(true, "01/01/1900", ltAccountBalanceEndDate, personID, fundEventID, false, true, "B");//planType);
        //			if (dsAcctBalByBasis.Tables.Count != 0 && dsAcctBalByBasis.Tables[0].Rows.Count != 0)
        //			{
        //				dtAccountsByBasis = dsAcctBalByBasis.Tables[0].Copy();
        //				dtAccountsByBasis.AcceptChanges();
        //			}
        //			
        //			bool isBasisType = false;			
        //			DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
        //			foreach(DataRow drActByBasis in dtAccountsByBasis.Rows)
        //				foreach(DataRow drMetaActTypes in l_dsMetaAccountTypes.Tables[0].Rows)
        //				{
        //					if (drActByBasis["chrAcctType"].ToString().Trim().ToUpper() == 
        //						drMetaActTypes["chrAcctType"].ToString().Trim().ToUpper()) 
        //						isBasisType = true;
        //				}
        //			bool _blnBasisType = isBasisType;
        //
        //			// Check if additional contribution of type Monthly Percentage is prsesent
        //			bool isMonthlyPercentageContributionPresent = false;
        //			DataRow[] drMonthPerc = dataSetElectiveAccounts.Tables[0].Select("chrAdjustmentBasisCode='MOPERC'");
        //			if (drMonthPerc.Length > 0)
        //				isMonthlyPercentageContributionPresent = true;
        //
        //			// YREN-3257
        //			if(!_blnBasisType && isEstimate && isMonthlyPercentageContributionPresent)
        //			{
        //				if(txtFutureSalary == string.Empty || txtFutureSalary == "0.00" || txtFutureSalary == "0" || txtFutureSalaryEffDate == string.Empty)
        //				{
        //					if(fundStatus == "PE" || fundStatus == "PEML" || fundStatus == "RE" || fundStatus == "PENP")
        //						errorMessage = "Participant's fund status is Pre-Eligible & does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further.";
        //					else
        //						errorMessage = "Participant does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further.";
        //
        //					hasNoErrors = false;
        //					goto ReturnError;
        //				}
        //			}
        //
        //			// Step 3. - Create the projection table with initial values from dtAccountsByBasis
        //			// Projection table will contain 3 more columns TranDate, ProjPeriod, ProjPerSequence with 
        //			// default values '01/01/1900', "", 0
        //			dtAccountsBasisByProjection = dtAccountsByBasis.Clone();
        //			createProjectionTable(dtAccountsBasisByProjection, dtAccountsByBasis);
        //
        //			// Step 4. - Get Elective Account Details. Accounts to which he has already made contributions.
        //			// This routine integrates the additional contributions that the user has specified.
        //			dsElectiveAccounts = dataSetElectiveAccounts;
        //			//ASHISH:TEST,START
        //			//getElectiveAccountDetails(dsElectiveAccounts, dtAccountsBasisByProjection, _blnBasisType);
        //			//ASHISH:TEST,END
        //			// Step 5. - Update Employment Details
        //			// Get the copy of each employment			
        //			if (dsRetEstimateEmployment.Tables.Count != 0 && dsRetEstimateEmployment.Tables[0].Rows.Count != 0) // will never run as the given dataset is never populated.
        //			{
        //				dtRetEstimateEmployment = dsRetEstimateEmployment.Tables[0].Clone();
        //				foreach(DataRow dr in dsRetEstimateEmployment.Tables[0].Rows)
        //					dtRetEstimateEmployment.ImportRow(dr);
        //				dtRetEstimateEmployment.GetChanges(DataRowState.Added);
        //				dtRetEstimateEmployment.AcceptChanges();
        //			}
        //			
        //			// Set annual %age salary increase for each employment
        //			foreach(DataRow dr in dtRetEstimateEmployment.Rows)
        //			{
        //				dr["numAnnualPctgIncrease"] = annualSalaryIncrease;
        //				dtRetEstimateEmployment.GetChanges(DataRowState.Modified);
        //				dtRetEstimateEmployment.AcceptChanges();
        //			}
        //
        //			// Set his future salary and its effective date, if provided, for the active employment.
        //			// Multiple active employment will result in change of this routine.
        //			string terminationdate = txtEndWorkDate.Trim();
        //			//if ((Convert.ToDouble(txtFutureSalary) > 0 & IsDate(txtFutureSalaryEffDate) == true) || (terminationdate != "")) 
        //			if ((Convert.ToDouble(txtFutureSalary) > 0) || (terminationdate != "")) 
        //			{
        //				DataRow[] drRows = dtRetEstimateEmployment.Select("dtmTerminationDate Is Null");
        //				if (drRows.Length > 0) 
        //				{
        //					DataRow drRow = drRows[0];
        //					if (Convert.ToDouble(txtFutureSalary) > 0 && txtFutureSalaryEffDate.Trim() != string.Empty) //& IsDate(txtFutureSalaryEffDate) == true) 
        //					{
        //						drRow["numFutureSalary"] = txtFutureSalary;
        //						drRow["dtmFutureSalaryDate"] = txtFutureSalaryEffDate;
        //					}
        //					if (terminationdate != string.Empty) 
        //						drRow["dtmterminationdate"] = terminationdate;
        //					
        //					drRow.AcceptChanges();
        //				}
        //			}
        //
        //			// Use method
        //			// Step 6. - Take a side copy of  dtRetEstimateEmployment and populate the modified salary entered by the user
        //			DataTable dtEmploymentProjectedSalary = new DataTable();
        //			createProjectedSalary(dtEmploymentProjectedSalary, dtRetEstimateEmployment
        //				, lnMaximumContributionSalary, txtModifiedSal, isEstimate);
        //					
        //					
        //			// Step 7. Fetch the Future accounts specified by the user 
        //			// (Elective accounts He has not yet made the contribution and intends to do it in future)
        //			getFutureAccountDetails(dtAccountsByBasis, dsElectiveAccounts);
        //			
        //			// Step 8. Get YMCA resolutions and insert into dtAccountsByBasis
        //			getYMCAResolutions(dtAccountsByBasis, personID, "B");//planType);
        //			
        //			// Step 9. Get the details required for calculating the Interest on Contributions
        //			// Get Pre 96 InterestRate and its termination date.
        //			string l_CalcMonthStartPre96 = string.Empty;
        //			DataSet l_dsAnnuityFactor = new DataSet();
        //			l_dsAnnuityFactor = RetirementBOClass.SearchAnnuityFactorPre96();
        //			if (l_dsAnnuityFactor.Tables.Count != 0 && l_dsAnnuityFactor.Tables[0].Rows.Count != 0) 
        //			{
        //				if (l_dsAnnuityFactor.Tables[0].Rows[0]["numGuaranteedInterestRate"] != DBNull.Value)
        //					l_numGuaranteedInterestRatePre96 = Convert.ToInt32(l_dsAnnuityFactor.Tables[0].Rows[0]["numGuaranteedInterestRate"].ToString());
        //				if (l_dsAnnuityFactor.Tables[0].Rows[0]["dtmTermDate"] != DBNull.Value)
        //				{
        //					l_CalcMonthStartPre96 = Convert.ToDateTime(l_dsAnnuityFactor.Tables[0].Rows[0]["dtmTermDate"]).ToString("MM/dd/yyyy");
        //					if (Convert.ToDateTime(l_CalcMonthStartPre96).Month == 12) 
        //						l_CalcMonthStartPre96 = "01/01/" + (Convert.ToDateTime(l_CalcMonthStartPre96).Year + 1);
        //					else 
        //						l_CalcMonthStartPre96 = Convert.ToDateTime((Convert.ToDateTime(l_CalcMonthStartPre96).Month + 1) + "/01/" + Convert.ToDateTime(l_CalcMonthStartPre96).Year + 1).ToString("MM/dd/yyyy");
        //				}
        //			}
        //			
        //			bool calculateForwardExists = false;
        //			laProjectionPeriods[C_EFFDATE, 1] = l_CalcMonthStartPre96;
        //			laProjectionPeriods[C_PROJECTIONPERIOD, 1] = "PRE96_FORWARD";
        //			if (ldCalcMonthRetire > Convert.ToDateTime(ldCalcMonthToday)) 
        //			{
        //				laProjectionPeriods[C_EFFDATE, 2] = ldCalcMonthToday;
        //				laProjectionPeriods[C_PROJECTIONPERIOD, 2] = "CALCDATE_FORWARD";
        //				calculateForwardExists = true;
        //			}
        //
        //			// Get the projected retirement date. 
        //			if (retireType == "DISABL") 
        //			{
        //				string l_CalcMonthAge60;
        //				if (!(Convert.ToDateTime(txtRetireeBirthday).Month == 12)) 
        //				{
        //					if (Convert.ToDateTime(txtRetireeBirthday).Day == 1) 
        //						l_CalcMonthAge60 = Convert.ToDateTime(txtRetireeBirthday).Month + 1 + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(txtRetireeBirthday).Year + 60);
        //					else 
        //						l_CalcMonthAge60 = Convert.ToDateTime(txtRetireeBirthday).Month + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(txtRetireeBirthday).Year + 60);
        //				} 
        //				else 
        //					l_CalcMonthAge60 = Convert.ToDateTime("01/01/1900").Month + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(txtRetireeBirthday).Year + 61);
        //				
        //				if (ldCalcMonthRetire.Month == 12) 
        //					laProjectionPeriods[C_EFFDATE, laProjectionPeriods.GetUpperBound(1)] = "01/01/" + (ldCalcMonthRetire.Year + 1);
        //				else 
        //					laProjectionPeriods[C_EFFDATE, laProjectionPeriods.GetUpperBound(1)] = ldCalcMonthRetire.Month + 1 + "/1/" + ldCalcMonthRetire.Year;
        //				
        //				laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)] = l_CalcMonthAge60;
        //				laProjectionPeriods[C_PROJECTIONPERIOD, laProjectionPeriods.GetUpperBound(1)] = "RETIREDATE_TO_AGE60";
        //			} 
        //			else // Normal retirement
        //			{				
        //				laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)] = ldCalcMonthRetire.ToShortDateString();
        //				if(!calculateForwardExists)
        //					laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1) - 1] = ldCalcMonthRetire.ToShortDateString();
        //			}
        //
        //			// Format dates in the projection period array.
        //			string ldStartDateTmp = string.Empty;
        //			for (int i = laProjectionPeriods.GetUpperBound(1); i >= 1; i--) 
        //			{
        //				if (i != laProjectionPeriods.GetUpperBound(1)) 
        //				{
        //					if (ldStartDateTmp != string.Empty && ldStartDateTmp != null)
        //					{
        //						if (!(Convert.ToDateTime(ldStartDateTmp).Month == 1)) 
        //						{
        //							if (Convert.ToDateTime(ldStartDateTmp).Month > 1 & Convert.ToDateTime(ldStartDateTmp).Month < 10) 
        //								laProjectionPeriods[C_TERMDATE, i] = 0 + Convert.ToDateTime(ldStartDateTmp).Month - 1 + "/" + Convert.ToDateTime(ldStartDateTmp).ToString("dd/yyyy");
        //							else 
        //								laProjectionPeriods[C_TERMDATE, i] = Convert.ToDateTime(ldStartDateTmp).Month - 1 + "/" + Convert.ToDateTime(ldStartDateTmp).ToString("dd/yyyy");
        //						} 
        //						else 
        //							laProjectionPeriods[C_TERMDATE, i] = "12/" + Convert.ToDateTime(ldStartDateTmp).Day.ToString() + "/" + (Convert.ToDateTime(ldStartDateTmp).Year - 1);
        //					}
        //				}
        //				ldStartDateTmp = laProjectionPeriods[C_EFFDATE, i];
        //			}
        //
        //			lnProjectionPeriod = 1;
        //			lnRETIREMENT_PROJECTION_PHASE = 1;
        //			ycCalcMonth = Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod]);
        //						
        //			// Check if calcDate is greater than the termination Date.
        //			bool _llDone = false;
        //			_llDone = (ycCalcMonth >= Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)]));
        //
        //			// Step 10.
        //			int lnRETIREDATE_TO_AGE60 = 0;			
        //			// Now till here we have calculated and fetched the following information
        //			// 1. dtAccountsBasisByProjection -- All the new account contributions along with existing account contribution
        //			// 2. dtRetEstimateEmployment -- All the employment details till date
        //			// 3. dtEmploymentProjectedSalary -- Employment details with modified data.
        //			// 4. dtAccountsByBasis -- Existing account contributions with YMCA resolution details
        //			// 5. dsElectiveAccounts -- All the account details -- Existing and user modified (read proposed)
        //			calculateInterestOnContributions(ycCalcMonth, ref lnRETIREMENT_PROJECTION_PHASE, ref lnRETIREDATE_TO_AGE60
        //				, dtAccountsBasisByProjection, dtRetEstimateEmployment, dtEmploymentProjectedSalary, dtAccountsByBasis
        //				, laProjectionPeriods, lnProjectionPeriod, _llDone, _blnBasisType, personID
        //				, projectedInterestRate, retireType, l_numGuaranteedInterestRatePre96
        //				, dsElectiveAccounts.Tables[0], txtRetireeBirthday);
        //
        //
        //			//Added by Ashish For Phase V part III changes
        //			//Get AnnuityBasisTypeList from atsMetaAnnuityBasisTypes
        //			//			if(this.g_dtAnnuityBasisTypeList==null)
        //			//			{
        //				
        //			this.g_dtAnnuityBasisTypeList=RetirementBOClass.GetAnnuityBasisTypeList();  
        //				
        //			//			}
        //			//'' Check for the $5000 limit as per the Plan Type chosen and the Fund Status.
        //			if(isEstimate)
        //			{
        //				calculatePlanBalances(dtAccountsBasisByProjection);
        //				errorMessage = checkAccountBalanceLimits(fundStatus, planType, personID);
        //				if(errorMessage != string.Empty)
        //				{
        //					hasNoErrors = false;
        //					goto ReturnError;
        //				}				
        //			}
        //			
        //			// Step 10
        //			calculateFinalAmounts(dtAccountsBasisByProjection, lnRETIREMENT_PROJECTION_PHASE, lnRETIREDATE_TO_AGE60, planType);
        //						
        //			combinedDataSet.Tables.Add(dtAccountsBasisByProjection);
        //
        //			ReturnError:
        //				return hasNoErrors;
        //		}
        //
        //
        //		private static void calculateInterestOnContributions(DateTime ycCalcMonth, 
        //			ref int lnRETIREMENT_PROJECTION_PHASE, ref int lnRETIREDATE_TO_AGE60
        //			, DataTable dtAccountsBasisByProjection, DataTable dtRetEstimateEmployment
        //			, DataTable dtEmploymentProjectedSalary, DataTable dtAccountsByBasis
        //			, string[,] laProjectionPeriods, int lnProjectionPeriod, bool _llDone, bool _blnBasisType
        //			, string personID, double selectedProjectedInterestRate, string lcRetireType
        //			, double l_numGuaranteedInterestRatePre96, DataTable dtRetEstEmpElectives, string retireeBirthday)
        //		{
        //			// Get Contribution limits
        //			DataSet dsContributionLimits;
        //			double lnMaximumContributionSalary = 0;
        //			dsContributionLimits = RetirementBOClass.SearchContributionLimits();
        //			if (dsContributionLimits.Tables[0].Rows.Count != 0)
        //			{
        //				if (dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"] != DBNull.Value)
        //				{
        //					lnMaximumContributionSalary = Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"]);
        //				}
        //			}
        //			else
        //			{
        //				throw (new Exception("Please specify the maximum Contribution Limit for the year, to proceed further."));
        //			}
        //
        //			// Get the ProjectedInterestRate
        //			double lnNormalProjectedRate = 0;
        //			if (selectedProjectedInterestRate != 0)
        //				lnNormalProjectedRate = selectedProjectedInterestRate;
        //			else 
        //				lnNormalProjectedRate = 3;
        //
        //			// Get the Year of MaximumInterestRate
        //			double lnMaxInterestYearMonth = 0;
        //			DataSet l_dsMetaInterestRates = new DataSet();
        //			l_dsMetaInterestRates = RetirementBOClass.SearchMetaInterestRates();
        //			if (l_dsMetaInterestRates.Tables.Count != 0 && l_dsMetaInterestRates.Tables[0].Rows.Count != 0)
        //				lnMaxInterestYearMonth = (Convert.ToDouble(l_dsMetaInterestRates.Tables[0].Compute("MAX(chrYear)", "")) * 12) + (Convert.ToDouble(l_dsMetaInterestRates.Tables[0].Compute("MAX(chrMonth)", "")) - 1);
        //
        //			// Check if TD accounts exists
        //			bool TDAccountExists = false;
        //			if(dtAccountsByBasis.Columns.Contains("chrAcctType"))
        //				TDAccountExists = (dtAccountsByBasis.Select("chrAcctType = 'TD'").Length > 0);
        //
        //			// Get the difference between the retiree birtdate and today. 
        //			// And set the flag if it is greater than 50
        //			// And accordingly set his maximum salary contribution
        //			//bool _ll50 = false;
        //			double lnContributionMaxAnnualTD = 0;
        //			double lnContributionMaxAnnual = 0;
        //			if (dsContributionLimits.Tables[0].Rows.Count != 0)
        //			{
        //				DataRow dr = dsContributionLimits.Tables[0].Rows[0];
        //				if (DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(retireeBirthday), ycCalcMonth) >= 50) 
        //				{
        //					//_ll50 = true;
        //					lnContributionMaxAnnualTD = Convert.ToDouble(dr["ContributionMaxAnnualTD"]) + Convert.ToDouble(dr["ContributionMaxAnnual50Addl"]);
        //					lnContributionMaxAnnual = Convert.ToDouble(dr["ContributionMaxAnnual"]) + Convert.ToDouble(dr["ContributionMaxAnnual50Addl"]);
        //				} 
        //				else 
        //				{
        //					//_ll50 = false;
        //					lnContributionMaxAnnualTD = Convert.ToDouble(dr["ContributionMaxAnnualTD"]);
        //					lnContributionMaxAnnual = Convert.ToDouble(dr["ContributionMaxAnnual"]);
        //				}
        //			}
        //
        //			string errMsg;
        //			try
        //			{
        //				int ctrPre96 = 0;
        //				int ctrCalcForward = 0;
        //				int ctrCalcForwardCalculation = 0;
        //				int C_PROJECTIONPERIOD = 1;
        //				int C_EFFDATE = 2;
        //				int C_TERMDATE = 3;
        //
        //				string dtsTransactDate;
        //				string chrProjPeriod;
        //				int intProjPeriodSequence;
        //
        //				double lnAnnualTDContributions = 0;
        //				bool isFirstTimeInMonthLoop = true; 
        //				double lnAnnualContributions = 0;
        //				DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(personID);
        //				DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
        //
        //				// If CurrentProjectedSalary is not set then set it to 0 so that calculations dont fail for DBNull
        //				foreach (DataRow dr in dtEmploymentProjectedSalary.Rows)
        //					if (dr["CurrentProjectedSalary"] == DBNull.Value)
        //						dr["CurrentProjectedSalary"] = 0;
        //
        //				//YMCA Phase IV - Part 2 - added by hafiz on 4-Jun-2008 
        //				int _noOfDaysInMonth = 0;
        //				DateTime dt;
        //				while (_llDone == false) 
        //				{
        //					#region ASHISH:TEST:START
        //					dtsTransactDate = ycCalcMonth.ToString();
        //
        //					//YMCA Phase IV - Part 2 - added by hafiz on 4-Jun-2008 
        //					if (_noOfDaysInMonth==0)
        //					{
        //						dt = RetirementBOClass.getDailyInterestLog();
        //						_noOfDaysInMonth = DateAndTime.DateDiffNew(DateIntervalNew.Day, dt, Convert.ToDateTime(DateTime.Now.Date.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Date.Year, DateTime.Now.Date.Month).ToString() + "/" + DateTime.Now.Date.Year.ToString()));
        //					}
        //					else
        //					{
        //						_noOfDaysInMonth = DateTime.DaysInMonth(ycCalcMonth.Year, ycCalcMonth.Month);
        //					}
        //					
        //					chrProjPeriod = laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod];
        //					intProjPeriodSequence = lnProjectionPeriod;
        //
        //					if(chrProjPeriod != "CALCDATE_FORWARD")
        //						ctrPre96++;
        //				
        //					// Set the correct CurrentProjectedSalary 
        //					// (Future salary becomes the CurrentProjectedSalary if the calculation date equals the FutureEffective date)
        //					// Set projected current salary cursor on the right employment event (for updating later)
        //					if (chrProjPeriod == "CALCDATE_FORWARD") 
        //					{
        //						ctrCalcForward++;
        //
        //						// See if a salary increase is warranted this month
        //						foreach (DataRow dr in dtRetEstimateEmployment.Rows) 
        //						{
        //							string guiEmpEventIDSal = dr["guiUniqueID"].ToString();
        //							foreach (DataRow drPrjSal in dtEmploymentProjectedSalary.Rows) 
        //							{
        //								if (drPrjSal["guiEmpEventID"].ToString() == guiEmpEventIDSal) 
        //								{
        //									// Set Salary at Maximum possible salary
        //									// If future salary is given then check if it is the effective date is same as this month
        //									// Then set that as the CurrentProjectedSalary
        //									if (dr["numFutureSalary"] != DBNull.Value && dr["dtmFutureSalaryDate"] != DBNull.Value) 
        //									{
        //										if (ycCalcMonth.Month == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Month && 
        //											ycCalcMonth.Year == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Year) 
        //										{
        //											if (Convert.ToDouble(dr["numFutureSalary"]) >= lnMaximumContributionSalary / 12) 
        //												drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
        //											else 
        //												drPrjSal["CurrentProjectedSalary"] = Convert.ToDecimal(dr["numFutureSalary"]);
        //										}
        //									} 
        //										// Else check if the annual PctgIncrease is specified.
        //										// Do the annual increase every 12 months keeping calculation(today) month as the start month
        //									else 
        //									{
        //										//Determine if % Salary increase is warranted
        //										string annualSalaryIncreaseEffDate = dr["dtmAnnualSalaryIncreaseEffDate"].ToString();
        //										string annualPctgIncrease = dr["numAnnualPctgIncrease"].ToString();
        //										if (annualSalaryIncreaseEffDate != string.Empty && annualPctgIncrease != string.Empty)//Phase IV Changes
        //										{											
        //											//if ((ycCalcMonth.Month == DateTime.Now.Month && dr["numAnnualPctgIncrease"] != DBNull.Value)
        //											//&& (Convert.ToDouble(dr["numAnnualPctgIncrease"]) != 0))
        //											if (ycCalcMonth.Month == DateTime.Parse(annualSalaryIncreaseEffDate).Month 
        //												&& Convert.ToDouble(annualPctgIncrease) != 0)//Phase IV Changes
        //											{
        //												if (Convert.ToDouble(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDouble(dr["numAnnualPctgIncrease"]) / 100)) >= lnMaximumContributionSalary / 12) 
        //													drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
        //												else 
        //													drPrjSal["CurrentProjectedSalary"] = Convert.ToDecimal(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDecimal(dr["numAnnualPctgIncrease"]) / 100));
        //											}
        //										}
        //										
        //									}
        //									dtEmploymentProjectedSalary.GetChanges(DataRowState.Modified);
        //									dtEmploymentProjectedSalary.AcceptChanges();
        //								}
        //							}
        //						}
        //					}
        //					// This is where we add contributions and interest
        //					// Prevent entry if we are in the last projection period and 
        //					// this month = the termination period of that period
        //					if (!((lnProjectionPeriod == laProjectionPeriods.GetUpperBound(1) 
        //						&& ycCalcMonth == Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod]))) 
        //						&& ycCalcMonth >= Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod]) 
        //						&& ycCalcMonth <= Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod]) 
        //						&& chrProjPeriod == "CALCDATE_FORWARD") 
        //					{
        //						ctrCalcForwardCalculation++;
        //						// Process each account for this projection month
        //						//if (_blnBasisType) // Should this flag be here in place. coz now we are retiring only on Savings as well
        //					
        //						foreach (DataRow drActBasis in dtAccountsByBasis.Rows)
        //						{
        //							string planType = drActBasis["chvPlanType"].ToString();
        //							string chrAnnuityBasisType = drActBasis["chrAnnuityBasisType"].ToString().ToUpper().Trim();
        //							string chrAcctType = drActBasis["chrAcctType"].ToString().ToUpper().Trim();
        //							// Exit this for loop if this is a new money since we are not in a new money projection period
        //							if (chrAnnuityBasisType == "PST96" && 
        //								(chrProjPeriod == "PRE96_FORWARD" || chrProjPeriod == "RETIREDATE_TO_AGE60")) 
        //							{
        //								isFirstTimeInMonthLoop = false;
        //							} 
        //							else 
        //							{
        //								//Determine if this account is basic or not
        //								bool _llBasicAcct = false;
        //								if (l_dsMetaAccountTypes.Tables[0].Rows.Count != 0)
        //								{
        //									if (l_dsMetaAccountTypes.Tables[0].Select("chrAcctType='" + chrAcctType + "'").Length != 0)
        //										_llBasicAcct = true;
        //									else 
        //										_llBasicAcct = false;
        //								} 
        //								
        //								//Exit this account-basis loop if this is a 
        //								//    1. Non - Basic account during
        //								//    2. the RETIREDATE_TO_AGE60 projection period
        //								if (_llBasicAcct == false && chrProjPeriod.Trim().ToUpper() == "RETIREDATE_TO_AGE60") 
        //									isFirstTimeInMonthLoop = false;
        //								else 
        //								{
        //									double mnyPersonalPreTax = 0;
        //									double mnyPersonalPostTax = 0;
        //									double mnyYMCAPreTax = 0;
        //									double mnyYMCAInterestBalance = 0;
        //									double mnyPersonalInterestBalance = 0;
        //									double mnyYMCAContribBalance = 0;
        //									double mnyPersonalContribBalance = 0;
        //
        //									// If we are between calculate date and retirement date, assess account contributions for new money
        //									if (chrProjPeriod == "CALCDATE_FORWARD" && chrAnnuityBasisType == "PST96") 
        //									{
        //										//	Contributions
        //										//  New Money
        //										//  Determine Contributions for this account according to each active employment event
        //										mnyYMCAContribBalance = 0;
        //										mnyPersonalContribBalance = 0;
        //										double m_CurrentProjectedSalary = 0;
        //										for (int j = 0; j <= dtRetEstimateEmployment.Rows.Count - 1; j++) 
        //										{
        //											DataRow drEstEmp = dtRetEstimateEmployment.Rows[j];
        //											if (drEstEmp["dtmTerminationDate"] == DBNull.Value) // Only for active employments
        //											{
        //												string guiEmpEventID = drEstEmp["guiUniqueID"].ToString().Trim().ToUpper();
        //												string guiYMCAID = drEstEmp["guiYMCAID"].ToString();
        //												// Get the current projected salary
        //												if (dtEmploymentProjectedSalary.Select("guiEmpEventID='" + guiEmpEventID + "'").Length > 0) 
        //												{
        //													DataRow drEmploymentProjectedSalary = dtEmploymentProjectedSalary.NewRow();
        //													drEmploymentProjectedSalary = dtEmploymentProjectedSalary.Select("guiEmpEventID='" + guiEmpEventID + "'")[0];
        //													m_CurrentProjectedSalary = Convert.ToDouble(drEmploymentProjectedSalary["CurrentProjectedSalary"]);
        //												}
        //												// Get the YMCA and Member percentage contribution
        //												// Get contribution percentages for member and YMCA
        //												// (do this every month regardless of other options)
        //												double lnMemberPct = 0;
        //												double lnYMCAPct = 0;
        //												//DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(personID);
        //												if (l_dsYmcaResolutions.Tables[0].Rows.Count != 0 && 
        //													l_dsYmcaResolutions.Tables[0].Select("chrBasicAcctType='" + chrAcctType + "' AND guiYmcaID='" + guiYMCAID + "'").Length > 0) 
        //												{
        //													DataRow drYmcaRes = l_dsYmcaResolutions.Tables[0].Rows[0];
        //													if (drYmcaRes["numYmcaComboPctg"] == DBNull.Value) 
        //														lnMemberPct = Convert.ToDouble(drYmcaRes["numConstituentPctg"]);
        //													else if (drYmcaRes["numConstituentPctg"] == DBNull.Value) 
        //														lnMemberPct = Convert.ToDouble(drYmcaRes["numYmcaComboPctg"]);
        //													else 
        //														lnMemberPct = Convert.ToDouble(drYmcaRes["numConstituentPctg"]) + Convert.ToDouble(drYmcaRes["numYmcaComboPctg"]);
        //															
        //													if (drYmcaRes["numYmcaPctg"] == DBNull.Value) 
        //														lnYMCAPct = Convert.ToDouble(drYmcaRes["numAddlMarginPctg"]);
        //													else if (drYmcaRes["numAddlMarginPctg"] == DBNull.Value) 
        //														lnYMCAPct = Convert.ToDouble(drYmcaRes["numYmcaPctg"]);
        //													else 
        //														lnYMCAPct = Convert.ToDouble(drYmcaRes["numYmcaPctg"]) + Convert.ToDouble(drYmcaRes["numAddlMarginPctg"]);
        //												} 
        //												else 
        //												{
        //													lnMemberPct = 0;
        //													lnYMCAPct = 0;
        //												}
        //												
        //												// From the percentage get the actual contribution
        //												double lnMemberContrib = m_CurrentProjectedSalary * (lnMemberPct / 100);
        //												double lnYMCAContrib = m_CurrentProjectedSalary * (lnYMCAPct / 100);
        //												
        //												// Now get the additional contribution
        //												double lnAddlContrib = 0;
        //												if (dtRetEstEmpElectives.Rows.Count != 0)
        //												{
        //													foreach(DataRow drElective in dtRetEstEmpElectives.Rows)
        //													{
        //														if (drElective["guiEmpEventID"].ToString().Trim().ToUpper() == guiEmpEventID
        //															&& drElective["chrAcctType"].ToString().Trim().ToUpper() == chrAcctType)
        //														{
        //															if (Convert.ToDouble(drElective["mnyAddlContribution"]) != 0 || Convert.ToDouble(drElective["numAddlPctg"]) != 0)
        //															{
        //																if (drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim() == MONTHLY_PERCENT_SALARY) 
        //																{
        //																	if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && drElective["dtsTerminationDate"] == DBNull.Value) 
        //																		lnAddlContrib = lnAddlContrib + (Convert.ToDouble(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]) * Convert.ToDouble(drElective["numAddlPctg"]) / 100);
        //																	else if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && drElective["dtsTerminationDate"].ToString().Trim() == string.Empty) 
        //																		lnAddlContrib = lnAddlContrib + (Convert.ToDouble(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]) * Convert.ToDouble(drElective["numAddlPctg"]) / 100);
        //																	else if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && ycCalcMonth <= Convert.ToDateTime(drElective["dtsTerminationDate"])) 
        //																		lnAddlContrib = lnAddlContrib + (Convert.ToDouble(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]) * Convert.ToDouble(drElective["numAddlPctg"]) / 100);
        //																}
        //																if (drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim() == MONTHLY_PAYMENTS) 
        //																{
        //																	if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && drElective["dtsTerminationDate"] == DBNull.Value) 
        //																		lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
        //																	else if (ycCalcMonth >= (Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year)) && drElective["dtsTerminationDate"].ToString().Trim() == string.Empty) 
        //																		lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
        //																	else if (ycCalcMonth >= (Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year)) && ycCalcMonth <= Convert.ToDateTime(drElective["dtsTerminationDate"])) 
        //																		lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
        //																}
        //																if (drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim() == ONE_LUMP_SUM) 
        //																{
        //																	if (ycCalcMonth.Month == Convert.ToDateTime(drElective["dtmEffDate"]).Month && ycCalcMonth.Year == Convert.ToDateTime(drElective["dtmEffDate"]).Year) 
        //																		lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
        //																}
        //																if (drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim() == YEARLY_LUMP_SUM_PAYMENT) 
        //																{
        //																	if (ycCalcMonth.Month == Convert.ToDateTime(drElective["dtmEffDate"]).Month) 
        //																		lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
        //																}
        //															}
        //														}
        //													}
        //												}
        //													
        //												// Now get the total Member and Ymca balance
        //												mnyPersonalContribBalance += lnMemberContrib + lnAddlContrib;
        //												mnyYMCAContribBalance += lnYMCAContrib;
        //											}
        //										} // End For loop
        //									}
        //
        //									// Interest Calculations
        //									// Get the applicable interest rates
        //									double lnKnownInterestRate = 0;
        //									DataSet l_dsKnownInterestRate = RetirementBOClass.SearchKnownInterestRate(chrAcctType, ycCalcMonth.Year.ToString(), ycCalcMonth.Month.ToString());
        //									
        //									// This IF had been modified
        //									if (l_dsKnownInterestRate.Tables.Count != 0 && l_dsKnownInterestRate.Tables[0].Rows.Count != 0)
        //										lnKnownInterestRate = Convert.ToDouble(l_dsKnownInterestRate.Tables[0].Rows[0]["numInterestRate"]);
        //									else
        //										lnKnownInterestRate = selectedProjectedInterestRate;
        //									
        //									double lnInterestRate = 0;
        //									double lnInterestRateNewMoney = 0;
        //									double lnInterestRateDifferential = 0;
        //									// Old Money determine rate for Pre96 balances and differential
        //									if (chrAnnuityBasisType == "PRE96") 
        //									{
        //										if (chrProjPeriod.Trim().ToUpper() == "CALCDATE_FORWARD") 
        //										{
        //											//figure out new money interest rate first so we can compute
        //											if (lcRetireType == "NORMAL") 
        //											{
        //												if (Convert.ToDouble(ycCalcMonth.Year) * 12 + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1) 
        //													lnInterestRateNewMoney = lnNormalProjectedRate;
        //												else 
        //													// if lnKnownInterestRate is null (Should never happen) then assign new interest money to 0
        //													lnInterestRateNewMoney = lnKnownInterestRate;
        //													
        //												// If pst96 known or projected rate is higher than pre96 guaranteed rate, capture the difference
        //												// then apply it (later) to pre96 balances but credit the result to pst96 balances
        //												lnInterestRate = l_numGuaranteedInterestRatePre96;
        //												if (lnInterestRateNewMoney > lnInterestRate) 
        //													lnInterestRateDifferential = lnInterestRateNewMoney - lnInterestRate;
        //												else 
        //													lnInterestRateDifferential = 0;
        //											} 
        //											else  // Disabled
        //											{
        //												lnInterestRateDifferential = 0;
        //												// Disability \ Known rate exists and is greater than guaranteed then use the known rate for old balances
        //												if (Convert.ToDouble(ycCalcMonth.Year) * 12 + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1 & lnKnownInterestRate > l_numGuaranteedInterestRatePre96) 
        //													lnInterestRate = lnKnownInterestRate;
        //												else 
        //													lnInterestRate = l_numGuaranteedInterestRatePre96;
        //											}
        //										} 
        //										else 
        //										{
        //											lnInterestRate = l_numGuaranteedInterestRatePre96;
        //											lnInterestRateDifferential = 0;
        //										}
        //									} 
        //									else if (chrAnnuityBasisType == "PST96")  //New Money
        //									{
        //										if ((Convert.ToDouble(ycCalcMonth.Year) * 12) + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1) 
        //											lnInterestRate = lnNormalProjectedRate;
        //										else 
        //											lnInterestRate = lnKnownInterestRate;
        //									} 
        //									else if (chrAnnuityBasisType == "ROLL") //ROLLINS are treated same as new money
        //									{
        //										if ((Convert.ToDouble(ycCalcMonth.Year) * 12) + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1) 
        //											lnInterestRate = lnNormalProjectedRate;
        //										else 
        //											lnInterestRate = lnKnownInterestRate;
        //									}
        //									// -- End of Interest rate calculation
        //
        //									// Get account balances to date
        //									double[] laAccountsByBasisProjectionToDate = new double[2];
        //									laAccountsByBasisProjectionToDate[0] = 0;
        //									laAccountsByBasisProjectionToDate[1] = 0;
        //									if (dtAccountsBasisByProjection.Rows.Count != 0
        //										&& dtAccountsBasisByProjection.Select("chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisType='" + chrAnnuityBasisType + "' AND chrProjPeriod <> 'CALCDATE_FORWARD' AND dtsTransactDate <>'" + dtsTransactDate + "'").Length > 0) 
        //									{
        //										laAccountsByBasisProjectionToDate[0] = Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisType='" + chrAnnuityBasisType + "' AND NOT(dtsTransactDate = '" + dtsTransactDate + "' AND chrProjPeriod = 'CALCDATE_FORWARD')"));
        //										laAccountsByBasisProjectionToDate[1] = Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisType='" + chrAnnuityBasisType + "' AND NOT(dtsTransactDate = '" + dtsTransactDate + "' AND chrProjPeriod = 'CALCDATE_FORWARD')"));
        //									}
        //
        //									//Start - YMCA Phase IV - Part 2 - commented by Hafiz on 4-Jun-2008
        //									//mnyPersonalInterestBalance = (laAccountsByBasisProjectionToDate[0] * lnInterestRate) / 100 / 12;
        //									//mnyYMCAInterestBalance = (laAccountsByBasisProjectionToDate[1] * lnInterestRate) / 100 / 12;
        //									//End - YMCA Phase IV - Part 2 - commented by Hafiz on 4-Jun-2008
        //									
        //									//Start - YMCA Phase IV - Part 2 - added on 4-Jun-2008 by Hafiz 											
        //									mnyPersonalInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[0], lnInterestRate, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
        //									mnyYMCAInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[1], lnInterestRate, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
        //									//End - YMCA Phase IV - Part 2 - added on 4-Jun-2008 by Hafiz 
        //
        //									// Check Limits
        //									// TD first
        //									if (chrAcctType.ToUpper().Trim() == "TD") 
        //									{
        //										// If we have already over the TD limit 
        //										if (lnAnnualTDContributions >= lnContributionMaxAnnualTD) 
        //										{
        //											mnyPersonalContribBalance = 0;
        //											mnyYMCAContribBalance = 0;
        //										} 
        //										else 
        //										{
        //											//See if we will hit the limit with this contribution cycle
        //											if (mnyPersonalContribBalance + mnyYMCAContribBalance + lnAnnualTDContributions >= lnContributionMaxAnnualTD) 
        //											{
        //												// See if just the personal side causes us to hit the limit
        //												// Adjust the YMCA side so it brings us to the limit
        //												if (mnyPersonalContribBalance + lnAnnualTDContributions >= lnContributionMaxAnnualTD) 
        //												{
        //													mnyPersonalContribBalance = lnContributionMaxAnnualTD - lnAnnualTDContributions;
        //													mnyYMCAContribBalance = 0;
        //												} 
        //												else 
        //													//See if we will hit the limit with this contribution cycle
        //													// Adjust the YMCA side so it brings us to the limit
        //													mnyYMCAContribBalance = lnContributionMaxAnnualTD - lnAnnualTDContributions + mnyPersonalContribBalance;
        //											}
        //										}
        //									}
        //
        //									// All contributions limit check  -- Exclude RT account from the check
        //									// See if we are already over the limit
        //									if (chrAcctType.ToUpper().Trim() != "RT") 
        //									{
        //										if (lnAnnualContributions >= lnContributionMaxAnnual) 
        //										{
        //											mnyPersonalContribBalance = 0;
        //											mnyYMCAContribBalance = 0;
        //										} 
        //										else 
        //										{
        //											//See if we will hit the limit with this contribution cycle
        //											if ((mnyPersonalContribBalance + mnyYMCAContribBalance + lnAnnualContributions) >= lnContributionMaxAnnual) 
        //											{
        //												//See if just the personal side causes us to hit the limit
        //												if ((mnyPersonalContribBalance + lnAnnualContributions) >= lnContributionMaxAnnual) 
        //												{
        //													//'Adjust the personal side and set YMCA side to 0 which should bring us to the limit
        //													mnyPersonalContribBalance = lnContributionMaxAnnual - lnAnnualContributions;
        //													mnyYMCAContribBalance = 0;
        //												} 
        //												else 
        //													//Adjust the YMCA side so it brings us to the limit
        //													mnyYMCAContribBalance = (lnContributionMaxAnnual - lnAnnualContributions) + mnyPersonalContribBalance;
        //											}
        //										}
        //									}
        //									//Update the YTD contributions for future limit comparisons
        //									if (chrAcctType.ToUpper().Trim() == "TD") 
        //										lnAnnualTDContributions = lnAnnualTDContributions + mnyPersonalContribBalance + mnyYMCAContribBalance;
        //									
        //									if (chrAcctType.ToUpper().Trim() != "RT") // Exclude RT account from the check
        //										lnAnnualContributions = lnAnnualContributions + mnyPersonalContribBalance + mnyYMCAContribBalance;
        //		
        //									//Prepare variables for inserting into the account projection table
        //									mnyPersonalPreTax = mnyPersonalInterestBalance + mnyPersonalContribBalance;
        //									mnyYMCAPreTax = mnyYMCAInterestBalance + mnyYMCAContribBalance;
        //									mnyPersonalPostTax = 0;
        //									double YMCAAmt = mnyYMCAPreTax;
        //									double PersonalAmt = mnyPersonalPreTax + mnyPersonalPostTax;
        //									double mnyBalance = YMCAAmt + PersonalAmt;
        //
        //									// Update ongoing account balance total
        //									// Insert projected account growth for this account into dtAccountsByBasisProjection
        //									if (!(chrProjPeriod.Trim().ToUpper() == "PRE96_FORWARD")) 
        //									{
        //										DataRow drAccountsBasisByProjection = dtAccountsBasisByProjection.NewRow();
        //										drAccountsBasisByProjection["dtsTransactDate"] = dtsTransactDate;
        //										drAccountsBasisByProjection["chrProjPeriod"] = chrProjPeriod;
        //										drAccountsBasisByProjection["chrAcctType"] = chrAcctType;
        //										drAccountsBasisByProjection["chrAnnuityBasisType"] = chrAnnuityBasisType;
        //										drAccountsBasisByProjection["mnyYMCAContribBalance"] = mnyYMCAContribBalance;
        //										drAccountsBasisByProjection["mnyPersonalContribBalance"] = mnyPersonalContribBalance;
        //										drAccountsBasisByProjection["mnyPersonalInterestBalance"] = mnyPersonalInterestBalance;
        //										drAccountsBasisByProjection["mnyYMCAInterestBalance"] = mnyYMCAInterestBalance;
        //										drAccountsBasisByProjection["mnyPersonalPreTax"] = mnyPersonalPreTax;
        //										drAccountsBasisByProjection["mnyYMCAPreTax"] = mnyYMCAPreTax;
        //										drAccountsBasisByProjection["mnyPersonalPostTax"] = mnyPersonalPostTax;
        //										drAccountsBasisByProjection["YMCAAmt"] = YMCAAmt;
        //										drAccountsBasisByProjection["PersonalAmt"] = PersonalAmt;
        //										drAccountsBasisByProjection["mnyBalance"] = mnyBalance;
        //										drAccountsBasisByProjection["intProjPeriodSequence"] = intProjPeriodSequence;
        //										drAccountsBasisByProjection["chvPlanType"] = planType;
        //										dtAccountsBasisByProjection.Rows.Add(drAccountsBasisByProjection);
        //										dtAccountsBasisByProjection.GetChanges(DataRowState.Added);
        //									}
        //									if (lcRetireType == "NORMAL" & chrProjPeriod.Trim().ToUpper() == "CALCDATE_FORWARD" & chrAnnuityBasisType.Trim().ToUpper() == "PRE96" & !(lnInterestRateDifferential == 0)) 
        //									{
        //										// Credit interest over the pre96 guaranteed rate on old money to new money balances
        //
        //										// The following conditions have been met, so credit the interest differential
        //										//  1. Normal retirement
        //										//  2. We are currently processing old money
        //										//  3. We are in the projection period between today and the retirement date
        //										//  4. The rate being applied to new money for this account type and projection month
        //										//		is higher than the corresponding rate being applied to old money 
        //                                                
        //										// Reinitialize vars that will change w/ this transaction row
        //										chrAnnuityBasisType = "PST96";
        //										mnyYMCAContribBalance = 0;
        //										mnyPersonalContribBalance = 0;
        //										//Apply interest differential (pst96 rate - pre96 rate) to old money (pre96)
        //										//Start - YMCA Phase IV - Part 2 - commented by Hafiz on 4-Jun-2008
        //										//mnyPersonalInterestBalance = (laAccountsByBasisProjectionToDate[0] * lnInterestRateDifferential) / 100 / 12;
        //										//mnyYMCAInterestBalance = (laAccountsByBasisProjectionToDate[1] * lnInterestRateDifferential) / 100 / 12;
        //										//End - YMCA Phase IV - Part 2 - commented by Hafiz on 4-Jun-2008
        //										
        //										//Start - YMCA Phase IV - Part 2 - added on 4-Jun-2008 by Hafiz 											
        //										mnyPersonalInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[0], lnInterestRateDifferential, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
        //										mnyYMCAInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[1], lnInterestRateDifferential, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
        //										//End - YMCA Phase IV - Part 2 - added on 4-Jun-2008 by Hafiz 
        //
        //
        //										//'Sum categories
        //										mnyPersonalPreTax = mnyPersonalInterestBalance + mnyPersonalContribBalance;
        //										mnyYMCAPreTax = mnyYMCAInterestBalance + mnyYMCAContribBalance;
        //										mnyPersonalPostTax = 0;
        //										YMCAAmt = mnyYMCAPreTax;
        //										PersonalAmt = mnyPersonalPreTax + mnyPersonalPostTax;
        //										mnyBalance = YMCAAmt + PersonalAmt;
        //										//Update ongoing account balance total
        //										// Insert projected account growth for this account into dtAccountsBasisByProjection
        //										DataRow drAccountsBasisByProjection = dtAccountsBasisByProjection.NewRow();
        //										drAccountsBasisByProjection["dtsTransactDate"] = dtsTransactDate;
        //										drAccountsBasisByProjection["chrProjPeriod"] = chrProjPeriod;
        //										drAccountsBasisByProjection["chrAcctType"] = chrAcctType;
        //										drAccountsBasisByProjection["chrAnnuityBasisType"] = chrAnnuityBasisType;
        //										drAccountsBasisByProjection["mnyYMCAContribBalance"] = mnyYMCAContribBalance;
        //										drAccountsBasisByProjection["mnyPersonalContribBalance"] = mnyPersonalContribBalance;
        //										drAccountsBasisByProjection["mnyPersonalInterestBalance"] = mnyPersonalInterestBalance;
        //										drAccountsBasisByProjection["mnyYMCAInterestBalance"] = mnyYMCAInterestBalance;
        //										drAccountsBasisByProjection["mnyPersonalPreTax"] = mnyPersonalPreTax;
        //										drAccountsBasisByProjection["mnyYMCAPreTax"] = mnyYMCAPreTax;
        //										drAccountsBasisByProjection["mnyPersonalPostTax"] = mnyPersonalPostTax;
        //										drAccountsBasisByProjection["YMCAAmt"] = YMCAAmt;
        //										drAccountsBasisByProjection["PersonalAmt"] = PersonalAmt;
        //										drAccountsBasisByProjection["mnyBalance"] = mnyBalance;
        //										drAccountsBasisByProjection["intProjPeriodSequence"] = intProjPeriodSequence;
        //										drAccountsBasisByProjection["chvPlanType"] = planType;
        //										dtAccountsBasisByProjection.Rows.Add(drAccountsBasisByProjection);
        //										dtAccountsBasisByProjection.GetChanges(DataRowState.Added);
        //									}
        //								} // End of Non Basic Ret60 Else block
        //							}// End of FirstTimeInMonthLoop Else block
        //						}
        //					}
        ////ASHISH:TEST,END
        //					#endregion
        //					// Get the next month to calculate
        //					if (ycCalcMonth.Month == 12) 
        //						ycCalcMonth = Convert.ToDateTime("01/01/" + (Convert.ToDateTime(ycCalcMonth).Year + 1));
        //					else 
        //						ycCalcMonth = Convert.ToDateTime(Convert.ToDateTime(ycCalcMonth).Month + 1 + "/1/ " + ycCalcMonth.Year);
        //				
        //					// If entered a new calendar year, reset annual Contribution limit counters
        //					if (ycCalcMonth.Year != Convert.ToDateTime(ycCalcMonth.AddMonths(-1)).Year) 
        //					{
        //						lnAnnualTDContributions = 0;
        //						lnAnnualContributions = 0;
        //					}
        //				
        //					// See if participant is >= 50 in this projection period
        //					if (DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(retireeBirthday), ycCalcMonth) >= 50)
        //					{
        //						//_ll50 = true;
        //                    
        //						if(dsContributionLimits.Tables[0].Rows.Count > 0) 
        //							lnContributionMaxAnnualTD = Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnualTD"]) + 
        //								Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnual50Addl"]);
        //                    
        //						// Only increase the total contribution by the over-50-td-addl if there is a TD account
        //						if(TDAccountExists)
        //							lnContributionMaxAnnual = Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnual"]) + 
        //								Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnual50Addl"]);
        //					}
        //
        //					if (ycCalcMonth > Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod])) 
        //					{
        //						if (lnProjectionPeriod == laProjectionPeriods.GetUpperBound(1)) 
        //							_llDone = true;
        //						else if(ycCalcMonth > Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)]))
        //						{
        //							_llDone = true;
        //						}
        //						else 
        //						{
        //							if (ycCalcMonth == Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod + 1])) 
        //							{
        //								lnProjectionPeriod = lnProjectionPeriod + 1;
        //								if (laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod].Trim().ToUpper() == "PRE96_FORWARD") 
        //									lnRETIREMENT_PROJECTION_PHASE = lnProjectionPeriod;
        //								else if (laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod].Trim().ToUpper() == "CALCDATE_FORWARD") 
        //									lnRETIREMENT_PROJECTION_PHASE = lnProjectionPeriod;
        //								else if (laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod].Trim().ToUpper() == "RETIREDATE_TO_AGE60") 
        //									lnRETIREDATE_TO_AGE60 = lnProjectionPeriod;
        //							} 
        //						}
        //					}
        //					isFirstTimeInMonthLoop = false;
        //				}// End While
        //
        //				dtAccountsBasisByProjection.AcceptChanges();
        //			}
        //			catch //(Exception ex)
        //			{
        //				throw;//errMsg = ex.Message;
        //			}
        //			errMsg = string.Empty; 
        //		}


        #endregion
        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment, This Function is not in use.
        ///// <summary>
        ///// For regular retirement calculate the YMCA, Personal, and (YMCA+Personal for basic accounts only) amounts 
        ///// for Pre, Post and Rollover serise accounts.
        ///// For Disability retirement calculate the projected YMCA and Personal, Age60 balance.
        ///// </summary>
        ///// <param name="dtAccountsBasisByProjection"></param>
        ///// <param name="lnRETIREMENT_PROJECTION_PHASE"></param>
        ///// <param name="lnRETIREDATE_TO_AGE60"></param>
        ///// <param name="planType"></param>
        //private void calculateFinalAmounts(DataTable dtAccountsBasisByProjection, int lnRETIREMENT_PROJECTION_PHASE
        //    , int lnRETIREDATE_TO_AGE60, string planType)
        //{

        //    try
        //    {
        //        //Added by Ashish for phase V part II changes
        //        CreateAcctBalancesByBasisTypeSchema(ref g_dtAcctBalancesByBasisType);

        //        ycProjected_Basic_YMCA_Age60_Balance = 0;
        //        ycProjected_Basic_Personal_Age60_Balance = 0;
        //        ycProjected_NonBasic_YMCA_Age60_Balance = 0;
        //        ycProjected_NonBasic_Personal_Age60_Balance = 0;

        //        // Set the correct plan type
        //        if (planType == "R")
        //            planType = "RETIREMENT";
        //        else if (planType == "S")
        //            planType = "SAVINGS";
        //        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
        //        //else if (planType == "B")
        //        //{
        //        //    planType = "";
        //        //    foreach(DataRow dr in dtAccountsBasisByProjection.Rows)
        //        //        dr["chvPlanType"] = string.Empty;
        //        //}
        //        ////Added by Ashish for phase V part III changes
        //        //decimal Personal_Retirement_Balance=0;
        //        //decimal YMCA_Retirement_Balance=0;
        //        //decimal Basic_Retirement_Balance=0;
        //        //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
        //        CreateAcctBalancesByBasisType(dtAccountsBasisByProjection, planType, "");

        //        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
        //        //DataRow []drAcctProjectionFoundRows=null;
        //        //DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
        //        //if(g_dtAnnuityBasisTypeList!=null)
        //        //{
        //        //    if(g_dtAnnuityBasisTypeList.Rows.Count >0)
        //        //    {
        //        //        if(lnRETIREMENT_PROJECTION_PHASE != 0)
        //        //        {
        //        //            foreach(DataRow drAnnuityBasisTypeList in g_dtAnnuityBasisTypeList.Rows )
        //        //            {
        //        //                Personal_Retirement_Balance=0;
        //        //                YMCA_Retirement_Balance=0;
        //        //                Basic_Retirement_Balance=0;								
        //        //                string annuityBasisType=string.Empty ;
        //        //                string annuityBasisGroup=string.Empty ;
        //        //                annuityBasisType=drAnnuityBasisTypeList["chrAnnuityBasisType"].ToString().Trim().ToUpper(); 
        //        //                annuityBasisGroup=drAnnuityBasisTypeList["chrAnnuityBasisGroup"].ToString().Trim().ToUpper(); 
        //        //                drAcctProjectionFoundRows=dtAccountsBasisByProjection.Select("chrAnnuityBasisType='" +annuityBasisType+"'");  
        //        //                if(drAcctProjectionFoundRows.Length >0)
        //        //                {

        //        //                    if (dtAccountsBasisByProjection.Select("intProjPeriodSequence<=" + lnRETIREMENT_PROJECTION_PHASE + "AND chrAnnuityBasisType='"+annuityBasisType+"' AND chvPlanType = '" + planType + "'").Length > 0)
        //        //                    {
        //        //                        //personal side money
        //        //                        Personal_Retirement_Balance = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "intProjPeriodSequence<=" + lnRETIREMENT_PROJECTION_PHASE + "AND chrAnnuityBasisType='"+annuityBasisType+"' AND chvPlanType = '" + planType + "'").ToString());
        //        //                        //YMCA side money
        //        //                        YMCA_Retirement_Balance = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "intProjPeriodSequence<=" + lnRETIREMENT_PROJECTION_PHASE + "AND chrAnnuityBasisType='"+annuityBasisType+"' AND chvPlanType = '" + planType + "'").ToString());
        //        //                    }

        //        //                    // basis account money
        //        //                    if (l_dsMetaAccountTypes.Tables.Count != 0)
        //        //                    {
        //        //                        foreach(DataRow drMetaAccts in l_dsMetaAccountTypes.Tables[1].Rows)
        //        //                            foreach(DataRow drProjection in dtAccountsBasisByProjection.Rows)
        //        //                            {
        //        //                                if (drMetaAccts["chrAcctType"].ToString().ToUpper().Trim() == drProjection["chrAcctType"].ToString().ToUpper().Trim() 
        //        //                                    && drProjection["chrAnnuityBasisType"].ToString().ToUpper().Trim() == annuityBasisType 
        //        //                                    && Convert.ToInt32(drProjection["intProjPeriodSequence"].ToString()) <= lnRETIREMENT_PROJECTION_PHASE 
        //        //                                    && drMetaAccts["bitBasicAcct"].ToString() == "True"
        //        //                                    && drProjection["chvPlanType"].ToString() == planType) 
        //        //                                    Basic_Retirement_Balance += 
        //        //                                        Convert.ToDecimal(drProjection["PersonalAmt"]) + Convert.ToDecimal(drProjection["YmcaAmt"]);
        //        //                            }
        //        //                    }

        //        //                    //Added balances into g_dtAcctBalancesByBasisType table
        //        //                    AddAcctBalancesByBasisTypeRow(Personal_Retirement_Balance,YMCA_Retirement_Balance,Basic_Retirement_Balance
        //        //                        ,annuityBasisType,annuityBasisGroup ); 

        //        //                }//if(drAcctProjectionFoundRows.Length >0)
        //        //            } //foreach(DataRow drAnnuityBasisTypeList in g_dtAnnuityBasisTypeList.Rows )
        //        //            //Get disabilty 
        //        //            if (lnRETIREDATE_TO_AGE60 != 0)
        //        //            {
        //        //                if (l_dsMetaAccountTypes.Tables.Count != 0)
        //        //                {
        //        //                    foreach(DataRow drMetaAccts in l_dsMetaAccountTypes.Tables[1].Rows)
        //        //                        foreach(DataRow drProjection in dtAccountsBasisByProjection.Rows)
        //        //                        {
        //        //                            if (drMetaAccts["chrAcctType"].ToString().ToUpper().Trim() == drProjection["chrAcctType"].ToString().ToUpper().Trim() 
        //        //                                && Convert.ToInt32(drProjection["intProjPeriodSequence"].ToString()) <= lnRETIREDATE_TO_AGE60 
        //        //                                && drMetaAccts["bitBasicAcct"].ToString() == "True"
        //        //                                && drProjection["chvPlanType"].ToString() == planType) 
        //        //                            {
        //        //                                //ycProjected_Basic_YMCA_Age60_Balance
        //        //                                ycProjected_Basic_YMCA_Age60_Balance += Convert.ToDecimal(drProjection["YmcaAmt"]);

        //        //                                // ycProjected_Basic_Personal_Age60_Balance
        //        //                                ycProjected_Basic_Personal_Age60_Balance += Convert.ToDecimal(drProjection["PersonalAmt"]);
        //        //                            }
        //        //                        }
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        //Added by Ashish for phase V part III changes ,start

        /// <summary>
        /// This Method calculate account balances for retirement proccessing 
        /// </summary>
        /// <param name="para_RetireeBirthDate"></param>
        /// <param name="para_RetirementDate"></param>
        /// <param name="para_PersID"></param>
        /// <param name="para_FundEventID"></param>
        /// <param name="para_RetireType"></param>
        /// <param name="para_PlanType"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool CalculateAccountBalances(string para_RetireeBirthDate, string para_RetirementDate, string para_PersID, string para_FundEventID, string para_RetireType, string para_PlanType, ref string errorMessage)
        {
            DataTable dtAccountsByBasis = null;
            DataTable dtAccountsBasisByProjection = null;

            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            //int lnRETIREMENT_PROJECTION_PHASE = 0;
            //int lnRETIREDATE_TO_AGE60 = 0;
            errorMessage = string.Empty;
            try
            {

                //step 1.-Get Account balances as on retirement date
                DataSet dsAcctBalByBasis = RetirementBOClass.SearchAccountBalances(true, "01/01/1900", para_RetirementDate, para_PersID, para_FundEventID, false, true, "B");//planType);
                if (dsAcctBalByBasis.Tables.Count > 0 && dsAcctBalByBasis.Tables[0].Rows.Count > 0)
                {
                    dtAccountsByBasis = new DataTable();
                    dtAccountsByBasis = dsAcctBalByBasis.Tables[0].Copy();
                    dtAccountsByBasis.AcceptChanges();
                }
                // Step 2. - Create the projection table with initial values from dtAccountsByBasis
                dtAccountsBasisByProjection = new DataTable();
                dtAccountsBasisByProjection = dtAccountsByBasis.Clone();
                createProjectionTable(dtAccountsBasisByProjection, dtAccountsByBasis);
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //Step 3.- Set retirement phase for calculating final balances
                //if (para_RetireType == "DISABL")
                //{
                //    lnRETIREMENT_PROJECTION_PHASE = 1;
                //    lnRETIREDATE_TO_AGE60 = 1;
                //}
                //else
                //{
                //    lnRETIREMENT_PROJECTION_PHASE = 1;
                //    lnRETIREDATE_TO_AGE60 = 0;
                //}
                //step 4.- Get Annuity basis type List 
                this.g_dtAnnuityBasisTypeList = RetirementBOClass.GetAnnuityBasisTypeList();

                //step 5.-Calculate final ammount 
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                // calculateFinalAmounts(dtAccountsBasisByProjection, lnRETIREMENT_PROJECTION_PHASE, lnRETIREDATE_TO_AGE60, para_PlanType);
                calculateFinalAmounts(dtAccountsBasisByProjection, para_PlanType);
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added by Ashish for phase V part III changes ,end
        ///
        /// <summary>
        /// This Method calculate account balances for retirement proccessing 
        ///This method calculate balances upto Exact effective date
        /// </summary>
        /// <param name="para_RetireeBirthDate"></param>
        /// <param name="para_RetirementDate"></param>
        /// <param name="para_PersID"></param>
        /// <param name="para_FundEventID"></param>
        /// <param name="para_RetireType"></param>
        /// <param name="para_PlanType"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool CalculateAccountBalancesBeforeExactAgeEffDate(string para_RetireeBirthDate, string para_RetirementDate, string para_PersID, string para_FundEventID, string para_RetireType, string para_PlanType, ref string errorMessage)
        {
            DataTable dtAccountsByBasis = null;
            DataTable dtAccountsBasisByProjection = null;

            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            //int lnRETIREMENT_PROJECTION_PHASE = 0;
            //int lnRETIREDATE_TO_AGE60 = 0;
            errorMessage = string.Empty;
            try
            {

                //step 1.-Get Account balances as on retirement date
                DataSet dsAcctBalByBasis = RetirementBOClass.SearchAccountBalancesUptoExactAgeEffDate("1/1/1900", para_RetirementDate, para_PersID, para_FundEventID, "B");//planType);
                if (dsAcctBalByBasis.Tables.Count > 0 && dsAcctBalByBasis.Tables[0].Rows.Count > 0)
                {
                    dtAccountsByBasis = new DataTable();
                    dtAccountsByBasis = dsAcctBalByBasis.Tables[0].Copy();
                    dtAccountsByBasis.AcceptChanges();
                }
                // Step 2. - Create the projection table with initial values from dtAccountsByBasis
                dtAccountsBasisByProjection = new DataTable();
                dtAccountsBasisByProjection = dtAccountsByBasis.Clone();
                createProjectionTable(dtAccountsBasisByProjection, dtAccountsByBasis);
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //Step 3.- Set retirement phase for calculating final balances
                //if (para_RetireType == "DISABL")
                //{
                //    lnRETIREMENT_PROJECTION_PHASE = 1;
                //    lnRETIREDATE_TO_AGE60 = 1;
                //}
                //else
                //{
                //    lnRETIREMENT_PROJECTION_PHASE = 1;
                //    lnRETIREDATE_TO_AGE60 = 0;
                //}
                //step 4.- Get Annuity basis type List 
                this.g_dtAnnuityBasisTypeList = RetirementBOClass.GetAnnuityBasisTypeList();

                //step 5.-Calculate final ammount 
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //calculateFinalAmounts(dtAccountsBasisByProjection, lnRETIREMENT_PROJECTION_PHASE, lnRETIREDATE_TO_AGE60, para_PlanType);
                calculateFinalAmounts(dtAccountsBasisByProjection, para_PlanType);
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Calculate Annuities

        /// <summary>
        /// Fetching the annuity factors on the basis of retirement type, retirement date, retiree birth date, beneficiary birth date
        /// </summary>
        /// <param name="dsRetEstInfo"></param>
        /// <param name="retireType"></param>
        /// <param name="retirementDate"></param>
        /// <param name="retireeBirthdate"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <returns></returns>
        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment,
        // private static DataSet getMetaAnnuityFactors(DataSet dsRetEstInfo, string retireType, string retirementDate, string retireeBirthdate, string beneficiaryBirthDate)
        private static DataSet getMetaAnnuityFactors(DataSet para_dsRetBeneficiaryInfo, string retireType, string retirementDate, string retireeBirthdate, string beneficiaryBirthDate)
        {
            DataSet ds = new DataSet();

            bool boolSurvivor = false;
            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            //bool boolContAndSpouse = false;
            //bool boolPrimDOBPctg = false;
            //double benefitPctg = 0.00;
            string beneficiaryDateOfBirth = string.Empty;

            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            // Step 1. Get survivor status
            //if (dsRetEstInfo.Tables.Count > 0)
            //{
            //    foreach (DataRow dr in dsRetEstInfo.Tables[0].Rows)
            //    {
            //        benefitPctg = Convert.ToDouble(dr["intBenefitPctg"].ToString());
            //        if (dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "PRIM"
            //            && dr["BenBirthDate"].ToString().Trim() != string.Empty
            //            //&& dr["intBenefitPctg"].ToString().Trim().ToUpper() == "100.00000") 
            //            && benefitPctg == 100.00)
            //        {
            //            beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
            //            if (dsRetEstInfo.Tables[0].Rows.Count == 1)
            //            {
            //                boolSurvivor = true;
            //                break;
            //            }
            //            boolPrimDOBPctg = true;
            //        }
            //        if (dr["chvRelationshipCode"].ToString().Trim().ToUpper() == "SP"
            //            & dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "CONT")
            //        {
            //            beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
            //            boolContAndSpouse = true;
            //        }
            //        if ((dr["chvRelationshipCode"].ToString().Trim().ToUpper() == "SP"
            //            & dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "PRIM"))
            //        {
            //            beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
            //            boolSurvivor = true;
            //            break;
            //        }
            //    }
            //}
            //if (boolSurvivor == false && boolPrimDOBPctg == true && boolContAndSpouse == false)
            //    boolSurvivor = true;

            //if (beneficiaryBirthDate.ToString() != "1/1/1900")
            //{
            //    beneficiaryDateOfBirth = beneficiaryBirthDate.ToString();
            //    boolSurvivor = true;
            //}
            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
            //Step 1. Get survivor status and beneficiaryDateOfBirth

            //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary-Start
            //boolSurvivor = IsSurvivorExists(para_dsRetBeneficiaryInfo, beneficiaryBirthDate, out beneficiaryDateOfBirth);

            // Step 2. Get the Annuity Factors as per the survivor status.
            if (IsNonEmpty(para_dsRetBeneficiaryInfo))
            {
                if (para_dsRetBeneficiaryInfo.Tables[0].Rows[0]["BenBirthDate"].ToString().Trim() != string.Empty)
                {
                    beneficiaryDateOfBirth = para_dsRetBeneficiaryInfo.Tables[0].Rows[0]["BenBirthDate"].ToString().Trim();
                    boolSurvivor = true;
                }
            }
            //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary -End


            // Step 2. Get the Annuity Factors as per the survivor status.
            if (boolSurvivor)
                ds = RetirementBOClass.SearchMetaAnnuityFactors(retireType, retirementDate, retireeBirthdate, beneficiaryDateOfBirth);
            else
                ds = RetirementBOClass.SearchMetaAnnuityFactors(retireType, retirementDate, retireeBirthdate, string.Empty);

            return ds;
        }
        ///
        /// <summary>
        /// Fetching the annuity factors on the basis of retirement type, retirement date, retiree birth date, beneficiary birth date
        ///This method get annuity factors use nearst age logic for all annuity type except M annuity
        /// </summary>
        /// <param name="dsRetEstInfo"></param>
        /// <param name="retireType"></param>
        /// <param name="retirementDate"></param>
        /// <param name="retireeBirthdate"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <returns></returns>
        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
        // private static DataSet getMetaAnnuityFactorsBeforeExactAgeEffDate(DataSet dsRetEstInfo, string retireType, string retirementDate, string retireeBirthdate, string beneficiaryBirthDate)
        private static DataSet getMetaAnnuityFactorsBeforeExactAgeEffDate(DataSet para_dsRetBeneficiaryInfo, string retireType, string retirementDate, string retireeBirthdate, string beneficiaryBirthDate)
        {
            DataSet ds = new DataSet();
            bool boolSurvivor = false;
            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            //bool boolContAndSpouse = false;
            //bool boolPrimDOBPctg = false;
            //double benefitPctg = 0.00;
            string beneficiaryDateOfBirth = string.Empty;

            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            //// Step 1. Get survivor status
            //if (dsRetEstInfo.Tables.Count > 0)
            //{
            //    foreach (DataRow dr in dsRetEstInfo.Tables[0].Rows)
            //    {
            //        benefitPctg = Convert.ToDouble(dr["intBenefitPctg"].ToString());
            //        if (dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "PRIM"
            //            && dr["BenBirthDate"].ToString().Trim() != string.Empty
            //            //&& dr["intBenefitPctg"].ToString().Trim().ToUpper() == "100.00000") 
            //            && benefitPctg == 100.00)
            //        {
            //            beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
            //            if (dsRetEstInfo.Tables[0].Rows.Count == 1)
            //            {
            //                boolSurvivor = true;
            //                break;
            //            }
            //            boolPrimDOBPctg = true;
            //        }
            //        if (dr["chvRelationshipCode"].ToString().Trim().ToUpper() == "SP"
            //            & dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "CONT")
            //        {
            //            beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
            //            boolContAndSpouse = true;
            //        }
            //        if ((dr["chvRelationshipCode"].ToString().Trim().ToUpper() == "SP"
            //            & dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "PRIM"))
            //        {
            //            beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
            //            boolSurvivor = true;
            //            break;
            //        }
            //    }
            //}
            //if (boolSurvivor == false && boolPrimDOBPctg == true && boolContAndSpouse == false)
            //    boolSurvivor = true;

            //if (beneficiaryBirthDate.ToString() != "1/1/1900")
            //{
            //    beneficiaryDateOfBirth = beneficiaryBirthDate.ToString();
            //    boolSurvivor = true;
            //}
            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
            //Step 1. Get survivor status and beneficiaryDateOfBirth

            //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary-Start
            //boolSurvivor = IsSurvivorExists(para_dsRetBeneficiaryInfo, beneficiaryBirthDate, out beneficiaryDateOfBirth); 

            // Step 2. Get the Annuity Factors as per the survivor status.
            if (IsNonEmpty(para_dsRetBeneficiaryInfo))
            {
                if (para_dsRetBeneficiaryInfo.Tables[0].Rows[0]["BenBirthDate"].ToString().Trim() != string.Empty)
                {
                    beneficiaryDateOfBirth = para_dsRetBeneficiaryInfo.Tables[0].Rows[0]["BenBirthDate"].ToString().Trim();
                    boolSurvivor = true;
                }
            }
            //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary -End
            if (boolSurvivor)
                ds = RetirementBOClass.SearchMetaAnnuityFactorsBeforeExactAgeEfftDate(retireType, retirementDate, retireeBirthdate, beneficiaryDateOfBirth);
            else
                ds = RetirementBOClass.SearchMetaAnnuityFactorsBeforeExactAgeEfftDate(retireType, retirementDate, retireeBirthdate, string.Empty);

            return ds;
        }
        //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary
        private static bool IsNonEmpty(DataSet ds)
        {
            if (ds == null)
                return false;
            else if (ds.Tables.Count == 0)
                return false;
            else if (ds.Tables[0].Rows.Count == 0)
                return false;

            return true;
        }
        //START - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
        // Addtional things to datatable Is Empty Or not
        private static bool IsNonEmpty(DataTable dt)
        {
            if (dt == null)
                return false;
            else if (dt.Rows.Count == 0)
                return false;

            return true;
        }
        //END - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
        //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isFullBalancePassed"></param>
        /// <param name="benefitValue"></param>		
        /// <param name="dtAnnuitiesStaging"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="retirementDate"></param>
        ///ASHISH:2010.11.16:Remove Average salary parameter YRS 5.0-1215
        //private static void calculateSSBalancing(bool isFullBalancePassed, decimal benefitValue, decimal averageSalary, 
        //    DataTable dtAnnuitiesStaging , DateTime beneficiaryBirthDate, DateTime retireeBirthday, DateTime retirementDate)
        private static void calculateSSBalancing(bool isFullBalancePassed, decimal benefitValue,
            DataTable dtAnnuitiesStaging, DateTime beneficiaryBirthDate, DateTime retireeBirthday, DateTime retirementDate)
        {
            // If full balance is passed set the SS adjustment to 0
            if (isFullBalancePassed)
            {
                foreach (DataRow dr in dtAnnuitiesStaging.Rows)
                {
                    dr["mnySSBefore62"] = 0;
                    dr["mnySSAfter62"] = 0;
                }
            }
            else
            {
                // Get the no of months to retire
                decimal lnRetireeAgeInMonths = Convert.ToDecimal(retirementDate.Year) * 12 + (retirementDate.Month)
                    - Convert.ToDecimal(retireeBirthday.Year * 12) + (retireeBirthday.Month);
                string Ret_Date = retirementDate.ToString();
                //decimal mnySSSalary = averageSalary * 12;
                DataSet dsSSMetaConfigDetails = RetirementBOClass.getSSMetaConfigDetails();
                if (dsSSMetaConfigDetails.Tables.Count != 0)
                {
                    // Get the SS_Min_Age and SS_Max_Age
                    decimal SS_Min_Age = 0;
                    decimal SS_Max_Age = 0;
                    decimal ycSSReductionFactor;
                    decimal lnLeveling;
                    if (dsSSMetaConfigDetails.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in dsSSMetaConfigDetails.Tables[0].Rows)
                        {
                            if (dr["chvKey"].ToString().Trim().ToUpper() == "SS_MIN_AGE")
                                SS_Min_Age = Convert.ToDecimal(dr["chvValue"]);

                            if (dr["chvKey"].ToString().Trim().ToUpper() == "SS_MAX_AGE")
                                SS_Max_Age = Convert.ToDecimal(dr["chvValue"]);
                        }
                    }

                    decimal ycSSEstimatedBenefit = 0;
                    if ((lnRetireeAgeInMonths >= (SS_Min_Age * 12)) & (lnRetireeAgeInMonths <= (SS_Max_Age * 12)))
                    {
                        ycSSEstimatedBenefit = benefitValue;

                        DataSet dsSSReductionFactor = RetirementBOClass.getSSReductionFactor(retireeBirthday.ToString(), Ret_Date);
                        if (dsSSReductionFactor.Tables.Count != 0 && dsSSReductionFactor.Tables[0].Rows.Count != 0)
                            ycSSReductionFactor = Convert.ToDecimal(dsSSReductionFactor.Tables[0].Rows[0]["numReductionFactor"]);
                        else
                            ycSSReductionFactor = 0;

                        lnLeveling = (ycSSEstimatedBenefit * ycSSReductionFactor);
                        if (dtAnnuitiesStaging.Rows.Count != 0)
                        {
                            foreach (DataRow dr in dtAnnuitiesStaging.Rows)
                            {
                                if (Convert.ToDecimal(dr["FinalAnnuity"]) > 0)
                                {
                                    if (Convert.ToDecimal(dr["FinalAnnuity"]) + lnLeveling - ycSSEstimatedBenefit <= 0 || lnLeveling == 0 || ycSSEstimatedBenefit == 0)
                                    {
                                        dr["mnySSBefore62"] = Math.Round(Convert.ToDecimal(dr["FinalAnnuity"]), 2);
                                        dr["mnySSAfter62"] = Math.Round(Convert.ToDecimal(dr["FinalAnnuity"]), 2);
                                    }
                                    else
                                    {
                                        dr["mnySSBefore62"] = Math.Round(Convert.ToDecimal(dr["FinalAnnuity"]) + lnLeveling, 2);
                                        dr["mnySSAfter62"] = Math.Round(Convert.ToDecimal(dr["FinalAnnuity"]) + lnLeveling - ycSSEstimatedBenefit, 2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            dtAnnuitiesStaging.GetChanges(DataRowState.Modified);
            dtAnnuitiesStaging.AcceptChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtAnnuitiesStaging"></param>
        /// <param name="retirementDate"></param>
        private static void calculateSurvivorBenefeciary(DataTable dtAnnuitiesStaging, DateTime retirementDate)
        {
            if (dtAnnuitiesStaging.Rows.Count != 0)
            {
                decimal lnMAnnuity = 0;
                foreach (DataRow dr in dtAnnuitiesStaging.Rows)
                {
                    if (dr["chrAnnuityFactorType"].ToString().Trim().ToUpper() == "M")
                        lnMAnnuity = Convert.ToDecimal(dr["FinalAnnuity"]);
                }

                foreach (DataRow dr in dtAnnuitiesStaging.Rows)
                {
                    if (Convert.ToDecimal(dr["FinalAnnuity"]) > 0)
                    {
                        bool isProcessed = false;
                        bool annuityOptionStatus;
                        decimal lnJointSurvivorPct = 0;
                        string chrAnnuityFactorType = dr["chrAnnuityFactorType"].ToString().Trim().ToUpper();
                        annuityOptionStatus = YCGetAnnuityOption(chrAnnuityFactorType, "JOINTSURVIVORPCTG", retirementDate, ref lnJointSurvivorPct);
                        if (annuityOptionStatus)
                        {
                            if (YCGetAnnuityOption(chrAnnuityFactorType, "POPUP", retirementDate, ref lnJointSurvivorPct))
                            {
                                dr["mnySurvivorBeneficiary"] = (Convert.ToDecimal(dr["FinalAnnuity"]) * (Convert.ToDecimal(lnJointSurvivorPct) / 100));
                                dr["mnySurvivorRetiree"] = Convert.ToDecimal(lnMAnnuity);
                                isProcessed = true;
                            }
                            if (YCGetAnnuityOption(chrAnnuityFactorType, "LASTTODIE", retirementDate, ref lnJointSurvivorPct))
                            {
                                dr["mnySurvivorBeneficiary"] = (Convert.ToDecimal(dr["FinalAnnuity"]) * (Convert.ToDecimal(lnJointSurvivorPct) / 100));
                                dr["mnySurvivorRetiree"] = (Convert.ToDecimal(dr["FinalAnnuity"]) * (Convert.ToDecimal(lnJointSurvivorPct) / 100));
                                isProcessed = true;
                            }
                            if (!isProcessed)
                            {
                                dr["mnySurvivorBeneficiary"] = (Convert.ToDecimal(dr["FinalAnnuity"]) * (Convert.ToDecimal(lnJointSurvivorPct) / 100));
                                dr["mnySurvivorRetiree"] = (Convert.ToDecimal(dr["FinalAnnuity"]));
                                isProcessed = true;
                            }
                        }
                        else
                        {
                            dr["mnySurvivorBeneficiary"] = 0;
                            dr["mnySurvivorRetiree"] = Convert.ToDecimal(dr["FinalAnnuity"]);
                        }
                    }
                }
            }
            dtAnnuitiesStaging.GetChanges(DataRowState.Modified);
            dtAnnuitiesStaging.AcceptChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtAnnuitiesTmp"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="lnNP_RET_M"></param>
        /// <param name="lnB_60_M"></param>
        /// <param name="lnN_RET_M"></param>
        /// <param name="pst96annuity"></param>
        /// <param name="finalannuity"></param>
        /// <param name="lnProjected_Basic_YMCA_Age60_Balance"></param>
        /// <param name="lnProjected_Basic_Personal_Age60_Balance"></param>
        /// <param name="lnProjected_NonBasic_YMCA_Retirement_Balance"></param>
        /// <param name="lnProjected_NonBasic_Personal_Retirement_Balance"></param>
        /// <returns></returns>
        private static decimal getMCTypeAnnuityForDisabilityRetirement(DataTable dtAnnuitiesTmp, DateTime retireeBirthday
            , ref decimal lnNP_RET_M, ref decimal lnB_60_M, ref decimal lnN_RET_M, ref decimal pst96annuity, ref decimal finalannuity
            , decimal lnProjected_Basic_YMCA_Age60_Balance, decimal lnProjected_Basic_Personal_Age60_Balance
            , decimal lnProjected_NonBasic_YMCA_Retirement_Balance, decimal lnProjected_NonBasic_Personal_Retirement_Balance)
        {
            string ldRetireeBirthdate = retireeBirthday.ToString();
            string ldRetireeAge60Date = Convert.ToDateTime(ldRetireeBirthdate).Month + "/1/" + (Convert.ToDateTime(ldRetireeBirthdate).Year + 60);

            decimal lnNY_RET_M = 0;

            decimal lnBY_60_M = 0;
            DataSet dslnBY_60_M = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "M", "NORMAL", "PST96", string.Empty, lnProjected_Basic_YMCA_Age60_Balance);
            if (dslnBY_60_M.Tables.Count != 0 & dslnBY_60_M.Tables[0].Rows.Count != 0)
                lnBY_60_M = Convert.ToDecimal(dslnBY_60_M.Tables[0].Rows[0]["Annuity"]) / 12;

            decimal lnBP_60_M = 0;
            DataSet dslnBP_60_M = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "M", "NORMAL", "PST96", string.Empty, lnProjected_Basic_Personal_Age60_Balance);
            if (dslnBP_60_M.Tables.Count != 0 & dslnBP_60_M.Tables[0].Rows.Count != 0)
                lnBP_60_M = Convert.ToDecimal(dslnBP_60_M.Tables[0].Rows[0]["Annuity"]) / 12;

            lnB_60_M = lnBY_60_M + lnBP_60_M;

            DataSet dslnNY_RET_M = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "M", "NORMAL", "PST96", string.Empty, lnProjected_NonBasic_YMCA_Retirement_Balance);
            if (dslnNY_RET_M.Tables.Count != 0 & dslnNY_RET_M.Tables[0].Rows.Count != 0)
                lnNY_RET_M = Convert.ToDecimal(dslnNY_RET_M.Tables[0].Rows[0]["Annuity"]) / 12;

            DataSet dslnNP_RET_M = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "M", "NORMAL", "PST96", string.Empty, lnProjected_NonBasic_Personal_Retirement_Balance);
            if (dslnNP_RET_M.Tables.Count != 0 & dslnNP_RET_M.Tables[0].Rows.Count != 0)
                lnNP_RET_M = Convert.ToDecimal(dslnNP_RET_M.Tables[0].Rows[0]["Annuity"]) / 12;

            lnN_RET_M = lnNY_RET_M + lnNP_RET_M;
            decimal lnM = lnB_60_M + lnN_RET_M;
            pst96annuity = lnM;
            finalannuity = lnM;

            string chrannuityfactortype = "M";
            if (!dtAnnuitiesTmp.Columns.Contains("chrAnnuityFactorType"))
            {
                dtAnnuitiesTmp.Columns.Add("chrAnnuityFactorType");
                dtAnnuitiesTmp.Columns.Add("pre96Annuity", typeof(decimal));
                dtAnnuitiesTmp.Columns.Add("pst96Annuity", typeof(decimal));
                dtAnnuitiesTmp.Columns.Add("finalAnnuity", typeof(decimal));
            }
            DataRow drAnnuitiesTmp1 = dtAnnuitiesTmp.NewRow();
            drAnnuitiesTmp1["chrAnnuityFactorType"] = chrannuityfactortype;
            drAnnuitiesTmp1["pre96Annuity"] = 0; //pre96Annuity; This is set to hardcoded 0 in CalcAnnuities
            drAnnuitiesTmp1["pst96Annuity"] = pst96annuity;
            drAnnuitiesTmp1["finalannuity"] = finalannuity;
            dtAnnuitiesTmp.GetChanges(DataRowState.Added);
            dtAnnuitiesTmp.AcceptChanges();

            // Calculate the C type annuity
            decimal lnBP_RET_M = 0;
            DataSet dslnBP_RET_M = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "M", "DISABL", "PST96", string.Empty, lnProjected_NonBasic_YMCA_Retirement_Balance);
            if (dslnBP_RET_M.Tables.Count != 0 & dslnBP_RET_M.Tables[0].Rows.Count != 0)
                lnBP_RET_M = Convert.ToDecimal(dslnBP_RET_M.Tables[0].Rows[0]["Annuity"]) / 12;

            decimal lnBP_RET_C = 0;
            DataSet dslnBP_RET_C = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "C", "DISABL", "PST96", string.Empty, lnBP_RET_M);
            if (dslnBP_RET_C.Tables.Count != 0 & dslnBP_RET_C.Tables[0].Rows.Count != 0)
                lnBP_RET_C = Convert.ToDecimal(dslnBP_RET_C.Tables[0].Rows[0]["Annuity"]) / 12;

            decimal lnNP_RET_C = 0;
            DataSet dslnNP_RET_C = RetirementBOClass.SearchAnnuity(ldRetireeBirthdate, ldRetireeAge60Date, "C", "DISABL", "PST96", string.Empty, lnNP_RET_M);
            if (dslnNP_RET_C.Tables.Count != 0 & dslnNP_RET_C.Tables[0].Rows.Count != 0)
                lnNP_RET_C = Convert.ToDecimal(dslnNP_RET_C.Tables[0].Rows[0]["Annuity"]) / 12;

            decimal lnPIA_M = lnB_60_M - lnBP_RET_M;
            decimal lnC = lnBP_RET_C + lnPIA_M + lnNP_RET_C + lnNY_RET_M;
            chrannuityfactortype = "C";
            pst96annuity = lnC;
            finalannuity = lnC;
            drAnnuitiesTmp1 = dtAnnuitiesTmp.NewRow();
            drAnnuitiesTmp1["chrAnnuityFactorType"] = chrannuityfactortype;
            drAnnuitiesTmp1["pre96Annuity"] = 0; //pre96Annuity; This is set to hardcoded 0 in CalcAnnuities
            drAnnuitiesTmp1["pst96Annuity"] = pst96annuity;
            drAnnuitiesTmp1["finalannuity"] = finalannuity;
            dtAnnuitiesTmp.GetChanges(DataRowState.Added);
            dtAnnuitiesTmp.AcceptChanges();

            return lnM;
        }


        /// <summary>
        /// This portion of code has never been executed in the original code, because of the Hardcode
        /// Hence the erroneous peice of code has never been detected.
        /// </summary>
        /// <param name="dtAnnuitiesTmp"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="personID"></param>
        /// <param name="ldRetireeAge60Date"></param>
        /// <param name="lnB_60_M"></param>
        /// <param name="lnN_RET_M"></param>
        private static void getJointTypeAnnuityForDisabilityRetirement(DataTable dtAnnuitiesTmp, DateTime retireeBirthday, string personID
            , string ldRetireeAge60Date, decimal lnB_60_M, decimal lnN_RET_M)
        {
            DataSet dsMetaAnnuityTypes = RetirementBOClass.getMetaAnnuityTypes(retireeBirthday);
            DataSet dsSurvivorsInfo = RetirementBOClass.SearchSurvivorsInfo(personID);
            if (dsMetaAnnuityTypes.Tables.Count != 0 && dsMetaAnnuityTypes.Tables[0].Rows.Count != 0
                && dsSurvivorsInfo.Tables.Count != 0 && dsSurvivorsInfo.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow drMeta in dsMetaAnnuityTypes.Tables[0].Rows)
                    foreach (DataRow drSur in dsSurvivorsInfo.Tables[0].Rows)
                    {
                        decimal lnB_60_J = 0;
                        decimal lnN_RET_J = 0;
                        DataSet dslnB_60_J = RetirementBOClass.SearchAnnuity(drSur["dtmBirthDate"].ToString(), ldRetireeAge60Date, drMeta["chrAnnuityFactorType"].ToString(), "DISABL", "PST96", string.Empty, lnB_60_M);
                        if (dslnB_60_J.Tables.Count != 0 & dslnB_60_J.Tables[0].Rows.Count != 0)
                            lnB_60_J = Convert.ToDecimal(dslnB_60_J.Tables[0].Rows[0]["Annuity"]) / 12;

                        DataSet dslnN_RET_J = RetirementBOClass.SearchAnnuity(drSur["dtmBirthDate"].ToString(), ldRetireeAge60Date, drMeta["chrAnnuityFactorType"].ToString(), "NORMAL", "PST96", string.Empty, lnN_RET_M);
                        if (dslnN_RET_J.Tables.Count != 0 & dslnN_RET_J.Tables[0].Rows.Count != 0)
                            lnN_RET_J = Convert.ToDecimal(dslnN_RET_J.Tables[0].Rows[0]["Annuity"]) / 12;

                        decimal lnJ = lnB_60_J + lnN_RET_J;
                        string chrannuityfactortype = drMeta["chrAnnuityFactorType"].ToString();
                        decimal pst96annuity = lnJ;
                        decimal finalannuity = lnJ;
                        DataRow drAnnuitiesTmp1 = dtAnnuitiesTmp.NewRow();
                        drAnnuitiesTmp1["chrAnnuityFactorType"] = chrannuityfactortype;
                        drAnnuitiesTmp1["pre96Annuity"] = 0; //pre96Annuity; This is set to hardcoded 0 in CalcAnnuities
                        drAnnuitiesTmp1["pst96Annuity"] = pst96annuity;
                        drAnnuitiesTmp1["finalannuity"] = finalannuity;
                    }
            }
            dtAnnuitiesTmp.GetChanges(DataRowState.Added);
            dtAnnuitiesTmp.AcceptChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtAnnuities"></param>
        /// <param name="dtAnnuitiesTmp"></param>
        /// <param name="pst96annuity"></param>
        /// <param name="finalannuity"></param>
        /// <param name="lnM"></param>
        private static void updateAnnuitiesForDisabilityRetirement(DataTable dtAnnuities, DataTable dtAnnuitiesTmp
            , decimal pst96annuity, decimal finalannuity, decimal lnM)
        {
            decimal lnMult = 0;

            dtAnnuities.Columns.Add("chrAnnuityFactorType");
            dtAnnuities.Columns.Add("Pre96Annuity", typeof(decimal));
            dtAnnuities.Columns.Add("Pst96Annuity", typeof(decimal));
            dtAnnuities.Columns.Add("FinalAnnuity", typeof(decimal));
            dtAnnuities.Columns.Add("mnySSBefore62", typeof(decimal));
            dtAnnuities.Columns.Add("mnySSAfter62", typeof(decimal));
            dtAnnuities.Columns.Add("mnySurvivorRetiree", typeof(decimal));
            dtAnnuities.Columns.Add("mnySurvivorBeneficiary", typeof(decimal));
            foreach (DataRow dr in dtAnnuitiesTmp.Rows)
            {
                decimal mnySSBefore62 = 0;
                decimal mnySSAfter62 = 0;
                decimal mnySurvivorBeneficiary = 0;
                decimal mnySurvivorRetiree = 0;
                bool _JfactorType = false;
                bool _JLfactorType = false;
                if (dtAnnuitiesTmp.Select("chrAnnuityFactorType like 'J1%'").Length > 0)
                {
                    lnMult = 1;
                    _JfactorType = true;
                }
                else if (dtAnnuitiesTmp.Select("chrAnnuityFactorType like 'J5%'").Length > 0)
                {
                    lnMult = 0.5M;
                    _JfactorType = true;
                }
                else if (dtAnnuitiesTmp.Select("chrAnnuityFactorType like 'J7%'").Length > 0)
                {
                    lnMult = 0.75M;
                    _JfactorType = true;
                }
                if (dtAnnuitiesTmp.Select("chrAnnuityFactorType like 'JL%'").Length > 0)
                {
                    mnySurvivorBeneficiary = Convert.ToDecimal(dr["FinalAnnuity"]) * lnMult;
                    _JLfactorType = true;
                }
                else if (dtAnnuitiesTmp.Select("chrAnnuityFactorType like 'JP%'").Length > 0)
                {
                    mnySurvivorBeneficiary = Convert.ToDecimal(dr["FinalAnnuity"]) * lnMult;
                    mnySurvivorRetiree = lnM;
                    _JLfactorType = true;
                }
                else
                {
                    mnySurvivorBeneficiary = Convert.ToDecimal(dr["FinalAnnuity"]) * lnMult;
                    mnySurvivorRetiree = 0;
                    _JLfactorType = true;
                }
                if (_JfactorType == false & _JLfactorType == false)
                {
                    dr["mnySurvivorBeneficiary"] = 0;
                    dr["mnySurvivorRetiree"] = 0;
                }
                DataRow drAnnuities = dtAnnuities.NewRow();
                drAnnuities["chrAnnuityFactorType"] = dr["chrAnnuityFactorType"].ToString();
                drAnnuities["Pre96Annuity"] = 0; //pre96Annuity; Had been hard coded to 0
                drAnnuities["Pst96Annuity"] = pst96annuity;
                drAnnuities["FinalAnnuity"] = finalannuity;
                drAnnuities["mnySSBefore62"] = mnySSBefore62;
                drAnnuities["mnySSAfter62"] = mnySSAfter62;
                drAnnuities["mnySurvivorRetiree"] = mnySurvivorRetiree;
                drAnnuities["mnySurvivorBeneficiary"] = mnySurvivorBeneficiary;
                dtAnnuities.Rows.Add(drAnnuities);
                dtAnnuities.GetChanges(DataRowState.Added);
            }
        }


        /// <summary>
        /// Calculate the annuities for the given account values.
        /// </summary>
        /// <param name="tnFullBalance"></param>
        /// <param name="retirementType"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="retirementDate"></param>
        /// <param name="dsRetEstInfoParam"></param>
        /// <param name="averageSalary"></param>
        /// <param name="benefitValue"></param>
        /// <param name="amountToUse"></param>
        /// <param name="ssNO"></param>
        /// <param name="personID"></param>
        /// <param name="dtAnnuitiesList"></param>
        /// <param name="dtAnnuitiesParam"></param>
        /// <param name="finalAnnuityParam"></param>
        /// <returns></returns>
        public DataTable CalculateAnnuities(decimal tnFullBalance, string retirementType, DateTime beneficiaryBirthDate
            , DateTime retireeBirthday, DateTime retirementDate, DataSet dsParticipantBeneficiaries
            , decimal averageSalary, decimal benefitValue, decimal amountToUse, string ssNO, string personID
            , DataTable dtAnnuitiesList, DataTable dtAnnuitiesParam, ref decimal finalAnnuityParam)
        {
            DataTable dtAnnuities = new DataTable();
            //Commented by Ashish for phase V part III changes
            //int lnRetireeRetirementFactorAge = 0;
            string ldRetireeBirthdate;
            string ldRetirementDate;
            decimal finalannuity = 0;
            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
            //This method calculates only annuity for Normal retirement type so set retirment type set with NORMAL type
            retirementType = "NORMAL";
            //DataTable dtAnnuitiesFullBalance = new DataTable();			
            //DataTable dtAnnuitiesTmp = new DataTable();
            //DataTable dtAnnuitiesStaging = new DataTable();
            //decimal pre96Annuity;
            //decimal pst96annuity;

            //Added by Ashish for phase V Part III changes, Start
            DataTable dtM_AnnuityByBasisType = null;
            DataTable dtMetaAnnuityFactor = null;
            DataTable dtAnnuitiesStaging = null;
            //Added by Ashish for phase V Part III changes, End
            ldRetireeBirthdate = retireeBirthday.ToString();
            ldRetirementDate = retirementDate.ToShortDateString();
            //DataSet dsMetaAnnuityTypes = RetirementBOClass.getMetaAnnuityTypes(retirementDate);
            DataSet dsMetaAnnuityFactors = new DataSet();

            // Step 1. Get annuity factors
            dsMetaAnnuityFactors = RetirementBOClass.getMetaAnnuityFactorsBeforeExactAgeEffDate(dsParticipantBeneficiaries, retirementType, ldRetirementDate, ldRetireeBirthdate, beneficiaryBirthDate.ToShortDateString());
            //Added by Ashish for phase V part III changes 
            if (dsMetaAnnuityFactors != null && dsMetaAnnuityFactors.Tables.Count > 0)
            {
                dtMetaAnnuityFactor = dsMetaAnnuityFactors.Tables[0];
            }
            //			//Commented by Ashish for phase V part III changes ,Start
            //			DataTable dtFactorsPRE96 = new DataTable();
            //			DataTable dtFactorsPST96 = new DataTable();
            //			DataTable dtFactorsROLL = new DataTable();

            //			// Using methods instead
            //			// Step 2. Extract the account details as per the basis type.
            //			RetirementBOClass.getAnnuityFactorsByBasis(dsMetaAnnuityFactors, dtFactorsPRE96, dtFactorsPST96, dtFactorsROLL);

            //			int lnBeneficiaryRetirementFactorAge = 0;
            //			int lnRetireeRetirementFactorAgePlusOne = 0;
            //			// Step 3. Get beneficiary and retiree factor age.
            //			RetirementBOClass.getBeneficiaryRetireeFactorAge(personID, retirementType, beneficiaryBirthDate, retireeBirthday, retirementDate
            //				, ref lnBeneficiaryRetirementFactorAge, ref lnRetireeRetirementFactorAge, ref lnRetireeRetirementFactorAgePlusOne);

            //Commented by Ashish for phase V part III changes ,End

            // This method doesnt add any intelligence hence the call has been commented
            // Step 4. Get M annuity factor and Prorate.
            //			RetirementBOClass.getMAnnuityFactorAndProrate(
            //				retirementType, lnRetireeRetirementFactorAge, lnBeneficiaryRetirementFactorAge, retirementDate,
            //				lnRetireeRetirementFactorAgePlusOne, retireeBirthday, retirementDate);

            //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
            //retirementType = "NORMAL";  // This is contentitous should be removed.
            ////Ashish Need to discussed

            // Get vested info
            bool isVested = false;
            DataSet dsIsVested = RetirementBOClass.getVestedInfo(ssNO, ldRetirementDate);
            if (dsIsVested.Tables.Count != 0)
            {
                if (dsIsVested.Tables[0].Rows.Count != 0)
                {
                    if (dsIsVested.Tables[0].Rows[0]["IsVested"].ToString() == "True")
                        isVested = true;
                    else
                        isVested = false;
                }
            }

            // Get full balance info
            decimal lnFullBalance = 0;
            bool isFullBalancePassed = false;
            if (tnFullBalance == 0)
            {
                lnFullBalance = 0;
            }
            else
            {
                lnFullBalance = tnFullBalance;
                isFullBalancePassed = true;
            }

            //Create Table schema for M AnnuityBy BasisType
            dtM_AnnuityByBasisType = CrateTableForM_AnnuityByBasisType();

            // For normal retirement type
            if (retirementType == "NORMAL" || isFullBalancePassed)
            {
                //decimal lnPre96Balance = 0;
                //decimal lnPst96Balance = 0;
                //decimal lnRollBalance = 0;
                //Added by Ashish for phase V part III changes
                decimal lnBalancesByBasisType = 0;

                //If death benefit amount used
                if (isFullBalancePassed)
                {
                    //Commented by Ashish for Phase v part III changes
                    //					lnPre96Balance = 0;
                    //					lnPst96Balance = lnFullBalance;
                    //calculate M annuity for death benefit used amount
                    DataRow drEfectiveBasisType = null;
                    lnBalancesByBasisType = lnFullBalance;

                    drEfectiveBasisType = GetEffectiveAnnuityBasisType(this.g_dtAnnuityBasisTypeList, "PST", retirementDate.Date);
                    if (drEfectiveBasisType != null)
                    {
                        CalculateMAnnuityByBasisType(ref dtM_AnnuityByBasisType, ldRetireeBirthdate, ldRetirementDate, retirementType, drEfectiveBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper(), lnBalancesByBasisType);
                    }
                }
                else
                {
                    //Commented by Ashish for Phase v part III changes
                    //					if (isVested) 
                    //					{
                    //						// GET THESE VALUES POPULATED FROM THE UI
                    //						lnPre96Balance = ycProjected_Pre96_YMCA_Retirement_Balance + ycProjected_Pre96_Personal_Retirement_Balance;
                    //						lnPst96Balance = ycProjected_Pst96_YMCA_Retirement_Balance + ycProjected_Pst96_Personal_Retirement_Balance;
                    //						lnRollBalance = ycProjected_Roll_YMCA_Retirement_Balance + ycProjected_Roll_Personal_Retirement_Balance;
                    //					} 
                    //					else 
                    //					{
                    //						lnPre96Balance = ycProjected_Pre96_Personal_Retirement_Balance;
                    //						lnPst96Balance = ycProjected_Pst96_Personal_Retirement_Balance;
                    //						lnRollBalance = ycProjected_Roll_Personal_Retirement_Balance;
                    //					}



                    if (this.g_dtAcctBalancesByBasisType != null)
                    {
                        if (this.g_dtAcctBalancesByBasisType.Rows.Count > 0)
                        {
                            foreach (DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                            {
                                lnBalancesByBasisType = 0;
                                if (isVested)
                                {
                                    lnBalancesByBasisType = Convert.ToDecimal(drAcctBalancesByBasisType["mnyPersonalRetirementBalance"]) + Convert.ToDecimal(drAcctBalancesByBasisType["mnyYmcaRetirementBalance"]);
                                    //									
                                }
                                else
                                {
                                    lnBalancesByBasisType = Convert.ToDecimal(drAcctBalancesByBasisType["mnyPersonalRetirementBalance"]);
                                    //									
                                }
                                CalculateMAnnuityByBasisType(ref dtM_AnnuityByBasisType, ldRetireeBirthdate, ldRetirementDate, retirementType, drAcctBalancesByBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper(), lnBalancesByBasisType);


                            }//foreach(DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                        }//if(this.g_dtAcctBalancesByBasisType.Rows.Count >0 )
                    }//if(this.g_dtAcctBalancesByBasisType!=null)



                } //Else if (isFullBalancePassed)

                dtAnnuitiesStaging = CalculateAnnuityByBasisType(dtM_AnnuityByBasisType, dtMetaAnnuityFactor, isFullBalancePassed, retirementDate);

                //Commented by Ashish for phase V part III changes,Start


                //				decimal lnPre96M = 0; 
                //				decimal lnPst96M = 0;
                //				decimal lnRollM = 0;
                //				// Get consolidated and active annuity details
                //				dtAnnuitiesTmp = RetirementBOClass.getConsolidatedNormalRetiremenAnnuity(					 
                //					dtFactorsPRE96, dtFactorsPST96, dtFactorsROLL, beneficiaryBirthDate, retireeBirthday, retirementDate
                //					, lnPre96Balance, lnPst96Balance, lnRollBalance, ref lnPre96M, ref lnPst96M, ref lnRollM);
                //
                //				finalAnnuityParam = lnPre96M + lnPst96M + lnRollM;
                //				pre96Annuity = lnPre96M;
                //				pst96annuity = lnPst96M;
                //
                //				// Get staging table with active annuity types
                //				dtAnnuitiesStaging = RetirementBOClass.createStagingTable(isFullBalancePassed, retirementDate,
                //					lnPre96M, lnPst96M, lnRollM, dtAnnuitiesTmp);

                //Commented by Ashish for phase V part III changes,End
                ///ASHISH:2010.11.16:Remove Average Salary parameter for YRS 5.0-1215
                // Update the staging table with the SSBalancing amounts
                //RetirementBOClass.calculateSSBalancing(isFullBalancePassed, benefitValue , averageSalary
                //    , dtAnnuitiesStaging, beneficiaryBirthDate, retireeBirthday, retirementDate);
                RetirementBOClass.calculateSSBalancing(isFullBalancePassed, benefitValue
                    , dtAnnuitiesStaging, beneficiaryBirthDate, retireeBirthday, retirementDate);

                // Update the staging table with the Beneficiary and Survivor amounts
                RetirementBOClass.calculateSurvivorBenefeciary(dtAnnuitiesStaging, retirementDate);

                {
                    dtAnnuities = dtAnnuitiesStaging.Clone();
                    if (dtAnnuitiesStaging.Rows.Count != 0)
                    {
                        for (int i = 0; i <= dtAnnuitiesStaging.Rows.Count - 1; i++)
                        {
                            dtAnnuities.ImportRow(dtAnnuitiesStaging.Rows[i]);
                            dtAnnuities.GetChanges(DataRowState.Added);
                            dtAnnuities.AcceptChanges();
                        }
                    }
                }
            }
            dtAnnuitiesParam = dtAnnuities;

            // Call the contentious method 
            DataTable dt = RetirementBOClass.contentiousMethod(dtAnnuities, isFullBalancePassed, retirementDate, finalannuity);
            dtAnnuitiesList = dt;

            return dt;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtAnnuities"></param>
        /// <param name="isFullBalancePassed"></param>
        /// <param name="retirementDate"></param>
        /// <param name="finalannuity"></param>
        /// <returns></returns>
        private static DataTable contentiousMethod(DataTable dtAnnuities, bool isFullBalancePassed,
            DateTime retirementDate, decimal finalannuity)
        {
            DataTable lcAnnuities = dtAnnuities;
            DataTable lcAnnuitiesListCursor;
            //decimal ycSSEstimatedBenefit  = 0; //not in use

            // 1. Get a list of all active annuities
            DataSet dsAnnuityList = RetirementBOClass.LookUpMetaAnnuityTypes(retirementDate);
            lcAnnuities = dtAnnuities;

            // 4. Take a copy of active annuity list into AnnuitiesListCursor  table
            // This is same as dtAnnuityList
            lcAnnuitiesListCursor = dsAnnuityList.Tables[0].Clone();
            foreach (DataRow dr in dsAnnuityList.Tables[0].Rows)
            {
                lcAnnuitiesListCursor.ImportRow(dr);
                lcAnnuitiesListCursor.GetChanges(DataRowState.Added);
                lcAnnuitiesListCursor.AcceptChanges();
            }

            // 5. In the copy table of annuity list set bitSSLeveling =  false
            // 6. In the same table now set the money columns to 0 
            foreach (DataRow dr in lcAnnuitiesListCursor.Rows)
            {
                //				if (dr["bitSSLeveling"] == DBNull.Value || dr["bitSSLeveling"].ToString() == "false") 
                //				{
                //					dr["bitSSLeveling"] = false;
                //				}
                dr["mnyCurrentPayment"] = 0;
                dr["mnyPersonalPreTaxCurrentPayment"] = 0;
                dr["mnyPersonalPostTaxCurrentPayment"] = 0;
                dr["mnyYmcaPreTaxCurrentPayment"] = 0;
                dr["mnyYmcaPostTaxCurrentPayment"] = 0;
                dr["mnySocialSecurityAdjPayment"] = 0;
                dr["mnyDeathBenefitPayment"] = 0;
                dr["mnySSBefore62"] = 0;
                dr["mnySSAfter62"] = 0;
                dr["mnySSIncrease"] = 0;
                dr["mnySSDecrease"] = 0;
                dr["mnySurvivorRetiree"] = 0;
                dr["mnySurvivorBeneficiary"] = 0;

                //SP: BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
                dr["YmcaAnnuityValue"] = 0;

                lcAnnuitiesListCursor.GetChanges(DataRowState.Modified);
                lcAnnuitiesListCursor.AcceptChanges();
            }

            // 7. Now read all the money values from annuities table and  
            //     populate the copy table of annuity list cursor.
            if (lcAnnuities.Rows.Count != 0 && lcAnnuitiesListCursor.Rows.Count != 0)
            {
                string chrannuityfactortype = string.Empty;
                foreach (DataRow dr in lcAnnuities.Rows)
                {
                    //					decimal mnySSBefore62 = 0;
                    //					decimal mnySSAfter62 = 0;
                    decimal mnySurvivorBeneficiary = 0;
                    decimal mnySurvivorRetiree = 0;
                    //					if ((dr["mnySSBefore62"]) != DBNull.Value) 
                    //						mnySSBefore62 = Convert.ToDecimal(dr["mnySSBefore62"]);
                    //					
                    //					if ((dr["mnySSAfter62"]) != DBNull.Value) 
                    //						mnySSAfter62 = Convert.ToDecimal(dr["mnySSAfter62"]);

                    if ((dr["mnySurvivorBeneficiary"]) != DBNull.Value)
                        mnySurvivorBeneficiary = Convert.ToDecimal(dr["mnySurvivorBeneficiary"]);

                    if ((dr["mnySurvivorRetiree"]) != DBNull.Value)
                        mnySurvivorRetiree = Convert.ToDecimal(dr["mnySurvivorRetiree"]);

                    if ((dr["Finalannuity"]) != DBNull.Value)
                        finalannuity = Convert.ToDecimal(dr["Finalannuity"]);


                    if ((dr["chrAnnuityFactorType"]) != DBNull.Value)
                        chrannuityfactortype = dr["chrAnnuityFactorType"].ToString();

                    foreach (DataRow drCur in lcAnnuitiesListCursor.Rows)
                    {
                        //						if (mnySSBefore62 != 0) 
                        //						{
                        //							if (drCur["chrAnnuityFactorType"].ToString().Trim().ToUpper() == chrannuityfactortype.ToString().Trim().ToUpper() 
                        //								&& drCur["bitSSLeveling"].ToString() == "True")
                        //							{
                        //								drCur["mnySSBefore62"] = mnySSBefore62;
                        //								drCur["mnySSAfter62"] = mnySSAfter62;
                        //								drCur["mnyCurrentPayment"] = finalannuity;
                        //								drCur["mnySurvivorBeneficiary"] = mnySurvivorBeneficiary;
                        //								drCur["mnySurvivorRetiree"] = mnySurvivorRetiree;
                        //								drCur["mnySSIncrease"] = mnySSBefore62 - finalannuity; // mnySSBefore62 = finalannuity + Levelling amount
                        //								drCur["mnySSDecrease"] = ycSSEstimatedBenefit;
                        //							}
                        //						}

                        if (drCur["chrAnnuityType"].ToString().Trim().ToUpper() == dr["chrAnnuityFactorType"].ToString().Trim().ToUpper())
                        {
                            drCur["mnyCurrentPayment"] = Convert.ToDecimal(dr["Finalannuity"]);

                            //SP: BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
                            drCur["YmcaAnnuityValue"] = Convert.ToDecimal(dr["FinalYmcaSideAnnuity"]);

                            //2011.07.27    Sanket Vaidya       YRS 5.0-1150/BT-596 : Option C reduction amount should not include Dth Ben annty
                            if (!isFullBalancePassed)
                                drCur["AnnuityWithoutRDB"] = Convert.ToDecimal(dr["Finalannuity"]);
                            drCur["mnySurvivorRetiree"] = Convert.ToDecimal(dr["mnySurvivorRetiree"]);
                            drCur["mnySurvivorBeneficiary"] = Convert.ToDecimal(dr["mnySurvivorBeneficiary"]);
                            //START: PPP | 03/09/2017 | YRS-AT-2625 | Passing calculated mnyReserveReductionPercent, only in case of "C" annuity calculated for disability could have less than 1 (100%)
                            drCur["mnyReserveReductionPercent"] = Convert.ToDecimal(dr["mnyReserveReductionPercent"]);
                            //END: PPP | 03/09/2017 | YRS-AT-2625 | Passing calculated mnyReserveReductionPercent, only in case of "C" annuity calculated for disability could have less than 1 (100%)
                        }
                    }
                }
            }
            lcAnnuitiesListCursor.GetChanges(DataRowState.Modified);
            lcAnnuitiesListCursor.AcceptChanges();

            return lcAnnuitiesListCursor;
        }


        /// <summary>
        /// Combine the values of the two given tables into one single table.
        /// This method is used only when Death Benefit is used to purchase annuity.
        /// First annuity is calculated using the various contribution amounts 
        /// Then annuity is calculated using the Death Benefit amount
        /// Then both the annuities are combined using this table to arrive at the total final annuity.
        /// </summary>
        /// <param name="lcAnnuitiesListCursor"></param>
        /// <param name="dtAnnuitiesListWithDeathBenefit"></param>
        /// <returns></returns>
        public static DataTable CreateComboTable(DataTable lcAnnuitiesListCursor, DataTable dtAnnuitiesListWithDeathBenefit)//bool isFullBalancePassed)
        {
            // 8. Create table(dtAnnuitiesFullBalanceList) that will only contain non-zero annuity amount.
            DataTable dtAnnuitiesList = new DataTable();
            DataTable dtAnnuitiesFullBalanceList = new DataTable();

            dtAnnuitiesFullBalanceList = dtAnnuitiesListWithDeathBenefit.Clone();
            foreach (DataRow dr in dtAnnuitiesListWithDeathBenefit.Rows)
            {
                if (Convert.ToDecimal(dr["mnyCurrentPayment"]) != 0)
                {
                    dtAnnuitiesFullBalanceList.ImportRow(dr);
                    dtAnnuitiesFullBalanceList.GetChanges(DataRowState.Added);
                    dtAnnuitiesFullBalanceList.AcceptChanges();
                }
            }
            dtAnnuitiesList = lcAnnuitiesListCursor;

            DataRow drAnnuitieslistComboTmp;
            DataTable dtAnnuitieslistComboTmp = new DataTable();
            dtAnnuitieslistComboTmp = dtAnnuitiesFullBalanceList.Clone();

            if (dtAnnuitiesList.Rows.Count != 0 & dtAnnuitiesFullBalanceList.Rows.Count != 0)
            {
                foreach (DataRow drAnnLst in dtAnnuitiesList.Rows)
                    foreach (DataRow drAnnFulBal in dtAnnuitiesFullBalanceList.Rows)
                    {
                        if (drAnnLst["chrDBAnnuitytype"].ToString().Trim().ToUpper() == drAnnFulBal["chrDBAnnuitytype"].ToString().Trim().ToUpper()
                            && (Convert.ToDecimal(drAnnLst["mnyCurrentPayment"]) != 0))
                        {
                            drAnnuitieslistComboTmp = dtAnnuitieslistComboTmp.NewRow();
                            drAnnuitieslistComboTmp["chrAnnuityType"] = drAnnLst["chrAnnuityType"];
                            drAnnuitieslistComboTmp["chrAnnuityFactorType"] = drAnnLst["chrAnnuityFactorType"];
                            drAnnuitieslistComboTmp["chvAnnuityCategoryCode"] = drAnnLst["chvAnnuityCategoryCode"];
                            drAnnuitieslistComboTmp["chvShortDescription"] = drAnnLst["chvShortDescription"];
                            drAnnuitieslistComboTmp["chvDescription"] = drAnnLst["chvDescription"];
                            drAnnuitieslistComboTmp["intCodeOrder"] = drAnnLst["intCodeOrder"];
                            drAnnuitieslistComboTmp["numJointSurvivorPctg"] = drAnnLst["numJointSurvivorPctg"];
                            drAnnuitieslistComboTmp["numIncreasePctg"] = drAnnLst["numIncreasePctg"];
                            drAnnuitieslistComboTmp["bitIncreasing"] = drAnnLst["bitIncreasing"];
                            drAnnuitieslistComboTmp["bitPopup"] = drAnnLst["bitPopup"];
                            drAnnuitieslistComboTmp["bitLastToDie"] = drAnnLst["bitLastToDie"];
                            drAnnuitieslistComboTmp["bitSSLeveling"] = drAnnLst["bitSSLeveling"];
                            drAnnuitieslistComboTmp["bitInsuredReserve"] = drAnnLst["bitInsuredReserve"];
                            drAnnuitieslistComboTmp["bitJointSurvivor"] = drAnnLst["bitJointSurvivor"];
                            drAnnuitieslistComboTmp["chrDBAnnuityType"] = drAnnLst["chrDBAnnuityType"];
                            //2011.07.27    Sanket Vaidya       YRS 5.0-1150/BT-596 : Option C reduction amount should not include Dth Ben annty
                            drAnnuitieslistComboTmp["AnnuityWithoutRDB"] = Convert.ToDecimal(drAnnLst["mnyCurrentPayment"]);
                            drAnnuitieslistComboTmp["mnyCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"]));
                            drAnnuitieslistComboTmp["mnyPersonalPreTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyPersonalPreTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyPersonalPreTaxCurrentPayment"]));
                            drAnnuitieslistComboTmp["mnyPersonalPostTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyPersonalPostTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyPersonalPostTaxCurrentPayment"]));
                            drAnnuitieslistComboTmp["mnyYmcaPreTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyYmcaPreTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyYmcaPreTaxCurrentPayment"]));
                            drAnnuitieslistComboTmp["mnyYmcaPostTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyYmcaPostTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyYmcaPostTaxCurrentPayment"]));
                            drAnnuitieslistComboTmp["mnySocialSecurityAdjPayment"] = (Convert.ToDecimal(drAnnLst["mnySocialSecurityAdjPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnySocialSecurityAdjPayment"]));
                            drAnnuitieslistComboTmp["mnyDeathBenefitPayment"] = (Convert.ToDecimal(drAnnLst["mnyDeathBenefitPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyDeathBenefitPayment"]));


                            if (drAnnLst["bitSSLeveling"].ToString() == "True")
                                drAnnuitieslistComboTmp["mnySSBefore62"] = (Convert.ToDecimal(drAnnLst["mnySSBefore62"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnySSBefore62"]));
                            else
                                drAnnuitieslistComboTmp["mnySSBefore62"] = (Convert.ToDecimal(drAnnLst["mnySSBefore62"])) + (Convert.ToDecimal(drAnnFulBal["mnySSBefore62"]));

                            if (drAnnLst["bitSSLeveling"].ToString() == "True")
                                drAnnuitieslistComboTmp["mnySSAfter62"] = (Convert.ToDecimal(drAnnLst["mnySSAfter62"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnySSBefore62"]));
                            else
                                drAnnuitieslistComboTmp["mnySSBefore62"] = (Convert.ToDecimal(drAnnLst["mnySSAfter62"])) + (Convert.ToDecimal(drAnnFulBal["mnySSAfter62"]));

                            drAnnuitieslistComboTmp["mnySSIncrease"] = (Convert.ToDecimal(drAnnLst["mnySSIncrease"])) + (Convert.ToDecimal(drAnnFulBal["mnySSIncrease"]));
                            drAnnuitieslistComboTmp["mnySSDecrease"] = (Convert.ToDecimal(drAnnLst["mnySSDecrease"])) + (Convert.ToDecimal(drAnnFulBal["mnySSDecrease"]));
                            drAnnuitieslistComboTmp["mnySurvivorRetiree"] = (Convert.ToDecimal(drAnnLst["mnySurvivorRetiree"])) + (Convert.ToDecimal(drAnnFulBal["mnySurvivorRetiree"]));
                            drAnnuitieslistComboTmp["mnySurvivorBeneficiary"] = (Convert.ToDecimal(drAnnLst["mnySurvivorBeneficiary"])) + (Convert.ToDecimal(drAnnFulBal["mnySurvivorBeneficiary"]));

                            //SP: BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
                            drAnnuitieslistComboTmp["YmcaAnnuityValue"] = (Convert.ToDecimal(drAnnLst["YmcaAnnuityValue"]));

                            //START: PPP | 04/18/2017 | YRS-AT-3390 | Copying mnyReserveReductionPercent of main annuity
                            drAnnuitieslistComboTmp["mnyReserveReductionPercent"] = Convert.ToDecimal(drAnnLst["mnyReserveReductionPercent"]);
                            //END: PPP | 04/18/2017 | YRS-AT-3390 | Copying mnyReserveReductionPercent of main annuity

                            dtAnnuitieslistComboTmp.Rows.Add(drAnnuitieslistComboTmp);
                            dtAnnuitieslistComboTmp.GetChanges(DataRowState.Added);
                        }
                    }
                dtAnnuitieslistComboTmp.AcceptChanges();
            }
            else
            {
                dtAnnuitieslistComboTmp = lcAnnuitiesListCursor;
            }

            return dtAnnuitieslistComboTmp;
        }

        //Added by Ashish for phase V part III changes, Start
        //

        /// <summary>
        /// Create schema for M annuity table
        /// </summary>
        /// <returns></returns>
        private DataTable CrateTableForM_AnnuityByBasisType()
        {
            DataTable dtMAnnuityByBasisType = null;
            DataColumn dtNewColumn = null;
            try
            {
                dtMAnnuityByBasisType = new DataTable();
                dtNewColumn = new DataColumn("chrAnnuityType", typeof(string));
                dtMAnnuityByBasisType.Columns.Add(dtNewColumn);

                dtNewColumn = new DataColumn("chrAnnuityBasisTypeCode", typeof(string));
                dtMAnnuityByBasisType.Columns.Add(dtNewColumn);

                dtNewColumn = new DataColumn("mnyAnnuityAmount", typeof(decimal));
                dtNewColumn.DefaultValue = 0;
                dtMAnnuityByBasisType.Columns.Add(dtNewColumn);
                return dtMAnnuityByBasisType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate M annuity as per basis type
        /// </summary>
        /// <param name="para_dtMAnnuityByBasisType"></param>
        /// <param name="para_retireeBirthDate"></param>
        /// <param name="para_retirementDate"></param>
        /// <param name="para_RetirementType"></param>
        /// <param name="para_AnnuityBasisType"></param>
        /// <param name="mnyBalances"></param>
        private void CalculateMAnnuityByBasisType(ref DataTable para_dtMAnnuityByBasisType, string para_retireeBirthDate, string para_retirementDate, string para_RetirementType, string para_AnnuityBasisType, decimal mnyBalances)
        {
            DataSet dsMAnnuity = null;
            decimal mnyMAnnuityValue = 0;
            DataRow drMAnnuityByBasisType;
            try
            {
                if (para_dtMAnnuityByBasisType != null)
                {
                    dsMAnnuity = RetirementBOClass.SearchAnnuity(para_retireeBirthDate, para_retirementDate, "M", para_RetirementType, para_AnnuityBasisType, para_retirementDate, mnyBalances);

                    if (dsMAnnuity.Tables.Count != 0 && dsMAnnuity.Tables[0].Rows.Count != 0)
                    {
                        if (dsMAnnuity.Tables[0].Rows[0]["Annuity"] != DBNull.Value)
                            mnyMAnnuityValue = Math.Round(Convert.ToDecimal(dsMAnnuity.Tables[0].Rows[0]["Annuity"]) / 12, 2);
                        if (mnyMAnnuityValue > 0)
                        {
                            drMAnnuityByBasisType = para_dtMAnnuityByBasisType.NewRow();
                            drMAnnuityByBasisType["chrAnnuityType"] = "M";
                            drMAnnuityByBasisType["chrAnnuityBasisTypeCode"] = para_AnnuityBasisType;
                            drMAnnuityByBasisType["mnyAnnuityAmount"] = mnyMAnnuityValue;
                            para_dtMAnnuityByBasisType.Rows.Add(drMAnnuityByBasisType);
                            para_dtMAnnuityByBasisType.AcceptChanges();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calculate annuity value as per annuity basis type
        /// </summary>
        /// <param name="para_dtMAnnuityByBasisType"></param>
        /// <param name="para_MetaAnnuityFactor"></param>
        /// <param name="para_IsFullBalancePassed"></param>
        /// <param name="para_RetirementDate"></param>
        /// <returns></returns>
        private DataTable CalculateAnnuityByBasisType(DataTable para_dtMAnnuityByBasisType, DataTable para_MetaAnnuityFactor, bool para_IsFullBalancePassed, DateTime para_RetirementDate)
        {
            DataTable l_dtAnnuityList = null;
            DataRow[] drMetaAnnuityFactorRows = null;
            DataRow drMetaAnnuityFactor = null;
            decimal totalMAnnuityValue = 0;
            decimal annuityAmount = 0;
            decimal annuityFactor = 0;

            try
            {
                CreateAnnuityListSchema(ref l_dtAnnuityList);
                if (para_dtMAnnuityByBasisType != null && para_MetaAnnuityFactor != null)
                {
                    if (para_dtMAnnuityByBasisType.Rows.Count > 0 && para_MetaAnnuityFactor.Rows.Count > 0)
                    {
                        string basisTypeCode = string.Empty;
                        string annuityType = string.Empty;
                        decimal basisTypeMAnnuityAmount = 0;
                        foreach (DataRow drMAnnuityBasisType in para_dtMAnnuityByBasisType.Rows)
                        {
                            basisTypeCode = string.Empty;
                            basisTypeMAnnuityAmount = 0;

                            basisTypeCode = drMAnnuityBasisType["chrAnnuityBasisTypeCode"].ToString().Trim().ToUpper();
                            basisTypeMAnnuityAmount = drMAnnuityBasisType["mnyAnnuityAmount"] == System.DBNull.Value ? 0 : Convert.ToDecimal(drMAnnuityBasisType["mnyAnnuityAmount"]);
                            totalMAnnuityValue += basisTypeMAnnuityAmount;

                            drMetaAnnuityFactorRows = para_MetaAnnuityFactor.Select("BasisTypeCode='" + basisTypeCode + "' AND NOT ( AnnuityType='M')");
                            if (drMetaAnnuityFactorRows.Length > 0)
                            {
                                for (int i = 0; i < drMetaAnnuityFactorRows.Length; i++)
                                {
                                    annuityAmount = 0;
                                    annuityFactor = 0;
                                    annuityType = string.Empty;
                                    drMetaAnnuityFactor = drMetaAnnuityFactorRows[i];
                                    annuityType = drMetaAnnuityFactor["AnnuityType"].ToString().Trim().ToUpper();
                                    annuityFactor = drMetaAnnuityFactor["Factor"] == System.DBNull.Value ? 0 : Convert.ToDecimal(drMetaAnnuityFactor["Factor"]);

                                    annuityAmount = Math.Round(annuityFactor * basisTypeMAnnuityAmount, 2);
                                    AddUpdateAnnuityListRow(ref l_dtAnnuityList, annuityType, annuityAmount);
                                }
                            }

                        }//foreach(DataRow drMAnnuityBasisType in para_dtMAnnuityByBasisType.Rows)

                        //Create M Annuity Row in l_dtAnnuityList
                        AddUpdateAnnuityListRow(ref l_dtAnnuityList, "M", totalMAnnuityValue);
                        if (para_IsFullBalancePassed)
                        {
                            DataSet dsDBAnnuityOptions = RetirementBOClass.getDBAnnuityOptions(para_RetirementDate);
                            if (dsDBAnnuityOptions.Tables.Count != 0 && dsDBAnnuityOptions.Tables[0].Rows.Count != 0)
                            {
                                foreach (DataRow dr in l_dtAnnuityList.Rows)
                                {
                                    if (dsDBAnnuityOptions.Tables[0].Select("chrDBAnnuityType='" + dr["chrAnnuityFactorType"].ToString() + "'").Length == 0)
                                    {
                                        dr.Delete();
                                    }
                                }
                                l_dtAnnuityList.AcceptChanges();
                            }
                        }
                    }//if(para_dtMAnnuityByBasisType.Rows.Count >0 && para_MetaAnnuityFactor.Rows.Count >0)

                }//if(para_dtMAnnuityByBasisType!=null && para_MetaAnnuityFactor!=null)
                return l_dtAnnuityList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Create schema for Annuity list table
        /// </summary>
        /// <param name="para_dtAnnuityList"></param>
        private void CreateAnnuityListSchema(ref DataTable para_dtAnnuityList)
        {
            //DataTable dtAnnuityList=null;
            DataColumn dtColumn = null;
            try
            {
                para_dtAnnuityList = new DataTable();
                dtColumn = new DataColumn("chrAnnuityFactorType", typeof(string));
                para_dtAnnuityList.Columns.Add(dtColumn);

                dtColumn = new DataColumn("FinalAnnuity", typeof(decimal));
                dtColumn.DefaultValue = 0;
                para_dtAnnuityList.Columns.Add(dtColumn);

                dtColumn = new DataColumn("mnySSBefore62", typeof(decimal));
                dtColumn.DefaultValue = 0;
                para_dtAnnuityList.Columns.Add(dtColumn);

                dtColumn = new DataColumn("mnySSAfter62", typeof(decimal));
                dtColumn.DefaultValue = 0;
                para_dtAnnuityList.Columns.Add(dtColumn);

                dtColumn = new DataColumn("mnySurvivorRetiree", typeof(decimal));
                para_dtAnnuityList.Columns.Add(dtColumn);
                dtColumn.DefaultValue = 0;

                dtColumn = new DataColumn("mnySurvivorBeneficiary", typeof(decimal));
                dtColumn.DefaultValue = 0;
                para_dtAnnuityList.Columns.Add(dtColumn);

                //SP :BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
                dtColumn = new DataColumn("FinalYmcaSideAnnuity", typeof(decimal));
                dtColumn.DefaultValue = 0;
                para_dtAnnuityList.Columns.Add(dtColumn);
                //SP :BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect

                //START: PPP | 03/09/2017 | YRS-AT-2625 | Following Column reqquired to store reserve reduction factor which will be used monthly payroll process
                // Reservers Remaining (each bucket) = Reservers Remaining (each bucket) - [Current payment of each bucket * reserve reduction factor]
                dtColumn = new DataColumn("mnyReserveReductionPercent", typeof(decimal));
                dtColumn.DefaultValue = 1; //1 represents 100%
                para_dtAnnuityList.Columns.Add(dtColumn);
                //END: PPP | 03/09/2017 | YRS-AT-2625 | Following Column reqquired to store reserve reduction factor which will be used monthly payroll process
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Add or update annuity value  
        /// //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect (addtiona parameter added fo rcalculate ymcaside annuity amt.)
        /// </summary>
        /// <param name="para_dtAnnuityList"></param>
        /// <param name="para_AnnuityType"></param>
        /// <param name="para_AnnuityAmount"></param>
        /// <param name="para_YmcaAnnuityAmount"></param>
        private void AddUpdateAnnuityListRow(ref DataTable para_dtAnnuityList, string para_AnnuityType, decimal para_AnnuityAmount, decimal para_YmcaAnnuityAmount = 0)
        {
            DataRow[] drAnnuityListFoundRow = null;
            DataRow drAnnuityList = null;
            try
            {
                if (para_dtAnnuityList != null)
                {
                    drAnnuityListFoundRow = para_dtAnnuityList.Select(" chrAnnuityFactorType='" + para_AnnuityType + "'");
                    if (drAnnuityListFoundRow.Length > 0)
                    {
                        drAnnuityList = drAnnuityListFoundRow[0];
                        drAnnuityList["FinalAnnuity"] = Math.Round((drAnnuityList["FinalAnnuity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(drAnnuityList["FinalAnnuity"])) + para_AnnuityAmount, 2);

                        //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
                        drAnnuityList["FinalYmcaSideAnnuity"] = Math.Round((drAnnuityList["FinalYmcaSideAnnuity"] == System.DBNull.Value ? 0 : Convert.ToDecimal(drAnnuityList["FinalYmcaSideAnnuity"])) + para_YmcaAnnuityAmount, 2);
                    }
                    else
                    {
                        drAnnuityList = para_dtAnnuityList.NewRow();
                        drAnnuityList["chrAnnuityFactorType"] = para_AnnuityType;
                        drAnnuityList["FinalAnnuity"] = Math.Round(para_AnnuityAmount, 2);
                        drAnnuityList["mnySSBefore62"] = 0;
                        drAnnuityList["mnySSAfter62"] = 0;
                        drAnnuityList["mnySurvivorRetiree"] = 0;
                        drAnnuityList["mnySurvivorBeneficiary"] = 0;

                        //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
                        drAnnuityList["FinalYmcaSideAnnuity"] = Math.Round(para_YmcaAnnuityAmount, 2);

                        //START: PPP | 03/08/2017 | YRS-AT-2625 | For all annuities mnyReserveReductionPercent will 1 (100%), only C Annuity calculated for disability retirement may have have different value based on Actual balance and (Actual balance + projected balance through age 60) 
                        drAnnuityList["mnyReserveReductionPercent"] = GetAnnuityReductionFactor(para_AnnuityType);
                        //END: PPP | 03/08/2017 | YRS-AT-2625 | For all annuities mnyReserveReductionPercent will 1 (100%), only C Annuity calculated for disability retirement may have have different value based on Actual balance and (Actual balance + projected balance through age 60) 
                        para_dtAnnuityList.Rows.Add(drAnnuityList);

                    }
                    para_dtAnnuityList.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        //Added by Ashish for phase V part III changes, End

        #endregion

        #region Custom Methods

        /// <summary>
        /// Calculate the payments for each annuity type to each benefiting entity.
        /// SP: BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
        /// Addtional parameter is added "retirementType
        /// </summary>
        /// <param name="calculateDeathBenefitAnnuity"></param>
        /// <param name="amountToUse"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="retirementDate"></param>
        /// <param name="accounts">Values are read</param>
        /// <param name="annuitiesList">Values are read and returned</param>
        /// <param name="dtFinalAnnuities">Values are returned</param>
        /// <param name="retirementType">Value are read</param>
        public static DataTable CalculatePayments(bool calculateDeathBenefitAnnuity, decimal amountToUse
            , DateTime beneficiaryBirthDate, DateTime retireeBirthday, DateTime retirementDate
            , DataTable dtAnnuitiesList, DataTable dtFinalAnnuities, string planType, string personID, string fundEventID,
            string retirementType = "")
        {
            // The calculation is done in 3 steps
            // Step 1. Get the balance amount in various accounts, and Then for each Annuity Type
            // Step 2. Calculate the basic payments i.e. PersPreTax, PersPstTax, YmcaPreTax, YmcaPstTax, SSAdjPmt n DeathBenPmt
            // Step 3. Calculate the annuity payments Retiree, Pre62, Post62, Survivor n Beneficiary 

            // Get balances by account and by YMCA 
            DataTable dtAccounts = new DataTable();
            DataSet dsAccts = RetirementBOClass.SearchAccountBalances(false, "01/01/1900", retirementDate.ToShortDateString(), personID, fundEventID, false, true, planType);
            if (dsAccts.Tables.Count != 0 && dsAccts.Tables[0].Rows.Count != 0)
            {
                dtAccounts = dsAccts.Tables[0].Copy();
                dtAccounts.AcceptChanges();
            }




            DataTable annuitiesListCursor = dtAnnuitiesList;
            bool bitSSLeveling;
            bool bitJointSurvivor;
            decimal personalNonTaxableBalance = 0;
            decimal personalNonTaxablePayment = 0;
            decimal personalTaxablePayment;
            decimal yMCATaxablePayment;
            decimal yMCAAccountBalance = 0;
            decimal totalAccountBalance = 0;
            decimal mnyCurrentPayment;
            int lnSafeHarborFactor = 0;

            // Step 1.
            if (calculateDeathBenefitAnnuity)
            {
                personalNonTaxableBalance = 0;
                yMCAAccountBalance = amountToUse;
                totalAccountBalance = amountToUse;
            }
            else
            {
                if (dtAccounts.Rows.Count > 0)
                {
                    personalNonTaxableBalance = Convert.ToDecimal(dtAccounts.Compute("SUM(mnyPersonalPostTax)", string.Empty));
                    yMCAAccountBalance = Convert.ToDecimal(dtAccounts.Compute("SUM(mnyYMCAPreTax)", string.Empty));
                    totalAccountBalance = Convert.ToDecimal(dtAccounts.Compute("SUM(mnyBalance)", string.Empty));
                }
            }

            // Step 2.
            //ASHISH:2011.12.09 YRS 5.0-1353/BT-883 /BT-BT-877

            if (annuitiesListCursor != null && annuitiesListCursor.Rows.Count != 0)
            {
                // Iterate through each annuity type and calculate the various amounts.
                foreach (DataRow dr in annuitiesListCursor.Rows)
                {
                    mnyCurrentPayment = Convert.ToDecimal(dr["mnyCurrentPayment"]);
                    bitSSLeveling = dr["bitSSLeveling"].ToString() == "True" ? true : false;
                    bitJointSurvivor = dr["bitJointSurvivor"].ToString() == "True" ? true : false;

                    // Get Safe Harbor Factor
                    lnSafeHarborFactor = RetirementBOClass.YCGetSafeHarborFactor(bitJointSurvivor, beneficiaryBirthDate, retireeBirthday, retirementDate);

                    //Apply Safe Harbor Factor to Payment
                    if (personalNonTaxableBalance <= 0)
                        personalNonTaxablePayment = 0;
                    else
                        personalNonTaxablePayment = Math.Round(personalNonTaxableBalance / lnSafeHarborFactor, 2);

                    // Determine:
                    // YMCA Taxable portion of payment
                    // (Total Payment) x (Taxable YMCA account balances \ Total account balances)
                    // Personal Taxable portion of payment
                    // (Total Payment) - (Personal Nontaxable portion of payment) - (YMCA Taxable portion of payment)                    
                    if (yMCAAccountBalance == 0)
                        yMCATaxablePayment = 0;
                    //SP: BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect - Start
                    else if (retirementType.Equals("DISABL") && dr["chrAnnuityType"].ToString().Trim().Equals("C", StringComparison.InvariantCultureIgnoreCase))
                    {
                        yMCATaxablePayment = Math.Round(Convert.ToDecimal(dr["YmcaAnnuityValue"]), 2);
                    }
                    //SP: BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect -End

                    else
                        yMCATaxablePayment = Math.Round(mnyCurrentPayment * (yMCAAccountBalance / totalAccountBalance), 2);

                    personalTaxablePayment = mnyCurrentPayment - personalNonTaxablePayment - yMCATaxablePayment;

                    dr["mnyPersonalPreTaxCurrentPayment"] = personalTaxablePayment;
                    dr["mnyPersonalPostTaxCurrentPayment"] = personalNonTaxablePayment;
                    dr["mnyYmcaPreTaxCurrentPayment"] = yMCATaxablePayment;
                    dr["mnyYmcaPostTaxCurrentPayment"] = 0;

                    if (bitSSLeveling == true && calculateDeathBenefitAnnuity == false)
                        dr["mnySocialSecurityAdjPayment"] = Convert.ToDecimal(dr["mnySSBefore62"]) - Convert.ToDecimal(dr["mnyCurrentPayment"]);
                    else
                        dr["mnySocialSecurityAdjPayment"] = 0;

                    if (calculateDeathBenefitAnnuity)
                        dr["mnyDeathBenefitPayment"] = dr["mnyCurrentPayment"];
                    else
                        dr["mnyDeathBenefitPayment"] = 0;
                }

                annuitiesListCursor.GetChanges(DataRowState.Modified);
                annuitiesListCursor.AcceptChanges();
            }


            DataRow[] drow_mnySSIncrease;
            //ASHISH:2011.12.09 YRS 5.0-1353 
            if (annuitiesListCursor != null)
            {
                drow_mnySSIncrease = annuitiesListCursor.Select("mnySSIncrease <> 0");
                if (drow_mnySSIncrease.Length == 0)
                    SetAnnuitiesToZero(annuitiesListCursor);
            }

            dtAnnuitiesList = annuitiesListCursor;

            return annuitiesListCursor;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lcAnnuitiesListCursor"></param>
        /// <returns></returns>
        public static DataTable CalculateFinalAnnuity(DataTable lcAnnuitiesListCursor)
        {
            // Create & Populate the holding table that will be displayed in the UI
            DataTable dtFinalAnnuity = new DataTable();
            DataRow drFinalAnnuity;
            dtFinalAnnuity.Columns.Add("Annuity");
            dtFinalAnnuity.Columns.Add("Retire");
            dtFinalAnnuity.Columns.Add("Before62");
            dtFinalAnnuity.Columns.Add("After62");
            dtFinalAnnuity.Columns.Add("Survivor");
            dtFinalAnnuity.Columns.Add("Beneficiary");
            dtFinalAnnuity.AcceptChanges();

            foreach (DataRow dr in lcAnnuitiesListCursor.Rows)
            {

                #region "12/04/2013 : Dinesh.k: BT:1687:Retirement batch estimate giving error when no beneficiaries defined."
                //drFinalAnnuity = dtFinalAnnuity.NewRow();
                //if (Convert.ToDecimal(dr["mnyCurrentPayment"].ToString()) != 0)
                //{
                //    drFinalAnnuity["Annuity"] = dr["chrAnnuityType"].ToString();

                //    drFinalAnnuity["Retire"] = Math.Round(Convert.ToDecimal(dr["mnyCurrentPayment"]), 2).ToString();

                //    if (dr["mnySSBefore62"] != DBNull.Value && Convert.ToDecimal(dr["mnySSBefore62"].ToString()) != 0)
                //        drFinalAnnuity["Before62"] = Math.Round(Convert.ToDecimal(dr["mnySSBefore62"]), 2).ToString();
                //    else
                //        drFinalAnnuity["Before62"] = string.Empty;

                //    if (dr["mnySSAfter62"] != DBNull.Value && Convert.ToDecimal(dr["mnySSAfter62"].ToString()) != 0)
                //        drFinalAnnuity["After62"] = Math.Round(Convert.ToDecimal(dr["mnySSAfter62"]), 2).ToString();
                //    else
                //        drFinalAnnuity["After62"] = string.Empty;

                //    if (dr["mnySurvivorRetiree"] != DBNull.Value && Convert.ToDecimal(dr["mnySurvivorRetiree"].ToString()) != 0)
                //        drFinalAnnuity["Survivor"] = Math.Round(Convert.ToDecimal(dr["mnySurvivorRetiree"]), 2).ToString();
                //    else
                //        drFinalAnnuity["Survivor"] = string.Empty;

                //    if (dr["mnySurvivorBeneficiary"] != DBNull.Value && Convert.ToDecimal(dr["mnySurvivorBeneficiary"].ToString()) != 0)
                //        drFinalAnnuity["Beneficiary"] = Math.Round(Convert.ToDecimal(dr["mnySurvivorBeneficiary"]), 2).ToString();
                //    else
                //        drFinalAnnuity["Beneficiary"] = string.Empty;

                //    dtFinalAnnuity.Rows.Add(drFinalAnnuity);
                //}


                drFinalAnnuity = dtFinalAnnuity.NewRow();
                if (!string.IsNullOrEmpty(dr["mnyCurrentPayment"].ToString()))
                {
                    if (Convert.ToDecimal(dr["mnyCurrentPayment"].ToString()) != 0)
                    {
                        drFinalAnnuity["Annuity"] = dr["chrAnnuityType"].ToString();

                        if (!string.IsNullOrEmpty(dr["mnyCurrentPayment"].ToString()))
                        {
                            drFinalAnnuity["Retire"] = Math.Round(Convert.ToDecimal(dr["mnyCurrentPayment"]), 2).ToString();
                        }


                        if (!string.IsNullOrEmpty(dr["mnySSBefore62"].ToString()))
                        {
                            if (dr["mnySSBefore62"] != DBNull.Value && Convert.ToDecimal(dr["mnySSBefore62"].ToString()) != 0)
                                drFinalAnnuity["Before62"] = Math.Round(Convert.ToDecimal(dr["mnySSBefore62"]), 2).ToString();
                            else
                                drFinalAnnuity["Before62"] = string.Empty;
                        }
                        else
                        {
                            drFinalAnnuity["Before62"] = string.Empty;
                        }


                        if (!string.IsNullOrEmpty(dr["mnySurvivorRetiree"].ToString()))
                        {

                            if (dr["mnySurvivorRetiree"] != DBNull.Value && Convert.ToDecimal(dr["mnySurvivorRetiree"].ToString()) != 0)
                                drFinalAnnuity["Survivor"] = Math.Round(Convert.ToDecimal(dr["mnySurvivorRetiree"]), 2).ToString();
                            else
                                drFinalAnnuity["Survivor"] = string.Empty;
                        }
                        else
                        {
                            drFinalAnnuity["Survivor"] = string.Empty;
                        }


                        if (!string.IsNullOrEmpty(dr["mnySurvivorBeneficiary"].ToString()))
                        {
                            if (dr["mnySurvivorBeneficiary"] != DBNull.Value && Convert.ToDecimal(dr["mnySurvivorBeneficiary"].ToString()) != 0)
                                drFinalAnnuity["Beneficiary"] = Math.Round(Convert.ToDecimal(dr["mnySurvivorBeneficiary"]), 2).ToString();
                            else
                                drFinalAnnuity["Beneficiary"] = string.Empty;
                        }
                        else
                        {
                            drFinalAnnuity["Beneficiary"] = string.Empty;
                        }

                        dtFinalAnnuity.Rows.Add(drFinalAnnuity);
                    }
                }
                #endregion

            }
            dtFinalAnnuity.GetChanges(DataRowState.Added);
            dtFinalAnnuity.AcceptChanges();

            return dtFinalAnnuity;
        }


        /// <summary>
        /// Get retirement death benefit as M type annuity
        /// </summary>
        /// <param name="retireType"></param>
        /// <param name="retirementDate"></param>
        /// <param name="birthDate"></param>
        /// <param name="projectedBasicPre96RetBal"></param>
        /// <param name="projectedBasicPst96RetBal"></param>
        /// <returns></returns>
        public decimal GetRetiredDeathBenefit(string pRetireType, DateTime pRetirementDate, DateTime birthDate, string FundEventId, out string stRdbMessage) //Ranu : YRS-AT-4133 Adding fund event id and rdb message as out
        {
            string retireeBirthdate = birthDate.ToString();
            string beneficiaryBirthdate = string.Empty;
            string retirementDate;
            //string basis = "PST96";
            string annuityOption = "M";
            string retireType = pRetireType;
            //decimal basicPre96RetirementBalance = ycProjected_Basic_Pre96_Retirement_Balance;
            //decimal basicPst96RetirementBalance = ycProjected_Basic_Pst96_Retirement_Balance;
            decimal basicAge60Balance;
            //decimal mAnnuityPre96 = 0;
            //decimal mAnnuityPst96 = 0;
            decimal mAnnuity = 0;
            DataSet getAnnuitize;
            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
            DataRow[] drAcctBalancesByBasisTypeFoundRows = null;
            //Added by Ashish for phase V part III changes
            //string _EffectiveAnnuityBasisType=string.Empty ;
            DataRow drAnnuityBasisType = null;
            decimal mAnnuityByBasisType = 0;
            decimal basicRetirementBalance = 0;

            try
            {
                //START : Benhan David : YRS-AT-4133 Adding fund event id and rdb message as out
                stRdbMessage = string.Empty;
                if (!string.IsNullOrEmpty(FundEventId) && !IsFirstEnrolledBeforeCutOffDate_2019PlanChange(FundEventId) == true)
                {
                    stRdbMessage = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByMessageNo(MetaMessageList.MESSAGE_RETIREMENT_RDB_RESTRICTED_DUE_TO_2019_PLAN_CHANGE, GetRDBPlanChangeCutOffDateReplacementDict()).DisplayText;
                    return 0;
                }
                //END : Benhan David : YRS-AT-4133 Adding fund event id and rdb message as out
                if (g_dtAcctBalancesByBasisType != null)
                {
                    if (g_dtAcctBalancesByBasisType.Rows.Count > 0)
                    {
                        if (retireType == "NORMAL")
                        {
                            retirementDate = pRetirementDate.ToString();
                            if (Convert.ToDateTime(retirementDate).Day != 1)
                            {
                                if (Convert.ToDateTime(retirementDate).Month == 12)
                                    retirementDate = "01/01/" + (Convert.ToDateTime(retirementDate).Year + 1);
                                else
                                    retirementDate = Convert.ToDateTime(retirementDate).Month + 1 + "/01/" + Convert.ToDateTime(retirementDate).Year;
                            }
                            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                            drAcctBalancesByBasisTypeFoundRows = this.g_dtAcctBalancesByBasisType.Select("chvPlanType='RETIREMENT'");
                            if (drAcctBalancesByBasisTypeFoundRows.Length > 0)
                            {
                                for (int i = 0; i < drAcctBalancesByBasisTypeFoundRows.Length; i++)
                                {
                                    //foreach (DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                                    //{
                                    //mAnnuityByBasisType = 0;
                                    //basicRetirementBalance = 0;
                                    //getAnnuitize = null;
                                    //basicRetirementBalance = Convert.ToDecimal(drAcctBalancesByBasisType["mnyBasicAcctRetirementBalance"]);
                                    //getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, retireType, drAcctBalancesByBasisType["chrAnnuityBasisType"].ToString().Trim(), string.Empty, basicRetirementBalance);
                                    //if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0)
                                    //{
                                    //    if (getAnnuitize.Tables[0].Rows[0]["Annuity"] != System.DBNull.Value)
                                    //        mAnnuityByBasisType = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
                                    //}



                                    //mAnnuity += mAnnuityByBasisType;
                                    //}
                                    mAnnuityByBasisType = 0;
                                    basicRetirementBalance = 0;
                                    getAnnuitize = null;
                                    basicRetirementBalance = Convert.ToDecimal(drAcctBalancesByBasisTypeFoundRows[i]["mnyBasicAcctRetirementBalance"]);

                                    getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, retireType, drAcctBalancesByBasisTypeFoundRows[i]["chrAnnuityBasisType"].ToString().Trim(), string.Empty, basicRetirementBalance);
                                    if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0)
                                    {
                                        if (getAnnuitize.Tables[0].Rows[0]["Annuity"] != System.DBNull.Value)
                                            mAnnuityByBasisType = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
                                        //Sanket Vaidya           24 Mar 2011     For YRS 5.0-1295,BT 796  : Minor adjustment to the calculation of the RDB
                                        mAnnuityByBasisType = mAnnuityByBasisType / 12;
                                        mAnnuityByBasisType = Math.Round(mAnnuityByBasisType, 2);

                                    }



                                    mAnnuity += mAnnuityByBasisType;
                                }
                            }
                        }
                        if (retireType == "DISABL")
                        {
                            ////retirementDate = Convert.ToDateTime(birthDate).Month + "/01/" + (Convert.ToDateTime(birthDate).Year + 60);
                            //retirementDate = GetRetireeAge60Date(birthDate.ToShortDateString());
                            //drAnnuityBasisType=RetirementBOClass.GetEffectiveAnnuityBasisType( this.g_dtAnnuityBasisTypeList,"PST",Convert.ToDateTime(retirementDate).Date );   
                            //if(drAnnuityBasisType!=null)
                            //{

                            //    basicAge60Balance = ycProjected_Basic_YMCA_Age60_Balance + ycProjected_Basic_Personal_Age60_Balance;

                            //    beneficiaryBirthdate = retirementDate;
                            //    //getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, retireType, basis, beneficiaryBirthdate, basicAge60Balance);
                            //    // As hardcoded in Maintenance application.
                            //    getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, "NORMAL", drAnnuityBasisType["chrAnnuityBasisType"].ToString().Trim()  , beneficiaryBirthdate, basicAge60Balance);				
                            //    if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0) 
                            //    {
                            //        if(getAnnuitize.Tables[0].Rows[0]["Annuity"]!=System.DBNull.Value)  
                            //        mAnnuity = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
                            //    }
                            //}
                            //ASHISH:2011.02.20
                            //retirementDate = GetRetireeAge60Date(birthDate.ToShortDateString());
                            retirementDate = pRetirementDate.ToShortDateString();
                            drAnnuityBasisType = RetirementBOClass.GetEffectiveAnnuityBasisType(this.g_dtAnnuityBasisTypeList, "PST", Convert.ToDateTime(retirementDate).Date);
                            if (drAnnuityBasisType != null)
                            {
                                drAcctBalancesByBasisTypeFoundRows = this.g_dtAcctBalancesByBasisType.Select("chvPlanType='RETIREMENT'");
                                if (drAcctBalancesByBasisTypeFoundRows.Length > 0)
                                {
                                    //ASHISH:2011.02.20 Set Retiree birth date with age 60 date
                                    //retireeBirthdate = pRetirementDate.AddYears(-60).ToShortDateString(); // PPP | 03/07/2017 | YRS-AT-2625 | Retirees actual age will be passed to gather annuity factor for M annuity
                                    beneficiaryBirthdate = retirementDate;
                                    basicAge60Balance = Convert.ToDecimal(this.g_dtAcctBalancesByBasisType.Compute("SUM(mnyBasicPersonalBalances)+SUM(mnyBasicYmcaBalance)", "chvPlanType='RETIREMENT'"));

                                    //START: PPP | 03/07/2017 | YRS-AT-2625 | "DISABL" retirement type will be passed, So that M annuitywill be calculated based on M disability annuity factor 
                                    //getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, "NORMAL", drAnnuityBasisType["chrAnnuityBasisType"].ToString().Trim(), beneficiaryBirthdate, basicAge60Balance);
                                    getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, pRetireType, drAnnuityBasisType["chrAnnuityBasisType"].ToString().Trim(), beneficiaryBirthdate, basicAge60Balance);
                                    //END: PPP | 03/07/2017 | YRS-AT-2625 | "DISABL" retirement type will be passed, So that M annuitywill be calculated based on M disability annuity factor 
                                    if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0)
                                    {
                                        if (getAnnuitize.Tables[0].Rows[0]["Annuity"] != System.DBNull.Value)
                                            mAnnuity = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
                                        //Sanket Vaidya           24 Mar 2011     For YRS 5.0-1295,BT 796  : Minor adjustment to the calculation of the RDB
                                        mAnnuity = mAnnuity / 12;
                                        mAnnuity = Math.Round(mAnnuity, 2);
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //			if (retireType == "NORMAL") 
            //			{
            //				retirementDate = pRetirementDate.ToString();
            //				if (Convert.ToDateTime(retirementDate).Day != 1) 
            //				{
            //					if (Convert.ToDateTime(retirementDate).Month == 12) 
            //						retirementDate = "01/01/" + (Convert.ToDateTime(retirementDate).Year + 1);
            //					else 
            //						retirementDate = Convert.ToDateTime(retirementDate).Month + 1 + "/01/" + Convert.ToDateTime(retirementDate).Year;
            //				}
            //				getAnnuitize = null;
            //				getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, retireType, "PRE96", string.Empty, basicPre96RetirementBalance);
            //				if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0) 
            //						mAnnuityPre96 = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
            //				
            //				getAnnuitize = null;
            //				getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, retireType, "PST96", string.Empty, basicPst96RetirementBalance);
            //				if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0) 
            //						mAnnuityPst96 = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
            //				
            //				mAnnuity = mAnnuityPst96 + mAnnuityPre96;
            //			} 
            //			else if (retireType == "DISABL") 
            //			{
            //				basicAge60Balance = ycProjected_Basic_YMCA_Age60_Balance + ycProjected_Basic_Personal_Age60_Balance;
            //				retirementDate = Convert.ToDateTime(birthDate).Month + "/01/" + (Convert.ToDateTime(birthDate).Year + 60);
            //				beneficiaryBirthdate = retirementDate;
            //				//getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, retireType, basis, beneficiaryBirthdate, basicAge60Balance);
            //				// As hardcoded in Maintenance application.
            //				getAnnuitize = RetirementBOClass.SearchAnnuity(retireeBirthdate, retirementDate, annuityOption, "NORMAL", basis, beneficiaryBirthdate, basicAge60Balance);				
            //				if (getAnnuitize.Tables.Count != 0 && getAnnuitize.Tables[0].Rows.Count != 0) 
            //						mAnnuity = Convert.ToDecimal(getAnnuitize.Tables[0].Rows[0]["Annuity"]);
            //			}
            //Sanket Vaidya           24 Mar 2011     For YRS 5.0-1295,BT 796  : Minor adjustment to the calculation of the RDB
            mAnnuity = mAnnuity * 12;
            return mAnnuity;
        }


        /// <summary>
        /// The return type of this method is always used as a bool value
        /// Why not convert it into bool then.
        /// Also this is used only in CalculateAnnuities
        /// </summary>
        /// <param name="chrAnnuityFactorType"></param>
        /// <param name="lcOption"></param>
        /// <param name="retirementDate"></param>
        /// <param name="jointSurvivorPctg"></param>
        /// <returns></returns>
        public static bool YCGetAnnuityOption(string annuityFactorType, string option, DateTime retirementDate, ref decimal jointSurvivorPctg)
        {
            DataSet dsMetaAnnuityTypes;
            bool annuityOption = false;

            dsMetaAnnuityTypes = RetirementBOClass.getMetaAnnuityTypes(retirementDate);

            if (dsMetaAnnuityTypes.Tables.Count != 0 && dsMetaAnnuityTypes.Tables[0].Rows.Count != 0)
            {
                DataRow drAnnuityTypes;
                switch (option)
                {
                    case "JOINTSURVIVORPCTG":
                        if (dsMetaAnnuityTypes.Tables[0].Select("chrAnnuityType= '" + annuityFactorType + "'").Length > 0)
                        {
                            drAnnuityTypes = dsMetaAnnuityTypes.Tables[0].NewRow();
                            drAnnuityTypes = dsMetaAnnuityTypes.Tables[0].Select("chrAnnuityType= '" + annuityFactorType + "'")[0];
                            annuityOption = true;
                            jointSurvivorPctg = Convert.ToDecimal(drAnnuityTypes["numJointSurvivorPctg"].ToString());
                        }
                        break;
                    case "INCREASING":
                        if (dsMetaAnnuityTypes.Tables[0].Select("chrAnnuityType= '" + annuityFactorType + "'").Length > 0)
                            annuityOption = true;
                        break;
                    case "POPUP":
                        if (dsMetaAnnuityTypes.Tables[0].Select("chrAnnuityType= '" + annuityFactorType + "' AND bitPopup = 1").Length > 0)
                            annuityOption = true;
                        break;
                    case "LASTTODIE":
                        if (dsMetaAnnuityTypes.Tables[0].Select("chrAnnuityType= '" + annuityFactorType + "' AND bitLastToDie = 1").Length > 0)
                            annuityOption = true;
                        break;
                    case "SSLEVELING":
                        if (dsMetaAnnuityTypes.Tables[0].Select("chrAnnuityType= '" + annuityFactorType + "'+'S' AND bitSSLeveling = 1").Length > 0)
                            annuityOption = true;
                        break;
                    default:
                        annuityOption = false;
                        break;
                }
            }
            return annuityOption;
        }


        /// <summary>
        /// Get and Set the Max & Min Beneficiary & Retiree age from the database
        /// This method is called only from CalculateAnnuities
        /// </summary>
        /// <param name="tdBirthDate"></param>
        /// <param name="tdRetireDate"></param>
        /// <param name="tlBeneficiary"></param>
        /// <param name="retireType"></param>
        /// <returns></returns>
        public static int GetFactorAge(DateTime dateOfBirth, DateTime retirementDate, bool isBeneficiary, string retirementType)
        {
            int maxBen = 100;
            int maxRet = 100;
            int minBen = 0;
            int minRet = 0;
            string retireType = retirementType;

            // Check if Retirement and BirthDate is provided
            string birthDate;
            if (dateOfBirth.ToString() == EMPTY_DATE)
                return 0;
            else
                birthDate = dateOfBirth.ToString();

            DateTime retireDate;
            if (retirementDate.ToString() == EMPTY_DATE)
                return 0;
            else
                retireDate = retirementDate;

            // Get Min and Max of both Retirement and Beneficiary Age from the database.
            DataSet dsBeneficiaryMinMaxAge = new DataSet();

            if (retireType == "NORMAL")
                dsBeneficiaryMinMaxAge = RetirementBOClass.SearchBeneficiaryMinMaxAge();
            else if (retireType == "DISABL")
                dsBeneficiaryMinMaxAge = RetirementBOClass.SearchDisabledBeneficiaryMinMaxAge();

            if ((dsBeneficiaryMinMaxAge.Tables.Count != 0) && (dsBeneficiaryMinMaxAge.Tables[0].Rows.Count != 0))
            {
                DataRow dr = dsBeneficiaryMinMaxAge.Tables[0].Rows[0];
                // Parse the value to decimal and then integer as direct conversion to int throws error
                maxBen = Convert.ToInt32(Decimal.Parse(dr["maxBen"].ToString()));
                maxRet = Convert.ToInt32(Decimal.Parse(dr["maxRet"].ToString()));
                minBen = Convert.ToInt32(Decimal.Parse(dr["minBen"].ToString()));
                minRet = Convert.ToInt32(Decimal.Parse(dr["minRet"].ToString()));
            }

            // Add 6 months to age as actuarial adjustment
            // Legacy Comment - retiree age is not adjusted as per Posey 		
            //                - reiree is to be adjusted actuarialy as per Alan Mooney
            string factorBirthDate = DateTime.MinValue.ToString();
            if (Convert.ToDateTime(birthDate).Day == 1)
            {
                if (Convert.ToDateTime(birthDate).Month == 1)
                {
                    factorBirthDate = "7/" + Convert.ToDateTime(birthDate).Day + "/" + (Convert.ToDateTime(birthDate).Year - 1);
                }
            }
            else
            {
                if (Convert.ToDateTime(birthDate).Month != 12)
                    birthDate = Convert.ToDateTime(birthDate).Month + 1 + "/01/" + Convert.ToDateTime(birthDate).Year + 1;
                else
                    birthDate = Convert.ToDateTime(birthDate).Month + "/01/" + Convert.ToDateTime(birthDate).Year + 1;
            }

            // Get the retirement age in years
            int years;
            years = DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(factorBirthDate), Convert.ToDateTime(retireDate));

            // Set the age to be with in the limits
            if (isBeneficiary)
            {
                if (years < minBen)
                    years = minBen;
                if (years > maxBen)
                    years = maxBen;
            }
            else
            {
                if (years < minRet)
                    years = minRet;
                if (years > maxRet)
                    years = maxRet;
            }
            return years;
        }


        /// <summary>
        /// Get the safe harbour factor from the database
        /// It is used only in Calculate Payments
        /// </summary>
        /// <param name="jointSurvivor"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="retirementDate"></param>
        /// <returns></returns>
        public static int YCGetSafeHarborFactor(bool jointSurvivor, DateTime beneficiaryBirthDate, DateTime retireeBirthday, DateTime retirementDate)
        {
            string beneBirthDate = string.Empty;
            int safeHarborFactor;
            if (jointSurvivor)
            {
                if (Convert.ToString(beneficiaryBirthDate) != EMPTY_DATE)
                {
                    beneBirthDate = beneficiaryBirthDate.ToString("MM/dd/yyyy");
                }
            }
            safeHarborFactor = RetirementBOClass.getSafeHarborFactor(retireeBirthday.ToString(), beneBirthDate, retirementDate.ToString());
            return safeHarborFactor;
        }


        /// <summary>
        /// Set the social security options
        /// </summary>
        /// <param name="dtSSOptions"></param>
        public static void SetAnnuitiesToZero(DataTable dtSSOptions)
        {
            DataRow[] drSSOptions = dtSSOptions.Select("chrAnnuityType LIKE '%S'");
            if (drSSOptions.Length > 0)
            {
                for (int i = 0; i <= drSSOptions.Length - 1; i++)
                {
                    drSSOptions[i]["mnyCurrentPayment"] = 0;
                    drSSOptions[i]["mnySSBefore62"] = 0;
                    drSSOptions[i]["mnySSAfter62"] = 0;
                    drSSOptions[i]["mnySurvivorRetiree"] = 0;
                    drSSOptions[i]["mnySurvivorBeneficiary"] = 0;
                }
            }
        }


        /// <summary>
        /// This method seems to be a copy of ComboData method and hence the operation is getting duplicated and
        /// the resulting values getting doubled.
        /// </summary>
        /// <param name="calculateDeathBenefitAnnuity"></param>
        /// <param name="AnnuityList"></param>
        /// <returns></returns>
        public static DataTable GetComboData(bool calculateDeathBenefitAnnuity, DataTable AnnuityList)
        {
            DataTable dtAnnuityListTmp;
            DataTable dtAnnuityList;
            DataRow drAnnuityList;
            DataRow[] drFindAnnuityFullLists;
            DataRow drFindAnnuityFullList;
            DataTable dtAnnuityFullBalanceListTmp;

            // Reset blank decimal values to 0.0
            foreach (DataRow dr in AnnuityList.Rows)
            {
                for (int i = 18; i < AnnuityList.Columns.Count; i++)
                {
                    if (dr[i].ToString().Trim() == string.Empty)
                    {
                        dr[i] = "0";
                    }
                    AnnuityList.GetChanges(DataRowState.Modified);
                    AnnuityList.AcceptChanges();
                }
            }

            dtAnnuityFullBalanceListTmp = AnnuityList;

            dtAnnuityListTmp = AnnuityList.Clone();
            foreach (DataRow dr in AnnuityList.Rows)
            {
                dtAnnuityListTmp.ImportRow(dr);
                dtAnnuityListTmp.GetChanges(DataRowState.Added);
                dtAnnuityListTmp.AcceptChanges();
            }

            dtAnnuityList = dtAnnuityListTmp.Clone();
            foreach (DataRow dr in dtAnnuityListTmp.Rows)
            {
                dtAnnuityList.ImportRow(dr);
                dtAnnuityList.GetChanges(DataRowState.Added);
                dtAnnuityList.AcceptChanges();
            }

            if (dtAnnuityListTmp.Rows.Count > 0 && calculateDeathBenefitAnnuity)
            {
                for (int i = 0; i <= dtAnnuityList.Rows.Count - 1; i++)
                {
                    drAnnuityList = dtAnnuityList.Rows[i];
                    drFindAnnuityFullLists = dtAnnuityFullBalanceListTmp.Select("chrDBAnnuityType = '" + drAnnuityList["chrDBAnnuityType"].ToString() + "' AND mnyCurrentPayment <> 0");
                    if (drFindAnnuityFullLists.Length > 0 & Convert.ToDecimal(drAnnuityList["mnyCurrentPayment"].ToString()) > 0)
                    {
                        drFindAnnuityFullList = drFindAnnuityFullLists[0];
                        drAnnuityList["mnyCurrentPayment"] = Convert.ToDecimal(drAnnuityList["mnyCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyCurrentPayment"].ToString());
                        drAnnuityList["mnyPersonalPreTaxCurrentPayment"] = Convert.ToDecimal(drAnnuityList["mnyPersonalPreTaxCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyPersonalPreTaxCurrentPayment"].ToString());
                        drAnnuityList["mnyPersonalPostTaxCurrentPayment"] = Convert.ToDecimal(drAnnuityList["mnyPersonalPostTaxCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyPersonalPostTaxCurrentPayment"].ToString());
                        drAnnuityList["mnyYmcaPreTaxCurrentPayment"] = Convert.ToDecimal(drAnnuityList["mnyYmcaPreTaxCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyYmcaPreTaxCurrentPayment"].ToString());
                        drAnnuityList["mnyYmcaPostTaxCurrentPayment"] = Convert.ToDecimal(drAnnuityList["mnyYmcaPostTaxCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyYmcaPostTaxCurrentPayment"].ToString());
                        drAnnuityList["mnySocialSecurityAdjPayment"] = Convert.ToDecimal(drAnnuityList["mnySocialSecurityAdjPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySocialSecurityAdjPayment"].ToString());
                        drAnnuityList["mnyDeathBenefitPayment"] = Convert.ToDecimal(drAnnuityList["mnyDeathBenefitPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyDeathBenefitPayment"].ToString());
                        if (((bool)(drAnnuityList["bitSSLeveling"])) == true)
                        {
                            drAnnuityList["mnySSBefore62"] = Convert.ToDecimal(drAnnuityList["mnySSBefore62"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySSBefore62"].ToString());
                        }
                        else
                        {
                            drAnnuityList["mnySSBefore62"] = Convert.ToDecimal(drAnnuityList["mnySSBefore62"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySSBefore62"].ToString());
                        }
                        if (((bool)(drAnnuityList["bitSSLeveling"])) == true)
                        {
                            drAnnuityList["mnySSAfter62"] = Convert.ToDecimal(drAnnuityList["mnySSAfter62"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnyCurrentPayment"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySSAfter62"].ToString());
                        }
                        else
                        {
                            drAnnuityList["mnySSAfter62"] = Convert.ToDecimal(drAnnuityList["mnySSAfter62"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySSAfter62"].ToString());
                        }
                        drAnnuityList["mnySSIncrease"] = Convert.ToDecimal(drAnnuityList["mnySSIncrease"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySSIncrease"].ToString());
                        drAnnuityList["mnySSDecrease"] = Convert.ToDecimal(drAnnuityList["mnySSDecrease"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySSDecrease"].ToString());
                        drAnnuityList["mnySurvivorRetiree"] = Convert.ToDecimal(drAnnuityList["mnySurvivorRetiree"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySurvivorRetiree"].ToString());
                        drAnnuityList["mnySurvivorBeneficiary"] = Convert.ToDecimal(drAnnuityList["mnySurvivorBeneficiary"].ToString()) + Convert.ToDecimal(drFindAnnuityFullList["mnySurvivorBeneficiary"].ToString());
                        drAnnuityList.AcceptChanges();
                    }
                }
            }
            return dtAnnuityList;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="isEstimate"></param>
        /// <param name="fundEventID"></param>
        /// <param name="retirementDate"></param>
        /// <param name="retirementType"></param>
        /// <param name="personID"></param>
        /// <param name="ssNO"></param>		
        /// <param name="fundSatus"></param>
        /// <param name="planType"></param>
        /// <returns></returns>
        //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624, Remove terminationDate parameter ,it is not in use.
        //public static bool IsRetirementValid(ref string errorMessage, bool isEstimate, string fundEventID, string retirementDate, 
        //    string retirementType, string personID, string ssNO, string terminationDate, string fundSatus, string planType )
        public static bool IsRetirementValid(ref string errorMessage, bool isEstimate, string fundEventID, string retirementDate,
            string retirementType, string personID, string ssNO, string fundSatus, string planType, bool PersonalWithdrawalExists = false
            ,bool HasSatisfiedPaidService = true  // SR | 2017.04.07 | YRS-AT-3390 | Pass HasSatisfiedPaidService parameter to get Paid service to validate insufficient Paid service.
            )
        {
            string RET_CONSTANT = string.Empty;
            if (isEstimate)
                RET_CONSTANT = "ESTIMATE";
            else
                RET_CONSTANT = "RETIRE";
            DataSet ycEligibleNoRefundsPending = new DataSet();
            string ycRetireeRetirementDate = retirementDate;
            string strFundEventId = fundEventID;
            bool retirementIsValid = true;
            //errorMessage += "Eligibility criteria is not met:"; //commented on 6-Jan-2009 for YRS 5.0-636
            //errorMessage = "Eligibility criteria is not met:"; //added on 6-Jan-2009 for YRS 5.0-636
            //errorMessage = "Eligibility criteria is not met:"; //commented on 24-sep for BT-1126
            errorMessage = "MESSAGE_RETIREMENT_BOC_ELEGIBILITY_CRITERTIA";
            /*'PENDING 'CHECK ELIGIBILITY */
            if (!isEstimate)
            {
                // PENDING FUND EXISTS
                ycEligibleNoRefundsPending = RetirementBOClass.EligNoRefundsPending(personID);
                if (!(ycEligibleNoRefundsPending.Tables.Count == 0))
                {
                    if (!(ycEligibleNoRefundsPending.Tables[0].Rows.Count == 0))
                    {
                        //errorMessage += "<br> * " + "Pending refund exists"; //commented on 24-sep for BT-1126
                        errorMessage += ",MESSAGE_RETIREMENT_BOC_PENDING_REFUND_EXISTS";
                        retirementIsValid = false;
                    }
                }

                // UNFUNDED TRANSACTION EXISTS
                DataSet ycEligibleUnFundedTransactionExist = new DataSet();
                if (strFundEventId != string.Empty)
                {
                    ycEligibleUnFundedTransactionExist = RetirementBOClass.EligibleUnFundedTransactionExist(strFundEventId);
                    if (!(ycEligibleUnFundedTransactionExist.Tables.Count == 0))
                    {
                        if (!(ycEligibleUnFundedTransactionExist.Tables[0].Rows.Count == 0))
                        {
                            //errorMessage += "<br> * " + "Unfunded transaction exists";//commented on 24-sep for BT-1126
                            errorMessage += ",MESSAGE_RETIREMENT_BOC_UNFUNDED_TRANS_EXISTS";
                            retirementIsValid = false;
                        }
                    }
                }
            }

            // INVALID OPERATION FOR FUND EVENT
            DataSet ycEligibleFundEvent = new DataSet();
            if (strFundEventId != string.Empty)
            {
                ycEligibleFundEvent = RetirementBOClass.EligibleFundEvent(RET_CONSTANT, "NORMAL", strFundEventId);
                if (ycEligibleFundEvent.Tables.Count != 0 && ycEligibleFundEvent.Tables[0].Rows.Count != 0)
                {
                    if (ycEligibleFundEvent.Tables[0].Rows[0]["bitCan"].ToString() != "True")
                    {
                        //errorMessage += "<br> * " + "Normal retirement not permitted for this fund event";//commented on 24-sep for BT-1126
                        errorMessage += ",MESSAGE_RETIREMENT_BOC_NORMAL_RETIREMENT_NOT_PERMITTED";
                        retirementIsValid = false;
                    }
                }
                else
                {
                    //errorMessage += "<br> * " + "Invalid fund event data encountered";//commented on 24-sep for BT-1126
                    errorMessage += ",MESSAGE_RETIREMENT_BOC_INVALID_FUND_EVENT";
                    retirementIsValid = false;
                }
            }

            // Get Vested info
            bool llVestedOnRetireDate = false;
            decimal lnRetireDatePIABalance = 0;
            int ycEligiblePIA = 0;
            decimal lnNonPIAFunds = 0;
            decimal lnPIAFunds = 0;
            DataSet ycIsVested = RetirementBOClass.getVestedInfo(ssNO, ycRetireeRetirementDate);
            if (ycIsVested.Tables.Count != 0)
            {
                if (ycIsVested.Tables[0].Rows[0]["IsVested"].ToString() != "True")
                {
                    llVestedOnRetireDate = false;
                    lnRetireDatePIABalance = 0;
                }
                else
                {
                    llVestedOnRetireDate = true;
                    lnRetireDatePIABalance = lnNonPIAFunds + lnPIAFunds;
                }
            }
            if (lnPIAFunds == 0 && lnNonPIAFunds == 0)
            {
                //No money was passed, so instead of passing a balance,pass if they were vested or not
                ycEligiblePIA = RetirementBOClass.getEligiblePIA(strFundEventId, llVestedOnRetireDate);
            }

            if (ycEligiblePIA != 0)
            {
                if (lnPIAFunds != 0 & lnNonPIAFunds == 0)
                {
                    //errorMessage += "<br> * " + "Insufficient account balances";//commented on 24-sep for BT-1126
                    errorMessage += ",MESSAGE_RETIREMENT_BOC_INSUFFICIENT_ACCOUNT";
                    retirementIsValid = false;
                }
            }

            // Validation as per Retirement Type
            // This check will not be invoked if the person is already retired (RT or RD)
            //if (fundSatus != "RT")
            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment, take away disability validation from fund status(RT,RPT) check 
            //if ((fundSatus != "RT") && (fundSatus != "RPT"))// && fundSatus != "RD") //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            //{
            int ycEligibleIsQDRO;
            bool llEligibleServiceInMonths;
            //int ycEligibleServiceInMonths; //PPP | 11/24/2017 | YRS-AT-3319 | Variable is declared but not used
            string ycEligibleAge;
            DataSet ycEligibleSSPlanPre1974;
            bool llEligibleSSPlanPre1974 = false;
            int ycEstimateAge;
            //int ycEligibleNotTerminatedWithinMonths;
            string strRetireType;
            strRetireType = retirementType;

            //string l_string_RetirementDate = DateTime.Now.ToString("MM/dd/yyyy");//added by hafiz on 12/16/2008
            if ((fundSatus != "RT") && (fundSatus != "RPT"))// && fundSatus != "RD") //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            {
                if (strRetireType == "NORMAL")
                {
                    ycEligibleIsQDRO = RetirementBOClass.EligibleIsQDRO(strFundEventId);
                    if (ycEligibleIsQDRO == 0) // '''Not a QDRO  '''Continue Processing
                    {
                        //ycEligibleServiceInMonths = RetirementBOClass.getEligibleServiceMinMonths(strFundEventId, false, RET_CONSTANT, ycRetireeRetirementDate);
                        llEligibleServiceInMonths = true;

                        // '''Try Age Criteria again (min 55) with service result

                        //l_string_RetirementDate = ycRetireeRetirementDate;

                        //commented by hafiz on 12/16/2008
                        //ycEligibleAge = RetirementBOClass.EligibleAge(personID, DateTime.Now.ToString("MM/dd/yyyy"), false, "NORMAL", llEligibleServiceInMonths, false, strFundEventId);
                        //ASHISH:YRS-1345/BT-859 commented code, it is a repeated  code
                        //ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, false, "NORMAL", llEligibleServiceInMonths, false, strFundEventId);

                        // '''SS Plan Participation before 1/1/74
                        ycEligibleSSPlanPre1974 = RetirementBOClass.EligibleSSPlanPre1974(personID);
                        if (ycEligibleSSPlanPre1974.Tables.Count != 0)
                        {
                            if (ycEligibleSSPlanPre1974.Tables[0].Rows.Count != 0)
                            {
                                if (!(ycEligibleSSPlanPre1974.Tables[0].Rows[0]["bitValue"] == DBNull.Value))
                                {
                                    if (ycEligibleSSPlanPre1974.Tables[0].Rows[0]["bitValue"].ToString() == "0")
                                        llEligibleSSPlanPre1974 = false;
                                    else if (ycEligibleSSPlanPre1974.Tables[0].Rows[0]["bitValue"].ToString() == "1")
                                        llEligibleSSPlanPre1974 = true;
                                }
                                else
                                    llEligibleSSPlanPre1974 = false;
                            }
                            else
                                llEligibleSSPlanPre1974 = false;
                        }

                        //''''Try age critera again (min 55) with SS Plan participation result
                        //ASHISH:YRS-1345/BT-859 commented code, it is a repeated  code
                        //if (isEstimate)
                        //{
                        //    ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, false, "NORMAL", llEligibleServiceInMonths, llEligibleSSPlanPre1974, strFundEventId);
                        //}
                        //else
                        //{
                        //    //l_string_RetirementDate = ycRetireeRetirementDate;	//added by hafiz on 12/16/2008
                        //    //commented by hafiz on 12/16/2008
                        //    //ycEligibleAge = RetirementBOClass.EligibleAge(personID, DateTime.Now.ToString("MM/dd/yyyy"), false, "NORMAL", llEligibleServiceInMonths, llEligibleSSPlanPre1974, strFundEventId);
                        //    ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, false, "NORMAL", llEligibleServiceInMonths, llEligibleSSPlanPre1974, strFundEventId);
                        //}

                        ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, false, "NORMAL", llEligibleServiceInMonths, llEligibleSSPlanPre1974, strFundEventId);
                        if (ycEligibleAge.Trim().ToUpper() == "D" | ycEligibleAge.Trim().ToUpper() == "N") //NOT Eligible
                        {
                            ycEstimateAge = RetirementBOClass.EstimateAge(fundEventID, ycRetireeRetirementDate);
                            if (ycEstimateAge == 0)
                            {
                                //errorMessage += "<br> * " + "Person is too young";//commented on 24-sep for BT-1126
                                errorMessage += ",MESSAGE_RETIREMENT_BOC_PERSON_IS_YOUNG";
                                retirementIsValid = false;
                            }
                        }
                    }
                    else // QDRO ''''QDRO age >= 55 or QDRO judgee age >=50
                    {
                        //ASHISH:YRS-1345/BT-859 commented code, it is a repeated  code
                        //if (isEstimate)
                        //{
                        //    //added by hafiz on 12/16/2008
                        //    //ycEligibleAge = RetirementBOClass.EligibleAge(personID, DateTime.Now.ToString("MM/dd/yyyy"), true, "NORMAL", false, false, strFundEventId);
                        //    ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, true, "NORMAL", false, false, strFundEventId);
                        //}
                        //else
                        //{
                        //    ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, true, "NORMAL", false, false, strFundEventId);
                        //}
                        //
                        ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, true, "NORMAL", false, false, strFundEventId);

                        if (ycEligibleAge.Trim().ToUpper() == "D" | ycEligibleAge.Trim().ToUpper() == "N")
                        {
                            //ASHISH:YRS-1345/BT-859
                            //errorMessage += "<br> * " + "Person is too young";
                            //errorMessage += "<br> * " + "Original participant is too young";//commented on 24-sep for BT-1126
                            errorMessage += ",MESSAGE_RETIREMENT_BOC_ORIGINAL_PARTICIPANT_IS_YOUNG";
                            retirementIsValid = false;
                        }
                    }
                }
            } //if ((fundSatus != "RT") && (fundSatus != "RPT"))
            //ASHISH:2011-01-28 commented for BT- 665 Disability Retirment
            //else if (strRetireType == "DISABL") //''''Disabled Retirement''''Age<60
            if (strRetireType == "DISABL") //''''Disabled Retirement''''Age<60
            {
                //l_string_RetirementDate = ycRetireeRetirementDate;
                //ycEligibleAge = RetirementBOClass.EligibleAge(personID, DateTime.Now.ToString("MM/dd/yyyy"), false, "DISABL", false, false, strFundEventId);
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //ycEligibleAge = RetirementBOClass.EligibleAge(personID, l_string_RetirementDate, false, "DISABL", false, false, strFundEventId);
                ycEligibleAge = RetirementBOClass.EligibleAge(personID, ycRetireeRetirementDate, false, "DISABL", false, false, strFundEventId);
                if (ycEligibleAge.Trim().ToUpper() == "D" | ycEligibleAge.Trim().ToUpper() == "N")
                {
                    //errorMessage += "<br> * " + "Person is too old for disability";//commented on 24-sep for BT-1126
                    errorMessage += ",MESSAGE_RETIREMENT_BOC_PERSON_IS_TOO_OLD";
                    retirementIsValid = false;
                }


                //YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero : To allow check for estimation
                if (isEstimate)
                {
                    //Added by Sanket 2011-02-07  For Issues No BT-665 for Disability avoiding the paid service validation for Service plan
                    //ASHISH:2011.02.16-Added null condition in plantype check BT-665 for Disability

                    if ((planType == null) || (planType.ToUpper() == "R") || (planType.ToUpper() == "B"))
                    {

                        if (PersonalWithdrawalExists == false)  // SR | 03/08/2017 | YRS-AT-2625 | If withdrawal exists then no need to validate insufficiaent service validation. 
                        {                         
                            //''''Paid Service >= 60
                            // Check if the person has rendered 60 months of service on the date of retirement.
                            // Service months will be projected only if the status is Active (AE)
                            //START: SR | 2017.04.07 | YRS-AT-3390 | Replace old method with new method to calculate Paid service based on existing transacts
                            //ycEligibleServiceInMonths = RetirementBOClass.getEligibleServiceMinMonths(strFundEventId, true, RET_CONSTANT, ycRetireeRetirementDate);
                            //if (ycEligibleServiceInMonths == 0)
                            if (HasSatisfiedPaidService == false)
                            //END: SR | 2017.04.07 | YRS-AT-3390 | Replace old method with new method to calculate Paid service based on existing transacts
                            {
                                //errorMessage += "<br> * " + "Insufficient service";//commented on 24-sep for BT-1126
                                errorMessage += ",MESSAGE_RETIREMENT_BOC_INSUFFICENT_SERVICE";
                                retirementIsValid = false;
                            }
                        }
                    }
                }

                //''''Termination Date within 6 months of retirement date
                //2011.04.18    Sanket vaidya        BT-816 : Disability Retirement Estimate Issues.
                //if (!isEstimate)
                //{
                //    //2011.03.24    Sanket Vaidya        For YRS 5.0-1294,BT 794  : For disability requirement 
                //    if (retirementType == "NORMAL")
                //    {
                //        ycEligibleNotTerminatedWithinMonths = RetirementBOClass.EligibleNotTerminatedWithinMonths(strFundEventId, ycRetireeRetirementDate);

                //        if (ycEligibleNotTerminatedWithinMonths == 0)
                //        {
                //            errorMessage += "<br> * " + "Termination not close enough";
                //            retirementIsValid = false;
                //        }
                //    }
                //}
            }


            //NP:2008.04.25 - Check aging of RT accounts
            //	The rule says that RT money has to be on account for atleast 10 years to be annuitized.
            //	If there are no RT accounts older than 10 years old then sum of non-RT accounts (TD, TM) has to be greater than $5000 to be albe to annuitize the RT account with ROLL basis type.
            //	All aged RT accounts will be annuitized on PST96 basis type (7% factors)
            if (!isEstimate)
            {
                string err = string.Empty;
                if (planType == "B")
                {
                    if (!isSufficientBalance(personID, fundEventID, fundSatus, retirementDate, "R", ref err, null))
                    {
                        errorMessage += err; retirementIsValid = false;
                    }
                    if (!isSufficientBalance(personID, fundEventID, fundSatus, retirementDate, "S", ref err, null))
                    {
                        errorMessage += err; retirementIsValid = false;
                    }
                }
                else
                {
                    if (!isSufficientBalance(personID, fundEventID, fundSatus, retirementDate, planType, ref err, null))
                    {
                        //errorMessage += "<br> * " + err; retirementIsValid = false; //SP 2014-02-20	BT-2436 : commented
                        errorMessage += err; retirementIsValid = false; //SP 2014.02.20 BT-2436 :Added 
                    }
                }

            }
            return retirementIsValid;
        }

        //YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
        public static bool IsPaidServiceValid(string planType, string strFundEventID, bool isEstimate, string ycRetireeRetirementDate)
        {
            if ((planType == null) || (planType.ToUpper() == "R") || (planType.ToUpper() == "B"))
            {
                string RET_CONSTANT = string.Empty;
                int ycEligibleServiceInMonths;
                if (isEstimate)
                    RET_CONSTANT = "ESTIMATE";
                else
                    RET_CONSTANT = "RETIRE";
                //''''Paid Service >= 60
                // Check if the person has rendered 60 months of service on the date of retirement.
                // Service months will be projected only if the status is Active (AE)
                ycEligibleServiceInMonths = RetirementBOClass.getEligibleServiceMinMonths(strFundEventID, true, RET_CONSTANT, ycRetireeRetirementDate);
                if (ycEligibleServiceInMonths == 0)
                {
                    //Insufficiet Service
                    return false;
                }                 
            }
            return true;
        }

        //YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
        // START : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services 
        //public static bool HasBasicMoneyWithdrawn(string strFundeventID)
        //public static bool HasBasicMoneyWithdrawn(string strFundeventID,DateTime RetirementDate)
        public static DataSet HasBasicMoneyWithdrawn(string strFundeventID,DateTime RetirementDate)
        {
            //bool withdrawnTransType=false;
            DataSet withdrawalDetails;
            //withdrawnTransType = RetirementBOClass.getWithdrawntransactionCheckForDisability(strFundeventID);   //  SB | 03/08/2017 | YRS-AT-2625 | Old code is commented replaced with additional parameter
            //START |  SR | 2017.03.20 | commented & rewritten below line of code to return table instead of output parameter
            //withdrawnTransType = RetirementBOClass.getWithdrawntransactionCheckForDisability(strFundeventID, RetirementDate);
            withdrawalDetails = RetirementBOClass.getWithdrawntransactionCheckForDisability(strFundeventID, RetirementDate);           
            //return withdrawnTransType;
            return withdrawalDetails;
            //END |  SR | 2017.03.20 | commented & rewritten below line of code to return table instead of output parameter
            //if (withdrawnTransType == 0)
            //    return false;
            //else
            //    return true;
        }

        //NP:IVP1:2008.04.28 - Creating function to identify if there are enough balances to perform a retirement
        //	for the specified plan type. This will check balances only on one plan at a time.
        //	RETURN:		1 - There are enough balances after applying all rules
        //				0 - Not enough balances
        //	Parameter - err: Contains the specific message on why the balances are not enough.
        /// <summary>
        /// Check if there are sufficient balances for a participant to retire for a specific plan type. This method only works on one plan at a time.
        /// </summary>
        /// <param name="fundEventID">The fund event of the participant for whom to compute balance requirements.</param>
        /// <param name="planType">An enumeration of PlanType values. Determines which plan calculations are made for. Valid values are "R" - Retirement plan, "S" - Savings Plan and null - Pre-PlanSplit conditions where balances are determined by balances on all accounts.</param>
        /// <param name="retirementDate">The date when participant is going to retire. This could be either in the past or future.</param>
        /// <param name="err">Output error messages related to insufficient balances. In case balances are sufficient then this may contain informational messages also.</param>
        /// <returns>True, if balances are sufficient after applying all the rules to determine balances else false with an optional error message in the err variable.</returns>
        private static bool isSufficientBalance(string persID, string fundEventID, string fundStatus, string retirementDate, string planType, ref string err, DataTable dtAccountsByBasis)
        {
            DataSet accountBalance;
            DataTable balances = null;
            string filterExpression = string.Empty;

            if (dtAccountsByBasis == null) //it means if not called from estimate process but from retirement process
            {
                try
                {
                    string personID = string.Empty;
                    //accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceData(true, "01/01/1900", retirementDate, personID, fundEventID, true, true, planType);
                    accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceData(true, "01/01/1900", retirementDate, personID, fundEventID, false, true, planType);
                }
                catch
                {
                    throw;
                }

                if (accountBalance.Tables.Count == 3)
                {
                    balances = accountBalance.Tables[0];
                }
                else
                {
                    balances = accountBalance.Tables[0];
                }
            }
            else
            {
                string localPlanType = string.Empty;

                if (planType == "S" || planType == "R")
                {
                    localPlanType = (planType == "S" ? "SAVINGS" : "RETIREMENT");
                    filterExpression = "[chvPlanType] = '" + localPlanType + "'";
                }

                balances = dtAccountsByBasis;
            }

            // We now have the dataTable to compute balances from. 
            decimal totalBalance = 0.0M;
            string val = string.Empty;
            val = balances.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYMCAPreTax)", filterExpression).ToString();
            totalBalance = (val != string.Empty) ? decimal.Parse(val) : 0;

            //Perform 1st level validation to identify if there are enough funds to retire - balance > 0
            if (totalBalance <= 0)
            {
                //err = "Not enough funds to retire";//commented on 24-sep for BT-1126
                err = ",MESSAGE_RETIREMENT_BOC_NOT_ENOUGH_TO_RETIRE";
                //if (planType == "R") err += " with RETIREMENT Plan";//commented on 24-sep for BT-1126
                if (planType == "R") err += ",MESSAGE_RETIREMENT_BOC_WITH_RETIREMENT";
                //if (planType == "S") err += " with TD Savings Plan";//commented on 24-sep for BT-1126
                if (planType == "S") err += ",MESSAGE_RETIREMENT_BOC_WITH_SAVINGS";
                return false;
            }

            // PUT ALL SELF FULFILLING CONDITIONS THAT RETURN TRUE HERE
            //If current fund status is RD then return true for the current plan
            if (fundStatus == "RD" || fundStatus == "RP")
            {
                err = string.Empty; return true;
            }

            //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            //if (fundStatus == "RT" || fundStatus == "RA" || fundStatus == "RE") 
            //Added by Ashish for phase V changes ,Added fund Status RDNP 
            if (fundStatus == "RT" || fundStatus == "RPT" || fundStatus == "RA" || fundStatus == "RE" || fundStatus == "RDNP")
            {	// Load the past annuity purchase history for the participant 
                // Balance checks are not applicable on plans on which person has retired.
                RetirementBOClass r = new RetirementBOClass();
                r.GetPurchasedAnnuityDetails(persID);
                if (r.existingAnnuityPurchaseDate == "")
                {
                    //err = "Annuity details are missing for the participant, cannot proceed.";//commented on 24-sep for BT-1126
                    err = ",MESSAGE_RETIREMENT_BOC_ANNUITY_DETAILS_MISSING";
                    return false;
                }
                // If last retirement was before plan split date then assume person already retired on both plans
                if (DateTime.Parse(r.existingAnnuityPurchaseDate) < GetPlanSplitDate()) { err = string.Empty; return true; }
                // If person has retired on Retirement Plan and we are computing for Retirement plan then return true
                if (planType == "R" && r.RetiredOnRetirementPlan) { err = string.Empty; return true; }
                // If person has retired on Savings Plan and we are computing for Retirement plan then return true
                if (planType == "S" && r.RetiredOnSavingsPlan) { err = string.Empty; return true; }
                //added by hafiz on 6-Jan-2009 for YRS 5.0-636
                // If person has retired on Retirement as well as on Savings Plan and we are computing for both the plans then return true
                if (planType == "B" && (r.RetiredOnRetirementPlan && r.RetiredOnSavingsPlan)) { err = string.Empty; return true; }
            }

            //We are dealing with cases where the person has never retired before
            //Apply normal balance checks - balance >= 5000 and non-inclusion of unaged RT acccounts
            //Perform preliminary check for minimum balance

            //2011.03.01    Sanket Vaidya     BT-731  The minimum required to purchase an annutiy is $5,000.01.  The retirement process allows an annutiy purchase with $5,000.00. 
            //if (totalBalance < 5000)
            if (totalBalance <= 5000)
            {
                //err = "Not enough funds to retire";//commented on 24-sep for BT-1126
                err = ",MESSAGE_RETIREMENT_BOC_NOT_ENOUGH_TO_RETIRE";
                //if (planType == "R") err += " with RETIREMENT Plan";//commented on 24-sep for BT-1126
                if (planType == "R") err += ",MESSAGE_RETIREMENT_BOC_WITH_RETIREMENT";
                //if (planType == "S") err += " with TD Savings Plan";//commented on 24-sep for BT-1126
                if (planType == "S") err += ",MESSAGE_RETIREMENT_BOC_WITH_SAVINGS";
                return false;
            }
            //Commented By Ashish 21-Aug-2009,For Issue YRS 5.0-868
            //			//Perform check to see if RT account balance requirements are satisfied
            //			//The rule says that if there is only RT money (and no other accounts) it cannot be annuitized unless it has been with the fund for atleast 10 years
            //			//If there are other accounts whose total meets the minimum balance threshold then aged RT accounts will be annuitized using PST96 annuity factors and other RT accounts will be annuitized using ROLL factors.
            //			// Logic for checking the above rule is as follows:
            //			//	1. Check if there is atleast 1 aged RT account. This can be identified by seeing if atleast 1 RT record with transact type PST96 exists with non-zero balances.
            //			//	2. If there are no aged accounts then we need to subtract the RT account balances to see if the remaining accounts can purchase the annuity by themselves. If not then we cannot proceed.
            //			if (planType == null || planType == "S" || planType == "B")
            //			{
            //				//if (balances.Select("[chvPlanType] = 'SAVINGS' AND [chrAcctType] = 'RT' AND [chrAnnuityBasisType] = 'PST96' AND ([mnyPersonalPreTax] <> 0 OR [mnyPersonalPostTax] <> 0 OR [mnyYMCAPreTax] <> 0)").Length == 0) 
            //				if (balances.Select("[chvPlanType] = 'SAVINGS'").Length > 0)
            //				{
            //					if (balances.Select("[chrAcctType] = 'RT'").Length > 0) 
            //					{
            //						//commented by Ashish 28-july-2009
            ////						if (balances.Select("[chrAcctType] = 'RT' AND [chrAnnuityBasisType] = 'PST96' AND ([mnyPersonalPreTax] <> 0 OR [mnyPersonalPostTax] <> 0 OR [mnyYMCAPreTax] <> 0)").Length > 0) 
            ////						{
            //							//Added chrAnnuityBasisType filter for computeing RT account balances 
            //							decimal RTBalance = 0.0M;
            //							val = string.Empty;
            //							val = balances.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYMCAPreTax)", "chrAcctType = 'RT' AND [chrAnnuityBasisGroup] <> 'PST'").ToString();
            //					
            //							RTBalance = (val != string.Empty) ? decimal.Parse(val) : 0;
            //
            //							val = string.Empty;
            //							val = balances.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYMCAPreTax)", "chvPlanType = 'SAVINGS'").ToString();
            //							totalBalance = (val != string.Empty) ? decimal.Parse(val) : 0;
            //
            //							if (totalBalance - RTBalance < 5000) 
            //							{
            //								err = "Insufficient balance in TD Savings Plan. RT accounts were not considered since none of them were held for more than 10 years.";
            //								return false;
            //							}
            ////						}
            //					}
            //				}
            //			}
            //Finally return true since none of our Conditional checks were fired
            return true;
        }







        //Added by Ashish ,This method is not used. 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fundeventID"></param>
        /// <returns></returns>
        public static DataSet GetBasicAccountsByPlan(string fundeventID)
        {
            DataSet accountBalance;
            try
            {
                accountBalance = RetirementDAClass.GetBasicAccountsByPlan(fundeventID);
            }
            catch
            {
                throw;
            }

            return accountBalance;
        }


        /// <summary>
        /// Get the details of annuities that the participant has already purchased.
        /// </summary>
        /// <param name="personID"></param>
        /// <returns></returns>
        public DataTable GetPurchasedAnnuityDetails(string personID)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            RetiredOnRetirementPlan = false;
            RetiredOnSavingsPlan = false;

            try
            {
                ds = RetirementDAClass.GetPurchasedAnnuityDetails(personID);

                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr["PlanType"].ToString() == "RETIREMENT")
                            RetiredOnRetirementPlan = true;
                        if (dr["PlanType"].ToString() == "SAVINGS")
                            RetiredOnSavingsPlan = true;
                        //Ashish BT-917
                        //this.RetirementDate = dr["RetirementDate"].ToString();
                        this.existingAnnuityPurchaseDate = dr["RetirementDate"].ToString();
                    }
                }
            }
            catch
            {
                throw;
            }

            if (ds.Tables.Count > 0)
                dt = ds.Tables[0];

            return dt;
        }


        /// <summary>
        /// Decide what will be the final fund status after retirement.
        /// If ther participant retires on both plans then fundStatus  = "RD"
        /// If he retires on one plan and
        ///		If there is no fund left in other plan then  fundStatus  = "RD"
        ///		If some fund is left in the other plan then  fundStatus  = "RT"
        /// </summary>
        /// <param name="dtSelectedAnnuity"></param>
        /// <returns></returns>
        public string DecideFinalRetirementFundStatus(DataTable dtSelectedAnnuity, string personID, string fundStatus, decimal retirementBalance, decimal savingsBalance)
        {
            string finalFindStatus = string.Empty;

            bool retirementPlanUsed = false;
            bool savingsPlanUsed = false;
            if ((dtSelectedAnnuity.Select("planType = 'R'")).Length > 0)
                retirementPlanUsed = true;
            if ((dtSelectedAnnuity.Select("planType = 'S'")).Length > 0)
                savingsPlanUsed = true;

            decimal retBalance = retirementBalance;
            decimal savBalance = savingsBalance;

            if (retirementPlanUsed && savingsPlanUsed)
            {
                //finalFindStatus = (fundStatus == "PT" ? "RP" : "RD");
                finalFindStatus = ((fundStatus == "PT" || fundStatus == "RPT") ? "RP" : "RD");
            }
            else if (retirementPlanUsed && !savingsPlanUsed)
            {
                if (savBalance <= 0)
                    //finalFindStatus = (fundStatus == "PT" ? "RP" : "RD");
                    finalFindStatus = ((fundStatus == "PT" || fundStatus == "RPT") ? "RP" : "RD");
                else
                    //finalFindStatus = "RT";
                    finalFindStatus = ((fundStatus == "PT" || fundStatus == "RPT") ? "RPT" : "RT");//as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            }
            else if (!retirementPlanUsed && savingsPlanUsed)
            {
                if (retBalance <= 0)
                    //finalFindStatus = (fundStatus == "PT" ? "RP" : "RD");
                    finalFindStatus = ((fundStatus == "PT" || fundStatus == "RPT") ? "RP" : "RD");
                else
                    //finalFindStatus = "RT";					
                    finalFindStatus = ((fundStatus == "PT" || fundStatus == "RPT") ? "RPT" : "RT");//as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            }

            return finalFindStatus;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DateTime GetPlanSplitDate()
        {
            DataSet ds = new DataSet();
            string dateString = string.Empty;

            try
            {
                ds = RetirementDAClass.GetPlanSplitDate();

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        dateString = ds.Tables[0].Rows[0]["PlanSplitDate"].ToString();
                }
            }
            catch
            {
                throw;
            }

            if (dateString != string.Empty)
                return Convert.ToDateTime(dateString);
            else
                return DateTime.MinValue;


        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static DateTime getDailyInterestLog()
        {
            return YMCARET.YmcaDataAccessObject.RetirementDAClass.getDailyInterestLog();
        }

        /// <summary>
        /// Projection table will contain 3 more columns TranDate, ProjPeriod, ProjPerSequence with 
        /// default values '01/01/1900', "", 0
        /// </summary>
        /// <param name="dtAccountsBasisByProjection"></param>
        /// <param name="dtAccountsByBasis"></param>
        private static void createProjectionTable(DataTable dtAccountsBasisByProjection, DataTable dtAccountsByBasis)
        {
            try
            {


                //Commented by Ashish for phase V part changes

                if (dtAccountsByBasis.Rows.Count != 0)
                {
                    for (int i = 0; i <= dtAccountsByBasis.Rows.Count - 1; i++)
                    {
                        dtAccountsBasisByProjection.ImportRow(dtAccountsByBasis.Rows[i]);
                        dtAccountsBasisByProjection.GetChanges(DataRowState.Added);
                    }
                    //dtAccountsBasisByProjection.AcceptChanges();
                    //dtAccountsBasisByProjection.Columns.Add("dtsTransactDate", typeof(DateTime));
                    //dtAccountsBasisByProjection.Columns.Add("chrProjPeriod");
                    //dtAccountsBasisByProjection.Columns.Add("intProjPeriodSequence", typeof(int));
                    //dtAccountsBasisByProjection.AcceptChanges();
                }

                if (!dtAccountsBasisByProjection.Columns.Contains("dtsTransactDate"))
                    dtAccountsBasisByProjection.Columns.Add("dtsTransactDate", typeof(DateTime));

                if (!dtAccountsBasisByProjection.Columns.Contains("chrProjPeriod"))
                    dtAccountsBasisByProjection.Columns.Add("chrProjPeriod");

                if (!dtAccountsBasisByProjection.Columns.Contains("intProjPeriodSequence"))
                    dtAccountsBasisByProjection.Columns.Add("intProjPeriodSequence", typeof(int));

                dtAccountsBasisByProjection.AcceptChanges();
                DataRow drAccountsBasisByProjection = null;
                for (int i = 0; i < dtAccountsBasisByProjection.Rows.Count; i++)
                {
                    drAccountsBasisByProjection = dtAccountsBasisByProjection.Rows[i];
                    if (drAccountsBasisByProjection["dtsTransactDate"] == DBNull.Value)
                    {
                        //Commented by Ashish for phase V part III changes
                        //									drAccountsBasisByProjection = dtAccountsBasisByProjection.NewRow();
                        //									drAccountsBasisByProjection["dtsTransactDate"] = Convert.ToDateTime("01/01/1900");
                        //									drAccountsBasisByProjection["chrProjPeriod"] = "";
                        //									drAccountsBasisByProjection["intProjPeriodSequence"] = 0;
                        //									drAccountsBasisByProjection["chrAcctType"] = dr["chrAcctType"].ToString();
                        //									drAccountsBasisByProjection["chrAnnuityBasisType"] = dr["chrAnnuityBasisType"].ToString();
                        //									drAccountsBasisByProjection["mnyYMCAContribBalance"] = dr["mnyYMCAContribBalance"];
                        //									drAccountsBasisByProjection["mnyPersonalContribBalance"] = dr["mnyPersonalContribBalance"];
                        //									drAccountsBasisByProjection["mnyPersonalInterestBalance"] = dr["mnyPersonalInterestBalance"];
                        //									drAccountsBasisByProjection["mnyYMCAInterestBalance"] = dr["mnyYMCAInterestBalance"];
                        //									drAccountsBasisByProjection["mnyPersonalPreTax"] = dr["mnyPersonalPreTax"];
                        //									drAccountsBasisByProjection["mnyYMCAPreTax"] = dr["mnyYMCAPreTax"];
                        //									drAccountsBasisByProjection["mnyPersonalPostTax"] = dr["mnyPersonalPostTax"];
                        //									drAccountsBasisByProjection["YMCAAmt"] = dr["YMCAAmt"];
                        //									drAccountsBasisByProjection["PersonalAmt"] = dr["PersonalAmt"];
                        //									drAccountsBasisByProjection["mnyBalance"] = dr["mnyBalance"];
                        //									drAccountsBasisByProjection["chvPlanType"] = dr["chvPlanType"];
                        //									drAccountsBasisByProjection["chrAnnuityBasisGroup"] = dr["chrAnnuityBasisGroup"];
                        //									dtAccountsBasisByProjection.Rows.Add(drAccountsBasisByProjection);
                        drAccountsBasisByProjection["dtsTransactDate"] = Convert.ToDateTime("01/01/1900");
                        drAccountsBasisByProjection["chrProjPeriod"] = "";
                        drAccountsBasisByProjection["intProjPeriodSequence"] = 0;

                    }
                }
                //dtAccountsBasisByProjection.GetChanges(DataRowState.Modified);
                dtAccountsBasisByProjection.AcceptChanges();

                //							for (int i = 0; i <= dtAccountsBasisByProjection.Rows.Count - 1; i++) 
                //							{
                //								if (dtAccountsBasisByProjection.Rows[i]["dtsTransactDate"] == DBNull.Value) 
                //								{
                //									dtAccountsBasisByProjection.Rows[i].Delete();
                //								}
                //							}
                //							dtAccountsBasisByProjection.GetChanges(DataRowState.Deleted);
                //							dtAccountsBasisByProjection.AcceptChanges();
                //				
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //	Added by Ashish for phase V part III changes, Start

        /// <summary>
        /// This method gets the effective basis type
        /// </summary>
        /// <param name="para_dtAnnuityBasisTypeList"></param>
        /// <param name="para_BasisGroupType"></param>
        /// <param name="para_EffectiveDate"></param>
        /// <returns></returns>
        private static DataRow GetEffectiveAnnuityBasisType(DataTable para_dtAnnuityBasisTypeList, string para_BasisGroupType, DateTime para_EffectiveDate)
        {
            DataRow drAnnuityBasisRow = null;
            DataRow[] drAnutiyFoundRows = null;
            try
            {

                if (para_dtAnnuityBasisTypeList != null)
                {
                    string filter = string.Empty;
                    //drAnnuityBasisRow=para_dtAnnuityBasisTypeList.NewRow();
                    //					if(para_BasisGroupType=="PRE")
                    //					{
                    //						filter="chrAnnuityBasisGroup='"+para_BasisGroupType+"' AND dtmEffDate <= '" +para_EffectiveDate.ToString("MM/dd/yyyy") +"'";
                    //					}
                    //					else
                    //					{
                    //						filter="chrAnnuityBasisGroup='"+para_BasisGroupType+"' AND dtmEffDate <= '" +para_EffectiveDate.ToString("MM/dd/yyyy") +"' AND  ISNULL(dtmTermDate,'"+para_EffectiveDate.AddDays(1).ToString("MM/dd/yyyy")+"') >= '" +para_EffectiveDate.ToString("MM/dd/yyyy")+"'";
                    //					}
                    filter = "chrAnnuityBasisGroup='" + para_BasisGroupType + "' AND dtmEffDate <= '" + para_EffectiveDate.ToString("MM/dd/yyyy") + "'";
                    drAnutiyFoundRows = para_dtAnnuityBasisTypeList.Select(filter, "dtmEffDate DESC");
                    if (drAnutiyFoundRows.Length > 0)
                    {
                        drAnnuityBasisRow = para_dtAnnuityBasisTypeList.NewRow();
                        drAnnuityBasisRow = drAnutiyFoundRows[0];
                    }
                }
                return drAnnuityBasisRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method find out changed basis type list, whose annuity percentage is less than effective basis type annuity percentage. 
        /// </summary>
        /// <param name="para_AnnuityBasisTypeList"></param>
        /// <param name="para_RetirementDate"></param>
        /// <returns></returns>
        private DataTable GetChangedAnnuityBasisList(DataTable para_AnnuityBasisTypeList, string para_RetirementDate, string para_RetireType)
        {
            DataTable dtChangedAnnuityBasisTypeList = null;
            DataRow drChangedAnnuityBasis = null;
            string effectiveBasisType = string.Empty;
            decimal effectiveAnnuityBasisPct = 0;
            string annuityBasisEfftDate = string.Empty;
            DataRow drEfectiveBasisType = null;
            string selectQuery = string.Empty;
            try
            {
                if (para_AnnuityBasisTypeList != null && para_RetirementDate != string.Empty)
                {
                    dtChangedAnnuityBasisTypeList = new DataTable();
                    dtChangedAnnuityBasisTypeList.Columns.Add(new DataColumn("chrAnnuityBasisType", typeof(string)));
                    dtChangedAnnuityBasisTypeList.Columns.Add(new DataColumn("chrEffectiveAnnuityBasisType", typeof(string)));

                    if (para_AnnuityBasisTypeList.Rows.Count > 0)
                    {
                        DataRow[] drAnnuityBasisList = null;
                        //Get PST serise annuityBasisType list which is less than effective basistype percentage 
                        //Get regular effective AnnuitybasisType of PST series
                        drEfectiveBasisType = GetEffectiveAnnuityBasisType(para_AnnuityBasisTypeList, "PST", Convert.ToDateTime(para_RetirementDate).Date);
                        if (drEfectiveBasisType != null)
                        {
                            effectiveBasisType = drEfectiveBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                            effectiveAnnuityBasisPct = drEfectiveBasisType["numAnnuityBasisPct"] == System.DBNull.Value ? 0 : Convert.ToDecimal(drEfectiveBasisType["numAnnuityBasisPct"]);
                            annuityBasisEfftDate = drEfectiveBasisType["dtmEffDate"] == System.DBNull.Value ? string.Empty : Convert.ToDateTime(drEfectiveBasisType["dtmEffDate"]).ToString("MM/dd/yyyy");
                            if (para_RetireType.ToUpper() == "NORMAL")
                                selectQuery = "numAnnuityBasisPct < " + effectiveAnnuityBasisPct + " AND chrAnnuityBasisGroup='PST' AND dtmEffDate < '" + annuityBasisEfftDate + "' AND NOT (  chrAnnuityBasisType='" + effectiveBasisType + "')";
                            else
                                //For Disability
                                selectQuery = "(chrAnnuityBasisGroup='PST' OR chrAnnuityBasisGroup='PRE' ) AND dtmEffDate < '" + annuityBasisEfftDate + "' AND NOT (  chrAnnuityBasisType='" + effectiveBasisType + "')";

                            drAnnuityBasisList = para_AnnuityBasisTypeList.Select(selectQuery);
                            if (drAnnuityBasisList.Length > 0)
                            {
                                for (int i = 0; i < drAnnuityBasisList.Length; i++)
                                {
                                    drChangedAnnuityBasis = dtChangedAnnuityBasisTypeList.NewRow();
                                    drChangedAnnuityBasis["chrAnnuityBasisType"] = drAnnuityBasisList[i]["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                                    drChangedAnnuityBasis["chrEffectiveAnnuityBasisType"] = effectiveBasisType;
                                    dtChangedAnnuityBasisTypeList.Rows.Add(drChangedAnnuityBasis);

                                }
                                dtChangedAnnuityBasisTypeList.AcceptChanges();
                            }
                        }
                        //Get ROL serise Annuity BasisType list which is less than effective basistype percentage 
                        //Get Rollover effective AnnuitybasisType of ROL series
                        drEfectiveBasisType = GetEffectiveAnnuityBasisType(para_AnnuityBasisTypeList, "ROL", Convert.ToDateTime(para_RetirementDate).Date);
                        if (drEfectiveBasisType != null)
                        {
                            effectiveBasisType = drEfectiveBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                            effectiveAnnuityBasisPct = drEfectiveBasisType["numAnnuityBasisPct"] == System.DBNull.Value ? 0 : Convert.ToDecimal(drEfectiveBasisType["numAnnuityBasisPct"]);
                            annuityBasisEfftDate = drEfectiveBasisType["dtmEffDate"] == System.DBNull.Value ? string.Empty : Convert.ToDateTime(drEfectiveBasisType["dtmEffDate"]).ToString("MM/dd/yyyy");
                            if (para_RetireType.ToUpper() == "NORMAL")
                                selectQuery = "numAnnuityBasisPct < " + effectiveAnnuityBasisPct + " AND chrAnnuityBasisGroup='ROL' AND dtmEffDate < '" + annuityBasisEfftDate + "' AND NOT ( chrAnnuityBasisType='" + effectiveBasisType + "')";
                            else
                                //For Disability
                                selectQuery = " chrAnnuityBasisGroup='ROL' AND dtmEffDate < '" + annuityBasisEfftDate + "' AND NOT ( chrAnnuityBasisType='" + effectiveBasisType + "')";

                            drAnnuityBasisList = para_AnnuityBasisTypeList.Select("numAnnuityBasisPct < " + effectiveAnnuityBasisPct + " AND chrAnnuityBasisGroup='ROL' AND dtmEffDate < '" + annuityBasisEfftDate + "' AND NOT ( chrAnnuityBasisType='" + effectiveBasisType + "')");
                            if (drAnnuityBasisList.Length > 0)
                            {
                                for (int i = 0; i < drAnnuityBasisList.Length; i++)
                                {
                                    drChangedAnnuityBasis = dtChangedAnnuityBasisTypeList.NewRow();
                                    drChangedAnnuityBasis["chrAnnuityBasisType"] = drAnnuityBasisList[i]["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                                    drChangedAnnuityBasis["chrEffectiveAnnuityBasisType"] = effectiveBasisType;
                                    dtChangedAnnuityBasisTypeList.Rows.Add(drChangedAnnuityBasis);

                                }
                                dtChangedAnnuityBasisTypeList.AcceptChanges();
                            }
                        }

                    }
                }
                return dtChangedAnnuityBasisTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update basis type with effective basis type ,if effective basis type annuity percentage is grater.  
        /// </summary>
        /// <param name="para_dtChangedBasisTypeList"></param>
        /// <param name="para_dtAcctBalancesByBasis"></param>
        private void UpdateAcctBalBasisTypeWithEffectiveBasisType(DataTable para_dtChangedBasisTypeList, ref DataTable para_dtAcctBalancesByBasis)
        {
            DataTable l_dtTmpAcctBalancesByBasis = null;
            DataRow[] drActBalancesByBasisType = null;
            DataTable dtGroupedBalancesByBasisType;
            try
            {
                if (para_dtAcctBalancesByBasis != null && para_dtChangedBasisTypeList != null)
                {
                    if (para_dtAcctBalancesByBasis.Rows.Count > 0 && para_dtChangedBasisTypeList.Rows.Count > 0)
                    {
                        l_dtTmpAcctBalancesByBasis = para_dtAcctBalancesByBasis.Copy();

                        foreach (DataRow drChangedBasisType in para_dtChangedBasisTypeList.Rows)
                        {
                            drActBalancesByBasisType = l_dtTmpAcctBalancesByBasis.Select("chrAnnuityBasisType='" + drChangedBasisType["chrAnnuityBasisType"].ToString().Trim() + "'");
                            if (drActBalancesByBasisType.Length > 0)
                            {
                                drActBalancesByBasisType[0]["chrAnnuityBasisType"] = drChangedBasisType["chrEffectiveAnnuityBasisType"].ToString();
                            }
                        }
                        l_dtTmpAcctBalancesByBasis.AcceptChanges();
                        //Grouped Balances with BasisType
                        string[] targetColumns = null;
                        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                        //string []compareColumns={"chrAnnuityBasisType"};                 
                        //string []clubbedColumns={"mnyPersonalRetirementBalance","mnyYmcaRetirementBalance","mnyBasicAcctRetirementBalance"};
                        //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                        string[] compareColumns = { "chrAnnuityBasisType", "chvPlanType" };
                        string[] clubbedColumns = { "mnyPersonalRetirementBalance", "mnyYmcaRetirementBalance", "mnyBasicAcctRetirementBalance", "mnyBasicPersonalBalances", "mnyBasicYmcaBalance", "mnyNonBasicPersonalBalance", "mnyNonBasicYmcalBalance" };
                        // Get distinct basis type clubbed amount
                        dtGroupedBalancesByBasisType = SelectDistinct(l_dtTmpAcctBalancesByBasis, targetColumns, compareColumns, clubbedColumns);
                        if (dtGroupedBalancesByBasisType != null)
                        {
                            if (dtGroupedBalancesByBasisType.Rows.Count > 0)
                            {
                                para_dtAcctBalancesByBasis.Clear();
                                foreach (DataRow drGroupedAcctBalancesByBasis in dtGroupedBalancesByBasisType.Rows)
                                {
                                    para_dtAcctBalancesByBasis.ImportRow(drGroupedAcctBalancesByBasis);
                                }
                                para_dtAcctBalancesByBasis.AcceptChanges();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// This method find low annuity factor basis type and update with effective higher anuuity factor basis type 
        /// </summary>
        /// <param name="para_RetirementType"></param>
        /// <param name="para_RetirementDate"></param>
        public void UpdateBasisTypeAsPerAnnuitizeFactor(string para_RetirementType, string para_RetirementDate)
        {

            DataTable dtChangedBasisTypeList;
            try
            {
                if (this.g_dtAnnuityBasisTypeList != null && this.g_dtAcctBalancesByBasisType != null && para_RetirementDate != string.Empty)
                {
                    if (this.g_dtAnnuityBasisTypeList.Rows.Count > 0 && this.g_dtAcctBalancesByBasisType.Rows.Count > 0)
                    {

                        // Get list of annuity basis type which will be annuitize with effective basis type factor
                        dtChangedBasisTypeList = GetChangedAnnuityBasisList(this.g_dtAnnuityBasisTypeList, para_RetirementDate, para_RetirementType);
                        //update basis type of AcctBalancesByBasisType with effective basistype, which having greater annuitize factor
                        //grouped balances by basistype
                        UpdateAcctBalBasisTypeWithEffectiveBasisType(dtChangedBasisTypeList, ref this.g_dtAcctBalancesByBasisType);


                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Find Distinct Reords And Clubbed Amount
        /// <summary>
        /// This method finds field values are equal or not
        /// </summary>
        /// <param name="lastValues"></param>
        /// <param name="currentRow"></param>
        /// <param name="compareFieldNames"></param>
        /// <returns></returns>
        private static bool FieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] compareFieldNames)
        {
            bool areEqual = true;
            try
            {
                for (int i = 0; i < compareFieldNames.Length; i++)
                {
                    if (lastValues[i] == null || !lastValues[i].Equals(currentRow[compareFieldNames[i]]))
                    {
                        areEqual = false;
                        break;
                    }
                }
                return areEqual;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        /// <summary>
        /// This method will create clone row from source table
        /// </summary>
        /// <param name="sourceRow"></param>
        /// <param name="newRow"></param>
        /// <param name="targetfieldNames"></param>
        /// <returns></returns>
        private static DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] targetfieldNames)
        {
            try
            {
                if (targetfieldNames == null || targetfieldNames.Length == 0)
                {
                    object[] source = sourceRow.ItemArray;
                    for (int i = 0; i < source.Length; i++)
                    {
                        newRow[i] = source[i];
                    }
                }
                else
                {
                    foreach (string field in targetfieldNames)
                        newRow[field] = sourceRow[field];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newRow;
        }
        /// <summary>
        /// This method lase value for next comparison
        /// </summary>
        /// <param name="lastValues"></param>
        /// <param name="sourceRow"></param>
        /// <param name="compareFieldNames"></param>
        private static void SetLastValues(object[] lastValues, DataRow sourceRow, string[] compareFieldNames)
        {
            try
            {
                for (int i = 0; i < compareFieldNames.Length; i++)
                    lastValues[i] = sourceRow[compareFieldNames[i]];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method find distinct row from source table and clubbed column amount
        /// </summary>
        /// <param name="parameterSourceDataTable"></param>
        /// <param name="parameterTargetColumnNames"></param>
        /// <param name="parameterCompareColumnNames"></param>
        /// <param name="parameterClubbedColumnList"></param>
        /// <returns></returns>
        private static DataTable SelectDistinct(DataTable parameterSourceDataTable, string[] parameterTargetColumnNames, string[] parameterCompareColumnNames, string[] parameterClubbedColumnList)
        {
            DataTable dtDistinct = null;
            object[] lastValues;
            DataRow[] selectedRows;
            bool clubbedFlag = true;
            try
            {
                if (parameterCompareColumnNames == null || parameterCompareColumnNames.Length == 0)
                    throw new ArgumentNullException("ColumnNames");

                if (parameterClubbedColumnList == null || parameterClubbedColumnList.Length == 0)
                    clubbedFlag = false;

                lastValues = new object[parameterCompareColumnNames.Length];
                dtDistinct = new DataTable();

                if (parameterTargetColumnNames == null || parameterTargetColumnNames.Length == 0)
                {
                    dtDistinct = parameterSourceDataTable.Clone();
                }
                else
                {
                    foreach (string columnName in parameterTargetColumnNames)
                    {
                        dtDistinct.Columns.Add(columnName, parameterSourceDataTable.Columns[columnName].DataType);
                    }
                }




                selectedRows = parameterSourceDataTable.Select("", string.Join(", ", parameterCompareColumnNames));

                int clubbedCounter = -1;
                foreach (DataRow dtRow in selectedRows)
                {
                    if (!FieldValuesAreEqual(lastValues, dtRow, parameterCompareColumnNames))
                    {
                        dtDistinct.Rows.Add(CreateRowClone(dtRow, dtDistinct.NewRow(), parameterTargetColumnNames));
                        SetLastValues(lastValues, dtRow, parameterCompareColumnNames);
                        clubbedCounter++;
                    }
                    else if (clubbedFlag)
                    {
                        if (parameterClubbedColumnList.Length > 0)
                        {
                            for (int i = 0; i < parameterClubbedColumnList.Length; i++)
                            {
                                if (dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]] != System.DBNull.Value && dtRow[parameterClubbedColumnList[i]] != System.DBNull.Value)
                                {
                                    dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]] = Convert.ToDecimal(dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]]) + Convert.ToDecimal(dtRow[parameterClubbedColumnList[i]]);
                                    dtDistinct.Rows[clubbedCounter].AcceptChanges();
                                }

                            }
                        }
                    }

                }
                //dtDistinct.AcceptChanges(); 


                return dtDistinct;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// This method create schema for calculating account balances by annuity basis type
        /// </summary>
        /// <param name="para_g_dtAcctBalancesByBasisType"></param>
        private void CreateAcctBalancesByBasisTypeSchema(ref DataTable para_g_dtAcctBalancesByBasisType)
        {
            //DataTable l_dtAcctBalancesByBasisType=null;
            DataColumn dtNewColumn = null;
            try
            {
                if (para_g_dtAcctBalancesByBasisType != null)
                {
                    para_g_dtAcctBalancesByBasisType.Rows.Clear();
                }
                else
                {
                    para_g_dtAcctBalancesByBasisType = new DataTable();
                }


                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("chrAnnuityBasisType"))
                {
                    dtNewColumn = new DataColumn("chrAnnuityBasisType", typeof(string));
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("chrAnnuityBasisGroup"))
                {
                    dtNewColumn = new DataColumn("chrAnnuityBasisGroup", typeof(string));
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyPersonalRetirementBalance"))
                {
                    dtNewColumn = new DataColumn("mnyPersonalRetirementBalance", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyYmcaRetirementBalance"))
                {
                    dtNewColumn = new DataColumn("mnyYmcaRetirementBalance", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyBasicAcctRetirementBalance"))
                {
                    dtNewColumn = new DataColumn("mnyBasicAcctRetirementBalance", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }

                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("chvPlanType"))
                {
                    dtNewColumn = new DataColumn("chvPlanType", typeof(string));
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyBasicPersonalBalances"))
                {
                    dtNewColumn = new DataColumn("mnyBasicPersonalBalances", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyBasicYmcaBalance"))
                {
                    dtNewColumn = new DataColumn("mnyBasicYmcaBalance", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyNonBasicPersonalBalance"))
                {
                    dtNewColumn = new DataColumn("mnyNonBasicPersonalBalance", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }
                if (!para_g_dtAcctBalancesByBasisType.Columns.Contains("mnyNonBasicYmcalBalance"))
                {
                    dtNewColumn = new DataColumn("mnyNonBasicYmcalBalance", typeof(decimal));
                    dtNewColumn.DefaultValue = 0;
                    para_g_dtAcctBalancesByBasisType.Columns.Add(dtNewColumn);
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method add update row in balances by basis type
        /// </summary>
        /// <param name="para_Personal_Retirement_Balance"></param>
        /// <param name="para_YMCA_Retirement_Balance"></param>
        /// <param name="para_Basic_Retirement_Balance"></param>
        /// <param name="para_AnnuityBasisType"></param>
        /// <param name="para_AnnuityBasisGroup"></param>
        private void AddAcctBalancesByBasisTypeRow(decimal para_Personal_Retirement_Balance, decimal para_YMCA_Retirement_Balance, decimal para_Basic_Retirement_Balance
            , string para_AnnuityBasisType, string para_AnnuityBasisGroup, string para_PlanType, decimal para_mnyBasicPersonalBalances, decimal para_mnyBasicYmcaBalance, decimal para_mnyNonBasicPersonalBalance, decimal para_mnyNonBasicYmcalBalance)//, string strPreYmcaLegacy = "", string strPostYmcaLegacy = "", string strPreYmcaAccount = "", string strPostYmcaAccount = "")
        {
            DataRow drNewAcctBalancesRow = null;
            try
            {
                if (g_dtAcctBalancesByBasisType != null)
                {
                    if (para_Personal_Retirement_Balance > 0 || para_YMCA_Retirement_Balance > 0 || para_Basic_Retirement_Balance > 0 || para_mnyBasicPersonalBalances > 0 || para_mnyBasicYmcaBalance > 0 || para_mnyNonBasicPersonalBalance > 0 || para_mnyNonBasicYmcalBalance > 0)
                    {


                        drNewAcctBalancesRow = g_dtAcctBalancesByBasisType.NewRow();
                        drNewAcctBalancesRow["chrAnnuityBasisType"] = para_AnnuityBasisType;
                        drNewAcctBalancesRow["chrAnnuityBasisGroup"] = para_AnnuityBasisGroup;
                        drNewAcctBalancesRow["mnyPersonalRetirementBalance"] = para_Personal_Retirement_Balance;
                        drNewAcctBalancesRow["mnyYmcaRetirementBalance"] = para_YMCA_Retirement_Balance;
                        drNewAcctBalancesRow["mnyBasicAcctRetirementBalance"] = para_Basic_Retirement_Balance;


                        //					drNewAcctBalancesRow["mnyBasicPersonalAge60Balance"]=para_Basic_Personal_Age60_Balance;
                        //					drNewAcctBalancesRow["mnyBasicYmcaAge60Balance"]=para_Basic_YMCA_Age60_Balance;
                        drNewAcctBalancesRow["chvPlanType"] = para_PlanType;
                        drNewAcctBalancesRow["mnyBasicPersonalBalances"] = para_mnyBasicPersonalBalances;
                        drNewAcctBalancesRow["mnyBasicYmcaBalance"] = para_mnyBasicYmcaBalance;
                        drNewAcctBalancesRow["mnyNonBasicPersonalBalance"] = para_mnyNonBasicPersonalBalance;
                        drNewAcctBalancesRow["mnyNonBasicYmcalBalance"] = para_mnyNonBasicYmcalBalance;


                        g_dtAcctBalancesByBasisType.Rows.Add(drNewAcctBalancesRow);

                        g_dtAcctBalancesByBasisType.AcceptChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added by Ashish for phase V part III changes, End

        #endregion Custom Methods

        #region Data Access
        //Getting Data from Database through Data Access Object Layer

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_RetireeSSNo"></param>
        /// <returns></returns>
        public static DataSet SearchRetireeInfo(string Param_RetireeSSNo)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetireeInformation(Param_RetireeSSNo));
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_guiUniqueID"></param>
        /// <returns></returns>
        //public static DataSet SearchRetEstInfo(string Param_guiUniqueID)
        public static DataSet getParticipantBeneficiaries(string Param_guiUniqueID)
        {
            try
            {
                //return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetEstInformation(Param_guiUniqueID));
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getParticipantBeneficiaries(Param_guiUniqueID));
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_guiFundEventID"></param>
        /// <returns></returns>
        //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
        //public static DataSet SearchRetEmpInfo(string Param_guiPersID)
        public static DataSet SearchRetEmpInfo(string param_guiFundEventID, string retireType, DateTime retirementDate) //MMR | 2017.03.09 | YRS-AT-2625 | Added parameter for retire type and retirement date
        {
            try
            {
                //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                //return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetEmpInformation(Param_guiPersID));
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetEmpInformation(param_guiFundEventID, retireType, retirementDate)); //MMR | 2017.03.09 | YRS-AT-2625 | Added parameter for retire type and retirement date
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_guiPersID"></param>
        /// <param name="Param_guiYmcaID"></param>
        /// <param name="Param_StartDate"></param>
        /// <param name="Param_EndDate"></param>
        /// <returns></returns>
        public static DataSet SearchRetEmpSalInfo(string Param_guiFundEventID, string Param_guiYmcaID, string Param_StartDate, string Param_EndDate)
        {
            //2009.11.17 change parameter persID with fundEventID
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetEmpSalInformation(Param_guiFundEventID, Param_guiYmcaID, Param_StartDate, Param_EndDate));
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_guiPersID"></param>
        /// <param name="Param_guiYmcaID"></param>
        /// <returns></returns>
        public static DataSet SearchElectiveAccounts(string Param_guiPersID, string Param_guiYmcaID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetElectiveAccounts(Param_guiPersID, Param_guiYmcaID));
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_guiPersID"></param>
        /// <returns></returns>
        public static DataSet SearchElectiveAccounts(string Param_guiPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetElectiveAccounts(Param_guiPersID));
            }
            catch
            {
                throw;
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param_guiFundEventID"></param>
        /// <param name="Param_RetireType"></param>
        /// <param name="ProjectedRetirementDate"></param>
        /// <returns></returns>
        //public static DataSet getRetEstimateEmployment(string Param_guiFundEventID, string Param_RetireType, string ProjectedRetirementDate)
        public static DataSet getEmploymentDetails(string Param_guiFundEventID, string Param_RetireType, string ProjectedRetirementDate)
        {
            try
            {
                //return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetEstimateEmployment(Param_guiFundEventID, Param_RetireType, ProjectedRetirementDate));
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getEmploymentDetails(Param_guiFundEventID, Param_RetireType, ProjectedRetirementDate));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ByBasis"></param>
        /// <param name="Param_StartDate"></param>
        /// <param name="Param_EndDate"></param>
        /// <param name="Param_guiPersID"></param>
        /// <param name="Param_guiFundEventID"></param>
        /// <param name="Param_AllocateDummyAssnMoney"></param>
        /// <param name="Param_CondenseMoneyByAcctbyBasis"></param>
        /// <param name="planType"></param>
        /// <returns></returns>
        public static DataSet SearchAccountBalances(bool _ByBasis, string Param_StartDate, string Param_EndDate, string Param_guiPersID, string Param_guiFundEventID, bool Param_AllocateDummyAssnMoney, bool Param_CondenseMoneyByAcctbyBasis, string planType)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceData(_ByBasis, Param_StartDate, Param_EndDate, Param_guiPersID, Param_guiFundEventID, Param_AllocateDummyAssnMoney, Param_CondenseMoneyByAcctbyBasis, planType));
            }
            catch
            {
                throw;
            }

        }


        public static DataSet SearchContributionLimits()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getContributionLimits());
            }
            catch
            {
                throw;
            }

        }
        public static DataSet SearchAnnuityFactorPre96()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getAnnuityFactorPre96());
            }
            catch
            {
                throw;
            }

        }

        public static DataSet SearchMetaInterestRates()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getMetaInterestRates());
            }
            catch
            {
                throw;
            }

        }
        public static DataSet SearchActiveEmploymentEvents(string Param_guiPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getActiveEmploymentEvents(Param_guiPersID));
            }
            catch
            {
                throw;
            }
        }
        //ASHISH:2011.02.21 Replace persid with fundevent id
        //public static DataSet SearchYmcaResolutions(string Param_guiPersID) 
        //START: SB | 03/06/2017 | YRS-AT-2625 | Existing method is replaced with additional parameters to highest resolution for retirement estimates
        //public static DataSet SearchYmcaResolutions(string Param_guiFundEventID)
		public static DataSet SearchYmcaResolutions(string Param_guiFundEventID, DateTime Param_RetirementDate, string Param_RetirementType)
        //END: SB | 03/06/2017 | YRS-AT-2625 | Existing method is replaced with additional parameters to highest resolution for retirement estimates
        {
            try
            {
                // return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getYmcaResolutions(Param_guiPersID));
                //START: SB | 03/06/2017 | YRS-AT-2625 | Existing method is replaced with additional parameters to highest resolution for retirement estimates
                //return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getYmcaResolutions(Param_guiFundEventID));
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getYmcaResolutions(Param_guiFundEventID, Param_RetirementDate, Param_RetirementType));
                //END: SB | 03/06/2017 | YRS-AT-2625 | Existing method is replaced with additional parameters to highest resolution for retirement estimates
            }
            catch
            {
                throw;
            }
        }

        //ASHISH:2011.02.21 This method is not in use. 
        ///// <summary>
        ///// Get YMCA resolutions as per PlanType
        ///// </summary>
        ///// <param name="Param_guiPersID"></param>
        ///// <param name="planType"></param>
        ///// <returns></returns>
        //public static DataSet SearchYmcaResolutions(string Param_guiPersID, string planType)
        //{
        //    try
        //    {
        //        return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getYmcaResolutions(Param_guiPersID, planType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static DataSet SearchMetaAccountTypes()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getMetaAccountTypes());
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchKnownInterestRate(string Param_guiAcctType, string Param_Year, string Param_Month)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getKnownInterestRate(Param_guiAcctType, Param_Year, Param_Month));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchAnnuity(string tdRetireeBirthDate, string tdRetireDate, string tcAnnuityType, string tcRetireType, string tcBasisType, string tdBeneficiaryBirthDate, decimal tnBalance)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getAnnuitize(tdRetireeBirthDate, tdRetireDate, tcAnnuityType, tcRetireType, tcBasisType, tdBeneficiaryBirthDate, tnBalance));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchMetaAnnuityFactors(string tcRetireType, string tdRetireDate, string tdRetireeBirthDate, string tdBeneficiaryBirthDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getMetaAnnuityFactors(tcRetireType, tdRetireDate, tdRetireeBirthDate, tdBeneficiaryBirthDate));
            }
            catch
            {
                throw;
            }
        }
        //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
        public static DataSet SearchMetaAnnuityFactorsForDisability(string tcRetireType, string tdRetireDate, string tdRetireeBirthDate, string tdBeneficiaryBirthDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getMetaAnnuityFactorsForDisability(tcRetireType, tdRetireDate, tdRetireeBirthDate, tdBeneficiaryBirthDate));
            }
            catch
            {
                throw;
            }
        }
        ///ASHISH:2010.11.16:Added new method for YRS 5.0-1215
        public static DataSet SearchMetaAnnuityFactorsBeforeExactAgeEfftDate(string tcRetireType, string tdRetireDate, string tdRetireeBirthDate, string tdBeneficiaryBirthDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getMetaAnnuityFactorsBeforeExactAgeEffDate(tcRetireType, tdRetireDate, tdRetireeBirthDate, tdBeneficiaryBirthDate));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet SearchSurvivorsInfo(string guiPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getSurvivorsInfo(guiPersID));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet SearchBeneficiaryMinMaxAge()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getBeneficiaryMinMaxAge());
            }
            catch
            {
                throw;
            }
        }
        public static DataSet SearchDisabledBeneficiaryMinMaxAge()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getDisabledBeneficiaryMinMaxAge());
            }
            catch
            {
                throw;
            }
        }

        public DataSet getBenAnnuityFactors(string tcBasis, string tcBeneficiary, string tcRetireType, int tnRetireeRetirementFactorAge, int tnBeneficiaryRetirementFactorAge, DateTime p_datetime_RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getBenAnnuityFactors(tcBasis, tcBeneficiary, tcRetireType, tnRetireeRetirementFactorAge, tnBeneficiaryRetirementFactorAge, p_datetime_RetirementDate));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet getVestedInfo(string SSNo, string RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getVestedInfo(SSNo, RetirementDate));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet getDBAnnuityOptions(DateTime p_datetime_RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getDBAnnuityOptions(p_datetime_RetirementDate));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet getSSMetaConfigDetails()
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getSSMetaConfigDetails());
            }
            catch
            {
                throw;
            }
        }

        public static DataSet getSSReductionFactor(string dtmbirthdate, string dtmProjectedRetDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getSSReductionFactor(dtmbirthdate, dtmProjectedRetDate));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet getMetaAnnuityTypes(DateTime p_datetime_RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getMetaAnnuityTypes(p_datetime_RetirementDate));

            }
            catch
            {
                throw;
            }

        }

        public static DataSet LookUpMetaAnnuityTypes(DateTime p_datetime_RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.LookUpMetaAnnuityTypes(p_datetime_RetirementDate));
            }
            catch
            {
                throw;
            }
        }
        public static int getSafeHarborFactor(string RetireeBirthDate, string Benebirthdate, string RetireeRetireDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getSafeHarborFactor(RetireeBirthDate, Benebirthdate, RetireeRetireDate));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet EligNoRefundsPending(string guiPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligNoRefundsPending(guiPersID));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet EligibleUnFundedTransactionExist(string guiFundEventID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleUnFundedTransactionExist(guiFundEventID));
            }
            catch
            {
                throw;
            }
        }

        public static DataSet EligibleFundEvent(string EligibilityType, string Retiretype, string guiFundEventID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleFundEvent(EligibilityType, Retiretype, guiFundEventID));
            }
            catch
            {
                throw;
            }
        }

        public static int getEligiblePIA(string FundEventID, bool llVestedOnRetireDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getEligiblePIA(FundEventID, llVestedOnRetireDate));
            }
            catch
            {
                throw;
            }
        }
        public static int EligibleIsQDRO(string FundEventID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleIsQDRO(FundEventID));
            }
            catch
            {
                throw;
            }
        }

        public static string EligibleAge(string PersId, string dtm_Date, bool bit_QDRO, string RetireType, bool bit_EligibleServiceMinMonths, bool bit_SSPlanPre1974, string FundEventId)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleAge(PersId, dtm_Date, bit_QDRO, RetireType, bit_EligibleServiceMinMonths, bit_SSPlanPre1974, FundEventId));
            }
            catch
            {
                throw;
            }
        }

        //YRS 5.0-1035 : Set paid service - atsFundEvents.intPaid - to zero
        // START : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services 
        // public static Boolean getWithdrawntransactionCheckForDisability(string FundEventID)
        public static DataSet getWithdrawntransactionCheckForDisability(string FundEventID,DateTime RetirementDate)
        {
            try
            {
               // return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getWithdrawntransactionCheckForDisability(FundEventID));
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getWithdrawntransactionCheckForDisability(FundEventID, RetirementDate));
              // END : SB | 03/08/2017 | YRS-AT-2625 | Adding additional parameter required to get the paid services            
            }
            catch
            {
                throw;
            }
        }

        public static int getEligibleServiceMinMonths(string FundEventID, bool Bit_tlpaidOnly, string EligibilyType, string ProjectedRetDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getEligibleServiceMinMonths(FundEventID, Bit_tlpaidOnly, EligibilyType, ProjectedRetDate));
            }
            catch
            {
                throw;
            }
        }
        public static DataSet EligibleSSPlanPre1974(string guiPersID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleSSPlanPre1974(guiPersID));
            }
            catch
            {
                throw;
            }
        }
        public static int EstimateAge(string fundeventID, string RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EstimateAge(fundeventID, RetirementDate));
            }
            catch
            {
                throw;
            }
        }
        public static int EligibleNotTerminatedWithinMonths(string FundEventID, string RetirementDate, string TerminationDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleNotTerminatedWithinMonths(FundEventID, RetirementDate, TerminationDate));
            }
            catch
            {
                throw;
            }
        }

        //2011.03.24    Sanket Vaidya        For YRS 5.0-1294,BT 794  : For disability requirement 
        public static DateTime GetLastTerminationDate(string FundEventID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.GetLastTerminationDate(FundEventID));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Overloaded method to use the new stored procedure
        /// </summary>
        /// <param name="FundEventID"></param>
        /// <param name="RetirementDate"></param>
        /// <returns></returns>
        public static int EligibleNotTerminatedWithinMonths(string FundEventID, string RetirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.EligibleNotTerminatedWithinMonths(FundEventID, RetirementDate));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PersId"></param>
        /// <param name="RetDate"></param>
        /// <param name="RetireeBirthDate"></param>
        /// <param name="RetTypeCode"></param>
        /// <param name="UserId"></param>
        /// <param name="yrsDeathBenefit"></param>
        /// <param name="yrsDeathBenefitUsed"></param>
        /// <param name="lnTaxable"></param>
        /// <param name="lnnonTaxable"></param>
        /// <param name="yrsSSReductionAmount"></param>
        /// <param name="yrsSSDate"></param>
        /// <param name="yrsSSDateSav"></param>
        /// <param name="yrsOption"></param>
        /// <param name="yrsOptionSav"></param>
        /// <param name="dsFedWithDrawal"></param>
        /// <param name="dsGenWithDrawal"></param>
        /// <param name="dsRetired"></param>
        /// <param name="dsNotes"></param>
        /// <param name="dsAnnuityJointSurvivors"></param>
        /// <param name="dtSelectedAnnuity"></param>
        /// <param name="fundEventId"></param>
        /// <param name="fundEventStatus"></param>
        /// <param name="finalFundStatus"></param>
        /// <param name="isPrePlanSplitRetirement"></param>
        /// <returns></returns>
        public static bool Purchase(string PersId, string RetDate, string RetireeBirthDate, string RetTypeCode, string UserId,
            decimal yrsDeathBenefit, decimal yrsDeathBenefitUsed, decimal lnTaxable, decimal lnnonTaxable, decimal yrsSSReductionAmount, string yrsSSDate, string yrsSSDateSav,
            string yrsOption, string yrsOptionSav, DataSet dsFedWithDrawal, DataSet dsGenWithDrawal, DataSet dsRetired, DataSet dsBeneficiaryAddress,
            DataSet dsNotes, DataSet dsAnnuityJointSurvivors, DataTable dtSelectedAnnuity, string fundEventId,
            string fundEventStatus, string finalFundStatus, bool isPrePlanSplitRetirement,
            DataSet dsBeneficiariesSSN = null // SR | 2016.08.02 | YRS-AT-2382 | Added a new parameter for Beneficiary SSN change auditing.
            , YMCAObjects.StateWithholdingDetails objSTWPersonDetail = null // ML | 2019.11.26 | Yrs-AT- 4597| Added new paramater for StateTax Saving
            ) 
        {
            try
            {
                RetirementDAClass cls = new RetirementDAClass();                
                return cls.Purchase(dsFedWithDrawal, dsGenWithDrawal,
                    dsRetired, dsBeneficiaryAddress, dsNotes, dsAnnuityJointSurvivors, dtSelectedAnnuity,
                    fundEventId, fundEventStatus, PersId, RetDate, RetireeBirthDate, RetTypeCode, UserId,
                    yrsDeathBenefit, yrsDeathBenefitUsed, lnTaxable, lnnonTaxable, yrsSSReductionAmount, yrsSSDate, yrsSSDateSav,
                    yrsOption, yrsOptionSav, finalFundStatus, isPrePlanSplitRetirement,
                    dsBeneficiariesSSN  // SR | 2016.08.02 | YRS-AT-2382 | Added a new parameter for Beneficiary SSN change auditing
                    , objSTWPersonDetail // ML | 2019.11.26 | Yrs-AT- 4597 | Added new paramater for StateTax Saving
                    );
            }
            catch
            {
                throw;
            }
        }


        #region InsertPRA_ReportValues
        //2012.07.13 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page (adding new parameter projFinalYearSal)
        public static string InsertPRA_ReportValues(string PersID, int AgeRetired,

            string BenBirthDate,
            string BeneficiaryName,

            string PRAAssumption,
            string RetBirthDate,

            string RetiredDate,
            decimal RetiredDeathBen,

            decimal C_Insured,
            decimal C_Monthly,
            decimal C_Reduction,

            decimal C62_Insured,
            decimal C62_Monthly,
            decimal C62_Reduction,
            decimal CS_Insured,
            decimal CS_Monthly,
            decimal CS_Reduction,


            decimal J1_Retiree,
            decimal J1_Survivor,
            decimal J162_Retiree,
            decimal J162_Survivor,
            decimal J1I_Retiree,
            decimal J1I_Survivor,


            decimal J1P_Retiree,
            decimal J1P_Survivor,
            decimal J1P62_Retiree,
            decimal J1P62_Survivor,
            decimal J1PS_Retiree,

            decimal J1PS_Survivor,
            decimal J1S_Retiree,
            decimal J1S_Survivor,
            decimal J5_Retiree,
            decimal J5_Survivor,

            decimal J562_Retiree,
            decimal J562_Survivor,
            decimal J5I_Retiree,
            decimal J5I_Survivor,
            decimal J5L_Retiree,

            decimal J5L_Survivor,
            decimal J5L62_Retiree,
            decimal J5L62_Survivor,
            decimal J5LS_Retiree,
            decimal J5LS_Survivor,


            decimal J5P_Retiree,
            decimal J5P_Survivor,
            decimal J5P62_Retiree,
            decimal J5P62_Survivor,
            decimal J5PS_Retiree,

            decimal J5PS_Survivor,
            decimal J5S_Retiree,
            decimal J5S_Survivor,
            decimal J7_Retiree,
            decimal J7_Survivor,

            decimal J762_Retiree,
            decimal J762_Survivor,
            decimal J7I_Retiree,
            decimal J7I_Survivor,
            decimal J7L_Retiree,


            decimal J7L_Survivor,
            decimal J7L62_Retiree,
            decimal J7L62_Survivor,
            decimal J7LS_Retiree,
            decimal J7LS_Survivor,

            decimal J7P_Retiree,
            decimal J7P_Survivor,
            decimal J7P62_Retiree,
            decimal J7P62_Survivor,
            decimal J7PS_Retiree,

            decimal J7PS_Survivor,
            decimal J7S_Retiree,
            decimal J7S_Survivor,
            decimal M_Retiree,
            decimal M62_Retiree,
            decimal MI_Retiree,
            decimal MS_Retiree,


            decimal ZC_Annually,
            decimal ZC62_Annually,
            decimal ZCS_Annually,
            decimal ZJ1_Retiree,
            decimal ZJ1_Survivor,
            decimal ZJ162_Retiree,
            decimal ZJ162_Survivor,

            decimal ZJ1I_Retiree,
            decimal ZJ1I_Survivor,
            decimal ZJ1P_Retiree,
            decimal ZJ1P_Survivor,
            decimal ZJ1P62_Retiree,
            decimal ZJ1P62_Survivor,
            decimal ZJ1PS_Retiree,


            decimal ZJ1PS_Survivor,
            decimal ZJ1S_Retiree,
            decimal ZJ1S_Survivor,
            decimal ZJ5_Retiree,
            decimal ZJ5_Survivor,
            decimal ZJ562_Retiree,
            decimal ZJ562_Survivor,


            decimal ZJ5I_Retiree,
            decimal ZJ5I_Survivor,
            decimal ZJ5L_Retiree,
            decimal ZJ5L_Survivor,

            decimal ZJ5L62_Retiree,
            decimal ZJ5L62_Survivor,
            decimal ZJ5LS_Retiree,

            decimal ZJ5LS_Survivor,
            decimal ZJ5P_Retiree,
            decimal ZJ5P_Survivor,

            decimal ZJ5P62_Retiree,
            decimal zJ5P62_Survivor,
            decimal ZJ5PS_Retiree,


            decimal ZJ5PS_Survivor,
            decimal ZJ5S_Retiree,
            decimal ZJ5S_Survivor,

            decimal ZJ7_Retiree,
            decimal ZJ7_Survivor,
            decimal ZJ762_Retiree,

            decimal ZJ762_Survivor,
            decimal ZJ7I_Retiree,
            decimal ZJ7I_Survivor,

            decimal ZJ7L_Retiree,
            decimal ZJ7L_Survivor,
            decimal zJ7L62_Retiree,

            decimal ZJ7L62_Survivor,
            decimal ZJ7LS_Retiree,
            decimal ZJ7LS_Survivor,

            decimal ZJ7P_Retiree,
            decimal zJ7P_Survivor,
            decimal ZJ7P62_Retiree,

            decimal zJ7P62_Survivor,
            decimal ZJ7PS_Retiree,
            decimal zJ7PS_Survivor,

            decimal ZJ7S_Retiree,
            decimal ZJ7S_Survivor,
            decimal ZM_Retiree,

            decimal ZM62_Retiree,
            decimal ZMI_Retiree,
            decimal ZMS_Retiree,
            decimal SSBenefit,
            decimal SSIncrease,
            decimal DeathBenAmtUsed,
            decimal DeathBenPercUsed, string paraFundNo
            , decimal projFinalYearSal //2012.07.13 SP : BT-1041/YRS 5.0-1599:
            )//DataSet parameterPRAReportValues)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.InsertPRA_ReportValues(PersID, AgeRetired,

                    BenBirthDate,
                    BeneficiaryName,

                    PRAAssumption,
                    RetBirthDate,

                    RetiredDate,
                    RetiredDeathBen,

                    C_Insured,
                    C_Monthly,
                    C_Reduction,

                    C62_Insured,
                    C62_Monthly,
                    C62_Reduction,
                    CS_Insured,
                    CS_Monthly,
                    CS_Reduction,


                    J1_Retiree,
                    J1_Survivor,
                    J162_Retiree,
                    J162_Survivor,
                    J1I_Retiree,
                    J1I_Survivor,


                    J1P_Retiree,
                    J1P_Survivor,
                    J1P62_Retiree,
                    J1P62_Survivor,
                    J1PS_Retiree,

                    J1PS_Survivor,
                    J1S_Retiree,
                    J1S_Survivor,
                    J5_Retiree,
                    J5_Survivor,

                    J562_Retiree,
                    J562_Survivor,
                    J5I_Retiree,
                    J5I_Survivor,
                    J5L_Retiree,

                    J5L_Survivor,
                    J5L62_Retiree,
                    J5L62_Survivor,
                    J5LS_Retiree,
                    J5LS_Survivor,


                    J5P_Retiree,
                    J5P_Survivor,
                    J5P62_Retiree,
                    J5P62_Survivor,
                    J5PS_Retiree,

                    J5PS_Survivor,
                    J5S_Retiree,
                    J5S_Survivor,
                    J7_Retiree,
                    J7_Survivor,

                    J762_Retiree,
                    J762_Survivor,
                    J7I_Retiree,
                    J7I_Survivor,
                    J7L_Retiree,


                    J7L_Survivor,
                    J7L62_Retiree,
                    J7L62_Survivor,
                    J7LS_Retiree,
                    J7LS_Survivor,

                    J7P_Retiree,
                    J7P_Survivor,
                    J7P62_Retiree,
                    J7P62_Survivor,
                    J7PS_Retiree,

                    J7PS_Survivor,
                    J7S_Retiree,
                    J7S_Survivor,
                    M_Retiree,
                    M62_Retiree,
                    MI_Retiree,
                    MS_Retiree,


                    ZC_Annually,
                    ZC62_Annually,
                    ZCS_Annually,
                    ZJ1_Retiree,
                    ZJ1_Survivor,
                    ZJ162_Retiree,
                    ZJ162_Survivor,

                    ZJ1I_Retiree,
                    ZJ1I_Survivor,
                    ZJ1P_Retiree,
                    ZJ1P_Survivor,
                    ZJ1P62_Retiree,
                    ZJ1P62_Survivor,
                    ZJ1PS_Retiree,


                    ZJ1PS_Survivor,
                    ZJ1S_Retiree,
                    ZJ1S_Survivor,
                    ZJ5_Retiree,
                    ZJ5_Survivor,
                    ZJ562_Retiree,
                    ZJ562_Survivor,


                    ZJ5I_Retiree,
                    ZJ5I_Survivor,
                    ZJ5L_Retiree,
                    ZJ5L_Survivor,

                    ZJ5L62_Retiree,
                    ZJ5L62_Survivor,
                    ZJ5LS_Retiree,

                    ZJ5LS_Survivor,
                    ZJ5P_Retiree,
                    ZJ5P_Survivor,

                    ZJ5P62_Retiree,
                    zJ5P62_Survivor,
                    ZJ5PS_Retiree,


                    ZJ5PS_Survivor,
                    ZJ5S_Retiree,
                    ZJ5S_Survivor,

                    ZJ7_Retiree,
                    ZJ7_Survivor,
                    ZJ762_Retiree,

                    ZJ762_Survivor,
                    ZJ7I_Retiree,
                    ZJ7I_Survivor,

                    ZJ7L_Retiree,
                    ZJ7L_Survivor,
                    zJ7L62_Retiree,

                    ZJ7L62_Survivor,
                    ZJ7LS_Retiree,
                    ZJ7LS_Survivor,

                    ZJ7P_Retiree,
                    zJ7P_Survivor,
                    ZJ7P62_Retiree,

                    zJ7P62_Survivor,
                    ZJ7PS_Retiree,
                    zJ7PS_Survivor,

                    ZJ7S_Retiree,
                    ZJ7S_Survivor,
                    ZM_Retiree,

                    ZM62_Retiree,
                    ZMI_Retiree,
                    ZMS_Retiree,
                    SSBenefit,
                    SSIncrease,
                    DeathBenAmtUsed,
                    DeathBenPercUsed, paraFundNo
                    , projFinalYearSal   //2012.07.13 SP : BT-1041/YRS 5.0-1599:
                    ));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataSet getLastPayrollDate()
        {
            return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getLastPayrollDate());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tnTotalTaxable"></param>
        /// <param name="tcPersID"></param>
        /// <param name="tlForce"></param>
        /// <returns></returns>
        public DataSet getWithHoldings(decimal tnTotalTaxable, string tcPersID, int tlForce)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getWithHoldings(tnTotalTaxable, tcPersID, tlForce));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pstrFundEventId"></param>
        /// <returns></returns>
        public DataSet GetPersonDetails(string pstrFundEventId)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.GetPersonDetails(pstrFundEventId));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataSet TaxFactors()
        {
            try
            {
                return YMCARET.YmcaDataAccessObject.RetirementDAClass.TaxFactors();
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="persId"></param>
        /// <returns></returns>
        public static DataSet GetActiveBeneficiaries(string persId)
        {
            try
            {
                return RetirementDAClass.GetActiveBeneficiaries(persId);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="persId"></param>
        /// <returns></returns>
        public static DataSet GetJointAnnuitySurvivors(string persId)
        {
            try
            {
                return RetirementDAClass.GetJointAnnuitySurvivors(persId);
            }
            catch
            {
                throw;
            }
        }


        //public static decimal GetExistingAnnuities(string fundEventID)
        public static DataTable GetExistingAnnuities(string fundEventID)
        {
            DataSet dsExistingAnnuities = null;
            dsExistingAnnuities = RetirementDAClass.GetExistingAnnuities(fundEventID);

            //return Convert.ToDecimal(dsExistingAnnuities.Tables[0].Rows[0]["ExistingAnnuities"].ToString());
            return dsExistingAnnuities.Tables[0];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="retirementDate"></param>
        /// <returns></returns>
        public static decimal GetExperienceDividends(string retirementDate)
        {
            DataSet dsExperienceDividends = null;
            dsExperienceDividends = RetirementDAClass.GetExperienceDividends(retirementDate);

            return Convert.ToDecimal(dsExperienceDividends.Tables[0].Rows[0]["ExpDividend"].ToString());
        }
        //Added by Ashish for phase V part III changes, Start
        /// <summary>
        /// This method get the list of basis type
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAnnuityBasisTypeList()
        {
            DataSet dsAnnuityBasisTypeList = null;
            DataTable dtAnnuityBasisTypeList = null;
            try
            {
                dsAnnuityBasisTypeList = YMCARET.YmcaDataAccessObject.RetirementDAClass.GetAnnuityBasisTypeList();
                if (dsAnnuityBasisTypeList != null)
                {
                    if (dsAnnuityBasisTypeList.Tables.Count > 0)
                    {
                        if (dsAnnuityBasisTypeList.Tables["AnnuityBasisTypeList"].Rows.Count > 0)
                        {
                            dtAnnuityBasisTypeList = dsAnnuityBasisTypeList.Tables["AnnuityBasisTypeList"];
                        }
                    }

                }
                else
                {
                    throw new Exception("Unable to retrive AnnuityBasisType information");
                }
                return dtAnnuityBasisTypeList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added by Ashish for phase V part III changes, End
        #endregion Data Access

        #region  Retirement Estimate

        /// <summary>
        /// Calculate the balance in each account that the user has directed to use for his retirement.
        /// </summary>
        /// <param name="p_dataset_RetEstimateEmployment"></param>
        /// <param name="txtRetireeBirthday"></param>
        /// <param name="txtFutureSalary"></param>
        /// <param name="txtRetirementDate"></param>
        /// <param name="txtFutureSalaryEffDate"></param>
        /// <param name="txtModifiedSal"></param>
        /// <param name="txtEndWorkDate"></param>
        /// <param name="personID"></param>
        /// <param name="retireType"></param>
        /// <param name="projectedInterestRate"></param>
        /// <param name="dataSetElectiveAccounts"></param>
        /// <param name="annualSalaryIncrease"></param>
        /// <param name="combinedDataSet"></param>
        /// <param name="isEstimate"></param>
        /// <param name="planType"></param>
        /// <param name="fundStatus"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>        
        public bool CalculateAccountBalancesAndProjections(DataSet p_dataset_RetEstimateEmployment, string txtRetireeBirthday
            , string txtRetirementDate
            , string personID, string fundeventID, string retireType
            , double projectedInterestRate, DataSet dataSetElectiveAccounts
            , DataSet combinedDataSet, bool isEstimate, string planType, string fundStatus, ref string errorMessage, ref string warningMessage
            , DataTable dtExcludedAccounts, DataTable employmentDetails, bool isEsimateProjBal
            , string para_MaxTerminationDate, ref bool isYmcaLegacyAcctTotalExceed, ref bool isYmcaAcctTotalExceed, int iAge = 0, bool bIsPersonTerminated = false)
        {
            bool hasNoErrors = true;
            #region commented for YRS 5.0-855 BT-624
            //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 Start
            //string futSal = string.Empty;
            //string modSal = string.Empty;
            //string salIncrease = string.Empty;        


            //// Update the Employment details from the employmentDetails			
            //if (employmentDetails != null) //in case a person does not have employment events, it could be for qdro, death or an annuity beneficiary.
            //{
            //    if (employmentDetails.Rows.Count > 0)
            //    {
            //        foreach(DataRow dr in employmentDetails.Rows)
            //        {
            //            if (!dr.IsNull("FutureSalary") && dr["FutureSalary"].ToString() != string.Empty && !dr .IsNull("FutureSalaryEffDate") && dr["FutureSalaryEffDate"].ToString() != string.Empty)
            //            {
            //                txtFutureSalary = string.Empty;
            //                txtFutureSalaryEffDate = string.Empty;							
            //                //Phase IV Changes - Start
            //                //txtModifiedSal = string.Empty;
            //                //txtEndWorkDate = string.Empty;
            //                //annualSalaryIncrease = 0;
            //                //Phase IV Changes - End

            //                //DataRow dr = employmentDetails.Rows[0];
            //                futSal = dr["FutureSalary"].ToString();
            //                txtFutureSalary = (futSal == string.Empty ? "0" : futSal);

            //                txtFutureSalaryEffDate = dr["FutureSalaryEffDate"].ToString();

            //                //Phase IV Changes - Start
            //                //string modSal = dr["ModifiedSal"].ToString();
            //                //txtModifiedSal = (modSal == string.Empty ? "0" : modSal);
            //                //Phase IV Changes - End

            //                //txtEndWorkDate = dr["EndWorkDate"].ToString();//Phase IV Changes

            //                //string salIncrease = dr["AnnualSalaryIncrease"].ToString();
            //                //annualSalaryIncrease = (salIncrease == string.Empty ? 0 : Convert.ToInt32(salIncrease));

            //                //break;
            //            }

            //            //Phase IV Changes - Start
            //            txtModifiedSal = string.Empty;
            //            if (!dr.IsNull("ModifiedSal") && dr["ModifiedSal"].ToString() != string.Empty)
            //            {
            //                modSal = dr["ModifiedSal"].ToString();
            //                txtModifiedSal = (modSal == string.Empty ? "0" : modSal);
            //            }
            //            //Phase IV Changes - End

            //            //Phase IV Changes - Start
            //            txtEndWorkDate = string.Empty;						
            //            if (!dr.IsNull("EndWorkDate") && dr["EndWorkDate"].ToString() != string.Empty)
            //            {
            //                txtEndWorkDate = dr["EndWorkDate"].ToString();
            //            }
            //            //Phase IV Changes - End

            //            //Phase IV Changes - Start
            //            if (!dr.IsNull("AnnualSalaryIncrease") && dr["AnnualSalaryIncrease"].ToString() != string.Empty && !dr.IsNull("AnnualSalaryIncreaseEffDate") && dr["AnnualSalaryIncreaseEffDate"].ToString() != string.Empty)
            //            {
            //                annualSalaryIncrease = 0;

            //                salIncrease = dr["AnnualSalaryIncrease"].ToString();
            //                annualSalaryIncrease = (salIncrease == string.Empty ? 0 : Convert.ToInt32(salIncrease));

            //                txtAnnualSalaryIncreaseEffDate = dr["AnnualSalaryIncreaseEffDate"].ToString();
            //            }
            //            //Phase IV Changes - End
            //            break;
            //        }
            //    }
            //}
            //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 End
            #endregion
            DataTable dtAccountsBasisByProjection = null;
            DataSet dsElectiveAccounts = null;
            DataTable dtAccountsByBasis = null;
            //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 
            //DataSet dsRetEstimateEmployment = new DataSet();
            DataTable dtRetEstimateEmployment = null;
            DataSet l_dsContributionLimits = null;

            string[,] laProjectionPeriods = new string[4, 3];

            int lnProjectionPeriod;
            int lnRETIREMENT_PROJECTION_PHASE;
            int C_PROJECTIONPERIOD = 1;
            int C_EFFDATE = 2;
            int C_TERMDATE = 3;
            double l_numGuaranteedInterestRatePre96 = 0;
            double lnMaximumContributionSalary = 0;
            //double lnContributionMaxAnnualTD = 0;//not in use
            //double lnContributionMaxAnnual = 0;//not in use
            DateTime ycCalcMonth;
            DateTime ldCalcMonthRetire;

            string strError = string.Empty;
            //START: MMR | 2017.03.03 | YRS-AT-2625 | Declared string  and object variable to hold manal trasnaction details
            string transactionDetails = string.Empty; 
            DataSet manualTransactionDetails = null;
            //END: MMR | 2017.03.03 | YRS-AT-2625 | Declared string  and object variable to hold manal trasnaction details
            bool isUnsubmittedBalanceConsidered; //PPP | 03/17/2017 | YRS-AT-2625 | It will instruct the prgram whether to include unsubmitted balance 
            //START: PPP | 03/20/2017 | YRS-AT-2625 | If in DISABL, active person is doing future dated retirment date 
            //--then from current date to retirement date projections should be based on NORMAL type, and then from retirement date through age 60 
            //--projection should be based on DISABL, So this flag will help to identify this condition
            bool isNormalProjectionRequiredInDisability;
            //END: PPP | 03/20/2017 | YRS-AT-2625 | If in DISABL, active person is doing future dated retirment date 
            try
            {
                dtAccountsBasisByProjection = new DataTable();
                dsElectiveAccounts = new DataSet();
                dtAccountsByBasis = new DataTable();
                dtRetEstimateEmployment = new DataTable();
                l_dsContributionLimits = new DataSet();
                manualTransactionDetails = new DataSet(); //MMR | 2017.03.03 | YRS-AT-2625 | Initialising dataset object
                //Create  local copy of parameter
                // Create copy of Employment DataTable
                if (p_dataset_RetEstimateEmployment != null)
                    dtRetEstimateEmployment = p_dataset_RetEstimateEmployment.Tables[0].Copy();
                // Create copy of Elective Account Details.

                //START: MMR | 2017.03.03 | YRS-AT-2625 | Adding manual transaction details into dataset for XML format
                if (p_dataset_RetEstimateEmployment.Tables.Contains("ManualTransactionDetails"))
                {
                    manualTransactionDetails.Tables.Add(p_dataset_RetEstimateEmployment.Tables["ManualTransactionDetails"].Copy());
                    if (HelperFunctions.isNonEmpty(manualTransactionDetails))
                    {
                        transactionDetails = manualTransactionDetails.GetXml();
                    }
                }
                //END: MMR | 2017.03.03 | YRS-AT-2625 | Adding manual transaction details into dataset for XML format
                dsElectiveAccounts = dataSetElectiveAccounts;

                ldCalcMonthRetire = Convert.ToDateTime(Convert.ToDateTime(txtRetirementDate).Month.ToString() + "/1/" + Convert.ToDateTime(txtRetirementDate).Year.ToString());
                string ldCalcMonthToday = Convert.ToDateTime(DateTime.Now.Month + "/1/" + DateTime.Now.Year).ToString("MM/dd/yyyy");
                // Step 1. - Get Maximum contribution salary
                l_dsContributionLimits = RetirementBOClass.SearchContributionLimits();
                if (l_dsContributionLimits.Tables[0].Rows.Count != 0)
                {
                    if (l_dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"] != DBNull.Value)
                    {
                        lnMaximumContributionSalary = Convert.ToDouble(l_dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"]);
                    }
                }
                else
                {
                    //throw (new Exception("Please specify the maximum Contribution Limit for the year, to proceed further."));//commented on 24-sep for BT-1126
                    throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_CONTRIBUTION"));
                }

                //START: PPP | 03/20/2017 | YRS-AT-2625 | If in DISABL, active person is doing future dated retirment date 
                //--then from current date to retirement date projections should be based on NORMAL type, and then from retirement date through age 60 
                //--projection should be based on DISABL, So changing the retirement type from DISABL to NORMAL
                //--This will help to keep salary and TD contribuion being added till retirement date
                isNormalProjectionRequiredInDisability = false;
                if (retireType == "DISABL" && ldCalcMonthRetire > Convert.ToDateTime(ldCalcMonthToday))
                {
                    isNormalProjectionRequiredInDisability = true;
                    retireType = "NORMAL";
                }
                //END: PPP | 03/20/2017 | YRS-AT-2625 | If in DISABL, active person is doing future dated retirment date 

                //Step 2
                //Get YMCA Resolution]
                // START : SB | 03/06/2017 | YRS-AT-2625 | Existing function is being replaced by additional parameters for finding out resolution in calculating retire Estimates 
                // DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(fundeventID)
                DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(fundeventID, Convert.ToDateTime(txtRetirementDate), retireType);
                // END : SB | 03/06/2017 | YRS-AT-2625 | Existing function is being replaced by additional parameters for finding out resolution in calculating retire Estimates 
                ymcaResolution = l_dsYmcaResolutions; //PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI

                //ASHISH:2011.02.19 Added validation for if retirement type disability and there is no active YMCA resolution
                //if (retireType.ToUpper() == "DISABL")
                //{
                //    if (l_dsYmcaResolutions != null)
                //    {
                //        if (l_dsYmcaResolutions.Tables[0].Rows.Count == 0)
                //        {
                //            if (errorMessage == string.Empty)
                //                errorMessage = "There is no active YMCA resolution.";
                //            else
                //                errorMessage += "<br> There is no active YMCA resolution.";
                //            hasNoErrors = false;
                //            goto ReturnError;
                //        }
                //    }
                //}

                //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                //Step 3 Create copy of Employement Table And update modified salary 
                //This method update modified salary information into dtRetEstimateEmployment table  from employmentDetails tables
                UpdateEmployeeInformation(ref dtRetEstimateEmployment, employmentDetails, l_dsYmcaResolutions.Tables[0], retireType, ldCalcMonthToday, txtRetirementDate);
                if (retireType.ToUpper() == "DISABL")
                {
                    //2011.04.18    Sanket vaidya        BT-816 : Disability Retirement Estimate Issues.
                    //if (isEstimate)
                    //{
                    //    //if(fundStatus.Trim().ToUpper() !="RT" || fundStatus.Trim().ToUpper()=="RPT")   
                    //    if (!ValidateTerminationIsCloseEnough(dtRetEstimateEmployment, txtRetirementDate, ref errorMessage))
                    //    {
                    //        hasNoErrors = false;
                    //        goto ReturnError;
                    //    }
                    //}
                    //This method terminate voluntary acct for projection
                    TerminateElectiveAccountsForDisability(ref dsElectiveAccounts, ldCalcMonthToday, txtRetirementDate);


                }

                // Step 4. - Get account balances
                // Get balances by account and by Basis
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //DataSet dsAcctBalByBasis = RetirementBOClass.SearchAccountBalances(true, "01/01/1900", ltAccountBalanceEndDate, personID, fundeventID, false, true, "B");//planType);			
                DataSet dsAcctBalByBasis = RetirementBOClass.SearchAccountBalances(true, "01/01/1900", txtRetirementDate, personID, fundeventID, false, true, planType);
                if (dsAcctBalByBasis.Tables.Count != 0) // && dsAcctBalByBasis.Tables[0].Rows.Count != 0)
                {	//NP:2008.05.06 - We still want to copy the structure if we do not get the data. Thus, removing the second part of the condition for Row.Count != 0 in the above IF statement
                    dtAccountsByBasis = dsAcctBalByBasis.Tables[0].Copy();
                    dtAccountsByBasis.AcceptChanges();
                }

                //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment

                if (retireType.ToUpper() == "NORMAL")
                {
                    if (!ValidatePercentContributionWithSalary(dtRetEstimateEmployment, dtAccountsByBasis, dataSetElectiveAccounts
                        , fundStatus, ref errorMessage))
                    {
                        hasNoErrors = false;
                        goto ReturnError;
                    }
                }
                //ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624,End

                // Step 5. - Create the projection table with initial values from dtAccountsByBasis
                // Projection table will contain 3 more columns TranDate, ProjPeriod, ProjPerSequence with 
                // default values '01/01/1900', "", 0
                dtAccountsBasisByProjection = dtAccountsByBasis.Clone();
                createProjectionTable(dtAccountsBasisByProjection, dtAccountsByBasis);

                //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624, End
                // Step 6. - Take a side copy of  dtRetEstimateEmployment and populate the modified salary entered by the user
                DataTable dtEmploymentProjectedSalary = new DataTable();
                //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624, Start
                //createProjectedSalary(dtEmploymentProjectedSalary, dtRetEstimateEmployment
                //    , lnMaximumContributionSalary, txtModifiedSal, isEstimate, employmentDetails, ref warningMessage);
                createProjectedSalary(dtEmploymentProjectedSalary, dtRetEstimateEmployment
                    , lnMaximumContributionSalary, ref warningMessage, retireType, fundeventID, txtRetirementDate, transactionDetails); // MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format

                //START | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment record hence no need to get AverageSalary
                if (HelperFunctions.isNonEmpty(dtEmploymentProjectedSalary))
                {
                    averageSalary = Convert.ToDecimal(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]); //PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI
                }
                // END | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment record hence no need to get AverageSalary

                // Step 7. Fetch the Future accounts specified by the user 
                // (Elective accounts He has not yet made the contribution and intends to do it in future)
                //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                if (retireType.ToUpper() == "NORMAL")
                {
                    getFutureAccountDetails(dtAccountsByBasis, dsElectiveAccounts);
                }

                // Step 8. Get latest YMCA resolutions account type ,if not exists in dtAccountsByBasis then inserted into dtAccountsByBasis
                getYMCAResolutions(dtAccountsByBasis, l_dsYmcaResolutions);


                // Step 9. Get the details required for calculating the Interest on Contributions
                // Get Pre 96 InterestRate and its termination date.
                string l_CalcMonthStartPre96 = string.Empty;
                DataSet l_dsAnnuityFactor = new DataSet();
                l_dsAnnuityFactor = RetirementBOClass.SearchAnnuityFactorPre96();
                if (l_dsAnnuityFactor.Tables.Count != 0 && l_dsAnnuityFactor.Tables[0].Rows.Count != 0)
                {
                    if (l_dsAnnuityFactor.Tables[0].Rows[0]["numGuaranteedInterestRate"] != DBNull.Value)
                        l_numGuaranteedInterestRatePre96 = Convert.ToDouble(l_dsAnnuityFactor.Tables[0].Rows[0]["numGuaranteedInterestRate"]);
                    if (l_dsAnnuityFactor.Tables[0].Rows[0]["dtmTermDate"] != DBNull.Value)
                    {
                        l_CalcMonthStartPre96 = Convert.ToDateTime(l_dsAnnuityFactor.Tables[0].Rows[0]["dtmTermDate"]).ToString("MM/dd/yyyy");
                        if (Convert.ToDateTime(l_CalcMonthStartPre96).Month == 12)
                            l_CalcMonthStartPre96 = "01/01/" + (Convert.ToDateTime(l_CalcMonthStartPre96).Year + 1);
                        else
                            l_CalcMonthStartPre96 = Convert.ToDateTime((Convert.ToDateTime(l_CalcMonthStartPre96).Month + 1) + "/01/" + Convert.ToDateTime(l_CalcMonthStartPre96).Year + 1).ToString("MM/dd/yyyy");
                    }
                }

                //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                string ldStartDateTmp = string.Empty;
                bool _llDone = false;
                int lnRETIREDATE_TO_AGE60 = 0;
                isUnsubmittedBalanceConsidered = false; //PPP | 03/08/2017 | YRS-AT-2625 | This flag will indicate process whether pending balance is added or not, if added then it avoids it.
                //START: PPP | 03/20/2017 | YRS-AT-2625 | If normal projection required in DISABL then setting isNormalProjectionRequiredInDisability=true and retireType == "NORMAL"
                //--So instead of "if (retireType == "DISABL" && ldCalcMonthRetire > Convert.ToDateTime(ldCalcMonthToday))" now need to check "isNormalProjectionRequiredInDisability=true"
                //if (retireType == "DISABL" && ldCalcMonthRetire > Convert.ToDateTime(ldCalcMonthToday))
                if (isNormalProjectionRequiredInDisability)
                //END: PPP | 03/20/2017 | YRS-AT-2625 | If normal projection required in DISABL then setting isNormalProjectionRequiredInDisability=true and retireType == "NORMAL"
                {
                    //Calculate Interest up to Retirement Date as NORMAL retirement type
                    //Set projection period
                    SetProjectionPeriodsArray(ref laProjectionPeriods, ldCalcMonthRetire, ldCalcMonthToday, "NORMAL", txtRetireeBirthday, C_EFFDATE, C_TERMDATE, C_PROJECTIONPERIOD);
                    lnProjectionPeriod = 2;
                    lnRETIREMENT_PROJECTION_PHASE = 1;
                    ycCalcMonth = Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod]);

                    // Check if calcDate is greater than the termination Date.
                    _llDone = false;
                    _llDone = (ycCalcMonth >= Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)]));

                    // Step 10.
                    lnRETIREDATE_TO_AGE60 = 0;
                    // Now till here we have calculated and fetched the following information
                    // 1. dtAccountsBasisByProjection -- All the new account contributions along with existing account contribution
                    // 2. dtRetEstimateEmployment -- All the employment details till date
                    // 3. dtEmploymentProjectedSalary -- Employment details with modified data.
                    // 4. dtAccountsByBasis -- Existing account contributions with YMCA resolution details
                    // 5. dsElectiveAccounts -- All the account details -- Existing and user modified (read proposed)
                    //2012.05.08  SP:  BT-1032 : Additional comments added for Gemini ID YRS 5.0-1574
                    strError = string.Empty;
                    calculateInterestOnContributions(ycCalcMonth, ref lnRETIREMENT_PROJECTION_PHASE, ref lnRETIREDATE_TO_AGE60
                        , dtAccountsBasisByProjection, dtRetEstimateEmployment, dtEmploymentProjectedSalary, dtAccountsByBasis
                        , laProjectionPeriods, lnProjectionPeriod, _llDone, fundeventID
                        , projectedInterestRate, "NORMAL", l_numGuaranteedInterestRatePre96
                        , dsElectiveAccounts.Tables[0], txtRetireeBirthday, employmentDetails, ref warningMessage, fundStatus, para_MaxTerminationDate, ref isYmcaLegacyAcctTotalExceed, ref isYmcaAcctTotalExceed, out strError, iAge, bIsPersonTerminated
                        , !isUnsubmittedBalanceConsidered //PPP | 03/16/2017 | YRS-AT-2625 | Indicates function to "not consider" pending balance
                        );
                    if (!string.IsNullOrEmpty(strError))
                    {
                        hasNoErrors = false;
                        //errorMessage = "Numpayperiod is not defined. Unable to proceed further.";//commented on 24-sep for BT-1126
                        errorMessage = "MESSAGE_RETIREMENT_BOC_NUMPAYPERIOD_NOT_DEFINED";
                        goto ReturnError;
                    }
                    isUnsubmittedBalanceConsidered = true;

                    //START: PPP | 03/20/2017 | YRS-AT-2625 | Reversing the changes
                    isNormalProjectionRequiredInDisability = false;
                    retireType = "DISABL";

                    //Correcting salary information and terminating open TD contracts
                    l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(fundeventID, Convert.ToDateTime(txtRetirementDate), retireType);
                    ymcaResolution = l_dsYmcaResolutions; // Required for logging

                    UpdateEmployeeInformation(ref dtRetEstimateEmployment, employmentDetails, l_dsYmcaResolutions.Tables[0], retireType, ldCalcMonthToday, txtRetirementDate);
                    TerminateElectiveAccountsForDisability(ref dsElectiveAccounts, ldCalcMonthToday, txtRetirementDate);

                    dtEmploymentProjectedSalary = new DataTable();
                    createProjectedSalary(dtEmploymentProjectedSalary, dtRetEstimateEmployment
                                       , lnMaximumContributionSalary, ref warningMessage, retireType, fundeventID, txtRetirementDate, transactionDetails);
                    
                    //START | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment record hence no need to get AverageSalary                   
                    if (HelperFunctions.isNonEmpty(dtEmploymentProjectedSalary))
                    {
                        averageSalary = Convert.ToDecimal(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]); // Required for logging
                    }
                    //END | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment record hence no need to get AverageSalary   

                    getYMCAResolutions(dtAccountsBasisByProjection, l_dsYmcaResolutions);
                    //END: PPP | 03/20/2017 | YRS-AT-2625 | Reversing the changes
                }

                //START: PPP | 03/08/2017 | YRS-AT-2625 | Preserving actual balance which will be used to calculate reduction factor in case of C annuity purchase
                //--In case of active person disability projection, till retirement date projection happens on the basis of Normal factor and should be treated as actual balance
                if (retireType.ToUpper() == "DISABL") 
                    this.actualRetirementBalance = GetOnlyRetirementPlanDetails(dtAccountsBasisByProjection.Copy());
                //END: PPP | 03/08/2017 | YRS-AT-2625 | Preserving actual balance which will be used to calculate reduction factor in case of C annuity purchase
                actualBalanceAtRetirement = dtAccountsBasisByProjection.Copy(); //PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI

                laProjectionPeriods = new string[4, 3];
                SetProjectionPeriodsArray(ref laProjectionPeriods, ldCalcMonthRetire, ldCalcMonthToday, retireType, txtRetireeBirthday, C_EFFDATE, C_TERMDATE, C_PROJECTIONPERIOD);

                ////ASHISH:2011-01-28, If retirement type Disability then make employment termination date empty for allow to projection routine project acct upto age 60
                if (retireType.ToUpper() == "DISABL")
                {
                    SetAge60TerminationDateForDisability(dtEmploymentProjectedSalary, txtRetireeBirthday);
                }

                lnProjectionPeriod = 2;
                lnRETIREMENT_PROJECTION_PHASE = 1;
                ycCalcMonth = Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod]);

                // Check if calcDate is greater than the termination Date.
                _llDone = false;
                _llDone = (ycCalcMonth >= Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)]));

                // Step 10.
                lnRETIREDATE_TO_AGE60 = 0;
                // Now till here we have calculated and fetched the following information
                // 1. dtAccountsBasisByProjection -- All the new account contributions along with existing account contribution
                // 2. dtRetEstimateEmployment -- All the employment details till date
                // 3. dtEmploymentProjectedSalary -- Employment details with modified data.
                // 4. dtAccountsByBasis -- Existing account contributions with YMCA resolution details
                // 5. dsElectiveAccounts -- All the account details -- Existing and user modified (read proposed)

                //START: PPP | 03/01/2017 | YRS-AT-3317 | Adding unfunded and not received amount at this stage attracts current month DLIN also which reduces Annuity and Reserve amount if estimation performed every day
                //So moved GetPendingBalances methode inside "calculateInterestOnContributions" where balance gets added when next month begins
                ////START: PPP | 05/06/2016 | YRS-AT-2683 | Fetching unfunded and not received amount
                //GetPendingBalances(fundeventID, dtAccountsBasisByProjection, Convert.ToDateTime(txtRetirementDate));
                ////END: PPP | 05/06/2016 | YRS-AT-2683 | Fetching unfunded and not received amount
                //END: PPP | 03/01/2017 | YRS-AT-3317 | Adding unfunded and not received amount at this stage attracts current month DLIN also which reduces Annuity and Reserve amount if estimation performed every day

                //2012.05.08  SP:  BT-1032 : Additional comments added for Gemini ID YRS 5.0-1574
                strError = string.Empty;
                calculateInterestOnContributions(ycCalcMonth, ref lnRETIREMENT_PROJECTION_PHASE, ref lnRETIREDATE_TO_AGE60
                    , dtAccountsBasisByProjection, dtRetEstimateEmployment, dtEmploymentProjectedSalary, dtAccountsByBasis
                    , laProjectionPeriods, lnProjectionPeriod, _llDone, fundeventID
                    , projectedInterestRate, retireType, l_numGuaranteedInterestRatePre96
                    , dsElectiveAccounts.Tables[0], txtRetireeBirthday, employmentDetails, ref warningMessage, fundStatus, para_MaxTerminationDate, ref isYmcaLegacyAcctTotalExceed, ref isYmcaAcctTotalExceed, out strError, iAge, bIsPersonTerminated
                    , !isUnsubmittedBalanceConsidered //PPP | 03/16/2017 | YRS-AT-2625 | Indicates function to "consider" pending balance
                    );
                if (!string.IsNullOrEmpty(strError))
                {
                    hasNoErrors = false;
                    //errorMessage = "Numpayperiod is not defined. Unable to proceed further.";//commented on 24-sep for BT-1126
                    errorMessage = "MESSAGE_RETIREMENT_BOC_NUMPAYPERIOD_NOT_DEFINED";
                    goto ReturnError;
                }

                //START: PPP | 03/08/2017 | YRS-AT-2625 | Preserving total balance which includs actual balance + (projected balance through age 60) which will be used to calculate reduction factor in case of C annuity purchase
                if (retireType.ToUpper() == "DISABL")
                    this.totalRetirementBalance = GetOnlyRetirementPlanDetails(dtAccountsBasisByProjection.Copy());
                //END: PPP | 03/08/2017 | YRS-AT-2625 | Preserving total balance which includs actual balance + (projected balance through age 60) which will be used to calculate reduction factor in case of C annuity purchase
                totalBalanceAtRetirement = dtAccountsBasisByProjection.Copy(); //PPP | 03/14/2017 | YRS-AT-2625 | Data will be used to log details in AtsYRSActivityLog from UI

                //Added by Ashish For Phase V part III changes
                //Get AnnuityBasisTypeList from atsMetaAnnuityBasisTypes 
                this.g_dtAnnuityBasisTypeList = RetirementBOClass.GetAnnuityBasisTypeList();

                //'' Check for the $5000 limit as per the Plan Type chosen and the Fund Status.
                if (isEstimate)
                {
                    //Added by Ashish For Phase V changes
                    if (!isEsimateProjBal)
                    {
                        //calculatePlanBalances(dtAccountsBasisByProjection);
                        DataTable dtExAccounts = removeExcludedAccounts(dtAccountsBasisByProjection, dtExcludedAccounts, false);
                        calculatePlanBalances(dtExAccounts);
                        errorMessage = checkAccountBalanceLimits(fundStatus, planType, personID);
                        if (errorMessage != string.Empty)
                        {
                            hasNoErrors = false;
                            goto ReturnError;
                        }

                        if (!isSufficientBalance(personID, fundeventID, fundStatus, txtRetirementDate, planType, ref errorMessage, dtExAccounts))
                        {
                            if (errorMessage != string.Empty)
                            {
                                hasNoErrors = false;
                                goto ReturnError;
                            }
                        }
                    }
                    //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment

                    removeExcludedAccounts(dtAccountsBasisByProjection, dtExcludedAccounts, true);

                    //START: PPP | 03/17/2017 | YRS-AT-2625 | Following part of code will handle the ecluded account removal part
                    if (retireType.ToUpper() == "DISABL" && this.actualRetirementBalance != null && this.totalRetirementBalance != null)
                    {
                        removeExcludedAccounts(this.actualRetirementBalance, dtExcludedAccounts, true);
                        removeExcludedAccounts(this.totalRetirementBalance, dtExcludedAccounts, true);
                    }
                    removeExcludedAccounts(actualBalanceAtRetirement, dtExcludedAccounts, true);
                    removeExcludedAccounts(totalBalanceAtRetirement, dtExcludedAccounts, true);
                    //END: PPP | 03/17/2017 | YRS-AT-2625 | Following part of code will handle the ecluded account removal part
                }

                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                // Step 10
                //calculateFinalAmounts(dtAccountsBasisByProjection, dtExcludedAccounts, lnRETIREMENT_PROJECTION_PHASE, lnRETIREDATE_TO_AGE60, planType);

                combinedDataSet.Tables.Add(dtAccountsBasisByProjection);

                //2012.07.13 SP : BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page -Start
                if (dtEmploymentProjectedSalary != null && dtEmploymentProjectedSalary.Rows.Count > 0)
                {
                    // START: SR | 2018.11.21 | YRS-AT-4106 | revert to original since disability projected salary was not correct after new changes for projected final year salary.
                    projFinalYearSal = Convert.ToDecimal(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]);
                    //projFinalYearSal = ProjFinalYearMonthlySal;
                    // END: SR | 2018.11.21 | YRS-AT-4106 |  revert to original since disability projected salary was not correct after new changes for projected final year salary.
                }
            //2012.07.13 SP :BT-1041/YRS 5.0-1599:Add "Projected Final Year's Salary" to page -End
            ReturnError:
                return hasNoErrors;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return hasNoErrors;
        }

        /// <summary>
        /// Updates the CurrentModified salary for all the employments to the one specified by the user.
        /// For RetProcessing though it sets it to the value specified in the database as numModifiedSalary
        ///This method used for Retirement Esitamte
        /// </summary>
        /// <param name="dtEmploymentProjectedSalary"></param>
        /// <param name="dtRetEstimateEmployment"></param>
        /// <param name="lnMaximumContributionSalary"></param>
        /// <param name="warningMessage"></param>

        //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624, Remove some parameters form function
        //private static void createProjectedSalary(DataTable dtEmploymentProjectedSalary, DataTable dtRetEstimateEmployment,
        //    double lnMaximumContributionSalary, string modifiedSal, bool isEstimate, DataTable employmentDetails, ref string warningMessage)
        private static void createProjectedSalary(DataTable dtEmploymentProjectedSalary, DataTable dtRetEstimateEmployment,
                double lnMaximumContributionSalary, ref string warningMessage, string para_RetireType, string para_fundeventID, string para_RetirementDate, string transactionDetails) // MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format
        {
            DataRow drEmploymentProjectedSalary = null;
            bool l_OnlyOneActiveEmployment = false;
            DataRow[] drRetEstimateEmploymentFound = null;
            decimal lnDisabilityAvgSalary = 0;

            // START : SR | 2018.11.21 | YRS-AT-4106 | commented below variables as it will be no longer in use
            //string empEventID; //PPP | 11/23/2017 | YRS-AT-3319 | Holds employment ID
            //bool isTerminated; //PPP | 11/17/2017 | YRS-AT-3319 | Termination status
            //string futureSalaryDate = null; //SR | 30/07/2018 | YRS-AT-3790, YRS-AT-3811 | get future salary effective date
            // END : SR | 2018.11.21 | YRS-AT-4106 | commented below variables as it will be no longer in use

            try
            {
                if (para_RetireType.ToUpper() == "DISABL")
                {
                    lnDisabilityAvgSalary = GetDisabilityAverageSalary(para_fundeventID, para_RetirementDate, transactionDetails); // MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format
                }
                dtEmploymentProjectedSalary.Columns.Add("guiYmcaID");
                dtEmploymentProjectedSalary.Columns.Add("guiEmpEventID");
                dtEmploymentProjectedSalary.Columns.Add("dtmTerminationDate", typeof(DateTime));
                dtEmploymentProjectedSalary.Columns.Add("CurrentProjectedSalary", typeof(decimal));
                //START: SR | 2018.08.28 | YRS-AT-3790 | Added new column to maintain adjusted salary.
                if (!dtEmploymentProjectedSalary.Columns.Contains("UseSalary"))
                {
                    dtEmploymentProjectedSalary.Columns.Add("UseSalary", typeof(decimal));
                    //Set the Default Value.
                    dtEmploymentProjectedSalary.Columns["UseSalary"].DefaultValue = 0;
                }
                //END: SR | 2018.08.28 | YRS-AT-3790 | Added new column to maintain adjusted salary.

                dtEmploymentProjectedSalary.AcceptChanges();
                if (dtRetEstimateEmployment != null)
                {
                    drRetEstimateEmploymentFound = dtRetEstimateEmployment.Select("dtmTerminationDate IS NULL OR dtmTerminationDate=''");
                    if (drRetEstimateEmploymentFound.Length == 1)
                    {
                        l_OnlyOneActiveEmployment = true;
                    }

                    if (dtRetEstimateEmployment.Rows.Count != 0)
                    {
                        foreach (DataRow dr in dtRetEstimateEmployment.Rows)
                        {
                            drEmploymentProjectedSalary = dtEmploymentProjectedSalary.NewRow();
                            //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624,Start
                            //                    if(isEstimate)
                            //                    {
                            //                        // For all active employments
                            //                        // Update CurrentProjected Salary to the one specified by the user
                            ////						Commented By Anil for YRPS - 4536 for Future Salary End Date Issue. 2008.01.28					
                            ////						if ((dr["numModifiedSalary"] != DBNull.Value && dr["dtmTerminationDate"] == DBNull.Value) 
                            ////							|| (dr["numModifiedSalary"] == DBNull.Value && dr["dtmTerminationDate"] == DBNull.Value))
                            ////						{
                            //                            if (dr["guiYmcaID"] != DBNull.Value)
                            //                                drEmploymentProjectedSalary["guiYmcaID"] = dr["guiYmcaID"].ToString();

                            //                            if (dr["guiUniqueID"] != DBNull.Value)
                            //                                drEmploymentProjectedSalary["guiEmpEventID"] = dr["guiUniqueID"].ToString();
                            //                            if (dr["dtmTerminationDate"] != DBNull.Value)
                            //                                drEmploymentProjectedSalary["dtmTerminationDate"] = dr["dtmTerminationDate"];
                            //                            //drEmploymentProjectedSalary["CurrentProjectedSalary"] = modifiedSal;

                            //                            drEmploymentProjectedSalary["CurrentProjectedSalary"] = RetirementBOClass.getModifiedSalary(modifiedSal, dr, employmentDetails);

                            ////							
                            //                            dtEmploymentProjectedSalary.Rows.Add(drEmploymentProjectedSalary);
                            //                            dtEmploymentProjectedSalary.GetChanges(DataRowState.Added);
                            ////						}
                            //                        //Commented By Anil for YRPS - 4536 for Future Salary End Date Issue. 2008.01.28	
                            //                    }
                            //                    else // If it is estimate just take a side copy dont change a thing.
                            //                    {
                            //                        if (dr["guiYmcaID"] != DBNull.Value)
                            //                            drEmploymentProjectedSalary["guiYmcaID"] = dr["guiYmcaID"].ToString();
                            //                        if (dr["guiUniqueID"] != DBNull.Value)
                            //                            drEmploymentProjectedSalary["guiEmpEventID"] = dr["guiUniqueID"].ToString();
                            //                        if (dr["dtmTerminationDate"] != DBNull.Value)
                            //                            drEmploymentProjectedSalary["dtmTerminationDate"] = dr["dtmTerminationDate"];

                            //                        if (dr["numModifiedSalary"] != DBNull.Value)
                            //                            drEmploymentProjectedSalary["CurrentProjectedSalary"] = dr["numModifiedSalary"];
                            ////						
                            //                        dtEmploymentProjectedSalary.Rows.Add(drEmploymentProjectedSalary);
                            //                        dtEmploymentProjectedSalary.GetChanges(DataRowState.Added);
                            //                    }
                            //'ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624 End
                            if (dr["guiYmcaID"] != DBNull.Value)
                                drEmploymentProjectedSalary["guiYmcaID"] = dr["guiYmcaID"].ToString();
                            if (dr["guiEmpEventId"] != DBNull.Value)
                                drEmploymentProjectedSalary["guiEmpEventID"] = dr["guiEmpEventId"].ToString();
                            if (dr["dtmTerminationDate"].ToString() != string.Empty)
                                drEmploymentProjectedSalary["dtmTerminationDate"] = dr["dtmTerminationDate"];

                            if (para_RetireType.ToUpper() == "NORMAL")
                            {
                                if (dr["AvgSalaryPerEmployment"] != DBNull.Value)
                                {
                                    if (l_OnlyOneActiveEmployment)
                                        drEmploymentProjectedSalary["CurrentProjectedSalary"] = dr["AvgSalary"];
                                    else
                                        drEmploymentProjectedSalary["CurrentProjectedSalary"] = dr["AvgSalaryPerEmployment"];
                                }
                                else
                                    drEmploymentProjectedSalary["CurrentProjectedSalary"] = 0;
                            }
                            else if (para_RetireType.ToUpper() == "DISABL")
                            {
                                drEmploymentProjectedSalary["CurrentProjectedSalary"] = lnDisabilityAvgSalary;
                            }
                            //						
                            dtEmploymentProjectedSalary.Rows.Add(drEmploymentProjectedSalary);
                            dtEmploymentProjectedSalary.GetChanges(DataRowState.Added);
                        }
                        dtEmploymentProjectedSalary.AcceptChanges();
                    }

                    // START : SR | 2018.11.21 | YRS-AT-4106 | since we are using running total method, following lines are commented & moved avaerage salary exceed limit validation in calculateInterestOnContributions() method. Also, commented to remove changes done for YRS-AT-3790 & YRS-AT-3811.
                    //// If modified salary is greater than maxPermitted then reset it.
                    //foreach (DataRow dr in dtEmploymentProjectedSalary.Rows)
                    //{
                    //    //START: PPP | 11/17/2017 | YRS-AT-3319 | If current employment is not active then no need to validate employments high salary limit 
                    //    isTerminated = false;

                    //    empEventID = Convert.IsDBNull(dr["guiEmpEventID"]) ? string.Empty : Convert.ToString(dr["guiEmpEventID"]);
                    //    drRetEstimateEmploymentFound = dtRetEstimateEmployment.Select(string.Format("guiEmpEventID='{0}'", empEventID));
                    //    if (drRetEstimateEmploymentFound != null && drRetEstimateEmploymentFound.Length > 0)
                    //    {
                    //        if (drRetEstimateEmploymentFound.Length == 1)
                    //        {
                    //            if (!Convert.IsDBNull(drRetEstimateEmploymentFound[0]["End"]))
                    //            {
                    //                isTerminated = true;
                    //            }
                    //            //START : SR | 2018.07.30 |  YRS-AT-3790, YRS-AT-3811  | Get future salary effective date t0 identify average salary projection required or not.   
                    //            if (!Convert.IsDBNull(drRetEstimateEmploymentFound[0]["dtmFutureSalaryDate"]))
                    //            {
                    //                futureSalaryDate = drRetEstimateEmploymentFound[0]["dtmFutureSalaryDate"].ToString();
                    //            }
                    //            //END : SR | 2018.07.30 |  YRS-AT-3790, YRS-AT-3811  | Get future salary effective date t0 identify average salary projection required or not.   
                               
                    //        }
                    //    }

                    //    //START : SR | 2018.07.30 |  YRS-AT-3790, YRS-AT-3811  | get number of months for which current Salary will be effective.                      
                    //    int currentSalaryEffectiveMonths = 12;
                    //    // if current year in retirement year / termination year then current salary effective months will be same as retirement month / termination month.
                    //    if (DateTime.Now.Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year)
                    //    {
                    //        if (!(string.IsNullOrEmpty(dr["dtmTerminationDate"].ToString())))
                    //        {
                    //            // If termination date/Retirement date is other than 1st of month then consider that month for projection.
                    //            if (Convert.ToDateTime(dr["dtmTerminationDate"]).Day > 1)
                    //            {
                    //                currentSalaryEffectiveMonths = Convert.ToDateTime(dr["dtmTerminationDate"]).Month;
                    //            }
                    //            else  // If termination date/Retirement date is 1st of month then do not consider that month for projection.
                    //            {
                    //                currentSalaryEffectiveMonths = Convert.ToDateTime(dr["dtmTerminationDate"]).Month - 1;
                    //            }

                    //            // if current salary effective months derived as 0 then it mean retirement date  is 1st of january. hence calculate current salary effective months as default 12 months.
                    //            if (currentSalaryEffectiveMonths == 0)
                    //            {
                    //                currentSalaryEffectiveMonths = 12;
                    //            }
                    //        }
                    //    }

                    //    // SR | 2018.07.30  | YRS-AT-3790, YRS-AT-3811  | if retirement / termination date is 1st of next month then it means no salary projection. Hence, below validation for annual salary limit can be skip.
                    //    if ((Convert.ToDateTime(dr["dtmTerminationDate"]).Year == DateTime.Now.Year)
                    //       && (DateTime.Now.Month == currentSalaryEffectiveMonths))
                    //    {
                    //        continue;
                    //    }
                    //    // SR | 2018.07.30  | YRS-AT-3790, YRS-AT-3811  | if future salary date is 1st of next month then it means no avaerage salary projection. Hence, below validation for annual salary limit can be skip.
                    //    else if (!string.IsNullOrEmpty(futureSalaryDate)
                    //       && (Convert.ToDateTime(futureSalaryDate).Year == DateTime.Now.Year)
                    //       && (Convert.ToDateTime(futureSalaryDate).Month == DateTime.Now.Month + 1))
                    //    {
                    //        continue;
                    //    }
                    //    else
                    //    {
                    //        // SR | YRS-AT-3790, YRS-AT-3811 | instead of assigning 12 months, assign number of months current salary will be effective.
                    //        //if (dr["CurrentProjectedSalary"] != DBNull.Value)
                    //        if (dr["CurrentProjectedSalary"] != DBNull.Value && !isTerminated)
                    //        //END: PPP | 11/17/2017 | YRS-AT-3319 | If current employment is not active then no need to validate employments high salary limit 
                    //        {
                    //            //if (Convert.ToDouble(dr["CurrentProjectedSalary"].ToString()) > lnMaximumContributionSalary / 12)
                    //            if (Convert.ToDouble(dr["CurrentProjectedSalary"].ToString()) > lnMaximumContributionSalary / currentSalaryEffectiveMonths)
                    //            //END : SR | 2018.07.30  | YRS-AT-3790, YRS-AT-3811  | get number of months for which current salary effective months will be effective.
                    //            {
                    //                if (warningMessage.IndexOf("annual salary limit") <= 0)
                    //                    warningMessage += "<br> Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation";
                    //                // START : SR | 2018.07.30 | YRS-AT-3790, YRS-AT-3811 | instead of assigning 12 months, assign number of months current salary will be effective.
                    //                //dr["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
                    //                dr["CurrentProjectedSalary"] = lnMaximumContributionSalary / currentSalaryEffectiveMonths;
                    //                // END : SR | 2018.07.30 | YRS-AT-3790, YRS-AT-3811 | instead of assigning 12 months, assign number of months current salary will be effective.
                    //                dtEmploymentProjectedSalary.GetChanges(DataRowState.Modified);
                    //                dtEmploymentProjectedSalary.AcceptChanges();
                    //            }
                    //        }
                    //    }
                    //}
                    // END : SR | 2018.11.21 | YRS-AT-4106 | since we are using running total method, following lines are commented & moved avaerage salary exceed limit validation in calculateInterestOnContributions() method. Also, commented to remove changes done for YRS-AT-3790 & YRS-AT-3811.
                } //dtRetEstimateEmployment != null
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// This method used for projected balances and interest computation.
        /// This method only used for Retirement Estimate
        /// 2012.05.08    Shashank Patel      BT-1032 : Additional comments added for Gemini ID YRS 5.0-1574
        /// </summary>
        /// <param name="ycCalcMonth"></param>
        /// <param name="lnRETIREMENT_PROJECTION_PHASE"></param>
        /// <param name="lnRETIREDATE_TO_AGE60"></param>
        /// <param name="dtAccountsBasisByProjection"></param>
        /// <param name="dtRetEstimateEmployment"></param>
        /// <param name="dtEmploymentProjectedSalary"></param>
        /// <param name="dtAccountsByBasis"></param>
        /// <param name="laProjectionPeriods"></param>
        /// <param name="lnProjectionPeriod"></param>
        /// <param name="_llDone"></param>
        /// <param name="_blnBasisType"></param>
        /// <param name="personID"></param>
        /// <param name="selectedProjectedInterestRate"></param>
        /// <param name="lcRetireType"></param>
        /// <param name="l_numGuaranteedInterestRatePre96"></param>
        /// <param name="dtRetEstEmpElectives"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="employmentDetails"></param>
        /// <param name="warningMessage"></param>
        /// Added parameter for phase V changes
        /// <param name="para_FundStatus"></param>
        /// <param name="para_strMaxTerminationDate"></param>
        /// <param name="isYmcaLegacyAcctTotalExceed"></param>
        /// <param name="errorMessage"></param>
        //private static void calculateInterestOnContributions(DateTime ycCalcMonth, 
        //    ref int lnRETIREMENT_PROJECTION_PHASE, ref int lnRETIREDATE_TO_AGE60
        //    , DataTable dtAccountsBasisByProjection, DataTable dtRetEstimateEmployment
        //    , DataTable dtEmploymentProjectedSalary, DataTable dtAccountsByBasis
        //    , string[,] laProjectionPeriods, int lnProjectionPeriod, bool _llDone, bool _blnBasisType
        //    , string personID, double selectedProjectedInterestRate, string lcRetireType
        //    , double l_numGuaranteedInterestRatePre96, DataTable dtRetEstEmpElectives
        //    , string retireeBirthday, DataTable employmentDetails, ref string warningMessage
        //    ,string para_FundStatus,string para_strMaxTerminationDate,ref bool isYmcaLegacyAcctTotalExceed,ref bool isYmcaAcctTotalExceed)
        private static void calculateInterestOnContributions(DateTime ycCalcMonth,
            ref int lnRETIREMENT_PROJECTION_PHASE, ref int lnRETIREDATE_TO_AGE60
            , DataTable dtAccountsBasisByProjection, DataTable dtRetEstimateEmployment
            , DataTable dtEmploymentProjectedSalary, DataTable dtAccountsByBasis
            , string[,] laProjectionPeriods, int lnProjectionPeriod, bool _llDone
            , string fundEventID, double selectedProjectedInterestRate, string lcRetireType
            , double l_numGuaranteedInterestRatePre96, DataTable dtRetEstEmpElectives
            , string retireeBirthday, DataTable employmentDetails, ref string warningMessage
            , string para_FundStatus, string para_strMaxTerminationDate, ref bool isYmcaLegacyAcctTotalExceed, ref bool isYmcaAcctTotalExceed
            , out string errorMessage, int iAge = 0, bool bIsPersonTerminated = false
            , bool considerPendingBalance = false //PPP | 03/16/2017 | YRS-AT-2625 | In disability pending balance is being added twice, So if the flag is true then only program will add pending balance to projected balance
            )
        {
            errorMessage = string.Empty;
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Estimates", "calculateInterestOnContributions START");

            #region Get configuration key values
            // Get Contribution limits
            DataSet dsContributionLimits;
            double lnMaximumContributionSalary = 0;
            dsContributionLimits = RetirementBOClass.SearchContributionLimits();
            if (dsContributionLimits.Tables[0].Rows.Count != 0)
            {
                if (dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"] != DBNull.Value)
                {
                    lnMaximumContributionSalary = Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["SalaryMaxAnnual"]);
                }
            }
            else
            {
                //throw (new Exception("Please specify the maximum Contribution Limit for the year, to proceed further."));//commented on 24-sep for BT-1126
                throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_CONTRIBUTION"));
            }
            //Added by Ashish for phase v changes ,Start
            //get threshold limit for Ymac legacy & Ymca  account
            DataTable dtThresholdLimit;
            double maxConfigYmcaAcctThresholdLimit = 0;
            double maxConfigYmacLegacyThresholdLimit = 0;
            //START - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            double dblmaxConfigYmacAccAndLegacyThresholdLimit = 0;
            bool bIsBALegacyCombinedAccountRule = false;
            //END- 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            #region Commented code
            // Commented by START - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            //dtThresholdLimit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_55_ABOVE");
            //if (dtThresholdLimit != null)
            //{
            //    if (dtThresholdLimit.Rows.Count > 0)
            //    {
            //        foreach (DataRow drRow in dtThresholdLimit.Rows)
            //        {
            //            if (drRow["Key"].ToString().Trim().ToUpper() == "BA_MAX_LIMIT_55_ABOVE")
            //            {
            //                maxConfigYmcaAcctThresholdLimit = drRow["Value"].ToString() == string.Empty ? 0 : Convert.ToDouble(drRow["Value"]);
            //            }

            //        }
            //    }
            //    else
            //    {
            //        //throw (new Exception("Please specify the maximum Ymca Acct threshold limit with key 'BA_MAX_LIMIT_55_ABOVE' , to proceed further."));//commented on 24-sep for BT-1126
            //        throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_YMCA_ACCT_THRESHOLD"));
            //    }

            //}
            //else
            //{
            //    //throw (new Exception("Please specify the maximum Ymca Acct threshold limit with key 'BA_MAX_LIMIT_55_ABOVE' , to proceed further."));//commented on 24-sep for BT-1126
            //    throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_YMCA_ACCT_THRESHOLD"));
            //}
            ////Get Ymca Legacy Acct Threshold limit
            //dtThresholdLimit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("REFUND_MAX_PIA");
            //if (dtThresholdLimit != null)
            //{
            //    if (dtThresholdLimit.Rows.Count > 0)
            //    {
            //        foreach (DataRow drRow in dtThresholdLimit.Rows)
            //        {
            //            if (drRow["Key"].ToString().Trim().ToUpper() == "REFUND_MAX_PIA")
            //            {
            //                maxConfigYmacLegacyThresholdLimit = drRow["Value"].ToString() == string.Empty ? 0 : Convert.ToDouble(drRow["Value"]);
            //            }

            //        }
            //    }
            //    else
            //    {
            //        //throw (new Exception("Please specify the maximum Ymca Acct threshold limit under key 'BA_MAX_LIMIT_55_ABOVE' , to proceed further."));//commented on 24-sep for BT-1126
            //        throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_YMCA_ACCT_THRESHOLD"));
            //    }

            //}
            //else
            //{
            //    //throw (new Exception("Please specify the maximum Ymca Acct threshold limit under key 'REFUND_MAX_PIA' , to proceed further."));//commented on 24-sep for BT-1126
            //    throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_YMCA_ACCT_UNDER_KEY"));
            //}
            // Commented by END - 2016.05.27    Chandra sekar.c     YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            #endregion Commented code
            //Added by Ashish for phase v changes ,End
            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment Start
            DataTable dtMetaNormalProjInterestRate;
            double lnMetaNormalProjInterestRate = 0;
            // START | SR | 2017.03.09 | YRS-AT-2625 | Get project interest rate based on Retire type
            //Get all configured data for retirement Estimate calculator
            dtMetaNormalProjInterestRate = YMCARET.YmcaBusinessObject.RefundRequest.GetConfigurationCategoryWise("CALC");
            if (dtMetaNormalProjInterestRate != null && dtMetaNormalProjInterestRate.Rows.Count > 0)
            {
                foreach (DataRow drRow in dtMetaNormalProjInterestRate.Rows)
                {
                    //If retire type is disability then get Disability interest rate 
                    if (lcRetireType == "DISABL" && (drRow["Key"].ToString().Trim().ToUpper() == "EST_DISABLE_PROJECTED_INTEREST_RATE"))
                    {
                        lnMetaNormalProjInterestRate = drRow["Value"].ToString() == string.Empty ? 0 : Convert.ToDouble(drRow["Value"]);
                        break;
                    } //If retire type is not disability then get Normal  interest rate 
                    else if (drRow["Key"].ToString().Trim().ToUpper() == "EST_NORMAL_PROJECTED_INTEREST_RATE")
                    {
                        lnMetaNormalProjInterestRate = drRow["Value"].ToString() == string.Empty ? 0 : Convert.ToDouble(drRow["Value"]);
                        break;
                    }
                }
            }
            else
            {
                throw (new Exception("MESSAGE_RETIREMENT_BOC_NORMAL_INTREST_RATE_UNDER_KEY"));
            }

            //dtMetaNormalProjInterestRate = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("EST_NORMAL_PROJECTED_INTEREST_RATE");
            //if (dtMetaNormalProjInterestRate != null)
            //{
            //    if (dtMetaNormalProjInterestRate.Rows.Count > 0)
            //    {
            //        foreach (DataRow drRow in dtMetaNormalProjInterestRate.Rows)
            //        {
            //            if (drRow["Key"].ToString().Trim().ToUpper() == "EST_NORMAL_PROJECTED_INTEREST_RATE")
            //            {
            //                lnMetaNormalProjInterestRate = drRow["Value"].ToString() == string.Empty ? 0 : Convert.ToDouble(drRow["Value"]);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //throw (new Exception("Please specify the normal interest rate  under key 'EST_NORMAL_PROJECTED_INTEREST_RATE' , to proceed further."));//commented on 24-sep for BT-1126
            //        throw (new Exception("MESSAGE_RETIREMENT_BOC_NORMAL_INTREST_RATE_UNDER_KEY"));
            //    }
            //}
            //else
            //{
            //    //throw (new Exception("Please specify the normal interest rate  under key 'EST_NORMAL_PROJECTED_INTEREST_RATE' , to proceed further."));//commented on 24-sep for BT-1126
            //    throw (new Exception("MESSAGE_RETIREMENT_BOC_NORMAL_INTREST_RATE_UNDER_KEY"));
            //}
            // END | SR | 2017.03.09 | YRS-AT-2625 | Get project interest rate based on Retire type

            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment End
            //Get Combination of YMCA  Account and Ymca Legacy Acct Threshold limit
            // START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            dtThresholdLimit = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(iAge, bIsPersonTerminated);  // Added by : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            if (IsNonEmpty(dtThresholdLimit))
            {
                foreach (DataRow drRow in dtThresholdLimit.Rows)
                {
                    maxConfigYmcaAcctThresholdLimit = string.IsNullOrEmpty(drRow["YmcaAccountLimit"].ToString()) ? 0 : Convert.ToDouble(drRow["YmcaAccountLimit"]);
                    maxConfigYmacLegacyThresholdLimit = string.IsNullOrEmpty(drRow["YmcaLegacyAccountLimit"].ToString()) ? 0 : Convert.ToDouble(drRow["YmcaLegacyAccountLimit"]);
                    dblmaxConfigYmacAccAndLegacyThresholdLimit = string.IsNullOrEmpty(drRow["YmcaCombinedBasicAccountLimit"].ToString()) ? 0 : Convert.ToDouble(drRow["YmcaCombinedBasicAccountLimit"]);
                    bIsBALegacyCombinedAccountRule = string.IsNullOrEmpty(drRow["bitIsBALegacyCombinedRule"].ToString()) ? false : Convert.ToBoolean(drRow["bitIsBALegacyCombinedRule"]);
                }
            }
            else
            {
                throw (new Exception("MESSAGE_RETIREMENT_BOC_MAXIMUM_YMCA_ACCT_AND_YMCA_LEC_ACCT_THRESHOLD"));
            }
            // END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
            #endregion

            // Get the ProjectedInterestRate
            double lnNormalProjectedRate = 0;
            //START: MMR | 2020.05.22 | YRS-AT-4885 | Added for Interest rate project
            //if (selectedProjectedInterestRate != 0)
            if (selectedProjectedInterestRate != -1)
                //END: MMR | 2020.05.22 | YRS-AT-4885 | Added for Interest rate project
                lnNormalProjectedRate = selectedProjectedInterestRate;
            else
                lnNormalProjectedRate = lnMetaNormalProjInterestRate;

            // Get the Year of MaximumInterestRate
            double lnMaxInterestYearMonth = 0;
            DataSet l_dsMetaInterestRates = new DataSet();
            l_dsMetaInterestRates = RetirementBOClass.SearchMetaInterestRates();
            if (l_dsMetaInterestRates.Tables.Count != 0 && l_dsMetaInterestRates.Tables[0].Rows.Count != 0)
                lnMaxInterestYearMonth = (Convert.ToDouble(l_dsMetaInterestRates.Tables[0].Compute("MAX(chrYear)", "")) * 12) + (Convert.ToDouble(l_dsMetaInterestRates.Tables[0].Compute("MAX(chrMonth)", "")) - 1);

            // Check if TD accounts exists
            bool TDAccountExists = false;
            if (dtAccountsByBasis.Columns.Contains("chrAcctType"))
                TDAccountExists = (dtAccountsByBasis.Select("chrAcctType = 'TD'").Length > 0);

            // Get the difference between the retiree birtdate and today. 
            // And set the flag if it is greater than 50
            // And accordingly set his maximum salary contribution
            //bool _ll50 = false;
            double lnContributionMaxAnnualTD = 0;
            double lnContributionMaxAnnual = 0;
            if (dsContributionLimits.Tables[0].Rows.Count != 0)
            {
                DataRow dr = dsContributionLimits.Tables[0].Rows[0];
                if (DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(retireeBirthday), ycCalcMonth) >= 50)
                {
                    //_ll50 = true;
                    lnContributionMaxAnnualTD = Convert.ToDouble(dr["ContributionMaxAnnualTD"]) + Convert.ToDouble(dr["ContributionMaxAnnual50Addl"]);
                    // 2012.03.21  SP :  BT-1020 :intContributionMaxAnnual50Addl value from atsMetacontributionlimits should not consider if the participant do not have/specify TD account for estimate. 
                    if (TDAccountExists)
                        lnContributionMaxAnnual = Convert.ToDouble(dr["ContributionMaxAnnual"]) + Convert.ToDouble(dr["ContributionMaxAnnual50Addl"]);
                    else
                        lnContributionMaxAnnual = Convert.ToDouble(dr["ContributionMaxAnnual"]);

                }
                else
                {
                    //_ll50 = false;
                    lnContributionMaxAnnualTD = Convert.ToDouble(dr["ContributionMaxAnnualTD"]);
                    lnContributionMaxAnnual = Convert.ToDouble(dr["ContributionMaxAnnual"]);
                }
            }
            //Added by Ashish for phase V part III changes
            //Get AnnuityBasisType List
            DataTable dtAnnuityBasisTypeList = null;
            dtAnnuityBasisTypeList = GetAnnuityBasisTypeList();

            string errMsg;
            try
            {
                #region Declare Variables
                DataTable dtUniqueAcctList = null;
                DataTable dtUniqueBasisGroupList = null;
                int ctrPre96 = 0;
                int ctrCalcForward = 0;
                int ctrCalcForwardCalculation = 0;
                int C_PROJECTIONPERIOD = 1;
                int C_EFFDATE = 2;
                int C_TERMDATE = 3;

                string dtsTransactDate;
                string chrProjPeriod;
                int intProjPeriodSequence;

                double lnAnnualTDContributions = 0;
                //bool isFirstTimeInMonthLoop = true; //PPP | 11/24/2017 | YRS-AT-3319 | Variable is declared and assigned but not used
                double lnAnnualContributions = 0;
                double annualCompensation = 0; // SR | 2018.11.21 | YRS-AT-4106 | declare variable to store running annual compensation
                #endregion
                // START : SB | 03/06/2017 | YRS-AT-2625 | Existing function is being replaced by additional parameters for finding out resolution in calculating retire Estimates 
                //  DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(fundeventID);
                DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(fundEventID, Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, laProjectionPeriods.GetUpperBound(1)].ToString()), lcRetireType);
                // END : SB | 03/06/2017 | YRS-AT-2625 | Existing function is being replaced by additional parameters for finding out resolution in calculating retire Estimates 
                DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();

                // START | SR | 2015.12.17 | YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
                //DataSet dsCurrentContribution = new DataSet(); // PPP | 05/18/2016 | YRS-AT-2683 
                //dsCurrentContribution = YMCARET.YmcaDataAccessObject.RetirementDAClass.GetCurrentContributions(fundEventID); // PPP | 05/18/2016 | YRS-AT-2683 | Passing one more parameter to it, so commented the existing call
                string strRetirementDate = laProjectionPeriods[C_TERMDATE, lnProjectionPeriod].ToString(); // PPP | 05/18/2016 | YRS-AT-2683 | Retirement date is required, if it is less than next month then only till retirement date not submitted amount should be calculated
                DataSet dsCurrentContribution = YMCARET.YmcaDataAccessObject.RetirementDAClass.GetCurrentContributions(fundEventID, Convert.ToDateTime(strRetirementDate)); // PPP | 05/18/2016 | YRS-AT-2683 | Passing retirement date, if it is less than current date then till there current years not submitted amount will be calculated
                if (CommonClass.isNonEmpty(dsCurrentContribution))
                {

                    if (!string.IsNullOrEmpty(dsCurrentContribution.Tables[0].Rows[0]["TD_amt"].ToString()))
                    {
                        lnAnnualTDContributions = Convert.ToDouble(dsCurrentContribution.Tables[0].Rows[0]["TD_amt"].ToString());
                    }                    

                    if (!string.IsNullOrEmpty(dsCurrentContribution.Tables[0].Rows[0]["Contribution"].ToString()))
                    {
                        lnAnnualContributions = Convert.ToDouble(dsCurrentContribution.Tables[0].Rows[0]["Contribution"].ToString());
                    }                                      
                    // START :  SR | 2018.11.21 | YRS-AT-4106 | Get compensation for current year & warning message for previous year & current year.
                    if (!string.IsNullOrEmpty(dsCurrentContribution.Tables[0].Rows[0]["Compensation"].ToString()))
                    {
                        annualCompensation = Convert.ToDouble(dsCurrentContribution.Tables[0].Rows[0]["Compensation"].ToString());
                	}
                    if (!string.IsNullOrEmpty(dsCurrentContribution.Tables[0].Rows[0]["WarningMessage"].ToString()))
                    {                        
                        warningMessage = AppendWarningMessage(dsCurrentContribution.Tables[0].Rows[0]["WarningMessage"].ToString(), warningMessage);
                    }
                    // END :  SR | 2018.11.21 | YRS-AT-4106 | Get compensation for current year & warning message for previous year & current year.
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Reported & Unsubmitted compensation/Contribution ", String.Format("TDContributions - {0} BA Contributions - {1} BACompensation - {2}", lnAnnualTDContributions, lnAnnualContributions, annualCompensation));                
                // END | SR | 2015.12.17 | YRS-AT-2252 - Annuity Estimate (Website/YRS) -needs to check the current Annual Limits table 
                //Added by Ashish for phase V changes ,Start
                if (para_strMaxTerminationDate == string.Empty)
                {
                    para_strMaxTerminationDate = laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)].ToString();
                }
                //int l_RitreeAgeOnRetirmentdate;

                // Declare variable for calcule Ymca legacy acct total as on max termination date
                double mnyYmcaLegacyAcctTotal = 0;
                double mnyYmcaAcctTotal = 0;

                double mnyYmcaLegacyAcctThreshold = maxConfigYmacLegacyThresholdLimit;
                double mnyYmcaAcctThreshhold = maxConfigYmcaAcctThresholdLimit;
                //Commented by Ashish for removing QD validation
                //				if(laProjectionPeriods[C_TERMDATE,laProjectionPeriods.GetUpperBound(1)] !=null)
                //				{
                //					l_RitreeAgeOnRetirmentdate=DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(retireeBirthday),Convert.ToDateTime(laProjectionPeriods[C_TERMDATE,laProjectionPeriods.GetUpperBound(1)]));
                //					if(l_RitreeAgeOnRetirmentdate >=55)
                //					{
                //						
                //						mnyYmcaAcctThreshhold=25000;
                //						mnyYmcaLegacyAcctThreshold=25000;
                //						//for testing
                //						//mnyYmcaAcctThreshhold=2500;
                //						
                //						//mnyYmcaLegacyAcctThreshold=1000;
                //					}
                //					else
                //					{
                //						if(para_FundStatus=="QD")
                //						{
                //							mnyYmcaAcctThreshhold=5000;
                //						}
                //						else
                //						{
                //							mnyYmcaAcctThreshhold=25000;
                //							//for testing
                //							//mnyYmcaAcctThreshhold=25000;
                //						}
                //						mnyYmcaLegacyAcctThreshold=25000;
                //						// for testing
                //						//mnyYmcaLegacyAcctThreshold=1000;
                //
                //					}
                //				}

                #region  Threshold check for existing balances
                //Get existing YMCALegacy Acct Balances for treshold check
                DataRow[] drRetEstElectiveAcctFoundRow;
                DataRow[] drAccountsBasisByProjectionRowFound;
                if (dtRetEstEmpElectives != null && dtAccountsBasisByProjection != null)
                {
                    if (dtRetEstEmpElectives.Rows.Count > 0 && dtAccountsBasisByProjection.Rows.Count > 0)
                    {
                        //get YmcaLegacy Account existing balances
                        drRetEstElectiveAcctFoundRow = dtRetEstEmpElectives.Select("bitBasicAcct=True AND PlanType='RETIREMENT' AND bitYA=1");
                        if (drRetEstElectiveAcctFoundRow.Length > 0)
                        {
                            for (int i = 0; i < drRetEstElectiveAcctFoundRow.Length; i++)
                            {

                                drAccountsBasisByProjectionRowFound = dtAccountsBasisByProjection.Select("chrAcctType='" + drRetEstElectiveAcctFoundRow[i]["chrAcctType"] + "'");
                                if (drAccountsBasisByProjectionRowFound.Length > 0)
                                {
                                    mnyYmcaLegacyAcctTotal += Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(YMCAAmt)", "chrAcctType='" + drRetEstElectiveAcctFoundRow[i]["chrAcctType"] + "'"));
                                }

                            }

                        }
                        ////Get existing YMCA Acct Balances for treshold check

                        drRetEstElectiveAcctFoundRow = dtRetEstEmpElectives.Select("bitBasicAcct=True AND PlanType='RETIREMENT' AND bitEP=1");
                        if (drRetEstElectiveAcctFoundRow.Length > 0)
                        {
                            for (int i = 0; i < drRetEstElectiveAcctFoundRow.Length; i++)
                            {

                                drAccountsBasisByProjectionRowFound = dtAccountsBasisByProjection.Select("chrAcctType='" + drRetEstElectiveAcctFoundRow[i]["chrAcctType"] + "'");
                                if (drAccountsBasisByProjectionRowFound.Length > 0)
                                {
                                    mnyYmcaAcctTotal += Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(YMCAAmt)", "chrAcctType='" + drRetEstElectiveAcctFoundRow[i]["chrAcctType"] + "'"));
                                }
                            }

                        }


                    } // dtRetEstEmpElectives.Row.Count
                } // dtRetEstEmpElectives!=null
                if (para_FundStatus != "QD")
                {
                    //START: PPP | 12/28/2017 | YRS-AT-3328 | Applying partial withdrawal rules 
                    double ymcaLegacyAtTermination = 0;
                    if (mnyYmcaLegacyAcctTotal > 0)
                    {
                        // Participant has balance in YMCA legacy account
                        ymcaLegacyAtTermination = GetYmcaLegacyAtTerminationBalance(fundEventID);
                        if (ymcaLegacyAtTermination == 0) // If participant is active then GetYmcaLegacyAtTerminationBalance will return 0 balance
                        {
                            // Participant is active so for comparison consider current YMCA Legacy account balance 
                            ymcaLegacyAtTermination = mnyYmcaLegacyAcctTotal;
                        }
                    }
                    if (bIsBALegacyCombinedAccountRule)
                    {
                        // If withdrawal rule is ON then check total of "YMCA Legacy Balance As On Today" + "YMCA Account Balance" against combined balance rule amount
                        if ((mnyYmcaLegacyAcctTotal + mnyYmcaAcctTotal) > dblmaxConfigYmacAccAndLegacyThresholdLimit)
                    {
                            // if combined balnce balance is below defined amount then compare "YMCA Legacy Balance At TERMINATION" against defined ymca legacy balance
                            if (ymcaLegacyAtTermination > mnyYmcaLegacyAcctThreshold)
                            {
                                isYmcaLegacyAcctTotalExceed = true;
                    }

                            if (mnyYmcaAcctTotal > mnyYmcaAcctThreshhold)
                    {
                                isYmcaAcctTotalExceed = true;
                            }
                    }
                        else
                        {
                            // Can exclude Ymca or Ymca Legacy account
                }
                    }
                    else
                    {
                        if (ymcaLegacyAtTermination > mnyYmcaLegacyAcctThreshold)
                        {
                            isYmcaLegacyAcctTotalExceed = true;
                        }

                        if (mnyYmcaAcctTotal > mnyYmcaAcctThreshhold)
                        {
                            isYmcaAcctTotalExceed = true;
                        }
                    }

                    //// START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                    ////if existing YMCA Legacy account cross threshold limit then set true 
                    //// Indivdual Account Validation OR  Combined rules validation 
                    //// If YMCA Legacy Account > 25000 OR  (YMCA Account + YMCA Legacy Account) > $50000 
                    //if (mnyYmcaLegacyAcctTotal > mnyYmcaLegacyAcctThreshold)
                    //{
                    //    isYmcaLegacyAcctTotalExceed = true;
                    //}
                    ////if existing YMCA  account cross threshold limit hen set true 
                    //// Indivdual Account Validation OR  Combined rules validation 
                    //// If YMCA Account > 25000  OR  (YMCA Account + YMCA Legacy Account) > $50000 OR [(YMCA Account + YMCA Legacy Account) > and  YMCA Legacy Account at termination greater than $25000]
                    //if (mnyYmcaAcctTotal > mnyYmcaAcctThreshhold) 
                    //{
                    //    isYmcaAcctTotalExceed = true;
                    //}
                    //// END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                    //END: PPP | 12/28/2017 | YRS-AT-3328 | Applying partial withdrawal rules 
                }
                //Added by Ashish for phase V changes ,End
                #endregion

                //Added by Ashish for phase V part III changes, start
                //Get Unique Acct List from dtAccountsByBasis
                dtUniqueAcctList = GetUniqueAcctList(dtAccountsByBasis);
                //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                //if (lcRetireType.ToUpper() == "DISABL")
                //{
                //    if (dtUniqueAcctList != null && l_dsMetaAccountTypes != null)
                //    {
                //        for (int i = 0; i < dtUniqueAcctList.Rows.Count; i++)
                //        {
                //            if (l_dsMetaAccountTypes.Tables[0].Select("chrAcctType='" + dtUniqueAcctList.Rows[i]["chrAcctType"].ToString() + "'").Length == 0)
                //            {
                //                dtUniqueAcctList.Rows.RemoveAt(i);  
                //            }
                //        }
                //    }
                //}
                //Get unique basis group list from dtAnnuityBasisTypeList
                dtUniqueBasisGroupList = GetUniqueAnnuityBasisGroupList(dtAnnuityBasisTypeList);
                //Added by Ashish for phase V part III changes, End
                //START - Chandra sekar.c   2016.04.21    YRS-AT-2612 & YRS-AT-2891 - YRS enh: Annuity Estimate Calculator should include 15+ yrs of service catchup
                DataTable dtTDWarningMessages = CreateTDWarningMessageSchema();  // 2016.04.21| Chandra sekar.c | YRS-AT-2612 & YRS-AT-2891 | For displaying validation message for the Annual TD Contribution Limits
                Dictionary<int, string> diTDContributionLimits = new Dictionary<int, string> { };
                // Getting Participant Months of service, TDContribution limit,50+ TDContribution Limit and also TD Amount for Current Year.
                DataSet dsMaxTDContributionAllowedPerYear = YMCARET.YmcaDataAccessObject.RetirementDAClass.GetCurrentTDServiceCatchup(fundEventID, Convert.ToDateTime(strRetirementDate));
                //END - Chandra sekar.c   2016.04.21    YRS-AT-2612 & YRS-AT-2891 - YRS enh: Annuity Estimate Calculator should include 15+ yrs of service catchup
                // If CurrentProjectedSalary is not set then set it to 0 so that calculations dont fail for DBNull
                foreach (DataRow dr in dtEmploymentProjectedSalary.Rows)
                    if (dr["CurrentProjectedSalary"] == DBNull.Value)
                        dr["CurrentProjectedSalary"] = 0;

                //YMCA Phase IV - Part 2 - added by hafiz on 4-Jun-2008 
                int _noOfDaysInMonth = 0;
                DateTime dt;
                bool isUnReceivedAndUnfundedBalanceAdded = false; //PPP | 03/01/2017 | YRS-AT-3317 | This flag will indiacte whether "unfunded and not received" amount added to projection or not
                string endWorkDate = string.Empty; //PPP | 03/01/2017 | YRS-AT-3317 | To hold end work date. In case active employment it may conatin user passed end date and if terminated then terminate date
                string cutOffDateToGetUnreceivedAndPendingBalance = string.Empty; //PPP | 04/26/2017 | YRS-AT-3419 | Represents date, till we have to fetch unreceived and unfunded balance
                while (_llDone == false)
                {
                    //START: PPP | 03/01/2017 | YRS-AT-3317 | Adding unfunded and not received amount after current month is passed, So that interest will get added to it
                    //START: PPP | 03/16/2017 | YRS-AT-2625 | This same function is being called twice if the estimation is happening for Disability Retirement
                    //--It projects balance based on "Normal" retirement till future retirement date provided on screen
                    //--And projects balance from retirement date through age 60 based on "Disability" retirement
                    //--So in "Normal" projection period for disability "considerPendingBalance" flag will be"False"
                    //--And in "Disabiliy" projection period for disability "considerPendingBalance" flag will be "True"
                    //if (!isUnReceivedAndUnfundedBalanceAdded)
                    if (!isUnReceivedAndUnfundedBalanceAdded && considerPendingBalance)
                    //END: PPP | 03/16/2017 | YRS-AT-2625 | This same function is being called twice if the estimation is happening for Disability Retirement
                    {
                        if (ycCalcMonth.Year == DateTime.Now.Year && ycCalcMonth.Month == DateTime.Now.Month)
                        {
                            // Projection is running for current month so do not add "not received" and "unfunded" balances
                        }
                        else
                        {
                            // START | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment details hence end work date will not be available
                            if (HelperFunctions.isNonEmpty(employmentDetails))
                            {
                                //if (employmentDetails != null)
                                endWorkDate = Convert.IsDBNull(employmentDetails.Rows[0]["EndWorkDate"]) ? string.Empty : Convert.ToString(employmentDetails.Rows[0]["EndWorkDate"]);
                            }
                            // END | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment details hence end work date will not be available
                            
                            //START: PPP | 04/26/2017 | YRS-AT-3419 
                            // In disability code always try to fetch unreceived and unfunded balances in a Normal Projection Cycle
                            // but if the retirement date is current month start date then Normal Projection Cycle does not get called
                            // So code reach this area and try to fetch unreceived and unfunded balance 
                            // In this case strRetirementDate contains date when participant is attaining age 60
                            // impact of this is system then fetches unwanted months unreceived and unfunded balance 
                            // For e.g. Current date is 04/xx/2017, Retirement Date on screen is 4/1/2017 and participant is reaching age 60 by 6/1/2017
                            // So code passes 6/1/2017 as a retirement date and gets unreceived and unfunded balances till 5/1/2017 (because 5/1/2017 is next month start date, for more info check SP:yrs_usp_Ret_Est_GetPendingBalances)
                            // To avoid fetching balances till 5/1/2017, we have to pass 4/1/2017 to metod instead of 6/1/2017
                            // So replacing strRetirementDate with cutOffDateToGetUnreceivedAndPendingBalance which will get passed to the method
                            // and its value will be determined based on retirement type
                            //GetPendingBalances(fundEventID, dtAccountsBasisByProjection, Convert.ToDateTime(strRetirementDate), string.IsNullOrEmpty(endWorkDate) ? null : (DateTime?)Convert.ToDateTime(endWorkDate));
                            if (lcRetireType == "DISABL")
                                cutOffDateToGetUnreceivedAndPendingBalance = laProjectionPeriods[C_EFFDATE, lnProjectionPeriod].ToString();
                            else
                                cutOffDateToGetUnreceivedAndPendingBalance = strRetirementDate;

                            GetPendingBalances(fundEventID, dtAccountsBasisByProjection, Convert.ToDateTime(cutOffDateToGetUnreceivedAndPendingBalance), string.IsNullOrEmpty(endWorkDate) ? null : (DateTime?)Convert.ToDateTime(endWorkDate));
                            //END: PPP | 04/26/2017 | YRS-AT-3419 

                            isUnReceivedAndUnfundedBalanceAdded = true;
                        }
                    }
                    //END: PPP | 03/01/2017 | YRS-AT-3317 | Adding unfunded and not received amount after current month is passed, So that interest will get added to it

                    dtsTransactDate = ycCalcMonth.ToString();

                    //YMCA Phase IV - Part 2 - added by hafiz on 4-Jun-2008 
                    _noOfDaysInMonth = DateTime.DaysInMonth(ycCalcMonth.Year, ycCalcMonth.Month);

                    chrProjPeriod = laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod];
                    intProjPeriodSequence = lnProjectionPeriod;

                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("calculateInterestOnContributions", String.Format("Person FundEventId - {0} Retirement Type - {1} Iteration Date - {2}", fundEventID, lcRetireType, dtsTransactDate));
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace(String.Format("Retirement Type - {0} Iteration Date - {1}", lcRetireType, dtsTransactDate),"");

                    if (chrProjPeriod != "CALCDATE_FORWARD")
                        ctrPre96++;

                    // Set the correct CurrentProjectedSalary 
                    // (Future salary becomes the CurrentProjectedSalary if the calculation date equals the FutureEffective date)
                    // Set projected current salary cursor on the right employment event (for updating later)
                    if (chrProjPeriod == "CALCDATE_FORWARD")
                    {
                        ctrCalcForward++;
                        // START :  SR | 2018.11.21 | YRS-AT-4106 | calling common procedure for average as well as future salary projection.
                        SetFutureSalaryAndSalaryIncrease(dtRetEstimateEmployment, dtEmploymentProjectedSalary, ycCalcMonth, annualCompensation, lnMaximumContributionSalary, ref warningMessage);
                        #region Commented old code of Set Future Salary and SalaryIncrease
                        //START : SR | 2018.08.28 |  YRS-AT-3790 | since we are using running total method, following lines are commented to count number of months & to Remove changes done for YRS-AT-3790 & YRS-AT-3811.
                        //foreach (DataRow dr in dtRetEstimateEmployment.Rows)
                        //{
                        //    string guiEmpEventIDSal = dr["guiEmpEventId"].ToString();
                        //    foreach (DataRow drPrjSal in dtEmploymentProjectedSalary.Rows)
                        //    {
                        //        if (drPrjSal["guiEmpEventID"].ToString() == guiEmpEventIDSal)
                        //        {
                        //            //START : SR | 2018.07.26 |  YRS-AT-3790, YRS-AT-3811  | get number of months for which future Salary Or Annual Salary Percentage will be effective.
                        //            //NOTE : similar changes should be implemented in YRS Retirement estimate webservices
                        //            int futureSalaryOrAnnualSalaryPercentageEffectiveMonths = 12;
                        //            // if current year in retirement year / termination year then annualSalaryPercentageEffectiveMonths will be same as retirement month / termination month.
                        //            if (ycCalcMonth.Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year)
                        //            {
                        //                if (!(string.IsNullOrEmpty(dr["dtmTerminationDate"].ToString())))
                        //                {
                        //                    // If termination date/Retirement date is other than 1st of month then consider that month for projection.
                        //                    if (Convert.ToDateTime(dr["dtmTerminationDate"]).Day > 1)
                        //                    {
                        //                        futureSalaryOrAnnualSalaryPercentageEffectiveMonths = Convert.ToDateTime(dr["dtmTerminationDate"]).Month;
                        //                    }
                        //                    else  // If termination date/Retirement date is 1st of month then do not consider that month for projection.
                        //                    {
                        //                        futureSalaryOrAnnualSalaryPercentageEffectiveMonths = Convert.ToDateTime(dr["dtmTerminationDate"]).Month - 1;
                        //                    }

                        //                    // if annualSalaryPercentageEffectiveMonths derived as 0 then it mean retirement date  is 1st of january. hence calculate futureSalaryEffectiveMonths as 12 months.
                        //                    if (futureSalaryOrAnnualSalaryPercentageEffectiveMonths == 0)
                        //                    {
                        //                        futureSalaryOrAnnualSalaryPercentageEffectiveMonths = 12;
                        //                    }
                        //                }
                        //            }
                        //            //END : SR | 2018.07.26  | YRS-AT-3790, YRS-AT-3811  | get number of months for which future Salary Or Annual Salary Percentage will be effective.

                        //            if (Convert.ToDecimal(dr["numFutureSalary"]) != 0 && dr["dtmFutureSalaryDate"].ToString() != string.Empty)
                        //            {
                        //                //commented for YRS 5.0-445
                        //                //if (ycCalcMonth.Month == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Month && 
                        //                //ycCalcMonth.Year == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Year) 

                        //                //added for YRS 5.0-445
                        //                if (ycCalcMonth.Month >= Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Month &&
                        //                    ycCalcMonth.Year >= Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Year
                        //                    && ycCalcMonth < Convert.ToDateTime(dr["dtmTerminationDate"]) // SR | 2018.07.26 | YRS-AT-3790, YRS-AT-3811 | Salary Projection should not happen for dates after termination date. Its already handled for annual Salary percentage increment.
                        //                    )
                        //                {
                        //                    //Commented by Ashish on 23-jul-2009 for Issue YRS 5.0-835 ,commented else part ,allow both salary increment type,Start	
                        //                    //											if (Convert.ToDouble(dr["numFutureSalary"]) >= lnMaximumContributionSalary / 12) 
                        //                    //											{
                        //                    //												drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
                        //                    //												if (warningMessage.IndexOf("annual salary limit") <= 0)
                        //                    //													warningMessage += "<br> Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation";
                        //                    //											}
                        //                    //											else 
                        //                    //												drPrjSal["CurrentProjectedSalary"] = Convert.ToDecimal(dr["numFutureSalary"]);
                        //                    //Commented by Ashish on 23-jul-2009 for Issue YRS 5.0-835 ,commented else part ,allow both salary increment type,End

                        //                    //Added by Ashish on 23-jul-2009 for Issue YRS 5.0-835,Start
                        //                    if (ycCalcMonth.Month == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Month &&
                        //                        ycCalcMonth.Year == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Year)
                        //                    {
                        //                        drPrjSal["CurrentProjectedSalary"] = Convert.ToDecimal(dr["numFutureSalary"]);
                        //                    }

                        //                    // START : SR | 2018.07.24 | YRS-AT-3790, YRS-AT-3811 | instead of assigning 12 months, assign number of months future salary will be effective.
                        //                    //NOTE : similar changes should be implemented in YRS Retirement estimate webservices
                        //                    //if (Convert.ToDouble(drPrjSal["CurrentProjectedSalary"]) >= lnMaximumContributionSalary / 12)
                        //                    if (Convert.ToDouble(drPrjSal["CurrentProjectedSalary"]) >= lnMaximumContributionSalary / futureSalaryOrAnnualSalaryPercentageEffectiveMonths)  // use derived futureSalaryEffectiveMonths instead of 12 months
                        //                    {
                        //                        //drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
                        //                        drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / futureSalaryOrAnnualSalaryPercentageEffectiveMonths;
                        //                        // END : SR | 2018.07.24 | YRS-AT-3790, YRS-AT-3811  | instead of assigning 12 months, assign number of months future salary will be effective.
                        //                        if (warningMessage.IndexOf("annual salary limit") <= 0)
                        //                        {
                        //                            warningMessage += "<br> Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation";
                        //                            LoggerBO.LogMessage("Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation for " + ycCalcMonth, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which  Maximum annual salary limit crossed
                        //                        }
                        //                    }

                        //                    //Added by Ashish on 23-jul-2009 for Issue YRS 5.0-835,End 
                        //                }
                        //            }

                        //            //Added by Ashish on 23-jul-2009 for Issue YRS 5.0-835 ,Start
                        //            // Do the annual increase every 12 months keeping calculation(today) month as the start month
                        //            //Determine if % Salary increase is warranted
                        //            string annualSalaryIncreaseEffDate = dr["dtmAnnualSalaryIncreaseEffDate"].ToString();
                        //            string annualPctgIncrease = dr["numAnnualPctgIncrease"].ToString();

                        //            if (annualSalaryIncreaseEffDate != string.Empty && annualPctgIncrease != string.Empty)//Phase IV Changes
                        //            {
                        //                // if ((ycCalcMonth.Month == DateTime.Now.Month && dr["numAnnualPctgIncrease"] != DBNull.Value)
                        //                //	&& (Convert.ToDouble(dr["numAnnualPctgIncrease"]) != 0))
                        //                //if (ycCalcMonth.Month == DateTime.Parse(annualSalaryIncreaseEffDate).Month
                        //                //    && Convert.ToDouble(annualPctgIncrease) != 0)//Phase IV Changes
                        //                if (ycCalcMonth.Month == DateTime.Parse(annualSalaryIncreaseEffDate).Month
                        //                    && Convert.ToDouble(annualPctgIncrease) != 0)//Phase IV Changes
                        //                //SR:2012-07.30 : BT-1041/YRS 5.0-1599: if end date is provided then calculate the projected salary till end date.
                        //                {
                        //                    if ((ycCalcMonth < Convert.ToDateTime(dr["dtmTerminationDate"])) ||
                        //                            (ycCalcMonth.Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year &&
                        //                                ycCalcMonth.Month == Convert.ToDateTime(dr["dtmTerminationDate"]).Month &&
                        //                                    Convert.ToDateTime(dr["dtmTerminationDate"]).Day > 1))
                        //                    {
                        //                        if (
                        //                            !(DateTime.Parse(annualSalaryIncreaseEffDate).Month == Convert.ToDateTime(dr["dtmTerminationDate"]).Month &&
                        //                                DateTime.Parse(annualSalaryIncreaseEffDate).Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year)
                        //                            )
                        //                        //End: SR:2012-07.30 : BT-1041/YRS 5.0-1599: if end date is provided then calculate the projected salary till end date.
                        //                        {
                        //                            // START : SR | 2018.07.24 | YRS-AT-3790, YRS-AT-3811  | instead of assigning 12 months, assign number of months annual salary percentage will be effective.
                        //                            // Apply number of months annual percentage will be effective.
                        //                            if (Convert.ToDouble(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDouble(dr["numAnnualPctgIncrease"]) / 100)) >= lnMaximumContributionSalary / futureSalaryOrAnnualSalaryPercentageEffectiveMonths)
                        //                            //if (Convert.ToDouble(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDouble(dr["numAnnualPctgIncrease"]) / 100)) >= lnMaximumContributionSalary / 12)
                        //                            {
                        //                                //drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
                        //                                drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / futureSalaryOrAnnualSalaryPercentageEffectiveMonths; // use derived annualSalaryPercentageEffectiveMonths instead of 12 months
                        //                                // END : SR | 2018.07.24 |YRS-AT-3790, YRS-AT-3811 | instead of assigning 12 months, assign number of months annual salary percentage will be effective.                                                        
                        //                                if (warningMessage.IndexOf("annual salary limit") <= 0)
                        //                                {
                        //                                    warningMessage += "<br> Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation";
                        //                                    LoggerBO.LogMessage("Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation for " + ycCalcMonth, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which  Maximum annual salary limit crossed
                        //                                }
                        //                            }
                        //                            else
                        //                                drPrjSal["CurrentProjectedSalary"] = Convert.ToDecimal(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDecimal(dr["numAnnualPctgIncrease"]) / 100));
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            //Added by Ashish on 23-jul-2009 for Issue YRS 5.0-835 ,End

                        //            // Set Salary at Maximum possible salary
                        //            // If future salary is given then check if it is the effective date is same as this month
                        //            // Then set that as the CurrentProjectedSalary
                        //            //ASHISH:2010.10.11 code commented for YRS 5.0-855 BT-624
                        //            //if (dr["numFutureSalary"] != DBNull.Value && dr["dtmFutureSalaryDate"] != DBNull.Value) 
                        //            //{

                        //            // Else check if the annual PctgIncrease is specified.
                        //            // Do the annual increase every 12 months keeping calculation(today) month as the start month
                        //            //Commented by Ashish on 23-jul-2009 for Issue YRS 5.0-835 ,commented else part ,allow both salary increment type ,Start
                        //            //									else 
                        //            //									{
                        //            //										//Determine if % Salary increase is warranted
                        //            //										string annualSalaryIncreaseEffDate = dr["dtmAnnualSalaryIncreaseEffDate"].ToString();
                        //            //										string annualPctgIncrease = dr["numAnnualPctgIncrease"].ToString();
                        //            //										if (annualSalaryIncreaseEffDate != string.Empty && annualPctgIncrease != string.Empty)//Phase IV Changes
                        //            //										{												
                        //            //											// if ((ycCalcMonth.Month == DateTime.Now.Month && dr["numAnnualPctgIncrease"] != DBNull.Value)
                        //            //											//	&& (Convert.ToDouble(dr["numAnnualPctgIncrease"]) != 0))
                        //            //											if (ycCalcMonth.Month == DateTime.Parse(annualSalaryIncreaseEffDate).Month 
                        //            //												&& Convert.ToDouble(annualPctgIncrease) != 0)//Phase IV Changes
                        //            //											{
                        //            //												if (Convert.ToDouble(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDouble(dr["numAnnualPctgIncrease"]) / 100)) >= lnMaximumContributionSalary / 12) 
                        //            //												{
                        //            //													drPrjSal["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
                        //            //													if (warningMessage.IndexOf("annual salary limit") <= 0)
                        //            //														warningMessage += "<br> Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation";
                        //            //												}
                        //            //												else 
                        //            //													drPrjSal["CurrentProjectedSalary"] = Convert.ToDecimal(drPrjSal["CurrentProjectedSalary"]) * (1 + (Convert.ToDecimal(dr["numAnnualPctgIncrease"]) / 100));
                        //            //											}
                        //            //										}
                        //            //									}
                        //            //Commented by Ashish on 23-jul-2009 for Issue YRS 5.0-835 ,commented else part ,allow both salary increment type ,End
                        //            dtEmploymentProjectedSalary.GetChanges(DataRowState.Modified);
                        //            dtEmploymentProjectedSalary.AcceptChanges();
                        //        }
                        //    }
                        //}//foreach (DataRow dr in dtRetEstimateEmployment.Rows) 
                        //END : SR | 2018.08.28 |  YRS-AT-3790 | since we are using running total method, following lines are commented to count number of months & to Remove changes done for YRS-AT-3790 & YRS-AT-3811.

                        #endregion
                    }//if (chrProjPeriod == "CALCDATE_FORWARD") 
                    // This is where we add contributions and interest
                    // Prevent entry if we are in the last projection period and 
                    // this month = the termination period of that period
                    //if (!((lnProjectionPeriod == laProjectionPeriods.GetUpperBound(1) 
                    //    && ycCalcMonth == Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod]))) 
                    //    && ycCalcMonth >= Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod]) 
                    //    && ycCalcMonth <= Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod]) 
                    //    && chrProjPeriod == "CALCDATE_FORWARD") 
                    //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment                    
                    if (!((lnProjectionPeriod == laProjectionPeriods.GetUpperBound(1)
                        && ycCalcMonth == Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod])))
                        && ycCalcMonth >= Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod])
                        && ycCalcMonth <= Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod])
                        && (chrProjPeriod == "CALCDATE_FORWARD" || chrProjPeriod == "RETIREDATE_TO_AGE60"))
                    {
                        ctrCalcForwardCalculation++;
                        // Process each account for this projection month
                        //if (_blnBasisType) // Should this flag be here in place. coz now we are retiring only on Savings as well
                        #region Foreach loop for UniqueAcct Row
                        //Commented by Ashish for phase V Part III changes						
                        //						foreach (DataRow drActBasis in dtAccountsByBasis.Rows)
                        //Added by Ashish for phase V Part III changes	
                        foreach (DataRow drActBasis in dtUniqueAcctList.Rows)
                        {
                            string planType = drActBasis["chvPlanType"].ToString();
                            if (planType == string.Empty)
                            {
                                planType = "RETIREMENT";
                            }
                            //Commented by Ashish for phase V Part III changes	
                            //string chrAnnuityBasisType = drActBasis["chrAnnuityBasisType"].ToString().ToUpper().Trim();
                            //string chrAnnuityBasisType="";
                            string chrAcctType = drActBasis["chrAcctType"].ToString().ToUpper().Trim();
                            // START- Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                            string chrEffectiveDate = string.Empty;
                            string chrAdjustmentBasisCode = string.Empty;
                            string strContributionType = string.Empty;
                            string chrTerminateDate = string.Empty;
                            // END Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                            // Exit this for loop if this is a new money since we are not in a new money projection period
                            //Commented by Ashish for phase V Part III changes	
                            //							if (chrAnnuityBasisType == "PST96" && 
                            //								(chrProjPeriod == "PRE96_FORWARD" || chrProjPeriod == "RETIREDATE_TO_AGE60")) 
                            //Added by Ashish for phase V Part III changes
                            //ASHISH:2011-01-28 commented for BT- 665 Disability Retirment
                            //if (chrProjPeriod == "PRE96_FORWARD" || chrProjPeriod == "RETIREDATE_TO_AGE60") 
                            //{
                            //    isFirstTimeInMonthLoop = false;
                            //} 
                            //else 
                            //{
                            //Determine if this account is basic or not
                            bool _llBasicAcct = false;
                            if (l_dsMetaAccountTypes.Tables[0].Rows.Count != 0)
                            {
                                if (l_dsMetaAccountTypes.Tables[0].Select("chrAcctType='" + chrAcctType + "'").Length != 0)
                                    _llBasicAcct = true;
                                else
                                    _llBasicAcct = false;
                            }

                            //Exit this account-basis loop if this is a 
                            //    1. Non - Basic account during
                            //    2. the RETIREDATE_TO_AGE60 projection period
                            if (_llBasicAcct == false && chrProjPeriod.Trim().ToUpper() == "RETIREDATE_TO_AGE60")
                            {
                                //isFirstTimeInMonthLoop = false; //PPP | 11/24/2017 | YRS-AT-3319 | Variable is declared and assigned but not used
                            }

                            else
                            {
                                double mnyPersonalPreTax = 0;
                                //double mnyPersonalPostTax = 0;
                                double mnyYMCAPreTax = 0;
                                double mnyYMCAInterestBalance = 0;
                                double mnyPersonalInterestBalance = 0;
                                double mnyYMCAContribBalance = 0;
                                double mnyPersonalContribBalance = 0;
                                double mnyTempRunningLegacyAcctTotal = 0;
                                double mnyTempRunningYmcaAcctTotal = 0;
                                string l_EffectiveAnnuityBasisType = string.Empty;
                                string l_EffectiveAnnuityBasisGroup = string.Empty;
                                double dblTDMonthlyContrib = 0; //Chandra sekar.c | 2016.04.21 | YRS-AT-2612 & YRS-AT2891
                                string strTDServiceCatchupLimitMgs = string.Empty;
                                #region Interest Calculations
                                // Interest Calculations									
                                #region Get Interest rates
                                // Get the applicable interest rates
                                double lnKnownInterestRate = 0;
                                DataSet l_dsKnownInterestRate = RetirementBOClass.SearchKnownInterestRate(chrAcctType, ycCalcMonth.Year.ToString(), ycCalcMonth.Month.ToString());

                                // This IF had been modified
                                if (l_dsKnownInterestRate.Tables.Count != 0 && l_dsKnownInterestRate.Tables[0].Rows.Count != 0)
                                    lnKnownInterestRate = Convert.ToDouble(l_dsKnownInterestRate.Tables[0].Rows[0]["numInterestRate"]);
                                else
                                    //lnKnownInterestRate = selectedProjectedInterestRate;
                                    lnKnownInterestRate = selectedProjectedInterestRate == -1 ? 0 : selectedProjectedInterestRate; // MMR | 05/22/2020 | YRS-AT-4885 | Check if valid selected Interest rate
                                //Commented by Ashish for phase V Part III changes ,Start
                                //									double lnInterestRate = 0;
                                //									double lnInterestRateNewMoney = 0;
                                //									double lnInterestRateDifferential = 0;
                                //									
                                //									// Old Money determine rate for Pre96 balances and differential
                                //									if (chrAnnuityBasisType == "PRE96") 
                                //									{
                                //										if (chrProjPeriod.Trim().ToUpper() == "CALCDATE_FORWARD") 
                                //										{
                                //											//figure out new money interest rate first so we can compute
                                //											if (lcRetireType == "NORMAL") 
                                //											{
                                //												if (Convert.ToDouble(ycCalcMonth.Year) * 12 + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1) 
                                //													lnInterestRateNewMoney = lnNormalProjectedRate;
                                //												else 
                                //													// if lnKnownInterestRate is null (Should never happen) then assign new interest money to 0
                                //													lnInterestRateNewMoney = lnKnownInterestRate;
                                //													
                                //												// If pst96 known or projected rate is higher than pre96 guaranteed rate, capture the difference
                                //												// then apply it (later) to pre96 balances but credit the result to pst96 balances
                                //												lnInterestRate = l_numGuaranteedInterestRatePre96;
                                //												if (lnInterestRateNewMoney > lnInterestRate) 
                                //													lnInterestRateDifferential = lnInterestRateNewMoney - lnInterestRate;
                                //												else 
                                //													lnInterestRateDifferential = 0;
                                //											} 
                                //											else  // Disabled
                                //											{
                                //												lnInterestRateDifferential = 0;
                                //												// Disability \ Known rate exists and is greater than guaranteed then use the known rate for old balances
                                //												if (Convert.ToDouble(ycCalcMonth.Year) * 12 + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1 & lnKnownInterestRate > l_numGuaranteedInterestRatePre96) 
                                //													lnInterestRate = lnKnownInterestRate;
                                //												else 
                                //													lnInterestRate = l_numGuaranteedInterestRatePre96;
                                //											}
                                //										} 
                                //										else 
                                //										{
                                //											lnInterestRate = l_numGuaranteedInterestRatePre96;
                                //											lnInterestRateDifferential = 0;
                                //										}
                                //									} 
                                //									else if (chrAnnuityBasisType == "PST96")  //New Money
                                //									{
                                //										if ((Convert.ToDouble(ycCalcMonth.Year) * 12) + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1) 
                                //											lnInterestRate = lnNormalProjectedRate;
                                //										else 
                                //											lnInterestRate = lnKnownInterestRate;
                                //									} 
                                //									else if (chrAnnuityBasisType == "ROLL") //ROLLINS are treated same as new money
                                //									{
                                //										if ((Convert.ToDouble(ycCalcMonth.Year) * 12) + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1) 
                                //											lnInterestRate = lnNormalProjectedRate;
                                //										else 
                                //											lnInterestRate = lnKnownInterestRate;
                                //									}
                                //Commented by Ashish for phase V Part III changes, End
                                // -- End of Interest rate calculation
                                #endregion

                                #region loop for dtUniqueBasisGroupList
                                foreach (DataRow drAnnuityBasisGroup in dtUniqueBasisGroupList.Rows)
                                {
                                    string l_AnnuityBasisTypeGroup = string.Empty;
                                    l_AnnuityBasisTypeGroup = drAnnuityBasisGroup["chrAnnuityBasisGroup"].ToString().Trim().ToUpper();
                                    #region Get applicable Interest Rates
                                    double lnInterestRate = 0;
                                    double lnInterestRateNewMoney = 0;
                                    double lnInterestRateDifferential = 0;
                                    // Old Money determine rate for Pre96 balances and differential
                                    if (l_AnnuityBasisTypeGroup == "PRE")
                                    {
                                        //if (chrProjPeriod.Trim().ToUpper() == "CALCDATE_FORWARD") 
                                        //{
                                        //figure out new money interest rate first so we can compute
                                        if (lcRetireType == "NORMAL")
                                        {
                                            if (Convert.ToDouble(ycCalcMonth.Year) * 12 + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1)
                                                lnInterestRateNewMoney = lnNormalProjectedRate;
                                            else
                                                // if lnKnownInterestRate is null (Should never happen) then assign new interest money to 0
                                                lnInterestRateNewMoney = lnKnownInterestRate;

                                            // If pst96 known or projected rate is higher than pre96 guaranteed rate, capture the difference
                                            // then apply it (later) to pre96 balances but credit the result to pst96 balances
                                            lnInterestRate = l_numGuaranteedInterestRatePre96;
                                            if (lnInterestRateNewMoney > lnInterestRate)
                                                lnInterestRateDifferential = lnInterestRateNewMoney - lnInterestRate;
                                            else
                                                lnInterestRateDifferential = 0;
                                        }
                                        else  // Disabled
                                        {
                                            lnInterestRateDifferential = 0;
                                            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                                            //// Disability \ Known rate exists and is greater than guaranteed then use the known rate for old balances
                                            //if (Convert.ToDouble(ycCalcMonth.Year) * 12 + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1 & lnKnownInterestRate > l_numGuaranteedInterestRatePre96) 
                                            //    lnInterestRate = lnKnownInterestRate;
                                            //else 
                                            //    lnInterestRate = l_numGuaranteedInterestRatePre96;

                                            //START: PPP | 05/30/2017 | YRS-AT-2625 | For Disability key is "EST_DISABLE_PROJECTED_INTEREST_RATE"
                                            //--lnNormalProjectedRate variable holds "EST_DISABLE_PROJECTED_INTEREST_RATE" value
                                            //--but if user has selected different rate from UI dropdown then it contains new selected interest rate.
                                            
                                            //lnInterestRate = lnMetaNormalProjInterestRate;
                                            lnInterestRate = lnNormalProjectedRate;
                                            //END: PPP | 05/30/2017 | YRS-AT-2625 | For Disability key is "EST_DISABLE_PROJECTED_INTEREST_RATE"
                                        }
                                        //} 
                                        ////else 
                                        ////{
                                        ////    lnInterestRate = l_numGuaranteedInterestRatePre96;
                                        ////    lnInterestRateDifferential = 0;
                                        ////}
                                        //else if(chrProjPeriod.Trim().ToUpper() == "RETIREDATE_TO_AGE60")
                                        //{
                                        //    lnInterestRate = lnMetaNormalProjInterestRate ;
                                        //    lnInterestRateDifferential = 0;
                                        //}
                                    }
                                    else if (drAnnuityBasisGroup["chrAnnuityBasisGroup"].ToString().Trim().ToUpper() == "PST"
                                            || drAnnuityBasisGroup["chrAnnuityBasisGroup"].ToString().Trim().ToUpper() == "ROL")  //New Money
                                    {
                                        if (lcRetireType == "NORMAL")
                                        {
                                            if ((Convert.ToDouble(ycCalcMonth.Year) * 12) + Convert.ToDouble(ycCalcMonth.Month) > lnMaxInterestYearMonth + 1)
                                                lnInterestRate = lnNormalProjectedRate;
                                            else
                                                lnInterestRate = lnKnownInterestRate;
                                        }
                                        else if (lcRetireType == "DISABL")
                                        {
                                            //START: PPP | 05/30/2017 | YRS-AT-2625 | For Disability key is "EST_DISABLE_PROJECTED_INTEREST_RATE"
                                            //--lnNormalProjectedRate variable holds "EST_DISABLE_PROJECTED_INTEREST_RATE" value
                                            //--but if user has selected different rate from UI dropdown then it contains new selected interest rate.

                                            ////use interest rate which is defined in key EST_NORMAL_PROJECTED_INTEREST_RATE
                                            //lnInterestRate = lnMetaNormalProjInterestRate;
                                            lnInterestRate = lnNormalProjectedRate;
                                            //END: PPP | 05/30/2017 | YRS-AT-2625 | For Disability key is "EST_DISABLE_PROJECTED_INTEREST_RATE"
                                        }
                                    }

                                    #endregion

                                    // Get account balances to date									
                                    double[] laAccountsByBasisProjectionToDate = new double[2];
                                    laAccountsByBasisProjectionToDate[0] = 0;
                                    laAccountsByBasisProjectionToDate[1] = 0;


                                    // Removed CALCFORWARD condition from If Loop to compute interest on Additional Contributions Anil YRPS 4536 2008.01.28									
                                    //Commented by Ashish for phase V Part III changes, Start 
                                    //										if (dtAccountsBasisByProjection.Rows.Count != 0
                                    //											&& dtAccountsBasisByProjection.Select("chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisType='" + chrAnnuityBasisType + "' AND dtsTransactDate <>'" + dtsTransactDate + "'").Length > 0) 
                                    //											// Removed CALCFORWARD condition to compute interest on Additional Contributions Anil YRPS 4536 2008.01.28
                                    //										{
                                    //											laAccountsByBasisProjectionToDate[0] = Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisType='" + chrAnnuityBasisType + "' AND NOT(dtsTransactDate = '" + dtsTransactDate + "' AND chrProjPeriod = 'CALCDATE_FORWARD')"));
                                    //											laAccountsByBasisProjectionToDate[1] = Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisType='" + chrAnnuityBasisType + "' AND NOT(dtsTransactDate = '" + dtsTransactDate + "' AND chrProjPeriod = 'CALCDATE_FORWARD')"));
                                    //										}
                                    //Commented by Ashish for phase V Part III changes, End
                                    //Added by Ashish for phase V Part III changes,Start
                                    if (dtAccountsBasisByProjection.Rows.Count != 0
                                        && dtAccountsBasisByProjection.Select("chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisGroup='" + l_AnnuityBasisTypeGroup + "'").Length > 0)
                                    // Removed CALCFORWARD condition to compute interest on Additional Contributions Anil YRPS 4536 2008.01.28
                                    {
                                        laAccountsByBasisProjectionToDate[0] = Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisGroup='" + l_AnnuityBasisTypeGroup + "'"));
                                        laAccountsByBasisProjectionToDate[1] = Convert.ToDouble(dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "chrAcctType='" + chrAcctType + "' AND chrAnnuityBasisGroup='" + l_AnnuityBasisTypeGroup + "'"));
                                    }
                                    //Added by Ashish for phase V Part III changes,End

                                    //Start - YMCA Phase IV - Part 2 - commented by Hafiz on 4-Jun-2008
                                    //mnyPersonalInterestBalance = (laAccountsByBasisProjectionToDate[0] * lnInterestRate) / 100 / 12;
                                    //mnyYMCAInterestBalance = (laAccountsByBasisProjectionToDate[1] * lnInterestRate) / 100 / 12;
                                    //End - YMCA Phase IV - Part 2 - commented by Hafiz on 4-Jun-2008

                                    //Start - YMCA Phase IV - Part 2 - added on 4-Jun-2008 by Hafiz 											
                                    //17-Sep-2008 - considering the number of days of the month for which daily interest is not generated.
                                    if (lcRetireType == "NORMAL")
                                        if (ycCalcMonth.Year == DateTime.Now.Year && ycCalcMonth.Month == DateTime.Now.Month)
                                        {
                                            //getting the last date when the daily interest was generated.
                                            dt = RetirementBOClass.getDailyInterestLog();

                                            //getting the number of days between the last date on which the daily interest was generated and the last day of current month.
                                            _noOfDaysInMonth = DateAndTime.DateDiffNew(DateIntervalNew.Day, dt, Convert.ToDateTime(DateTime.Now.Date.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Date.Year, DateTime.Now.Date.Month).ToString() + "/" + DateTime.Now.Date.Year.ToString()));
                                        }
                                    //17-Sep-2008

                                    mnyPersonalInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[0], lnInterestRate, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
                                    mnyYMCAInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[1], lnInterestRate, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
                                    //End - YMCA Phase IV - Part 2 - added on 4-Jun-2008 by Hafiz 	 

                                    //Added by Ashish for phase V Part III changes ,Start

                                    mnyPersonalPreTax += mnyPersonalInterestBalance;
                                    mnyYMCAPreTax += mnyYMCAInterestBalance;
                                    mnyTempRunningLegacyAcctTotal = mnyYMCAPreTax;
                                    mnyTempRunningYmcaAcctTotal = mnyYMCAPreTax;
                                    //Added by Ashish for phase V Part III changes, End

                                    //Commented by Ashish for phase V Part III changes, Start
                                    //Prepare variables for inserting into the account projection table
                                    //										mnyPersonalPreTax = mnyPersonalInterestBalance + mnyPersonalContribBalance;
                                    //										mnyYMCAPreTax = mnyYMCAInterestBalance + mnyYMCAContribBalance;
                                    //										mnyPersonalPostTax = 0;
                                    //										double YMCAAmt = mnyYMCAPreTax;
                                    //										double PersonalAmt = mnyPersonalPreTax + mnyPersonalPostTax;
                                    //										double mnyBalance = YMCAAmt + PersonalAmt;

                                    //										//Added by Ashish for phase V changes ,start
                                    //										
                                    //										mnyTempRunningLegacyAcctTotal=YMCAAmt;
                                    //										mnyTempRunningYmcaAcctTotal=YMCAAmt;
                                    //Added by Ashish for phase V changes ,End

                                    //Commented by Ashish for phase V Part III changes, End
                                    //Added by Ashish for phase V Part III changes, start

                                    l_EffectiveAnnuityBasisType = string.Empty;
                                    l_EffectiveAnnuityBasisGroup = string.Empty;
                                    DataRow drAnnuityFindRow = null;
                                    drAnnuityFindRow = GetEffectiveAnnuityBasisType(dtAnnuityBasisTypeList, l_AnnuityBasisTypeGroup, ycCalcMonth);
                                    if (drAnnuityFindRow != null)
                                    {

                                        l_EffectiveAnnuityBasisType = drAnnuityFindRow["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                                        l_EffectiveAnnuityBasisGroup = drAnnuityFindRow["chrAnnuityBasisGroup"].ToString().Trim().ToUpper();

                                    }

                                    AddedInterestAmountInAcctProjection(ref dtAccountsBasisByProjection, chrAcctType, l_EffectiveAnnuityBasisGroup, l_EffectiveAnnuityBasisType, mnyPersonalInterestBalance, mnyYMCAInterestBalance, planType);
                                    //Added by Ashish for phase V Part III changes ,End

                                    #region Commented by Ashish for phase V Part III changes
                                    //Commented by Ashish for phase V Part III changes,Start
                                    //										// Update ongoing account balance total
                                    //										// Insert projected account growth for this account into dtAccountsByBasisProjection
                                    //										if (!(chrProjPeriod.Trim().ToUpper() == "PRE96_FORWARD")) 
                                    //										{
                                    //											DataRow drAccountsBasisByProjection = dtAccountsBasisByProjection.NewRow();
                                    //										
                                    //											drAccountsBasisByProjection["dtsTransactDate"] = dtsTransactDate;
                                    //											drAccountsBasisByProjection["chrProjPeriod"] = chrProjPeriod;
                                    //
                                    //											drAccountsBasisByProjection["chrAcctType"] = chrAcctType;
                                    //											drAccountsBasisByProjection["chrAnnuityBasisType"] =chrAnnuityBasisType;
                                    //											drAccountsBasisByProjection["mnyYMCAContribBalance"] = mnyYMCAContribBalance;
                                    //											drAccountsBasisByProjection["mnyPersonalContribBalance"] = mnyPersonalContribBalance;
                                    //											drAccountsBasisByProjection["mnyPersonalInterestBalance"] = mnyPersonalInterestBalance;
                                    //											drAccountsBasisByProjection["mnyYMCAInterestBalance"] = mnyYMCAInterestBalance;
                                    //											drAccountsBasisByProjection["mnyPersonalPreTax"] = mnyPersonalPreTax;
                                    //											drAccountsBasisByProjection["mnyYMCAPreTax"] = mnyYMCAPreTax;
                                    //											drAccountsBasisByProjection["mnyPersonalPostTax"] = mnyPersonalPostTax;
                                    //											drAccountsBasisByProjection["YMCAAmt"] = YMCAAmt;
                                    //											drAccountsBasisByProjection["PersonalAmt"] = PersonalAmt;
                                    //											drAccountsBasisByProjection["mnyBalance"] = mnyBalance;
                                    //										
                                    //											drAccountsBasisByProjection["intProjPeriodSequence"] = intProjPeriodSequence;
                                    //
                                    //											drAccountsBasisByProjection["chvPlanType"] = planType;
                                    //											dtAccountsBasisByProjection.Rows.Add(drAccountsBasisByProjection);
                                    //											dtAccountsBasisByProjection.GetChanges(DataRowState.Added);
                                    //										
                                    //										
                                    //										}

                                    //ASHISH:TEST,START
                                    //										if (lcRetireType == "NORMAL" & chrProjPeriod.Trim().ToUpper() == "CALCDATE_FORWARD" & chrAnnuityBasisType.Trim().ToUpper() == "PRE96" & !(lnInterestRateDifferential == 0)) 
                                    //										{
                                    //											// Credit interest over the pre96 guaranteed rate on old money to new money balances
                                    //
                                    //											// The following conditions have been met, so credit the interest differential
                                    //											//  1. Normal retirement
                                    //											//  2. We are currently processing old money
                                    //											//  3. We are in the projection period between today and the retirement date
                                    //											//  4. The rate being applied to new money for this account type and projection month
                                    //											//		is higher than the corresponding rate being applied to old money 
                                    //                                                
                                    //											// Reinitialize vars that will change w/ this transaction row
                                    //											chrAnnuityBasisType = "PST96";
                                    //											mnyYMCAContribBalance = 0;
                                    //											mnyPersonalContribBalance = 0;
                                    //											//Apply interest differential (pst96 rate - pre96 rate) to old money (pre96)
                                    //											//Start - YMCA Phase IV - Part 2 - commented by hafiz on 4-Jun-2008
                                    //											//mnyPersonalInterestBalance = (laAccountsByBasisProjectionToDate[0] * lnInterestRateDifferential) / 100 / 12;
                                    //											//mnyYMCAInterestBalance = (laAccountsByBasisProjectionToDate[1] * lnInterestRateDifferential) / 100 / 12;
                                    //											//End - YMCA Phase IV - Part 2 - commented by hafiz on 4-Jun-2008
                                    //
                                    //											//Start - YMCA Phase IV - Part 2 - added by hafiz on 4-Jun-2008
                                    //											mnyPersonalInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[0], lnInterestRateDifferential, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
                                    //											mnyYMCAInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[1], lnInterestRateDifferential, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
                                    //											//End - YMCA Phase IV - Part 2 - added by hafiz on 4-Jun-2008
                                    //
                                    //											//'Sum categories
                                    //											mnyPersonalPreTax = mnyPersonalInterestBalance + mnyPersonalContribBalance;
                                    //											mnyYMCAPreTax = mnyYMCAInterestBalance + mnyYMCAContribBalance;
                                    //											mnyPersonalPostTax = 0;
                                    //											YMCAAmt = mnyYMCAPreTax;
                                    //											PersonalAmt = mnyPersonalPreTax + mnyPersonalPostTax;
                                    //											mnyBalance = YMCAAmt + PersonalAmt;
                                    //											//Added by Ashish for V changes ,Start
                                    //											mnyTempRunningLegacyAcctTotal+=YMCAAmt;
                                    //											mnyTempRunningYmcaAcctTotal+=YMCAAmt;
                                    //											//Added by Ashish for V changes ,End
                                    //											//Update ongoing account balance total
                                    //											// Insert projected account growth for this account into dtAccountsBasisByProjection
                                    //											DataRow drAccountsBasisByProjection = dtAccountsBasisByProjection.NewRow();
                                    //										
                                    //											drAccountsBasisByProjection["dtsTransactDate"] = dtsTransactDate;
                                    //											drAccountsBasisByProjection["chrProjPeriod"] = chrProjPeriod;
                                    //
                                    //											drAccountsBasisByProjection["chrAcctType"] = chrAcctType;
                                    //											drAccountsBasisByProjection["chrAnnuityBasisType"] = chrAnnuityBasisType;
                                    //											drAccountsBasisByProjection["mnyYMCAContribBalance"] = mnyYMCAContribBalance;
                                    //											drAccountsBasisByProjection["mnyPersonalContribBalance"] = mnyPersonalContribBalance;
                                    //											drAccountsBasisByProjection["mnyPersonalInterestBalance"] = mnyPersonalInterestBalance;
                                    //											drAccountsBasisByProjection["mnyYMCAInterestBalance"] = mnyYMCAInterestBalance;
                                    //											drAccountsBasisByProjection["mnyPersonalPreTax"] = mnyPersonalPreTax;
                                    //											drAccountsBasisByProjection["mnyYMCAPreTax"] = mnyYMCAPreTax;
                                    //											drAccountsBasisByProjection["mnyPersonalPostTax"] = mnyPersonalPostTax;
                                    //											drAccountsBasisByProjection["YMCAAmt"] = YMCAAmt;
                                    //											drAccountsBasisByProjection["PersonalAmt"] = PersonalAmt;
                                    //											drAccountsBasisByProjection["mnyBalance"] = mnyBalance;
                                    //										
                                    //											drAccountsBasisByProjection["intProjPeriodSequence"] = intProjPeriodSequence;
                                    //										
                                    //											drAccountsBasisByProjection["chvPlanType"] = planType;
                                    //											dtAccountsBasisByProjection.Rows.Add(drAccountsBasisByProjection);
                                    //											dtAccountsBasisByProjection.GetChanges(DataRowState.Added);
                                    //										}
                                    //Commented by Ashish for phase V Part III changes,End
                                    #endregion
                                    if (lcRetireType == "NORMAL" & chrProjPeriod.Trim().ToUpper() == "CALCDATE_FORWARD" & l_AnnuityBasisTypeGroup.Trim().ToUpper() == "PRE" & !(lnInterestRateDifferential == 0))
                                    {
                                        // Credit interest over the pre96 guaranteed rate on old money to new money balances

                                        // The following conditions have been met, so credit the interest differential
                                        //  1. Normal retirement
                                        //  2. We are currently processing old money
                                        //  3. We are in the projection period between today and the retirement date
                                        //  4. The rate being applied to new money for this account type and projection month
                                        //		is higher than the corresponding rate being applied to old money 
                                        mnyPersonalInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[0], lnInterestRateDifferential, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
                                        mnyYMCAInterestBalance = CalculateCompoundInterest(laAccountsByBasisProjectionToDate[1], lnInterestRateDifferential, _noOfDaysInMonth, Convert.ToInt16(ycCalcMonth.Year));
                                        drAnnuityFindRow = GetEffectiveAnnuityBasisType(dtAnnuityBasisTypeList, "PST", ycCalcMonth);
                                        if (drAnnuityFindRow != null)
                                        {
                                            l_EffectiveAnnuityBasisType = drAnnuityFindRow["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                                            l_EffectiveAnnuityBasisGroup = drAnnuityFindRow["chrAnnuityBasisGroup"].ToString().Trim().ToUpper();
                                        }
                                        AddedInterestAmountInAcctProjection(ref dtAccountsBasisByProjection, chrAcctType, l_EffectiveAnnuityBasisGroup, l_EffectiveAnnuityBasisType, mnyPersonalInterestBalance, mnyYMCAInterestBalance, planType);
                                        mnyPersonalPreTax += mnyPersonalInterestBalance;
                                        mnyYMCAPreTax += mnyYMCAInterestBalance;
                                        mnyTempRunningLegacyAcctTotal = mnyYMCAPreTax;
                                        mnyTempRunningYmcaAcctTotal = mnyYMCAPreTax;

                                    }
                                #endregion
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Calculated Interest", String.Format("{0} - {1} Personal - {2} YMCA - {3}", chrAcctType, l_AnnuityBasisTypeGroup, mnyPersonalInterestBalance, mnyYMCAInterestBalance));                
                                }//foreach(DataRow drAnnuityBasisGroup in dtUniqueBasisGroupList.Rows)
                                #endregion

                                #region  Account Contibution Calculation
                                // If we are between calculate date and retirement date, assess account contributions for new money
                                //Commented by Ashish for phase V Part III changes
                                //if (chrProjPeriod == "CALCDATE_FORWARD" && chrAnnuityBasisType == "PST96" ) 
                                //Added by Ashish for phase V Part III changes
                                //if (chrProjPeriod == "CALCDATE_FORWARD" ) 
                                //{
                                //	Contributions
                                //  New Money
                                //  Determine Contributions for this account according to each active employment event
                                mnyYMCAContribBalance = 0;
                                mnyPersonalContribBalance = 0;
                                double m_CurrentProjectedSalary = 0;
                               
                                //added by Ashish 02-Jul-2009 ,Issue YRS 5.0-801
                                bool l_bool_IsContributed = false;
                                #region Rer Employment contibution for loop
                                for (int j = 0; j <= dtRetEstimateEmployment.Rows.Count - 1; j++)
                                {
                                    DataRow drEstEmp = dtRetEstimateEmployment.Rows[j];
                                    // Commented By Anil for YRPS - 4536 2008.01.28
                                    //											if (drEstEmp["dtmTerminationDate"] == DBNull.Value) // Only for active employments
                                    //											{

                                    string guiEmpEventID = drEstEmp["guiEmpEventId"].ToString().Trim().ToUpper();
                                    string guiYMCAID = drEstEmp["guiYMCAID"].ToString();

                                    //START: PPP | 03/14/2017 | YRS-AT-2625 | In case of disability emp YMCA ID may not match with resolution ymca id, because we fetch highest resolution
                                    if (lcRetireType == "DISABL")
                                    {
                                        if (l_dsYmcaResolutions.Tables[0].Rows.Count != 0 &&
                                           l_dsYmcaResolutions.Tables[0].Select("chrBasicAcctType='" + chrAcctType + "' AND guiYmcaID='" + guiYMCAID + "'").Length == 0)
                                        {
                                            // In disability retirement highest resolution YMCA will come and table will have only one record
                                            guiYMCAID = l_dsYmcaResolutions.Tables[0].Rows[0]["guiYmcaID"].ToString();
                                        }
                                    }
                                    //END: PPP | 03/14/2017 | YRS-AT-2625 | In case of disability emp YMCA ID may not match with resolution ymca id, because we fetch highest resolution

                                    //shashank 2012.02.02 BT-602/YRS 5.0-1130 : Change in how voluntary accounts are projected
                                    //double numPayPeriod = 0.0; //2012.05.08    SP :  BT-1032 : commented to hanle numpayperiod only in monthly payments
                                    //numPayPeriod = Convert.ToDouble(drEstEmp["NumPayPeriod"]);

                                    //added by Ashish 02-Jul-2009 ,Issue YRS 5.0-801
                                    l_bool_IsContributed = false;
                                    // Get the current projected salary
                                    if (dtEmploymentProjectedSalary.Select("guiEmpEventID='" + guiEmpEventID + "'").Length > 0)
                                    {
                                        DataRow drEmploymentProjectedSalary = dtEmploymentProjectedSalary.NewRow();
                                        drEmploymentProjectedSalary = dtEmploymentProjectedSalary.Select("guiEmpEventID='" + guiEmpEventID + "'")[0];
                                        //Commentd by Ashish 02-Jul-2009 , for Isssue YRS 5.0-801 , if termination date is greater than 1st day of month then contibution comes for termination month   
                                        // Added If loop for validating FutuRe Salary End Date by Anil YRPS 4536  2008.01.28
                                        //												if ((drEmploymentProjectedSalary["dtmTerminationDate"] == DBNull.Value) || (ycCalcMonth <= Convert.ToDateTime(Convert.ToDateTime(drEmploymentProjectedSalary["dtmTerminationDate"]).Month + "/1/" + Convert.ToDateTime(drEmploymentProjectedSalary["dtmTerminationDate"]).Year)))
                                        //Added by Ashish 02-Jul-2009 , for Isssue YRS 5.0-801 

                                        if ((drEmploymentProjectedSalary["dtmTerminationDate"] == DBNull.Value) || drEmploymentProjectedSalary["dtmTerminationDate"].ToString() == string.Empty ||

                                                (ycCalcMonth < Convert.ToDateTime(drEmploymentProjectedSalary["dtmTerminationDate"])) ||
                                                (ycCalcMonth.Year == Convert.ToDateTime(drEmploymentProjectedSalary["dtmTerminationDate"]).Year && ycCalcMonth.Month == Convert.ToDateTime(drEmploymentProjectedSalary["dtmTerminationDate"]).Month && Convert.ToDateTime(drEmploymentProjectedSalary["dtmTerminationDate"]).Day > 1))
                                        {
                                            //Added by Ashish 03-Aug-2009 for Issue YRS 5.0-801
                                            if (!(ycCalcMonth.Month == DateTime.Now.Month && ycCalcMonth.Year == DateTime.Now.Year) && lcRetireType != "DISABL") // SR | 2018.12.12 | YRS-AT-4106 | Added retirement type to handle Disability projection.
                                            {
                                                //START: PPP | 08/29/2018 | current projected salary available in use salary column.
                                                //m_CurrentProjectedSalary = Convert.ToDouble(drEmploymentProjectedSalary["CurrentProjectedSalary"]);//UseSalary
                                                m_CurrentProjectedSalary = Convert.ToDouble(drEmploymentProjectedSalary["UseSalary"]);
                                                //END: PPP | 08/29/2018 | current projected salary available in use salary column.
                                                //Added by Ashish 02-Jul-2009 , for Isssue YRS 5.0-801
                                                l_bool_IsContributed = true;
                                            }
                                            //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment
                                            else if (lcRetireType == "DISABL")
                                            {
                                                m_CurrentProjectedSalary = Convert.ToDouble(drEmploymentProjectedSalary["CurrentProjectedSalary"]);
                                                //Added by Ashish 02-Jul-2009 , for Isssue YRS 5.0-801
                                                l_bool_IsContributed = true;
                                            }
                                        }
                                        else
                                            m_CurrentProjectedSalary = 0;
                                        // Added If loop for validating Futue Salary End Date by Anil YRPS 4536  2008.01.28
                                    }


                                    // Get the YMCA and Member percentage contribution
                                    // Get contribution percentages for member and YMCA
                                    // (do this every month regardless of other options)
                                    double lnMemberPct = 0;
                                    double lnYMCAPct = 0;
                                    //ASHISH:2011.03.09, Commented this part of code, assume that effective resolution always defind in current effective account, 
                                    //so no need to check account is effective or not
                                    //Added by Ashish for Phase V changes ,check (chrAcctType) account type is terminated or not
                                    //bool l_boolAcctTerminated = false;

                                    //if (dtRetEstEmpElectives != null)
                                    //{
                                    //    if (dtRetEstEmpElectives.Rows.Count > 0)
                                    //    {
                                    //        DataRow[] drRetEstAcctFoundRow = dtRetEstEmpElectives.Select("bitBasicAcct=True AND chrAcctType='" + chrAcctType + "'");
                                    //        if (drRetEstAcctFoundRow.Length > 0)
                                    //        {
                                    //            if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drRetEstAcctFoundRow[0]["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drRetEstAcctFoundRow[0]["dtmEffDate"]).Year) && drRetEstAcctFoundRow[0]["dtsTerminationDate"].ToString().Trim() == string.Empty)
                                    //            {
                                    //                l_boolAcctTerminated = true;
                                    //            }
                                    //            else if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drRetEstAcctFoundRow[0]["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drRetEstAcctFoundRow[0]["dtmEffDate"]).Year) && ycCalcMonth <= Convert.ToDateTime(drRetEstAcctFoundRow[0]["dtsTerminationDate"]))
                                    //            {
                                    //                l_boolAcctTerminated = true;
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(personID);
                                    //Added by Ashish for phase V changes ,added if condition for checking chrAcctType account type is terminated or not
                                    //ASHISH:2011.03.09, Commented if (l_boolAcctTerminated)
                                    //if (l_boolAcctTerminated)
                                    //{
                                    if (l_dsYmcaResolutions.Tables[0].Rows.Count != 0 &&
                                        l_dsYmcaResolutions.Tables[0].Select("chrBasicAcctType='" + chrAcctType + "' AND guiYmcaID='" + guiYMCAID + "'").Length > 0)
                                    {
                                        //Commented by Ashish 03-Aug-2009 for Issue YRS 5.0-801
                                        //DataRow drYmcaRes = l_dsYmcaResolutions.Tables[0].Rows[0];
                                        //Added by Ashish 03-Aug-2009 for Issue YRS 5.0-801
                                        DataRow drYmcaRes = l_dsYmcaResolutions.Tables[0].Select("chrBasicAcctType='" + chrAcctType + "' AND guiYmcaID='" + guiYMCAID + "'")[0];
                                        if (drYmcaRes["numYmcaComboPctg"] == DBNull.Value)
                                            lnMemberPct = Convert.ToDouble(drYmcaRes["numConstituentPctg"]);
                                        else if (drYmcaRes["numConstituentPctg"] == DBNull.Value)
                                            lnMemberPct = Convert.ToDouble(drYmcaRes["numYmcaComboPctg"]);
                                        else
                                            lnMemberPct = Convert.ToDouble(drYmcaRes["numConstituentPctg"]) + Convert.ToDouble(drYmcaRes["numYmcaComboPctg"]);

                                        if (drYmcaRes["numYmcaPctg"] == DBNull.Value)
                                            lnYMCAPct = Convert.ToDouble(drYmcaRes["numAddlMarginPctg"]);
                                        else if (drYmcaRes["numAddlMarginPctg"] == DBNull.Value)
                                            lnYMCAPct = Convert.ToDouble(drYmcaRes["numYmcaPctg"]);
                                        else
                                            lnYMCAPct = Convert.ToDouble(drYmcaRes["numYmcaPctg"]) + Convert.ToDouble(drYmcaRes["numAddlMarginPctg"]);

                                        annualCompensation += m_CurrentProjectedSalary;  // SR | 2018.08.28 | YRS-AT-3790 | Incrementing annual compensation
                                    }
                                    else
                                    {
                                        lnMemberPct = 0;
                                        lnYMCAPct = 0;
                                    }
                                    //}
                                    //else
                                    //{
                                    //    lnMemberPct = 0;
                                    //    lnYMCAPct = 0;
                                    //}


                                    // From the percentage get the actual contribution
                                    double lnMemberContrib = m_CurrentProjectedSalary * (lnMemberPct / 100);
                                    double lnYMCAContrib = m_CurrentProjectedSalary * (lnYMCAPct / 100);

                                    // Now get the additional contribution
                                    double lnAddlContrib = 0;
                                    double MonthlyAddlContrib = 0;
                                    if (dtRetEstEmpElectives.Rows.Count != 0)
                                    {
                                        foreach (DataRow drElective in dtRetEstEmpElectives.Rows)
                                        {
                                            if (drElective["guiEmpEventID"].ToString().Trim().ToUpper() == guiEmpEventID
                                                && drElective["chrAcctType"].ToString().Trim().ToUpper() == chrAcctType)
                                            {
                                                if (Convert.ToDouble(drElective["mnyAddlContribution"]) != 0 || Convert.ToDouble(drElective["numAddlPctg"]) != 0)
                                                {
                                                    //Added by Ashish 02-Jul-2009 , for Isssue YRS 5.0-801
                                                    //Added 151-Jul-2009, no additional contribution for current month
                                                    if (l_bool_IsContributed && !(ycCalcMonth.Month == DateTime.Now.Month && ycCalcMonth.Year == DateTime.Now.Year))                                                  
                                                    {
                                                        chrAdjustmentBasisCode = drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim(); // Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                        strContributionType = chrAdjustmentBasisCode;
                                                        if (chrAdjustmentBasisCode == MONTHLY_PERCENT_SALARY)// Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891 | Using chrAdjustmentBasisCode instead of drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim()
                                                        {
                                                            //START: Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            chrEffectiveDate = drElective["dtmEffDate"].ToString().Trim();
                                                            if (drElective["ExistingCntType"].ToString().Trim() != string.Empty)
                                                            {
                                                                chrTerminateDate = drElective["dtsTerminationDate"].ToString().Trim();
                                                            }
                                                            //END: Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891

                                                            //start - code addition
                                                            //Commented By Ashish for phase V changes
                                                            //if (drElective["numAddlPctg"].ToString().Trim() != string.Empty)
                                                            //Added by Ashish for Phase V chanhes
                                                            if (Convert.ToDouble(drElective["numAddlPctg"]) != 0)
                                                            {
                                                                MonthlyAddlContrib = Convert.ToDouble(drElective["numAddlPctg"]);
                                                            }
                                                            else
                                                            {
                                                                MonthlyAddlContrib = Convert.ToDouble(drElective["mnyAddlContribution"]);
                                                            }
                                                            //end - code addition
                                                            //Ashish 08-Jul-2009 ,Remove equality from stop date( dtsTerminationdate) validation
                                                            // Changed numAddlPctg By mnyAddlContribution for Computing Right Interest  Anil YRPS 4536 2008.01.28
                                                            // Validation Code added by DInesh.k    2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                                                            //if (!string.IsNullOrEmpty(drElective["dtmEffDate"].ToString()))
                                                            //{
                                                            if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && drElective["dtsTerminationDate"].ToString().Trim() == string.Empty)                                                          
                                                            {
                                                                //lnAddlContrib = lnAddlContrib + (Convert.ToDouble(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]) * MonthlyAddlContrib / 100);
                                                                //Added by Ashish for Phase V part III changes
                                                                lnAddlContrib = lnAddlContrib + (m_CurrentProjectedSalary * MonthlyAddlContrib / 100);
                                                            }
                                                            else if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && ycCalcMonth < Convert.ToDateTime(drElective["dtsTerminationDate"]))
                                                            {
                                                                //lnAddlContrib = lnAddlContrib + (Convert.ToDouble(dtEmploymentProjectedSalary.Rows[0]["CurrentProjectedSalary"]) * MonthlyAddlContrib / 100);
                                                                //Added by Ashish for Phase V part III changes
                                                                lnAddlContrib = lnAddlContrib + (m_CurrentProjectedSalary * MonthlyAddlContrib / 100);
                                                            }
                                                            // }
                                                            // Changed numAddlPctg By mnyAddlContribution for Computing Rigth Interest  Anil YRPS 4536 2008.01.28
                                                        }
                                                        if (chrAdjustmentBasisCode == MONTHLY_PAYMENTS)// Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891 | Using chrAdjustmentBasisCode instead of drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim()
                                                        {
                                                            //2012.05.08    SP :  BT-1032 : 
                                                            if (drEstEmp["NumPayPeriod"] == System.DBNull.Value || string.IsNullOrEmpty(drEstEmp["NumPayPeriod"].ToString()))
                                                            {
                                                                errorMessage = "MESSAGE_RETIREMENT_NUMPAYPERIOD_NOTFOUND";
                                                                return;
                                                            }
                                                            
                                                            //START: Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            chrEffectiveDate = drElective["dtmEffDate"].ToString().Trim(); // ADDED BY Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            if (drElective["ExistingCntType"].ToString().Trim() != string.Empty)
                                                            {
                                                                chrTerminateDate = drElective["dtsTerminationDate"].ToString().Trim();
                                                            }
                                                            //END: Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            
                                                            //2012.05.08    SP :  BT-1032 : 
                                                            //Ashish 08-Jul-2009 ,Remove equality from stop date( dtsTerminationdate) validation
                                                            // Validation Code added by DInesh.k    2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                                                            //if (!string.IsNullOrEmpty(drElective["dtmEffDate"].ToString()))
                                                            //{                                                            
                                                            if (ycCalcMonth >= (Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year)) && drElective["dtsTerminationDate"].ToString().Trim() == string.Empty)
                                                                //lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"])); //Shashank  2012.02.02 Commented for BT-602/YRS 5.0-1130 : Change in how voluntary accounts are projected

                                                                //Shashank  2012.02.02 BT-602/YRS 5.0-1130 : Change in how voluntary accounts are projected
                                                                lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]) * Convert.ToInt32(drEstEmp["NumPayPeriod"])) / 12; //2012.05.08    SP :  BT-1032 : 
                                                            else if (ycCalcMonth >= (Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year)) && ycCalcMonth < Convert.ToDateTime(drElective["dtsTerminationDate"]))
                                                                // lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"])); //Shashank  2012.02.02 Commented for BT-602/YRS 5.0-1130 : Change in how voluntary accounts are projected

                                                                //Shashank  2012.02.02 BT-602/YRS 5.0-1130 : Change in how voluntary accounts are projected
                                                                lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]) * Convert.ToInt32(drEstEmp["NumPayPeriod"])) / 12; //2012.05.08    SP :  BT-1032 : 
                                                            // }
                                                        }
                                                        if (chrAdjustmentBasisCode == ONE_LUMP_SUM)// Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891 | Using chrAdjustmentBasisCode instead of drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim()
                                                        {
                                                            // Validation Code added by DInesh.k    2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                                                            //if (!string.IsNullOrEmpty(drElective["dtmEffDate"].ToString()))
                                                            //{
                                                            if (ycCalcMonth.Month == Convert.ToDateTime(drElective["dtmEffDate"]).Month && ycCalcMonth.Year == Convert.ToDateTime(drElective["dtmEffDate"]).Year)
                                                            {

                                                                lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
                                                                chrEffectiveDate = drElective["dtmEffDate"].ToString(); // Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            }
                                                            // }
                                                        }
                                                        if (chrAdjustmentBasisCode == YEARLY_LUMP_SUM_PAYMENT)// Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891 | Using chrAdjustmentBasisCode instead of drElective["chrAdjustmentBasisCode"].ToString().ToUpper().Trim()
                                                        {
                                                            //START: Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            chrEffectiveDate = drElective["dtmEffDate"].ToString().Trim(); // ADDED BY Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                                            if (drElective["ExistingCntType"].ToString().Trim() != string.Empty)
                                                            {
                                                                chrTerminateDate = drElective["dtsTerminationDate"].ToString().Trim();
                                                            }
                                                            //END: Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891

                                                            //Ashish 08-Jul-2009 ,Remove equality from stop date( dtsTerminationdate) validation
                                                            // Added by Anil YRPS - 4536 Validation for stop work date  2008.01.28
                                                            // Validation Code added by DInesh.k    2013.03.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                                                            // if (!string.IsNullOrEmpty(drElective["dtmEffDate"].ToString()))
                                                            // {
                                                            if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && (ycCalcMonth.Month == Convert.ToDateTime(drElective["dtmEffDate"]).Month) && drElective["dtsTerminationDate"].ToString().Trim() == string.Empty)
                                                                lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
                                                            else if (ycCalcMonth >= Convert.ToDateTime(Convert.ToDateTime(drElective["dtmEffDate"]).Month + "/1/" + Convert.ToDateTime(drElective["dtmEffDate"]).Year) && (ycCalcMonth.Month == Convert.ToDateTime(drElective["dtmEffDate"]).Month) && ycCalcMonth < Convert.ToDateTime(drElective["dtsTerminationDate"]))
                                                                lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
                                                            // Added by Anil YRPS - 4536 Validation for stop work date  2008.01.28
                                                            //																		if (ycCalcMonth.Month == Convert.ToDateTime(drElective["dtmEffDate"]).Month) 
                                                            //																		lnAddlContrib += (Convert.ToDouble(drElective["mnyAddlContribution"]));
                                                            // }
                                                        }
                                                    } // l_bool_IsContributed
                                                }
                                            }
                                        }
                                    }
                                    dblTDMonthlyContrib += lnAddlContrib; //Chandra sekar.c | 2016.04.21| YRS-AT-2612 & YRS-AT2891
                                    // Now get the total Member and Ymca balance
                                    mnyPersonalContribBalance += lnMemberContrib + lnAddlContrib;
                                    mnyYMCAContribBalance += lnYMCAContrib;
                                    // Commenting out If Condition YRPS 4536 2008.01.28
                                    //}
                                } // End For loop dtRetEstimateEmployment

                                #endregion
                                //}//if (chrProjPeriod == "CALCDATE_FORWARD" ) 
                                #endregion

                                // Check Limits
                                #region Contibution limit check

                                #region Check TD Contibution limit
                                // TD first
                                if (chrAcctType.ToUpper().Trim() == "TD")
                                {
                                    //START - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612  - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                                    lnContributionMaxAnnualTD = GetTDServiceCatchup(ycCalcMonth, dsMaxTDContributionAllowedPerYear, Convert.ToDateTime(retireeBirthday), dblTDMonthlyContrib, laProjectionPeriods[C_TERMDATE, lnProjectionPeriod], chrEffectiveDate, strContributionType, chrTerminateDate, dtTDWarningMessages, ref  strTDServiceCatchupLimitMgs);                                   //END - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612  - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                                    // If we have already over the TD limit 
                                    DateTime dtEndDate = Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod]);
                                    string strTraceWarningMgs;
                                    if (lnAnnualTDContributions >= lnContributionMaxAnnualTD)
                                    {
                                        mnyPersonalContribBalance = 0;
                                        mnyYMCAContribBalance = 0;
                                         if (!diTDContributionLimits.ContainsKey(ycCalcMonth.Year))
                                                diTDContributionLimits.Add(ycCalcMonth.Year, ycCalcMonth.ToString("MMM")); // Add year and max TD contribution amount to dictionary, it will help to log details
                                        //START - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612  - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                                        if ((ycCalcMonth.Month == 12) || ((ycCalcMonth.Month == dtEndDate.Month - 1) && (ycCalcMonth.Year == dtEndDate.Year)))
                                        {
                                            strTraceWarningMgs = string.Format("Maximum annual tax-deferred limit ${0} {1} used for the year {2} as the limit exhausted during the month of {3} {4}. ", lnContributionMaxAnnualTD.ToString(), strTDServiceCatchupLimitMgs, ycCalcMonth.Year, diTDContributionLimits[ycCalcMonth.Year], ycCalcMonth.Year);
                                            LoggerBO.LogMessage(strTraceWarningMgs, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which annual tax-deferred limit crossed.
                                        }
                    
                                        //if (!diTDContributionLimits.ContainsKey(ycCalcMonth.Year))
                                        //{
                                        //    diTDContributionLimits.Add(ycCalcMonth.Year, lnContributionMaxAnnualTD.ToString()); // Add year and max TD contribution amount to dictionary, it will help to log details
                                           
                                        //}
                                        //if (warningMessage.IndexOf("tax-deferred") == -1)
                                        //{
                                        //    warningMessage += "<br> Maximum annual tax-deferred limit $" + lnContributionMaxAnnualTD.ToString() + " used for estimate calculation";
                                        //    LoggerBO.LogMessage("Maximum annual tax-deferred limit $" + lnContributionMaxAnnualTD.ToString() + " used for estimate calculation for " + ycCalcMonth, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which annual tax-deferred limit crossed.
                                        //}
                                        //END - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)

                                    }
                                    else
                                    {
                                        //See if we will hit the limit with this contribution cycle
                                        if (mnyPersonalContribBalance + mnyYMCAContribBalance + lnAnnualTDContributions >= lnContributionMaxAnnualTD)
                                        {
                                            // See if just the personal side causes us to hit the limit
                                            // Adjust the YMCA side so it brings us to the limit
                                            if (mnyPersonalContribBalance + lnAnnualTDContributions >= lnContributionMaxAnnualTD)
                                            {
                                                mnyPersonalContribBalance = lnContributionMaxAnnualTD - lnAnnualTDContributions;
                                                mnyYMCAContribBalance = 0;
                                            }
                                            else
                                                //See if we will hit the limit with this contribution cycle
                                                // Adjust the YMCA side so it brings us to the limit
                                                mnyYMCAContribBalance = lnContributionMaxAnnualTD - lnAnnualTDContributions + mnyPersonalContribBalance;

                                            //START - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612  - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                                            if (!diTDContributionLimits.ContainsKey(ycCalcMonth.Year))
                                                diTDContributionLimits.Add(ycCalcMonth.Year, ycCalcMonth.ToString("MMM")); // Add year and max TD contribution amount to dictionary, it will help to log details
                                           if ((ycCalcMonth.Month == 12) || ((ycCalcMonth.Month == dtEndDate.Month - 1) && (ycCalcMonth.Year == dtEndDate.Year)))
                                           {
                                               strTraceWarningMgs = string.Format("Maximum annual tax-deferred limit ${0} {1} used for the year {2} as the limit exhausted during the month of {3} {4}. ", lnContributionMaxAnnualTD.ToString(), strTDServiceCatchupLimitMgs, ycCalcMonth.Year, diTDContributionLimits[ycCalcMonth.Year], ycCalcMonth.Year);
                                                LoggerBO.LogMessage(strTraceWarningMgs, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which annual tax-deferred limit crossed.
                                           }
                                            //Since we have adjusted the values, we have to display the message. NP:2012.03.16:YRS 5.0-1560:BT-1016 - Message was not being displayed when the limit was being crossed in the last month of the financial year.
                                            //if (warningMessage.IndexOf("tax-deferred") == -1)
                                            //{
                                            //    warningMessage += "<br> Maximum annual tax-deferred limit $" + lnContributionMaxAnnualTD.ToString() + " used for estimate calculation";
                                            //    LoggerBO.LogMessage("Maximum annual tax-deferred limit $" + lnContributionMaxAnnualTD.ToString() + " used for estimate calculation for " + ycCalcMonth, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which annual tax-deferred limit crossed.
                                            //}
                                            //END - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                                        }
                                    }
                                }
                                //2012.02.21    SP:  BT-1003 : Annual Contribution limits messages not showing properly - Start 
                                //Update the YTD contributions for future limit comparisons
                                if (chrAcctType.ToUpper().Trim() == "TD")
                                    lnAnnualTDContributions = lnAnnualTDContributions + mnyPersonalContribBalance + mnyYMCAContribBalance;
                                //2012.02.21   SP:  BT-1003 : Annual Contribution limits messages not showing properly - End
                                #endregion
                                // All contributions limit check  -- Exclude RT account from the check
                                #region Check other Acct contribution limit
                                // See if we are already over the limit
                                if (chrAcctType.ToUpper().Trim() != "RT")
                                {
                                    if (lnAnnualContributions >= lnContributionMaxAnnual)
                                    {
                                        mnyPersonalContribBalance = 0;
                                        mnyYMCAContribBalance = 0;
                                        if (warningMessage.IndexOf("annual contribution") == -1)
                                        {
                                            warningMessage += "<br> Maximum annual contribution limit $" + lnContributionMaxAnnual.ToString() + " used for estimate calculation";
                                            LoggerBO.LogMessage("Maximum annual contribution limit $" + lnContributionMaxAnnual.ToString() + " used for estimate calculation for" + ycCalcMonth, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which Maximum annual contribution limit crossed.
                                        }
                                    }
                                    else
                                    {
                                        //See if we will hit the limit with this contribution cycle
                                        if ((mnyPersonalContribBalance + mnyYMCAContribBalance + lnAnnualContributions) >= lnContributionMaxAnnual)
                                        {
                                            //See if just the personal side causes us to hit the limit
                                            if ((mnyPersonalContribBalance + lnAnnualContributions) >= lnContributionMaxAnnual)
                                            {
                                                //'Adjust the personal side and set YMCA side to 0 which should bring us to the limit
                                                mnyPersonalContribBalance = lnContributionMaxAnnual - lnAnnualContributions;
                                                mnyYMCAContribBalance = 0;
                                            }
                                            else
                                                //Adjust the YMCA side so it brings us to the limit
                                                mnyYMCAContribBalance = (lnContributionMaxAnnual - lnAnnualContributions) + mnyPersonalContribBalance;
                                            //Since we have adjusted the values, we have to display the message. NP:2012.03.16:YRS 5.0-1560:BT-1016 - Message was not being displayed when the limit was being crossed in the last month of the financial year.
                                            if (warningMessage.IndexOf("annual contribution") == -1)
                                            {
                                                warningMessage += "<br> Maximum annual contribution limit $" + lnContributionMaxAnnual.ToString() + " used for estimate calculation";
                                                LoggerBO.LogMessage("Maximum annual contribution limit $" + lnContributionMaxAnnual.ToString() + " used for estimate calculation for " + ycCalcMonth, TraceEventType.Information); // 2016.01.06 | Sanjay S. | YRS-AT-2252 - Log date for which Maximum annual contribution limit crossed.
                                            }
                                        }
                                    }
                                }
                                #endregion

                                //2012.02.21    SP:  BT-1003 : Annual Contribution limits messages not showing properly - Start (Commented for BT-1003)
                                //Update the YTD contributions for future limit comparisons
                                //if (chrAcctType.ToUpper().Trim() == "TD")
                                //    lnAnnualTDContributions = lnAnnualTDContributions + mnyPersonalContribBalance + mnyYMCAContribBalance;
                                //2012.02.21    SP:  BT-1003 : Annual Contribution limits messages not showing properly - End

                                if (chrAcctType.ToUpper().Trim() != "RT") // Exclude RT account from the check
                                    lnAnnualContributions = lnAnnualContributions + mnyPersonalContribBalance + mnyYMCAContribBalance;
                                #endregion
                                //Added by Ashish for phase V Part III changes
                                //Added or update contribution in dtAccountsBasisByProjection

                                l_EffectiveAnnuityBasisType = string.Empty;
                                l_EffectiveAnnuityBasisGroup = string.Empty;
                                DataRow drAnnuityBasisRow = null;
                                if (chrAcctType.ToUpper().Trim() != "RT")
                                {
                                    drAnnuityBasisRow = GetEffectiveAnnuityBasisType(dtAnnuityBasisTypeList, "PST", ycCalcMonth);

                                }
                                else if (chrAcctType.ToUpper().Trim() == "RT")
                                {
                                    string l_RetirementDate = laProjectionPeriods[C_TERMDATE, lnProjectionPeriod].ToString();
                                    if (mnyPersonalContribBalance > 0)
                                    {
                                        drAnnuityBasisRow = GetRollOverMatureBasisType(dtAnnuityBasisTypeList, l_RetirementDate, ycCalcMonth);

                                    }
                                }
                                if (drAnnuityBasisRow != null)
                                {
                                    l_EffectiveAnnuityBasisType = drAnnuityBasisRow["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                                    l_EffectiveAnnuityBasisGroup = drAnnuityBasisRow["chrAnnuityBasisGroup"].ToString().Trim().ToUpper();

                                    AddContributionInAccountsProjection(ref dtAccountsBasisByProjection, chrAcctType, l_EffectiveAnnuityBasisGroup, l_EffectiveAnnuityBasisType, mnyPersonalContribBalance, mnyYMCAContribBalance, planType);
                                }
                                mnyPersonalPreTax += mnyPersonalContribBalance;
                                mnyYMCAPreTax += mnyYMCAContribBalance;
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Projected Contribution", String.Format("{0} Personal - {1} YMCA - {2}", chrAcctType, mnyPersonalContribBalance, mnyYMCAContribBalance));                
                                #region Threshold Check
                                //Added by Ashish for pahase v changes Start, if account type is YmcaLegacy Account then add YmcaPeretax for thershhol limit
                                //Remove threshold check for fund status QD
                                if (para_FundStatus != "QD")
                                {
                                    bool l_flagIsYmcaLegacyAcctBalExceedFound = false;
                                    DataRow[] l_drRetEstAcctFoundRow;
                                    if (dtRetEstEmpElectives != null)
                                    {
                                        if (dtRetEstEmpElectives.Rows.Count > 0)
                                        {
                                            //YmcaLegacy Account threshold check
                                            l_drRetEstAcctFoundRow = dtRetEstEmpElectives.Select("bitBasicAcct=True AND chrAcctType='" + chrAcctType + "' AND bitYA=1");
                                            if (l_drRetEstAcctFoundRow.Length > 0)
                                            {
                                                if (ycCalcMonth <= Convert.ToDateTime(Convert.ToDateTime(para_strMaxTerminationDate).Month + "/1/" + Convert.ToDateTime(para_strMaxTerminationDate).Year) && !l_flagIsYmcaLegacyAcctBalExceedFound)
                                                {
                                                    mnyYmcaLegacyAcctTotal += mnyYMCAPreTax;
                                                    if (mnyYmcaLegacyAcctTotal > mnyYmcaLegacyAcctThreshold)
                                                    {
                                                        isYmcaLegacyAcctTotalExceed = true;
                                                        l_flagIsYmcaLegacyAcctBalExceedFound = true;
                                                    }
                                                 }
                                            }
                                            //Ymca Account threshold check
                                            l_drRetEstAcctFoundRow = dtRetEstEmpElectives.Select("bitBasicAcct=True AND chrAcctType='" + chrAcctType + "' AND bitEP=1");
                                            if (l_drRetEstAcctFoundRow.Length > 0)
                                            {
                                                mnyYmcaAcctTotal += mnyYMCAPreTax;
                                               if (mnyYmcaAcctTotal > mnyYmcaAcctThreshhold)
                                               {
                                                   isYmcaAcctTotalExceed = true;
                                               }
                                             }
                                        } // dtRetEstEmpElectives.Row.Count
                                    } // dtRetEstEmpElectives!=null
                                } // if(para_FundStatus !="QD")
                                //Added by Ashish for pahase v changes End
                                #endregion

                            } // End of Non Basic Ret60 Else block

                            //}// End of FirstTimeInMonthLoop Else block

                        }//foreach (DataRow drActBasis in dtUniqueAcctList.Rows)					
                        #endregion
                        //Added contribution and interest amount into main account total
                        UpdateAcctTotalInProjectionTable(ref dtAccountsBasisByProjection, lcRetireType);
                    }
                    // Get the next month to calculate
                    if (ycCalcMonth.Month == 12)
                        ycCalcMonth = Convert.ToDateTime("01/01/" + (Convert.ToDateTime(ycCalcMonth).Year + 1));
                    else
                        ycCalcMonth = Convert.ToDateTime(Convert.ToDateTime(ycCalcMonth).Month + 1 + "/1/ " + ycCalcMonth.Year);

                    // If entered a new calendar year, reset annual Contribution limit counters
                    if (ycCalcMonth.Year != Convert.ToDateTime(ycCalcMonth.AddMonths(-1)).Year)
                    {
                        lnAnnualTDContributions = 0;
                        lnAnnualContributions = 0;
                        annualCompensation = 0;  // SR | 2018.08.28 | YRS-AT-3790 | reset annual compensation.
                    }

                    // See if participant is >= 50 in this projection period
                    if (DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(retireeBirthday), ycCalcMonth) >= 50)
                    {
                        //_ll50 = true;

                        if (dsContributionLimits.Tables[0].Rows.Count > 0)
                            lnContributionMaxAnnualTD = Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnualTD"]) +
                                Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnual50Addl"]);

                        // Only increase the total contribution by the over-50-td-addl if there is a TD account
                        if (TDAccountExists)
                            lnContributionMaxAnnual = Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnual"]) +
                                Convert.ToDouble(dsContributionLimits.Tables[0].Rows[0]["ContributionMaxAnnual50Addl"]);
                    }

                    if (ycCalcMonth > Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, lnProjectionPeriod]))
                    {
                        if (lnProjectionPeriod == laProjectionPeriods.GetUpperBound(1))
                            _llDone = true;
                        else if (ycCalcMonth > Convert.ToDateTime(laProjectionPeriods[C_TERMDATE, laProjectionPeriods.GetUpperBound(1)]))
                        {
                            _llDone = true;
                        }
                        else
                        {
                            //if (ycCalcMonth == Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod + 1]))
                            if (ycCalcMonth == Convert.ToDateTime(laProjectionPeriods[C_EFFDATE, lnProjectionPeriod]))
                            {
                                //lnProjectionPeriod = lnProjectionPeriod + 1;
                                //if (laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod].Trim().ToUpper() == "PRE96_FORWARD")
                                //    lnRETIREMENT_PROJECTION_PHASE = lnProjectionPeriod;
                                //else 
                                if (laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod].Trim().ToUpper() == "CALCDATE_FORWARD")
                                    lnRETIREMENT_PROJECTION_PHASE = lnProjectionPeriod;
                                else if (laProjectionPeriods[C_PROJECTIONPERIOD, lnProjectionPeriod].Trim().ToUpper() == "RETIREDATE_TO_AGE60")
                                    lnRETIREDATE_TO_AGE60 = lnProjectionPeriod;
                            }
                        }
                    }
                    //isFirstTimeInMonthLoop = false; //PPP | 11/24/2017 | YRS-AT-3319 | Variable is declared and assigned but not used
                }// End While

                //START: PPP | 03/17/2017 | YRS-AT-2625 | Adding unfunded and not received amount after current month is passed, So that interest will get added to it
                if (!isUnReceivedAndUnfundedBalanceAdded && considerPendingBalance)
                {
                    // START | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment details hence end work date will not be available
                    if (HelperFunctions.isNonEmpty(employmentDetails))
                    {
                        //if (employmentDetails != null)
                        endWorkDate = Convert.IsDBNull(employmentDetails.Rows[0]["EndWorkDate"]) ? string.Empty : Convert.ToString(employmentDetails.Rows[0]["EndWorkDate"]);
                    }
                    // END | SR | 04/19/2017 | YRS-AT-3403 | For some fundstatus like QDRO, there will be no employment details hence end work date will not be available
                    //START: PPP | 04/26/2017 | YRS-AT-3419 
                    // In disability code always try to fetch unreceived and unfunded balances in a Normal Projection Cycle
                    // but if the retirement date is current month start date then Normal Projection Cycle does not get called
                    // So code reach this area and try to fetch unreceived and unfunded balance 
                    // In this case strRetirementDate contains date when participant is attaining age 60
                    // impact of this is system then fetches unwanted months unreceived and unfunded balance 
                    // For e.g. Current date is 04/xx/2017, Retirement Date on screen is 4/1/2017 and participant is reaching age 60 by 6/1/2017
                    // So code passes 6/1/2017 as a retirement date and gets unreceived and unfunded balances till 5/1/2017 (because 5/1/2017 is next month start date, for more info check SP:yrs_usp_Ret_Est_GetPendingBalances)
                    // To avoid fetching balances till 5/1/2017, we have to pass 4/1/2017 to metod instead of 6/1/2017
                    // So replacing strRetirementDate with cutOffDateToGetUnreceivedAndPendingBalance which will get passed to the method
                    // and its value will be determined based on retirement type
                    //GetPendingBalances(fundEventID, dtAccountsBasisByProjection, Convert.ToDateTime(strRetirementDate), string.IsNullOrEmpty(endWorkDate) ? null : (DateTime?)Convert.ToDateTime(endWorkDate));
                    if (lcRetireType == "DISABL")
                        cutOffDateToGetUnreceivedAndPendingBalance = laProjectionPeriods[C_EFFDATE, lnProjectionPeriod].ToString();
                    else
                        cutOffDateToGetUnreceivedAndPendingBalance = strRetirementDate;

                    GetPendingBalances(fundEventID, dtAccountsBasisByProjection, Convert.ToDateTime(cutOffDateToGetUnreceivedAndPendingBalance), string.IsNullOrEmpty(endWorkDate) ? null : (DateTime?)Convert.ToDateTime(endWorkDate));
                    //END: PPP | 04/26/2017 | YRS-AT-3419 
                    
                    isUnReceivedAndUnfundedBalanceAdded = true;
                }
                //END: PPP | 03/01/2017 | YRS-AT-2625 | Adding unfunded and not received amount after current month is passed, So that interest will get added to it

                //START - Chandra sekar.c   2016.04.21    YRS-AT-2891 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                // For displaying multiple TD contribution warning messages according to year ranges 
                if (dtTDWarningMessages.Rows.Count > 0)
                {
                    System.Text.StringBuilder sbTDWaringTDMessage = new System.Text.StringBuilder();
                    string strWaringTDMessage, strStartYear, strEndYear;
                    foreach (DataRow drTDWarningMessage in dtTDWarningMessages.Rows)
                    {
                        strStartYear = Convert.IsDBNull(drTDWarningMessage["StartYear"]) ? string.Empty : Convert.ToString(drTDWarningMessage["StartYear"]);
                        strEndYear = Convert.IsDBNull(drTDWarningMessage["EndYear"]) ? string.Empty : Convert.ToString(drTDWarningMessage["EndYear"]); 
                        if ((strStartYear == strEndYear) || (string.IsNullOrEmpty(strEndYear)))
                        {
                            // Annual TD  Contribution Limit For single year
                            strWaringTDMessage = string.Format("<br /> Maximum annual tax-deferred limit ${0} {1} used for {2}", drTDWarningMessage["TDContributionLimits"].ToString(), drTDWarningMessage["TDServiceCatchupLimitMgs"].ToString(), strStartYear);
                        }
                        else
                        {
                            strWaringTDMessage = string.Format("<br /> Maximum annual tax-deferred limit ${0} {1} used for {2} - {3}", drTDWarningMessage["TDContributionLimits"].ToString(), drTDWarningMessage["TDServiceCatchupLimitMgs"].ToString(), strStartYear, strEndYear);
                        }
                        sbTDWaringTDMessage.Append(strWaringTDMessage);
                    }
                    warningMessage += sbTDWaringTDMessage.ToString();
                }
                //END - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
                dtAccountsBasisByProjection.AcceptChanges();
            }
            catch //(Exception ex)
            {
                throw;//errMsg = ex.Message;
            }
            // START : SR | 2018.12.03 | YRS-AT-4106 | Added Finally block perform endtrace for logging estimate details.
            //errMsg = string.Empty;
            finally 
            {
                errMsg = string.Empty;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Estimates", "calculateInterestOnContributions END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
            // END : SR | 2018.12.03 | YRS-AT-4106 | Added Finally block perform endtrace for logging estimate details.
        }

        // START : SR | 2018.11.21 | YRS-AT-4106 | created common method to validate Max annual salary limit.       
        // This method will limit compensation if running annual compensation & current month salary reaches maximum annual salary limit. 
        // Also, it will set warning message if annual compensation & current moonth salary reaches maximum annual salary limit.
        private static double ValidateMaxAnnualSalaryLimit(DateTime ycCalcMonth, double lnMaximumContributionSalary, double annualCompensation, double currentProjectedSalary, out string salaryWarningMessage)
        {
            salaryWarningMessage = string.Empty;
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ValidateMaxAnnualSalaryLimit", String.Format("Running Salary : {0} Current Month Salary : {1}", annualCompensation, currentProjectedSalary));
            if (annualCompensation + currentProjectedSalary >= lnMaximumContributionSalary)
            {
                if (annualCompensation >= lnMaximumContributionSalary)
                {
                    currentProjectedSalary = 0;
                }
                else
                {
                    currentProjectedSalary = lnMaximumContributionSalary - annualCompensation;
                }

                salaryWarningMessage = "Maximum annual salary limit $" + lnMaximumContributionSalary.ToString() + " used for estimate calculation";
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("ValidateMaxAnnualSalaryLimit", String.Format("Maximum annual salary limit used for projection date {0} ", ycCalcMonth));
            }
            return currentProjectedSalary;
        }
        //END : SR | 2018.11.21 | YRS-AT-4106 | created common method to validate Max annual salary limit.

        // START : | Chandra sekar.c | 2016.04.21 | YRS-AT-2612  & YRS-AT-2891
        // Creating a table which will hold information of years where max TD limit crossed or reached.
        private static DataTable CreateTDWarningMessageSchema()
        {
            DataTable dtTDWarningMessageSchema = new DataTable("TDWarningMessages");
            dtTDWarningMessageSchema.Columns.Add("TDContributionLimits", typeof(string));
            dtTDWarningMessageSchema.Columns.Add("TDServiceCatchupLimitMgs", typeof(string));
            dtTDWarningMessageSchema.Columns.Add("StartYear", typeof(string));
            dtTDWarningMessageSchema.Columns.Add("EndYear", typeof(string));
            return dtTDWarningMessageSchema;
        }
        // END : | Chandra sekar.c | 2016.04.21 | YRS-AT-2612  & YRS-AT-2891

        /// <summary>
        /// For regular retirement calculate the YMCA, Personal, and (YMCA+Personal for basic accounts only) amounts 
        /// for Pre, Post and Rollover accounts.
        /// For Disability retirement calculate the projected YMCA and Personal, Age60 balance.
        /// This Method only used in Retirement Estimate.
        /// </summary>
        /// <param name="dtAccountsBasisByProjection"></param>
        /// <param name="lnRETIREMENT_PROJECTION_PHASE"></param>
        /// <param name="lnRETIREDATE_TO_AGE60"></param>
        /// <param name="planType"></param>
        //Commented by Ashish for phase V changes ,start
        //		private void calculateFinalAmounts(DataTable dtAccountsBasisByProjection, DataTable dtExcludedAccounts
        //			, int lnRETIREMENT_PROJECTION_PHASE, int lnRETIREDATE_TO_AGE60, string planType)
        //		{
        //			dtAccountsBasisByProjection = removeExcludedAccounts(dtAccountsBasisByProjection, dtExcludedAccounts);
        //Commented by Ashish for phase V changes ,End
        //Added  by Ashish for phase V changes ,start
        //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
        //public void calculateFinalAmounts(DataTable para_dtAccountsBasisByProjection, DataTable dtExcludedAccounts
        //    , int lnRETIREMENT_PROJECTION_PHASE, int lnRETIREDATE_TO_AGE60, string planType)
        // Commented by chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - To Added two agrument(decPIA_At_Termination,l_bool_IsPersonTerminated)for Seting the YMCA(LEGACY) ACCOUNT Balance as on Termination  
        // public void calculateFinalAmounts(DataTable para_dtAccountsBasisByProjection, string planType, int iAge = 0, bool bIspartial = false, decimal decPartialValue = 0)
        // Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Added two agrument(decPIA_At_Termination,l_bool_IsPersonTerminated) for Seting the YMCA(LEGACY) ACCOUNT Balance as on Termination  
        public void calculateFinalAmounts(DataTable para_dtAccountsBasisByProjection, string planType, int iAge = 0, bool bIspartial = false, decimal decPartialValue = 0, decimal decPIA_At_Termination = 0, bool bIsPersonTerminated = false, string strFundEventStatus = "")  // Added by Chandrasekar.c on 2015.06.22 YRS-AT-3010 One New Optional Parameter is added 
        {

            try
            {
                //para_dtAccountsBasisByProjection = removeExcludedAccounts(para_dtAccountsBasisByProjection, dtExcludedAccounts,true);
                ////Added by Ashish for phase V changes
                DataTable dtAccountsBasisByProjection;
                dtAccountsBasisByProjection = para_dtAccountsBasisByProjection.Copy();
                //Added  by Ashish for phase V changes ,End
                //Added by Ashish for phase V part III changes
                CreateAcctBalancesByBasisTypeSchema(ref g_dtAcctBalancesByBasisType);

                //Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate- Intialling the Variable's to assign values of Account Balances
                decimal decYMCAPreTax = 0;
                decimal decYMCAAmt = 0;
                decimal decPersonalAmt = 0;
                decimal decPersonalPreTax = 0;
                decimal decYMCALegacyAmountToValidate = 0;
                //End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate- Intialling the Variable's to assign values of Account Balances
                //ycProjected_Basic_YMCA_Age60_Balance = 0;
                //ycProjected_Basic_Personal_Age60_Balance = 0;
                // START Added by Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                bool bIsBACombinedAccountRule = false; 
                bool bIsPartialAmtDeductionLegacyAccount = false; //  
                bool bIsPartialAmtDeductionBAAccount = false;
                // END  Added by Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                // Set the correct plan type
                if (planType == "R")
                    planType = "RETIREMENT";
                else if (planType == "S")
                    planType = "SAVINGS";
                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //else if (planType == "B")
                //{
                //    planType = "";
                //    foreach(DataRow dr in dtAccountsBasisByProjection.Rows)
                //        dr["chvPlanType"] = string.Empty;
                //}
                //Added by Ashish for phase V part II changes
                //decimal Personal_Retirement_Balance=0;
                //decimal YMCA_Retirement_Balance=0;
                //decimal Basic_Retirement_Balance=0;
                //ASHISH:2011-01-28 Added for BT- 665 Disability Retirment	


                //Start: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                if (planType == "RETIREMENT")
                {
                    if (bIspartial && decPartialValue > 0)
                    {
                        string strAcctType = string.Empty;
                        decimal decYmcaLegacyAmount = 0;
                        decimal decYmcaAccount = 0;
                        //START: PPP | 11/24/2017 | YRS-AT-3319 | decPersonalTotal variable is declared and assigned but not used
                        //decimal decTotalProjRet = 0, decPersonalTotal = 0, decPartialAmount = 0, decAccountTotal = 0, REFUND_MAX_PIA = 0, BA_MAX_LIMIT_55_ABOVE = 0, REFUND_MAX_PIA_AT_TERM = 0, BA_LEGACY_COMBINED_MAX_LIMIT = 0;
                        decimal decTotalProjRet = 0, decPartialAmount = 0, decAccountTotal = 0, REFUND_MAX_PIA = 0, BA_MAX_LIMIT_55_ABOVE = 0, REFUND_MAX_PIA_AT_TERM = 0, BA_LEGACY_COMBINED_MAX_LIMIT = 0;
                        //END: PPP | 11/24/2017 | YRS-AT-3319 | decPersonalTotal variable is declared and assigned but not used
                        decTotalProjRet = 0;
                        //DataTable l_Datatable_REFUND_MAX_PIA, l_DataTable_BA_MAX_LIMIT_55_ABOVE; //PPP | 11/24/2017 | YRS-AT-3319 | Variables are declared and assigned but not used
                        // START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                        DataTable dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT;
                        decimal decYmcaLegacyAccountAndYmcaAccountTotal = 0;
                        // END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                        // Commented by START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                        //l_Datatable_REFUND_MAX_PIA = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("REFUND_MAX_PIA");
                        //l_DataTable_BA_MAX_LIMIT_55_ABOVE = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_55_ABOVE");

                        //if (l_Datatable_REFUND_MAX_PIA != null)
                        //{
                        //    if (l_Datatable_REFUND_MAX_PIA.Rows[0]["Key"].ToString().Trim().ToUpper() == "REFUND_MAX_PIA")
                        //    {
                        //        REFUND_MAX_PIA = Convert.ToDecimal(l_Datatable_REFUND_MAX_PIA.Rows[0]["Value"].ToString());
                        //    }
                        //}

                        //if (l_DataTable_BA_MAX_LIMIT_55_ABOVE != null)
                        //{
                        //    if (l_DataTable_BA_MAX_LIMIT_55_ABOVE.Rows[0]["Key"].ToString().Trim().ToUpper() == "BA_MAX_LIMIT_55_ABOVE")
                        //    {
                        //        BA_MAX_LIMIT_55_ABOVE = Convert.ToDecimal(l_DataTable_BA_MAX_LIMIT_55_ABOVE.Rows[0]["Value"].ToString());
                        //    }
                        //}
                        // Commented by END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                        // START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)

                        // START : ML| 2019.06.05 |YRS-AT-4458 - YRS bug-partial withdrawal not reducing estimates for active participants (TrackIT 38406)
                        // Passing bIsPersonTerminated as True because bIsPersonTerminated value is used to fetch withdrawal configuration only
                        // So that terminated participants withrawal rules will get applied to Active participants also.
                        //dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(iAge, bIsPersonTerminated);  // Added by : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                         dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestConfiguration(iAge, true);
                        // END : ML| 2019.06.05 |YRS-AT-4458 - YRS bug-partial withdrawal not reducing estimates for active participants (TrackIT 38406)


                        if (IsNonEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT))
                        {
                            BA_LEGACY_COMBINED_MAX_LIMIT = string.IsNullOrEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaCombinedBasicAccountLimit"].ToString()) ? 0 : Convert.ToDecimal(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaCombinedBasicAccountLimit"]);
                            BA_MAX_LIMIT_55_ABOVE = string.IsNullOrEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaAccountLimit"].ToString()) ? 0 : Convert.ToDecimal(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaAccountLimit"]);
                            REFUND_MAX_PIA = string.IsNullOrEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaLegacyAccountLimit"].ToString()) ? 0 : Convert.ToDecimal(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaLegacyAccountLimit"]);
                            REFUND_MAX_PIA_AT_TERM = string.IsNullOrEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaLegacyAccountAtTermLimit"].ToString()) ? 0 :Convert.ToDecimal(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["YmcaLegacyAccountAtTermLimit"]);
                            bIsBACombinedAccountRule = string.IsNullOrEmpty(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["bitIsBALegacyCombinedRule"].ToString()) ? false :Convert.ToBoolean(dt_BA_LEGACY_MAX_PARTIAL_WITHDRAWAL_LIMIT.Rows[0]["bitIsBALegacyCombinedRule"]);
                        }
                        // END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                        //2013.10.10    Dinesh Kanojia(DK)  BT-943: YRS 5.0-1443:Include partial withdrawals
                        if (!string.IsNullOrEmpty(dtAccountsBasisByProjection.Compute("SUM(mnyymcapretax)", "chvPlanType='" + planType + "' AND chrAcctType IN('SA','SS','RG')").ToString()))
                        {
                            decYmcaLegacyAmount = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyymcapretax)", "chvPlanType='" + planType + "' AND chrAcctType IN('SA','SS','RG')"));
                        }
                        else
                        {
                            decYmcaLegacyAmount = 0;
                        }

                        //2013.10.10    Dinesh Kanojia(DK)  BT-943: YRS 5.0-1443:Include partial withdrawals
                        if (!string.IsNullOrEmpty(dtAccountsBasisByProjection.Compute("SUM(mnyymcapretax)", "chvPlanType='" + planType + "' AND chrAcctType ='BA' ").ToString()))
                        {
                            decYmcaAccount = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyymcapretax)", "chvPlanType='" + planType + "' AND chrAcctType ='BA' "));
                        }
                        else
                        {
                            decYmcaAccount = 0;
                        }

                        //2013.10.10    Dinesh Kanojia(DK)  BT-943: YRS 5.0-1443:Include partial withdrawals
                        if (!string.IsNullOrEmpty(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='" + planType + "'").ToString()))
                        {
                            decTotalProjRet = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='" + planType + "'"));
                        }
                        else
                        {
                            decTotalProjRet = 0;
                        }

                        // Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Checking whether person is terminated or not , if Person is terminated and then assign the YMCA(LEGACY) ACCOUNT Balance as on Termination

                        if (strFundEventStatus != "QD") // Added by : Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation
                        {
                            decYMCALegacyAmountToValidate = bIsPersonTerminated ? decPIA_At_Termination : decYmcaLegacyAmount;

                            // START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                            decYmcaLegacyAccountAndYmcaAccountTotal = decYmcaAccount + decYmcaLegacyAmount;
                            // Sum of YMCA Account and YMCA Legacy Amount is greater than BA_MAX_COMBINED_LIMIT i.e $50000 then excluded the YMCA Legacy amount
                            if (bIsBACombinedAccountRule) // New Rule switch ON
                            {
                                // If (YMCA Acount + YMCA (legacy)Account)  > 50000 
                                if (decYmcaLegacyAccountAndYmcaAccountTotal > BA_LEGACY_COMBINED_MAX_LIMIT)
                                {
                                    // If (YMCA Acount + YMCA (legacy)Account)  > 50000 and also YMCA Legacy Amount at termination is greater than $25000 excluded YMCA(Legacy) Account for Partial Amount deduction
                                    if (decYMCALegacyAmountToValidate > REFUND_MAX_PIA_AT_TERM)
                                    {
                                        decTotalProjRet = decTotalProjRet - decYmcaLegacyAmount;
                                    }

                                    if (decYmcaAccount > BA_MAX_LIMIT_55_ABOVE)  // IF YMCA Account  > $25000 Or YMCA Account  > $50000  and Both rules excluded  Account for Partial Amount deduction
                                    {
                                        decTotalProjRet = decTotalProjRet - decYmcaAccount;
                                    }
                                }
                            }
                            else  // New Rule switch OFF
                            {
                                // If YMCA(LEGACY)ACCOUNT Balance/YMCA(LEGACY)ACCOUNT Balance as on Termination is greater than REFUND_MAX_PIA (i.e 25000K) then excluded the this account for Partial Withdrawal 
                                // If YMCA (legacy)Account) at Termination  > 25000 
                                if (decYMCALegacyAmountToValidate > REFUND_MAX_PIA_AT_TERM)
                                {
                                    decTotalProjRet = decTotalProjRet - decYmcaLegacyAmount;
                                }
                                if (decYmcaAccount > BA_MAX_LIMIT_55_ABOVE)  // IF YMCA Account  > $25000 Or YMCA Account  > $50000  and Both rules excluded  Account for Partial Amount deduction
                                {
                                    decTotalProjRet = decTotalProjRet - decYmcaAccount;
                                }
                            }
                            // END : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                            //End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Checking whether person is terminated or not , if Person is terminated and then assign the YMCA(LEGACY) ACCOUNT Balance as on Termination
                        }
                        decPartialAmount = decTotalProjRet / decPartialValue;
                        //2013-10-24    Dinesh.k            BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
                        if (decPartialAmount > 0)
                        {
                            for (int iCount = 0; iCount <= dtAccountsBasisByProjection.Rows.Count - 1; iCount++)
                            {                                
                                strAcctType = dtAccountsBasisByProjection.Rows[iCount]["chrAcctType"].ToString().Trim().ToUpper();                               
                         
                                //Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Refactoring for instead of fetching from data table instantly, assigned value to local variable’s used throughout below code
                                decYMCAPreTax = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim());
                                decYMCAAmt = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim());
                                decPersonalAmt = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString().Trim());
                                //End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Refactoring for instead of fetching from data table instantly, assigned value to local variable’s used throughout below code
                                bIsPartialAmtDeductionLegacyAccount = false;
                                bIsPartialAmtDeductionBAAccount = false;
                                if (strAcctType == "SA" || strAcctType == "SS" || strAcctType == "RG")
                                {
                                    if (strFundEventStatus != "QD") // Added by : Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation
                                    {
                                        // START : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                                        // Sum of YMCA Account Acount and YMCA (Legecy) Account are less than partial Withdrawal will do it. 
                                        // If current date greater than effective Date Combined account validate will execute 
                                        // If (YMCA Acount + YMCA (legacy)Account)  < 50000 and Combined rule
                                        if (bIsBACombinedAccountRule) // New rule is Switch ON 
                                        {
                                            if (decYmcaLegacyAccountAndYmcaAccountTotal <= BA_LEGACY_COMBINED_MAX_LIMIT)
                                            {
                                                bIsPartialAmtDeductionLegacyAccount = true;
                                            }
                                            else if (decYmcaLegacyAccountAndYmcaAccountTotal > BA_LEGACY_COMBINED_MAX_LIMIT)
                                            {
                                                if (decYMCALegacyAmountToValidate <= REFUND_MAX_PIA_AT_TERM)
                                                {
                                                    bIsPartialAmtDeductionLegacyAccount = true;
                                                }
                                            }
                                        }
                                        else // new rule switch OFF
                                        {
                                            if (decYMCALegacyAmountToValidate <= REFUND_MAX_PIA_AT_TERM)
                                            {
                                                bIsPartialAmtDeductionLegacyAccount = true;
                                            }
                                        }
                                        // If YMCA Legacy Account at termination (or current balance if person is active) is > 25k then money would not be removed from the YMCA Legacy Account
                                        // If YMCA(LEGACY)ACCOUNT Balance / YMCA(LEGACY)ACCOUNT Balance at Termination is less than REFUND_MAX_PIA (i.e 25000K) then included the this account for Partial Withdrawal 
                                        // If YMCA (legacy)Account) at Termination  < 50000 and Both Combined rule
                                    }
                                    //START: Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation
                                    else 
                                    {
                                        bIsPartialAmtDeductionLegacyAccount = true;
                                    }
                                    //END: Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation

                                   if (bIsPartialAmtDeductionLegacyAccount)
                                    {
                                        //Partial Amount Reduction upon the percentage amount that is calculation from Account and Total projected Amount for individual accounts
                                        decAccountTotal = Math.Round(decYMCAPreTax / decPartialAmount, 2);
                                        dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = decYMCAPreTax - decAccountTotal;
                                        decAccountTotal = Math.Round(decYMCAAmt / decPartialAmount, 2);
                                        dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(decYMCAAmt - decAccountTotal);
                                    }
                                }
                                else if (strAcctType == "BA")
                                {
                                    if (strFundEventStatus != "QD") // Added by : Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation
                                    {
                                        //If current participant’s age is 55 & above and current YMCA Account balance is > 25k then money would not be removed from the YMCA Account 
                                        // Sum of YMCA Account Acount and YMCA (Legecy) Account are less than partial Withdrawal will do it. 
                                        // If current date greater than effective Date Combined account validate will execute OR
                                        // If current date less than effective Date individual account validate will execute
                                        // YMCA Account Acount are less than partial Withdrawal will do it.
                                        // Added : Chandrasekar| 2016.05.27 |YRS-AT-3014 - YRS enh: Configurable Withdrawals project (RetirementEstimates) (RetirementEstimates)
                                        // If YMCA Acount  < 25000 and No Combined rule Or If YMCA Acount  < 50000 and Combined rule 
                                        //if ((decYmcaLegecyAccountAndYmcaAccountTotal <= BA_MAX_LIMIT_55_ABOVE) || (decYmcaAccount <= BA_MAX_LIMIT_55_ABOVE))
                                        if ((decYmcaLegacyAccountAndYmcaAccountTotal <= BA_LEGACY_COMBINED_MAX_LIMIT) && (bIsBACombinedAccountRule))
                                        {
                                            #region Commented code
                                            //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim()) / decPartialAmount, 2);
                                            //dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim()) - decAccountTotal;
                                            //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) / decPartialAmount, 2);
                                            //dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) - decAccountTotal);
                                            #endregion Commented code

                                            bIsPartialAmtDeductionBAAccount = true;
                                        }
                                        else if (decYmcaAccount <= BA_MAX_LIMIT_55_ABOVE) // If YMCA Acount  <= 50000 or YMCA Acount  <= 25000  and Both rule 
                                        {
                                            bIsPartialAmtDeductionBAAccount = true;
                                        }
                                    }
                                    //START: Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation
                                    else 
                                    {
                                        bIsPartialAmtDeductionBAAccount = true;
                                    }
                                    //END: Chandrasekar - 2016.06.22 - YRS-AT-3010 - For "QD" there is no restriction of Account's limit validation
                                    if (bIsPartialAmtDeductionBAAccount)
                                    {
                                        //YMCA Acount Partial Amount Reduction upon the percentage amount that is calculation from Account and Total projected Amount for individual accounts
                                        decAccountTotal = Math.Round(decYMCAPreTax / decPartialAmount, 2);
                                        dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = decYMCAPreTax - decAccountTotal;
                                        decAccountTotal = Math.Round(decYMCAAmt / decPartialAmount, 2);
                                        dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(decYMCAAmt - decAccountTotal);
                                    }
                                }
                                else
                                {
                                    //Irrespective any accounts on a YMCA side money will deducted here
                                    #region Commented code
                                    //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim()) / decPartialAmount, 2);
                                    //dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim()) - decAccountTotal;
                                    //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) / decPartialAmount, 2);
                                    //dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) - decAccountTotal);
                                    #endregion Commented code


                                    //Partial Amount Reduction upon the percentage amount that is calculation from Account and Total projected Amount for individual accounts
                                    decAccountTotal = Math.Round(decYMCAPreTax / decPartialAmount, 2);
                                    dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = decYMCAPreTax - decAccountTotal;
                                    decAccountTotal = Math.Round(decYMCAAmt / decPartialAmount, 2);
                                    dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(decYMCAAmt - decAccountTotal);
                                }

                                //All money on personal side will deducted here.
                                #region Commented code
                                //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString().Trim()) / decPartialAmount, 2);
                                //dtAccountsBasisByProjection.Rows[iCount]["personalAmt"] = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString().Trim()) - decAccountTotal;
                                //dtAccountsBasisByProjection.Rows[iCount]["mnyBalance"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) + Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString().Trim()));
                                //dtAccountsBasisByProjection.Rows[iCount]["mnyymcacontribbalance"] = dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString();
                                #endregion Commented code


                                //Partial Amount Reduction upon the percentage amount that is calculation from Account and Total projected Amount for individual accounts
                                decAccountTotal = Math.Round(decPersonalAmt / decPartialAmount, 2);
                                dtAccountsBasisByProjection.Rows[iCount]["personalAmt"] = decPersonalAmt - decAccountTotal;
                                dtAccountsBasisByProjection.Rows[iCount]["mnyBalance"] = Convert.ToString(decYMCAAmt + decPersonalAmt);
                                dtAccountsBasisByProjection.Rows[iCount]["mnyymcacontribbalance"] = decYMCAAmt;

                            }
                        }
                    }
                }
                //End: Dinesh Kanojia(DK)     2013.07.14      BT-943: YRS 5.0-1443:Include partial withdrawals
                if (planType == "SAVINGS")
                {
                    //2013-10-24    Dinesh.k            BT:Attempted to divide by zero error on Retirement Partial Withdrawal Estimate.
                    if (bIspartial && decPartialValue > 0)
                    {
                        decimal decTotalProjRet = 0, decPartialAmount = 0, decAccountTotal = 0;
                        decTotalProjRet = 0;
                        decTotalProjRet = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(mnyBalance)", "chvPlanType='" + planType + "'"));
                        decPartialAmount = decTotalProjRet / decPartialValue;

                        if (decPartialAmount > 0)
                        {
                            for (int iCount = 0; iCount <= dtAccountsBasisByProjection.Rows.Count - 1; iCount++)
                            {
                                //Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Refactoring for instead of fetching from data table instantly, assigned value to local variable’s used throughout below code
                                decPersonalPreTax = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnypersonalpretax"].ToString().Trim());
                                decYMCAAmt = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim());
                                decPersonalAmt = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString().Trim());
                                decYMCAPreTax = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim());

                                if (dtAccountsBasisByProjection.Rows[iCount]["chvPlanType"].ToString().Trim().ToUpper() == planType)
                                {
                                    // Personal Side money is deducted here
                                    #region Commented Code
                                    //decAccountTotal = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnypersonalpretax"].ToString().Trim()) / decPartialAmount;
                                    //dtAccountsBasisByProjection.Rows[iCount]["mnypersonalpretax"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnypersonalpretax"].ToString().Trim()) - decAccountTotal);
                                    //decAccountTotal = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalamt"].ToString().Trim()) / decPartialAmount;
                                    //dtAccountsBasisByProjection.Rows[iCount]["personalamt"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalamt"].ToString().Trim()) - decAccountTotal);
                                    //dtAccountsBasisByProjection.Rows[iCount]["mnyBalance"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString().Trim()) + Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["YMCAAmt"].ToString().Trim()));
                                    //dtAccountsBasisByProjection.Rows[iCount]["mnypersonalcontribbalance"] = dtAccountsBasisByProjection.Rows[iCount]["personalAmt"].ToString();

                                    //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim()) / decPartialAmount, 2);
                                    //dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"].ToString().Trim()) - decAccountTotal;
                                    //decAccountTotal = Math.Round(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) / decPartialAmount, 2);
                                    //dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(Convert.ToDecimal(dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"].ToString().Trim()) - decAccountTotal);
                                    #endregion

                                    //Start:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Refactoring for instead of fetching from data table instantly, assigned value to local variable’s used throughout below code
                                    //Partial Amount Reduction upon the percentage amount that is calculation from Account and as well as projected Total of accounts
                                    decAccountTotal = decPersonalPreTax / decPartialAmount;
                                    dtAccountsBasisByProjection.Rows[iCount]["mnypersonalpretax"] = Convert.ToString(decPersonalPreTax - decAccountTotal);
                                    decAccountTotal = decPersonalAmt / decPartialAmount;
                                    dtAccountsBasisByProjection.Rows[iCount]["personalamt"] = Convert.ToString(decPersonalAmt - decAccountTotal);
                                    dtAccountsBasisByProjection.Rows[iCount]["mnyBalance"] = Convert.ToString(decPersonalAmt + decYMCAAmt);
                                    dtAccountsBasisByProjection.Rows[iCount]["mnypersonalcontribbalance"] = Convert.ToString(decPersonalAmt);
                                    //// Ymca Side money is deducted here

                                    decAccountTotal = Math.Round(decYMCAPreTax / decPartialAmount, 2);
                                    dtAccountsBasisByProjection.Rows[iCount]["mnyymcapretax"] = decYMCAPreTax - decAccountTotal;
                                    decAccountTotal = Math.Round(decYMCAAmt / decPartialAmount, 2);
                                    dtAccountsBasisByProjection.Rows[iCount]["ymcaamt"] = Convert.ToString(decYMCAAmt - decAccountTotal);
                                    //End:Added by Chandrasekar.c on 2015.12.16 YRS-AT-2479 : Partial Withdrawal Estimate - Refactoring for instead of fetching from data table instantly, assigned value to local variable’s used throughout below code
                                }
                            }
                        }
                    }
                }

                CreateAcctBalancesByBasisType(dtAccountsBasisByProjection, planType, "");

                #region Commented code

                //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                //DataRow []drAcctProjectionFoundRows=null;
                //DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
                //if(g_dtAnnuityBasisTypeList!=null)
                //{
                //    if(g_dtAnnuityBasisTypeList.Rows.Count >0)
                //    {
                //        if(lnRETIREMENT_PROJECTION_PHASE != 0)
                //        {
                //            foreach(DataRow drAnnuityBasisTypeList in g_dtAnnuityBasisTypeList.Rows )
                //            {
                //                 Personal_Retirement_Balance=0;
                //                 YMCA_Retirement_Balance=0;
                //                 Basic_Retirement_Balance=0;								
                //                string annuityBasisType=string.Empty ;
                //                string annuityBasisGroup=string.Empty ;

                //                annuityBasisType=drAnnuityBasisTypeList["chrAnnuityBasisType"].ToString().Trim().ToUpper(); 
                //                annuityBasisGroup=drAnnuityBasisTypeList["chrAnnuityBasisGroup"].ToString().Trim().ToUpper(); 

                //                drAcctProjectionFoundRows=dtAccountsBasisByProjection.Select("chrAnnuityBasisType='" +annuityBasisType+"'");  
                //                if(drAcctProjectionFoundRows.Length >0)
                //                {

                //                    if (dtAccountsBasisByProjection.Select("intProjPeriodSequence<=" + lnRETIREMENT_PROJECTION_PHASE + "AND chrAnnuityBasisType='"+annuityBasisType+"' AND chvPlanType = '" + planType + "'").Length > 0)
                //                    {
                //                        //Personal_Retirement_Balance
                //                        Personal_Retirement_Balance = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "intProjPeriodSequence<=" + lnRETIREMENT_PROJECTION_PHASE + "AND chrAnnuityBasisType='"+annuityBasisType+"' AND chvPlanType = '" + planType + "'").ToString());

                //                        //YMCA_Retirement_Balance					
                //                        YMCA_Retirement_Balance = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "intProjPeriodSequence<=" + lnRETIREMENT_PROJECTION_PHASE + "AND chrAnnuityBasisType='"+annuityBasisType+"' AND chvPlanType = '" + planType + "'").ToString());
                //                    }

                //                    //Basic_AcctRetirement_Balance
                //                    if (l_dsMetaAccountTypes.Tables.Count != 0)
                //                    {
                //                        foreach(DataRow drMetaAccts in l_dsMetaAccountTypes.Tables[1].Rows)
                //                            foreach(DataRow drProjection in dtAccountsBasisByProjection.Rows)
                //                            {
                //                                if (drMetaAccts["chrAcctType"].ToString().ToUpper().Trim() == drProjection["chrAcctType"].ToString().ToUpper().Trim() 
                //                                    && drProjection["chrAnnuityBasisType"].ToString().ToUpper().Trim() == annuityBasisType 
                //                                    && Convert.ToInt32(drProjection["intProjPeriodSequence"].ToString()) <= lnRETIREMENT_PROJECTION_PHASE 
                //                                    && drMetaAccts["bitBasicAcct"].ToString() == "True"
                //                                    && drProjection["chvPlanType"].ToString() == planType) 
                //                                    Basic_Retirement_Balance += 
                //                                        Convert.ToDecimal(drProjection["PersonalAmt"]) + Convert.ToDecimal(drProjection["YmcaAmt"]);
                //                            }
                //                    }

                //                    //Added balances into g_dtAcctBalancesByBasisType table
                //                    AddAcctBalancesByBasisTypeRow(Personal_Retirement_Balance,YMCA_Retirement_Balance,Basic_Retirement_Balance
                //                                    ,annuityBasisType,annuityBasisGroup ); 

                //                }//if(drAcctProjectionFoundRows.Length >0)
                //            } //foreach(DataRow drAnnuityBasisTypeList in g_dtAnnuityBasisTypeList.Rows )

                //            if (lnRETIREDATE_TO_AGE60 != 0)
                //            {
                //                if (l_dsMetaAccountTypes.Tables.Count != 0)
                //                {
                //                    foreach(DataRow drMetaAccts in l_dsMetaAccountTypes.Tables[1].Rows)
                //                        foreach(DataRow drProjection in dtAccountsBasisByProjection.Rows)
                //                        {
                //                            if (drMetaAccts["chrAcctType"].ToString().ToUpper().Trim() == drProjection["chrAcctType"].ToString().ToUpper().Trim() 
                //                                && Convert.ToInt32(drProjection["intProjPeriodSequence"].ToString()) <= lnRETIREDATE_TO_AGE60 
                //                                && drMetaAccts["bitBasicAcct"].ToString() == "True"
                //                                && drProjection["chvPlanType"].ToString() == planType) 
                //                            {
                //                                //Basic_AcctYMCA_Age60_Balance
                //                                ycProjected_Basic_YMCA_Age60_Balance += Convert.ToDecimal(drProjection["YmcaAmt"]);

                //                                // Basic_AcctPersonal_Age60_Balance
                //                                ycProjected_Basic_Personal_Age60_Balance += Convert.ToDecimal(drProjection["PersonalAmt"]);
                //                            }
                //                        }
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion Commented code
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Ashish for Phase V changes, This method Rename as removeExcludedAccounts_Old and create new method with same name
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtAccountsBasisByProjection"></param>
        /// <param name="dtExcludedAccounts"></param>
        /// <returns></returns>
        private DataTable removeExcludedAccounts_Old(DataTable dtAccountsBasisByProjection, DataTable dtExcludedAccounts)
        {
            DataTable dtExcludeFromThis = dtAccountsBasisByProjection.Copy();
            bool boolBasicAccountExcluded = false;

            foreach (DataRow dr in dtExcludedAccounts.Rows)
            {
                foreach (DataRow drProj in dtExcludeFromThis.Rows)
                {
                    if (drProj["chrAcctType"].ToString() == dr["chrAcctType"].ToString())
                    {
                        //checking if the account excluded by user is a basic account and plan type is retirement
                        if (drProj["chvPlanType"].ToString() == "RETIREMENT" && Convert.ToBoolean(dr["bitBasicAcct"]) == true)
                        {
                            //then exclude only the personal side of money for the excluded basic account.
                            drProj["PersonalAmt"] = 0;

                            drProj["mnypersonalpretax"] = 0;
                            drProj["mnypersonalposttax"] = 0;

                            boolBasicAccountExcluded = true; //flagging if any single basic account is excluded from the estimate
                        }
                        else
                        {
                            //exclude the personal as well as YMCA side of money for any excluded non-basic account of any plan.
                            //if (drProj["chvPlanType"].ToString() == "RETIREMENT")
                            //{								
                            drProj["PersonalAmt"] = 0;
                            drProj["YmcaAmt"] = 0;

                            drProj["mnypersonalpretax"] = 0;
                            drProj["mnypersonalposttax"] = 0;
                            drProj["mnyymcapretax"] = 0;
                            //}
                        }
                    }
                }
            }

            //if any single basic account is excluded from the estimate then 
            //exclude personal side of money from all the basic accounts 
            //and both personal as well as ymca side of money from non-basic account under retirement plan.
            if (boolBasicAccountExcluded == true)
            {
                //this dataset will be populated with only the basic accounts
                DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();

                foreach (DataRow drProj in dtExcludeFromThis.Rows)
                {
                    if (drProj["chvPlanType"].ToString() == "RETIREMENT")
                    {
                        //string filterExpression = "chrAcctType='" + drProj["chrAcctType"].ToString() + "' AND bitBasicAcct = 'True'";
                        //checking if the account is a basic account then excluding only the personal otherwise personal as well as ymca side of money is excluded from the account.
                        string filterExpression = "chrAcctType='" + drProj["chrAcctType"].ToString() + "'";

                        //if (dtExcludedAccounts.Select(filterExpression).Length > 0)
                        if (l_dsMetaAccountTypes.Tables[0].Select(filterExpression).Length > 0)//basic account
                        {
                            drProj["PersonalAmt"] = 0;
                        }
                        else//non-basic account
                        {
                            drProj["PersonalAmt"] = 0;
                            drProj["YmcaAmt"] = 0;
                        }
                    }
                }
            }

            dtExcludeFromThis.AcceptChanges();

            return dtExcludeFromThis;
        }

        /// <summary>
        /// Updates the CurrentModified salary for all the employments to the one specified by the user.
        /// For RetProcessing though it sets it to the value specified in the database as numModifiedSalary
        /// </summary>
        /// <param name="dtEmploymentProjectedSalary"></param>
        /// <param name="dtRetEstimateEmployment"></param>
        /// <param name="lnMaximumContributionSalary"></param>
        /// <param name="modifiedSal"></param>
        /// <param name="isEstimate"></param>
        private static void createProjectedSalary(DataTable dtEmploymentProjectedSalary, DataTable dtRetEstimateEmployment,
            double lnMaximumContributionSalary, string modifiedSal, bool isEstimate)
        {
            dtEmploymentProjectedSalary.Columns.Add("guiYmcaID");
            dtEmploymentProjectedSalary.Columns.Add("guiEmpEventID");
            dtEmploymentProjectedSalary.Columns.Add("dtmTerminationDate", typeof(DateTime));
            dtEmploymentProjectedSalary.Columns.Add("CurrentProjectedSalary", typeof(decimal));
            //START: SR | 2018.08.28 | YRS-AT-3790 |  Added new column to maintain adjusted salary.
            if (!dtEmploymentProjectedSalary.Columns.Contains("UseSalary"))
            {
                dtEmploymentProjectedSalary.Columns.Add("UseSalary", typeof(decimal));
                //Set the Default Value.
                dtEmploymentProjectedSalary.Columns["UseSalary"].DefaultValue = 0;
            }
            //END: SR | 2018.08.28 | YRS-AT-3790 |  Added new column to maintain adjusted salary.

            dtEmploymentProjectedSalary.AcceptChanges();
            DataRow drEmploymentProjectedSalary;
            if (dtRetEstimateEmployment.Rows.Count != 0)
            {
                foreach (DataRow dr in dtRetEstimateEmployment.Rows)
                {
                    drEmploymentProjectedSalary = dtEmploymentProjectedSalary.NewRow();
                    // TODO Integrate the modified salary details for multiple active employment here.
                    if (isEstimate)
                    {
                        // For all active employments
                        // Update CurrentProjected Salary to the one specified by the user
                        if ((dr["numModifiedSalary"] != DBNull.Value && dr["dtmTerminationDate"] == DBNull.Value)
                            || (dr["numModifiedSalary"] == DBNull.Value && dr["dtmTerminationDate"] == DBNull.Value))
                        {
                            if (dr["guiYmcaID"] != DBNull.Value)
                                drEmploymentProjectedSalary["guiYmcaID"] = dr["guiYmcaID"].ToString();
                            if (dr["guiUniqueID"] != DBNull.Value)
                                drEmploymentProjectedSalary["guiEmpEventID"] = dr["guiUniqueID"].ToString();
                            if (dr["dtmTerminationDate"] != DBNull.Value)
                                drEmploymentProjectedSalary["dtmTerminationDate"] = dr["dtmTerminationDate"];
                            drEmploymentProjectedSalary["CurrentProjectedSalary"] = modifiedSal;
                            dtEmploymentProjectedSalary.Rows.Add(drEmploymentProjectedSalary);
                            dtEmploymentProjectedSalary.GetChanges(DataRowState.Added);
                        }
                    }
                    else // If it is estimate just take a side copy dont change a thing.
                    {
                        if (dr["guiYmcaID"] != DBNull.Value)
                            drEmploymentProjectedSalary["guiYmcaID"] = dr["guiYmcaID"].ToString();
                        if (dr["guiUniqueID"] != DBNull.Value)
                            drEmploymentProjectedSalary["guiEmpEventID"] = dr["guiUniqueID"].ToString();
                        if (dr["dtmTerminationDate"] != DBNull.Value)
                            drEmploymentProjectedSalary["dtmTerminationDate"] = dr["dtmTerminationDate"];
                        if (dr["numModifiedSalary"] != DBNull.Value)
                            drEmploymentProjectedSalary["CurrentProjectedSalary"] = dr["numModifiedSalary"];
                        dtEmploymentProjectedSalary.Rows.Add(drEmploymentProjectedSalary);
                        dtEmploymentProjectedSalary.GetChanges(DataRowState.Added);
                    }
                }
                dtEmploymentProjectedSalary.AcceptChanges();
            }

            // If modified salary is greater than maxPermitted then reset it.
            foreach (DataRow dr in dtEmploymentProjectedSalary.Rows)
            {
                if (dr["CurrentProjectedSalary"] != DBNull.Value)
                {
                    if (Convert.ToDouble(dr["CurrentProjectedSalary"].ToString()) > lnMaximumContributionSalary / 12)
                    {
                        dr["CurrentProjectedSalary"] = lnMaximumContributionSalary / 12;
                        dtEmploymentProjectedSalary.GetChanges(DataRowState.Modified);
                        dtEmploymentProjectedSalary.AcceptChanges();
                    }
                }
            }
        }


        /// <summary>
        /// Get all those accounts into dtAccountsByBasis that have been newly specified by user		
        /// </summary>
        /// <param name="dtAccountsByBasis"></param>
        /// <param name="dsElectiveAccounts"></param>
        private static void getFutureAccountDetails(DataTable dtAccountsByBasis, DataSet dsElectiveAccounts)
        {
            DataTable dtFutureAcctsTmp = new DataTable();
            DataRow drFutureAcctsTmp;
            dtFutureAcctsTmp.Columns.Add("chrAcctType");
            dtFutureAcctsTmp.Columns.Add("dtmEffDate");
            dtFutureAcctsTmp.Columns.Add("chvPlanType");
            dtFutureAcctsTmp.AcceptChanges();

            DataTable dtRetEstEmpElectives = new DataTable();
            if (dsElectiveAccounts != null)
                dtRetEstEmpElectives = dsElectiveAccounts.Tables[0];
            //2011.10.05 YRS 5.0-1428
            //if (dtAccountsByBasis.Rows.Count   != 0)
            if (dtAccountsByBasis != null)
            {
                string previousAccountType = string.Empty;
                foreach (DataRow dr in dtRetEstEmpElectives.Rows)
                {
                    string accountType = dr["chrAcctType"].ToString().Trim().ToUpper();
                    // If elective acct is not already added in ActByBasis, Add it to dtFutureAcctsTmp
                    if (dtAccountsByBasis.Select("chrAcctType = '" + accountType + "'").Length == 0)
                    {
                        if (dtFutureAcctsTmp.Rows.Count != 0)
                        {
                            drFutureAcctsTmp = dtFutureAcctsTmp.NewRow();
                            if (previousAccountType != accountType)
                            {
                                drFutureAcctsTmp["chrAcctType"] = dr["chrAcctType"].ToString().Trim().ToUpper();
                                drFutureAcctsTmp["dtmEffDate"] = dr["dtmEffDate"].ToString().Trim().ToUpper();
                                drFutureAcctsTmp["chvPlanType"] = dr["PlanType"].ToString();
                                dtFutureAcctsTmp.Rows.Add(drFutureAcctsTmp);
                                dtFutureAcctsTmp.GetChanges(DataRowState.Added);
                                dtFutureAcctsTmp.AcceptChanges();
                            }
                        }
                        else // Add the first row
                        {
                            drFutureAcctsTmp = dtFutureAcctsTmp.NewRow();
                            drFutureAcctsTmp["chrAcctType"] = dr["chrAcctType"].ToString().Trim().ToUpper();
                            drFutureAcctsTmp["dtmEffDate"] = dr["dtmEffDate"].ToString().Trim().ToUpper();
                            drFutureAcctsTmp["chvPlanType"] = dr["PlanType"].ToString();
                            dtFutureAcctsTmp.Rows.Add(drFutureAcctsTmp);
                            dtFutureAcctsTmp.GetChanges(DataRowState.Added);
                            dtFutureAcctsTmp.AcceptChanges();
                        }
                    }
                    previousAccountType = dr["chrAcctType"].ToString().Trim().ToUpper();
                }
            }

            // Insert the future accounts into dtAccountsByBasis
            foreach (DataRow dr in dtFutureAcctsTmp.Rows)
            {
                DataRow drAccountsByBasis = dtAccountsByBasis.NewRow();
                drAccountsByBasis["chraccttype"] = dr["chrAcctType"].ToString();
                //Commented by Ashish for phase V changes
                //drAccountsByBasis["chrAnnuityBasisType"] = "PST96";
                //Added by Ashish for phase V changes
                drAccountsByBasis["chrAnnuityBasisType"] = "";
                drAccountsByBasis["mnyPersonalPreTax"] = 0;
                drAccountsByBasis["mnyPersonalPostTax"] = 0;
                drAccountsByBasis["mnyYmcaPreTax"] = 0;
                drAccountsByBasis["PersonalAmt"] = 0;
                drAccountsByBasis["YmcaAmt"] = 0;
                drAccountsByBasis["mnyBalance"] = 0;
                drAccountsByBasis["mnyYmcaInterestBalance"] = 0;
                drAccountsByBasis["mnyYmcaContribBalance"] = 0;
                drAccountsByBasis["mnyPersonalInterestBalance"] = 0;
                drAccountsByBasis["mnyPersonalContribBalance"] = 0;
                drAccountsByBasis["chvPlanType"] = dr["chvPlanType"].ToString();
                dtAccountsByBasis.Rows.Add(drAccountsByBasis);
                dtAccountsByBasis.GetChanges(DataRowState.Added);
                dtAccountsByBasis.AcceptChanges();
            }
        }


        /// <summary>
        /// Add all those accounts for which YMCA resolution is present and 
        /// that account is not in dtAccountsByBasis
        /// </summary>
        /// <param name="dtAccountsByBasis"></param>
        /// <param name="personID"></param>
        private static void getYMCAResolutions(DataTable dtAccountsByBasis, DataSet l_dsYmcaResolutions)
        {
            //ASHISH:2011.02.21 Add parameter for YmcaResolution, isted from getting database
            //DataSet l_dsYmcaResolutions = RetirementBOClass.SearchYmcaResolutions(personID, planType);
            DataTable dtYmcaResolutionsDistinct = null;
            try
            {
                if (dtAccountsByBasis != null && l_dsYmcaResolutions != null)
                {
                    dtYmcaResolutionsDistinct = new DataTable();
                    if (l_dsYmcaResolutions.Tables.Count != 0)
                    {
                        if (l_dsYmcaResolutions.Tables.Count > 1)
                            dtYmcaResolutionsDistinct = l_dsYmcaResolutions.Tables[1];
                    }

                    foreach (DataRow dr in dtYmcaResolutionsDistinct.Rows)
                    {
                        // Add all those accounts for which YMCA resolution is present and 
                        // that account is not in dtAccountsByBasis
                        if (dtAccountsByBasis.Select("chrAcctType='" + dr["chrBasicAcctType"] + "'").Length == 0)
                        {
                            DataRow drAccountsByBasis = dtAccountsByBasis.NewRow();
                            drAccountsByBasis["chraccttype"] = dr["chrBasicAcctType"].ToString().Trim().ToUpper();
                            //commented by Ashish for pahse V part III changes
                            //drAccountsByBasis["chrAnnuityBasisType"] = "PST96";
                            //Added by Ashish for pahse V part III changes
                            drAccountsByBasis["chrAnnuityBasisType"] = "";
                            drAccountsByBasis["mnyPersonalPreTax"] = 0;
                            drAccountsByBasis["mnyPersonalPostTax"] = 0;
                            drAccountsByBasis["mnyYmcaPreTax"] = 0;
                            drAccountsByBasis["PersonalAmt"] = 0;
                            drAccountsByBasis["YmcaAmt"] = 0;
                            drAccountsByBasis["mnyBalance"] = 0;
                            drAccountsByBasis["mnyYmcaInterestBalance"] = 0;
                            drAccountsByBasis["mnyYmcaContribBalance"] = 0;
                            drAccountsByBasis["mnyPersonalInterestBalance"] = 0;
                            drAccountsByBasis["mnyPersonalContribBalance"] = 0;
                            dtAccountsByBasis.Rows.Add(drAccountsByBasis);
                            dtAccountsByBasis.GetChanges(DataRowState.Added);
                            dtAccountsByBasis.AcceptChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Calculating planwise balances.
        /// </summary>
        /// <param name="dtAccountsBasisByProjection"></param>
        private void calculatePlanBalances(DataTable dtAccountsBasisByProjection)
        {
            if (dtAccountsBasisByProjection.Rows.Count > 0)
            {
                string retPersonalAmt = dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "chvPlanType='RETIREMENT'").ToString();
                string retYMCAAmt = dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "chvPlanType='RETIREMENT'").ToString();
                if (retPersonalAmt == string.Empty)
                    retPersonalAmt = "0";
                if (retYMCAAmt == string.Empty)
                    retYMCAAmt = "0";

                retirementPlanBalance = Convert.ToDecimal(retPersonalAmt) + Convert.ToDecimal(retYMCAAmt);

                string savPersonalAmt = dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "chvPlanType='SAVINGS'").ToString();
                string savYMCAAmt = dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "chvPlanType='SAVINGS'").ToString();
                if (savPersonalAmt == string.Empty)
                    savPersonalAmt = "0";
                if (savYMCAAmt == string.Empty)
                    savYMCAAmt = "0";

                savingsPlanBalance = Convert.ToDecimal(savPersonalAmt) + Convert.ToDecimal(savYMCAAmt);
            }
            else
            {
                retirementPlanBalance = 0;
                savingsPlanBalance = 0;
            }
        }
        /// <summary>
        /// TODO: At this point check the balances in each account and check if it is more than 5000 
        /// If it is not more than 5000 for the selected Plan type raise error message 
        ///  1. Discard the funds for non qualifying Plan 
        ///  2. Raise error message.
        /// </summary>
        /// <returns></returns>
        private string checkAccountBalanceLimits(string fundStatus, string planType, string personID)
        {
            string errorMessage = string.Empty;
            string strMessageBothPlanType = "MESSAGE_RETIREMENT_BOC_NOT_ENOUGH_FUNDS";//added on 24-sep for BT-1126
            string strMessageRetSavPlanType = "MESSAGE_RETIREMENT_BOC_NOT_ENOUGH_FUNDS_SELECTED_PLANTYPE";//added on 2016.02.22 | CS | YRS-AT-2752 
            //ASHISH:2011.10.13 BT-917
            //this.GetPurchasedAnnuityDetails(personID);

            //Check if fund status in ('RT','RPT','RA','RDNP','RE') and 
            // Check if FundStatus is RT, 
            // Then dont run the check on the Used Plan

            //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            //if(fundStatus != "RT" && fundStatus != "RD" && fundStatus != "RP" && fundStatus != "RA")			
            if (fundStatus != "RT" && fundStatus != "RPT" && fundStatus != "RD" && fundStatus != "RP" && fundStatus != "RA" && fundStatus != "RE" && fundStatus != "RDNP")
            {
                if (retirementPlanBalance < 5000 || savingsPlanBalance < 5000)
                {
                    errorMessage = string.Empty;
                    if (planType == "R" && retirementPlanBalance < 5000)
                    {
                        if (savingsPlanBalance >= 5000)
                            errorMessage = "R";//"There is not enough fund in the Retirement plan of the participant. Please specify additional funds to proceed.";
                        else
                            //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                            //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                            errorMessage = strMessageRetSavPlanType;//added on 2016.02.22 | CS | YRS-AT-2752 
                        //End 11-Feb-09
                    }
                    else if (planType == "S" && savingsPlanBalance < 5000)
                    {
                        if (retirementPlanBalance >= 5000)
                            errorMessage = "S";//"There is not enough fund in the Savings plan of the participant. Please specify additional funds to proceed.";
                        else
                            //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                            //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                            errorMessage = strMessageRetSavPlanType;//added on 2016.02.22 | CS | YRS-AT-2752 
                        //End 11-Feb-09

                    }
                    else if (planType == "B")
                    {
                        if (retirementPlanBalance < 5000 && savingsPlanBalance >= 5000)
                            errorMessage = "R";//"There is not enough fund in the Retirement plan of the participant. Please specify additional funds to proceed.";
                        else if (retirementPlanBalance >= 5000 && savingsPlanBalance < 5000)
                            errorMessage = "S";//"There is not enough fund in the Savings plan of the participant. Please specify additional funds to proceed.";
                        else if (retirementPlanBalance < 5000 && savingsPlanBalance < 5000)
                            //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                            //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                            errorMessage = strMessageBothPlanType; //added on 2016.02.22 | CS | YRS-AT-2752 
                        //End 11-Feb-09
                    }
                }
            }//ASHISH:2011.10.13 BT-917 Commented condition
            //else if (
            //    ((fundStatus == "RD") || (fundStatus == "RP")) ||
            //    ((fundStatus == "RT") && (Convert.ToDateTime(this.RetirementDate) < RetirementBOClass.GetPlanSplitDate())) || // Previous retirement taken before PlanSplit
            //    ((fundStatus == "RPT") && (Convert.ToDateTime(this.RetirementDate) < RetirementBOClass.GetPlanSplitDate())) || // Previous retirement taken before PlanSplit //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
            //    ((fundStatus == "RA") && (Convert.ToDateTime(this.RetirementDate) < RetirementBOClass.GetPlanSplitDate())) || // Previous retirement taken before PlanSplit
            //    ((fundStatus == "RDNP") && (Convert.ToDateTime(this.RetirementDate) < RetirementBOClass.GetPlanSplitDate())) || // Previous retirement taken before PlanSplit
            //    ((fundStatus == "RE") && (Convert.ToDateTime(this.RetirementDate) < RetirementBOClass.GetPlanSplitDate())) // Previous retirement taken before PlanSplit
            //    )
            else if (
                (fundStatus == "RD") || (fundStatus == "RP") ||
                (fundStatus == "RT") || // Previous retirement taken before PlanSplit
                (fundStatus == "RPT") || // Previous retirement taken before PlanSplit //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
                (fundStatus == "RA") || // Previous retirement taken before PlanSplit
                (fundStatus == "RDNP") || // Previous retirement taken before PlanSplit
                (fundStatus == "RE")  // Previous retirement taken before PlanSplit
                )
            {
                //ASHISH:2011.10.13 BT-917               

                this.GetPurchasedAnnuityDetails(personID);
                DateTime planSplitDate = RetirementBOClass.GetPlanSplitDate();
                if ((fundStatus == "RD") || (fundStatus == "RP"))
                {
                    errorMessage = ValidateRetiredParticipantPlanWiseBalances_LessOrEqualToZero(this.retirementPlanBalance, this.savingsPlanBalance, planType);
                }
                else
                {
                    if (this.existingAnnuityPurchaseDate == string.Empty)
                        //return errorMessage = "Annuity details are missing for the participant, cannot proceed.";//commented on 24-sep for BT-1126
                        return errorMessage = "MESSAGE_RETIREMENT_BOC_PARICIPANT_ANNUITY_DETAILS_MISSING";
                    if (Convert.ToDateTime(this.existingAnnuityPurchaseDate) < planSplitDate)
                    {
                        errorMessage = ValidateRetiredParticipantPlanWiseBalances_LessOrEqualToZero(this.retirementPlanBalance, this.savingsPlanBalance, planType);
                    }
                }

                #region
                //if (retirementPlanBalance <= 0 || savingsPlanBalance <= 0)
                //{
                //    errorMessage = string.Empty;
                //    if (planType == "R" && retirementPlanBalance <= 0)
                //    {
                //        if (savingsPlanBalance > 0)
                //            errorMessage = "R";
                //        else
                //            //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                //            //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                //            errorMessage = strMessage;
                //        //End 11-Feb-09
                //    }
                //    else if (planType == "S" && savingsPlanBalance <= 0)
                //    {
                //        if (retirementPlanBalance > 0)
                //            errorMessage = "S";
                //        else
                //            //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                //            //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                //            errorMessage = strMessage;
                //        //End 11-Feb-09
                //    }
                //    else if (planType == "B")
                //    {
                //        if (retirementPlanBalance <= 0 && savingsPlanBalance > 0)
                //            errorMessage = "R";
                //        else if (retirementPlanBalance > 0 && savingsPlanBalance <= 0)
                //            errorMessage = "S";
                //        else if (retirementPlanBalance <= 0 && savingsPlanBalance <= 0)
                //            //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                //            //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                //            errorMessage = strMessage;
                //        //End 11-Feb-09
                //    }
                //}
                #endregion
            }
            else // fundStatus == "RT" or RA
            {
                if (!RetiredOnRetirementPlan && (planType == "R" || planType == "B") && retirementPlanBalance < 5000)
                {
                    if (savingsPlanBalance > 0)
                        errorMessage = "R";
                    else
                    {
                        //START - 2016.02.22 | CS | YRS-AT-2752 
                        if (planType == "B")
                            errorMessage = strMessageBothPlanType;
                        else
                            errorMessage = strMessageRetSavPlanType;
                        //END - 2016.02.22 | CS | YRS-AT-2752 
                    }
                    //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                    //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";

                    //End 11-Feb-09
                }
                else if (!RetiredOnSavingsPlan && (planType == "S" || planType == "B") && savingsPlanBalance < 5000)
                {
                    if (retirementPlanBalance > 0)
                        errorMessage = "S";
                    else
                    {
                        //START - 2016.02.22 | CS | YRS-AT-2752 
                        if (planType == "B")
                            errorMessage = strMessageBothPlanType;
                        else
                            errorMessage = strMessageRetSavPlanType;
                        //END - 2016.02.22 | CS | YRS-AT-2752 
                    }
                    //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                    //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                    // errorMessage = strMessageBothPlanType;
                    //End 11-Feb-09
                }
                //if(RetiredOnRetirementPlan && planType == "R" && retirementPlanBalance == 0)
                if (RetiredOnRetirementPlan && (planType == "R" || planType == "B") && retirementPlanBalance == 0)
                {
                    if (savingsPlanBalance > 0)
                        errorMessage = "R";
                    else
                    {
                        //START - 2016.02.22 | CS | YRS-AT-2752 
                        if (planType == "B")
                            errorMessage = strMessageBothPlanType;
                        else
                            errorMessage = strMessageRetSavPlanType;
                        //END - 2016.02.22 | CS | YRS-AT-2752 
                    }
                    //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                    //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                    //  errorMessage = strMessageBothPlanType;
                    //End 11-Feb-09
                }
                //else if(RetiredOnSavingsPlan && planType == "S" && savingsPlanBalance == 0)
                else if (RetiredOnSavingsPlan && (planType == "S" || planType == "B") && savingsPlanBalance == 0)
                {
                    if (retirementPlanBalance > 0)
                        errorMessage = "S";
                    else
                    {
                        //START - 2016.02.22 | CS | YRS-AT-2752 
                        if (planType == "B")
                            errorMessage = strMessageBothPlanType;
                        else
                            errorMessage = strMessageRetSavPlanType;
                        //END - 2016.02.22 | CS | YRS-AT-2752 
                    }
                    //Priya 11-Feb-09 : YRS 5.0-688 Change the wording "Fund" to "Funds"
                    //errorMessage = "There is not enough fund in both the plans of the participant. Please specify additional funds to proceed.";
                    // errorMessage = strMessageBothPlanType;
                    //End 11-Feb-09
                }
            }

            return errorMessage;
        }
        //ASHISH:2011.10.14 BT-971
        private string ValidateRetiredParticipantPlanWiseBalances_LessOrEqualToZero(decimal p_RetirementPlanBalance, decimal p_SavingPlanBalance, string p_PlanType)
        {
            string returnPlanType = string.Empty;

            if (p_RetirementPlanBalance <= 0 || p_SavingPlanBalance <= 0)
            {
                if (p_PlanType == "R" && p_RetirementPlanBalance <= 0)
                    returnPlanType = "R";
                else if (p_PlanType == "S" && p_SavingPlanBalance <= 0)
                    returnPlanType = "S";
                else if (p_PlanType == "B")
                {
                    if (p_RetirementPlanBalance <= 0 && p_SavingPlanBalance <= 0)
                        returnPlanType = "B";
                    else if (p_RetirementPlanBalance > 0 && p_SavingPlanBalance <= 0)
                        returnPlanType = "S";
                    else if (p_SavingPlanBalance > 0 && p_RetirementPlanBalance <= 0)
                        returnPlanType = "R";
                }

            }

            return returnPlanType;
        }

        /// <summary>
        /// Get the list of plans on which the participant can retire.
        /// </summary>
        /// <param name="personID"></param>
        /// <param name="fundStatus"></param>
        /// <returns></returns>
        public static string GetActivePlansForEstimate(string personID, string fundEventID, string fundStatus)//, string previousfundStatus)
        {
            DataSet accountBalance;
            decimal savingsBalance = 0;
            decimal retirementBalance = 0;
            string activeAccounts = "B";
            try
            {
                //as per the requirement given by Mark through mail dated dt:Monday, May 19, 2008 7:20 PM.
                //if(fundStatus == "RT")
                if ((fundStatus == "RT") || (fundStatus == "RPT"))
                {
                    activeAccounts = string.Empty;
                    accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceData(false, "01/01/1900", DateTime.Today.ToString(), personID, fundEventID, false, true, "BOTH");
                    if (accountBalance.Tables.Count == 2)
                    {
                        if (accountBalance.Tables[1].Rows.Count > 0)
                            foreach (DataRow dr in accountBalance.Tables[1].Rows)
                            {
                                //Calculate Savings balance
                                if (dr[0].ToString().ToUpper() == "RETIREMENT")
                                    retirementBalance = Convert.ToDecimal(dr["Balance"].ToString());

                                //Calculate Retirement balance
                                if (dr[0].ToString().ToUpper() == "SAVINGS")
                                    savingsBalance = Convert.ToDecimal(dr["Balance"].ToString());
                            }
                        if (retirementBalance > 0 && savingsBalance > 0)
                            activeAccounts = "B";
                        else if (retirementBalance > 0 && savingsBalance <= 0)
                            activeAccounts = "R";
                        else if (retirementBalance <= 0 && savingsBalance > 0)
                            activeAccounts = "S";
                    }
                }
            }
            catch
            {
                throw;
            }

            return activeAccounts;
        }

        /// <summary>
        /// Get the list of elective accounts for.
        /// </summary>
        /// <param name="ssNo"></param>
        /// <returns></returns>
        //public static DataSet GetElectiveAccountsByPlan(string personID)
        //public static DataSet GetElectiveAccountsByPlan(string personID, string planType)//Phase IV Changes
        //		public static DataSet GetElectiveAccountsByPlan(string fundEventID, string planType)//Commented by Ashish 15-Apr-2009 for Phase V Changes
        public static DataSet GetElectiveAccountsByPlan(string fundEventID, string planType, string retirementDate)//Phase IV Changes
        {
            DataSet accountBalance;
            try
            {
                //accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getElectiveAccountsByPlan(personID);
                //accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getElectiveAccountsByPlan(personID, planType);
                //accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getElectiveAccountsByPlan(fundEventID, planType);// Ashish 15-Apr-2009 Phase V changes
                accountBalance = YMCARET.YmcaDataAccessObject.RetirementDAClass.getElectiveAccountsByPlan(fundEventID, planType, retirementDate);
            }
            catch
            {
                throw;
            }
            return accountBalance;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="annSal"></param>
        /// <param name="drGivenEmp"></param>
        /// <param name="dtActiveEmp"></param>
        /// <returns></returns>
        private int getAnnSalIncrease(int annSal, DataRow drGivenEmp, DataTable dtActiveEmp)
        {
            int sal = 0;
            foreach (DataRow dr in dtActiveEmp.Rows)
                if (dr["EmpEventID"].ToString() == drGivenEmp["guiUniqueID"].ToString())
                {
                    string salIncrease = dr["AnnualSalaryIncrease"].ToString();
                    sal = (salIncrease == string.Empty ? 0 : Convert.ToInt32(salIncrease));
                    break;
                }
            if (sal == 0)
                sal = annSal;

            return sal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="annSalIncEffDate"></param>
        /// <param name="drGivenEmp"></param>
        /// <param name="dtActiveEmp"></param>
        /// <returns></returns>
        private string getAnnSalIncreaseEffDate(string annSalIncEffDate, DataRow drGivenEmp, DataTable dtActiveEmp)
        {
            string salIncEffDate = null;
            foreach (DataRow dr in dtActiveEmp.Rows)
                if (dr["EmpEventID"].ToString() == drGivenEmp["guiUniqueID"].ToString())
                {
                    string salIncreaseEffDate = dr["AnnualSalaryIncreaseEffDate"].ToString();
                    salIncEffDate = salIncreaseEffDate;
                    break;
                }

            return salIncEffDate;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modSal"></param>
        /// <param name="drGivenEmp"></param>
        /// <param name="dtActiveEmp"></param>
        /// <returns></returns>
        private static string getModifiedSalary(string modSal, DataRow drGivenEmp, DataTable dtActiveEmp)
        {
            string sal = "0";
            foreach (DataRow dr in dtActiveEmp.Rows)
                if (dr["EmpEventID"].ToString() == drGivenEmp["guiUniqueID"].ToString())
                {
                    string salIncrease = dr["ModifiedSal"].ToString();
                    sal = (salIncrease == string.Empty ? "0" : salIncrease);
                    break;
                }

            if (sal == "0")
                sal = (modSal == string.Empty ? "0" : modSal);

            return sal;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="futureSal"></param>
        /// <param name="futureSalEffDate"></param>
        /// <param name="endDate"></param>
        /// <param name="drGivenEmp"></param>
        /// <param name="dtActiveEmp"></param>
        private void getFutureSalaryDetails(ref string futureSal, ref string futureSalEffDate, ref string endDate, DataRow drGivenEmp, DataTable dtActiveEmp)
        {
            foreach (DataRow dr in dtActiveEmp.Rows)
                if (dr["EmpEventID"].ToString() == drGivenEmp["guiUniqueID"].ToString())
                {
                    string futSal = dr["FutureSalary"].ToString().Trim();
                    futureSal = (futSal == string.Empty ? "0" : futSal);
                    futureSalEffDate = dr["FutureSalaryEffDate"].ToString();
                    endDate = dr["EndWorkDate"].ToString();


                    break;
                }
            if (futureSal == string.Empty)
                futureSal = "0";
        }

        //Added by Ashish for Phase V changes ,start

        /// <summary>
        /// This Method remove excluded account balances from the projection table.
        /// This method  only used in Retirement Estimate.
        /// </summary>
        /// <param name="dtAccountsBasisByProjection"></param>
        /// <param name="dtExcludedAccounts"></param>
        /// <param name="isPermanentExcluded"></param>
        /// <returns></returns>
        private DataTable removeExcludedAccounts(DataTable dtAccountsBasisByProjection, DataTable dtExcludedAccounts, bool isPermanentExcluded)
        {
            DataTable dtExcludeFromAccountsBasisByProj;
            if (isPermanentExcluded)
            {
                dtExcludeFromAccountsBasisByProj = dtAccountsBasisByProjection;
            }
            else
            {
                dtExcludeFromAccountsBasisByProj = dtAccountsBasisByProjection.Copy();
            }

            //bool boolBasicAccountExcluded = false;
            DataRow[] l_drExcludeFromAcctProjFoundRow;
            string l_filterExpression = string.Empty;
            try
            {
                if (dtAccountsBasisByProjection != null && dtExcludedAccounts != null)
                {
                    foreach (DataRow drExcludedRow in dtExcludedAccounts.Rows)
                    {
                        //l_filterExpression="chvPlanType='"+drExcludedRow["chvPlanType"]+"' AND bitBasicAcct="+drExcludedRow["bitBasicAcct"]+" AND chrAcctType='"+drExcludedRow["chrAcctType"]+"' AND bitRet_Voluntary="+drExcludedRow["bitRet_Voluntary"];
                        l_filterExpression = "chvPlanType='" + drExcludedRow["chvPlanType"] + "' AND  chrAcctType='" + drExcludedRow["chrAcctType"] + "'";
                        l_drExcludeFromAcctProjFoundRow = dtExcludeFromAccountsBasisByProj.Select(l_filterExpression);
                        if (l_drExcludeFromAcctProjFoundRow.Length > 0)
                        {
                            for (int i = 0; i < l_drExcludeFromAcctProjFoundRow.Length; i++)
                            {
                                //excluded personal side money
                                if (Convert.ToBoolean(drExcludedRow["bitPersonalAmtExcluded"]) == true)
                                {
                                    l_drExcludeFromAcctProjFoundRow[i]["mnyBalance"] = Convert.ToDouble(l_drExcludeFromAcctProjFoundRow[i]["mnyBalance"] != System.DBNull.Value ? l_drExcludeFromAcctProjFoundRow[i]["mnyBalance"] : 0) - Convert.ToDouble(l_drExcludeFromAcctProjFoundRow[i]["PersonalAmt"] != System.DBNull.Value ? l_drExcludeFromAcctProjFoundRow[i]["PersonalAmt"] : 0);
                                    l_drExcludeFromAcctProjFoundRow[i]["mnypersonalpretax"] = 0;
                                    l_drExcludeFromAcctProjFoundRow[i]["mnypersonalposttax"] = 0;
                                    l_drExcludeFromAcctProjFoundRow[i]["PersonalAmt"] = 0;

                                }
                                // excluded Ymca side money
                                if (Convert.ToBoolean(drExcludedRow["bitYmcaAmtExcluded"]) == true)
                                {
                                    l_drExcludeFromAcctProjFoundRow[i]["mnyBalance"] = Convert.ToDouble(l_drExcludeFromAcctProjFoundRow[i]["mnyBalance"] != System.DBNull.Value ? l_drExcludeFromAcctProjFoundRow[i]["mnyBalance"] : 0) - Convert.ToDouble(l_drExcludeFromAcctProjFoundRow[i]["YmcaAmt"] != System.DBNull.Value ? l_drExcludeFromAcctProjFoundRow[i]["YmcaAmt"] : 0);
                                    l_drExcludeFromAcctProjFoundRow[i]["YmcaAmt"] = 0;
                                    l_drExcludeFromAcctProjFoundRow[i]["mnyymcapretax"] = 0;

                                }
                            }
                        }
                    }

                    dtExcludeFromAccountsBasisByProj.AcceptChanges();
                }

                return dtExcludeFromAccountsBasisByProj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //		Added by Ashish for Phase V changes ,End



        //Added by Ashish for phase V part III changes, Start
        #region Private Method
        /// <summary>
        /// This method returns unique account list. 
        /// </summary>
        /// <param name="para_dtAccountsByBasis"></param>
        /// <returns></returns>
        private static DataTable GetUniqueAcctList(DataTable para_dtAccountsByBasis)
        {
            try
            {
                DataTable dtUniqueAcctList = null;
                string[] targetColumns = { "chrAcctType", "chvPlanType" };
                string[] compareColumns = { "chrAcctType" };
                string[] clubbedColumns = null;
                // Get distinct AcctType
                dtUniqueAcctList = SelectDistinct(para_dtAccountsByBasis, targetColumns, compareColumns, clubbedColumns);
                if (dtUniqueAcctList == null)
                {
                    dtUniqueAcctList = new DataTable();
                    dtUniqueAcctList.Columns.Add("chrAcctType");
                }
                return dtUniqueAcctList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static DataTable GetUniqueAnnuityBasisGroupList(DataTable para_dtAnnuityBasisTypeList)
        {
            DataTable dtUniqueBasisTypeGroupList = null;
            try
            {

                string[] targetColumns = { "chrAnnuityBasisGroup" };
                string[] compareColumns = { "chrAnnuityBasisGroup" };
                string[] clubbedColumns = null;
                // Get distinct AcctType
                dtUniqueBasisTypeGroupList = SelectDistinct(para_dtAnnuityBasisTypeList, targetColumns, compareColumns, clubbedColumns);
                if (dtUniqueBasisTypeGroupList == null)
                {
                    dtUniqueBasisTypeGroupList = new DataTable();
                    dtUniqueBasisTypeGroupList.Columns.Add("chrAnnuityBasisGroup");
                }
                return dtUniqueBasisTypeGroupList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //This method add interest amount in dtAccountBasisByProjection 
        private static void AddedInterestAmountInAcctProjection(ref DataTable para_dtAccountsBasisByProjection, string para_chrAcctType, string para_EffectiveAnnuityBasisGroup, string para_EffectiveAnnuityBasisType, double para_mnyPersonalInterestBalance, double para_mnyYMCAInterestBalance, string para_PlanType)
        {
            DataRow[] drAcctProjectedFindRows;
            DataRow drAcctProjRow;
            try
            {
                if (para_dtAccountsBasisByProjection != null)
                {
                    if (para_mnyPersonalInterestBalance > 0 || para_mnyYMCAInterestBalance > 0)
                    {
                        drAcctProjectedFindRows = para_dtAccountsBasisByProjection.Select("chrAcctType='" + para_chrAcctType + "' AND chrAnnuityBasisGroup='" + para_EffectiveAnnuityBasisGroup + "' AND chrAnnuityBasisType='" + para_EffectiveAnnuityBasisType + "'");
                        if (drAcctProjectedFindRows.Length > 0)
                        {
                            drAcctProjRow = drAcctProjectedFindRows[0];
                            //drAcctProjRow["dtsTransactDate"] = dtsTransactDate;
                            //drAccountsBasisByProjection["chrProjPeriod"] = chrProjPeriod;
                            //drAcctProjRow["chrAcctType"] = chrAcctType;
                            //drAcctProjRow["chrAnnuityBasisType"] = chrAnnuityBasisType;
                            //drAcctProjRow["mnyYMCAContribBalance"] = mnyYMCAContribBalance;
                            //drAcctProjRow["mnyPersonalContribBalance"] = mnyPersonalContribBalance;
                            drAcctProjRow["mnyPersonalInterestBalance"] = (drAcctProjRow["mnyPersonalInterestBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalInterestBalance"])) + para_mnyPersonalInterestBalance;
                            drAcctProjRow["mnyYMCAInterestBalance"] = (drAcctProjRow["mnyYMCAInterestBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAInterestBalance"])) + para_mnyYMCAInterestBalance;
                            //drAcctProjRow["mnyPersonalPreTax"] = mnyPersonalPreTax;
                            //drAcctProjRow["mnyYMCAPreTax"] = mnyYMCAPreTax;
                            //drAcctProjRow["mnyPersonalPostTax"] = mnyPersonalPostTax;
                            //drAcctProjRow["YMCAAmt"] = YMCAAmt;
                            //drAcctProjRow["PersonalAmt"] = PersonalAmt;
                            //drAcctProjRow["mnyBalance"] = mnyBalance;

                            //drAcctProjRow["intProjPeriodSequence"] = intProjPeriodSequence;

                            //drAcctProjRow["chvPlanType"] = planType;

                        }
                        else
                        {
                            drAcctProjRow = para_dtAccountsBasisByProjection.NewRow();
                            //drAcctProjRow["dtsTransactDate"] = dtsTransactDate;
                            drAcctProjRow["chrProjPeriod"] = "";
                            drAcctProjRow["chrAcctType"] = para_chrAcctType;
                            drAcctProjRow["chrAnnuityBasisType"] = para_EffectiveAnnuityBasisType;
                            drAcctProjRow["mnyYMCAContribBalance"] = 0;
                            drAcctProjRow["mnyPersonalContribBalance"] = 0;
                            drAcctProjRow["mnyPersonalInterestBalance"] = para_mnyPersonalInterestBalance;
                            drAcctProjRow["mnyYMCAInterestBalance"] = para_mnyYMCAInterestBalance;
                            drAcctProjRow["mnyPersonalPreTax"] = 0;
                            drAcctProjRow["mnyYMCAPreTax"] = 0;
                            drAcctProjRow["mnyPersonalPostTax"] = 0;
                            drAcctProjRow["YMCAAmt"] = 0;
                            drAcctProjRow["PersonalAmt"] = 0;
                            drAcctProjRow["mnyBalance"] = 0;
                            drAcctProjRow["intProjPeriodSequence"] = 0;
                            drAcctProjRow["chrAnnuityBasisGroup"] = para_EffectiveAnnuityBasisGroup;
                            drAcctProjRow["chvPlanType"] = para_PlanType;
                            para_dtAccountsBasisByProjection.Rows.Add(drAcctProjRow);
                        }
                    }
                    para_dtAccountsBasisByProjection.AcceptChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method update contribution in projection table. 
        /// </summary>
        /// <param name="para_dtAccountsBasisByProjection"></param>
        /// <param name="para_chrAcctType"></param>
        /// <param name="para_EffectiveAnnuityBasisGroup"></param>
        /// <param name="para_EffectiveAnnuityBasisType"></param>
        /// <param name="para_mnyPersonalContribBalance"></param>
        /// <param name="para_mnyYMCAContribBalance"></param>
        /// <param name="para_PlanType"></param>
        private static void AddContributionInAccountsProjection(ref DataTable para_dtAccountsBasisByProjection, string para_chrAcctType, string para_EffectiveAnnuityBasisGroup, string para_EffectiveAnnuityBasisType, double para_mnyPersonalContribBalance, double para_mnyYMCAContribBalance, string para_PlanType)
        {
            DataRow[] drAcctProjectedFindRows;
            DataRow drAcctProjRow;
            try
            {
                if (para_dtAccountsBasisByProjection != null)
                {
                    if (para_mnyPersonalContribBalance > 0 || para_mnyYMCAContribBalance > 0)
                    {
                        drAcctProjectedFindRows = para_dtAccountsBasisByProjection.Select("chrAcctType='" + para_chrAcctType + "' AND chrAnnuityBasisGroup='" + para_EffectiveAnnuityBasisGroup + "' AND chrAnnuityBasisType='" + para_EffectiveAnnuityBasisType + "'");
                        if (drAcctProjectedFindRows.Length > 0)
                        {
                            drAcctProjRow = drAcctProjectedFindRows[0];
                            //drAcctProjRow["dtsTransactDate"] = dtsTransactDate;
                            //drAccountsBasisByProjection["chrProjPeriod"] = chrProjPeriod;
                            //drAcctProjRow["chrAcctType"] = chrAcctType;
                            //drAcctProjRow["chrAnnuityBasisType"] = chrAnnuityBasisType;
                            drAcctProjRow["mnyYMCAContribBalance"] = (drAcctProjRow["mnyYMCAContribBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAContribBalance"])) + para_mnyYMCAContribBalance;
                            drAcctProjRow["mnyPersonalContribBalance"] = (drAcctProjRow["mnyPersonalContribBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalContribBalance"])) + para_mnyPersonalContribBalance;
                            //drAcctProjRow["mnyPersonalInterestBalance"] = (drAcctProjRow["mnyPersonalInterestBalance"]==System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalInterestBalance"])) + para_mnyPersonalInterestBalance;
                            //drAcctProjRow["mnyYMCAInterestBalance"] =(drAcctProjRow["mnyYMCAInterestBalance"]==System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAInterestBalance"])) + para_mnyYMCAInterestBalance;
                            //drAcctProjRow["mnyPersonalPreTax"] = mnyPersonalPreTax;
                            //drAcctProjRow["mnyYMCAPreTax"] = mnyYMCAPreTax;
                            //drAcctProjRow["mnyPersonalPostTax"] = mnyPersonalPostTax;
                            //drAcctProjRow["YMCAAmt"] = YMCAAmt;
                            //drAcctProjRow["PersonalAmt"] = PersonalAmt;
                            //drAcctProjRow["mnyBalance"] = mnyBalance;

                            //drAcctProjRow["intProjPeriodSequence"] = intProjPeriodSequence;

                            //drAcctProjRow["chvPlanType"] = planType;

                        }
                        else
                        {
                            drAcctProjRow = para_dtAccountsBasisByProjection.NewRow();
                            //drAcctProjRow["dtsTransactDate"] = dtsTransactDate;
                            drAcctProjRow["chrProjPeriod"] = "";
                            drAcctProjRow["chrAcctType"] = para_chrAcctType;
                            drAcctProjRow["chrAnnuityBasisType"] = para_EffectiveAnnuityBasisType;
                            drAcctProjRow["mnyYMCAContribBalance"] = para_mnyYMCAContribBalance;
                            drAcctProjRow["mnyPersonalContribBalance"] = para_mnyPersonalContribBalance;
                            drAcctProjRow["mnyPersonalInterestBalance"] = 0;
                            drAcctProjRow["mnyYMCAInterestBalance"] = 0;
                            drAcctProjRow["mnyPersonalPreTax"] = 0;
                            drAcctProjRow["mnyYMCAPreTax"] = 0;
                            drAcctProjRow["mnyPersonalPostTax"] = 0;
                            drAcctProjRow["YMCAAmt"] = 0;
                            drAcctProjRow["PersonalAmt"] = 0;
                            drAcctProjRow["mnyBalance"] = 0;
                            drAcctProjRow["intProjPeriodSequence"] = 0;
                            drAcctProjRow["chrAnnuityBasisGroup"] = para_EffectiveAnnuityBasisGroup;
                            drAcctProjRow["chvPlanType"] = para_PlanType;
                            para_dtAccountsBasisByProjection.Rows.Add(drAcctProjRow);
                        }
                        para_dtAccountsBasisByProjection.AcceptChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method find out mature Roll over basis type
        /// </summary>
        /// <param name="para_dtAnnuityBasisTypeList"></param>
        /// <param name="para_RetirementDate"></param>
        /// <param name="para_ycCalcMonth"></param>
        /// <returns></returns>
        private static DataRow GetRollOverMatureBasisType(DataTable para_dtAnnuityBasisTypeList, string para_RetirementDate, DateTime para_ycCalcMonth)
        {
            DataRow drAnnuityBasisRow = null;
            int dateDiffInMonth = 0;
            try
            {
                drAnnuityBasisRow = GetEffectiveAnnuityBasisType(para_dtAnnuityBasisTypeList, "ROL", para_ycCalcMonth);
                if (drAnnuityBasisRow != null)
                {
                    if (drAnnuityBasisRow["intRolloverMaturityAge"] != System.DBNull.Value)
                    {
                        dateDiffInMonth = DateAndTime.DateDiffNew(DateIntervalNew.Month, para_ycCalcMonth, Convert.ToDateTime(para_RetirementDate));
                        if (dateDiffInMonth >= Convert.ToInt32(drAnnuityBasisRow["intRolloverMaturityAge"].ToString()))
                        {
                            drAnnuityBasisRow = GetEffectiveAnnuityBasisType(para_dtAnnuityBasisTypeList, "PST", para_ycCalcMonth);
                        }
                    }
                }
                return drAnnuityBasisRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// This method update monthly inerest and contribution in prjection table
        /// </summary>
        /// <param name="para_dtAccountsBasisByProjection"></param>
        private static void UpdateAcctTotalInProjectionTable(ref DataTable para_dtAccountsBasisByProjection, string para_RetireType)
        {
            double l_mnyPersonalPreTax = 0;
            double l_mnyYMCAPreTax = 0;
            double l_PersonalAmt = 0;

            try
            {
                if (para_dtAccountsBasisByProjection != null)
                {
                    if (para_dtAccountsBasisByProjection.Rows.Count > 0)
                    {
                        foreach (DataRow drAcctProjRow in para_dtAccountsBasisByProjection.Rows)
                        {
                            l_mnyPersonalPreTax = 0;
                            l_mnyYMCAPreTax = 0;
                            l_PersonalAmt = 0;
                            if (para_RetireType.ToUpper() == "NORMAL")
                            {
                                l_mnyPersonalPreTax = (drAcctProjRow["mnyPersonalPreTax"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalPreTax"])) +
                                                    (drAcctProjRow["mnyPersonalContribBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalContribBalance"])) +
                                                    (drAcctProjRow["mnyPersonalInterestBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalInterestBalance"]));

                                l_mnyYMCAPreTax = (drAcctProjRow["mnyYMCAPreTax"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAPreTax"])) +
                                    (drAcctProjRow["mnyYMCAContribBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAContribBalance"])) +
                                    (drAcctProjRow["mnyYMCAInterestBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAInterestBalance"]));
                                l_PersonalAmt = l_mnyPersonalPreTax + (drAcctProjRow["mnyPersonalPostTax"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalPostTax"]));


                                drAcctProjRow["mnyPersonalPreTax"] = l_mnyPersonalPreTax;
                                drAcctProjRow["mnyYMCAPreTax"] = l_mnyYMCAPreTax;
                                //drAcctProjRow["mnyPersonalPostTax"] = 0;
                                drAcctProjRow["YMCAAmt"] = l_mnyYMCAPreTax;
                                drAcctProjRow["PersonalAmt"] = l_PersonalAmt;
                                drAcctProjRow["mnyBalance"] = l_PersonalAmt + l_mnyYMCAPreTax;
                                drAcctProjRow["mnyYMCAContribBalance"] = 0;
                                drAcctProjRow["mnyPersonalContribBalance"] = 0;
                                drAcctProjRow["mnyPersonalInterestBalance"] = 0;
                                drAcctProjRow["mnyYMCAInterestBalance"] = 0;
                            }
                            else if (para_RetireType.ToUpper() == "DISABL")
                            {
                                l_mnyPersonalPreTax = (drAcctProjRow["mnyPersonalContribBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalContribBalance"])) +
                                                    (drAcctProjRow["mnyPersonalInterestBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalInterestBalance"]));

                                l_mnyYMCAPreTax = (drAcctProjRow["mnyYMCAPreTax"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAPreTax"])) +
                                    (drAcctProjRow["mnyYMCAContribBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAContribBalance"])) +
                                    (drAcctProjRow["mnyYMCAInterestBalance"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyYMCAInterestBalance"]));

                                l_PersonalAmt = (drAcctProjRow["mnyPersonalPreTax"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalPreTax"])) + (drAcctProjRow["mnyPersonalPostTax"] == System.DBNull.Value ? 0 : Convert.ToDouble(drAcctProjRow["mnyPersonalPostTax"]));


                                //drAcctProjRow["mnyPersonalPreTax"] = l_mnyPersonalPreTax;
                                drAcctProjRow["mnyYMCAPreTax"] = l_mnyYMCAPreTax + l_mnyPersonalPreTax;
                                //drAcctProjRow["mnyPersonalPostTax"] = 0;
                                drAcctProjRow["YMCAAmt"] = l_mnyYMCAPreTax + l_mnyPersonalPreTax;
                                //drAcctProjRow["PersonalAmt"] = l_PersonalAmt;
                                drAcctProjRow["mnyBalance"] = l_PersonalAmt + l_mnyYMCAPreTax + l_mnyPersonalPreTax;
                                drAcctProjRow["mnyYMCAContribBalance"] = 0;
                                drAcctProjRow["mnyPersonalContribBalance"] = 0;
                                drAcctProjRow["mnyPersonalInterestBalance"] = 0;
                                drAcctProjRow["mnyYMCAInterestBalance"] = 0;
                            }




                        }
                        para_dtAccountsBasisByProjection.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624  Start
        /// This Method update Modified salary information into Employee Details table

        private void UpdateEmployeeInformation(ref DataTable dtRetEmpDetails, DataTable dtEmpModifiedSalDetails, DataTable para_dtYMCAResolution, string para_RetireType, string para_ldCalcMonthToday, string para_RetirementDate)
        {

            DataRow[] drFoundRows = null;

            try
            {
                if (dtRetEmpDetails != null)
                {
                    if (dtEmpModifiedSalDetails != null)
                    {
                        foreach (DataRow drRetEmpRow in dtRetEmpDetails.Rows)
                        {
                            drFoundRows = dtEmpModifiedSalDetails.Select("EmpEventID = '" + drRetEmpRow["guiEmpEventId"].ToString().ToUpper() + "'");
                            if (drFoundRows.Length > 0)
                            {
                                if (para_RetireType.ToUpper() == "NORMAL")
                                {
                                    if (drFoundRows[0]["FutureSalary"].ToString() != String.Empty)
                                        drRetEmpRow["numFutureSalary"] = Convert.ToDecimal(drFoundRows[0]["FutureSalary"]);
                                    else
                                        drRetEmpRow["numFutureSalary"] = "0.00";

                                    if (drFoundRows[0]["FutureSalaryEffDate"].ToString() != String.Empty)
                                        drRetEmpRow["dtmFutureSalaryDate"] = Convert.ToDateTime(drFoundRows[0]["FutureSalaryEffDate"]).ToString("MM/dd/yyyy");


                                    if (drFoundRows[0]["AnnualSalaryIncreaseEffDate"].ToString() != String.Empty)
                                        drRetEmpRow["dtmAnnualSalaryIncreaseEffDate"] = Convert.ToDateTime(drFoundRows[0]["AnnualSalaryIncreaseEffDate"]).ToString("MM/dd/yyyy");


                                    if (drFoundRows[0]["AnnualSalaryIncrease"].ToString() != String.Empty)
                                        drRetEmpRow["numAnnualPctgIncrease"] = Convert.ToDecimal(drFoundRows[0]["AnnualSalaryIncrease"]);
                                    else
                                        drRetEmpRow["numAnnualPctgIncrease"] = "0.00";
                                }//if (drFoundRows.Length > 0)

                                if (drFoundRows[0]["EndWorkDate"].ToString() != String.Empty)
                                    drRetEmpRow["dtmTerminationDate"] = Convert.ToDateTime(drFoundRows[0]["EndWorkDate"]).ToString("MM/dd/yyyy");
                                //SR:2012-07.30 : BT-1041/YRS 5.0-1599:Assigning retirement date as end date if end dats is null
                                else if (string.IsNullOrEmpty(drRetEmpRow["dtmTerminationDate"].ToString()))
                                    drRetEmpRow["dtmTerminationDate"] = para_RetirementDate;


                            } // if (drFoundRows.Length > 0)
                        }//foreach (DataRow drRetEmpRow in dtRetEmpDetails.Rows)
                    }//if (dtRetEmpDetails != null )
                    dtRetEmpDetails.AcceptChanges();
                    if (para_RetireType == "DISABL")
                    {
                        GetDisabilityEligibleEmpRecord(ref dtRetEmpDetails, para_dtYMCAResolution, para_ldCalcMonthToday, para_RetirementDate);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        ///<summary> 

        private bool ValidatePercentContributionWithSalary(DataTable para_dtRetEstimateEmployment
            , DataTable para_dtAccountsByBasis, DataSet para_dataSetElectiveAccounts, string para_fundStatus
            , ref string para_errorMessage)
        {
            bool returnFlage = true;
            //START: PPP | 11/24/2017 | YRS-AT-3319 | Following variables are declared but not used anywhere
            //DataSet l_dsMetaAccountTypes = null;
            //bool isBasisType = false;
            //bool _blnBasisType = false;
            //END: PPP | 11/24/2017 | YRS-AT-3319 | Following variables are declared but not used anywhere
            try
            {
                //ASHISH:2010.12.27      YRS 5.0-855,BT 624, Comment this code
                // l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
                if (para_dtRetEstimateEmployment == null && para_dtAccountsByBasis == null && para_dataSetElectiveAccounts == null)
                {
                    return returnFlage;
                }
                //ASHISH:2010.12.27      YRS 5.0-855,BT 624, Comment this code
                //foreach (DataRow drActByBasis in para_dtAccountsByBasis.Rows)
                //    foreach (DataRow drMetaActTypes in l_dsMetaAccountTypes.Tables[0].Rows)
                //    {
                //        if (drActByBasis["chrAcctType"].ToString().Trim().ToUpper() ==
                //            drMetaActTypes["chrAcctType"].ToString().Trim().ToUpper())
                //        {
                //            isBasisType = true;
                //            break;
                //        }
                //        if (isBasisType)
                //        {
                //            break;
                //        }
                //    }

                //_blnBasisType = isBasisType;

                bool isMonthlyPercentageContributionPresent = false;
                DataRow[] RetEmpRecordFound = null;
                DataRow[] drMonthPerc = para_dataSetElectiveAccounts.Tables[0].Select("chrAdjustmentBasisCode='" + RetirementBOClass.MONTHLY_PERCENT_SALARY + "'");
                if (drMonthPerc.Length > 0)
                {
                    for (int i = 0; i < drMonthPerc.Length; i++)
                    {
                        RetEmpRecordFound = para_dtRetEstimateEmployment.Select("guiEmpEventId='" + drMonthPerc[i]["guiEmpEventId"].ToString().ToUpper() + "' AND (END IS NULL OR END='" + string.Empty + "') ");
                        if (RetEmpRecordFound.Length > 0)
                        {
                            if (RetEmpRecordFound[0]["AvgSalaryPerEmployment"].ToString() != string.Empty)
                            {
                                if (Convert.ToDecimal(RetEmpRecordFound[0]["AvgSalaryPerEmployment"].ToString()) == 0)
                                {
                                    if (Convert.ToDecimal(RetEmpRecordFound[0]["numFutureSalary"].ToString()) == 0 && RetEmpRecordFound[0]["dtmFutureSalaryDate"].ToString() == string.Empty)
                                    {
                                        isMonthlyPercentageContributionPresent = true;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                }


                //ASHISH:2010.12.27      YRS 5.0-855,BT 624, Comment this code
                //if (!_blnBasisType && isMonthlyPercentageContributionPresent)
                //{
                if (isMonthlyPercentageContributionPresent)
                {


                    if (para_fundStatus == "PE" || para_fundStatus == "PEML" || para_fundStatus == "RE")
                    {
                        //para_errorMessage = "Participant's fund status is Pre-Eligible & does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further.";//commented on 24-sep for BT-1126
                        para_errorMessage = "MESSAGE_RETIREMENT_BOC_PARTICIPANT_STATUS_PREELIGIBLE";
                        returnFlage = false;

                    }
                    else if (para_fundStatus == "AE" || para_fundStatus == "RA")
                    {
                        //para_errorMessage = "Participant does not have contributions to any basic account, so please specify the Future Salary and its Effective Date to proceed further.";//commented on 24-sep for BT-1126
                        para_errorMessage = "MESSAGE_RETIREMENT_BOC_PARTICIPANT_NOT_HAVE_CONTRIBUTIONS";
                        returnFlage = false;
                    }



                }
                return returnFlage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //ASHISH:2010.10.11 code Added for YRS 5.0-855 BT-624  Start	
        #endregion
        //Added by Ashish for phase V part III changes ,End

        //START: PPP | 05/06/2016 | YRS-AT-2683 | Fetching unfunded and not received amount
        private static void GetPendingBalances(string strFundEventID, DataTable dtAccountsBasisByProjection, DateTime dtRetirementDate, DateTime? endWorkDate) // PPP | 03/01/2017 | YRS-AT-3317 | Added EndWorkDate parameter 
        {
            DataSet dsPendingBalances;
            string strAnnuityBasisType, strAccountType;
            decimal dPersonSideMoney, dYmcaSideMoney, dTDContribution;
            decimal dPersonalPreTax, dPersonalPostTax, dYmcaPreTax;
            try
            {
                dsPendingBalances = YMCARET.YmcaDataAccessObject.RetirementDAClass.GetPendingBalances(strFundEventID, dtRetirementDate, endWorkDate); // PPP | 03/01/2017 | YRS-AT-3317 | endWorkDate will represent termination date or user selected EndWorkDate in case of active employment
                if (IsNonEmpty(dsPendingBalances) && dtAccountsBasisByProjection != null && dtAccountsBasisByProjection.Rows.Count > 0)
                {
                    strAnnuityBasisType = Convert.IsDBNull(dsPendingBalances.Tables[0].Rows[0]["AnnuityBasisType"]) ? string.Empty : Convert.ToString(dsPendingBalances.Tables[0].Rows[0]["AnnuityBasisType"]);
                    dPersonSideMoney = Convert.IsDBNull(dsPendingBalances.Tables[0].Rows[0]["PersonSideMoney"]) ? 0M : Convert.ToDecimal(dsPendingBalances.Tables[0].Rows[0]["PersonSideMoney"]);
                    dYmcaSideMoney = Convert.IsDBNull(dsPendingBalances.Tables[0].Rows[0]["YmcaSideMoney"]) ? 0M : Convert.ToDecimal(dsPendingBalances.Tables[0].Rows[0]["YmcaSideMoney"]);
                    dTDContribution = Convert.IsDBNull(dsPendingBalances.Tables[0].Rows[0]["TDContribution"]) ? 0M : Convert.ToDecimal(dsPendingBalances.Tables[0].Rows[0]["TDContribution"]);
                    foreach (DataRow row in dtAccountsBasisByProjection.Rows)
                    {
                        if (Convert.ToString(row["chrAcctType"]).Trim() == "BA" && Convert.ToString(row["chrAnnuityBasisType"]).Trim() == strAnnuityBasisType.Trim())
                        {
                            row["mnyPersonalPreTax"] = Convert.ToDecimal(row["mnyPersonalPreTax"]) + dPersonSideMoney;
                            row["PersonalAmt"] = Convert.ToDecimal(row["PersonalAmt"]) + dPersonSideMoney;

                            row["mnyYmcaPreTax"] = Convert.ToDecimal(row["mnyYmcaPreTax"]) + dYmcaSideMoney;
                            row["YmcaAmt"] = Convert.ToDecimal(row["YmcaAmt"]) + dYmcaSideMoney;

                            row["mnyBalance"] = Convert.ToDecimal(row["mnyBalance"]) + dPersonSideMoney + dYmcaSideMoney;
                        }
                        else if (Convert.ToString(row["chrAcctType"]).Trim() == "TD" && Convert.ToString(row["chrAnnuityBasisType"]).Trim() == strAnnuityBasisType.Trim())
                        {
                            row["mnyPersonalPreTax"] = Convert.ToDecimal(row["mnyPersonalPreTax"]) + dTDContribution;
                            row["PersonalAmt"] = Convert.ToDecimal(row["PersonalAmt"]) + dTDContribution;

                            row["mnyBalance"] = Convert.ToDecimal(row["mnyBalance"]) + dTDContribution;
                        }
                    }

                    if (dsPendingBalances.Tables.Count > 1 && dsPendingBalances.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow unfundedBalanceRow in dsPendingBalances.Tables[1].Rows)
                        {
                            strAnnuityBasisType = Convert.IsDBNull(unfundedBalanceRow["chrAnnuityBasisType"]) ? string.Empty : Convert.ToString(unfundedBalanceRow["chrAnnuityBasisType"]);
                            strAccountType = Convert.IsDBNull(unfundedBalanceRow["chrAcctType"]) ? string.Empty : Convert.ToString(unfundedBalanceRow["chrAcctType"]);
                            dPersonalPreTax = Convert.IsDBNull(unfundedBalanceRow["mnyPersonalPreTax"]) ? 0M : Convert.ToDecimal(unfundedBalanceRow["mnyPersonalPreTax"]);
                            dPersonalPostTax = Convert.IsDBNull(unfundedBalanceRow["mnyPersonalPostTax"]) ? 0M : Convert.ToDecimal(unfundedBalanceRow["mnyPersonalPostTax"]);
                            dYmcaPreTax = Convert.IsDBNull(unfundedBalanceRow["mnyYmcaPreTax"]) ? 0M : Convert.ToDecimal(unfundedBalanceRow["mnyYmcaPreTax"]);
                            foreach (DataRow projectedRow in dtAccountsBasisByProjection.Rows)
                            {
                                if (Convert.ToString(projectedRow["chrAcctType"]).Trim() == strAccountType.Trim() && Convert.ToString(projectedRow["chrAnnuityBasisType"]).Trim() == strAnnuityBasisType.Trim())
                                {
                                    projectedRow["mnyPersonalPreTax"] = Convert.ToDecimal(projectedRow["mnyPersonalPreTax"]) + dPersonalPreTax;
                                    projectedRow["mnyPersonalPostTax"] = Convert.ToDecimal(projectedRow["mnyPersonalPostTax"]) + dPersonalPostTax;
                                    projectedRow["PersonalAmt"] = Convert.ToDecimal(projectedRow["PersonalAmt"]) + dPersonalPreTax + dPersonalPostTax;

                                    projectedRow["mnyYmcaPreTax"] = Convert.ToDecimal(projectedRow["mnyYmcaPreTax"]) + dYmcaPreTax;
                                    projectedRow["YmcaAmt"] = Convert.ToDecimal(projectedRow["YmcaAmt"]) + dYmcaPreTax;

                                    projectedRow["mnyBalance"] = Convert.ToDecimal(projectedRow["mnyBalance"]) + dPersonalPreTax + dPersonalPostTax + dYmcaPreTax;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                strAccountType = null;
                strAnnuityBasisType = null;
                dsPendingBalances = null;
            }
        }
        //END: PPP | 05/06/2016 | YRS-AT-2683 | Fetching unfunded and not received amount
        #endregion

        ///ASHISH:2010.11.16:Added new method for YRS 5.0-1215

        #region Exact Age Annuity


        //////ASHISH:2010.11.16:Added new method for YRS 5.0-1215
        //public static DataSet SearchAnnuityWithExactAge(string tdRetireeBirthDate, string tdRetireDate, string tcAnnuityType, string tcRetireType, string tcBasisType, string tdBeneficiaryBirthDate, decimal tnBalance)
        //{
        //    try
        //    {
        //        return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getAnnuitizeWithExactAge(tdRetireeBirthDate, tdRetireDate, tcAnnuityType, tcRetireType, tcBasisType, tdBeneficiaryBirthDate, tnBalance));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static DataSet SearchAccountBalancesUptoExactAgeEffDate(string Param_StartDate, string Param_EndDate, string Param_guiPersID, string Param_guiFundEventID, string planType)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.getRetAccountsBalanceUptoExactAgeEffDate(Param_StartDate, Param_EndDate, Param_guiPersID, Param_guiFundEventID, planType));
            }
            catch
            {
                throw;
            }

        }

        //////ASHISH:2010.11.16:Added new method for YRS 5.0-1215
        /// <summary>
        /// Calculate the annuities for the given account values.
        ///Calculate Annuity with exact age factor logic
        /// </summary>
        /// <param name="tnFullBalance"></param>
        /// <param name="retirementType"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="retirementDate"></param>
        /// <param name="dsRetEstInfoParam"></param>        
        /// <param name="benefitValue"></param>        
        /// <param name="ssNO"></param>
        /// <param name="personID"></param>
        /// <param name="dtAnnuitiesList"></param>
        /// <param name="dtAnnuitiesParam"></param>
        /// <param name="finalAnnuityParam"></param>
        /// <returns></returns>
        public DataTable CalculateAnnuitiesWithExactAge(decimal tnFullBalance, string retirementType, DateTime beneficiaryBirthDate
            , DateTime retireeBirthday, DateTime retirementDate, DataSet dsParticipantBeneficiaries
            , decimal benefitValue, string ssNO, string personID
            , DataTable dtAnnuitiesList, DataTable dtAnnuitiesParam, ref decimal finalAnnuityParam)
        {
            DataTable dtAnnuities = new DataTable();

                string ldRetireeBirthdate;
                string ldRetirementDate;
                decimal finalannuity = 0;

                //Added by Ashish for phase V Part III changes, Start
                DataTable dtM_AnnuityByBasisType = null;
                DataTable dtMetaAnnuityFactor = null;
                DataTable dtAnnuitiesStaging = null;
                //Added by Ashish for phase V Part III changes, End

                //START : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation
                try
                {
                   
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Annuity Estimates", "CalculateAnnuitiesWithExactAge START");
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Annuity Estimates: Person Data", String.Format("PersonId - {0} RetirementDate - {1} Retiree Birthdate - {2} ", personID, retireeBirthday, retirementDate));
                    //END : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation

                    ldRetireeBirthdate = retireeBirthday.ToString();
                    ldRetirementDate = retirementDate.ToShortDateString();
                    //DataSet dsMetaAnnuityTypes = RetirementBOClass.getMetaAnnuityTypes(retirementDate);
                    DataSet dsMetaAnnuityFactors = new DataSet();

                    // Get full balance info
                    decimal lnFullBalance = 0;
                    bool isFullBalancePassed = false;
                    if (tnFullBalance == 0)
                    {
                        lnFullBalance = 0;
                        //START: PPP | 03/07/2017 | YRS-AT-2625 | In case of disability we have to use "DISABL" factors for annuity calculation
                        //--Calculation of "Savings" plan annuities are done by using this function and for "Retirement" plan "CalculateAnnuitiesWithExactAgeForDisability" is being used
                        //--This area is reached when "Savings" plan annuities are being calculated, however 'for our information' same function is used for annuities calculated on RDB
                        //retirementType = "NORMAL"; 
                        //END: PPP | 03/07/2017 | YRS-AT-2625 | In case of disability we have to use "DISABL" factors for annuity calculation
                    }
                    else
                    {
                        lnFullBalance = tnFullBalance;
                        isFullBalancePassed = true;
                    }

                    // Step 1. Get annuity factors
                    dsMetaAnnuityFactors = RetirementBOClass.getMetaAnnuityFactors(dsParticipantBeneficiaries, retirementType, ldRetirementDate, ldRetireeBirthdate, beneficiaryBirthDate.ToShortDateString());
                    //Added by Ashish for phase V part III changes 
                    if (dsMetaAnnuityFactors != null && dsMetaAnnuityFactors.Tables.Count > 0)
                    {
                        dtMetaAnnuityFactor = dsMetaAnnuityFactors.Tables[0];
                    }

                    //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                    //retirementType = "NORMAL";  // This is contentitous should be removed.
                    ////Ashish Need to discussed

                    // Get vested info
                    bool isVested = false;
                    DataSet dsIsVested = RetirementBOClass.getVestedInfo(ssNO, ldRetirementDate);
                    if (dsIsVested.Tables.Count != 0)
                    {
                        if (dsIsVested.Tables[0].Rows.Count != 0)
                        {
                            if (dsIsVested.Tables[0].Rows[0]["IsVested"].ToString() == "True")
                                isVested = true;
                            else
                                isVested = false;
                        }
                    }



                    //Create Table schema for AnnutiyList
                    dtM_AnnuityByBasisType = CrateTableForM_AnnuityByBasisType();
                    CreateAnnuityListSchema(ref dtAnnuitiesStaging);
                    //ASHISH:2011-01-28 Commented for BT- 665 Disability Retirment
                    //// For normal retirement type
                    //if (retirementType == "NORMAL" || isFullBalancePassed)
                    //{

                    //Added by Ashish for phase V part III changes
                    decimal lnBalancesByBasisType = 0;

                    //If death benefit amount used
                    if (isFullBalancePassed)
                    {
                        //calculate M annuity for death benefit used amount
                        DataRow drEfectiveBasisType = null;
                        lnBalancesByBasisType = lnFullBalance;

                        drEfectiveBasisType = GetEffectiveAnnuityBasisType(this.g_dtAnnuityBasisTypeList, "PST", retirementDate.Date);
                        if (drEfectiveBasisType != null)
                        {
                            //START: PPP | 03/15/2017 | YRS-AT-2625 | In case of disability ("DISABL") annuities calculated on RDB we have to replace factors. More information is provided in AdjustDisabilityAnnuityForRDB function.
                            if (retirementType.Trim().ToUpper() == "DISABL")
                                dtMetaAnnuityFactor = AdjustDisabilityAnnuityForRDB(drEfectiveBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper(), dtMetaAnnuityFactor);
                            //END: PPP | 03/15/2017 | YRS-AT-2625 | In case of disability ("DISABL") annuities calculated on RDB we have to replace factors. More information is provided in AdjustDisabilityAnnuityForRDB function.

                            // CalculateMAnnuityByBasisType(ref dtM_AnnuityByBasisType, ldRetireeBirthdate, ldRetirementDate, retirementType, drEfectiveBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper(), lnBalancesByBasisType);
                            CalculateAnnuityByBasisTypeWithExactAge(ref dtAnnuitiesStaging, dtMetaAnnuityFactor, drEfectiveBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper(), lnBalancesByBasisType);
                           
                        }
                    }
                    else
                    {

                        if (this.g_dtAcctBalancesByBasisType != null)
                        {
                            if (this.g_dtAcctBalancesByBasisType.Rows.Count > 0)
                            {
                                foreach (DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                                {
                                    lnBalancesByBasisType = 0;
                                    if (isVested)
                                    {
                                        lnBalancesByBasisType = Convert.ToDecimal(drAcctBalancesByBasisType["mnyPersonalRetirementBalance"]) + Convert.ToDecimal(drAcctBalancesByBasisType["mnyYmcaRetirementBalance"]);
                                        //									
                                    }
                                    else
                                    {
                                        lnBalancesByBasisType = Convert.ToDecimal(drAcctBalancesByBasisType["mnyPersonalRetirementBalance"]);
                                        //									
                                    }

                                    CalculateAnnuityByBasisTypeWithExactAge(ref dtAnnuitiesStaging, dtMetaAnnuityFactor, drAcctBalancesByBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper(), lnBalancesByBasisType);
                                   
                                }//foreach(DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                            }//if(this.g_dtAcctBalancesByBasisType.Rows.Count >0 )
                        }//if(this.g_dtAcctBalancesByBasisType!=null)



                    } //Else if (isFullBalancePassed)

                    //Delet Annuties which is not available for death benifit
                    if (isFullBalancePassed)
                    {
                        DataSet dsDBAnnuityOptions = RetirementBOClass.getDBAnnuityOptions(retirementDate);
                        if (dsDBAnnuityOptions.Tables.Count != 0 && dsDBAnnuityOptions.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow dr in dtAnnuitiesStaging.Rows)
                            {
                                if (dsDBAnnuityOptions.Tables[0].Select("chrDBAnnuityType='" + dr["chrAnnuityFactorType"].ToString() + "'").Length == 0)
                                {
                                    dr.Delete();
                                }
                            }
                            dtAnnuitiesStaging.AcceptChanges();
                        }
                    }

                    //dtAnnuitiesStaging = CalculateAnnuityByBasisType(dtM_AnnuityByBasisType, dtMetaAnnuityFactor, isFullBalancePassed, retirementDate);



                    // Update the staging table with the SSBalancing amounts
                    RetirementBOClass.calculateSSBalancing(isFullBalancePassed, benefitValue
                        , dtAnnuitiesStaging, beneficiaryBirthDate, retireeBirthday, retirementDate);

                    // Update the staging table with the Beneficiary and Survivor amounts
                    RetirementBOClass.calculateSurvivorBenefeciary(dtAnnuitiesStaging, retirementDate);

                    {
                        dtAnnuities = dtAnnuitiesStaging.Clone();
                        if (dtAnnuitiesStaging.Rows.Count != 0)
                        {
                            for (int i = 0; i <= dtAnnuitiesStaging.Rows.Count - 1; i++)
                            {
                                dtAnnuities.ImportRow(dtAnnuitiesStaging.Rows[i]);
                                dtAnnuities.GetChanges(DataRowState.Added);
                                dtAnnuities.AcceptChanges();
                            }
                        }
                    }
                    // }
                    dtAnnuitiesParam = dtAnnuities;

                    // Call the contentious method 
                    DataTable dt = RetirementBOClass.contentiousMethod(dtAnnuities, isFullBalancePassed, retirementDate, finalannuity);
                    dtAnnuitiesList = dt;

                    return dt;
                  //START : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation
            }
            catch 
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Error Occur", "CalculateAnnuitiesWithExactAge Error");
                throw ;
            }
            
            finally
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Annuity Estimates", "CalculateAnnuitiesWithExactAge END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
            //END : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation
        }

        /// <summary>
        /// Calculate annuity as per basis type and Exact Age factor
        /// </summary>
        /// <param name="para_dtMAnnuityByBasisType"></param>
        /// <param name="para_retireeBirthDate"></param>
        /// <param name="para_retirementDate"></param>
        /// <param name="para_RetirementType"></param>
        /// <param name="para_AnnuityBasisType"></param>
        /// <param name="mnyBalances"></param>
        private void CalculateAnnuityByBasisTypeWithExactAge(ref DataTable para_l_dtAnnuityList, DataTable para_MetaAnnuityFactor, string para_AnnuityBasisType, decimal mnyBalances)
        {

            DataRow[] drMetaAnnuityFactorRows = null;
            DataRow drMetaAnnuityFactor = null;
            decimal annuityAmount = 0;
            decimal annuityFactor = 0;
            string annuityType = string.Empty;

            try
            {
                //START : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Annuity Estimates", "calculateAnnuityByBasisTypeWithExactAge START");
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Annuity :", String.Format("AnnuityBasisType - {0} ProjectionBalance- {1}  ", para_AnnuityBasisType, mnyBalances));
                //END : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation 

                //CreateAnnuityListSchema(ref l_dtAnnuityList);
                if (para_MetaAnnuityFactor != null)
                {
                    if (para_MetaAnnuityFactor.Rows.Count > 0)
                    {

                        drMetaAnnuityFactorRows = para_MetaAnnuityFactor.Select("BasisTypeCode='" + para_AnnuityBasisType.ToUpper() + "'");
                        if (drMetaAnnuityFactorRows.Length > 0)
                        {
                            for (int i = 0; i < drMetaAnnuityFactorRows.Length; i++)
                            {
                                annuityAmount = 0;
                                annuityFactor = 0;
                                annuityType = string.Empty;
                                drMetaAnnuityFactor = drMetaAnnuityFactorRows[i];
                                annuityType = drMetaAnnuityFactor["AnnuityType"].ToString().Trim().ToUpper();
                                annuityFactor = drMetaAnnuityFactor["Factor"].ToString() == string.Empty ? 0 : Convert.ToDecimal(drMetaAnnuityFactor["Factor"]);


                                annuityAmount = Math.Round((mnyBalances / annuityFactor) / 12, 2);

                                AddUpdateAnnuityListRow(ref para_l_dtAnnuityList, annuityType, annuityAmount);
                                //START : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace(i + 1 + "", String.Format("AnnuityType - {0} AnnuityAmount- {1} AnnuityFactor- {2} ", annuityType, annuityAmount, annuityFactor));
                                //END : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation 
                            }
                        }

                       

                    }//if(para_MetaAnnuityFactor.Rows.Count >0)

                }//if( para_MetaAnnuityFactor!=null)

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //START : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation 
            finally
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Retirement Annuity Estimates", "calculateAnnuityByBasisTypeWithExactAge END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
            //END : ML | 2019.06.07 | YRS-AT-4458 | Implement Logging for Annuity Calculation 
           
        }

        public static string GetExactAgeEffectiveDate()
        {
            DataSet dsExactAgeEffDate = null;
            string ExactAgeEffdate = string.Empty;
            try
            {

                dsExactAgeEffDate = YMCACommonBOClass.getConfigurationValue("EXACT_AGE_EFFECTIVE_DATE");

                if (dsExactAgeEffDate != null)
                {
                    if (dsExactAgeEffDate.Tables.Count > 0)
                    {
                        if (dsExactAgeEffDate.Tables[0].Rows.Count > 0)
                        {
                            ExactAgeEffdate = dsExactAgeEffDate.Tables[0].Rows[0]["Value"].ToString();
                        }
                        else
                            //throw new Exception("Meta key EXACT_AGE_EFFECTIVE_DATE is not defind.");//commented on 24-sep for BT-1126
                            throw new Exception("MESSAGE_RETIREMENT_BOC_META_KEY_NOT_DEFINED");
                    }
                    else
                        // throw new Exception("Meta key EXACT_AGE_EFFECTIVE_DATE is not defind.");//commented on 24-sep for BT-1126
                        throw new Exception("MESSAGE_RETIREMENT_BOC_META_KEY_NOT_DEFINED");
                }
                return ExactAgeEffdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        //2010.11.17    Priya Jawale        Added validation method for exact age annuity
        public static string ValidationForAfterExactAgeEff(string strfundEventID)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.ValidationForAfterExactAgeEff(strfundEventID));
            }
            catch
            {
                throw;
            }
        }
        //End 2010.11.17 

        #endregion

        //2011-01-28    Ashish Srivastava       BT-665 Disability Retirement
        private void SetProjectionPeriodsArray(ref string[,] para_laProjectionPeriods, DateTime para_ldCalcMonthRetire, string para_ldCalcMonthToday, string para_RetireType, string para_RetireeBirthDate, int para_C_EFFDATE, int para_C_TERMDATE, int para_C_PROJECTIONPERIOD)
        {
            string ldStartDateTmp = string.Empty;
            bool calculateForwardExists = false;
            try
            {
                if (para_RetireType == "DISABL")
                {
                    string l_CalcMonthAge60;
                    l_CalcMonthAge60 = GetRetireeAge60Date(para_RetireeBirthDate);

                    //ASHISH:2011.03.07 , start date should be retirement date.
                    //if (para_ldCalcMonthRetire.Month == 12)
                    // para_laProjectionPeriods[para_C_EFFDATE, para_laProjectionPeriods.GetUpperBound(1)] = "01/01/" + (para_ldCalcMonthRetire.Year + 1);
                    //else
                    // para_laProjectionPeriods[para_C_EFFDATE, para_laProjectionPeriods.GetUpperBound(1)] = para_ldCalcMonthRetire.Month + "/1/" + para_ldCalcMonthRetire.Year;

                    para_laProjectionPeriods[para_C_EFFDATE, para_laProjectionPeriods.GetUpperBound(1)] = para_ldCalcMonthRetire.Month + "/1/" + para_ldCalcMonthRetire.Year;

                    para_laProjectionPeriods[para_C_TERMDATE, para_laProjectionPeriods.GetUpperBound(1)] = l_CalcMonthAge60;
                    para_laProjectionPeriods[para_C_PROJECTIONPERIOD, para_laProjectionPeriods.GetUpperBound(1)] = "RETIREDATE_TO_AGE60";
                }
                else // Normal retirement
                {
                    if (para_ldCalcMonthRetire > Convert.ToDateTime(para_ldCalcMonthToday))
                    {
                        para_laProjectionPeriods[para_C_EFFDATE, 2] = para_ldCalcMonthToday;
                        para_laProjectionPeriods[para_C_PROJECTIONPERIOD, 2] = "CALCDATE_FORWARD";
                        calculateForwardExists = true;

                    }
                    else
                    {
                        para_laProjectionPeriods[para_C_EFFDATE, 2] = para_ldCalcMonthRetire.ToShortDateString();
                        para_laProjectionPeriods[para_C_PROJECTIONPERIOD, 2] = "NOT_CALCDATE_FORWARD";
                    }
                    para_laProjectionPeriods[para_C_TERMDATE, para_laProjectionPeriods.GetUpperBound(1)] = para_ldCalcMonthRetire.ToShortDateString();
                    if (!calculateForwardExists)
                        para_laProjectionPeriods[para_C_TERMDATE, para_laProjectionPeriods.GetUpperBound(1) - 1] = para_ldCalcMonthRetire.ToShortDateString();
                }
                // Format dates in the projection period array.

                for (int i = para_laProjectionPeriods.GetUpperBound(1); i >= 1; i--)
                {
                    if (i != para_laProjectionPeriods.GetUpperBound(1))
                    {
                        if (ldStartDateTmp != string.Empty && ldStartDateTmp != null)
                        {
                            if (!(Convert.ToDateTime(ldStartDateTmp).Month == 1))
                            {
                                if (Convert.ToDateTime(ldStartDateTmp).Month > 1 & Convert.ToDateTime(ldStartDateTmp).Month < 10)
                                    para_laProjectionPeriods[para_C_TERMDATE, i] = 0 + Convert.ToDateTime(ldStartDateTmp).Month - 1 + "/" + Convert.ToDateTime(ldStartDateTmp).ToString("dd/yyyy");
                                else
                                    para_laProjectionPeriods[para_C_TERMDATE, i] = Convert.ToDateTime(ldStartDateTmp).Month - 1 + "/" + Convert.ToDateTime(ldStartDateTmp).ToString("dd/yyyy");
                            }
                            else
                                para_laProjectionPeriods[para_C_TERMDATE, i] = "12/" + Convert.ToDateTime(ldStartDateTmp).Day.ToString() + "/" + (Convert.ToDateTime(ldStartDateTmp).Year - 1);
                        }
                    }
                    ldStartDateTmp = para_laProjectionPeriods[para_C_EFFDATE, i];
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void GetDisabilityEligibleEmpRecord(ref DataTable para_dtRetEmpDetails, DataTable para_dtYMCAResolution, string para_ldCalcMonthToday, string para_RetirementDate)
        {
            DataTable dtRetEmpDetailsCopy = null;
            //DataTable dtRetEmpDetailsTemp = null; commented by Sanket
            DataRow[] drRetDetailsFoundRows = null;
            DataRow dtRow = null;
            DataColumn dtColumn = null;
            DataTable dtEligibleEmpForDisability = null;
            DataRow drEmpFinalRow = null;
            string ymcaIDWithHighestResolution = null; //SB | 03/06/2017 | YRS-AT-2625 | for storing the YMCAID in local variable is declared
            try
            {
                if (para_dtRetEmpDetails != null && para_dtYMCAResolution != null)
                {
                    //dtRetEmpDetailsCopy = new DataTable(); commented by Sanket
                    //make a new copy
                    dtRetEmpDetailsCopy = para_dtRetEmpDetails.Copy();

                    //If dtmTerminationDate is null then fill date first day of current date 
                    drRetDetailsFoundRows = dtRetEmpDetailsCopy.Select("dtmTerminationDate='' OR dtmTerminationDate IS NULL");
                    if (drRetDetailsFoundRows.Length > 0)
                    {
                        for (int i = 0; i < drRetDetailsFoundRows.Length; i++)
                        {
                            if (Convert.ToDateTime(para_ldCalcMonthToday) > Convert.ToDateTime(para_RetirementDate))
                                drRetDetailsFoundRows[i]["dtmTerminationDate"] = para_RetirementDate;
                            else
                                drRetDetailsFoundRows[i]["dtmTerminationDate"] = para_ldCalcMonthToday;
                        }
                    }

                    //IEnumerable<DataRow>  drEmpFoundRow=
                    //    from empDetails in para_dtRetEmpDetails.AsEnumerable()
                    //    where empDetails.Field<string>("dtmTerminationDate") == string.Empty
                    //    select empDetails;
                    //if (drEmpFoundRow.l
                    //dtRetEmpDetailsTemp = drEmpFoundRow.CopyToDataTable<DataRow>();

                    //Create schema table for to fidn out latest termination date
                    dtEligibleEmpForDisability = new DataTable();
                    dtColumn = new DataColumn("guiEmpEventId", typeof(string));
                    dtEligibleEmpForDisability.Columns.Add(dtColumn);

                    dtColumn = new DataColumn("guiYmcaID", typeof(string));
                    dtEligibleEmpForDisability.Columns.Add(dtColumn);
                    //dtColumn = new DataColumn("guiEmpEventId", typeof(string));
                    //dtEligibleEmpForDisability.Columns.Add(dtColumn);
                    dtColumn = new DataColumn("dtmTerminationDate", typeof(DateTime));
                    dtEligibleEmpForDisability.Columns.Add(dtColumn);

                    for (int i = 0; i < dtRetEmpDetailsCopy.Rows.Count; i++)
                    {
                        dtRow = dtEligibleEmpForDisability.NewRow();
                        dtRow["guiEmpEventId"] = dtRetEmpDetailsCopy.Rows[i]["guiEmpEventId"];
                        dtRow["guiYmcaID"] = dtRetEmpDetailsCopy.Rows[i]["guiYmcaID"];
                        dtRow["dtmTerminationDate"] = Convert.ToDateTime(dtRetEmpDetailsCopy.Rows[i]["dtmTerminationDate"]);
                        dtEligibleEmpForDisability.Rows.Add(dtRow);
                        //dtRow[""] = dtRetEmpDetailsCopy.Rows[i][""];

                    }
                    dtEligibleEmpForDisability.AcceptChanges();

                    //START : SB | 03/06/2017 | YRS-AT-2625 | Existing method is commented and replaced by new method which accepts retirement date and retirement type parameter to get highest resolution 
                    ymcaIDWithHighestResolution = para_dtYMCAResolution.Rows[0]["guiYmcaId"].ToString();
                    // drRetDetailsFoundRows = dtEligibleEmpForDisability.Select("dtmTerminationDate=MAX(dtmTerminationDate)");   
                    drRetDetailsFoundRows = dtEligibleEmpForDisability.Select(string.Format("dtmTerminationDate=MAX(dtmTerminationDate) AND guiYmcaID='{0}'", ymcaIDWithHighestResolution));
                    //END : SB | 03/06/2017 | YRS-AT-2625 | Existing method is commented and replaced by new method which accepts retirement date and retirement type parameter to get highest resolution 
                    
                    if (drRetDetailsFoundRows.Length > 1)
                    {
                        // DataRow []drYMCAResolutionRows = null;
                        decimal l_highestResolution = 0;
                        decimal l_Resolution = 0;
                        string l_YmcaID = string.Empty;
                        int index = 0;
                        for (int i = 0; i < drRetDetailsFoundRows.Length; i++)
                        {

                            if (para_dtYMCAResolution.Select("guiYmcaID='" + drRetDetailsFoundRows[i]["guiYmcaID"].ToString() + "'").Length > 0)
                            {
                                //drYMCAResolutionRows = para_dtYMCAResolution.Select("guiYmcaID='" + drRetDetailsFoundRows[i]["guiYmcaID"].ToString() + "'");

                                l_Resolution = Convert.ToDecimal(para_dtYMCAResolution.Compute("SUM(numConstituentPctg)+SUM(numYmcaComboPctg)+SUM(numYmcaPctg)+SUM(numAddlMarginPctg)", "guiYmcaID='" + drRetDetailsFoundRows[i]["guiYmcaID"].ToString() + "'"));
                                if (l_Resolution > l_highestResolution)
                                {
                                    l_highestResolution = l_Resolution;
                                    l_YmcaID = drRetDetailsFoundRows[i]["guiYmcaID"].ToString();
                                    index = i;
                                }
                            }
                        }
                        drEmpFinalRow = dtEligibleEmpForDisability.NewRow();
                        drEmpFinalRow = drRetDetailsFoundRows[index];

                    }
                    else if (drRetDetailsFoundRows.Length > 0)
                    {
                        drEmpFinalRow = dtEligibleEmpForDisability.NewRow();
                        drEmpFinalRow = drRetDetailsFoundRows[0];
                    }

                    if (drEmpFinalRow != null)
                    {
                        drRetDetailsFoundRows = dtRetEmpDetailsCopy.Select("guiYmcaID='" + drEmpFinalRow["guiYmcaID"].ToString() + "' AND dtmTerminationDate='" + Convert.ToDateTime(drEmpFinalRow["dtmTerminationDate"].ToString()).ToString("MM/dd/yyyy") + "'");
                        if (drRetDetailsFoundRows.Length > 0)
                        {
                            para_dtRetEmpDetails.Rows.Clear();
                            //drRetDetailsFoundRows[0]["dtmTerminationDate"] = string.Empty;
                            // dtRow = dtRetEmpDetailsCopy.NewRow();
                            dtRow = drRetDetailsFoundRows[0];
                            para_dtRetEmpDetails.ImportRow(dtRow);
                        }

                        //para_dtRetEmpDetails.Rows.Add(dtRow);  

                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // START: MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format
        private static decimal GetDisabilityAverageSalary(string para_fundEventID, string para_Retirementdate, string transactionDetails) 
        {
            return YMCARET.YmcaDataAccessObject.RetirementDAClass.GetDisabilityAverageSalary(para_fundEventID, para_Retirementdate, transactionDetails);
        }
        
        //private static decimal GetDisabilityAverageSalary(string para_fundEventID, string para_Retirementdate)
        //{
        //    try
        //    {
        //        return YMCARET.YmcaDataAccessObject.RetirementDAClass.GetDisabilityAverageSalary(para_fundEventID, para_Retirementdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        // END: MMR | 2017.03.03 | YRS-AR-2625 | Added 'transactionDetails' parameter for manual Transaction details in XML Format
        private static string GetRetireeAge60Date(string para_RetireeBirthDate)
        {
            string l_CalcMonthAge60 = string.Empty;
            try
            {
                if (!(Convert.ToDateTime(para_RetireeBirthDate).Month == 12))
                {
                    if (Convert.ToDateTime(para_RetireeBirthDate).Day == 1)
                        l_CalcMonthAge60 = Convert.ToDateTime(para_RetireeBirthDate).Month + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(para_RetireeBirthDate).Year + 60);
                    else
                        l_CalcMonthAge60 = Convert.ToDateTime(para_RetireeBirthDate).Month + 1 + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(para_RetireeBirthDate).Year + 60);
                }
                else
                {
                    if (Convert.ToDateTime(para_RetireeBirthDate).Day == 1)
                        l_CalcMonthAge60 = Convert.ToDateTime(para_RetireeBirthDate).Month + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(para_RetireeBirthDate).Year + 60);
                    else
                        l_CalcMonthAge60 = Convert.ToDateTime("01/01/1900").Month + "/" + Convert.ToDateTime("01/01/1900").Day + "/" + (Convert.ToDateTime(para_RetireeBirthDate).Year + 61);
                }
                return l_CalcMonthAge60;
            }
            catch (Exception ex) { throw ex; }
        }

        private void CreateAcctBalancesByBasisType(DataTable para_dtAccountsBasisByProjection
            , string para_planType, string para_RetireType)
        {
            decimal Personal_Retirement_Balance = 0;
            decimal YMCA_Retirement_Balance = 0;
            decimal Basic_Retirement_Balance = 0;
            decimal Basic_Personal_Balances = 0;
            decimal Basic_YMCA_Balances = 0;
            decimal NonBasic_Personal_Balances = 0;
            decimal NonBasic_YMCA_Balances = 0;
            //decimal decYmcaLegacyAmount = 0, decYmcaAccount = 0; //PPP | 11/24/2017 | YRS-AT-3319 | Variables are declared and assigned but not used
            try
            {
                DataTable dtAccountsBasisByProjection;
                dtAccountsBasisByProjection = para_dtAccountsBasisByProjection.Copy();
                DataRow[] drAcctProjectionFoundRows = null;
                DataSet l_dsMetaAccountTypes = RetirementBOClass.SearchMetaAccountTypes();
                if (g_dtAnnuityBasisTypeList != null)
                {
                    if (this.g_dtAnnuityBasisTypeList.Rows.Count > 0)
                    {
                        foreach (DataRow drAnnuityBasisTypeList in this.g_dtAnnuityBasisTypeList.Rows)
                        {
                            Personal_Retirement_Balance = 0;
                            YMCA_Retirement_Balance = 0;
                            Basic_Retirement_Balance = 0;
                            Basic_Personal_Balances = 0;
                            Basic_YMCA_Balances = 0;
                            NonBasic_Personal_Balances = 0;
                            NonBasic_YMCA_Balances = 0;
                            string annuityBasisType = string.Empty;
                            string annuityBasisGroup = string.Empty;

                            annuityBasisType = drAnnuityBasisTypeList["chrAnnuityBasisType"].ToString().Trim().ToUpper();
                            annuityBasisGroup = drAnnuityBasisTypeList["chrAnnuityBasisGroup"].ToString().Trim().ToUpper();

                            drAcctProjectionFoundRows = dtAccountsBasisByProjection.Select("chrAnnuityBasisType='" + annuityBasisType + "'");
                            if (drAcctProjectionFoundRows.Length > 0)
                            {
                                if (dtAccountsBasisByProjection.Select("chrAnnuityBasisType='" + annuityBasisType + "' AND chvPlanType = '" + para_planType + "'").Length > 0)
                                {
                                    //Personal_Retirement_Balance
                                    Personal_Retirement_Balance = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(PersonalAmt)", "chrAnnuityBasisType='" + annuityBasisType + "' AND chvPlanType = '" + para_planType + "'").ToString());
                                    //YMCA_Retirement_Balance					
                                    YMCA_Retirement_Balance = Convert.ToDecimal(dtAccountsBasisByProjection.Compute("SUM(YmcaAmt)", "chrAnnuityBasisType='" + annuityBasisType + "' AND chvPlanType = '" + para_planType + "'").ToString());
                                }
                                //Basic_AcctRetirement_Balance
                                if (l_dsMetaAccountTypes.Tables.Count != 0)
                                {
                                    foreach (DataRow drMetaAccts in l_dsMetaAccountTypes.Tables[1].Rows)
                                        foreach (DataRow drProjection in dtAccountsBasisByProjection.Rows)
                                        {
                                            if (drMetaAccts["chrAcctType"].ToString().ToUpper().Trim() == drProjection["chrAcctType"].ToString().ToUpper().Trim()
                                                && drProjection["chrAnnuityBasisType"].ToString().ToUpper().Trim() == annuityBasisType
                                                && drMetaAccts["bitBasicAcct"].ToString() == "True"
                                                && drProjection["chvPlanType"].ToString() == para_planType)
                                            {
                                                Basic_Retirement_Balance +=
                                                    Convert.ToDecimal(drProjection["PersonalAmt"]) + Convert.ToDecimal(drProjection["YmcaAmt"]);
                                                Basic_Personal_Balances += Convert.ToDecimal(drProjection["PersonalAmt"]);
                                                Basic_YMCA_Balances += Convert.ToDecimal(drProjection["YmcaAmt"]);
                                            }
                                            else if (drMetaAccts["chrAcctType"].ToString().ToUpper().Trim() == drProjection["chrAcctType"].ToString().ToUpper().Trim()
                                                && drProjection["chrAnnuityBasisType"].ToString().ToUpper().Trim() == annuityBasisType
                                                && drMetaAccts["bitBasicAcct"].ToString() == "False"
                                                && drProjection["chvPlanType"].ToString() == para_planType)
                                            {
                                                NonBasic_Personal_Balances += Convert.ToDecimal(drProjection["PersonalAmt"]);
                                                NonBasic_YMCA_Balances += Convert.ToDecimal(drProjection["YmcaAmt"]);
                                            }

                                        }
                                }
                                //Added balances into g_dtAcctBalancesByBasisType table
                                AddAcctBalancesByBasisTypeRow(Personal_Retirement_Balance, YMCA_Retirement_Balance, Basic_Retirement_Balance
                                                , annuityBasisType, annuityBasisGroup, para_planType, Basic_Personal_Balances, Basic_YMCA_Balances, NonBasic_Personal_Balances, NonBasic_YMCA_Balances);
                            }//if(drAcctProjectionFoundRows.Length >0)
                        } //foreach(DataRow drAnnuityBasisTypeList in g_dtAnnuityBasisTypeList.Rows ) 
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // public static void GetSetAcctBalByBasisTypeByPlanType(DataTable para_dtAcctBalByBasisType
        public static DataTable CombinedRetAndSavAnnuityListTable(DataTable para_dtAnnuityList_Ret, DataTable para_dtAnnuityList_Sav)
        {
            DataRow drAnnuitieslistComboTmp;
            DataTable dtAnnuitieslistComboTmp = null;
            DataTable dtAnnuitiesList_Ret = null;
            DataTable dtAnnuitiesList_Sav = null;
            try
            {
                if (para_dtAnnuityList_Ret == null && para_dtAnnuityList_Sav == null)
                    return null;


                dtAnnuitiesList_Ret = new DataTable();
                dtAnnuitiesList_Sav = new DataTable();



                //that will only contain non-zero annuity amount.
                dtAnnuitiesList_Ret = para_dtAnnuityList_Ret.Clone();
                foreach (DataRow dr in para_dtAnnuityList_Ret.Rows)
                {
                    if (Convert.ToDecimal(dr["mnyCurrentPayment"]) != 0)
                    {
                        if (dr["mnySSAfter62"].ToString() == string.Empty)
                        {
                            dr["mnySSAfter62"] = 0;
                        }
                        dtAnnuitiesList_Ret.ImportRow(dr);
                        dtAnnuitiesList_Ret.GetChanges(DataRowState.Added);
                        dtAnnuitiesList_Ret.AcceptChanges();
                    }
                }

                dtAnnuitiesList_Sav = para_dtAnnuityList_Sav.Clone();
                foreach (DataRow dr in para_dtAnnuityList_Sav.Rows)
                {
                    if (Convert.ToDecimal(dr["mnyCurrentPayment"]) != 0)
                    {
                        if (dr["mnySSAfter62"].ToString() == string.Empty)
                        {
                            dr["mnySSAfter62"] = 0;
                        }
                        dtAnnuitiesList_Sav.ImportRow(dr);
                        dtAnnuitiesList_Sav.GetChanges(DataRowState.Added);
                        dtAnnuitiesList_Sav.AcceptChanges();
                    }
                }

                dtAnnuitieslistComboTmp = new DataTable();
                dtAnnuitieslistComboTmp = para_dtAnnuityList_Ret.Clone();



                //  Create Combo table 
                if (dtAnnuitiesList_Ret.Rows.Count != 0 & para_dtAnnuityList_Sav.Rows.Count != 0)
                {
                    foreach (DataRow drAnnLst in dtAnnuitiesList_Ret.Rows)
                        foreach (DataRow drAnnFulBal in para_dtAnnuityList_Sav.Rows)
                        {
                            if (drAnnLst["chrAnnuityType"].ToString().Trim().ToUpper() == drAnnFulBal["chrAnnuityType"].ToString().Trim().ToUpper()
                                && (Convert.ToDecimal(drAnnLst["mnyCurrentPayment"]) != 0))
                            {
                                drAnnuitieslistComboTmp = dtAnnuitieslistComboTmp.NewRow();
                                drAnnuitieslistComboTmp["chrAnnuityType"] = drAnnLst["chrAnnuityType"];
                                drAnnuitieslistComboTmp["chrAnnuityFactorType"] = drAnnLst["chrAnnuityFactorType"];
                                drAnnuitieslistComboTmp["chvAnnuityCategoryCode"] = drAnnLst["chvAnnuityCategoryCode"];
                                drAnnuitieslistComboTmp["chvShortDescription"] = drAnnLst["chvShortDescription"];
                                drAnnuitieslistComboTmp["chvDescription"] = drAnnLst["chvDescription"];
                                drAnnuitieslistComboTmp["intCodeOrder"] = drAnnLst["intCodeOrder"];
                                drAnnuitieslistComboTmp["numJointSurvivorPctg"] = drAnnLst["numJointSurvivorPctg"];
                                drAnnuitieslistComboTmp["numIncreasePctg"] = drAnnLst["numIncreasePctg"];
                                drAnnuitieslistComboTmp["bitIncreasing"] = drAnnLst["bitIncreasing"];
                                drAnnuitieslistComboTmp["bitPopup"] = drAnnLst["bitPopup"];
                                drAnnuitieslistComboTmp["bitLastToDie"] = drAnnLst["bitLastToDie"];
                                drAnnuitieslistComboTmp["bitSSLeveling"] = drAnnLst["bitSSLeveling"];
                                drAnnuitieslistComboTmp["bitInsuredReserve"] = drAnnLst["bitInsuredReserve"];
                                drAnnuitieslistComboTmp["bitJointSurvivor"] = drAnnLst["bitJointSurvivor"];
                                drAnnuitieslistComboTmp["chrDBAnnuityType"] = drAnnLst["chrDBAnnuityType"];
                                drAnnuitieslistComboTmp["mnyCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"]));
                                drAnnuitieslistComboTmp["mnyPersonalPreTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyPersonalPreTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyPersonalPreTaxCurrentPayment"]));
                                drAnnuitieslistComboTmp["mnyPersonalPostTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyPersonalPostTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyPersonalPostTaxCurrentPayment"]));
                                drAnnuitieslistComboTmp["mnyYmcaPreTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyYmcaPreTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyYmcaPreTaxCurrentPayment"]));
                                drAnnuitieslistComboTmp["mnyYmcaPostTaxCurrentPayment"] = (Convert.ToDecimal(drAnnLst["mnyYmcaPostTaxCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyYmcaPostTaxCurrentPayment"]));
                                drAnnuitieslistComboTmp["mnySocialSecurityAdjPayment"] = (Convert.ToDecimal(drAnnLst["mnySocialSecurityAdjPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnySocialSecurityAdjPayment"]));
                                drAnnuitieslistComboTmp["mnyDeathBenefitPayment"] = (Convert.ToDecimal(drAnnLst["mnyDeathBenefitPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnyDeathBenefitPayment"]));
                                if (drAnnLst["bitSSLeveling"].ToString() == "True")
                                    drAnnuitieslistComboTmp["mnySSBefore62"] = (Convert.ToDecimal(drAnnLst["mnySSBefore62"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnySSBefore62"]));
                                else
                                    drAnnuitieslistComboTmp["mnySSBefore62"] = (Convert.ToDecimal(drAnnLst["mnySSBefore62"])) + (Convert.ToDecimal(drAnnFulBal["mnySSBefore62"]));

                                if (drAnnLst["bitSSLeveling"].ToString() == "True")
                                    drAnnuitieslistComboTmp["mnySSAfter62"] = (Convert.ToDecimal(drAnnLst["mnySSAfter62"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"])) + (Convert.ToDecimal(drAnnFulBal["mnySSBefore62"]));
                                else
                                    drAnnuitieslistComboTmp["mnySSBefore62"] = (Convert.ToDecimal(drAnnLst["mnySSAfter62"])) + (Convert.ToDecimal(drAnnFulBal["mnySSAfter62"]));

                                drAnnuitieslistComboTmp["mnySSIncrease"] = (Convert.ToDecimal(drAnnLst["mnySSIncrease"])) + (Convert.ToDecimal(drAnnFulBal["mnySSIncrease"]));
                                drAnnuitieslistComboTmp["mnySSDecrease"] = (Convert.ToDecimal(drAnnLst["mnySSDecrease"])) + (Convert.ToDecimal(drAnnFulBal["mnySSDecrease"]));
                                drAnnuitieslistComboTmp["mnySurvivorRetiree"] = (Convert.ToDecimal(drAnnLst["mnySurvivorRetiree"])) + (Convert.ToDecimal(drAnnFulBal["mnySurvivorRetiree"]));
                                drAnnuitieslistComboTmp["mnySurvivorBeneficiary"] = (Convert.ToDecimal(drAnnLst["mnySurvivorBeneficiary"])) + (Convert.ToDecimal(drAnnFulBal["mnySurvivorBeneficiary"]));
                                //2011.07.27    Sanket Vaidya       YRS 5.0-1150/BT-596 : Option C reduction amount should not include Dth Ben annty
                                drAnnuitieslistComboTmp["AnnuityWithoutRDB"] = (Convert.ToDecimal(drAnnLst["AnnuityWithoutRDB"])) + (Convert.ToDecimal(drAnnFulBal["mnyCurrentPayment"]));
                                //2012.06.11 SP : BT-833/YRS 5.0-1327: Re-opened
                                drAnnuitieslistComboTmp["YmcaAnnuityValue"] = (Convert.ToDecimal(drAnnLst["YmcaAnnuityValue"])) + (Convert.ToDecimal(drAnnFulBal["YmcaAnnuityValue"]));

                                dtAnnuitieslistComboTmp.Rows.Add(drAnnuitieslistComboTmp);
                                dtAnnuitieslistComboTmp.GetChanges(DataRowState.Added);
                            }
                        }
                    dtAnnuitieslistComboTmp.AcceptChanges();
                }


                return dtAnnuitieslistComboTmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Calculate the annuities for the given account values.
        ///Calculate Annuity with exact age factor logic for Disability
        /// </summary>
        /// <param name="tnFullBalance"></param>
        /// <param name="retirementType"></param>
        /// <param name="beneficiaryBirthDate"></param>
        /// <param name="retireeBirthday"></param>
        /// <param name="retirementDate"></param>
        /// <param name="dsRetEstInfoParam"></param>        
        /// <param name="benefitValue"></param>        
        /// <param name="ssNO"></param>
        /// <param name="personID"></param>
        /// <param name="dtAnnuitiesList"></param>
        /// <param name="dtAnnuitiesParam"></param>
        /// <param name="finalAnnuityParam"></param>
        /// <returns></returns>
        public DataTable CalculateAnnuitiesWithExactAgeForDisability(decimal tnFullBalance, string retirementType, DateTime beneficiaryBirthDate
            , DateTime retireeBirthday, DateTime retirementDate, DataSet dsParticipantBeneficiaries
            , decimal benefitValue, string ssNO, string personID
            , DataTable dtAnnuitiesList, DataTable dtAnnuitiesParam, ref decimal finalAnnuityParam)
        {
            DataTable dtAnnuities = null;
            DataTable dtDistinctAnnuityType = null;
            DataTable dtMetaAnnuityFactor = null;
            DataTable dtAnnuitiesStaging = null;
            string ldRetireeBirthdate;
            string ldRetirementDate;
            decimal finalannuity = 0;
            DataRow[] drMetaAnnuityFactorRows = null;
            string l_AnnuityType = string.Empty;
            decimal l_AnnuityAmount = 0;

            //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect 
            decimal l_YmcaAnnuityAmount = 0;

            try
            {
                dtAnnuities = new DataTable();
                //Added by Ashish for phase V Part III changes, Start


                //Added by Ashish for phase V Part III changes, End
                ldRetireeBirthdate = retireeBirthday.ToString();
                ldRetirementDate = retirementDate.ToShortDateString();

                DataSet dsMetaAnnuityFactors = new DataSet();
                //TO DO Get factor for disability annuity
                // Step 1. Get annuity factors
                dsMetaAnnuityFactors = RetirementBOClass.getMetaAnnuityFactorsForDisability(dsParticipantBeneficiaries, retirementType, ldRetirementDate, ldRetireeBirthdate, beneficiaryBirthDate.ToShortDateString());
                //Added by Ashish for phase V part III changes 
                if (dsMetaAnnuityFactors != null && dsMetaAnnuityFactors.Tables.Count > 0)
                {
                    dtMetaAnnuityFactor = dsMetaAnnuityFactors.Tables[0];
                }

                //// Get vested info
                //bool isVested = false;
                //DataSet dsIsVested = RetirementBOClass.getVestedInfo(ssNO, ldRetirementDate);
                //if (dsIsVested.Tables.Count != 0)
                //{
                //    if (dsIsVested.Tables[0].Rows.Count != 0)
                //    {
                //        if (dsIsVested.Tables[0].Rows[0]["IsVested"].ToString() == "True")
                //            isVested = true;
                //        else
                //            isVested = false;
                //    }
                //}

                string[] targetColumns = { "AnnuityType" };
                string[] compareColumns = { "AnnuityType" };
                string[] clubbedColumns = null;
                // Get distinct basis type clubbed amount
                dtDistinctAnnuityType = SelectDistinct(dtMetaAnnuityFactor, targetColumns, compareColumns, clubbedColumns);


                //Create Table schema for AnnutiyList            
                CreateAnnuityListSchema(ref dtAnnuitiesStaging);

                if (this.g_dtAcctBalancesByBasisType != null)
                {
                    if (this.g_dtAcctBalancesByBasisType.Rows.Count > 0)
                    {
                        foreach (DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                        {

                            drMetaAnnuityFactorRows = dtMetaAnnuityFactor.Select("BasisTypeCode='" + drAcctBalancesByBasisType["chrAnnuityBasisType"].ToString().Trim().ToUpper() + "'");
                            foreach (DataRow drDistinctAnnuityRow in dtDistinctAnnuityType.Rows)
                            {
                                l_AnnuityType = string.Empty;

                                //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect 
                                l_YmcaAnnuityAmount = 0;

                                l_AnnuityType = drDistinctAnnuityRow["AnnuityType"].ToString().Trim().ToUpper();

                                //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect 
                                l_AnnuityAmount = CalculateAnnuityValueForDisability(drMetaAnnuityFactorRows, drAcctBalancesByBasisType, l_AnnuityType, ref l_YmcaAnnuityAmount);
                                AddUpdateAnnuityListRow(ref dtAnnuitiesStaging, l_AnnuityType, l_AnnuityAmount, l_YmcaAnnuityAmount);
                            }



                        }//foreach(DataRow drAcctBalancesByBasisType in this.g_dtAcctBalancesByBasisType.Rows)
                    }//if(this.g_dtAcctBalancesByBasisType.Rows.Count >0 )
                }//if(this.g_dtAcctBalancesByBasisType!=null)



                // Update the staging table with the SSBalancing amounts
                RetirementBOClass.calculateSSBalancing(false, benefitValue
                    , dtAnnuitiesStaging, beneficiaryBirthDate, retireeBirthday, retirementDate);

                // Update the staging table with the Beneficiary and Survivor amounts
                RetirementBOClass.calculateSurvivorBenefeciary(dtAnnuitiesStaging, retirementDate);

                {
                    dtAnnuities = dtAnnuitiesStaging.Clone();
                    if (dtAnnuitiesStaging.Rows.Count != 0)
                    {
                        for (int i = 0; i <= dtAnnuitiesStaging.Rows.Count - 1; i++)
                        {
                            dtAnnuities.ImportRow(dtAnnuitiesStaging.Rows[i]);
                            dtAnnuities.GetChanges(DataRowState.Added);
                            dtAnnuities.AcceptChanges();
                        }
                    }
                }

                dtAnnuitiesParam = dtAnnuities;

                // Call the contentious method 
                DataTable dt = RetirementBOClass.contentiousMethod(dtAnnuities, false, retirementDate, finalannuity);
                dtAnnuitiesList = dt;

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //2012.05.18 SP:  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -(Commented )
        //private static bool IsSurvivorExists(DataSet para_dsRetireeBeneficiary, string para_BeneficiaryBirthDate, out string para_beneficiaryDateOfBirth)
        //{
        //    bool boolSurvivor = false;
        //    bool boolContAndSpouse = false;
        //    bool boolPrimDOBPctg = false;
        //    string beneficiaryDateOfBirth = string.Empty;
        //    double benefitPctg = 0.00;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        if (para_dsRetireeBeneficiary != null)
        //        {
        //            // Step 1. Get survivor status
        //            if (para_dsRetireeBeneficiary.Tables.Count > 0)
        //            {
        //                foreach (DataRow dr in para_dsRetireeBeneficiary.Tables[0].Rows)
        //                {
        //                    benefitPctg = Convert.ToDouble(dr["intBenefitPctg"].ToString());
        //                    if (dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "PRIM"
        //                        && dr["BenBirthDate"].ToString().Trim() != string.Empty
        //                        //&& dr["intBenefitPctg"].ToString().Trim().ToUpper() == "100.00000") 
        //                        && benefitPctg == 100.00)
        //                    {
        //                        beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
        //                        if (para_dsRetireeBeneficiary.Tables[0].Rows.Count == 1)
        //                        {
        //                            boolSurvivor = true;
        //                            break;
        //                        }
        //                        boolPrimDOBPctg = true;
        //                    }
        //                    if (dr["chvRelationshipCode"].ToString().Trim().ToUpper() == "SP"
        //                        & dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "CONT")
        //                    {
        //                        beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
        //                        boolContAndSpouse = true;
        //                    }
        //                    if ((dr["chvRelationshipCode"].ToString().Trim().ToUpper() == "SP"
        //                        & dr["chvBeneficiaryGroupCode"].ToString().Trim().ToUpper() == "PRIM"))
        //                    {
        //                        beneficiaryDateOfBirth = dr["BenBirthDate"].ToString().Trim();
        //                        boolSurvivor = true;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        if (boolSurvivor == false && boolPrimDOBPctg == true && boolContAndSpouse == false)
        //            boolSurvivor = true;

        //        if (para_BeneficiaryBirthDate.ToString() != "1/1/1900")
        //        {
        //            beneficiaryDateOfBirth = para_BeneficiaryBirthDate.ToString();
        //            boolSurvivor = true;
        //        }
        //        para_beneficiaryDateOfBirth = beneficiaryDateOfBirth;
        //        return boolSurvivor;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private static DataSet getMetaAnnuityFactorsForDisability(DataSet para_dsRetBeneficiaryInfo, string retireType, string retirementDate, string retireeBirthdate, string beneficiaryBirthDate)
        {
            DataSet ds = new DataSet();
            bool boolSurvivor = false;
            string beneficiaryDateOfBirth = string.Empty;
            //Step 1. Get survivor status and beneficiaryDateOfBirth

            //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary-Start
            //boolSurvivor = IsSurvivorExists(para_dsRetBeneficiaryInfo, beneficiaryBirthDate, out beneficiaryDateOfBirth);

            // Step 2. Get the Annuity Factors as per the survivor status.
            if (IsNonEmpty(para_dsRetBeneficiaryInfo))
            {
                if (para_dsRetBeneficiaryInfo.Tables[0].Rows[0]["BenBirthDate"].ToString().Trim() != string.Empty)
                {
                    beneficiaryDateOfBirth = para_dsRetBeneficiaryInfo.Tables[0].Rows[0]["BenBirthDate"].ToString().Trim();
                    boolSurvivor = true;
                }
            }
            //2012.05.18   SP :  BT-976/YRS 5.0-1507 - add grid of beneificiaries to select for annutiy beneficiary -Commented  becuase user has passing benificiary -End
            // Step 2. Get the Annuity Factors as per the survivor status.
            if (boolSurvivor)
                ds = RetirementBOClass.SearchMetaAnnuityFactorsForDisability(retireType, retirementDate, retireeBirthdate, beneficiaryDateOfBirth);
            else
                ds = RetirementBOClass.SearchMetaAnnuityFactorsForDisability(retireType, retirementDate, retireeBirthdate, string.Empty);

            return ds;
        }

        //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect - (new parameter is added para_YmcaAnnuityAmount for calculating ymca side annuity amt.)
        private decimal CalculateAnnuityValueForDisability(DataRow[] para_drMetaAnnuityFactorRows, DataRow para_drAcctBalancesByBasisType, string para_AnnuityType, ref decimal para_YmcaAnnuityAmount)
        {
            //decimal l_NormalAnnuityFactor = 0; //PPP | 03/06/2017 | YRS-AT-2625 | Not using normal annuity factors in disablity
            decimal l_DisableAnnuityFactor = 0;
            //START: PPP | 03/06/2017 | YRS-AT-2625 | Not using normal annuity factors in disablity and exact age disablity is used
            //decimal l_NormalAge60AnnuityFactor = 0;
            //decimal l_Normal_M_AtRetDateAnnuityFactor = 0;
            //decimal l_Disable_M_AtRetDateAnnuityFactor = 0;
            //END: PPP | 03/06/2017 | YRS-AT-2625 | Not using normal annuity factors in disablity and exact age disablity is used
            decimal l_AnnuityAmount = 0;
            decimal l_BasicPersonalAcctBalance = 0;
            decimal l_BasicYmcaAcctBalance = 0;
            decimal l_NonBasicPersonalAcctBalance = 0;
            decimal l_NonBasicYmcaAcctBalance = 0;

            //SP  BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect
            decimal l_finalYMCAAnnuityAmount = 0;
            try
            {
                //START: PPP | 03/06/2017 | YRS-AT-2625 
                // Please find attached document "New disability requirements.docx" from this gemini ticket, where it is mentioned
                // how and which factors to use to calculate annuities
                //END: PPP | 03/06/2017 | YRS-AT-2625 

                if (para_drMetaAnnuityFactorRows.Length > 0 && para_drAcctBalancesByBasisType != null)
                {
                    //Get Disability Factors for which annuity we are going to compute and set into the variables
                    for (int i = 0; i < para_drMetaAnnuityFactorRows.Length; i++)
                    {
                        //START: PPP | 03/06/2017 | YRS-AT-2625 | "DISABL" factors will be used for disability annuity calculation
                        //DISABLE Factor for actual age at RetirmentDate 
                        if (para_drMetaAnnuityFactorRows[i]["RetirementType"].ToString().Trim().ToUpper() == "DISABL"
                            && para_drMetaAnnuityFactorRows[i]["AnnuityType"].ToString().Trim().ToUpper() == para_AnnuityType.Trim().ToUpper())
                        {
                            l_DisableAnnuityFactor = Convert.ToDecimal(para_drMetaAnnuityFactorRows[i]["Factor"]);
                            break;
                        }

                        ////Normal Factor for age at RetirmentDate 
                        //if (para_drMetaAnnuityFactorRows[i]["RetirementType"].ToString().Trim().ToUpper() == "NORMAL"
                        //    && para_drMetaAnnuityFactorRows[i]["AnnuityType"].ToString().Trim().ToUpper() == para_AnnuityType.Trim().ToUpper()
                        //    && Convert.ToBoolean(para_drMetaAnnuityFactorRows[i]["IsFactorAge60"].ToString()) == false)
                        //    l_NormalAnnuityFactor = Convert.ToDecimal(para_drMetaAnnuityFactorRows[i]["Factor"]);
                        ////DISABLE Factor for age at RetirmentDate 
                        //if (para_drMetaAnnuityFactorRows[i]["RetirementType"].ToString().Trim().ToUpper() == "DISABL"
                        //    && para_drMetaAnnuityFactorRows[i]["AnnuityType"].ToString().Trim().ToUpper() == para_AnnuityType.Trim().ToUpper()
                        //    && Convert.ToBoolean(para_drMetaAnnuityFactorRows[i]["IsFactorAge60"].ToString()) == false)
                        //    l_DisableAnnuityFactor = Convert.ToDecimal(para_drMetaAnnuityFactorRows[i]["Factor"]);

                        //// M NORMAL factor for Age60 Factor
                        //if (para_drMetaAnnuityFactorRows[i]["RetirementType"].ToString().Trim().ToUpper() == "NORMAL"
                        //    && para_drMetaAnnuityFactorRows[i]["AnnuityType"].ToString().Trim().ToUpper() == "M"
                        //    && Convert.ToBoolean(para_drMetaAnnuityFactorRows[i]["IsFactorAge60"].ToString()) == true)
                        //    l_NormalAge60AnnuityFactor = Convert.ToDecimal(para_drMetaAnnuityFactorRows[i]["Factor"]);

                        //if (para_AnnuityType.Trim().ToUpper() == "C")
                        //{
                        //    //M NORMAL factor for age at RetirmentDate 
                        //    if (para_drMetaAnnuityFactorRows[i]["RetirementType"].ToString().Trim().ToUpper() == "NORMAL"
                        //    && para_drMetaAnnuityFactorRows[i]["AnnuityType"].ToString().Trim().ToUpper() == "M"
                        //    && Convert.ToBoolean(para_drMetaAnnuityFactorRows[i]["IsFactorAge60"].ToString()) == false)
                        //        l_Normal_M_AtRetDateAnnuityFactor = Convert.ToDecimal(para_drMetaAnnuityFactorRows[i]["Factor"]);
                        //    //M Disable factor for age at RetirmentDate 
                        //    if (para_drMetaAnnuityFactorRows[i]["RetirementType"].ToString().Trim().ToUpper() == "DISABL"
                        //    && para_drMetaAnnuityFactorRows[i]["AnnuityType"].ToString().Trim().ToUpper() == "M"
                        //    && Convert.ToBoolean(para_drMetaAnnuityFactorRows[i]["IsFactorAge60"].ToString()) == false)
                        //        l_Disable_M_AtRetDateAnnuityFactor = Convert.ToDecimal(para_drMetaAnnuityFactorRows[i]["Factor"]);
                        //}
                        //END: PPP | 03/06/2017 | YRS-AT-2625 | "DISABL" factors will be used for disability annuity calculation
                    }

                    //Get balances
                    l_BasicPersonalAcctBalance = Convert.ToDecimal(para_drAcctBalancesByBasisType["mnyBasicPersonalBalances"].ToString());
                    l_BasicYmcaAcctBalance = Convert.ToDecimal(para_drAcctBalancesByBasisType["mnyBasicYmcaBalance"].ToString());
                    l_NonBasicPersonalAcctBalance = Convert.ToDecimal(para_drAcctBalancesByBasisType["mnyNonBasicPersonalBalance"].ToString());
                    l_NonBasicYmcaAcctBalance = Convert.ToDecimal(para_drAcctBalancesByBasisType["mnyNonBasicYmcalBalance"].ToString());

                    //START: PPP | 03/06/2017 | YRS-AT-2625 | Calculation is now simple, just devide it by respective annuities factor
                    /*
                     * Following formula is mentioned in the "New disability requirements.docx" document
                     * a) Retirement Plan Option M:
                     * (Basic Personal + Basic YMCA + non-Basic Personal +Non-basic YMCA) divided by DISABILITY PST96 ‘M’ factor for actual age at retirement
                     * b) Retirement Plan Option C:
                     * Basic Personal + Basic YMCA + non-Basic Personal + Non-basic YMCA) divided by DISABILITY PST96 ‘C’ factor for actual age at retirement
                     * c) Retirement Plan J&S Options:
                     * Basic Personal + Basic YMCA + non-Basic Personal + non-Basic YMCA) divided by DISABILITY PST96 J&S factor for actual participant age and actual beneficiary age at retirement 
                     */
                    l_AnnuityAmount = (l_BasicPersonalAcctBalance + l_BasicYmcaAcctBalance + l_NonBasicPersonalAcctBalance + l_NonBasicYmcaAcctBalance) / l_DisableAnnuityFactor;
                    if (para_AnnuityType == "C")
                        l_finalYMCAAnnuityAmount = (l_BasicYmcaAcctBalance + l_NonBasicYmcaAcctBalance) / l_DisableAnnuityFactor;

                    //switch (para_AnnuityType)
                    //{
                    //    case "M":
                    //        //formula (Basic Personal + Basic YMCA) divided by NORMAL PST96 ‘M’ factor for age 60 +
                    //        //          (non-Basic Personal + non-Basic YMCA) divided by NORMAL PST96 ‘M’ factor for their age at retirement

                    //        if (l_NormalAge60AnnuityFactor != 0 && l_NormalAnnuityFactor != 0)
                    //            l_AnnuityAmount = (l_BasicPersonalAcctBalance + l_BasicYmcaAcctBalance) / l_NormalAge60AnnuityFactor
                    //                    + (l_NonBasicPersonalAcctBalance + l_NonBasicYmcaAcctBalance) / l_NormalAnnuityFactor;

                    //        break;
                    //    case "C":
                    //        // Formula (Basic Personal + Basic YMCA) divided by NORMAL PST96 ‘M’ factor for age 60 - 
                    //        //Basic Personal divided by Disability PST96 ‘M’ factor for age at retirement + 
                    //        //Basic Personal divided by Disability PST96 ‘C’ factor for age at retirement’ + 
                    //        //Non-Basic Personal divided by NORMAL PST96 ‘C’ factor for age at retirement +
                    //        //Non-Basic YMCA divided by NORMAL PST96 ‘M’ factor for age at retirement.
                    //        if (l_NormalAge60AnnuityFactor != 0 && l_Disable_M_AtRetDateAnnuityFactor != 0 && l_DisableAnnuityFactor != 0 && l_NormalAnnuityFactor != 0 && l_Normal_M_AtRetDateAnnuityFactor != 0)
                    //            l_AnnuityAmount = (l_BasicPersonalAcctBalance + l_BasicYmcaAcctBalance) / l_NormalAge60AnnuityFactor
                    //                            - (l_BasicPersonalAcctBalance / l_Disable_M_AtRetDateAnnuityFactor) + (l_BasicPersonalAcctBalance / l_DisableAnnuityFactor)
                    //                            + (l_NonBasicPersonalAcctBalance / l_NormalAnnuityFactor) + (l_NonBasicYmcaAcctBalance / l_Normal_M_AtRetDateAnnuityFactor);

                    //        //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect -(forcalculating YMCA side annuity amount
                    //        l_finalYMCAAnnuityAmount = ((l_BasicPersonalAcctBalance + l_BasicYmcaAcctBalance) / l_NormalAge60AnnuityFactor)
                    //                                    - (l_BasicPersonalAcctBalance / l_Disable_M_AtRetDateAnnuityFactor)
                    //                                       + (l_NonBasicYmcaAcctBalance / l_Normal_M_AtRetDateAnnuityFactor);
                    //        break;
                    //    default:
                    //        // (Basic Personal + Basic YMCA) divided by DISABILITY PST96 J&S factor for participant  age  and beneficiary age at  retirement date +
                    //        //(non-Basic Personal + non-Basic YMCA) divided by NORMAL PST96 J&S factor for their age at retirement date.
                    //        if (l_DisableAnnuityFactor != 0 && l_NormalAnnuityFactor != 0)
                    //            l_AnnuityAmount = (l_BasicPersonalAcctBalance + l_BasicYmcaAcctBalance) / l_DisableAnnuityFactor
                    //                        + (l_NonBasicPersonalAcctBalance + l_NonBasicYmcaAcctBalance) / l_NormalAnnuityFactor;


                    //        break;


                    //}
                    l_AnnuityAmount = Math.Round(l_AnnuityAmount / 12, 2);

                    //SP : BT-833/YRS 5.0-1327:Option C Insured Reserves and Reduction Amt incorrect 
                    para_YmcaAnnuityAmount = Math.Round(l_finalYMCAAnnuityAmount / 12, 2);
                }

                return l_AnnuityAmount;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private bool ValidateTerminationIsCloseEnough(DataTable para_dtRetEmpDetails, string para_RetirementDate, ref string para_errorMessage)
        {
            bool flag = true;
            DateTime start;
            DateTime end;
            DateTime terminationDate;
            DataTable dtKeyValue = null;
            int termInterval = 6;
            try
            {
                if (para_dtRetEmpDetails != null)
                {
                    if (para_dtRetEmpDetails.Rows.Count > 0)
                    {
                        dtKeyValue = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("RETIRE_DISABILITY_MAX_TERM_INTERVAL_MONTHS");
                        if (dtKeyValue != null)
                        {
                            if (dtKeyValue.Rows.Count > 0)
                            {
                                if (dtKeyValue.Rows[0]["Key"].ToString().Trim().ToUpper() == "RETIRE_DISABILITY_MAX_TERM_INTERVAL_MONTHS")
                                    termInterval = Convert.ToInt32(dtKeyValue.Rows[0]["Value"].ToString());
                            }
                        }
                        terminationDate = Convert.ToDateTime(para_dtRetEmpDetails.Rows[0]["dtmTerminationDate"].ToString());
                        start = Convert.ToDateTime(para_RetirementDate).AddMonths(-termInterval);

                        end = Convert.ToDateTime(para_RetirementDate);
                        if (terminationDate < start || terminationDate > end)
                        {
                            flag = false;
                            para_errorMessage = "Termination not close enough.";
                        }
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //This methos set termination date with Retiree age 60 date, so system can project contribution
        private void SetAge60TerminationDateForDisability(DataTable para_dtRetProjectedSalDetails, string para_RetireeBirthday)
        {
            string l_CalMonthAge60 = string.Empty;
            try
            {
                if (para_dtRetProjectedSalDetails != null)
                {
                    if (para_dtRetProjectedSalDetails.Rows.Count > 0)
                    {
                        l_CalMonthAge60 = GetRetireeAge60Date(para_RetireeBirthday);
                        para_dtRetProjectedSalDetails.Rows[0]["dtmTerminationDate"] = Convert.ToDateTime(l_CalMonthAge60);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //This Method Terminated elective accounts for projection
        private void TerminateElectiveAccountsForDisability(ref DataSet para_dsElectiveAccounts, string para_ldCalcMonthToday, string para_RetirementDate)
        {
            DataRow[] drElectiveFoundRows = null;
            try
            {
                if (para_dsElectiveAccounts != null)
                {
                    if (para_dsElectiveAccounts.Tables.Count > 0)
                    {
                        if (para_dsElectiveAccounts.Tables[0].Rows.Count > 0)
                        {
                            drElectiveFoundRows = para_dsElectiveAccounts.Tables[0].Select("bitBasicAcct=False AND bitRet_Voluntary=True");
                            if (drElectiveFoundRows.Length > 0)
                            {
                                for (int i = 0; i < drElectiveFoundRows.Length; i++)
                                {
                                    if (Convert.ToDateTime(para_ldCalcMonthToday) > Convert.ToDateTime(para_RetirementDate))
                                        drElectiveFoundRows[i]["dtsTerminationDate"] = para_RetirementDate;
                                    else
                                        drElectiveFoundRows[i]["dtsTerminationDate"] = para_ldCalcMonthToday;
                                }
                                para_dsElectiveAccounts.AcceptChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        //YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        public static int GetRetireeAndBeneficiaryAgeDiff(string p_RetireeBirthDate, string p_BeneficiaryBirthDate, string p_RetirementDate)
        {
            DateTime RetireeBirthDate = Convert.ToDateTime(p_RetireeBirthDate);
            DateTime BeneficiaryBirthDate = Convert.ToDateTime(p_BeneficiaryBirthDate);
            DateTime RetirementDate = Convert.ToDateTime(p_RetirementDate);
            //Ashish:2011.10.04 YRS 5.0-1329
            //if (RetireeBirthDate.Day > 1)
            //    RetireeBirthDate = RetireeBirthDate.AddDays(-(RetireeBirthDate.Day - 1)).AddMonths(1);
            //if (BeneficiaryBirthDate.Day > 1)
            //    BeneficiaryBirthDate = BeneficiaryBirthDate.AddDays(-(BeneficiaryBirthDate.Day - 1)).AddMonths(1);

            //int retireeAge = RetirementDate.Year - RetireeBirthDate.Year - (RetireeBirthDate.Month > RetirementDate.Month ? 1 : 0);
            //int beneficiaryAge = RetirementDate.Year - BeneficiaryBirthDate.Year - (BeneficiaryBirthDate.Month > RetirementDate.Month ? 1 : 0);

            int retireeAge = RetirementDate.Year - RetireeBirthDate.Year;
            int beneficiaryAge = RetirementDate.Year - BeneficiaryBirthDate.Year;

            int ageDiff = 0;
            if (retireeAge >= 70)
                ageDiff = retireeAge - beneficiaryAge;
            else if (retireeAge < 70)
            {
                int yearsForAge70 = 70 - retireeAge;
                ageDiff = (retireeAge - beneficiaryAge) - yearsForAge70;
            }
            return ageDiff;
        }

        //YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        //START : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783| New paramters added to handle new Secure Act rule.
        //If beneficiary selected as non-spouse who is more than 10 years younger than the Participant ,Participant does not allow J options else existing logic will work as it is
        //public static DataTable AvailJandSAnnuityOptions(int p_Agediff, DataTable p_DtAnnuity)
        public static DataTable AvailJandSAnnuityOptions(int p_Agediff, DataTable p_DtAnnuity, bool secureActApplicable)
        //END : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | New paramters added to handle new Secure Act rule.
        {
            string expression = string.Empty;
            DataRow[] founRows;

            if (p_Agediff >= 11 && p_Agediff <= 19)
            {
                expression = "Annuity IN ('J1','J1P')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["Retire"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                    dr["Survivor"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                    if (Convert.ToString(dr["Beneficiary"]) != String.Empty)
                        dr["Beneficiary"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                }
            }
            else if (p_Agediff >= 20)
            {
                expression = "Annuity IN ('J1','J1P','J7','J7P')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["Retire"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                    dr["Survivor"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                    if (Convert.ToString(dr["Beneficiary"]) != String.Empty)
                        dr["Beneficiary"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                }
            }

            //START : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | Non-spouse who is more than 10 years younger than the Participant and does not display J options.                  
            if (secureActApplicable)
                {
                    expression = "Annuity IN ('J1','J1P','J7','J7P','J5','J5P')";
                    founRows = p_DtAnnuity.Select(expression);
                    foreach (DataRow dr in founRows)
                    {
                        dr["Retire"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                        dr["Survivor"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                        if (Convert.ToString(dr["Beneficiary"]) != String.Empty)
                            dr["Beneficiary"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                    }
                }
            //END : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | Non-spouse who is more than 10 years younger than the Participant and does not display J options.

            p_DtAnnuity.AcceptChanges();
            return p_DtAnnuity;
        }

        //YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        public static string GetNonSpouseBeneficiaryBirthDate(string p_PersID)
        {
            DataSet dsBeneficiaryInfo;
            DataRow[] drowsBeneficiary;
            DataRow drowBeneficary = null;
            string dateOfBirth = string.Empty;
            dsBeneficiaryInfo = getParticipantBeneficiaries(p_PersID);

            if (dsBeneficiaryInfo != null)
            {
                if (dsBeneficiaryInfo.Tables[0].Rows.Count > 0)
                {
                    drowsBeneficiary = dsBeneficiaryInfo.Tables[0].Select("chvRelationshipCode <> 'SP' AND chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100");
                    if (drowsBeneficiary.Length > 0)
                    {
                        drowBeneficary = drowsBeneficiary[0];
                        if (!Convert.IsDBNull(drowBeneficary["BenBirthDate"]))
                        {
                            if (drowBeneficary["BenBirthDate"].ToString() != string.Empty)
                                dateOfBirth = Convert.ToDateTime(drowBeneficary["BenBirthDate"]).ToString("MM/dd/yyyy");
                        }
                    }
                }
            }

            return dateOfBirth;
        }
        //ASHISH:2011.10.04 -YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        public static string GetNonSpouseBeneficiaryBirthDateForProcessing(string p_PersID, string p_PlanType)
        {
            DataSet dsBeneficiaryInfo;
            DataRow[] drowsBeneficiary;
            DataRow drowBeneficary = null;
            string dateOfBirth = string.Empty;
            dsBeneficiaryInfo = getParticipantBeneficiaries(p_PersID);

            if (dsBeneficiaryInfo != null)
            {
                if (dsBeneficiaryInfo.Tables[0].Rows.Count > 0)
                {
                    if (p_PlanType == "R" || p_PlanType == string.Empty)
                    {
                        drowsBeneficiary = dsBeneficiaryInfo.Tables[0].Select("chvRelationshipCode <> 'SP' AND chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100 AND chvBeneficiaryTypeCode = 'MEMBER'");
                        if (drowsBeneficiary.Length > 0)
                        {
                            drowBeneficary = drowsBeneficiary[0];
                            if (!Convert.IsDBNull(drowBeneficary["BenBirthDate"]))
                            {
                                if (drowBeneficary["BenBirthDate"].ToString() != string.Empty)
                                    dateOfBirth = Convert.ToDateTime(drowBeneficary["BenBirthDate"]).ToString("MM/dd/yyyy");
                            }
                        }
                    }
                    else if (p_PlanType == "S" || p_PlanType == string.Empty)
                    {
                        drowsBeneficiary = dsBeneficiaryInfo.Tables[0].Select("chvRelationshipCode <> 'SP' AND chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100 AND chvBeneficiaryTypeCode = 'SAVING'");
                        if (drowsBeneficiary.Length > 0)
                        {
                            drowBeneficary = drowsBeneficiary[0];
                            if (!Convert.IsDBNull(drowBeneficary["BenBirthDate"]))
                            {
                                if (drowBeneficary["BenBirthDate"].ToString() != string.Empty)
                                    dateOfBirth = Convert.ToDateTime(drowBeneficary["BenBirthDate"]).ToString("MM/dd/yyyy");
                            }
                        }
                        else
                        {
                            drowsBeneficiary = dsBeneficiaryInfo.Tables[0].Select("chvRelationshipCode <> 'SP' AND chvBeneficiaryGroupCode = 'PRIM' AND intBenefitPctg=100 AND chvBeneficiaryTypeCode = 'MEMBER'");
                            if (drowsBeneficiary.Length > 0)
                            {
                                drowBeneficary = drowsBeneficiary[0];
                                if (!Convert.IsDBNull(drowBeneficary["BenBirthDate"]))
                                {
                                    if (drowBeneficary["BenBirthDate"].ToString() != string.Empty)
                                        dateOfBirth = Convert.ToDateTime(drowBeneficary["BenBirthDate"]).ToString("MM/dd/yyyy");
                                }
                            }
                        }
                    }
                }
            }

            return dateOfBirth;
        }

        //YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        public static DataTable AvailJandSAnnuityOptions_RetProcessing(int p_Agediff, DataTable p_DtAnnuity)
        {
            string expression = string.Empty;
            DataRow[] founRows;

            if (p_Agediff >= 11 && p_Agediff <= 19)
            {
                expression = "Annuity IN ('J1','J1P')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["Amount"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                }
            }
            else if (p_Agediff >= 20)
            {
                expression = "Annuity IN ('J1','J1P','J7','J7P')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["Amount"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                }
            }
            p_DtAnnuity.AcceptChanges();
            return p_DtAnnuity;
        }

        //YRS 5.0-1329:J&S options available to non-spouse beneficiaries
        //START : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | New paramters added to handle new Secure Act rule.
        //If beneficiary selected as non-spouse who is more than 10 years younger than the Participant ,Participant does not allow J options else existing logic will work as it is
        //public static DataTable AvailJandSAnnuityOptionsForPrintEstimate(int p_Agediff, DataTable p_DtAnnuity)
        public static DataTable AvailJandSAnnuityOptionsForPrintEstimate(int p_Agediff, DataTable p_DtAnnuity, bool secureActApplicable)
        //END : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | New paramters added to handle new Secure Act rule.      
        {
            string expression = string.Empty;
            DataRow[] founRows;
            if (p_Agediff >= 11 && p_Agediff <= 19)
            {
                expression = "chrAnnuityType IN ('J1','J1P','J1S','J1PS')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["mnyCurrentPayment"] = 0;
                    dr["mnySurvivorRetiree"] = 0;
                    dr["mnySurvivorBeneficiary"] = 0;
                    dr["AnnuityWithoutRDB"] = 0;
                }
            }
            else if (p_Agediff >= 20)
            {
                expression = "chrAnnuityType IN ('J1','J1P','J1S','J1PS','J7','J7P','J7S','J7PS')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["mnyCurrentPayment"] = 0;
                    dr["mnySurvivorRetiree"] = 0;
                    dr["mnySurvivorBeneficiary"] = 0;
                    dr["AnnuityWithoutRDB"] = 0;
                }
            }

            //START : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | Non-spouse who is more than 10 years younger than the Participant and does not display J options.
            if (secureActApplicable)
            {
                expression = "chrAnnuityType IN ('J1','J1P','J1S','J1PS','J7','J7P','J7S','J7PS','J5','J5P','J5PS','J5S')";
                founRows = p_DtAnnuity.Select(expression);
                foreach (DataRow dr in founRows)
                {
                    dr["mnyCurrentPayment"] = 0;
                    dr["mnySurvivorRetiree"] = 0;
                    dr["mnySurvivorBeneficiary"] = 0;
                    dr["AnnuityWithoutRDB"] = 0;
                }
            }
            //END : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | Non-spouse who is more than 10 years younger than the Participant and does not display J options.

            p_DtAnnuity.AcceptChanges();
            return p_DtAnnuity;
        }

        //2012.06.28 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -Start
        /// <summary>
        /// This methods checks, if any DLIN records exist before retirement date then return true else false 
        /// </summary>
        /// <param name="retireeGuiFundEventId"></param>
        /// <param name="retireeRetireDate"></param>
        /// <returns>Boolean</returns>
        public static bool IsExistsDLINRecordBeforeRetirement(string retireeGuiFundEventId, string retireeRetireDate)
        {
            bool isDLINRecordExist = false;
            try
            {

                isDLINRecordExist = RetirementDAClass.IsExistsDLINRecordBeforeRetirement(retireeGuiFundEventId, retireeRetireDate);
            }
            catch
            {
                throw;
            }
            return isDLINRecordExist;
        }
        //2012.06.28 SP : BT-712/YRS 5.0-1246 : Handling DLIN records -End

        //2015.11.24 SP : YRS Ticket-2610 :  Restriction for purchase of Death Benefit Annuity Purchase-Start
        //Start Added by Chandra sekar.c YRS- Method Return boolean value if any Participant is below or above of Retriee service Minimun age(55) as of Death Benefit Annuity Purchase Restriction date(01/01/2019)
        public static bool IsDeathBenefitAnnuityPurchaseRestricted(int Param_PersonMinAgeToRetire, DateTime Param_PersonDOB, DateTime Param_AsOfDate)
        {
            DateTime cutOffDate = Param_AsOfDate.AddYears((Param_PersonMinAgeToRetire * -1));
            if (Param_PersonDOB > cutOffDate)
                return true;
            else
                return false;
        }
        //End Added by Chandra sekar.c YRS- Method Return boolean value if any Participant is below or above of Retriee service Minimun age(55) as of Death Benefit Annuity Purchase Restriction date(01/01/2019)

        //Start Added by Chandra sekar.c YRS-Getting Minimun Service Retriee Age
        public static int GetMinimumAgeToRetire()
        {
            DataSet l_DataTableRETIRE_SERVICE_MIN_AGE = null;

            int intRetireeServiceMinAge = 55;
            try
            {

                l_DataTableRETIRE_SERVICE_MIN_AGE = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("RETIRE_SERVICE_MIN_AGE");


                if (IsNonEmpty(l_DataTableRETIRE_SERVICE_MIN_AGE))
                {
                    if (l_DataTableRETIRE_SERVICE_MIN_AGE.Tables[0].Rows[0]["Value"].ToString().Trim() != string.Empty)
                    {
                        intRetireeServiceMinAge = Convert.ToInt32(l_DataTableRETIRE_SERVICE_MIN_AGE.Tables[0].Rows[0]["Value"]);
                    }

                }

                return intRetireeServiceMinAge;
            }
            catch
            {
                throw;
            }

        }
        //End Added by Chandra sekar.c YRS-Getting Minimun Service Retriee Age

        //Start Added by Chandra sekar.c YRS-Getting Death Benefit Annuity Purchase Restricted Date
        public static DateTime DeathBenefitAnnuityPurchaseRestrictedDate()
        {
            DataSet l_dsDth_Benefit_Ann_Purchase_Retriction_date = null;

            DateTime DthBenRestrictedDate = Convert.ToDateTime("1/1/2019");
            try
            {
                l_dsDth_Benefit_Ann_Purchase_Retriction_date = YMCARET.YmcaBusinessObject.YMCACommonBOClass.getConfigurationValue("RDB_ADB_2019Plan_Change_CutOFF_Date");//Dharmesh : 11/28/2018 : YRS-AT-3837 : Changed the configuration key for cut off date 1/1/2019


                if (IsNonEmpty(l_dsDth_Benefit_Ann_Purchase_Retriction_date))
                {

                    if (l_dsDth_Benefit_Ann_Purchase_Retriction_date.Tables[0].Rows[0]["Value"].ToString().Trim() != string.Empty)
                    {

                        DthBenRestrictedDate = Convert.ToDateTime(l_dsDth_Benefit_Ann_Purchase_Retriction_date.Tables[0].Rows[0]["Value"]);

                    }
                }
                return DthBenRestrictedDate;
            }
            catch
            {
                throw;
            }

        }
        //End- Added by Chandra sekar.c YRS-Method Return boolean value if any Participant is below or above of Retriee service Minimun age(55) as of Death Benefit Annuity Purchase Restriction date(01/01/2019)
        //2015.11.24 SP : YRS Ticket-2610 :  Restriction for purchase of Death Benefit Annuity Purchase-End
        //Start-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor
        // Delete all J'sOption Annuity for QDRO Person which has relationship as Spouse in the Retirement Processing when the person select  annuity
        public static DataTable BlockJAnnuityOptions_RetProcessing(DataTable p_DtAnnuity)
        {
            DataRow[] founRows = p_DtAnnuity.Select("Annuity IN ('J1','J1P','J1S','J1PS','J7','J7P','J7S','J7PS','J5','J5P','J5PS','J5S')");
            foreach (DataRow dr in founRows)
            {
                dr.Delete();
            }
            p_DtAnnuity.AcceptChanges();
            return p_DtAnnuity;

        }
        // Set *N/A to all J'sOption Annuity for QDRO Person which has relationship as Spouse in the Retirement Esitmate 
        public static DataTable BlockJandSAnnuityOptionsForQDRO(DataTable p_DtAnnuity)
        {
            DataRow[] founRows = p_DtAnnuity.Select("Annuity IN ('J1','J1P','J1S','J1PS','J7','J7P','J7S','J7PS','J5','J5P','J5PS','J5S')");
            foreach (DataRow dr in founRows)
            {
                dr["Retire"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                dr["Survivor"] = RetirementBOClass.JSAnnuityUnAvailableValue;
                if (!(Convert.IsDBNull(dr["Beneficiary"])))
                    dr["Beneficiary"] = RetirementBOClass.JSAnnuityUnAvailableValue;
            }

            p_DtAnnuity.AcceptChanges();
            return p_DtAnnuity;
        }
        // Set 0 to all J'sOption Annuity for QDRO Person which has relationship as Spouse in the Retirement Esitmate of Print Option
        public static DataTable BlockJandSAnnuityOptionsForPrintEstimateQDRO(DataTable p_DtAnnuity)
        {
            DataRow[] founRows = p_DtAnnuity.Select("chrAnnuityType IN ('J1','J1P','J1S','J1PS','J7','J7P','J7S','J7PS','J5','J5P','J5PS','J5S')");
            foreach (DataRow dr in founRows)
            {
                dr["mnyCurrentPayment"] = 0;
                dr["mnySurvivorRetiree"] = 0;
                dr["mnySurvivorBeneficiary"] = 0;
                dr["AnnuityWithoutRDB"] = 0;
            }

            p_DtAnnuity.AcceptChanges();
            return p_DtAnnuity;
        }

        //End-Chandra sekar.c  2016.01.19  YRS-AT-2689 - YRS enh- For participants with Fund status QDRO, block J annuities with spouse as survivor

        //START - Chandra sekar.c   2016.04.21    YRS-AT-2891 & YRS-AT-2612- YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)
        public static double GetTDServiceCatchup(DateTime ycCalcMonth, DataSet dsMaxTDContributionAllowedPerYear, DateTime retreeBirthday, double intMonthlyTDContrib, string strRetirementDate, string strEffectiveDate, string strContributionType, string strTerminateDate, DataTable dtTDWarningMessage, ref string strTDServiceCatchupLimitMgs)
        {
            int intParticipantAge;
            int intMonthsOfService;
            int intTotalMonthsOfService; // Adding Total Months of service
            int intTDCatchupAgeLimit;
            int intMonthOfServiceLimit;
            DataRow[] drTDContributionDetail;
            double dblAnnualTDContribution = 0;
            double dblContributionMaxAnnualTD;
            double dblContributionMaxAnnual50Addl;
            double dblLifeTimeTDCatchupAmount;
            double dblUsedTDCatchupAmount;
            double dblTDContribPerYearAmount;
            double dblTDCatchupLimitAllowedPerYear; // TD Catch Up Limit Allowed Per year
            double dblAvgTDLifeTimeLimit;
            double dblTDContribAmount;
            double dblLastYearTDcontributionLimits;
            bool bIsEligibleForTDCatchup;
            bool bIsNotUsedTDCatchup;
            DateTime dtEndDate;
            int intPreviousEndYear;
            string strEndYear;
            string strMonthOfServiceLimit;
            //string strTDServiceCatchupLimitMgs;
            string strAgeMgs;
            double dblContributionMaxAnnualAnd50Addl;
            double defaultLimitOfTDCatchupLimitAllowedPerYear; //PPP | 02/02/2017 | YRS-AT-3289 | Default limit availed by participant in a year against TD Catchup

            dtEndDate = string.IsNullOrEmpty(strTerminateDate) ? Convert.ToDateTime(strRetirementDate) : Convert.ToDateTime(strTerminateDate);
            // find Participant's age 
            drTDContributionDetail = dsMaxTDContributionAllowedPerYear.Tables[0].Select(String.Format("ForYear={0}", ycCalcMonth.Year)); // Getting Annual TD Contribution limts for year
            intParticipantAge = (DateAndTime.DateDiffNew(DateIntervalNew.Year, Convert.ToDateTime(retreeBirthday), ycCalcMonth)); // find Participant's age 
            if (drTDContributionDetail.Length == 0)
                // Getting Latest year of Annual TD Contribution limts if Not Annual Mamimun TD Contribution limts for corresponding  year
                drTDContributionDetail = dsMaxTDContributionAllowedPerYear.Tables[0].Select("ForYear=" + int.Parse(dsMaxTDContributionAllowedPerYear.Tables[0].Compute("MAX(ForYear)", string.Empty).ToString()) + "");

            if (drTDContributionDetail.Length > 0)
            {
                dblContributionMaxAnnualTD = Convert.ToDouble(drTDContributionDetail[0]["intContributionMaxAnnualTD"].ToString().Trim());  // Getting Annual Maximum TD  Contribution 
                dblContributionMaxAnnual50Addl = Convert.ToDouble(drTDContributionDetail[0]["intContributionMaxAnnual50Addl"].ToString().Trim()); // Getting Annual Maximum TD  Contribution for 50+ age participants
                // START | SR | 2018.12.04 | YRS-AT- 4106 | Handled Null value error.
                //dblLifeTimeTDCatchupAmount = Convert.ToDouble(drTDContributionDetail[0]["TDLifeTimeLimit"].ToString()); // Getting Lifetime limits for catch-up
                dblLifeTimeTDCatchupAmount = string.IsNullOrEmpty(drTDContributionDetail[0]["TDLifeTimeLimit"].ToString())? 0.0:Convert.ToDouble(drTDContributionDetail[0]["TDLifeTimeLimit"].ToString()); // Getting Lifetime limits for catch-up
                // END | SR | 2018.12.04 | YRS-AT- 4106 | Handled Null value error.
                intMonthsOfService = Convert.ToInt32(drTDContributionDetail[0]["MonthsOfService"].ToString());//Getting Month of Service for participant.
                // START | SR | 2018.12.04 | YRS-AT- 4106 | Handled Null value error.
                //dblTDCatchupLimitAllowedPerYear = Convert.ToDouble(drTDContributionDetail[0]["TDCatchupLimtAllowedPerYear"].ToString());// Getting TD Catchup Limit allowed for year.
                dblTDCatchupLimitAllowedPerYear = string.IsNullOrEmpty(drTDContributionDetail[0]["TDCatchupLimtAllowedPerYear"].ToString()) ? 0.0 : Convert.ToDouble(drTDContributionDetail[0]["TDCatchupLimtAllowedPerYear"].ToString());// Getting TD Catchup Limit allowed for year.
                // END | SR | 2018.12.04 | YRS-AT- 4106 | Handled Null value error.
                intTDCatchupAgeLimit = Convert.ToInt32(drTDContributionDetail[0]["TDCatchupAgeLimit"].ToString());// Getting TD Catchup Limit age to find out the Maximum contribution Limits.
                strMonthOfServiceLimit = Convert.ToString(drTDContributionDetail[0]["MonthOfserviceLimit"].ToString());// Getting Month Of service Limit for TD Catchup .
                dblTDContribPerYearAmount = Convert.ToDouble(drTDContributionDetail[0]["TDContribution"].ToString());// // Getting Total TD contribution for year.
                bIsEligibleForTDCatchup = Convert.ToInt32(drTDContributionDetail[0]["IsEligibleforTDCatchup"].ToString()) == 1 ? true : false;// Initial values is true and also Updating will be ever year whether is eligiable to Catch-up based average catchup Limit.
                bIsNotUsedTDCatchup = Convert.ToInt32(drTDContributionDetail[0]["IsNotUsedTDCatchup"].ToString()) == 1 ? true : false; // Getting whether Catch-up limits is already used.
                intMonthOfServiceLimit = strMonthOfServiceLimit != string.Empty ? Convert.ToInt32(strMonthOfServiceLimit.Remove(strMonthOfServiceLimit.Length - 1)) : 180;
                dblTDContribAmount = Convert.ToDouble(drTDContributionDetail[0]["TotalTDAmount"].ToString());// Getting Total TD contribution which is already Exists for past year.
                dblAvgTDLifeTimeLimit = Convert.ToDouble(drTDContributionDetail[0]["AvgTDLifeTimeLimit"].ToString());// Getting Average TD Lifetime Limit

                // TD Catchup limit needs to be fixed at the begining of every year
                if (ycCalcMonth.Month == 1 && ycCalcMonth.Year != DateTime.Now.Year) 
                {
                    if (bIsNotUsedTDCatchup)
                    {
                        bIsEligibleForTDCatchup = IsEligibleForCatchup(dblTDContribAmount, dblAvgTDLifeTimeLimit, intMonthsOfService);
                        drTDContributionDetail[0]["IsEligibleforTDCatchup"] = bIsEligibleForTDCatchup == true ? 1 : 0;
                    }
                }
                else if (ycCalcMonth.Month == 12) 
                {
                    if (ycCalcMonth.Year == DateTime.Today.Year)
                        intTotalMonthsOfService = intMonthsOfService + (12 - DateTime.Today.Month);
                    else
                        intTotalMonthsOfService = intMonthsOfService + 12;

                    drTDContributionDetail[0]["MonthsOfService"] = intTotalMonthsOfService; // Adding Month of service at end of the year
                }
                
    			//START: PPP | 02/02/2017 | YRS-AT-3289 | If TD catchup table do not have participant's record then following code will determining it
                if (bIsEligibleForTDCatchup && bIsNotUsedTDCatchup && dblLifeTimeTDCatchupAmount > 0 && dblTDCatchupLimitAllowedPerYear == 0 && (intMonthsOfService >= intMonthOfServiceLimit))
                {
                    if (IsEligibleForCatchup(dblTDContribAmount, dblAvgTDLifeTimeLimit, intMonthsOfService))
                    {
                        defaultLimitOfTDCatchupLimitAllowedPerYear = Convert.ToDouble(drTDContributionDetail[0]["DefaultTDCatchUpLimitPerYear"]);
                        dblTDCatchupLimitAllowedPerYear = GetTDCatchUpAmountForAYear(dblTDContribAmount, dblAvgTDLifeTimeLimit, intMonthsOfService, defaultLimitOfTDCatchupLimitAllowedPerYear, dblLifeTimeTDCatchupAmount);
                        drTDContributionDetail[0]["TDCatchupLimtAllowedPerYear"] = dblTDCatchupLimitAllowedPerYear;
                    }
                }
                //END: PPP | 02/02/2017 | YRS-AT-3289 | If TD catchup table do not have participant's record then following code will determining it

                //if ((strContributionType == MONTHLY_PAYMENTS) || (strContributionType == MONTHLY_PERCENT_SALARY))
                //{
                //    if ((ycCalcMonth.Month == (DateTime.Now.Month + 1)) && (ycCalcMonth.Year == DateTime.Now.Year)) // For Current Year finding remaining Months from current Month to add up Existing Year of service.
                //    {
                //        intMonthsOfService = intMonthsOfService + (12 - (ycCalcMonth.Month - 1));
                //        drTDContributionDetail[0]["MonthsOfService"] = intMonthsOfService;
                //    }
                //    else if (ycCalcMonth.Month == 1) // Incrementing the Year Of Service in the first month of Year
                //    {
                //        if (bIsNotUsedTDCatchup)
                //        {
                //            bIsEligibleforTDCatchup = IsEligibleForCatchup(dblTDContribAmount, dblAvgTDLifeTimeLimit, intMonthsOfService);
                //            drTDContributionDetail[0]["IsEligibleforTDCatchup"] = bIsEligibleforTDCatchup == true ? 1 : 0;
                //        }
                //        intMonthsOfService = intMonthsOfService + 12;
                //        drTDContributionDetail[0]["MonthsOfService"] = intMonthsOfService; // Update YrsOfService to dataset for Year
                //    }
                //}
                //else if (strContributionType == YEARLY_LUMP_SUM_PAYMENT)
                //{
                //    if (ycCalcMonth.Month == Convert.ToDateTime(strEffectiveDate).Month)
                //    {
                //        if (bIsNotUsedTDCatchup)
                //        {
                //            bIsEligibleforTDCatchup = IsEligibleForCatchup(dblTDContribAmount, dblAvgTDLifeTimeLimit, intMonthsOfService);
                //            drTDContributionDetail[0]["IsEligibleforTDCatchup"] = bIsEligibleforTDCatchup ? 1 : 0;
                //        }

                //        if (DateTime.Now.Year == ycCalcMonth.Year)
                //            intMonthsOfService = intMonthsOfService + (12 - (ycCalcMonth.Month - 1));
                //        else if (DateTime.Now.Year == (ycCalcMonth.Year - 1))
                //            intMonthsOfService = (intMonthsOfService + 12) + (12 - (ycCalcMonth.Month));
                //        else
                //            intMonthsOfService = intMonthsOfService + 12;

                //        drTDContributionDetail[0]["MonthsOfService"] = intMonthsOfService;
                //    }
                //}
                //else if (strContributionType == ONE_LUMP_SUM)
                //{
                //    //START - Below logic for Annual Lump Sum and also Lump Sum
                //    if (!string.IsNullOrEmpty(strEffectiveDate) && ycCalcMonth.Month == Convert.ToDateTime(strEffectiveDate).Month)
                //    {
                //        if (bIsNotUsedTDCatchup)
                //        {
                //            bIsEligibleforTDCatchup = IsEligibleForCatchup(dblTDContribAmount, dblAvgTDLifeTimeLimit, intMonthsOfService);
                //            drTDContributionDetail[0]["IsEligibleforTDCatchup"] = bIsEligibleforTDCatchup == true ? 1 : 0;
                //        }
                //        // Incrementing the Year Of Service in the first month of Year
                //        intMonthsOfService = intMonthsOfService + (12 - (ycCalcMonth.Month));
                //        drTDContributionDetail[0]["MonthsOfService"] = intMonthsOfService;
                //    }
                //    //END - Below logic for Annual Lump Sum and also Lump Sum
                //}

                // Adding Annual Max TD  Contribution and Annual Max TD  Contribution for 50+ age participants,
                // if participants is 50 and also over 50 ages then only Annual Maximum TD Contribution of 50+ age will added other wise Annual Maximum TD Contribution of 50+ age will not added.
                dblContributionMaxAnnual50Addl = intParticipantAge >= intTDCatchupAgeLimit ? dblContributionMaxAnnual50Addl : 0;
                if ((intMonthsOfService < intMonthOfServiceLimit) || (dblLifeTimeTDCatchupAmount <= 0) || (!bIsEligibleForTDCatchup)) // Restrict the Catchup Amount based on Age ,Lifetimelimit and Avgcatchup amount
                    dblTDCatchupLimitAllowedPerYear = 0;
                dblAnnualTDContribution = dblContributionMaxAnnualTD + dblContributionMaxAnnual50Addl + dblTDCatchupLimitAllowedPerYear;

                dblTDContribPerYearAmount = intMonthlyTDContrib + dblTDContribPerYearAmount; //Adding Monthly Contribtion into Total
                drTDContributionDetail[0]["TDContribution"] = dblTDContribPerYearAmount;

                if ((ycCalcMonth.Month == 12) || ((ycCalcMonth.Month == dtEndDate.Month - 1) && (ycCalcMonth.Year == dtEndDate.Year)))
                {
                    // At the end of the year or end of the estimator period setting TD Contribution of the current year period to 0
                    //  as well as checking it against the max TD per year limit 
                    drTDContributionDetail[0]["TDContribution"] = 0;
                    dblTDContribAmount = dblTDContribAmount + (dblTDContribPerYearAmount >= dblAnnualTDContribution ? dblAnnualTDContribution : dblTDContribPerYearAmount); // Increasing the total TD contribution at the end of the year or end of the projection period
                    drTDContributionDetail[0]["TotalTDAmount"] = dblTDContribAmount;

                    if (dblTDContribPerYearAmount >= dblAnnualTDContribution)
                    {
                        strAgeMgs = intParticipantAge >= intTDCatchupAgeLimit ? "age 50 and above " : "age under 50 ";
                        dblContributionMaxAnnualAnd50Addl = dblContributionMaxAnnualTD + dblContributionMaxAnnual50Addl;
                        if (dblTDCatchupLimitAllowedPerYear != 0)
                        {
                            strTDServiceCatchupLimitMgs = String.Format("(TD max limit {0} : ${1} plus available TD catchup : ${2} )", strAgeMgs, dblContributionMaxAnnualAnd50Addl, dblTDCatchupLimitAllowedPerYear);
                        }
                        else
                        {
                            strTDServiceCatchupLimitMgs = String.Format("(TD max limit {0} : ${1} )", strAgeMgs, dblContributionMaxAnnualAnd50Addl);
                        }
                        // Limit has been crossed in this year So record it in warning table (dtTDWarningMessage)
                        if (dtTDWarningMessage.Rows.Count > 0)
                        {
                            dblLastYearTDcontributionLimits = Convert.ToDouble(dtTDWarningMessage.Rows[dtTDWarningMessage.Rows.Count - 1]["TDContributionLimits"].ToString());
                            //Check for the AnnuaTDContributionLimit for Current year and Previous year
                            if (dblAnnualTDContribution != dblLastYearTDcontributionLimits)
                            {
                                // New limit has been crossed for e.g. previous row is for 24000 and current years limit is 27000
                                // So create a new row for it
                                DataRow drMessage = dtTDWarningMessage.NewRow();
                                drMessage["TDContributionLimits"] = dblAnnualTDContribution.ToString();
                                drMessage["TDServiceCatchupLimitMgs"] = strTDServiceCatchupLimitMgs.ToString();
                                drMessage["StartYear"] = ycCalcMonth.Year.ToString();
                                dtTDWarningMessage.Rows.Add(drMessage);
                                dtTDWarningMessage.AcceptChanges();
                            }
                            else
                            {
                                // Last row in the warnings table has entry for the same Max TD limit, So check for the end year
                                // If end year is not there then update it with current year
                                // Else check for the continuation, for e.g. end year is 2015 and current year is 2016 then 
                                // update the same row else create a new row.
                                strEndYear = dtTDWarningMessage.Rows[dtTDWarningMessage.Rows.Count - 1]["EndYear"].ToString();
                                if (string.IsNullOrEmpty(strEndYear))
                                {
                                    dtTDWarningMessage.Rows[dtTDWarningMessage.Rows.Count - 1]["EndYear"] = ycCalcMonth.Year.ToString();
                                }
                                else
                                {
                                    intPreviousEndYear = Convert.ToInt32(dtTDWarningMessage.Rows[dtTDWarningMessage.Rows.Count - 1]["EndYear"].ToString());
                                    if (ycCalcMonth.Year != (intPreviousEndYear + 1))
                                    {
                                        // It is not a continuation, So create a new row
                                        // Initial Insert of Validation Messages with Start Period
                                        DataRow drMessage = dtTDWarningMessage.NewRow();
                                        drMessage["TDContributionLimits"] = dblAnnualTDContribution.ToString();
                                        drMessage["TDServiceCatchupLimitMgs"] = strTDServiceCatchupLimitMgs.ToString();
                                        drMessage["StartYear"] = ycCalcMonth.Year.ToString();
                                        dtTDWarningMessage.Rows.Add(drMessage);
                                        dtTDWarningMessage.AcceptChanges();
                                    }
                                    else
                                    {
                                        // It is a continuation, So update the end year with current year
                                        dtTDWarningMessage.Rows[dtTDWarningMessage.Rows.Count - 1]["EndYear"] = ycCalcMonth.Year.ToString();
                                    }
                                }
                                dtTDWarningMessage.AcceptChanges();
                            }
                        }
                        else
                        {
                            DataRow drMessage = dtTDWarningMessage.NewRow();
                            drMessage["TDContributionLimits"] = dblAnnualTDContribution.ToString();
                            drMessage["TDServiceCatchupLimitMgs"] = strTDServiceCatchupLimitMgs.ToString();
                            drMessage["StartYear"] = ycCalcMonth.Year.ToString();
                            dtTDWarningMessage.Rows.Add(drMessage);
                            dtTDWarningMessage.AcceptChanges();
                        }

                        if (dblTDCatchupLimitAllowedPerYear > 0 && ycCalcMonth.Year.ToString() != Convert.ToString(drTDContributionDetail[0]["YearOfUsedTDCatchup"].ToString()))
                        {
                            dblLifeTimeTDCatchupAmount = (dblLifeTimeTDCatchupAmount - dblTDCatchupLimitAllowedPerYear) <= 0 ? 0 : (dblLifeTimeTDCatchupAmount - dblTDCatchupLimitAllowedPerYear);
                            drTDContributionDetail[0]["TDLifeTimeLimit"] = dblLifeTimeTDCatchupAmount;
                            drTDContributionDetail[0]["YearOfUsedTDCatchup"] = ycCalcMonth.Year;
                        }
                    }
                    else if (dblTDContribPerYearAmount < dblAnnualTDContribution)
                    {
                        dblContributionMaxAnnualTD = intParticipantAge >= intTDCatchupAgeLimit ? dblContributionMaxAnnual50Addl + dblContributionMaxAnnualTD : dblContributionMaxAnnualTD;
                        if (dblTDContribPerYearAmount >= dblContributionMaxAnnualTD)
                        {
                            dblUsedTDCatchupAmount = dblTDContribPerYearAmount - dblContributionMaxAnnualTD;
                            if (dblTDCatchupLimitAllowedPerYear != 0 && dblUsedTDCatchupAmount > 0)
                            {
                                dblLifeTimeTDCatchupAmount = dblLifeTimeTDCatchupAmount - dblUsedTDCatchupAmount <= 0 ? 0 : dblLifeTimeTDCatchupAmount - dblUsedTDCatchupAmount;
                                drTDContributionDetail[0]["TDLifeTimeLimit"] = dblLifeTimeTDCatchupAmount;
                                drTDContributionDetail[0]["YearOfUsedTDCatchup"] = ycCalcMonth.Year;
                            }
                        }
                    }

                    // If all the TD Catchup amount is utilized then set both flags as "False"
                    if (bIsNotUsedTDCatchup && dblLifeTimeTDCatchupAmount <= 0)
                    {
                        drTDContributionDetail[0]["IsEligibleforTDCatchup"] = 0;
                        drTDContributionDetail[0]["IsNotUsedTDCatchup"] = 0;
                    }
                }
            }
            dsMaxTDContributionAllowedPerYear.AcceptChanges();
            return dblAnnualTDContribution;
        }

        public static bool IsEligibleForCatchup(double lnTotalTDAmount, double lnAvgTDLifeTimeLimit, int intMonthsOfService)
        {
            double dblAvgTDAmount;
            intMonthsOfService = intMonthsOfService == 0 ? 1 : intMonthsOfService;
            dblAvgTDAmount = (lnTotalTDAmount * 12) / intMonthsOfService;
            return dblAvgTDAmount < lnAvgTDLifeTimeLimit ? true : false;
        }
        //END - Chandra sekar.c   2016.04.21    YRS-AT-2891 YRS-AT-2612 - YRS bug: YRS and website enhancement-in annuity calculator distinguish under age 50 and 50+ TD limits (TrackIT 25543)

        //START: PPP | 02/02/2017 | YRS-AT-3289 | Determines participant's TD CatchUp amount
        public static double GetTDCatchUpAmountForAYear(double totalTDAmount, double defaultAverageTDPerYear, int monthOfService, double defaultPerYearLimit, double totalRemainingTDCatchup)
        {
            double averageTDPerYear, currentYearTDCatchUpAmount, excess;
            //int yearsOfService; //PPP | 07/03/2017 | YRS-AT-3289 | Not storing service value in this variable
            currentYearTDCatchUpAmount = 0;

            monthOfService = monthOfService == 0 ? 1 : monthOfService;
            averageTDPerYear = (totalTDAmount * 12) / monthOfService;
            if (averageTDPerYear < defaultAverageTDPerYear)
            {
                // Following logic of "excess" calculation and defining TD Catchup per year is taken from YERDI_P_FIN_TDCatchupList
                //START: //PPP | 07/03/2017 | YRS-AT-3289 | Instead of saving the result in yearsOfService which is getting converted in int, directly using the output of 'monthOfService / 12' in excess calculation
                //-- Also rounding the result upto 2 decimal points in excess
                //yearsOfService = monthOfService / 12;
                //excess = (defaultAverageTDPerYear - averageTDPerYear) * Convert.ToDouble(yearsOfService);
                excess = Math.Round((defaultAverageTDPerYear - averageTDPerYear) * Convert.ToDouble(monthOfService / 12), 2);
                //END: //PPP | 07/03/2017 | YRS-AT-3289 | Instead of saving the result in yearsOfService which is getting converted in int, directly using the output of 'monthOfService / 12' in excess calculation
                if (excess > defaultPerYearLimit)
                    currentYearTDCatchUpAmount = defaultPerYearLimit;
                else
                    currentYearTDCatchUpAmount = excess;
            }
            if (totalRemainingTDCatchup < currentYearTDCatchUpAmount)
                currentYearTDCatchUpAmount = totalRemainingTDCatchup;
            return currentYearTDCatchUpAmount;
        }
        //END: PPP | 02/02/2017 | YRS-AT-3289 | Determines participant's TD CatchUp amount

        // START | MMR | 2017.02.22 | YRS-AT-2625 | Get Manual Transaction details for 'MAPR' Transact type
        public static DataSet GetManualTransactions(string fundEventId, string retireeType, DateTime retirementDate)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.RetirementDAClass.GetManualTransactions(fundEventId, retireeType, retirementDate));
            }
            catch
            {
                throw;
            }
        }
        // END | MMR | 2017.02.22 | YRS-AT-2625 | Get Manual Transaction details for 'MAPR' Transact type
 
        //START: PPP | 03/09/2017 | YRS-AT-2625 
        /// <summary>
        ///Reduction factor will be used in monthly payroll to deduct remaining reserves. 
        ///So every month reserves will be deducted like: Remaining Reserves = Remaining Reserves - (Current Payment * Reduction Factor)
        ///Except C annuity calculated for Disability retirement, all other annuity reduction factor will be 100% (return value will be 1)
        ///In case of Disability C annuity, it will be calculated = SUM(Actual Balance) / SUM(Total Balance)
        /// </summary>
        /// <param name="annuityType">Annuity Type</param>
        /// <returns></returns>
        public decimal GetAnnuityReductionFactor(string annuityType)
        {
            decimal factor = 1; //Default factor is 100% for all annuities except "C" annuity calculated for Diability which needs to be computed based on actual balance and total balance
            decimal actualBalance, totalBalance;
            if (annuityType.ToUpper() == "C")
            {
                if (this.actualRetirementBalance != null && this.totalRetirementBalance != null)
                {
                    actualBalance = Convert.ToDecimal(this.actualRetirementBalance.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYmcaPreTax)", ""));
                    totalBalance = Convert.ToDecimal(this.totalRetirementBalance.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYmcaPreTax)", ""));

                    factor = Math.Round(actualBalance / totalBalance, 10);
                }
                //else if (this.actualBalanceAtRetirement != null && this.totalBalanceAtRetirement != null)
                //{
                //    // 1. In estimation, code will return same reduction factor for both plan Annuities (Retirement as well as Savings)
                //    // 2. In Purchase, If annuities are being calculated for Savings plan then this area will be reached. Code will return 1 as both variable actualBalance & totalBalance will contain 0 value
                //    actualBalance = Convert.ToDecimal(this.actualBalanceAtRetirement.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYmcaPreTax)", "chvPlanType='RETIREMENT'"));
                //    totalBalance = Convert.ToDecimal(this.totalBalanceAtRetirement.Compute("SUM(mnyPersonalPreTax) + SUM(mnyPersonalPostTax) + SUM(mnyYmcaPreTax)", "chvPlanType='RETIREMENT'"));

                //    if (actualBalance > 0 && totalBalance > 0)
                //        factor = Math.Round(actualBalance / totalBalance, 10);
                //}
            }
            return factor;
        }

        private DataTable AdjustDisabilityAnnuityForRDB(string annuityBasisType, DataTable factorTable)
        {
            // In case of annuity calculation of Retired Death Benefit 
            // Following factors will be used:
            // •	Main annuity: Option M, MS, C or CS
            // •	RDB annuity: Option M DISABILITY PST96 factor for actual age at retirement
            // 
            // •	Main Annuity: Option J1, J1P, J1S, J1PS
            // •	RDB annuity: Option J1 DISABILITY PST96 factor for actual participant age and actual beneficiary age at retirement
            // 
            // •	Main Annuity: Option J7, J7P, J7S, J7PS
            // •	RDB annuity: Option J7 DISABILITY PST96 factor for actual participant age and actual beneficiary age at retirement
            //
            // •	Main Annuity: Option J5, J5P, J5S, J5PS
            // •	RDB annuity: Option J5 DISABILITY PST96 factor for actual participant age and actual beneficiary age at retirement
            //
            // So copying factors of RDB annuity to main annuity
            // for example Replacing MS, C and CS annuity factors with M annuity factor 
            // Please note that factorTable as of now do not contain annuities ending with 'S', for e.g. MS, CS, J1S, J1PS etc.

            decimal factorM, factorJ1, factorJ5, factorJ7;

            factorM = GetFactor(annuityBasisType, "M", factorTable);
            factorJ1 = GetFactor(annuityBasisType, "J1", factorTable);
            factorJ5 = GetFactor(annuityBasisType, "J5", factorTable);
            factorJ7 = GetFactor(annuityBasisType, "J7", factorTable);

            factorTable = ChangeFactor(annuityBasisType, "M", factorM, factorTable);
            factorTable = ChangeFactor(annuityBasisType, "C", factorM, factorTable);
            factorTable = ChangeFactor(annuityBasisType, "J1", factorJ1, factorTable);
            factorTable = ChangeFactor(annuityBasisType, "J5", factorJ5, factorTable);
            factorTable = ChangeFactor(annuityBasisType, "J7", factorJ7, factorTable);

            return factorTable;
        }

        private decimal GetFactor(string annuityBasisType, string annuityType, DataTable factorTable)
        {
            DataRow[] annuityBasisTypeRows;
            decimal factor;
            try
            {
                factor = 0;
                annuityBasisTypeRows = factorTable.Select(string.Format("BasisTypeCode='{0}' AND AnnuityType='{1}'", annuityBasisType, annuityType));
                if (annuityBasisTypeRows != null && annuityBasisTypeRows.Length > 0)
                {
                    factor = Convert.ToDecimal(Convert.IsDBNull(annuityBasisTypeRows[0]["Factor"]) ? 0 : annuityBasisTypeRows[0]["Factor"]);
                }
                return factor;
            }
            catch
            {
                throw;
            }
            finally
            {
                annuityBasisTypeRows = null;
            }
        }

        private DataTable ChangeFactor(string annuityBasisType, string annuityType, decimal factor, DataTable factorTable)
        {
            DataRow[] annuityBasisTypeRows;
            try
            {
                annuityBasisTypeRows = factorTable.Select(string.Format("BasisTypeCode='{0}' AND AnnuityType LIKE '{1}*'", annuityBasisType, annuityType));
                if (annuityBasisTypeRows != null && annuityBasisTypeRows.Length > 0)
                {
                    for (int i = 0; i < annuityBasisTypeRows.Length; i++)
                    {
                        annuityBasisTypeRows[i]["Factor"] = factor;
                    }
                }
                return factorTable;
            }
            catch
            {
                throw;
            }
            finally
            {
                annuityBasisTypeRows = null;
            }
        }

        private DataTable GetOnlyRetirementPlanDetails(DataTable source)
        {
            DataTable retirementPlanDetails = null;
            DataRow[] rows = source.Select("chvPlanType='RETIREMENT'");
            if (rows != null && rows.Length > 0)
            {
                retirementPlanDetails = source.Clone();
                foreach (DataRow row in rows)
                {
                    retirementPlanDetails.ImportRow(row);
                }
                retirementPlanDetails.AcceptChanges();
            }
            return retirementPlanDetails;
        }
        //END: PPP | 03/09/2017 | YRS-AT-2625 

        //START: PPP | 12/28/2017 | YRS-AT-3328 | Fetching YMCA Legacy balance as on termination date
        public static double GetYmcaLegacyAtTerminationBalance(string fundEventID)
        {
            return RetirementDAClass.GetYmcaLegacyAtTerminationBalance(fundEventID);
        }
        //END: PPP | 12/28/2017 | YRS-AT-3328 | Fetching YMCA Legacy balance as on termination date

        //START: Benhan David | 11/01/2018 | YRS-AT-4133   RDB_ADB_2019Plan_Change_CutOFF_Date 
        /// <summary>
        /// Method to check participant enrolled before 1/1/2019
        /// </summary>
        /// <param name="fundEventID"></param>
        /// <returns>bool: If enrolled before 1/1/2019 returns 1 else 0</returns>
        public static bool IsFirstEnrolledBeforeCutOffDate_2019PlanChange(string fundEventID)
        {
            return RetirementDAClass.IsFirstEnrolledBeforeCutOffDate_2019PlanChange(fundEventID);
        }

        /// <summary>
        /// Get RDB cut off date
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetRDBPlanChangeCutOffDateReplacementDict()
        {
            Dictionary<string, string> DictReplacement = new Dictionary<string, string>();
            DictReplacement.Add("CutOffDate", ConfigurationBOClass.RDB_ADB_2019PlanChangeCutOffDate.ToString("MMMM dd, yyyy"));
            return DictReplacement;
        }
        //END: Benhan David | 11/01/2018 | YRS-AT-4133  RDB_ADB_2019Plan_Change_CutOFF_Date         
		
        // START | 2018.11.21 | YRS-AT-4106 | Created common method to set running total salary in future salary and future salary percentage increase.
        // maintain running total of compensation so that new salary and salary increases that happen during the middle of the year are validated appropriately.
        public static void SetFutureSalaryAndSalaryIncrease(DataTable dtRetEstimateEmployment, DataTable dtEmploymentProjectedSalary, DateTime ycCalcMonth, double annualCompensation, double lnMaximumContributionSalary, ref string warningMessage)
        {
            // Name of the input parameters are used from existing code, So that it can easily be traced in entire code.

            DataRow projectedSalaryRow;
            string employmentID;
            double currentProjectedSalary;
            string annualSalaryIncreaseEffDate;
            string annualPctgIncrease;
            string salaryWarningMessage = string.Empty;
                       
            foreach (DataRow dr in dtRetEstimateEmployment.Rows)
            {
                employmentID = dr["guiEmpEventId"].ToString();
                projectedSalaryRow = dtEmploymentProjectedSalary.Select("guiEmpEventID='" + employmentID + "'")[0];
                
                // See if a salary increase is warranted this month
                if ((dr["dtmTerminationDate"] == DBNull.Value) || dr["dtmTerminationDate"].ToString() == string.Empty ||

                   (ycCalcMonth < Convert.ToDateTime(dr["dtmTerminationDate"])) ||
                   (ycCalcMonth.Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year && ycCalcMonth.Month == Convert.ToDateTime(dr["dtmTerminationDate"]).Month
                   && Convert.ToDateTime(dr["dtmTerminationDate"]).Day > 1))
                {
                    // SR | 2018.11.21 | YRS-AT-4106 | If input date month is prior/same month as the current month then do not set salary. 
                    // Prior or current month salary will be considered through submitted or unsubmiited compensation logic before this method calling.
                    if ((ycCalcMonth.Month <= DateTime.Now.Month && ycCalcMonth.Year == DateTime.Now.Year))
                    {
                        continue;
                    }

                    // Future Salary by amount
                    if (Convert.ToDecimal(dr["numFutureSalary"]) != 0 && dr["dtmFutureSalaryDate"].ToString() != string.Empty)
                    {
                        if (ycCalcMonth.Month >= Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Month &&
                            ycCalcMonth.Year >= Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Year
                            )
                        {

                            if (ycCalcMonth.Month == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Month &&
                                ycCalcMonth.Year == Convert.ToDateTime(dr["dtmFutureSalaryDate"]).Year)
                            {
                                projectedSalaryRow["CurrentProjectedSalary"] = Convert.ToDecimal(dr["numFutureSalary"]); // Get future salary for remining months in year from efective year.
                                currentProjectedSalary = Convert.ToDouble(projectedSalaryRow["CurrentProjectedSalary"]);
                            }
                        }
                    }

                    // Do the annual increase every 12 months keeping calculation(today) month as the start month
                    //Determine if % Salary increase is warranted
                    annualSalaryIncreaseEffDate = dr["dtmAnnualSalaryIncreaseEffDate"].ToString();
                    annualPctgIncrease = dr["numAnnualPctgIncrease"].ToString();

                    if (annualSalaryIncreaseEffDate != string.Empty && annualPctgIncrease != string.Empty)//Phase IV Changes
                    {
                        if (ycCalcMonth.Month == DateTime.Parse(annualSalaryIncreaseEffDate).Month
                            && Convert.ToDouble(annualPctgIncrease) != 0)
                        {
                            if ((ycCalcMonth < Convert.ToDateTime(dr["dtmTerminationDate"])) ||
                                    (ycCalcMonth.Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year &&
                                        ycCalcMonth.Month == Convert.ToDateTime(dr["dtmTerminationDate"]).Month &&
                                            Convert.ToDateTime(dr["dtmTerminationDate"]).Day > 1))
                            {
                                if (
                                    !(DateTime.Parse(annualSalaryIncreaseEffDate).Month == Convert.ToDateTime(dr["dtmTerminationDate"]).Month &&
                                        DateTime.Parse(annualSalaryIncreaseEffDate).Year == Convert.ToDateTime(dr["dtmTerminationDate"]).Year)
                                    )
                                {
                                    // Apply number of months annual percentage will be effective.
                                    currentProjectedSalary = Convert.ToDouble(projectedSalaryRow["CurrentProjectedSalary"]) * (1 + (Convert.ToDouble(dr["numAnnualPctgIncrease"]) / 100));
                                    projectedSalaryRow["CurrentProjectedSalary"] = currentProjectedSalary;
                                }
                            }
                        }
                    }
                    currentProjectedSalary = Convert.ToDouble(projectedSalaryRow["CurrentProjectedSalary"]);
                }
                else
                {
                    currentProjectedSalary = 0;
                }

                currentProjectedSalary = ValidateMaxAnnualSalaryLimit(ycCalcMonth, lnMaximumContributionSalary, annualCompensation, currentProjectedSalary, out salaryWarningMessage);
                warningMessage = AppendWarningMessage(salaryWarningMessage, warningMessage);
                projectedSalaryRow["UseSalary"] = currentProjectedSalary;

                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("FutureSalaryAndSalaryIncrease", String.Format("Employment : {0} Used Salary : {1}", employmentID, currentProjectedSalary));                
                //dtEmploymentProjectedSalary.GetChanges(DataRowState.Modified);
                //dtEmploymentProjectedSalary.AcceptChanges();
                
            }
                    
        }
		// END | 2018.11.21 | YRS-AT-4106 | Created common method to set running total salary.
        // START | 2018.11.21 | YRS-AT-4106 | Created common method to append new message in currentMessage(s).
        public static string AppendWarningMessage(string newMessage, string currentMessage)
        {
            if (string.IsNullOrEmpty(currentMessage)) return newMessage;
            if (string.IsNullOrEmpty(newMessage)) return currentMessage;
            if (currentMessage.IndexOf(newMessage) > -1) return currentMessage;
            return string.Format("{0}<br />{1}", currentMessage, newMessage);
        }
        // END | 2018.11.21 | YRS-AT-4106 | Created common method to append new message in currentMessage(s).

        public static Boolean HasInsuredReserveAnnuity(DataSet dsAnnuityDetails)
        {
            if (HelperFunctions.isNonEmpty(dsAnnuityDetails) && dsAnnuityDetails.Tables[0].Select("bitInsuredReserve=1").Length > 0)
                return true;

            return false;
        }
        /// <summary>
        /// Checks weather eff date of fund event is null or not
        /// TODO: Can be moved to common BO in future
        /// </summary>
        /// <param name="FundEventId"></param>
        /// <returns></returns>
        public static bool IsEffectiveDateNull(string FundEventId)
        {
            DataSet dsFundEvents = RetirementDAClass.GetFundEventDetails(FundEventId);
            bool bIsEffectiveDateNull = true;
            if (HelperFunctions.isNonEmpty(dsFundEvents))
            {
                DataRow drFundEvent = dsFundEvents.Tables[0].Rows[0];
                if (!string.IsNullOrEmpty(drFundEvent["dtmEffDate"].ToString()))
                {
                    bIsEffectiveDateNull = false;
                }
            }
            return bIsEffectiveDateNull;
        }
        
        //START : ML |2019.10.24 | YRS-AT-4598 | Return Additional Withholding Amount based on Input 
        public static double GetTotalWithHoldingAmount(DataSet dsFedWithDrawals, DataSet dsGenWithDrawals, int l_integer_NoOfMonths, double annuityAmt,DateTime annuityPurchaseDate)
        {           
            double l_double_Gen_Withholding = 0;
            double l_double_Fed_WithHolding = 0;
            double l_double_Tot_Withholding = 0;           
           
            try
            {
                if (HelperFunctions.isNonEmpty(dsFedWithDrawals))
                    l_double_Fed_WithHolding = GetFedWithDrawalAmount(dsFedWithDrawals, l_integer_NoOfMonths, annuityAmt);

                if (HelperFunctions.isNonEmpty(dsGenWithDrawals))
                    l_double_Gen_Withholding = GetGenWithDrawalAmount(dsGenWithDrawals, annuityPurchaseDate); 

                l_double_Tot_Withholding = Math.Round(l_double_Fed_WithHolding + l_double_Gen_Withholding, 2);

                return l_double_Tot_Withholding;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Returns Federal Withholding Amount
        public static double GetFedWithDrawalAmount(DataSet dsFedWithDrawals, int l_integer_NoOfMonths, double annuityAmt)
        {
          
            DataRow[] drRows;

            DataSet dsTaxEntityTypes;
            DataSet dsTaxFactors;

            double l_double_ExemptionsDefault;
            string l_string_MarriageStatusDefault;
            double l_double_ValidExemptionsMaximum = 0;

            string l_string_row_MaritalStatusCode;
            string l_string_row_WithholdingType;
            double l_double_row_Exemptions;
            string l_string_row_TaxEntityCode;
            double l_double_row_AdditionalAmount;

            int l_integer_MetaTaxFactorCount = 0;
            double l_double_CreditAdjustment = 0;
            double l_double_TaxPercent = 0;
            double l_double_Withholding = 0;
            double l_double_Fed_WithHolding = 0;
           

            double l_double_TaxCalculationBase;
            double l_double_TotalTaxable;
            double l_double_Allowance = 0;
            try
            {               
                if (HelperFunctions.isNonEmpty(dsFedWithDrawals))
                {                   
                        
                            // --------------Federal withholdings------------------
                    dsTaxEntityTypes = RetireesInformationBOClass.TaxEntityTypes();
                    dsTaxFactors = TaxFactors();
                    try
                    {
                        l_double_ExemptionsDefault = Convert.ToDouble(MetaConfigMaintenance.GetConfigurationKeyValue("WITHHOLDING_DEFAULT_EXEMPTIONS"));
                    }
                    catch
                    {
                        l_double_ExemptionsDefault = 0;
                    }
                            
                    try
                    {
                        l_string_MarriageStatusDefault = MetaConfigMaintenance.GetConfigurationKeyValue("WITHHOLDING_DEFAULT_MARRIAGE_STATUS");
                    }
                    catch
                    {
                        l_string_MarriageStatusDefault = "M";
                    }
                    // ----------Formula (tax table) withholdings----------------------------
                     // ----------Get number of maximum valid exemptions----------------------
                    try
                    {
                        l_double_ValidExemptionsMaximum = Convert.ToDouble(MetaConfigMaintenance.GetConfigurationKeyValue("WITHHOLDING_MAXIMUM_VALID_EXEMPTIONS"));
                    }
                    catch (Exception ex)
                    {
                        l_double_ValidExemptionsMaximum = 50;
                    }


                    if (l_double_ValidExemptionsMaximum == 0)
                        l_double_ValidExemptionsMaximum = 50;

                    if (l_integer_NoOfMonths < 1)
                        l_integer_NoOfMonths = 1;

                    l_double_TotalTaxable = annuityAmt; 

                    foreach (DataRow dr in dsFedWithDrawals.Tables[0].Rows)
                    {
                        l_string_row_WithholdingType = dr["Type"].ToString().Trim();
                                                                                                
                        if (dr["Marital Status"].ToString() == "System.DBNull")
                            l_string_row_MaritalStatusCode = "";
                        else
                            l_string_row_MaritalStatusCode = dr["Marital Status"].ToString().Trim();
                                                              

                        l_double_row_Exemptions = Convert.ToDouble(dr["Exemptions"]);
                        l_string_row_TaxEntityCode = dr["Tax Entity"].ToString().Trim();                               

                        if (dr["Add'l Amount"].ToString() == "System.DBNull")
                            l_double_row_AdditionalAmount = 0;
                        else
                            l_double_row_AdditionalAmount = Convert.ToDouble(dr["Add'l Amount"]);


                        // ------------------Determine which allowance to use NYState or Fed---------------------------
                        drRows = dsTaxEntityTypes.Tables[0].Select("TaxEntitytype='" + l_string_row_TaxEntityCode + "'");
                        if (drRows.Length > 0)
                            l_double_Allowance = Convert.ToDouble(drRows[0]["ExemptionAllowance"]);

                        switch (l_string_row_WithholdingType)
                        {
                            case "DEFALT":
                                {
                                    l_double_row_Exemptions = l_double_ExemptionsDefault;
                                    l_string_row_MaritalStatusCode = l_string_MarriageStatusDefault;
                                    l_double_row_AdditionalAmount = 0;
                                    break;
                                }

                            case "FLAT":
                                {
                                    l_double_row_Exemptions = 0;
                                    if (dr["Add'l Amount"].ToString() == "System.DBNull")
                                        l_double_row_AdditionalAmount = 0;
                                    else
                                        l_double_row_AdditionalAmount = Convert.ToDouble(dr["Add'l Amount"]);
                                    break;
                                }

                            case "FORMUL":
                                {
                                    if (dr["Add'l Amount"].ToString() == "System.DBNull")
                                        l_double_row_AdditionalAmount = 0;
                                    else
                                        l_double_row_AdditionalAmount = Convert.ToDouble(dr["Add'l Amount"]);
                                    l_double_row_Exemptions = Convert.ToDouble(dr["Exemptions"]);
                                    break;
                                }

                            default:
                                {
                                    throw (new Exception("Invalid Withholding type encountered while getting the withheld amount."));
                                    break;
                                }
                        }

                        // --------------------See if these settings qualify for formula based withholdings-------------------
                        if (l_string_row_WithholdingType != "FLAT" & l_double_row_Exemptions <= l_double_ValidExemptionsMaximum)
                        {
                            l_double_TaxCalculationBase = l_double_TotalTaxable - (l_double_row_Exemptions * l_double_Allowance);

                            // ---------Set lnTaxCalculationBase to zero if lesser than zero--------------
                            if (l_double_TaxCalculationBase <= 0)
                                l_double_TaxCalculationBase = 0;

                            // ---------Check for Invalid marriage status-----------------------
                            if (!(l_string_row_MaritalStatusCode == "S" | l_string_row_MaritalStatusCode == "M"))
                                l_string_row_MaritalStatusCode = "S";

                            drRows = dsTaxFactors.Tables[0].Select("TaxEntityCode='" + l_string_row_TaxEntityCode + "'" + " AND MaritalStatusCode = '" + l_string_row_MaritalStatusCode + "'" + " AND TaxableLow <" + l_double_TaxCalculationBase + " AND TaxableHigh >=" + l_double_TaxCalculationBase);
                            l_integer_MetaTaxFactorCount = drRows.Length;
                            if (l_integer_MetaTaxFactorCount > 0)
                            {
                                l_double_TaxPercent = Convert.ToDouble(drRows[0]["TaxPercent"]);
                                l_double_CreditAdjustment = Convert.ToDouble(drRows[0]["CreditAdjustment"]);

                                l_double_Withholding = ((l_double_TaxCalculationBase * l_double_TaxPercent / 100) + l_double_CreditAdjustment);
                            }

                            // ---------------Flat Tax Calculation----------------------
                            l_double_row_AdditionalAmount = l_double_Withholding + l_double_row_AdditionalAmount;
                        }

                        l_double_Fed_WithHolding = l_double_Fed_WithHolding + l_double_row_AdditionalAmount;
                           
                      
                    }
                }
                return l_double_Fed_WithHolding;  
            }
            catch
            {
                throw;
            }
            finally
            {
            
            }        
        }
        //Returns General Withholding Amount
        public static double GetGenWithDrawalAmount(DataSet dsGenWithDrawals, DateTime annuityPurchaseDate)
        { 
            string l_string_row_StartDate;
            string l_string_row_EndDate;
            double l_double_row_Amount;
            double l_double_Gen_Withholding = 0;
            try
            {
                if (HelperFunctions.isNonEmpty(dsGenWithDrawals))
                {
                    
                        // --------------General withholdings------------------
                        foreach (DataRow drGenRow in dsGenWithDrawals.Tables[0].Rows)
                        {
                            l_string_row_StartDate = drGenRow["Start Date"].ToString();
                            if (drGenRow["End Date"].ToString() != null)
                                l_string_row_EndDate = drGenRow["End Date"].ToString();
                            else
                                l_string_row_EndDate = string.Empty;

                            l_double_row_Amount = Convert.ToDouble(drGenRow["Add'l Amount"]);

                            if (Convert.ToDateTime(l_string_row_StartDate) <= annuityPurchaseDate)
                            {
                                // If l_string_row_EndDate.GetType.ToString() = "System.DBNull" Then - '2012.07.16	SP: BT-753/YRS 5.0-1270 : purchase page -Commented 
                                if (string.IsNullOrEmpty(l_string_row_EndDate))
                                {
                                    if (l_double_row_Amount > 0)
                                        l_double_Gen_Withholding = l_double_Gen_Withholding + l_double_row_Amount;
                                }
                                else if (!(string.IsNullOrEmpty(l_string_row_EndDate)))
                                {
                                    if (Convert.ToDateTime(l_string_row_EndDate) > annuityPurchaseDate)
                                    {
                                        if (l_double_row_Amount > 0)
                                            l_double_Gen_Withholding = l_double_Gen_Withholding + l_double_row_Amount;
                                    }
                                }
                            }
                        }
                  
                }
                return l_double_Gen_Withholding;
            }
            catch
            {
                throw;
            }
            finally {
            }
        }
        //END : ML |2019.10.24 | YRS-AT-4598 | Returns Additional Withholding Amount based on Input 

        //START : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | Non-spouse who is more than 10 years younger than the Participant and does not display J options.
        // For Annuity Estimation cutOffDate is RetirementDate and For Death Notification cutoffDate is DeathDate
        public static Boolean IsSecureActApplicable(DateTime perssBirthDate, DateTime beneficiaryBirthDate, string beneficiaryRelationShipCode, DateTime cutOffDate, bool chronicallyIll)
        {
           bool IsSecureActApplicable ;
           try
           {
               IsSecureActApplicable = SecureActBOClass.IsSecureActApplicable(perssBirthDate, beneficiaryBirthDate, beneficiaryRelationShipCode, cutOffDate, chronicallyIll);
               return IsSecureActApplicable;
           }
           catch
           {
               throw;
           }           
        }

        //END : ML | 2020.02.05 | YRS-AT-4769 ,YRS-AT-4783 | Non-spouse who is more than 10 years younger than the Participant and does not display J options.
    }
}
