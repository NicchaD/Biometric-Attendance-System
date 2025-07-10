//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for DisbursementReversalBOClass.
	/// </summary>
	public sealed class DisbursementReversalBOClass
	{
		public DisbursementReversalBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
/// <summary>
/// 
/// </summary>
/// <returns></returns>
		public static DataSet GetStatusList()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DisbursementReversalDAClass.GetStatusList();
			}
			catch
			{
				throw;
			}
			
		}
/// <summary>
/// 
/// </summary>
/// <param name="parameterPersId"></param>
/// <param name="parameterAnnuityOnly"></param>
/// <returns></returns>
		public static DataSet GetDisbursementsByPersId(string parameterPersId,int parameterAnnuityOnly)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DisbursementReversalDAClass.GetDisbursementsByPersId(parameterPersId,parameterAnnuityOnly );
			}
			catch
			{
				throw;
			}
		}
/// <summary>
/// /
/// </summary>
/// <param name="parameterPersId"></param>
/// <returns></returns>
		public static DataSet GetStatusByPersId(string parameterPersId)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DisbursementReversalDAClass.GetStatusByPersId(parameterPersId);
			}
			catch
			{
				throw;
			}
			
		}
/// <summary>
/// 
/// </summary>
/// <param name="parameterRevDisb"></param>
/// <returns></returns>
		public static DataSet GetWithholdingInfoForRev(string parameterRevDisb)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.DisbursementReversalDAClass.GetWithholdingInfoForRev(parameterRevDisb);

			}
			catch
			{
				throw;
			}
			
		}


	}
}
