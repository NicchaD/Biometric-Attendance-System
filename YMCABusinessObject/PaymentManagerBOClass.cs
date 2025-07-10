//****************************************************
//Modification History
//****************************************************
//Modified by       Date        Description
//****************************************************
//Prasad Jadhav     24/10/11    For BT-931,YRS 5.0-1412:Changes for the .bch file
//Prasad Jadhav     29/10/11    For BT-931,YRS 5.0-1412:Changes for the .bch file
//Prasad Jadhav     29/03/2012  For BT-931,YRS 5.0-1412:Changes for the .bch file
//Priya Patil	    28.09.2012	BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//Sanjeev Gupta     2012.12.07  BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//Shashank Patel	2013.07.02  BT:1456/YRS 5.0-1735:Read current active address at time of disbursement funding
//Shashank Patel    2015.01.05  BT-2026\YRS 5.0-2079: Add additional line to payee information for rollover payments(reverted due to check file printing issue in UAT in 14.1.0 version)
//Manthan Rajguru   2015.09.16  YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Anudeep A         2016.02.05  YRS-AT-2079: Add additional line to payee information for rollover payments (-Removed new line from beneficiary of)
//Chandra sekar     2016.07.25  YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
//Sanjay GS Rawat   2018.04.06  YRS-AT-3101 - YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Pramod P. Pokale  2018.04.11  YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)
//Manthan Rajguru   2018.04.24  YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Vinayan C         2018.09.19  YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Sanjay R          2018.09.19  YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Manthan Rajguru   2018.12.03  YRS-AT-3101 -  YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
//Pramod P. Pokale  2018.12.21  YRS-AT-4253 - Changes to the output file. 
// Sanjay GS Rawat  2019.01.11  YRS-AT-4282 - YRS enh: EFT Loan Maint. Changes to the output file.(Track it ticket 36767)  
// Sanjay GS(Rawat) 2019.10.07  YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll 
//Megha Lad         2019.10.10  YRS-AT-4601 - YRS enh: State Withholding Project - Export file First Annuity Payroll
//Megha Lad         2019.12.18  YRS-AT-4676 - State Withholding - Validations for exporting file from Payment Manager (First Annuities)
//Megha Lad         2020.03.12  YRS-AT-4601 - (Hotfix) Check number auto increment funcationality is not working. 
//****************************************************

using System;
using System.IO;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Globalization;
using System.Data.SqlClient;
// START : SR | 2018.04.25 | adding reference for send Email & exception logging.
using System.Collections.Generic;
using YMCAObjects; 
// END : SR | 2018.04.25 | adding reference for send Email & exception logging.
using YMCARET.YmcaDataAccessObject; // SR | 2018.05.25 | YRS-AT-3101 | Added reference to have access to HelperFunctions class.

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for PaymentManagerBOClass.
    /// </summary>
    /// 
    public class PaymentManagerBOClass
    {
        DateTime l_DateTime_AcctDate, l_DateTime_CheckDate;
        bool l_bool_NegetivePayments;
        Int32 l_Int32CheckNoRefund;
        Int32 l_Int32CheckNoPayrol;
        Int32 l_Int32CheckNoCanada;
        Int32 l_Int32CheckNoTDLoan;
        Int32 l_Int32CheckNoEXP;
        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        Int32 l_Int32CheckNoSHIRAM;
        string l_string_CheckNoSHIRAMPrefix;
        bool m_bool_ShiraM;

        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
        //bool m_bool_IsMRDEligible = false;
        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        //Int32 mrdcheckno;
        //end 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

        string l_string_CheckNoPayrolPrefix;
        string l_string_CheckNoRefundPrefix;
        string l_string_CheckNoCanadaPrefix;
        string l_string_CheckNoTDLoanPrefix;
        string l_string_CheckNoEXPPrefix;
        DataTable l_datatable_Disbursements;
        DataTable l_datatable_Withheld;
        DataTable l_datatable_disbursementType;
        DataTable l_datatable_ANNDisbursements;
        DataTable l_datatable_REFDisbursements;
        DataTable l_datatable_TDLoanDisbursements;
        DataTable l_datatable_EXPDisbursements;
        DataTable l_datatable_DisbursementsREPL;
        DataTable l_datatable_ANNDisbursementsREPL;
        DataTable l_datatable_REFDisbursementsREPL;
        //BY APARNA
        DataTable l_datatable_TDLoanDisbursementsREPL;
        DataTable l_datatable_EXPDisbursementsREPL;
        //END 
        //Phase V Changes-start
        DataTable l_datatable_DWDisbursement;
        DataTable l_datatable_HWDisbursement;
        //Phase V Changes-end
        //start of change by ruchi
        //string [][] arrayFileList; commented to be replaced by datatable
        DataTable l_datatable_FileList;
        string l_string_ErrorString = string.Empty;
        public DataTable dtExpceptionLogDetails;//ML | 2019.12.17 |YRS-AT-4676 | Datatable will store error which is return by FSTANN file creation procedure
        

        //end of change
        private enum ValidateMode { MainOutputFileInfo = 0, MainOutputFileAddlInfo, ANNYTDOutputFileInfo, ANNYTDWithholdingOutputFileInfo, ANNWithholdingOutputFileInfo, REFAccountDetailRefund, REFWithholdingsRefund, REFDeductions };

        public PaymentManagerBOClass()
        {
            l_DateTime_AcctDate = System.Convert.ToDateTime("01/01/1900");
            l_bool_NegetivePayments = false;
            l_DateTime_CheckDate = System.DateTime.Now;
            l_Int32CheckNoRefund = 0;
            l_Int32CheckNoPayrol = 0;
            l_Int32CheckNoCanada = 0;
            l_Int32CheckNoTDLoan = 0;
            l_Int32CheckNoEXP = 0;
            //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            l_Int32CheckNoSHIRAM = 0;
            l_bool_NegetivePayments = false;
            l_datatable_Disbursements = null;
            l_datatable_disbursementType = null;
            l_datatable_Withheld = null;


        }

        public DataTable DataTableNameFileList
        {
            get
            {
                return l_datatable_FileList;
            }
            set
            {
                l_datatable_FileList = value;
            }
        }


        //		public  bool getdisbursementswithoutfunding(string DisbursementType,string CheckBoxRefundWithDrawal)
        //		{
        //			DataSet l_dataset ;
        //			//DataTable l_datatable_AccountingDate;
        //			DataTable l_dataTable_DisbursementsNegative =null;
        //					
        //			try
        //			{
        //				l_dataset = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.getdisbursementswithoutfunding(DisbursementType,CheckBoxRefundWithDrawal);
        //				if (l_dataset == null) return false;
        //				
        //				l_datatable_Disbursements = l_dataset.Tables["Disbursements"];
        //				l_datatable_DisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
        //
        //				l_dataTable_DisbursementsNegative = l_dataset.Tables["DisbursementsNegative"];
        //				if (l_dataTable_DisbursementsNegative !=null) 
        //				{
        //					if (l_dataTable_DisbursementsNegative.Rows.Count > 0)
        //					{
        //						this.l_bool_NegetivePayments = true;
        //					}
        //				}
        //
        //				// populate Check Numbers
        //				validateCheckSeries();
        //						
        //
        //				return true;
        //
        ///* 
        //				// Set Accounting Date.
        //				l_datatable_AccountingDate = l_dataset.Tables["AccoutDate"]; 
        //				if (l_datatable_AccountingDate != null) 
        //				{
        //					this.l_DateTime_AcctDate = System.Convert.ToDateTime(l_datatable_AccountingDate.Rows[0]["dtmEndDate"]);   
        //				}
        //
        //				
        //
        //				l_datatable_ANNDisbursements = l_dataset.Tables["ANNDisbursement"];
        //				l_datatable_ANNDisbursements = l_dataset.Tables["Disbursements"];
        //				l_datatable_REFDisbursements = l_dataset.Tables["REFDisbursement"];
        //				l_datatable_REFDisbursements = l_dataset.Tables["Disbursements"];
        //				l_datatable_TDLoanDisbursements = l_dataset.Tables["TDLoanDisbursement"];
        //				l_datatable_TDLoanDisbursements = l_dataset.Tables["Disbursements"];
        //				l_datatable_EXPDisbursements = l_dataset.Tables["EXPDisbursement"];
        //				l_datatable_EXPDisbursements = l_dataset.Tables["Disbursements"];
        //				l_datatable_DisbursementsREPL = l_dataset.Tables["DisbursementsREPL"];
        //
        //				l_datatable_ANNDisbursementsREPL = l_dataset.Tables["ANNDisbursementREPL"];
        //				l_datatable_ANNDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
        //				l_datatable_REFDisbursementsREPL = l_dataset.Tables["REFDisbursementREPL"];
        //				l_datatable_REFDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
        //				//APARNA
        //				l_datatable_TDLoanDisbursementsREPL = l_dataset.Tables["TDLoanDisbursementREPL"];
        //				l_datatable_TDLoanDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
        //				l_datatable_EXPDisbursementsREPL = l_dataset.Tables["EXPDisbursementREPL"];
        //				l_datatable_EXPDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
        //				//APARNA
        //				Phase V Changes-start
        //				l_datatable_DWDisbursement = l_dataset.Tables["DWDisbursement"];
        //				l_datatable_DWDisbursement = l_dataset.Tables["Disbursements"];
        //				l_datatable_HWDisbursement = l_dataset.Tables["HWDisbursement"];
        //				l_datatable_HWDisbursement = l_dataset.Tables["Disbursements"];
        //				l_datatable_disbursementType = l_dataset.Tables["disbursementtypes"];  
        //				l_datatable_Withheld = l_dataset.Tables["Withhoidings"];				
        //				l_datatable_Withheld = l_dataset.Tables["Disbursements"];
        //
        //*/			
        //				
        //			}
        //			catch
        //			{
        //				throw;
        //			}
        //
        //		}

        //Modified the function for Gemini Issue 786 -Amit
        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 add new parameter for shira
        public bool getdisbursementswithoutfunding(string DisbursementType, string checkRefundWithDrawalAll, string checkRefundWithDrawalHard, string checkRefundWithDrawalDeath, string checkRefundWithDrawalShira, string paymentMethod) // SR | 2018.04.11 | YRS-AT-3101 | pass new parameter(PaymentMethod) to retrive disbusement records based on payment type.
        {
            DataSet l_dataset;
            //DataTable l_datatable_AccountingDate;
            DataTable l_dataTable_DisbursementsNegative = null;

            try
            {
                l_dataset = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.getdisbursementswithoutfunding(DisbursementType, checkRefundWithDrawalAll, checkRefundWithDrawalHard, checkRefundWithDrawalDeath, checkRefundWithDrawalShira, paymentMethod); // SR | 2018.04.11 | YRS-AT-3101 | pass new parameter(PaymentMethod) to retrive disbusement records based on payment type.
                if (l_dataset == null) return false;

                l_datatable_Disbursements = l_dataset.Tables["Disbursements"];

                //Shashi Shekhar-2009-11-25: Modified below code to return all disbursement data in one table 
                //l_datatable_DisbursementsREPL = l_dataset.Tables["DisbursementREPL"];

                l_dataTable_DisbursementsNegative = l_dataset.Tables["DisbursementsNegative"];
                if (l_dataTable_DisbursementsNegative != null)
                {
                    if (l_dataTable_DisbursementsNegative.Rows.Count > 0)
                    {
                        this.l_bool_NegetivePayments = true;
                    }
                }

                // populate Check Numbers
                validateCheckSeries();


                return true;

                /* 
                                // Set Accounting Date.
                                l_datatable_AccountingDate = l_dataset.Tables["AccoutDate"]; 
                                if (l_datatable_AccountingDate != null) 
                                {
                                    this.l_DateTime_AcctDate = System.Convert.ToDateTime(l_datatable_AccountingDate.Rows[0]["dtmEndDate"]);   
                                }

				

                                l_datatable_ANNDisbursements = l_dataset.Tables["ANNDisbursement"];
                                l_datatable_ANNDisbursements = l_dataset.Tables["Disbursements"];
                                l_datatable_REFDisbursements = l_dataset.Tables["REFDisbursement"];
                                l_datatable_REFDisbursements = l_dataset.Tables["Disbursements"];
                                l_datatable_TDLoanDisbursements = l_dataset.Tables["TDLoanDisbursement"];
                                l_datatable_TDLoanDisbursements = l_dataset.Tables["Disbursements"];
                                l_datatable_EXPDisbursements = l_dataset.Tables["EXPDisbursement"];
                                l_datatable_EXPDisbursements = l_dataset.Tables["Disbursements"];
                                l_datatable_DisbursementsREPL = l_dataset.Tables["DisbursementsREPL"];

                                l_datatable_ANNDisbursementsREPL = l_dataset.Tables["ANNDisbursementREPL"];
                                l_datatable_ANNDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
                                l_datatable_REFDisbursementsREPL = l_dataset.Tables["REFDisbursementREPL"];
                                l_datatable_REFDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
                                //APARNA
                                l_datatable_TDLoanDisbursementsREPL = l_dataset.Tables["TDLoanDisbursementREPL"];
                                l_datatable_TDLoanDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
                                l_datatable_EXPDisbursementsREPL = l_dataset.Tables["EXPDisbursementREPL"];
                                l_datatable_EXPDisbursementsREPL = l_dataset.Tables["DisbursementREPL"];
                                //APARNA
                                Phase V Changes-start
                                l_datatable_DWDisbursement = l_dataset.Tables["DWDisbursement"];
                                l_datatable_DWDisbursement = l_dataset.Tables["Disbursements"];
                                l_datatable_HWDisbursement = l_dataset.Tables["HWDisbursement"];
                                l_datatable_HWDisbursement = l_dataset.Tables["Disbursements"];
                                l_datatable_disbursementType = l_dataset.Tables["disbursementtypes"];  
                                l_datatable_Withheld = l_dataset.Tables["Withhoidings"];				
                                l_datatable_Withheld = l_dataset.Tables["Disbursements"];

                */

            }
            catch
            {
                throw;
            }

        }



        public void validateCheckSeries()
        {
            DataTable l_datatable_CheckSeries;
            try
            {
                l_datatable_CheckSeries = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.CurrentCheckSeriesAll();
                if (l_datatable_CheckSeries != null)
                {
                    foreach (DataRow oRow in l_datatable_CheckSeries.Rows)
                    {
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "PAYROL")
                        {
                            this.l_Int32CheckNoPayrol = System.Int32.Parse(oRow["CurrentCheckNumber"].ToString());
                            this.l_string_CheckNoPayrolPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoPayrolPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();


                            // EXP Devident will use the same check series  of Payroll
                            this.l_Int32CheckNoEXP = System.Int32.Parse(oRow["CurrentCheckNumber"].ToString());
                            this.l_string_CheckNoEXPPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoEXPPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "CANADA")
                        {
                            this.l_Int32CheckNoCanada = System.Int32.Parse(oRow["CurrentCheckNumber"].ToString());
                            this.l_string_CheckNoCanadaPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoCanadaPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }

                        if (oRow["CheckSeriesCode"].ToString().Trim() == "REFUND")
                        {
                            this.l_Int32CheckNoRefund = System.Int32.Parse(oRow["CurrentCheckNumber"].ToString());
                            this.l_string_CheckNoRefundPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoRefundPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //mrdcheckno = l_Int32CheckNoRefund;
                            //mrdcheckno = mrdcheckno + 1;

                        }
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "TDLOAN")
                        {
                            this.l_Int32CheckNoTDLoan = System.Int32.Parse(oRow["CurrentCheckNumber"].ToString());
                            this.l_string_CheckNoTDLoanPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoTDLoanPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }

                        /*
                        if (oRow["CheckSeriesCode"].ToString().Trim()  =="EXP") 
                        {
                            this.l_Int32CheckNoEXP  =  System.Int32.Parse(oRow["CurrentCheckNumber"].ToString() ); 
                            this.l_string_CheckNoEXPPrefix= ""; 
                            if (oRow["CheckSeriesPrefix"].GetType().ToString()!= "System.DBNull") this.l_string_CheckNoEXPPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();  

                        }
                        */
                        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "SHIRAM")
                        {
                            this.l_Int32CheckNoSHIRAM = System.Int32.Parse(oRow["CurrentCheckNumber"].ToString());
                            this.l_string_CheckNoSHIRAMPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoSHIRAMPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }
                        //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    }
                }


            }
            catch
            {
                throw;
            }
        }

        private Int32 m_parameter_Int32_checkNoTestUS;
        public Int32 parameter_Int32_checkNoTestUS
        {
            get { return m_parameter_Int32_checkNoTestUS; }
            set { m_parameter_Int32_checkNoTestUS = value; }
        }


        //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Added "l_bool_ShiraM" parameter 
        public bool validateNewCheckNo(Int32 parameter_Int32_checkNoUS, Int32 parameter_Int32_checkNoCANADA, string parameter_string_AnnuityType, bool l_bool_ShiraM)
        {
            Int32 int32_CurrentCheckNumber = 0, int32_EndingCheckNumber = 0, int32_StartingCheckNumber = 0;
            DataTable l_datatable_CheckSeries = null;
            Int32 ln_CheckNumber = 0;

            try
            {
                l_datatable_CheckSeries = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.CurrentCheckSeriesAll();

                if (l_datatable_CheckSeries != null)
                {
                    foreach (DataRow oRow in l_datatable_CheckSeries.Rows)
                    {

                        if ((parameter_string_AnnuityType == "ANN" || parameter_string_AnnuityType == "REF" || parameter_string_AnnuityType == "TDLOAN" || parameter_string_AnnuityType == "EXP") &&
                            (oRow["CheckSeriesCode"].ToString().Trim() == "CANADA"))
                        {
                            //Dont do anything
                        }
                        else if (parameter_string_AnnuityType == "ANN" && oRow["CheckSeriesCode"].ToString().Trim() == "PAYROL")
                        {
                            //Dont do anything
                        }

                        //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Check "l_bool_ShiraM" parameter 
                        else if (parameter_string_AnnuityType == "REF" && oRow["CheckSeriesCode"].ToString().Trim() == "REFUND")
                        {
                            //Dont do anything
                        }
                        //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 & Check "l_bool_ShiraM" parameter 
                        else if (parameter_string_AnnuityType == "REF" && oRow["CheckSeriesCode"].ToString().Trim() == "SHIRAM")
                        {
                            //Dont do anything
                        }
                        else if (parameter_string_AnnuityType == "TDLOAN" && oRow["CheckSeriesCode"].ToString().Trim() == "TDLOAN")
                        {
                            //Dont do anything
                        }
                        else if (parameter_string_AnnuityType == "EXP" && oRow["CheckSeriesCode"].ToString().Trim() == "PAYROL")
                        {
                            //Dont do anything
                        }
                        else continue;

                        // Assign the Check Series Prefix.

                        if (oRow["CheckSeriesCode"].ToString().Trim() == "PAYROL")
                        {
                            this.l_string_CheckNoPayrolPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoPayrolPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                            this.l_string_CheckNoEXPPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoEXPPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "CANADA")
                        {
                            this.l_string_CheckNoCanadaPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoCanadaPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }

                        if (oRow["CheckSeriesCode"].ToString().Trim() == "REFUND")
                        {
                            this.l_string_CheckNoRefundPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoRefundPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();
                        }
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "TDLOAN")
                        {
                            this.l_string_CheckNoTDLoanPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoTDLoanPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }
                        /*
                        if (oRow["CheckSeriesCode"].ToString().Trim()  =="EXP") 
                        {
                            this.l_string_CheckNoEXPPrefix = ""; 
                            if (oRow["CheckSeriesPrefix"].GetType().ToString()!= "System.DBNull") this.l_string_CheckNoEXPPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();  

                        }
                        */
                        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        if (oRow["CheckSeriesCode"].ToString().Trim() == "SHIRAM")
                        {
                            this.l_string_CheckNoSHIRAMPrefix = "";
                            if (oRow["CheckSeriesPrefix"].GetType().ToString() != "System.DBNull") this.l_string_CheckNoSHIRAMPrefix = oRow["CheckSeriesPrefix"].ToString().Trim();

                        }
                        //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000						
                        switch (oRow["CheckSeriesCode"].ToString().Trim())
                        {

                            case "REFUND":
                                if (this.m_bool_ShiraM == true)
                                {
                                    m_parameter_Int32_checkNoTestUS = Convert.ToInt32(oRow["CurrentCheckNumber"].ToString()); //Fetch cheque no
                                    m_parameter_Int32_checkNoTestUS += 1;
                                    ln_CheckNumber = m_parameter_Int32_checkNoTestUS;
                                }
                                else
                                {
                                    ln_CheckNumber = parameter_Int32_checkNoUS;
                                }
                                break;
                            case "TDLOAN":
                                ln_CheckNumber = parameter_Int32_checkNoUS;
                                break;
                            /* case "EXP":
                                ln_CheckNumber = parameter_Int32_checkNoUS;
                                break;
                                */
                            case "PAYROL":
                                ln_CheckNumber = parameter_Int32_checkNoUS;
                                break;
                            case "CANADA":
                                ln_CheckNumber = parameter_Int32_checkNoCANADA;
                                break;
                            case "SHIRAM":
                                if (this.m_bool_ShiraM == false)
                                {
                                    m_parameter_Int32_checkNoTestUS = Convert.ToInt32(oRow["CurrentCheckNumber"].ToString()); //Fetch cheque no
                                    m_parameter_Int32_checkNoTestUS += 1;
                                    ln_CheckNumber = m_parameter_Int32_checkNoTestUS;
                                }
                                else
                                {
                                    ln_CheckNumber = parameter_Int32_checkNoUS;
                                }
                                //ln_CheckNumber = parameter_Int32_checkNoUS;
                                break;
                        }

                        int32_CurrentCheckNumber = Convert.ToInt32(oRow["CurrentCheckNumber"]);
                        int32_EndingCheckNumber = Convert.ToInt32(oRow["EndingCheckNumber"]);
                        int32_StartingCheckNumber = Convert.ToInt32(oRow["StartingCheckNumber"]);
                        // if (ln_CheckNumber > int32_CurrentCheckNumber &&  ln_CheckNumber <= int32_EndingCheckNumber &&  ln_CheckNumber >= int32_StartingCheckNumber)
                        if (ln_CheckNumber > int32_CurrentCheckNumber)
                        {
                            //Continue
                        }
                        // START | ML | 2019.10.22 | YRS-AT-4601 | Do not validate check number in case of "Ann".(If IsPaymentOutSourcingKeyON is True)
                        else if ((parameter_string_AnnuityType == "ANN") && (parameter_Int32_checkNoUS == 0) && (IsPaymentOutSourcingKeyON  == true ))
                        {
                            return true;                        
                    }
                        // END | ML | 2019.10.22 | YRS-AT-4601 | Do not validate check number in case of "Ann".(If IsPaymentOutSourcingKeyON is True)
                        else                                                 
                            return false;
                    }

                    return true;
                }
                return false;
            }

            catch
            {
                return false;
            }

        }


        public string getCurrentCheckNo(string parameter_string_refundtype, Int32 parameter_Int32CheckUS, Int32 parameter_Int32CheckCANANDA, bool bool_add)
        {
            string l_string_CheckNO = string.Empty;
            try
            {
                // validateCheckSeries(); 

                //Refresh the current  Check Series.
                if (parameter_Int32CheckUS != 0)
                {
                    if (this.m_bool_ShiraM == true)
                    {
                        l_Int32CheckNoRefund = m_parameter_Int32_checkNoTestUS;
                    }
                    else
                    {
                        l_Int32CheckNoRefund = parameter_Int32CheckUS;
                    }

                }

                if (parameter_Int32CheckUS != 0)
                {
                    l_Int32CheckNoPayrol = parameter_Int32CheckUS;
                }
                if (parameter_Int32CheckUS != 0)
                {
                    l_Int32CheckNoTDLoan = parameter_Int32CheckUS;

                }

                if (parameter_Int32CheckUS != 0)
                {
                    l_Int32CheckNoEXP = parameter_Int32CheckUS;

                }

                //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                if (parameter_Int32CheckUS != 0)
                {
                    l_Int32CheckNoSHIRAM = parameter_Int32CheckUS;
                }
                //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                if (parameter_Int32CheckCANANDA != 0)
                {
                    l_Int32CheckNoCanada = parameter_Int32CheckCANANDA;
                }



                switch (parameter_string_refundtype)
                {
                    case "REFUND":
                        if (this.m_bool_ShiraM == true)
                        {
                            l_string_CheckNO = this.l_string_CheckNoRefundPrefix + this.m_parameter_Int32_checkNoTestUS.ToString().Trim();

                        }
                        else
                        {
                            l_string_CheckNO = this.l_string_CheckNoRefundPrefix + this.l_Int32CheckNoRefund.ToString().Trim();
                            //l_Int32CheckNoRefund = parameter_Int32CheckUS;
                        }

                        //l_string_CheckNO = this.l_string_CheckNoRefundPrefix + this.l_Int32CheckNoRefund.ToString().Trim();
                        if (bool_add) this.l_Int32CheckNoRefund++;
                        break;
                    case "PAYROL":

                        l_string_CheckNO = this.l_string_CheckNoPayrolPrefix + this.l_Int32CheckNoPayrol.ToString().Trim();
                        if (bool_add) this.l_Int32CheckNoPayrol++;
                        break;
                    case "CANADA":

                        l_string_CheckNO = this.l_string_CheckNoCanadaPrefix + this.l_Int32CheckNoCanada.ToString().Trim();
                        if (bool_add) this.l_Int32CheckNoCanada++;
                        break;
                    case "TDLOAN":
                        l_string_CheckNO = this.l_string_CheckNoTDLoanPrefix + this.l_Int32CheckNoTDLoan.ToString().Trim();
                        if (bool_add) this.l_Int32CheckNoTDLoan++;
                        break;
                    case "EXP":
                        l_string_CheckNO = this.l_string_CheckNoEXPPrefix + this.l_Int32CheckNoEXP.ToString().Trim();
                        if (bool_add) this.l_Int32CheckNoEXP++;
                        break;
                    //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    case "SHIRAM":
                        l_string_CheckNO = this.l_string_CheckNoSHIRAMPrefix + this.l_Int32CheckNoSHIRAM.ToString().Trim();
                        if (bool_add) this.l_Int32CheckNoSHIRAM++;
                        break;
                    //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                }
                return l_string_CheckNO;
            }
            catch
            {
                throw;
            }

        }


        public DataTable GetErroneousDisbursements(DateTime parameter_Date)
        {
            try
            {
                return (YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.GetErroneousDisbursements(parameter_Date));
            }
            catch
            {
                throw;
            }

        }


        private bool AddManualDisbursementforFunding(DataTable parameter_DataTable_Dibursement, DateTime Parameter_DateTimeIssueDate, Int32 Parameter_Int32_CheckNbrUS, Int32 parameter_Int32_checkNoCANADA, bool parameter_bool_Fund, string parameter_string_Expr, string parameter_manualCheckNO)
        {
            //CUSDISBURSEMENT.DSADDMANUALDISBURSEMENTFORFUNDING
            DataTable l_Datatable_DisbursementOutputFileInfo = null;
            DataTable l_Datatable_DisbursementOutputFileAddlInfo = null;
            DataTable l_Datatable_YTDDisbursementOutputFileInfo = null;
            DataTable l_Datatable_YTDWithholdingOutputFileInfo = null;
            DataTable l_Datatable_WithholdingsByDisbursement = null;
            DataTable l_Datatable_DisbursementDetailsbyRefundDisbursement = null;
            DataTable l_Datatable_DisbursementWithholdingsbyRefundDisbursement = null;
            DataTable l_Datatable_DisbursementDeductions = null;
            DataTable l_Datatable_WithholdingsByTDLoan = null;
            // START | SR | 2019.10.10 | YRS-AT-4601 | Declare variables for first annuity export file.
            DataSet dsFirstAnnuityExportData = null;
            string errorMessage = string.Empty;
            // END | SR | 2019.10.10 | YRS-AT-4601 | Declare variables for first annuity export file.

            YMCARET.YmcaBusinessObject.PaymentManagerBOWraperClass Paymentoprocess = new YMCARET.YmcaBusinessObject.PaymentManagerBOWraperClass();
            YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass objDisbursementFunding = new YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass();

            string l_string_CheckNbr = string.Empty;
            string l_string_CheckSeries = string.Empty;

            //string l_string_message = string.Empty;
            string l_string_persid;
            //Aparna
            string l_string_DisbursementType;

            DateTime l_dateTime_effectiveYear;
            string l_string_DisbursementId = string.Empty;
            string l_string_DisbursementNumber = string.Empty;
            string l_string_InstrumentTypeCode = string.Empty; ;
            double l_double_NetAmount = 0.00;
            DataRow l_Datarow = null;
            DataSet l_dataset_disbursal = null;
            DataRow[] l_DataRow_Dibursements;
            bool l_bool_ErrorProcess = false;
            string l_string_ErrorMsg = string.Empty;
            bool bool_addcheckNumber = false;
            string l_string_CheckSeries_UPDATE = string.Empty;
            //START : MMR | 2018.12.03 | YRS-AT-3101 |  Declared objects for email sending activity
            ReturnObject<bool> emailResult;
            MailUtil mailClient = new MailUtil();
            //END : MMR | 2018.12.03 | YRS-AT-3101 |  Declared objects for email sending activity
            bool insertDisbursementFundingIsRequired = true; // ML | 2019.12.17 | YRS-AT-4601 | set flag if Insert disburseent funding is required
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("AddManualDisbursementforFunding - START")); // ML |2020.01.21 |YRS-AT-4601 | Logger implementation 
                l_string_ErrorString = string.Empty;
                //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                Paymentoprocess.boolShiraProcess = this.m_bool_ShiraM;
                Paymentoprocess.bool_OutputFileCShira_MilleTrust = this.m_bool_ShiraM;
                //END PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000	

                Paymentoprocess.IdentifyPaymentTypes(parameter_DataTable_Dibursement, parameter_string_Expr);

                l_dateTime_effectiveYear = Parameter_DateTimeIssueDate;

                l_dataset_disbursal = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.GetOutPutFileforDisbursal();

                //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                if (this.m_bool_ShiraM == true)
                {
                    //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    //validateCheckSeries();
                    //mrdcheckno = l_Int32CheckNoRefund;
                    //mrdcheckno = mrdcheckno +1;

                    //Added By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                    //Paymentoprocess.MRDCnt = parameter_DataTable_Dibursement.Select("MRD = 1").Length;
                    //Paymentoprocess.MilleCnt = parameter_DataTable_Dibursement.Select("MRD = 0").Length;
                    Paymentoprocess.MilleCnt = parameter_DataTable_Dibursement.Rows.Count;
                }

                //END PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                Paymentoprocess.ProduceOutputFilesOutput(l_dataset_disbursal.Tables["OutPutfilePath"]);
                //START : ML |2020.01.21 |YRS-AT-4601 | Create Export base entry
                int processId = 0;
                string currencyCode = "U";
                DataRow[] CurrencyCodeRows = parameter_DataTable_Dibursement.Select("CurrencyCode='" + currencyCode  + "'");
                if (CurrencyCodeRows.Length > 0 && IsPaymentOutSourcingKeyON)
                {
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("CreateExportBaseHeaderEntry - START"));
                    processId = objDisbursementFunding.CreateExportBaseHeaderEntry("FSTANN", "ANN", "PENDING"); // SR | 2019.10.07 | YRS-AT-4601 | Generate ProcessId For First Annuity Batch.
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("CreateExportBaseHeaderEntry - END"));
                }
                //END : ML |2020.01.21 |YRS-AT-4601 | Create Export base entry
                Paymentoprocess.ProduceOutputFilesCreateHeader();

                l_DataRow_Dibursements = parameter_DataTable_Dibursement.Select(parameter_string_Expr);

                foreach (DataRow parameter_DataRow_Dibursement in l_DataRow_Dibursements)
                {
                    l_string_DisbursementId = parameter_DataRow_Dibursement["DisbursementID"].ToString().Trim();
                    l_string_persid = parameter_DataRow_Dibursement["PersId"].ToString().Trim();
                    l_string_DisbursementType = parameter_DataRow_Dibursement["DisbursementType"].ToString().Trim();

					//	02.07.2013 SP: BT:1456/YRS 5.0-1735:Read current active address at time of disbursement funding -Start
					YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.UpdateDisbursementAddressID(l_string_DisbursementId);
					//	02.07.2013 SP: BT:1456/YRS 5.0-1735:Read current active address at time of disbursement funding -End

                    //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    if (this.m_bool_ShiraM == true)
                    {
                        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                        //m_bool_IsMRDEligible = Convert.ToBoolean(parameter_DataRow_Dibursement["MRD"].ToString().Trim());
                        //if (m_bool_IsMRDEligible == true)
                        //{
                        //    Paymentoprocess.boolShiraProcess = false;
                        //    Paymentoprocess.bool_OutputFileCShira_MilleTrust = false;
                        //    Paymentoprocess.boolIsMRDEligibleProcess = true;
                        //}
                        //else
                        //{
                        Paymentoprocess.boolShiraProcess = true;
                        Paymentoprocess.bool_OutputFileCShira_MilleTrust = true;

                        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                        //Paymentoprocess.boolIsMRDEligibleProcess = false;
                        //}
                    }
                    //END PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                    //AddManualDisbursementforFunding(oRow,System.DateTime.Now,l_string_checkNo,parameter_bool_Fund);				
                    //l_string_checkNo= string.Empty; 
                    l_Datatable_DisbursementOutputFileInfo = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementOutputFileInfo(l_string_DisbursementId);

                    if (ValidateObject(l_Datatable_DisbursementOutputFileInfo, (int)ValidateMode.MainOutputFileInfo, l_string_DisbursementId) == false)
                    {
                        l_bool_ErrorProcess = true;
                        throw new Exception(this.l_string_ErrorString);

                    }

                    l_Datatable_DisbursementOutputFileAddlInfo = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementOutputFileAddlInfo(l_string_DisbursementId);
                    if (ValidateObject(l_Datatable_DisbursementOutputFileAddlInfo, (int)ValidateMode.MainOutputFileAddlInfo, l_string_DisbursementId) == false)
                    {
                        l_bool_ErrorProcess = true;
                        throw new Exception(this.l_string_ErrorString);

                    }
                    l_Datarow = l_Datatable_DisbursementOutputFileInfo.Rows[0];

                    switch (l_Datarow["DisbursementType"].ToString().ToUpper().Trim())
                    {
                        case "ANN":
                            l_Datatable_YTDDisbursementOutputFileInfo = objDisbursementFunding.YTDDisbursementsByPayee(l_string_persid, l_dateTime_effectiveYear, l_string_DisbursementType);

                            if (ValidateObject(l_Datatable_YTDDisbursementOutputFileInfo, (int)ValidateMode.ANNYTDOutputFileInfo, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                            l_Datatable_YTDWithholdingOutputFileInfo = objDisbursementFunding.YTDWithholdingsByPayee(l_string_persid, l_dateTime_effectiveYear, l_string_DisbursementType);
                            if (ValidateObject(l_Datatable_YTDWithholdingOutputFileInfo, (int)ValidateMode.ANNYTDWithholdingOutputFileInfo, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }


                            l_Datatable_WithholdingsByDisbursement = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.WithholdingsByDisbursement(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_WithholdingsByDisbursement, (int)ValidateMode.ANNWithholdingOutputFileInfo, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                           
                           // START : ML | 2019.12.17 | YRS-AT-4601 | Set flag for disbursement funding and file creation
                            if (IsPaymentOutSourcingKeyON == true)
                            {
                                if (l_Datarow["CurrencyCode"].ToString().Trim().ToUpper() == "C")
                                    insertDisbursementFundingIsRequired = true;
                                else
                                {
                                    Paymentoprocess.CreateOutputFileFSTANN = true;
                                    insertDisbursementFundingIsRequired = false;
                                }
                            }
                            
                            // END : ML | 2019.12.17 | YRS-AT-4601 | Set flag for disbursement funding and file creation
                            break;
                        case "REF":
                            l_Datatable_DisbursementDetailsbyRefundDisbursement = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementDetailsbyRefundDisbursement(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_DisbursementDetailsbyRefundDisbursement, (int)ValidateMode.REFAccountDetailRefund, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }


                            l_Datatable_DisbursementWithholdingsbyRefundDisbursement = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementWithholdingsbyRefundDisbursement(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_DisbursementWithholdingsbyRefundDisbursement, (int)ValidateMode.REFAccountDetailRefund, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                            l_Datatable_DisbursementDeductions = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementRefundDeductions(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_DisbursementDeductions, (int)ValidateMode.REFDeductions, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }


                            break;
                        case "TDLOAN":
                            l_Datatable_WithholdingsByTDLoan = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.WithholdingDetailByTDLoanDisbursement(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_WithholdingsByTDLoan, (int)ValidateMode.REFAccountDetailRefund, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                            l_Datatable_DisbursementDetailsbyRefundDisbursement = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementDetailsbyRefundDisbursement(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_DisbursementDetailsbyRefundDisbursement, (int)ValidateMode.REFAccountDetailRefund, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                            break;
                        //APARNA - TO INCLUDE EXP DIVIDENDS-18/10/2006

                        case "EXP":
                            l_Datatable_YTDDisbursementOutputFileInfo = objDisbursementFunding.YTDDisbursementsByPayee(l_string_persid, l_dateTime_effectiveYear, l_string_DisbursementType);

                            if (ValidateObject(l_Datatable_YTDDisbursementOutputFileInfo, (int)ValidateMode.ANNYTDOutputFileInfo, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                            l_Datatable_YTDWithholdingOutputFileInfo = objDisbursementFunding.YTDWithholdingsByPayee(l_string_persid, l_dateTime_effectiveYear, l_string_DisbursementType);
                            if (ValidateObject(l_Datatable_YTDWithholdingOutputFileInfo, (int)ValidateMode.ANNYTDWithholdingOutputFileInfo, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }


                            l_Datatable_WithholdingsByDisbursement = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.WithholdingsByDisbursement(l_string_DisbursementId);

                            if (ValidateObject(l_Datatable_WithholdingsByDisbursement, (int)ValidateMode.ANNWithholdingOutputFileInfo, l_string_DisbursementId) == false)
                            {
                                l_bool_ErrorProcess = true;
                                break;
                            }

                            break;


                    }


                    if (l_bool_ErrorProcess)
                    {
                        throw new Exception(this.l_string_ErrorString);
                        //throw new Exception("ERROR");
                    }

                    //l_Datarow = l_Datatable_DisbursementOutputFileInfo.Rows[0];

                    if (l_Datarow["PaymentMethodCode"].GetType().ToString() != "System.DBNull")
                    {
                        if (l_Datarow["PaymentMethodCode"].ToString().Trim().ToUpper() == "EFT")
                            l_string_InstrumentTypeCode = "E";
                        else if (l_Datarow["PaymentMethodCode"].ToString().Trim().ToUpper() == "ACH")
                            l_string_InstrumentTypeCode = "E";
                        else if (l_Datarow["PaymentMethodCode"].ToString().Trim().ToUpper() == "CHECK")
                        {

                            l_string_InstrumentTypeCode = "C";

                            if (l_Datarow["CurrencyCode"].ToString().Trim().ToUpper() == "C")
                            {
                                l_string_CheckSeries = "CANADA";
                                //Modified the Parameter value from US to Canadian-BUG id
                                //l_string_CheckNbr = Parameter_Int32_CheckNbrUS.ToString().Trim();
                                l_string_CheckNbr = parameter_Int32_checkNoCANADA.ToString().Trim();
                                //Modified the Parameter value from US to Canadian
                            }
                            //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //else if (l_Datarow["DisbursementType"].ToString().Trim().ToUpper() == "REF" && m_bool_ShiraM == true && m_bool_IsMRDEligible == false)
                            else if (l_Datarow["DisbursementType"].ToString().Trim().ToUpper() == "REF" && m_bool_ShiraM == true)
                            {
                                l_string_CheckSeries = "SHIRAM";
                                l_string_CheckNbr = Parameter_Int32_CheckNbrUS.ToString().Trim();
                            }
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //else if (l_Datarow["DisbursementType"].ToString().Trim().ToUpper() == "REF" && m_bool_ShiraM == true && m_bool_IsMRDEligible == true)
                            //{
                            //    l_string_CheckSeries = "REFUND";
                            //    l_string_CheckNbr = m_parameter_Int32_checkNoTestUS.ToString();
                            //    //Parameter_Int32_CheckNbrUS = m_parameter_Int32_checkNoTestUS;
                            //}
                            else if (l_Datarow["DisbursementType"].ToString().Trim().ToUpper() == "REF")
                            {
                                l_string_CheckSeries = "REFUND";
                                l_string_CheckNbr = Parameter_Int32_CheckNbrUS.ToString().Trim();

                            }
                            //ENd 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                            else if (l_Datarow["DisbursementType"].ToString().Trim().ToUpper() == "TDLOAN")
                            {
                                l_string_CheckSeries = "TDLOAN";
                                l_string_CheckNbr = Parameter_Int32_CheckNbrUS.ToString().Trim();

                            }
                            else if (l_Datarow["DisbursementType"].ToString().Trim().ToUpper() == "EXP")
                            {
                                l_string_CheckSeries = "PAYROL";
                                l_string_CheckNbr = Parameter_Int32_CheckNbrUS.ToString().Trim();

                            }
                            else
                            {
                                l_string_CheckSeries = "PAYROL";
                                l_string_CheckNbr = Parameter_Int32_CheckNbrUS.ToString().Trim();

                            }

                            if (parameter_bool_Fund && parameter_manualCheckNO.Trim() != string.Empty)
                            {
                                //l_string_CheckNbr = getCurrentCheckNo(l_string_CheckSeries,Parameter_Int32_CheckNbrUS,parameter_Int32_checkNoCANADA,bool_addcheckNumber);  
                                l_string_CheckNbr = parameter_manualCheckNO;
                                l_string_DisbursementNumber = parameter_manualCheckNO;

                            }
                            //START : ML | YRS-AT-4601 | 2020.03.12 | If DisbursementType is ANN , CurrencyCode is U and Paymentoutsourcing Key is ON then Check number will be always 0.
                            else if ((parameter_bool_Fund == false) && (l_Datarow["DisbursementType"].ToString().ToUpper().Trim() == "ANN") && (l_Datarow["CurrencyCode"].ToString().Trim().ToUpper() == "U") && this.isPaymentOutSourcingKeyON)
                            {
                                l_string_DisbursementNumber = getCurrentCheckNo(l_string_CheckSeries, Parameter_Int32_CheckNbrUS, parameter_Int32_checkNoCANADA, false);
                            }
                            //END : ML | YRS-AT-4601 | 2020.03.12 | If DisbursementType is ANN , CurrencyCode is U and Paymentoutsourcing Key is ON then Check number will be always 0.
                            else if (parameter_bool_Fund == false && bool_addcheckNumber == false)
                            {
                                //START : ML | YRS-AT-4601 | 2019.12.16 | If DisbursementType is ANN and CurrencyCode is U then Check number will be always 0
                                //if ((l_Datarow["DisbursementType"].ToString().ToUpper().Trim() == "ANN") && (l_Datarow["CurrencyCode"].ToString().Trim().ToUpper() == "U"))
                                //    l_string_DisbursementNumber = getCurrentCheckNo(l_string_CheckSeries, Parameter_Int32_CheckNbrUS, parameter_Int32_checkNoCANADA, false);
                                //else
                                //END : ML | YRS-AT-4601 | 2019.12.16 | If DisbursementType is ANN and CurrencyCode is U then Check number will be always 0
                                l_string_DisbursementNumber = getCurrentCheckNo(l_string_CheckSeries, Parameter_Int32_CheckNbrUS, parameter_Int32_checkNoCANADA, true);
                            }

                            else if (parameter_bool_Fund == false)
                            {
                                l_string_DisbursementNumber = getCurrentCheckNo(l_string_CheckSeries, 0, 0, true);
                            }
                        }


                        Paymentoprocess.PreparOutPutFileData(l_dataset_disbursal,
                            l_Datatable_DisbursementOutputFileInfo,
                            l_Datatable_DisbursementOutputFileAddlInfo,
                            l_Datatable_YTDDisbursementOutputFileInfo,
                            l_Datatable_YTDWithholdingOutputFileInfo,
                            l_Datatable_WithholdingsByDisbursement,
                            l_Datatable_DisbursementDetailsbyRefundDisbursement,
                            l_Datatable_DisbursementWithholdingsbyRefundDisbursement,
                            l_Datatable_DisbursementDeductions,
                            l_Datatable_WithholdingsByTDLoan,
                            l_string_DisbursementId, l_dateTime_effectiveYear, l_string_DisbursementNumber, false);


                        l_double_NetAmount = Paymentoprocess.Property_double_NetBalance;

                        if (l_string_CheckSeries == "EXP")
                            l_string_CheckSeries_UPDATE = "PAYROL";
                        else l_string_CheckSeries_UPDATE = l_string_CheckSeries;


                        //objDisbursementFunding.InsertDisbursementFunding(l_string_DisbursementId,
                        //    l_string_DisbursementNumber,
                        //    l_double_NetAmount,
                        //    l_dateTime_effectiveYear,
                        //    l_string_InstrumentTypeCode,
                        //    l_string_CheckSeries_UPDATE,
                        //    l_string_CheckNbr,
                        //    l_Datarow["DisbursementType"].ToString().ToUpper().Trim(),parameter_bool_Fund); 


                        //START: PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        //m_bool_IsMRDEligible = false if mrd eligible then bitpaid = false
                        bool l_bool_bitPaid = false;

                        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                        //if (m_bool_ShiraM == true && m_bool_IsMRDEligible == false)
                        if (m_bool_ShiraM == true)
                        {
                            l_bool_bitPaid = true;
                        }

                        // START | ML | 2019.10.22 | YRS-AT-4601 | Do not Insert DisbursementFundig Record For CurrencyCode U and DisbType "Ann".(If IsPaymentOutSourcingKeyON is True)
                        

                        if (insertDisbursementFundingIsRequired == true)
                        {
                        	objDisbursementFunding.InsertDisbursementFunding(l_string_DisbursementId,
                            l_string_DisbursementNumber,
                            l_double_NetAmount,
                            l_dateTime_effectiveYear,
                            l_string_InstrumentTypeCode,
                            l_string_CheckSeries_UPDATE,
                            l_string_CheckNbr,
                            l_Datarow["DisbursementType"].ToString().ToUpper().Trim(), parameter_bool_Fund, l_bool_bitPaid);
                        }
                       
                        //END: PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        // END | ML | 2019.10.22 | YRS-AT-4601 | Do not Insert DisbursementFundig Record For CurrencyCode U and DisbType "Ann".(If IsPaymentOutSourcingKeyON is True)

                        bool_addcheckNumber = true;
                        
                        //Assign the Next Check Numbers to process.
                        if (l_string_CheckSeries == "REFUND")
                        {
                            if (m_bool_ShiraM == true)
                            {
                                m_parameter_Int32_checkNoTestUS = this.l_Int32CheckNoRefund;
                            }
                            else
                            {
                                Parameter_Int32_CheckNbrUS = this.l_Int32CheckNoRefund;
                            }
                            parameter_Int32_checkNoCANADA = this.l_Int32CheckNoCanada;
                        }
                        else if (l_string_CheckSeries == "TDLOAN")
                        {
                            Parameter_Int32_CheckNbrUS = this.l_Int32CheckNoTDLoan;
                            parameter_Int32_checkNoCANADA = this.l_Int32CheckNoCanada;
                        }
                        else if (l_string_CheckSeries == "EXP")
                        {
                            Parameter_Int32_CheckNbrUS = this.l_Int32CheckNoEXP;
                            parameter_Int32_checkNoCANADA = this.l_Int32CheckNoCanada;
                        }
                        //Added By SG: 2012.08.30: BT-960
                        else if (l_string_CheckSeries == "SHIRAM")
                        {
                            Parameter_Int32_CheckNbrUS = this.l_Int32CheckNoSHIRAM;
                            parameter_Int32_checkNoCANADA = this.l_Int32CheckNoCanada;
                        }
                        else
                        {
                            Parameter_Int32_CheckNbrUS = this.l_Int32CheckNoPayrol;
                            parameter_Int32_checkNoCANADA = this.l_Int32CheckNoCanada;
                        }


                    }
                    ////PP '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    //if ((m_bool_ShiraM == false && m_bool_IsMRDEligible == true) || (m_bool_ShiraM == true && m_bool_IsMRDEligible == false))
                    //{
                    //    Paymentoprocess.ProduceOutputFilesCreateFooters();
                    //    Paymentoprocess.CloseOutputStreamFiles();
                    //}
                    ////END PP '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                    //START : SR | 2019.10.07 | YRS-AT-4601 | generate record for export file.
                    if (insertDisbursementFundingIsRequired == false)
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("CreateExportBaseEntryForFirstAnnuityPayment - START"));
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("Disbursement No :" + l_string_DisbursementId + " ProcessID :" + processId )); 
                        objDisbursementFunding.CreateExportBaseEntryForFirstAnnuityPayment(l_string_DisbursementId, processId);
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("CreateExportBaseEntryForFirstAnnuityPayment - END")); 
                    }
                    //END : SR | 2019.10.07 | YRS-AT-4601 | generate record for export file.

                }
                //PP '28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                //Added into if condtion for shira
                //if (m_bool_ShiraM == false && m_bool_IsMRDEligible == false)
                //	{
                Paymentoprocess.ProduceOutputFilesCreateFooters();
                Paymentoprocess.CloseOutputStreamFiles();
                //}
                // Save the disbursment realtions

                //START : ML | 2019.10.07 | YRS-AT-4601 | Get record for export file from database and write to the file.
                if (processId > 0)
                {
                    dsFirstAnnuityExportData = new DataSet();
                    dsFirstAnnuityExportData = objDisbursementFunding.GetFirstAnnuityExportFile(processId, ref errorMessage);

                    if (HelperFunctions.isNonEmpty(dsFirstAnnuityExportData.Tables["dtExpceptionLogDetails"]))
                        dtExpceptionLogDetails = dsFirstAnnuityExportData.Tables["dtExpceptionLogDetails"];

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("FSTANN File Write - START"));
                        Paymentoprocess.WriteOutputFilesDataTableIntoStream(dsFirstAnnuityExportData.Tables["dtFirstAnnuityExportData"]);
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("ProcessID :" + processId + " File Status : EXPORTED"));
                        objDisbursementFunding.ChangeExportFileStatus(processId, "EXPORTED");
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("FSTANN File Write - END"));
                    }
                    else
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("ProcessID :" + processId + " File Status : FAILED"));
                        objDisbursementFunding.ChangeExportFileStatus(processId, "FAILED");
                    }
                    
                    
                }
                //END : ML | 2019.10.07 | YRS-AT-4601 |  Get record for export file from database and write to the file.
                objDisbursementFunding.InsertDisbursementFileTypes(l_dataset_disbursal);

                this.l_datatable_FileList = Paymentoprocess.DataTableNameFileList;
                //START : MMR | 2018.12.03 | YRS-AT-3101 |  Added to send email to participant for dusbursement type of TD Loan for payment method 'CHECK'
                l_DataRow_Dibursements = parameter_DataTable_Dibursement.Select(string.Format("{0} and DisbursementType='TDLOAN'", parameter_string_Expr));
                if (l_DataRow_Dibursements != null && l_DataRow_Dibursements.Length > 0)
                {
                    foreach (DataRow emailRow in l_DataRow_Dibursements)
                    {
                        emailResult = mailClient.SendLoanEmail(LoanStatus.PAID, Convert.ToString(emailRow["LoanNumber"]), Convert.ToString(emailRow["PaymentMethodCode"]), "");
                        emailRow["IsEmailSent"] = emailResult.Value;
                        if (!emailResult.Value)
                        {
                            emailRow["ReasonCode"] = emailResult.MessageList[0];
                        }
                    }
                    parameter_DataTable_Dibursement.AcceptChanges();
                }
                //END : MMR | 2018.12.03 | YRS-AT-3101 |  Added to send email to participant for dusbursement type of TD Loan for payment method 'CHECK'

				//START : ML | 2019.10.07 | YRS-AT-4676 |  Return false if error exists
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return false;
                }
				//END : ML | 2019.10.07 | YRS-AT-4676 |  Return false if error exists
                return true;
            }

            catch
            {

                if (objDisbursementFunding.Property_bool_TransactionStarted == true)
                {
                    // Removed the Extra Validation on error.
                    //&& l_bool_ErrorProcess == true
                    // Revert Updates happend before.
                    objDisbursementFunding.RevertTransactions();
                }
                throw;


            }
            finally {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "AddManualDisbursementforFunding() END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }

        }


        private bool ValidateObject(DataTable parameter_Datatable_Tovalidate, int ValidateItem, string parameter_String_DisbursmentId)
        {
            string l_string_message = String.Empty;

            string l_string_paramExpr1, l_string_paramExpr2, l_string_paramExpr3, l_string_paramExpr4;
            bool l_bool_Continue = true;

            try
            {
                l_string_paramExpr1 = string.Empty;
                l_string_paramExpr2 = string.Empty;
                l_string_paramExpr3 = string.Empty;
                l_string_paramExpr4 = string.Empty;

                switch (ValidateItem)
                {
                    case 0:
                        //ValidateItem.MainOutputFileInfo

                        if (parameter_Datatable_Tovalidate != null)
                        {
                            if (parameter_Datatable_Tovalidate.Rows.Count < 1)
                            {
                                l_string_message = "Unable to obtain (outputfile) disbursement related information in disbursement object.";
                                l_bool_Continue = false;

                            }

                        }
                        else
                        {
                            l_string_message = "Invalid object disbursementOutPutFileInfo";
                            l_bool_Continue = false;
                        }

                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "MISC";
                        l_string_paramExpr4 = l_string_message;


                        break;

                    case 1:
                        //ValidateItem.MainOutputFileAddlInfo:

                        if (parameter_Datatable_Tovalidate != null)
                        {
                            if (parameter_Datatable_Tovalidate.Rows.Count < 1)
                            {
                                l_string_message = "Unable to obtain (additional) retiree/legal entity information in disbursement object.";


                            }
                            else if (parameter_Datatable_Tovalidate.Rows[0]["Descr"].ToString().PadRight(100, ' ').Substring(0, 5) == "Error")
                            {
                                l_string_message = parameter_Datatable_Tovalidate.Rows[0]["Descr"].ToString().PadRight(100, ' ');
                                l_bool_Continue = false;
                            }

                        }
                        else
                        {
                            l_string_message = "Invalid object DisbursementOutputFileAddlInfo";
                            l_bool_Continue = false;
                        }
                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "MISC";
                        l_string_paramExpr4 = l_string_message;


                        break;

                    case 2:
                        //ValidateItem.ANNYTDOutputFileInfo:

                        if (parameter_Datatable_Tovalidate != null)
                        {
                            //							if (parameter_Datatable_Tovalidate.Rows.Count < 1) 
                            //							{
                            //								l_string_message =  "YTD Disbursement data not available in addmandisbforfund function of disbursement object.";
                            //								l_bool_Continue = false;
                            //							
                            //							}
                        }
                        else
                        {
                            l_string_message = "YTD Disbursement data not available in addmandisbforfund function of disbursement object.";
                            l_bool_Continue = false;

                        }

                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "YTD";
                        l_string_paramExpr4 = l_string_message;

                        break;

                    case 3:
                        //ValidateItem.ANNYTDWithholdingOutputFileInfo:


                        if (parameter_Datatable_Tovalidate == null)
                        {
                            l_string_message = "YTD Withholdings data not available in addmandisbforfund function of disbursement object.";
                            l_bool_Continue = false;


                        }

                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "YTDWH";
                        l_string_paramExpr4 = l_string_message;

                        break;

                    case 4:
                        // ValidateItem.ANNWithholdingOutputFileInfo:

                        if (parameter_Datatable_Tovalidate == null)
                        {

                            l_string_message = "Withholding data not available in addmandisbforfund function of disbursement object.";
                            l_bool_Continue = false;


                        }

                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "WH";
                        l_string_paramExpr4 = l_string_message;


                        break;
                    case 5:
                        //ValidateItem.AREFAccountDetailRefund:

                        if (parameter_Datatable_Tovalidate == null)
                        {
                            l_string_message = "Disbursement detail of accounts data not available in addmandisbforfund function of disbursement object.";
                            l_bool_Continue = false;

                        }

                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "MISC";
                        l_string_paramExpr4 = l_string_message;


                        break;

                    case 6:
                        //ValidateItem.REFWithholdingsRefund:

                        if (parameter_Datatable_Tovalidate == null)
                        {

                            l_string_message = "Disbursement withholding data not available in addmandisbforfund function of disbursement object.";
                            l_bool_Continue = false;


                        }

                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "WH";
                        l_string_paramExpr4 = l_string_message;


                        break;

                    case 7:
                        //ValidateItem.REFDeductions

                        if (parameter_Datatable_Tovalidate == null)
                        {

                            l_string_message = "Disbursement deduction data not available in addmandisbforfunding method of disbursement object.";
                            l_bool_Continue = false;

                        }
                        l_string_paramExpr1 = "DISBRS";
                        l_string_paramExpr2 = parameter_String_DisbursmentId;
                        l_string_paramExpr3 = "MISC";
                        l_string_paramExpr4 = l_string_message.Trim();


                        break;

                }

                if (l_string_message != String.Empty)
                {
                    //Process Log
                    //InsertDisbursementFunding
                    this.l_string_ErrorString = l_string_message;
                    YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.InsertPaymentManagerExceptionLogs(l_string_paramExpr1, l_string_paramExpr2, l_string_paramExpr3, l_string_paramExpr4);

                }
                return l_bool_Continue;
            }
            catch
            {
                //donot do anything as it is already written in the exception log.
                return false;
            }

        }

        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000 Added l_bool_ShiraM
        public bool Pay(DataTable parameter_DataTable_Dibursement, DateTime parameter_dateTimeCheckDate, bool parameter_bool_Fund, Int32 parameter_Int32_checkNoUS, Int32 parameter_Int32_checkNoCANADA, string parameter_string_AnnuityType, string parameter_string_Expr, string parameter_manualCheckNO, bool l_bool_ShiraM)
        {
            string l_string_checkNo = string.Empty;
            bool l_bool_Continue = true;
            try
            {
                //Added By SG: 2012.12.07: BT-960
                this.m_bool_ShiraM = l_bool_ShiraM;

                if (parameter_bool_Fund)
                {

                    l_bool_Continue = true;
                }
                else
                {
                    ////PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    //this.m_bool_ShiraM = l_bool_ShiraM;
                    ////END PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                    l_bool_Continue = validateNewCheckNo(parameter_Int32_checkNoUS, parameter_Int32_checkNoCANADA, parameter_string_AnnuityType, l_bool_ShiraM);
                    //if (l_bool_ShiraM)
                    //{
                    //    parameter_Int32_checkNoUS = this.l_Int32CheckNoSHIRAM;
                    //}
                }
                if (l_bool_Continue)
                {
                    //START : ML | 2019.10.22 | YRS-AT-4601 | Handle Success and failure for Northern Trust Bank file exported 
                    bool isSuccessDisbursementforFunding = false;
                    //AddManualDisbursementforFunding(parameter_DataTable_Dibursement, parameter_dateTimeCheckDate, parameter_Int32_checkNoUS, parameter_Int32_checkNoCANADA, parameter_bool_Fund, parameter_string_Expr, parameter_manualCheckNO);
                    isSuccessDisbursementforFunding = AddManualDisbursementforFunding(parameter_DataTable_Dibursement, parameter_dateTimeCheckDate, parameter_Int32_checkNoUS, parameter_Int32_checkNoCANADA, parameter_bool_Fund, parameter_string_Expr, parameter_manualCheckNO);
                    //END : ML | 2019.10.22 | YRS-AT-4601 | Handle Success and failure for Northern Trust Bank file exported 
                    l_string_checkNo = string.Empty;

                    if (parameter_string_AnnuityType == "REF")
                    {
                        YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.UpdateRefundsAfterCheckRrun();
                    }

                    if (isSuccessDisbursementforFunding == false) return false; // ML |2019.12.26 | YRS-AT-4601 |  Return False if Northern Trust Bank file not exported successfully

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                throw;

            }

        }


        public bool getDisbursementType()
        {
            DataSet l_dataset;
            DataTable l_datatable_AccountingDate;
            //DataTable l_dataTable_DisbursementsNegative =null;
            try
            {
                l_dataset = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.getDisbursementType();
                if (l_dataset == null) return false;
                l_datatable_disbursementType = l_dataset.Tables["disbursementtypes"];
                l_datatable_AccountingDate = l_dataset.Tables["AccoutDate"];
                if (l_datatable_AccountingDate != null)
                {
                    this.l_DateTime_AcctDate = System.Convert.ToDateTime(l_datatable_AccountingDate.Rows[0]["dtmEndDate"]);
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        //START: MMR | 2018.04.24 | YRS-AT-3101 | YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024) 
        #region "ReadEFTFile"
        /// <summary>
        /// Reads EFT File and return its content in datatable
        /// </summary>
        /// <param name="filePath">EFT File Address</param>
        /// <returns>Datatable with EFT file content</returns>
        public DataTable ReadEFTFile(string filePath)
        {
            //Following steps will be performed
            // 1. Validates file exists or not.
            // 2. Fetches File extension from filename and based on that calls associated format function.
            // 3. Ignores Header detail lines. Generally it is first two lines. 
            // 4. Starts reading each line by opening a loop. Generally valid line begins with 6.

            DataTable data;
            DataRow row;
            string fileExtension, line, expectedRowCode;
            int lineCount = 1;
            try
            {
                expectedRowCode = "6"; //Assigning predefined value to variable indicating valid start point to read line from file. 
                data = CreateEFTDataTable();

                //Check input parameter is empty or not
                if (!string.IsNullOrEmpty(filePath))
                {
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("ReadEFTFile : {0}", filePath));
                    //Check if file exists or not
                    if (File.Exists(filePath))
                    {
                        fileExtension = Path.GetExtension(filePath).Replace(".", string.Empty).Trim().ToLower();
                        switch (fileExtension)
                        {
                            case "dat":
                                //Defined index value indicating start position to get part of string from line
                                int indexRowCode = 0;
                                int indexEftTypeCode = 1;
                                int indexBankABANumber = 3;
                                int indexAccountNumber = 12;
                                int indexNetPayMonthTotal = 29;
                                int indexFundNo = 39;
                                int indexName = 54;
                                int indexCompanyData = 76;
                                int indexAddEndIndicator = 78;
                                int indexTraceNumber = 79;
                                int indexEFTNumber = 94;

                                using (StreamReader file = new StreamReader(filePath))
                                {
                                    //Begin reading text from file till end of text
                                    while (!file.EndOfStream)
                                    {
                                        //Store each line in a string which will be used for futher operation
                                        line = file.ReadLine();
                                        //Check line no should be greater than 2 as we need to Ignore Header details(First two lines are header details.)
                                        if (lineCount > 2)
                                        {
                                            //Check line begins with value 6. if yes then proceed further operation otherwise read from next line
                                            if (line.Substring(indexRowCode, 1).Trim() == expectedRowCode)
                                            {
                                                row = ReadEFTDATFileLine(line, expectedRowCode, indexEftTypeCode, indexBankABANumber, indexAccountNumber, indexNetPayMonthTotal, indexFundNo, indexName, indexCompanyData, indexAddEndIndicator, indexTraceNumber, indexEFTNumber, data.NewRow());
                                                //Adding row to datatable
                                                data.Rows.Add(row);
                                            }
                                        }
                                        lineCount++;
                                    }
                                }
                                break;
                            default:
                                throw new Exception("Invalid file format");
                            //break;
                        }
                    }
                    else
                    {
                        throw new Exception("File does not exists.");
                    }
                }
                else
                {
                    throw new Exception("Invalid file Path.");
                }
                return data;
            }
            catch
            {
                throw;
            }
            finally
            {
                fileExtension = null;
                line = null;
                expectedRowCode = null;
                row = null;
                data = null;

                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "ReadEFTFile END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }

        /// <summary>
        /// Returns datarow containing EFT payment details.
        /// </summary>
        /// <param name="line">Line</param>
        /// <param name="expectedRowCode">Exptected Row Code</param>
        /// <param name="indexEftTypeCode">Index value of EFT Type Code</param>
        /// <param name="indexBankABANumber">Index value of Bank ABA Number</param>
        /// <param name="indexAccountNumber">Index value of Account Number</param>
        /// <param name="indexNetPayMonthTotal">Index value of NetPayMonthTotal</param>
        /// <param name="indexFundNo">Index value of FundNo</param>
        /// <param name="indexName">Index value of Name</param>
        /// <param name="indexCompanyData">Index value of Company Data</param>
        /// <param name="indexADDENDAINDICATOR">Index value of AddEndIndicator</param>
        /// <param name="indexTRACENUMBER">Index value of Trace Number</param>
        /// <param name="indexEFTNumber">Index value of EFT Number</param>
        /// <param name="row">row</param>
        /// <returns>Datarow</returns>
        private DataRow ReadEFTDATFileLine(string line, string expectedRowCode, int indexEftTypeCode, int indexBankABANumber, int indexAccountNumber, int indexNetPayMonthTotal, int indexFundNo, int indexName, int indexCompanyData, int indexAddEndIndicator, int indexTraceNumber, int indexEFTNumber, DataRow row)
        {
            //Retreiving individual data from line and assigning it to datarow
            row["RowCode"] = expectedRowCode;
            row["EftTypeCode"] = line.Substring(indexEftTypeCode, 2);
            row["BankABANumber"] = line.Substring(indexBankABANumber, 9);
            row["AccountNumber"] = line.Substring(indexAccountNumber, 17);
            row["NetPayMonthTotal"] = line.Substring(indexNetPayMonthTotal, 10);
            row["FundIdNo"] = line.Substring(indexFundNo, 15);
            row["Name"] = line.Substring(indexName, 22);
            row["CompanyData"] = line.Substring(indexCompanyData, 2);
            row["AddEndINDICATOR"] = line.Substring(indexAddEndIndicator, 1);
            row["TraceNumber"] = line.Substring(indexTraceNumber, 15);
            row["EFTNumber"] = line.Substring(indexEFTNumber);
			// START | SR | 2018.10.05 | YRS-AT-3101 |  As per YRSOffice, Bank file will only contain Good-in-order EFT records. Hence by default all records in eft file will be approved.
            //row["IsApproved"] = (!string.IsNullOrEmpty(line.Substring(indexEFTNumber))) ? true : false;
            row["IsApproved"] = true;
			// END | SR | 2018.10.05 | YRS-AT-3101 |  As per YRSOffice, Bank file will only contain Good-in-order EFT records. Hence by default all records in eft file will be approved.
            return row;
        }

        /// <summary>
        /// Creates EFT datatable 
        /// </summary>
        /// <returns>Datatable</returns>
        private DataTable CreateEFTDataTable()
        {
            DataTable data = new DataTable(); // Intializing object 
            //Creating schema for table
            data.Columns.Add(new DataColumn("RowCode", typeof(String)));
            data.Columns.Add(new DataColumn("EftTypeCode", typeof(String)));
            data.Columns.Add(new DataColumn("BankABANumber", typeof(String)));
            data.Columns.Add(new DataColumn("AccountNumber", typeof(String)));
            data.Columns.Add(new DataColumn("NetPayMonthTotal", typeof(String)));
            data.Columns.Add(new DataColumn("FundIdNo", typeof(String)));
            data.Columns.Add(new DataColumn("Name", typeof(String)));
            data.Columns.Add(new DataColumn("CompanyData", typeof(String)));
            data.Columns.Add(new DataColumn("AddEndINDICATOR", typeof(String)));
            data.Columns.Add(new DataColumn("TraceNumber", typeof(String)));
            data.Columns.Add(new DataColumn("EFTNumber", typeof(String)));
            data.Columns.Add(new DataColumn("IsApproved", typeof(bool)));
            return data;
        }

        /// <summary>
        /// Validates File Extention and its name
        /// Validates Persons in the file as compared to persons actually in the batch.
        /// Validates Mix and match persons in the shared bank file.
        /// </summary>
        /// <param name="fileName">EFT File</param>
        /// <returns>True / False, If False then with error message code</returns>
        public ReturnObject<bool> IsValidBankEFTFile(string fileName, DataTable eftDisbursementsPendingForApproval)
        {
            ReturnObject<bool> result = new ReturnObject<bool>();  // SR | 2018.10.05 | YRS-AT-3101 | initialise object at start only.
            string messageInvalidFile;            
            string fullFileName;
            string firstPartFileName;
            string thirdPartFileName;
            string[] filePathInArray, tempFileNameHolder ;
            bool flag;
            // START | SR | 2018.10.05 | YRS-AT-3101 | define variable to Validate Person in batch.            
            string searchTextfordifferentBatch = string.Empty;
            string batchIdFromFileName = string.Empty;
            bool isImportedBatchProcessed = false;
            bool isBatchIDNotExist = false; //VC | 2018.11.16 | YRS-AT-3101 | declared boolean variable to store batch id existing or not
            string messageInvalidFileHeader = string.Empty; ;
            DataTable fileData;
            DataTable disbursementTable;           
            DataRow[] rows;
            result.MessageList = new List<string>();           
            bool validEFTFileHeader = false;
            Dictionary<string, string> messageParameters = new Dictionary<string, string>();
            string fundIdNotInImportedBatch = string.Empty;
            string fundIdInOtherBatch = string.Empty;            
            // END | SR | 2018.10.05 | YRS-AT-3101 | define variable to Validate Person in batch.
            DataSet validateImportedBatch; //VC | 2018.11.16 | YRS-AT-3101 | Declared dataset
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("IsValidBankEFTFile START"));
                flag = false;                
                filePathInArray = fileName.Split('\\');
                fullFileName = filePathInArray[filePathInArray.Length - 1];
                if (fullFileName.ToUpper().EndsWith(".DAT"))
                {
                    tempFileNameHolder = fullFileName.Split('_');
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("Imported Bank EFT File Name : {0}", fullFileName));
                    // Validate - EFT File Name format
                    if (tempFileNameHolder.Length == 3)
                    {
                        firstPartFileName = tempFileNameHolder.GetValue(0).ToString();
                        thirdPartFileName = tempFileNameHolder.GetValue(2).ToString().ToUpper().Replace(".DAT", "");
                        if (!(firstPartFileName.Equals("EFT") && thirdPartFileName.Length >= 9))
                        {
                            messageInvalidFile = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_INVALID_FILE").DisplayText;
                            result.MessageList.Add(messageInvalidFile);
                        }
                    }
                    else
                    {
                        messageInvalidFile = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_INVALID_FILE").DisplayText;
                        result.MessageList.Add(messageInvalidFile);
                    }

                    // Validate - EFT File header
                    validEFTFileHeader = ValidateEFTFileHeader(fileName);
                    if (!validEFTFileHeader)
                    {
                        messageInvalidFileHeader = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_INVALID_EFT_FILE_HEADER").DisplayText;
                        result.MessageList.Add(messageInvalidFileHeader);
                    }

                    // SR | 2018.10.05 | YRS-AT-3101 | Validate Person in batch.
                                      
                    disbursementTable = YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass.GetEFTNonPendingDisbursements(DisbursementType);
                    batchIdFromFileName = GetBatchIdFromFileName(fileName);
                    //START: VC | 2018.11.16 | YRS-AT-3101 | Fetching batch file status from dataset to variables
                    validateImportedBatch = YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass.GetImportedBatchFileStatus(batchIdFromFileName);
                    if (validateImportedBatch != null && validateImportedBatch.Tables[0] != null)
                    {
                        isImportedBatchProcessed = Convert.ToBoolean(validateImportedBatch.Tables[0].Rows[0]["IsBatchProcesssed"]);
                        isBatchIDNotExist = Convert.ToBoolean(validateImportedBatch.Tables[0].Rows[0]["IsBatchIDNotExist"]);                  
                    }
                    //END: VC | 2018.11.16 | YRS-AT-3101 | Fetching batch file status from dataset to variables
                    if (isImportedBatchProcessed)
                    {
                        messageInvalidFileHeader = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_INVALID_EFT_FILE_PROCESSED").DisplayText;
                        result.MessageList.Add(messageInvalidFileHeader);
                    }
                    //START: VC | 2018.11.16 | YRS-AT-3101 | If batch id not exist then save validation message into message list
                    if (isBatchIDNotExist)
                    {
                        messageInvalidFileHeader = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_LOAN_EFT_INVALID_BATCHID").DisplayText;
                        result.MessageList.Add(messageInvalidFileHeader);
                    }
                    //END: VC | 2018.11.16 | YRS-AT-3101 | If batch id not exist then save validation message into message list

                    if (!(string.IsNullOrEmpty (batchIdFromFileName)))
                    {
                        fileData = ReadEFTFile(fileName);
                        if (HelperFunctions.isNonEmpty(fileData))
                        {
                            foreach (DataRow dtRow in fileData.Rows)
                            {
                                // Validate - Mix and match persons in the shared bank file.
                                searchTextfordifferentBatch = String.Format("FundIdNo = '{0}' AND BatchId <> '{1}'", dtRow["FundIdNo"].ToString().Trim(), batchIdFromFileName);
                                rows = disbursementTable.Select(searchTextfordifferentBatch);
                                if (rows.Length > 0)
                                {
                                    if (string.IsNullOrEmpty(fundIdInOtherBatch))
                                    {
                                        fundIdInOtherBatch = dtRow["FundIdNo"].ToString().Trim();
                                    }
                                    else
                                    {
                                        fundIdInOtherBatch = fundIdInOtherBatch + "," + dtRow["FundIdNo"].ToString().Trim();
                                    }

                                }
                            }

                            if (!(string.IsNullOrEmpty(fundIdInOtherBatch)))
                            {
                                messageParameters.Clear();
                                messageParameters.Add("FundIdNo", fundIdInOtherBatch);
                                result.MessageList.Add(YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_INVALID_PERSON_IN_BATCH", messageParameters).DisplayText);
                            }
                      } 
                }
                    // END | SR | 2018.10.05 | YRS-AT-3101 | Validate Person in batch.
            }
            else
            {
                    messageInvalidFile = YMCARET.YmcaBusinessObject.MetaMessageBO.GetMessageByCode("MESSAGE_PM_INVALID_FILE").DisplayText;
                    result.MessageList.Add(messageInvalidFile);
            }
               
            if (result.MessageList.Count > 0)
            {
                    flag = false;
            }
            else 
            { 
                    flag = true; 
            }                               
                
            result.Value = flag;
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("Imported Bank EFT File is Valid : {0}", flag.ToString()));                               
            return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                messageInvalidFile = null; //  SR | 2018.10.05 | YRS-AT-3101 | set message variables to null 				
                fullFileName = null;
                firstPartFileName = null;
                thirdPartFileName = null;
                filePathInArray= null;
                tempFileNameHolder = null;
                result = null;                
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "ImportBankEFTFile END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }
               
        /// <summary>
        /// validate EFT File header 
        /// </summary>
        /// <param name="filePath">EFT File Address</param>
        /// <returns>boolean</returns>
        public bool ValidateEFTFileHeader(string filePath)
        {
            //Following steps will be performed
            // 1. Validates file exists or not.
            // 2. Fetches File extension from filename and based on that calls associated format function.
            // 3. Validate Header detail lines. Generally it is first two lines. 
                        
            bool isValidateEFTFileHeader = false;
            string fileExtension, line, expectedRowCodeForLine1,expectedRowCodeForLine2;
            int lineCount = 1;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("ValidateEFTFileHeader START"));
                expectedRowCodeForLine1 = "1";
                expectedRowCodeForLine2 = "5"; //Assigning predefined value to variable indicating valid start point to read line from file. 
                
                //Check input parameter is empty or not
                if (!string.IsNullOrEmpty(filePath))
                {
                    //Check if file exists or not
                    if (File.Exists(filePath))
                    {
                        fileExtension = Path.GetExtension(filePath).Replace(".", string.Empty).Trim().ToLower();
                        switch (fileExtension)
                        {
                            case "dat":
                                //Defined index value indicating start position to get part of string from line
                                int indexRowCode = 0;
                                int indexCompanyEntryDescription = 53;

                                using (StreamReader file = new StreamReader(filePath))
                                {
                                    //Begin reading text from file till end of text
                                    while (!file.EndOfStream)
                                    {
                                        //Store each line in a string which will be used for futher operation
                                        line = file.ReadLine();
                                        if (lineCount == 1)
                                        {
                                            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("Imported Bank EFT File First Line has  : {0}", line.Substring(indexRowCode, 1).Trim()));
                                            if (line.Substring(indexRowCode, 1).Trim() == expectedRowCodeForLine1)
                                            {
                                                isValidateEFTFileHeader = true;
                                            }
                                            else
                                            { 
                                                isValidateEFTFileHeader = false;
                                                break;
                                            }
                                        }
                                        //Check line no should be greater than 2 as we need to Ignore Header details(First two lines are header details.)
                                        if (lineCount == 2)
                                        {
                                            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("Imported Bank EFT File second Line has  : {0}", line.Substring(indexRowCode, 1).Trim()));
                                            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("Imported Bank EFT File second Line has  company description as : {0}", line.Substring(indexCompanyEntryDescription, 10)));
                                            //Check line begins with value 6. if yes then proceed further operation otherwise read from next line
                                            if (line.Substring(indexRowCode, 1).Trim() == expectedRowCodeForLine2)
                                            {
                                                if (line.Substring(indexCompanyEntryDescription, 10) == EFTHeader)
                                                {
                                                    isValidateEFTFileHeader = true;
                                                }
                                                else { isValidateEFTFileHeader = false; }
                                            }
                                            else
                                            {
                                                isValidateEFTFileHeader = false;
                                            }
                                            
                                            break;
                                        }
                                        lineCount++;
                                    }
                                }
                                break;                           
                        }
                    }
                    else
                    {
                        isValidateEFTFileHeader = false;
                    }
                }
                else
                {
                    isValidateEFTFileHeader = false;
                }

                return isValidateEFTFileHeader;
            }
            catch
            {
                isValidateEFTFileHeader = false;
                throw;
            }
            finally
            {
                fileExtension = null;
                line = null;
                expectedRowCodeForLine1 = null;
                expectedRowCodeForLine2 = null;                  
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("ValidateEFTFileHeader END"));
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }

		// START | SR | 2018.10.05 | YRS-AT-3101 | Get BatchId from EFT Bank file.
        public string GetBatchIdFromFileName(string sourceFilePath)
        {
            string[] filePathInArray;            
            string fileName = string.Empty;           
            string [] tempFileNameHolder;
            string thirdPartFileName = string.Empty;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("GetBatchIdFromFileName START"));
                if (!sourceFilePath.Equals(string.Empty))
                {
                    filePathInArray = sourceFilePath.Split(new[] { "\\" }, StringSplitOptions.None);
                    fileName = filePathInArray[filePathInArray.Length - 1];
                    fileName = fileName.ToString().ToUpper().Replace(".DAT", "");
                    tempFileNameHolder = fileName.Split('_');
                    if (tempFileNameHolder.Length == 3)
                    {
                        thirdPartFileName = tempFileNameHolder.GetValue(2).ToString().ToUpper();
                    }
                }

                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("BatchId From FileName:", thirdPartFileName));
                return thirdPartFileName;
            }
            catch (Exception ex)
            {
                CommonClass.LogException("GetBatchIdFromFileName()", ex);
                return fileName;
            }
            finally 
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", String.Format("GetBatchIdFromFileName END"));
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }
		// END | SR | 2018.10.05 | YRS-AT-3101 | Get BatchId from EFT Bank file

        #endregion "ReadEFTFile"
        //END: MMR | 2018.04.24 | YRS-AT-3101 | YRS enh: EFT: Loans ( FIRST EFT PROJECT) - updates for "Payment Manager" (TrackIT 33024)

        #region "Properties"
        public DateTime DateTime_AcctDate
        {
            get
            {
                return l_DateTime_AcctDate;
            }
            set
            {
                l_DateTime_AcctDate = value;
            }
        }
        public bool bool_NegetivePayments
        {
            get
            {
                return l_bool_NegetivePayments;
            }
            set
            {
                l_bool_NegetivePayments = value;
            }
        }

        public Int32 Int32CheckNoCanada
        {
            get
            {
                return l_Int32CheckNoCanada;
            }
            set
            {
                l_Int32CheckNoCanada = value;
            }
        }

        public Int32 Int32CheckNoPayrol
        {
            get
            {
                return l_Int32CheckNoPayrol;
            }
            set
            {
                l_Int32CheckNoPayrol = value;
            }
        }
        public Int32 Int32CheckNoTDLoan
        {
            get
            {
                return l_Int32CheckNoTDLoan;
            }
            set
            {
                l_Int32CheckNoTDLoan = value;
            }
        }

        public Int32 Int32CheckNoEXP
        {
            get
            {
                return l_Int32CheckNoEXP;
            }
            set
            {
                l_Int32CheckNoEXP = value;
            }
        }
        public Int32 Int32CheckNoRefund
        {
            get
            {
                return l_Int32CheckNoRefund;
            }
            set
            {
                l_Int32CheckNoRefund = value;
            }
        }
        //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        public Int32 Int32CheckNoSHIRAM
        {
            get
            {
                return l_Int32CheckNoSHIRAM;
            }
            set
            {
                l_Int32CheckNoSHIRAM = value;
            }
        }
        //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

        public DataTable datatable_Disbursements
        {
            get
            {
                return l_datatable_Disbursements;
            }
            set
            {
                l_datatable_Disbursements = value;
            }
        }

        public DataTable datatable_ANNdDisbursements
        {
            get
            {
                return l_datatable_ANNDisbursements;
            }
            set
            {
                l_datatable_ANNDisbursements = value;
            }
        }

        public DataTable datatable_REFdDisbursements
        {
            get
            {
                return l_datatable_REFDisbursements;
            }
            set
            {
                l_datatable_REFDisbursements = value;
            }
        }

        //property added by ruchi to add the TDloan table
        public DataTable datatable_TDLoanDisbursements
        {
            get
            {
                return l_datatable_TDLoanDisbursements;
            }
            set
            {
                l_datatable_TDLoanDisbursements = value;
            }
        }

        //property added by Aparna to add the EXP Dividends table - 16/10/2006
        public DataTable datatable_EXPDisbursements
        {
            get
            {
                return l_datatable_EXPDisbursements;
            }
            set
            {
                l_datatable_EXPDisbursements = value;
            }
        }

        public DataTable datatable_ANNDisbursementsREPL
        {
            get
            {
                return l_datatable_ANNDisbursementsREPL;
            }
            set
            {
                l_datatable_ANNDisbursementsREPL = value;
            }
        }

        public DataTable datatable_REFdDisbursementsREPL
        {
            get
            {
                return l_datatable_REFDisbursementsREPL;
            }
            set
            {
                l_datatable_REFDisbursementsREPL = value;
            }
        }

        public DataTable datatable_TDLoanDisbursementsREPL
        {
            get
            {
                return l_datatable_TDLoanDisbursementsREPL;
            }
            set
            {
                l_datatable_TDLoanDisbursementsREPL = value;
            }
        }

        public DataTable datatable_EXPDisbursementsREPL
        {
            get
            {
                return l_datatable_EXPDisbursementsREPL;
            }
            set
            {
                l_datatable_EXPDisbursementsREPL = value;
            }
        }

        public DataTable datatable_DisbursementsREPL
        {
            get
            {
                return l_datatable_DisbursementsREPL;
            }
            set
            {
                l_datatable_DisbursementsREPL = value;
            }
        }

        public DataTable datatable_Withheld
        {
            get
            {
                return l_datatable_Withheld;
            }
            set
            {
                l_datatable_Withheld = value;
            }
        }

        //Phase V Changes-start
        public DataTable datatable_DWDisbursement
        {
            get
            {
                return l_datatable_DWDisbursement;
            }
            set
            {
                l_datatable_DWDisbursement = value;
            }
        }
        public DataTable datatable_HWDisbursement
        {
            get
            {
                return l_datatable_HWDisbursement;
            }
            set
            {
                l_datatable_HWDisbursement = value;
            }
        }
        //Phase V Changes-end



        public DataTable datatable_disbursementType
        {
            get
            {
                return l_datatable_disbursementType;
            }
            set
            {
                l_datatable_disbursementType = value;
            }
        }

        public DateTime DateTime_CheckDate
        {
            get
            {
                return l_DateTime_CheckDate;
            }
        }


        public string string_ErrorString
        {
            get
            {
                return l_string_ErrorString;
            }
            set
            {
                l_string_ErrorString = value;
            }
        }

        // START : SR | 2018.05.25 | YRS-AT-3101 |  Defined property to get EFT Header.
        string eftHeader;
        public string EFTHeader
        {
            get
            {
                return eftHeader;
            }
            set
            {
                eftHeader = value;
            }
        }

         // SR | 2018.05.25 | YRS-AT-3101 |  Defined property to get EFT Header.
        string disbursementType;
        public string DisbursementType
        {
            get
            {
                return disbursementType;
            }
            set
            {
                disbursementType = value;
            }
        }
        // END : SR | 2018.05.25 | YRS-AT-3101 |  Defined property to get EFT Header.

        // START | SR | 2018.10.05 | YRS-AT-3101 | defined to Get EFT BatchId.
        string eFTBatchId = "";
        public string EFTBatchId
        {
            get { return eFTBatchId; }
            set { eFTBatchId = value; }
        }
        // END | SR | 2018.10.05 | YRS-AT-3101 | defined to Get EFT BatchId.
        public List<string> BatchList = new List<string>();  // SR | 2018.10.05 | YRS-AT-3101 | defined to Get selected BatchId's List from UI.

        // START : ML | 2019.10.22 | YRS-AT-4601 |  Defined property to check Payment Outsourcing is live or not
        bool isPaymentOutSourcingKeyON;
        public bool  IsPaymentOutSourcingKeyON
        {
            get
            {
                return isPaymentOutSourcingKeyON;
            }
            set
            {
                isPaymentOutSourcingKeyON = value;
            }
        }
        // END : ML | 2019.10.22 | YRS-AT-4601 |  Defined property to check Payment Outsourcing is live or not
        #endregion "Properties"

        #region "Payment Method EFT"      
        // START : SR | 2018.04.10 | YRS-AT-3101 | Get EFT disbursement type.
        /// <summary>
        ///This method will return disbursement type for EFT payment method.
        /// </summary>       
        /// <returns>DataSet</returns>        
        public DataSet GetEFTDisbursementType()
        {
            return YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.GetEFTDisbursementType();
        }
        // END : SR | 2018.04.10 | YRS-AT-3101 | Get EFT disbursement type.

        // START : SR | 2018.04.12 | YRS-AT-3101 | This method is Copy of PAY() method and will only handle disbursement with EFT payment method.
        /// <summary>
        ///This method will call method to generate EFT file(s).
        /// </summary>
        /// <param name="Disbursement">Disbursement</param>
        /// <param name="CheckDate">CheckDate</param>
        /// <param name="FilterExpression">FilterExpression</param>
        /// <returns>True/False</returns>
        ///
        public bool Pay_EFT(DataTable Disbursement, DateTime CheckDate, string FilterExpression, string moduleName)
        {
            return GenerateEFTFiles(Disbursement, CheckDate, FilterExpression, moduleName);
        }
        // END : SR | 2018.04.12 | YRS-AT-3101 | This method is Copy of PAY() method and will only handle disbursement with EFT payment method.

        // START : SR | 2018.04.12 | YRS-AT-3101 | This method is Copy of AddManualDisbursementforFunding() method and will only handle disbursement with EFT payment method.
        /// <summary>
        ///This method will generate EFT file(s) in given detination path, Update EFT disbursement details(EFT Batchid & EFT status).
        /// </summary>
        /// <param name="Disbursement">Disbursement</param>
        /// <param name="CheckDate">CheckDate</param>
        /// <param name="FilterExpression">FilterExpression</param>
        /// <returns>True/False</returns>
        private bool GenerateEFTFiles(DataTable Disbursement, DateTime EFTDate, string FilterExpression, string moduleName)
        {
         
            DataTable disbursementOutputFileInfo = null;
            DataTable disbursementOutputFileAddlInfo = null;
            DataTable ytdDisbursementOutputFileInfo = null;
            DataTable ytdWithholdingOutputFileInfo = null;
            DataTable withholdingsByDisbursement = null;
            DataTable disbursementDetailsbyRefundDisbursement = null;
            DataTable disbursementWithholdingsbyRefundDisbursement = null;
            DataTable disbursementDeductions = null;
            DataTable withholdingsByTDLoan = null;
            YMCARET.YmcaBusinessObject.PaymentManagerBOWraperClass paymentBusinessRulesWrapper = new YMCARET.YmcaBusinessObject.PaymentManagerBOWraperClass();
            YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass paymentDBAccessWrapper = new YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass();
            string persid;           
            string disbursementType;
            DateTime effectiveYear;
            string disbursementId = string.Empty;
            string disbursementNumber = string.Empty;
            string instrumentTypeCode = string.Empty; ;
            double netAmount = 0.00;
            DataRow dataRow = null;
            DataSet disbursal = null;
            DataRow[] disbursementRows;
            bool errorProcess = false;
            string errorMsg = string.Empty;
            DataTable auditTable = null;
            string eftBuissnessDate = string.Empty;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("paymnet Manager", "GenerateEFTFiles START");
                l_string_ErrorString = string.Empty;

                // filter disbursementd based on payment method
                paymentBusinessRulesWrapper.IdentifyPaymentTypes(Disbursement, FilterExpression);
                // Get year for EFT file creation
                effectiveYear = EFTDate;

                disbursal = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.GetOutPutFileforDisbursal();

                EFTBatchId = Convert.ToString(YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass.GetNextEFTBatchId().Tables[0].Rows[0][0]);
                eftBuissnessDate = EFTBatchId.Substring(0, 4) + "/" + EFTBatchId.Substring(4, 2) +"/"+ EFTBatchId.Substring(6, 2);
                paymentBusinessRulesWrapper.EFTBuissnessDate  = Convert.ToDateTime(eftBuissnessDate);


                paymentBusinessRulesWrapper.EFTBatchId = EFTBatchId;
                // Get Disbursement type for EFT file naming
                paymentBusinessRulesWrapper.DisbursementType = DisbursementType;
                // Set Payment Method for EFT file creation
                paymentBusinessRulesWrapper.PaymentMethod = PaymentMethod.EFT;
                // Get out put file details.
                paymentBusinessRulesWrapper.ProduceOutputFilesOutput(disbursal.Tables["OutPutfilePath"]);
                // Get EFT Header
                paymentBusinessRulesWrapper.CompanyEntryDesc = EFTHeader;
                // Create header for EFT file.
                paymentBusinessRulesWrapper.ProduceOutputFilesCreateHeader();
                // Add column status for auditing purpose.
                if (!Disbursement.Columns.Contains("Status"))
                    Disbursement.Columns.Add("Status", typeof(string));
                // get user selected records only                 
                disbursementRows = Disbursement.Select(FilterExpression);
                // Copy batch file before processing for auditing purpose.
                auditTable = Disbursement.Clone();
                auditTable = AuditTable(disbursementRows, auditTable);
                MaintainEFTFileBatch(auditTable, paymentBusinessRulesWrapper.EFTBatchId, moduleName);
                // loopt through each disbursements for EFT file generation.
                foreach (DataRow row in disbursementRows)
                {
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("paymnet Manager", "START : Process EFT For " + row["PersId"].ToString().Trim());
                    disbursementId = row["DisbursementID"].ToString().Trim();
                    persid = row["PersId"].ToString().Trim();
                    disbursementType = row["DisbursementType"].ToString().Trim();

                    //	Read current active address at time of disbursement funding -Start
                    YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.UpdateDisbursementAddressID(disbursementId);

                    disbursementOutputFileInfo = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementOutputFileInfo(disbursementId);

                    if (ValidateObject(disbursementOutputFileInfo, (int)ValidateMode.MainOutputFileInfo, disbursementId) == false)
                    {
                        errorProcess = true;
                        throw new Exception(this.l_string_ErrorString);
                    }

                    disbursementOutputFileAddlInfo = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementOutputFileAddlInfo(disbursementId);
                    if (ValidateObject(disbursementOutputFileAddlInfo, (int)ValidateMode.MainOutputFileAddlInfo, disbursementId) == false)
                    {
                        errorProcess = true;
                        throw new Exception(this.l_string_ErrorString);

                    }
                    dataRow = disbursementOutputFileInfo.Rows[0];

                    switch (dataRow["DisbursementType"].ToString().ToUpper().Trim())
                    {
                        case "TDLOAN":
                            withholdingsByTDLoan = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.WithholdingDetailByTDLoanDisbursement(disbursementId);

                            if (ValidateObject(withholdingsByTDLoan, (int)ValidateMode.REFAccountDetailRefund, disbursementId) == false)
                            {
                                errorProcess = true;
                                break;
                            }

                            disbursementDetailsbyRefundDisbursement = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.DisbursementDetailsbyRefundDisbursement(disbursementId);
                            if (ValidateObject(disbursementDetailsbyRefundDisbursement, (int)ValidateMode.REFAccountDetailRefund, disbursementId) == false)
                            {
                                errorProcess = true;
                                break;
                            }

                            break;
                    }


                    if (errorProcess)
                    {
                        throw new Exception(this.l_string_ErrorString);

                    }

                    if (dataRow["PaymentMethodCode"].GetType().ToString() != "System.DBNull")
                    {
                        if (dataRow["PaymentMethodCode"].ToString().Trim().ToUpper() == "EFT")
                            instrumentTypeCode = "E";
                        else if (dataRow["PaymentMethodCode"].ToString().Trim().ToUpper() == "ACH")
                            instrumentTypeCode = "E";

                    }

                    paymentBusinessRulesWrapper.PreparOutPutFileData(disbursal,
                            disbursementOutputFileInfo,
                            disbursementOutputFileAddlInfo,
                            ytdDisbursementOutputFileInfo,
                            ytdWithholdingOutputFileInfo,
                            withholdingsByDisbursement,
                            disbursementDetailsbyRefundDisbursement,
                            disbursementWithholdingsbyRefundDisbursement,
                            disbursementDeductions,
                            withholdingsByTDLoan,
                            disbursementId, effectiveYear, disbursementNumber, false);

                    netAmount = paymentBusinessRulesWrapper.Property_double_NetBalance;
                                        
                   paymentDBAccessWrapper.UpdateEFTDisbursementDetails(disbursementId, paymentBusinessRulesWrapper.EFTBatchId);
                   row["Status"] = LoanStatus.PROOF; // Update Status to PROOF mode
                  
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("paymnet Manager", "END : Process EFT For " + row["PersId"].ToString().Trim());
                }
                // create footer for EFT file.
                paymentBusinessRulesWrapper.ProduceOutputFilesCreateFooters();
                paymentBusinessRulesWrapper.CloseOutputStreamFiles();
                paymentDBAccessWrapper.InsertDisbursementFileTypes(disbursal);
                this.l_datatable_FileList = paymentBusinessRulesWrapper.DataTableNameFileList;
                // Copy batch file for auditing purpose
                auditTable.Clear();
                auditTable = AuditTable(disbursementRows, auditTable);
                MaintainEFTFileBatch(auditTable, paymentBusinessRulesWrapper.EFTBatchId, moduleName);
                return true;
            }

            catch
            {
                if (paymentDBAccessWrapper.Property_bool_TransactionStarted == true)
                {
                    // Removed the Extra Validation on error.
                    //&& errorProcess == true
                    // Revert Updates happend before.
                    paymentDBAccessWrapper.RevertTransactions();
                }
                throw;
            }
            finally 
            {

                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("paymnet Manager", "GenerateEFTFiles END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }
        

        // START : SR | 2018.04.12 | YRS-AT-3101 | This method is Copy of PAY() method and will only handle disbursement with EFT payment method for Approval or rejection process.
        /// <summary>
        /// Marks EFT payments as Approve or Reject.
        /// </summary>
        /// <param name="disbursement">EFT Disbursements</param>
        /// <param name="processDate">Process start date</param>
        /// <returns>EFT Disbursements with completion flags and error message for failed disbursements</returns>        
       public DataTable SetEFTDisbursementPaymentStatus(DataTable disbursement, DateTime processDate, string moduleName)
        {   
            string disbursementEFTID, persBankingEFTID, disbursementID, bankID;            
            bool isApproved;
            double netAmount;
            string disbursementType;            
            DataSet disbursementData;                        
            string loanNo = "0";            
            YMCAObjects.ReturnObject<bool> result;
			MailUtil mailClient = new MailUtil();
            DataRow[] disbursementRows;
           ReturnObject<bool> emailResult;
           DataTable auditTable;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("paymnet Manager", "SetEFTDisbursementPaymentStatus START");               
                disbursementData = new DataSet();                
                disbursementData.Tables.Add(CommonClass.DeepCopy<DataTable> (disbursement));
                // record EFT disbursements details into atsBatchCreationLogs table for auditing purpose                
                disbursementRows = disbursementData.Tables[0].Select("BatchId = '" + BatchList[0] + "'");
                auditTable = disbursementData.Tables[0].Clone();
                auditTable = AuditTable(disbursementRows, auditTable);
                MaintainEFTFileBatch(auditTable, BatchList[0], moduleName);
              
                try // To preserve the log of disbursementData extra try catch block is placed
                {                    
                    // loop through each disbursements for EFT file generation.
                    foreach (DataRow drDibursement in disbursementData.Tables[0].Rows)
                    {
                       if (BatchList.Contains(drDibursement["BatchId"].ToString())) // SR | 2018.10.05 | YRS-AT-3101 | Perform approve or reject process only for selected Batches.
                       {
                           YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "START Approval/Rejection for disbursement " + drDibursement["DisbursementID"].ToString().Trim());       
                           drDibursement["SelectedBatchId"] = true; //  SR | 2018.10.11 | YRS-AT-3101 |This is required to get count of Approved and rejected EFT records.
                        disbursementType = drDibursement["DisbursementType"].ToString().ToUpper().Trim();
                        switch (disbursementType)
                        {
                            case "TDLOAN":
                                disbursementEFTID = drDibursement["DisbursementEFTID"].ToString().Trim();
                                persBankingEFTID = drDibursement["PersBankingEFTID"].ToString().Trim();
                                disbursementID = drDibursement["DisbursementID"].ToString().Trim();
                                bankID = drDibursement["BankId"].ToString().Trim();
                                netAmount = Convert.ToDouble(drDibursement["NetAmount"].ToString().Trim());
                                loanNo = drDibursement["LoanNumber"].ToString().Trim();
                                if (string.IsNullOrEmpty(drDibursement["Selected"].ToString().Trim()))
                                {
                                    isApproved = false;
                                }
                                else
                                {
                                    isApproved = Convert.ToBoolean(drDibursement["Selected"]);
                                }

                                if (isApproved)
                                {
                                    result = YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass.ApproveEFTPayment(disbursementEFTID, persBankingEFTID, bankID, disbursementID, netAmount); //, drDibursement);
                                    drDibursement["IsDatabaseUpdated"] = result.Value;
                                    if (!result.Value)
                                    {
                                        drDibursement["ReasonCode"] = result.MessageList[0];

                                    }
									// START | SR | 2018.10.05 | YRS-AT-3101 | Send mail to participant if Approve process is succeed.
                                    else 
                                    {
                                        //	Send Email to Participant. If application fails to send email due to invalid emaill address, error message will be displayed on screen.  
                                        try
                                        {
                                            emailResult = mailClient.SendLoanEmail(LoanStatus.PAID, loanNo, PaymentMethod.EFT, "");
                                            drDibursement["IsEmailSent"] = emailResult.Value;
                                            if (!emailResult.Value)
                                            {
                                                drDibursement["ReasonCode"] = emailResult.MessageList[0];                                                
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            drDibursement["IsEmailSent"] = false;
                                            CommonClass.LogException("EFTApprovalOrRejectionProcess()", ex);
                                            drDibursement["ReasonCode"] = " MESSAGE_PM_ERROR_APPROVAL_EMAIL";
                                            ex = null;
                                        }
                                    }
									// END | SR | 2018.10.05 | YRS-AT-3101 | Send mail to participant if Approve process is succeed.
                                }
                                else
                                {
                                    result = YMCARET.YmcaDataAccessObject.PaymentManagerDAClassWrapperClass.RejectEFTPayment(disbursementEFTID, persBankingEFTID, bankID, disbursementID); //, drDibursement);
                                    drDibursement["IsDatabaseUpdated"] = result.Value;
                                    if (!result.Value)
                                    {
                                        drDibursement["ReasonCode"] = result.MessageList[0];
                                    }
                                    else
                                    {
                                        if (result.MessageList.Contains("AllowedPaymentIterationNotCrossed"))
                                        {
                                            //Send Email to participant with a copy (Cc) to customer service. If application fails to send email then the error message will be displayed on screen.                                                                        
                                            try
                                            {
                                                emailResult = mailClient.SendLoanEmail(LoanStatus.REJECTED, loanNo, PaymentMethod.EFT, "");
                                                drDibursement["IsEmailSent"] = emailResult.Value;
                                                if (!emailResult.Value)
                                                {
                                                    drDibursement["ReasonCode"] = emailResult.MessageList[0];
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                drDibursement["IsEmailSent"] = false;
                                                CommonClass.LogException("EFTApprovalOrRejectionProcess()", ex);
                                                drDibursement["ReasonCode"] = "MESSAGE_PM_ERROR_REJECTION_EMAIL";
                                                ex = null;
                                            }
                                        }
                                        else 
                                        {
                                            drDibursement["IsEmailSent"] = true;
                                        }
                                       
                                    }
                                }
                                break;
                        }
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "END Approval/Rejection for disbursement " + drDibursement["DisbursementID"].ToString().Trim());       
                    }
                    } // START | SR | 2018.10.05 | YRS-AT-3101 |end of batchlist condition
                }
                catch (Exception ex)
                {
                    CommonClass.LogException("SetEFTDisbursementPaymentStatus()", ex);
                    ex = null;
                }
                // record EFT disbursements details into atsBatchCreationLogs table for auditing purpose after approval/rejection process
                auditTable.Clear();
                disbursementRows = disbursementData.Tables[0].Select("BatchId = '" + BatchList[0] + "'");
                auditTable = AuditTable(disbursementRows, auditTable);
                MaintainEFTFileBatch(auditTable, BatchList[0], moduleName);

                // record EFT disbursements details into atsBatchCreationLogs table for auditing purpose after approval/rejection process
                return disbursementData.Tables[0];
            }
            catch (Exception ex)
            {
                CommonClass.LogException("SetEFTDisbursementPaymentStatus()", ex);
                throw ex;
            }
            finally
            {
                disbursementEFTID = null;
                persBankingEFTID = null;
                disbursementID = null;
                bankID = null;
                disbursementType = null;                
                result = null;
                disbursementData = null;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payment Manager", "SetEFTDisbursementPaymentStatus END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }

        public static DataTable GetEFTDisbursements(string type, string status)
        {
            return YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.GetEFTDisbursements(type, status);
        }
        //END: PPP | 04/11/2018 | YRS-AT-3101 | Provides EFT disbursement details based on given status
        //START : SR | 10/16/2018 | YRS-AT-3101 | Copy EFT batch records in auditing table.

        private DataTable AuditTable(DataRow[] auditRecords, DataTable auditTable)
        {            
            try
            {
                foreach (DataRow dr in auditRecords)
                {
                    auditTable.Rows.Add(dr.ItemArray);
                }

                return auditTable;
            }
            catch (Exception ex)
            {
                CommonClass.LogException("PMEFTLoanProcessing_AuditTable", ex);
                return auditTable;
            }
            finally
            {              
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PMEFTLoanProcessing", "MaintainEFTFileBatch END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }

        private static void MaintainEFTFileBatch(DataTable batchList, string batchID, string moduleName)
        {            
            string xmlData;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PMEFTLoanProcessing", "MaintainEFTFileBatch START");              
                xmlData = HelperFunctions.ConvertToXML(batchList);
                YMCARET.YmcaBusinessObject.MRDBO.InsertAtsTemp(batchID, moduleName, xmlData);                
            }
            catch (Exception ex)
            {
                CommonClass.LogException("PMEFTLoanProcessing_MaintainEFTFileBatch", ex);
            }
            finally
            {
                moduleName = null;
                xmlData = null;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("PMEFTLoanProcessing", "MaintainEFTFileBatch END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
        }
        //END : SR | 10/16/2018 | YRS-AT-3101 | Copy EFT batch records in auditing table.
        #endregion
    }

    public class PaymentManagerBOWraperClass
    {
        // Payee data varaibles
        #region "Constants"
        // Constants fro OUTPUT.H file.
        const string C_BATCHCOUNT = "000001";
        const string C_BATCHNUMBER = "0000001";
        const string C_BLOCKINGFACTOR = "10";
        const string C_COMPANYENTRYDESC = "ANNUITY_PY";
        const string C_COMPANYID = "1135562401";
        const string C_COMPANYNAME = "YMCA Retirement Fund             ";
        const string C_FILEFORMATCODE = "1";
        const string C_FILEIDMODIFIER = "A";
        const string C_IMMEDIATEDEST = " 071000152";
        const string C_IMMEDIATEDESTNAME = "Northern Trust Company ";
        const string C_IMMEDIATEORIGIN = "1135562401";
        const string C_IMMEDORIGINNAME = "YMCA Retirement Fund   ";
        const string C_ORIGINATINGDFIID = "07100015";
        const string C_ORIGINATORSTATUSCODE = "1";
        const string C_PRIORITYCODE = "01";
        const string C_RECORDSIZE = "094";
        const string C_ROWCODE1 = "1";
        const string C_ROWCODE5 = "5";
        const string C_ROWCODE8 = "8";
        const string C_ROWCODE9 = "9";
        const string C_ROWCODEH = "H";
        const string C_SERVICECLASSCODE = "200";
        const string C_STANDARDECCODE = "PPD";
        const string C_COMPANYNAMESHORT = "YMCA Retire Fund";
        const string C_ADDENDAINDICATOR = "0";
        const string C_ROWCODE6 = "6";
        const string C_ROWCODEA = "A";
        const string C_ROWCODEB = "B";
        const string C_ROWCODER = "R";
        const string C_ROWCODET = "T";
        const string C_TRACENUMBER = "000000000000000";
        const string C_TRANSACTIONCODE = "SA";
        const string C_YMCABANKACCOUNT = "0030362081";
        const string C_ROWCODE7_ADDENDA = "7";
        const string C_ADDENDATYPE_ADDENDA = "5";
        const string C_DEDTEXT = "Ded       ";
        const string C_EXPDIVTEXT = "Experience Dividend - Taxable      ";
        const string C_GROSSTEXT = "Gross     ";
        const string C_NETTEXT = "Net       ";
        const string C_NONTAXTEXT = "Non-Taxable     ";
        const string C_REGALLOWTAXTEXT = "Regular Allowance: Taxable         ";
        const string C_EXPTAXTEXT = "Special Dividend                   ";

        const string C_ROWCODEE = "E";
        const string C_YTDDEDTEXT = "YTD Ded   ";
        const string C_YTDGROSSTEXT = "YTD Gross ";
        const string C_YTDNETTEXT = "YTD Net   ";
        const string C_ROWCODED = "D";
        const string C_ROWCODEC = "C";
        const string C_ROWCODEF = "F";
        const string C_ROWCODEG = "G";
        const string C_POSPAY_EFT_LINE1 = "$$ADD ID=YMCRE1B BID='9902827 A";
        const string C_POSPAY_LINE1_SUFFIX = "RPI'";
        const string C_EFT_LINE1_SUFFIX = "CHI'";
       

        // Constants for identifying the check detail file.
        const int CSDetail = 1;
        const int PPDetail = 2;
        const int EFTDetail = 3;

        //PP : 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        const int SHIRADetails = 4;
        //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        #endregion "Constants"

        #region "Members"
        string String_PersID,
            String_Address1,
            String_Address2,
            String_Address3,
            String_Address4,
            String_Address5,
            String_CompanyData,
            String_Description,
            String_DisbursementID,
            String_DisbursementNumber,
            String_FilingStatusExemptions,
            String_FundID,
            String_IndividualID,
            String_Name22,
            String_Name60,
            String_ReceivingDFEAccount,
            String_EftTypeCode,
            String_BankABANumberNinthDigit,
            String_DisbursementType,
            String_UsCanadaBankCode,
            String_PaymentMethodCode,
            String_BankAbaNumber;
        //start of change
        //string [][] arrayFileList; commented to be replaced by Datatable
        DataTable l_datatable_FileList;
        //end of change

        // PaymentData_Addenda, EntryDetailSeqNum_Addenda, AddendaSeqNum_Addenda,

        DateTime CheckDate, DisbursementDate, dateTimePayrollMonth;

        double double_NetPayMonthTotal, TotalMonthlyNet;
        //long RegisterCount;
        //int ExpDivMonth, ExpDivYTD; 

        // PayeePayroll Variables
        double double_GrossPayMonthNontaxable,
            double_GrossPayMonthTaxable,
            double_GrossPayMonthTotal,
            double_GrossPayYTDNontaxable,
            double_GrossPayYTDTaxable,
            double_GrossPayYTDTotal,
            double_NetPayYTDTotal,
            double_WithholdingMonthTotal,
            double_WithholdingYTDTotal,
            double_WithholdingMonthAddl;

        double double_OutputFilesEFTDetailSum, double_OutputFileEFTDetailCount;
        double double_OutputFilesPPDetailCount;
        double int32_OutputFileEFTDetailHash;

        //string 	String_OutputFileCSCanadianGuid,String_OutputFileCSUSGuid,String_OutputFilePosPayGuid,String_OutputFileEFT_NorTrustGuid;
        string String_OutputFileCSCanadianGuid, String_OutputFileCSUSGuid, String_OutputFilePosPayGuid, String_OutputFileEFT_NorTrustGuid, String_OutputFileCShira_MilleTrustGuid;
		string String_OutputFileFSTANN_Guid;//ML | 2019.12.17 | YRS-AT-4601 | Declare variable to store FSTANN file GUIID

        //PayeePayrollWithholding varables

        double double_WithholdingMonthDetail, double_WithholdingYTDDetail;
        string String_WithholdingDetailDesc;

        string String_OutputFileType;

        double double_Exemptions, double_OutputFilesPPDetailSum;
        //Int32 OutputFileFormatCurrency,	OutputFilesPPDetailSum,lcOutputCursor,;

        bool bool_OutputFilePayRoll,
            bool_OutputFileRefund,
            bool_OutputFilePosPay,
            bool_OutputFileCSUS,
            bool_OutputFileCSCanadian,
            bool_OutputFileEFT_NorTrust,
            bool_OutputFileTDLoan,
        bool_OutputFileEXP;
        public bool bool_OutputFileCShira_MilleTrust; //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

        //OutputFileOutputFileRefund,OutputFileRefund,

        //Refund Members
        string String_AcctTypeDesc,
            String_Descr,
            String_Detail1,
            String_Detail2,
            String_Detail3,
            String_Detail4,
            String_DeductionDetailDesc;
        
        string strBeneficiaryOf; //AA 2016.02.04	YRS-AT-2079 

        double Double_AccountNonTaxable,
            Double_AccountTaxable,
            Double_AccountInterest,
            Double_AccountTotal,
            Double_AccountTaxableSum,
            Double_AccountInterestSum,
            Double_AccountTotalSum,
            Double_WithholdingDetailTaxable,
            Double_WithholdingDetailInterest,
            Double_WithholdingDetailTotal,
            Double_WithholdingTotal,
            Double_DeductionDetailTaxable,
            Double_DeductionDetailInterest,
            Double_DeductionDetailTotal,
            Double_DeductionTotal,
            Double_NetNonTaxable,
            Double_NetTaxable,
            Double_NetInterest,
            Double_WithholdingTaxable,
            Double_WithholdingInterest,
            Double_TaxablePrincipal,
            Double_TaxableWithHeldPrincipal,
            Double_GrossBalance,
            Double_inDeductions,
            Double_InterestBalance,
            Double_NetBalance;



        StreamWriter StreamOutputFileCSCanadian, StreamOutputFileCSUS, StreamOutputFilePosPay, StreamOutputFileEFT_NorTrust,
            StreamOutputFileCShira_MilleTrust, //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            StreamOutputFileFstAnn;//ML |2019.11.18 |YRS-AT-4601 | First Annuity Generate file Output stream declare.
        StreamWriter StreamOutputFileCSCanadianBack, StreamOutputFileCSUSBack, StreamOutputFilePosPayBack, StreamOutputFileEFT_NorTrustBack,
            StreamOutputFileCShira_MilleTrustBack,//PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            StreamOutputFileFstAnnBack;//ML |2019.11.18 |YRS-AT-4601 | First Annuity Generate file Output stream declare.
            
        bool ProofReport;


        //START: PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
        bool m_bool_ShiraProcess;
        public Boolean boolShiraProcess
        {
            get { return m_bool_ShiraProcess; }
            set { m_bool_ShiraProcess = value; }
        }
        string String_FirstName,
           String_LastName,
           String_MiddleName,
           String_City,
           String_State,
           String_ZipCode,
           String_Phone_number,
           String_Date_of_Birth,
           String_Tax_id_Number;

        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
        //bool m_bool_IsMRDEligibleProcess;
        //public Boolean boolIsMRDEligibleProcess
        //{
        //    get { return m_bool_IsMRDEligibleProcess; }
        //    set { m_bool_IsMRDEligibleProcess = value; }
        //}

        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
        //Int32 l_int_MRDCnt = 0;
        //public Int32 MRDCnt
        //{
        //    get { return l_int_MRDCnt; }
        //    set { l_int_MRDCnt = value; }
        //}

        Int32 l_int_MilleCnt = 0;
        public Int32 MilleCnt
        {
            get { return l_int_MilleCnt; }
            set { l_int_MilleCnt = value; }
        }

		// START : 2018.04.06 | YRS-AT-3101 |  Defined properties which will be used for EFT file creation.
        string companyEntryDesc = "";
        public string CompanyEntryDesc
        {
            get { return companyEntryDesc; }
            set { companyEntryDesc = value; }
        }

        string eFTBatchId = ""; 
        public string EFTBatchId
        {
            get { return eFTBatchId; }
            set { eFTBatchId = value; }
        }
       
        string disbursementType = "";
        public string DisbursementType
        {
            get { return disbursementType; }
            set { disbursementType = value; }
        }        
     
        string paymentMethod = "";
        public string PaymentMethod
        {
            get { return paymentMethod; }
            set { paymentMethod = value; }
        }
        
        // YRS-AT-3101 | Use buissnessdate in Loan EFT Files.
        DateTime eftBuissnessDate = DateTime.Today;
        public DateTime EFTBuissnessDate
        {
            get { return eftBuissnessDate; }
            set { eftBuissnessDate = value; }
        }
        // END | SR | 2018.10.11 | YRS-AT-3101 |  Defined properties which will be used for EFT file creation.

        public bool bool_OutputFileFSTANN; // ML | 2019.12.17 | YRA-AT-4601 | set flag to insert file infor in datatable 
        //START : ML |2019.12.17 | YRS-AT-4601 | Set Payment Outsourcing key based on Database value
        bool createOutputFileFSTANN = false;
        public bool CreateOutputFileFSTANN
        {
            get { return createOutputFileFSTANN; }
            set { createOutputFileFSTANN = value; }
        }
        //END : ML |2019.12.17 | YRS-AT-4601 | Set Payment Outsourcing key based on Database value
        //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

        #endregion "Members"

        public PaymentManagerBOWraperClass()
        {
            //
            // TODO: Add constructor logic here
            //
            this.String_OutputFileCSCanadianGuid = "";
            this.String_OutputFileCSUSGuid = "";
            this.String_OutputFilePosPayGuid = "";
            this.String_OutputFileEFT_NorTrustGuid = "";
            this.String_OutputFileFSTANN_Guid = "";//ML |YRS-AT-4601 | Declare variable
            this.int32_OutputFileEFTDetailHash = 0;
            this.double_OutputFilesPPDetailSum = 0;
            this.double_OutputFilesPPDetailCount = 0;

            this.l_datatable_FileList = new DataTable();
            DataColumn SourceFolder = new DataColumn("SourceFolder", typeof(String));
            DataColumn SourceFile = new DataColumn("SourceFile", typeof(String));
            DataColumn DestFolder = new DataColumn("DestFolder", typeof(String));
            DataColumn DestFile = new DataColumn("DestFile", typeof(String));

            this.l_datatable_FileList.Columns.Add(SourceFolder);
            this.l_datatable_FileList.Columns.Add(SourceFile);
            this.l_datatable_FileList.Columns.Add(DestFolder);
            this.l_datatable_FileList.Columns.Add(DestFile);
        }

        public void ProduceOutputFilesCreateFooters()
        {
            //string lcOutputPPBatchDate,lcCompanyDate;
            string l_String_temp;
            //string l_String_temp1;
            string l_String_Output;
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencyGroupSeparator = "";
            nfi.CurrencySymbol = "";
            nfi.CurrencyDecimalDigits = 2;

            try
            {
                Int64 l_long_Blocks;
                // Gets a NumberFormatInfo associated with the en-US culture.
                l_String_Output = "";



                if (this.bool_OutputFileEFT_NorTrust)
                {
                    this.String_OutputFileType = "EFT";

                    l_String_Output = C_ROWCODE8 + C_SERVICECLASSCODE;
                    l_String_Output += this.double_OutputFileEFTDetailCount.ToString().Trim().PadLeft(6, '0');
                    l_String_temp = this.int32_OutputFileEFTDetailHash.ToString().Trim().PadLeft(20, '0');
                    l_String_Output += l_String_temp.Substring(10, 10).ToString();
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(12, '0');

                    l_String_Output += this.double_OutputFilesEFTDetailSum.ToString("C", nfi).Trim().PadLeft(13, '0').Remove(10, 1);
                    l_String_Output += C_COMPANYID;
                    l_String_Output += l_String_temp.PadRight(25, ' ');
                    l_String_Output += C_ORIGINATINGDFIID + C_BATCHNUMBER;

                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                    //Insert.

                    l_String_Output = C_ROWCODE9 + C_BATCHCOUNT;
                    l_long_Blocks = (long)this.double_OutputFileEFTDetailCount + 4;
                    l_long_Blocks *= 94;
                    if (l_long_Blocks % 940 > 0)
                    {
                        l_long_Blocks = (l_long_Blocks / 940) + 1;
                    }
                    else
                    {
                        l_long_Blocks = (l_long_Blocks / 940);
                    }

                    //l_long_Blocks = IIF(MOD(l_long_Blocks,940) > 0, (l_long_Blocks / 940) + 1, (l_long_Blocks / 940) )
                    //l_long_Blocks = (long)this.lcOutputEFT_2+= l_long_Blocks.ToString().Trim().PadLeft(6,'0');

                    l_String_Output += l_long_Blocks.ToString().Trim().PadLeft(6, '0');
                    l_String_Output += this.double_OutputFileEFTDetailCount.ToString().Trim().PadLeft(8, '0');

                    l_String_Output += this.int32_OutputFileEFTDetailHash.ToString().Trim().PadLeft(10, '0');
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(12, '0');

                    l_String_Output += this.double_OutputFilesEFTDetailSum.ToString("C", nfi).Trim().PadLeft(13, '0').Remove(10, 1);
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(39, ' ');

                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                    //Insert
                }
                ////PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                //if (this.bool_OutputFilePosPay == true && this.bool_OutputFileCShira_MilleTrust == true)
                //{
                //    this.String_OutputFileType = "SHIRA";
                //    //l_String_Output = C_ROWCODET;
                //    //l_String_temp = C_YMCABANKACCOUNT;
                //    //l_String_Output += l_String_temp.Trim().PadLeft(10, '0');
                //    //l_String_Output += this.double_OutputFilesPPDetailCount.ToString().PadLeft(10, '0');
                //    //l_String_Output += this.double_OutputFilesPPDetailSum.ToString("C", nfi).Trim().PadLeft(14, '0').Remove(11, 1);
                //    //l_String_temp = "";
                //    //l_String_Output += l_String_temp.PadRight(46, ' ');
                //    //WriteOutputFilesDataLineIntoStream(l_String_Output);
                //    //Insert
                //}
                //END PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                //PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                //Added && this.m_bool_ShiraProcess == false in following if condition
                if (this.bool_OutputFilePosPay)
                {
                    this.String_OutputFileType = "PP";
                    l_String_Output = C_ROWCODET;
                    l_String_temp = C_YMCABANKACCOUNT;
                    l_String_Output += l_String_temp.Trim().PadLeft(10, '0');
                    l_String_Output += this.double_OutputFilesPPDetailCount.ToString().PadLeft(10, '0');
                    l_String_Output += this.double_OutputFilesPPDetailSum.ToString("C", nfi).Trim().PadLeft(14, '0').Remove(11, 1);
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(46, ' ');
                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                    //Insert
                }

            }
            catch
            {
                throw;
            }


        }


        public void ProduceOutputFilesCreateHeader()
        {
            string l_String_Output;
            string l_String_temp, l_String_OutputPPBatchDate, l_String_lcEffectiveEntryDate, l_String_lcCompanyDate;
            string lc_year, lc_month, lc_day;
            DateTime ld_date;
            string l_String_temp1;

            try
            {
                l_String_temp1 = l_String_temp = lc_year = lc_month = lc_day = l_String_OutputPPBatchDate = "";
                ld_date = DateTime.Now;
                l_String_OutputPPBatchDate = ld_date.ToString("MMddyy");

                if (DisbursementType == "TDLOAN" && PaymentMethod == YMCAObjects.PaymentMethod.EFT)
                {
                    dateTimePayrollMonth = EFTBuissnessDate; // SR | 2018.10.09 | YRS-AT-3101 | Use buissnessdate in Loan EFT Files as payroll month date. 
                    //START: PPP | 12/20/2018 | YRS-AT-4253 | For Loans EFT date format should be first 4 char of month and last 2 digits of year.
                    //                                        So for example, Jaunuary it should be JANU and in case of may it must be MAY0 (zero)
                    //                                        So final date will be JANU18, DECE18, MAY018
                    l_String_temp = this.dateTimePayrollMonth.ToString("MMMM");
                    if (l_String_temp.Length > 4)
                        l_String_temp1 = l_String_temp.ToUpper().Substring(0, 4);
                    else
                        l_String_temp1 = l_String_temp.ToUpper().PadRight(4, '0');
                }
                else
                {
                    // #YREN-2569 
                    l_String_temp = this.dateTimePayrollMonth.ToString("MMM").ToUpper().Substring(0, 3);

                    if (l_String_temp == "MAY")
                    {
                        l_String_temp1 = "MAY ";
                    }
                    else if (l_String_temp == "JUN")
                    {
                        l_String_temp1 = "JUNE";
                    }
                    else if (l_String_temp == "JUL")
                    {
                        l_String_temp1 = "JULY";
                    }
                    else
                    {
                        l_String_temp1 = l_String_temp + "'";
                    }
                }
                l_String_lcCompanyDate = l_String_temp1 + this.dateTimePayrollMonth.ToString("yy");
                //END: PPP | 12/20/2018 | YRS-AT-4253 | For Loans EFT date format should be first 4 char of month and last 2 digits of year.

                l_String_lcEffectiveEntryDate = this.dateTimePayrollMonth.ToString("yyMMdd");

                //l_String_lcCompanyDate = ld_date.ToString("MMM") +  "'" + ld_date.ToString("yy");   

                //START: PPP | 12/20/2018 | YRS-AT-4253 | For Loans EFT date format should be first 4 char of month and last 2 digits of year.
                //                                        So commenting below lines of code
                //// #YREN-2569 
                //l_String_temp = this.dateTimePayrollMonth.ToString("MMM").ToUpper().Substring(0, 3);

                //if (l_String_temp == "MAY")
                //{
                //    l_String_temp1 = "MAY ";
                //}
                //else if (l_String_temp == "JUN")
                //{
                //    l_String_temp1 = "JUNE";
                //}
                //else if (l_String_temp == "JUL")
                //{
                //    l_String_temp1 = "JULY";
                //}
                //else
                //{
                //    l_String_temp1 = l_String_temp + "'";

                //}

                //l_String_lcCompanyDate = l_String_temp1 + this.dateTimePayrollMonth.ToString("yy");
                //END: PPP | 12/20/2018 | YRS-AT-4253 | For Loans EFT date format should be first 4 char of month and last 2 digits of year.

                // #YREN-2569
                //PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                //if (this.bool_OutputFilePosPay == true && this.bool_OutputFileCShira_MilleTrust == true)
                //{
                //    this.String_OutputFileType = "SHIRA";
                //    //l_String_Output = C_POSPAY_EFT_LINE1 + C_POSPAY_LINE1_SUFFIX;

                //    //WriteOutputFilesDataLineIntoStream(l_String_Output);

                //    ////Insert
                //    //l_String_Output = C_ROWCODEH;
                //    //l_String_temp = C_COMPANYNAME;
                //    //l_String_temp = l_String_temp.Trim().PadRight(33, ' ');
                //    ////l_String_OutputPPBatchDate
                //    //l_String_Output += l_String_temp;
                //    //l_String_Output += l_String_OutputPPBatchDate;
                //    //l_String_temp = "";
                //    //l_String_Output += l_String_temp.PadRight(40, ' ');
                //    //WriteOutputFilesDataLineIntoStream(l_String_Output);
                //    //Insert
                //}
                //End PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                //PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                //Added  && this.m_bool_ShiraProcess == false
                if (this.bool_OutputFilePosPay)
                {
                    this.String_OutputFileType = "PP";
                    l_String_Output = C_POSPAY_EFT_LINE1 + C_POSPAY_LINE1_SUFFIX;

                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                    //Insert
                    l_String_Output = C_ROWCODEH;
                    l_String_temp = C_COMPANYNAME;
                    l_String_temp = l_String_temp.Trim().PadRight(33, ' ');
                    //l_String_OutputPPBatchDate
                    l_String_Output += l_String_temp;
                    l_String_Output += l_String_OutputPPBatchDate;
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(40, ' ');
                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                    //Insert

                }

                if (this.bool_OutputFileEFT_NorTrust)
                {
                    this.String_OutputFileType = "EFT";
                    l_String_Output = C_ROWCODE1 + C_PRIORITYCODE + C_IMMEDIATEDEST + C_IMMEDIATEORIGIN;
                    l_String_Output += ld_date.ToString("yyMMdd");
                    l_String_Output += ld_date.ToString("hhmm");
                    l_String_Output += C_FILEIDMODIFIER + C_RECORDSIZE + C_BLOCKINGFACTOR;
                    l_String_Output += C_FILEFORMATCODE + C_IMMEDIATEDESTNAME + C_IMMEDORIGINNAME;
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(8, ' ');
                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                    //Insert


                    l_String_Output = C_ROWCODE5 + C_SERVICECLASSCODE + C_COMPANYNAMESHORT;
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(20, ' ');
                    // START : SR | 2018.05.25 | YRS-AT-3101 | For Loan EFT use 'YRFLOANEFT' as C_COMPANYENTRYDESC. It is table driven value rather than using constant variable.
                    //l_String_Output += C_COMPANYID + C_STANDARDECCODE + C_COMPANYENTRYDESC;
                    l_String_Output += C_COMPANYID + C_STANDARDECCODE + companyEntryDesc;
                    // END : SR | 2018.05.25 | YRS-AT-3101 | For Loan EFT use 'YRFLOANEFT' as C_COMPANYENTRYDESC. It is table driven value rather than using constant variable.
                    l_String_Output += l_String_lcCompanyDate;
                    l_String_Output += l_String_lcEffectiveEntryDate;
                    l_String_temp = "";
                    l_String_Output += l_String_temp.PadRight(3, ' ');
                    l_String_Output += C_ORIGINATORSTATUSCODE + C_ORIGINATINGDFIID + C_BATCHNUMBER;
                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                    //Insert

                }
            }
            catch
            {
                throw;
            }

        }


        private void ProduceOutputFilesCreateDetail(int index, int intPhase)
        {

            string l_String_Output, lc_space, l_String_temp;
            double double_temp;
            // Number Formats.

            try
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                nfi.CurrencyGroupSeparator = "";
                nfi.CurrencySymbol = "";
                nfi.CurrencyDecimalDigits = 2;

                lc_space = l_String_temp = "";
                double_temp = 0.00;

                switch (index)
                {

                    //START: PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    case SHIRADetails:
                        #region "SHIRADetails"

                        this.String_OutputFileType = "SHIRA";
                        l_String_Output = "\"MTCez\"";
                        l_String_Output += "\t\"" + "YMCA Retirement Fund\"";
                        l_String_Output += "\t\"" + "YMCA Retirement Fund\"";
                        l_String_Output += "\t\"" + this.String_FirstName.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_MiddleName.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_LastName.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_Address1.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_Address2.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_Address3.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_City.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_State.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_ZipCode.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_Phone_number.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_Date_of_Birth.Trim() + "\"";
                        l_String_Output += "\t\"" + this.String_Tax_id_Number.Trim() + "\"";
                        l_String_Output += "\t" + this.Double_GrossBalance.ToString("C", nfi).Trim();

                        WriteOutputFilesDataLineIntoStream(l_String_Output);
                        // insert

                        //this.double_OutputFileEFTDetailCount += 1;
                        //this.double_OutputFilesEFTDetailSum += this.double_NetPayMonthTotal;

                        break;

                        #endregion "SHIRADetails"
                    //END: PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    case CSDetail:

                        if (this.String_UsCanadaBankCode == "1" || this.String_UsCanadaBankCode == "3" || this.String_UsCanadaBankCode == "5")
                        {
                            //this.lcOutputCursor = "curCSUS";
                            this.String_OutputFileType = "CHKSCU";
                        }
                        else if (this.String_UsCanadaBankCode == "2" || this.String_UsCanadaBankCode == "4" || this.String_UsCanadaBankCode == "6")
                        {
                            //this.lcOutputCursor = "curCSCanadian";
                            this.String_OutputFileType = "CHKSCC";
                        }

                        if (intPhase == 1)
                        {
                            l_String_Output = C_ROWCODEA;
                            l_String_Output += this.String_DisbursementNumber.Trim().PadRight(10, ' ');
                            l_String_Output += this.CheckDate.ToString("yyyy/MM/dd");
                            l_String_Output += this.TotalMonthlyNet.ToString("C", nfi).PadLeft(11, '0');
                            l_String_Output += this.String_UsCanadaBankCode.Trim();

                            WriteOutputFilesDataLineIntoStream(l_String_Output);
                            //Insert 
                            // Row B
                            l_String_Output = C_ROWCODEB;
                            l_String_Output += this.String_Name60.PadRight(60, ' ');
                            if (this.String_Address1.Trim() != "") l_String_Output += this.String_Address1.PadRight(60, ' ');
                            if (this.String_Address2.Trim() != "") l_String_Output += this.String_Address2.PadRight(60, ' ');
                            if (this.String_Address3.Trim() != "") l_String_Output += this.String_Address3.PadRight(60, ' ');
                            if (this.String_Address4.Trim() != "") l_String_Output += this.String_Address4.PadRight(60, ' ');
                            if (this.String_Address5.Trim() != "") l_String_Output += this.String_Address5.PadRight(60, ' ');
                            WriteOutputFilesDataLineIntoStream(l_String_Output);

                            // Insert

                            // Payroll 
                            if (this.bool_OutputFilePayRoll)
                            {
                                l_String_Output = C_ROWCODEC;
                                l_String_Output += this.String_FundID.Trim().PadRight(9, ' ');
                                l_String_Output += this.String_FilingStatusExemptions.Trim();
                                l_String_Output += "$" + this.double_WithholdingMonthAddl.ToString("C", nfi).Trim().PadLeft(9, ' ');
                                WriteOutputFilesDataLineIntoStream(l_String_Output);

                                // Insert
                                // Row D - (Current)
                                l_String_Output = C_ROWCODED + C_GROSSTEXT;
                                l_String_Output += "$" + this.double_GrossPayMonthTotal.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                l_String_Output += lc_space.PadRight(27, ' ') + C_DEDTEXT;
                                l_String_Output += "$" + this.double_WithholdingMonthTotal.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                l_String_Output += lc_space.PadRight(27, ' ') + C_NETTEXT;
                                l_String_Output += "$" + this.double_NetPayMonthTotal.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                WriteOutputFilesDataLineIntoStream(l_String_Output);

                                //Insert

                                //Record type D - (YTD)
                                l_String_Output = C_ROWCODED + C_YTDGROSSTEXT;
                                l_String_Output += "$" + this.double_GrossPayYTDTotal.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                l_String_Output += lc_space.PadRight(27, ' ') + C_YTDDEDTEXT;
                                l_String_Output += "$" + this.double_WithholdingYTDTotal.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                l_String_Output += lc_space.PadRight(27, ' ') + C_YTDNETTEXT;
                                l_String_Output += "$" + this.double_NetPayYTDTotal.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                WriteOutputFilesDataLineIntoStream(l_String_Output);


                                // Insert
                                // Record Type D - (Other Deductions) - Can be multiple records

                            }
                            else if (this.bool_OutputFileRefund)
                            {
                                l_String_Output = C_ROWCODEC;
                                if (this.String_FundID != String.Empty) l_String_Output += this.String_FundID.PadRight(9, ' ');
                                WriteOutputFilesDataLineIntoStream(l_String_Output);


                            }

                        }
                        else if (intPhase == 2)
                        {
                            if (this.bool_OutputFileRefund)
                            {
                                l_String_Output = C_ROWCODED;
                                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false)
                                {
                                    //Prasad Jadhav   29/10/11   For BT-931,YRS 5.0-1412:Changes for the .bch file
                                    l_String_Output += ("$" + this.Double_NetNonTaxable.ToString("C", nfi).Trim()).PadLeft(12, ' ');
                                    l_String_Output += (" " + "$" + this.Double_TaxablePrincipal.ToString("C", nfi).Trim()).PadLeft(13, ' ');
                                    l_String_Output += (" " + "$" + this.Double_InterestBalance.ToString("C", nfi).Trim()).PadLeft(13, ' ');


                                }
                                else if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == true)
                                {
                                    l_String_Output += "$";
                                    l_String_Output += "  ";
                                    l_String_temp = l_String_temp.PadRight(36, ' ');
                                    l_String_Output += l_String_temp;
                                }
                                //PP28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false && this.bool_OutputFileCShira_MilleTrust == true)
                                {
                                    l_String_Output += (" " + "$" + this.Double_GrossBalance.ToString("C", nfi).Trim()).PadLeft(13, ' ');
                                    this.String_OutputFileType = "SHIRA";
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                                }

                                //Prasad Jadhav   29/10/11   For BT-931,YRS 5.0-1412:Changes for the .bch file
                                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false && this.bool_OutputFileCShira_MilleTrust == false)
                                {
                                    l_String_Output += (" " + "$" + this.Double_GrossBalance.ToString("C", nfi).Trim()).PadLeft(13, ' ');
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                                }
                                else
                                {
                                    l_String_Output += " ";
                                    l_String_Output += "$" + this.Double_GrossBalance.ToString("C", nfi).Trim().PadLeft(11, ' ');
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                                }
                            }
                        }
                        else if (intPhase == 3)
                        {
                            if (this.bool_OutputFilePayRoll)
                            {
                                if (this.bool_OutputFileEXP)
                                {
                                    l_String_Output = C_ROWCODEE + C_EXPTAXTEXT;
                                }
                                else
                                {
                                    l_String_Output = C_ROWCODEE + C_REGALLOWTAXTEXT;
                                }

                                l_String_Output += "$" + this.double_GrossPayMonthTaxable.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                l_String_Output += lc_space.PadRight(1, ' ');
                                l_String_Output += "$" + this.double_GrossPayYTDTaxable.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                WriteOutputFilesDataLineIntoStream(l_String_Output);

                                //Insert
                                if (!(this.double_GrossPayMonthNontaxable == 0 && double_GrossPayYTDNontaxable == 0))
                                {
                                    l_String_Output = C_ROWCODEE;

                                    //commented by hafiz on 13Jul2006
                                    //l_String_Output += lc_space.PadRight(39,' ');

                                    //code added by hafiz on 13Jul2006
                                    l_String_Output += lc_space.PadRight(19, ' ');

                                    l_String_Output += C_NONTAXTEXT;
                                    l_String_Output += "$" + this.double_GrossPayMonthNontaxable.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                    l_String_Output += lc_space.PadRight(1, ' ');
                                    l_String_Output += "$" + this.double_GrossPayYTDNontaxable.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                                    //Insert

                                }
                            }
                            else if (this.bool_OutputFileRefund)
                            {
                                //Prasad Jadhav   29/03/2012   for YRS 5.0-1412:Changes for the .bch file
                                l_String_Output = C_ROWCODEH;
                                double_temp = this.Double_WithholdingTotal + this.Double_inDeductions;
                                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false)
                                {
                                    //Prasad Jadhav   24/10/11   for YRS 5.0-1412:Changes for the .bch file
                                    if (double_temp != 0.0)
                                    {
                                        l_String_Output += ("$" + this.Double_WithholdingTaxable.ToString("C", nfi).Trim()).PadLeft(13, ' ');
                                        l_String_Output += (" " + "$" + this.Double_WithholdingInterest.ToString("C", nfi).Trim()).PadLeft(14, ' ');
                                        l_String_Output += (" " + "$" + double_temp.ToString("C", nfi).Trim()).PadLeft(14, ' ');
                                    }
                                    //Prasad Jadhav   29/03/2012   for YRS 5.0-1412:Changes for the .bch file
                                    else
                                    {
                                        WriteOutputFilesDataLineIntoStream(C_ROWCODEE);
                                    }
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                                }
                                else if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == true)
                                {

                                    l_String_Output += "$";
                                    l_String_temp = l_String_temp.PadRight(27, ' ');
                                    l_String_Output += l_String_temp;
                                    //Prasad Jadhav   24/10/11   for YRS 5.0-1412:Changes for the .bch file
                                    l_String_Output += " ";
                                    double_temp = this.Double_WithholdingTotal + this.Double_inDeductions;
                                    l_String_Output += "$" + double_temp.ToString("C", nfi).Trim().PadLeft(12, ' ');
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                                }

                                l_String_Output = C_ROWCODEF;

                                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false)
                                {
                                    //Prasad Jadhav   29/10/11   For BT-931,YRS 5.0-1412:Changes for the .bch file
                                    l_String_Output += ("$" + this.Double_NetNonTaxable.ToString("C", nfi).Trim()).PadLeft(13, ' ');
                                    l_String_Output += (" " + "$" + this.Double_NetTaxable.ToString("C", nfi).Trim()).PadLeft(14, ' ');
                                    l_String_Output += (" " + "$" + this.Double_NetInterest.ToString("C", nfi).Trim()).PadLeft(14, ' ');
                                    l_String_Output += (" " + "$" + this.Double_NetBalance.ToString("C", nfi).Trim()).PadLeft(14, ' ');
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                                }
                                else if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == true)
                                {

                                    l_String_Output += "$";
                                    l_String_temp = "";
                                    l_String_temp = l_String_temp.PadRight(41, ' ');
                                    l_String_Output += l_String_temp;
                                    l_String_Output += " ";
                                    l_String_Output += "$" + this.Double_NetBalance.ToString("C", nfi).Trim().PadLeft(11, ' ');
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);

                                }


                                l_String_Output = C_ROWCODEG;
                                l_String_Output += this.String_Descr.Trim().PadRight(90, ' ');
                                if (this.String_Detail1 != "") l_String_Output += " " + this.String_Detail1.Trim().PadRight(60, ' ');
                                if (this.String_Detail2 != "") l_String_Output += " " + this.String_Detail2.PadRight(60, ' ');
                                if (this.String_Detail3 != "") l_String_Output += " " + this.String_Detail3.PadRight(60, ' ');
                                if (this.String_Detail4 != "") l_String_Output += " " + this.String_Detail4.PadRight(60, ' ');
                                // WriteOutputFilesDataLineIntoStream(l_String_Output);//AA:2016.02.05  YRS-AT-2079 -commented because below lines are adding new line for beneficary

                                //AA:2016.02.05	YRS-AT-2079: -Start
                                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false && !string.IsNullOrEmpty(this.strBeneficiaryOf))
                                {
                                    l_String_Output += " " + this.strBeneficiaryOf.Trim().PadRight(60, ' ');
                                }
                                // -Added because it will add new line for beneficary
                                WriteOutputFilesDataLineIntoStream(l_String_Output);
                                //AA:2016.02.05	YRS-AT-2079: -End

                            }
                        }

                        break;

                    case PPDetail:
                        this.String_OutputFileType = "PP";
                        l_String_Output = C_ROWCODER + C_YMCABANKACCOUNT;
                        l_String_Output += this.String_DisbursementNumber.Trim().PadLeft(10, '0');
                        l_String_Output += this.double_NetPayMonthTotal.ToString("C", nfi).Trim().PadLeft(11, '0').Remove(8, 1);
                        l_String_Output += this.DisbursementDate.ToString("MMddyy");
                        l_String_Output += C_TRANSACTIONCODE;
                        l_String_Output += this.String_Description.Trim().PadRight(15, ' ');
                        WriteOutputFilesDataLineIntoStream(l_String_Output);
                        this.double_OutputFilesPPDetailSum += this.double_NetPayMonthTotal;
                        //Insert

                        this.double_OutputFilesPPDetailCount += 1;


                        break;
                    case EFTDetail:
                        if (this.String_UsCanadaBankCode == "5") // SR | 2018.04.06 | YRS-AT-3101 |  Replaced UsCanadaBankCode from "1" to "5" for "TDLOAN"
                        {
                            //this.lcOutputCursor = "curNorthernTrust";
                            this.String_OutputFileType = "EFT";
                            l_String_Output = C_ROWCODE6;
                            l_String_Output += this.String_EftTypeCode.Trim().PadRight(2, ' ');
                            l_String_Output += this.String_BankAbaNumber.Trim().PadRight(8, ' ');
                            l_String_temp += this.String_BankABANumberNinthDigit.Trim().Substring(this.String_BankABANumberNinthDigit.Trim().Length - 1, 1);
                            l_String_Output += l_String_temp;
                            l_String_Output += this.String_ReceivingDFEAccount.Trim().PadRight(17, ' ');
                            l_String_Output += this.double_NetPayMonthTotal.ToString("C", nfi).Trim().PadLeft(11, '0').Remove(8, 1);
                            l_String_Output += this.String_IndividualID.Trim().PadRight(15, ' ');
                            l_String_Output += this.String_Name22.Trim().PadRight(22, ' ');
                            l_String_Output += this.String_CompanyData.Trim().PadRight(2, ' ');
                            l_String_Output += C_ADDENDAINDICATOR + C_TRACENUMBER;

                            WriteOutputFilesDataLineIntoStream(l_String_Output);
                            // insert
                            this.double_OutputFileEFTDetailCount += 1;
                            this.double_OutputFilesEFTDetailSum += this.double_NetPayMonthTotal;
                            if (this.String_BankAbaNumber.Trim().Length > 8)
                            {
                                this.int32_OutputFileEFTDetailHash += Int32.Parse(this.String_BankAbaNumber.Trim().Substring(0, 8));
                            }
                            else
                            {
                                this.int32_OutputFileEFTDetailHash += Int32.Parse(this.String_BankAbaNumber.Trim());

                            }

                            //this.double_OutputFilesEFTDetailSum += this.double_NetPayMonthTotal;  

                        }

                        break;
                    default:
                        break;
                }


            }
            catch
            {
            }

        }


        private void ProduceOutputFilesCreateDetailMonthlyHolding()
        {
            string l_String_Output;
            string l_String_temp;
            string lc_space;
            // define Number Formats
            try
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                nfi.CurrencyGroupSeparator = "";
                nfi.CurrencySymbol = "";
                nfi.CurrencyDecimalDigits = 2;

                lc_space = "";

                l_String_Output = C_ROWCODED;
                l_String_temp = this.String_WithholdingDetailDesc.Trim().PadRight(29, ' ');
                if (l_String_temp.Length > 29) l_String_temp = l_String_temp.Substring(0, 28);
                l_String_Output += l_String_temp;
                l_String_Output += "$" + this.double_WithholdingMonthDetail.ToString("C", nfi).Trim().PadLeft(12, ' ');
                l_String_Output += lc_space.PadRight(1, ' ');
                l_String_Output += "$" + this.double_WithholdingYTDDetail.ToString("C", nfi).Trim().PadLeft(12, ' ');
                WriteOutputFilesDataLineIntoStream(l_String_Output);
            }
            catch
            {


            }

            // Insert

        }


        private void ProduceOutputFilesCreateDetailRefundWithHoldings()
        {
            string l_String_Output;
            // define Number Formats

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencyGroupSeparator = "";
            nfi.CurrencySymbol = "";
            nfi.CurrencyDecimalDigits = 2;
            l_String_Output = C_ROWCODEE;

            //Prasad Jadhav   24/10/11   for YRS 5.0-1412:Changes for the .bch file
            l_String_Output += this.String_WithholdingDetailDesc.Trim().PadRight(35, ' ');
            l_String_Output += ("$" + this.Double_WithholdingDetailTaxable.ToString("C", nfi).Trim()).PadLeft(10, ' ');
            l_String_Output += (" " + "$" + this.Double_WithholdingDetailInterest.ToString("C", nfi).Trim()).PadLeft(11, ' ');
            l_String_Output += (" " + "$" + this.Double_WithholdingDetailTotal.ToString("C", nfi).Trim()).PadLeft(11, ' ');
            WriteOutputFilesDataLineIntoStream(l_String_Output);


            // Insert

        }


        private void ProduceOutputFilesCreateDetailRefundDeductions()
        {
            string l_String_Output;
            try
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                nfi.CurrencyGroupSeparator = "";
                nfi.CurrencySymbol = "";
                nfi.CurrencyDecimalDigits = 2;
                l_String_Output = C_ROWCODEE;
                if (this.String_DeductionDetailDesc.Length > 37) l_String_Output += this.String_DeductionDetailDesc.Substring(0, 37).PadRight(57, ' ');
                else l_String_Output += this.String_DeductionDetailDesc.PadRight(57, ' ');

                //Prasad Jadhav   24/10/11   for YRS 5.0-1412:Changes for the .bch file
                if (bool_OutputFileRefund == true && bool_OutputFileTDLoan == false)
                {
                    l_String_Output += ("$" + this.Double_DeductionDetailTotal.ToString("C", nfi).Trim()).PadLeft(10, ' ');
                }
                else
                    if (bool_OutputFileRefund == true && bool_OutputFileTDLoan == true)
                    {
                        l_String_Output += "$" + this.Double_DeductionDetailTotal.ToString("C", nfi).Trim().PadLeft(9, ' ');
                    }

                WriteOutputFilesDataLineIntoStream(l_String_Output);
                //lnTotalDeductions = lnTotalDeductions + curRefundDeductions.DeductionDetailTotal
            }
            catch
            {


            }

            // Insert

        }


        private void ProduceOutputFilesCreateDetailRefundAccounts()
        {
            string l_String_Output;
            string l_string_temp;

            // define Number Formats
            try
            {
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                nfi.CurrencyGroupSeparator = "";
                nfi.CurrencySymbol = "";
                nfi.CurrencyDecimalDigits = 2;
                l_string_temp = "";

                l_String_Output = C_ROWCODED;
                if (this.String_AcctTypeDesc.Length > 37) l_String_Output += this.String_AcctTypeDesc.Substring(0, 37);
                else l_String_Output += this.String_AcctTypeDesc.PadRight(37, ' ');
                if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == false)
                {
                    //Prasad Jadhav   29/10/11   For BT-931,YRS 5.0-1412:Changes for the .bch file
                    l_String_Output += ("$" + this.Double_AccountNonTaxable.ToString("C", nfi).Trim()).PadLeft(10, ' ');
                    l_String_Output += (" " + "$" + this.Double_AccountTaxable.ToString("C", nfi).Trim()).PadLeft(11, ' ');
                    l_String_Output += (" " + "$" + this.Double_AccountInterest.ToString("C", nfi).Trim()).PadLeft(11, ' ');
                    l_String_Output += " ";
                    l_String_Output += ("$" + this.Double_AccountTotal.ToString("C", nfi).Trim()).PadLeft(10, ' ');
                    l_String_Output += " ";
                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                }
                else if (this.bool_OutputFileRefund == true && this.bool_OutputFileTDLoan == true)
                {
                    l_string_temp = l_string_temp.PadRight(33, ' ');
                    l_String_Output += l_string_temp;
                    l_String_Output += "$" + this.Double_AccountTotal.ToString("C", nfi).Trim().PadLeft(9, ' ');
                    l_String_Output += " ";
                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                }
            }
            catch
            {


            }

            // Insert

        }


        public bool PreparOutPutFileData(DataSet parameter_dataset_disbursal, DataTable Parameter_Datatable_DisbursementOutputFileInfo,
            DataTable Parameter_Datatable_DisbursementOutputFileAddlInfo,
            DataTable Parameter_Datatable_YTDDisbursementOutputFileInfo,
            DataTable Parameter_Datatable_YTDWithholdingOutputFileInfo,
            DataTable Parameter_Datatable_WithholdingsByDisbursement,
            DataTable Parameter_Datatable_DisbursementDetailsbyRefundDisbursement,
            DataTable Parameter_Datatable_DisbursementWithholdingsbyRefundDisbursement,
            DataTable Parameter_Datatable_DisbursementDeductions,
            DataTable Parameter_Datatable_WithholdingsByTDLoan,
            string parameter_String_DisbursementID, DateTime parameterDateCheckDate,
            string parameter_string_DisbursementNumber, bool parameterboolproof)
        {

            DataSet ldataset = parameter_dataset_disbursal;
            //string lstr_tmp;
            double ln_tmp, ln_tmp1, ln_tmp2;
            string lcMaritalStatusCode, lcWithholdingType;
            string l_String_expr, l_String_Guid;
            DataRow[] foundFedTaxWithholdingRORows;
            //	String_WithholdingDetailDesc,str_WithholdingTypeCode,
            this.dateTimePayrollMonth = parameterDateCheckDate;

            DataRowCollection rc;
            DataRow DfNewRow, YTDRow;

            l_String_Guid = "";
            object[] rowVals = new object[3];
            DataRow oRow, RefAddlRow;


            this.ProofReport = parameterboolproof;

            //Hashtable HTwithHoldings = new Hashtable();
            this.ProofReport = false;


            try
            {

                DataTable oDataTableDisbursementFiles = ldataset.Tables["DisbursementFiles"];
                DataTable oDataTableOutPutfilePath = ldataset.Tables["OutPutfilePath"];

                oRow = Parameter_Datatable_DisbursementOutputFileInfo.Rows[0];
                this.String_PersID = oRow["PersID"].ToString();

                DataTable oDataTableFedTaxWithholdingRO = YMCARET.YmcaDataAccessObject.PaymentManagerDAClass.GetfedtaxwithholdingsRO(this.String_PersID);

                rc = oDataTableDisbursementFiles.Rows;

                this.String_DisbursementType = oRow["DisbursementType"].ToString().Trim();

                //IdentifyPaymentTypes(Parameter_Datatable_DisbursementOutputFileInfo);

                if (this.String_DisbursementType == "ANN")
                {
                    this.bool_OutputFilePayRoll = true;
                    this.bool_OutputFileRefund = false;
                    this.bool_OutputFileTDLoan = false;
                    this.bool_OutputFileEXP = false;
                }
                else if (this.String_DisbursementType == "REF")
                {
                    this.bool_OutputFilePayRoll = false;
                    this.bool_OutputFileRefund = true;
                    this.bool_OutputFileTDLoan = false;
                    this.bool_OutputFileEXP = false;
                    //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    if (boolShiraProcess == true)
                    {
                        this.bool_OutputFileCShira_MilleTrust = true;
                    }
                    else
                    {
                        this.bool_OutputFileCShira_MilleTrust = false;
                    }
                    //'28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                }
                else if (this.String_DisbursementType == "TDLOAN")
                {
                    this.bool_OutputFilePayRoll = false;
                    this.bool_OutputFileRefund = true;
                    this.bool_OutputFileTDLoan = true;
                    this.bool_OutputFileEXP = false;

                }
                else if (this.String_DisbursementType == "EXP")
                {
                    this.bool_OutputFilePayRoll = true;
                    this.bool_OutputFileRefund = false;
                    this.bool_OutputFileTDLoan = false;
                    this.bool_OutputFileEXP = true;
                }


                // Create Streams to write payments.
                //if (!ProduceOutputFilesOutput(oDataTableOutPutfilePath)) return false;
                // Add rows for the Header Information.



                InitializseMembers();

                this.String_Address1 = oRow["Addr1"].ToString().Trim();

                if (oRow["Addr2"].GetType().ToString() == "System.DBNull" || oRow["Addr2"].ToString() == "")
                {

                    this.String_Address2 = "";
                    if (oRow["City"].GetType().ToString() != "System.DBNull")
                    {
                        this.String_Address2 = oRow["City"].ToString().Trim();
                    }

                    this.String_Address2 = this.String_Address2 + ",";

                    if (oRow["StateType"].GetType().ToString() != "System.DBNull")
                    {
                        this.String_Address2 = this.String_Address2 + oRow["StateType"].ToString().Trim();
                    }

                    this.String_Address2 = this.String_Address2 + " ";

                    if (oRow["Zip"].GetType().ToString() != "System.DBNull")
                    {
                        this.String_Address2 = this.String_Address2 + oRow["Zip"].ToString().Trim();
                    }
                }
                else
                {
                    this.String_Address2 = oRow["Addr2"].ToString().Trim();

                    this.String_Address3 = "";

                    if (oRow["City"].GetType().ToString() != "System.DBNull")
                    {
                        this.String_Address3 = oRow["City"].ToString().Trim();
                    }

                    this.String_Address3 = this.String_Address3 + ",";

                    if (oRow["StateType"].GetType().ToString() != "System.DBNull")
                    {
                        this.String_Address3 = this.String_Address3 + oRow["StateType"].ToString().Trim();
                    }

                    this.String_Address3 = this.String_Address3 + " ";

                    if (oRow["Zip"].GetType().ToString() != "System.DBNull")
                    {
                        this.String_Address3 = this.String_Address3 + oRow["Zip"].ToString().Trim();
                    }

                }

                this.String_Address4 = "";
                this.String_Address5 = "";
                // Check Date should be assigned.

                this.CheckDate = parameterDateCheckDate;
                this.String_CompanyData = "  ";
                this.String_Description = " ";
                this.String_Description = this.String_Description.PadLeft(39, ' ');
                this.DisbursementDate = this.CheckDate;
                this.String_DisbursementID = parameter_String_DisbursementID;
                // Check if the Disbursement Number is null 
                //<bjr20030818

                this.String_DisbursementNumber = parameter_string_DisbursementNumber;
                //this.String_DisbursementNumber = Convert.ToString(oRow["DisbursementNumber"]);


                this.String_FundID = Convert.ToString(oRow["FundIDNo"]);
                this.String_IndividualID = Convert.ToString(oRow["FundIDNo"]);
                //this.String_IndividualID = (string) oRow["SSNo"];
                if (oRow["MiddleName"].ToString().Trim() == "Guardian of")
                {
                    this.String_Name22 = oRow["FirstName"].ToString();
                    this.String_Name60 = oRow["FirstName"].ToString() + " " + oRow["MiddleName"].ToString() + " " + oRow["LastName"].ToString();
                }
                else
                {
                    this.String_Name22 = oRow["FirstName"].ToString().Trim() + " ";
                    if (oRow["MiddleName"].ToString().Trim() != "")
                    {
                        this.String_Name22 = this.String_Name22 + oRow["MiddleName"].ToString().Trim() + " ";

                    }
                    this.String_Name22 = this.String_Name22 + oRow["Lastname"].ToString().Trim();

                    if (this.String_Name22.Length > 60) this.String_Name60 = this.String_Name22.Substring(0, 60);
                    else this.String_Name60 = this.String_Name22;
                    if (this.String_Name22.Length > 22) this.String_Name22 = this.String_Name22.Substring(0, 22);

                }
                //PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                if (this.bool_OutputFileCShira_MilleTrust == true)
                {

                    this.String_City = "";
                    this.String_State = "";
                    this.String_ZipCode = "";

                    //this.String_FirstName = "";
                    //if (oRow["FirstName"].GetType().ToString() != "System.DBNull" || oRow["FirstName"].ToString() != "")
                    //{
                    //    this.String_FirstName = oRow["FirstName"].ToString();
                    //}
                    //this.String_MiddleName = "";
                    //if (oRow["MiddleName"].GetType().ToString() != "System.DBNull" || oRow["FirstName"].GetType().ToString() != "")
                    //{
                    //    this.String_MiddleName = oRow["MiddleName"].ToString();
                    //}
                    //this.String_LastName = "";
                    //if (oRow["LastName"].GetType().ToString() != "System.DBNull" || oRow["LastName"].GetType().ToString() != "")
                    //{
                    //    this.String_LastName = oRow["LastName"].ToString();
                    //}


                    if (oRow["MiddleName"].ToString().Trim() == "Guardian of")
                    {
                        this.String_Name22 = oRow["FirstName"].ToString();
                        this.String_Name60 = oRow["FirstName"].ToString() + " " + oRow["MiddleName"].ToString() + " " + oRow["LastName"].ToString();

                        //Added By SG: 2012.08.06: BT-960
                        if (!string.IsNullOrEmpty(oRow["FirstName"].ToString()))
                            this.String_FirstName = oRow["FirstName"].ToString();
                        else
                            this.String_FirstName = string.Empty;

                        if (!string.IsNullOrEmpty(oRow["MiddleName"].ToString()))
                            this.String_MiddleName = oRow["MiddleName"].ToString();
                        else
                            this.String_MiddleName = string.Empty;

                        if (!string.IsNullOrEmpty(oRow["Lastname"].ToString()))
                            this.String_LastName = oRow["Lastname"].ToString();
                        else
                            this.String_LastName = string.Empty;
                    }
                    else
                    {
                        //Added By SG: 2012.08.06: BT-960
                        if (!string.IsNullOrEmpty(oRow["FirstName"].ToString()))
                        {
                            this.String_Name22 = oRow["FirstName"].ToString().Trim() + " ";
                            this.String_FirstName = oRow["FirstName"].ToString();
                        }
                        else
                            this.String_FirstName = string.Empty;

                        //if (oRow["MiddleName"].ToString().Trim() !="") 
                        if (!string.IsNullOrEmpty(oRow["MiddleName"].ToString()))
                        {
                            this.String_Name22 = this.String_Name22 + oRow["MiddleName"].ToString().Trim() + " ";
                            this.String_MiddleName = oRow["MiddleName"].ToString();
                        }
                        else
                            this.String_MiddleName = string.Empty;

                        if (!string.IsNullOrEmpty(oRow["Lastname"].ToString()))
                        {
                            this.String_Name22 = this.String_Name22 + oRow["Lastname"].ToString().Trim();
                            this.String_LastName = oRow["Lastname"].ToString();
                        }
                        else
                            this.String_LastName = string.Empty;

                        if (this.String_Name22.Length > 60) this.String_Name60 = this.String_Name22.Substring(0, 60);
                        else this.String_Name60 = this.String_Name22;
                        if (this.String_Name22.Length > 22) this.String_Name22 = this.String_Name22.Substring(0, 22);

                    }


                    this.String_Phone_number = "";
                    if (oRow["Phone_Number"].GetType().ToString() != "System.DBNull" || oRow["Phone_Number"].GetType().ToString() != "")
                    {
                        this.String_Phone_number = oRow["Phone_Number"].ToString();
                    }
                    this.String_Date_of_Birth = "";
                    if (oRow["BirthDate"].GetType().ToString() != "System.DBNull" || oRow["BirthDate"].GetType().ToString() != "")
                    {
                        this.String_Date_of_Birth = oRow["BirthDate"].ToString();
                    }
                    this.String_Tax_id_Number = "";
                    if (oRow["SSNo"].GetType().ToString() != "System.DBNull" || oRow["SSNo"].GetType().ToString() != "")
                    {
                        this.String_Tax_id_Number = oRow["SSNo"].ToString();
                    }
                    this.String_Address2 = "";
                    if (oRow["Addr2"].GetType().ToString() != "System.DBNull" || oRow["Addr2"].ToString() != "")
                    {

                        this.String_Address2 = oRow["Addr2"].ToString();
                    }
                    if (oRow["Addr3"].GetType().ToString() != "System.DBNull" || oRow["Addr3"].ToString() != "")
                    {

                        this.String_Address3 = oRow["Addr3"].ToString();
                    }
                    if (oRow["City"].GetType().ToString() != "System.DBNull" || oRow["City"].GetType().ToString() != "")
                    {
                        this.String_City = oRow["City"].ToString().Trim();
                    }

                    if (oRow["StateType"].GetType().ToString() != "System.DBNull" || oRow["StateType"].GetType().ToString() != "")
                    {
                        this.String_State = oRow["StateType"].ToString().Trim();
                    }

                    if (oRow["Zip"].GetType().ToString() != "System.DBNull" || oRow["Zip"].GetType().ToString() != "")
                    {
                        this.String_ZipCode = oRow["Zip"].ToString().Trim();
                    }

                }

                //this.Double_GrossBalance="";
                //ENd PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                // Monthly Withholding data.
                ln_tmp = ln_tmp1 = ln_tmp2 = 0;
                if (oRow["DisbursementType"].GetType().ToString() != "System.DBNull")
                {
                    if (oRow["DisbursementType"].ToString().Trim() == "ANN" || oRow["DisbursementType"].ToString().Trim() == "EXP")
                    {
                        //MonthlyWithholdings	

                        //this.double_WithholdingMonthTotal = Convert.ToDouble( Parameter_Datatable_WithholdingsByDisbursement.Compute("SUM(Amount)", string.Empty));
                        this.double_WithholdingMonthTotal = SumFields(Parameter_Datatable_WithholdingsByDisbursement, "Amount");

                    }
                    else if (oRow["DisbursementType"].ToString().Trim() == "REF")
                    {
                        //MonthlyWithholdings	

                        //this.double_WithholdingMonthTotal  = Convert.ToDouble( Parameter_Datatable_DisbursementWithholdingsbyRefundDisbursement.Compute("SUM(Amount)", ""));
                        //this.double_inDeductions = Convert.ToDouble( Parameter_Datatable_DisbursementDeductions.Compute("SUM(DeductionDetailTotal)", ""));
                        this.double_WithholdingMonthTotal = SumFields(Parameter_Datatable_DisbursementWithholdingsbyRefundDisbursement, "WithholdingDetailTotal");
                        this.Double_inDeductions = SumFields(Parameter_Datatable_DisbursementDeductions, "DeductionDetailTotal");
                    }
                    else if (oRow["DisbursementType"].ToString().Trim() == "TDLOAN")
                    {
                        //MonthlyWithholdings	

                        //this.double_WithholdingMonthTotal  = Convert.ToDouble( Parameter_Datatable_DisbursementWithholdingsbyRefundDisbursement.Compute("SUM(Amount)", ""));
                        //this.double_inDeductions = Convert.ToDouble( Parameter_Datatable_DisbursementDeductions.Compute("SUM(DeductionDetailTotal)", ""));
                        //this.double_WithholdingMonthTotal = SumFields(Parameter_Datatable_WithholdingsByTDLoan,"WithholdingDetailTotal");
                        this.Double_inDeductions = SumFields(Parameter_Datatable_WithholdingsByTDLoan, "DeductionDetailTotal");
                    }

                }


                if (oRow["TaxableAmount"].GetType().ToString() != "System.DBNull")
                {
                    ln_tmp = Convert.ToDouble(oRow["TaxableAmount"]);

                }


                if (oRow["NonTaxableAmount"].GetType().ToString() != "System.DBNull")
                {
                    ln_tmp2 = Convert.ToDouble(oRow["NonTaxableAmount"]);

                }

                this.double_NetPayMonthTotal = (ln_tmp + ln_tmp2) - (this.double_WithholdingMonthTotal + this.Double_inDeductions);

                if (this.double_NetPayMonthTotal < 0) this.double_NetPayMonthTotal = 0;

                this.String_ReceivingDFEAccount = oRow["BankAcctNumber"].ToString();
                this.TotalMonthlyNet = this.double_NetPayMonthTotal;

                if (oRow["DisbursementType"].GetType().ToString() != "System.DBNull")
                {
                    if (oRow["DisbursementType"].ToString().Trim() == "ANN" || oRow["DisbursementType"].ToString().Trim() == "EXP")
                    {
                        if (oRow["CurrencyCode"].ToString().Trim() == "U") this.String_UsCanadaBankCode = "1";
                        else if (oRow["CurrencyCode"].ToString().Trim() == "C") this.String_UsCanadaBankCode = "2";
                        else this.String_UsCanadaBankCode = "";


                    }
                    else if (oRow["DisbursementType"].ToString().Trim() == "REF")
                    {

                        if (oRow["CurrencyCode"].ToString().Trim() == "U") this.String_UsCanadaBankCode = "3";
                        else if (oRow["CurrencyCode"].ToString().Trim() == "C") this.String_UsCanadaBankCode = "4";
                        else this.String_UsCanadaBankCode = "";

                    }
                    else if (oRow["DisbursementType"].ToString().Trim() == "TDLOAN")
                    {

                        if (oRow["CurrencyCode"].ToString().Trim() == "U") this.String_UsCanadaBankCode = "5";
                        else if (oRow["CurrencyCode"].ToString().Trim() == "C") this.String_UsCanadaBankCode = "6";
                        else this.String_UsCanadaBankCode = "";

                    }

                }

                if (oRow["PaymentMethodCode"].GetType().ToString() != "System.DBNull") this.String_PaymentMethodCode = oRow["PaymentMethodCode"].ToString().Trim();
                else this.String_PaymentMethodCode = "";

                if (this.String_PaymentMethodCode.Trim().ToUpper() != "CHECK")
                {
                    if (oRow["EftTypeCode"].GetType().ToString() != "System.DBNull") this.String_EftTypeCode = oRow["EftTypeCode"].ToString();
                    else this.String_EftTypeCode = "";

                    if (oRow["BankAbaNumber"].GetType().ToString() != "System.DBNull") this.String_BankAbaNumber = oRow["BankAbaNumber"].ToString().Substring(0, 8);
                    else this.String_BankAbaNumber = "";

                    if (oRow["BankAbaNumber"].GetType().ToString() != "System.DBNull") this.String_BankABANumberNinthDigit = oRow["BankAbaNumber"].ToString().Substring(oRow["BankAbaNumber"].ToString().Trim().Length - 1, 1);
                    else this.String_BankABANumberNinthDigit = "";
                }
                //this.PaymentData_Addenda = "";
                //this.AddendaSeqNum_Addenda = "";
                //this.EntryDetailSeqNum_Addenda = "";

                //Get additional withHolding information If ANY




                switch (this.String_OutputFileType)
                {

                    case "CHKSCU":
                        l_String_Guid = this.String_OutputFileCSUSGuid;
                        break;
                    case "CHKSCC":
                        l_String_Guid = this.String_OutputFileCSCanadianGuid;
                        break;
                    case "EFT":
                        l_String_Guid = this.String_OutputFileEFT_NorTrustGuid;
                        break;
                    case "PP":
                        l_String_Guid = this.String_OutputFilePosPayGuid;
                        break;
                    //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    case "SHIRA":
                        l_String_Guid = this.String_OutputFileCShira_MilleTrustGuid;
                        break;
					//START | ML | YRS-AT-4601 | 2019.12.17 | Set guiID to loccal variable
                    case "FSTANN":
                        l_String_Guid = this.String_OutputFileFSTANN_Guid;
                        break;
					//END | ML |  YRS-AT-4601 | 2019.12.17 | Set guiID to loccal variable

                }

                switch (oRow["DisbursementType"].ToString().Trim())
                {
                    case "ANN":
                        #region "ANN"


                        this.String_FilingStatusExemptions = "?-??";
                        l_String_expr = "guiPersID = '" + this.String_PersID + "' and chrTaxEntityType = 'IRS'";

                        foundFedTaxWithholdingRORows = oDataTableFedTaxWithholdingRO.Select(l_String_expr);

                        foreach (DataRow qRow in foundFedTaxWithholdingRORows)
                        {
                            if (qRow["mnyAmount"].GetType().ToString() != "System.DBNull") this.double_WithholdingMonthAddl = Convert.ToDouble(qRow["mnyAmount"]);
                            else this.double_WithholdingMonthAddl = 0;

                            this.String_FilingStatusExemptions = "?-??";
                            lcMaritalStatusCode = "";

                            if (qRow["chrWithholdingType"].GetType().ToString() != "System.DBNull" || qRow["chrWithholdingType"].ToString().Trim() != "")
                            {
                                lcWithholdingType = qRow["chrWithholdingType"].ToString();
                                if (qRow["chrWithholdingType"].GetType().ToString() != "System.DBNull") lcMaritalStatusCode = qRow["chvMaritalStatusCode"].ToString();
                                if (lcMaritalStatusCode.Trim() == "") lcMaritalStatusCode = "U";

                                if (qRow["intExemptions"].GetType().ToString() != "System.DBNull") this.double_Exemptions = Convert.ToDouble(qRow["intExemptions"]);
                                else double_Exemptions = 0;

                                // DSFILINGSTATUSEXEMPTIONSOUTPUT
                                switch (lcWithholdingType.Trim())
                                {
                                    case "FORMUL":
                                        this.String_FilingStatusExemptions = lcMaritalStatusCode.Trim() + "-" + this.double_Exemptions.ToString().Trim().PadLeft(2, '0');
                                        break;
                                    case "FLAT":
                                        this.String_FilingStatusExemptions = "9-99";
                                        break;
                                    default:
                                        this.String_FilingStatusExemptions = "?-??";
                                        break;
                                }

                            }

                            break;
                        }


                        //this.ExpDivMonth = 0;
                        //this.ExpDivYTD = 0;

                        if (oRow["NonTaxableAmount"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthNontaxable = Convert.ToDouble(oRow["NonTaxableAmount"]);
                        else this.double_GrossPayMonthNontaxable = 0;


                        if (oRow["TaxableAmount"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTaxable = Convert.ToDouble(oRow["TaxableAmount"]);
                        else this.double_GrossPayMonthTaxable = 0;

                        this.double_GrossPayMonthTotal = this.double_GrossPayMonthNontaxable + this.double_GrossPayMonthTaxable;

                        this.TotalMonthlyNet = this.double_GrossPayMonthTotal - this.double_WithholdingMonthTotal;

                        this.double_GrossPayYTDNontaxable = 0;
                        this.double_GrossPayYTDTaxable = 0;
                        this.double_NetPayYTDTotal = 0;
                        // YTDDisbursementOutputFileInfo 
                        if (Parameter_Datatable_YTDDisbursementOutputFileInfo.Rows.Count > 0)
                        {
                            YTDRow = Parameter_Datatable_YTDDisbursementOutputFileInfo.Rows[0];

                            if (YTDRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDNontaxable = Convert.ToDouble(YTDRow["YTDPayNonTaxable"]);

                            if (YTDRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDTaxable = Convert.ToDouble(YTDRow["YTDPayTaxable"]);

                            if (YTDRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal = Convert.ToDouble(YTDRow["YTDPayTaxable"]);

                            if (YTDRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal += Convert.ToDouble(YTDRow["YTDPayNonTaxable"]);



                        }

                        this.double_GrossPayYTDNontaxable += this.double_GrossPayMonthNontaxable;
                        this.double_GrossPayYTDTaxable += this.double_GrossPayMonthTaxable;

                        this.double_GrossPayYTDTotal = this.double_GrossPayYTDNontaxable + this.double_GrossPayYTDTaxable;

                        this.double_NetPayMonthTotal = this.TotalMonthlyNet;

                        //Get Year to Date (YTD) WithHoldings
                        //this.double_WithholdingYTDTotal = Convert.ToDouble( Parameter_Datatable_YTDWithholdingOutputFileInfo.Compute("SUM(YTDWithholding)", ""));
                        this.double_WithholdingYTDTotal = SumFields(Parameter_Datatable_YTDWithholdingOutputFileInfo, "YTDWithholding");

                        //if (YTDRow["YTDPayTaxable"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  = Convert.ToDouble(YTDRow["YTDPayTaxable"]); 
                        //else this.double_NetPayYTDTotal     = 0;
                        //if (YTDRow["YTDPayNonTaxable"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  += Convert.ToDouble(YTDRow["YTDPayNonTaxable"]); 
                        //else this.double_NetPayYTDTotal     += 0;

                        this.double_NetPayYTDTotal -= double_WithholdingYTDTotal;

                        this.double_NetPayYTDTotal += this.double_NetPayMonthTotal;

                        this.double_WithholdingYTDTotal += this.double_WithholdingMonthTotal;

                        //Withholdings

                        this.String_WithholdingDetailDesc = "";
                        this.double_WithholdingMonthDetail = 0;
                        this.double_WithholdingYTDDetail = 0;

                        this.Double_NetBalance = this.TotalMonthlyNet;

                        //l_String_expr = "PersID = '" + this.String_PersID   + "'";
                        //foundWithholdingsRows = oDataTableWithholdings.Select(l_String_expr);


                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 1);

                            foreach (DataRow lRow in Parameter_Datatable_WithholdingsByDisbursement.Rows)
                            {
                                if (lRow["Description"].GetType().ToString() != "System.DBNull") this.String_WithholdingDetailDesc = lRow["Description"].ToString();
                                else String_WithholdingDetailDesc = "";

                                if (lRow["Amount"].GetType().ToString() != "System.DBNull") this.double_WithholdingMonthDetail = Convert.ToDouble(lRow["Amount"]);
                                else double_WithholdingMonthDetail = 0;

                                double_WithholdingYTDDetail = double_WithholdingMonthDetail;

                                //'l_String_expr = "WithholdingTypeCode = '" + lRow["WithholdingTypeCode"].ToString().Trim()+ "'" ;

                                //'foundYTDWithholdingsRows = Parameter_Datatable_YTDWithholdingOutputFileInfo.Select(l_String_expr);

                                foreach (DataRow pRow in Parameter_Datatable_YTDWithholdingOutputFileInfo.Rows)
                                {
                                    if (pRow["YTDWithholding"].GetType().ToString() != "System.DBNull") this.double_WithholdingYTDDetail += Convert.ToDouble(pRow["YTDWithholding"]);
                                    else double_WithholdingYTDDetail = 0;
                                    break;
                                }

                                //HTwithHoldings.Add(String_WithholdingDetailDesc,double_WithholdingMonthDetail,double_WithholdingYTDDetail);

                                ProduceOutputFilesCreateDetailMonthlyHolding();
                            }
                            ProduceOutputFilesCreateDetail(CSDetail, 3);
                        }
                        else if (this.String_PaymentMethodCode.Trim() == "EFT")
                        {
                            ProduceOutputFilesCreateDetail(EFTDetail, 1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = "EFT";
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                        }

                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            //ProduceOutputFilesCreateDetail(CSDetail,1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = this.String_OutputFileType.Trim();
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);
                      //START : ML | YRS-AT-4601 | 2019.12.17 | Insert data for FSTANN file in Local datatable
							if (this.CreateOutputFileFSTANN == true)
                            {
                                rowVals[0] = l_String_Guid;
                                rowVals[1] = "FSTANN";
                                rowVals[2] = this.String_DisbursementID.ToString().Trim();
                                DfNewRow = rc.Add(rowVals);
                            }
					//END : ML | YRS-AT-4601 | 2019.12.17 | Insert data for FSTANN file in Local datatable
                            if (this.String_UsCanadaBankCode == "1" || this.String_UsCanadaBankCode == "3")
                            {
                                ProduceOutputFilesCreateDetail(PPDetail, 1);
                                rowVals[0] = l_String_Guid;
                                rowVals[1] = "PP";
                                rowVals[2] = this.String_DisbursementID.ToString().Trim();
                                DfNewRow = rc.Add(rowVals);

                            }

                        }

                        break;
                        #endregion
                    case "REF":

                        RefAddlRow = Parameter_Datatable_DisbursementOutputFileAddlInfo.Rows[0];
                        if (RefAddlRow["Descr"].GetType().ToString() != "System.DBNull") this.String_Descr = RefAddlRow["Descr"].ToString().Trim();
                        if (RefAddlRow["Detail1"].GetType().ToString() != "System.DBNull") this.String_Detail1 = RefAddlRow["Detail1"].ToString().Trim();
                        if (RefAddlRow["Detail2"].GetType().ToString() != "System.DBNull") this.String_Detail2 = RefAddlRow["Detail2"].ToString().Trim();
                        if (RefAddlRow["Detail3"].GetType().ToString() != "System.DBNull") this.String_Detail3 = RefAddlRow["Detail3"].ToString().Trim();
                        if (RefAddlRow["Detail4"].GetType().ToString() != "System.DBNull") this.String_Detail4 = RefAddlRow["Detail4"].ToString().Trim();

                        if (RefAddlRow["beneficiaryOf"].GetType().ToString() != "System.DBNull") this.strBeneficiaryOf = RefAddlRow["beneficiaryOf"].ToString().Trim(); //AA 2016.02.05	YRS-AT-2079

                        // Moved the Code from CSdetail 2 to CsDetail 1 on 12/30/2005

                        if (oRow["WithholdingTaxable"].GetType().ToString() != "System.DBNull") this.Double_WithholdingTaxable = Convert.ToDouble(oRow["WithholdingTaxable"]);
                        if (oRow["WithholdingInterest"].GetType().ToString() != "System.DBNull") this.Double_WithholdingInterest = Convert.ToDouble(oRow["WithholdingInterest"]);
                        if (oRow["NonTaxableAmount"].GetType().ToString() != "System.DBNull") this.Double_NetNonTaxable = Convert.ToDouble(oRow["NonTaxableAmount"]);

                        if (oRow["Taxableprincipal"].GetType().ToString() != "System.DBNull") this.Double_TaxablePrincipal = Convert.ToDouble(oRow["Taxableprincipal"]);
                        if (oRow["TaxWithheldPrincipal"].GetType().ToString() != "System.DBNull") this.Double_TaxableWithHeldPrincipal = Convert.ToDouble(oRow["TaxWithheldPrincipal"]);
                        this.Double_NetTaxable = this.Double_TaxablePrincipal - Double_TaxableWithHeldPrincipal;

                        if (oRow["NetInterest"].GetType().ToString() != "System.DBNull") this.Double_NetInterest = Convert.ToDouble(oRow["NetInterest"]);
                        if (oRow["WithholdingInterest"].GetType().ToString() != "System.DBNull") this.Double_WithholdingInterest = Convert.ToDouble(oRow["WithholdingInterest"]);

                        this.Double_NetInterest -= this.Double_WithholdingInterest;
                        this.Double_NetBalance = this.Double_NetNonTaxable + this.Double_NetTaxable + this.Double_NetInterest;
                        this.Double_NetBalance -= this.Double_inDeductions;

                        this.TotalMonthlyNet = this.Double_NetBalance;


                        //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        //Added another if condition amd put privious if condition in if else
                        if ((this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == false)
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //|| (this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == true && boolIsMRDEligibleProcess == true)
                            )
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 1);
                            //ProduceOutputFilesCreateDetail(SHIRADetails, 4);
                        }
                        //else if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        //{
                        //    ProduceOutputFilesCreateDetail(CSDetail, 1);

                        //}


                        foreach (DataRow RefRow in Parameter_Datatable_DisbursementDetailsbyRefundDisbursement.Rows)
                        {

                            if (RefRow["AcctTypeDesc"].GetType().ToString() != "System.DBNull") this.String_AcctTypeDesc = RefRow["AcctTypeDesc"].ToString();
                            else this.String_AcctTypeDesc = "";

                            if (RefRow["AccountNonTaxable"].GetType().ToString() != "System.DBNull") this.Double_AccountNonTaxable = Convert.ToDouble(RefRow["AccountNonTaxable"]);
                            else this.Double_AccountNonTaxable = 0;

                            if (RefRow["AccountTaxable"].GetType().ToString() != "System.DBNull") this.Double_AccountTaxable = Convert.ToDouble(RefRow["AccountTaxable"]);
                            else this.Double_AccountTaxable = 0;

                            this.Double_AccountTaxableSum += this.Double_AccountTaxable;

                            if (RefRow["AccountInterest"].GetType().ToString() != "System.DBNull") this.Double_AccountInterest = Convert.ToDouble(RefRow["AccountInterest"]);
                            else this.Double_AccountInterest = 0;

                            this.Double_AccountInterestSum += this.Double_AccountInterest;

                            if (RefRow["AccountTotal"].GetType().ToString() != "System.DBNull") this.Double_AccountTotal = Convert.ToDouble(RefRow["AccountTotal"]);
                            else this.Double_AccountTotal = 0;

                            this.Double_AccountTotalSum += this.Double_AccountTotal;


                            //Added By SG: 2012.08.31: BT-960
                            //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                            if ((this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == false)
                                //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                                //|| (this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == true && boolIsMRDEligibleProcess == true)
                                )
                            {
                                ProduceOutputFilesCreateDetailRefundAccounts();
                            }

                        }

                        //this.Double_TaxablePrincipal = this.Double_AccountTaxableSum;
                        this.Double_InterestBalance = this.Double_AccountInterestSum;
                        this.Double_GrossBalance = this.Double_AccountTotalSum;


                        // Moved the Code to CSdetail 1 12/30/2005
                        //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        if ((this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == false)
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //|| (this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == true && boolIsMRDEligibleProcess == true)
                            )
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 2);
                        }



                        foreach (DataRow RefWithRow in Parameter_Datatable_DisbursementWithholdingsbyRefundDisbursement.Rows)
                        {
                            if (RefWithRow["WithholdingDetailDesc"].GetType().ToString() != "System.DBNull") this.String_WithholdingDetailDesc = RefWithRow["WithholdingDetailDesc"].ToString();
                            if (RefWithRow["WithholdingDetailTaxable"].GetType().ToString() != "System.DBNull") this.Double_WithholdingDetailTaxable = Convert.ToDouble(RefWithRow["WithholdingDetailTaxable"]);
                            if (RefWithRow["WithholdingDetailInterest"].GetType().ToString() != "System.DBNull") this.Double_WithholdingDetailInterest = Convert.ToDouble(RefWithRow["WithholdingDetailInterest"]);
                            if (RefWithRow["WithholdingDetailTotal"].GetType().ToString() != "System.DBNull") this.Double_WithholdingDetailTotal = Convert.ToDouble(RefWithRow["WithholdingDetailTotal"]);
                            this.Double_WithholdingTotal += this.Double_WithholdingDetailTotal;

                            //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                            //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                            if ((this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == false)
                                //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                                //|| (this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == true && boolIsMRDEligibleProcess == true)
                                )
                            {
                                ProduceOutputFilesCreateDetailRefundWithHoldings();
                            }




                        }


                        foreach (DataRow RefDeductRow in Parameter_Datatable_DisbursementDeductions.Rows)
                        {

                            if (RefDeductRow["DeductionDetailDesc"].GetType().ToString() != "System.DBNull") this.String_DeductionDetailDesc = RefDeductRow["DeductionDetailDesc"].ToString();
                            if (RefDeductRow["DeductionDetailTaxable"].GetType().ToString() != "System.DBNull") this.Double_DeductionDetailTaxable = Convert.ToDouble(RefDeductRow["DeductionDetailTaxable"]);
                            if (RefDeductRow["DeductionDetailInterest"].GetType().ToString() != "System.DBNull") this.Double_DeductionDetailInterest = Convert.ToDouble(RefDeductRow["DeductionDetailInterest"]);
                            if (RefDeductRow["DeductionDetailTotal"].GetType().ToString() != "System.DBNull") this.Double_DeductionDetailTotal = Convert.ToDouble(RefDeductRow["DeductionDetailTotal"]);
                            this.Double_DeductionTotal += this.Double_DeductionDetailTotal;

                            //Added By SG: 2012.08.31: BT-960
                            //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                            if ((this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == false)
                                //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                                //|| (this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == true && boolIsMRDEligibleProcess == true)
                                )
                            {
                                ProduceOutputFilesCreateDetailRefundDeductions();
                            }




                        }
                        //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                        //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        //Added another if condition amd put privious if condition in if else
                        //if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        if ((this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == false)
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //|| (this.String_PaymentMethodCode.Trim() == "CHECK" && m_bool_ShiraProcess == true && boolIsMRDEligibleProcess == true)
                            )
                        //if (this.String_PaymentMethodCode.Trim() == "CHECK" && bool_OutputFileCShira_MilleTrust == false)
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 3);
                        }

                        if (this.String_PaymentMethodCode.Trim() == "EFT")
                        {
                            ProduceOutputFilesCreateDetail(EFTDetail, 1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = "EFT";
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                        }
                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                            //if (m_bool_ShiraProcess == true && m_bool_IsMRDEligibleProcess == false)
                            if (m_bool_ShiraProcess == true)
                                ProduceOutputFilesCreateDetail(SHIRADetails, 1);

                            //ProduceOutputFilesCreateDetail(CSDetail,1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = this.String_OutputFileType.Trim();
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                            if (this.String_UsCanadaBankCode == "1" || this.String_UsCanadaBankCode == "3")
                            {
                                ProduceOutputFilesCreateDetail(PPDetail, 1);
                                rowVals[0] = l_String_Guid;
                                rowVals[1] = "PP";
                                rowVals[2] = this.String_DisbursementID.ToString().Trim();
                                DfNewRow = rc.Add(rowVals);

                            }

                        }


                        break;
                    case "TDLOAN":

                        RefAddlRow = Parameter_Datatable_DisbursementOutputFileAddlInfo.Rows[0];
                        if (RefAddlRow["Descr"].GetType().ToString() != "System.DBNull") this.String_Descr = RefAddlRow["Descr"].ToString().Trim();
                        if (RefAddlRow["Detail1"].GetType().ToString() != "System.DBNull") this.String_Detail1 = RefAddlRow["Detail1"].ToString().Trim();
                        if (RefAddlRow["Detail2"].GetType().ToString() != "System.DBNull") this.String_Detail2 = RefAddlRow["Detail2"].ToString().Trim();
                        if (RefAddlRow["Detail3"].GetType().ToString() != "System.DBNull") this.String_Detail3 = RefAddlRow["Detail3"].ToString().Trim();
                        if (RefAddlRow["Detail4"].GetType().ToString() != "System.DBNull") this.String_Detail4 = RefAddlRow["Detail4"].ToString().Trim();


                        // Moved the Code from CSdetail 2 to CsDetail 1 on 12/30/2005

                        if (oRow["WithholdingTaxable"].GetType().ToString() != "System.DBNull") this.Double_WithholdingTaxable = Convert.ToDouble(oRow["WithholdingTaxable"]);
                        if (oRow["WithholdingInterest"].GetType().ToString() != "System.DBNull") this.Double_WithholdingInterest = Convert.ToDouble(oRow["WithholdingInterest"]);
                        if (oRow["NonTaxableAmount"].GetType().ToString() != "System.DBNull") this.Double_NetNonTaxable = Convert.ToDouble(oRow["NonTaxableAmount"]);

                        if (oRow["Taxableprincipal"].GetType().ToString() != "System.DBNull") this.Double_TaxablePrincipal = Convert.ToDouble(oRow["Taxableprincipal"]);
                        if (oRow["TaxWithheldPrincipal"].GetType().ToString() != "System.DBNull") this.Double_TaxableWithHeldPrincipal = Convert.ToDouble(oRow["TaxWithheldPrincipal"]);
                        this.Double_NetTaxable = this.Double_TaxablePrincipal - Double_TaxableWithHeldPrincipal;

                        if (oRow["NetInterest"].GetType().ToString() != "System.DBNull") this.Double_NetInterest = Convert.ToDouble(oRow["NetInterest"]);
                        if (oRow["WithholdingInterest"].GetType().ToString() != "System.DBNull") this.Double_WithholdingInterest = Convert.ToDouble(oRow["WithholdingInterest"]);

                        this.Double_NetInterest -= this.Double_WithholdingInterest;
                        this.Double_NetBalance = this.Double_NetNonTaxable + this.Double_NetTaxable + this.Double_NetInterest;
                        this.Double_NetBalance -= this.Double_inDeductions;

                        this.TotalMonthlyNet = this.Double_NetBalance;


                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 1);
                        }
                        // Uncommented the Block 34231
                        foreach (DataRow RefRow in Parameter_Datatable_DisbursementDetailsbyRefundDisbursement.Rows)
                        {

                            if (RefRow["AcctTypeDesc"].GetType().ToString() != "System.DBNull") this.String_AcctTypeDesc = RefRow["AcctTypeDesc"].ToString();
                            else this.String_AcctTypeDesc = "";

                            if (RefRow["AccountNonTaxable"].GetType().ToString() != "System.DBNull") this.Double_AccountNonTaxable = Convert.ToDouble(RefRow["AccountNonTaxable"]);
                            else this.Double_AccountNonTaxable = 0;

                            if (RefRow["AccountTaxable"].GetType().ToString() != "System.DBNull") this.Double_AccountTaxable = Convert.ToDouble(RefRow["AccountTaxable"]);
                            else this.Double_AccountTaxable = 0;

                            this.Double_AccountTaxableSum += this.Double_AccountTaxable;

                            if (RefRow["AccountInterest"].GetType().ToString() != "System.DBNull") this.Double_AccountInterest = Convert.ToDouble(RefRow["AccountInterest"]);
                            else this.Double_AccountInterest = 0;

                            this.Double_AccountInterestSum += this.Double_AccountInterest;

                            if (RefRow["AccountTotal"].GetType().ToString() != "System.DBNull") this.Double_AccountTotal = Convert.ToDouble(RefRow["AccountTotal"]);
                            else this.Double_AccountTotal = 0;

                            this.Double_AccountTotalSum += this.Double_AccountTotal;

                            if (this.String_PaymentMethodCode.Trim() == "CHECK")
                            {
                                ProduceOutputFilesCreateDetailRefundAccounts();
                            }

                        }
                        // Uncommented the Block 34231																															
                        //this.Double_TaxablePrincipal = this.Double_AccountTaxableSum;
                        this.Double_InterestBalance = this.Double_AccountInterestSum;
                        this.Double_GrossBalance = this.Double_AccountTotalSum;

                        // Moved the Code to CSdetail 1 12/30/2005 

                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 2);
                        }

                        ////						foreach (DataRow RefWithRow in Parameter_Datatable_WithholdingsByTDLoan.Rows)
                        ////						{
                        ////							if (RefWithRow["WithholdingDetailDesc"].GetType().ToString()!="System.DBNull")  this.String_WithholdingDetailDesc = RefWithRow["WithholdingDetailDesc"].ToString();  
                        ////							if (RefWithRow["WithholdingDetailTaxable"].GetType().ToString()!="System.DBNull")  this.Double_WithholdingDetailTaxable  = Convert.ToDouble(RefWithRow["WithholdingDetailTaxable"]);  
                        ////							if (RefWithRow["WithholdingDetailInterest"].GetType().ToString()!="System.DBNull")  this.Double_WithholdingDetailInterest  = Convert.ToDouble(RefWithRow["WithholdingDetailInterest"]);  
                        ////							if (RefWithRow["WithholdingDetailTotal"].GetType().ToString()!="System.DBNull")  this.Double_WithholdingDetailTotal  = Convert.ToDouble(RefWithRow["WithholdingDetailTotal"]);  
                        ////							this.Double_WithholdingTotal  += this.Double_WithholdingDetailTotal;
                        ////
                        ////							if (this.String_PaymentMethodCode.Trim()  =="CHECK")	
                        ////							{
                        ////								ProduceOutputFilesCreateDetailRefundWithHoldings();
                        ////							}
                        ////							
                        ////						
                        ////						}


                        foreach (DataRow RefDeductRow in Parameter_Datatable_WithholdingsByTDLoan.Rows)
                        {

                            if (RefDeductRow["DeductionDetailDesc"].GetType().ToString() != "System.DBNull") this.String_DeductionDetailDesc = RefDeductRow["DeductionDetailDesc"].ToString();
                            if (RefDeductRow["DeductionDetailTaxable"].GetType().ToString() != "System.DBNull") this.Double_DeductionDetailTaxable = Convert.ToDouble(RefDeductRow["DeductionDetailTaxable"]);
                            if (RefDeductRow["DeductionDetailInterest"].GetType().ToString() != "System.DBNull") this.Double_DeductionDetailInterest = Convert.ToDouble(RefDeductRow["DeductionDetailInterest"]);
                            if (RefDeductRow["DeductionDetailTotal"].GetType().ToString() != "System.DBNull") this.Double_DeductionDetailTotal = Convert.ToDouble(RefDeductRow["DeductionDetailTotal"]);
                            this.Double_DeductionTotal += this.Double_DeductionDetailTotal;

                            if (this.String_PaymentMethodCode.Trim() == "CHECK")
                            {
                                ProduceOutputFilesCreateDetailRefundDeductions();
                            }


                        }

                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 3);
                        }

                        if (this.String_PaymentMethodCode.Trim() == "EFT")
                        {
                            ProduceOutputFilesCreateDetail(EFTDetail, 1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = "EFT";
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                        }
                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            //ProduceOutputFilesCreateDetail(CSDetail,1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = this.String_OutputFileType.Trim();
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                            if (this.String_UsCanadaBankCode == "1" || this.String_UsCanadaBankCode == "3" || this.String_UsCanadaBankCode == "5")
                            {
                                ProduceOutputFilesCreateDetail(PPDetail, 1);
                                rowVals[0] = l_String_Guid;
                                rowVals[1] = "PP";
                                rowVals[2] = this.String_DisbursementID.ToString().Trim();
                                DfNewRow = rc.Add(rowVals);

                            }

                        }


                        break;

                    case "EXP":
                        this.String_FilingStatusExemptions = "?-??";
                        l_String_expr = "guiPersID = '" + this.String_PersID + "' and chrTaxEntityType = 'IRS'";

                        foundFedTaxWithholdingRORows = oDataTableFedTaxWithholdingRO.Select(l_String_expr);

                        foreach (DataRow qRow in foundFedTaxWithholdingRORows)
                        {
                            if (qRow["mnyAmount"].GetType().ToString() != "System.DBNull") this.double_WithholdingMonthAddl = Convert.ToDouble(qRow["mnyAmount"]);
                            else this.double_WithholdingMonthAddl = 0;

                            this.String_FilingStatusExemptions = "?-??";
                            lcMaritalStatusCode = "";

                            if (qRow["chrWithholdingType"].GetType().ToString() != "System.DBNull" || qRow["chrWithholdingType"].ToString().Trim() != "")
                            {
                                lcWithholdingType = qRow["chrWithholdingType"].ToString();
                                if (qRow["chrWithholdingType"].GetType().ToString() != "System.DBNull") lcMaritalStatusCode = qRow["chvMaritalStatusCode"].ToString();
                                if (lcMaritalStatusCode.Trim() == "") lcMaritalStatusCode = "U";

                                if (qRow["intExemptions"].GetType().ToString() != "System.DBNull") this.double_Exemptions = Convert.ToDouble(qRow["intExemptions"]);
                                else double_Exemptions = 0;

                                // DSFILINGSTATUSEXEMPTIONSOUTPUT
                                switch (lcWithholdingType.Trim())
                                {
                                    case "FORMUL":
                                        this.String_FilingStatusExemptions = lcMaritalStatusCode.Trim() + "-" + this.double_Exemptions.ToString().Trim().PadLeft(2, '0');
                                        break;
                                    case "FLAT":
                                        this.String_FilingStatusExemptions = "9-99";
                                        break;
                                    default:
                                        this.String_FilingStatusExemptions = "?-??";
                                        break;
                                }

                            }

                            break;
                        }


                        //this.ExpDivMonth = 0;
                        //this.ExpDivYTD = 0;

                        if (oRow["NonTaxableAmount"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthNontaxable = Convert.ToDouble(oRow["NonTaxableAmount"]);
                        else this.double_GrossPayMonthNontaxable = 0;


                        if (oRow["TaxableAmount"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTaxable = Convert.ToDouble(oRow["TaxableAmount"]);
                        else this.double_GrossPayMonthTaxable = 0;

                        this.double_GrossPayMonthTotal = this.double_GrossPayMonthNontaxable + this.double_GrossPayMonthTaxable;

                        this.TotalMonthlyNet = this.double_GrossPayMonthTotal - this.double_WithholdingMonthTotal;

                        this.double_GrossPayYTDNontaxable = 0;
                        this.double_GrossPayYTDTaxable = 0;
                        this.double_NetPayYTDTotal = 0;
                        // YTDDisbursementOutputFileInfo 
                        if (Parameter_Datatable_YTDDisbursementOutputFileInfo.Rows.Count > 0)
                        {
                            YTDRow = Parameter_Datatable_YTDDisbursementOutputFileInfo.Rows[0];

                            if (YTDRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDNontaxable = Convert.ToDouble(YTDRow["YTDPayNonTaxable"]);

                            if (YTDRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDTaxable = Convert.ToDouble(YTDRow["YTDPayTaxable"]);

                            if (YTDRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal = Convert.ToDouble(YTDRow["YTDPayTaxable"]);

                            if (YTDRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal += Convert.ToDouble(YTDRow["YTDPayNonTaxable"]);


                        }

                        this.double_GrossPayYTDNontaxable += this.double_GrossPayMonthNontaxable;
                        this.double_GrossPayYTDTaxable += this.double_GrossPayMonthTaxable;

                        this.double_GrossPayYTDTotal = this.double_GrossPayYTDNontaxable + this.double_GrossPayYTDTaxable;

                        this.double_NetPayMonthTotal = this.TotalMonthlyNet;

                        //Get Year to Date (YTD) WithHoldings
                        //this.double_WithholdingYTDTotal = Convert.ToDouble( Parameter_Datatable_YTDWithholdingOutputFileInfo.Compute("SUM(YTDWithholding)", ""));
                        this.double_WithholdingYTDTotal = SumFields(Parameter_Datatable_YTDWithholdingOutputFileInfo, "YTDWithholding");

                        //if (YTDRow["YTDPayTaxable"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  = Convert.ToDouble(YTDRow["YTDPayTaxable"]); 
                        //else this.double_NetPayYTDTotal     = 0;
                        //if (YTDRow["YTDPayNonTaxable"].GetType().ToString() !="System.DBNull") this.double_NetPayYTDTotal  += Convert.ToDouble(YTDRow["YTDPayNonTaxable"]); 
                        //else this.double_NetPayYTDTotal     += 0;

                        this.double_NetPayYTDTotal -= double_WithholdingYTDTotal;

                        this.double_NetPayYTDTotal += this.double_NetPayMonthTotal;

                        this.double_WithholdingYTDTotal += this.double_WithholdingMonthTotal;

                        //Withholdings

                        this.String_WithholdingDetailDesc = "";
                        this.double_WithholdingMonthDetail = 0;
                        this.double_WithholdingYTDDetail = 0;

                        this.Double_NetBalance = this.TotalMonthlyNet;

                        //l_String_expr = "PersID = '" + this.String_PersID   + "'";
                        //foundWithholdingsRows = oDataTableWithholdings.Select(l_String_expr);


                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            ProduceOutputFilesCreateDetail(CSDetail, 1);

                            foreach (DataRow lRow in Parameter_Datatable_WithholdingsByDisbursement.Rows)
                            {
                                if (lRow["Description"].GetType().ToString() != "System.DBNull") this.String_WithholdingDetailDesc = lRow["Description"].ToString();
                                else String_WithholdingDetailDesc = "";

                                if (lRow["Amount"].GetType().ToString() != "System.DBNull") this.double_WithholdingMonthDetail = Convert.ToDouble(lRow["Amount"]);
                                else double_WithholdingMonthDetail = 0;

                                double_WithholdingYTDDetail = double_WithholdingMonthDetail;

                                //'l_String_expr = "WithholdingTypeCode = '" + lRow["WithholdingTypeCode"].ToString().Trim()+ "'" ;

                                //'foundYTDWithholdingsRows = Parameter_Datatable_YTDWithholdingOutputFileInfo.Select(l_String_expr);

                                foreach (DataRow pRow in Parameter_Datatable_YTDWithholdingOutputFileInfo.Rows)
                                {
                                    if (pRow["YTDWithholding"].GetType().ToString() != "System.DBNull") this.double_WithholdingYTDDetail += Convert.ToDouble(pRow["YTDWithholding"]);
                                    else double_WithholdingYTDDetail = 0;
                                    break;
                                }

                                //HTwithHoldings.Add(String_WithholdingDetailDesc,double_WithholdingMonthDetail,double_WithholdingYTDDetail);

                                ProduceOutputFilesCreateDetailMonthlyHolding();
                            }
                            ProduceOutputFilesCreateDetail(CSDetail, 3);
                        }
                        else if (this.String_PaymentMethodCode.Trim() == "EFT")
                        {
                            ProduceOutputFilesCreateDetail(EFTDetail, 1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = "EFT";
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                        }

                        if (this.String_PaymentMethodCode.Trim() == "CHECK")
                        {
                            //ProduceOutputFilesCreateDetail(CSDetail,1);
                            rowVals[0] = l_String_Guid;
                            rowVals[1] = this.String_OutputFileType.Trim();
                            rowVals[2] = this.String_DisbursementID.ToString().Trim();
                            DfNewRow = rc.Add(rowVals);

                            if (this.String_UsCanadaBankCode == "1" || this.String_UsCanadaBankCode == "3")
                            {
                                ProduceOutputFilesCreateDetail(PPDetail, 1);
                                rowVals[0] = l_String_Guid;
                                rowVals[1] = "PP";
                                rowVals[2] = this.String_DisbursementID.ToString().Trim();
                                DfNewRow = rc.Add(rowVals);

                            }

                        }

                        break;
                }

                return true;
            }
            catch
            {
                throw;
            }

        }


        private double SumFields(DataTable Parameter_Datatable, string string_Field_name)
        {
            double l_double_sum = 0.00;

            try
            {

                foreach (DataRow sumRow in Parameter_Datatable.Rows)
                {
                    l_double_sum += Convert.ToDouble(sumRow[string_Field_name]);

                }
                return l_double_sum;
            }
            catch
            {
                return l_double_sum;

            }


        }


        private void InitializseMembers()
        {
            try
            {
                this.String_Address1 = this.String_Address2 = this.String_Address3 = "";
                this.String_Address4 = this.String_Address5 = String_CompanyData = String_Description = String_DisbursementID = "";
                this.String_DisbursementNumber = this.String_FilingStatusExemptions = this.String_FundID = "";
                this.String_IndividualID = this.String_Name22 = this.String_Name60 = this.String_ReceivingDFEAccount = "";
                this.String_EftTypeCode = this.String_BankABANumberNinthDigit = "";
                //	this.PaymentData_Addenda = ""; 
                //this.AddendaSeqNum_Addenda = "";
                //this.EntryDetailSeqNum_Addenda = "";
                //this.String_PersID	= "";

                //Refund Varaibles
                this.String_AcctTypeDesc = string.Empty;
                this.String_Descr = string.Empty;
                this.String_Detail1 = string.Empty;
                this.String_Detail2 = string.Empty;
                this.String_Detail3 = string.Empty;
                this.String_Detail4 = string.Empty;
                this.String_DeductionDetailDesc = string.Empty;

                this.strBeneficiaryOf = string.Empty; //AA 2016.02.05	YRS-AT-2079:

                this.Double_AccountNonTaxable = 0.00;
                this.Double_AccountTaxable = 0.00;
                this.Double_AccountInterest = 0.00;
                this.Double_AccountTotal = 0.00;
                this.Double_AccountTaxableSum = 0.00;
                this.Double_AccountInterestSum = 0.00;
                this.Double_AccountTotalSum = 0.00;
                this.Double_WithholdingDetailTaxable = 0.00;
                this.Double_WithholdingDetailInterest = 0.00;
                this.Double_WithholdingDetailTotal = 0.00;
                this.Double_WithholdingTotal = 0.00;
                this.Double_DeductionDetailTaxable = 0.00;
                this.Double_DeductionDetailInterest = 0.00;
                this.Double_DeductionDetailTotal = 0.00;
                this.Double_DeductionTotal = 0.00;
                this.Double_NetNonTaxable = 0.00;
                this.Double_NetTaxable = 0.00;
                this.Double_NetInterest = 0.00;
                this.Double_NetBalance = 0.00;
                this.Double_WithholdingTaxable = 0.00;
                this.Double_WithholdingInterest = 0.00;
                this.Double_TaxablePrincipal = 0.00;
                this.Double_TaxableWithHeldPrincipal = 0.00;
                this.Double_GrossBalance = 0.00;
                this.Double_inDeductions = 0.00;
                this.Double_AccountTaxableSum = 0.00;
                this.Double_AccountTotalSum = 0.00;
                this.Double_InterestBalance = 0.00;
                this.Double_AccountTaxable = 0.00;
                this.Double_AccountTotal = 0.00;
                this.Double_NetNonTaxable = 0.00;
                this.Double_WithholdingTaxable = 0.00;
                this.Double_NetTaxable = 0.00;

                this.String_DisbursementType = this.String_UsCanadaBankCode = this.String_PaymentMethodCode = this.String_BankAbaNumber = "";

                this.double_NetPayMonthTotal = this.TotalMonthlyNet = 0;

                // PayeePayroll Variables
                this.double_GrossPayMonthNontaxable = this.double_GrossPayMonthTaxable = this.double_GrossPayMonthTotal = this.double_GrossPayYTDNontaxable = 0;
                this.double_GrossPayYTDTaxable = this.double_GrossPayYTDTotal = this.double_NetPayYTDTotal = this.double_WithholdingMonthTotal = 0;
                this.double_WithholdingYTDTotal = this.double_WithholdingMonthAddl = 0;

                this.double_WithholdingMonthDetail = this.double_WithholdingYTDDetail = 0;


                this.String_WithholdingDetailDesc = "";

                //string lcOutputPP_1,lcOutputPP_2,lcOutputEFT_1,lcOutputEFT_2;
                //string lcOutputCursor,String_OutputFileType;

                this.double_Exemptions = 0;
                //Int32 OutputFileFormatCurrency,	OutputFilesPPDetailSum;

                //bool bool_OutputFilePayRoll,OutputFileRefund,OutputFilePosPay,OutputFileCSUS,OutputFileCSCanadian,OutputFileOutputFileRefund,OutputFileEFT_NorTrust; 
            }
            catch
            {
                throw;
            }

        }


        public bool ProduceOutputFilesOutput(DataTable oDataTableOutPutfilePath)
        {
            //StreamOutputFileCSCanadian,StreamOutputFileCSUS,StreamOutputFilePosPay,StreamOutputFileEFT_NorTrust;
            string lcFileName, lcFilenameSuffix, lcOutPutFileNameBak, lcDateSuffix, lcDateTimeSuffix, lcOutputFileName, lcMetaOutputFileType, lcOutputFileNameSuffix;// Chandra sekar | 2016.07.25 | YRS-AT-2386 
            DateTime ld_date;
            DataSet l_dataset = new DataSet();
            bool IsProductionEnvironment = false; // Chandra sekar | 2016.07.25 | YRS-AT-2386 
            DataRowCollection rc;
            DataRow DfNewRow;
            DataSet dsNewGuid;
            int i = -1;
            object[] rowVals = new object[4];
            try
            {

                rc = oDataTableOutPutfilePath.Rows;

                if (this.ProofReport) lcFilenameSuffix = "Proof";
                else lcFilenameSuffix = "";

                l_dataset = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.MetaOutputChkFileType();

                lcFileName = "";
                lcOutPutFileNameBak = "";
                lcMetaOutputFileType = "";
                lcOutputFileNameSuffix = ""; // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                // START- Chandra sekar | 2016.07.25 |  YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
                // Add the "_Test" Word is the suffix BCH File name if other then the Production Environment
                // EXAMPLE : If it is not a production environment then File Name : C072616_Test.bch 
                // EXAMPLE : If Production Environment File Name : C072616.bch
                IsProductionEnvironment = YMCARET.YmcaBusinessObject.YMCACommonBOClass.IsProductionEnvironment();
                if (!IsProductionEnvironment)
                {
                    lcOutputFileNameSuffix = "_Test";
                }
                // END- Chandra sekar | 2016.07.25 |  YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).

                if (l_dataset == null) return false;
                ld_date = DateTime.Now;
                lcDateSuffix = ld_date.ToString("MMddyy").Trim();
                lcDateTimeSuffix = ld_date.ToString("yyyyMMdd").Trim();
                lcDateTimeSuffix += "_" + ld_date.ToString("hhmmss").Trim();

                foreach (DataRow oRow in l_dataset.Tables[0].Rows)
                {
                    lcFileName = oRow["FilenamePrefix"].ToString().Trim();

                    lcMetaOutputFileType = oRow["OutputFileType"].ToString().Trim();

                    if (Convert.ToBoolean(oRow["DateSuffix"]) == true)
                    {
                        lcFileName += lcDateSuffix;
                    }

                    if (Convert.ToBoolean(oRow["PaymentManagerSuffix"]) == true)
                    {
                        lcFileName += lcFilenameSuffix;
                    }

                    if (oRow["OutputDirectory"].GetType().ToString() == "System.DBNull" || oRow["OutputDirectory"].ToString().Trim() == "") return false;

                    // Check whether the directory is existed or not.
                    //					if (!Directory.Exists(oRow["OutputDirectory"].ToString().Trim() ))
                    //					{
                    //						// Throw an exception.
                    //						throw new Exception("Error! Output Directory does not exists. \nFolder: " + oRow["OutputDirectory"].ToString().Trim() + " Not Found"); 
                    //
                    //							
                    //					}

                    //////					lcOutputFileName = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                    //////					lcOutPutFileNameBak = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  

                    //PP:28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    //if(this.bool_OutputFileCSCanadian && lcMetaOutputFileType == "CHKSCC")
                    if ((this.bool_OutputFileCSCanadian && lcMetaOutputFileType == "CHKSCC" && m_bool_ShiraProcess == false)
                        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                        //|| (this.bool_OutputFileCSCanadian && lcMetaOutputFileType == "CHKSCC" && m_bool_ShiraProcess == true && l_int_MRDCnt > 0)
                        )
                    {
                        //commented by ruchi

                        //						this.arrayFileList[i][0] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]);
                        //						this.arrayFileList[i][1] = arrayFileList[i][0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); 
                        //						
                        //						this.arrayFileList[i][2] = oRow["OutputDirectory"].ToString().Trim();
                        //						this.arrayFileList[i][3] = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                        //
                        //						if (!Directory.Exists(arrayFileList[i][0]))
                        //                            {
                        //									// Throw an exception.
                        //									throw new Exception("Error! Output Directory does not exists. \nFolder: " + arrayFileList[i][0].Trim() + " Not Found"); 
                        //																				
                        //							}
                        //						i++;
                        //end of comments

                        //start of change by ruchi for implementation of datatable
                        DataRow l_datarow;
                        //copying the original file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]);
                        //l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); // Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        //START  - Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        // Add the "_Test" Word is the suffix BCH File name if other then the Production Environment
                        // EXAMPLE : If it is not a production environment then File Name : C072616_Test.bch 
                        // EXAMPLE : If Production Environment File Name : C072616.bch 
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        //END  - Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        // l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();// Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        if (!Directory.Exists(l_datarow["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow["SourceFolder"].ToString() + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow);
                        }

                        //copying the backup file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]);
                        //l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();// Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        //l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); // Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        this.l_datatable_FileList.Rows.Add(l_datarow);
                        //end of change
                        //START  - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                       // lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                       // lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        //END  - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        this.StreamOutputFileCSCanadian = File.CreateText(lcOutputFileName);
                        this.StreamOutputFileCSCanadianBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                //rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                                rowVals[2] = lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileCSCanadianGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }
                    }

                    //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    //if(this.bool_OutputFileCSUS && lcMetaOutputFileType == "CHKSCU")
                    if ((this.bool_OutputFileCSUS && lcMetaOutputFileType == "CHKSCU" && m_bool_ShiraProcess == false)
                        //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                        //|| (this.bool_OutputFileCSUS && lcMetaOutputFileType == "CHKSCU" && m_bool_ShiraProcess == true && l_int_MRDCnt > 0)
                        )
                    {
                        i++;

                        ////						this.arrayFileList[i][0] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]);
                        ////						this.arrayFileList[i][1] = arrayFileList[i][0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); 
                        ////						
                        ////						this.arrayFileList[i][2] = oRow["OutputDirectory"].ToString().Trim();
                        ////						this.arrayFileList[i][3] = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                        ////
                        ////						if (!Directory.Exists(arrayFileList[i][0]))
                        ////						{
                        ////							// Throw an exception.
                        ////							throw new Exception("Error! Output Directory does not exists. \nFolder: " + arrayFileList[i][0].Trim() + " Not Found"); 
                        ////																				
                        ////						}
                        ///
                        //start of change by ruchi for implementation of datatble
                        DataRow l_datarow;
                        //copying the original file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]);
                        // l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        //l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                        if (!Directory.Exists(l_datarow["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow["SourceFolder"].ToString() + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow);
                        }

                        //copying the backup file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]);
                        //l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        //l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.l_datatable_FileList.Rows.Add(l_datarow);
                        //end of change
                        //START - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        //lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        //lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        //END - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFileCSUS = File.CreateText(lcOutputFileName);
                        this.StreamOutputFileCSUSBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {
                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                //rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileCSUSGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }
                    }

                    if (this.bool_OutputFilePosPay && lcMetaOutputFileType == "PP")
                    {
                        ////						this.arrayFileList[i][0] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PP"]) ;
                        ////						this.arrayFileList[i][1] = arrayFileList[i][0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); 
                        ////						
                        ////						this.arrayFileList[i][2] = oRow["OutputDirectory"].ToString().Trim();
                        ////						this.arrayFileList[i][3] = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                        ////
                        ////						if (!Directory.Exists(arrayFileList[i][0]))
                        ////						{
                        ////							// Throw an exception.
                        ////							throw new Exception("Error! Output Directory does not exists. \nFolder: " + arrayFileList[i][0].Trim() + " Not Found"); 
                        ////																				
                        ////						}
                        ////						i++;
                        //start of change by ruchi for implementation of datatble
                        DataRow l_datarow;
                        //copying the original file
                        l_datarow = this.l_datatable_FileList.NewRow();

                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PP"]);
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        if (!Directory.Exists(l_datarow["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow["SourceFolder"].ToString() + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow);
                        }

                        //copying the backup file
                        l_datarow = this.l_datatable_FileList.NewRow();

                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PP"]);
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.l_datatable_FileList.Rows.Add(l_datarow);
                        //end of change

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PP"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["PP"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFilePosPay = File.CreateText(lcOutputFileName);
                        this.StreamOutputFilePosPayBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();
                        if (dsNewGuid != null)
                        {
                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFilePosPayGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }
                    }

                    if (this.bool_OutputFileEFT_NorTrust && lcMetaOutputFileType == "EFT")
                    {
                        ////						this.arrayFileList[i][0] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]);
                        ////						this.arrayFileList[i][1] = arrayFileList[i][0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); 
                        ////						
                        ////						this.arrayFileList[i][2] = oRow["OutputDirectory"].ToString().Trim();
                        ////						this.arrayFileList[i][3] = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                        ////
                        ////						if (!Directory.Exists(arrayFileList[i][0]))
                        ////						{
                        ////							// Throw an exception.
                        ////							throw new Exception("Error! Output Directory does not exists. \nFolder: " + arrayFileList[i][0].Trim() + " Not Found"); 
                        ////																				
                        ////						}
                        ////						i++;
                        //start of change by ruchi for implementation of datatble

                                             

                        DataRow l_datarow;
                        //copying the original file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]);
                        // START | SR | 2019.01.11 | YRS-AT-4282 | Change EFT file name as EFT_DisbursementType(Ex. EFT_TDLOAN.dat)
                        //l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + DisbursementType + "." + oRow["FilenameExtension"].ToString().Trim();
                        // END | SR | 2019.01.11 | YRS-AT-4282 | Change EFT file name as EFT_DisbursementType(Ex. EFT_TDLOAN.dat)
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        // START | SR | 2019.01.11 | YRS-AT-4282 | Change EFT file name as EFT_DisbursementType(Ex. EFT_TDLOAN.dat)
                        //l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + DisbursementType + "." + oRow["FilenameExtension"].ToString().Trim();
                        // END | SR | 2019.01.11 | YRS-AT-4282 | Change EFT file name as EFT_DisbursementType(Ex. EFT_TDLOAN.dat)
                        if (!Directory.Exists(l_datarow["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow["SourceFolder"].ToString() + " Not Found");

                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow);
                        }

                        //copying the backup file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]);
                        // START | SR | 2018.04.06 | YRS-AT-3101 | Change EFT file name as EFT_TDLOAN_yyyymmddxx.dat
                        //l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + DisbursementType + "_" + EFTBatchId.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        //l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + DisbursementType + "_" + EFTBatchId.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        // END | SR | 2018.04.06 | YRS-AT-3101 | Change EFT file name as EFT_TDLOAN_yyyymmddxx.dat
                        this.l_datatable_FileList.Rows.Add(l_datarow);
                        //end of change

                        // START | SR | 2019.01.11 | YRS-AT-4282 | commented existing code & added new line to change primary EFT file name as EFT_TDLOAN.dat
                        //lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]) + "\\" + lcFileName.Trim() +  "." + oRow["FilenameExtension"].ToString().Trim();
                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "_" + DisbursementType + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        // END | SR | 2019.01.11 | YRS-AT-4282 | commented existing code & added new line to Change primary EFT file name as EFT_TDLOAN.dat
                        // START | SR | 2018.04.06 | YRS-AT-3101 | commented existing code & added new line to Change EFT file name as EFT_TDLOAN_yyyymmddxx.dat
                        //lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "_" + DisbursementType + "_" + EFTBatchId.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        // END | SR | 2018.04.06 | YRS-AT-3101 | commented existing code & added new line to Change EFT file name as EFT_TDLOAN_yyyymmddxx.dat

                        this.StreamOutputFileEFT_NorTrust = File.CreateText(lcOutputFileName.Trim());
                        this.StreamOutputFileEFT_NorTrustBack = File.CreateText(lcOutPutFileNameBak.Trim());

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {
                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileEFT_NorTrustGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }
                    }

                    #region PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    if (this.bool_OutputFileCShira_MilleTrust && lcMetaOutputFileType == "SHIRA" && m_bool_ShiraProcess == true && l_int_MilleCnt > 0)
                    {
                        lcFileName += "_" + ld_date.ToString("MM").Trim() + "_" + ld_date.ToString("dd").Trim() + "_" + ld_date.ToString("yyyy").Trim();

                        DataRow l_datarow;
                        //copying the original file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SHIRA"]);
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        if (!Directory.Exists(l_datarow["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow["SourceFolder"].ToString() + " Not Found");

                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow);
                        }

                        //copying the backup file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SHIRA"]);
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + ld_date.ToString("hhmmss").Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + ld_date.ToString("hhmmss").Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.l_datatable_FileList.Rows.Add(l_datarow);
                        //end of change

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SHIRA"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["SHIRA"]) + "\\" + lcFileName.Trim() + "_" + ld_date.ToString("hhmmss").Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFileCShira_MilleTrust = File.CreateText(lcOutputFileName.Trim());
                        this.StreamOutputFileCShira_MilleTrustBack = File.CreateText(lcOutPutFileNameBak.Trim());

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {
                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileCShira_MilleTrustGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }
                    }
                    #endregion PP 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000

                    // START : ML | 2019.10.22 | YRS-AT-4601 | Define file name and location for First Annuity Payment for US Participant.
                    if ((this.bool_OutputFileCSUS && lcMetaOutputFileType == "FSTANN" && m_bool_ShiraProcess == false))
                    {
                        //start of change by ruchi for implementation of datatable
                        DataRow l_datarow;
                        //copying the original file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FSTANN"]);
                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        if (!Directory.Exists(l_datarow["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow["SourceFolder"].ToString() + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow);
                        }

                        //copying the backup file
                        l_datarow = this.l_datatable_FileList.NewRow();
                        l_datarow["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FSTANN"]);

                        l_datarow["SourceFile"] = l_datarow["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();

                        l_datarow["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        this.l_datatable_FileList.Rows.Add(l_datarow);
                        
                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FSTANN"]) + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["FSTANN"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); // Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        this.StreamOutputFileFstAnn = File.CreateText(lcOutputFileName);
                        this.StreamOutputFileFstAnnBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileFSTANN_Guid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }
                    }
                    // END : ML | 2019.10.22 | YRS-AT-4601 | Define file name and location for First Annuity Payment for US Participant.
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void WriteOutputFilesDataLineIntoStream(string parameterStringOutput)
        {
            string l_String_Output = parameterStringOutput;

            try
            {

                switch (this.String_OutputFileType)
                {
                    case "CHKSCU":
                        StreamOutputFileCSUS.WriteLine(l_String_Output);
                        StreamOutputFileCSUSBack.WriteLine(l_String_Output);
                        break;
                    case "CHKSCC":
                        StreamOutputFileCSCanadian.WriteLine(l_String_Output);
                        StreamOutputFileCSCanadianBack.WriteLine(l_String_Output);
                        break;
                    case "EFT":
                        StreamOutputFileEFT_NorTrust.WriteLine(l_String_Output);
                        StreamOutputFileEFT_NorTrustBack.WriteLine(l_String_Output);
                        break;
                    case "PP":
                        StreamOutputFilePosPay.WriteLine(l_String_Output);
                        StreamOutputFilePosPayBack.WriteLine(l_String_Output);
                        break;
                    //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                    case "SHIRA":
                        StreamOutputFileCShira_MilleTrust.WriteLine(l_String_Output);
                        StreamOutputFileCShira_MilleTrustBack.WriteLine(l_String_Output);
                        break;
					//START :ML |2019.11.18 |YRS-AT-4601 | Write line to Output file.
                    case "FSTANN":
                        StreamOutputFileFstAnn.WriteLine(l_String_Output);
                        StreamOutputFileFstAnnBack.WriteLine(l_String_Output);
                        break;
					//END :ML |2019.11.18 |YRS-AT-4601 |Write line to Output file.
                }
            }
            catch
            {

                throw;
            }

        }


        public void CloseOutputStreamFiles()
        {
            try
            {
                ////Added By SG: 2012.09.05: BT-960
                //if (this.bool_OutputFileCSCanadian)
                if ((this.bool_OutputFileCSCanadian && m_bool_ShiraProcess == false)
                    //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                    //|| (this.bool_OutputFileCSCanadian && m_bool_ShiraProcess == true && l_int_MRDCnt > 0)
                    )
                {

                    this.StreamOutputFileCSCanadian.Close();

                    this.StreamOutputFileCSCanadianBack.Close();

                }
                //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                //Added && condition
                //if(this.bool_OutputFileCSUS)
                if ((this.bool_OutputFileCSUS && m_bool_ShiraProcess == false)
                    //Commented By SG: 2012.12.19: BT-1524: Removing RMD check in payment manager page
                    //|| (this.bool_OutputFileCSUS && m_bool_ShiraProcess == true && l_int_MRDCnt > 0)
                    )
                //if (this.bool_OutputFileCSUS && l_int_MilleCnt <= 0)
                {

                    this.StreamOutputFileCSUS.Close();

                    this.StreamOutputFileCSUSBack.Close();
                }

                if (this.bool_OutputFilePosPay)
                {
                    this.StreamOutputFilePosPay.Close();

                    this.StreamOutputFilePosPayBack.Close();
                }

                if (this.bool_OutputFileEFT_NorTrust)
                {
                    this.StreamOutputFileEFT_NorTrust.Close();

                    this.StreamOutputFileEFT_NorTrustBack.Close();

                }

                //28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                if (StreamOutputFileCShira_MilleTrustBack != null)
                {
                    this.StreamOutputFileCShira_MilleTrust.Close();

                    this.StreamOutputFileCShira_MilleTrustBack.Close();
                }

                //if (this.bool_OutputFileCShira_MilleTrust && l_int_MilleCnt > 0)
                //{
                //    this.StreamOutputFileCShira_MilleTrust.Close();

                //    this.StreamOutputFileCShira_MilleTrustBack.Close();
                //}
                //END 28.09.2012 BT:-960,YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
            }
            catch
            {
                throw;

            }
        }
		//START :ML |2019.11.18 |YRS-AT-4601 |Write DataTable to Output file.
        public void WriteOutputFilesDataTableIntoStream(DataTable dtOutput)
        {
            try
            {
                if (HelperFunctions.isNonEmpty(dtOutput))
                {
                    this.String_OutputFileType = "FSTANN";   
                    foreach (DataRow dtRow in dtOutput.Rows)
                    {
                        WriteOutputFilesDataLineIntoStream(dtRow[0].ToString());             
                    }
                    if (StreamOutputFileFstAnn != null)
                    {
                        this.StreamOutputFileFstAnn.Close();
                        this.StreamOutputFileFstAnnBack.Close();
                    }
                }                 
            }
            catch 
            {
                throw;
            }
        }
		//END :ML |2019.11.18 |YRS-AT-4601 |Write DataTable to Output file.
        public void IdentifyPaymentTypes(DataTable ParameterDataTable, string parameter_string_Expr)
        {
            try
            {
                DataTable l_datatable;
                string l_String_expr = string.Empty;
                DataRow[] foundRows;

                l_datatable = ParameterDataTable;

                //Poyroll
                /*
                if (this.String_DisbursementType =="ANN")
                {
                    this.bool_OutputFilePayRoll = true;
                    this.bool_OutputFileRefund = false;
                }
                else if (this.String_DisbursementType == "REF")
                {
                    this.bool_OutputFilePayRoll = false;
                    this.bool_OutputFileRefund = true;
                }
                */

                //Canadian Checkscribe
                l_String_expr = parameter_string_Expr;
                l_String_expr += " and CurrencyCode = 'C' and PaymentMethodCode = 'CHECK' ";
                foundRows = l_datatable.Select(l_String_expr);
                if (foundRows.Length > 0) this.bool_OutputFileCSCanadian = true;
                else this.bool_OutputFileCSCanadian = false;

                //US Checkscribe
                l_String_expr = parameter_string_Expr;
                l_String_expr += " and  CurrencyCode = 'U' and PaymentMethodCode = 'CHECK' ";
                foundRows = l_datatable.Select(l_String_expr);
                if (foundRows.Length > 0) this.bool_OutputFileCSUS = true;
                else this.bool_OutputFileCSUS = false;

                // EFT (Northern Trust) Files
                l_String_expr = parameter_string_Expr;
                l_String_expr += " and PaymentMethodCode = 'EFT' ";
                foundRows = l_datatable.Select(l_String_expr);
                if (foundRows.Length > 0) this.bool_OutputFileEFT_NorTrust = true;
                else this.bool_OutputFileEFT_NorTrust = false;

                //Positive Pay
                if (this.bool_OutputFileCSUS || this.bool_OutputFileCSCanadian) this.bool_OutputFilePosPay = true;
                else this.bool_OutputFilePosPay = false;
            }
            catch
            {
                throw;

            }
        }


        public double Property_double_NetBalance
        {
            get
            {
                return Double_NetBalance;
            }
            set
            {
                Double_NetBalance = value;
            }
        }


        public DataTable DataTableNameFileList
        {
            get
            {
                return l_datatable_FileList;
            }
            set
            {
                l_datatable_FileList = value;
            }
        }

   

    }

}
