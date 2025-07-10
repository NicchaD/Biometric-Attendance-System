//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	CashApplicationBOClass.cs
// Author Name		:	Ruchi Saxena
// Employee ID		:	33494
// Email				:	ruchi.saxena@3i-infotech.com
// Contact No		:	8751
// Creation Time		:	10/24/2005 5:03:15 PM
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified By			 Date            Desription
//*******************************************************************************
//Shashank Patel		10-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
//Manthan Rajguru       2015.09.16       YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
using System.Data;
using System;
using YMCARET.YmcaDataAccessObject;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for CashApplicationBOClass.
	/// </summary>
	public class CashApplicationBOClass
	{
		public CashApplicationBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet LookUpYmca(string parameterYMCANo, string parameterYMCAName, string parameterCity, string parameterState)

		{
			
			try
			{
				return CashApplicationDaClass.LookUpYmca(parameterYMCANo, parameterYMCAName, parameterCity, parameterState);
			}	
			catch
			{
				throw;
			}
		}

			public static DataSet LookUpYmcaReceipts(string parameterYmcaId)
			{
				try
				{
					return CashApplicationDaClass.LookUpYmcaReceipts(parameterYmcaId);
				}
				catch
				{
					throw;
				}
			}

		public static DataSet LookUpYmcaTransmittals(string parameterYmcaId)
		{
			try
			{
				return CashApplicationDaClass.LookUpYmcaTransmittals(parameterYmcaId);
			}
			catch
			{
				throw;
			}
		}
/* Commentd by Ashish on 23-May-2008
		public static DataSet LookUpYmcaInterest(string parameterYmcaId)
		{
			try
			{
				return CashApplicationDaClass.LookUpYmcaInterest(parameterYmcaId);
			}
			catch
			{
				throw;
			}
		}
*/
		public static string LookUpYmcaCredit(string parameterYmcaId)
		{
			try
			{
				return CashApplicationDaClass.LookUpYmcaCredit(parameterYmcaId);
			}
			catch
			{
				throw;
			}
		}


		public static DataSet GetAccountingDate()
		{
			try
			{
				return CashApplicationDaClass.GetAccountingDate();
			}
			catch
			{
				throw;
			}
		}

//		public static string UpdateYmcaTransmittals(string parameterUniqueid,double parameterAmount)
//		{
//			try
//			{
//				return CashApplicationDaClass.UpdateYmcaTransmittals(parameterUniqueid,parameterAmount);
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		public static string InsertYmcaCredit(string parameterYmcaId,string parameterUniqueId,double parameterAmount,string parameterDate,string parameterAcctDate)
//		{
//			try
//			{
//				return CashApplicationDaClass.InsertYmcaCredit(parameterYmcaId, parameterUniqueId,parameterAmount, parameterDate, parameterAcctDate);
//			}
//			catch
//			{
//				throw;
//			}
//		}


//		public static string InsertYmcaAppliedRcpts(string parameterYmcaId,string parameterRcptId,string parameterTransmittalId,double parameterAmount,string parameterDate)
//		{
//			try
//			{
//				return CashApplicationDaClass.InsertYmcaAppliedRcpts(parameterYmcaId, parameterRcptId, parameterTransmittalId, parameterAmount, parameterDate);
//			}
//			catch
//			{
//				
//				throw;
//			}
//		}

//		public static string UpdateYmcaRcpts(string parameterUniqueId,string parameterTransmittalId,string parameterDate,string parameterAcctDate)
//		{
//			try
//			{
//				return CashApplicationDaClass.UpdateYmcaRcpts(parameterUniqueId, parameterTransmittalId, parameterDate, parameterAcctDate);
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		public static string InsertYMCACreditsOvrPay(string parameterYmcaId,string parameterTransmittalId,double parameterAmount,string parameterReceivedDate,string parameterReceivedAcctDate,string parameterYmcaRcptId)
//		{
//			try
//			{
//				return CashApplicationDaClass.InsertYMCACreditsOvrPay(parameterYmcaId, parameterTransmittalId, parameterAmount, parameterReceivedDate, parameterReceivedAcctDate, parameterYmcaRcptId);
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		public static string UpdatePaymentInterest(string parameterTransmittalId,string parameterDate,string parameterAcctDate,string parameterUniqueId)
//		{
//			try
//			{
//				return CashApplicationDaClass.UpdatePaymentInterest(parameterTransmittalId, parameterDate, parameterAcctDate, parameterUniqueId);
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		public static string InsertYmcaCreditsRcpts(string parameterYmcaId,string parameterTransmittalId,double parameterAmount,string parameterDate,string parameterAcctDate,string parameterYmcaRcptId)
//		{
//			try
//			{
//				return CashApplicationDaClass.InsertYmcaCreditsRcpts(parameterYmcaId, parameterTransmittalId, parameterAmount, parameterDate, parameterAcctDate, parameterYmcaRcptId);
//			}
//			catch
//			{
//				throw;
//			}
//		}
//
//		public static DataSet SelectPersonDetails(string parameterTransmittalId)
//		{
//			try
//			{
//				return CashApplicationDaClass.SelectPersonDetails(parameterTransmittalId);
//			}
//			catch
//			{
//				throw;
//			}
//		}
		//commented by Ashish on 19 May 2008 ,Start
//		public static DataTable SaveTransmittals(DataTable parameterDataTableTransmittals,DataRow parameterDataRowReceipts,DataRow parameterDataRowInterest,double parameterTotAppliedRcpts,double parameterDoubleTotInterest,string parameterStringAccountingDate)
//		{
//			try
//			{
//				
//				return CashApplicationDaClass.SaveTransmittals(parameterDataTableTransmittals,parameterDataRowReceipts,parameterDataRowInterest,parameterTotAppliedRcpts,parameterDoubleTotInterest,parameterStringAccountingDate);
//			}
//			catch
//			{
//				throw;
//			}
//		}
		//commented by Ashish on 19 May 2008 ,End
		public static DataSet SaveTransmittals(DataTable parameterDataTableTransmittals,DataRow parameterDataRowReceipts,double parameterTotAppliedRcpts,string parameterStringAccountingDate,ref bool serviceUpdate_Flag,ref Int64 logged_BatchID)
		{
			//UEINBOClass ueinBOClass=null;
			ACHDebitImportBOClass achDebitImportBOClass=null; 
			DataSet l_dsLoanPersonalDetails=null;
			DataTable l_dtFundedTransmittal=null;
			Int64 l_fundedTransmittalLogID=0;
			try
			{
				if(parameterDataTableTransmittals !=null)
				{
					 
					achDebitImportBOClass=new ACHDebitImportBOClass(); 
					l_dtFundedTransmittal=achDebitImportBOClass.GetFundedTransmittalSchema(); 
					DataTable dtNewTransactRecords=new DataTable();
					DataTable dtNewTransmittalRecords=new DataTable();
					DataRow l_dtRow=null;
					//bool flag=true;
					foreach(DataRow dtTransmittalRow in parameterDataTableTransmittals.Rows )
					{
						if(Convert.ToBoolean(dtTransmittalRow["Slctd"])==true && Math.Round(Convert.ToDecimal(dtTransmittalRow["Balance"]),2)==0 && dtTransmittalRow["FundedDate"].ToString()!=string.Empty   )
						{
//							string l_NewUeinTransmittalID=string.Empty;
//							ueinBOClass=new UEINBOClass(dtTransmittalRow["UniqueId"].ToString() ,Convert.ToDateTime(dtTransmittalRow["FundedDate"]).Date);
//							ueinBOClass.GenerateUEINRecords(ref dtNewTransactRecords,ref dtNewTransmittalRecords,flag,ref l_NewUeinTransmittalID ) ;
//							if(flag)
//							{
//								flag=false;
//							}	

							// Add row for funded transmittal Log
							l_dtRow=l_dtFundedTransmittal.NewRow();
							l_dtRow["guiTransmittalID"]=dtTransmittalRow["UniqueId"].ToString(); 
							if(parameterDataRowReceipts !=null)
							{								
								l_dtRow["guiRcptID"]=parameterDataRowReceipts["UniqueId"].ToString(); 
							}
//							l_dtRow["guiUeinTransmittalID"]=l_NewUeinTransmittalID;
							l_dtFundedTransmittal.Rows.Add(l_dtRow);  

						
						}
					}
					
					l_dsLoanPersonalDetails=CashApplicationDaClass.SaveTransmittals(parameterDataTableTransmittals,parameterDataRowReceipts,parameterTotAppliedRcpts,parameterStringAccountingDate,l_dtFundedTransmittal,ref l_fundedTransmittalLogID);

//					// Save funded Transmittal Log
//					if(l_dtFundedTransmittal.Rows.Count>0)
//					{
//						l_fundedTransmittalLogID=achDebitImportBOClass.SaveTransmittalFundedLog(null,"Cash Application",l_dtFundedTransmittal,"CASH");  
//					}
					if (l_fundedTransmittalLogID >0)
					{
						logged_BatchID=l_fundedTransmittalLogID;
						try
						{
							serviceUpdate_Flag=ServiceTimeAndVestingBOClass.ServiceTimeVestingUpdate( l_fundedTransmittalLogID); 
						}
						catch
						{
							serviceUpdate_Flag=false;
						}
					}
				}
				return l_dsLoanPersonalDetails;
			}
			catch
			{
				throw;
			}
		}


		//Shashank Patel		10-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
		#region Cash Application - Person
		
		/// <summary>
		/// This method returns participant based on selcted YMCA
		/// </summary>
		/// <param name="parameterStrGuiYmcaID">GuiYmcaID</param>
		/// <param name="parameterStrSSN">StrSSN</param>
		/// <param name="parameterStrFundNo">StrFundNo</param>
		/// <param name="parameterStrFirstName">StrFirstName</param>
		/// <param name="parameterStrLastName">StrLastName</param>
		/// <returns></returns>
		public static DataSet GetParticipantsByYmcaID(string parameterStrGuiYmcaID, string parameterStrSSN, string parameterStrFundNo, string parameterStrFirstName, string parameterStrLastName)
		{
			try
			{
				return CashApplicationDaClass.GetParticipantsByYmcaID(parameterStrGuiYmcaID, parameterStrSSN, parameterStrFundNo, parameterStrFirstName, parameterStrLastName);
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method returns the all transmittal (only Un-Funded) In which based on person's GuiFundEventID
		/// </summary>
		/// <param name="parameterStrGuiYmcaID">GuiYmcaID</param>
		/// <param name="parameterStrGuiFundEventID">GuiFundEventID</param>
		/// <returns></returns>
		public static DataSet GetTransmittalsByFundID(string parameterStrGuiYmcaID, string parameterStrGuiFundEventID)
		{
			try
			{
				return CashApplicationDaClass.GetTransmittalsByFundID(parameterStrGuiYmcaID, parameterStrGuiFundEventID);
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method returns the transaction based on transmittal GuiUniqueID and GuiFundEventID
		/// </summary>
		/// <param name="parameterStrGuiYmcaID">GuiYmcaID</param>
		/// <param name="parameterStrGuiFundEventID">GuiFundEventID</param>
		/// <param name="parameterStrGuiTransmittalID">GuiTransmittalID</param>
		/// <returns></returns>
		public static DataSet GetTransactionsByTransmittalID(string parameterStrGuiYmcaID, string parameterStrGuiFundEventID, string parameterStrGuiTransmittalID)
		{
			try
			{
				return CashApplicationDaClass.GetTransactionsByTransmittalID(parameterStrGuiYmcaID, parameterStrGuiFundEventID, parameterStrGuiTransmittalID);
			}
			catch
			{
				throw;
			}

		}

		/// <summary>
		/// This method is returns the transmittals detail Header information like mnypaid amount,total transmittal amount etc.
		/// </summary>
		/// <param name="parameterStrGuiTransmittalID">GuiTransmittalID</param>
		/// <returns>Dataset </returns>
		public static DataSet GetTransmittalDetailsByTransmittalID(string parameterStrGuiTransmittalID)
		{
			try
			{
				return CashApplicationDaClass.GetTransmittalDetailsByTransmittalID(parameterStrGuiTransmittalID);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// This method is used to fund the transaction for a person in a 
		/// transmittal and also generate UEIN & Update Vesting serivce
		/// </summary>
		/// <param name="parameterDataTableTransmittals"></param>
		/// <param name="parameterDataRowReceipts"></param>
		/// <param name="parameterDataTableTransactions"></param>
		/// <param name="parameterdoubleTotAppliedRcpts"></param>
		/// <param name="parameterStringAccountingDate"></param>
		/// <param name="parameterBoolServiceUpdateFlag"></param>
		/// <param name="parameterIntBatchID"></param>
		/// <returns></returns>
		public static DataSet SaveTransmittals(DataTable parameterDataTableTransmittals, DataRow parameterDataRowReceipts, DataTable parameterDataTableTransactions, double parameterdoubleTotAppliedRcpts, string parameterStringAccountingDate, ref bool parameterBoolServiceUpdateFlag, ref Int64 parameterIntBatchID)
		{
			ACHDebitImportBOClass achDebitImportBOClass = null;
			DataSet dsLoanPersonalDetails = null;
			DataTable dtFundedTransmittal = null;
			Int64 iFundedTransmittalLogID = 0;
			DataTable dtFinalTransaction = null;
			try
			{
				if (parameterDataTableTransmittals != null)
				{
					dtFinalTransaction = new DataTable();
					dtFinalTransaction = parameterDataTableTransactions.Clone();

					achDebitImportBOClass = new ACHDebitImportBOClass();
					dtFundedTransmittal = achDebitImportBOClass.GetFundedTransmittalSchema();

					DataTable dtNewTransactRecords = new DataTable();
					DataTable dtNewTransmittalRecords = new DataTable();
					DataRow drdtRow = null;

					foreach (DataRow dtTransmittalRow in parameterDataTableTransmittals.Rows)
					{
							// Add row for funded transmittal Log
							drdtRow = dtFundedTransmittal.NewRow();
							drdtRow["guiTransmittalID"] = dtTransmittalRow["UniqueId"].ToString();
							if (parameterDataRowReceipts != null)
							{
								drdtRow["guiRcptID"] = parameterDataRowReceipts["UniqueId"].ToString();
							}
							dtFundedTransmittal.Rows.Add(drdtRow);

					}

					//checking the transaction which has balance zero means fully paid
					foreach (DataRow drTransact in parameterDataTableTransactions.Rows)
					{
						if (Math.Round(Convert.ToDecimal(drTransact["Balance"]), 2) == 0 && drTransact["FundedDate"].ToString() != string.Empty)
						{
							drdtRow = dtFinalTransaction.NewRow();

							drdtRow["UniqueID"] = drTransact["UniqueID"].ToString();
							drdtRow["FundEventID"] = drTransact["FundEventID"].ToString();
							
							dtFinalTransaction.Rows.Add(drdtRow);
						}
					}

					//Saving the trasnmittal details
					dsLoanPersonalDetails = CashApplicationDaClass.SaveTransmittals(parameterDataTableTransmittals, parameterDataRowReceipts, dtFinalTransaction, parameterdoubleTotAppliedRcpts, parameterStringAccountingDate, dtFundedTransmittal, ref iFundedTransmittalLogID);

					if (iFundedTransmittalLogID > 0)
					{
						parameterIntBatchID = iFundedTransmittalLogID;
						try
						{
							// vesting service to handle the individual serive person
							parameterBoolServiceUpdateFlag = ServiceTimeAndVestingBOClass.ServiceTimeVestingUpdateForPerson(iFundedTransmittalLogID);
						}
						catch
						{
							parameterBoolServiceUpdateFlag = false;
						}
					}
				}
				return dsLoanPersonalDetails;
			}
			catch
			{
				throw;
			}
		}

		
		#endregion
		//Shashank Patel		10-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal 
	}
}
