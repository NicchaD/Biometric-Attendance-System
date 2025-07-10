//*******************************************************************************
// Copyright YMCA Retirement Fund All Rights Reserved. 
//
// Project Name		:	YMCA-YRS
// FileName			:	UpdateCheckCashDateBOClass.cs
// Author Name		:	Shashi Shekhar
// Employee ID		:	51426
// Email			:	shashi.singh@3i-infotech.com
// Contact No		:	
// Creation Time	:	
// Program Specification Name	:	
// Unit Test Plan Name			:	
// Description					:	<<Please put the brief description here...>>
//****************************************************
//Modification History
//****************************************************
//Modified by          Date          Description
//****************************************************
//Manthan Rajguru      2015.09.16    YRS-AT-2550: YRS data cleanup: copyright comments and namespace (Changed the Namespace reference from 'Infotech' to 'YMCARET')
//*****************************************************
using System;
using System.Data;
using System.Collections ;
using YMCARET.YmcaDataAccessObject; 

namespace YMCARET.YmcaBusinessObject
{
	/// <summary>
	/// Summary description for UpdateCheckCashDateBOClass.
	/// </summary>
	public class UpdateCheckCashDateBOClass
	{
		public UpdateCheckCashDateBOClass()
		{
			//
			// TODO: Add constructor logic here
			//
		}


/// <summary>
/// To get the data from database
/// </summary>
/// <param name="alCheckNo">disburesement number</param>
/// <returns>Dataset</returns>
		public static DataSet GetCheckDetailsDataSet(ArrayList alCheckNo)
		{
			try
			{
				return(UpdateCheckCashDateDAClass.GetCheckDetailsDataSet(alCheckNo));
			}
			catch
			{
				throw;
			}
			
		}


/// <summary>
///To Update the bitPAid field of Cashed check in databse 
/// </summary>
/// <param name="alUniqueId">Unique identifier</param>
		
		public static void UpdateCashedChecks(ArrayList alUniqueId)
		{
			try
			{
				UpdateCheckCashDateDAClass.UpdateCashedChecks (alUniqueId);
			}
			catch
			{
				throw;
			}
			
		}
//Shashi Shekhar Singh:19-01-2010: For YRS 5.0-970
/// <summary>
/// To get the Outfut file details for cashed check date module
/// </summary>
/// <returns></returns>
		public static DataSet GetExceptionMetaOutputFileType()
		{
			try
			{
				return (YMCARET.YmcaDataAccessObject.UpdateCheckCashDateDAClass.GetExceptionMetaOutputFileType()); 
				
			}
			catch
			{
				throw;
			}

		}

	//--------------------------------------------	

	}
}
