/*************************************************************************************
' Copyright YMCA Retirement Fund All Rights Reserved. 
'
' Project Name		:	YMCA-YRS
' FileName			:	
' Author Name		:	Dhananjay Prajapti 
' Employee ID		:	33338	
' Email				:	dhananjay.prajapati@3i-infotech.com
' Contact No		:	55928713
' Creation Time		:	17/10/2005 
' Program Specification Name	:	
' Unit Test Plan Name			:	
' Description					:	This form is used from safeharborDisbursments 
'****************************************************************************************/
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Collections;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for SafeHarborDisbursmentBOClass.
	/// </summary>

	public class SafeHarborDisbursmentBOClass
	{
		public string  llUseit = ".T.";
		public string  llYside = ".T.";
		public string  ilVested = ".F.";
		public decimal inEmpTax;						
		public decimal inEmpNontax;
		public decimal inEmpInt;
		public decimal inYmcaTax;
		public decimal inYmcaInt;
		public decimal inYmcaAmtAvail;
		public DataTable l_obj_DataTable;
		public DataTable l_obj_RefundDataTable ;
		public DataTable l_obj_r_RefRequests;
		public DataTable l_obj_C_temp;
		public DataTable l_obj_r_DisbursementsFunding;
		public DataTable l_obj_r_DisbursementsRefunds;
		public DataTable l_obj_r_DisbursementsDetailsWithHolding;
		public DataTable l_obj_r_DisbursementsDetails;
		public DataTable l_obj_r_Disbursements;
		public DataTable l_obj_r_ViewTransactions;
		public DataTable l_obj_r_ViewRefRequestsDetails;
		public decimal inTaxable ;
		public decimal inNonTaxable;
		public decimal inGross;
		public string llCancel="";
		public string icSsno="";
		public string icLastName="";
		public string icFirstName="";
		public string icPayee1="";
		public string icMiddleName="";
		public string idBirthDate="";
		public DataTable c_shiraDetails;
		public DataTable c_SHiraHeader;
		public DataTable c_Requested;
		public DataSet l_obj_Dataset = new DataSet();
		public double  lnGrandTotal=0;
		public string lcSql ="";
		public string lcFilePath ="";
		public string lcSendTo="";
		public string lcDateFormat = DateTime.Now.ToString();
		public string lcMarkValue ="-";
		public DataTable R_Transactions;
		//Variable declare for create disbursment 
		public int  lnMonth;
		public string lcMessage,lcTemp;
		public string lcFundEventPK, lcRefRequestDetailsPK,	RefRequestDetailsID, lcRefRequestsPK, lcsql;
		public double llFound,	llFirst,	lgPersID, lcSelect, lnx, lnz, lnTaxableCtr,	lnTaxCtr,lnNonTaxCtr, lnNum,	lcPersPK, ldTermDate,	lcDisbursementifFK,	lcDistPK;		
		public string lcAcctBreakdownType;
		public double lnSortOrder, lnemptax, lnempnontax, laTemp,ldTotalAmount;
		public int lnempint, lnymcatax, lnymcaint, lnEmpTotal, lnYmcaTotal,	 lnTermTime, lcTermTime;
		public int lnFedTax, lnDisbTaxable, lnDisbNonTaxable, lcRecID, lcEmpEventPK,  lnDisbursementsCtr;
		public string idAcctDate = "";
		public string idRequestDate = "";
		public DataTable c_RefRequests;
		public string iadisbrese = "";
		public string iadisbdetailsreset = "";
		public string iarefundsrest = "";
		public string iatransreset = "";
		public string icYmcaID = "";
		string[,] iaBasis = new string[1,1];
		//ArrayList iaBasis  = new ArrayList();
		public string icStreet1,icStreet2,icCity,icState,icZip,icPayeeAddrID,icCurrency ;
		string[,] arRefundable = new string[1,4];
		//ArrayList arRefundable = new ArrayList();
		public int Lnx = 0;
		public string l_string_fundrequestID="";
		
		
			


               
		public SafeHarborDisbursmentBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region "Method to Get Account Date"
		public string GetAcctDateBL()
		{
			return "";

		}
		#endregion 
		#region  "Method to Create Disbursement"
		public string  CreateDisbursementBL(string l_string_Refundtype)
		{
			bool l_bool_updateflg;
			DateTime dt = new DateTime();
			int l_int_Month;
			string l_return_flg ="";
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			dt = Convert.ToDateTime(GetAccountDateBL());
			idRequestDate = dt.ToShortDateString();
			l_int_Month = dt.Month ;
			l_int_Month = l_int_Month - 3;
			DateTime l_dt_RequestDate = Convert.ToDateTime(idRequestDate);
			lnMonth = l_dt_RequestDate.Month    ;
			//Call for Create cursor 
			//CreateCursorShiraDetails();
			if (lnMonth == l_int_Month)
				dt = l_dt_RequestDate.AddDays(1)  ;
			else
				dt = l_dt_RequestDate.AddDays(-1);

			c_RefRequests = LookupRefRequestsBL(dt.ToShortDateString(),l_string_Refundtype);
			if (c_RefRequests.Rows.Count < 1)
				//if ".T." Show message "There is no pending cashout SHIRA Request to Process"
				l_return_flg = ".F.";
			else
			{
				l_return_flg = ".T.";
				return ".F.";
			}


			iadisbrese = ".f.";
			iadisbdetailsreset = ".f.";
			iarefundsrest = ".f.";
			iatransreset = ".f.";
			foreach (DataRow l_obj_RefRequest in c_RefRequests.Rows)
			{
				// check for MemberHasdied 
				l_string_fundrequestID = l_obj_RefRequest["FundEventID"].ToString();
				string l_PersID = l_obj_RefRequest["PersID"].ToString() ;
				if (LookupMemberHasDiedBL(l_PersID) == ".F.")
				{
					l_bool_updateflg = Update_BalanceCheckBL(l_obj_RefRequest["UniqueID"].ToString());
					if (l_bool_updateflg == false)
					{
						l_return_flg = ".F.";
					}
				}
				//Check for ReEmployed 
				if (LookupMemberHasDiedBL(l_obj_RefRequest["PersID"].ToString()) == ".F.")
				{
					l_bool_updateflg = Update_BalanceCheckBL(l_obj_RefRequest["UniqueID"].ToString());
					if (l_bool_updateflg == false)
					{
						l_return_flg = ".F.";
					}
				}
				//Check for PendingQDRO
				if (LookupPendingQDROBL(l_obj_RefRequest["PersID"].ToString()) == ".F.")
				{
					l_bool_updateflg = Update_BalanceCheckBL(l_obj_RefRequest["UniqueID"].ToString());
					if (l_bool_updateflg == false)
					{
						l_return_flg = ".F.";
					}
				}
				//check for Vesting Status 
				if (LookupMemberHasDiedBL(l_obj_RefRequest["FundEventID"].ToString()) == ".F.")
				{
					l_return_flg = ".F.";
				}
				//check for Balance Check 
				//Check for Cash out disbursement 
				if (l_string_Refundtype != "CASHOU")
				{
					if (LookupBalanceCheckBL(l_obj_RefRequest["FundEventID"].ToString(), ilVested)==".F.")
					{
						l_bool_updateflg = Update_BalanceCheckBL(l_obj_RefRequest["UniqueID"].ToString());
						if (l_bool_updateflg == false)
						{
							l_return_flg = ".F.";
						}
					}
				}
				//check should be there that if RequestStatus <> "Pend" then only loop
				//Check for Create Cursor 
				if (CreateCursorShiraDetails() == false)
				{
					l_return_flg = ".F.";
				}
				//Note OPEN File code will be added here 
				if (OpenFileBL(l_obj_RefRequest["FundEventID"].ToString()) == ".F.")
				{
					l_return_flg = ".F.";
				}
				
				// check for getCurrentBalance
				if (LookupCurrentBalanceBL(l_obj_RefRequest["FundEventID"].ToString()) == ".F.")
				{
					l_return_flg = ".F.";
				}
				lnFedTax = 0;
				lnFedTax = 0;
				lnDisbTaxable = 0;
				lnDisbNonTaxable = 0;
				lcDisbursementifFK= 0.00;
				lnz= 1;
				lnNum= 1;
				llFirst = 0.00;
				lnemptax = 0.00;
				lnempnontax =0.00;
				lnempint = 0;
				lnymcatax = 0;
				lnymcaint = 0;
				lnTaxableCtr = 0;
				lnTaxCtr = 0.00;
				lnNonTaxCtr = 0.00;
				lgPersID = 0.00;
				lnNum = 0.00;
				lcPersPK = 0.00;
				lcDistPK = 0.00; 

				//Call for Available Amunity 
				if (LookupMetaAnnuityBasisTypes()==".F.")
				{
					l_return_flg = ".F."; 

				}
				//Call for Lookup History Address
				if (LookupAddressHistory(l_obj_RefRequest["AddressID"].ToString())==".F.")
					l_return_flg =  ".F.";
				else
					l_return_flg = ".T.";
				//Call for Currency Code 
				if (LookupPersBankingBeforeEffDate(DateTime.Now, l_obj_RefRequest["PersID"].ToString()) == ".F.")
				{
					l_return_flg = ".F.";
				}

				//Call updateShiraDetails method 
				if (l_string_Refundtype != "CASHOU")
				{
					if (UpdateShiraDetails(ldTotalAmount) == ".F.")
						return ".F.";
				}

				//Call update 
				if (Update_StatusBL(l_obj_RefRequest["FundEventID"].ToString())== false)
				{
					return ".F.";
				}
				//Call function to Call Bank File 
				if (l_string_Refundtype != "CASHOU")
				{
					if (CreateBankFile() == ".F.")
						l_return_flg = ".F.";
					else
						l_return_flg = ".T.";
				}

				//CALL FOR UPDATE FUNDDISVURSEMENT
				if (l_string_Refundtype != "CASHOU")
				{
					if (UpdateFundDisbursement()==false)
						l_return_flg =".F.";
					else
						l_return_flg = ".T.";
				}

			}
	

			return l_return_flg;

		}
		#endregion
		#region "Look up for LookupCurrentBalance"
		public string  LookupCurrentBalanceBL(string l_string_RefRequest_FundEventID)
		{
			DataSet l_obj_DataSet;
			l_obj_DataTable = new DataTable();
			DataColumn l_obj_DataColumn;
			
			
			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_DataSet = l_obj_SafeHarborDisbursementDAClass.LookupCurrentBalance(l_string_RefRequest_FundEventID);
				l_obj_DataTable = l_obj_DataSet.Tables["TransferSumForRefund"];
				l_obj_DataColumn = new DataColumn("PersonalTotal",System.Type.GetType("System.Decimal"));
				l_obj_DataTable.Columns.Add(l_obj_DataColumn);
				l_obj_DataColumn = new DataColumn("YmcaTotal",System.Type.GetType("System.Decimal"));
				l_obj_DataTable.Columns.Add(l_obj_DataColumn);
				l_obj_DataColumn = new DataColumn("TotalTotal",System.Type.GetType("System.Decimal"));
				l_obj_DataTable.Columns.Add(l_obj_DataColumn);
				foreach(DataRow l_obj_DataRow in l_obj_DataTable.Rows)
				{
					if(l_obj_DataRow.HasErrors)
						return ".F.";
					else
					{
						l_obj_DataRow["YmcaTotal"] = (decimal)l_obj_DataRow["mnyYmcaPreTax"] +  (decimal)l_obj_DataRow["mnyYMCAInterestBalance"];
						l_obj_DataRow["PersonalTotal"] = (decimal)l_obj_DataRow["mnyPersonalPreTax"] +  (decimal)l_obj_DataRow["mnyPersonalPostTax"] + (decimal)l_obj_DataRow["mnyPersonalInterestBalance"];
						l_obj_DataRow["TotalTotal"] = (decimal)l_obj_DataRow["YmcaTotal"]  + (decimal) l_obj_DataRow["PersonalTotal"];
						l_obj_DataTable.AcceptChanges();
					}


				}
				foreach(DataRow l_obj_DataRow in l_obj_DataTable.Rows)
				{
					string l_string_AccountType =  (string) l_obj_DataRow["chrAcctType"];
					string l_string_account = LookupAccountTypeBL(l_string_AccountType);
					switch (l_string_account)
					{
						case ".T.":
							llUseit = ".T.";
							if (ilVested == ".T.")
							{
								llYside = ".T.";
							}
							else
							{
								llYside =".F.";
							}
							break;
					}

					switch(l_string_AccountType)
					{
						
						case "AP" :
							llUseit = ".T.";
							llYside = ".F.";
							break;
						case "TD" :
							llUseit =".T.";
							llYside =".F.";
							break;
						case "TM" :
							llUseit =".T.";
							llYside =".T.";
							break;
						case "RP" :
							llUseit =".T.";
							llYside =".F.";
							break;
						case "RT" :
							llUseit =".T.";
							llYside =".F.";
							break;
						case "AM" :
							llUseit = ".T.";
							llYside = ".T.";
							break;
						case "SR" :
							if (ilVested == ".T.")
							{
								llUseit = ".T.";
								llYside = ".T.";
							}
							else
							{
								llUseit =".F.";
								llYside =".F.";
							}
							break;



					}
					if (llUseit == ".F.")
						l_obj_DataRow.Delete();
					if (llYside == ".F.")
					{
						l_obj_DataRow["mnyYmcaPreTax"] = 0.00;
						l_obj_DataRow["mnyYMCAInterestBalance"] = 0.00;
						l_obj_DataRow["TotalTotal"] = (decimal) l_obj_DataRow["TotalTotal"] -  (decimal)l_obj_DataRow["YmcaTotal"] ;
						l_obj_DataRow["YmcaTotal"] = 0.00;
						l_obj_DataTable.AcceptChanges();

						// other variables
						l_obj_DataRow["YmcaTotal"] = (decimal)l_obj_DataRow["mnyYmcaPreTax"] +  (decimal)l_obj_DataRow["mnyYMCAInterestBalance"];
						l_obj_DataRow["PersonalTotal"] = (decimal)l_obj_DataRow["mnyPersonalPreTax"] +  (decimal)l_obj_DataRow["mnyPersonalPostTax"] +  + (decimal)l_obj_DataRow["mnyPersonalInterestBalance"];
						l_obj_DataRow["TotalTotal"] = (decimal)l_obj_DataRow["YmcaTotal"]  + (decimal) l_obj_DataRow["PersonalTotal"];
		
						l_obj_DataTable.AcceptChanges();
			
					}
					inEmpTax = inEmpTax + (decimal) l_obj_DataRow["mnyPersonalPreTax"];
					inEmpNontax =inEmpNontax + (decimal)l_obj_DataRow["mnyPersonalPostTax"];
					inEmpInt = inEmpInt + (decimal)l_obj_DataRow["mnyPersonalInterestBalance"];
					inYmcaTax = inYmcaTax + (decimal)l_obj_DataRow["mnyYmcaPreTax"];
					inYmcaInt = inYmcaInt + (decimal)l_obj_DataRow["mnyYMCAInterestBalance"];
					
					/*
					 * Call to function LookupRefundAmount
					 */
					LookupRefundAmountBL(l_obj_DataTable);
//					foreach(DataRow l_obj_DataRowRefund in l_obj_DataTable.Rows)
//					{
//
//					}

					
				}
				return ".T.";




				
			}
			catch(Exception ex)
			{
				throw ex;
			}

		}
		#endregion
		#region "Look up for LookupAccountType"
		public string LookupAccountTypeBL(string l_string_AccountType)
		{
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			return l_obj_SafeHarborDisbursementDAClass.LookupAccountType(l_string_AccountType);

		}
		#endregion
		#region "Look up for LookupRefundAmount"
		public void  LookupRefundAmountBL(DataTable  l_obj_DataTableRefund)
		{
			l_obj_RefundDataTable = new DataTable();
			l_obj_RefundDataTable = l_obj_DataTableRefund.Clone();
			inYmcaAmtAvail =0;
			foreach(DataRow l_obj_DataRowRefund in l_obj_RefundDataTable.Rows)
			{
				string l_string_AccountType =  (string) l_obj_DataRowRefund["chrAcctType"];
				string l_string_account = LookupAccountTypeBL(l_string_AccountType);
				switch (l_string_account)
				{
					case ".T.":
						llUseit = ".T.";
						if (ilVested == ".T.")
						{
							llYside = ".T.";
						}
						else
						{
							llYside =".F.";
						}
						break;
				}

				switch(l_string_AccountType)
				{
						
					case "AP" :
						llUseit = ".T.";
						llYside = ".F.";
						break;
					case "TD" :
						llUseit =".T.";
						llYside =".F.";
						break;
					case "TM" :
						llUseit =".T.";
						llYside =".T.";
						break;
					case "RP" :
						llUseit =".T.";
						llYside =".F.";
						break;
					case "RT" :
						llUseit =".T.";
						llYside =".F.";
						break;
					case "AM" :
						llUseit = ".T.";
						llYside = ".T.";
						break;
					case "SR" :
						if (ilVested == ".T.")
						{
							llUseit = ".T.";
							llYside = ".T.";
						}
						else
						{
							llUseit =".F.";
							llYside =".F.";
						}
						break;



				}
				if (llUseit == ".F.")
					l_obj_DataRowRefund.Delete();
				if (llYside == ".F.")
				{
					l_obj_DataRowRefund["mnyYmcaPreTax"] = 0.00;
					l_obj_DataRowRefund["mnyYMCAInterestBalance"] = 0.00;
					l_obj_DataRowRefund["TotalTotal"] = (decimal) l_obj_DataRowRefund["TotalTotal"] -  (decimal)l_obj_DataRowRefund["YmcaTotal"] ;
					l_obj_DataRowRefund["YmcaTotal"] = 0.00;
					l_obj_RefundDataTable.AcceptChanges();

					// other variables
					l_obj_DataRowRefund["YmcaTotal"] = (decimal)l_obj_DataRowRefund["mnyYmcaPreTax"] +  (decimal)l_obj_DataRowRefund["mnyYMCAInterestBalance"];
					l_obj_DataRowRefund["PersonalTotal"] = (decimal)l_obj_DataRowRefund["mnyPersonalPreTax"] +  (decimal)l_obj_DataRowRefund["mnyPersonalPostTax"] +  + (decimal)l_obj_DataRowRefund["mnyPersonalInterestBalance"];
					l_obj_DataRowRefund["TotalTotal"] = (decimal)l_obj_DataRowRefund["YmcaTotal"]  + (decimal) l_obj_DataRowRefund["PersonalTotal"];
		
					l_obj_RefundDataTable.AcceptChanges();
			
				}
				inTaxable = 0;
				inNonTaxable	= 0;
				inGross		= 0;
			
			}
			
	
		}
		#endregion
		
		#region "Look up for LookupVestingStatus"
		public string  LookupVestingStatusBL(string l_string_EventID)
		{
		   
			DataSet l_obj_VestingDataSet = new DataSet();
			l_obj_C_temp = new DataTable();
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			l_obj_VestingDataSet = l_obj_SafeHarborDisbursementDAClass.LookupVestingStatus(l_string_EventID);
			l_obj_C_temp = l_obj_VestingDataSet.Tables["VestingStatus"];
			if (l_obj_C_temp.Rows.Count < 1 )
			{
				return ".F.";
			}
			if (l_obj_C_temp.Rows.Count != 1 )
			{
				return ".F.";

			}
			foreach(DataRow l_obj_DatarowVesting in l_obj_C_temp.Rows)
			{
				if ((string)l_obj_DatarowVesting["VestingDate"] == "" )
					ilVested = ".F.";
				else
					ilVested = ".T.";
			}
			return ""; 
		}
		#endregion
		#region "Look up for LookBalanceCheck"
		public string LookupBalanceCheckBL(string l_string_EventID, string l_string_VestingFlg)
		{
			
			DataSet l_obj_BalanceCheck = new DataSet();
			l_obj_C_temp = new DataTable();
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			l_obj_BalanceCheck = l_obj_SafeHarborDisbursementDAClass.LookupBalanceCheck(l_string_EventID,l_string_VestingFlg);
			l_obj_C_temp = l_obj_BalanceCheck.Tables["BalanceCheck"];
			if (l_obj_C_temp.Rows.Count < 1 )
			{
				return ".F.";
			}
			foreach(DataRow l_obj_DatarowBalanceCheck in l_obj_C_temp.Rows)
			{
				double l_decimal_Balance = Convert.ToDouble(l_obj_DatarowBalanceCheck["Balance"]);
				if (l_decimal_Balance < 0.01)
					llCancel = ".T.";
				if (l_decimal_Balance < 1000.00)
					llCancel = ".T.";
				if (l_decimal_Balance < 4999.99)
					llCancel = ".T.";
				else
					llCancel = ".F.";				
			  
			}
			if (llCancel == ".T.") 
			{
				bool l_bool_checkupdate;
//				string l_string_UnqiueID="";
				//Vipul, the fundEvent ID was not passed 
				//l_bool_checkupdate = Update_BalanceCheckBL(l_string_UnqiueID); //Note : here u have to pass the unique id from datarows
				l_bool_checkupdate = Update_BalanceCheckBL(l_string_EventID); 
				// IMP : here u need to update the RequestStatus  with cancel 
			}

			
			return "" ;
			
		}
		#endregion
		#region "Look up for LookBalanceCheck"
		public bool Update_BalanceCheckBL(string l_string_RefRequest_UnqiueID)
		{
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			return l_obj_SafeHarborDisbursementDAClass.Update_BalanceCheck(l_string_RefRequest_UnqiueID);
		
			
		}
		#endregion
		#region "Look up for number of member had died"
		public string LookupMemberHasDiedBL(string l_string_RefRequest_PersID)
		{
			string l_string_flg="";
			DataSet l_obj_TempDataSet = new DataSet();
			l_obj_C_temp = new DataTable();
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			l_obj_TempDataSet = l_obj_SafeHarborDisbursementDAClass.LookupMemberHasDied(l_string_RefRequest_PersID);
			l_obj_C_temp = l_obj_TempDataSet.Tables["MemberHasDied"];
			if (l_obj_C_temp.Rows.Count < 1)
			{
				l_string_flg =  ".F.";
			}
			foreach(DataRow l_obj_MemberHasDiedRow in l_obj_C_temp.Rows)
			{
				
				icSsno = (string) l_obj_MemberHasDiedRow["ssno"];
				icLastName	=	(string) l_obj_MemberHasDiedRow["LastName"];
				icFirstName	=	(string) l_obj_MemberHasDiedRow["FirstName"];
				icMiddleName	=	(string) l_obj_MemberHasDiedRow["MiddleName"];
				idBirthDate	=	 l_obj_MemberHasDiedRow["BirthDate"].ToString();
				icPayee1 = (string) l_obj_MemberHasDiedRow["FirstName"];
				icPayee1 = icPayee1 +  " "  + (string) l_obj_MemberHasDiedRow["MiddleName"];
				icPayee1 = icPayee1 +  " "  +  (string) l_obj_MemberHasDiedRow["LastName"];
				string l_obj_BirthDate =  l_obj_MemberHasDiedRow["BirthDate"].ToString();
				if (l_obj_BirthDate != "" )
				{
					bool l_update_flg;
					l_update_flg = Update_BalanceCheckBL(l_string_RefRequest_PersID);
					//Call update method 
					if (l_update_flg == true)
					{
						//Message here "Unable to Update vrRefRequests Table"
						l_string_flg = ".T.";
					}
					else
					{
						l_string_flg = ".F.";
					}

					/*
					 * NOTE: THIS CODE NEED TO BE WRITTEN : UPDATE THE DATASET  c_RefRequests
					 */

					//SELECT c_RefRequests
					//REPLACE RequestStatus WITH 'CANCEL'
				}
			}
		
			return l_string_flg ;
			

		}
		#endregion
		#region "Look up for update status"
		public bool Update_StatusBL(string l_string_FundEventID)
		{
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			return  l_obj_SafeHarborDisbursementDAClass.Update_Status(l_string_FundEventID);
			// Note : if true : Data update 
			//			 false: Data not updated 
		}
		#endregion
		#region "Create Cursor c_ShiraDetails"
		public bool CreateCursorShiraDetails()
		{
			try
			{

				//CREATE DATATABLE c_shiraDetails AND ADDED THE FOLLOWING COLOUMN INTO THE TABLE 
				c_shiraDetails = new DataTable("ShiraDetails");
				DataColumn l_obj_ShiraDataColumn;
				l_obj_ShiraDataColumn = new DataColumn("Plan_Sponser_ID",System.Type.GetType("System.Int32"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
				l_obj_ShiraDataColumn = new DataColumn("Admin_ID",System.Type.GetType("System.Int32"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
				l_obj_ShiraDataColumn = new DataColumn("Transfer_Type",System.Type.GetType("System.Char"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("Plan_ID",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
				l_obj_ShiraDataColumn = new DataColumn("Amount",System.Type.GetType("System.Decimal"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
				l_obj_ShiraDataColumn = new DataColumn("Ssn",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
				l_obj_ShiraDataColumn = new DataColumn("Birth_Dt",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				//			l_obj_ShiraDataColumn = new DataColumn("Birth_Dt",System.Type.GetType("System.String"));
				//			c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("First_nm",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("Middle_Nm",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("Last_Nm",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("Addr_Line1",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
			
				l_obj_ShiraDataColumn = new DataColumn("Addr_Line2",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("City",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("State",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);

				l_obj_ShiraDataColumn = new DataColumn("ZipCode",System.Type.GetType("System.String"));
				c_shiraDetails.Columns.Add(l_obj_ShiraDataColumn);
				l_obj_Dataset.Tables.Add(c_shiraDetails); //ADDED TABLES IN THE DATASETS

				// Create Datatable c_SHiraHeader and added following fields in the datatable 

				c_SHiraHeader = new DataTable("ShiraHeader");
				DataColumn l_obj_ShiraHeaderDataColumn;
				l_obj_ShiraHeaderDataColumn = new DataColumn("Transfer_Type",System.Type.GetType("System.Char"));
				c_SHiraHeader.Columns.Add(l_obj_ShiraHeaderDataColumn);
				l_obj_ShiraHeaderDataColumn = new DataColumn("Plan_Sponsor_ID",System.Type.GetType("System.Int32"));
				c_SHiraHeader.Columns.Add(l_obj_ShiraHeaderDataColumn);
				l_obj_ShiraHeaderDataColumn = new DataColumn("Admin_ID",System.Type.GetType("System.Int32"));
				c_SHiraHeader.Columns.Add(l_obj_ShiraHeaderDataColumn);
				l_obj_ShiraHeaderDataColumn = new DataColumn("Total_Accounts",System.Type.GetType("System.Decimal"));
				c_SHiraHeader.Columns.Add(l_obj_ShiraHeaderDataColumn);
				l_obj_ShiraHeaderDataColumn = new DataColumn("Total_Amt",System.Type.GetType("System.Decimal"));
				c_SHiraHeader.Columns.Add(l_obj_ShiraHeaderDataColumn);
				l_obj_Dataset.Tables.Add(c_SHiraHeader); //ADDED TABLES IN THE DATASETS

				//Create Datatable c_Requested  and added following column in the datatable 
				c_Requested  = new DataTable("c_Requested");
				DataColumn l_obj_c_RequestedDataColumn;

				l_obj_c_RequestedDataColumn = new DataColumn("AcctType",System.Type.GetType("System.Char"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("PersonalPostTax",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("PersonalPreTax",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("PersonalInterest",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("PersonalTotal",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);

				l_obj_c_RequestedDataColumn = new DataColumn("YmcaPreTax",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("YmcaInterest",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("YmcaTotal",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);
				l_obj_c_RequestedDataColumn = new DataColumn("TotalTotal",System.Type.GetType("System.Decimal"));
				c_Requested.Columns.Add(l_obj_c_RequestedDataColumn);

				l_obj_Dataset.Tables.Add(c_Requested); //ADDED TABLES IN THE DATASETS

				return true; //True : indicate Cursor is created 
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		#endregion
		#region "Create Cursor c_ShiraDetails"
		public string  CreateBankFile()
		{
			try
			{
				foreach(DataRow l_obj_CreateBankFile  in c_shiraDetails.Rows)
				{
					lnGrandTotal = lnGrandTotal + Convert.ToDouble ( l_obj_CreateBankFile["Amount"]);
					l_obj_CreateBankFile["Plan_Sponser_ID"] = 135562401;
					l_obj_CreateBankFile["Admin_ID"] = 135562401;
					l_obj_CreateBankFile["Transfer_Type"] = 'N';
					l_obj_CreateBankFile["Plan_ID"] = "S135562401";
					c_shiraDetails.AcceptChanges(); 
				}
				//Call for LookupMetaOutputFile to Create CSV File 
				if (LookupMetaOutputFileTypesBL(c_shiraDetails)== ".T.")
					return ".T.";
				else
					return ".F.";
				}
			catch
			{
				throw;
			}

		}
		#endregion
		#region "Look up for LookupMetaOutputFileTypes"
		public string LookupMetaOutputFileTypesBL(DataTable l_obj_BankDataTable)
		{
			
			l_obj_C_temp = new DataTable();
			DataSet l_obj_MetaOutputDataSet = new DataSet();
			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_MetaOutputDataSet = l_obj_SafeHarborDisbursementDAClass.LookupMetaOutputFileTypes();
				l_obj_C_temp = l_obj_MetaOutputDataSet.Tables["MetaOutputFileTypes"];
				if (l_obj_C_temp.Rows.Count < 1)
				{
					lcDateFormat = DateTime.Now.ToString();
					lcMarkValue = lcMarkValue; 
					return ".F.";
				}
				foreach(DataRow l_obj_datarowtemp in l_obj_C_temp.Rows)
				{
					string l_string_outputdirectory = (string)l_obj_datarowtemp["chvOutPutDirectory"];
					if (l_string_outputdirectory == "" ) 
					{
						return ".F.";

					}
					lcFilePath = l_string_outputdirectory + "\\" ;
				
				}
				//called function WriteCSV to convert datatable to CSV file 
				if (WriteCSVBL(l_obj_BankDataTable,lcFilePath) == ".T.") 
					return ".T.";
				else
					return ".F.";
			}
			catch  (Exception ex)
			{
				throw (ex);
			}
	
		}
		#endregion
		#region "Function to convert datatable to CSV"
		public string  WriteCSVBL(DataTable l_obj_DatatableCSV,string l_string_filename)
		{
			
			StreamWriter l_obj_StreamWriter;
			string filePath = l_string_filename + ".csv";

			//File.Delete(filePath);
			l_obj_StreamWriter = File.CreateText(filePath);	

			try
			{
				// write the data in each row & column
				foreach (DataRow l_obj_row in l_obj_DatatableCSV.Rows)
				{
					
					StringBuilder rowToWrite = new StringBuilder();

					for (int counter = 0; counter <= l_obj_DatatableCSV.Columns.Count - 1 ; counter++)
					{
						rowToWrite.Append("'" + l_obj_row[counter] + "'");
					}

					rowToWrite.Replace("''","','");

					rowToWrite.Append("\r\n");
					l_obj_StreamWriter.Write(rowToWrite);
				}
				return ".T.";
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				l_obj_StreamWriter.Close();
			}
		}
		#endregion
		#region "Look up for LookupPendingQDRO"
		public string LookupPendingQDROBL(string l_string_RefRequest_PersID)
		{
		
			DataSet l_obj_PendingQDRO = new DataSet();
			l_obj_C_temp = new DataTable();
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			l_obj_PendingQDRO = l_obj_SafeHarborDisbursementDAClass.LookupPendingQDRO(l_string_RefRequest_PersID);
			l_obj_C_temp = l_obj_PendingQDRO.Tables["PendingQDRO"];
//			if (l_obj_C_temp.Rows.Count < 1 )
//				return ".F.";
//			else
//				return ".T.";
			if (l_obj_C_temp.Rows.Count < 1 )
				return ".T.";
			else
			{
				
				bool l_bool_checkupdate;
				string l_string_UnqiueID="";
				l_bool_checkupdate = Update_BalanceCheckBL(l_string_UnqiueID); //Note : here u have to pass the unique id from datarows
					
				if (l_bool_checkupdate == false)
					return ".F."; // update fail 
				else
					return ".T."; // updated
				// IMP : here u need to update the RequestStatus  with cancel 
				

			}
			
			
		}
		#endregion
		#region "Look up for ReEmployed"
		public string LookupReEmployedBL(string l_string_RefRequest_PersID)
		{
			
			DataSet l_obj_ReEmployed = new DataSet();
			l_obj_C_temp = new DataTable();
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			l_obj_ReEmployed = l_obj_SafeHarborDisbursementDAClass.LookupReEmployed(l_string_RefRequest_PersID);
			l_obj_C_temp = l_obj_ReEmployed.Tables["ReEmployed"];
//			if (l_obj_C_temp.Rows.Count < 1 )
//				return ".F.";
//			else
//				return ".T.";
			if (l_obj_C_temp.Rows.Count < 1 )
				return ".T.";
			else
			{
				
				bool l_bool_checkupdate;
				string l_string_UnqiueID="";
				l_bool_checkupdate = Update_BalanceCheckBL(l_string_UnqiueID); //Note : here u have to pass the unique id from datarows
					
				if (l_bool_checkupdate == false)
					return ".F."; // update fail 
				else
					return ".T."; // updated
				// IMP : here u need to update the RequestStatus  with cancel 
				

			}
			

		}
		#endregion
		#region "Look up for ReEmployed"
//		public int RefundIntrest0BL(DataRow  parameterSelectedDataRow , DataRow  parameterAvailableBalance, string parameterAccountType , int parameterIndex ,string parameterCurrencyCode , string  parameterTransactionType )
//		{
//			DataRow l_SelectedDataRow;
//			DataRow l_AvailableBalance;
//			string l_AccountBreakDownType;
//			string l_AccountType;
//			int l_SortOrder;
//			Decimal l_Decimal_PersonalPreTax;
//			Decimal l_Decimal_PersonalPostTax;
//			Decimal l_Decimal_YMCAPreTax;
//			Decimal l_Decimal_TaxRate;
//			Decimal l_Decimal_SelectedTaxableAmount;
//			Decimal l_Decimal_SelectedNonTaxableAmount;
//			DataRow l_DisbursementDataRow;
//			DataRow l_DisbursementDetailsDataRow;
//			DataRow l_RefundsDataRow;
//			DataRow l_TransactionDataRow;
//			// 'For Setup Transactions Array
//
//			// ' Assign the Values. 
//			l_SelectedDataRow = parameterSelectedDataRow;
//			l_AvailableBalance = parameterAvailableBalance;
//			l_AccountType = parameterAccountType;
//			if ((l_SelectedDataRow == null)) 
//			{
//				return 0;
//			}
//			if ((l_AvailableBalance == null)) 
//			{
//				return 0;
//			}
//			l_Decimal_SelectedTaxableAmount = ((l_SelectedDataRow["Taxable"] == "System.DBNull" )) ? 0 : ((decimal)(l_SelectedDataRow["Taxable"]) );
//			l_Decimal_SelectedNonTaxableAmount = ( (l_SelectedDataRow["NonTaxable"] == "System.DBNull") ? 0 : ((decimal)(l_SelectedDataRow["NonTaxable"])) );
//			l_Decimal_TaxRate = ( (l_SelectedDataRow["TaxRate"] == "System.DBNull") ? 0 : ((decimal)(l_SelectedDataRow["TaxRate"])) );
//			l_Decimal_PersonalPreTax = ( (l_AvailableBalance["PersonalPreTax"] == "System.DBNull") ? 0 : ((decimal)(l_AvailableBalance["PersonalPreTax"])) );
//			l_Decimal_PersonalPostTax = ( (l_AvailableBalance["PersonalPostTax"] == "System.DBNull") ? 0 : ((decimal)(l_AvailableBalance["PersonalPostTax"])) );
//			l_Decimal_YMCAPreTax = ( (l_AvailableBalance["YmcaPreTax"] == "System.DBNull") ? 0 : ((decimal)(l_AvailableBalance["YmcaPreTax"]))) ;
//			//'***********************************************************************************************
//			//'* Personal Side Interest
//			//'***********************************************************************************************
//
//			if ((l_Decimal_SelectedTaxableAmount > 0.0) && (l_Decimal_PersonalPreTax))
//			{
//				if ((l_AccountType.Trim = "AM" ) &&  ((l_Decimal_PersonalPreTax + l_Decimal_PersonalPostTax) == 0.0))
//					l_AccountBreakDownType = "07" ;
//				else
//				{
//					l_AccountBreakDownType = GetAccountBreakDownType(l_AccountType, true, false, false, true);
//				}
//				
//				l_SortOrder = GetAccountBreakDownSortOrder(l_AccountBreakDownType);
//				l_DisbursementDataRow = SetDisbursementDataRow(parameterIndex, parameterCurrencyCode);
//				if (l_DisbursementDataRow==null)
//				{
//					l_DisbursementDetailsDataRow = SetDisbursementDetailsDataRow(l_AccountType, l_AccountBreakDownType, (string) l_DisbursementDataRow["UniqueID"], l_SortOrder);
//
//					l_TransactionDataRow = SetTransactionDataRow(parameterAvailableBalance, parameterIndex, parameterTransactionType);
//
//					if (l_TransactionDataRow == null)
//					{
//						l_RefundsDataRow = SetRefundsDataRow((string)l_TransactionDataRow["UniqueID"], parameterSelectedDataRow, l_AccountType, (string)l_DisbursementDataRow["UniqueID"]);
//						if (l_RefundsDataRow == null)
//						{
//							if (l_Decimal_SelectedTaxableAmount >= l_Decimal_PersonalPreTax)
//							{
//								l_DisbursementDataRow["TaxableAmount"] =(decimal)l_DisbursementDataRow["TaxableAmount"] + l_Decimal_PersonalPreTax;
//								l_DisbursementDetailsDataRow["TaxableInterest"] = (decimal) l_DisbursementDetailsDataRow["TaxableInterest"] + l_Decimal_PersonalPreTax;
//								l_DisbursementDetailsDataRow["TaxWithheldInterest"] = (decimal) l_DisbursementDetailsDataRow["TaxWithheldInterest"] + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0));
//								l_TransactionDataRow["PersonalPreTax"] = (decimal) l_TransactionDataRow["PersonalPreTax"] + (l_Decimal_PersonalPreTax * (-1.0));
//								l_RefundsDataRow["Taxable"] = (decimal) l_RefundsDataRow["Taxable"] + l_Decimal_PersonalPreTax;
//								l_RefundsDataRow["Tax"] = (decimal) l_RefundsDataRow["Tax"] + (l_Decimal_PersonalPreTax * (l_Decimal_TaxRate / 100.0));
//								l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
//								l_SelectedDataRow["Taxable"] = (decimal)l_SelectedDataRow["Taxable"] - l_Decimal_PersonalPreTax;
//								l_AvailableBalance["PersonalPreTax"] = "0.00";
//							}
//							else
//							{
//								l_DisbursementDataRow["TaxableAmount"] = (decimal) l_DisbursementDataRow["TaxableAmount"] + l_Decimal_SelectedTaxableAmount;
//								l_DisbursementDetailsDataRow["TaxableInterest"] = (decimal)l_DisbursementDetailsDataRow["TaxableInterest"] + l_Decimal_SelectedTaxableAmount;
//								l_DisbursementDetailsDataRow["TaxWithheldInterest"] = (decimal)l_DisbursementDetailsDataRow["TaxWithheldInterest"] + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0));
//								l_TransactionDataRow["PersonalPreTax"] = (decimal)l_TransactionDataRow["PersonalPreTax"] + (l_Decimal_SelectedTaxableAmount * (-1.0));
//								l_RefundsDataRow["Taxable"] = (decimal)l_RefundsDataRow["Taxable"] + l_Decimal_SelectedTaxableAmount;
//								l_RefundsDataRow["Tax"] = (decimal)l_RefundsDataRow["Tax"] + (l_Decimal_SelectedTaxableAmount * (l_Decimal_TaxRate / 100.0));
//								l_RefundsDataRow["TaxRate"] = l_Decimal_TaxRate;
//								l_AvailableBalance["PersonalPreTax"] = (decimal)l_SelectedDataRow["Taxable"] - l_Decimal_SelectedTaxableAmount;
//								l_SelectedDataRow["Taxable"] = "0.00";
//							}
//						}
//					}
//				}
//			}
//		}
//		
// 	
	#endregion
		#region "Set disbusment Datarow"
//		public DataRow SetDisbursementDataRow(int parameterIndex, string parameterCurrencyCode,string l_PersonID ) 
//		{
//			DataSet l_obj_DisbursementDataRow = new DataSet();
//			DataTable l_DataTable;
//			DataRow l_DataRow;
//			DataRow[] l_FoundRows;
//			string l_QueryString;
//			try 
//			{
//				l_DataTable = l_obj_DisbursementDataRow.Tables["R_Disbursements"];
//				if (!(l_DataTable == null)) 
//				{
//					if (((parameterIndex == 1) 
//						|| (parameterIndex == 4))) 
//					{
//						l_QueryString = ("PayeeEntityID = \'" 
//							+ (l_PersonID.ToString().Trim + "\'"));
//					}
//					
//					l_FoundRows = l_DataTable.Select(l_QueryString);
//					l_DataRow = null;
//					if (!(l_FoundRows == null)) 
//					{
//						if ((l_FoundRows.Length > 1)) 
//						{
//							l_DataRow = l_FoundRows[0];
//						}
//					}
//					if ((l_DataRow == null)) 
//					{
//						l_DataRow = l_DataTable.NewRow();
//						// 'l_DataRow("UniqueID") = System.DBNull.Value     '' this Unique iD is used to keep track of other details
//						l_DataRow["UniqueID"] = l_DataTable.Rows.Count;
//						// ' this ID is used in Disbursement Details as Disbusement ID. 
//						switch (parameterIndex) 
//						{
//							case 1:
//								l_DataRow["PayeeEntityID"] = l_PersonID;
//								// && Payee1 Entity ID
//								break;
//							case 2:
//								l_DataRow["PayeeEntityID"] = this.Payee2ID;
//								// && Payee2 Entity ID
//								break;
//							case 3:
//								l_DataRow["PayeeEntityID"] = this.Payee3ID;
//								// && Payee3 Entity ID
//								break;
//							case 4:
//								l_DataRow["PayeeEntityID"] = this.PersonID;
//								// && Payee1 Entity ID
//								break;
//						}
//						l_DataRow["PayeeAddrID"] = this.PayeeAddressID;
//						// && Payee Address ID
//						l_DataRow["PayeeEntityTypeCode"] = ( ((parameterIndex == 1) 
//							|| (parameterIndex == 4)) ? "PERSON" : "ROLINS" );
//						// && Payee Entity Type
//						l_DataRow["DisbursementType"] = "REF";
//						l_DataRow["IrsTaxTypeCode"] = System.DBNull.Value;
//						// && Irs Tax Type Code
//						l_DataRow["TaxableAmount"] = "0.00";
//						l_DataRow["NonTaxableAmount"] = "0.00";
//						l_DataRow["PaymentMethodCode"] = "CHECK";
//						l_DataRow["CurrencyCode"] = parameterCurrencyCode;
//						// && Currency Code
//						l_DataRow["BankID"] = System.DBNull.Value;
//						// && Bank ID
//						l_DataRow["DisbursementNumber"] = System.DBNull.Value;
//						// && Disbursement Number (Check No)
//						l_DataRow["PersID"] = l_PersonID;
//						// && Person ID
//						l_DataRow["DisbursementRefID"] = System.DBNull.Value;
//						// && Disburs Ref ID
//						l_DataRow["Rollover"] = System.DBNull.Value;
//						// && Rollover Institution Type
//						l_DataTable.Rows.Add(l_DataRow);
//						return l_DataRow;
//					}
//					else 
//					{
//						return l_DataRow;
//					}
//				}
//				return l_DataRow;
//			}
//			catch (Exception ex) 
//			{
//				throw;
//			}
//		}
//
//
		#endregion
		#region "Set disbusment Details Datarow"
		public DataRow  SetDisbursementDetailsDataRow(string parameterAccountType, string parameterAcctBreakdownType, string parameterDisbursementID, string parameterSortOrder) 
		{
			
			DataTable l_DataTable;
			DataSet l_obj_DataSet = new DataSet();
			DataRow l_DataRow;
			DataRow[] l_FoundRows;
			string l_QueryString;
		
				
				l_DataTable = l_obj_DataSet.Tables["R_DisbursementDetails"];
				if (!(l_DataTable == null)) 
				{
					l_QueryString = ("AcctType = \'" 
						+ (parameterAccountType.ToString() + ("\' AND AcctBreakdownType = \'" 
						+ (parameterAcctBreakdownType + ("\' AND DisbursementID = \'" 
						+ (parameterDisbursementID + "\'"))))));
					l_FoundRows = l_DataTable.Select(l_QueryString);
					l_DataRow = null;
					if (!(l_FoundRows == null)) 
					{
						if ((l_FoundRows.Length > 1)) 
						{
							l_DataRow = l_FoundRows[0];
						}
					}
					if ((l_DataRow == null)) 
					{
						l_DataRow = l_DataTable.NewRow();
						l_DataRow["UniqueID"] = System.DBNull.Value;
						// && UniqueID
						l_DataRow["DisbursementID"] = parameterDisbursementID;
						// && Disbursment ID, I am using for Temp.
						l_DataRow["AcctType"] = parameterAccountType;
						// && Account Type
						l_DataRow["AcctBreakdownType"] = parameterAcctBreakdownType;
						// && Account Break Down
						l_DataRow["SortOrder"] = parameterSortOrder;
						// && Check Sort order
						l_DataRow["TaxablePrincipal"] = "0.00";
						l_DataRow["TaxableInterest"] = "0.00";
						l_DataRow["NonTaxablePrincipal"] = "0.00";
						l_DataRow["TaxWithheldPrincipal"] = "0.00";
						l_DataRow["TaxWithheldInterest"] = "0.00";
						l_DataTable.Rows.Add(l_DataRow);
						return l_DataRow;
					}
						
				}
			return null;
			
		
		}

		#endregion
		#region "Set Refund datarow"
		public void SetRefundsDataRow(string parameterTransactionID, DataRow paramterSelectedDataRow, string parameterAccountType, string parameterDisbursementID) 
		{
			
			DataTable l_DataTable;
			DataSet l_obj_SetRefundsDataRow = new DataSet();
			DataRow l_DataRow;
			DataRow[] l_FoundRows;
			string l_QueryString;
		
				l_DataTable = l_obj_SetRefundsDataRow.Tables["R_Refunds"];
				if (!(l_DataTable == null)) 
				{
					l_QueryString = ("TransactID = \'" 
						+ (parameterTransactionID + "\'"));
					l_FoundRows = l_DataTable.Select(l_QueryString);
					l_DataRow = null;
					if (!(l_FoundRows == null)) 
					{
						if ((l_FoundRows.Length > 1)) 
						{
							l_DataRow = l_FoundRows[0];
						}
					}
					if ((l_DataRow == null)) 
					{
						l_DataRow = l_DataTable.NewRow();
						l_DataRow["Uniqueid"] = System.DBNull.Value;
						// && UniqueID
						l_DataRow["RefRequestsID"] = l_string_fundrequestID;
						l_DataRow["AcctType"] = parameterAccountType;
						// && Account Type
						l_DataRow["Taxable"] = "0.00";
						l_DataRow["NonTaxable"] = "0.00";
						l_DataRow["Tax"] = "0.00";
						l_DataRow["TaxRate"] = "0.00";
						l_DataRow["Payee"] = "";
						l_DataRow["FundedDate"] = System.DBNull.Value;
						// ' This chek for Check Whether given refund is VOL Or Realy HardShip
//						if ((this.ChangedRefundType == String.Empty)) 
//						{
//							l_DataRow["RequestType"] = this.RefundType;
//						}
//						else 
//						{
//							l_DataRow["RequestType"] = this.ChangedRefundType;
//						}
						l_DataRow["TransactID"] = parameterTransactionID;
						l_DataRow["AnnuityBasisType"] = paramterSelectedDataRow["AnnuityBasisType"];
						l_DataRow["DisbursementID"] = parameterDisbursementID;
						l_DataTable.Rows.Add(l_DataRow);
						
					}
					
				
	
			}
		
		}
		#endregion
//		#region "Set Transaction Datarow"
//		public DataRow SetTransactionDataRow(DataRow  parameterAvailableBalance, int parameterIndex, string parameterTransactionType) 
//		{
//			
//
//			DataTable l_DataTable;
//			DataRow l_DataRow;
//			DataRow[] l_FoundRows;
//			string l_QueryString;
//			string l_PayeeID;
//			string l_AccountType;
//			string l_TransactType;
//			string l_AnnuityBasisType;
//			string l_TransactionRefID;
//			try 
//			{
//				
//
//				l_DataTable = R_Transactions.Clone();
//				if (((parameterIndex == 1) || (parameterIndex == 4))) 
//				{
//					l_PayeeID = PersonID;
//				}
//				else if ((parameterIndex == 2)) 
//				{
//					l_PayeeID = Payee2ID;
//				}
//				else if ((parameterIndex == 3)) 
//				{
//					l_PayeeID = Payee3ID;
//				}
//				if ((!(l_DataTable == null) 
//					&& !(parameterAvailableBalance == null))) 
//				{
//					l_AccountType = ((string)parameterAvailableBalance["AcctType"];
//						l_TransactType = parameterTransactionType;
//					l_AnnuityBasisType = ((string)parameterAvailableBalance["AnnuityBasisType"];
//						l_QueryString = ("AcctType = \'" 
//							+ (l_AccountType + ("\' AND TransactType = \'" 
//							+ (l_TransactType + ("\' AND AnnuityBasisType = \'" 
//							+ (l_AnnuityBasisType + ("\' AND Creator = \'" 
//							+ (l_PayeeID + "\'"))))))));
//					l_FoundRows = l_DataTable.Select(l_QueryString);
//					l_DataRow = null;
//					string l_AvailableBalanceRefID;
//					string l_TempDataRowRefID;
//					if (!(l_FoundRows == null)) 
//					{
//						if ((l_FoundRows.Length > 1)) 
//						{
//							l_DataRow = l_FoundRows[0];
//							l_AvailableBalanceRefID = ( (parameterAvailableBalance["TransactionRefID"].GetType.ToString == "System.DBNull") ? String.Empty : ((string)(parameterAvailableBalance("TransactionRefID"))) );
//							l_TempDataRowRefID = ( (l_DataRow["TransactionRefID"].GetType.ToString == "System.DBNull") ? String.Empty : ((string)(l_DataRow["TransactionRefID"])) );
//							if (!(l_AvailableBalanceRefID == l_TempDataRowRefID)) 
//							{
//								l_DataRow = null;
//							}
//						}
//					}
//					if ((l_DataRow == null)) 
//					{
//						l_DataRow = l_DataTable.NewRow();
//						// 'l_DataRow("UniqueID") = System.DBNull.Value   '&& UniqueID '' this unique id is needed to filter in the RefundsArray. So i am just assigning the unique value.
//						l_DataRow["UniqueID"] = l_DataTable.Rows.Count;
//						l_DataRow["PersID"] = PersonID;
//						l_DataRow["FundEventID"] = FundID;
//						l_DataRow["YmcaID"] = System.DBNull.Value;
//						// ' We can add in store Proc Itself
//						l_DataRow["AcctType"] = l_AccountType;
//						l_DataRow["TransactType"] = l_TransactType;
//						l_DataRow["AnnuityBasisType"] = l_AnnuityBasisType;
//						l_DataRow["MonthlyComp"] = "0.00";
//						l_DataRow["PersonalPreTax"] = "0.00";
//						l_DataRow["PersonalPostTax"] = "0.00";
//						l_DataRow["YmcaPreTax"] = "0.00";
//						l_DataRow["ReceivedDate"] = DateTime.Now;
//						l_DataRow["AccountingDate"] = DateTime.Now ;         //'' Needs to Get Account date. in SP we can do.
//						l_DataRow["TransactDate"] = DateTime.Now ;
//						l_DataRow["FundedDate"] = DateTime.Now;             ///'' Needs to Get Account date. in SP we can d
//				
//						// ' Needs to Get Account date. in SP we can do.
//						l_DataRow["TransmittalID"] = System.DBNull.Value;
//						l_DataRow["TransactionRefID"] = parameterAvailableBalance["TransactionRefID"];
//						l_DataRow["Creator"] = l_PayeeID;
//						// ' temperarily i am stroing the Payee ID to filter the Rows, in the above filter.
//						l_DataTable.Rows.Add(l_DataRow);
//						return l_DataRow;
//					}
//					else 
//					{
//						return l_DataRow;
//					}
//				}
//				return l_DataRow;
//			}
//			catch (Exception ex) 
//			{
//				throw;
//			}
//		}
//
//		#endregion
//		#region "Sortorder"
//		public int GetAccountBreakDownSortOrder(string paramterAccountType) 
//		{
//			
//			DataTable l_DataTable;
//			
//			string l_AccountType;
//			try 
//			{
//			
//				l_DataTable = ((DataTable)"AccountBreakDown")));
//				if (!(l_DataTable == null)) 
//				{
//					foreach (DataRow l_DataRow in l_DataTable.Rows) 
//					{
//						l_AccountType = ((string)(l_DataRow["chrAcctBreakDownType"]));
//						if ((l_AccountType != "")) 
//						{
//							if ((l_AccountType.ToString() == paramterAccountType.ToString())) 
//							{
//								return ((int)(l_DataRow["intSortOrder"]));
//							}
//						}
//					}
//				}
//				else 
//				{
//					return 0;
//				}
//			}
//			catch (Exception ex) 
//			{
//				throw;
//			}
//		}
//		#endregion
		#region "Get Account Date"
		public string GetAccountDateBL()
		{
			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				return l_obj_SafeHarborDisbursementDAClass.GetAccountDate();
			}
			catch (Exception ex)
			{
				throw (ex);
			}
		    
            
		}
		#endregion
		#region "Look up for Ref Request"
		public DataTable LookupRefRequestsBL(string l_string_RequestDate,string l_string_disbursements_type)
		{
			DataSet l_obj_DataSet;
			DataTable l_obj_RefRequests = new DataTable();
			


			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_DataSet = l_obj_SafeHarborDisbursementDAClass.LookupRefRequests(l_string_RequestDate,l_string_disbursements_type);
				l_obj_RefRequests = l_obj_DataSet.Tables["RefRequests"];
				return l_obj_RefRequests;
			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}
		#endregion
		#region "Look up for Ref Request"
		public string OpenFileBL(string l_FundEventID)
		{
			string l_return_flgVal = ".T.";
			if (LookupViewRefRequestsBL(l_FundEventID) == ".F.")
				l_return_flgVal = ".F.";
			if (LookupViewRefRequestsDetailsBL(l_FundEventID)  == ".F.")
				l_return_flgVal =  ".F.";	
			if (LookupRTransactionsBL(l_FundEventID) == ".F.")
				l_return_flgVal =  ".F.";
			if (LookupDisbursementsBL() == ".F.")
				l_return_flgVal =  ".F.";
			if (LookupDisbursementsDetailsBL(l_FundEventID) == ".F.")
				l_return_flgVal =  ".F.";
			if (LookupDisbursementsDetailsWithHoldingBL(l_FundEventID) == ".F.")
				l_return_flgVal =  ".F.";
			if (LookupDisbursementsRefundsBL() == ".F.")
				l_return_flgVal =  ".F.";
			
			if (LookupDisbursementsFundingBL() == ".F.")
				l_return_flgVal =  ".F.";		
	
			return l_return_flgVal;
					
		}
		#endregion
		#region "Look up for LookupAvailableTransaction"
		public string LookupAvailableTransaction(string l_string_FundEventID)
		{
			DataSet l_obj_DataSet;
			DataTable l_obj_AvailableTransaction= new DataTable();
			


			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_DataSet = l_obj_SafeHarborDisbursementDAClass.LookupAvailableTransaction(l_string_FundEventID);
				l_obj_AvailableTransaction = l_obj_DataSet.Tables["AvailableTransaction"];
				if (l_obj_AvailableTransaction.Rows.Count < 0)
				{
					return ".F."; //No Records Found 
				}
				else
				{
					foreach(DataRow l_obj_DataRowobj in l_obj_RefundDataTable.Rows)
					{
						string[,] arRefundable = {{""},{""},{""},{""}};
		

						lnx=0;
						for(int inBasisCtr = 0; inBasisCtr <= iaBasis.Length;inBasisCtr ++)
						{
							if (l_obj_DataRowobj["AnnuityBasisType"].ToString() == iaBasis[1,inBasisCtr])
							{
								lnx=0;
								if ((double)l_obj_DataRowobj["PersonalInterest"] + (double)l_obj_DataRowobj["YmcaInterest"] > 0.00)
								{
									lcTemp  = l_obj_DataRowobj["AnnuityBasisType"].ToString() + l_obj_DataRowobj["accttype"].ToString() + "IN" ;
									foreach(string str_refund in arRefundable)
									{
										if(lcTemp == str_refund)
										{
											lnx = lnz;
											
										}
									}
									

								}

							}
							//code here
							if (lnx == 0)
							{
							

							}
						}
						

					}
					return ".T.";
				}
			}
			catch (Exception ex)
			{
				throw (ex);
			}
		
		}
		#endregion
		#region "Look up for LookupMetaAnnuityBasisTypes"
		public string LookupMetaAnnuityBasisTypes()
		{
			DataSet l_obj_DataSet;
			DataTable l_obj_MetaAnnuityBasisTypes= new DataTable();
			int iVal=0;		


			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_DataSet = l_obj_SafeHarborDisbursementDAClass.LookupMetaAnnuityBasisTypes();
				l_obj_MetaAnnuityBasisTypes = l_obj_DataSet.Tables["MetaAnnuityBasisTypes"];
				if (l_obj_MetaAnnuityBasisTypes.Rows.Count < 0)
				{
					return ".F."; //No Records Found 
				}
				else
				{
					foreach(DataRow l_obj_MetaAnnu in l_obj_MetaAnnuityBasisTypes.Rows)
					{
						string l_str_val = iVal.ToString();
						iaBasis = new string[,]{{l_str_val,l_obj_MetaAnnu["AnnuityBasisType"].ToString()}};
						iVal = iVal + 1;
				
					}
					return ".T.";
				}
			}
			catch (Exception ex)
			{
				throw (ex);
			}
		}
		#endregion
		#region "Look up for LookupAddressHistory"
		public string LookupAddressHistory(string l_string_AddressHistoryId)
		{
			DataSet l_obj_DataSet;
			DataTable l_obj_AddressHistory= new DataTable();
			


			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_DataSet = l_obj_SafeHarborDisbursementDAClass.LookupAddressHistory(l_string_AddressHistoryId);
				l_obj_AddressHistory = l_obj_DataSet.Tables["AddressHistory"];
				if (l_obj_AddressHistory.Rows.Count < 0)
				{
					return ".F."; //No Records Found 
				}
				else
				{
					foreach(DataRow l_Datarow_History in l_obj_AddressHistory.Rows)
					{
						icStreet1 = l_Datarow_History["chvAddr1"].ToString();
						icStreet2 = l_Datarow_History["chvAddr2"].ToString();
						icCity    = l_Datarow_History["chvCity"].ToString();
						icState   = l_Datarow_History["chrStateType"].ToString();
						icZip     = "";
						for(int i=0;i<=l_Datarow_History["chrZip"].ToString().Length - 1 ;i++)
						{
							if (l_Datarow_History["chrZip"].ToString().Substring(i,1) != "-")
							{
								icZip = icZip  + l_Datarow_History["chrZip"].ToString().Substring(i,1);
							}
						}
						icPayeeAddrID = l_Datarow_History["guiUniqueID"].ToString();
	
					}
					return ".T.";
				}
	
			}
			catch (Exception ex)
			{
				throw (ex);
			}
		}
		#endregion
		#region "Look up for LookupPersBankingBeforeEffDate"
		public string LookupPersBankingBeforeEffDate(DateTime l_datetime_Date,string l_string_PersID)
		{
			DataSet l_obj_DataSet;
			DataTable l_obj_BankingBeforeEffDate= new DataTable();
			


			try
			{
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_DataSet = l_obj_SafeHarborDisbursementDAClass.LookupPersBankingBeforeEffDate(l_datetime_Date);
				l_obj_BankingBeforeEffDate = l_obj_DataSet.Tables["BankingBeforeEffDate"];
				if (l_obj_BankingBeforeEffDate.Rows.Count < 0)
				{
					return ".F."; //No Records Found 
				}
				else
				{
					foreach(DataRow l_DataRow_BankingBeforeEffDate in l_obj_BankingBeforeEffDate.Rows)
					{
						if (l_string_PersID != "")
							icCurrency =  l_DataRow_BankingBeforeEffDate["CurrencyCode"].ToString();
						else
							icCurrency = "U";

					}
					return ".T.";
				}
			}
			catch (Exception ex)
			{
				throw (ex);
			}

		}
		#endregion
		#region "Update shira Details"
		public string  UpdateShiraDetails(double l_double_amount)
		{
			string l_return_flg =".F.";
			//Vipul - Not as per foxpro method UpdateShiraDetails, hence modified.
			
			DataRow l_obj_ShiraDatarow = c_shiraDetails.NewRow(); 
//			foreach(DataRow l_obj_ShiraDataow in c_shiraDetails.Rows)
//			{	
				l_obj_ShiraDatarow["Amount"] = l_double_amount   ;
				l_obj_ShiraDatarow["SSN"] = icSsno;									
				l_obj_ShiraDatarow["Birth_DT"]=idBirthDate;
				l_obj_ShiraDatarow["First_NM"]	=icFirstName;
				l_obj_ShiraDatarow["Middle_NM"]	=icMiddleName;
				l_obj_ShiraDatarow["Last_NM"] =	icLastName;	
				l_obj_ShiraDatarow["Addr_Line1"]=icStreet1;
				l_obj_ShiraDatarow["Addr_Line2"] =icStreet2;
				l_obj_ShiraDatarow["City"]=icCity; 			
				l_obj_ShiraDatarow["State"] =icState; 	
				l_obj_ShiraDatarow["ZipCode"] =icZip; 		
			    c_shiraDetails.Rows.Add(l_obj_ShiraDatarow);
				c_shiraDetails.AcceptChanges();
				l_return_flg = ".T.";
//			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for ViewRefRequests"
		public string LookupViewRefRequestsBL(string l_string_RefRequest_FundEventID)
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_RefRequests = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupViewRefRequests(l_string_RefRequest_FundEventID);
				l_obj_r_RefRequests = l_obj_viewDataSet.Tables["ViewRefRequests"];
				if (l_obj_r_RefRequests.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
				foreach(DataRow l_obj_RefRequests in l_obj_r_RefRequests.Rows)
				{
					ldTotalAmount = Convert.ToDouble (l_obj_RefRequests["Amount"]);
							
				}
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for LookupViewRefRequestsDetails"
		public string LookupViewRefRequestsDetailsBL(string l_string_RefRequest_DetailsPK)
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_ViewRefRequestsDetails = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupViewRefRequestsDetails(l_string_RefRequest_DetailsPK);
				l_obj_r_ViewRefRequestsDetails = l_obj_viewDataSet.Tables["ViewRefRequestsDetails"];
				if (l_obj_r_ViewRefRequestsDetails.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for LookupRTransactions"
		public string LookupRTransactionsBL(string l_string_RefRequest_FundEventPK)
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_ViewTransactions = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupRTransactions(l_string_RefRequest_FundEventPK);
				l_obj_r_ViewTransactions = l_obj_viewDataSet.Tables["ViewTransactions"];
				if (l_obj_r_ViewTransactions.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;

		}
		#endregion
		#region "Look up for Disbursements"
		public string LookupDisbursementsBL()
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_Disbursements = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupDisbursements();
				l_obj_r_Disbursements = l_obj_viewDataSet.Tables["r_Disbursements"];
				if (l_obj_r_Disbursements.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for DisbursementsDetails"
		public string LookupDisbursementsDetailsBL(string l_string_RefRequest_DisbursementIDFK)
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_DisbursementsDetails = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupDisbursementsDetails(l_string_RefRequest_DisbursementIDFK);
				l_obj_r_DisbursementsDetails = l_obj_viewDataSet.Tables["DisbursementsDetails"];
				if (l_obj_r_DisbursementsDetails.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for DisbursementsDetailsWithHolding"
		public string LookupDisbursementsDetailsWithHoldingBL(string l_string_RefRequest_DistPK)
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_DisbursementsDetailsWithHolding = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupDisbursementsDetailsWithHolding(l_string_RefRequest_DistPK);
				l_obj_r_DisbursementsDetailsWithHolding = l_obj_viewDataSet.Tables["DisbursementsDetailsWithHolding"];
				if (l_obj_r_DisbursementsDetailsWithHolding.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for DisbursementsRefunds"
		public string LookupDisbursementsRefundsBL()
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_DisbursementsRefunds = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupDisbursementsRefunds();
				l_obj_r_DisbursementsRefunds = l_obj_viewDataSet.Tables["DisbursementsRefunds"];
				if (l_obj_r_DisbursementsRefunds.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;
		}
		#endregion
		#region "Look up for DisbursementsFunding"
		public string LookupDisbursementsFundingBL()
		{
			string l_return_flg= "";
			DataSet l_obj_viewDataSet = new DataSet();
			l_obj_r_DisbursementsFunding = new DataTable();
			try
			{
				
				SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
				l_obj_viewDataSet = l_obj_SafeHarborDisbursementDAClass.LookupDisbursementsFunding();
				l_obj_r_DisbursementsFunding = l_obj_viewDataSet.Tables["DisbursementsFunding"];
				if (l_obj_r_DisbursementsFunding.Rows.Count < 0 )
					l_return_flg = ".F.";
				else
					l_return_flg = ".T.";
			}
				
			catch(Exception ex)
			{
				throw(ex);
			}
			finally
			{
				l_obj_viewDataSet = null;


			}
			return l_return_flg;

		}
		#endregion
		#region "FunsDisbursement"
		public bool UpdateFundDisbursement()
		{
			
			SafeHarborDisbursementDAClass l_obj_SafeHarborDisbursementDAClass = new SafeHarborDisbursementDAClass();
			return l_obj_SafeHarborDisbursementDAClass.UpdateFundDisbursement();
		}
		#endregion
	}  

}
