
//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		    :	YMCA YRS
// FileName			    :	CashOutBOClass.cs
// Author Name		    :	
// Employee ID		    :	
// Creation Time		:	
// Program Specification Name	:	

//Modification History :
//*******************************************************************************
//Modified Date		Modified By			Description
//*******************************************************************************
//2010.05.07        Ashish Srivastava   Resolve Production cashout lock Issue       
//2012.06.06        Sanjeev(SG)         BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2012.10.31		Priya				BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
//2012.12.31        Sanjeev(SG)         BT-1511: Changes requested while perfoming demo to the client for BT-960: YRS 5.0-1489
//2014.02.18        Dinesh Kanojia      BT-2139: YRS 5.0-2165:RMD enhancements 
//2014.12.09        Dinesh Kanojia      BT-2708:YRS 5.0-2259 - Optimizing the Cash out process to improve its performance (Re-Open)
//2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
//2015.07.01        Sanjay S.           BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
//2015.09.16        Manthan Rajguru     YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//2015.10.12        Sanjay Singh        YRS-AT-2463 - Cashout utility for participants with two plans. One release blank rather than two per participant 
//2015.10.21        Anudeep A           YRS-AT-2463 - Cashout utility for participants with two plans. One release blank rather than two per participant (TrackIT 21783)
//*******************************************************************************
using System;
using System.IO;
using System.Data;
using System.Globalization;
using System.Collections;
//using YMCAObjects;
namespace YMCARET.YmcaBusinessObject
{
    /// <summary>
    /// Summary description for CashOutBOClass.
    /// </summary>
    public class CashOutBOClass
    {
        # region "declaration"

        DataTable l_datatable_SelectedCashOuts;
        DataTable l_datatable_AccountBreakDown;
        DataTable l_datatable_AtsRefunds;
        DataTable l_datatable_AtsRefRequests;
        DataTable l_datatable_atsRefRequestDetails;
        decimal m_decimal_totalamount;
        decimal m_decimal_withheldtax;
        decimal m_decimal_terminationPIA;
        int l_integer_PersonAge;
        decimal m_decimal_MinimumDistributedAge;
        decimal m_decimal_MaximumPIAAmount;
        decimal m_decimal_MinimumDistributedTaxRate;
        decimal m_decimal_FederalTaxRate;
        int m_integer_RefundExpiryDate;
        string l_string_UniqueId = "";
        string m_string_GetPersonName = "";
        string m_string_PersonId;
        string m_string_FundEventId;
        string m_string_BatchId;
        bool m_bool_IsVested;
        bool m_bool_IsTerminated;
        string m_string_StatusType = "";

        //Added By SG: 2012.08.07: BT-960
        //DataTable l_datatable_RefundRequestProcessing = null;

        //Added By SG: 2012.09.06: BT-960
        decimal l_decimal_TaxRate;
        # endregion //"declaration"

        public CashOutBOClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        # region "Properties"


        public DataTable DataTableSelectedCashOuts
        {
            get
            {
                return l_datatable_SelectedCashOuts;
            }
            set
            {
                l_datatable_SelectedCashOuts = value;
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
        public DataTable DataTableAccountBreakDown
        {
            get
            {
                return l_datatable_AccountBreakDown;
            }
            set
            {
                l_datatable_AccountBreakDown = value;
            }
        }

        public int IntegerPersonAge
        {
            get
            {
                return l_integer_PersonAge;
            }
            set
            {
                l_integer_PersonAge = value;
            }
        }
        public decimal MinimumDistributedAge
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
        public decimal MinimumDistributedTaxRate
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
        public decimal MaximumPIAAmount
        {
            get
            {
                return m_decimal_MaximumPIAAmount;
            }
            set
            {
                m_decimal_MaximumPIAAmount = value;
            }
        }
        public decimal FederalTaxRate
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

        //Added By SG: 2012.09.06: BT-960
        public decimal TaxRate
        {
            get
            {
                return l_decimal_TaxRate;
            }
            set
            {
                l_decimal_TaxRate = value;
            }
        }

        # endregion //"Properties"

        # region "Methods"

        //this method will return us impt configurations as MinDistributionAge,ExpiryDate
        private void GetRefundConfigurationDataTable()
        {
            DataTable l_DataTable_RefundConfiguration;
            try
            {
                l_DataTable_RefundConfiguration = YMCARET.YmcaBusinessObject.RefundRequest.GetRefundConfiguration();

                foreach (DataRow l_DataRow in l_DataTable_RefundConfiguration.Rows)
                {
                    //Added By SG: 2012.010.04: BT-960
                    //if (Convert.ToString(l_DataRow["Key"]).Trim() == "REFUND_EXPIRE_DAYS")
                    if (Convert.ToString(l_DataRow["Key"]).Trim() == "CASHOUT_REFUND_EXPIRE_DAYS")
                    {
                        this.m_integer_RefundExpiryDate = Convert.ToInt32(l_DataRow["Value"]);
                    }
                    if (Convert.ToString(l_DataRow["Key"]).Trim() == "MIN_DISTRIBUTION_AGE")
                    {
                        this.m_decimal_MinimumDistributedAge = Convert.ToDecimal(l_DataRow["Value"]);
                    }
                    if (Convert.ToString(l_DataRow["Key"]).Trim() == "REFUND_MAX_PIA")
                    {
                        this.m_decimal_MaximumPIAAmount = Convert.ToDecimal(l_DataRow["Value"]);
                    }
                    if (Convert.ToString(l_DataRow["Key"]).Trim() == "REFUND_FEDERALTAXRATE")
                    {
                        this.l_decimal_TaxRate = Convert.ToDecimal(l_DataRow["Value"]);
                    }
                    if (Convert.ToString(l_DataRow["Key"]).Trim() == "REFUND_MINIMUMDISTRIBUTION_TAXRATE")
                    {
                        this.m_decimal_MinimumDistributedTaxRate = Convert.ToDecimal(l_DataRow["Value"]);
                    }
                }

                this.l_datatable_AccountBreakDown = YMCARET.YmcaBusinessObject.RefundRequest.GetAccountBreakDown();
            }
            catch
            {
                throw;
            }
        }


        //this method will fetch the Schema of the Refunds Table
        private void GetSchemaRefundDataTable()
        {
            DataSet l_DataSet;
            try
            {
                l_DataSet = YMCARET.YmcaBusinessObject.RefundRequest.GetSchemaRefundTable();
                if (l_DataSet != null)
                {
                    this.l_datatable_AtsRefunds = l_DataSet.Tables["atsRefunds"];
                    this.l_datatable_AtsRefRequests = l_DataSet.Tables["atsRefRequests"];
                    this.l_datatable_atsRefRequestDetails = l_DataSet.Tables["atsRefRequestDetails"];
                    //Added By SG: 2012.08.07: BT-960
                    //this.l_datatable_RefundRequestProcessing = l_DataSet.Tables["RefundProcessingDetails"];
                }
            }
            catch
            {
                throw;
            }
        }

        //this method will be invoked by the object of this class in UI.Only the selected records will
        //be passed onto this class.
        //Added By SG: 2012.06.19: BT-960
        //public void CreateAndProcessRequest(DataTable l_datatable_SelectedCashOuts, out bool l_bool_status)
        public void CreateAndProcessRequest(DataTable l_datatable_SelectedCashOuts, out bool l_bool_status, string CashOutReqType = null)
        {
            CashOutRequestBOClass objRefundRequest;
            CashOutProcessBOClass objRefundRequestProcessing;

            DataSet l_dataset_RefundRequest = null;
            DataSet l_dataset_RefundRequestProcessing = null;
            bool l_bool_RefundRequestProcessing;
            string l_string_errormessage = "";
            l_bool_status = false;
            YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = null;
            bool l_bool_IsRMDEligible = false;

            try
            {

                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: CreateAndProcessRequest method executing.");

                this.l_datatable_SelectedCashOuts = l_datatable_SelectedCashOuts;

                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Gettting SchemaRefundDataTable");
                this.GetSchemaRefundDataTable();
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Gettting SchemaRefundDataTable");

                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Reading RefundConfiguration");
                this.GetRefundConfigurationDataTable();
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Reading RefundConfiguration");

                foreach (DataRow drow in l_datatable_SelectedCashOuts.Rows)
                {
                    try
                    {

                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Loop Started to process selected cashout.");
                        //create a new request
                        objRefundRequest = new CashOutRequestBOClass();
                        objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();

                        #region START: SG: 2012.06.14: BT-960

                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("CreateAndProcessRequest Method", "Start: Batch Process Started for PerssID:" + m_string_PersonId);

                        #region Commented Code
                        //l_dataset_RefundRequest = new DataSet();
                        //l_dataset_RefundRequestProcessing = new DataSet();
                        //this.SetPropertiesForRefundRequest(drow, objRefundRequest);
                        //this.GetPersonName(drow);
                        //objRefundRequest.MakeRefundRequestDataTables();

                        //l_dataset_RefundRequest = objRefundRequest.DataSetRefundRequest;

                        //m_decimal_totalamount = objRefundRequest.DecimalTotalAmount;
                        //m_decimal_withheldtax = objRefundRequest.DecimalWithheldTax;
                        //m_decimal_terminationPIA = objRefundRequest.DecimalTerminationPIA;
                        ////these properties are used to pass the total amount,tax amount,termination pia and max pia amount to Log table for report purpose.
                        ////Its equivalent to refund amount
                        ////YMCARET.YmcaDataAccessObject.CashOutDAClass.BeginTransaction();	
                        //objCashOutDAClass.BeginTransaction();
                        ////l_string_UniqueId=YMCARET.YmcaDataAccessObject.CashOutDAClass.InsertRefunds(l_dataset_RefundRequest);
                        //l_string_UniqueId = objCashOutDAClass.InsertRefunds(l_dataset_RefundRequest);

                        ////update(method)										
                        ////l_dataset_RefundRequest = this.UpdateRefundRequestDataTable(l_string_UniqueId,l_dataset_RefundRequest);
                        ////objRefundRequest.DataSetRefundRequest = l_dataset_RefundRequest;

                        ////processing the newly created request
                        //objRefundRequestProcessing = new CashOutProcessBOClass();

                        //this.SetPropertiesForRefundRequestProcessing(objRefundRequest, objRefundRequestProcessing);
                        //objRefundRequestProcessing.MakeRefundRequestProcessingDataTables(objCashOutDAClass);
                        //l_dataset_RefundRequestProcessing = objRefundRequestProcessing.DataSetRefundRequestProcessing;
                        ////l_bool_RefundRequestProcessing = YMCARET.YmcaDataAccessObject.CashOutDAClass.SaveRefundRequestProcessing(l_dataset_RefundRequestProcessing,m_string_PersonId,m_string_FundEventId,"CASH",m_bool_IsVested, m_bool_IsTerminated,false);
                        //string para_out_string_ExeceptionReason = "";
                        //l_bool_RefundRequestProcessing = objCashOutDAClass.SaveRefundRequestProcessing(l_dataset_RefundRequestProcessing, m_string_PersonId, m_string_FundEventId, "CASH", m_bool_IsVested, m_bool_IsTerminated, false, this.m_string_StatusType.Trim().ToUpper(), out para_out_string_ExeceptionReason);
                        //if (l_bool_RefundRequestProcessing == true)
                        //{
                        //    l_string_errormessage = "";
                        //}
                        //else
                        //{
                        //    l_string_errormessage = para_out_string_ExeceptionReason;//todo exception message
                        //    l_string_UniqueId = "";
                        //    objCashOutDAClass.RollbackTransaction();
                        //}

                        ////now we will update the atsCashOutLogStatus where we will update RefRequestId for the successfully
                        ////processed requests.
                        ////todo calling the update status.
                        //objCashOutDAClass.UpdateCashOutStatus(l_string_UniqueId, m_string_FundEventId, m_decimal_totalamount, m_decimal_withheldtax, m_decimal_terminationPIA, m_decimal_MaximumPIAAmount, l_string_errormessage, m_string_BatchId);
                        ////YMCARET.YmcaDataAccessObject.CashOutDAClass.CommitTransaction();
                        ////objCashOutDAClass.RollbackTransaction();
                        #endregion

                        m_decimal_totalamount = objRefundRequest.DecimalTotalAmount;
                        m_decimal_withheldtax = objRefundRequest.DecimalWithheldTax;
                        m_decimal_terminationPIA = objRefundRequest.DecimalTerminationPIA;
                        //these properties are used to pass the total amount,tax amount,termination pia and max pia amount to Log table for report purpose.
                        //Its equivalent to refund amount
                        //YMCARET.YmcaDataAccessObject.CashOutDAClass.BeginTransaction();	
                        objCashOutDAClass.BeginTransaction();
                        //l_string_UniqueId=YMCARET.YmcaDataAccessObject.CashOutDAClass.InsertRefunds(l_dataset_RefundRequest);

                        l_bool_IsRMDEligible = Convert.ToBoolean(drow["IsRMDEligible"].ToString());

                        if (CashOutReqType == null)
                        {
                            objRefundRequest.StringRefundType = "CASH";
                            objRefundRequest.strCashoutType = "cashout_0-50";  //SR|2015.10.09| YRS-AT-2463 - Added cashout type as "CashOut_50-5k" to create one Refund request for both(Retirement & Savings) the plans
                            //Added By SG: 2012.09.06: BT-960
                            this.m_decimal_FederalTaxRate = this.l_decimal_TaxRate;
                        }
                        else if (CashOutReqType == "Request")
                        {
                            //Commented By SG: 2012.12.31: BT-1511
                            //objRefundRequest.StringRefundType = "SHIRA";
                            objRefundRequest.strCashoutType = "cashout_50-5k";  //SR|2015.10.09| YRS-AT-2463 - Added cashout type as "CashOut_50-5k" to create one Refund request for both(Retirement & Savings) the plans

                            //Added By SG: 2012.09.06: BT-960
                            if (l_bool_IsRMDEligible)
                            {
                                this.m_decimal_FederalTaxRate = this.l_decimal_TaxRate;

                                //Added By SG: 2012.12.31: BT-1511
                                objRefundRequest.StringRefundType = "REG";
                            }
                            else
                            {
                                this.m_decimal_FederalTaxRate = 0;

                                //Added By SG: 2012.12.31: BT-1511
                                objRefundRequest.StringRefundType = "SHIRA";
                            }
                        }
                        //Start: 2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                        else if (CashOutReqType.Trim().ToLower() == "special")
                        {
                            objRefundRequest.strCashoutType = "special"; //SR:2015.07.01 - YRS 5.0-2523: Set Cashout type as Special
                            this.m_decimal_FederalTaxRate = this.l_decimal_TaxRate;
                            objRefundRequest.StringRefundType = "REG";
                        }
                        //End: 2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                        else if (CashOutReqType.Trim().ToLower() == "process") 
                        {
                            objRefundRequest.strCashoutType = "cashout_50-5k";
                        }
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Setting Properties for Refund Request.");
                        this.SetPropertiesForRefundRequest(drow, objRefundRequest);
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Setting Properties for Refund Request.");

                        this.GetPersonName(drow);

                        objRefundRequest.MakeRefundRequestDataTables();

                        if (CashOutReqType == null || CashOutReqType == "Request" || CashOutReqType.Trim().ToLower() == "special") //2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                        {
                            l_dataset_RefundRequest = new DataSet();
                            l_dataset_RefundRequest = objRefundRequest.DataSetRefundRequest;
                            l_string_UniqueId = objCashOutDAClass.InsertRefunds(l_dataset_RefundRequest);
                            drow["RefRequestID"] = l_string_UniqueId;
                        }

                        //Commented By SG: 2012.08.28: BT-960
                        //if (Convert.ToBoolean(drow["IsRMDEligible"].ToString()) == true || CashOutReqType == null || CashOutReqType == "Process")
                        if (CashOutReqType == null || CashOutReqType == "Process")
                        {
                            //processing the newly created request
                            objRefundRequestProcessing = new CashOutProcessBOClass();
                            l_dataset_RefundRequestProcessing = new DataSet();

                            if (CashOutReqType == "Process")
                            {
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Initiate Process is started.");
                                l_string_UniqueId = drow["RefRequestId"].ToString();
                                //Added By SG: 2012.08.28: BT-960
                                objRefundRequestProcessing.IsRMDeligible = l_bool_IsRMDEligible;
                                //Added By SG: 2012.12.31: BT-1511
                                if (l_bool_IsRMDEligible)
                                    objRefundRequestProcessing.CashOutRefundType = "REG";
                                else
                                    objRefundRequestProcessing.CashOutRefundType = "SHIRA";
                                objRefundRequestProcessing.PlanType = drow["PlansType"].ToString().Trim();
                            }
                            else
                                objRefundRequestProcessing.CashOutRefundType = "CASH";


                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Set Properties for refund request processing.");
                            this.SetPropertiesForRefundRequestProcessing(objRefundRequest, objRefundRequestProcessing);
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Set Properties for refund request processing.");

                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: MakeRefundRequestProcessingDataTables for refund request processing.");
                            objRefundRequestProcessing.MakeRefundRequestProcessingDataTables(objCashOutDAClass);
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: MakeRefundRequestProcessingDataTables for refund request processing.");
                            l_dataset_RefundRequestProcessing = objRefundRequestProcessing.DataSetRefundRequestProcessing;
                            //l_bool_RefundRequestProcessing = YMCARET.YmcaDataAccessObject.CashOutDAClass.SaveRefundRequestProcessing(l_dataset_RefundRequestProcessing,m_string_PersonId,m_string_FundEventId,"CASH",m_bool_IsVested, m_bool_IsTerminated,false);
                            string para_out_string_ExeceptionReason = "";


                            l_bool_RefundRequestProcessing = objCashOutDAClass.SaveRefundRequestProcessing(l_dataset_RefundRequestProcessing, m_string_PersonId, m_string_FundEventId, "CASH", m_bool_IsVested, m_bool_IsTerminated, false, this.m_string_StatusType.Trim().ToUpper(), out para_out_string_ExeceptionReason);

                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Initiate Process is started.");

                            if (l_bool_RefundRequestProcessing == true)
                            {
                                l_string_errormessage = "";
                                if (CashOutReqType == null)
                                {
                                    objCashOutDAClass.InsertLettersFor_0to50(this.m_string_PersonId, this.m_string_FundEventId);
                                }
                            }
                            else
                            {
                                l_string_errormessage = para_out_string_ExeceptionReason;//todo exception message
                                l_string_UniqueId = "";
                            }
                        }

                        if (l_string_UniqueId == "")
                        {
                            //2012.10.31 Priya BT-960: YRS 5.0-1489:Add process for cash outs of $50.01 to $5,000
                            l_string_UniqueId = "";
                            drow["RefRequestID"] = "";
                            objCashOutDAClass.RollbackTransaction();
                        }
                        else
                        {
                            //update the atsCashOutLogStatus where we will update RefRequestId for the successfully processed requests. todo calling the update status.
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Update Cashout Status.");
                            objCashOutDAClass.UpdateCashOutStatus(l_string_UniqueId, m_string_FundEventId, m_decimal_totalamount, m_decimal_withheldtax, m_decimal_terminationPIA, m_decimal_MaximumPIAAmount, l_string_errormessage, m_string_BatchId, drow["PlansType"].ToString().Trim(), CashOutReqType);
                            objCashOutDAClass.CommitTransaction();
                            YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Update Cashout Status.");
                            //l_datatable_SelectedCashOuts.AcceptChanges();
                        }

                        #endregion END: SG: 2016.06.14: BT-960

                        l_bool_status = true;
                        //TODO : datatable creation which is to be used by UI layer for release blank report generation.
                        //adding a new row for the successfully processed requests. - l_dataset_RequestsProcessed;
                    }
                    catch (Exception ex)
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Exceptiom raised while processing records.");
                        l_string_errormessage = ex.Message;
                        l_string_UniqueId = "";
                        objCashOutDAClass.RollbackTransaction();
                        l_bool_status = false;
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Exceptiom raised while processing records.");
                        throw;
                    }
                    finally
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Finally block executing.");
                        //Added by Ashish 2010.05.07
                        objRefundRequest = null;
                        objRefundRequestProcessing = null;
                        //START: SG: 2016.06.14: BT-960
                        if (CashOutReqType == null || CashOutReqType == "Request" || CashOutReqType.Trim().ToLower() == "special") //2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
                            l_dataset_RefundRequest.Dispose();
                        else if (CashOutReqType == null || CashOutReqType == "Process")
                            l_dataset_RefundRequestProcessing.Dispose();
                        //END: SG: 2016.06.14: BT-960
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Finally block executing.");
                    }
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Loop Started to process selected cashout.");
                }

                if (CashOutReqType == "Process")
                {
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Start: Cancel Pending Cashout Request.");
                    CancelPendingCashoutRequest(this.m_string_BatchId, "CASOUT");
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: Cancel Pending Cashout Request.");
                }
                YMCARET.CommonUtilities.WebPerformanceTracer.LogPerformanceTrace("CreateAndProcessRequest Method", "Finish: CreateAndProcessRequest method executing.");
            }
            catch
            {
                l_bool_status = false;
                throw;
            }//Added By SG: 2012.08.07
            finally
            {
                objCashOutDAClass = null;
            }
        }

        public void CancelPendingCashoutRequest(string parameterBatchID, string parameterRefCanResCode)
        {
            YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = null;

            try
            {
                objCashOutDAClass = new YmcaDataAccessObject.CashOutDAClass();
                objCashOutDAClass.CancelPendingCashoutRequest(parameterBatchID, parameterRefCanResCode);
            }
            catch
            {
                throw;
            }
            finally
            {
                objCashOutDAClass = null;
            }
        }

        private void SetPropertiesForRefundRequest(DataRow parameterDataRow, CashOutRequestBOClass parameterobjRefundRequest)
        {
            DataSet dsMrdConfiguration = new DataSet();
            try
            {
                if (parameterDataRow["FundEventId"].GetType().ToString() != "System.DBNull")
                {
                    m_string_FundEventId = Convert.ToString(parameterDataRow["FundEventId"]);
                    parameterobjRefundRequest.StringFundEventId = m_string_FundEventId;
                }
                if (parameterDataRow["IsTerminated"].GetType().ToString() != "System.DBNull")
                {
                    if (Convert.ToInt32(parameterDataRow["IsTerminated"]) == 1)
                    {
                        m_bool_IsTerminated = true;
                        parameterobjRefundRequest.BoolIsTerminated = true;
                    }
                    else
                    {
                        m_bool_IsTerminated = false;
                        parameterobjRefundRequest.BoolIsTerminated = false;
                    }
                    //parameterobjRefundRequest.BoolIsTerminated = Convert.ToBoolean(parameterDataRow["IsTerminated"]);
                }
                if (parameterDataRow["PersonId"].GetType().ToString() != "System.DBNull")
                {

                    m_string_PersonId = Convert.ToString(parameterDataRow["PersonID"]);

                    // Added By Dinesh Kanojia on 12/02/2014
                    //2014.02.18        Dinesh Kanojia      BT-2139: YRS 5.0-2165:RMD enhancements - Get define MRD tax rate for pariticpants
                    dsMrdConfiguration = MRDBO.GetPersonMetaConfigurationDetails(Convert.ToString(parameterDataRow["PersonID"]), "RMD");
                    if (dsMrdConfiguration != null)
                    {
                        if (dsMrdConfiguration.Tables.Count > 0)
                        {
                            if (dsMrdConfiguration.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsMrdConfiguration.Tables[0].Rows[0]["RMDTAXRATE"].ToString()))
                                {
                                    this.m_decimal_MinimumDistributedTaxRate = Convert.ToDecimal(dsMrdConfiguration.Tables[0].Rows[0]["RMDTAXRATE"].ToString());
                                }
                                else
                                {
                                    this.m_decimal_MinimumDistributedTaxRate = 10;
                                }
                            }
                        }
                    }

                    parameterobjRefundRequest.StringPersonId = Convert.ToString(parameterDataRow["PersonID"]);
                }
                if (parameterDataRow["IsVested"].GetType().ToString() != "System.DBNull")
                {
                    if (Convert.ToInt32(parameterDataRow["IsVested"]) == 1)
                    {
                        m_bool_IsVested = true;
                        parameterobjRefundRequest.IsVested = true;
                    }
                    else
                    {
                        m_bool_IsVested = false;
                        parameterobjRefundRequest.IsVested = false;
                    }

                }

                if (parameterDataRow["PersonAge"].GetType().ToString() != "System.DBNull")
                {
                    parameterobjRefundRequest.DecimalPersonAge = Convert.ToDecimal(parameterDataRow["PersonAge"]);
                }
                else
                {
                    //TODO :
                    parameterobjRefundRequest.DecimalPersonAge = 0;
                }

                if (parameterDataRow["IntAddressId"].GetType().ToString() != "System.DBNull")
                {
                    parameterobjRefundRequest.IntegerAddressId = Convert.ToInt32(parameterDataRow["IntAddressId"]);
                }
                if (parameterDataRow["BatchId"].GetType().ToString() != "System.DBNull")
                {
                    m_string_BatchId = Convert.ToString(parameterDataRow["BatchId"]);
                }
                //plan split changes
                if (parameterDataRow["PlansType"].GetType().ToString() != "System.DBNull")
                {
                    parameterobjRefundRequest.StringPlanType = Convert.ToString(parameterDataRow["PlansType"]);
                    //this.m_string_PlanType = Convert.ToString(parameterDataRow["PlansType"]);
                }
                if (parameterDataRow["StatusType"].GetType().ToString() != "System.DBNull")
                {
                    this.m_string_StatusType = Convert.ToString(parameterDataRow["StatusType"]);
                }
                //plan split changes
                parameterobjRefundRequest.RefundExpiryDate = this.m_integer_RefundExpiryDate;
                parameterobjRefundRequest.DecimalMaxPIAAmount = this.m_decimal_MaximumPIAAmount;
                parameterobjRefundRequest.DataTableMemberAccountBreakdown = this.l_datatable_AccountBreakDown;
                parameterobjRefundRequest.DataTableAtsRefunds = this.l_datatable_AtsRefunds;
                parameterobjRefundRequest.DataTableAtsRefRequests = this.l_datatable_AtsRefRequests;
                parameterobjRefundRequest.DataTableatsRefRequestDetails = this.l_datatable_atsRefRequestDetails;
                parameterobjRefundRequest.DecimalMinimumDistributedAge = this.m_decimal_MinimumDistributedAge;
                parameterobjRefundRequest.DecimalFederalTaxRate = this.m_decimal_FederalTaxRate;
                parameterobjRefundRequest.DecimalMinimumDistributedTaxRate = this.m_decimal_MinimumDistributedTaxRate;
            }
            catch
            {
                throw;
            }
        }


        private DataSet UpdateRefundRequestDataTable(string parameterUniqueId, DataSet parameterDataSetRefundRequest)
        {
            DataSet l_RefundRequestDataSet = null;
            try
            {
                if (parameterUniqueId != "")
                {
                    l_RefundRequestDataSet = parameterDataSetRefundRequest;
                    l_RefundRequestDataSet.Tables["l_RefundRequestDataTable"].Rows[0][0] = parameterUniqueId;
                    //this.l_string_UniqueId=parameterUniqueId;
                    l_RefundRequestDataSet.AcceptChanges();
                    //parameterObjectRefundRequest.DataSetRefundRequest=l_RefundRequestDataSet;
                    //TODO
                    //PrintReports(); 					
                }

                return l_RefundRequestDataSet;
            }
            catch
            {
                throw;
            }
        }


        private void SetPropertiesForRefundRequestProcessing(CashOutRequestBOClass parameterobjRefundRequest, CashOutProcessBOClass parameterobjRefundRequestProcessing)
        {
            try
            {
                parameterobjRefundRequestProcessing.DataSetRefundRequest = parameterobjRefundRequest.DataSetRefundRequest;
                parameterobjRefundRequestProcessing.BoolIsMemberVested = parameterobjRefundRequest.IsVested;
                parameterobjRefundRequestProcessing.DecimalTerminationPIA = parameterobjRefundRequest.DecimalTerminationPIA;
                parameterobjRefundRequestProcessing.DecimalMaxPIAAmount = parameterobjRefundRequest.DecimalMaxPIAAmount;
                parameterobjRefundRequestProcessing.StringFundEventId = parameterobjRefundRequest.StringFundEventId;
                parameterobjRefundRequestProcessing.DataTableMemberAccountBreakdown = parameterobjRefundRequest.DataTableMemberAccountBreakdown;
                parameterobjRefundRequestProcessing.StringFundEventId = parameterobjRefundRequest.StringFundEventId;
                parameterobjRefundRequestProcessing.DecimalMinimumDistributionTaxRate = parameterobjRefundRequest.DecimalMinimumDistributedTaxRate;
                parameterobjRefundRequestProcessing.BoolIsTerminated = parameterobjRefundRequest.BoolIsTerminated;
                parameterobjRefundRequestProcessing.DecimalPersonAge = parameterobjRefundRequest.DecimalPersonAge;
                parameterobjRefundRequestProcessing.BoolCheckForTerminationDate = parameterobjRefundRequest.CheckForDistributionDate();
                parameterobjRefundRequestProcessing.DataTableCalculateContribution = parameterobjRefundRequest.DataTableCalculateContribution;
                parameterobjRefundRequestProcessing.DataTableMemberAccountContributionProcessing = parameterobjRefundRequest.DataTableMemberAccountContribution;
                parameterobjRefundRequestProcessing.StringGetPayeeName = this.m_string_GetPersonName;
                parameterobjRefundRequestProcessing.StringRefRequestId = this.l_string_UniqueId;
                parameterobjRefundRequestProcessing.StringPersonId = parameterobjRefundRequest.StringPersonId;
            }
            catch
            {
                throw;
            }

        }

        private void GetPersonName(DataRow parameterDataRow)
        {
            string l_firstname = "";
            string l_middlename = "";
            string l_lastname = "";
            try
            {
                if (parameterDataRow["LastName"].GetType().ToString() != "System.DBNull")
                {
                    l_lastname = Convert.ToString(parameterDataRow["LastName"]);
                }
                else
                {
                    l_lastname = string.Empty;
                }
                if (parameterDataRow["FirstName"].GetType().ToString() != "System.DBNull")
                {
                    l_firstname = Convert.ToString(parameterDataRow["FirstName"]);
                }
                else
                {
                    l_firstname = string.Empty;
                }
                if (parameterDataRow["MiddleName"].GetType().ToString() != "System.DBNull")
                {
                    l_middlename = Convert.ToString(parameterDataRow["MiddleName"]);
                }
                else
                {
                    l_middlename = string.Empty;
                }
                this.m_string_GetPersonName = (Convert.ToString(parameterDataRow["LastName"])) + " " + (Convert.ToString(parameterDataRow["MiddleName"])) + " " + (Convert.ToString(parameterDataRow["FirstName"]));
            }
            catch
            {
                throw;
            }
        }
        # endregion //"Methods"
        # region "CallForUI"
        public void InsertReportData(DataSet parameterReportData)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                objCashOutDAClass.InsertReportData(parameterReportData);
                objCashOutDAClass = null;
            }

            catch
            {
                throw;
            }
        }
        //Added By SG: 2012.19.11: BT-960
        //public void DeleteReportData(string parameterUserId, string parameterIPAddress)
        public void DeleteReportData(string parameterUserId, string parameterIPAddress, string parameterBatchId)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                //Added By SG: 2012.19.11: BT-960
                //objCashOutDAClass.DeleteReportData(parameterUserId, parameterIPAddress);
                objCashOutDAClass.DeleteReportData(parameterUserId, parameterIPAddress, parameterBatchId);
                objCashOutDAClass = null;
            }

            catch
            {
                throw;
            }
        }
        public void UpdateCashoutLog(int parameterintStdReportCount, int parameterintActualReportCount, string parameterchvExceptionReason, string parameterguiFundEventId)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                objCashOutDAClass.UpdateCashoutLog(parameterintStdReportCount, parameterintActualReportCount, parameterchvExceptionReason, parameterguiFundEventId);
                objCashOutDAClass = null;
            }
            catch
            {
                throw;
            }
        }
        public void UpdateCashoutLogForRpts(DataSet parameterLogData)
        {
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                objCashOutDAClass.UpdateCashoutLogForRpts(parameterLogData);
                objCashOutDAClass = null;
            }
            catch
            {
                throw;
            }
        }


        public DataSet GetCashoutLogSchema()
        {
            DataSet l_dataset_CashoutLogSchema = null;
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_CashoutLogSchema = objCashOutDAClass.GetCashoutLogSchema();
                objCashOutDAClass = null;
                return l_dataset_CashoutLogSchema;
                //return YMCARET.YmcaDataAccessObject.CashoutDAClass.GetCashoutLogSchema();
            }
            catch
            {
                throw;
            }
        }

        public DataSet SelectReportData(string parameterIPAddress, string parameterUserId, string parameterBatchId)
        {
            DataSet l_dataset_SelectReportData = null;
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_SelectReportData = objCashOutDAClass.SelectReportData(parameterIPAddress, parameterUserId, parameterBatchId);
                objCashOutDAClass = null;
                return l_dataset_SelectReportData;

            }
            catch
            {
                throw;
            }
        }
        public DataSet GetNextBatchId()
        {
            DataSet l_dataset_BatchId = new DataSet();
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_BatchId = objCashOutDAClass.GetNextBatchId();
                objCashOutDAClass = null;
                return l_dataset_BatchId;
                //return YMCARET.YmcaDataAccessObject.CashOutDAClass.GetAmountRange();
            }
            catch
            {
                throw;
            }
        }
        public DataSet GetAmountRange()
        {
            DataSet l_dataset_AmountRange = new DataSet();
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_AmountRange = objCashOutDAClass.GetAmountRange();
                objCashOutDAClass = null;
                return l_dataset_AmountRange;
                //return YMCARET.YmcaDataAccessObject.CashOutDAClass.GetAmountRange();
            }
            catch
            {
                throw;
            }
        }





        public DataSet GetEligibleParticipants(Double parameterLowerLimit, Double parameterUpperLimit, string parameterCashOutRptDesc, string parameterIPAddress, string parameterUserId)
        {
            DataSet l_dataset_EligibleParticipants = new DataSet();
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_EligibleParticipants = objCashOutDAClass.GetEligibleParticipants(parameterLowerLimit, parameterUpperLimit, parameterCashOutRptDesc, parameterIPAddress, parameterUserId);
                objCashOutDAClass = null;
                return l_dataset_EligibleParticipants;

                //return YMCARET.YmcaDataAccessObject.CashOutDAClass.GetEligibleParticipants(parameterLowerLimit,parameterUpperLimit);
            }
            catch
            {
                throw;
            }
        }

        //Added By SG: 2012.06.06: BT-960
        public DataSet GetRequestedParticipantForProcessing(string parameterStBatchID)
        {
            DataSet l_dataset_RequestedParticipant = new DataSet();
            YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = null;
            try
            {
                objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_RequestedParticipant = objCashOutDAClass.GetRequestedParticipantForProcessing(parameterStBatchID);

                return l_dataset_RequestedParticipant;
            }
            catch
            {
                throw;
            }
            finally
            {
                objCashOutDAClass = null;
            }
        }
        //2015.10.21        Anudeep A               YRS-AT-2463 Added a new parameter to filter as per the cashout range
        public DataTable GetDataTableCashoutBatchRecords(string strCashoutRange)
        {
            YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = null;
            try
            {
                objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();

                return objCashOutDAClass.GetDataTableCashoutBatchRecords(strCashoutRange);//2015.10.21        Anudeep A               YRS-AT-2463 Added a new parameter to filter as per the cashout range
            }
            catch
            {
                throw;
            }
            finally
            {
                objCashOutDAClass = null;
            }
        }

        # endregion //"CallForUI"
        //START: Priya: 2012.10.31: BT-960
        public DataSet GetBatchDetails(string parameterStBatchID)
        {
            DataSet l_dataset_BatchParticipant = new DataSet();
            YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = null;
            try
            {
                objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_BatchParticipant = objCashOutDAClass.GetBatchDetails(parameterStBatchID);

                return l_dataset_BatchParticipant;
            }
            catch
            {
                throw;
            }
            finally
            {
                objCashOutDAClass = null;
            }
        }

        public DataSet getCashOutFormsLetters(string key)
        {
            DataSet l_dataset_CashOutFormsLetters = new DataSet();
            YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = null;
            try
            {
                objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_CashOutFormsLetters = objCashOutDAClass.getCashOutFormsLetters(key);

                return l_dataset_CashOutFormsLetters;
            }
            catch
            {
                throw;
            }
            finally
            {
                objCashOutDAClass = null;
            }
        }


        public DataSet GetCashOutUnprocessIDM(string strBatchId)
        {
            YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDaClass = null;
            try
            {
                objCashOutDaClass = new YmcaDataAccessObject.CashOutDAClass();
                return objCashOutDaClass.GetUnProcessIDM(strBatchId);
            }
            catch
            {
                throw;
            }
            finally
            {
                objCashOutDaClass = null;
            }
        }

        //SP 2014.10.07 BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS -Start
        public void ExpiredRefRequestsForSelectedPerson(string XmlSelectedPersDetails)
        {
            YmcaDataAccessObject.CashOutDAClass objCashOutDaClass = null;
            try
            {
                objCashOutDaClass = new YmcaDataAccessObject.CashOutDAClass();
                objCashOutDaClass.ExpiredRefRequestsForSelectedPerson(XmlSelectedPersDetails);
            }
            catch
            {
                throw;
            }
        }
        //SP 2014.10.07 BT-2633\YRS 5.0-2403: Remove expiration dates from withdrawal requests in YRS -End

        //Start: 2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
        public DataSet GetSpecialEligibleParticipants(string strFundiD)
        {
            DataSet l_dataset_EligibleParticipants = new DataSet();
            try
            {
                YMCARET.YmcaDataAccessObject.CashOutDAClass objCashOutDAClass = new YMCARET.YmcaDataAccessObject.CashOutDAClass();
                l_dataset_EligibleParticipants = objCashOutDAClass.GetSpecialEligibleParticipants(strFundiD.Replace(",\r\n", ","));
                objCashOutDAClass = null;
                return l_dataset_EligibleParticipants;
            }
            catch
            {
                throw;
            }
        }
        //End: 2015.06.15        Dinesh Kanojia      BT-2890: YRS 5.0-2523:Create script to populate tables for Release blanks
    }
}
