//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	ACHDebitImportBOClass.cs
// Author Name		:	Ashish Srivastava
// Employee ID		:	51821
// Email			:	ashish.srivastava@3i-infotech.com
// Contact No		:	8609
// Creation Time	:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by           Date            Description
//*******************************************************************************
//Ashish Srivastava	    12-Jan-2009		Remove comma seperated Ymca parameter from function GetAchDebitMatchedTransmittals
//Ashish Srivastava	    27-Jan-2009		Issue YRS 5.0-651
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*******************************************************************************
#region using Namespace

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject;

#endregion
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for ACHDebitImportBOClass.
	/// </summary>
	public class ACHDebitImportBOClass
	{
		public ACHDebitImportBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region "Public Methods"
		/// <summary>
		/// This Method Validate File Extention and File name
		/// </summary>
		/// <param name="strFileName"></param>
		/// < outparam name="strMessage"></param>
		
		/// <returns>bool</returns>
		
		public bool ValidateFileNameAndExt(string strFileName,out string strMessage)
		{
			bool bolFlag=false;

			string strFullFileName=string.Empty ;
			string strFirstPartFName=string.Empty ;
			string strSecondPartFName=string.Empty ;
			string strThirdPartFName=string.Empty ;
			string strTemp=string.Empty ;
			try
			{
				string []strArrayFilePath =strFileName.Split('\\');
				strFullFileName=strArrayFilePath[strArrayFilePath.Length-1];
				if(strFullFileName.ToUpper().EndsWith(".CSV"))				
				{
					string []strFileNameTemp =strFullFileName.Split('_'); 
					
					if(strFileNameTemp.Length ==3)
					{
						strFirstPartFName=strFileNameTemp.GetValue(0).ToString() ;
						//Commented by Ashish on 27-Jan-2009
						//strSecondPartFName=strFileNameTemp.GetValue(1).ToString().ToUpper().Replace(".CSV","") ;
						strSecondPartFName=strFileNameTemp.GetValue(1).ToString();
						strThirdPartFName=strFileNameTemp.GetValue(2).ToString().ToUpper().Replace(".CSV","");
						if(strFirstPartFName.Equals("ACHDebit") &&  strSecondPartFName.Length==9 && strThirdPartFName=="YRS" )
						{
							bolFlag=true;
							strTemp=strSecondPartFName;
					
						}
						else
						{
							strTemp="This is not a valid File";
							bolFlag=false;
						}
					}
					else
					{
						strTemp="This is not a valid File";
						bolFlag=false;
					}

 

				}
				else
				{
					strTemp="Please Select only the CSV File to Import.";
				}
				
				strMessage=strTemp;
				
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			
			}
			return bolFlag;
			
		}

		/// <summary>
		/// This Methods validate Batch ID
		/// </summary>
		/// <param name="parameterBatchId"></param>
		/// <returns>int</returns>
		public int ValidateBatchID(string parameterBatchId)
		{
			ACHDebitImportDAClass  achDebitImportDAClass=null;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass();
				return achDebitImportDAClass.ValidateBatchID( parameterBatchId);
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				achDebitImportDAClass=null;
			}

		}
		
		/// <summary>
		/// Get Total ACHDebit Amount for Batch ID
		/// </summary>
		/// <param name="parameterBatchId"></param>
		/// <returns>decimal</returns>
		public decimal GetACHAmount(string parameterBatchId)
		{
			ACHDebitImportDAClass  achDebitImportDAClass=null;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass();
				return achDebitImportDAClass.GetACHAmount( parameterBatchId);
			}
			catch(Exception ex)
			{
				throw ex;

			}
			finally
			{
				achDebitImportDAClass=null;
			}
		}
/*
		public DataSet GetYMCANameandNos(string parameterYmcaNos, string parameterBatchId)
		{
			ACHDebitImportDAClass achDebitImportDAClass=null;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass();
				return achDebitImportDAClass.GetYMCANameandNos( parameterYmcaNos,parameterBatchId);

			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				achDebitImportDAClass=null;
			}

		}
*/
		
		/// <summary>
		/// This Method Validate YMCA wise ACDDebit Amount
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="parameterBatchId"></param>
		/// <param name="strmessage"></param>
		/// <param name="dsACHData"></param>
		/// <returns>bool</returns>
		public bool GetACHDataAndValidateYmcaAmt(DataTable dt,string parameterBatchId,out string strmessage,out DataSet dsACHData)
		{
			bool l_bool_flag=false;
			
			DataSet ds=null;
			DataRow []dtFindRow;
			string l_strYmcaNo=string.Empty ;
			string l_strMsg=string.Empty ;
			bool l_bool_InvalidYmcaFound=false;
			try
			{
				if( dt!=null)
				{
					ds=GetACHDebitDetails(parameterBatchId);
					if(ds!=null)
					{
						foreach(DataRow dtRow in  dt.Rows)
						{
							
							dtFindRow=ds.Tables[0].Select("YMCANO = '" + dtRow["YMCANO"] + "'" ); 
							if(dtFindRow.Length >0)
							{
								decimal dbAmount=0;
								decimal fileAmount=0;
								for(int i=0;i < dtFindRow.Length;i++)
								{
									dbAmount+=Convert.ToDecimal( dtFindRow[i]["AMOUNT"]);
								}
								
								fileAmount=Convert.ToDecimal( dtRow["AMOUNT"]);

								if(dbAmount==fileAmount)
								{
									l_bool_flag=true;									 
								}
								else
								{
									l_bool_flag=false;
									l_strYmcaNo+=dtRow["YMCANO"].ToString()+",";
									
									
								}
							}
							else
							{
								l_bool_InvalidYmcaFound=true;
							}
							
						}
//						if(!l_bool_flag )
//						{
//							l_strMsg="Amount mismatch for YMCANo " +l_strYmcaNo.Remove(l_strYmcaNo.LastIndexOf(","),1);  
//						}
						if(l_strYmcaNo.Length >0)
						{
							l_strMsg="Amount mismatch for YMCANo " +l_strYmcaNo.Remove(l_strYmcaNo.LastIndexOf(","),1); 
							l_bool_flag=false;
						}
						if(l_strYmcaNo==string.Empty && l_bool_InvalidYmcaFound)
						{
							l_strMsg="One or more YMCA's specified in the import file were not found in the exported batch of records.";
							l_bool_flag = false;
						}
					
					}
				}
					strmessage=l_strMsg;
				dsACHData=ds;
				return l_bool_flag;
			}
			catch(Exception  ex)
			{
				throw ex;
			}
			finally
			{
			
			}
		}

		/// <summary>
		/// This Methods Save Records
		/// </summary>
		/// <param name="dsACHDebit"></param>
		public  void SaveImportedRecords(DataSet dsACHDebit)
		{
			ACHDebitImportDAClass achDebitImportDAClass=null;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass();
				if(dsACHDebit!=null)
				{
					if(dsACHDebit.Tables[0].Rows.Count >0)
					{
						achDebitImportDAClass.InsertAndUpdateAtsYmcaRcpts(dsACHDebit); 
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				achDebitImportDAClass=null;
			}

		}

		public   DataSet GetAchDebitMatchedTransmittals(string parameterBatchId)
		{
			ACHDebitImportDAClass achDebitImportDAClass=null;
			DataSet ds=null;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass ();
				ds=achDebitImportDAClass.GetAchDebitMatchedTransmittals(parameterBatchId);
				if(ds!=null)
				{
					if(ds.Tables.Count>0)
					{
						if(ds.Tables[0].Rows.Count >0)
						{
							ds.Tables[0].Columns.Add(new DataColumn("ValidRecord",typeof(int)));
							ds.Tables[0].Columns["ValidRecord"].DefaultValue=0; 
						}
					}
				}
				return ds; 
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				achDebitImportDAClass=null;
			}
		}

		public DataSet SavePostAndApplyReceipts(DataSet parameterDataSetACHDebit,DataSet parameterDataSetMatchedTransmittal,DateTime parameterFundedDate,ref bool serviceUpdate_Flag,ref Int64 logged_BatchID )
		{
			ACHDebitImportDAClass achDebitImportDAClass=null;
			DataSet l_dsPersonalDetails=null;			
			//UEINBOClass ueinBOClass=null;
			DataTable l_dtFundedTransmittal=null;
			Int64 l_fundedTransmittalLogID=0;
			DataRow l_dtRow=null;
			string l_BatchID=string.Empty ;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass();
				if(parameterDataSetACHDebit!=null && parameterDataSetMatchedTransmittal!=null )
				{
					//DataTable dtNewTransactRecords=new DataTable();
					//DataTable dtNewTransmittalRecords=new DataTable();
					//bool flag=true;
					l_dtFundedTransmittal=GetFundedTransmittalSchema();
					foreach(DataRow dtMatchTransmittalRow in parameterDataSetMatchedTransmittal.Tables["MatchedTransmittals"].Rows)
					{
						if(dtMatchTransmittalRow["Selected"].ToString()  =="1" && dtMatchTransmittalRow["ValidRecord"].ToString() =="1" )
						{
//							string l_NewUeinTransmittalID=string.Empty; 
//							ueinBOClass=new UEINBOClass(dtMatchTransmittalRow["guiTransmittalId"].ToString() ,parameterFundedDate);
//							ueinBOClass.GenerateUEINRecords(ref dtNewTransactRecords,ref dtNewTransmittalRecords,flag,ref l_NewUeinTransmittalID ) ;
//							if(flag)
//							{
//								flag=false;
//							}
							// Add row for funded transmittal Log
							l_BatchID=dtMatchTransmittalRow["BatchId"].ToString(); 
							l_dtRow=l_dtFundedTransmittal.NewRow();
							l_dtRow["guiTransmittalID"]=dtMatchTransmittalRow["guiTransmittalId"].ToString(); 
							//l_dtRow["guiUeinTransmittalID"]=l_NewUeinTransmittalID; 
							l_dtRow["guiRcptID"]=string.Empty ; 
							l_dtFundedTransmittal.Rows.Add(l_dtRow);  
						}

					
					}
					
					l_dsPersonalDetails=achDebitImportDAClass.SavePostAndApplyReceipts(parameterDataSetACHDebit,parameterDataSetMatchedTransmittal,parameterFundedDate,l_BatchID,l_dtFundedTransmittal,ref l_fundedTransmittalLogID); 
//					// Save funded Transmittal Log
//					if(l_dtFundedTransmittal.Rows.Count>0 && l_BatchID!=string.Empty )
//					{
//						l_fundedTransmittalLogID=SaveTransmittalFundedLog(l_BatchID,"ACH Debit",l_dtFundedTransmittal,"ACHDebit");  
//					}
					if (l_fundedTransmittalLogID >0)
					{
						logged_BatchID=l_fundedTransmittalLogID;
						try
						{
							//System.Diagnostics.Debug.WriteLine("{0}:Service Update Start", DateTime.Now.ToString());
							serviceUpdate_Flag=ServiceTimeAndVestingBOClass.ServiceTimeVestingUpdate( l_fundedTransmittalLogID); 
							//System.Diagnostics.Debug.WriteLine("{0}:Service Update End", DateTime.Now.ToString());
						}
						catch
						{
							serviceUpdate_Flag=false;
						}
					}
					//serviceUpdate_Flag=false;
					
				}
					return l_dsPersonalDetails;
			}
				
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				achDebitImportDAClass=null;
			}

		}
		//Commented by Ashish on 01-Oct-2008 
/*
		public Int64 SaveTransmittalFundedLog(string parameterBatchID,string parameterDescription,DataTable parameterDataTableFundedTransmittal,string parameterModuleType )
		{
			Int64 l_logID=0;
			ACHDebitImportDAClass achImportDAClass=null;
			try
			{
				achImportDAClass=new ACHDebitImportDAClass(); 
				l_logID=achImportDAClass.SaveTransmittalFunding(parameterBatchID,parameterDescription,parameterDataTableFundedTransmittal,parameterModuleType);

				return l_logID;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		*/
		#endregion
		public DataTable GetFundedTransmittalSchema()
		{
			DataTable l_dtFundedTransmittal=null;
			try
			{
				l_dtFundedTransmittal=new DataTable();
				l_dtFundedTransmittal.Columns.Add(new DataColumn("guiTransmittalID",typeof(string))); 
				l_dtFundedTransmittal.Columns.Add(new DataColumn("guiRcptID",typeof(string))); 
				l_dtFundedTransmittal.Columns.Add(new DataColumn("guiUeinTransmittalID",typeof(string)));
				return l_dtFundedTransmittal;
 
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#region "Private Methods"
		/// <summary>
		/// Get ACHDebitDetails Records Form DB
		/// </summary>
		/// <param name="parameterBatchId"></param>
		/// <returns>DataSet</returns>
		private DataSet GetACHDebitDetails(string parameterBatchId)
		{
			ACHDebitImportDAClass achDebitImportDAClass=null;
			DataSet ds=null;
			try
			{
				achDebitImportDAClass=new ACHDebitImportDAClass ();
				ds=achDebitImportDAClass.GetACHDebitDetails( parameterBatchId);
				return ds; 
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				achDebitImportDAClass=null;
			}
		}
		#endregion
	}
	
}
