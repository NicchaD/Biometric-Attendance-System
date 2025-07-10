//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for AccountTypesDAClass.
	/// </summary>
	public sealed class DeferredPayment
	{
		private DeferredPayment()
		{
		}

		public static DataSet GetDeferredPaymentList()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DeferredPayment.GetDeferredPaymentList();
			}
			catch
			{
				throw;
			}

		}
		public static DataSet GetDeferredPaymentDetail()
		{
			try
			{
				return null;//YMCARET.YmcaDataAccessObject.DeferredPayment.GetDeferredPaymentDetail();
			}
			catch
			{
				throw;
			}

		}

		public static DataSet GetDeferredPaymentTableSchemas ()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.DeferredPayment. GetDeferredPaymentTableSchemas());
			}
			catch 
			{
				throw;
			}

		}

		public static string GetRolloverInstitutionID (string paramStrInstitutionName)
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.DeferredPayment.GetRolloverInstitutionID(paramStrInstitutionName));
			}
			catch 
			{
				throw;
			}

		}

		public static bool SaveDeferredPayment (DataSet paramdsDPDataset)
		{

			try
			{
				return (YMCARET.YmcaDataAccessObject.DeferredPayment.SaveDeferredPayment(paramdsDPDataset));
			}
			catch 
			{
				throw;
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
		private  static DataRow CreateRowClone(DataRow sourceRow, DataRow newRow, string[] targetfieldNames)
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
			catch(Exception ex)
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
		public static DataTable SelectDistinct(DataTable parameterSourceDataTable,string [] parameterTargetColumnNames,string[] parameterCompareColumnNames,string[] parameterClubbedColumnList )
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
								if(dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]]!=System.DBNull.Value && dtRow[parameterClubbedColumnList[i]] != System.DBNull.Value)
								{
									dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]]=Convert.ToDecimal(dtDistinct.Rows[clubbedCounter][parameterClubbedColumnList[i]])+Convert.ToDecimal(dtRow[parameterClubbedColumnList[i]]);
									dtDistinct.Rows[clubbedCounter].AcceptChanges(); 
								}
								
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
			
		#endregion
	}
}
