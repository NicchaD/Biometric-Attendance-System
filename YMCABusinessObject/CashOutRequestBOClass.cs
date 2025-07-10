/****************************************************************************************/
//Modification History
/****************************************************************************************/
//Date              Modified by         Description
/****************************************************************************************/
//2012.06.06        Sanjeev(SG)         BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2014.12.09        Dinesh Kanojia      BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
//2015.05.05        Anudeep A           BT-2699:YRS 5.0-2441 : Modifications for 403b Loans
//2015.07.01        Sanjay S.           BT 2890/YRS 5.0-2523:Create script to populate tables for Release blanks
//2015.09.16        Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2015.10.12        Sanjay Singh        YRS-AT-2463 - Cashout utility for participants with two plans. One release blank rather than two per participant 
/****************************************************************************************/
using System;
using System.Web;
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
    /// Summary description for CashOutRequestBOClass.
    /// </summary>
    public class CashOutRequestBOClass
    {
        # region Declarations
        DataSet m_DataSet_RefundRequest = new DataSet();
        DataTable m_datatable_MemberEmployment;
        DataTable m_datatable_MemberAccountContribution;
        DataTable m_datatable_MemberAccountBreakdown;
        DataTable l_datatable_CalculateContribution;
        DataTable l_datatable_AtsRefunds;
        DataTable l_datatable_AtsRefRequests;
        DataTable l_datatable_atsRefRequestDetails;
        string m_string_FundEventId = "";
        bool l_bool_IsTerminated;
        decimal m_decimal_PIACurrent;
        decimal m_decimal_TotalAmount;
        decimal m_decimal_PersonTotalAmount;
        decimal m_decimal_NonTaxedAmount;
        decimal m_decimal_TaxedAmount;
        decimal m_decimal_NetAmount;
        decimal m_decimal_FederalTaxRate;
        decimal m_decimal_Tax;
        decimal m_decimal_TerminationPIA;
        decimal m_decimal_MaxPIAAmount;
        decimal m_decimal_YMCAAvailableAmount;
        decimal m_decimal_MinDistributionAmount;
        decimal m_decimal_PersonAge;
        decimal m_decimal_WithheldTax;
        int m_integer_RefundExpiryDate;
        bool l_bool_Vested;
        int m_integer_IntegerAddressId;
        decimal m_decimal_MinimumDistributedAge;
        decimal m_decimal_MinimumDistributedTaxRate;
        string m_string_PersonId;
        //plan split variables
        string m_string_PlanType;
        //START - Retirement plan account groups
        const string m_const_RetirementPlan_AP = "RAP";
        const string m_const_RetirementPlan_AM = "RAM";
        const string m_const_RetirementPlan_RG = "RRG";
        const string m_const_RetirementPlan_SA = "RSA";
        const string m_const_RetirementPlan_SS = "RSS";
        const string m_const_RetirementPlan_RP = "RRP";
        const string m_const_RetirementPlan_SR = "RSR";
        const string m_const_RetirementPlan_BA = "RBA"; //SR:2015.07.01 - YRS 5.0-2523
        //END - Retirement plan account groups
        //START - Ssvings plan account groups
        const string m_const_SavingsPlan_TD = "STD";
        const string m_const_SavingsPlan_TM = "STM";
        const string m_const_SavingsPlan_RT = "SRT";
        //Added By SG: 2012.06.06: BT-960
        string m_string_RefundType;
        //END - Savings plan account groups
        //plan split variables
        # endregion
        public CashOutRequestBOClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        # region Properties
        public DataSet DataSetRefundRequest
        {
            get
            {
                return m_DataSet_RefundRequest;
            }
            set
            {
                m_DataSet_RefundRequest = value;
            }
        }

        public DataTable DataTableAtsRefunds
        {
            get
            {
                return l_datatable_AtsRefunds;
            }
            set
            {
                l_datatable_AtsRefunds = value;
            }
        }
        public DataTable DataTableAtsRefRequests
        {
            get
            {
                return l_datatable_AtsRefRequests;
            }
            set
            {
                l_datatable_AtsRefRequests = value;
            }
        }
        public DataTable DataTableatsRefRequestDetails
        {
            get
            {
                return l_datatable_atsRefRequestDetails;
            }
            set
            {
                l_datatable_atsRefRequestDetails = value;
            }
        }
        public DataTable DataTableMemberEmployment
        {
            get
            {
                return m_datatable_MemberEmployment;
            }
            set
            {
                m_datatable_MemberEmployment = value;
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
        public DataTable DataTableMemberAccountContribution
        {
            get
            {
                return m_datatable_MemberAccountContribution;
            }
            set
            {
                m_datatable_MemberAccountContribution = value;
            }
        }
        public DataTable DataTableCalculateContribution
        {
            get
            {
                return l_datatable_CalculateContribution;
            }
            set
            {
                l_datatable_CalculateContribution = value;
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

        public decimal DecimalTotalAmount
        {
            get
            {
                return m_decimal_TotalAmount;
            }
            set
            {
                m_decimal_TotalAmount = value;
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
        public decimal DecimalMinDistributionAmount
        {
            get
            {
                return m_decimal_MinDistributionAmount;
            }
            set
            {
                m_decimal_MinDistributionAmount = value;
            }
        }
        public decimal DecimalPersonTotalAmount
        {
            get
            {
                return m_decimal_PersonTotalAmount;
            }
            set
            {
                m_decimal_PersonTotalAmount = value;
            }
        }
        public decimal DecimalNonTaxedAmount
        {
            get
            {
                return m_decimal_NonTaxedAmount;
            }
            set
            {
                m_decimal_NonTaxedAmount = value;
            }
        }
        public decimal DecimalYMCAAvailableAmount
        {
            get
            {
                return m_decimal_YMCAAvailableAmount;
            }
            set
            {
                m_decimal_YMCAAvailableAmount = value;
            }
        }
        public decimal DecimalTaxedAmount
        {
            get
            {
                return m_decimal_TaxedAmount;
            }
            set
            {
                m_decimal_TaxedAmount = value;
            }
        }
        public decimal DecimalNetAmount
        {
            get
            {
                return m_decimal_NetAmount;
            }
            set
            {
                m_decimal_NetAmount = value;
            }
        }
        public decimal DecimalFederalTaxRate
        {
            get
            {
                return m_decimal_FederalTaxRate;
            }
            set
            {
                m_decimal_FederalTaxRate = value;
            }
        }
        public decimal DecimalTax
        {
            get
            {
                return m_decimal_Tax;
            }
            set
            {
                m_decimal_Tax = value;
            }
        }
        public decimal DecimalWithheldTax
        {
            get
            {
                return m_decimal_WithheldTax;
            }
            set
            {
                m_decimal_WithheldTax = value;
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
        public bool BoolIsTerminated
        {
            get
            {
                return l_bool_IsTerminated;
            }
            set
            {
                l_bool_IsTerminated = value;
            }
        }
        public bool IsVested
        {
            get
            {
                return l_bool_Vested;
            }
            set
            {
                l_bool_Vested = value;
            }
        }
        public int RefundExpiryDate
        {
            get
            {
                return m_integer_RefundExpiryDate;
            }
            set
            {
                m_integer_RefundExpiryDate = value;
            }
        }
        public int IntegerAddressId
        {
            get
            {
                return m_integer_IntegerAddressId;
            }
            set
            {
                m_integer_IntegerAddressId = value;
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
        public decimal DecimalMinimumDistributedAge
        {
            get
            {
                return m_decimal_MinimumDistributedAge;
            }
            set
            {
                m_decimal_MinimumDistributedAge = value;
            }
        }
        public decimal DecimalMinimumDistributedTaxRate
        {
            get
            {
                return m_decimal_MinimumDistributedTaxRate;
            }
            set
            {
                m_decimal_MinimumDistributedTaxRate = value;
            }
        }
        //plan split changes
        public string StringPlanType
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
        //plan split changes

        //Added By SG: 2012.06.06: BT-960
        public string StringRefundType
        {
            get { return m_string_RefundType; }
            set { m_string_RefundType = value; }
        }

        //SR:2015.07.01 - YRS 5.0-2523:Create script to populate tables for Release blanks
        public string m_string_CashoutType;
        public string strCashoutType
        {
            get
            {
                return m_string_CashoutType;
            }
            set
            {
                m_string_CashoutType = value;
            }
        }

        private decimal m_BAMaxLimit;
        public decimal BAMaxLimit
        {
            get
            {
                if (m_BAMaxLimit != System.Decimal.Zero)
                {
                    return ((decimal)m_BAMaxLimit);
                }
                else
                {
                    return System.Decimal.Zero;
                }
            }
            set { m_BAMaxLimit = value; }
        }
       //SR:2015.07.01 - YRS 5.0-2523:Create script to populate tables for Release blanks
        # endregion

        # region Methods
        public void DataTableEmploymentDetails()
        {
            DataTable l_datatable_EmploymentDetails = null;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DataTableEmploymentDetails Method", "Start: Getting Employment Details.");
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_datatable_EmploymentDetails = objCashOutDAClass.GetMemberEmploymentDetails(this.StringPersonId, this.StringFundEventId);
                this.DataTableMemberEmployment = l_datatable_EmploymentDetails;
                objCashOutDAClass = null;
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DataTableEmploymentDetails Method", "Finish: Getting Employment Details.");

                //this.DataTableMemberEmployment=YMCARET.YmcaDataAccessObject.CashOutDAClass.GetMemberEmploymentDetails(this.StringPersonId);
            }
            catch
            {
                throw;
            }
        }
        public void DataTableAccountContribution()
        {
            DataSet l_dataset_AccountContribution = new DataSet();
            DataTable l_DataTable_AccountContributionPlanWise = null;
            DataRow[] drow_AccountContributionPlanWise;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DataTableAccountContribution Method", "Start: Get Account Contribution details.");

                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DataTableAccountContribution Method", "Start: Get LookUp Transaction for refunds.");
                l_dataset_AccountContribution = RefundRequest.LookupMemberAccounts(this.StringFundEventId, false); //AA:05.05.2015 BT-2699 - YRS 5.0-2411: Added for not to display the loan accounts for computations
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DataTableAccountContribution Method", "Finish: Get LookUp Transaction for refunds.");
                //this.DataTableMemberAccountContribution = l_dataset_AccountContribution.Tables["AccountForPaid"];
                //plan split changes
                if (l_dataset_AccountContribution != null)
                {
                    l_DataTable_AccountContributionPlanWise = l_dataset_AccountContribution.Tables["AccountForPaid"].Clone();

                    //Start:SR:2015.07.01 - YRS 5.0-2523: calaculate Account contribution for both plan
                    if (this.strCashoutType == "special" || this.strCashoutType == "cashout_50-5k" || this.strCashoutType == "cashout_0-50") //SR|2015.10.09| YRS-AT-2463 - Added cashout type as "CashOut_50-5k" & "cashout_0-50" to create one Refund request for both(Retirement & Savings) the plans
                    {
                        if (this.m_string_PlanType == "BOTH")
                        {
                            drow_AccountContributionPlanWise = l_dataset_AccountContribution.Tables["AccountForPaid"].Select("PlanType IN ('RETIREMENT','SAVINGS')");
                        }
                        else
                        {
                            drow_AccountContributionPlanWise = l_dataset_AccountContribution.Tables["AccountForPaid"].Select("PlanType = '" + this.StringPlanType + "'");
                        }
                    }
                    else {
                            drow_AccountContributionPlanWise = l_dataset_AccountContribution.Tables["AccountForPaid"].Select("PlanType = '" + this.StringPlanType + "'");
                    
                    }
                    //End:SR:2015.07.01 - YRS 5.0-2523: calaculate Account contribution for both plan

                    if (drow_AccountContributionPlanWise.Length > 0)
                    {
                        for (int i = 0; i <= drow_AccountContributionPlanWise.Length - 1; i++)
                        {
                            l_DataTable_AccountContributionPlanWise.ImportRow(drow_AccountContributionPlanWise[i]);
                        }
                        l_DataTable_AccountContributionPlanWise.AcceptChanges();
                        this.DataTableMemberAccountContribution = l_DataTable_AccountContributionPlanWise;
                    }
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DataTableAccountContribution Method", "Start: Get Account Contribution details.");
                //plan split changes
            }
            catch
            {
                throw;
            }
        }
        public bool CheckForDistributionDate()
        {
            DataTable l_datatable_MemberEmployment = null;
            //DataRow l_DataRow; 
            //DateTime l_DateTime; 
            DateTime l_datetime_TerminationDate = new DateTime();


            bool l_flag = true;
            try
            {
                l_datatable_MemberEmployment = this.m_datatable_MemberEmployment;
                if (l_datatable_MemberEmployment == null)
                {
                    l_flag = false;
                }
                if (l_flag == true)
                {
                    foreach (DataRow l_DataRow in l_datatable_MemberEmployment.Rows)
                    {
                        if (l_DataRow["TermDate"].GetType().ToString() != "System.DBNull")
                        {
                            if (DateTime.Compare(l_datetime_TerminationDate, Convert.ToDateTime(l_DataRow["TermDate"])) < 0)
                            {
                                l_datetime_TerminationDate = (Convert.ToDateTime(l_DataRow["TermDate"]));
                            }
                        }
                    }
                }

                //if (System.DateTime.Now.Year >= l_datetime_TerminationDate.Year && System.DateTime.Now.Month > 3 && System.DateTime.Now.Day > 31)
                if (System.DateTime.Now.Year >= l_datetime_TerminationDate.Year)
                {
                    l_flag = true;
                }
                else
                {
                    l_flag = false;
                }

                return l_flag;
            }
            catch
            {
                throw;
            }
        }
        private void CopyAccountContributionTable()
        {
            DataTable l_datatable_AccountContribution;
            DataTable l_datatable_CalculationDataTable;
            DataColumn l_datacolumn_selected;
            //DataRow l_DataRow; 
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopyAccountContributionTable Method", "Start: Get Account Contribution details.");
                l_datatable_AccountContribution = this.DataTableMemberAccountContribution;
                if (l_datatable_AccountContribution != null)
                {
                    l_datatable_CalculationDataTable = l_datatable_AccountContribution.Clone();
                    l_datacolumn_selected = new DataColumn("Selected");//used for voluntary refund types
                    l_datacolumn_selected.DataType = System.Type.GetType("System.Boolean");
                    l_datacolumn_selected.AllowDBNull = true;
                    l_datatable_CalculationDataTable.Columns.Add(l_datacolumn_selected);
                    foreach (DataRow l_DataRow in l_datatable_AccountContribution.Rows)
                    {
                        if ((l_DataRow["AccountType"].GetType().ToString()) != "System.DBNull")
                        {
                            if (Convert.ToString(l_DataRow["AccountType"]) != "Total")
                            {
                                l_datatable_CalculationDataTable.ImportRow(l_DataRow);
                            }
                        }
                    }
                    this.DataTableCalculateContribution = l_datatable_CalculationDataTable;
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CopyAccountContributionTable Method", "Finish: Get Account Contribution details.");
            }
            catch
            {
                throw;
            }
        }

        private void CalculateTotal()
        {
            DataTable l_datatable_Calculated;
            DataRow l_DataRow = null;
            //l_DataRow=new DataRow();
            bool l_bool_Flag = false;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateTotal Method", "Start: Calculating Total.");
                l_datatable_Calculated = this.DataTableCalculateContribution;
                if (l_datatable_Calculated != null)
                {
                    foreach (DataRow l_CalculatedDataRow in l_datatable_Calculated.Rows)
                    {
                        if (l_CalculatedDataRow["AccountType"].GetType().ToString() != "System.DBNull")
                        {
                            if (Convert.ToString(l_CalculatedDataRow["AccountType"]) == "Total")
                            {
                                l_bool_Flag = true;
                                l_DataRow = l_CalculatedDataRow;
                                break;
                            }
                        }
                    }

                    if (l_bool_Flag == false)
                    {
                        l_DataRow = l_datatable_Calculated.NewRow();
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

                    l_DataRow["Taxable"] = l_datatable_Calculated.Compute("SUM (Taxable)", "");
                    l_DataRow["Non-Taxable"] = l_datatable_Calculated.Compute("SUM ([Non-Taxable])", "");
                    l_DataRow["Interest"] = l_datatable_Calculated.Compute("SUM (Interest)", "");
                    l_DataRow["Emp.Total"] = l_datatable_Calculated.Compute("SUM ([Emp.Total])", "");
                    l_DataRow["YMCATaxable"] = l_datatable_Calculated.Compute("SUM (YMCATaxable)", "");
                    l_DataRow["YMCAInterest"] = l_datatable_Calculated.Compute("SUM (YMCAInterest)", "");
                    l_DataRow["YMCATotal"] = l_datatable_Calculated.Compute("SUM (YMCATotal)", "");
                    l_DataRow["Total"] = l_datatable_Calculated.Compute("SUM (Total)", "");

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
                    if (l_DataRow["Total"].GetType().ToString() == "System.DBNull")
                    {
                        this.m_decimal_TotalAmount = 0;
                    }
                    else
                    {
                        this.m_decimal_TotalAmount = Convert.ToDecimal(l_DataRow["Total"]);
                    }
                    if (l_DataRow["Emp.Total"].GetType().ToString() == "System.DBNull")
                    {
                        this.m_decimal_PersonTotalAmount = 0;
                    }
                    else
                    {
                        this.m_decimal_PersonTotalAmount = Convert.ToDecimal(l_DataRow["Emp.Total"]);
                    }
                    if (l_bool_Flag == false)
                    {
                        l_DataRow["AccountType"] = "Total";
                        l_datatable_Calculated.Rows.Add(l_DataRow);
                    }
                    this.CalculateTaxes(l_DataRow);
                    this.DataTableCalculateContribution = l_datatable_Calculated;
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateTotal Method", "Finish: Calculating Total.");
            }
            catch
            {
                throw;
            }
        }
        private void CalculateTaxes(DataRow parameterDataRow)
        {

            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateTaxes Method", "Start: Calculating Taxes.");
                if (parameterDataRow != null)
                {
                    //this.m_decimal_FederalTaxRate = 20; we have aDDed FED tax rate to AtsMetaConfiguration

                    if (parameterDataRow["Non-Taxable"].GetType().ToString() == "System.DBNull")
                    {
                        this.DecimalNonTaxedAmount = 0;
                    }
                    else
                    {
                        this.DecimalNonTaxedAmount = Convert.ToDecimal(parameterDataRow["Non-Taxable"]);
                    }

                    if (parameterDataRow["Total"].GetType().ToString() == "System.DBNull")
                    {
                        this.DecimalTaxedAmount = 0;
                    }

                    else
                    {
                        this.DecimalTaxedAmount = Convert.ToDecimal(parameterDataRow["Total"]);
                    }

                    this.m_decimal_TaxedAmount = this.m_decimal_TaxedAmount - this.m_decimal_NonTaxedAmount;
                    this.m_decimal_Tax = this.m_decimal_TaxedAmount * (this.m_decimal_FederalTaxRate / 100);
                    this.DecimalNetAmount = this.m_decimal_NonTaxedAmount + this.m_decimal_TaxedAmount - (this.m_decimal_TaxedAmount * (this.m_decimal_FederalTaxRate / 100));
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateTaxes Method", "Finish: Calculating Taxes.");
            }
            catch
            {
                throw;
            }
        }
        //the method will now compare the account groups rather than the account types
        private void DoFullRefund()
        {
            DataTable l_datatable_Contribution;
            //DataRow l_datarow_current; 
            bool l_bool_UserSide;
            bool l_bool_YMCASide;
            string l_string_AccountGroup;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoFullRefund Method", "Start: Calculating contribution.");
                l_datatable_Contribution = this.l_datatable_CalculateContribution;
                if (l_datatable_Contribution != null)
                {
                    foreach (DataRow l_datarow_current in l_datatable_Contribution.Rows)
                    {
                        l_bool_UserSide = true;
                        l_bool_YMCASide = true;
                        if ((l_datarow_current["AccountGroup"].GetType().ToString() != "System.DBNull"))
                        {
                            if (Convert.ToString(l_datarow_current["AccountGroup"]).Trim() != "Total")
                            {
                                l_string_AccountGroup = Convert.ToString(l_datarow_current["AccountGroup"]).Trim().ToUpper();
                                if (this.IsBasicAccount(l_datarow_current))
                                {
                                    l_bool_UserSide = true;
                                    if (this.IsVested && this.DecimalTerminationPIA < this.DecimalMaxPIAAmount)
                                    {
                                        l_bool_YMCASide = true;
                                    }
                                    else
                                    {
                                        l_bool_YMCASide = false;
                                    }

                                    //Start:SR:2015.07.01 - YRS 5.0-2523: calaculate BA Acoount money
                                    if (l_string_AccountGroup == m_const_RetirementPlan_BA)
                                    {                                    
                                        if (this.IsVested && (decimal)l_datarow_current["YMCATotal"] <= this.BAMaxLimit)
                                        {
                                            l_bool_YMCASide = true;
                                        }
                                        else
                                        {
                                            l_bool_YMCASide = false;
                                        }
                                    }
                                    //End:SR:2015.07.01 - YRS 5.0-2523: calaculate BA Acoount money
                                }

                                //start - Retirement Plan Group
                                if (l_string_AccountGroup == m_const_RetirementPlan_AM)
                                {
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = true;
                                }

                                else if (l_string_AccountGroup == m_const_RetirementPlan_AP)
                                {
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = false;
                                }

                                else if (l_string_AccountGroup == m_const_RetirementPlan_RP)
                                {
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = false;
                                }


                                else if (l_string_AccountGroup == m_const_RetirementPlan_SR)
                                {
                                    //									if (this.IsVested && this.DecimalTerminationPIA < this.DecimalMaxPIAAmount) 
                                    //									{ 
                                    //										l_bool_UserSide = true; 
                                    //										l_bool_YMCASide = true; 
                                    //									} 
                                    //									else 
                                    //									{ 
                                    //										l_bool_UserSide = false; 
                                    //										l_bool_YMCASide = false; 
                                    //									} 
                                    //In cash out an active person will never be allowed hence SR will be treated similarlt to SR
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = true;
                                }
                                //end - Retirement Plan Group
                                //start - Savings Plan Group		
                                else if (l_string_AccountGroup == m_const_SavingsPlan_TD)
                                {
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = false;
                                }
                                else if (l_string_AccountGroup == m_const_SavingsPlan_TM)
                                {
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = true;
                                }
                                else if (l_string_AccountGroup == m_const_SavingsPlan_RT)
                                {
                                    l_bool_UserSide = true;
                                    l_bool_YMCASide = false;
                                }
                                //end - Savings Plan Group
                                if (l_bool_UserSide)
                                {
                                    if (l_bool_YMCASide == false)
                                    {
                                        l_datarow_current["YMCATaxable"] = "0.00";
                                        l_datarow_current["YMCAInterest"] = "0.00";
                                        l_datarow_current["Total"] = Convert.ToDecimal(l_datarow_current["Total"]) - Convert.ToDecimal(l_datarow_current["YMCATotal"]);
                                        l_datarow_current["YMCATotal"] = "0.00";
                                    }
                                    l_datarow_current["Selected"] = "True";
                                    this.DecimalYMCAAvailableAmount += Convert.ToDecimal(l_datarow_current["YMCATaxable"]);
                                }
                                else
                                {
                                    l_datarow_current.Delete();
                                }
                            }
                        }
                    }
                    l_datatable_Contribution.AcceptChanges();
                    this.DataTableCalculateContribution = l_datatable_Contribution;
                    this.CalculateTotal();
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DoFullRefund Method", "Finish: Calculating contribution.");
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

                return Convert.ToBoolean(parameterDataRow["IsBasicAccount"]);
            }
            catch
            {
                throw;
            }
        }

        public void DecimalPIAAmount(string parameterFundEventID, bool parameterIsTerminated)
        {
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DecimalPIAAmount Method", "Start: Get Current PIA Amount.");
                if (parameterIsTerminated == false)
                {
                    this.m_decimal_PIACurrent = YMCARET.YmcaBusinessObject.RefundRequest.GetCurrentPIA(parameterFundEventID);
                    this.m_decimal_TerminationPIA = this.m_decimal_PIACurrent;
                }
                else
                {
                    this.m_decimal_TerminationPIA = YMCARET.YmcaBusinessObject.RefundRequest.GetTerminatePIA(parameterFundEventID);
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("DecimalPIAAmount Method", "Finish: Get Current PIA Amount.");
            }
            catch
            {
                throw;
            }
        }

        //public 
        private int GetAccountBreakDownSortOrder(string paramterAccountType)
        {
            DataTable l_DataTable;
            //DataRow l_DataRow; 
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

        private DataRow GetDataRow(string parameterAccountBreakDownType, string parameterAcctType, DataTable parameterDataTable)
        {
            string l_QueryString;
            DataRow[] l_FoundRow;
            try
            {
                if (parameterDataTable != null)
                {
                    l_QueryString = "AcctBreakDownType = '" + parameterAccountBreakDownType.Trim().ToUpper() + "' AND AcctType = '" + parameterAcctType + "'";
                    l_FoundRow = parameterDataTable.Select(l_QueryString);
                    if (l_FoundRow.Length > 0)
                    {
                        return l_FoundRow[0];
                    }
                    else
                    {
                        return null;
                    }
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
        private DataRow GetDataRow(string parameterAccountBreakDownType, int parameterSortOrder, DataTable parameterDataTable, string parameterAcctType)
        {
            string l_QueryString;
            DataRow[] l_FoundRow;
            try
            {
                if (parameterDataTable != null)
                {
                    l_QueryString = "AcctBreakDownType = '" + parameterAccountBreakDownType.Trim().ToUpper() + "' AND SortOrder = " + parameterSortOrder + " AND AcctType = '" + parameterAcctType + "'";
                    l_FoundRow = parameterDataTable.Select(l_QueryString);
                    if (l_FoundRow.Length > 0)
                    {
                        return l_FoundRow[0];
                    }
                    else
                    {
                        return null;
                    }
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
        private bool UpdateRefundRequestTable()
        {
            DataTable l_RefundRequestDataTable;
            DataTable l_RefundRequestDetailsDataTable;
            DataTable l_ContributionDataTable;
            DataSet l_RefundDataSet;
            DataRow l_DataRow;
            //			DataRow l_ContributionDataRow; 
            decimal l_decimal_Tax;
            bool l_flag = true;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Start: Updating refund request table.");

                l_ContributionDataTable = this.l_datatable_CalculateContribution;
                if (l_ContributionDataTable == null)
                {
                    l_flag = false;
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Finish: Populating AtsRefRequests datatable.");
                l_RefundRequestDataTable = this.DataTableAtsRefRequests.Clone();
                if (l_RefundRequestDataTable == null)
                {
                    l_flag = false;
                }

                if (l_flag == false)
                {
                    return l_flag;
                }


                l_RefundRequestDataTable.Columns.Add("PlanType", System.Type.GetType("System.String"));
                l_DataRow = l_RefundRequestDataTable.NewRow();
                //l_DataRow("UniqueID") = this.m_string_PersonId; 
                l_DataRow["PersID"] = this.m_string_PersonId;
                l_DataRow["FundEventID"] = this.m_string_FundEventId;

                //Added By SG: 2012.06.06: BT-960
                //l_DataRow["RefundType"] = "CASH"; //TODO Refund Type as CASHOUT 
                l_DataRow["RefundType"] = StringRefundType;

                l_DataRow["RequestStatus"] = "PEND";
                l_DataRow["StatusDate"] = System.DateTime.Now;
                l_DataRow["RequestDate"] = System.DateTime.Now;
                l_DataRow["ReleaseBlankType"] = "???";
                l_DataRow["Amount"] = this.m_decimal_TotalAmount;
                l_DataRow["ExpireDate"] = System.DateTime.Now.Date.AddDays(this.m_integer_RefundExpiryDate);
                l_DataRow["TaxRate"] = this.m_decimal_FederalTaxRate;
                l_DataRow["Taxable"] = this.m_decimal_TaxedAmount;
                l_DataRow["NonTaxable"] = this.m_decimal_NonTaxedAmount;
                l_DataRow["HardShipAmt"] = "0.00";
                l_DataRow["Deductions"] = "0.00";
                l_decimal_Tax = this.m_decimal_Tax;
                if (this.m_decimal_MinDistributionAmount > 0)
                {
                    l_decimal_Tax = l_decimal_Tax - (this.m_decimal_MinDistributionAmount * (this.m_decimal_MinimumDistributedTaxRate / 100));
                }
                l_DataRow["Tax"] = l_decimal_Tax;
                //the tax amount is set in a property to be able to be accessed as we are to put in it Log table for
                //reporting purpose
                this.m_decimal_WithheldTax = l_decimal_Tax;
                l_DataRow["AddressID"] = this.m_integer_IntegerAddressId;
                l_DataRow["MinDistribution"] = this.m_decimal_MinDistributionAmount;
                l_DataRow["ReleaseSentDate"] = System.DateTime.Now;
                //plan split changes
                //for Savings plan TerminationPIA will be 0
                if (this.m_string_PlanType.Trim().ToUpper() == "SAVINGS")
                {
                    this.m_decimal_TerminationPIA = 0;
                }
                l_DataRow["PIA"] = this.m_decimal_TerminationPIA;
                //Added the column Plantype --Amit Nigam 25/11/2009
                l_DataRow["PlanType"] = this.m_string_PlanType.Trim().ToUpper();
                //Added the column Plantype --Amit Nigam 25/11/2009


                //plan split changes

                l_RefundRequestDataTable.Rows.Add(l_DataRow);
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Finish: Populating AtsRefRequests datatable.");
                //request details
                string l_String_AccountType;
                string l_String_AccountGroup = "";
                string l_String_AccountBreakDownType;
                decimal l_Decimal_PersonalInterest;
                decimal l_Decimal_YmcaInterest;
                decimal l_Decimal_AccountTotal;
                decimal l_Decimal_PersonalPostTax;
                decimal l_Decimal_PersonalPreTax;
                decimal l_Decimal_PersonalTotal;
                decimal l_Decimal_YMCAPreTax;
                decimal l_Decimal_YMCATotal;
                decimal l_Decimal_GrandTotal;
                decimal l_Decimal_Total;
                decimal l_Decimal_PreTax;
                decimal l_Decimal_PostTax;
                decimal l_Decimal_Interst;
                decimal l_Decimal_TDAmount = 0;
                decimal l_Decimal_OtherAmount = 0;
                int l_Integer_SortOrder;
                l_RefundRequestDetailsDataTable = this.l_datatable_atsRefRequestDetails.Clone();
                foreach (DataRow l_ContributionDataRow in l_ContributionDataTable.Rows)
                { //1
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Start: Loop populate atsRefrequestDetails.");
                    if (l_ContributionDataRow.RowState != DataRowState.Deleted)
                    { //2

                        if (l_ContributionDataRow["AccountType"].GetType().ToString() != "System.DBNull")
                        { //3 
                            if (Convert.ToString(l_ContributionDataRow["AccountType"]) != "Total")
                            { //4 
                                l_Decimal_PreTax = 0;
                                l_Decimal_PostTax = 0;
                                l_Decimal_Interst = 0;
                                l_Decimal_PersonalPostTax = 0;
                                l_Decimal_PersonalPreTax = 0;
                                l_Decimal_PersonalInterest = 0;
                                l_Decimal_PersonalTotal = 0;
                                l_Decimal_YMCAPreTax = 0;
                                l_Decimal_YmcaInterest = 0;
                                l_Decimal_YMCATotal = 0;
                                l_Decimal_AccountTotal = 0;
                                l_Decimal_GrandTotal = 0;
                                l_String_AccountType = Convert.ToString(l_ContributionDataRow["AccountType"]).Trim().ToUpper();
                                l_String_AccountGroup = Convert.ToString(l_ContributionDataRow["AccountGroup"]).Trim().ToUpper();
                                l_Decimal_Total = Convert.ToDecimal(l_ContributionDataRow["Total"]);
                                if ((l_String_AccountGroup == m_const_SavingsPlan_TD) || (l_String_AccountGroup == m_const_SavingsPlan_TM))
                                {
                                    l_Decimal_TDAmount += l_Decimal_Total;
                                }
                                else
                                {
                                    l_Decimal_OtherAmount += l_Decimal_Total;
                                }

                                if (l_String_AccountGroup == m_const_RetirementPlan_AM && (l_Decimal_PersonalPostTax + l_Decimal_PersonalPreTax) == 0)
                                { //5
                                    l_String_AccountBreakDownType = "07";
                                    l_Integer_SortOrder = this.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType);

                                    l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType);
                                    //l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable);

                                    if (l_DataRow != null)
                                    { //6
                                        l_DataRow["YMCAPreTax"] = Convert.ToDecimal(l_ContributionDataRow["YMCATaxable"]) + Convert.ToDecimal(l_DataRow["YMCAPreTax"]);
                                        l_DataRow["YMCAInterest"] = Convert.ToDecimal(l_ContributionDataRow["YMCAInterest"]) + Convert.ToDecimal(l_DataRow["YMCAInterest"]);
                                        l_DataRow["YMCATotal"] = Convert.ToDecimal(l_ContributionDataRow["YMCAPreTax"]) + Convert.ToDecimal(l_ContributionDataRow["YMCAInterest"]) + Convert.ToDecimal(l_DataRow["YMCATotal"]);
                                        l_DataRow["AcctTotal"] = Convert.ToDecimal(l_DataRow["PersonalTotal"]) + Convert.ToDecimal(l_DataRow["YMCATotal"]);
                                        l_DataRow["GrandTotal"] = l_DataRow["AcctTotal"];
                                    } //6
                                    else
                                    {
                                        l_DataRow = l_RefundRequestDetailsDataTable.NewRow();
                                        l_Decimal_YMCAPreTax = Convert.ToDecimal(l_ContributionDataRow["YMCATaxable"]);
                                        l_Decimal_YmcaInterest = Convert.ToDecimal(l_ContributionDataRow["YMCAInterest"]);
                                        l_Decimal_YMCATotal = l_Decimal_YMCAPreTax + l_Decimal_YmcaInterest;
                                        l_Decimal_AccountTotal = l_Decimal_YMCATotal;
                                        l_Decimal_GrandTotal = l_Decimal_YMCATotal;
                                        l_DataRow["YMCAPreTax"] = l_Decimal_YMCAPreTax;
                                        l_DataRow["YMCAInterest"] = l_Decimal_YmcaInterest;
                                        l_DataRow["YMCATotal"] = l_Decimal_YMCATotal;
                                        l_DataRow["AcctTotal"] = l_Decimal_AccountTotal;
                                        l_DataRow["GrandTotal"] = l_Decimal_GrandTotal;
                                        l_DataRow["AcctType"] = l_String_AccountType;
                                        l_DataRow["PersonalPostTax"] = l_ContributionDataRow["Non-Taxable"];
                                        l_DataRow["PersonalPreTax"] = l_ContributionDataRow["Taxable"];
                                        l_DataRow["PersonalInterest"] = l_ContributionDataRow["Interest"];
                                        l_DataRow["PersonalTotal"] = l_ContributionDataRow["Emp.Total"];
                                        l_DataRow["AcctBreakDownType"] = l_String_AccountBreakDownType;
                                        l_DataRow["SortOrder"] = l_Integer_SortOrder;
                                        l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow);
                                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Finish: Loop populate atsRefrequestDetails.");
                                    }

                                    l_ContributionDataRow["YMCATaxable"] = "0.00";
                                    l_Decimal_YMCAPreTax = 0;
                                    l_Decimal_YmcaInterest = 0;

                                    //added by hafiz on 09-Jan-2007 for changes in Refunds
                                    l_Decimal_YMCATotal = 0;
                                    l_Decimal_PersonalTotal = 0;

                                    l_Decimal_AccountTotal = 0;
                                    l_Decimal_GrandTotal = 0;
                                }//5 
                                else
                                {
                                    if (l_ContributionDataRow["Emp.Total"].GetType().ToString() == "System.DBNull")
                                    {
                                        l_Decimal_PersonalTotal = 0;
                                        l_ContributionDataRow["Emp.Total"] = "0.00";
                                    }
                                    else
                                    {
                                        l_Decimal_PersonalTotal = Convert.ToDecimal(l_ContributionDataRow["Emp.Total"]);
                                    }
                                    if (l_ContributionDataRow["YMCATotal"].GetType().ToString() == "System.DBNull")
                                    {
                                        l_Decimal_YMCATotal = 0;
                                        l_ContributionDataRow["YMCATotal"] = "0.00";
                                    }
                                    else
                                    {
                                        l_Decimal_YMCATotal = Convert.ToDecimal(l_ContributionDataRow["YMCATotal"]);
                                    }
                                }

                                while ((l_Decimal_PersonalTotal + l_Decimal_YMCATotal) > 0)
                                { //7
                                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Start: While Loop to populate atsRefrequestDetails.");
                                    l_Decimal_Interst = 0;
                                    if (Convert.ToDecimal(l_ContributionDataRow["YMCATotal"]) > 0)
                                    { //8
                                        l_String_AccountBreakDownType = this.GetAccountBreakDownType(l_String_AccountGroup, false, true, false, false);
                                        l_Integer_SortOrder = this.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType);

                                        //commented & added by hafiz on 09-Jan-2007 for changes in Refunds
                                        l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType);
                                        //l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable);

                                        if (l_DataRow != null)
                                        { //9
                                            //commented & added by hafiz on 09-Jan-2007
                                            //l_DataRow["YMCAPreTax"] = Convert.ToDecimal(l_ContributionDataRow["YMCAPreTax"]) + Convert.ToDecimal(l_DataRow["YMCAPreTax"]); 
                                            l_DataRow["YMCAPreTax"] = Convert.ToDecimal(l_ContributionDataRow["YMCATaxable"]) + Convert.ToDecimal(l_DataRow["YMCAPreTax"]);

                                            l_DataRow["YMCAInterest"] = Convert.ToDecimal(l_ContributionDataRow["YMCAInterest"]) + Convert.ToDecimal(l_DataRow["YMCAInterest"]);
                                            l_DataRow["YMCATotal"] = Convert.ToDecimal(l_ContributionDataRow["YMCATotal"]) + Convert.ToDecimal(l_DataRow["YMCATotal"]);
                                            l_DataRow["AcctTotal"] = Convert.ToDecimal(l_DataRow["PersonalTotal"]) + Convert.ToDecimal(l_DataRow["YMCATotal"]);
                                            l_DataRow["GrandTotal"] = l_DataRow["AcctTotal"];
                                        } //9
                                        else
                                        {
                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow();
                                            l_Decimal_YMCAPreTax = Convert.ToDecimal(l_ContributionDataRow["YMCATaxable"]);
                                            l_Decimal_YmcaInterest = Convert.ToDecimal(l_ContributionDataRow["YMCAInterest"]);
                                            l_Decimal_YMCATotal = Convert.ToDecimal(l_ContributionDataRow["YMCATotal"]);
                                            l_Decimal_AccountTotal = l_Decimal_YMCATotal;
                                            l_Decimal_GrandTotal = l_Decimal_YMCATotal;
                                            l_DataRow["YMCAPreTax"] = l_Decimal_YMCAPreTax;
                                            l_DataRow["YMCAInterest"] = l_Decimal_YmcaInterest;
                                            l_DataRow["YMCATotal"] = l_Decimal_YMCATotal;
                                            l_DataRow["AcctTotal"] = l_Decimal_AccountTotal;
                                            l_DataRow["GrandTotal"] = l_Decimal_GrandTotal;
                                            l_DataRow["AcctType"] = l_String_AccountType;
                                            l_DataRow["PersonalPostTax"] = "0.00";
                                            l_DataRow["PersonalPreTax"] = "0.00";
                                            l_DataRow["PersonalInterest"] = "0.00";
                                            l_DataRow["PersonalTotal"] = "0.00";
                                            l_DataRow["AcctBreakDownType"] = l_String_AccountBreakDownType;
                                            l_DataRow["SortOrder"] = l_Integer_SortOrder;
                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow);
                                        }
                                        l_ContributionDataRow["YMCATaxable"] = "0.0";
                                        l_ContributionDataRow["YMCAInterest"] = "0.0";
                                        l_ContributionDataRow["YMCATotal"] = "0.0";
                                        l_Decimal_YMCAPreTax = 0;
                                        l_Decimal_YmcaInterest = 0;
                                        l_Decimal_YMCATotal = 0;
                                        l_Decimal_AccountTotal = 0;
                                        l_Decimal_GrandTotal = 0;
                                        l_Decimal_YMCATotal = 0;
                                    }//8 
                                    else if (Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]) > 0)
                                    { //10
                                        l_String_AccountBreakDownType = this.GetAccountBreakDownType(l_String_AccountGroup, true, false, false, true);
                                        l_Integer_SortOrder = this.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType);

                                        //commented & added by hafiz on 09-Jan-2007 for changes in Refunds
                                        l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType);
                                        //l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable);

                                        if (Convert.ToDecimal(l_ContributionDataRow["Taxable"]) > 0)
                                        {
                                            l_Decimal_PreTax = Convert.ToDecimal(l_ContributionDataRow["Taxable"]);
                                            l_Decimal_PostTax = Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]);
                                            l_Decimal_Interst = Convert.ToDecimal(l_ContributionDataRow["Interest"]);
                                        }
                                        else
                                        {
                                            l_Decimal_Interst = Convert.ToDecimal(l_ContributionDataRow["Interest"]);
                                        }

                                        if (l_DataRow != null)
                                        { //11
                                            l_DataRow["PersonalPreTax"] = l_ContributionDataRow["Taxable"];
                                            l_DataRow["PersonalPostTax"] = Convert.ToDecimal(l_DataRow["PersonalPostTax"]) + Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]);
                                            l_DataRow["PersonalInterest"] = Convert.ToDecimal(l_DataRow["PersonalInterest"]) + Convert.ToDecimal(l_ContributionDataRow["Interest"]);
                                            l_DataRow["PersonalTotal"] = Convert.ToDecimal(l_DataRow["PersonalTotal"]) + Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]) + l_Decimal_Interst;
                                            l_DataRow["AcctTotal"] = Convert.ToDecimal(l_DataRow["PersonalTotal"]) + Convert.ToDecimal(l_ContributionDataRow["Emp.Total"]);
                                            l_DataRow["GrandTotal"] = l_DataRow["AcctTotal"];
                                        } //11
                                        else
                                        { //12
                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow();
                                            l_Decimal_PostTax = Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]);
                                            l_Decimal_Total = l_Decimal_PostTax + l_Decimal_Interst;
                                            l_Decimal_AccountTotal = l_Decimal_Total;
                                            l_Decimal_GrandTotal = l_Decimal_AccountTotal;
                                            l_DataRow["PersonalPreTax"] = l_Decimal_PreTax;
                                            l_DataRow["PersonalPostTax"] = l_Decimal_PostTax;
                                            l_DataRow["PersonalInterest"] = l_Decimal_Interst;
                                            l_DataRow["PersonalTotal"] = l_Decimal_Total;
                                            l_DataRow["AcctTotal"] = l_Decimal_AccountTotal;
                                            l_DataRow["GrandTotal"] = l_Decimal_GrandTotal;
                                            l_DataRow["AcctType"] = l_String_AccountType;
                                            l_DataRow["YMCAPreTax"] = "0.00";
                                            l_DataRow["YMCAInterest"] = "0.00";
                                            l_DataRow["YMCATotal"] = "0.00";
                                            l_DataRow["AcctBreakDownType"] = l_String_AccountBreakDownType;
                                            l_DataRow["SortOrder"] = l_Integer_SortOrder;
                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow);
                                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Finish: While Loop to populate atsRefrequestDetails.");
                                        } //12
                                        l_ContributionDataRow["Taxable"] = "0.00";
                                        l_ContributionDataRow["Interest"] = Convert.ToDecimal(l_ContributionDataRow["Interest"]) - l_Decimal_Interst;
                                        l_ContributionDataRow["Emp.Total"] = l_Decimal_PersonalPreTax + l_Decimal_PersonalInterest;
                                        l_Decimal_PersonalTotal = 0;
                                    }//10 
                                    else
                                    { //13
                                        l_String_AccountBreakDownType = this.GetAccountBreakDownType(l_String_AccountGroup, true, false, true, false);
                                        l_Integer_SortOrder = this.GetAccountBreakDownSortOrder(l_String_AccountBreakDownType);

                                        //commented & added hafiz on 09-Jan-2007 for changes in Refunds
                                        l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_Integer_SortOrder, l_RefundRequestDetailsDataTable, l_String_AccountType);
                                        //l_DataRow = this.GetDataRow(l_String_AccountBreakDownType, l_String_AccountType, l_RefundRequestDetailsDataTable);

                                        if (Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]) > 0)
                                        {
                                            l_Decimal_PreTax = Convert.ToDecimal(l_ContributionDataRow["Taxable"]);
                                            l_Decimal_PostTax = Convert.ToDecimal(l_ContributionDataRow["Non-Taxable"]);
                                            l_Decimal_Interst = Convert.ToDecimal(l_ContributionDataRow["Interest"]);
                                            l_Decimal_Interst = l_Decimal_Interst * (l_Decimal_PreTax / (Convert.ToDecimal(l_ContributionDataRow["Emp.Total"]) - Convert.ToDecimal(l_ContributionDataRow["Interest"])));
                                        }
                                        else
                                        {
                                            l_Decimal_Interst = Convert.ToDecimal(l_ContributionDataRow["Interest"]);
                                        }
                                        if (l_DataRow != null)
                                        { //14
                                            l_DataRow["PersonalPreTax"] = Convert.ToDecimal(l_DataRow["PersonalPreTax"]) + Convert.ToDecimal(l_ContributionDataRow["Taxable"]);
                                            l_DataRow["PersonalInterest"] = Convert.ToDecimal(l_DataRow["PersonalInterest"]) + l_Decimal_Interst;
                                            l_DataRow["PersonalTotal"] = Convert.ToDecimal(l_DataRow["PersonalTotal"]) + Convert.ToDecimal(l_ContributionDataRow["Taxable"]) + l_Decimal_Interst;
                                            l_DataRow["AcctTotal"] = Convert.ToDecimal(l_DataRow["PersonalTotal"]) + Convert.ToDecimal(l_DataRow["PersonalTotal"]);
                                            l_DataRow["GrandTotal"] = l_DataRow["AcctTotal"];
                                        }//14 
                                        else
                                        { //15
                                            l_DataRow = l_RefundRequestDetailsDataTable.NewRow();
                                            l_Decimal_PersonalPreTax = Convert.ToDecimal(l_ContributionDataRow["Taxable"]);
                                            l_Decimal_PersonalInterest = l_Decimal_Interst;
                                            l_Decimal_PersonalTotal = l_Decimal_Interst + l_Decimal_PersonalPreTax;
                                            l_Decimal_AccountTotal = l_Decimal_PersonalTotal;
                                            l_Decimal_GrandTotal = l_Decimal_AccountTotal;
                                            l_DataRow["PersonalPreTax"] = l_Decimal_PersonalPreTax;
                                            l_DataRow["PersonalInterest"] = l_Decimal_PersonalInterest;
                                            l_DataRow["PersonalTotal"] = l_Decimal_PersonalTotal;
                                            l_DataRow["AcctTotal"] = l_Decimal_AccountTotal;
                                            l_DataRow["GrandTotal"] = l_Decimal_GrandTotal;
                                            l_DataRow["AcctType"] = l_String_AccountType;
                                            l_DataRow["PersonalPostTax"] = l_ContributionDataRow["Non-Taxable"];
                                            l_DataRow["YMCAPreTax"] = l_Decimal_YMCAPreTax;
                                            l_DataRow["YMCAInterest"] = l_Decimal_YmcaInterest;
                                            l_DataRow["YMCATotal"] = l_Decimal_YMCATotal;
                                            l_DataRow["AcctBreakDownType"] = l_String_AccountBreakDownType;
                                            l_DataRow["SortOrder"] = l_Integer_SortOrder;
                                            l_RefundRequestDetailsDataTable.Rows.Add(l_DataRow);
                                        } //15

                                        l_ContributionDataRow["Non-Taxable"] = "0.00";
                                        l_ContributionDataRow["Interest"] = Convert.ToDecimal(l_ContributionDataRow["Interest"]) - l_Decimal_Interst;
                                        l_ContributionDataRow["Emp.Total"] = l_ContributionDataRow["Interest"];
                                        l_Decimal_PersonalPostTax = 0;
                                        l_Decimal_PersonalInterest = 0;
                                        l_Decimal_PersonalTotal = 0;
                                        l_Decimal_AccountTotal = 0;
                                        l_Decimal_GrandTotal = 0;
                                        l_Decimal_PersonalTotal = 0;
                                        l_Decimal_YMCATotal = 0;
                                    }//YMCA(taxable)<0
                                }
                            }//4
                        }//3

                    }//2
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Finish: Loop started to calculate contribution.");
                }//1						  

                l_RefundDataSet = new DataSet("RefundRequest");
                if (l_Decimal_TDAmount > 0)
                {
                    l_DataRow = l_RefundRequestDataTable.Rows[0];
                    l_DataRow["HardShipAmt"] = l_Decimal_TDAmount;
                }

                l_RefundDataSet.Tables.Add(l_RefundRequestDataTable);
                l_RefundDataSet.Tables.Add(l_RefundRequestDetailsDataTable);

                //l_StringUniqueId = YMCARET.YmcaBusinessObject.RefundRequest.InsertRefunds(l_RefundDataSet); 
                this.m_DataSet_RefundRequest = l_RefundDataSet;
                //				if (l_StringUniqueId != "") 
                //				{ 
                //					TODO
                //					 we will set this Ref Request Id for processing
                //					YMCARET.YmcaBusinessObject.CashOutProcessBOClass objRequestProcessing=new CashOutProcessBOClass();
                //					objRequestProcessing.StringRefRequestId = l_StringUniqueId;
                //					objRequestProcessing.DataTableMemberAccountContributionProcessing=this.m_datatable_MemberAccountContribution;
                //					
                //				}
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("UpdateRefundRequestTable Method", "Finish: Updating refund request table.");
                return l_flag;
            }//for try 
            catch
            {
                throw;
            }
        }

        private void CalculateAge(DateTime parameterDOB, DateTime parameterDOD)
        {

            try
            {
                if (parameterDOB == Convert.ToDateTime("01/01/1900"))
                {
                    return;
                }
                if (parameterDOD == Convert.ToDateTime("01/01/1900"))
                {
                    this.m_decimal_PersonAge = Convert.ToInt32(this.DateDiff("m", Convert.ToDateTime(parameterDOB), System.DateTime.Now));
                }
                else
                {
                    this.m_decimal_PersonAge = Convert.ToInt32(this.DateDiff("y", Convert.ToDateTime(parameterDOB), Convert.ToDateTime(parameterDOD)));
                }
            }
            catch
            {
                throw;
            }
        }
        public int DateDiff(string Interval, DateTime Date1, DateTime Date2)
        {
            try
            {
                int intDateDiff = 0;
                TimeSpan time = Date1 - Date2;
                int timeHours = Math.Abs(time.Hours);
                int timeDays = Math.Abs(time.Days);

                switch (Interval.ToLower())
                {
                    case "h": // hours
                        intDateDiff = timeHours;
                        break;
                    case "d": // days
                        intDateDiff = timeDays;
                        break;
                    case "w": // weeks
                        intDateDiff = timeDays / 7;
                        break;
                    case "bw": // bi-weekly
                        intDateDiff = (timeDays / 7) / 2;
                        break;
                    case "m": // monthly
                        timeDays = timeDays - ((timeDays / 365) * 5);
                        intDateDiff = timeDays / 30;
                        break;
                    case "bm": // bi-monthly
                        timeDays = timeDays - ((timeDays / 365) * 5);
                        intDateDiff = (timeDays / 30) / 2;
                        break;
                    case "q": // quarterly
                        timeDays = timeDays - ((timeDays / 365) * 5);
                        intDateDiff = (timeDays / 90);
                        break;
                    case "y": // yearly
                        intDateDiff = (timeDays / 365);
                        break;
                }

                return intDateDiff;
            }
            catch
            {
                throw;
            }
        }



        public void MakeRefundRequestDataTables()
        {
            try
            {
                //this.CalculateAge(Convert.ToDateTime(this.m_datetime_PersonAgeDateOfBirth),Convert.ToDateTime(this.m_datetime_PersonAgeDateOfDeath));
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeRefundRequestDataTables Method", "Start: Make Refund Request DataTables.");
                this.DataTableEmploymentDetails();//this method will set necessary employment details.
                this.DecimalPIAAmount(this.m_string_FundEventId, this.l_bool_IsTerminated);//this method will set necessary PIA details
                this.LoadBAMaxLimit();  //SR:2015.07.01 - YRS 5.0-2523: calaculate BAMaxLimit
                this.DataTableAccountContribution(); //will fetch total funded account contribution
                this.CopyAccountContributionTable(); // will ready a Contribution data table for the calculations
                this.DoFullRefund();
                this.CalculateMinimumDistributionAmount();
                this.UpdateRefundRequestTable();
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("MakeRefundRequestDataTables Method", "Finish: Make Refund Request DataTables.");
            }
            catch
            {
                throw;
            }
        }

        private void CalculateMinimumDistributionAmount()
        {
            //decimal l_decimal_DistributionPeriod;
            DataTable dtRmdRecords = null;
            DataRow[] drRmdRecords = null;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateMinimumDistributionAmount Method", "Start: Calculating RMD Amount.");
                //if (!(this.l_bool_IsTerminated))
                //{
                //    return;
                //}
                //if (this.m_decimal_PersonAge < this.m_decimal_MinimumDistributedAge)
                //{
                //    return;
                //}
                //if (this.CheckForDistributionDate() == true)
                //{
                //    if (this.m_decimal_TaxedAmount != 0)
                //    {
                //        l_decimal_DistributionPeriod = YMCARET.YmcaBusinessObject.RefundRequest.GetDistributionPeriod(Convert.ToInt32(this.m_decimal_PersonAge));

                //        if (l_decimal_DistributionPeriod != 0)
                //        {
                //            this.m_decimal_MinDistributionAmount = this.m_decimal_TaxedAmount / l_decimal_DistributionPeriod;
                //        }
                //    }
                //}

                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateMinimumDistributionAmount Method", "Start: Get MRD records based on fundevents.");
                dtRmdRecords = YMCARET.YmcaBusinessObject.RefundRequest.GetMRDRecords(this.m_string_FundEventId);
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateMinimumDistributionAmount Method", "Finish: Get MRD records based on fundevents.");
                if (dtRmdRecords != null && dtRmdRecords.Rows.Count > 0)
                {
                    this.m_datatable_RmdRecordsDataTable = dtRmdRecords;
                    //drRmdRecords = dtRmdRecords.Select("PlanType = '" + this.m_string_PlanType + "'");

                    //Start:SR:2015.07.01 - YRS 5.0-2523: calaculate RMD records for both plan
                    if (this.strCashoutType == "special" || this.strCashoutType == "cashout_50-5k" || this.strCashoutType == "cashout_0-50") //SR|2015.10.09| YRS-AT-2463 - Added cashout type as "CashOut_50-5k" & "cashout_0-50" to create one Refund request for both(Retirement & Savings) the plans
                    {
                        if (this.m_string_PlanType == "BOTH")
                        {
                            drRmdRecords = dtRmdRecords.Select("PlanType IN ('RETIREMENT','SAVINGS')"); ;
                        }
                        else
                        {
                            drRmdRecords = dtRmdRecords.Select("PlanType = '" + this.m_string_PlanType + "'");
                        }
                    }
                    else 
                    {
                        drRmdRecords = dtRmdRecords.Select("PlanType = '" + this.m_string_PlanType + "'");
                    }
                    //Start:SR:2015.07.01 - YRS 5.0-2523: calaculate RMD records for both plan

                    if (drRmdRecords != null && drRmdRecords.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(drRmdRecords[0]["MRDTaxable"].ToString()))
                        {
                            this.m_decimal_MinDistributionAmount = Convert.ToDecimal(drRmdRecords[0]["MRDTaxable"].ToString());
                        }
                    }
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CalculateMinimumDistributionAmount Method", "Finish: Calculating RMD Amount.");
            }
            catch
            {
                throw;
            }
            finally
            {
                dtRmdRecords = null;
                drRmdRecords = null;
            }
        }

        DataTable m_datatable_RmdRecordsDataTable;
        public DataTable RmdRecordsDataTable
        {
            get
            {
                return m_datatable_RmdRecordsDataTable;
            }
            set
            {
                m_datatable_RmdRecordsDataTable = value;
            }
        }

        private void LoadBAMaxLimit()
        {
            DataTable l_DataTable_BAMaxLimit = null;
            try
            {
                if (this.DecimalPersonAge < 55)
                {
                    if ((HttpContext.Current.Cache["MaxBALimitUnder55"] == null))
                    {
                        //l_DataTable_BAMaxLimit = SearchConfigurationMaintenance("BA_MAX_LIMIT_UNDER_55");
                        l_DataTable_BAMaxLimit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_UNDER_55");
                        if (l_DataTable_BAMaxLimit != null)
                        {
                            foreach (DataRow l_DataRow in l_DataTable_BAMaxLimit.Rows)
                            {
                                if (((string)l_DataRow["Key"]).Trim().ToUpper() == "BA_MAX_LIMIT_UNDER_55")
                                {
                                    m_BAMaxLimit = decimal.Parse(l_DataRow["Value"].ToString());
                                    HttpContext.Current.Cache["MaxBALimitUnder55"] = m_BAMaxLimit;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        m_BAMaxLimit = (Decimal)HttpContext.Current.Cache["MaxBALimitUnder55"];
                    }
                }
                else if (this.DecimalPersonAge >= 55)
                {
                    if ((HttpContext.Current.Cache["MaxBALimitAbove55"] == null))
                    {
                        l_DataTable_BAMaxLimit = YMCARET.YmcaBusinessObject.RefundRequest.SearchConfigurationMaintenance("BA_MAX_LIMIT_55_ABOVE");
                        {
                            foreach (DataRow l_DataRow in l_DataTable_BAMaxLimit.Rows)
                            {
                                if (((string)l_DataRow["Key"]).Trim().ToUpper() == "BA_MAX_LIMIT_55_ABOVE")
                                {
                                    m_BAMaxLimit = decimal.Parse(l_DataRow["Value"].ToString());
                                    HttpContext.Current.Cache["MaxBALimitAbove55"] = m_BAMaxLimit;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        m_BAMaxLimit = (Decimal)HttpContext.Current.Cache["MaxBALimitAbove55"];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        # endregion

    }
}
