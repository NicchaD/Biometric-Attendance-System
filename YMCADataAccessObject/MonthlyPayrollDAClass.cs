//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	MonthlyPayrollDAClass.cs
// Author Name		:	Ragesh V.P
// Employee ID		:	34231	
// Email				:	ragesh_vp@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	7/25/2005 11:55:28 AM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	Checked out to change the name of the Dataset
//*******************************************************************************
// Updated by: Nikunj Patel
// Date: 2007.02.14 - 2007.02.21
// Description:	The code has been modified to provide an output in EDI formats for
//				US cheques that need to be printed. This requirement came up because
//				the organization wants to outsource the printing of cheques
// Updated by: Nikunj Patel
// Date: 2007.04.10
// Description:	The mail handling codes were missing from the EDI output. The sub-
//				function has been updated as per email from Mark dt. 2007.04.09.
// Updated by: Nikunj Patel
// Date: 2007.04.26
// Description:	The RMR^IV and RMR^IK lines should not be printed when processing
//				Exp Dividends as per email from Mark dt. 2007.04.24. The ISA segment
//				should contain T or P depending on EDI_MODE key from the 
//				atsMetaConfiguration table as per another conversation between Ragesh
//				and Mark earlier.
// Updated by: Nikunj Patel
// Date: 2007.05.08
// Description:	The RMR^2G and RMR^IK lines should not be printed when both current and
//				YTD amounts are zero as per email from Mark dt. 2007.04.30.
// Updated by: Nikunj Patel
// Date: 2007.05.09
// Description:	The Payee N4 address line should contain the country code if it is not 
//				US or USA as per talk with Ragesh on 2007.05.09.
// Updated by: Ashish Srivastava
// Date: 2010.07.07
// Description: Enhancements 08 changes
// Updated by: Prasad Jadhav
// Date: 2011.07.21
// Description:	BT:855- YRS 5.0-1344 : Create new IAT output file   
// Updated by: Prasad Jadhav
// Date: 2011.08.18 (updated)
// Check date passed to IAT file. Batch number for IAT is being created using a new logic. Changed code to call the IAT batch number during normal payroll as well as EXP dividend payroll process.
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			            Date			Description
//*******************************************************************************
//Prasad Jadhav                     10/04/2011      For BT: 645: YRS 5.0-632 : Test database output files need word "test" in them.
//Prasad Jadhav                     10/11/2011      For BT-645:  YRS 5.0-632 : Test database output files need word "test" in them. 
//Manthan Rajguru                   2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Manthan Rajguru                   2016.03.31      YRS-AT-2181: Allow for non-US addresses in annuity EDI file 
//Chandra sekar                     2016.07.25      YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
//Manthan Rajguru                   2017.01.26      YRS-AT-3288 -  YRS bug: bitActive and bitPrimary validation for Annuity Processing
//Megha Lad                         2019.11.22      YRS-AT-4602 - YRS enh:State Withholding Project - Export file Annuity Payroll
//Megha Lad                         2019.11.22      YRS-AT-4677 - State Withholding - Validations for exporting "Change File" (Monthly Payroll).
//Sanjay Singh                      2020.01.13      YRS-AT-4641 - YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
//*******************************************************************************

using System;
using System.IO;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections; 
using System.Globalization;

namespace YMCARET.YmcaDataAccessObject
{
	/// <summary>
	/// Summary description for MonthlyPayrollDAClass.
	/// </summary>
	public class MonthlyPayrollDAClass
	{
		private DataTable l_datatable_FileList;

		public MonthlyPayrollDAClass()
		{
			//
			// TODO: Add constructor logic here
			//

		}

		//This method Returns the dataset dsCurrntCheckSeries from Procedure 'vrCheckSeries' 
		//
		//
		
		public static DataSet CurrentCheckSeries()
		{
			DataSet dsCurrentCheckSeries = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PAYROL_CheckSeries");
				
				if (getCommandWrapper == null) return null;
						
				dsCurrentCheckSeries = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsCurrentCheckSeries,"CheckSeries");
				
				return dsCurrentCheckSeries;
			}
			catch 
			{
				throw;
			}
		}

		//This method Returns the dataset dsCurrntCheckSeries from Procedure 'vrCheckSeries' 
		//
		//


		public static DataSet GetNewGuid()
		{
			DataSet dsNewGuid = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PAYROLL_GetNewGuid");
				
				if (getCommandWrapper == null) return null;
						
				dsNewGuid = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsNewGuid,"NewGuid");
				
				return dsNewGuid;
			}
			catch 
			{
				throw;
			}
		}

		//This method Returns the dataset dsCurrntCheckSeries from Procedure 'vrCheckSeries' 
		//
		//

		public static DataSet PayRollLast()
		{
			DataSet dsPayRollLast = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PAYROL_MonthYear");
				
				if (getCommandWrapper == null) return null;
				
				dsPayRollLast = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsPayRollLast,"LastPayrollDate");
				
				return dsPayRollLast;
			}
			catch 
			{
				throw;
			}
		}

		public static DataSet GetNextBusinessDay(System.DateTime parameterGetNextBusinessDate)
		{
			DataSet dsNextBusinessDay = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.ap_SEL_DWPeriodNextBusinessDay");
				
				if (getCommandWrapper == null) return null;

				db.AddInParameter(getCommandWrapper, "@date",DbType.Date,parameterGetNextBusinessDate);
						
				dsNextBusinessDay  = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsNextBusinessDay ,"Next Business Day");
				
				return dsNextBusinessDay;
			}
			catch 
			{
				throw;
			}
		}

        // SR | 2020.01.09 | YRS-AT-4602 | Pass parameter payroll check date to validate payroll outsourcing key ON/OFF in procedure.
        //public static DataSet MetaOutputChkFileType()
        public static DataSet MetaOutputChkFileType(DateTime? dtCheckdate = null)
		{
			DataSet dsMetaOutputChkFileType = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;
                // START: SR | 2020.01.09 | YRS-AT-4602 | if payroll check date has no value then use today date as default value.
                if (!dtCheckdate.HasValue)
                    dtCheckdate = DateTime.Now;
                // END: SR | 2020.01.09 | YRS-AT-4602 | if payroll check date has no value then use today date as default value.

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_MetaOutputChkFileType");
				
				if (getCommandWrapper == null) return null;
                db.AddInParameter(getCommandWrapper, "@dateTimePayrollDate", DbType.DateTime, dtCheckdate); // SR | 2020.01.09 | YRS-AT-4602 | Pass parameter payroll check date to validate payroll outsourcing key ON/OFF in procedure.

				dsMetaOutputChkFileType = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsMetaOutputChkFileType,"MetaOutput");
				
				return dsMetaOutputChkFileType;
			}
			catch 
			{
				throw;
			}
		}


		public static bool UpdateCheckSeriesCurrentValues(DataSet parameter_DatasetCheckSeries)
		{
			Database l_DataBase = null;

			DbCommand InsertDBCommandWrapper = null; 
			DbCommand UpdateDBCommandWrapper = null; 
			DbCommand DeleteDBCommandWrapper = null;

			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (parameter_DatasetCheckSeries == null) return false;

				if (l_DataBase  == null) return false;

				UpdateDBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_Payroll_UpdateCheckSeriesVal");

				if (UpdateDBCommandWrapper != null)
				{
					l_DataBase.AddInParameter (UpdateDBCommandWrapper, "@intCurrentCheckNumber", DbType.String,"CurrentCheckNumber"  , DataRowVersion.Current);
					l_DataBase.AddInParameter (UpdateDBCommandWrapper, "@ChrCheckSeriesCode", DbType.String,"CheckSeriesCode" , DataRowVersion.Current);
					
					l_DataBase.UpdateDataSet(parameter_DatasetCheckSeries, "CheckSeries", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper, UpdateBehavior.Standard);

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

		public static string RetrieveBatchNoforEDI(Database l_DataBase,DbTransaction P_DBTransaction,bool parameterboolproof)
		{
			//Database l_DataBase = null;

			DbCommand l_DBCommandWrapper = null; 
			DataSet l_DataSet = null;
			string l_retrunstring = "";
			
			try
			{
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_Payroll_EDICheckSeriesVal");
				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);
					
				if (l_DBCommandWrapper != null) 
				{
					l_DataBase.AddInParameter (l_DBCommandWrapper, "@bitProofReport", DbType.Boolean,parameterboolproof);
								
					l_DataSet = new DataSet ("Batch");
									
					l_DataBase.LoadDataSet (l_DBCommandWrapper,l_DataSet, "Batch",P_DBTransaction);
				
					if (l_DataSet != null) 
					{
						l_retrunstring = l_DataSet.Tables[0].Rows[0]["intCurrentCheckNumber"].ToString().Trim();  
						
					}
					else
						throw new Exception("Cannot read the Existing Batch Number for the EDI Process"); 

				}
				return l_retrunstring;
			}
			catch 
			{
				throw;
			}
		}
        public static int RetrieveBatchNoforIAT(Database l_DataBase, DbTransaction P_DBTransaction, bool parameterboolproof)
        {
            //Database l_DataBase = null;
            DbCommand l_DBCommandWrapper = null;
            DataSet l_DataSet = null;
            int l_returnint = 1;

            try
            {
                l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_Payroll_IATCheckSeriesVal");
                l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmallConnectionTimeOut"]);

                if (l_DBCommandWrapper != null)
                {
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@bitProofReport", DbType.Boolean, parameterboolproof);
                    l_DataSet = new DataSet("Batch");
                    l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, "Batch", P_DBTransaction);
                    if (l_DataSet != null)
                    {
                        l_returnint = int.Parse(l_DataSet.Tables[0].Rows[0]["intCurrentCheckNumber"].ToString().Trim());
                    }
                    else
                        throw new Exception("Cannot read the Existing Batch Number for the IAT Process");
                }
                return l_returnint;
            }
            catch
            {
                throw;
            }
        }

		public static bool InsertDisbursementFileTypes(DataSet DatasetdisbursementFileTypes, Database l_DataBase, DbTransaction P_DBTransaction )
		{
			//Database l_DataBase = null;

			DbCommand InsertDBCommandWrapper = null; 
			DbCommand UpdateDBCommandWrapper = null; 
			DbCommand DeleteDBCommandWrapper = null;

			try
			{
				//l_DataBase = DatabaseFactory.CreateDatabase ("YRS");

				if (DatasetdisbursementFileTypes == null) return false;

				if (l_DataBase  == null) return false;

				InsertDBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_Payroll_InsertDisbursementFiles");
				if (InsertDBCommandWrapper != null)
				{					
					InsertDBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
					l_DataBase.AddInParameter (InsertDBCommandWrapper, "@DisbursementID", DbType.Guid, "DisbursementID" , DataRowVersion.Current);
					l_DataBase.AddInParameter (InsertDBCommandWrapper, "@OutputFileID", DbType.Guid, "GuiUniqueID" , DataRowVersion.Current);
					
					l_DataBase.UpdateDataSet(DatasetdisbursementFileTypes, "DisbursementFiles", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper,P_DBTransaction);
					
				}
				else
				{
					return false;
				}

				InsertDBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_Payroll_InsertDisbursementOutputFiles");
				
				if (InsertDBCommandWrapper != null)
				{
					InsertDBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
 					l_DataBase.AddInParameter (InsertDBCommandWrapper, "@UniqueID", DbType.Guid,"GuiUniqueID"  , DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileLocation", DbType.String, "chvOutputFileLocation", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileName", DbType.String, "chvOutputFileName", DataRowVersion.Current);
                    l_DataBase.AddInParameter(InsertDBCommandWrapper, "@OutputFileType", DbType.String, "chrOutputFileType", DataRowVersion.Current);
					
					l_DataBase.UpdateDataSet(DatasetdisbursementFileTypes, "OutPutfilePath", InsertDBCommandWrapper, UpdateDBCommandWrapper, DeleteDBCommandWrapper,P_DBTransaction);
					
				}
				else
				{
					return false;
				}

				return true;

			}
			catch 
			{
				return false;
			}
		}
			
		public DataTable datatable_FileList
		{
			get{return l_datatable_FileList;}
		}
	//START : ML | 2019.12.18 | YRS-AT-4677 | Declare public table to capture validations from database (for NTPYRL file)
        private DataTable ExceptionLogForNTPYRL; 
        public DataTable dtExceptionLogForNTPYRL
        {
            get { return ExceptionLogForNTPYRL; }
        }

        // START : ML | 2019.10.22 | YRS-AT-4601 |  Defined property to check Payment Outsourcing is live or not
        bool isPaymentOutSourcingKeyON = true;
        public bool IsPaymentOutSourcingKeyON
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

	//END : ML | 2019.12.18 | YRS-AT-4677 | Declare public table to capture validations from database (for NTPYRL file)
		//public  static bool getPayRollData (DateTime parameterdateTimePayrollDate,bool parameterboolproof ,long parameterFirstCheckCanada,long parameterFirstCheckUS,DateTime parameterDateCheckDate )
		public bool getPayRollData (DateTime parameterdateTimePayrollDate,bool parameterboolproof ,long parameterFirstCheckCanada,long parameterFirstCheckUS,DateTime parameterDateCheckDate)
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			DbConnection DBconnectYRS;
			DbTransaction DBTransaction;
			YMCARET.YmcaDataAccessObject.MonthlyProcessBOWraperClass ProcessPayroll = new YMCARET.YmcaDataAccessObject.MonthlyProcessBOWraperClass(); 
			bool l_bool_TransactionStarted  = false;


            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", "GetPayrollData() START");
                l_DataBase = DatabaseFactory.CreateDatabase("YRS");

                DBconnectYRS = l_DataBase.CreateConnection();

                DBconnectYRS.Open();

                // Set the Transaction process ON

                DBTransaction = DBconnectYRS.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                l_bool_TransactionStarted = true;

                try
                {

                    //START: SR | 2020.01.13 | YRS-AT-4641 | Update StateTax Withholding for final Payroll Process
                    try
                    {
                        // This call will update state tax details from imported reverse feed file for given payroll date in general withholding table
                        SetStateTaxWithholdingForPayrollProcess(parameterboolproof, parameterDateCheckDate, l_DataBase, DBTransaction); 
                    }
                    catch {
                        throw;  
                    }
                    //END: SR | 2020.01.13 | YRS-AT-4641 | Update StateTax Withholding For Payroll Process


                    if (l_DataBase == null) return false;

                    l_DBCommandWrapper = l_DataBase.GetStoredProcCommand("dbo.yrs_usp_PAYROLL_GetPayrollData");
                    l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);

                    if (l_DBCommandWrapper == null) return false;

                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@dateTimePayrollDate", DbType.DateTime, parameterdateTimePayrollDate);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@dateTimeCheckDate", DbType.DateTime, parameterDateCheckDate);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@bitProofReport", DbType.Boolean, parameterboolproof);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@ln_FirstCheckCANADA", DbType.Int32, parameterFirstCheckCanada);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@ln_FirstCheckUS", DbType.Int32, parameterFirstCheckUS);

                    l_DataSet = new DataSet("Payroll Data");
                    //START: ML | 2019.12.18 | YRS-AT-4677 ,YRS-AT-4602 | Add new tables to store data
                    if (parameterboolproof == true)
                        l_TableNames = new string[] { "Payroll", "FedTaxWithholdingRO", "Withholdings", "YTDWithholdings", "DisbursementFiles", "OutPutfilePath", "BankMonthlyPayrollData", "ExpceptionLogData", "ExportBaseHeaderId" };//ML | YRS-AT-4602 | 2019.11.22 | Monthly payroll data for Bank 
                    else
                        l_TableNames = new string[] { "Payroll", "FedTaxWithholdingRO", "Withholdings", "YTDWithholdings", "DisbursementFiles", "OutPutfilePath" };
                    //END : ML | 2019.12.18 | YRS-AT-4677 | Declare public table to capture validations from database (for NTPYRL file)
                    l_DataBase.LoadDataSet(l_DBCommandWrapper, l_DataSet, l_TableNames, DBTransaction);


                    //Check whether the Payroll mode is on Proof report mode. 

                    // If yes donot Update Current Check Numbers.


                    //					if (!parameterboolproof)
                    //					{
                    //						// Update Current Check Series.
                    //
                    //						l_DatasetCheckSeries = CurrentCheckSeries();
                    //
                    //						foreach (DataRow oRow in l_DatasetCheckSeries.Tables[0].Rows)
                    //						{
                    //							if (oRow["CheckSeriesCode"].ToString() ==  "PAYROL")
                    //							{
                    //								oRow["CurrentCheckNumber"] = parameterFirstCheckUS;
                    //							}
                    //							else
                    //							{
                    //								oRow["CurrentCheckNumber"] = parameterFirstCheckCanada; 
                    //							}
                    //
                    //
                    //						}
                    //
                    //						if (!UpdateCheckSeriesCurrentValues(l_DatasetCheckSeries))
                    //						{
                    //							// Check Number updation is not completed properly.
                    //							return false;
                    //
                    //						}
                    //					}

                    //START: SR | 2020.01.13 | YRS-AT-4641 | Update StateTax Withholding for final Payroll Process
                    try
                    {
                        // This call will update check series from imported reverse feed file for given payroll date in disbursement funding table.
                        UpdatePayrollCheckSeries(parameterboolproof, parameterDateCheckDate, l_DataBase, DBTransaction);
                        // This call will update EFT record as funded for given payroll date in disbursement funding table.
                        //UpdatePayrollEFTFundingRecords(parameterboolproof, parameterDateCheckDate, l_DataBase, DBTransaction);
                    }
                    catch
                    {
                        throw;
                    }
                    //END: SR | 2020.01.13 | YRS-AT-4641 | Update StateTax Withholding for final Payroll Process

                    // Populate Batch NO for the EDI.
                    ProcessPayroll.EDIBatchNO = RetrieveBatchNoforEDI(l_DataBase, DBTransaction, parameterboolproof);
                    ProcessPayroll.IATBatchNO = RetrieveBatchNoforIAT(l_DataBase, DBTransaction, parameterboolproof);

                    // SATRT | SR | 2020.01.02 | YRS-AT-4602 | get Payment out sourcing key.
                    IsPaymentOutSourcingKeyON = SetPaymentOutsourcingKey(parameterDateCheckDate);
                    ProcessPayroll.IsPaymentOutSourcingKeyON = IsPaymentOutSourcingKeyON;
                    // END | SR | 2020.01.02 | YRS-AT-4602 | get Payment out sourcing key.

                    //New parameter 'parameterdateTimePayrollDate' passed to function to pass payroll date to IAT file
                    if (ProcessPayroll.PreparOutPutFileData(l_DataSet, parameterboolproof, parameterDateCheckDate, "ANN"))
                    {
                        if (parameterboolproof != true)
                        {
                            // Insert the Disbursement file and the output files should be updated only when 
                            // the output mode not in proof method mode.
                            InsertDisbursementFileTypes(l_DataSet, l_DataBase, DBTransaction);

                        }

                        // START | SR | 2019.12.31 | YRS-at-4602 | update export file status.
                        if (IsPaymentOutSourcingKeyON)
                        {
                            if (parameterboolproof == true)
                            {
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("Update export file status in db. - START"));
                                DataSet ldataset = l_DataSet;
                                DataTable ExportBaseHeader = ldataset.Tables["ExportBaseHeaderId"];
                                if (HelperFunctions.isNonEmpty(ExportBaseHeader))
                                {
                                    int processId = Convert.ToInt16(ExportBaseHeader.Rows[0][0].ToString());
                                    ChangeExportFileStatus(processId, "EXPORTED", l_DataBase, DBTransaction);
                                }
                                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("Update export file status in db. - END"));
                            }
                        }
                        // END | SR | 2019.12.31 | YRS-at-4602 | update export file status.

                        DBTransaction.Commit();
                        DBconnectYRS.Close();

                        l_datatable_FileList = ProcessPayroll.datatable_FileList;
                        l_bool_TransactionStarted = false;
                        //START : ML | 2019.12.18 | YRS-AT-4677 | Set datain  table (validations from database for NTPYRL file)
                        if (parameterboolproof == true)
                        {
                            ExceptionLogForNTPYRL = l_DataSet.Tables["ExpceptionLogData"];
                        }
                        //END :ML | 2019.12.18 | YRS-AT-4677 | Set datain  table (validations from database for NTPYRL file)
                        return true;
                    }
                    else
                    {

                        DBTransaction.Rollback();
                        DBconnectYRS.Close();
                        l_bool_TransactionStarted = false;
                        return false;

                    }

                }
                catch (Exception ex)
                {

                    if (l_bool_TransactionStarted)
                    {
                        DBTransaction.Rollback();
                        DBconnectYRS.Close();
                    }
                    throw new Exception(ex.Message);

                }
            }
            catch
            {
                throw;
            }
            finally { 
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", "GetPayrollData() END");
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }
		
		}

		public bool getEXPDividentData (DateTime parameterdateTimePayrollDate,bool parameterboolproof ,long parameterFirstCheckCanada,long parameterFirstCheckUS,DateTime parameterDateCheckDate,string parameterCurrentExpUniqueiId)
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			DataSet l_DataSet = null;
			string [] l_TableNames;
			DbConnection DBconnectYRS;
			DbTransaction DBTransaction;
			YMCARET.YmcaDataAccessObject.MonthlyProcessBOWraperClass ProcessPayroll = new YMCARET.YmcaDataAccessObject.MonthlyProcessBOWraperClass(); 
			bool l_bool_TransactionStarted  = false;
        
			try
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				
				DBconnectYRS = l_DataBase.CreateConnection();
				
				DBconnectYRS.Open ();

				// Set the Transaction process ON

				DBTransaction = DBconnectYRS.BeginTransaction (System.Data.IsolationLevel.ReadUncommitted);
				l_bool_TransactionStarted = true;
			
				try
				{
					if (l_DataBase == null) return false;
				
					l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_PAYROLL_GetExperienceDividendsData");
					l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
					
					if (l_DBCommandWrapper == null) return false;

                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@dateTimePayrollDate", DbType.DateTime, parameterdateTimePayrollDate);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@dateTimeCheckDate", DbType.DateTime, parameterDateCheckDate);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@bitProofReport", DbType.Boolean, parameterboolproof);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@ln_FirstCheckCANADA", DbType.Int32, parameterFirstCheckCanada);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@ln_FirstCheckUS", DbType.Int32, parameterFirstCheckUS);
                    l_DataBase.AddInParameter(l_DBCommandWrapper, "@CurrentExpUniqueiId", DbType.String, parameterCurrentExpUniqueiId);
					
					l_DataSet = new DataSet ("EXPDividendPayroll Data");
								
					l_TableNames = new string [] {"Payroll","FedTaxWithholdingRO", "Withholdings","YTDWithholdings","DisbursementFiles", "OutPutfilePath"};
				
					l_DataBase.LoadDataSet (l_DBCommandWrapper,l_DataSet, l_TableNames,DBTransaction);

					// Populate Batch NO for the EDI.

					ProcessPayroll.EDIBatchNO = RetrieveBatchNoforEDI(l_DataBase,DBTransaction,parameterboolproof);
                    ProcessPayroll.IATBatchNO = RetrieveBatchNoforIAT(l_DataBase, DBTransaction, parameterboolproof);

                    //New parameter 'parameterdateTimePayrollDate' passed to function to pass payroll date to IAT file
                    if (ProcessPayroll.PreparOutPutFileData(l_DataSet, parameterboolproof, parameterDateCheckDate, "EXP"))
					{
						if (parameterboolproof != true)
						{	
							// Insert the Disbursement file and the output files should be updated only when 
							// the output mode not in proof method mode.
							InsertDisbursementFileTypes(l_DataSet,l_DataBase,DBTransaction);
										
						}

						DBTransaction.Commit(); 
						DBconnectYRS.Close(); 

						l_datatable_FileList = ProcessPayroll.datatable_FileList;
						l_bool_TransactionStarted = false;
						return true;
					
					}
					else
					{
					
						DBTransaction.Rollback(); 
						DBconnectYRS.Close(); 
						l_bool_TransactionStarted = false;
						return false;

					}
                    									
				}
				catch (Exception ex)
				{
				
					if (l_bool_TransactionStarted) 
					{
						DBTransaction.Rollback(); 
						DBconnectYRS.Close(); 
					}
					throw new Exception(ex.Message); 

				}
			}
			catch
			{    
				throw;
			}
		
		}
		// EXP Dividends -Aparna Samala -03/11/2006
		public static DataSet getnextPayrollDayteForExp()
		{
			DataSet dsNextBusinessDay = null;
			Database db = null;
			DbCommand getCommandWrapper = null;

			try
			{
				db= DatabaseFactory.CreateDatabase("YRS");
			
				if (db == null) return null;

				getCommandWrapper = db.GetStoredProcCommand("dbo.yrs_usp_PM_GetEXPPeriodNextBusinessDay");
				
				if (getCommandWrapper == null) return null;

				//getCommandWrapper.AddInParameter("@date",DbType.Date,parameterGetNextBusinessDate);
						
				dsNextBusinessDay  = new DataSet();
				db.LoadDataSet(getCommandWrapper, dsNextBusinessDay ,"Next Business Day");
				
				return dsNextBusinessDay;
			}
			catch 
			{
				throw;
			}
		}

		public static int ValidateLastPayroll(DateTime p_ldate_SpecialDvidentDate)
		{
			DbCommand l_DBCommandWrapper;
			Database db = null;
			int l_int_count = 0;
			try
			{
				db = DatabaseFactory.CreateDatabase("YRS");
				if (db == null) return l_int_count;
				l_DBCommandWrapper = db.GetStoredProcCommand ("dbo.yrs_usp_Payroll_GetPayrollLast");
				
				l_DBCommandWrapper.CommandTimeout =  System.Convert.ToInt32 (System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return l_int_count;
				
				db.AddInParameter(l_DBCommandWrapper, "@dateTime_EXPDividend",DbType.DateTime,p_ldate_SpecialDvidentDate);
				db.AddOutParameter(l_DBCommandWrapper, "@int_count",DbType.Int32,2);
				db.ExecuteNonQuery(l_DBCommandWrapper);
                //TODOMIGRATION - Check if Convert to Int32 would work or not
				//l_int_count = Convert.ToInt32(l_DBCommandWrapper.GetParameterValue("@int_count"));
                l_int_count = Convert.ToInt32(db.GetParameterValue(l_DBCommandWrapper, "@int_count"));
				return l_int_count;
				
			}
			
			catch(Exception ex)
			{
				throw ex;
			}

		}

        //START: MMR | 2017.01.25 | YRS-AT-3288 | Added to get list of persons with missing and duplicate Address
        public static DataSet GetDuplicateMissingAddressInfo(DateTime checkDate)
        {
            DataSet duplicateMissingAddressInfo = null;
            Database db = null;
            DbCommand cmd = null;

            try
            {
                db = DatabaseFactory.CreateDatabase("YRS");

                if (db == null) return null;

                cmd = db.GetStoredProcCommand("dbo.yrs_usp_PAYROLL_ValidateAddressInfo");

                if (cmd == null) return null;

                db.AddInParameter(cmd, "@dateTimeCheckDate", DbType.Date, checkDate);

                duplicateMissingAddressInfo = new DataSet();
                db.LoadDataSet(cmd, duplicateMissingAddressInfo, "DuplicateMissingAddressInfo");

                return duplicateMissingAddressInfo;
            }
            catch
            {
                throw;
            }
        }
        //END: MMR | 2017.01.25 | YRS-AT-3288 | Added to get list of persons with missing and duplicate Address

        //START: SR | 2019.12.31 | YRS-AT-4602 | update export file status.
        public static void ChangeExportFileStatus(int processId, string exportStatus, Database dataBase, DbTransaction dbTransaction)
        {           
            DbCommand dbCommandWrapper = null;
            try
            {
                dataBase = DatabaseFactory.CreateDatabase("YRS");
                if (dataBase != null)
                {
                    dbCommandWrapper = dataBase.GetStoredProcCommand("dbo.yrs_usp_STW_Export_ModifyStatus");
                    dbCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                    if (dbCommandWrapper != null)
                    {
                        dataBase.AddInParameter(dbCommandWrapper, "@INT_ProcessId", DbType.Int32, processId);
                        dataBase.AddInParameter(dbCommandWrapper, "@VARCHAR_Status", DbType.String, exportStatus);
                        dataBase.ExecuteNonQuery(dbCommandWrapper, dbTransaction);
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        public static bool SetPaymentOutsourcingKey(DateTime CheckDate)
        {
            DataSet ConfigKey = new DataSet() ;
            DateTime paymentOutsourcingGoliveDate;
            bool isPaymentOutSourcingKeyON  = false;
            
            try
            {
                ConfigKey = YMCARET.YmcaDataAccessObject.MetaConfigMaintenanceDAClass.SearchConfigurationMaintenance("PAYMENT_OUTSOURCING_START_DATE");
                if (HelperFunctions.isNonEmpty(ConfigKey))
                {
                    paymentOutsourcingGoliveDate = Convert.ToDateTime(ConfigKey.Tables[0].Rows[0]["Value"].ToString().Trim());
                    if (CheckDate >= paymentOutsourcingGoliveDate)
                        isPaymentOutSourcingKeyON =  true;                   
                }
                return isPaymentOutSourcingKeyON;
            }
            catch {
                throw;
            }
          }
        //END: SR | 2019.12.31 | YRS-AT-4602 | update export file status.
        
        // START : SR | 2020.01.13 | YRS-AT-4641 | YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 
        public static void SetStateTaxWithholdingForPayrollProcess(Boolean proofReport, DateTime checkDate, Database dataBase, DbTransaction dbTransaction)
        {
            DbCommand dbCommandStateTaxWithholding = null;
            try
            {
                //dataBase = DatabaseFactory.CreateDatabase("YRS");
                if (dataBase != null)
                {
                    dbCommandStateTaxWithholding = dataBase.GetStoredProcCommand("dbo.yrs_usp_STW_PAYROLL_SetStateTaxWithholding");
                    dbCommandStateTaxWithholding.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                    if (dbCommandStateTaxWithholding != null)
                    {
                        dataBase.AddInParameter(dbCommandStateTaxWithholding, "@BIT_ProofReport", DbType.Boolean, proofReport);
                        dataBase.AddInParameter(dbCommandStateTaxWithholding, "@Date_CheckDate", DbType.DateTime, checkDate);
                        dataBase.ExecuteNonQuery(dbCommandStateTaxWithholding, dbTransaction);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void UpdatePayrollCheckSeries(Boolean proofReport, DateTime checkDate, Database dataBase, DbTransaction dbTransaction)
        {
            DbCommand dbCheckSeries = null;
            try
            {
                dataBase = DatabaseFactory.CreateDatabase("YRS");
                if (dataBase != null)
                {
                    dbCheckSeries = dataBase.GetStoredProcCommand("dbo.yrs_usp_STW_PAYROLL_UpdateCheckSeries");
                    dbCheckSeries.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                    if (dbCheckSeries != null)
                    {
                        dataBase.AddInParameter(dbCheckSeries, "@BIT_ProofReport", DbType.Boolean, proofReport);
                        dataBase.AddInParameter(dbCheckSeries, "@Date_CheckDate", DbType.Date, checkDate);
                        dataBase.ExecuteNonQuery(dbCheckSeries, dbTransaction);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static void UpdatePayrollEFTFundingRecords(Boolean proofReport, DateTime checkDate, Database dataBase, DbTransaction dbTransaction)
        {
            DbCommand dbFundingRecords = null;
            try
            {
                dataBase = DatabaseFactory.CreateDatabase("YRS");
                if (dataBase != null)
                {
                    dbFundingRecords = dataBase.GetStoredProcCommand("dbo.yrs_usp_STW_PAYROLL_UpdateFundingRecords");
                    dbFundingRecords.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["LargeConnectionTimeOut"]);
                    if (dbFundingRecords != null)
                    {
                        dataBase.AddInParameter(dbFundingRecords, "@BIT_ProofReport", DbType.Boolean, proofReport);
                        dataBase.AddInParameter(dbFundingRecords, "@Date_CheckDate", DbType.Date, checkDate);
                        dataBase.ExecuteNonQuery(dbFundingRecords, dbTransaction);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        // END  : SR | 2020.01.13 | YRS-AT-4641 | YRS REPORTS: State Tax Withholding- New screen for "Monthly Annuity Payroll Processing" (Reverse Feed) 


	}

	class MonthlyProcessBOWraperClass
	{
		// Payee data varaibles
		#region "Constants"
		// Constants fro OUTPUT.H file.
		const string 	C_BATCHCOUNT		=	"000001";
		//const string	C_BATCHNUMBER		=	"0000001";
		const string	C_BLOCKINGFACTOR	=	"10";
		const string	C_COMPANYENTRYDESC	=	"ANNUITY_PY";
		const string	C_COMPANYENTRYDESC_SPDIVIDEND	=	"SpDividend";
		const string	C_COMPANYID			=	"1135562401";
		const string	C_COMPANYNAME		=	"YMCA Retirement Fund             ";
		const string	C_FILEFORMATCODE	=	"1";
		const string	C_FILEIDMODIFIER	=	"A";
		const string	C_IMMEDIATEDEST		=	" 071000152";
		const string	C_IMMEDIATEDESTNAME	=	"Northern Trust Company ";
		const string	C_IMMEDIATEORIGIN	=	"1135562401";
		const string	C_IMMEDORIGINNAME	=	"YMCA Retirement Fund   ";
		const string	C_ORIGINATINGDFIID	=	"07100015";
		const string	C_ORIGINATORSTATUSCODE	= "1";
		const string	C_PRIORITYCODE		=	"01";
		const string	C_RECORDSIZE		=	"094";
		const string	C_ROWCODE1			=	"1";
		const string	C_ROWCODE5			=	"5";
		const string	C_ROWCODE8			=	"8";
		const string	C_ROWCODE9			=	"9";
		const string	C_ROWCODEH			=	"H";
		const string	C_SERVICECLASSCODE	=	"200";
		const string	C_STANDARDECCODE	=	"PPD";
        //const removed by prasad
	     string	C_COMPANYNAMESHORT	=	"YMCA Retire Fund";
		const string	C_ADDENDAINDICATOR	=	"0";
		const string	C_ROWCODE6			=	"6";
		const string	C_ROWCODEA			=	"A";
		const string	C_ROWCODEB			=	"B";
		const string	C_ROWCODER			=	"R";
		const string	C_ROWCODET			=	"T";
		const string	C_TRACENUMBER		=	"000000000000000";
		const string	C_TRANSACTIONCODE	=	"SA";
		const string	C_YMCABANKACCOUNT	=	"0030362081";
		const string	C_ROWCODE7_ADDENDA	=	"7";
		const string	C_ADDENDATYPE_ADDENDA =	"5";
		const string	C_DEDTEXT			=	"Ded       ";
		//const string	C_EXPDIVTEXT		=	"Experience Dividend - Taxable      ";
		const string	C_EXPDIVTEXT		=	"Special Dividend:                  ";
		const string	C_GROSSTEXT			=	"Gross     ";
		const string	C_NETTEXT			=	"Net       ";
		const string	C_NONTAXTEXT		=	"Non-Taxable     ";
		const string	C_REGALLOWTAXTEXT	=	"Regular Allowance: Taxable         ";
		const string	C_ROWCODEE			=	"E";
		const string	C_YTDDEDTEXT		=	"YTD Ded   ";
		const string	C_YTDGROSSTEXT		=	"YTD Gross ";
		const string	C_YTDNETTEXT		=	"YTD Net   ";
		const string	C_ROWCODED			=	"D";
		const string	C_ROWCODEC			=	"C";
		const string	C_ROWCODEF			=	"F";
		const string	C_ROWCODEG			=	"G";
		const string    C_POSPAY_EFT_LINE1	=	"$$ADD ID=YMCRE1B BID='9902827 A";
		const string    C_POSPAY_LINE1_SUFFIX =	"RPI'";
		const string    C_EFT_LINE1_SUFFIX	=	"CHI'";

		const string EDI_END_OF_LINE = "[";
		const string EDI_FIELD_DELIMITER = "^";
		const string EDI_PAYOR_BANK_ABA_NUMBER = "071923828";
		const string EDI_PAYOR_BANK_ACCOUNT_NUMBER = "30362081";
		const string EDI_CONFIGURATION_CATEGORY_CODE = "EDIISA";
		// For the ISA Line
		const string EDI_INTERCHANGE_CONTROL_NUMBER_FORMAT = "000000000";
		// From the First GS Line
		const string EDI_APPLICATION_SENDER_CODE = "901234572000"; 
		const string EDI_APPLICATION_RECEIVER_CODE = "908887732000";
		// For the First ST/SE Line
		const string EDI_ST_LINE_TRANSACTION_NUMBER_FORMAT = "0000";
		// For the Second ST/SE Line
		const string EDI_ST_LINE_TRANSACTION_NUMBER_LONG_FORMAT = "000000000";
		// For the BPR Line
		const string EDI_NETPAY_CURRENCY_FORMAT = "0.00";
		const string EDI_CHECK_DATE_FORMAT = "yyyyMMdd";
		// For the RMR Lines
		const string EDI_CURRENCY_FORMAT = "0.00";

		// Constants for identifying the check detail file.
		const int CSDetail  = 1;
		const int PPDetail  = 2;
		const int EFTDetail = 3;

		#endregion "Constants"

		#region "Members"

		string 	C_BATCHNUMBER		=	"0000001";
		public string  EDIBatchNO		
		{
			get
			{
				return C_BATCHNUMBER;
			}
			set
			{
				C_BATCHNUMBER = value;
				C_BATCHNUMBER = C_BATCHNUMBER.Trim().PadLeft(7,'0');
			}

		}

        int C_IAT_BATCHNUMBER = 1;
        public int IATBatchNO
		{
			get
			{
				return C_IAT_BATCHNUMBER;
			}
			set
			{
				C_IAT_BATCHNUMBER = value;
			}

		}

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
			String_BankAbaNumber,
            //Added by prasad
            String_Test_Production;

		//For use with EDI generation
		string String_City, String_State, String_ZIP, String_Country, 
			String_DBAddress2, String_DBAddress3,
			String_DBFirstName, String_DBMiddleName, String_DBLastName;
		DateTime DateTime_currentDateTime;
		TruncateFormatter myFormatter = new TruncateFormatter();
		DataSet DataSet_ExclusionList = null; 
		bool bool_ExcludeThisPersonFromEDI = false;

		// PaymentData_Addenda, EntryDetailSeqNum_Addenda, AddendaSeqNum_Addenda,
		DateTime CheckDate,DisbursementDate,dateTimePayrollMonth;

		double double_NetPayMonthTotal,TotalMonthlyNet;
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
			double_WithholdingMonthAddl,
			double_GrossPayDividentMonthTotal, 
			double_GrossPayDividentYTDNontaxable;


		double double_OutputFilesEFTDetailSum,	double_OutputFileEFTDetailCount;
		double double_OutputFilesPPDetailCount; 
		double int32_OutputFileEFTDetailHash;
		
		string 	String_OutputFileCSCanadianGuid,String_OutputFileCSUSGuid,String_OutputFilePosPayGuid,String_OutputFileEFT_NorTrustGuid;
		string String_OutputFileNTPYRLGuid;// ML : 2019.12.26 | YRS-AT-4601 | Declare output file guid
				
		//PayeePayrollWithholding varables
	        		
		double double_WithholdingMonthDetail,double_WithholdingYTDDetail;
		string String_WithholdingDetailDesc; 

		string String_OutputFileType;

		double double_Exemptions,double_OutputFilesPPDetailSum;
		//Int32 OutputFileFormatCurrency,	OutputFilesPPDetailSum,lcOutputCursor,;

		bool bool_OutputFilePayRoll,bool_OutputFilePosPay,bool_OutputFileCSUS,bool_OutputFileCSCanadian,bool_OutputFileEFT_NorTrust;
		bool bool_OutputFileEDI_US;
        bool outPutFileNTPyrl; // SR | 2020.01.09 | YRS-AT-4602 | define flag for creating export change file for NT bank.

		//OutputFileOutputFileRefund,OutputFileRefund,
			    
		StreamWriter StreamOutputFileCSCanadian,StreamOutputFileCSUS,StreamOutputFilePosPay,StreamOutputFileEFT_NorTrust;
		StreamWriter StreamOutputFileEDI_US; //NP added to support EDI_US
        StreamWriter StreamOutputFileNTPYRL; //ML | 2019.12.19 | YRS-AT-4602 | Declare Variable
		StreamWriter StreamOutputFileCSCanadianBack,StreamOutputFileCSUSBack,StreamOutputFilePosPayBack,StreamOutputFileEFT_NorTrustBack;
		StreamWriter StreamOutputFileEDI_USBack; //NP added to support EDI_US
        StreamWriter StreamOutputFileNTPYRLBack; //ML | 2019.12.19 | YRS-AT-4602 | Declare Variable
		bool ProofReport;
			
		private DataTable l_datatable_FileList;

		#region "EDI counters"
		// Variables to keep track of Transaction numbers and other stuff related to EDI
		//private double double_checkTotal = 0;		//This variable is no longer used to keep running totals	// Net amount of credits and debits for all the detail blocks. This figure is sent out in the summary block
		private int i_numberOfFunctionalBlocks = 0;	// Number of functional detail blocks. These equal to 2 in our case. One is the Details block and the other is the summary block.
		private int i_numberOfDetailBlocks = 0;		// Total number of detail blocks. There is one detail block per record that is outputted. This figure is used in the summary block.
		private int i_numberOfSegmentsInDetailBlock = 0; // Number of segments in a detail block. This is needed in the trailing SE of each transaction.
		private double double_checkTotal = 0;		// Net of all the payments made in this EDI document
		//	We are not keeping running totals for these variables. Instead they are 
		//	being printed directly from their base variables
		//		private double double_currentWithholdingTotal = 0; // Net amount of withholdings for all people
		//		private double double_ytdWithholdingTotal = 0; // YTD amount of withholdings for all people
		//		private double double_currentGrossAllowance = 0; // Current Gross allowance for all people
		//		private double double_ytdGrossAllowance = 0;	// YTD amount of Gross allowance for all people
		//		private double double_currentNetPaymentAmount = 0; // Current amount of Net payment for all people
		//		private double double_ytdNetPaymentAmount = 0;	// YTD amount of Net payment for all people
		#endregion "EDI counters"

        // START : ML | 2019.10.22 | YRS-AT-4601 |  Defined property to check Payment Outsourcing is live or not
        bool isPaymentOutSourcingKeyON = true;
        public bool IsPaymentOutSourcingKeyON
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
		#endregion "Members"

		// Here we initialize variables that need to be reset for each participant 
		// or payroll entry that is retrieved from the database.
		private void initializeEDIVariables() 
		{
			//NP - Added code to initialize the values used in EDI generation
			i_numberOfSegmentsInDetailBlock = 0;
			this.String_City = this.String_State = this.String_ZIP = this.String_Country = string.Empty;
			this.String_DBAddress2 = this.String_DBAddress3 = string.Empty;
			this.String_DBFirstName = this.String_DBMiddleName = this.String_DBLastName = string.Empty;
		}


		public MonthlyProcessBOWraperClass()
			{
				//
				// TODO: Add constructor logic here
				//
				this.String_OutputFileCSCanadianGuid = "";
				this.String_OutputFileCSUSGuid = "";
				this.String_OutputFilePosPayGuid = "";
				this.String_OutputFileEFT_NorTrustGuid = "";
				this.int32_OutputFileEFTDetailHash = 0;
				this.double_OutputFilesPPDetailSum = 0;
				this.double_OutputFilesPPDetailCount = 0;
                this.String_OutputFileNTPYRLGuid = "";//ML | 2019.12.19 | YRS-AT-4602 | Assign Variable

				this.l_datatable_FileList = new DataTable();
				DataColumn SourceFolder = new DataColumn ("SourceFolder",typeof(String));
				DataColumn SourceFile = new DataColumn ("SourceFile", typeof(String));
				DataColumn DestFolder = new DataColumn ("DestFolder", typeof(String));
				DataColumn DestFile = new DataColumn ("DestFile", typeof(String));

				this.l_datatable_FileList.Columns.Add(SourceFolder);
				this.l_datatable_FileList.Columns.Add(SourceFile);
				this.l_datatable_FileList.Columns.Add(DestFolder);
				this.l_datatable_FileList.Columns.Add(DestFile);
                //Added by prasad YRS 5.0-632 : Test database output files need word "test" in them.
               DataSet  ds = getEDIModeFromDatabase();

                DataRow[] dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_15'");
                if (dr.Length == 0 || dr[0].IsNull("Value"))
                {
                    throw new Exception("The value for the key EDI_ISA_15 needs to be defined in the Configuration table");
                }
                switch (dr[0]["Value"].ToString().ToUpper())
                {
                    case "TEST":
                        String_Test_Production = "TEST";
                        break;
                    case "PRODUCTION":
                        String_Test_Production = "PRODUCTION";
                        break;
                    default:
                        throw new Exception("The value for the key EDI_ISA_15 is not a valid value.");

                }
			}

		private void ProduceOutputFilesCreateFooters()
		{
			//string lcOutputPPBatchDate,lcCompanyDate;
			string l_String_temp;
			//string l_String_temp1;
			string l_String_Output;
			NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
			nfi.CurrencyGroupSeparator ="";
			nfi.CurrencySymbol=""; 
			nfi.CurrencyDecimalDigits = 2;

			try
			{
				Int64 l_long_Blocks; 
				// Gets a NumberFormatInfo associated with the en-US culture.
				l_String_Output ="";
		
				if (this.bool_OutputFileEFT_NorTrust)
				{
					this.String_OutputFileType = "EFT";

					l_String_Output  = C_ROWCODE8 + C_SERVICECLASSCODE;
					l_String_Output += this.double_OutputFileEFTDetailCount.ToString().Trim().PadLeft(6,'0');    
					l_String_temp = this.int32_OutputFileEFTDetailHash.ToString().Trim().PadLeft(20,'0');
					l_String_Output += l_String_temp.Substring(10,10).ToString();
					l_String_temp = "";
					l_String_Output +=l_String_temp.PadRight(12,'0');
	         
					l_String_Output+=  this.double_OutputFilesEFTDetailSum.ToString("C",nfi).Trim().PadLeft(13,'0').Remove(10,1); 
					l_String_Output+=C_COMPANYID;
					l_String_Output += l_String_temp.PadRight(25,' ');
					l_String_Output += C_ORIGINATINGDFIID +	C_BATCHNUMBER;
							
					WriteOutputFilesDataLineIntoStream(l_String_Output);
						
					//Insert.

					l_String_Output = C_ROWCODE9 + C_BATCHCOUNT;
					l_long_Blocks = (long)this.double_OutputFileEFTDetailCount + 4;
					l_long_Blocks *=94;
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
							
					l_String_Output += l_long_Blocks.ToString().Trim().PadLeft(6,'0');   
					l_String_Output += this.double_OutputFileEFTDetailCount.ToString().Trim().PadLeft(8,'0');
							
					//l_String_Output += this.int32_OutputFileEFTDetailHash.ToString().Trim().PadLeft(10,'0');
					l_String_temp ="";

					l_String_temp = this.int32_OutputFileEFTDetailHash.ToString().Trim().PadLeft(20,'0');
					l_String_Output += l_String_temp.Substring(10,10).ToString();
					l_String_temp ="";

					l_String_Output +=l_String_temp.PadRight(12,'0'); 

					l_String_Output+=  this.double_OutputFilesEFTDetailSum.ToString("C",nfi).Trim().PadLeft(13,'0').Remove(10,1); 
					l_String_temp ="";
					l_String_Output += l_String_temp.PadRight(39,' ');
						
					WriteOutputFilesDataLineIntoStream(l_String_Output);
					//Insert
				}

				if (this.bool_OutputFilePosPay) 
				{
					this.String_OutputFileType = "PP";
					l_String_Output= C_ROWCODET ;
					l_String_temp =C_YMCABANKACCOUNT;
					l_String_Output += l_String_temp.Trim().PadLeft(10,'0');
					l_String_Output += this.double_OutputFilesPPDetailCount.ToString().PadLeft(10,'0');
					l_String_Output += this.double_OutputFilesPPDetailSum.ToString("C",nfi).Trim().PadLeft(14,'0').Remove(11,1); 
					l_String_temp ="";
					l_String_Output +=l_String_temp.PadRight(46,' ');
					WriteOutputFilesDataLineIntoStream(l_String_Output);
					//Insert
				}

				if (this.bool_OutputFileEDI_US)
				{
					l_String_Output = this.writeGEForDetailFunctionalGroup();
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeGSForSummaryFunctionalGroup(this.DateTime_currentDateTime);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeSTLineForSummary(1);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					//ASK_C: What is the reference information here
					l_String_Output = this.writeBGNLineForSummary("0012345", this.DateTime_currentDateTime);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeN9LineForSummary(C_BATCHNUMBER);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					//Removing to comply with the new specification for AMT
					l_String_Output = this.writeAMTLineForSummary(this.double_checkTotal);
					//					l_String_Output = this.writeAMTLineForSummary(this.double_currentNetPaymentAmount, this.double_ytdNetPaymentAmount,
					//																	this.double_currentGrossAllowance, this.double_ytdGrossAllowance,
					//																	this.double_currentWithholdingTotal, this.double_ytdWithholdingTotal);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeQTYLineForSummary(this.i_numberOfDetailBlocks);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeSELineForSummary(this.i_numberOfSegmentsInDetailBlock, 1);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeGELineForSummary();
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = this.writeIEALineForSummary(this.i_numberOfFunctionalBlocks, 1); //1 is the interchange control number. Since it is the first and only interchange in the document
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					
				}

			}
			catch
			{
				throw;
			}

							
		}

		private void ProduceOutputFilesCreateHeader()
		{
			string l_String_Output;
			string l_String_temp,l_String_OutputPPBatchDate,l_String_lcEffectiveEntryDate,l_String_lcCompanyDate;
			string lc_year,lc_month,lc_day;
			DateTime ld_date;
			string l_String_temp1;

			try
			{		
				l_String_temp1 = l_String_temp = lc_year = lc_month =lc_day = l_String_OutputPPBatchDate="";
				ld_date = this.DateTime_currentDateTime;
				l_String_OutputPPBatchDate = ld_date.ToString("MMddyy");
				
				l_String_lcEffectiveEntryDate = this.dateTimePayrollMonth.ToString("yyMMdd");

				//l_String_lcCompanyDate = ld_date.ToString("MMM") +  "'" + ld_date.ToString("yy");   

				// #YREN-2569
				l_String_temp =this.dateTimePayrollMonth.ToString("MMM").ToUpper().Substring(0,3) ;

				if (l_String_temp   == "MAY") 
				{
					l_String_temp1 = "MAY ";
				}
				else if (l_String_temp   == "JUN")
				{
					l_String_temp1 = "JUNE";
				}
				else if (l_String_temp   == "JUL")
				{
					l_String_temp1 = "JULY";
				}
				else
				{
					l_String_temp1 = l_String_temp + "'";

				}

				l_String_lcCompanyDate = l_String_temp1  + this.dateTimePayrollMonth.ToString("yy");   

				// #YREN-2569

				if (this.bool_OutputFilePosPay)
				{
					this.String_OutputFileType = "PP";
					l_String_Output = C_POSPAY_EFT_LINE1 + C_POSPAY_LINE1_SUFFIX;

					WriteOutputFilesDataLineIntoStream(l_String_Output);

					//Insert
					l_String_Output = C_ROWCODEH;
					l_String_temp = C_COMPANYNAME;
					l_String_temp = l_String_temp.Trim().PadRight(33,' ');  
					//l_String_OutputPPBatchDate
					l_String_Output += l_String_temp;
					l_String_Output += l_String_OutputPPBatchDate;
					l_String_temp = "";
					l_String_Output += l_String_temp.PadRight(40,' ');
					WriteOutputFilesDataLineIntoStream(l_String_Output);
					//Insert
				
				}

				if (this.bool_OutputFileEFT_NorTrust) 
				{
					this.String_OutputFileType = "EFT";
					l_String_Output = C_ROWCODE1 + C_PRIORITYCODE + C_IMMEDIATEDEST + 	C_IMMEDIATEORIGIN;
					l_String_Output += ld_date.ToString("yyMMdd");
					l_String_Output += ld_date.ToString("hhmm");
					l_String_Output += C_FILEIDMODIFIER + C_RECORDSIZE + C_BLOCKINGFACTOR;
                    //Added by prasad for YRS 5.0-632 : Test database output files need word "test" in them.
                    if (String_Test_Production == "TEST")
                    {
                        l_String_Output += C_FILEFORMATCODE + "TEST" + C_IMMEDIATEDESTNAME + C_IMMEDORIGINNAME;
                    }
                    else if (String_Test_Production == "PRODUCTION")
                    {
                        l_String_Output += C_FILEFORMATCODE + C_IMMEDIATEDESTNAME + C_IMMEDORIGINNAME;
                    }
					l_String_temp = "";
					l_String_Output += l_String_temp.PadRight(8,' ');
                    if (String_Test_Production == "TEST")
                    {
                        l_String_Output = l_String_Output.Substring(0, 94);
                    }
					WriteOutputFilesDataLineIntoStream(l_String_Output);
					//Insert

                    // Added by prasad for YRS 5.0-632 : Test database output files need word "test" in them.
                    // Added by prasad for YRS 5.0-632 : Test database output files need word "test" in them.(reopen).
                    if (String_Test_Production == "TEST")
                    {
                        C_COMPANYNAMESHORT = "YMCA Reti1234567890re Fund";
                    }
					l_String_Output =	C_ROWCODE5 + C_SERVICECLASSCODE +	C_COMPANYNAMESHORT;
					l_String_temp = "";
					l_String_Output += l_String_temp.PadRight(20,' '); 

					if (this.String_DisbursementType.ToString() == "EXP") 
					{
						l_String_Output += C_COMPANYID + C_STANDARDECCODE + C_COMPANYENTRYDESC_SPDIVIDEND;
					}
					else
					{
						l_String_Output += C_COMPANYID + C_STANDARDECCODE + C_COMPANYENTRYDESC;
					}

					l_String_Output += l_String_lcCompanyDate;
					l_String_Output += l_String_lcEffectiveEntryDate;
					l_String_temp = "";
					l_String_Output += l_String_temp.PadRight(3,' '); 
					l_String_Output +=	C_ORIGINATORSTATUSCODE + C_ORIGINATINGDFIID + C_BATCHNUMBER;
                    // Added by prasad for YRS 5.0-632 : Test database output files need word "test" in them.
                    if (String_Test_Production == "TEST")
                    {
                        l_String_Output = l_String_Output.Substring(0, 94);
                    }
					WriteOutputFilesDataLineIntoStream(l_String_Output);

					//Insert
	 
				}

				if (this.bool_OutputFileEDI_US) 
				{
					// We have to print ISA and GS Lines in the header
					l_String_Output = writeISALine(this.DateTime_currentDateTime, 1, "053439083test  ") ; // 1 = Interchange control number. This is first and only one
					//Write to output stream
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
					l_String_Output = writeGSLineForDetailFunctionalGroup(this.DateTime_currentDateTime);
					//Write to output stream
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
				}

			}
			catch
			{
				throw;
			}
	            
		}

		#region EDI Printing helpers
		//Make sure that any data that comes in as a parameter fits the field size 
		//specifications of the corresponding EDI parameter

		//		Write the ISA line for the EDI document
		private string writeISALine(DateTime dt, int interchangeControlNumber, string interchangeReceiverId) 
		{
			
			DataSet ds;  	
			string stringISA07 ,stringISA08,stringISA15;
			stringISA07 = string.Empty;
			stringISA08 = string.Empty;
			stringISA15 = string.Empty;

			try
			{
				ds  = getEDIModeFromDatabase();			

				DataRow[] dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_15'");
				if (dr.Length == 0 || dr[0].IsNull("Value") ) 
				{
					throw new Exception("The value for the key EDI_ISA_15 needs to be defined in the Configuration table");
				}
				switch (dr[0]["Value"].ToString().ToUpper()) 
				{
					case "TEST": 
						stringISA15=  "T";
						break;
					case "PRODUCTION": 
						stringISA15 = "P";
						break;
					default: 
						throw new Exception("The value for the key EDI_ISA_15 is not a valid value.");
						
				}

				dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_07'");
				if (dr.Length == 0 || dr[0].IsNull("Value") ) 
				{
					throw new Exception("The value for the key EDI_ISA_07 needs to be defined in the Configuration table");
				}
				else
				{
					stringISA07 = dr[0]["Value"].ToString();
				}

				dr = ds.Tables[0].Select("[Key]=" + "'EDI_ISA_08'");
				if (dr.Length == 0 || dr[0].IsNull("Value") ) 
				{
					throw new Exception("The value for the key EDI_ISA_08 needs to be defined in the Configuration table");
				}
				else
				{
					stringISA08 = dr[0]["Value"].ToString();
				}


						
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("ISA"); sb.Append(EDI_FIELD_DELIMITER); // Control Type ISA - Length 3
			sb.Append("00"); sb.Append(EDI_FIELD_DELIMITER);  // Authorization Information Qualifier - Length 2
			sb.Append("          ".PadRight(10, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Authorization Information - Length 10
			sb.Append("00"); sb.Append(EDI_FIELD_DELIMITER);  // Security Information Qualifier - Length 2
			sb.Append("          ".PadRight(10, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Security Information Qualifier - Length 10
			sb.Append("01"); sb.Append(EDI_FIELD_DELIMITER);  // Interchange ID Qualifier - Length 2
			sb.Append("YMCA RETIREMENT".PadRight(15, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Interchange Sender ID - Length 15
			//sb.Append("16"); sb.Append(EDI_FIELD_DELIMITER);  // Interchange ID Qualifier - Length 2
			sb.Append(stringISA07.Trim()); sb.Append(EDI_FIELD_DELIMITER);  // Interchange ID Qualifier - Length 2
			//sb.Append("053439083test  ".PadRight(15, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Interchange Receiver ID - Length 15
			sb.Append(stringISA08.Trim().PadRight(15, ' ')); sb.Append(EDI_FIELD_DELIMITER); // Interchange Receiver ID - Length 15

			sb.Append(dt.ToString("yyMMdd")); sb.Append(EDI_FIELD_DELIMITER);// Interchange Date - Format YYMMDD - Length 6
			sb.Append(dt.ToString("HHmm")); sb.Append(EDI_FIELD_DELIMITER);  // Interchange Time - Format HHMM - Length 4
			sb.Append("U");	sb.Append(EDI_FIELD_DELIMITER);     // Interchange Control Standards Identifier - Length 1
			sb.Append("00200"); sb.Append(EDI_FIELD_DELIMITER); // Interchange Control Version Number - Length 5
			sb.Append(interchangeControlNumber.ToString(EDI_INTERCHANGE_CONTROL_NUMBER_FORMAT));	sb.Append(EDI_FIELD_DELIMITER); // Interchange Control Number - Length 9
			sb.Append("0");	sb.Append(EDI_FIELD_DELIMITER);		// Acknowledgement requested - Length 1
			//NP: 2007.04.26 - This value should come from the atsMetaConfiguration table with a key value of 'EDI_MODE'
			//sb.Append(getEDIModeFromDatabase());	sb.Append(EDI_FIELD_DELIMITER);		// Usage indicator - Length 1 - T or P indicating whether to be used in Test mode or Production mode
			sb.Append(stringISA15.Trim());	sb.Append(EDI_FIELD_DELIMITER);		// Usage indicator - Length 1 - T or P indicating whether to be used in Test mode or Production mode
			sb.Append("~");	sb.Append(EDI_END_OF_LINE);			// Component Element separator - Length 1
			return sb.ToString();

			}
			catch
			{ 
				throw;
				
			}
		}

		//		Write the GS line for the Details Functional Group
		private string writeGSLineForDetailFunctionalGroup(DateTime dt) 
		{ 
			this.i_numberOfFunctionalBlocks++;	// Increment the count for number of Functional Blocks
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("GS"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("RA"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_SENDER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_RECEIVER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(dt.ToString("yyyyMMdd")); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append(dt.ToString("HHmm")); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("1"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("T"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("004010"); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
						
						
		string _cache_STLine = string.Empty;
		private string writeSTLine(int transactionNumber) 
		{ 
			if (_cache_STLine == string.Empty) 
			{
				_cache_STLine += "ST" + EDI_FIELD_DELIMITER;
				_cache_STLine += "820" + EDI_FIELD_DELIMITER;
				_cache_STLine += "{0}" + EDI_END_OF_LINE;
			}
			string data = string.Empty ;
			this.i_numberOfSegmentsInDetailBlock = 1;	// Set number of Lines printed in current detail block to 1 as this is the first line in the current detail block
			data = string.Format(_cache_STLine, transactionNumber.ToString(EDI_ST_LINE_TRANSACTION_NUMBER_FORMAT));
			return data;
		}
		//		Write the BPR record
		private string writeBPRLine(double netPayMonthTotal, DateTime checkDate ) 
		{
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			this.double_checkTotal += double.Parse(netPayMonthTotal.ToString("0.00")); 	//Update the running count for the checks issued
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("BPR"); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append("D"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(netPayMonthTotal.ToString(EDI_NETPAY_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("C"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("YMC"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("PBC"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("01"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_PAYOR_BANK_ABA_NUMBER); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("DA"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_PAYOR_BANK_ACCOUNT_NUMBER); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(checkDate.ToString(EDI_CHECK_DATE_FORMAT)); sb.Append(EDI_END_OF_LINE); // Format YYYYMMDD
			return sb.ToString();
		}
		//		Write the TRN record
		string _cache_TRNLine = string.Empty;
		private string writeTRNLine(string chequeNumber) 
		{ 
			if (_cache_TRNLine == string.Empty) 
			{
				_cache_TRNLine += "TRN" + EDI_FIELD_DELIMITER;
				_cache_TRNLine += "1" + EDI_FIELD_DELIMITER;
				_cache_TRNLine += "{0:T30}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_TRNLine, chequeNumber.Trim());
			return data;
		}
		//		Write the REF record for Batch Number
		string _cache_REFBatchNumberLine = string.Empty;
		private string writeREFBatchNumberLine(string batchNumber) 
		{ 
			if (_cache_REFBatchNumberLine==string.Empty) 
			{
				_cache_REFBatchNumberLine += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFBatchNumberLine += "BT" + EDI_FIELD_DELIMITER;
				_cache_REFBatchNumberLine += "{0:T30}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_REFBatchNumberLine, batchNumber );
			return data;
		}
		//		Write the REF record for Mailing information
        string _cache_REFMailingInformationLines = string.Empty;
		//NP: Updated to include mail handling information FCM
		private string writeREFMailingInformationLines() 
		{ 
			if (_cache_REFMailingInformationLines == string.Empty) 
			{
				_cache_REFMailingInformationLines += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFMailingInformationLines += "W9" + EDI_FIELD_DELIMITER;
				//NP: Adding the constant mail handling code FCM to the output              
                _cache_REFMailingInformationLines += "FCM" + EDI_FIELD_DELIMITER;
				_cache_REFMailingInformationLines += "000" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(_cache_REFMailingInformationLines);
			return data;
		}
        //Start - Manthan Rajguru | 2016.03.31 | YRS-AT-2181 | Added method to write code REF^W9^IAM^000[ for person with Non US Address
        string _cache_REFMailingInformationLinesNonUS = string.Empty;
        private string writeREFMailingInformationLinesForNonUS()
        {
            if (_cache_REFMailingInformationLinesNonUS == string.Empty)
            {
                _cache_REFMailingInformationLinesNonUS += "REF" + EDI_FIELD_DELIMITER;
                _cache_REFMailingInformationLinesNonUS += "W9" + EDI_FIELD_DELIMITER;
                _cache_REFMailingInformationLinesNonUS += "IAM" + EDI_FIELD_DELIMITER;
                _cache_REFMailingInformationLinesNonUS += "000" + EDI_END_OF_LINE;
            }
            this.i_numberOfSegmentsInDetailBlock++;
            return string.Format(_cache_REFMailingInformationLinesNonUS);
        }
        //End - Manthan Rajguru | 2016.03.31 | YRS-AT-2181 | Added method to write code REF^W9^IAM^000[ for person with Non US Address

		//		Write the REF record for Fund Identification number        
        string _cache_REFFundId = string.Empty;
		private string writeREFFundIdentificationInformationLine(string fundIdNo) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			if (_cache_REFFundId == string.Empty) 
			{
				_cache_REFFundId  += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFFundId += "FU" + EDI_FIELD_DELIMITER;
				_cache_REFFundId += "{0:T30}" + EDI_END_OF_LINE;
			}
			string data = string.Format(this.myFormatter, _cache_REFFundId, fundIdNo ) ;
			return data;
		}
		//		Write the REF record for Accounting status
		string _cache_REFAccountStatus = string.Empty;
		private string writeREFAccountingStatusLine(string filingStatusExemptions) 
		{ 
			if (_cache_REFAccountStatus == string.Empty) 
			{
				_cache_REFAccountStatus += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFAccountStatus += "ACC" + EDI_FIELD_DELIMITER;
				_cache_REFAccountStatus += "{0:T30}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_REFAccountStatus , filingStatusExemptions );
			return data;
		}
		//		Write the REF record for Taxing Authority Identification number
		string _cache_REFTaxingAuthority = string.Empty;
		private string writeREFTaxingAuthorityIdentificationNumberLine(double amount) 
		{ 
			if (_cache_REFTaxingAuthority == string.Empty) 
			{
				_cache_REFTaxingAuthority += "REF" + EDI_FIELD_DELIMITER;
				_cache_REFTaxingAuthority += "61" + EDI_FIELD_DELIMITER;
				_cache_REFTaxingAuthority += "${0}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(_cache_REFTaxingAuthority, amount.ToString(EDI_CURRENCY_FORMAT));
			return data;
		}
		//		Write the N1-N4 Entries record for the Payor
		string _cache_PayorInformation = string.Empty;
		private string writeN1toN4PayorInformationLines() 
		{ 
			if (_cache_PayorInformation == string.Empty) 
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append("N1"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("PR"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("YMCA RETIREMENT FUND"); sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				sb.Append("N3"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("140 Broadway"); sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				sb.Append("N4"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("NEW YORK"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("NY"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append("100051197"); sb.Append(EDI_END_OF_LINE);
				_cache_PayorInformation = sb.ToString();
			}
			this.i_numberOfSegmentsInDetailBlock += 3;	// Increment number of Lines printed in current detail block printed
			return string.Format(_cache_PayorInformation);
		}
		//		Write the N1-N4 Entries record for the Payee
		private string writeN1toN4PayeeInformationLines(string firstName, string middleName, string lastName, string addressLine1, string addressLine2, string addressLine3, string city, string state, string zip, string country) 
		{ 
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("N1"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("PE"); sb.Append(EDI_FIELD_DELIMITER);

			string fullName = firstName.Trim() + ((firstName!=string.Empty)?" ":string.Empty) 
				+ middleName.Trim() + ((middleName!=string.Empty)?" ":string.Empty  )
				+ lastName.Trim();

			// If the fullname length is greater than 45 (usually this person 
			// is a guardian) then we need to make sure that we truncate the 
			// data and put it on a separate line if it exceeds 45 characters.
			if (fullName.Length > 45) 
			{
				string format = "{0:T45}" + EDI_END_OF_LINE + "\r\n" + 
					"N2" + EDI_FIELD_DELIMITER + "{1:T45}"; // The second section is optional -  + EDI_FIELD_DELIMITER;
				string tmp = string.Empty;
				// tmp will contain Guardian Name + the string "Guardian Of"
				tmp += firstName; if (firstName != string.Empty) tmp +=" ";
				tmp += middleName;
				// lastname contains the name of the entity who requires a guardian
				sb.Append(String.Format(this.myFormatter, format, tmp, lastName));
				this.i_numberOfSegmentsInDetailBlock++; // Increment number of Lines printed in current detail block printed
			} 
			else 
			{	// Truncate the full name of the person if needed and print it out to sb
				sb.Append(String.Format(this.myFormatter, "{0:T45}", fullName));
			}
 
			sb.Append(EDI_END_OF_LINE); sb.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			if (addressLine1.Trim() != "") 
			{
				sb.Append("N3"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append(String.Format(this.myFormatter, "{0:T45}", addressLine1.Trim())); 
				sb.Append(EDI_FIELD_DELIMITER);
				if (addressLine2.Trim() != "") sb.Append(String.Format(this.myFormatter, "{0:T45}", addressLine2.Trim()));
				sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			}
			if (addressLine3.Trim() != "") 
			{
				sb.Append("N3"); sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append(String.Format(this.myFormatter, "{0:T45}", addressLine3.Trim())); 
				sb.Append(EDI_FIELD_DELIMITER); 
				sb.Append(EDI_END_OF_LINE);
				sb.Append("\r\n");
				this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			}
			sb.Append("N4"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(String.Format(this.myFormatter, "{0:T30}", city.Trim())); 
			sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(String.Format(this.myFormatter, "{0:T2}", state.Trim())); 
			sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(String.Format(this.myFormatter, "{0:T15}", zip.Trim())); 
			if (!(country == null || country.Trim().ToUpper() == "USA" || country.Trim().ToUpper() == "US")) 
			{
				//	Country code is not required if printing for US cheques and the country is assumed to be US
				//	In other cases the country code should be printed as per Mark's request received orally through Ragesh on 2007.05.09
				sb.Append(EDI_FIELD_DELIMITER);
				sb.Append(String.Format(this.myFormatter, "{0:T3}", country.Trim())); 
			}
			sb.Append(EDI_END_OF_LINE);
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			return sb.ToString();
		}
		//		Write the ENT record
		string _cache_ENTLine = string.Empty;
		private string writeENTLine(string param) 
		{ 
			if (_cache_ENTLine==string.Empty) 
			{
				_cache_ENTLine += "ENT" + EDI_FIELD_DELIMITER;
				_cache_ENTLine += "{0:T6}" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(this.myFormatter, _cache_ENTLine, param);
			return data;
		}


		#region "Commented out to support the newer specification of EDI for RMR lines"
		//		//		Write the RMR record for Current Payments/Regular Allowance
		//		private string writeRMRCurrentPaymentLine(double netPayMonthTotal, double grossPayMonthTotal, 
		//			double withholdingMonthTotal, int billingCode, double grossPayMonthTaxable) 
		//		{ 
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("AB"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Regular Allowance: Taxable"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(netPayMonthTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayMonthTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(withholdingMonthTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(billingCode.ToString("00")); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayMonthTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		//		//		Write the RMR record for YTD Payments/YTD Allowance
		//		private string writeRMRYTDPaymentLine(double netPayYTDTotal, double grossPayYTDTotal, 
		//			double withholdingYTDTotal, int billingCode, double grossPayYTDTaxable) 
		//		{ 
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("BT"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Regular Allowance:Taxable YTD"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(netPayYTDTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayYTDTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(withholdingYTDTotal.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(billingCode.ToString("00")); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append(grossPayYTDTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		//		//		Write the NTE*ALL record for special notes/Non-taxable income entries
		//		private string writeNTELines(double grossPayMonthNonTaxable, double grossPayYTDNonTaxable ) 
		//		{ 
		//			// If both values are zero then no need to print this line item
		//			if (grossPayMonthNonTaxable == 0 && grossPayYTDNonTaxable ==0) return string.Empty ;
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("NTE"); sb.Append(EDI_FIELD_DELIMITER);
		//			sb.Append("ALL"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Non-Taxable                   ".PadRight(30, ' '));
		//			sb.Append((grossPayMonthNonTaxable * 100).ToString().PadLeft(10, '0')); 
		//			sb.Append((grossPayYTDNonTaxable * 100).ToString().PadLeft(10, '0')); // This field must not be larget than 10 characters
		//			sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		#endregion

		//		Write the RMR record for Current Taxable Payment
		private string writeRMR_IV_CurrentTaxablePaymentLine(double grossPayMonthTaxable, double grossPayYTDTaxable) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("IV"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("Taxable"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayMonthTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayYTDTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
		//		Write the RMR record for Current Non-Taxable Payment
		private string writeRMR_IK_CurrentNonTaxablePaymentLine(double grossPayMonthNonTaxable, double grossPayYTDNonTaxable) 
		{ 
			// This line is not printed if both current and ytd amounts are 0. As per Mark's email dt. 2007.04.30
			if (grossPayMonthNonTaxable == 0 && grossPayYTDNonTaxable == 0) return string.Empty;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("IK"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("Non-Taxable"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayMonthNonTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossPayYTDNonTaxable.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
		//		Write the RMR record for Special Dividend Payment
		private string writeRMR_2G_SpecialDividendPaymentLine(double grossDividendMonth, double grossDividendYTD )
		{ 
			// This line is not printed if both current and ytd amounts are 0. As per Mark's email dt. 2007.04.30
			if (grossDividendMonth == 0 && grossDividendYTD == 0) return string.Empty;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("RMR"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("2G"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("Not Used"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(""); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossDividendMonth.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(grossDividendYTD.ToString(EDI_CURRENCY_FORMAT)); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}


		#region "Commented out to support the newer specifications of EDI for REF lines"
		//		//		Write the NTE*ADD record for special notes/Special Dividends entry
		//		private string writeNTELinesSpecialDividend(double grossDividendMonth, double grossDividendYTD ) 
		//		{
		//			// If both values are zero then no need to print this line item
		//			if (grossDividendMonth == 0 && grossDividendYTD == 0) return string.Empty ;
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			System.Text.StringBuilder sb = new System.Text.StringBuilder();
		//			sb.Append("NTE"); sb.Append(EDI_FIELD_DELIMITER);
		//			sb.Append("ADD"); sb.Append(EDI_FIELD_DELIMITER); 
		//			sb.Append("Special Dividend".PadRight(30, ' '));
		//			sb.Append((grossDividendMonth * 100).ToString().PadLeft(10, '0')); 
		//			sb.Append((grossDividendYTD * 100).ToString().PadLeft(10, '0')); // This field must not be larget than 10 characters
		//			sb.Append(EDI_END_OF_LINE);
		//			return sb.ToString();
		//		}
		//
		
		//		//		Write the REF record for Deductions from current pay
		//		private string writeREFDeductionsLines(string description, double amount, double yTDAmount) 
		//		{ 
		//			// No need to filter this. This line needs to be printed especially for the Federal Withholding part
		//			// if (amount == 0 && yTDAmount == 0) return string.Empty ; // If amount and yTDAmount are both zero then no need to print this line item
		//			string format = "REF" + EDI_FIELD_DELIMITER + "CM" + EDI_FIELD_DELIMITER 
		//				+ EDI_FIELD_DELIMITER + "{0}{1}{2}" + EDI_END_OF_LINE;
		//			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
		//			return String.Format(format, description.PadRight(30, ' '), 
		//				((amount * 100)).ToString().PadLeft(10,'0'), 
		//				((yTDAmount * 100)).ToString().PadLeft(10, '0'));	// The last field must not exceed 10 characters. The money values are without the decimal point.
		//		}
		#endregion

		//		Write the DED record for Deductions from current pay
		private string writeDEDDeductionsLines(string description, double amount, double ytdAmount, DateTime d) 
		{ 
			// No need to filter this. This line needs to be printed especially for the Federal Withholding part
			// if (amount == 0 && yTDAmount == 0) return string.Empty ; // If amount and yTDAmount are both zero then no need to print this line item
			string format = "DED" + EDI_FIELD_DELIMITER + "CM" + EDI_FIELD_DELIMITER 
				+ "IGNORE" + EDI_FIELD_DELIMITER + "{0}" + EDI_FIELD_DELIMITER + "{1}"
				+ EDI_FIELD_DELIMITER + "{2}" + EDI_FIELD_DELIMITER + "N" 
				+ EDI_FIELD_DELIMITER + "{3:T60}" + EDI_END_OF_LINE;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			return String.Format(this.myFormatter, format, d.ToString("yyyyMMdd"), (amount * 100).ToString(),
				(ytdAmount * 100).ToString().PadLeft(25, '0'), description);
		}
		//		Write the N9 record for each detail block
		string _cache_N9Line = string.Empty;
		private string writeN9Line() 
		{
			if (_cache_N9Line == string.Empty) 
			{
				_cache_N9Line += "N9" + EDI_FIELD_DELIMITER;
				_cache_N9Line += "ZZ" + EDI_FIELD_DELIMITER;
				_cache_N9Line += "NOT USED" + EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Format(_cache_N9Line);
			return data;
		}
		//		Write the AMT record for current payee
		private string writeAMTLine(double currentNetPayAmount, double ytdNetPayAmount, 
			double currentGrossPayAmount, double ytdGrossPayAmount,
			double currentWithholdingAmount, double ytdWithholdingAmount) 
		{ 
			System.Text.StringBuilder data = new System.Text.StringBuilder();
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("TA"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(currentNetPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("TD"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(currentWithholdingAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("TG"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(currentGrossPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("YTA"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(ytdNetPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("YTD"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(ytdWithholdingAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			data.Append("\r\n");
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			data.Append("AMT"); data.Append(EDI_FIELD_DELIMITER);
			data.Append("YTG"); data.Append(EDI_FIELD_DELIMITER);
			data.Append(ytdGrossPayAmount.ToString(EDI_CURRENCY_FORMAT)); data.Append(EDI_END_OF_LINE);
			return data.ToString();
		}
		//		Write the Trailing SE record
		string _cache_SELine = string.Empty;
		private string writeSELine(int lc_int_numberOfSegments, int transactionNumber) 
		{ 
			if (_cache_SELine == string.Empty) 
			{
				_cache_SELine  += "SE" + EDI_FIELD_DELIMITER;
				_cache_SELine  += "{0}" + EDI_FIELD_DELIMITER;
				_cache_SELine  += "{1:" + EDI_ST_LINE_TRANSACTION_NUMBER_FORMAT + "}";
				_cache_SELine  += EDI_END_OF_LINE;
			}
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			lc_int_numberOfSegments++; // Number of segments we are incrementing to include this one 
			string data = string.Format(_cache_SELine, lc_int_numberOfSegments, transactionNumber);
			this.i_numberOfSegmentsInDetailBlock = 0;	// Set number of Lines printed in current detail block to zero as we finished using the value
			return data;
		}
						
		//		Write the Trailing GE for the Details record
		private string writeGEForDetailFunctionalGroup() 
		{ 
			string data = string.Empty;
			data += "GE" + EDI_FIELD_DELIMITER;
			data += "104" + EDI_FIELD_DELIMITER; 
			data += "1" + EDI_END_OF_LINE;
			return data;
		}
		//		Write the opening GS record for summaries
		private string writeGSForSummaryFunctionalGroup(DateTime dt) 
		{ 
			this.i_numberOfFunctionalBlocks++;	// Increment the number of Functional Blocks being printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("GS"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("CT"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_SENDER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(EDI_APPLICATION_RECEIVER_CODE); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(dt.ToString("yyyyMMdd")); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append(dt.ToString("HHmm")); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("1"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("T"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append("004010"); sb.Append(EDI_END_OF_LINE);
			return sb.ToString();
		}
		//		Write the ST record for Summary
		private string writeSTLineForSummary(int transactionNumber) 
		{ 
			this.i_numberOfSegmentsInDetailBlock = 1;	// Set number of Lines printed in current detail block as this is the first line to be printed
			string data = string.Empty;
			data += "ST" + EDI_FIELD_DELIMITER;
			data += "831" + EDI_FIELD_DELIMITER;
			data += transactionNumber.ToString(EDI_ST_LINE_TRANSACTION_NUMBER_LONG_FORMAT);
			data += EDI_END_OF_LINE;
			return data;
		}
		//		Write the BGN record for the summaries
		private string writeBGNLineForSummary(string referenceInformation, DateTime dt) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("BGN"); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append("27"); sb.Append(EDI_FIELD_DELIMITER); 
			sb.Append(referenceInformation); sb.Append(EDI_FIELD_DELIMITER);
			sb.Append(dt.ToString("yyyyMMdd")); sb.Append(EDI_FIELD_DELIMITER); // Format yyyyMMdd
			sb.Append(dt.ToString("HHmmssff")); sb.Append(EDI_END_OF_LINE);	// Format HHmmssff
			return sb.ToString();
		}
		//		Write the N9 record to specify the Batch number
		private string writeN9LineForSummary(string batchNumber) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Empty;
			data += "N9" + EDI_FIELD_DELIMITER;
			data += "BT" + EDI_FIELD_DELIMITER;
			data += String.Format(this.myFormatter, "{0:T30}", batchNumber) + EDI_END_OF_LINE;
			return data;
		}

		//		Write the AMT record for net of all Credit/Debit operations
		private string writeAMTLineForSummary(double checkTotal) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Empty;
			data += "AMT" + EDI_FIELD_DELIMITER;
			data += "OP" + EDI_FIELD_DELIMITER;
			data += checkTotal.ToString(EDI_CURRENCY_FORMAT)  + EDI_END_OF_LINE;
			return data;
		}

		//		Write the QTY record to indicate the number of records written
		private string writeQTYLineForSummary(int numberOfDetailBlocks) 
		{ 
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			string data = string.Empty;
			data += "QTY" + EDI_FIELD_DELIMITER;
			data += "46" + EDI_FIELD_DELIMITER;
			data += numberOfDetailBlocks + EDI_END_OF_LINE;
			return data;
		}
		//		Write the trailing SE record to mark close of summaries
		private string writeSELineForSummary(int lc_int_numberOfSegments, int transactionNumber) 
		{ 
			string data = string.Empty;
			data += "SE" + EDI_FIELD_DELIMITER;
			this.i_numberOfSegmentsInDetailBlock++;	// Increment number of Lines printed in current detail block printed
			lc_int_numberOfSegments++;	// Increment count to include the lines for current segment
			data += lc_int_numberOfSegments.ToString() + EDI_FIELD_DELIMITER;
			data += transactionNumber.ToString(EDI_ST_LINE_TRANSACTION_NUMBER_LONG_FORMAT);
			data += EDI_END_OF_LINE;
			this.i_numberOfSegmentsInDetailBlock = 0;	// Set number of Lines printed in current detail block to zero since this is the end of the current block
			return data;
		}
		//		Write the trailing GE record to mark close of summaries
		private string writeGELineForSummary() 
		{ 
			string data = string.Empty;
			data += "GE" + EDI_FIELD_DELIMITER;
			data += "1" + EDI_FIELD_DELIMITER; 
			data += "1" + EDI_END_OF_LINE;
			return data;
		}
		//		Write the IEA record to mark the end of the EDI document
		private string writeIEALineForSummary(int numberOfFunctionalBlocks, int interchangeControlNumber) 
		{ 
			string data = string.Empty;
			data += "IEA" + EDI_FIELD_DELIMITER;
			data += numberOfFunctionalBlocks + EDI_FIELD_DELIMITER; 
			data += interchangeControlNumber.ToString(EDI_INTERCHANGE_CONTROL_NUMBER_FORMAT) + EDI_END_OF_LINE;
			return data;
		}

		#endregion
		private void ProduceOutputFilesCreateDetail(int index,int intPhase )
		{
				
			string l_String_Output,lc_space,l_String_temp,l_String_double_NetPayMonthTotal;
			// Number Formats.

			try
			{
				NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
				nfi.CurrencyGroupSeparator=""; 
				nfi.CurrencySymbol=""; 
				nfi.CurrencyDecimalDigits = 2;

				lc_space = l_String_temp = "";
				
				
				switch(index)
				{
					case CSDetail:
						if (this.String_UsCanadaBankCode =="1") 
						{
							//this.lcOutputCursor = "curCSUS";
							this.String_OutputFileType = "CHKSCU";
						}
						else if (this.String_UsCanadaBankCode =="2")
						{
							//this.lcOutputCursor = "curCSCanadian";
							this.String_OutputFileType = "CHKSCC";
						}
							
						if (intPhase == 1)
						{			
							l_String_Output = C_ROWCODEA;
							l_String_Output += this.String_DisbursementNumber.Trim().PadRight(10,' ');
							l_String_Output += this.CheckDate.ToString("yyyy/MM/dd");
							l_String_Output += this.TotalMonthlyNet.ToString("C",nfi).PadLeft(11,'0'); 
							l_String_Output += this.String_UsCanadaBankCode.Trim();

							WriteOutputFilesDataLineIntoStream(l_String_Output);

							//Insert 
							// Row B
							l_String_Output = C_ROWCODEB;
							l_String_Output += this.String_Name60.PadRight(60,' ');
							if (this.String_Address1.Trim() !="")  l_String_Output += this.String_Address1.PadRight(60,' ');
							if (this.String_Address2.Trim() !="")  l_String_Output += this.String_Address2.PadRight(60,' ');
							if (this.String_Address3.Trim() !="")  l_String_Output += this.String_Address3.PadRight(60,' ');
							if (this.String_Address4.Trim() !="")  l_String_Output += this.String_Address4.PadRight(60,' ');
							if (this.String_Address5.Trim() !="")  l_String_Output += this.String_Address5.PadRight(60,' ');
							WriteOutputFilesDataLineIntoStream(l_String_Output);

							// Insert

							// Payroll 
							if (this.bool_OutputFilePayRoll)
							{
								l_String_Output = C_ROWCODEC;
								l_String_Output += this.String_FundID.Trim().PadRight(9,' ');
								l_String_Output += this.String_FilingStatusExemptions.Trim();
								l_String_Output += "$" + this.double_WithholdingMonthAddl.ToString("C",nfi).Trim().PadLeft(9,' ');
								WriteOutputFilesDataLineIntoStream(l_String_Output);

								// Insert
								// Row D - (Current)
								l_String_Output = C_ROWCODED +	C_GROSSTEXT;
								l_String_Output += "$" + this.double_GrossPayMonthTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
								l_String_Output += lc_space.PadRight(27,' ') + C_DEDTEXT;  
								l_String_Output += "$" + this.double_WithholdingMonthTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
								l_String_Output += lc_space.PadRight(27,' ') + C_NETTEXT;  
								l_String_Output += "$" + this.double_NetPayMonthTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
								WriteOutputFilesDataLineIntoStream(l_String_Output);
								
								//Insert

								//Record type D - (YTD)
								l_String_Output = C_ROWCODED + C_YTDGROSSTEXT;
								l_String_Output += "$" + this.double_GrossPayYTDTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
								l_String_Output += lc_space.PadRight(27,' ') + C_YTDDEDTEXT;  
								l_String_Output += "$" + this.double_WithholdingYTDTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
								l_String_Output += lc_space.PadRight(27,' ') + C_YTDNETTEXT;  
								l_String_Output += "$" + this.double_NetPayYTDTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
								WriteOutputFilesDataLineIntoStream(l_String_Output);
										
								
								// Insert
								// Record Type D - (Other Deductions) - Can be multiple records
	   					
								//NP - Added to support printing of EDI document - Here we are writing the first part of the EDI details
								if (this.bool_OutputFileEDI_US && this.String_UsCanadaBankCode=="1" && !this.bool_ExcludeThisPersonFromEDI) 
								{
									//Reset count for the numberOfSegments since we are starting a new block
									this.i_numberOfSegmentsInDetailBlock = 0;
									//Write the ST*820, BPR*D, TRN*1, REF*BT, REF*W9, REF*FU lines
									this.i_numberOfDetailBlocks++;	// Increment number of records/detail blocks being printed
									l_String_Output = this.writeSTLine(this.i_numberOfDetailBlocks);
									// write to output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//									//Increment counter for currentNetPaymentAmount, ytdNetPaymentAmount
									//									this.double_currentNetPaymentAmount += Double.Parse(this.double_NetPayMonthTotal.ToString("0.00"));
									//									this.double_ytdNetPaymentAmount += Double.Parse(this.double_NetPayYTDTotal.ToString("0.00"));
									//									//Increment running total for currentGrossAllowance, ytdGrossAllowance
									//									this.double_currentGrossAllowance += Double.Parse(this.double_GrossPayMonthTotal.ToString("0.00"));
									//									this.double_ytdGrossAllowance += Double.Parse(this.double_GrossPayYTDTotal.ToString("0.00"));
									//									//Increment running total for currentWithholding, ytdWithholding
									//									//Since this is run from another loop, these totals are maintained from the writeDEDLine function for EDI.
									//									//I tried doing that and the figures do not match for ytdWithholding. There is some missing data somewhere.
									//									//Hence I have switched back to this where the ytdAmount is coming from the DB and has been set in the global
									//									//variable in the main loop when this call is made.
									//
									//									//Increment running totals for current withholdings and YTD withholdings
									//									this.double_currentWithholdingTotal += this.double_WithholdingMonthTotal;
									//									this.double_ytdWithholdingTotal += this.double_WithholdingYTDTotal;

									l_String_Output = this.writeBPRLine(this.double_NetPayMonthTotal,
										this.CheckDate);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeTRNLine(this.String_DisbursementNumber);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFBatchNumberLine(C_BATCHNUMBER);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
                                    if (this.String_Country.Trim() == "US" || this.String_Country.Trim() == "USA" || string.IsNullOrEmpty(this.String_Country)) // Manthan Rajguru | 2016.03.31 | YRS-AT-2181 | Added condition to identify US and Non-US country and is copied from 'function = writeN1toN4PayeeInformationLines'
                                        l_String_Output = this.writeREFMailingInformationLines();
                                    else
                                        l_String_Output = this.writeREFMailingInformationLinesForNonUS();// Manthan Rajguru | 2016.03.31 | YRS-AT-2181 | Assigning value returning from writeREFMailingInformationLinesForNonUS() function to variable
                                    //Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFFundIdentificationInformationLine(this.String_FundID);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFAccountingStatusLine(this.String_FilingStatusExemptions);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeREFTaxingAuthorityIdentificationNumberLine(double_WithholdingMonthAddl);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									//					N1-N4 Payor Lines no longer need to be printed as per Mark/Clarence's email.
									//									l_String_Output = this.writeN1toN4PayorInformationLines();
									//									//Write to Output
									//									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeN1toN4PayeeInformationLines(this.String_DBFirstName, this.String_DBMiddleName, this.String_DBLastName, 
										this.String_Address1, this.String_DBAddress2, this.String_DBAddress3, 
										this.String_City, this.String_State, this.String_ZIP, this.String_Country);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									l_String_Output = this.writeENTLine("1");
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//									l_String_Output = this.writeRMRCurrentPaymentLine(this.double_NetPayMonthTotal, this.double_GrossPayMonthTotal,
									//										this.double_WithholdingMonthTotal, 1, this.double_GrossPayMonthTaxable);
									//									l_String_Output = this.writeRMRYTDPaymentLine(this.double_NetPayYTDTotal, this.double_GrossPayYTDTotal, 
									//										this.double_WithholdingYTDTotal, 1, this.double_GrossPayYTDTaxable);
									//									// Write Non-taxable line to EDI
									//									l_String_Output = this.writeNTELines(this.double_GrossPayMonthNontaxable, this.double_GrossPayYTDNontaxable);
									
									//NP: 2007.04.26 We want to print the following lines only if we are 
									//	performing regular Payroll processing. - as per Mark's email
									if (this.String_DisbursementType.ToString() != "EXP") 
									{
										l_String_Output = this.writeRMR_IV_CurrentTaxablePaymentLine(this.double_GrossPayMonthTaxable, this.double_GrossPayYTDTaxable);
										//Write to Output
										WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
										l_String_Output = this.writeRMR_IK_CurrentNonTaxablePaymentLine(this.double_GrossPayMonthNontaxable, 
											this.double_GrossPayYTDNontaxable);
										//Write to Output
										WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
									}

									// Write Special Dividend line to EDI. The value is stored in different places depending on the 
									// value in this.String_DisbursementType. (either ANN or something else)
									//									l_String_Output = this.writeNTELinesSpecialDividend(
									//										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentMonthTotal : this.double_GrossPayMonthTaxable, 
									//										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentYTDNontaxable : this.double_GrossPayYTDTaxable
									//										);
									l_String_Output = this.writeRMR_2G_SpecialDividendPaymentLine(
										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentMonthTotal : this.double_GrossPayMonthTaxable, 
										this.String_DisbursementType == "ANN" ? this.double_GrossPayDividentYTDNontaxable : this.double_GrossPayYTDTaxable
										);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);


									// Insert
									// Record - (Other Deductions) - Can be multiple records
								}
							}
						}
						else
						{
							if (this.bool_OutputFilePayRoll)
							{
								// D Line has been printed. Now we print the E line
								// If we are printing Special Dividend then appropriate text is output but using the same values
								if (this.String_DisbursementType.ToString() == "ANN")
									l_String_Output = C_ROWCODEE + C_REGALLOWTAXTEXT;
								else
									l_String_Output = C_ROWCODEE + C_EXPDIVTEXT;

								l_String_Output += "$" + this.double_GrossPayMonthTaxable.ToString("C",nfi).Trim().PadLeft(12,' ');
								l_String_Output += lc_space.PadRight(1,' ');
								l_String_Output += "$" + this.double_GrossPayYTDTaxable.ToString("C",nfi).Trim().PadLeft(12,' ');
								WriteOutputFilesDataLineIntoStream(l_String_Output);

								//Insert
								// Create non-taxable line here if there are values to put in there
								// else print a blank line if we are processing monthly annuities
                                if (!(this.double_GrossPayMonthNontaxable == 0 && double_GrossPayYTDNontaxable  ==0))
                                {
									l_String_Output = C_ROWCODEE;
											
									//start of comment by hafiz on 17Apr2006
									//l_String_Output += lc_space.PadRight(39,' ');
									//end of comment by hafiz on 17Apr2006
											
									//start of code add by hafiz on 17Apr2006
									l_String_Output += lc_space.PadRight(19,' ');
									//end of code add by hafiz on 17Apr2006
											
									l_String_Output += C_NONTAXTEXT;
									l_String_Output += "$" + this.double_GrossPayMonthNontaxable.ToString("C",nfi).Trim().PadLeft(12,' ');
									l_String_Output += lc_space.PadRight(1,' ');
									l_String_Output += "$" + this.double_GrossPayYTDNontaxable.ToString("C",nfi).Trim().PadLeft(12,' ');
									WriteOutputFilesDataLineIntoStream(l_String_Output);
											

									//Insert
                                }
								else if(this.String_DisbursementType.ToString()  == "ANN") 
                                {
                                    l_String_Output = C_ROWCODEE;
                                    WriteOutputFilesDataLineIntoStream(l_String_Output);
                                }

								if (this.String_DisbursementType.ToString()  == "ANN") 
								{
									l_String_Output = C_ROWCODEE + C_EXPDIVTEXT;
									l_String_Output += "$" + double_GrossPayDividentMonthTotal.ToString("C",nfi).Trim().PadLeft(12,' ');
									l_String_Output += lc_space.PadRight(1,' ');
									l_String_Output += "$" + double_GrossPayDividentYTDNontaxable.ToString("C",nfi).Trim().PadLeft(12,' ');
									WriteOutputFilesDataLineIntoStream(l_String_Output);
								}

								//NP - Added to support printing of EDI document - Here we are printing the N9, AMT and SE the EDI detail section
								if (this.bool_OutputFileEDI_US && this.String_UsCanadaBankCode == "1" && !this.bool_ExcludeThisPersonFromEDI) 
								{
									//Write the N9 Line for the detail segment
									l_String_Output = this.writeN9Line();
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//Write AMT Lines for person details
									l_String_Output = this.writeAMTLine(this.double_NetPayMonthTotal, this.double_NetPayYTDTotal,
										this.double_GrossPayMonthTotal, this.double_GrossPayYTDTotal,
										this.double_WithholdingMonthTotal, this.double_WithholdingYTDTotal);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);

									//Write the closing SE line for the Detail section
									l_String_Output = this.writeSELine(this.i_numberOfSegmentsInDetailBlock, this.i_numberOfDetailBlocks);
									//Write to Output
									WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
								}

							}
							
						}

						break;

					case PPDetail:
						this.String_OutputFileType = "PP";
						l_String_Output = C_ROWCODER + C_YMCABANKACCOUNT;
						l_String_Output += this.String_DisbursementNumber.Trim().PadLeft(10,'0');
						l_String_Output += this.double_NetPayMonthTotal.ToString("C",nfi).Trim().PadLeft (11,'0').Remove(8,1);
						l_String_Output += this.DisbursementDate.ToString("MMddyy");
						l_String_Output += C_TRANSACTIONCODE; 
						l_String_Output += this.String_Description.Trim().PadRight(15,' ');
						WriteOutputFilesDataLineIntoStream(l_String_Output);
						this.double_OutputFilesPPDetailSum += this.double_NetPayMonthTotal;
						//Insert

						this.double_OutputFilesPPDetailCount +=1;
								

						break;
					case EFTDetail:
						if (this.String_UsCanadaBankCode == "1")
						{
							//this.lcOutputCursor = "curNorthernTrust";
							this.String_OutputFileType = "EFT";
							l_String_Output = C_ROWCODE6;
							l_String_Output += this.String_EftTypeCode.Trim().PadRight(2,' ');
							l_String_Output += this.String_BankAbaNumber.Trim().PadRight(8,' ');
							l_String_temp += this.String_BankABANumberNinthDigit.Trim().Substring(this.String_BankABANumberNinthDigit.Trim().Length-1,1); 
							l_String_Output += l_String_temp;
							l_String_Output += this.String_ReceivingDFEAccount.Trim().PadRight(17,' ');
							l_String_Output += this.double_NetPayMonthTotal.ToString("C",nfi).Trim().PadLeft(11,'0').Remove(8,1);
							l_String_Output += this.String_IndividualID.Trim().PadRight(15,' ');
							l_String_Output += this.String_Name22.Trim().PadRight(22,' ');
							l_String_Output += this.String_CompanyData.Trim().PadRight(2,' ');
							l_String_Output += C_ADDENDAINDICATOR + C_TRACENUMBER;

							WriteOutputFilesDataLineIntoStream(l_String_Output);
							// insert
							this.double_OutputFileEFTDetailCount +=1;
							//34231 Changed to equalize the EFT Total in Special Dividends to avoid penny differences.
							// 02-01-2007
							if (this.String_DisbursementType.ToString() == "ANN")
							{
								this.double_OutputFilesEFTDetailSum += this.double_NetPayMonthTotal; 
							}
							else
							{
								l_String_double_NetPayMonthTotal = this.double_NetPayMonthTotal.ToString("C",nfi).Trim();
								this.double_OutputFilesEFTDetailSum += Convert.ToDouble(l_String_double_NetPayMonthTotal); 

							}

							if(this.String_BankAbaNumber.Trim().Length > 8)
							{
								this.int32_OutputFileEFTDetailHash += Int32.Parse(this.String_BankAbaNumber.Trim().Substring(0,8));
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
				NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
				nfi.CurrencyGroupSeparator=""; 
				nfi.CurrencySymbol=""; 
				nfi.CurrencyDecimalDigits = 2;

				lc_space = "";

				l_String_Output = C_ROWCODED;
				l_String_temp = this.String_WithholdingDetailDesc.Trim().PadRight(29,' ');
				if (l_String_temp.Length > 29) l_String_temp = l_String_temp.Substring(0,28);
						
				//start of comment by hafiz on 13Apr2006
				//l_String_Output = l_String_temp;
				//end of comment by hafiz on 13Apr2006
						
				//start of code add by hafiz on 13Apr2006
				l_String_Output += l_String_temp;
				//end of code add by hafiz on 13Apr2006

				l_String_Output += "$" + this.double_WithholdingMonthDetail.ToString("C",nfi).Trim().PadLeft (12,' ');
				l_String_Output += lc_space.PadRight(1,' '); 
				l_String_Output += "$" + this.double_WithholdingYTDDetail.ToString("C",nfi).Trim().PadLeft (12,' ');
				WriteOutputFilesDataLineIntoStream(l_String_Output);
				//NP - Added to support printing of EDI document - Here we are printing the withholding details
				if (this.bool_OutputFileEDI_US && this.String_UsCanadaBankCode == "1" && !this.bool_ExcludeThisPersonFromEDI) 
				{
					
					//					//Write to EDI_US
					//					l_String_Output = this.writeREFDeductionsLines(this.String_WithholdingDetailDesc.Trim(), this.double_WithholdingMonthDetail, 
					//						this.double_WithholdingYTDDetail);
					l_String_Output = this.writeDEDDeductionsLines(this.String_WithholdingDetailDesc.Trim(), this.double_WithholdingMonthDetail, 
						this.double_WithholdingYTDDetail, this.DateTime_currentDateTime);
					//Write to Output
					WriteOutputFilesDataLineIntoStreamEDI(l_String_Output);
				}
			}
			catch
			{


			}

			// Insert
	            
		}

		public DataTable datatable_FileList
				{
					get{return l_datatable_FileList;}
				}
        //New parameter 'parameterPayrollDate' passed to function to pass payroll date to IAT file
		public  bool PreparOutPutFileData(DataSet ParameterDatasetPayroll,bool parameterboolproof,DateTime parameterDateCheckDate,string parameterDibursementType)
		{
			DataSet ldataset = ParameterDatasetPayroll;
			//string lstr_tmp;
			double ln_tmp,ln_tmp1,ln_tmp2;
			string lcMaritalStatusCode,lcWithholdingType;
			string l_String_expr,l_String_Guid;
			DataRow[] foundWithholdingsRows,foundYTDWithholdingsRows,foundFedTaxWithholdingRORows;
			//	String_WithholdingDetailDesc,str_WithholdingTypeCode,
			this.dateTimePayrollMonth = parameterDateCheckDate;

			DataRowCollection rc; 
			DataRow DfNewRow;
					
			l_String_Guid ="";
			object[] rowVals = new object[3];

			this.ProofReport = parameterboolproof;
				    
			//Hashtable HTwithHoldings = new Hashtable();
			//start of comment by hafiz on 12April2006
			//this.ProofReport = false;
			//end of comment by hafiz on 12April2006

			//NP - Added code here to populate the Exclusion List for EDI
			this.DataSet_ExclusionList = loadExclusionList();
			if (this.DataSet_ExclusionList == null) throw new Exception("Unable to load Exclusion List for EDI from the table atsPayrollExclusionListForEDI") ;

            try
            {

                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", "PreparOutPutFileData() START");
                DataTable oDataTable = ldataset.Tables["PayRoll"];
                DataTable oDataTableWithholdings = ldataset.Tables["Withholdings"];
                DataTable oDataTableYTDWithholdings = ldataset.Tables["YTDWithholdings"];
                DataTable oDataTableDisbursementFiles = ldataset.Tables["DisbursementFiles"];
                DataTable oDataTableOutPutfilePath = ldataset.Tables["OutPutfilePath"];
                DataTable oDataTableFedTaxWithholdingRO = ldataset.Tables["FedTaxWithholdingRO"];

                //NP - Added to support exclusions for EDI
                DataTable oDataTableEDI_Exclusions = this.DataSet_ExclusionList.Tables["EDI_Exclusions"];

                rc = oDataTableDisbursementFiles.Rows;

                IdentifyPaymentTypes(oDataTable);
                // Create Streams to write payments.

                // Initialize the global dateTime value that is to be used at various 
                // locations in the EDI and Batch process. This value is initialized  
                // to the DateTime when the process is being executed.
                this.DateTime_currentDateTime = DateTime.Now;
                this.CheckDate = parameterDateCheckDate; // SR | 2020.01.09 | YRS--AT-4602 | checkdate property is required to identify payment outsourcing is ON/OFF.
				// START | SR | 2020.01.09 | YRS-AT-4602 | Set flag to create export change file.
                DataTable changeFileData = ldataset.Tables["BankMonthlyPayrollData"];//ML | YRS-AT-4602 | 2019.11.22 | Monthly payroll data for Bank 
                if (HelperFunctions.isNonEmpty(changeFileData))
                { outPutFileNTPyrl = true; }
				// END| SR | 2020.01.09 | YRS-AT-4602 | Set flag to create export change file.
                if (!ProduceOutputFilesOutput(oDataTableOutPutfilePath)) return false;
                // Add rows for the Header Information.

                this.String_DisbursementType = parameterDibursementType.ToString().Trim();

                ProduceOutputFilesCreateHeader();


                if (oDataTable == null) { }

                foreach (DataRow oRow in oDataTable.Rows)
                {
                    //PJ:BT-855:YRS 5.0-1344 - Create new IAT output file   


                    if (oRow["PaymentMethodCode"].ToString() == "IAT")
                    {
                        continue;
                    }
                    InitializseMembers();

                    this.String_PersID = oRow["PersID"].ToString();
                    this.String_Address1 = oRow["Addr1"].ToString().Trim();

                    //NP - Added to support printing of EDI documents
                    if (!oRow.IsNull("City")) this.String_City = oRow["City"].ToString();
                    if (!oRow.IsNull("StateType")) this.String_State = oRow["StateType"].ToString();
                    if (!oRow.IsNull("Zip")) this.String_ZIP = oRow["Zip"].ToString();
                    if (!oRow.IsNull("Country")) this.String_Country = oRow["Country"].ToString();
                    if (!oRow.IsNull("Addr2")) this.String_DBAddress2 = oRow["Addr2"].ToString();
                    this.String_DBAddress3 = string.Empty;
                    //NP - We decide whether this person needs to be excluded from EDI generation or not
                    DataRow[] dr = oDataTableEDI_Exclusions.Select("guiPersId='" + this.String_PersID + "'");
                    this.bool_ExcludeThisPersonFromEDI = (dr.Length > 0) ? true : false;

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
                    this.String_DisbursementID = Convert.ToString(oRow["DisbursementID"]);
                    this.String_DisbursementNumber = Convert.ToString(oRow["DisbursementNumber"]);

                    this.String_DisbursementType = parameterDibursementType.ToString().Trim();

                    this.String_FundID = Convert.ToString(oRow["FundIDNo"]);
                    //aparna -12/02/2007 -YREN-3074
                    //this.String_IndividualID = (string) oRow["SSNo"];
                    this.String_IndividualID = this.String_FundID;
                    if (!oRow.IsNull("FirstName")) this.String_DBFirstName = oRow["FirstName"].ToString();
                    if (!oRow.IsNull("MiddleName")) this.String_DBMiddleName = oRow["MiddleName"].ToString();
                    if (!oRow.IsNull("LastName")) this.String_DBLastName = oRow["LastName"].ToString();
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

                    // Monthly Withholding data.
                    ln_tmp = ln_tmp1 = ln_tmp2 = 0;
                    if (oRow["CurrentPayment"].GetType().ToString() != "System.DBNull")
                    {
                        ln_tmp = Convert.ToDouble(oRow["CurrentPayment"]);

                    }

                    if (oRow["MonthlyWithhold"].GetType().ToString() != "System.DBNull")
                    {
                        ln_tmp1 = Convert.ToDouble(oRow["MonthlyWithhold"]);

                    }

                    if (oRow["SocialSecurityAdjPayment"].GetType().ToString() != "System.DBNull")
                    {
                        ln_tmp2 = Convert.ToDouble(oRow["SocialSecurityAdjPayment"]);

                    }

                    // CurrentPayment - lnTotalMonthlyWithholdings + SocialSecurityAdjPayment

                    this.double_NetPayMonthTotal = ln_tmp - ln_tmp1 + ln_tmp2;

                    if (this.double_NetPayMonthTotal < 0) this.double_NetPayMonthTotal = 0;

                    this.String_ReceivingDFEAccount = oRow["BankAcctNumber"].ToString();
                    this.TotalMonthlyNet = this.double_NetPayMonthTotal;
                    if (oRow["CurrencyCode"].ToString().Trim() == "U") this.String_UsCanadaBankCode = "1";
                    else if (oRow["CurrencyCode"].ToString().Trim() == "C") this.String_UsCanadaBankCode = "2";
                    else this.String_UsCanadaBankCode = "";

                    if (oRow["PaymentMethodCode"].GetType().ToString() != "System.DBNull") this.String_PaymentMethodCode = oRow["PaymentMethodCode"].ToString().Trim();
                    else this.String_PaymentMethodCode = "";

                    if (this.String_PaymentMethodCode.Trim().ToUpper() != "CHECK")
                    {
                        if (oRow["EftTypeCode"].GetType().ToString() != "System.DBNull") this.String_EftTypeCode = oRow["EftTypeCode"].ToString();
                        else this.String_EftTypeCode = "";

                        if (oRow["EftTypeCode"].GetType().ToString() != "System.DBNull") this.String_BankAbaNumber = oRow["BankAbaNumber"].ToString().Substring(0, 8);
                        else this.String_BankAbaNumber = "";

                        if (oRow["EftTypeCode"].GetType().ToString() != "System.DBNull") this.String_BankABANumberNinthDigit = oRow["BankAbaNumber"].ToString().Substring(oRow["BankAbaNumber"].ToString().Trim().Length - 1, 1);
                        else this.String_BankABANumberNinthDigit = "";

                    }

                    //this.PaymentData_Addenda = "";
                    //this.AddendaSeqNum_Addenda = "";
                    //this.EntryDetailSeqNum_Addenda = "";

                    //Get additional withHolding information If ANY

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

                    if (oRow["YmcaPostTaxCurrentPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthNontaxable = Convert.ToDouble(oRow["YmcaPostTaxCurrentPayment"]);
                    else this.double_GrossPayMonthNontaxable = 0;
                    if (oRow["PersonalPostTaxCurrentPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthNontaxable += Convert.ToDouble(oRow["PersonalPostTaxCurrentPayment"]);
                    else this.double_GrossPayMonthNontaxable += 0;

                    if (oRow["PersonalPreTaxCurrentPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTaxable = Convert.ToDouble(oRow["PersonalPreTaxCurrentPayment"]);
                    else this.double_GrossPayMonthTaxable = 0;
                    if (oRow["YmcaPreTaxCurrentPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTaxable += Convert.ToDouble(oRow["YmcaPreTaxCurrentPayment"]);
                    else this.double_GrossPayMonthTaxable += 0;
                    if (oRow["SocialSecurityAdjPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTaxable += Convert.ToDouble(oRow["SocialSecurityAdjPayment"]);
                    else this.double_GrossPayMonthTaxable += 0;


                    if (oRow["CurrentPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTotal = Convert.ToDouble(oRow["CurrentPayment"]);
                    else this.double_GrossPayMonthTotal = 0;
                    if (oRow["SocialSecurityAdjPayment"].GetType().ToString() != "System.DBNull") this.double_GrossPayMonthTotal += Convert.ToDouble(oRow["SocialSecurityAdjPayment"]);
                    else this.double_GrossPayMonthTotal += 0;

                    if (oRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDNontaxable = Convert.ToDouble(oRow["YTDPayNonTaxable"]);
                    else this.double_GrossPayYTDNontaxable = 0;
                    this.double_GrossPayYTDNontaxable += this.double_GrossPayMonthNontaxable;

                    if (oRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDTaxable = Convert.ToDouble(oRow["YTDPayTaxable"]);
                    else this.double_GrossPayYTDTaxable = 0;
                    this.double_GrossPayYTDTaxable += this.double_GrossPayMonthTaxable;

                    if (oRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDTotal = Convert.ToDouble(oRow["YTDPayNonTaxable"]);
                    else this.double_GrossPayYTDTotal = 0;
                    if (oRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_GrossPayYTDTotal += Convert.ToDouble(oRow["YTDPayTaxable"]);
                    else this.double_GrossPayYTDTotal += 0;

                    //populate Divident Values
                    this.double_GrossPayDividentMonthTotal = 0.00;

                    if (oRow["YTDPayTaxableDivident"].GetType().ToString() != "System.DBNull") this.double_GrossPayDividentYTDNontaxable = Convert.ToDouble(oRow["YTDPayTaxableDivident"]);
                    else this.double_GrossPayDividentYTDNontaxable = 0;

                    this.double_GrossPayYTDTotal += this.double_GrossPayMonthTaxable + this.double_GrossPayMonthNontaxable + double_GrossPayDividentYTDNontaxable;
                    this.double_NetPayMonthTotal = this.TotalMonthlyNet;

                    //Get Year to Date (YTD) WithHoldings

                    if (oRow["YTDPayTaxable"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal = Convert.ToDouble(oRow["YTDPayTaxable"]);
                    else this.double_NetPayYTDTotal = 0;
                    if (oRow["YTDPayNonTaxable"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal += Convert.ToDouble(oRow["YTDPayNonTaxable"]);
                    else this.double_NetPayYTDTotal += 0;
                    if (oRow["YTDWithholding"].GetType().ToString() != "System.DBNull") this.double_NetPayYTDTotal -= Convert.ToDouble(oRow["YTDWithholding"]);
                    else this.double_NetPayYTDTotal -= 0;
                    this.double_NetPayYTDTotal += this.double_NetPayMonthTotal;

                    this.double_NetPayYTDTotal += this.double_GrossPayDividentYTDNontaxable;

                    if (oRow["YTDWithholding"].GetType().ToString() != "System.DBNull") this.double_WithholdingYTDTotal = Convert.ToDouble(oRow["YTDWithholding"]);
                    else this.double_WithholdingYTDTotal = 0;
                    if (oRow["MonthlyWithhold"].GetType().ToString() != "System.DBNull") this.double_WithholdingYTDTotal += Convert.ToDouble(oRow["MonthlyWithhold"]);
                    else this.double_WithholdingYTDTotal += 0;

                    if (oRow["MonthlyWithhold"].GetType().ToString() != "System.DBNull") this.double_WithholdingMonthTotal = Convert.ToDouble(oRow["MonthlyWithhold"]);
                    else this.double_WithholdingMonthTotal = 0;




                    //Withholdings

                    this.String_WithholdingDetailDesc = "";
                    this.double_WithholdingMonthDetail = 0;
                    this.double_WithholdingYTDDetail = 0;

                    l_String_expr = "PersID = '" + this.String_PersID + "'";
                    foundWithholdingsRows = oDataTableWithholdings.Select(l_String_expr);

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
                        //START : ML | 2019.12.19 | YRS-AT-4602 | Declare Variable
                        case "NTPYRL":
                            l_String_Guid = this.String_OutputFileNTPYRLGuid;
                            break;
                        //END : ML | 2019.12.19 | YRS-AT-4602 | Declare Variable
                    }

                    if (this.String_PaymentMethodCode.Trim() == "CHECK")
                    {
                        ProduceOutputFilesCreateDetail(CSDetail, 1);

                        foreach (DataRow lRow in foundWithholdingsRows)
                        {
                            if (lRow["Description"].GetType().ToString() != "System.DBNull") this.String_WithholdingDetailDesc = lRow["Description"].ToString();
                            else String_WithholdingDetailDesc = "";

                            if (lRow["Amount"].GetType().ToString() != "System.DBNull") this.double_WithholdingMonthDetail = Convert.ToDouble(lRow["Amount"]);
                            else double_WithholdingMonthDetail = 0;

                            double_WithholdingYTDDetail = double_WithholdingMonthDetail;

                            l_String_expr = "PersID = '" + this.String_PersID + "' and WithholdingTypeCode = '" + lRow["WithholdingTypeCode"].ToString().Trim() + "'";

                            foundYTDWithholdingsRows = oDataTableYTDWithholdings.Select(l_String_expr);

                            foreach (DataRow pRow in foundYTDWithholdingsRows)
                            {
                                if (pRow["YTDWithholding"].GetType().ToString() != "System.DBNull") this.double_WithholdingYTDDetail += Convert.ToDouble(pRow["YTDWithholding"]);
                                else double_WithholdingYTDDetail = 0;
                                break;
                            }

                            //HTwithHoldings.Add(String_WithholdingDetailDesc,double_WithholdingMonthDetail,double_WithholdingYTDDetail);

                            ProduceOutputFilesCreateDetailMonthlyHolding();
                        }
                        ProduceOutputFilesCreateDetail(CSDetail, 2);
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

                }

                ProduceOutputFilesCreateFooters();
                //START : ML | 2019.12.19 | YRS-AT-4602 | Declare Variable
                if (parameterboolproof == true && IsPaymentOutSourcingKeyON)
                {
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("NT Payroll export file write. - START"));
                    DataTable bankMontlyPayrollData = ldataset.Tables["BankMonthlyPayrollData"];//ML | YRS-AT-4602 | 2019.11.22 | Monthly payroll data for Bank 
                    if (HelperFunctions.isNonEmpty(bankMontlyPayrollData))
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("Change file Records exist."));
                        WriteOutputFilesDataTableIntoStream(bankMontlyPayrollData);
                    }                   
                    YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("NT Payroll export file write. - END"));
                }
                //END :ML | 2019.12.19 | YRS-AT-4602 | Declare Variable            

                CloseOutputStreamFiles();
                //PJ:BT-855:YRS 5.0-1344: Create new IAT output file -  CALL TO IAT FILE
                YMCARET.YmcaDataAccessObject.IATFile objIAT = new IATFile();
                //New parameter 'parameterPayrollDate' passed to function to pass payroll date to IAT file

                if (objIAT.CreateIATFile(ParameterDatasetPayroll, C_IAT_BATCHNUMBER, parameterDateCheckDate, parameterboolproof, String_Test_Production))
                {
                    if (objIAT.GeneratedFiles != null)
                    {
                        foreach (DataRow l_DataRow in objIAT.GeneratedFiles.Rows)
                        {
                            datatable_FileList.ImportRow(l_DataRow);
                        }
                    }
                }


                return true;
            }
            catch
            {
                throw;
            }
            finally { 
            YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", "PrepareOutputFileData() END");
            YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            
            }

		}
		private void InitializseMembers()
		{
			try
			{
				this.String_PersID	= this.String_Address1 = this.String_Address2 = this.String_Address3 ="";
				this.String_Address4 = this.String_Address5 = String_CompanyData = String_Description = String_DisbursementID = "";
				this.String_DisbursementNumber = this.String_FilingStatusExemptions = this.String_FundID = "";
				this.String_IndividualID = this.String_Name22 = this.String_Name60 = this.String_ReceivingDFEAccount = "";
				this.String_EftTypeCode = this.String_BankABANumberNinthDigit ="";
				//	this.PaymentData_Addenda = ""; 
				//this.AddendaSeqNum_Addenda = "";
				//this.EntryDetailSeqNum_Addenda = "";
				this.String_DisbursementType = this.String_UsCanadaBankCode = this.String_PaymentMethodCode = this.String_BankAbaNumber ="";

				this.double_NetPayMonthTotal=this.TotalMonthlyNet=0;
				
				// PayeePayroll Variables
				this.double_GrossPayMonthNontaxable = this.double_GrossPayMonthTaxable = this.double_GrossPayMonthTotal= this.double_GrossPayYTDNontaxable = 0; 
				this.double_GrossPayYTDTaxable = this.double_GrossPayYTDTotal = this.double_NetPayYTDTotal = this.double_WithholdingMonthTotal = 0; 
				this.double_WithholdingYTDTotal = this.double_WithholdingMonthAddl = 0;

				this.double_WithholdingMonthDetail = this.double_WithholdingYTDDetail = 0;

				this.double_GrossPayDividentMonthTotal = 0;
				this.double_GrossPayDividentYTDNontaxable = 0;

				this.String_WithholdingDetailDesc=""; 

				//string lcOutputPP_1,lcOutputPP_2,lcOutputEFT_1,lcOutputEFT_2;
				//string lcOutputCursor,String_OutputFileType;

				this.double_Exemptions = 0;
				//Int32 OutputFileFormatCurrency,	OutputFilesPPDetailSum;

				//bool bool_OutputFilePayRoll,OutputFileRefund,OutputFilePosPay,OutputFileCSUS,OutputFileCSCanadian,OutputFileOutputFileRefund,OutputFileEFT_NorTrust; 

				initializeEDIVariables();
			}
			catch
			{
				throw;
			}

		}

		private void GetRowsByFilter()
			{
				/*
					DataTable myTable;
					myTable = DataSet1.Tables["Orders"];
					// Presuming the DataTable has a column named Date.
					string strExpr;
					strExpr = "Date > '1/1/00'";
					DataRow[] foundRows;
					// Use the Select method to find all rows matching the filter.
					foundRows = myTable.Select(strExpr);
					// Print column 0 of each returned row.
					for(int i = 0; i < foundRows.Length; i ++)
					{
						Console.WriteLine(foundRows[i][0]);
					}
					*/

				// Gets a NumberFormatInfo associated with the en-US culture.
				NumberFormatInfo nfi = new CultureInfo( "en-US", false ).NumberFormat;
				nfi.NumberGroupSeparator ="";    
				nfi.CurrencySymbol=""; 
				nfi.CurrencyDecimalDigits = 2;
				string vpr;
				

				// Displays a negative value with the default number of decimal digits (2).
				double myInt = 10.1;
				//Console.WriteLine( myInt.ToString( "C", nfi ) );
				vpr =myInt.ToString();
				nfi.CurrencySymbol=""; 
				vpr = myInt.ToString( "C", nfi );

				// Displays the same value with four decimal digits.
				nfi.CurrencyDecimalDigits = 4;
				//vpr = myInt.ToString( "d", nfi );
		
			
				Console.WriteLine( myInt.ToString( "C", nfi ) );
			}

		private bool ProduceOutputFilesOutput(DataTable  oDataTableOutPutfilePath)
		{
			//StreamOutputFileCSCanadian,StreamOutputFileCSUS,StreamOutputFilePosPay,StreamOutputFileEFT_NorTrust;
            string lcFileName, lcFilenameSuffix, lcOutPutFileNameBak, lcDateSuffix, lcDateTimeSuffix, lcOutputFileName, lcMetaOutputFileType, lcOutputFileNameSuffix; // Chandra sekar | 2016.07.25 | YRS-AT-2386 
            lcOutputFileName = string.Empty;
			DateTime ld_date = this.DateTime_currentDateTime;
			DataSet l_dataset= new DataSet();
			DataRowCollection rc; 
			DataRow DfNewRow;
			DataSet dsNewGuid;
            bool IsProductionEnvironment; // Chandra sekar | 2016.07.25 | YRS-AT-2386 
			object[] rowVals = new object[4];
					
			DataRow l_datarow_FileList;
            try
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", "ProduceOutputFilesOutput() START");
                rc = oDataTableOutPutfilePath.Rows;

                if (this.ProofReport) lcFilenameSuffix = "Proof";
                else lcFilenameSuffix = "";

                // START: SR | 2020.01.09 | YRS-AT-4206 | If payment outsourcing is ON then create Source/ destination files & folder for export change file too.
                //l_dataset = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.MetaOutputChkFileType();
                if (IsPaymentOutSourcingKeyON)
                {
                    l_dataset = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.MetaOutputChkFileType(CheckDate);
                }
                else { l_dataset = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.MetaOutputChkFileType(); 
                }
                // END: SR | 2020.01.09 | YRS-AT-4206 | If payment outsourcing is ON then create Source/ destination files & folder for export change file too.
                lcFileName = "";
                lcOutPutFileNameBak = "";
                lcMetaOutputFileType = "";

                if (l_dataset == null) return false;
                ld_date = this.DateTime_currentDateTime;
                lcDateSuffix = ld_date.ToString("MMddyy").Trim();
                lcDateTimeSuffix = ld_date.ToString("yyyyMMdd").Trim();
                lcDateTimeSuffix += "_" + ld_date.ToString("hhmmss").Trim();
                // START- Chandra sekar | 2016.07.25 |  YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
                // Add the "_Test" Word is the suffix BCH File name if other then the Production environment
                // EXAMPLE : If it is not a production environment then File Name : C072616_Test.bch 
                // EXAMPLE : If Production environment File Name : C072616.bch
                lcOutputFileNameSuffix = "";
                IsProductionEnvironment = YMCARET.YmcaDataAccessObject.YMCACommonDAClass.IsProductionEnvironment();
                if (!IsProductionEnvironment)
                {
                    lcOutputFileNameSuffix = "_Test";
                }
                // END- Chandra sekar | 2016.07.25 |  YRS-AT-2386 - Test files should include the word "test" within the file name (BCH file).
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

                    //						// Check whether the directory is existed or not.
                    //						if (!Directory.Exists(oRow["OutputDirectory"].ToString().Trim() ))
                    //						{
                    //							// Throw an exception.
                    //							throw new Exception("Error! Output Directory does not exists. \nFolder: " + oRow["OutputDirectory"].ToString().Trim() + " Not Found"); 
                    //							
                    //						}

                    ////						lcOutputFileName = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                    ////						lcOutPutFileNameBak = oRow["OutputDirectory"].ToString().Trim()+ "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  

                    if (this.bool_OutputFileCSCanadian && lcMetaOutputFileType == "CHKSCC")
                    {
                        //code for Original file
                        l_datarow_FileList = l_datatable_FileList.NewRow();
                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCC"]);
                        //l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        //l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList["SourceFolder"]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        //code for Backup file
                        l_datarow_FileList = l_datatable_FileList.NewRow();
                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCC"]);
                        //l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        // l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList["SourceFolder"]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }
                        //START  - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        // lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        // lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                        //END  - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCC"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFileCSCanadian = File.CreateText(lcOutputFileName);

                        this.StreamOutputFileCSCanadianBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                //rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
                                // rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                                rowVals[2] = lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileCSCanadianGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();

                            }

                        }


                    }

                    if (this.bool_OutputFileCSUS && lcMetaOutputFileType == "CHKSCU")
                    {
                        l_datarow_FileList = l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCU"]);
                        // l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        // l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList[0]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        l_datarow_FileList = l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCU"]);
                        //l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        //l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList[0]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }
                        //START  - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 
                        //lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        //lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();  
                        //END  - Commented by Chandra sekar | 2016.07.25 | YRS-AT-2386 

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["CHKSCU"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFileCSUS = File.CreateText(lcOutputFileName);

                        this.StreamOutputFileCSUSBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                //rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
                                //rowVals[2] = lcFileName.Trim() +  "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileCSUSGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();

                            }

                        }


                    }

                    //NP Added to support creation of the output streams for EDI
                    if (this.bool_OutputFileEDI_US && lcMetaOutputFileType == "EDI_US")
                    {
                        l_datarow_FileList = l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EDI_US"]);
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList[0]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        l_datarow_FileList = l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EDI_US"]);
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList[0]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EDI_US"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EDI_US"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFileEDI_US = File.CreateText(lcOutputFileName);
                        this.StreamOutputFileEDI_USBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                //rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                //this.String_OutputFileCSUSGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();  

                            }
                        }
                    }


                    if (this.bool_OutputFilePosPay && lcMetaOutputFileType == "PP")
                    {
                        //start - added by hafiz on 12April2006
                        l_datarow_FileList = this.l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PP"]);
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        if (!Directory.Exists(l_datarow_FileList["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow_FileList["SourceFolder"].ToString() + " Not Found");

                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        l_datarow_FileList = this.l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PP"]);
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        if (!Directory.Exists(l_datarow_FileList["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow_FileList["SourceFolder"].ToString() + " Not Found");

                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }
                        //end - added by hafiz on 12April2006
                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PP"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["PP"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFilePosPay = File.CreateText(lcOutputFileName);

                        this.StreamOutputFilePosPayBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();
                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                //rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFilePosPayGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();

                            }

                        }


                    }

                    if (this.bool_OutputFileEFT_NorTrust && lcMetaOutputFileType == "EFT")
                    {
                        //start of change by hafiz for implementation of datatble
                        l_datarow_FileList = this.l_datatable_FileList.NewRow();
                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EFT"]);
                        //Added by prasad:10/11/2011 For BT-645,YRS 5.0-632 : Test database output files need word "test" in them. 
                        if (String_Test_Production == "TEST")
                        {
                            l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                        }
                        else
                            if (String_Test_Production == "PRODUCTION")
                            {
                                l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                            }

                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        if (String_Test_Production == "TEST")
                        {
                            l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                        }
                        else
                            if (String_Test_Production == "PRODUCTION")
                            {
                                l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                            }

                        if (!Directory.Exists(l_datarow_FileList["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow_FileList["SourceFolder"].ToString() + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        l_datarow_FileList = this.l_datatable_FileList.NewRow();
                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EFT"]);
                        if (String_Test_Production == "TEST")
                        {
                            l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                        }
                        else
                            if (String_Test_Production == "PRODUCTION")
                            {
                                l_datarow_FileList["SourceFile"] = l_datarow_FileList["SourceFolder"].ToString() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                            }


                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        if (String_Test_Production == "TEST")
                        {
                            l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                        }
                        else
                            if (String_Test_Production == "PRODUCTION")
                            {
                                l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                            }

                        if (!Directory.Exists(l_datarow_FileList["SourceFolder"].ToString()))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + l_datarow_FileList["SourceFolder"].ToString() + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }
                        //end of change
                        //Added by prasad for YRS 5.0-632 : Test database output files need word "test" in them.

                        if (String_Test_Production == "TEST")
                        {
                            lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                            lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "_TEST" + "." + oRow["FilenameExtension"].ToString().Trim();
                        }
                        else if (String_Test_Production == "PRODUCTION")
                        {
                            lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                            lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EFT"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                        }


                        this.StreamOutputFileEFT_NorTrust = File.CreateText(lcOutputFileName.Trim());

                        this.StreamOutputFileEFT_NorTrustBack = File.CreateText(lcOutPutFileNameBak.Trim());

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {

                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                //rowVals[1] = oRow["OutputDirectory"].ToString().Trim();
                                rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileEFT_NorTrustGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();

                            }

                        }


                    }

                    // START : ML | 2019.10.22 | YRS-AT-4601 | Define file name and location for NT Payroll export file.
                    //if (this.bool_OutputFileCSUS = true && lcMetaOutputFileType == "NTPYRL" && IsPaymentOutSourcingKeyON)
                    if (outPutFileNTPyrl && lcMetaOutputFileType == "NTPYRL" && IsPaymentOutSourcingKeyON)
                    {
                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("Generate source & destination folder/file for NT Payroll export file - START"));

                        l_datarow_FileList = l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NTPYRL"]);
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList[0]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        l_datarow_FileList = l_datatable_FileList.NewRow();

                        l_datarow_FileList["SourceFolder"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NTPYRL"]);
                        l_datarow_FileList["SourceFile"] = l_datarow_FileList[0] + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                        l_datarow_FileList["DestFolder"] = oRow["OutputDirectory"].ToString().Trim();
                        l_datarow_FileList["DestFile"] = oRow["OutputDirectory"].ToString().Trim() + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        if (!Directory.Exists((string)l_datarow_FileList[0]))
                        {
                            // Throw an exception.
                            throw new Exception("Error! Output Directory does not exists. \nFolder: " + (string)l_datarow_FileList["SourceFolder"] + " Not Found");
                        }
                        else
                        {
                            this.l_datatable_FileList.Rows.Add(l_datarow_FileList);
                        }

                        lcOutputFileName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NTPYRL"]) + "\\" + lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim(); ;
                        lcOutPutFileNameBak = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["NTPYRL"]) + "\\" + lcFileName.Trim() + "_" + lcDateTimeSuffix.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();

                        this.StreamOutputFileNTPYRL = File.CreateText(lcOutputFileName);
                        this.StreamOutputFileNTPYRLBack = File.CreateText(lcOutPutFileNameBak);

                        dsNewGuid = YMCARET.YmcaDataAccessObject.MonthlyPayrollDAClass.GetNewGuid();

                        if (dsNewGuid != null)
                        {
                            if (dsNewGuid.Tables[0] != null)
                            {
                                rowVals[0] = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                                rowVals[1] = l_datarow_FileList["SourceFolder"].ToString().Trim();
                                rowVals[2] = lcFileName.Trim() + lcOutputFileNameSuffix + "." + oRow["FilenameExtension"].ToString().Trim();
                                rowVals[3] = lcMetaOutputFileType;
                                DfNewRow = rc.Add(rowVals);

                                this.String_OutputFileNTPYRLGuid = dsNewGuid.Tables[0].Rows[0]["Newguid"].ToString().Trim();
                            }
                        }

                        YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("Define file name and location for NT Payroll export file. - END"));
                    }
                    // END : ML | 2019.10.22 | YRS-AT-4601 | Define file name and location for First Annuity Payment for US Participant.

                }

                //NP -	Code checks here if the configuration entries exist in the 
                //		AtsMetaOutputFileTypes table for all files we need to create. If not then 
                //		we need to throw an exception which is caught in the main process which 
                //		eventually aborts the process.
                if (this.bool_OutputFileEDI_US && this.StreamOutputFileEDI_US == null) throw new Exception("Configuration entry not defined for Electronic Data Interchange for US cheques (EDI_US) output file in the database.");
                if (this.bool_OutputFileCSUS && this.StreamOutputFileCSUS == null) throw new Exception("Configuration entry not defined for Check Scribe US (CHKSCU) output file in the database.");
                if (this.bool_OutputFileCSCanadian && this.StreamOutputFileCSCanadian == null) throw new Exception("Configuration entry not defined for Check Scribe Canadian (CHKSCC) output file in the database.");
                if (this.bool_OutputFileEFT_NorTrust && this.StreamOutputFileEFT_NorTrust == null) throw new Exception("Configuration entry not defined for Electronic Fund Transfer (EFT) output file in the database.");
                if (this.bool_OutputFilePosPay && this.StreamOutputFilePosPay == null) throw new Exception("Configuration entry not defined for Positive Pay (PP) output file in the database.");
                if (this.outPutFileNTPyrl && this.StreamOutputFileNTPYRL == null) throw new Exception("Configuration entry not defined for NTPayroll (NTPyrl) output file in the database."); // SR | 2020.01.09 | YRS-AT-4602 | Raise error if Configuration entry not defined for NTPayroll.



                return true;
            }
            catch (Exception ex)
            {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", String.Format("Error : {0}", ex.Message));
                throw ex;
            }
            finally {
                YMCARET.CommonUtilities.WebPerformanceTracer.LogInfoTrace("Payroll Process", "ProduceOutputFilesOutput() END");
                YMCARET.CommonUtilities.WebPerformanceTracer.EndTrace();
            }

		}

		private void WriteOutputFilesDataLineIntoStream(string parameterStringOutput)
		{
			string l_String_Output = parameterStringOutput;

			try
			{		

				switch(this.String_OutputFileType)
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
                    //START :ML |2019.11.18 |YRS-AT-4602 | Write line to Output file.
                    case "NTPYRL":
                        if (StreamOutputFileNTPYRL != null)
                        {
                            StreamOutputFileNTPYRL.WriteLine(l_String_Output);
                            StreamOutputFileNTPYRLBack.WriteLine(l_String_Output);
                        }
                        break;
                    //END :ML |2019.11.18 |YRS-AT-4602 |Write line to Output file.
				}
			}
			catch
			{

				throw;
			}
				
		}

        //START :ML |2019.11.18 |YRS-AT-4602 |Write DataTable to Output file.
        public void WriteOutputFilesDataTableIntoStream(DataTable dtOutput)
        {
            try
            {
                if (HelperFunctions.isNonEmpty(dtOutput))
                {
                    this.String_OutputFileType = "NTPYRL";
                    foreach (DataRow dtRow in dtOutput.Rows)
                    {
                        WriteOutputFilesDataLineIntoStream(dtRow[0].ToString());
                    }                   
                }
            }
            catch
            {
                throw;
            }
        }
        //END :ML |2019.11.18 |YRS-AT-4602 |Write DataTable to Output file.
		//NP - Added to support writing to the output stream for EDI documents
		private void WriteOutputFilesDataLineIntoStreamEDI(string parameterStringOutput) 
		{
			try 
			{
				if (parameterStringOutput != string.Empty && parameterStringOutput != "\r\n") 
				{
					if (StreamOutputFileEDI_US!=null) StreamOutputFileEDI_US.WriteLine(parameterStringOutput);
					if (StreamOutputFileEDI_USBack!=null) StreamOutputFileEDI_USBack.WriteLine(parameterStringOutput);
				}
			} 
			catch 
			{
				throw;
			}
		}

		//NP - Added to support Exclusions for EDI
		//		Load the List for Excluding people from EDI generation
		//		The records are stored in atsPayrollEDIExclusionList Table
		//		and accessed through a stored procedure.
		private DataSet loadExclusionList() 
		{

			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			IDbConnection DBconnectYRS = null;
			DataSet ds = null;

			try 
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = l_DataBase.CreateConnection();
				DBconnectYRS.Open ();

				if (l_DataBase == null) return ds;
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_AtsPayrollEDIExcludeList_GetList");
				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) return ds;
				ds = new DataSet ("Participants_to_Exclude");
				l_DataBase.LoadDataSet (l_DBCommandWrapper,ds, "EDI_Exclusions");

			} 
			finally 
			{
				if (DBconnectYRS != null) DBconnectYRS.Close(); 
			}
			return ds;
		}

		//NP: 2007.04.26 - Returns the characters 'T' or 'P' depending on the value 
		//		of EDI_MODE key in the atsMetaConfiguration table. 
		//		'T' = Test mode and 'P' = Production mode
		//		EXCEPTIONS:
		//		Throws an exception if the EDI_MODE key is not defined. 
		//		Throws an exception if the value for the key is not a valid value.
		private DataSet getEDIModeFromDatabase() 
		{
			Database l_DataBase = null;
			DbCommand l_DBCommandWrapper = null;			
			IDbConnection DBconnectYRS = null;
			DataSet ds = null;

			try 
			{
				l_DataBase = DatabaseFactory.CreateDatabase("YRS");
				DBconnectYRS = l_DataBase.CreateConnection();
				DBconnectYRS.Open ();

				if (l_DataBase == null) throw new Exception("Unable to initialize database object");
				l_DBCommandWrapper = l_DataBase.GetStoredProcCommand ("dbo.yrs_usp_AtsMetaConfiguration_by_ConfigCategoryCode");
				l_DBCommandWrapper.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ExtraLargeConnectionTimeOut"]);
				if (l_DBCommandWrapper == null) throw new Exception("Unable to initialize command wrapper object");
                l_DataBase.AddInParameter(l_DBCommandWrapper, "@ConfigCategoryCode", DbType.String, EDI_CONFIGURATION_CATEGORY_CODE);
				ds = new DataSet ("EDI Configuration Values");
				l_DataBase.LoadDataSet (l_DBCommandWrapper,ds, "EDI CONFIGURATION VALUES");
				return ds;
			} 
			catch
			{
				throw;

			}
			finally 
			{
				if (DBconnectYRS != null) DBconnectYRS.Close(); 
			}
					
			
		}


		
		private void CloseOutputStreamFiles()
		{
			try
			{

				if (this.bool_OutputFileCSCanadian)
				{

					this.StreamOutputFileCSCanadian.Close(); 
	 
					this.StreamOutputFileCSCanadianBack.Close(); 

				}

				if (this.bool_OutputFileCSUS)
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
				//NP - Added to support printing of EDI document - Here we are closing the output stream
				if (this.bool_OutputFileEDI_US)
				{
					this.StreamOutputFileEDI_US.Close();
					this.StreamOutputFileEDI_USBack.Close();
				}
                // SR | 2020.01.09 | YRS-AT-4602 | close NT Payroll files.
                if (this.outPutFileNTPyrl) {
                    if (StreamOutputFileNTPYRL != null)
                    {
                        this.StreamOutputFileNTPYRL.Close();
                        this.StreamOutputFileNTPYRLBack.Close();
                    }
                }              
                    
                // SR | 2020.01.09 | YRS-AT-4602 | close NT Payroll files.
			}
			catch
			{
				throw;

			}
		}

		private void IdentifyPaymentTypes(DataTable ParameterDataTable)
		{
			try
			{
				DataTable l_datatable;
				string l_String_expr;
				DataRow[] foundRows;

				l_datatable = ParameterDataTable;
			
				//Poyroll
				
				this.bool_OutputFilePayRoll = true;
				
				//Refund Not used in the payroll Process eventhough it has defined.
				
				//this.OutputFileOutputFileRefund = false; 

				//Canadian Checkscribe
				l_String_expr = "CurrencyCode = 'C' and PaymentMethodCode = 'CHECK' ";
				foundRows  = l_datatable.Select(l_String_expr);
				if (foundRows.Length > 0)	this.bool_OutputFileCSCanadian  = true;
				else this.bool_OutputFileCSCanadian = false; 

				//US Checkscribe
				l_String_expr = "CurrencyCode = 'U' and PaymentMethodCode = 'CHECK' ";
				foundRows  = l_datatable.Select(l_String_expr);
				if (foundRows.Length > 0)	this.bool_OutputFileCSUS  = true;
				else this.bool_OutputFileCSUS = false; 

				// EFT (Northern Trust) Files
				l_String_expr = "PaymentMethodCode = 'EFT' ";
				foundRows  = l_datatable.Select(l_String_expr);
				if (foundRows.Length > 0)	this.bool_OutputFileEFT_NorTrust   = true;
				else this.bool_OutputFileEFT_NorTrust = false; 

				//NP - Added to support printing of EDI_US Checks
				l_String_expr = "CurrencyCode = 'U' and PaymentMethodCode = 'CHECK'";
				foundRows = l_datatable.Select(l_String_expr);
				this.bool_OutputFileEDI_US = (foundRows.Length > 0)?true:false; 

				//Positive Pay
				if (this.bool_OutputFileCSUS || this.bool_OutputFileCSCanadian) this.bool_OutputFilePosPay = true;
				else this.bool_OutputFilePosPay = false;


			}
			catch
			{
				throw;
			}
		}
    }



	}

#region "TruncateFormatter - Custom Formatter class to truncate input strings"
	public class TruncateFormatter: System.IFormatProvider, System.ICustomFormatter 
	{
		public TruncateFormatter() 
		{
		}

		// Implement the IFormatProvider Functions
		public object GetFormat(System.Type formatType)
		{
			if (typeof(ICustomFormatter).Equals(formatType)) return this;
			return null;
		}


		//Implement the ICustomFormatter Functions
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			// Make sure the object to format isn't null.
			if (arg == null) throw new ArgumentNullException("arg");

			// Make sure the format specifier is valid.
			if (format != null && arg is string)
			{
				string s = format.Trim().ToLower();
				if (s.StartsWith("t"))
					return TruncateData(arg as string, format);
			}

			// Default to the format provided by the argument to format.
			if (arg is IFormattable)
				return ((IFormattable)arg).ToString(format, formatProvider);
			else return arg.ToString();
		}


		// Function that actually truncates the data passed into the object
		string TruncateData(string data, string format) 
		{
			// Make sure the value to format isn't null.
			if (data == null) throw new ArgumentNullException("data");

			// Get the optional line width, if provided.
			int truncateLength = -1;
			try
			{
				// Attempt to parse the optional truncateLength from the format specifier.
				string s = format.Substring(1);
				truncateLength = int.Parse(s);

			}
			catch
			{
				// An invalid format was provided and we were unable to parse the truncation length
				throw new ArgumentException("Invalid Format string for provider", format);
			}

			// Make sure the truncate length is valid.
			if (truncateLength < 0) throw new ArgumentException();
			
			// Perform the actual truncation
			if (data.Length > truncateLength) 
			{
				data = data.Substring(0, truncateLength);
			}
			//Finally return the modified data string 
			return data;

		}

	}

	#endregion



	
