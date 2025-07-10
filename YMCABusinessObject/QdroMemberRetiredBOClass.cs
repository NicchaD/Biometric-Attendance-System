//*******************************************************************************
// Project Name		:		
// FileName			:	QdroMemberRetiredDAClass.cs
// Author Name		:	Nidhin Raj	
// Employee ID		:	37232
// Email			:	nidhin.raj@3i-infotech.com
// Contact No		:	080-39876746
// Creation Time	:	13/6/2008 
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>

//
// Changed by			:	
// Changed on			:	
// Change Description	:	
//*******************************************************************************
//Modification History
//*******************************************************************************
//Modified by          Date          Description
//*******************************************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//Pramod P. Pokale     2017.01.24    YRS-AT-3299 - YRS enh:improve usability of QDRO split screens(Retired) (TrackIT 28050) 
//*******************************************************************************
using System;
using YMCARET.YmcaDataAccessObject;
using System.Data;
namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for QdroMemberRetiredBOClass.
	/// </summary>
	public class QdroMemberRetiredBOClass
	{
		public QdroMemberRetiredBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		public static DataSet LookUpRetiredList(string parameterSSNo,string parameterFundNo,string parameterLastName,string parameterFirstName,string parameterState,string parameterCity)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.LookUpRetiredList(parameterSSNo,parameterFundNo,parameterLastName,parameterFirstName,parameterState,parameterCity);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet getQDRORecipient(string parameterSSNo)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.getQDRORecipient(parameterSSNo);
			}
			catch
			{
				throw;
			}
		}
		public static DataSet getPartAccountDetail(string parameterSSNo,string parameterFundEventID,string parameterPlanType)
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.getParticipantAccountDetail(parameterSSNo,parameterFundEventID,parameterPlanType);
			}
			catch
			{
				throw;
			}
		}

		public static DataSet getNewPersonID()
		{
			try
			{
				return YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.GetNewGuid();
			}
			catch
			{
				throw;
			}

		}
		
        public static void SaveRetiredSplit(DataTable dtBenifAccountTempTable, DataTable dtRecptAccount, string RecptFundEventId, string OriginalFundEventId, string QDRORequestID, DataTable dtPartAccount, decimal Totalsplitpercentage, DataTable dtBenifAccount) //PPP | 01/24/2017 | YRS-AT-3299 | Changed the datatype of Totalsplitpercentage from Double to Decimal
        {
            try
            {
                YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.SaveRetiredSplit(dtBenifAccountTempTable, dtRecptAccount, RecptFundEventId, OriginalFundEventId, QDRORequestID, dtPartAccount, Totalsplitpercentage, dtBenifAccount);
            }
            catch
            {
                throw;
            }
        }
//		public void insertPersDetails(string parameterUniqueId,
//			string parameterSSNo,
//			string parameterLastName,
//			string parameterFirstName,
//			string parameterMiddleName,
//			string parameterSalCode,
//			string parameterSfixTitle,
//			string parameterBirthDate,
//			string parameterMaritalCode,
//			string parameterEMail,
//			string parameterPhoneNo,
//			string parameterAdd1,
//			string parameterAdd2,
//			string parameterAdd3,
//			string parameterCity,
//			string parameterState,
//			string parameterzip,
//			string parameterCountry)
//		{
//			try
//			{
//				YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.insertPersDetails(parameterUniqueId,parameterSSNo,
//																parameterLastName,
//																parameterFirstName,
//																parameterMiddleName,
//																parameterSalCode,
//																parameterSfixTitle,
//																parameterBirthDate,
//																parameterMaritalCode,
//																parameterEMail,
//																parameterPhoneNo,
//																parameterAdd1,
//																parameterAdd2,
//																parameterAdd3,
//																parameterCity,
//																parameterState,
//																parameterzip,
//																parameterCountry);
//					
//			}
//			catch
//			{
//				throw;
//			}
//		}

//		public void SaveRetiredSplit(string parameterQdroRequestID,string parameterAnnuityType,
//			string parameterRecipientPersonID,
//			string parameterAnnuitySourceCode,
//			string parameterPlanType,
//			string parameterPurchaseDate,
//			string parameterSSLevelingAmount,
//			string parameterSSReductionAmount,
//			string parameterSSReductionEffDate,
//			string parameterCurrentPayment,
//			string parameterPersonalPreTaxCurrentPayment,
//			string parameterPersonalPostTaxCurrentPayment,
//			string parameterYmcaPreTaxCurrentPayment,
//			string parameterPersonalPreTaxReserveRemaining,
//			string parameterPersonalPostTaxReserveRemaining,
//			string parameterStateYmcaPreTaxReserveRemaining,
//			string parameterRecipientSplitPercent,
//			string parameterAnnuityID,
//			string parameterRecipientPersID,
//			string parameterNewRecpient)
//		{
//			try
//			{
//				YMCARET.YmcaDataAccessObject.QdroMemberRetiredDAClass.SaveRetiredSplit(parameterQdroRequestID,		
//																						parameterAnnuityType,
//																						parameterRecipientPersonID,
//																						parameterAnnuitySourceCode,
//																						parameterPlanType,
//																						parameterPurchaseDate,
//																						parameterSSLevelingAmount,
//																						parameterSSReductionAmount,
//																						parameterSSReductionEffDate,
//																						parameterCurrentPayment,
//																						parameterPersonalPreTaxCurrentPayment,
//																						parameterPersonalPostTaxCurrentPayment,
//																						parameterYmcaPreTaxCurrentPayment,
//																						parameterPersonalPreTaxReserveRemaining,
//																						parameterPersonalPostTaxReserveRemaining,
//																						parameterStateYmcaPreTaxReserveRemaining,
//																						parameterRecipientSplitPercent,
//																						parameterAnnuityID,
//																						parameterRecipientPersID,
//																						parameterNewRecpient);
//																								
//			}
//			catch
//			{
//				throw;
//			}
//		}

		

	}
}
