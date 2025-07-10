/****************************************************************************************/
//Modified History
/****************************************************************************************/
//Shubhrata JAN 19th 2007 YREN-3025 YmcaPreTax was not getting summed up with PersonalPreTax in atsRefunds
//Priya 16-Jan-2009 : YRS 5.0-637 and YRS 5.0-672 AC Account interest components added new constant for AC account type group.
//Ashish 2010.05.07: YRS 5.0-1085 ,BT-526 resolve Cashout lock isuue

//Sanjay R.: 2010.07.09    - Enhancement changes.
//2012.06.14        Sanjeev(SG)         BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2012.11.26        Sanjeev(SG)         BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2014.01.07        Dinesh.k            BT-2032: YRS 5.0-2084: DLIN not converted to ININ by cash out process
//2014.09.10        Dinesh.k            BT:2437: Cashout process not inserting records in AtsMrdRecordsDisbursements for RMD eligible participants.
//2014.12.09        Dinesh Kanojia      BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
//2015.09.16        Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2016.06.06        Anudeep A           YRS-AT-3015 - YRS enh: Configurable Withdrawals project (YRSwebService/yrfWebsite)
/****************************************************************************************/
using System;
using System.IO;
using System.Data;
using System.Security.Permissions;
using System.Collections;
using System.Globalization;
using System.Data.SqlClient;
//using YMCAObjects;

namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for CashOutProcessBOClass.
    /// </summary>
    public class CashOutProcessBOClass
    {
        YMCARET.YmcaDataAccessObject.CashOutDAClass m_objCashOutDAClass = null;
        DataSet m_dataset_RefundRequest = new DataSet();
        DataSet m_dataset_RefundRequestProcessing = new DataSet();
        DataTable m_datatable_MemberAccountContributionProcessing;
        DataTable m_datatable_CalculatedDataTableForCurrentAccounts;
        DataTable m_datatable_CalculatedDataTableForRefundable;
        DataTable m_datatable_AvailableBalance;
        DataTable m_datatable_Refund_Transactions;
        DataTable m_datatable_Refund_Disbursements;
        DataTable m_datatable_Refund_DisbursementDetails;
        DataTable m_datatable_Refund_DisbursementWithholding;
        DataTable m_datatable_Refund__DisbursementRefunds;
        DataTable m_datatable_Payee1;
        DataTable m_datatable_RequestedAccounts;
        DataTable m_datatable_RefundRequestTable;
        DataTable m_RefundableDataTable;
        DataTable m_datatable_MemberAccountBreakdown;
        DataTable m_ArrayRefundDataTable;
        DataTable m_MinimumDistributionTable;
        DataTable m_datatable_CalculateContribution;
        DataTable m_datatable_AtsRefund;
        DataTable m_datatable_AtsRefRequests;
        DataTable m_datatable_AtsRefRequestDetails;
        string m_string_RefRequestId;
        string[] m_string_AnnuityBasisType;
        string m_string_FundEventId;
        string m_string_PersonId;
        string m_string_CurrencyCode;
        string m_string_GetPayeeName;
        bool m_bool_IsMemberVested;
        bool m_bool_IsTerminated;
        bool m_bool_CheckForTerminationDate;
        decimal m_decimal_TerminationPIA;
        decimal m_decimal_MaxPIAAmount;
        decimal m_decimal_YmcaAvailableAmount;
        decimal m_decimal_MinimumDistributionAmount;
        decimal m_decimal_MinimumDistributionTaxRate;
        decimal m_decimal_TaxRateForPayee;

        decimal m_decimal_PersonAge;

        //START - Retirement plan account groups
        const string m_const_RetirementPlan_AP = "RAP";
        const string m_const_RetirementPlan_AM = "RAM";
        const string m_const_RetirementPlan_RG = "RRG";
        const string m_const_RetirementPlan_SA = "RSA";
        const string m_const_RetirementPlan_SS = "RSS";
        const string m_const_RetirementPlan_RP = "RRP";
        const string m_const_RetirementPlan_SR = "RSR";
        //Priya 16-Jan-2009 : YRS 5.0-637 AC Account interest components added new constant for AC account type group.
        const string m_const_RetirementPlan_AC = "RAC";
        //End 16-Jan-2009 

        //END - Retirement plan account groups
        //START - Ssvings plan account groups
        const string m_const_SavingsPlan_TD = "STD";
        const string m_const_SavingsPlan_TM = "STM";
        const string m_const_SavingsPlan_RT = "SRT";
        //END - Savings plan account groups
        //plan split variables
        public CashOutProcessBOClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public bool BoolIsTerminated
        {
            get
            {
                return m_bool_IsTerminated;
            }
            set
            {
                m_bool_IsTerminated = value;
            }
        }
        public bool BoolCheckForTerminationDate
        {
            get
            {
                return m_bool_CheckForTerminationDate;
            }
            set
            {
                m_bool_CheckForTerminationDate = value;
            }
        }

        public string StringFundEventId
        {
            get
            {
                return m_string_FundEventId;
            }
            set
            {
                m_string_FundEventId = value;
            }
        }
        public string StringGetPayeeName
        {
            get
            {
                return m_string_GetPayeeName;
            }
            set
            {
                m_string_GetPayeeName = value;
            }
        }

        public string StringPersonId
        {
            get
            {
                return m_string_PersonId;
            }
            set
            {
                m_string_PersonId = value;
            }
        }
        public string StringRefRequestId
        {
            get
            {
                return m_string_RefRequestId;
            }
            set
            {
                m_string_RefRequestId = value;
            }
        }
        public string StringCurrencyCode
        {
            get
            {
                return m_string_CurrencyCode;
            }
            set
            {
                m_string_CurrencyCode = value;
            }
        }

        public string[] AnnuityBasisType
        {
            get
            {
                return m_string_AnnuityBasisType;
            }
            set
            {
                m_string_AnnuityBasisType = value;
            }
        }

        public decimal DecimalTerminationPIA
        {
            get
            {
                return m_decimal_TerminationPIA;
            }
            set
            {
                m_decimal_TerminationPIA = value;
            }
        }
        public decimal DecimalMinimumDistributionTaxRate
        {
            get
            {
                return m_decimal_MinimumDistributionTaxRate;
            }
            set
            {
                m_decimal_MinimumDistributionTaxRate = value;
            }
        }

        public decimal DecimalMinimumDistributionAmount
        {
            get
            {
                return m_decimal_MinimumDistributionAmount;
            }
            set
            {
                m_decimal_MinimumDistributionAmount = value;
            }
        }

        public decimal DecimalYmcaAvailableAmount
        {
            get
            {
                return m_decimal_YmcaAvailableAmount;
            }
            set
            {
                m_decimal_YmcaAvailableAmount = value;
            }
        }

        public bool BoolIsMemberVested
        {
            get
            {
                return m_bool_IsMemberVested;
            }
            set
            {
                m_bool_IsMemberVested = value;
            }
        }
        public decimal DecimalMaxPIAAmount
        {
            get
            {
                return m_decimal_MaxPIAAmount;
            }
            set
            {
                m_decimal_MaxPIAAmount = value;
            }
        }
        public decimal DecimalPersonAge
        {
            get
            {
                return m_decimal_PersonAge;
            }
            set
            {
                m_decimal_PersonAge = value;
            }
        }
        public decimal DecimalTaxRateForPayee
        {
            get
            {
                return m_decimal_TaxRateForPayee;
            }
            set
            {
                m_decimal_TaxRateForPayee = value;
            }
        }
        public DataSet DataSetRefundRequest
        {
            get
            {
                return m_dataset_RefundRequest;
            }
            set
            {
                m_dataset_RefundRequest = value;
            }
        }
        public DataSet DataSetRefundRequestProcessing
        {
            get
            {
                return m_dataset_RefundRequestProcessing;
            }
            set
            {
                m_dataset_RefundRequestProcessing = value;
            }
        }
        public DataTable DataTableCalculateContribution
        {
            get
            {
                return m_datatable_CalculateContribution;
            }
            set
            {
                m_datatable_CalculateContribution = value;
            }
        }
        public DataTable DataTableArrayRefundDataTable
        {
            get
            {
                return m_ArrayRefundDataTable;
            }
            set
            {
                m_ArrayRefundDataTable = value;
            }
        }
        public DataTable DataTableRefundRequestTable
        {
            get
            {
                return m_datatable_RefundRequestTable;
            }
            set
            {
                m_datatable_RefundRequestTable = value;
            }
        }


        public DataTable DataTableAtsRefund
        {
            get
            {
                return m_datatable_AtsRefund;
            }
            set
            {
                m_datatable_AtsRefund = value;
            }
        }
        public DataTable DataTableAtsRefRequests
        {
            get
            {
                return m_datatable_AtsRefRequests;
            }
            set
            {
                m_datatable_AtsRefRequests = value;
            }
        }
        public DataTable DataTableAtsRefRequestDetails
        {
            get
            {
                return m_datatable_AtsRefRequestDetails;
            }
            set
            {
                m_datatable_AtsRefRequestDetails = value;
            }
        }


        public YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass
        {
            //			get
            //			{
            //				return null;
            //			}
            set
            {
                m_objCashOutDAClass = value;
            }
        }
        public DataTable DataTableMinimumDistributionTable
        {
            get
            {
                return m_MinimumDistributionTable;
            }
            set
            {
                m_MinimumDistributionTable = value;
            }
        }

        public DataTable DataTableAvailableBalance
        {
            get
            {
                return m_datatable_AvailableBalance;
            }
            set
            {
                m_datatable_AvailableBalance = value;
            }
        }
        public DataTable DataTableRefundableDataTable
        {
            get
            {
                return m_RefundableDataTable;
            }
            set
            {
                m_RefundableDataTable = value;
            }
        }

        public DataTable DataTablePayee1
        {
            get
            {
                return m_datatable_Payee1;
            }
            set
            {
                m_datatable_Payee1 = value;
            }
        }
        public DataTable DataTableRefundDisbursements
        {
            get
            {
                return m_datatable_Refund_Disbursements;
            }
            set
            {
                m_datatable_Refund_Disbursements = value;
            }
        }
        public DataTable DataTableRequestedAccounts
        {
            get
            {
                return m_datatable_RequestedAccounts;
            }
            set
            {
                m_datatable_RequestedAccounts = value;
            }
        }
        public DataTable DataTableRefundDisbursementDetails
        {
            get
            {
                return m_datatable_Refund_DisbursementDetails;
            }
            set
            {
                m_datatable_Refund_DisbursementDetails = value;
            }
        }
        public DataTable DataTableRefundDisbursementWithholding
        {
            get
            {
                return m_datatable_Refund_DisbursementWithholding;
            }
            set
            {
                m_datatable_Refund_DisbursementWithholding = value;
            }
        }
        public DataTable DataTableRefundDisbursementRefunds
        {
            get
            {
                return m_datatable_Refund__DisbursementRefunds;
            }
            set
            {
                m_datatable_Refund__DisbursementRefunds = value;
            }
        }
        public DataTable DataTableRefundTransactions
        {
            get
            {
                return m_datatable_Refund_Transactions;
            }
            set
            {
                m_datatable_Refund_Transactions = value;
            }
        }
        public DataTable DataTableMemberAccountBreakdown
        {
            get
            {
                return m_datatable_MemberAccountBreakdown;
            }
            set
            {
                m_datatable_MemberAccountBreakdown = value;
            }
        }
        public DataTable DataTableMemberAccountContributionProcessing
        {
            get
            {
                return m_datatable_MemberAccountContributionProcessing;
            }
            set
            {
                m_datatable_MemberAccountContributionProcessing = value;
            }
        }
        public DataTable DataTableCalculatedDataTableForRefundable
        {
            get
            {
                return m_datatable_CalculatedDataTableForRefundable;
            }
            set
            {
                m_datatable_CalculatedDataTableForRefundable = value;
            }
        }

        public DataTable DataTableCalculatedDataTableForCurrentAccounts
        {
            get
            {
                return m_datatable_CalculatedDataTableForCurrentAccounts;
            }
            set
            {
                m_datatable_CalculatedDataTableForCurrentAccounts = value;
            }
        }

        //Added By SG: 2012.06.14: BT-960
        string m_StringCashOutRefundType;
        public string CashOutRefundType
        {
            get
            {
                return m_StringCashOutRefundType;
            }
            set
            {
                m_StringCashOutRefundType = value;
            }
        }

        //Added By SG: 2012.08.28: BT-960
        bool m_boolIsRMDeligible;
        public bool IsRMDeligible
        {
            get
            {
                return m_boolIsRMDeligible;
            }
            set
            {
                m_boolIsRMDeligible = value;
            }
        }

        //Added By SG: 2012.08.29: BT-960
        string m_string_PlanType;
        public string PlanType
        {
            get
            {
                return m_string_PlanType;
            }
            set
            {
                m_string_PlanType = value;
            }
        }

        DataTable m_datatable_Refund_MrdRecordsDisbursements;
        public DataTable DataTablerefund_MrdRecordsDisbursements
        {
            get
            {
                return m_datatable_Refund_MrdRecordsDisbursements;
            }
            set
            {
                m_datatable_Refund_MrdRecordsDisbursements = value;
            }
        }

        DataTable m_RmdRecordsDataTable;
        public DataTable RmdRecordsDataTable
        {
            get { return m_RmdRecordsDataTable; }
            set { m_RmdRecordsDataTable = value; }
        }

        private DataTable ClearZerosRows(DataTable parameterDataTable)
        {

            decimal l_Decimal_Taxable;
            decimal l_Decimal_NonTaxable;
            bool l_BooleanFlag = false;
            try
            {
                if (parameterDataTable == null)
                {
                    return null;
                }
                parameterDataTable.AcceptChanges();
                foreach (DataRow l_DataRow in parameterDataTable.Rows)
                {
                    if (l_DataRow["Taxable"].GetType().ToString() == "System.DBNull")
                    {
                        l_Decimal_Taxable = 0;
                    }
                    else
                    {
                        l_Decimal_Taxable = Convert.ToDecimal(l_DataRow["Taxable"]);
                    }
                    if (l_DataRow["NonTaxable"].GetType().ToString() == "System.DBNull")
                    {
                        l_Decimal_NonTaxable = 0;
                    }
                    else
                    {
                        l_Decimal_NonTaxable = Convert.ToDecimal(l_DataRow["NonTaxable"]);
                    }
                    if ((l_Decimal_Taxable + l_Decimal_NonTaxable) == 0)
                    {
                        l_DataRow.Delete();
                        l_BooleanFlag = true;
                    }
                }
                if (l_BooleanFlag == true)
                {
                    parameterDataTable.AcceptChanges();
                }
                return parameterDataTable;
            }
            catch
            {
                throw;
            }
        }
        private void CopyAccountContributionTableForCurrentAccounts()
        {
            DataTable l_AccountContributionTable;
            DataTable l_CalculationDataTable;

            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopyAccountContributionTableForCurrentAccounts Method", "Start: Copy AccountContribution For CurrentAccounts.");
                l_AccountContributionTable = this.m_datatable_MemberAccountContributionProcessing;
                if (l_AccountContributionTable != null)
                {
                    l_CalculationDataTable = l_AccountContributionTable.Clone();

                    foreach (DataRow l_DataRow in l_AccountContributionTable.Rows)
                    {
                        if (l_DataRow["AccountGroup"].GetType().ToString() != "System.DBNull")
                        {
                            if (Convert.ToString(l_DataRow["AccountGroup"]) != "Total")
                            {
                                l_CalculationDataTable.ImportRow(l_DataRow);

                            }
                            if (Convert.ToString(l_DataRow["AccountGroup"]) == "Total")
                            {
                                //								this.TotalRefundAmount = IIf(l_DataRow["Emp.Total"].GetType().ToString == "System.DBNull", 0,Convert.ToDecimal(l_DataRow["Emp.Total"])) + IIf(l_DataRow["YMCATotal"].GetType().ToString == "System.DBNull", 0,Convert.ToDecimal(l_DataRow["YMCATotal"])); //todo check if this is needed if yes then 
                                //                                this.PersonalAmount = IIf(l_DataRow["Emp.Total"].GetType().ToString == "System.DBNull", 0,Convert.ToDecimal(l_DataRow["Emp.Total"])); //make property
                            }
                        }
                    }
                    bool l_Bool_Useit;
                    bool l_Bool_Yside;
                    foreach (DataRow l_CurrentRow in l_CalculationDataTable.Rows)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = true;
                        if (this.IsBasicAccount(l_CurrentRow))
                        {
                            l_Bool_Useit = true;
                            if (this.BoolIsMemberVested == true && this.DecimalTerminationPIA < this.DecimalMaxPIAAmount)
                            {
                                l_Bool_Yside = true;
                            }
                            else
                            {
                                l_Bool_Yside = false;
                            }

                        }
                        //start - Retirement Plan Group
                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_AM)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = true;
                        }

                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_AP)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = false;
                        }

                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_RP)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = false;
                        }


                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_SR)
                        {
                            //							if (this.BoolIsMemberVested== true && this.DecimalTerminationPIA < this.DecimalMaxPIAAmount) 
                            //							{ 
                            //								l_Bool_Useit = true; 
                            //								l_Bool_Yside = true; 
                            //							} 
                            //							else 
                            //							{ 
                            //								l_Bool_Useit = false; 
                            //								l_Bool_Yside = false; 
                            //							} 

                            l_Bool_Useit = true;
                            l_Bool_Yside = true;

                        }
                        //Priya 16-Jan-2009 : YRS 5.0-637 AC Account interest components Added ElseIf condition for AC account type
                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_AC)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = false;

                        }//End 16-Jan-2009
                        //end - Retirement Plan Group
                        //start - Savings Plan Group
                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_SavingsPlan_TD)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = false;
                        }
                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_SavingsPlan_TM)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = true;
                        }
                        else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_SavingsPlan_RT)
                        {
                            l_Bool_Useit = true;
                            l_Bool_Yside = false;
                        }
                        //end - Savings Plan Group
                        if (!(l_Bool_Useit))
                        {
                            l_CurrentRow.Delete();
                        }
                        if (l_Bool_Yside == false && l_Bool_Useit == true)
                        {
                            l_CurrentRow["YMCATaxable"] = 0;
                            l_CurrentRow["YMCAInterest"] = 0;
                            l_CurrentRow["Total"] = Convert.ToDouble(l_CurrentRow["Total"]) - Convert.ToDouble(l_CurrentRow["YMCATotal"]);
                            l_CurrentRow["YMCATotal"] = 0;
                        }
                    }
                    l_CalculationDataTable.AcceptChanges();
                    this.m_datatable_CalculatedDataTableForCurrentAccounts = l_CalculationDataTable;
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopyAccountContributionTableForCurrentAccounts Method", "Finish: Copy AccountContribution For CurrentAccounts.");
            }
            catch
            {
                throw;
            }
        }

        private void CopyAccountContributionTableForRefundable()
        {
            DataTable l_AccountContributionTable;
            DataTable l_CalculationDataTable;


            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopyAccountContributionTableForRefundable Method", "Start: Copy Account Contribution Table For Refundable.");
                l_AccountContributionTable = this.DataTableMemberAccountContributionProcessing;
                if (l_AccountContributionTable != null)
                {
                    l_CalculationDataTable = l_AccountContributionTable.Clone();
                    foreach (DataRow l_DataRow in l_AccountContributionTable.Rows)
                    {
                        if (l_DataRow["AccountType"].GetType().ToString() == "System.DBNull")
                        {
                            if (Convert.ToString(l_DataRow["AccountType"]) == "Total")
                            {
                                l_CalculationDataTable.ImportRow(l_DataRow);
                            }
                        }
                    }
                    this.m_datatable_CalculatedDataTableForRefundable = l_CalculationDataTable;
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopyAccountContributionTableForRefundable Method", "Finish: Copy Account Contribution Table For Refundable.");
            }
            catch
            {
                throw;
            }
        }
        private void CreateAnnuityBasisTypes()
        {
            DataTable l_AnnuityBasisTypeDataTable;
            int l_Counter;
            string[] l_AnnuityBasisType = null;
            l_AnnuityBasisType = new string[] { "", "", "" };
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAnnuityBasisTypes Method", "Start: Create Annuity Basis Types.");
                l_AnnuityBasisTypeDataTable = YMCARET.YmcaBusinessObject.RefundRequestRegularBOClass.LookUpAnnuityBasisTypes();
                if (l_AnnuityBasisTypeDataTable != null)
                {
                    l_Counter = 0;
                    // TODO: NotImplemented statement: ICSharpCode.SharpRefactory.Parser.AST.VB.ReDimStatement 
                    foreach (DataRow l_DataRow in l_AnnuityBasisTypeDataTable.Rows)
                    {
                        if (l_DataRow["AnnuityBasisType"].GetType().ToString().Trim() != "System.DBNull")
                        {
                            l_AnnuityBasisType[l_Counter] = Convert.ToString(l_DataRow["AnnuityBasisType"]);
                        }
                        l_Counter += 1;
                    }
                }
                this.AnnuityBasisType = l_AnnuityBasisType;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAnnuityBasisTypes Method", "Finish: Create Annuity Basis Types.");
            }
            catch
            {
                throw;
            }
        }

        //Added by Ashish 2010.05.07 resolve cashout issue
        //public void MakeRefundRequestProcessingDataTables()
        public void MakeRefundRequestProcessingDataTables(YMCARET.YmcaDataAccessObject.CashOutDAClass paraCashOutDA)
        {
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeRefundRequestProcessingDataTables Method", "Start: MakeRefundRequestProcessingDataTables for refund request processing.");
                this.LoadSchemaRefundTable();
                this.LoadRefundRequestDetails(this.StringPersonId, this.StringFundEventId, paraCashOutDA);
                this.CopyAccountContributionTableForCurrentAccounts();
                this.DoRegularRefundForCurrentAccounts();
                this.LoadRequestedAccounts(paraCashOutDA);
                this.LoadRefundableDataTable(paraCashOutDA);
                this.CalculateMinimumDistributionAmount();
                this.CreatePayees();
                this.LoadRefundableDataTable(paraCashOutDA);
                this.CopyAccountContributionTableForRefundable();
                this.DoRegularRefundForRefundAccounts();
                this.CreateAnnuityBasisTypes();
                this.OpenFiles();
                this.GetCurrencyCode();
                this.GetTransactionDetails();
                this.ReduceAvailableBalance();
                this.CreateRowsForDisbursements();
                this.LoadRefundableDataTable(paraCashOutDA);
                this.MakeTablesToUpdate();
                this.SaveAllTable();
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeRefundRequestProcessingDataTables Method", "Finish: MakeRefundRequestProcessingDataTables for refund request processing.");
            }
            catch
            {
                throw;
            }
        }
        private void GetTransactionDetails()
        {
            DataTable l_DataTable;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("GetTransactionDetails Method", "Start: Get Transaction Details.");
                l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetTransactions(this.m_string_FundEventId);
                this.m_datatable_AvailableBalance = l_DataTable;
                //we are not checking for IsPersonalOnly and then manipulating the fields as this refund will not
                //happen in cash out.
                //				if (this.IsPersonalOnly == true) 
                //				{ 
                //					foreach (DataRow l_DataRow in l_DataTable.Rows) 
                //					{ 
                //						l_DataRow["YmcaPreTax"] = "0.00"; 
                //					} 
                //					l_DataTable = ((DataTable)(Session("CalculatedDataTableForRefundable"))); 
                //					if (!(l_DataTable == null)) 
                //					{ 
                //						foreach (DataRow l_DataRow in l_DataTable.Rows) 
                //						{ 
                //							l_DataRow["YMCATaxable"] = "0.00"; 
                //							l_DataRow["YMCAInterest"] = "0.00"; 
                //							l_DataRow["YMCATotal"] = "0.00"; 
                //							l_DataRow["Total"] = l_DataRow["Emp.Total"]; 
                //						} 
                //					} 
                //				} 
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("GetTransactionDetails Method", "Finish: Get Transaction Details.");
            }
            catch
            {
                throw;
            }
        }
        private DataRow[] GetDataRow(DataTable parameterDataTable, string parameterAnnuityBasisType)
        {
            string l_QueryString;
            DataRow[] l_FoundRows;
            try
            {
                if (parameterDataTable != null)
                {
                    l_QueryString = "AnnuityBasisType = '" + parameterAnnuityBasisType.Trim() + "'";
                    l_FoundRows = parameterDataTable.Select(l_QueryString);
                    if (l_FoundRows == null)
                    {
                        return null;
                    }
                    if (l_FoundRows.Length == 0)
                    {
                        return null;
                    }
                    return l_FoundRows;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        private int GetAccountBreakDownSortOrder(string paramterAccountType)
        {
            DataTable l_DataTable;
            string l_AccountType;
            int l_SortOrder = 0;
            try
            {
                l_DataTable = this.DataTableMemberAccountBreakdown;
                if (l_DataTable != null)
                {
                    foreach (DataRow l_DataRow in l_DataTable.Rows)
                    {
                        l_AccountType = Convert.ToString(l_DataRow["chrAcctBreakDownType"]);
                        if (l_AccountType != "")
                        {
                            if (l_AccountType.Trim().ToUpper() == paramterAccountType.Trim().ToUpper())
                            {
                                return (l_SortOrder = Convert.ToInt32(l_DataRow["intSortOrder"]));
                            }
                        }
                    }
                }
                else
                {
                    l_SortOrder = 0;
                }
                return l_SortOrder;
            }
            catch
            {
                throw;
            }
        }

        private string GetAccountBreakDownType(string parameterAccountGroup, bool parameterPersonal, bool parameterYMCA, bool parameterPreTax, bool parameterPostTax)
        {
            DataTable l_DataTable;
            DataRow l_DataRow;
            DataRow[] l_FoundRows;
            string l_QueryString;
            try
            {
                l_DataTable = this.DataTableMemberAccountBreakdown;
                if (l_DataTable != null)
                {
                    //l_QueryString = "chrAcctType = '" + parameterAccountType.Trim() + "' "; 
                    l_QueryString = "chvAcctGroups = '" + parameterAccountGroup.Trim() + "' ";
                    if (parameterPersonal == true)
                    {
                        l_QueryString += " AND bitPersonal = 1 ";
                    }
                    if (parameterYMCA == true)
                    {
                        l_QueryString += " AND bitYmca = 1 ";
                    }
                    if (parameterPreTax == true)
                    {
                        l_QueryString += " AND bitPreTax = 1 ";
                    }
                    if (parameterPostTax == true)
                    {
                        l_QueryString += " AND bitPostTax = 1 ";
                    }
                    l_FoundRows = l_DataTable.Select(l_QueryString);
                    if (l_FoundRows.Length > 0)
                    {
                        l_DataRow = l_FoundRows[0];
                        return Convert.ToString(l_DataRow["chrAcctBreakDownType"]);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                throw;
            }
        }
        private void GetCurrencyCode()
        {
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("GetCurrencyCode Method", "Start: GetCurrencyCode.");
                this.StringCurrencyCode = YMCARET.YmcaBusinessObject.RefundRequest.GetPersonBankingBeforeEffectiveDate(this.StringPersonId);
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("GetCurrencyCode Method", "Finish: GetCurrencyCode.");
            }
            catch
            {
                throw;
            }
        }
        private DataRow GetDataRowForSelectedTable(DataTable parameterDataTable, string parameterAccountType, string parameterColumnName)
        {
            string l_QueryString;
            DataRow[] l_FoundRows;
            try
            {
                if (parameterDataTable != null)
                {
                    if (parameterColumnName == "")
                    {
                        l_QueryString = "AcctType = '" + parameterAccountType.Trim() + "'";
                    }
                    else
                    {
                        l_QueryString = parameterColumnName + " = '" + parameterAccountType.Trim() + "'";
                    }
                    l_FoundRows = parameterDataTable.Select(l_QueryString);
                    if (l_FoundRows == null)
                    {
                        return null;
                    }
                    if (l_FoundRows.Length == 0)
                    {
                        return null;
                    }
                    return l_FoundRows[0];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
        private int RefundPrinciple(DataRow parameterSelectedDataRow, DataRow parameterAvailableBalance, string parameterAccountType, int parameterIndex, string parameterCurrencyCode, string parameterTransactionType, string parameterAnnuityBasisType, string parameterAccountGroup)
        {
            DataRow l_SelectedDataRow;
            DataRow l_AvailableBalance;
            string l_AccountBreakDownType;
            string l_AccountType;
            int l_SortOrder;
            decimal l_Decimal_PersonalPreTax;
            decimal l_Decimal_PersonalPostTax;
            decimal l_Decimal_YMCAPreTax;
            decimal l_Decimal_TaxRate;
            decimal l_Decimal_SelectedTaxableAmount;
            decimal l_Decimal_SelectedNonTaxableAmount;
            DataRow l_DisbursementDataRow;
            DataRow l_DisbursementDetailsDataRow;
            DataRow l_RefundsDataRow;
            DataRow l_TransactionDataRow;
            string l_AccountGroup = "";
            try
            {
                l_SelectedDataRow = parameterSelectedDataRow;
                l_AvailableBalance = parameterAvailableBalance;
                l_AccountType = parameterAccountType;
                l_AccountGroup = parameterAccountGroup;
                if (l_SelectedDataRow == null)
                {
                    return 0;

                }
                if (l_AvailableBalance == null)
                {
                    return 0;
                }
                if (l_SelectedDataRow["Taxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["Taxable"]);
                }
                if (l_SelectedDataRow["NonTaxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedNonTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedNonTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["NonTaxable"]);
                }
                if (l_SelectedDataRow["TaxRate"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_TaxRate = 0;
                }
                else
                {
                    l_Decimal_TaxRate = Convert.ToDecimal(l_SelectedDataRow["TaxRate"]);
                }
                if (l_AvailableBalance["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPreTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"]);
                }
                if (l_AvailableBalance["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPostTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalance["PersonalPostTax"]);
                }

                if (l_AvailableBalance["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_YMCAPreTax = 0;
                }
                else
                {
                    l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalance["YmcaPreTax"]);
                }


                if (l_Decimal_SelectedTaxableAmount > 0 && l_Decimal_PersonalPreTax > 0)
                {
                    if (l_AccountGroup == m_const_RetirementPlan_AM && ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) == 0))
                    {
                        l_AccountBreakDownType = "07";
                    }
                    //Priya 20-Jan-2009 : YRS 5.0-637 AC Account interest components added ElseIf condition for AccountGroup
                    else if (l_AccountGroup == m_const_RetirementPlan_AC)
                    {
                        l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, true, false, true, false);
                    }
                    //End 20-Jan-2008
                    else
                    {
                        l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, true, false, true, false);
                    }
                    l_SortOrder = this.GetAccountBreakDownSortOrder(l_AccountBreakDownType);
                    l_DisbursementDataRow = this.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode);
                    if (!(l_DisbursementDataRow == null))
                    {
                        l_DisbursementDetailsDataRow = this.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), l_SortOrder);
                        l_TransactionDataRow = this.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType);
                        if (!(l_TransactionDataRow == null))
                        {
                            l_RefundsDataRow = this.SetRefundsDataRow((Convert.ToString(l_TransactionDataRow["UniqueID"])), parameterSelectedDataRow, l_AccountType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), parameterAnnuityBasisType);
                            if (!(l_RefundsDataRow == null))
                            {
                                if (l_Decimal_SelectedTaxableAmount >= l_Decimal_PersonalPreTax)
                                {
                                    l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_PersonalPreTax;
                                    l_DisbursementDetailsDataRow["TaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxablePrincipal"])) + l_Decimal_PersonalPreTax;
                                    l_DisbursementDetailsDataRow["TaxWithheldPrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldPrincipal"])) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100));
                                    l_TransactionDataRow["PersonalPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["PersonalPreTax"])) + (l_Decimal_PersonalPreTax * (-1));
                                    l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_PersonalPreTax;
                                    l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100));
                                    l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                    l_SelectedDataRow["Taxable"] = (Convert.ToDecimal(l_SelectedDataRow["Taxable"])) - l_Decimal_PersonalPreTax;
                                    l_AvailableBalance["PersonalPreTax"] = "0.00";
                                }
                                else
                                {
                                    l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_SelectedTaxableAmount;
                                    l_DisbursementDetailsDataRow["TaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxablePrincipal"])) + l_Decimal_SelectedTaxableAmount;
                                    l_DisbursementDetailsDataRow["TaxWithheldPrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldPrincipal"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                    l_TransactionDataRow["PersonalPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["PersonalPreTax"])) + (l_Decimal_SelectedTaxableAmount * (-1));
                                    l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_SelectedTaxableAmount;
                                    l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                    l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                    l_AvailableBalance["PersonalPreTax"] = (Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"])) - (Convert.ToDecimal(l_SelectedDataRow["Taxable"]));
                                    l_SelectedDataRow["Taxable"] = "0.00";
                                }
                            }
                        }
                    }
                }
                if (l_SelectedDataRow["Taxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["Taxable"]);
                }
                if (l_SelectedDataRow["NonTaxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedNonTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedNonTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["NonTaxable"]);
                }
                if (l_SelectedDataRow["TaxRate"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_TaxRate = 0;
                }
                else
                {
                    l_Decimal_TaxRate = Convert.ToDecimal(l_SelectedDataRow["TaxRate"]);
                }
                if (l_AvailableBalance["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPreTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"]);
                }
                if (l_AvailableBalance["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPostTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalance["PersonalPostTax"]);
                }

                if (l_AvailableBalance["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_YMCAPreTax = 0;
                }
                else
                {
                    l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalance["YmcaPreTax"]);
                }



                if ((l_Decimal_SelectedTaxableAmount > 0 && l_Decimal_YMCAPreTax > 0))
                {
                    if (this.BoolIsMemberVested == true || l_AccountGroup == m_const_RetirementPlan_AM || l_AccountGroup == m_const_SavingsPlan_TM || l_AccountGroup == m_const_RetirementPlan_SR)
                    {
                        if (l_AccountGroup == m_const_RetirementPlan_AM && ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) == 0))
                        {
                            l_AccountBreakDownType = "07";
                        }
                        else
                        {
                            l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, false, true, false, false);
                        }
                        l_SortOrder = this.GetAccountBreakDownSortOrder(l_AccountBreakDownType);
                        l_DisbursementDataRow = this.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode);
                        if (l_DisbursementDataRow != null)
                        {
                            l_DisbursementDetailsDataRow = this.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), l_SortOrder);
                            l_TransactionDataRow = this.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType);
                            if (l_TransactionDataRow != null)
                            {
                                l_RefundsDataRow = this.SetRefundsDataRow((Convert.ToString(l_TransactionDataRow["UniqueID"])), parameterSelectedDataRow, l_AccountType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), parameterAnnuityBasisType);
                                if (l_RefundsDataRow != null)
                                {
                                    if (l_Decimal_SelectedTaxableAmount >= l_Decimal_YMCAPreTax)
                                    {
                                        l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_YMCAPreTax;
                                        l_DisbursementDetailsDataRow["TaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxablePrincipal"])) + l_Decimal_YMCAPreTax;
                                        l_DisbursementDetailsDataRow["TaxWithheldPrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldPrincipal"])) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100));
                                        l_TransactionDataRow["YmcaPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["YmcaPreTax"])) + (l_Decimal_YMCAPreTax * (-1));
                                        //Commented By Shubhrata JAN 19th 2007 YREN-3025 YmcaPreTax was not getting summed up with PersonalPreTax in atsRefunds
                                        //l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_PersonalPreTax;
                                        //Added By Shubhrata JAN 19th 2007 YREN-3025 YmcaPreTax was not getting summed up with PersonalPreTax in atsRefunds
                                        l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_YMCAPreTax;
                                        l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100));
                                        l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                        l_SelectedDataRow["Taxable"] = (Convert.ToDecimal(l_SelectedDataRow["Taxable"])) - l_Decimal_YMCAPreTax;
                                        l_AvailableBalance["YmcaPreTax"] = "0.00";
                                    }
                                    else
                                    {
                                        l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_SelectedTaxableAmount;
                                        l_DisbursementDetailsDataRow["TaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxablePrincipal"])) + l_Decimal_SelectedTaxableAmount;
                                        l_DisbursementDetailsDataRow["TaxWithheldPrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldPrincipal"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                        l_TransactionDataRow["YmcaPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["YmcaPreTax"])) + (l_Decimal_SelectedTaxableAmount * (-1));
                                        l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_SelectedTaxableAmount;
                                        l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                        l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                        l_AvailableBalance["YmcaPreTax"] = l_Decimal_YMCAPreTax - l_Decimal_SelectedTaxableAmount;
                                        l_SelectedDataRow["Taxable"] = "0.00";
                                    }
                                }
                            }
                        }
                    }
                }
                if (l_SelectedDataRow["Taxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["Taxable"]);
                }
                if (l_SelectedDataRow["NonTaxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedNonTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedNonTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["NonTaxable"]);
                }
                if (l_SelectedDataRow["TaxRate"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_TaxRate = 0;
                }
                else
                {
                    l_Decimal_TaxRate = Convert.ToDecimal(l_SelectedDataRow["TaxRate"]);
                }
                if (l_AvailableBalance["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPreTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"]);
                }
                if (l_AvailableBalance["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPostTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalance["PersonalPostTax"]);
                }

                if (l_AvailableBalance["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_YMCAPreTax = 0;
                }
                else
                {
                    l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalance["YmcaPreTax"]);
                }

                if (l_Decimal_SelectedNonTaxableAmount > 0 && l_Decimal_PersonalPostTax > 0)
                {
                    if (l_AccountGroup == m_const_RetirementPlan_AM && ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) == 0))
                    {
                        l_AccountBreakDownType = "07";
                    }
                    else
                    {
                        l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, true, false, false, true);
                    }
                    l_SortOrder = this.GetAccountBreakDownSortOrder(l_AccountBreakDownType);
                    l_DisbursementDataRow = this.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode);
                    if (!(l_DisbursementDataRow == null))
                    {
                        l_DisbursementDetailsDataRow = this.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), l_SortOrder);
                        l_TransactionDataRow = this.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType);
                        if (!(l_TransactionDataRow == null))
                        {
                            l_RefundsDataRow = this.SetRefundsDataRow((Convert.ToString(l_TransactionDataRow["UniqueID"])), parameterSelectedDataRow, l_AccountType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), parameterAnnuityBasisType);
                            if (!(l_RefundsDataRow == null))
                            {
                                if (l_Decimal_SelectedNonTaxableAmount >= l_Decimal_PersonalPostTax)
                                {
                                    l_DisbursementDataRow["NonTaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["NonTaxableAmount"])) + l_Decimal_PersonalPostTax;
                                    l_DisbursementDetailsDataRow["NonTaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["NonTaxablePrincipal"])) + l_Decimal_PersonalPostTax;
                                    l_TransactionDataRow["PersonalPostTax"] = (Convert.ToDecimal(l_TransactionDataRow["PersonalPostTax"])) + (l_Decimal_PersonalPostTax * (-1));
                                    l_RefundsDataRow["NonTaxable"] = (Convert.ToDecimal(l_RefundsDataRow["NonTaxable"])) + l_Decimal_PersonalPostTax;
                                    l_SelectedDataRow["NonTaxable"] = (Convert.ToDecimal(l_SelectedDataRow["NonTaxable"])) - l_Decimal_PersonalPostTax;
                                    l_AvailableBalance["PersonalPostTax"] = "0.00";
                                }
                                else
                                {
                                    l_DisbursementDataRow["NonTaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["NonTaxableAmount"])) + l_Decimal_SelectedNonTaxableAmount;
                                    l_DisbursementDetailsDataRow["NonTaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["NonTaxablePrincipal"])) + l_Decimal_SelectedNonTaxableAmount;
                                    l_TransactionDataRow["PersonalPostTax"] = (Convert.ToDecimal(l_TransactionDataRow["PersonalPostTax"])) + (l_Decimal_SelectedNonTaxableAmount * (-1));
                                    l_RefundsDataRow["NonTaxable"] = (Convert.ToDecimal(l_RefundsDataRow["NonTaxable"])) + l_Decimal_SelectedNonTaxableAmount;
                                    l_AvailableBalance["PersonalPostTax"] = (Convert.ToDecimal(l_AvailableBalance["PersonalPostTax"])) - l_Decimal_SelectedNonTaxableAmount;
                                    l_SelectedDataRow["NonTaxable"] = "0.00";
                                }
                            }
                        }
                    }
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }
        private int RefundInterest(DataRow parameterSelectedDataRow, DataRow parameterAvailableBalance, string parameterAccountType, int parameterIndex, string parameterCurrencyCode, string parameterTransactionType, string parameterAnnuityBasisType, string parameterAccountGroup)
        {
            DataRow l_SelectedDataRow;
            DataRow l_AvailableBalance;
            string l_AccountBreakDownType;
            string l_AccountType;
            int l_SortOrder;
            decimal l_Decimal_PersonalPreTax;
            decimal l_Decimal_PersonalPostTax;
            decimal l_Decimal_YMCAPreTax;
            decimal l_Decimal_TaxRate;
            decimal l_Decimal_SelectedTaxableAmount;
            decimal l_Decimal_SelectedNonTaxableAmount;
            DataRow l_DisbursementDataRow;
            DataRow l_DisbursementDetailsDataRow;
            DataRow l_RefundsDataRow;
            DataRow l_TransactionDataRow;
            string l_AccountGroup = "";
            try
            {
                l_SelectedDataRow = parameterSelectedDataRow;
                l_AvailableBalance = parameterAvailableBalance;
                l_AccountType = parameterAccountType;
                l_AccountGroup = parameterAccountGroup;
                if (l_SelectedDataRow == null)
                {
                    return 0;
                }
                if (l_AvailableBalance == null)
                {
                    return 0;
                }
                if (l_SelectedDataRow["Taxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["Taxable"]);
                }
                if (l_SelectedDataRow["NonTaxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedNonTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedNonTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["NonTaxable"]);
                }
                if (l_SelectedDataRow["TaxRate"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_TaxRate = 0;
                }
                else
                {
                    l_Decimal_TaxRate = Convert.ToDecimal(l_SelectedDataRow["TaxRate"]);
                }
                if (l_AvailableBalance["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPreTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"]);
                }
                if (l_AvailableBalance["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPostTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalance["PersonalPostTax"]);
                }

                if (l_AvailableBalance["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_YMCAPreTax = 0;
                }
                else
                {
                    l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalance["YmcaPreTax"]);
                }


                if (l_Decimal_SelectedTaxableAmount > 0 && l_Decimal_PersonalPreTax > 0)
                {
                    if (l_AccountGroup == m_const_RetirementPlan_AM && (l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax == 0))
                    {
                        l_AccountBreakDownType = "07";
                    }
                    //Priya 20-Jan-2009 : YRS 5.0-637 AC Account interest components added ElseIf condition for AccountGroup
                    else if (l_AccountGroup == m_const_RetirementPlan_AC)
                    {
                        l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, true, false, true, false);
                    }
                    //End 20-Jan-2008
                    else
                    {
                        l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, true, false, false, true);
                    }
                    l_SortOrder = this.GetAccountBreakDownSortOrder(l_AccountBreakDownType);
                    l_DisbursementDataRow = this.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode);
                    if (l_DisbursementDataRow != null)
                    {
                        l_DisbursementDetailsDataRow = this.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), l_SortOrder);
                        l_TransactionDataRow = this.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType);
                        if (!(l_TransactionDataRow == null))
                        {
                            l_RefundsDataRow = this.SetRefundsDataRow((Convert.ToString(l_TransactionDataRow["UniqueID"])), parameterSelectedDataRow, l_AccountType, (Convert.ToString(l_DisbursementDataRow["UniqueID"])), parameterAnnuityBasisType);
                            if (l_RefundsDataRow != null)
                            {
                                if (l_Decimal_SelectedTaxableAmount >= l_Decimal_PersonalPreTax)
                                {
                                    l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_PersonalPreTax;
                                    l_DisbursementDetailsDataRow["TaxableInterest"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxableInterest"])) + l_Decimal_PersonalPreTax;
                                    l_DisbursementDetailsDataRow["TaxWithheldInterest"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldInterest"])) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100));
                                    l_TransactionDataRow["PersonalPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["PersonalPreTax"])) + (l_Decimal_PersonalPreTax * (-1));
                                    l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_PersonalPreTax;
                                    l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100));
                                    l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                    l_SelectedDataRow["Taxable"] = (Convert.ToDecimal(l_SelectedDataRow["Taxable"])) - l_Decimal_PersonalPreTax;
                                    l_AvailableBalance["PersonalPreTax"] = "0.00";
                                }
                                else
                                {
                                    l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_SelectedTaxableAmount;
                                    l_DisbursementDetailsDataRow["TaxableInterest"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxableInterest"])) + l_Decimal_SelectedTaxableAmount;
                                    l_DisbursementDetailsDataRow["TaxWithheldInterest"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldInterest"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                    l_TransactionDataRow["PersonalPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["PersonalPreTax"])) + (l_Decimal_SelectedTaxableAmount * (-1));
                                    l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_SelectedTaxableAmount;
                                    l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                    l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                    l_AvailableBalance["PersonalPreTax"] = (Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"])) - l_Decimal_SelectedTaxableAmount;
                                    l_SelectedDataRow["Taxable"] = "0.00";
                                }
                            }
                        }
                    }
                }
                if (l_SelectedDataRow["Taxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["Taxable"]);
                }
                if (l_SelectedDataRow["NonTaxable"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_SelectedNonTaxableAmount = 0;
                }
                else
                {
                    l_Decimal_SelectedNonTaxableAmount = Convert.ToDecimal(l_SelectedDataRow["NonTaxable"]);
                }
                if (l_SelectedDataRow["TaxRate"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_TaxRate = 0;
                }
                else
                {
                    l_Decimal_TaxRate = Convert.ToDecimal(l_SelectedDataRow["TaxRate"]);
                }
                if (l_AvailableBalance["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPreTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalance["PersonalPreTax"]);
                }
                if (l_AvailableBalance["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_PersonalPostTax = 0;
                }
                else
                {
                    l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalance["PersonalPostTax"]);
                }

                if (l_AvailableBalance["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                {
                    l_Decimal_YMCAPreTax = 0;
                }
                else
                {
                    l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalance["YmcaPreTax"]);
                }

                if ((l_Decimal_SelectedTaxableAmount > 0 && l_Decimal_YMCAPreTax > 0))
                {
                    if (this.BoolIsMemberVested == true || l_AccountGroup == m_const_RetirementPlan_AM || l_AccountGroup == m_const_SavingsPlan_TM || l_AccountGroup == m_const_RetirementPlan_SR)
                    {
                        if (l_AccountGroup == m_const_RetirementPlan_AM & ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) == 0))
                        {
                            l_AccountBreakDownType = "07";
                        }

                        else
                        {
                            l_AccountBreakDownType = this.GetAccountBreakDownType(l_AccountGroup, false, true, false, false);
                        }
                        l_SortOrder = this.GetAccountBreakDownSortOrder(l_AccountBreakDownType);
                        l_DisbursementDataRow = this.SetDisbursementDataRow(parameterIndex, parameterCurrencyCode);
                        if (l_DisbursementDataRow != null)
                        {
                            l_DisbursementDetailsDataRow = this.SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, (Convert.ToString(l_DisbursementDataRow["Uniqueid"])), l_SortOrder);
                            l_TransactionDataRow = this.SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType);
                            if (l_TransactionDataRow != null)
                            {
                                l_RefundsDataRow = this.SetRefundsDataRow((Convert.ToString(l_TransactionDataRow["Uniqueid"])), parameterSelectedDataRow, l_AccountType, (Convert.ToString(l_DisbursementDataRow["Uniqueid"])), parameterAnnuityBasisType);
                                if (l_RefundsDataRow != null)
                                {
                                    if (l_Decimal_SelectedTaxableAmount >= l_Decimal_YMCAPreTax)
                                    {
                                        l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_YMCAPreTax;
                                        l_DisbursementDetailsDataRow["TaxableInterest"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxableInterest"])) + l_Decimal_YMCAPreTax;
                                        l_DisbursementDetailsDataRow["TaxWithheldInterest"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldInterest"])) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100));
                                        l_TransactionDataRow["YmcaPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["YmcaPreTax"])) + (l_Decimal_YMCAPreTax * (-1));
                                        //Commented By Shubhrata JAN 19th 2007 YREN-3025 YmcaPreTax was not getting summed up with PersonalPreTax in atsRefunds
                                        //l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_PersonalPreTax;
                                        //Added By Shubhrata JAN 19th 2007 YREN-3025 YmcaPreTax was not getting summed up with PersonalPreTax in atsRefunds
                                        l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_YMCAPreTax;
                                        l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_YMCAPreTax * (l_Decimal_TaxRate / 100));
                                        l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                        l_SelectedDataRow["Taxable"] = (Convert.ToDecimal(l_SelectedDataRow["Taxable"])) - l_Decimal_YMCAPreTax;
                                        l_AvailableBalance["YmcaPreTax"] = "0.00";
                                    }
                                    else
                                    {
                                        l_DisbursementDataRow["TaxableAmount"] = (Convert.ToDecimal(l_DisbursementDataRow["TaxableAmount"])) + l_Decimal_SelectedTaxableAmount;
                                        l_DisbursementDetailsDataRow["TaxablePrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxablePrincipal"])) + l_Decimal_SelectedTaxableAmount;
                                        l_DisbursementDetailsDataRow["TaxWithheldPrincipal"] = (Convert.ToDecimal(l_DisbursementDetailsDataRow["TaxWithheldPrincipal"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                        l_TransactionDataRow["YmcaPreTax"] = (Convert.ToDecimal(l_TransactionDataRow["YmcaPreTax"])) + (l_Decimal_SelectedTaxableAmount * (-1));
                                        l_RefundsDataRow["Taxable"] = (Convert.ToDecimal(l_RefundsDataRow["Taxable"])) + l_Decimal_SelectedTaxableAmount;
                                        l_RefundsDataRow["Tax"] = (Convert.ToDecimal(l_RefundsDataRow["Tax"])) + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100));
                                        l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
                                        l_AvailableBalance["YmcaPreTax"] = l_Decimal_YMCAPreTax - l_Decimal_SelectedTaxableAmount;
                                        l_SelectedDataRow["Taxable"] = "0.00";
                                    }
                                }
                            }
                        }
                    }
                }
                return 1;
            }
            catch
            {
                throw;
            }
        }
        private int CalculateMinimumDistributionAmount()
        {
            //decimal l_DecimalDistributionPeriod;
            this.m_decimal_MinimumDistributionAmount = 0;

            DataTable dtRmdRecords = null;
            DataRow[] drRmdRecords = null;

            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateMinimumDistributionAmount Method", "Start: Calculate RMD.");
                if (this.BoolIsTerminated == false)
                {
                    return 0;
                }


                //Code Commented by Dinesh Kanojia on 10/09/2014
                //Start: BT:2437: Cashout process not inserting records in AtsMrdRecordsDisbursements for RMD eligible participants.
                //if (Convert.ToDouble(this.DecimalPersonAge) < 70.5)
                //{
                //    return 0;
                //}
                if (!IsRMDeligible)
                {
                    return 0;
                }
                //End: BT:2437: Cashout process not inserting records in AtsMrdRecordsDisbursements for RMD eligible participants.
                if (this.BoolCheckForTerminationDate == true)
                {
                    //l_DecimalDistributionPeriod = YMCARET.YmcaBusinessObject.RefundRequest.GetDistributionPeriod((Convert.ToInt32(Math.Round(this.DecimalPersonAge, 2) - (5 / 10)))); //(5/10) is used as we were to subtract 0.5 
                    //DataTable l_DataTable;
                    //decimal l_Decimal_Total = 0;
                    //l_DataTable = this.m_datatable_CalculatedDataTableForCurrentAccounts;
                    //if (l_DataTable != null)
                    //{
                    //    if (l_DataTable.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow l_DataRow in l_DataTable.Rows)
                    //        {
                    //            if (l_DataRow["AccountType"].GetType().ToString() != "System.DBNull")
                    //            {
                    //                if ((Convert.ToString(l_DataRow["AccountType"])).ToUpper() != "TOTAL")
                    //                {
                    //                    l_Decimal_Total = l_Decimal_Total + (Convert.ToDecimal(l_DataRow["Taxable"])) + (Convert.ToDecimal(l_DataRow["Interest"]));
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //if (l_Decimal_Total != 0)
                    //{
                    //    if (l_DecimalDistributionPeriod != 0)
                    //    {
                    //        this.m_decimal_MinimumDistributionAmount = Math.Round((l_Decimal_Total) / l_DecimalDistributionPeriod, 2);
                    //    }
                    //}
                    //else
                    //{
                    //    this.m_decimal_MinimumDistributionAmount = 0;
                    //}

                    dtRmdRecords = YMCARET.YmcaBusinessObject.RefundRequest.GetMRDRecords(this.m_string_FundEventId);

                    if (dtRmdRecords != null && dtRmdRecords.Rows.Count > 0)
                    {
                        this.m_RmdRecordsDataTable = dtRmdRecords;
                        drRmdRecords = dtRmdRecords.Select("PlanType = '" + this.m_string_PlanType + "'");

                        if (drRmdRecords != null && drRmdRecords.Length > 0)
                        {
                            if (!String.IsNullOrEmpty(drRmdRecords[0]["MRDTaxable"].ToString()))
                                this.m_decimal_MinimumDistributionAmount = Convert.ToDecimal(drRmdRecords[0]["MRDTaxable"].ToString());
                        }
                    }

                    this.DoMinimumDistributionCalculation();
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateMinimumDistributionAmount Method", "Finish: Calculate RMD.");
                return 1;

            }
            catch
            {
                throw;
            }
        }


        private string DoMinimumDistributionCalculation()
        {
            DataTable l_MinimumDistributionTable;
            DataTable l_CurrentAccountDataTable;
            DataTable l_DataTable;
            decimal l_Decimal_PersonalPreTax;
            decimal l_Decimal_PersonalPostTax;
            decimal l_Decimal_PersonalInterest;
            decimal l_Decimal_YMCAPreTax;
            decimal l_Decimal_YMCAInterest;
            decimal l_Decimal_MinimumDistributionAmount;
            bool l_Boolean_IsMinimumDistribution;
            DataRow l_MinimumdistributionDataRow;
            decimal l_Decimal_Taxable;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoMinimumDistributionCalculation Method", "Start: Do RMD Calculation.");
                l_DataTable = this.m_datatable_AtsRefund; //todo
                if (!(l_DataTable == null))
                {
                    l_MinimumDistributionTable = l_DataTable.Clone();
                }
                else
                {
                    return "0";
                }
                l_CurrentAccountDataTable = this.m_datatable_CalculatedDataTableForCurrentAccounts;
                if (l_CurrentAccountDataTable == null)
                {
                    return "0";
                }
                if (this.m_decimal_MinimumDistributionAmount == 0)//todo check where it is originally set 
                {
                    return "0";
                }
                l_Decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount;
                this.m_decimal_MinimumDistributionAmount = 0;
                foreach (DataRow l_DataRow in l_CurrentAccountDataTable.Rows)
                {
                    if (l_Decimal_MinimumDistributionAmount == 0)
                    {
                        break;
                    }
                    if (l_DataRow["Taxable"].GetType().ToString() != "System.DBNull")
                    {
                        l_Decimal_PersonalPreTax = (Convert.ToDecimal(l_DataRow["Taxable"]));
                    }
                    else
                    {
                        l_Decimal_PersonalPreTax = 0;
                    }
                    if (l_DataRow["Non-Taxable"].GetType().ToString() != "System.DBNull")
                    {
                        l_Decimal_PersonalPostTax = (Convert.ToDecimal(l_DataRow["Non-Taxable"]));
                    }
                    else
                    {
                        l_Decimal_PersonalPostTax = 0;
                    }
                    if (l_DataRow["Interest"].GetType().ToString() != "System.DBNull")
                    {
                        l_Decimal_PersonalInterest = (Convert.ToDecimal(l_DataRow["Interest"]));
                    }
                    else
                    {
                        l_Decimal_PersonalInterest = 0;
                    }
                    if (l_DataRow["YMCATaxable"].GetType().ToString() != "System.DBNull")
                    {
                        l_Decimal_YMCAPreTax = (Convert.ToDecimal(l_DataRow["YMCATaxable"]));
                    }
                    else
                    {
                        l_Decimal_YMCAPreTax = 0;
                    }
                    if (l_DataRow["YMCAInterest"].GetType().ToString() != "System.DBNull")
                    {
                        l_Decimal_YMCAInterest = (Convert.ToDecimal(l_DataRow["YMCAInterest"]));
                    }
                    else
                    {
                        l_Decimal_YMCAInterest = 0;
                    }
                    if ((l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest + l_Decimal_YMCAPreTax + l_Decimal_YMCAInterest) > 0)
                    {
                        l_Boolean_IsMinimumDistribution = false;
                        if (l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalPreTax)
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            l_Decimal_Taxable = l_Decimal_PersonalPreTax;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_PersonalPreTax;
                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersonalPreTax;
                            l_DataRow["Emp.Total"] = (Convert.ToDecimal(l_DataRow["Taxable"])) + (Convert.ToDecimal(l_DataRow["Non-Taxable"])) + (Convert.ToDecimal(l_DataRow["Interest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                        }
                        else
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount;
                            l_DataRow["Taxable"] = Convert.ToDecimal(l_DataRow["Taxable"]) - l_Decimal_MinimumDistributionAmount;
                            l_DataRow["Emp.Total"] = (Convert.ToDecimal(l_DataRow["Taxable"])) + (Convert.ToDecimal(l_DataRow["Non-Taxable"])) + (Convert.ToDecimal(l_DataRow["Interest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                            l_Decimal_Taxable = l_Decimal_MinimumDistributionAmount;
                            l_Decimal_MinimumDistributionAmount = 0;
                        }
                        if (l_Decimal_MinimumDistributionAmount >= l_Decimal_PersonalInterest)
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_PersonalInterest;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_PersonalInterest;
                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_PersonalInterest;
                            l_DataRow["Interest"] = "0.00";
                            l_DataRow["Emp.Total"] = (Convert.ToDecimal(l_DataRow["Taxable"])) + (Convert.ToDecimal(l_DataRow["Non-Taxable"])) + (Convert.ToDecimal(l_DataRow["Interest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                        }
                        else
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount;
                            l_DataRow["Interest"] = Convert.ToDecimal(l_DataRow["Interest"]) - l_Decimal_MinimumDistributionAmount;
                            l_DataRow["Emp.Total"] = (Convert.ToDecimal(l_DataRow["Taxable"])) + (Convert.ToDecimal(l_DataRow["Non-Taxable"])) + (Convert.ToDecimal(l_DataRow["Interest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount;
                            l_Decimal_MinimumDistributionAmount = 0;
                        }
                        if (l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAPreTax)
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_YMCAPreTax;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_YMCAPreTax;
                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_YMCAPreTax;
                            l_DataRow["YMCATaxable"] = "0.00";
                            l_DataRow["YMCATotal"] = (Convert.ToDecimal(l_DataRow["YMCATaxable"])) + (Convert.ToDecimal(l_DataRow["YMCAInterest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                        }
                        else
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount;
                            l_DataRow["YMCATaxable"] = Convert.ToDecimal(l_DataRow["YMCATaxable"]) - l_Decimal_MinimumDistributionAmount;
                            l_DataRow["YMCATotal"] = (Convert.ToDecimal(l_DataRow["YMCATaxable"])) + (Convert.ToDecimal(l_DataRow["YMCAInterest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount;
                            l_Decimal_MinimumDistributionAmount = 0;
                        }
                        if (l_Decimal_MinimumDistributionAmount >= l_Decimal_YMCAInterest)
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_YMCAInterest;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_YMCAInterest;
                            l_Decimal_MinimumDistributionAmount = l_Decimal_MinimumDistributionAmount - l_Decimal_YMCAInterest;
                            l_DataRow["YMCAInterest"] = "0.00";
                            l_DataRow["YMCATotal"] = (Convert.ToDecimal(l_DataRow["YMCATaxable"])) + (Convert.ToDecimal(l_DataRow["YMCAInterest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                        }
                        else
                        {
                            l_Boolean_IsMinimumDistribution = true;
                            this.m_decimal_MinimumDistributionAmount = this.m_decimal_MinimumDistributionAmount + l_Decimal_MinimumDistributionAmount;
                            l_DataRow["YMCAInterest"] = l_Decimal_YMCAInterest - l_Decimal_MinimumDistributionAmount;
                            l_DataRow["YMCATotal"] = (Convert.ToDecimal(l_DataRow["YMCATaxable"])) + (Convert.ToDecimal(l_DataRow["YMCAInterest"]));
                            l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Emp.Total"])) + (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                            l_Decimal_Taxable = l_Decimal_Taxable + l_Decimal_MinimumDistributionAmount;
                            l_Decimal_MinimumDistributionAmount = 0;
                        }
                        if (l_Boolean_IsMinimumDistribution == true)
                        {
                            l_MinimumdistributionDataRow = l_MinimumDistributionTable.NewRow();
                            l_MinimumdistributionDataRow["Taxable"] = l_Decimal_Taxable;
                            l_MinimumdistributionDataRow["TaxRate"] = this.m_decimal_MinimumDistributionTaxRate;
                            l_MinimumdistributionDataRow["Tax"] = Math.Round(l_Decimal_Taxable * (this.m_decimal_MinimumDistributionTaxRate / 100), 2);
                            l_MinimumdistributionDataRow["NonTaxable"] = "0.00";
                            l_MinimumdistributionDataRow["AcctType"] = l_DataRow["AccountType"];
                            l_MinimumdistributionDataRow["Payee"] = this.m_string_GetPayeeName;
                            l_MinimumdistributionDataRow["FundedDate"] = System.DBNull.Value;
                            l_MinimumdistributionDataRow["RefRequestsID"] = this.m_string_RefRequestId;
                            l_MinimumDistributionTable.Rows.Add(l_MinimumdistributionDataRow);
                        }
                    }
                }

                if (l_Decimal_MinimumDistributionAmount > this.m_decimal_MinimumDistributionAmount)
                {
                    //todo n ask(to be discussed with mark)
                    //return (" Minimum Distrubtion Not Met. A Minimum Distribution of " + Math.Round(this.m_decimal_MinimumDistributionAmount, 2).ToString() + " is Required.. There isn't enough Taxable Money to Roll any over."); 
                }
                this.m_MinimumDistributionTable = l_MinimumDistributionTable;
                l_CurrentAccountDataTable.AcceptChanges();
                this.m_datatable_CalculatedDataTableForCurrentAccounts = l_CurrentAccountDataTable;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoMinimumDistributionCalculation Method", "Finish: Do RMD Calculation.");
                return " ";
            }
            catch
            {
                throw;
            }
        }


        private void CreateRowsForDisbursements()
        {
            DataTable l_AvailableBalanceDataTable;
            DataTable l_SelectDataTable = null;
            string[] l_AnnuityBasisTypeArray;
            string l_AccountType;
            DataRow[] l_AvailableBalanceDataRows;
            int l_Counter;
            decimal l_Decimal_PersonalPreTax;
            decimal l_Decimal_PersonalPostTax;
            decimal l_Decimal_YMCAPreTax;
            string l_AccountGroup = "";
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRowsForDisbursements Method", "Start: Create Rows For Disbursements.");
                l_AvailableBalanceDataTable = this.m_datatable_AvailableBalance;
                l_AnnuityBasisTypeArray = this.m_string_AnnuityBasisType;
                if ((l_AvailableBalanceDataTable != null) && (l_AnnuityBasisTypeArray != null))
                {
                    l_Counter = 0;
                    foreach (string l_AnnuityBasisType in l_AnnuityBasisTypeArray)
                    {
                        if (l_AnnuityBasisType != null)
                        {
                            l_AvailableBalanceDataRows = (this.GetDataRow(l_AvailableBalanceDataTable, l_AnnuityBasisType.Trim()));
                            if (l_AvailableBalanceDataRows != null)
                            {
                                foreach (DataRow l_AvailableBalanceDataRow in l_AvailableBalanceDataRows)
                                {
                                    if (l_AvailableBalanceDataRow != null)
                                    {
                                        if (l_AvailableBalanceDataRow["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                                        {
                                            l_Decimal_PersonalPreTax = 0;
                                        }
                                        else
                                        {
                                            l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPreTax"]);
                                        }
                                        if (l_AvailableBalanceDataRow["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                                        {
                                            l_Decimal_PersonalPostTax = 0;
                                        }
                                        else
                                        {
                                            l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPostTax"]);
                                        }
                                        if (l_AvailableBalanceDataRow["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                                        {
                                            l_Decimal_YMCAPreTax = 0;
                                        }
                                        else
                                        {
                                            l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalanceDataRow["YmcaPreTax"]);
                                        }


                                        if ((l_Decimal_PersonalPostTax + l_Decimal_PersonalPreTax + l_Decimal_YMCAPreTax) > 0)
                                        {
                                            l_AccountType = (Convert.ToString(l_AvailableBalanceDataRow["AcctType"]));
                                            l_AccountGroup = (Convert.ToString(l_AvailableBalanceDataRow["AccountGroup"]));
                                            for (int l_PayeeCounter = 1; l_PayeeCounter <= 2; l_PayeeCounter++)
                                            {
                                                if (l_PayeeCounter == 1)
                                                {
                                                    l_SelectDataTable = this.m_datatable_Payee1;
                                                }
                                                if (l_PayeeCounter == 2)
                                                {
                                                    l_SelectDataTable = this.m_MinimumDistributionTable;
                                                }

                                                DataRow l_SelectedRowFromDataTable;
                                                DataTable l_CurrentDataTable;
                                                DataRow l_CurrentDataRow;
                                                string l_TransactionType;

                                                l_CurrentDataTable = this.m_datatable_CalculatedDataTableForCurrentAccounts;
                                                if (l_SelectDataTable != null)
                                                {
                                                    l_SelectDataTable = this.ClearZerosRows(l_SelectDataTable);
                                                    l_SelectedRowFromDataTable = this.GetDataRowForSelectedTable(l_SelectDataTable, l_AccountType, "");
                                                    if (l_SelectedRowFromDataTable != null)
                                                    {
                                                        if (l_CurrentDataTable != null)
                                                        {
                                                            l_CurrentDataRow = this.GetDataRowForSelectedTable(l_CurrentDataTable, l_AccountType, "AccountType");
                                                            if (l_CurrentDataRow == null)
                                                            {
                                                                l_AvailableBalanceDataRow["PersonalPreTax"] = "0.00";
                                                                l_AvailableBalanceDataRow["PersonalPostTax"] = "0.00";
                                                                l_AvailableBalanceDataRow["YmcaPreTax"] = "0.00";
                                                            }
                                                            do
                                                            {
                                                                l_TransactionType = string.Empty;

                                                                if ((Convert.ToString(l_AvailableBalanceDataRow["MoneyType"])) == "PR")
                                                                {
                                                                    if (((Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPreTax"])) > 0) && ((Convert.ToDecimal(l_SelectedRowFromDataTable["Taxable"])) > 0))
                                                                    {
                                                                        l_TransactionType = "RFPR";
                                                                    }
                                                                    if (((Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPostTax"])) > 0) && ((Convert.ToDecimal(l_SelectedRowFromDataTable["NonTaxable"])) > 0))
                                                                    {
                                                                        l_TransactionType = "RFPR";
                                                                    }
                                                                    if (((Convert.ToDecimal(l_AvailableBalanceDataRow["YmcaPreTax"])) > 0) && ((Convert.ToDecimal(l_SelectedRowFromDataTable["Taxable"])) > 0))
                                                                    {
                                                                        l_TransactionType = "RFPR";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (((Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPreTax"])) > 0) && ((Convert.ToDecimal(l_SelectedRowFromDataTable["Taxable"])) > 0))
                                                                    {
                                                                        l_TransactionType = "RFIN";
                                                                    }
                                                                    if (((Convert.ToDecimal(l_AvailableBalanceDataRow["YmcaPreTax"])) > 0) && ((Convert.ToDecimal(l_SelectedRowFromDataTable["Taxable"])) > 0))
                                                                    {
                                                                        l_TransactionType = "RFIN";
                                                                    }
                                                                }
                                                                if (l_TransactionType == string.Empty)
                                                                {
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    l_AccountType = (Convert.ToString(l_AvailableBalanceDataRow["AcctType"]));
                                                                    l_AccountGroup = (Convert.ToString(l_AvailableBalanceDataRow["AccountGroup"]));
                                                                }
                                                                if (l_TransactionType.Trim() == "RFPR")
                                                                {
                                                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRowsForDisbursements Method", "Start: Calculating Refunf principle based on transaction type RFPR.");
                                                                    this.RefundPrinciple(l_SelectedRowFromDataTable, l_AvailableBalanceDataRow, l_AccountType, l_PayeeCounter, this.m_string_CurrencyCode, l_TransactionType, l_AnnuityBasisType.Trim(), l_AccountGroup.Trim().ToUpper());
                                                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRowsForDisbursements Method", "Finish: Calculating Refunf principle based on transaction type RFPR.");
                                                                }
                                                                else if (l_TransactionType.Trim() == "RFIN")
                                                                {
                                                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRowsForDisbursements Method", "Start: Calculating Refund Interest based on transaction type RFIN.");
                                                                    this.RefundInterest(l_SelectedRowFromDataTable, l_AvailableBalanceDataRow, l_AccountType, l_PayeeCounter, this.m_string_CurrencyCode, l_TransactionType, l_AnnuityBasisType.Trim(), l_AccountGroup.Trim().ToUpper());
                                                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRowsForDisbursements Method", "Finish: Calculating Refund Interest based on transaction type RFIN.");
                                                                }
                                                            }
                                                            while (true);

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        l_Counter += 1;
                    }
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateRowsForDisbursements Method", "Finish: Create Rows For Disbursements.");
            }
            catch
            {
                throw;
            }
        }

        private bool OpenFiles()
        {
            DataSet l_DataSet;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("OpenFiles Method", "Start: OpenFiles.");
                l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundTransactionSchemas();
                if (l_DataSet == null)
                {
                    return false;
                }
                //we will set the property of various schemas for refund processing
                this.m_datatable_Refund_Transactions = l_DataSet.Tables["Transactions"];
                this.m_datatable_Refund_Disbursements = l_DataSet.Tables["Disbursements"];
                this.m_datatable_Refund_DisbursementDetails = l_DataSet.Tables["DisbursementDetails"];
                this.m_datatable_Refund_DisbursementWithholding = l_DataSet.Tables["DisbursementWithholding"];
                this.m_datatable_Refund__DisbursementRefunds = l_DataSet.Tables["DisbursementRefunds"];
                this.m_datatable_Refund_MrdRecordsDisbursements = l_DataSet.Tables["MrdRecordsDisbursements"];
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("OpenFiles Method", "Finish: OpenFiles.");
                return true;
            }
            catch
            {
                throw;
            }
        }
        private DataRow SetDisbursementDataRow(int parameterIndex, string parameterCurrencyCode)
        {
            DataTable l_DataTable;
            DataRow l_DataRow;
            DataRow[] l_FoundRows;
            string l_QueryString = "";
            try
            {
                l_DataTable = this.m_datatable_Refund_Disbursements;
                l_DataRow = null;
                if (l_DataTable != null)
                {
                    if (parameterIndex == 1 || parameterIndex == 2)
                    {
                        //Added By SG: 2012.08.28: BT-960
                        //if (m_StringCashOutRefundType == "SHIRA")
                        if (m_StringCashOutRefundType == "SHIRA" && m_boolIsRMDeligible == false)
                            l_QueryString = "PayeeEntityID = '" + GetPayeeID("Millennium Trust", parameterIndex) + "'";
                        else
                            l_QueryString = "PayeeEntityID = '" + this.StringPersonId.ToString().Trim() + "'";
                    }

                    l_FoundRows = l_DataTable.Select(l_QueryString);
                    l_DataRow = null;
                    if (l_FoundRows != null)
                    {
                        if (l_FoundRows.Length > 0)
                        {
                            l_DataRow = l_FoundRows[0];
                        }
                    }
                    if (l_DataRow == null)
                    {
                        l_DataRow = l_DataTable.NewRow();
                        //l_DataRow["UniqueID"] = l_DataTable.Rows.Count;   // SR:2010.07.09
                        l_DataRow["UniqueID"] = Guid.NewGuid();    // SR:2010.07.09

                        //START: SG: 2012.06.14: BT-960
                        //if (parameterIndex == 1)
                        //{
                        //    l_DataRow["PayeeEntityID"] = this.StringPersonId;
                        //}
                        //else if (parameterIndex == 4)
                        //{
                        //    l_DataRow["PayeeEntityID"] = this.StringPersonId;
                        //}

                        //Added By SG: 2012.11.26: BT-960
                        //if (parameterIndex == 1)
                        if (parameterIndex == 1 || parameterIndex == 2)
                        {
                            //Added By SG: 2012.08.28: BT-960
                            //if (m_StringCashOutRefundType == "SHIRA")
                            if (m_StringCashOutRefundType == "SHIRA" && m_boolIsRMDeligible == false)
                                l_DataRow["PayeeEntityID"] = GetPayeeID("Millennium Trust", parameterIndex);
                            else
                                l_DataRow["PayeeEntityID"] = this.StringPersonId;
                        }
                        else if (parameterIndex == 4)
                        {
                            //Added By SG: 2012.08.28: BT-960
                            //if (m_StringCashOutRefundType == "SHIRA")
                            if (m_StringCashOutRefundType == "SHIRA" && m_boolIsRMDeligible == false)
                                l_DataRow["PayeeEntityID"] = GetPayeeID("Millennium Trust", parameterIndex);
                            else
                                l_DataRow["PayeeEntityID"] = this.StringPersonId;
                        }
                        //l_DataRow["PayeeAddrID"] = this.PayeeAddressID; this column will be inserted in the proc itself where it is picking the latest add id.
                        //l_DataRow["PayeeEntityTypeCode"] = "PERSON";
                        //l_DataRow["DisbursementType"] = "REF"; 
                        //END: SG: 2012.06.14: BT-960

                        l_DataRow["IrsTaxTypeCode"] = System.DBNull.Value;
                        l_DataRow["TaxableAmount"] = "0.00";
                        l_DataRow["NonTaxableAmount"] = "0.00";

                        //Commented By SG: 2012.08.28: BT-960
                        //if (m_StringCashOutRefundType == "SHIRA")
                        //    l_DataRow["PaymentMethodCode"] = "EFT";
                        //else
                        l_DataRow["PaymentMethodCode"] = "CHECK";

                        l_DataRow["CurrencyCode"] = parameterCurrencyCode;
                        l_DataRow["BankID"] = System.DBNull.Value;
                        l_DataRow["DisbursementNumber"] = System.DBNull.Value;

                        //START: SG: 2012.06.14: BT-960
                        l_DataRow["DisbursementType"] = "REF";

                        //Added By SG: 2012.08.28: BT-960
                        //if (m_StringCashOutRefundType == "SHIRA")
                        if (m_StringCashOutRefundType == "SHIRA" && m_boolIsRMDeligible == false)
                            l_DataRow["PayeeEntityTypeCode"] = "ROLINS";
                        else
                            l_DataRow["PayeeEntityTypeCode"] = "PERSON";
                        //END: SG: 2012.06.14: BT-960

                        l_DataRow["PersID"] = this.StringPersonId;
                        l_DataRow["DisbursementRefID"] = System.DBNull.Value;
                        l_DataRow["Rollover"] = System.DBNull.Value;
                        l_DataTable.Rows.Add(l_DataRow);
                        return l_DataRow;
                    }
                    else
                    {
                        return l_DataRow;
                    }
                }
                return l_DataRow;
            }
            catch
            {
                throw;
            }
        }

        //START: SG: 2012.06.14: BT-960
        private string GetPayeeID(string parameterPayeeName, int parameterIndex)
        {
            string PayeeID = "";

            try
            {
                string l_string_RolloverInstitutionID;
                YMCARET.YmcaBusinessObject.RefundRequest.Get_RefundRolloverInstitutionID(parameterPayeeName, out l_string_RolloverInstitutionID);

                if (l_string_RolloverInstitutionID == "")
                    return "Unable to retrieve Rollover Institution Information Data";
                else
                    if (parameterIndex == 1 || parameterIndex == 2)
                    {
                        PayeeID = l_string_RolloverInstitutionID;
                        return PayeeID;
                    }
                    else
                        return string.Empty;
            }
            catch
            {
                throw;
            }
        }

        //END: SG: 2012.06.14: BT-960

        private void CreatePayees()
        {
            DataTable l_CurrentDataTable;
            DataTable l_PayeeDataTable;
            DataTable l_Payee1DataTable;
            DataRow l_PayeeDataRow;
            string l_AccountType;
            decimal l_Decimal_PersonalPreTax;
            decimal l_Decimal_PersonalInterest;
            decimal l_Decimal_YMCAPreTax;
            decimal l_Decimal_YMCAInterest;


            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreatePayees Method", "Start: Create Payee Records.");
                l_CurrentDataTable = this.m_datatable_CalculatedDataTableForCurrentAccounts;
                l_PayeeDataTable = this.m_datatable_AtsRefund; //todo(check)
                if ((l_CurrentDataTable != null) && (l_PayeeDataTable != null))
                {
                    l_Payee1DataTable = l_PayeeDataTable.Clone();
                    foreach (DataRow l_DataRow in l_CurrentDataTable.Rows)
                    {
                        if (!(l_DataRow["AccountType"].GetType().ToString().Trim() == "System.DBNull"))
                        {
                            l_AccountType = (Convert.ToString(l_DataRow["AccountType"])).Trim();

                            if ((l_AccountType != "") && (l_AccountType != "Total"))
                            {
                                if (this.IsExistInRequestedAccounts(l_AccountType.Trim().ToUpper()) == true)
                                {
                                    l_PayeeDataRow = l_Payee1DataTable.NewRow();
                                    if (l_DataRow["Taxable"].GetType().ToString() == "System.DBNull")
                                    {
                                        l_Decimal_PersonalPreTax = 0;
                                    }
                                    else
                                    {
                                        l_Decimal_PersonalPreTax = (Convert.ToDecimal(l_DataRow["Taxable"]));
                                    }
                                    if (l_DataRow["Interest"].GetType().ToString() == "System.DBNull")
                                    {
                                        l_Decimal_PersonalInterest = 0;
                                    }
                                    else
                                    {
                                        l_Decimal_PersonalInterest = (Convert.ToDecimal(l_DataRow["Interest"]));
                                    }
                                    if (l_DataRow["YMCATaxable"].GetType().ToString() == "System.DBNull")
                                    {
                                        l_Decimal_YMCAPreTax = 0;
                                    }
                                    else
                                    {
                                        l_Decimal_YMCAPreTax = (Convert.ToDecimal(l_DataRow["YMCATaxable"]));
                                    }
                                    if (l_DataRow["YMCAInterest"].GetType().ToString() == "System.DBNull")
                                    {
                                        l_Decimal_YMCAInterest = 0;
                                    }
                                    else
                                    {
                                        l_Decimal_YMCAInterest = (Convert.ToDecimal(l_DataRow["YMCAInterest"]));
                                    }
                                    l_PayeeDataRow["AcctType"] = l_DataRow["AccountType"];
                                    l_PayeeDataRow["Taxable"] = l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax;
                                    l_PayeeDataRow["NonTaxable"] = l_DataRow["Non-Taxable"];

                                    //START: Added By SG: 2012.09.06: BT-960
                                    Int32 l_int_TaxRate = 0;
                                    if (m_StringCashOutRefundType == "SHIRA" && m_boolIsRMDeligible == false)
                                        l_int_TaxRate = 0;
                                    else
                                    {
                                        //Start AA:06.16.2016 YRS-AT-3015 Modified below to use the federala tax rate from atsmetaconfiguration
                                        //    l_int_TaxRate = 20;
                                        DataSet dsConfiguration;
                                        dsConfiguration = YMCACommonBOClass.getConfigurationValue("REFUND_FEDERALTAXRATE");
                                        if (YMCARET.YmcaDataAccessObject.HelperFunctions.isNonEmpty(dsConfiguration))
                                        {
                                            if (dsConfiguration.Tables["MetaConfigDeathValue"].Columns.Contains("Value") && dsConfiguration.Tables["MetaConfigDeathValue"].Rows[0]["Value"] != DBNull.Value && !string.IsNullOrEmpty(dsConfiguration.Tables["MetaConfigDeathValue"].Rows[0]["Value"].ToString()))
                                            {
                                                l_int_TaxRate = Convert.ToInt32(dsConfiguration.Tables["MetaConfigDeathValue"].Rows[0]["Value"]);
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("'REFUND_FEDERALTAXRATE' key not defined in atsMetaConfiguration");
                                        }
                                        //End AA:06.16.2016 YRS-AT-3015 Modified below to use the federala tax rate from atsmetaconfiguration
                                    }
                                    l_PayeeDataRow["TaxRate"] = l_int_TaxRate;
                                    //l_PayeeDataRow["Tax"] = (l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax) * (20 / 100);//todo heres its TaxRate/100
                                    l_PayeeDataRow["Tax"] = (l_Decimal_PersonalInterest + l_Decimal_PersonalPreTax + l_Decimal_YMCAInterest + l_Decimal_YMCAPreTax) * (l_int_TaxRate / 100);
                                    //END: Added By SG: 2012.09.06: BT-960

                                    l_PayeeDataRow["Payee"] = this.m_string_GetPayeeName;
                                    l_PayeeDataRow["FundedDate"] = System.DBNull.Value;

                                    //Added By SG: 2012.06.18: BT-960
                                    //l_PayeeDataRow["RequestType"] = "CASH";   //check with refunds what shud be in this field
                                    l_PayeeDataRow["RequestType"] = m_StringCashOutRefundType;

                                    l_PayeeDataRow["RefRequestsID"] = this.StringRefRequestId;
                                    l_Payee1DataTable.Rows.Add(l_PayeeDataRow);
                                }
                            }
                        }
                    }
                    this.m_datatable_Payee1 = l_Payee1DataTable;

                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreatePayees Method", "Finish: Create Payee Records.");
            }
            catch
            {
                throw;
            }
        }

        private void LoadRequestedAccounts(YMCARET.YmcaDataAccessObject.CashOutDAClass paraCashOutDA)
        {
            DataTable l_DataTable;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("LoadRequestedAccounts Method", "Start: Load Requested Accounts.");
                if (this.StringRefRequestId != string.Empty)
                {
                    //Commented by Ashish 2010.05.07
                    //l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundRequestsDetails(this.StringRefRequestId); 
                    l_DataTable = paraCashOutDA.GetRefundRequestDetails(this.StringRefRequestId);
                    this.CalculateTotal(l_DataTable, 0);
                    this.m_datatable_RequestedAccounts = l_DataTable;

                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("LoadRequestedAccounts Method", "Finish: Load Requested Accounts.");
            }
            catch
            {
                throw;
            }
        }
        private bool IsExistInRequestedAccounts(string parameterAccountType)
        {
            DataTable l_RequestedDataTable;
            DataRow[] l_FoundRows;
            string l_QueryString;
            try
            {
                l_RequestedDataTable = this.m_datatable_RequestedAccounts;
                if (l_RequestedDataTable != null)
                {
                    l_QueryString = "AccountType = '" + parameterAccountType.Trim().ToUpper() + "'";
                    l_FoundRows = l_RequestedDataTable.Select(l_QueryString);
                    if (l_FoundRows != null)
                    {
                        if (l_FoundRows.Length > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
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

        private void CalculateTotal(DataTable parameterDataTable, int parameterCalledBy)
        {
            DataTable l_CalculatedDataTable = null;
            DataRow l_DataRow = null;
            bool l_BooleanFlag = false;
            try
            {
                if (parameterDataTable != null)
                {
                    l_CalculatedDataTable = parameterDataTable;
                }

                //				else 
                //				{ 
                //					l_CalculatedDataTable = parameterDataTable;//TODO: if not this then take the data table inserted in request
                //				} 
                if (l_CalculatedDataTable != null)
                {
                    foreach (DataRow l_CalculatedDataRow in l_CalculatedDataTable.Rows)
                    {
                        if ((l_CalculatedDataRow["AccountType"].GetType().ToString() != "System.DBNull"))
                        {
                            if ((Convert.ToString(l_CalculatedDataRow["AccountType"])) == "Total")
                            {
                                l_BooleanFlag = true;
                                l_DataRow = l_CalculatedDataRow;
                                break;
                            }
                        }
                    }

                    if (l_BooleanFlag == false)
                    {
                        l_DataRow = l_CalculatedDataTable.NewRow();

                    }
                    else
                    {
                        l_DataRow["Taxable"] = "0.00";
                        l_DataRow["Non-Taxable"] = "0.00";
                        l_DataRow["Interest"] = "0.00";
                        l_DataRow["Emp.Total"] = "0.00";
                        l_DataRow["YMCATaxable"] = "0.00";
                        l_DataRow["YMCAInterest"] = "0.00";
                        l_DataRow["YMCATotal"] = "0.00";
                        l_DataRow["Total"] = "0.00";
                    }
                    l_DataRow["Taxable"] = l_CalculatedDataTable.Compute("SUM (Taxable)", "");
                    l_DataRow["Non-Taxable"] = l_CalculatedDataTable.Compute("SUM ([Non-Taxable])", "");
                    l_DataRow["Interest"] = l_CalculatedDataTable.Compute("SUM (Interest)", "");
                    l_DataRow["Emp.Total"] = l_CalculatedDataTable.Compute("SUM ([Emp.Total])", "");
                    l_DataRow["YMCATaxable"] = l_CalculatedDataTable.Compute("SUM (YMCATaxable)", "");
                    l_DataRow["YMCAInterest"] = l_CalculatedDataTable.Compute("SUM (YMCAInterest)", "");
                    l_DataRow["YMCATotal"] = l_CalculatedDataTable.Compute("SUM (YMCATotal)", "");
                    l_DataRow["Total"] = l_CalculatedDataTable.Compute("SUM (Total)", "");
                    if (l_DataRow["Taxable"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["Taxable"] = "0.00";
                    }
                    if (l_DataRow["Non-Taxable"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["Non-Taxable"] = "0.00";
                    }
                    if (l_DataRow["Interest"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["Interest"] = "0.00";
                    }
                    if (l_DataRow["Emp.Total"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["Emp.Total"] = "0.00";
                    }
                    if (l_DataRow["YMCATaxable"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["YMCATaxable"] = "0.00";
                    }
                    if (l_DataRow["YMCAInterest"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["YMCAInterest"] = "0.00";
                    }
                    if (l_DataRow["YMCATotal"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["YMCATotal"] = "0.00";
                    }
                    if (l_DataRow["Total"].GetType().ToString() == "System.DBNull")
                    {
                        l_DataRow["Total"] = "0.00";
                    }
                    if (l_BooleanFlag == false)
                    {
                        l_DataRow["AccountType"] = "Total";
                        l_CalculatedDataTable.Rows.Add(l_DataRow);
                    }
                }
                if (parameterCalledBy == 1)
                {
                    this.m_datatable_CalculatedDataTableForCurrentAccounts = l_CalculatedDataTable;
                }
                else
                {
                    //l_CalculatedDataTable = parameterDataTable;//TODO: if not this then take the data table inserted in request
                }
            }
            catch
            {
                throw;
            }
        }


        private DataRow SetDisbursementDetailsDataRow(string parameterAccountType, string parameterAcctBreakdownType, string parameterDisbursementID, int parameterSortOrder)
        {
            DataTable l_DataTable;
            DataRow l_DataRow;
            DataRow[] l_FoundRows;
            string l_QueryString;
            try
            {
                l_DataTable = this.m_datatable_Refund_DisbursementDetails;
                l_DataRow = null;
                if (l_DataTable != null)
                {
                    l_QueryString = "AcctType = '" + parameterAccountType.ToString() + "' AND AcctBreakdownType = '" + parameterAcctBreakdownType + "' AND DisbursementID = '" + parameterDisbursementID + "'";
                    l_FoundRows = l_DataTable.Select(l_QueryString);
                    l_DataRow = null;
                    if (l_FoundRows != null)
                    {
                        if (l_FoundRows.Length > 0)
                        {
                            l_DataRow = l_FoundRows[0];
                        }
                    }
                    if (l_DataRow == null)
                    {
                        //l_DataRow = l_DataTable.NewRow();
                        l_DataRow = l_DataTable.NewRow();
                        //l_DataRow["UniqueID"] = System.DBNull.Value; //SR:2010.07.09 - Enhancement changes.
                        l_DataRow["UniqueID"] = Guid.NewGuid();
                        l_DataRow["DisbursementID"] = parameterDisbursementID;
                        l_DataRow["AcctType"] = parameterAccountType;
                        l_DataRow["AcctBreakdownType"] = parameterAcctBreakdownType;
                        l_DataRow["SortOrder"] = parameterSortOrder;
                        l_DataRow["TaxablePrincipal"] = "0.00";
                        l_DataRow["TaxableInterest"] = "0.00";
                        l_DataRow["NonTaxablePrincipal"] = "0.00";
                        l_DataRow["TaxWithheldPrincipal"] = "0.00";
                        l_DataRow["TaxWithheldInterest"] = "0.00";
                        l_DataTable.Rows.Add(l_DataRow);
                        return l_DataRow;
                    }
                    else
                    {
                        return l_DataRow;
                    }
                }
                return l_DataRow;
            }
            catch
            {
                throw;
            }
        }
        private DataRow SetRefundsDataRow(string parameterTransactionID, DataRow paramterSelectedDataRow, string parameterAccountType, string parameterDisbursementID, string parameterAnnuityBasisType)
        {
            DataTable l_DataTable;
            DataRow[] l_FoundRows;
            DataRow l_DataRow;
            string l_QueryString;
            try
            {
                l_DataTable = this.m_datatable_AtsRefund;
                l_DataRow = null;
                if ((l_DataTable != null))
                {
                    l_QueryString = "TransactID = '" + parameterTransactionID + "'";
                    l_FoundRows = l_DataTable.Select(l_QueryString);
                    l_DataRow = null;
                    if ((l_FoundRows != null))
                    {
                        if (l_FoundRows.Length > 0)
                        {
                            l_DataRow = l_FoundRows[0];
                        }
                    }
                    if (l_DataRow == null)
                    {
                        l_DataRow = l_DataTable.NewRow();
                        //l_DataRow["Uniqueid"] = System.DBNull.Value;
                        l_DataRow["UniqueID"] = Guid.NewGuid();  //SR:2010.07.09 - Enhancement changes.
                        l_DataRow["RefRequestsID"] = this.m_string_RefRequestId;
                        l_DataRow["AcctType"] = parameterAccountType;
                        l_DataRow["Taxable"] = "0.00";
                        l_DataRow["NonTaxable"] = "0.00";
                        l_DataRow["Tax"] = "0.00";
                        l_DataRow["TaxRate"] = "0.00";
                        l_DataRow["Payee"] = paramterSelectedDataRow["Payee"];
                        l_DataRow["FundedDate"] = System.DBNull.Value;

                        //Added By SG: 2012.06.18: BT-960
                        //l_DataRow["RequestType"] = "CASH";
                        l_DataRow["RequestType"] = m_StringCashOutRefundType;

                        l_DataRow["TransactID"] = parameterTransactionID;
                        l_DataRow["AnnuityBasisType"] = parameterAnnuityBasisType;
                        l_DataRow["DisbursementID"] = parameterDisbursementID;
                        l_DataTable.Rows.Add(l_DataRow);
                        return l_DataRow;
                    }
                    else
                    {
                        return l_DataRow;
                    }
                }
                return l_DataRow;
            }
            catch
            {
                throw;
            }
        }

        private DataRow SetTransactionDataRow(DataRow parameterAvailableBalance, int parameterIndex, string parameterTransactionType)
        {
            DataTable l_DataTable;
            DataRow l_DataRow;
            DataRow[] l_FoundRows;
            string l_QueryString;
            string l_PayeeID = "";
            string l_AccountType;
            string l_TransactType;
            string l_AnnuityBasisType;
            try
            {
                l_DataTable = this.m_datatable_Refund_Transactions;
                l_DataRow = null;
                if ((parameterIndex == 1) || (parameterIndex == 2))
                {
                    l_PayeeID = this.StringPersonId;
                }

                if ((l_DataTable != null) && (parameterAvailableBalance != null))
                {
                    l_AccountType = (Convert.ToString(parameterAvailableBalance["AcctType"]));
                    l_TransactType = parameterTransactionType;
                    l_AnnuityBasisType = (Convert.ToString(parameterAvailableBalance["AnnuityBasisType"]));
                    l_QueryString = "AcctType = '" + l_AccountType.Trim() + "' AND TransactType = '" + l_TransactType.Trim() + "' AND AnnuityBasisType = '" + l_AnnuityBasisType.Trim() + "' AND Creator = '" + l_PayeeID.Trim() + "'";
                    l_FoundRows = l_DataTable.Select(l_QueryString);
                    l_DataRow = null;
                    string l_AvailableBalanceRefID;
                    string l_TempDataRowRefID;
                    if (l_FoundRows != null)
                    {
                        if (l_FoundRows.Length > 0)
                        {
                            l_DataRow = l_FoundRows[0];
                            if (parameterAvailableBalance["TransactionRefID"].GetType().ToString().Trim() == "System.DBNull")
                            {
                                l_AvailableBalanceRefID = string.Empty;
                            }
                            else
                            {
                                l_AvailableBalanceRefID = parameterAvailableBalance["TransactionRefID"].ToString();
                            }
                            if (l_DataRow["TransactionRefID"].GetType().ToString().Trim() == "System.DBNull")
                            {
                                l_TempDataRowRefID = string.Empty;
                            }
                            else
                            {
                                l_TempDataRowRefID = l_DataRow["TransactionRefID"].ToString();
                            }
                            if (l_AvailableBalanceRefID.Trim() != l_TempDataRowRefID.Trim())
                            {
                                l_DataRow = null;
                            }
                        }
                    }
                    if (l_DataRow == null)
                    {
                        l_DataRow = l_DataTable.NewRow();
                        //l_DataRow["UniqueID"] = l_DataTable.Rows.Count; 
                        l_DataRow["UniqueID"] = Guid.NewGuid(); //SR:2010.07.09 - Enhancement changes.
                        l_DataRow["PersID"] = this.m_string_PersonId;
                        l_DataRow["FundEventID"] = this.m_string_FundEventId;
                        l_DataRow["YmcaID"] = System.DBNull.Value;
                        l_DataRow["AcctType"] = l_AccountType;
                        l_DataRow["TransactType"] = l_TransactType;
                        l_DataRow["AnnuityBasisType"] = l_AnnuityBasisType;
                        l_DataRow["MonthlyComp"] = "0.00";
                        l_DataRow["PersonalPreTax"] = "0.00";
                        l_DataRow["PersonalPostTax"] = "0.00";
                        l_DataRow["YmcaPreTax"] = "0.00";
                        l_DataRow["ReceivedDate"] = System.DateTime.Now;
                        l_DataRow["AccountingDate"] = System.DateTime.Now;
                        l_DataRow["TransactDate"] = System.DateTime.Now;
                        l_DataRow["FundedDate"] = System.DateTime.Now;
                        l_DataRow["TransmittalID"] = System.DBNull.Value;
                        l_DataRow["TransactionRefID"] = parameterAvailableBalance["TransactionRefID"];
                        l_DataRow["Creator"] = l_PayeeID;
                        l_DataTable.Rows.Add(l_DataRow);
                        return l_DataRow;
                    }
                    else
                    {
                        return l_DataRow;
                    }
                }
                return l_DataRow;
            }
            catch
            {
                throw;
            }
        }

        private void ReduceAvailableBalance()
        {
            DataTable l_RefundableDataTable;
            DataTable l_ArrayRefundDataTable;
            DataTable l_AvailableBalanceDataTable;
            string[] l_AnnuityBasisArray;
            string l_AnnuityBasisType;
            string l_TempString;
            decimal l_Decimal_PersonalInterest;
            decimal l_Decimal_YMCAInterest;
            DataRow[] l_FoundRows;
            DataRow l_ArrayRefundDataRow;
            bool l_BooleanFlag = false;
            decimal l_Decimal_PersonalPreTax;
            decimal l_Decimal_PersonalPostTax;
            decimal l_Decimal_YMCAPreTax;

            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("ReduceAvailableBalance Method", "Start: Reduce Available Balance.");
                l_AnnuityBasisArray = this.m_string_AnnuityBasisType;
                l_RefundableDataTable = this.m_RefundableDataTable;
                l_ArrayRefundDataTable = l_RefundableDataTable.Clone();
                if ((l_AnnuityBasisArray != null) && (l_RefundableDataTable != null))
                {
                    for (int l_Counter = 0; l_Counter <= l_AnnuityBasisArray.Length - 1; l_Counter++)
                    {
                        l_AnnuityBasisType = l_AnnuityBasisArray[l_Counter];
                        if (l_AnnuityBasisType != null)
                        {
                            foreach (DataRow l_RefundDataRow in l_RefundableDataTable.Rows)
                            {
                                if (l_RefundDataRow["AnnuityBasisType"].GetType().ToString() != "System.DBNull")
                                {
                                    if (l_AnnuityBasisType.Trim() == (Convert.ToString(l_RefundDataRow["AnnuityBasisType"])).Trim())
                                    {
                                        if (((l_RefundDataRow["Interest"].GetType().ToString().Trim() != "System.DBNull")))
                                        {
                                            l_Decimal_PersonalInterest = (Convert.ToDecimal(l_RefundDataRow["Interest"]));
                                        }
                                        else
                                        {
                                            l_Decimal_PersonalInterest = 0;
                                        }
                                        if (((l_RefundDataRow["YMCAInterest"].GetType().ToString().Trim() != "System.DBNull")))
                                        {
                                            l_Decimal_YMCAInterest = (Convert.ToDecimal(l_RefundDataRow["YMCAInterest"]));
                                        }
                                        else
                                        {
                                            l_Decimal_YMCAInterest = 0;
                                        }
                                        if (l_Decimal_PersonalInterest + l_Decimal_YMCAInterest > 0)
                                        {
                                            l_TempString = (Convert.ToString(l_RefundDataRow["AnnuityBasisType"])) + (Convert.ToString(l_RefundDataRow["AccountType"])) + "IN";
                                            l_FoundRows = l_ArrayRefundDataTable.Select("AnnuityBasisType = '" + l_TempString + "'");
                                            if (!(l_FoundRows == null))
                                            {
                                                if (l_FoundRows.Length < 1)
                                                {
                                                    l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow();
                                                    l_ArrayRefundDataRow["AnnuityBasisType"] = l_TempString;
                                                    l_ArrayRefundDataRow["Taxable"] = "0.00";
                                                    l_ArrayRefundDataRow["Non-Taxable"] = "0.00";
                                                    l_ArrayRefundDataRow["YMCATaxable"] = "0.00";
                                                    l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow);
                                                }
                                                else
                                                {
                                                    l_ArrayRefundDataRow = l_FoundRows[0];
                                                }
                                            }
                                            else
                                            {
                                                l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow();
                                                l_ArrayRefundDataRow["AnnuityBasisType"] = l_TempString;
                                                l_ArrayRefundDataRow["Taxable"] = "0.00";
                                                l_ArrayRefundDataRow["Non-Taxable"] = "0.00";
                                                l_ArrayRefundDataRow["YMCATaxable"] = "0.00";
                                                l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow);
                                            }
                                            if (l_RefundDataRow["Interest"].GetType().ToString() == "System.DBNull")
                                            {
                                                l_ArrayRefundDataRow["Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Taxable"])) + 0;
                                            }
                                            else
                                            {
                                                l_ArrayRefundDataRow["Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Taxable"])) + (Convert.ToDecimal(l_RefundDataRow["Interest"]));
                                            }

                                            l_ArrayRefundDataRow["Non-Taxable"] = "0.00";
                                            if (l_RefundDataRow["YMCAInterest"].GetType().ToString() == "System.DBNull")
                                            {
                                                l_ArrayRefundDataRow["YMCATaxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["YMCATaxable"])) + 0;
                                            }
                                            else
                                            {
                                                l_ArrayRefundDataRow["YMCATaxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["YMCATaxable"])) + (Convert.ToDecimal(l_RefundDataRow["YMCAInterest"]));
                                            }

                                        }
                                        l_TempString = (Convert.ToString(l_RefundDataRow["AnnuityBasisType"])) + (Convert.ToString(l_RefundDataRow["AccountType"])) + "PR";
                                        l_FoundRows = l_ArrayRefundDataTable.Select("AnnuityBasisType = '" + l_TempString + "'");
                                        if (l_FoundRows != null)
                                        {
                                            if (l_FoundRows.Length < 1)
                                            {
                                                l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow();
                                                l_ArrayRefundDataRow["AnnuityBasisType"] = l_TempString;
                                                l_ArrayRefundDataRow["Taxable"] = "0.00";
                                                l_ArrayRefundDataRow["Non-Taxable"] = "0.00";
                                                l_ArrayRefundDataRow["YMCATaxable"] = "0.00";
                                                l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow);
                                            }
                                            else
                                            {
                                                l_ArrayRefundDataRow = l_FoundRows[0];
                                            }
                                        }
                                        else
                                        {
                                            l_ArrayRefundDataRow = l_ArrayRefundDataTable.NewRow();
                                            l_ArrayRefundDataRow["AnnuityBasisType"] = l_TempString;
                                            l_ArrayRefundDataRow["Taxable"] = "0.00";
                                            l_ArrayRefundDataRow["Non-Taxable"] = "0.00";
                                            l_ArrayRefundDataRow["YMCATaxable"] = "0.00";
                                            l_ArrayRefundDataTable.Rows.Add(l_ArrayRefundDataRow);
                                        }
                                        if (l_RefundDataRow["Taxable"].GetType().ToString() == "System.DBNull")
                                        {
                                            l_ArrayRefundDataRow["Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Taxable"])) + 0;
                                        }
                                        else
                                        {
                                            l_ArrayRefundDataRow["Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Taxable"])) + (Convert.ToDecimal(l_RefundDataRow["Taxable"]));
                                        }

                                        if (l_RefundDataRow["Non-Taxable"].GetType().ToString() == "System.DBNull")
                                        {
                                            l_ArrayRefundDataRow["Non-Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Non-Taxable"])) + 0;
                                        }
                                        else
                                        {
                                            l_ArrayRefundDataRow["Non-Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Non-Taxable"])) + (Convert.ToDecimal(l_RefundDataRow["Non-Taxable"]));
                                        }

                                        if (l_RefundDataRow["YMCATaxable"].GetType().ToString() == "System.DBNull")
                                        {
                                            l_ArrayRefundDataRow["YMCATaxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["YMCATaxable"])) + 0;
                                        }
                                        else
                                        {
                                            l_ArrayRefundDataRow["YMCATaxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["YMCATaxable"])) + (Convert.ToDecimal(l_RefundDataRow["YMCATaxable"]));
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                this.m_ArrayRefundDataTable = l_ArrayRefundDataTable;
                l_AvailableBalanceDataTable = this.m_datatable_AvailableBalance;
                if (((l_AnnuityBasisArray != null)) && ((l_AvailableBalanceDataTable != null)))
                {
                    for (int l_Counter = 0; l_Counter <= l_AnnuityBasisArray.Length - 1; l_Counter++)
                    {
                        l_AnnuityBasisType = l_AnnuityBasisArray[l_Counter];
                        if ((l_AnnuityBasisType != null))
                        {
                            foreach (DataRow l_AvailableBalanceDataRow in l_AvailableBalanceDataTable.Rows)
                            {
                                if (l_AvailableBalanceDataRow.RowState != DataRowState.Deleted)
                                {
                                    if ((l_AvailableBalanceDataRow["AnnuityBasisType"].GetType().ToString() != "System.DBNull"))
                                    {
                                        if (l_AnnuityBasisType.Trim() == (Convert.ToString(l_AvailableBalanceDataRow["AnnuityBasisType"])).Trim())
                                        {
                                            l_TempString = Convert.ToString(l_AvailableBalanceDataRow["AnnuityBasisType"]) + Convert.ToString(l_AvailableBalanceDataRow["AcctType"]) + Convert.ToString(l_AvailableBalanceDataRow["MoneyType"]);
                                            l_FoundRows = l_ArrayRefundDataTable.Select("AnnuityBasisType = '" + l_TempString + "'");
                                            l_BooleanFlag = false;
                                            if (l_FoundRows != null)
                                            {
                                                if (l_FoundRows.Length < 1)
                                                {
                                                    l_BooleanFlag = false;
                                                }
                                                else
                                                {
                                                    l_BooleanFlag = true;
                                                }
                                            }
                                            else
                                            {
                                                l_BooleanFlag = false;
                                            }
                                            if (l_BooleanFlag == false)
                                            {
                                                if (l_AvailableBalanceDataRow.RowState != DataRowState.Deleted)
                                                {

                                                    l_AvailableBalanceDataRow.Delete();

                                                }
                                            }
                                            else
                                            {
                                                l_ArrayRefundDataRow = l_FoundRows[0];
                                                if (l_AvailableBalanceDataRow.RowState != DataRowState.Deleted)
                                                {
                                                    if (l_AvailableBalanceDataRow["PersonalPreTax"].GetType().ToString() == "System.DBNull")
                                                    {
                                                        l_Decimal_PersonalPreTax = 0;
                                                    }
                                                    else
                                                    {
                                                        l_Decimal_PersonalPreTax = Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPreTax"]);
                                                    }
                                                    if (l_AvailableBalanceDataRow["PersonalPostTax"].GetType().ToString() == "System.DBNull")
                                                    {
                                                        l_Decimal_PersonalPostTax = 0;
                                                    }
                                                    else
                                                    {
                                                        l_Decimal_PersonalPostTax = Convert.ToDecimal(l_AvailableBalanceDataRow["PersonalPostTax"]);
                                                    }
                                                    if (l_AvailableBalanceDataRow["YmcaPreTax"].GetType().ToString() == "System.DBNull")
                                                    {
                                                        l_Decimal_YMCAPreTax = 0;
                                                    }
                                                    else
                                                    {
                                                        l_Decimal_YMCAPreTax = Convert.ToDecimal(l_AvailableBalanceDataRow["YmcaPreTax"]);
                                                    }

                                                    if (l_Decimal_PersonalPreTax >= (Convert.ToDecimal(l_ArrayRefundDataRow["Taxable"])))
                                                    {
                                                        l_AvailableBalanceDataRow["PersonalPreTax"] = l_ArrayRefundDataRow["Taxable"];
                                                        l_ArrayRefundDataRow["Taxable"] = "0.00";
                                                    }
                                                    else
                                                    {
                                                        l_ArrayRefundDataRow["Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Taxable"])) - l_Decimal_PersonalPreTax;
                                                    }
                                                    if (l_Decimal_PersonalPostTax >= (Convert.ToDecimal(l_ArrayRefundDataRow["Non-Taxable"])))
                                                    {
                                                        l_AvailableBalanceDataRow["PersonalPostTax"] = l_ArrayRefundDataRow["Non-Taxable"];
                                                        l_ArrayRefundDataRow["Non-Taxable"] = "0.00";
                                                    }
                                                    else
                                                    {
                                                        l_ArrayRefundDataRow["Non-Taxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["Non-Taxable"])) - l_Decimal_PersonalPostTax;
                                                    }
                                                    if (l_Decimal_YMCAPreTax >= (Convert.ToDecimal(l_ArrayRefundDataRow["YMCATaxable"])))
                                                    {
                                                        l_AvailableBalanceDataRow["YmcaPreTax"] = l_ArrayRefundDataRow["YMCATaxable"];
                                                        l_ArrayRefundDataRow["YMCATaxable"] = "0.00";
                                                    }
                                                    else
                                                    {
                                                        l_ArrayRefundDataRow["YMCATaxable"] = (Convert.ToDecimal(l_ArrayRefundDataRow["YMCATaxable"])) - l_Decimal_YMCAPreTax;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.m_ArrayRefundDataTable = l_ArrayRefundDataTable;
                    l_AvailableBalanceDataTable.AcceptChanges();
                    this.m_datatable_AvailableBalance = l_AvailableBalanceDataTable;
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("ReduceAvailableBalance Method", "Finish: Reduce Available Balance.");
            }
            catch
            {
                throw;
            }
        }
        private void LoadSchemaRefundTable()
        {
            DataSet l_DataSet;

            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("GetSchemaRefundTable Method", "Start: GetSchemaRefundTable.");
                l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSchemaRefundTable();
                if ((l_DataSet != null))
                {
                    this.m_datatable_AtsRefund = l_DataSet.Tables["atsRefunds"];

                    this.m_datatable_AtsRefRequests = l_DataSet.Tables["atsRefRequests"];

                    this.m_datatable_AtsRefRequestDetails = l_DataSet.Tables["atsRefRequestDetails"];

                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("GetSchemaRefundTable Method", "Finish: GetSchemaRefundTable.");
            }
            catch
            {
                throw;
            }
        }
        private void LoadRefundableDataTable(YMCARET.YmcaDataAccessObject.CashOutDAClass paraCashOutDA)
        {
            DataTable l_RefundableDataTable;

            try
            {
                //Commented by ashish 2010.05.07 resolve cashout lock issue
                //l_RefundableDataTable = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundableDataTable(this.m_string_FundEventId); 
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("LoadRefundableDataTable Method", "Start: Load Refundable DataTable.");
                l_RefundableDataTable = paraCashOutDA.GetDataTableRefundable(this.m_string_FundEventId);
                this.DoFilterRefundableTable(l_RefundableDataTable);
                bool l_Bool_Useit;
                bool l_Bool_Yside;
                this.m_decimal_YmcaAvailableAmount = 0;
                foreach (DataRow l_CurrentRow in l_RefundableDataTable.Rows)
                {
                    l_Bool_Useit = true;
                    l_Bool_Yside = true;
                    if (this.IsBasicAccount(l_CurrentRow))
                    {
                        l_Bool_Useit = true;
                        if (this.BoolIsMemberVested == true && (this.DecimalTerminationPIA < this.DecimalMaxPIAAmount))
                        {
                            l_Bool_Yside = true;
                        }
                        else
                        {
                            l_Bool_Yside = false;
                        }

                    }
                    //START - Retirement plan account groups

                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_AM)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = true;
                    }
                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_AP)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = false;
                    }

                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_RP)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = false;
                    }


                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_SR)
                    {
                        //						if (this.BoolIsMemberVested == true && (this.DecimalTerminationPIA < this.DecimalMaxPIAAmount)) 
                        //						{ 
                        //							l_Bool_Useit = true; 
                        //							l_Bool_Yside = true; 
                        //						} 
                        //						else 
                        //						{ 
                        //							l_Bool_Useit = false; 
                        //							l_Bool_Yside = false; 
                        //						} 
                        l_Bool_Useit = true;
                        l_Bool_Yside = true;

                    }
                    //Priya 20-Jan-2009 : YRS 5.0-637 AC Account interest components Added ElseIf condition for AC account type
                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_RetirementPlan_AC)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = false;

                    }//End 20-Jan-2009
                    //END - Retirement plan account groups
                    //START - Ssvings plan account groups
                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_SavingsPlan_TD)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = false;
                    }
                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_SavingsPlan_TM)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = true;
                    }
                    else if (l_CurrentRow["AccountGroup"].ToString().ToUpper() == m_const_SavingsPlan_RT)
                    {
                        l_Bool_Useit = true;
                        l_Bool_Yside = false;
                    }
                    //END - Savings plan account groups
                    if (!l_Bool_Useit)
                    {
                        l_CurrentRow.Delete();
                    }
                    if (!(l_Bool_Yside))
                    {
                        l_CurrentRow["YMCATaxable"] = 0;
                        l_CurrentRow["YMCAInterest"] = 0;
                        l_CurrentRow["Total"] = Convert.ToDouble(l_CurrentRow["Total"]) - Convert.ToDouble(l_CurrentRow["YMCATotal"]);
                        l_CurrentRow["YMCATotal"] = 0;
                    }
                    this.DecimalYmcaAvailableAmount = this.DecimalYmcaAvailableAmount + Convert.ToDecimal(l_CurrentRow["YMCATaxable"]);
                }
                l_RefundableDataTable.AcceptChanges();
                this.m_RefundableDataTable = l_RefundableDataTable;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("LoadRefundableDataTable Method", "Finish: Load Refundable DataTable.");
            }
            catch
            {
                throw;
            }
        }
        private void DoFilterRefundableTable(DataTable parameterDataTable)
        {
            string l_AccountType;
            try
            {
                if (parameterDataTable != null)
                {
                    foreach (DataRow l_DataRow in parameterDataTable.Rows)
                    {
                        if (l_DataRow["AccountType"].GetType().ToString().Trim() == "System.DBNull")
                        {
                            l_AccountType = string.Empty;
                        }
                        else
                        {
                            l_AccountType = Convert.ToString(l_DataRow["AccountType"]);
                        }

                        if (l_AccountType != string.Empty)
                        {
                            if (this.IsExistInRequestedAccounts(l_AccountType) == false)
                            {
                                l_DataRow.Delete();
                            }
                        }
                    }
                    parameterDataTable.AcceptChanges();
                }
                this.m_RefundableDataTable = parameterDataTable;
            }
            catch
            {
                throw;
            }
        }
        private void DoRegularRefundForCurrentAccounts()
        {
            DataTable l_DataTable;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoRegularRefundForCurrentAccounts Method", "Start: Do Regular Refund For Current Accounts.");
                l_DataTable = this.m_datatable_CalculatedDataTableForCurrentAccounts;
                if (l_DataTable != null)
                {
                    this.DoFullRefund(l_DataTable);
                    this.CalculateTotal(l_DataTable, 1);
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoRegularRefundForCurrentAccounts Method", "Finish: Do Regular Refund For Current Accounts.");
            }
            catch
            {
                throw;
            }
        }
        private void DoRegularRefundForRefundAccounts()
        {
            DataTable l_DataTable;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoRegularRefundForRefundAccounts Method", "Start: Do Regular Refund For Refund Accounts.");
                l_DataTable = this.m_datatable_CalculatedDataTableForRefundable;
                if ((l_DataTable != null))
                {
                    this.m_decimal_YmcaAvailableAmount = 0;
                    l_DataTable.RejectChanges();
                    this.DoFullRefund(l_DataTable);
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoRegularRefundForRefundAccounts Method", "Finish: Do Regular Refund For Refund Accounts.");
            }
            catch
            {
                throw;
            }
        }

        private void DoFullRefund(DataTable parameterDataTable)
        {
            DataTable l_ContributionDataTable;
            bool l_UserSide;
            bool l_YMCASide;
            string l_AccountGroup;
            try
            {
                if (parameterDataTable != null)
                {
                    l_ContributionDataTable = parameterDataTable;
                }
                else
                {
                    l_ContributionDataTable = this.m_datatable_CalculateContribution;
                }
                if (l_ContributionDataTable != null)
                {
                    foreach (DataRow l_DataRow in l_ContributionDataTable.Rows)
                    {
                        l_UserSide = true;
                        l_YMCASide = true;
                        if (((l_DataRow["AccountGroup"].GetType().ToString() != "System.DBNull")))
                        {
                            if ((((Convert.ToString(l_DataRow["AccountGroup"])).Trim() != "Total")))
                            {
                                l_UserSide = true;
                                l_YMCASide = true;
                                l_AccountGroup = (Convert.ToString(l_DataRow["AccountGroup"])).Trim().ToUpper();
                                if (this.IsBasicAccount(l_DataRow))
                                {
                                    l_UserSide = true;
                                    if (this.BoolIsMemberVested && (this.DecimalTerminationPIA < this.DecimalMaxPIAAmount))
                                    {
                                        l_YMCASide = true;
                                    }
                                    else
                                    {
                                        l_YMCASide = false;
                                    }

                                }

                                if (l_AccountGroup == m_const_RetirementPlan_AM)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = true;
                                }
                                else if (l_AccountGroup == m_const_RetirementPlan_AP)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = false;
                                }

                                else if (l_AccountGroup == m_const_RetirementPlan_RP)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = false;
                                }


                                else if (l_AccountGroup == m_const_RetirementPlan_SR)
                                {
                                    //									if (this.BoolIsMemberVested && (this.DecimalTerminationPIA < this.DecimalMaxPIAAmount))
                                    //									{ 
                                    //										l_UserSide = true; 
                                    //										l_YMCASide = true; 
                                    //									} 
                                    //									else 
                                    //									{ 
                                    //										l_UserSide = false; 
                                    //										l_YMCASide = false; 
                                    //									} 
                                    l_UserSide = true;
                                    l_YMCASide = true;
                                }
                                //Priya 20-Jan-2009 : YRS 5.0-637 AC Account interest components Added ElseIf condition for AC account type
                                else if (l_AccountGroup == m_const_RetirementPlan_AC)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = false;

                                }//End 20-Jan-2009
                                else if (l_AccountGroup == m_const_SavingsPlan_TD)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = false;
                                }
                                else if (l_AccountGroup == m_const_SavingsPlan_TM)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = true;
                                }
                                else if (l_AccountGroup == m_const_SavingsPlan_RT)
                                {
                                    l_UserSide = true;
                                    l_YMCASide = false;
                                }
                                if (l_UserSide)
                                {
                                    if (l_YMCASide == false)
                                    {
                                        l_DataRow["YMCATaxable"] = "0.00";
                                        l_DataRow["YMCAInterest"] = "0.00";
                                        l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Total"])) - (Convert.ToDecimal(l_DataRow["YMCATotal"]));
                                        l_DataRow["YMCATotal"] = "0.00";
                                    }
                                }
                                else
                                {
                                    l_DataRow.Delete();
                                }
                                //} 
                                //								else if (this.RefundType.ToUpper() == "PERS".ToUpper()) 
                                //								{ 
                                //									l_UserSide = true; 
                                //									l_YMCASide = false; 
                                //									if (l_PersonalOnly) 
                                //									{ 
                                //										l_YMCASide = false; 
                                //									} 
                                //									if (l_AccountGroup == "AP") 
                                //									{ 
                                //										l_UserSide = true; 
                                //									} 
                                //									else if (l_AccountGroup == "TD") 
                                //									{ 
                                //										l_UserSide = true; 
                                //									} 
                                //									else if (l_AccountGroup == "TM") 
                                //									{ 
                                //										l_UserSide = true; 
                                //									} 
                                //									else if (l_AccountGroup == "RP") 
                                //									{ 
                                //										l_UserSide = true; 
                                //									} 
                                //									else if (l_AccountGroup == "RT") 
                                //									{ 
                                //										l_UserSide = true; 
                                //									} 
                                //									else if (l_AccountGroup == "AM") 
                                //									{ 
                                //										l_UserSide = true; 
                                //									} 
                                //									else if (l_AccountGroup == "SR") 
                                //									{ 
                                //										l_UserSide = false; 
                                //									} 
                                //									if (l_UserSide) 
                                //									{ 
                                //										if (l_YMCASide == false) 
                                //										{ 
                                //											l_DataRow["YMCATaxable"] = "0.00"; 
                                //											l_DataRow["YMCAInterest"] = "0.00"; 
                                //											l_DataRow["Total"] = (Convert.ToDecimal(l_DataRow["Total"])) - (Convert.ToDecimal(l_DataRow["YMCATotal"])); 
                                //											l_DataRow["YMCATotal"] = "0.00"; 
                                //										} 
                                //									} 
                                //									else 

                                //										l_DataRow.Delete(); 
                                //									} 
                                //								} 
                                if (!(l_DataRow.RowState == DataRowState.Deleted))
                                {
                                    this.DecimalYmcaAvailableAmount += (Convert.ToDecimal(l_DataRow["YMCATaxable"]));
                                }
                            }
                        }
                    }
                    l_ContributionDataTable.AcceptChanges();
                    this.m_datatable_CalculateContribution = l_ContributionDataTable;
                }
            }
            catch
            {
                throw;
            }
        }
        private void MakeTablesToUpdate()
        {
            DataTable l_RefundsDataTable;
            DataTable l_TransactionsDataTable;
            DataTable l_DisbursementsDataTable;
            DataTable l_DisbursementDetailsDataTable;
            DataTable l_DisbursementWithholdingDataTable;
            DataTable l_DisbursementRefundsDataTable;
            decimal l_Decimal_TaxableCounter;
            decimal l_Decimal_NonTaxableCounter;
            DataRow[] l_FoundRow;
            string l_QueryString;
            decimal l_Decimal_TaxWithheldPrincipal;
            decimal l_Decimal_TaxWithheldInterest;
            decimal l_Decimal_TaxCounter;
            DataRow l_WithholdingDataRow;
            DataRow l_DisbursementRefundDataRow;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeTablesToUpdate Method", "Start: Make Tables ToUpdate before processing .");
                l_Decimal_NonTaxableCounter = 0;
                l_Decimal_TaxableCounter = 0;
                l_RefundsDataTable = this.m_datatable_AtsRefund;
                l_TransactionsDataTable = this.m_datatable_Refund_Transactions;
                l_DisbursementsDataTable = this.m_datatable_Refund_Disbursements;
                l_DisbursementDetailsDataTable = this.m_datatable_Refund_DisbursementDetails;
                l_DisbursementWithholdingDataTable = this.m_datatable_Refund_DisbursementWithholding;
                l_DisbursementRefundsDataTable = this.m_datatable_Refund__DisbursementRefunds;
                if (l_DisbursementsDataTable != null)
                {
                    if (l_DisbursementsDataTable.Rows.Count > 0)
                    {
                        l_Decimal_TaxableCounter = (Convert.ToDecimal(l_DisbursementsDataTable.Compute("SUM (TaxableAmount)", "")));
                        l_Decimal_NonTaxableCounter = (Convert.ToDecimal(l_DisbursementsDataTable.Compute("SUM (NonTaxableAmount)", "")));
                    }
                }
                l_Decimal_TaxCounter = 0;
                if (l_DisbursementDetailsDataTable != null)
                {
                    foreach (DataRow l_DataRow in l_DisbursementDetailsDataTable.Rows)
                    {
                        if (l_DataRow["TaxWithheldPrincipal"].GetType().ToString() == "System.DBNull")
                        {
                            l_Decimal_TaxWithheldPrincipal = 0;
                        }
                        else
                        {
                            l_Decimal_TaxWithheldPrincipal = Convert.ToDecimal(l_DataRow["TaxWithheldPrincipal"]);
                        }
                        if (l_DataRow["TaxWithheldInterest"].GetType().ToString() == "System.DBNull")
                        {
                            l_Decimal_TaxWithheldInterest = 0;
                        }
                        else
                        {
                            l_Decimal_TaxWithheldInterest = Convert.ToDecimal(l_DataRow["TaxWithheldInterest"]);
                        }

                        if (l_Decimal_TaxWithheldPrincipal > 0 || l_Decimal_TaxWithheldInterest > 0)
                        {
                            l_Decimal_TaxCounter = l_Decimal_TaxCounter + l_Decimal_TaxWithheldInterest + l_Decimal_TaxWithheldPrincipal;
                            l_QueryString = "DisbursementID = '" + (Convert.ToString(l_DataRow["DisbursementID"])).Trim() + "'";
                            l_FoundRow = l_DisbursementWithholdingDataTable.Select(l_QueryString);
                            l_WithholdingDataRow = null;
                            if (l_FoundRow != null)
                            {
                                if (l_FoundRow.Length > 0)
                                {
                                    l_WithholdingDataRow = l_FoundRow[0];
                                }
                            }
                            if (l_WithholdingDataRow == null)
                            {
                                l_WithholdingDataRow = l_DisbursementWithholdingDataTable.NewRow();
                                l_WithholdingDataRow["DisbursementID"] = l_DataRow["DisbursementID"];
                                l_WithholdingDataRow["WithholdingTypeCode"] = "FEDTAX";
                                l_WithholdingDataRow["Amount"] = l_Decimal_TaxWithheldInterest + l_Decimal_TaxWithheldPrincipal;
                                l_DisbursementWithholdingDataTable.Rows.Add(l_WithholdingDataRow);
                            }
                            else
                            {
                                l_WithholdingDataRow["Amount"] = (Convert.ToDecimal(l_WithholdingDataRow["Amount"])) + l_Decimal_TaxWithheldInterest + l_Decimal_TaxWithheldPrincipal;
                            }
                        }
                    }
                }
                if (l_RefundsDataTable != null)
                {
                    foreach (DataRow l_DataRow in l_RefundsDataTable.Rows)
                    {
                        l_QueryString = "DisbursementID = '" + (Convert.ToString(l_DataRow["DisbursementID"])) + "'";
                        l_FoundRow = l_DisbursementRefundsDataTable.Select(l_QueryString);
                        l_DisbursementRefundDataRow = null;
                        if (l_FoundRow != null)
                        {
                            if (l_FoundRow.Length > 0)
                            {
                                l_DisbursementRefundDataRow = l_FoundRow[0];
                            }
                        }
                        if (l_DisbursementRefundDataRow == null)
                        {
                            l_DisbursementRefundDataRow = l_DisbursementRefundsDataTable.NewRow();
                            l_DisbursementRefundDataRow["RefRequestID"] = l_DataRow["RefRequestsID"];
                            l_DisbursementRefundDataRow["DisbursementID"] = l_DataRow["DisbursementID"];
                            l_DisbursementRefundsDataTable.Rows.Add(l_DisbursementRefundDataRow);
                        }
                    }
                }
                DataTable l_RefundRequestDataTable;
                DataRow l_RefunRequestDataRow;
                //2014.01.07        Dinesh.k            BT-2032: YRS 5.0-2084: DLIN not converted to ININ by cash out process
                DataView dv_RefeundRequest = new DataView(this.m_datatable_RefundRequestTable);
                l_RefundRequestDataTable = this.m_datatable_RefundRequestTable;
                if (l_RefundRequestDataTable != null)
                {
                    l_QueryString = "UniqueID ='" + this.m_string_RefRequestId.Trim() + "'";
                    l_FoundRow = l_RefundRequestDataTable.Select(l_QueryString);
                    l_RefunRequestDataRow = null;
                    if (l_FoundRow != null)
                    {
                        if (l_FoundRow.Length > 0)
                        {
                            l_RefunRequestDataRow = l_FoundRow[0];
                        }
                    }
                    if (l_RefunRequestDataRow != null)
                    {
                        l_RefunRequestDataRow["Gross Amt."] = l_Decimal_TaxableCounter + l_Decimal_NonTaxableCounter;
                        l_RefunRequestDataRow["RequestStatus"] = "DISB";
                        l_RefunRequestDataRow["StatusDate"] = System.DateTime.Now;
                        l_RefunRequestDataRow["NonTaxable"] = l_Decimal_NonTaxableCounter;
                        l_RefunRequestDataRow["Taxable"] = l_Decimal_TaxableCounter;
                        l_RefunRequestDataRow["Tax"] = l_Decimal_TaxCounter;
                        if (l_Decimal_TaxCounter != 0 && l_Decimal_TaxableCounter != 0)
                        {
                            l_RefunRequestDataRow["TaxRate"] = Math.Round((l_Decimal_TaxCounter * 100) / (l_Decimal_TaxableCounter), 2);
                        }

                        else
                        {
                            l_RefunRequestDataRow["TaxRate"] = 0.00;
                        }
                        l_RefunRequestDataRow["Deductions"] = 0.00;

                    }
                    //2014.01.07        Dinesh.k            BT-2032: YRS 5.0-2084: DLIN not converted to ININ by cash out process
                    dv_RefeundRequest.RowFilter = "UniqueID ='" + this.m_string_RefRequestId.Trim() + "'";
                }
                this.m_datatable_Refund_DisbursementWithholding = l_DisbursementWithholdingDataTable;
                this.m_datatable_Refund__DisbursementRefunds = l_DisbursementRefundsDataTable;
                //2014.01.07        Dinesh.k            BT-2032: YRS 5.0-2084: DLIN not converted to ININ by cash out process
                //this.m_datatable_RefundRequestTable = l_RefundRequestDataTable;
                this.m_datatable_RefundRequestTable = dv_RefeundRequest.ToTable();
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeTablesToUpdate Method", "Finish: Make Tables ToUpdate before processing .");
            }
            catch
            {
                throw;
            }
        }

        //DataTable m_datatable_RMDdatatable;
        //public DataTable RMDdatatable
        //{
        //    get 
        //    {
        //        return m_datatable_Refund_Disbursements;
        //    }
        //    set
        //    {
        //        m_datatable_Refund_Disbursements = value;
        //    }
        //}

        private void SaveAllTable()
        {
            DataTable l_RefundsDataTable;
            DataTable l_TransactionsDataTable;
            DataTable l_DisbursementsDataTable;
            DataTable l_DisbursementDetailsDataTable;
            DataTable l_DisbursementWithholdingDataTable;
            DataTable l_DisbursementRefundsDataTable;
            DataTable l_RefundRequestDataTable;
            DataTable l_RMDDisbursementDataTable = null;

            DataSet l_DataSet;

            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SaveAllTable Method", "Start: Save All datatables .");
                l_DisbursementsDataTable = this.m_datatable_Refund_Disbursements;
                l_DisbursementDetailsDataTable = this.m_datatable_Refund_DisbursementDetails;
                l_TransactionsDataTable = this.m_datatable_Refund_Transactions;
                l_RefundsDataTable = this.m_datatable_AtsRefund;
                l_DisbursementWithholdingDataTable = this.m_datatable_Refund_DisbursementWithholding;
                l_DisbursementRefundsDataTable = this.m_datatable_Refund__DisbursementRefunds;
                l_RefundRequestDataTable = this.m_datatable_RefundRequestTable;
                l_DataSet = new DataSet("RefundTables");
                l_DataSet.Tables.Add(this.CopyTable(l_DisbursementsDataTable, "DisbursementsDataTable"));
                l_DataSet.Tables.Add(this.CopyTable(l_DisbursementDetailsDataTable, "DisbursementDetailsDataTable"));
                l_DataSet.Tables.Add(this.CopyTable(l_TransactionsDataTable, "TransactionsDataTable"));
                l_DataSet.Tables.Add(this.CopyTable(l_RefundsDataTable, "RefundsDataTable"));
                l_DataSet.Tables.Add(this.CopyTable(l_DisbursementWithholdingDataTable, "DisbursementWithholdingDataTable")); //we are not inserting any deductions Withholding as the user has no choice of any withholdong deductions.

                l_DataSet.Tables.Add(this.CopyTable(l_DisbursementRefundsDataTable, "DisbursementRefundsDataTable"));
                l_DataSet.Tables.Add(this.CopyTable(l_RefundRequestDataTable, "RefundRequestDataTable"));
                //l_BooleanFlag = YMCARET.YmcaBusinessObject.RefundRequest.SaveRefundRequestProcess(l_DataSet, this.StringPersonId, this.StringFundEventId, "CSHOUT", this.BoolIsMemberVested, this.BoolIsTerminated,false);
                //the last parameter is set to false which corresponds to the IsTookTDAccount,since here no hardship refund will happen
                //hence this paramter to set to false

                //Added By SG: 2012.08.30: BT-960
                //Code Commented By Dinesh kanojia on 10/09/2014 
                //Start: BT:2437: Cashout process not inserting records in AtsMrdRecordsDisbursements for RMD eligible participants.
                //if (m_StringCashOutRefundType == "SHIRA" && m_boolIsRMDeligible == true)
                //End: BT:2437: Cashout process not inserting records in AtsMrdRecordsDisbursements for RMD eligible participants.
                if (m_boolIsRMDeligible == true)
                {
                    l_RMDDisbursementDataTable = this.SetRefundRMDDisbursementDataRow();
                    if (l_RMDDisbursementDataTable != null)
                    {
                        l_DataSet.Tables.Add(this.CopyTable(l_RMDDisbursementDataTable, "RMDDisbursementDataTable"));
                    }
                }

                this.m_dataset_RefundRequestProcessing = l_DataSet;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("SaveAllTable Method", "Finish: Save All datatables .");
            }
            catch
            {
                throw;
            }
        }
        private DataTable CopyTable(DataTable parameterDataTable, string parameterDataTableName)
        {
            DataTable l_DataTable;
            try
            {
                if (parameterDataTable == null)
                {
                    return null;
                }
                l_DataTable = parameterDataTable.Clone();
                if (parameterDataTableName.Trim() != string.Empty)
                {
                    l_DataTable.TableName = parameterDataTableName;
                }
                foreach (DataRow l_DataRow in parameterDataTable.Rows)
                {
                    l_DataTable.ImportRow(l_DataRow);
                }
                return l_DataTable;
            }
            catch
            {
                throw;
            }
        }

        private void LoadRefundRequestDetails(string parameterPersonID, string parameterFundID, YMCARET.YmcaDataAccessObject.CashOutDAClass paraCashOutDA)
        {
            DataTable l_DataTable = null;
            DataColumn dtColumn = null;
            try
            { //Commented by Ashish 2010.05.07
                //l_DataTable = YMCARET.YmcaBusinessObject.RefundRequest.MemberRefundRequestDetails(parameterPersonID, parameterFundID); 
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("LoadRefundRequestDetails Method", "Start: LoadRefundRequestDetails.");
                l_DataTable = paraCashOutDA.LoadRefundRequestDetails(parameterPersonID, parameterFundID);
                if (l_DataTable != null)
                {
                    if (!l_DataTable.Columns.Contains("RolloverOptions"))
                    {
                        dtColumn = new DataColumn("RolloverOptions");
                        dtColumn.DefaultValue = System.DBNull.Value;
                        l_DataTable.Columns.Add(dtColumn);
                    }
                    if (!l_DataTable.Columns.Contains("FirstRolloverAmt"))
                    {
                        dtColumn = new DataColumn("FirstRolloverAmt");
                        dtColumn.DefaultValue = System.DBNull.Value;
                        l_DataTable.Columns.Add(dtColumn);
                    }
                    if (!l_DataTable.Columns.Contains("TotalRolloverAmt"))
                    {
                        dtColumn = new DataColumn("TotalRolloverAmt");
                        dtColumn.DefaultValue = System.DBNull.Value;
                        l_DataTable.Columns.Add(dtColumn);
                    }
                }
                this.m_datatable_RefundRequestTable = l_DataTable;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("LoadRefundRequestDetails Method", "Finish: LoadRefundRequestDetails.");
            }
            catch
            {
                throw;
            }
        }


        public bool IsBasicAccount(DataRow parameterDataRow)
        {
            try
            {
                if (parameterDataRow["IsBasicAccount"].GetType().ToString() == "System.DBNull")
                {
                    return false;
                }
                if (Convert.ToBoolean(parameterDataRow["IsBasicAccount"]))
                {
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

        //Added By SG: 2012.08.29: BT-960
        private DataTable SetRefundRMDDisbursementDataRow()
        {
            DataTable l_DataTable = null;
            DataTable l_DisbursementDatatable = null;
            DataTable l_RMDdatatable = null;
            DataRow l_RMDDataRow = null;
            DataRow[] l_Datarow = null;

            try
            {
                //Fetch AtsMrdRecordsDisbursements table schema
                l_DataTable = this.m_datatable_Refund_MrdRecordsDisbursements;

                //Fetch AtsMrdRecords records of participants
                //l_RMDdatatable = YMCARET.YmcaBusinessObject.RefundRequest.GetMRDRecords(this.m_string_FundEventId);
                l_RMDdatatable = this.m_RmdRecordsDataTable;

                //Fetch AtsDisbursements records of participants
                l_DisbursementDatatable = this.m_datatable_Refund_Disbursements;

                if (l_RMDdatatable != null)
                {
                    l_Datarow = l_RMDdatatable.Select("PlanType = '" + this.m_string_PlanType + "'");
                    if (l_Datarow != null && l_Datarow.Length > 0)
                    {
                        if (l_RMDDataRow == null)
                            l_RMDDataRow = l_DataTable.NewRow();

                        if (l_RMDDataRow != null)
                        {
                            if (l_DisbursementDatatable != null)
                            {
                                l_RMDDataRow["mnyTaxablePaidAmount"] = l_DisbursementDatatable.Rows[0]["TaxableAmount"];
                                l_RMDDataRow["mnyNonTaxablePaidAmount"] = l_DisbursementDatatable.Rows[0]["NonTaxableAmount"];
                                l_RMDDataRow["mnyPaidAmount"] = Convert.ToDecimal(l_DisbursementDatatable.Rows[0]["TaxableAmount"].ToString()) + Convert.ToDecimal(l_DisbursementDatatable.Rows[0]["NonTaxableAmount"].ToString());
                                l_RMDDataRow["guiDisbursementId"] = l_DisbursementDatatable.Rows[0]["UniqueID"];
                            }

                            l_RMDDataRow["intMrdTrackingId"] = l_Datarow[0]["intMrdRecordsId"];

                            l_DataTable.Rows.Add(l_RMDDataRow);
                        }
                    }
                }

                return l_DataTable;
            }
            catch
            {
                throw;
            }
            finally
            {
                l_DisbursementDatatable.Dispose();
                l_RMDdatatable.Dispose();
                l_RMDDataRow = null;
                l_Datarow = null;
            }
        }
    }
}
