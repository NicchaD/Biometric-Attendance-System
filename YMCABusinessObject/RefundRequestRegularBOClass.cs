//****************************************************
//Modification History
//****************************************************
//Modified by           Date            Description
//****************************************************
//Imran 	            29-Sep-2010		YRS 5.0-1181 :Add validation for Hardhship withdrawal - No YMCA contact
//Manthan Rajguru       2015.09.16      YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//****************************************************

using System.Data;
using YMCARET.YmcaDataAccessObject;

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for RefundRequestRegularBOClass.
	/// </summary>
	public class RefundRequestRegularBOClass
	{
		public RefundRequestRegularBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DataSet GetTermDate(string parameterFundId)
		{
			try
			{
				return RefundRequestRegularDAClass.GetTermDate(parameterFundId);
		    }
			catch
			{
				throw;
			}
		}

			public static string GetMinDistPeriods(double parameterAge)
			{
				try
				{
					return RefundRequestRegularDAClass.GetMinDistPeriods(parameterAge);
				}
				catch
				{
					throw;
				}
			}


		public static DataSet GetDeductions()
		{
			try
			{
				return RefundRequestRegularDAClass.GetDeductions();
			}
			catch
			{
				throw;
			}
		}
		public static DataSet GetAddress(string parameterPersId)
		{
			try
			{
				return RefundRequestRegularDAClass.GetAddress(parameterPersId);
			}
			catch
			{
				throw;
			}
		}
		public static DataTable LookUpAnnuityBasisTypes()
		{
			try
			{
				DataSet l_DataSet_AnnuityBasisTypes = null;
				
				l_DataSet_AnnuityBasisTypes = RefundRequestRegularDAClass.LookUpAnnuityBasisTypes();

				if (l_DataSet_AnnuityBasisTypes != null)
				{
					return l_DataSet_AnnuityBasisTypes.Tables["Table_AnnuityBasisTypes"];
				}

				return null;				
			}
			catch
			{
				throw;
			}
		}
		public static string LookUpYMCAIds()
		{
			try
			{				
				return RefundRequestRegularDAClass.LookUpYMCAIds();
			}
			catch
			{
				throw;
			}
		}
		public static DataTable LookUpTransacts(string paramFundEventId)
		{
			try
			{
				DataSet l_Ds_Transacts = null;
				l_Ds_Transacts = new DataSet();
				l_Ds_Transacts=RefundRequestRegularDAClass.LookUpTransacts(paramFundEventId);
				return l_Ds_Transacts.Tables["Table_LookUpTransacts"];
			}
			catch
			{
				throw;

			}
		}

		public static DataTable GetRefundable(string paramFundEventId)
		{
			try
			{
				DataSet l_Dataset_Refundable ;
				DataTable l_DataTable_Refundable ;

				l_Dataset_Refundable =  RefundRequestRegularDAClass.GetRefundable(paramFundEventId);
				l_DataTable_Refundable = l_Dataset_Refundable.Tables["Refundable"];
				
				return l_DataTable_Refundable;

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
				return  ManualTransactionsBOClass.GetAccountingDate();
				
			}
			catch
			{
				throw;
			}
		}
		public static DataTable GetPersBankingBeforeEffDate(string paramEffDate)
		{
			try
			{
				DataSet l_DataSet_PersBanking;
				
				l_DataSet_PersBanking =  RefundRequestRegularDAClass.GetPersBankingBeforeEffDate(paramEffDate);

				if (l_DataSet_PersBanking != null)
				{
					return l_DataSet_PersBanking.Tables["PersBanking"];
				}
				else
					return null;
				
			}
			catch
			{
				throw;
			}
		}

		//Priya 03-June-08
		public static int ValidateDailyInterest()//(string paramPersId ,string paramFundEventId)
		{
			try
			{
				return RefundRequestRegularDAClass.ValidateDailyInterest();//(paramPersId,paramFundEventId);
			}
			catch
			{
				throw;
			}
		}
		public static int ValidateLastPayrollDate(string paramPersId ,string paramFundEventId)
		{
			try
			{
				return RefundRequestRegularDAClass.ValidateLastPayrollDate(paramPersId,paramFundEventId);
			}
			catch
			{
				throw;
			}
		}
		//End Priya 03-June-08

		//Priya as on 22-Jan-2009 : YRS 5.0-666  added to get 6 month for hardship withdrawal
		public static DataSet getConfigurationValue(string ParameterConfigKey)
		{
			try
			{
				return RefundRequestRegularDAClass.getConfigurationValue(ParameterConfigKey);
			}
			catch
			{
				throw;
			}
		}
		//End 22-Jan-2009

        public static DataSet ValidateRefundProcess(string paramRequestId)
        {
            try
            {
                return RefundRequestRegularDAClass.ValidateRefundProcess(paramRequestId);//(paramPersId,paramFundEventId);
            }
            catch
            {
                throw;
            }
        }

	}
}
