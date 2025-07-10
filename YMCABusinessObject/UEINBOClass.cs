//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Bala                 2016.01.27    YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//Pramod P. Pokale     2016.01.29    YRS-AT-2594 -  YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//Anudeep A            2016.03.21    YRS-AT-2594 - YRS enh: Utility for Unearned Interest Transmittals (UEIN) emails (review/select//send)
//*****************************************************
#region using Namespace

using System;
using System.Data;
using YMCARET.YmcaDataAccessObject; 
 
#endregion

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for UEINBOClass.
	/// </summary>
	public class UEINBOClass
	{
		#region Constructor
        public UEINBOClass()
        { 
            //Empty constructor is created for Unfunded UEIN screen
        }

		public UEINBOClass(string TransmittalId,DateTime fundedDate)
		{
			//
			// TODO: Add constructor logic here
			//
			_strTransmittalId=TransmittalId;
			_fundedDate=fundedDate;
			_dtOldTransactRecords=null;
			_ymcaTransmittalExist=false;

		}
		#endregion

		#region  Private Member
			private string _strTransmittalId;
			private DateTime _fundedDate;
			private DateTime _transactionDueDate;
			
			private DateTime _currentAccountingDate;			
			private DataTable _dtOldTransactRecords;
			private int _noOfDueDays;
			private string _YmcaNo;
			private string _guiYmcaID;
			private string _newTransmittalID;
			private double _thresholdAmount;
			private bool _ymcaTransmittalExist;
			private DataRow _dtOldTransmittalRow;

		#endregion

		#region Public Methods
		/// <summary>
		/// This Methods accept Transaction  and Transmittal tablle and Create UEIN Records in it.
		/// </summary>
		/// <param name="dtNewTransactions"></param>
		/// <param name="dtNewTransmittals"></param>
		/// <param name="createSchema"></param>
		public void GenerateUEINRecords(ref DataTable dtNewTransactions,ref DataTable dtNewTransmittals,bool createSchema,ref string newUeinTransmittalID)
		{
			try
			{
				//Get Old Transcat records
				_dtOldTransactRecords=GetTransactsRecords(_strTransmittalId);
				if (_dtOldTransactRecords.Rows.Count >0)
				{
					
					//Create schema 
					if(createSchema)
					{
						CreateTransactionSchema(ref dtNewTransactions);
						CreateTransmittalSchema(ref dtNewTransmittals);
					}
					
						
						DataRow[] dtNewTrasmittalRows=dtNewTransmittals.Select("guiYmcaId='"+_guiYmcaID+"'");
						if(dtNewTrasmittalRows.Length >0)
						{
							_newTransmittalID=dtNewTrasmittalRows[0]["guiTransmittalID"].ToString();
							_dtOldTransmittalRow=dtNewTrasmittalRows[0];
							_ymcaTransmittalExist=true;
						}

						CreateTransactionAndTransmittalRecord(ref dtNewTransactions,ref dtNewTransmittals,ref newUeinTransmittalID);
					
				}
	
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
			
			}
		}

        //START: Bala | 2016.01.27 | YRS-AT-2594 | Provides Unfunded UEIN transmittal details
        public DataSet GetUnfundedUEIN()
        {
            return (new UEINDAClass()).GetUnfundedUEIN();
        }
        //END: Bala | 2016.01.27 | YRS-AT-2594 | Provides Unfunded UEIN transmittal details

        //START: PPP | 2016.01.29 | YRS-AT-2594 | Sends email with selected UEIN transmittal to LPA's
        //Start: AA:2016.03.21 :YRS-AT-2594 Commented below code as it will be not used
        //public bool SendEmailOfUnfundedUEINs(string strXMLOutput)
        //{
        //    return (new UEINDAClass()).SendEmailOfUnfundedUEINs(strXMLOutput);
        //}
        //End: AA:2016.03.21 :YRS-AT-2594 Commented below code as it will be not used
        //END: PPP | 2016.01.29 | YRS-AT-2594 | Sends email with selected UEIN transmittal to LPA's

		#endregion

		#region Private Methods
		/// <summary>
		/// This Method Get Transaction Records for perticular transmittal ID.
		/// </summary>
		/// <param name="parameterTransmittalId"></param>
		/// <returns> DataTable</returns>
		private DataTable GetTransactsRecords(string parameterTransmittalId)
		{
			UEINDAClass ueinDAClass=null;
			DataSet dsTransacts=null;
			DataTable dtTransacts=null;
			try
			{
				ueinDAClass=new UEINDAClass();
				dsTransacts=ueinDAClass.GetTransactsRecords(parameterTransmittalId);
				if( dsTransacts != null)
				{
					if(dsTransacts.Tables.Count >0)
					{
						dtTransacts=dsTransacts.Tables["Transacts"];
						if(dtTransacts.Rows.Count>0)
						{
							_currentAccountingDate=((DateTime)dtTransacts.Rows[0]["CurrentAcctDate"]).Date ;
							_YmcaNo=dtTransacts.Rows[0]["YmcaNo"].ToString(); 
							_guiYmcaID=dtTransacts.Rows[0]["guiYmcaId"].ToString(); 
							_newTransmittalID=dtTransacts.Rows[0]["NewTransmittalID"].ToString();
							if(dtTransacts.Rows[0]["ThresholdAmount"].ToString()!="")
							{
								_thresholdAmount=Convert.ToDouble(dtTransacts.Rows[0]["ThresholdAmount"]); 
							}

						}


					}
				}
				return dtTransacts;
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				ueinDAClass=null;
			}
		}
		#region commented
		/*
		/// <summary>
		/// This Method Set the Due Date for Transaction According to Transact Type.
		/// </summary>
		/// <param name="parameterTransactDataRow"></param>
		
		private void GetTransactionDueDate(DataRow parameterTransactDataRow)
		{
			try
			{
						

				string transactType=string.Empty ;
				transactType=parameterTransactDataRow["TransactType"].ToString() ;
				switch(transactType)
				{
					case "MCPR":
						
						_transactionDueDate=((DateTime)parameterTransactDataRow["BusinessDate"]).Date; 
						break;
					case "RIIN":
						
						_transactionDueDate=((DateTime)parameterTransactDataRow["dtsReceivedDate"]).Date;
						break;
					default:
						
						_transactionDueDate=((DateTime)parameterTransactDataRow["dtsReceivedDate"]).Date; 
						break;
				


				}
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				
			}
		}
		*/
		#endregion	
		
		/// <summary>
		/// This Method Calculate Number of Days Delay.
		/// </summary>
		/// <param name="parameterDueDate"></param>
		/// <returns> number of days dealy</returns>
		private int NumberOfDueDays(DateTime parameterDueDate)
		{
			int numberOFDueDays=0;
			DateTime tempDueDate;

			try
			{
				tempDueDate=_fundedDate;
				if(_fundedDate >  parameterDueDate)
				{
					if(_fundedDate.Year ==  parameterDueDate.Year  && _fundedDate.Month ==  parameterDueDate.Month  )
					{
						numberOFDueDays=_fundedDate.Day - parameterDueDate.Day ;
						
					}
					else 
					{
						tempDueDate=tempDueDate.AddDays(-(_fundedDate.Day-1)); 
						numberOFDueDays=(_fundedDate.Day-tempDueDate.Day)+1 ;
					}

				}
				return numberOFDueDays;
			}
			catch(Exception ex)
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
		private double CalculateCompoundInterest(double parameterPrincipalAmount,double parameterAnualInterestRate,int parameterNumOfPeriodsComputed)
		{
			//This method accept parameterAnualInterestRate in decimal value e.g 10%= 10/100->.1
			double interestAmount=0;
			int numberOfCompoundingPeriods; 
			try
			{
				
				numberOfCompoundingPeriods=GetNumberOfDaysInYear(_fundedDate.Year); 
				interestAmount=(parameterPrincipalAmount*(Math.Pow (1 + parameterAnualInterestRate/(double)(numberOfCompoundingPeriods),(double)parameterNumOfPeriodsComputed)))-parameterPrincipalAmount;

				return interestAmount;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// This method find how many days in supplied year.  
		/// </summary>
		/// <param name="parameterYear"></param>
		/// <returns>Number of days in a Year</returns>
		private int GetNumberOfDaysInYear(int parameterYear)
		{
			int days=0;
			try
			{
				if(parameterYear%4==0 )
				{
					if(parameterYear%100==0 )
					{
						if( parameterYear%400==0)
						{
							days=366;
						}
						else
						{
							days=365;
						}
					}
					else
					{
						days=366;
					}
					
				}
				else
				{
					days=365;
				}
				return days;
			}
			catch(Exception ex)
			{
				throw ex;

			}
		}
		/// <summary>
		/// This Method Create Schema in New Transaction Table.
		/// </summary>
		/// <param name="parameterdtTransaction"></param>
		private void CreateTransactionSchema(ref DataTable parameterdtTransaction)
		{
			try
			{
				parameterdtTransaction=_dtOldTransactRecords.Clone();
				
 
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// This Method Create Schema in New Transmittal Table.
		/// </summary>
		/// <param name="parameterdtTransmittal"></param>
		private void CreateTransmittalSchema(ref DataTable  parameterdtTransmittal)
		{
			try
			{
				
				parameterdtTransmittal.TableName="NewTransmittals"; 
				parameterdtTransmittal.Columns.Add(new DataColumn("guiTransmittalID",typeof(string))) ;
				parameterdtTransmittal.Columns.Add(new DataColumn("guiYmcaID",typeof(string))) ;
				parameterdtTransmittal.Columns.Add(new DataColumn("chvTransmittalSourceCode",typeof(string))) ;
				parameterdtTransmittal.Columns.Add(new DataColumn("chvTransmittalNo",typeof(string))) ;
				parameterdtTransmittal.Columns.Add(new DataColumn("dtsTransmittalDate",typeof(string))) ;
				parameterdtTransmittal.Columns.Add(new DataColumn("dtsAccountingDate",typeof(string)) );
				parameterdtTransmittal.Columns.Add(new DataColumn("mnyAmtDue",typeof(double))) ;
				parameterdtTransmittal.Columns.Add(new DataColumn("mnyAmtPaid",typeof(double))) ;
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// This Method Generate Transmittal Number.
		/// </summary>
		/// <param name="parameterTransactType"></param>
		/// <returns></returns>
		private string GenerateTransmittalNo()
		{
			string transmittalNo=string.Empty ;
			string transmittalYear=string.Empty;
			string transmittalMonth=string.Empty;
			string transmittalDay=string.Empty;
			try
			{
				
				DateTime currentDate=_fundedDate ; 
				transmittalYear=currentDate.Year.ToString();
				transmittalMonth=currentDate.Month <= 9 ? "0"+currentDate.Month.ToString(): currentDate.Month.ToString();
				transmittalDay=currentDate.Day <= 9 ? "0"+currentDate.Day.ToString(): currentDate.Day.ToString();

				transmittalNo=_YmcaNo+transmittalYear+transmittalMonth+transmittalDay+"01"+"UEIN";
  
				return transmittalNo;	
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// This Method Create New Transaction and Transmittal Records.
		/// </summary>
		/// <param name="parameterdtNewTransactions"></param>
		/// <param name="parameterdtNewTransmittals"></param>
		/// <returns>Transmittal Number</returns>
		private void CreateTransactionAndTransmittalRecord(ref DataTable parameterdtNewTransactions,ref DataTable parameterdtNewTransmittals,ref string newUeinTransmittalID)
		{
			double totalTransmittalAmtDue=0.0;
			
			try
			{
				int recordCounter=0;
				//bool flag=true;
				string ymcaGuiID=string.Empty ;
				DataTable dtTempNewTransactRecord=null;
				dtTempNewTransactRecord=new DataTable();
				dtTempNewTransactRecord= parameterdtNewTransactions.Clone(); 

				foreach(DataRow dtTransactRow in _dtOldTransactRecords.Rows )
				{
					DataRow dtNewTransactionRow;
					double personalInterestAmt=0.0;
					double ymcaInterestAmt=0.0;
					double totalPersonalPerPostTaxAmt=0.0;
					double personalPreTaxAmt=0.0;
					double personalPostTaxAmt=0.0;
					double ymcaPreAmount=0.0;
					double personalInterestRate=0.0;
					double ymcaInterestRate=0.0;
					// Get transaction Due Date
					_transactionDueDate=((DateTime)dtTransactRow["DueDate"]).Date; 
					//GetTransactionDueDate(dtTransactRow);
					//Calculate no of days delay 
					_noOfDueDays=NumberOfDueDays(_transactionDueDate);
					if(_noOfDueDays>0)
					{
						personalPreTaxAmt= Convert.ToDouble(dtTransactRow["PersonalPreTax"]);
						personalPostTaxAmt= Convert.ToDouble(dtTransactRow["PersonalPostTax"]);
						ymcaPreAmount=Convert.ToDouble(dtTransactRow["YmcaPreTax"]);
						personalInterestRate=Convert.ToDouble(dtTransactRow["PersonInterestRate"]);
						ymcaInterestRate=Convert.ToDouble(dtTransactRow["YmcaInterestRate"]);
						totalPersonalPerPostTaxAmt=personalPreTaxAmt+personalPostTaxAmt;
						//calculate Interest Amount
						personalInterestAmt=CalculateCompoundInterest(totalPersonalPerPostTaxAmt,personalInterestRate,_noOfDueDays);
						ymcaInterestAmt=CalculateCompoundInterest(ymcaPreAmount,ymcaInterestRate,_noOfDueDays);
						
						dtNewTransactionRow=dtTempNewTransactRecord.NewRow();

						dtNewTransactionRow["guiPersID"]=dtTransactRow["guiPersID"].ToString();
						dtNewTransactionRow["guiFundEventID"]=dtTransactRow["guiFundEventID"].ToString();
						dtNewTransactionRow["guiYmcaID"]=dtTransactRow["guiYmcaID"].ToString();
						dtNewTransactionRow["AcctType"]=dtTransactRow["AcctType"].ToString();

						dtNewTransactionRow["TransactType"]="UEIN" ;
						dtNewTransactionRow["AnnuityBasisType"]=dtTransactRow["AnnuityBasisType"].ToString();
						dtNewTransactionRow["MonthlyComp"]=0.0;
						dtNewTransactionRow["PersonalPreTax"]=Math.Round(personalInterestAmt,2);
						dtNewTransactionRow["PersonalPostTax"]=0.0;
						dtNewTransactionRow["YmcaPreTax"]=Math.Round(ymcaInterestAmt,2);
						dtNewTransactionRow["dtsReceivedDate"]=_fundedDate  ;
						dtNewTransactionRow["dtsAccountingDate"]=_currentAccountingDate;
						dtNewTransactionRow["dtsTransactDate"]=_fundedDate;
						dtNewTransactionRow["guiTransmittalID"]=_newTransmittalID ;
						//totalTransmittalAmtDue+=(personalInterestAmt+ymcaInterestAmt);
						recordCounter+=1;
						dtTempNewTransactRecord.Rows.Add(dtNewTransactionRow);
					}
					

				}
				// Get clubbed amoumt according PersId
				string []targetColumns={"guiPersID","PersonalPreTax","YmcaPreTax"};
				string []compareColumns={"guiPersID"};
				string []clubbedColumns={"PersonalPreTax","YmcaPreTax"};
				string []verifyThresholdColumns={"PersonalPreTax","YmcaPreTax"};
				DataTable dtDistinctPersID=null;
				if(dtTempNewTransactRecord.Rows.Count>0)  
				{ //start If
					// Get distinct PersID
					dtDistinctPersID=SelectDistinct(dtTempNewTransactRecord,targetColumns,compareColumns,clubbedColumns);

					// check for threshold Amount
					ApplyThreshAmountCheck(dtTempNewTransactRecord,dtDistinctPersID,verifyThresholdColumns);
					if(dtTempNewTransactRecord.Rows.Count  >0)
					{
						foreach(DataRow dtRow in dtTempNewTransactRecord.Rows)
							parameterdtNewTransactions.ImportRow(dtRow);  
						parameterdtNewTransactions.AcceptChanges(); 
					}

					//calculate total transmittal amount due
			
					foreach(DataRow dtRow in dtTempNewTransactRecord.Rows )
					{
						totalTransmittalAmtDue+=Convert.ToDouble(dtRow["PersonalPreTax"])+Convert.ToDouble(dtRow["YmcaPreTax"]);
					}
					if(dtTempNewTransactRecord.Rows.Count >0)
					{
						newUeinTransmittalID=_newTransmittalID;
						if(_ymcaTransmittalExist && _dtOldTransmittalRow !=null )
						{
							_dtOldTransmittalRow["mnyAmtDue"]=Convert.ToDouble(_dtOldTransmittalRow["mnyAmtDue"])+totalTransmittalAmtDue;
						}
						else if(!_ymcaTransmittalExist)
						{
							DataRow dtNewTransmittalRow;
							dtNewTransmittalRow=parameterdtNewTransmittals.NewRow();
							dtNewTransmittalRow["guiTransmittalID"]=_newTransmittalID;
							dtNewTransmittalRow["guiYmcaID"]=_guiYmcaID ;
							dtNewTransmittalRow["chvTransmittalSourceCode"]="YRS";
							dtNewTransmittalRow["chvTransmittalNo"]=GenerateTransmittalNo()  ;
							dtNewTransmittalRow["dtsTransmittalDate"]=_fundedDate;
							dtNewTransmittalRow["dtsAccountingDate"]=_currentAccountingDate;
							dtNewTransmittalRow["mnyAmtDue"]=totalTransmittalAmtDue;
							dtNewTransmittalRow["mnyAmtPaid"]=0.0;
							parameterdtNewTransmittals.Rows.Add(dtNewTransmittalRow); 

						}
					}
				}//End If
			}
			catch(Exception ex)
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
		private  bool FieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] compareFieldNames)
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
			catch(Exception ex)
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
		private  DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] targetfieldNames)
		{
			try
			{
				if(targetfieldNames==null || targetfieldNames.Length==0)
				{
					object []source= sourceRow.ItemArray;
					for(int i=0;i<source.Length;i++)
					{
						newRow[i]=source[i];
					}
				}
				else
				{
					foreach (string field in targetfieldNames)
						newRow[field] = sourceRow[field];
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}

			return newRow;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lastValues"></param>
		/// <param name="sourceRow"></param>
		/// <param name="compareFieldNames"></param>
		private  void SetLastValues(object[] lastValues, DataRow sourceRow, string[] compareFieldNames)
		{
			try
			{
				for (int i = 0; i < compareFieldNames.Length; i++)
					lastValues[i] = sourceRow[compareFieldNames[i]];
			}
			catch(Exception ex)
			{
				throw ex;
			}
		} 
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterSourceDataTable"></param>
		/// <param name="parameterTargetColumnNames"></param>
		/// <param name="parameterCompareColumnNames"></param>
		/// <param name="parameterClubbedColumnList"></param>
		/// <returns></returns>
		private  DataTable SelectDistinct(DataTable parameterSourceDataTable,string [] parameterTargetColumnNames,string[] parameterCompareColumnNames,string[] parameterClubbedColumnList )
		{
			DataTable dtDistinct=null;
			object[] lastValues;
			DataRow[]  selectedRows; 
			bool clubbedFlag=true;
			try
			{
				if ( parameterCompareColumnNames==null || parameterCompareColumnNames.Length ==0)
					throw new ArgumentNullException("ColumnNames"); 
				if ( parameterClubbedColumnList==null || parameterClubbedColumnList.Length==0)
					clubbedFlag=false;
				lastValues=new object[parameterCompareColumnNames.Length];
				dtDistinct=new DataTable(); 
				if (parameterTargetColumnNames==null || parameterTargetColumnNames.Length==0)
				{
					dtDistinct=parameterSourceDataTable.Clone(); 
				}
				else
				{
					foreach(string columnName in parameterTargetColumnNames)
					{
						dtDistinct.Columns.Add(columnName,parameterSourceDataTable.Columns[columnName].DataType);      
					}
				}
				
				

				
				selectedRows=parameterSourceDataTable.Select("",string.Join(", ", parameterCompareColumnNames) ); 

				int clubbedCounter=-1;
				foreach(DataRow dtRow in selectedRows)
				{
					if(!FieldValuesAreEqual(lastValues, dtRow, parameterCompareColumnNames))
					{
						dtDistinct.Rows.Add( CreateRowClone(dtRow,dtDistinct.NewRow(),parameterTargetColumnNames));
						SetLastValues(lastValues,dtRow,parameterCompareColumnNames);
						clubbedCounter++;
					}
					else if(clubbedFlag)
					{
						if(parameterClubbedColumnList.Length >0)
						{
							for(int i=0;i<parameterClubbedColumnList.Length;i++)
							{
								dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]]=Math.Round(Convert.ToDouble(dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]])+Convert.ToDouble(dtRow[parameterClubbedColumnList[i]]),2);
								dtDistinct.Rows[clubbedCounter].AcceptChanges(); 
								
							}
						}
					}
					
				}
				//dtDistinct.AcceptChanges(); 

  
			return dtDistinct;
			
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterSourceTable"></param>
		/// <param name="parameterDistinctTable"></param>
		/// <param name="parameterValidateColumns"></param>
		private void ApplyThreshAmountCheck(DataTable parameterSourceTable,DataTable parameterDistinctTable,string[] parameterValidateColumns )
		{
			//DataRow[] dtFindRows=null;
			bool l_boolFlag=false;
			try
			{
				if(parameterSourceTable.Rows.Count >0 &&  parameterDistinctTable.Rows.Count>0)
				{
					double totPersonalAndYmcaAmt=0.0;
					foreach(DataRow dtRow in parameterDistinctTable.Rows )
					{
						totPersonalAndYmcaAmt=0.0;
						for(int i=0;i< parameterValidateColumns.Length;i++)
						{
							totPersonalAndYmcaAmt+=Convert.ToDouble(dtRow[parameterValidateColumns[i]]);
						}
						if(totPersonalAndYmcaAmt > _thresholdAmount)
						{
							l_boolFlag=true;
							break;
//							dtFindRows=parameterSourceTable.Select("guiPersID='"+dtRow["guiPersID"]+"'");
//							for(int i=0;i<dtFindRows.Length ;i++)
//							{
//								parameterSourceTable.Rows.Remove( dtFindRows[i]); 
//								parameterSourceTable.AcceptChanges(); 
//							}
						}
					}
					if(!l_boolFlag)
					{
						parameterSourceTable.Rows.Clear();  
						parameterSourceTable.AcceptChanges(); 
					}
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		
		#endregion

		#endregion

	}
}
