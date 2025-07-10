//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCa-YRS
// FileName			:	CashApplicationBOClass.cs
// Author Name		:	
// Employee ID		:	
// Email				:	
// Contact No		:	
// Creation Time		:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//****************************************************
//Modification History
//****************************************************
//Modified by           Date            Description
//****************************************************
//Shashank Patel		19-Sep-2013		BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal
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
	/// Summary description for ServiceTimeAndVestingBO.
	/// </summary>
	public class ServiceTimeAndVestingBOClass
	{
		#region Constructor
		public ServiceTimeAndVestingBOClass(string parameterYmcaId,string parameterTransmittalID)
		{
//			this._fundedTransmittalLogID =parameterYmcaId;
//			this._transmittalID =parameterTransmittalID;
			
		}
		#endregion

		#region Privae Member
//		private string _ymcaID;
//		private string _transmittalID;
//		private DataSet _dsTransacts;
//		private _fundedTransmittalLogID Int64;
		#endregion

		#region Private Method
//		private void GetTransactAndFundEventsRecords(string parameterTransmittalId)
//		{
//			try
//			{
//				
//			}
//			catch(Exception ex)
//			{
//				throw ex;
//			}
//			finally
//			{
//				
//			}
//		
//		}
		#endregion
		
		#region Public Method
		public static bool ServiceTimeVestingUpdate(Int64 parameterFundedTransmittalLogID)
		{
			bool l_Success;
			try
			{
				//Get transact and fundevents records for perticular transmittal
				//GetTransactAndFundEventsRecords(_transmittalID);
				l_Success=ServiceTimeAndVestingDAClass.ServiceTimeVestingUpdate( parameterFundedTransmittalLogID);
				return l_Success;
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal 
		#region Cash Application-Person

		/// <summary>
		/// This method update the person Paid and nonpaid service and well as vesting date
		/// </summary>
		/// <param name="parameterIntFundedTransmittalLogID"></param>
		/// <returns>Boolean</returns>
		public static bool ServiceTimeVestingUpdateForPerson(Int64 parameterIntFundedTransmittalLogID)
		{
			bool bSuccess;
			try
			{

				bSuccess = ServiceTimeAndVestingDAClass.ServiceTimeVestingUpdateForPerson(parameterIntFundedTransmittalLogID);
				return bSuccess;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		#endregion
		//Shashank Patel		19-Sep-2013		 BT:618/YRS 5.0-842 : Need ability to pay one person on a transmittal 
	}
	
}
